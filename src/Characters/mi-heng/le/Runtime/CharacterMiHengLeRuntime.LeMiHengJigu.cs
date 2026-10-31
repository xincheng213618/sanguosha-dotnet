#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMiHengLeRuntime
{
    internal const string JiguDrawReason = "skill-program.mi-heng.jigu.draw";
    internal bool HasLeMiHengJiguCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveJiguDamageEquipmentDraw);

    internal static CardConversionSource JiguSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_mi_heng_le_LeMiHengJigu.JiguSource(f);
    internal static bool IsJiguDamageTrigger(SkillProgramTrigger t) => RuntimeSkills.RuntimeRules_mi_heng_le_LeMiHengJigu.IsJiguDamageTrigger(t);
    internal bool HasJiguEntityMark(int cardId) => _cardEntityMarks.Has(cardId, "jigu");
    internal bool HasJiguEntityMark(int ownerSeat, int cardId) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(ownerSeat) && _cardZonesOperations._cardZones.GetLocation(cardId) == CardLocation.Hand(ownerSeat) && HasJiguEntityMark(cardId);
    internal bool IsJiguHandLimitExempt(CharacterState owner, int cardId) => _coreNationalSkillsOperations.HasRuntimeSkill(owner, "le:jigu") && HasJiguEntityMark(owner.Seat, cardId);
    internal int JiguMarkedHandCount(int owner) => _cardEntityMarks.CountMarked(owner, "jigu", CardZoneKind.Hand);
    internal int JiguStartedSeatCount => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ActualDiscardRecoveryPhaseStartedEvent>().Where(e => e.Phase.Kind == ActualDiscardRecoveryPhaseKind.Preparation && e.Phase.ActorSeat == e.Phase.ActualTurnOwnerSeat).Select(e => e.Phase.ActorSeat).Distinct().Count();

    internal int JiguEmptySlotCount(CharacterState owner) => Enum.GetValues<EquipmentSlot>().Sum(slot => Math.Max(0, owner.EquipmentSlotCapacity(slot) - _corePhysicalOwnedCardQueriesOperations.GetEquipment(owner).Count(c => _coreEquipmentContentOperations.EquipmentRules.Get(c.Kind).Slot == slot)));
    internal int JiguUseCount(int owner, int round) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<JiguActivationPaidEvent>().Count(e => e.Source.OwnerSeat == owner && e.Source.SkillId == "le:jigu" && e.RoundNumber == round);
    internal bool JiguRoundDisabled(int owner, int round) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<JiguActivationPaidEvent>().Any(e => e.Source.OwnerSeat == owner && e.Source.SkillId == "le:jigu" && e.RoundNumber == round && e.DisabledForRound);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 28210, SkillProgramEffectOp.ResolveJiguDamageEquipmentDraw)]
    internal bool CanOfferJiguGate(ProgramTriggerLegalityContext gate) => CanOfferJigu(gate.Candidate, gate.Trigger, gate.Context);
    internal bool CanOfferJigu(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (!IsJiguDamageTrigger(trigger))
            return true;
        if (context.Window != SkillProgramTriggerWindow.AfterDamageApplied || context.OwnerSeat != candidate.OwnerSeat || context.OccurrenceIndex != 0 || candidate.OccurrenceIndex != 0 || context.Amount <= 0 || context.DamageFrameId is not { } damageId || context.TargetSeat != candidate.OwnerSeat && context.SourceSeat != candidate.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive || JiguRoundDisabled(candidate.OwnerSeat, _coreActualRoundProgressRoundNumberAccess._roundNumber) || JiguMarkedHandCount(candidate.OwnerSeat) != _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat]).Count || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<JiguActivationPaidEvent>().Any(e => e.Source.OwnerSeat == candidate.OwnerSeat && e.DamageFrameId == damageId) || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == context.ParentFrameId)is not DamageTriggerWindowFrame w || w.TriggerWindow != context.Window || w.ParentFrameId != damageId || w.CandidateIndex < 0 || w.CandidateIndex >= w.Candidates.Count || w.Candidates[w.CandidateIndex].ToProgramCandidate() != candidate || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == damageId)is not DamageFrame d || d.TargetSeat != context.TargetSeat || d.Amount != context.Amount)
            return false;
        return _coreNativeDamageHandlesOperations.Value.GetDamageTriggerAttack(w)is { } attack && d.ParentFrameId == attack.ResolutionId && (attack.IsSourceLess ? context.SourceSeat is null : context.SourceSeat == attack.SourceSeat);
    }

    SkillProgramStepOutcome ILeMiHengJiguHost.Draw(ProgramSkillFrame frame) => BeginJiguDraw(frame);
    internal SkillProgramStepOutcome BeginJiguDraw(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.Jigu is not null || f.SkillId != "le:jigu" || f.TriggerId != "damage-equipment-draw" || f.InstructionIndex != 1 || f.WindowContext is not { } c || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || !CanOfferJigu(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, c.OccurrenceIndex), _coreProgramLifecycleOperations.GetProgramTrigger(f), c))
            throw new InvalidOperationException("Jigu requires one unspent native source-or-target damage occurrence.");
        var threshold = JiguStartedSeatCount;
        var use = JiguUseCount(f.OwnerSeat, _coreActualRoundProgressRoundNumberAccess._roundNumber) + 1;
        var r = new JiguReceipt(JiguSource(f), f.GameplayHash, c.DamageFrameId!.Value, c.ParentFrameId, _coreActualRoundProgressRoundNumberAccess._roundNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, JiguMarkedHandCount(f.OwnerSeat), _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count, JiguEmptySlotCount(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]), threshold, use, use >= threshold, JiguStage.Drawing)
        {
            Paid = true
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Jigu = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(JiguPayment(f.Id, r));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)} 发动【激鼓】，摸 {(r.EmptySlotCount)} 张牌。", f.OwnerSeat);
        ContinueJigu(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal static JiguActivationPaidEvent JiguPayment(long id, JiguReceipt r) => RuntimeSkills.RuntimeRules_mi_heng_le_LeMiHengJigu.JiguPayment(id, r);
    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveJiguDamageEquipmentDraw)]
    internal void ReturnJiguDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        if (f.Jigu is not { Paid: true, Stage: JiguStage.Drawing } r || outcome.RecipientSeat != f.OwnerSeat || outcome.RequestedCount != r.EmptySlotCount || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<JiguDrawReturnedEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Jigu cannot repeat or replace its paid native draw request.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Jigu = r with { Stage = JiguStage.DrawChildren, DrawOutcome = outcome }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JiguDrawReturnedEvent(f.Id, outcome.ActualDrawIds.Count, outcome.ReplacementGainedIds.Count, outcome.SequenceBefore, outcome.SequenceAfter, outcome.AttemptFrameId));
        ContinueJigu(f.Id);
    }

    internal bool DrainJigu(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    internal void ContinueJigu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Jigu: { } r } f || f.Id != id)
            return;
        if (r.Stage == JiguStage.Drawing)
        {
            if (f.NativeDraw is null)
                _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, r.EmptySlotCount, new(JiguDrawReason));
            return;
        }

        if (DrainJigu(f))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JiguCompletedEvent(f.Id, r.DamageFrameId));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Jigu = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 28211)]
    internal bool ResumeJigu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Jigu: not null } f || f.Id != id)
            return false;
        ContinueJigu(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 28211)]
    internal bool ReturnJiguMovement(ProgramSkillFrame f)
    {
        if (f.Jigu is null)
            return false;
        ContinueJigu(f.Id);
        return true;
    }
}
