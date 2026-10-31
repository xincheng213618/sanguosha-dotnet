#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineKuangfuReceiptsRuntime
{
    internal bool ValidKuangfu(ProgramSkillFrame f)
    {
        if (f.Kuangfu is not { } r || f.TriggerId is not null || f.WindowContext is not null || f.InstructionIndex != 1 || r.InstructionIndex != f.InstructionIndex || r.Source != global::CardGame.Core.EngineOwners.CoreGameEngineKuangfuRuntime.KuangfuSource(f) || r.GameplayHash != f.GameplayHash || r.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || r.ActualTurnOwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || r.ActualTurnOwnerSeat != f.OwnerSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play && !_coreKuangfuOperations.KuangfuGameEnded() || !Enum.IsDefined(r.Stage) || f.SelectedCardIds.Count != 0 || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program is not { } definition || definition.GameplayHash != f.GameplayHash)
            return false;
        var plan = ProgramInstructionResolver.Default.Resolve(f, definition);
        if (plan.Activation is not { UsesPerPhase: 1, UsesPerTurn: null, MinCards: 0, MaxCards: 0, MinTargets: 0, MaxTargets: 0 } || plan.Instructions is not [{ Op: SkillProgramEffectOp.DiscardEquipmentThenSlashAndOwnershipOutcome }])
            return false;
        var facts = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        if (facts.OfType<KuangfuStartedEvent>().Count(e => e == new KuangfuStartedEvent(f.Id, r.Source, r.GameplayHash, r.ActualTurn, r.ActualTurnOwnerSeat)) != 1 || r.SelectedHandIds.Distinct().Count() != r.SelectedHandIds.Count || r.DiscardedHandIds.Distinct().Count() != r.DiscardedHandIds.Count || r.SelectedHandIds.Any(id => id <= 0) || r.DiscardedHandIds.Any(id => id <= 0))
            return false;
        if (r.Stage == KuangfuStage.EquipmentChoice)
            return r.EquipmentOwnerSeat is null && r.EquipmentCardId is null && r.EquipmentKind is null && r.PaymentBefore == 0 && r.PaymentAfter == 0 && r.SlashReturn is null && !r.CausedDamage && f.SelectedTargetSeats.Count == 0 && f.PendingMovementContinuation is null && r.SelectedHandIds.Count == 0 && r.DiscardedHandIds.Count == 0 && r.DiscardRequired == 0 && r.ActualDraw == 0 && r.OutcomeBefore == 0 && r.OutcomeAfter == 0;
        if (r.EquipmentOwnerSeat is not { } equipmentOwner || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(equipmentOwner) || r.EquipmentCardId is not { } card || card <= 0 || r.EquipmentKind is not { } kind || _coreCardAttackStateOperations.GetAttackCard(card).IsGeneralWeapon || r.PaymentBefore < 0 || r.PaymentAfter <= r.PaymentBefore || _coreCardMovementJournalCardMovementsAccess._cardMovements.Count(m => m.Sequence > r.PaymentBefore && m.Sequence <= r.PaymentAfter && m.CardId == card && m.CardKind == kind && m.From == CardLocation.Equipment(equipmentOwner) && m.To == CardLocation.DiscardPile && m.Reason.Value == "skill-program.kuangfu.equipment-cost") != 1 || facts.OfType<KuangfuEquipmentPaidEvent>().Count(e => e == new KuangfuEquipmentPaidEvent(f.Id, equipmentOwner, card, kind, r.PaymentBefore, r.PaymentAfter)) != 1)
            return false;
        if (_coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.Sequence > r.PaymentBefore && m.Sequence <= r.PaymentAfter && m.CardId != card && !(kind == CardKind.WoodenOx && m.From == CardLocation.Grain(equipmentOwner) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard)))
            return false;
        if (r.Stage is KuangfuStage.EquipmentChildren or KuangfuStage.SlashTargetChoice)
            return r.SlashReturn is null && !r.CausedDamage && f.SelectedTargetSeats.Count == 0 && r.SelectedHandIds.Count == 0 && r.DiscardedHandIds.Count == 0 && r.DiscardRequired == 0 && r.ActualDraw == 0 && r.OutcomeBefore == 0 && r.OutcomeAfter == 0 && (r.Stage == KuangfuStage.EquipmentChildren ? IsKuangfuPending(f) : f.PendingMovementContinuation is null);
        if (r.SlashReturn is not { } returned || returned.ProgramFrameId != f.Id || returned.InstructionIndex != r.InstructionIndex || returned.Source != r.Source || returned.GameplayHash != r.GameplayHash || returned.ActualTurn != r.ActualTurn || returned.ActualTurnOwnerSeat != r.ActualTurnOwnerSeat || returned.EquipmentOwnerSeat != equipmentOwner || returned.EquipmentCardId != card || returned.CardUseFrameId <= f.Id || returned.CardActionId <= 0 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(returned.OriginalTargetSeat) || returned.OriginalTargetSeat == f.OwnerSeat || !f.SelectedTargetSeats.SequenceEqual([returned.OriginalTargetSeat]) || facts.OfType<KuangfuSlashIssuedEvent>().Count(e => e.Return == returned) != 1)
            return false;
        var damageFacts = facts.OfType<KuangfuActualDamageObservedEvent>().Where(e => e.ProgramFrameId == f.Id).ToArray();
        if (r.CausedDamage != (damageFacts.Length > 0) || damageFacts.Select(e => e.DamageFrameId).Distinct().Count() != damageFacts.Length || damageFacts.Any(e => e.CardUseFrameId != returned.CardUseFrameId || e.Amount <= 0 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(e.SourceSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(e.TargetSeat) || facts.OfType<DamageRequestedEvent>().Count(q => q.ResolutionId == e.DamageFrameId && q.SourceSeat == e.SourceSeat && q.TargetSeat == e.TargetSeat && q.Amount == e.Amount && q.SourceLess == e.SourceLess && (q.SourceCard is CardKind.Slash or CardKind.FireSlash or CardKind.ThunderSlash or CardKind.ColdSlash)) != 1))
            return false;
        if (r.Stage == KuangfuStage.SlashIssued)
            return f.PendingMovementContinuation is null && r.SelectedHandIds.Count == 0 && r.DiscardedHandIds.Count == 0 && r.OutcomeBefore == 0 && r.OutcomeAfter == 0 && r.DiscardRequired == 0 && r.ActualDraw == 0;
        if (facts.OfType<KuangfuSlashResolvedEvent>().Count(e => e == new KuangfuSlashResolvedEvent(f.Id, returned.CardUseFrameId, returned.CardActionId, r.CausedDamage)) != 1 || facts.OfType<CardUseFinishedEvent>().Count(e => e.ResolutionId == returned.CardUseFrameId) != 1)
            return false;
        if (r.Stage == KuangfuStage.HandDiscardChoice)
            return equipmentOwner != f.OwnerSeat && !r.CausedDamage && r.DiscardRequired is> 0 and <= 2 && r.SelectedHandIds.Count < r.DiscardRequired && r.SelectedHandIds.All(id => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Any(c => c.Id == id)) && r.DiscardedHandIds.Count == 0 && f.PendingMovementContinuation is null;
        if (r.Stage != KuangfuStage.OutcomeChildren || !IsKuangfuPending(f) || r.SelectedHandIds.Count != 0 || r.OutcomeBefore < r.PaymentAfter || r.OutcomeAfter < r.OutcomeBefore)
            return false;
        if (equipmentOwner == f.OwnerSeat)
            return r.CausedDamage && r.DiscardedHandIds.Count == 0 && r.ActualDraw is >= 0 and <= 2 && facts.OfType<KuangfuDrawIssuedEvent>().Count(e => e == new KuangfuDrawIssuedEvent(f.Id, 2, r.ActualDraw, r.OutcomeBefore, r.OutcomeAfter)) == 1 && _coreCardMovementJournalCardMovementsAccess._cardMovements.Count(m => m.Sequence > r.OutcomeBefore && m.Sequence <= r.OutcomeAfter && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == "skill-program.kuangfu.own-equipment-draw") == r.ActualDraw;
        return !r.CausedDamage && r.DiscardRequired is> 0 and <= 2 && r.DiscardedHandIds.Count == r.DiscardRequired && r.ActualDraw == 0 && facts.OfType<KuangfuHandDiscardPaidEvent>().Count(e => e.ProgramFrameId == f.Id && e.Before == r.OutcomeBefore && e.After == r.OutcomeAfter && e.CardIds.SequenceEqual(r.DiscardedHandIds)) == 1 && r.DiscardedHandIds.All(id => _coreCardMovementJournalCardMovementsAccess._cardMovements.Count(m => m.Sequence > r.OutcomeBefore && m.Sequence <= r.OutcomeAfter && m.CardId == id && m.From == CardLocation.Hand(f.OwnerSeat) && m.To == CardLocation.DiscardPile && m.Reason.Value == "skill-program.kuangfu.no-damage-hand-cost") == 1);
    }

    internal static bool IsKuangfuPending(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_KuangfuReceipts.IsKuangfuPending(f);
    internal bool IsKuangfuSlashUse(CardUseFrame use) => use.KuangfuSlashReturn is { } returned && use.Id == returned.CardUseFrameId && use.CardId == 0 && use.PhysicalCardIds is { Count: 0 } && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == returned.ProgramFrameId)is { } root && root.Kuangfu is { Stage: KuangfuStage.SlashIssued } receipt && receipt.SlashReturn == returned && ValidKuangfu(root) && (use.CurrentSlashFirePolicy?.OriginalAction ?? use.Action)is { Type: CardActionType.Use, EffectiveKind: CardKind.Slash, EffectiveSuit: Suit.None, EffectiveRank: 0, PhysicalCards.Count: 0 } action && action.ActionId == returned.CardActionId && _coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(use, root.OwnerSeat, action.ProviderSeat) && action.ConversionChain.Count == 0;
    internal bool HasIssuedKuangfuDistance(long? id, int actor) => id is { } useId && _coreCardUseLifecycleOperations.LifecycleCardUse(useId)is { } use && use.KuangfuSlashReturn?.Source.OwnerSeat == actor && IsKuangfuSlashUse(use);
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 390)]
    internal void ObserveKuangfuAppliedDamage(IGameEvent payload)
    {
        if (payload is not DamageAppliedEvent { Amount: > 0 } applied || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { } attack || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { KuangfuSlashReturn: { } returned } use || !IsKuangfuSlashUse(use) || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(attack.EffectiveCardKind ?? CardKind.Slash) || _resolutionStackOperations._resolutionStack.OfType<DamageFrame>().LastOrDefault(d => d.ParentFrameId == use.Id)is not { } damage || damage.SourceSeat != applied.SourceSeat || damage.TargetSeat != applied.TargetSeat || damage.Amount != applied.Amount || attack.SourceSeat != applied.SourceSeat || attack.TargetSeat != applied.TargetSeat || attack.IsSourceLess != applied.SourceLess || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DamageRequestedEvent>().Count(e => e.ResolutionId == damage.Id && e.SourceSeat == applied.SourceSeat && e.TargetSeat == applied.TargetSeat && e.Amount == applied.Amount && e.SourceLess == applied.SourceLess) != 1)
            return;
        var root = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Single(f => f.Id == returned.ProgramFrameId);
        if (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<KuangfuActualDamageObservedEvent>().Any(e => e.DamageFrameId == damage.Id))
            throw new InvalidOperationException("One actual equipment Slash damage cannot be recorded twice.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(root.Id, root with { Kuangfu = root.Kuangfu!with { CausedDamage = true } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new KuangfuActualDamageObservedEvent(root.Id, use.Id, damage.Id, applied.SourceSeat, applied.TargetSeat, applied.Amount, applied.SourceLess));
    }

    internal bool KuangfuFirstPaidChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (!ValidKuangfu(root) || !IsKuangfuPending(root) || root.Kuangfu is not { } r || r.Stage is not (KuangfuStage.EquipmentChildren or KuangfuStage.OutcomeChildren))
            return false;
        if (child is ProgramLifecycleTriggerWindowFrame changed && changed.Window == SkillProgramTriggerWindow.SkillsChanged)
            return changed.ResumeProgramFrameId == root.Id && changed.Continuation == ProgramLifecycleContinuation.ResumeParentProgram && changed.CandidateIndex >= 0 && changed.CandidateIndex <= changed.Candidates.Count;
        if (child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange)
            return state.ResumeProgramFrameId == root.Id && state.CharacterStateContinuation == CharacterStateContinuation.Program && state.Window is SkillProgramTriggerWindow.CharacterTurnedOver or SkillProgramTriggerWindow.CharacterTurnedFaceUp or SkillProgramTriggerWindow.CharacterEnteredChain && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CharacterStateChangedEvent>().Count(e => e.Change.Id == state.Id && e.Change.ParentFrameId == root.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == state.Window) == 1;
        var before = r.Stage == KuangfuStage.EquipmentChildren ? r.PaymentBefore : r.OutcomeBefore;
        var after = r.Stage == KuangfuStage.EquipmentChildren ? r.PaymentAfter : r.OutcomeAfter;
        if (child is CardsMovedTriggerWindowFrame moved)
            return moved.Batch.ParentFrameId == root.Id && moved.ResumeProgramFrameId is null && moved.Batch.AwaitingProgramFrameId == root.Id && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) && m.Sequence > before && m.Sequence <= after);
        if (r.Stage != KuangfuStage.EquipmentChildren || r.EquipmentKind != CardKind.SilverLion || r.EquipmentOwnerSeat is not { } payer)
            return false;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.ResumeFrameId == root.Id && hp.Change.ParentFrameId == root.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == payer && hp.Change.TargetSeat == payer && hp.Change.Amount == 1;
        return child is RecoveryReplacementFrame recovery && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, root) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == "skill-program.kuangfu.equipment-cost" && recovery.Attempt.SourceSeat == payer && recovery.Attempt.TargetSeat == payer && recovery.Attempt.Amount == 1;
    }

    internal ProgramSkillFrame? KuangfuPaidObserverRoot(long? damageWindow = null)
    {
        for (var i = 0; i + 1 < _resolutionStackOperations._resolutionStack.Count; i++)
        {
            if (_resolutionStackOperations._resolutionStack[i] is not ProgramSkillFrame root || !KuangfuFirstPaidChild(root, _resolutionStackOperations._resolutionStack[i + 1]))
                continue;
            if (damageWindow is { } id && !_resolutionStackOperations._resolutionStack.Skip(i + 1).Any(f => f.Id == id && f is DamageTriggerWindowFrame or BeforeDamageProgramWindowFrame))
                continue;
            var valid = true;
            for (var j = i + 2; j < _resolutionStackOperations._resolutionStack.Count; j++)
            {
                if (_resolutionStackOperations._resolutionStack[j] is ProgramLifecycleTriggerWindowFrame changed && _resolutionStackOperations._resolutionStack[j - 1] is ProgramSkillFrame owner && changed.Window == SkillProgramTriggerWindow.SkillsChanged && changed.ResumeProgramFrameId == owner.Id && changed.Continuation == ProgramLifecycleContinuation.ResumeParentProgram && changed.CandidateIndex >= 0 && changed.CandidateIndex <= changed.Candidates.Count)
                    continue;
                if (!_corePaidColorDamageClaimObserversOperations.PaidColorDamageClaimObserverEdge(j) && !_coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(_resolutionStackOperations._resolutionStack[j - 1], _resolutionStackOperations._resolutionStack[j]))
                {
                    valid = false;
                    break;
                }

                if (_resolutionStackOperations._resolutionStack[j] is DyingFrame dying && j + 1 < _resolutionStackOperations._resolutionStack.Count && (_coreParticipantHandDamageRidesOperations.IsPaidHandRepaymentRescueRide(j, dying) || _coreParticipantHandProgramRescueRidesOperations.IsPaidHandRepaymentProgramAlcoholRide(j, dying) || _coreUnrespondableCounterspellsOperations.PolicyCounterspellVirtualAlcoholRide(j, dying) || _coreHalfHandPhaseDebtDamageObserversOperations.PaidObserverDamageVirtualAlcoholRide(j, dying)))
                    break;
            }

            if (valid)
                return root;
        }

        return null;
    }
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 180)]
    internal bool IsKuangfuProgramDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && KuangfuPaidObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 490)]
    internal bool TryAdvanceKuangfuSubtree()
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || KuangfuPaidObserverRoot()is null)
            return false;
        return _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: false);
    }

    [ProgramDistanceRule(ProgramDistanceStage.CardUseExemption, 110)]
    internal bool DistanceCardUseExemption110HasIssuedKuangfuDistance(ProgramCardDistanceQuery q) => HasIssuedKuangfuDistance(q.CardUseFrameId, q.Actor.Seat);
    [ProgramDistanceRule(ProgramDistanceStage.PhysicalSlash, 130)]
    internal bool DistancePhysicalSlash130HasIssuedKuangfuDistance(ProgramSlashDistanceQuery q) => HasIssuedKuangfuDistance(q.ExistingUseFrameId, q.Actor.Seat);
}
