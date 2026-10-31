#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineLastDamageSourceReciprocityObserversRuntime
{
    internal global::CardGame.Core.GameEngine.IDamageAttempt? LastDamageSourceOriginalAttack(long id) => _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == id) switch
    {
        ProgramSkillFrame { AttackAttempt: not null } => _kernelCoreGameEngineProgramAttackAttemptOperations.CreateProgramAttackHandle(id),
        ProgramSkillFrame { CardAttack.Active: true } => _kernelCoreMainOperations.CreateCardAttackHandle(id),
        CardUseFrame { CardAttack.Active: true } => _kernelCoreMainOperations.CreateCardAttackHandle(id),
        JudgmentFrame { CardAttack.Active: true } => _kernelCoreMainOperations.CreateCardAttackHandle(id),
        _ => null
    };
    internal bool ValidLastDamageSourceReceipt(ProgramSkillFrame f)
    {
        if (f.LastDamageSourceReciprocity is not { } r || f.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, SourceSeat: { } source, TargetSeat: { } target, DamageFrameId: { } damageId, Amount: > 0 } context || f.TriggerId is null || f.InstructionIndex != 1 || r.InstructionIndex != f.InstructionIndex || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || r.Source != new CardConversionSource(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId) || r.GameplayHash != f.GameplayHash || r.DamageWindowId != context.ParentFrameId || r.DamageFrameId != damageId || r.SourceSeat != source || r.TargetSeat != target || source == target || r.Amount != context.Amount || r.Amount <= 0 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(source) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.RecordedSourceSeat) || !Enum.IsDefined(r.Direction) || !Enum.IsDefined(r.Stage) || !Enum.IsDefined(r.Nature) || r.ParticipantSeat != (r.Direction == LastDamageSourceReciprocityDirection.DrawOwner ? f.OwnerSeat : source) || (r.Direction == LastDamageSourceReciprocityDirection.DrawOwner ? source != f.OwnerSeat || r.RecordedSourceSeat != target : target != f.OwnerSeat || r.RecordedSourceSeat != source))
            return false;
        var plan = ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!);
        if (plan.Trigger is not { } trigger || !global::CardGame.Core.EngineOwners.CoreGameEngineLastDamageSourceReciprocityRuntime.IsLastDamageSourceTrigger(trigger) || trigger.Effects[0].StateId != r.StateId || global::CardGame.Core.EngineOwners.CoreGameEngineLastDamageSourceReciprocityRuntime.LastDamageSourceDirection(trigger) != r.Direction || plan.Instructions.Count != 1 || plan.Instructions[0].Op != SkillProgramEffectOp.LastDamageSourceReciprocity || plan.Instructions[0].StateId != r.StateId)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == f.Id);
        if (index < 2 || _resolutionStackOperations._resolutionStack[index - 1] is not DamageTriggerWindowFrame window || window.Id != r.DamageWindowId || window.ParentFrameId != r.DamageFrameId || window.TriggerWindow != context.Window || window.SourceSeat != source || window.TargetSeat != target || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, window.Candidates[window.CandidateIndex].ToProgramCandidate()) || _resolutionStackOperations._resolutionStack[index - 2] is not DamageFrame damage || damage.Id != r.DamageFrameId || damage.ParentFrameId != r.AttackFrameId || damage.SourceSeat != source || damage.TargetSeat != target || damage.Amount != r.Amount || damage.Nature != r.Nature || LastDamageSourceOriginalAttack(r.AttackFrameId)is not { } attack || attack.IsSourceLess || !attack.DamageWasApplied || attack.SourceSeat != source || attack.TargetSeat != target || attack.DamageAmount != r.Amount || _coreNativeCardClassificationsOperations.GetDamageNature(attack) != r.Nature)
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        if (history.OfType<DamageRequestedEvent>().Count(e => e.ResolutionId == damage.Id && e.SourceSeat == source && e.TargetSeat == target && e.Amount == r.Amount && e.Nature == r.Nature && !e.SourceLess && e.SourceCard == attack.EffectiveCardKind) != 1)
            return false;
        var requestIndex = Array.FindIndex(history, e => e is DamageRequestedEvent requested && requested.ResolutionId == damage.Id);
        if (history.Skip(requestIndex + 1).TakeWhile(e => e is not DamageRequestedEvent).OfType<DamageAppliedEvent>().FirstOrDefault()is not { SourceLess: false } applied || applied.SourceSeat != source || applied.TargetSeat != target || applied.Amount != r.Amount || applied.Nature != r.Nature)
            return false;
        var eligibility = history.OfType<LastDamageSourceReciprocityEligibleEvent>().Where(e => e.OwnerSeat == f.OwnerSeat && e.SkillId == f.SkillId && e.StateId == r.StateId && e.Direction == r.Direction && e.DamageFrameId == r.DamageFrameId).ToArray();
        if (eligibility is not [var earned] || earned.SkillInstanceId != f.SkillInstanceId || earned.GameplayHash != f.GameplayHash || earned.BindingId != f.TriggerId || earned.AttackFrameId != r.AttackFrameId || earned.SourceSeat != source || earned.TargetSeat != target || earned.Amount != r.Amount || earned.Nature != r.Nature || earned.RecordedSourceSeat != r.RecordedSourceSeat)
            return false;
        // Validate the remembered source at the original occurrence. Later nested
        // damage may update memory, but cannot replace this earned conclusion.
        var remembered = history.TakeWhile(e => !Equals(e, earned)).OfType<LastDamageSourceRecordedEvent>().LastOrDefault(e => e.OwnerSeat == f.OwnerSeat && e.SkillId == f.SkillId && e.StateId == r.StateId);
        if (remembered is not { } memory || memory.SourceSeat != r.RecordedSourceSeat || r.Direction == LastDamageSourceReciprocityDirection.DiscardSource && (memory.DamageFrameId != r.DamageFrameId || memory.AttackFrameId != r.AttackFrameId || memory.Amount != r.Amount || memory.Nature != r.Nature))
            return false;
        var started = history.OfType<LastDamageSourceReciprocityStartedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (started is not [var start] || start.Source != r.Source || start.GameplayHash != r.GameplayHash || start.StateId != r.StateId || start.Direction != r.Direction || start.DamageWindowId != r.DamageWindowId || start.DamageFrameId != r.DamageFrameId || start.AttackFrameId != r.AttackFrameId || start.SourceSeat != source || start.TargetSeat != target || start.Amount != r.Amount || start.Nature != r.Nature || start.ParticipantSeat != r.ParticipantSeat || start.RecordedSourceSeat != r.RecordedSourceSeat || history.OfType<LastDamageSourceReciprocityStartedEvent>().Count(e => e.Source.OwnerSeat == f.OwnerSeat && e.Source.SkillId == f.SkillId && e.StateId == r.StateId && e.Direction == r.Direction && e.DamageFrameId == r.DamageFrameId) != 1)
            return false;
        if (r.EligibleMaterials.Select(m => m.CardId).Distinct().Count() != r.EligibleMaterials.Count || r.EligibleMaterials.Any(m => m.CardId <= 0 || m.From.OwnerSeat != r.ParticipantSeat || m.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment)) || r.ActualCount is < 0 or > 1 || r.SequenceBefore < 0 || r.SequenceAfter < r.SequenceBefore || r.SequenceAfter > _coreLastDamageSourceReciprocityOperations.LastDamageSourceMovementSequence)
            return false;
        var issued = history.OfType<LastDamageSourceReciprocityMovementIssuedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (!r.MovementIssued)
            return r.Stage == LastDamageSourceReciprocityStage.ChoosingDiscard && r.Direction == LastDamageSourceReciprocityDirection.DiscardSource && r.EligibleMaterials.Count > 0 && r.PaidMaterial is null && r.ActualCount == 0 && r.BatchId is null && r.SequenceBefore == 0 && r.SequenceAfter == 0 && issued.Length == 0 && f.PendingMovementContinuation is null && r.EligibleMaterials.SequenceEqual(_coreLastDamageSourceReciprocityOperations.LastDamageSourceLegalMaterials(r.ParticipantSeat));
        if (r.Stage is not (LastDamageSourceReciprocityStage.MovementChildren or LastDamageSourceReciprocityStage.Complete) || issued is not [var invoice] || invoice.ParticipantSeat != r.ParticipantSeat || invoice.Direction != r.Direction || invoice.ActualCount != r.ActualCount || invoice.SequenceBefore != r.SequenceBefore || invoice.SequenceAfter != r.SequenceAfter || invoice.BatchId != r.BatchId)
            return false;
        var records = _coreCardMovementJournalCardMovementsAccess._cardMovements.Where(m => m.Sequence > r.SequenceBefore && m.Sequence <= r.SequenceAfter && m.Reason.Value == global::CardGame.Core.EngineOwners.CoreGameEngineLastDamageSourceReciprocityRuntime.LastDamageSourceReason(f, r.Direction == LastDamageSourceReciprocityDirection.DrawOwner)).ToArray();
        if (records.Length != r.ActualCount)
            return false;
        if (r.Direction == LastDamageSourceReciprocityDirection.DrawOwner)
        {
            if (r.EligibleMaterials.Count != 0 || r.PaidMaterial is not null || r.BatchId is not null || records.Any(m => m.From != CardLocation.DrawPile || m.To != CardLocation.Hand(f.OwnerSeat)))
                return false;
        }
        else if (r.ActualCount != 1 || r.BatchId is null || r.PaidMaterial is not { } paid || !r.EligibleMaterials.Contains(paid) || records is not [var movement] || movement.CardId != paid.CardId || movement.CardKind != paid.PrintedKind || movement.From != paid.From || movement.To != (paid.IsGeneralWeapon && paid.From.Zone == CardZoneKind.Equipment ? CardLocation.OutsideGame : CardLocation.DiscardPile))
            return false;
        return r.Stage == LastDamageSourceReciprocityStage.Complete ? f.PendingMovementContinuation is null : f.PendingMovementContinuation is { BeforeCount: 0, CoverageResultBind: null } pending && pending.SubjectSeat == r.ParticipantSeat;
    }

    internal bool LastDamageSourceFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        var r = f.LastDamageSourceReciprocity!;
        if (!r.MovementIssued || r.Stage != LastDamageSourceReciprocityStage.MovementChildren)
            return false;
        var draw = r.Direction == LastDamageSourceReciprocityDirection.DrawOwner;
        bool PaidEquipment(CardKind kind) => !draw && r.PaidMaterial is { From.Zone: CardZoneKind.Equipment } material && material.PrintedKind == kind && _coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.Sequence > r.SequenceBefore && m.Sequence <= r.SequenceAfter && m.CardId == material.CardId && m.From == material.From && m.To == CardLocation.DiscardPile && m.Reason.Value == global::CardGame.Core.EngineOwners.CoreGameEngineLastDamageSourceReciprocityRuntime.LastDamageSourceReason(f, false));
        if (child is CardsMovedTriggerWindowFrame moved)
            return moved.ResumeProgramFrameId is null && moved.Batch.Id == moved.Id && moved.Batch.ParentFrameId == f.Id && moved.Batch.AwaitingProgramFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) && (m.Sequence > r.SequenceBefore && m.Sequence <= r.SequenceAfter && (draw ? m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == global::CardGame.Core.EngineOwners.CoreGameEngineLastDamageSourceReciprocityRuntime.LastDamageSourceReason(f, true) || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle : moved.Batch.Id == r.BatchId && r.PaidMaterial is { } material && m.CardId == material.CardId && m.From == material.From && m.To == (material.IsGeneralWeapon && material.From.Zone == CardZoneKind.Equipment ? CardLocation.OutsideGame : CardLocation.DiscardPile) && m.Reason.Value == global::CardGame.Core.EngineOwners.CoreGameEngineLastDamageSourceReciprocityRuntime.LastDamageSourceReason(f, false)) || !draw && PaidEquipment(CardKind.WoodenOx) && m.Reason == CardMoveReasons.WoodenOxGrainDiscard && m.From == CardLocation.WoodenOxGrain(r.ParticipantSeat) && m.To == CardLocation.DiscardPile));
        if (!PaidEquipment(CardKind.SilverLion))
            return false;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.ParentFrameId == f.Id && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == r.ParticipantSeat && hp.Change.TargetSeat == r.ParticipantSeat && hp.Change.Amount == 1;
        return child is RecoveryReplacementFrame recovery && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.SourceSeat == r.ParticipantSeat && recovery.Attempt.TargetSeat == r.ParticipantSeat && recovery.Attempt.Amount == 1 && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == global::CardGame.Core.EngineOwners.CoreGameEngineLastDamageSourceReciprocityRuntime.LastDamageSourceReason(f, false);
    }

    internal ProgramSkillFrame? LastDamageSourceReciprocityObserverRoot(long damageWindowId)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == damageWindowId);
        if (index < 0 || index + 2 >= _resolutionStackOperations._resolutionStack.Count || _resolutionStackOperations._resolutionStack[index] is not DamageTriggerWindowFrame || _resolutionStackOperations._resolutionStack[index + 1] is not ProgramSkillFrame root || root.LastDamageSourceReciprocity?.DamageWindowId != damageWindowId || !ValidLastDamageSourceReceipt(root) || !LastDamageSourceFirstChild(root, _resolutionStackOperations._resolutionStack[index + 2]))
            return null;
        return _coreObserverSubtreesOperations.HasObserverSuffix(index + 1, (child) => global::CardGame.Core.EngineOwners.CoreGameEngineObserverSubtreesRuntime.IsObserverSkillsChangedEdge(_resolutionStackOperations2._resolutionStack()[child - 1], _resolutionStackOperations2._resolutionStack()[child]) || _coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(_resolutionStackOperations2._resolutionStack()[child - 1], _resolutionStackOperations2._resolutionStack()[child]) || _coreHalfHandPhaseDebtDamageObserversOperations.HalfHandPaidDamageObserverEdge(child) || _coreProgramChildResolutionOperations.ProgramChildResolutionEdge(child), (child, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(child, dying)) ? root : null;
    }
    internal ProgramSkillFrame? AnyLastDamageSourceReciprocityObserverRoot() => _resolutionStackOperations._resolutionStack.OfType<DamageTriggerWindowFrame>().Select(window => LastDamageSourceReciprocityObserverRoot(window.Id)).FirstOrDefault(root => root is not null);
    [ProgramRuntimeHook(ProgramRuntimePhase.BeforeResponseHealth, 80)]
    internal bool TryAdvanceLastDamageSourceReciprocitySubtree()
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || AnyLastDamageSourceReciprocityObserverRoot()is null)
            return false;
        return _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
    }

    internal bool IsLastDamageSourceReciprocityChoice(ProgramSkillFrame f, PendingDecision decision)
    {
        if (f.LastDamageSourceReciprocity is not { Stage: LastDamageSourceReciprocityStage.ChoosingDiscard, MovementIssued: false } r || !ValidLastDamageSourceReceipt(f) || decision.Kind != DecisionKind.ProgramTrigger || !decision.IsPrivate || decision.PlayerSeat != r.ParticipantSeat || decision.TargetSeat != r.ParticipantSeat || decision.SourceSeat != f.OwnerSeat || decision.ValidTargetSeats.Count != 0 || decision.ValidContentIds.Count != 0 || decision.RequiredCardCount != 0 || decision.SkillPrompt?.SkillId != f.SkillId)
            return false;
        var expected = _coreLastDamageSourceReciprocityOperations.LastDamageSourceDiscardChoices(f);
        return decision.ValidCardIds.SequenceEqual(expected.SelectMany(c => c.Cards).Distinct()) && decision.Choices.Count == expected.Count && decision.Choices.Zip(expected).All(pair => pair.First.Id == pair.Second.Id && pair.First.Cards.SequenceEqual(pair.Second.Cards) && pair.First.Targets.SequenceEqual(pair.Second.Targets) && pair.First.Parameters.OrderBy(x => x.Key).SequenceEqual(pair.Second.Parameters.OrderBy(x => x.Key)));
    }
}
