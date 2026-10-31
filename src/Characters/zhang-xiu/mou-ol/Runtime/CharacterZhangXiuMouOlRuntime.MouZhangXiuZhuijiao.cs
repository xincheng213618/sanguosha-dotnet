#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangXiuMouOlRuntime
{
    internal bool MouZhangXiuOlUseDamaged(long origin, int? instruction) => _coreProgramEventHistoryOperations.ProgramEventHistory<MouZhangXiuOlUseDamageEvent>().Any(e => e.OriginFrameId == origin && e.InstructionIndex == instruction && e.Amount > 0);
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 33001)]
    internal void CaptureMouZhangXiuOlPreviousUse(IGameEvent fact)
    {
        if (!HasMouZhangXiuOl || fact is not CurrentTurnCardUseKindsRecordedEvent { CardActionId: { } actionId, RoundActionType: CardActionType.Use } current || _coreCardUseLifecycleOperations.LifecycleCardUse(current.OriginFrameId)is not { Action: { Type: CardActionType.Use } action } use || action.ActionId != actionId || action.ActorSeat != current.ActorSeat || !CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind))
            return;
        var previous = _coreProgramEventHistoryOperations.ProgramEventHistory<CurrentTurnCardUseKindsRecordedEvent>().LastOrDefault(e => e.ActorSeat == current.ActorSeat && e.CardActionId != actionId);
        // No previous true Use is not a zero-damage previous card. Pure Played
        // actions do not enter the common declaration ledger in the first place.
        if (previous is null || previous.RoundActionType == CardActionType.Use && MouZhangXiuOlUseDamaged(previous.OriginFrameId, previous.ProgramInstructionIndex))
            return;
        foreach (var c in _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(_coreMatchParticipantsPlayersAccess._players[current.ActorSeat], SkillProgramTriggerWindow.CardUseCommitted).Where(c => c.SkillId == MouZhangXiuOlComposition.Zhuijiao && c.BindingId == "previous-undamaged"))
        {
            if (use.MouZhangXiuOlZhuijiao?.Any(e => e.Source.OwnerSeat == c.OwnerSeat && e.Source.SkillInstanceId == c.SkillInstanceId) == true)
                continue;
            var entry = new MouZhangXiuOlZhuijiaoEntry(use.Id, actionId, new(c.SkillId, c.BindingId, c.OwnerSeat, c.SkillInstanceId), c.GameplayHash, previous);
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { MouZhangXiuOlZhuijiao = (u.MouZhangXiuOlZhuijiao ?? []).Append(entry).ToArray() });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhangXiuOlQualifiedEvent(entry));
            use = _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)!;
        }
    }

    internal bool ValidMouZhangXiuOlEntry(CardUseFrame use, MouZhangXiuOlZhuijiaoEntry e)
    {
        if (use.Action is not { Type: CardActionType.Use } a || a.ActionId != e.ActionId || use.Id != e.UseId || !CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || e.Source.SkillId != MouZhangXiuOlComposition.Zhuijiao || e.Source.BindingId != "previous-undamaged" || e.Source.OwnerSeat != e.Previous.ActorSeat || _contentRegistryOperations._contentRegistry.GetSkill(e.Source.SkillId).Program?.GameplayHash != e.GameplayHash || _coreProgramEventHistoryOperations.ProgramEventHistory<MouZhangXiuOlQualifiedEvent>().Count(q => q.Entry == e with { Boosted = false, DrawReturned = false, PenaltyIssued = false, PenaltyReturned = false }) != 1)
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var index = Array.FindIndex(history, f => f is CurrentTurnCardUseKindsRecordedEvent c && c.CardActionId == e.ActionId && c.OriginFrameId == e.UseId && c.ActorSeat == e.Source.OwnerSeat);
        if (index < 0 || history.Take(index).OfType<CurrentTurnCardUseKindsRecordedEvent>().LastOrDefault(c => c.ActorSeat == e.Source.OwnerSeat) != e.Previous)
            return false;
        return !e.Boosted || _coreProgramEventHistoryOperations.ProgramEventHistory<MouZhangXiuOlBoostedEvent>().Count(q => q.UseId == use.Id && q.Source == e.Source) == 1;
    }

    internal MouZhangXiuOlZhuijiaoEntry? MouZhangXiuOlEntry(CardUseFrame use, ProgramTriggerCandidate c) => use.MouZhangXiuOlZhuijiao?.SingleOrDefault(e => e.Source.OwnerSeat == c.OwnerSeat && e.Source.SkillId == c.SkillId && e.Source.SkillInstanceId == c.SkillInstanceId && e.GameplayHash == c.GameplayHash && ValidMouZhangXiuOlEntry(use, e));
    internal void SetMouZhangXiuOlEntry(long useId, CardConversionSource source, Func<MouZhangXiuOlZhuijiaoEntry, MouZhangXiuOlZhuijiaoEntry> update) => _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(useId, u => u with { MouZhangXiuOlZhuijiao = u.MouZhangXiuOlZhuijiao!.Select(e => e.Source.OwnerSeat == source.OwnerSeat && e.Source.SkillId == source.SkillId && e.Source.SkillInstanceId == source.SkillInstanceId ? update(e) : e).ToArray() });
    internal SkillProgramStepOutcome BeginMouZhangXiuOlZhuijiao(ProgramSkillFrame supplied, SkillProgramEffectOp operation)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var c = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, 0);
        if (f.MouZhangXiuOl is not null || f.InstructionIndex != 1 || f.ActivationId != f.TriggerId || f.WindowContext is not { } w || !CanRunIssuedMouZhangXiuOl(c, w))
            throw new InvalidOperationException("Zhuijiao requires its frozen previous-Use qualification and exact current Slash window.");
        var use = MouZhangXiuOlContextUse(c, w)!;
        var entry = MouZhangXiuOlEntry(use, c)!;
        var r = new MouZhangXiuOlReceipt(MouZhangXiuOlSource(f), f.GameplayHash, operation, operation == SkillProgramEffectOp.ResolveMouZhangXiuZhuijiao ? MouZhangXiuOlStage.Drawing : MouZhangXiuOlStage.Penalty)
        {
            UseId = use.Id,
            ActionId = use.Action!.ActionId
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouZhangXiuOl = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhangXiuOlStartedEvent(f.Id, r.Source, r.GameplayHash, operation, use.Id));
        if (operation == SkillProgramEffectOp.ResolveMouZhangXiuZhuijiao)
        {
            SetMouZhangXiuOlEntry(use.Id, entry.Source, e => e with { Boosted = true });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhangXiuOlBoostedEvent(f.Id, use.Id, entry.Source));
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, 1, new(MouZhangXiuOlDrawReason));
        }
        else
        {
            SetMouZhangXiuOlEntry(use.Id, entry.Source, e => e with { PenaltyIssued = true });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhangXiuOlPenaltyStartedEvent(f.Id, use.Id, entry.Source));
            PublishMouZhangXiuOl(f);
        }

        return operation == SkillProgramEffectOp.ResolveMouZhangXiuZhuijiao ? SkillProgramStepOutcome.AwaitChild : SkillProgramStepOutcome.AwaitChoice;
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveMouZhangXiuZhuijiao)]
    internal void ReturnMouZhangXiuOlDraw(ProgramSkillFrame f, NativeDrawOutcome draw)
    {
        if (!ValidMouZhangXiuOl(f) || f.MouZhangXiuOl is not { Stage: MouZhangXiuOlStage.Drawing, Draw: null } r || draw.RecipientSeat != f.OwnerSeat || draw.RequestedCount != 1)
            throw new InvalidOperationException("Zhuijiao cannot redirect or repeat its native Draw1.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouZhangXiuOl = r with { Draw = draw, Before = draw.SequenceBefore, After = draw.SequenceAfter, Stage = MouZhangXiuOlStage.DrawChildren }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhangXiuOlDrawReturnedEvent(f.Id, r.UseId, f.OwnerSeat, 1, draw.ActualDrawIds.Count, draw.ReplacementGainedIds.Count));
        ContinueMouZhangXiuOl(f.Id);
    }

    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 33003)]
    internal int MouZhangXiuOlDamageBonus(GameEngine.IDamageAttempt attack) => !attack.IsChainPropagation && attack is NativeOwners.CardAttackHandle && _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is { MouZhangXiuOlZhuijiao: { } entries } use ? entries.Count(e => e.Boosted && ValidMouZhangXiuOlEntry(use, e)) : 0;
    [ProgramDamageHook(ProgramDamagePhase.AppliedBeforeDying, 33003)]
    internal bool CaptureMouZhangXiuOlUseDamage(GameEngine.IDamageAttempt attack, long damageId)
    {
        if (!HasMouZhangXiuOl || !attack.DamageWasApplied || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == damageId)is not DamageFrame { Amount: > 0 } damage || damage.ParentFrameId != attack.ResolutionId)
            return false;
        var origin = attack.ResolutionId;
        var instruction = _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == origin)is ProgramSkillFrame { CardAttack: { ProgramUseInstructionIndex: { } i } } ? i : (int? )null;
        if (!_coreProgramEventHistoryOperations.ProgramEventHistory<CurrentTurnCardUseKindsRecordedEvent>().Any(e => e.OriginFrameId == origin && e.ProgramInstructionIndex == instruction && e.RoundActionType == CardActionType.Use))
            return false;
        if (!_coreProgramEventHistoryOperations.ProgramEventHistory<MouZhangXiuOlUseDamageEvent>().Any(e => e.DamageId == damageId))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhangXiuOlUseDamageEvent(origin, instruction, damageId, damage.Amount));
        return false;
    }

    [ProgramTargetWindow(ProgramTargetWindowStage.FinalizedAfterCategory, 33003)]
    internal bool ObserveMouZhangXiuOlAllUses(ProgramTargetCardWindowQuery query) => HasMouZhangXiuOl;
    [ProgramTargetWindow(ProgramTargetWindowStage.NormalizeImplicitDrawTwo, 33003)]
    internal bool NormalizeMouZhangXiuOlDrawTwo(ProgramTargetCardWindowQuery query) => HasMouZhangXiuOl;
}
