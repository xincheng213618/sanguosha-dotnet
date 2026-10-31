namespace CardGame.Core;

// Owns synchronous program health and character-state mutations. Native health
// children keep their original frames and receipts; this module stores no state.
[ProgramRuntimeModule]
internal sealed class ProgramHealthStateRuntime(IProgramRuntimeFrames frames, IProgramRuntimeEvents events,
    IMatchParticipantStateAccess participants, IProgramHealthStateFacts facts, IProgramHealthStateNativeEntries native)
{
    internal void RecoverProgramTargetTo(long frameId, int ownerSeat, int targetSeat,
        SkillProgramNumberExpression expression, int minimumValue, bool clampToMaxHp)
    {
        _ = frames.ActiveProgram(frameId);
        var target = participants.At(targetSeat);
        if (!target.HasHpValue || expression == SkillProgramNumberExpression.CurrentHp && !participants.At(ownerSeat).HasHpValue) return;
        var value = expression switch
        {
            SkillProgramNumberExpression.IntegerConstant => minimumValue,
            SkillProgramNumberExpression.CurrentHp => participants.At(ownerSeat).Hp,
            SkillProgramNumberExpression.LivingFactionCount => facts.LivingFactionCount,
            SkillProgramNumberExpression.LivingPlayersMinHp => facts.LivingPlayersMinHp,
            _ => throw new InvalidOperationException($"Unsupported numeric expression '{expression}'.")
        };
        value = Math.Max(minimumValue, value);
        // Recovery never raises HP above MaxHp. The flag remains part of the
        // serialized primitive contract for forward-compatible numeric policies.
        value = Math.Min(target.MaxHp, value);
        if (native.TryHandleHappyQiaoRecoverTo(frameId, ownerSeat, targetSeat, value)) return;
        var amount = Math.Max(0, value - target.Hp);
        if (amount == 0) return;
        if (native.TryQueueRecoveryReplacement(frameId, ownerSeat, targetSeat, amount,
            new(RecoveryAttemptProducer.Program, frames.ActiveProgram(frameId).InstructionIndex))) return;
        var recovery = native.BeginRecovery(frameId, ownerSeat, targetSeat, amount);
        target.Hp += amount;
        events.AdvanceFact(new RecoveryAppliedEvent(ownerSeat, targetSeat, amount, target.Hp));
        frames.Pop(recovery, ResolutionFrameKind.Recovery);
    }

    internal void SetProgramChainedState(ProgramSkillFrame frame, bool chained, int targetSeat)
    {
        var active = frames.ActiveProgram(frame.Id);
        if (active.OwnerSeat != frame.OwnerSeat || active.SkillId != frame.SkillId ||
            active.ActivationId != frame.ActivationId)
            throw new InvalidOperationException("Chained-state mutation requires the active program binding.");

        var target = participants.At(targetSeat);
        var wasChained = target.IsChained;
        var requested = chained || facts.HasCardPolicy(target, SkillProgramCardPolicyKind.ForceChained);
        target.IsChained = requested && !target.IsChained &&
            facts.HasCardPolicy(target, SkillProgramCardPolicyKind.PreventEnteringChain) ? false : requested;
        if (!wasChained && target.IsChained)
            native.RecordCharacterStateChange(targetSeat, SkillProgramTriggerWindow.CharacterEnteredChain);
        events.AdvanceFact(new ProgramChainedStateSetEvent(
            frame.Id, frame.SkillId, BindingId(frame), frame.OwnerSeat, participants.At(targetSeat).IsChained, targetSeat));
    }

    internal void ChangeProgramMaximumHp(ProgramSkillFrame frame, int amount, string? sourceSkillId = null) =>
        ChangeProgramParticipantMaximumHp(frame, frame.OwnerSeat, amount, sourceSkillId);

    internal void ChangeProgramParticipantMaximumHp(ProgramSkillFrame frame, int recipientSeat, int amount,
        string? sourceSkillId = null)
    {
        var active = frames.ActiveProgram(frame.Id);
        if (active.OwnerSeat != frame.OwnerSeat || active.SkillId != frame.SkillId ||
            active.TriggerId != frame.TriggerId)
            throw new InvalidOperationException("Maximum-HP mutation requires the active program binding.");

        var owner = participants.At(recipientSeat);
        var previous = owner.MaxHp;
        if (sourceSkillId is not null && sourceSkillId != frame.SkillId &&
            !facts.HasRuntimeSkillInstance(owner, sourceSkillId, facts.GetRuntimeSkillInstanceId(owner, sourceSkillId)))
            throw new InvalidOperationException("Maximum-HP attribution requires the owner's actual source skill instance.");
        var newMaximum = facts.HasWushuangTactic(owner.Seat, 2020) ? 7 : Math.Max(1, checked(owner.MaxHp + amount));
        if (native.TryApplyHappyQiaoMaximumHp(frame.Id, owner.Seat, newMaximum, sourceSkillId ?? frame.SkillId)) return;
        var hpBeforeClamp = owner.Hp;
        owner.MaxHp = newMaximum;
        owner.Hp = Math.Min(owner.Hp, owner.MaxHp);
        events.AdvanceFact(new MaximumHpChangedEvent(owner.Seat, owner.MaxHp - previous, owner.MaxHp,
            sourceSkillId ?? frame.SkillId)
        {
            HpBeforeClamp = facts.HasQimeiHealthSubject(owner.Seat) || owner.XiaHuiRestrictions is { Count: > 0 } ||
                facts.HasMouSunJianOlHpSubject(owner.Seat) || facts.HealthChangeSubject(owner.Seat) ? hpBeforeClamp : null
        });
    }

    internal void TurnOverProgramTarget(long frameId, int ownerSeat, int targetSeat)
    {
        _ = frames.ActiveProgram(frameId);
        var target = participants.At(targetSeat);
        if (facts.ShenwuCannotTurnOver(target)) return;
        var wasFaceDown = target.IsFaceDown;
        target.IsFaceDown = !target.IsFaceDown;
        if (!target.IsFaceDown) native.RecordCharacterStateChange(targetSeat, SkillProgramTriggerWindow.CharacterTurnedFaceUp);
        native.RecordCharacterTurnedOver(targetSeat, wasFaceDown);
        native.AddLog("SkillEffect", $"{target.Name} 的武将牌翻面。", ownerSeat, targetSeat);
    }

    internal void SetProgramTargetFaceState(long frameId, int ownerSeat, int targetSeat, bool faceDown)
    {
        _ = frames.ActiveProgram(frameId);
        var target = participants.At(targetSeat);
        if (target.IsFaceDown != faceDown && facts.ShenwuCannotTurnOver(target)) return;
        var wasFaceDown = target.IsFaceDown;
        target.IsFaceDown = faceDown;
        if (wasFaceDown && !faceDown) native.RecordCharacterStateChange(targetSeat, SkillProgramTriggerWindow.CharacterTurnedFaceUp);
        native.RecordCharacterTurnedOver(targetSeat, wasFaceDown);
        native.AddLog("SkillEffect",
            faceDown ? $"{target.Name} 的武将牌保持或变为背面。" : $"{target.Name} 的武将牌保持或变为正面。",
            ownerSeat, targetSeat);
    }

    private static string BindingId(ProgramSkillFrame frame) =>
        global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(frame);
}
