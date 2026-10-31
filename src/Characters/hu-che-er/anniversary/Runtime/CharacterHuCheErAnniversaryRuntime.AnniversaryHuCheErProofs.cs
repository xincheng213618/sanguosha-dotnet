#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuCheErAnniversaryRuntime
{
    internal bool HuCheErParent(ProgramSkillFrame f) => f.SkillId == AnniversaryHuCheErComposition.Daoji && f.TriggerId == "first-foreign-weapon" && f.ActivationId == f.TriggerId && f.InstructionIndex == 1 && f.SelectedCardIds.Count == 0 && f.SelectedTargetSeats.Count == 0 && f.WindowContext is { } c && HuCheErFirstWeapon(f.OwnerSeat, c)is not null && _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == c.ParentFrameId)is { } parent && parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(parent.Candidates[parent.CandidateIndex]));
    internal bool ValidHuCheEr(ProgramSkillFrame f)
    {
        if (f.AnniversaryHuCheEr is not { } r || !HuCheErParent(f) || r.Source != HuCheErSource(f) || r.Hash != f.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != r.Hash || r.ParentId != f.WindowContext!.ParentFrameId || HuCheErFirstWeapon(f.OwnerSeat, f.WindowContext)is not { Action: { } a } use || r.UseId != use.Id || r.ActionId != a.ActionId || r.Actor != a.ActorSeat || r.Turn != _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber || r.TurnOwner != _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || !Enum.IsDefined(r.Stage) || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryHuCheErStartedEvent>().Count(e => e == new AnniversaryHuCheErStartedEvent(f.Id, r.Source, r.Hash, r.ParentId, r.UseId, r.ActionId, r.Actor, r.Turn, r.TurnOwner)) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramBindingStartedEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.SkillId == f.SkillId && e.SkillInstanceId == f.SkillInstanceId && e.BindingId == f.TriggerId && e.Window == SkillProgramTriggerWindow.CardUseTargetsFinalized) != 1)
            return false;
        if (r.Stage == AnniversaryHuCheErStage.Choice)
            return r.Materials.Count == 0 && r.Moves.Count == 0 && r.Gain is null && f.PendingMovementContinuation is null;
        if (f.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null } p || p.SubjectSeat != f.OwnerSeat || r.Materials.Count == 0 || r.Materials.Distinct().Count() != r.Materials.Count || r.Materials.Any(id => !a.PhysicalCards.Any(c => c.CardId == id)) || r.Gain is not { } gain || gain.Reason.Value != HuCheErClaimReason || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || !gain.Movements.SequenceEqual(r.Moves))
            return false;
        _coreHappyWeiYanGainDestinationsOperations.ValidateHappyWeiYanGainRequestSettlement(gain, f.Id, r.Materials, CardLocation.Hand(f.OwnerSeat), r.Before);
        return _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryHuCheErWeaponClaimedEvent>().Count(e => e == new AnniversaryHuCheErWeaponClaimedEvent(f.Id, r.UseId, r.ActionId, f.OwnerSeat, r.Materials.Count, r.Before, r.After)) == 1 && r.Moves.All(m => r.Materials.Contains(m.CardId) && m.From == CardLocation.Processing && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
    }

    internal bool IsAnniversaryHuCheErClaimedUseMaterial(long useId, int cardId)
    {
        if (!HasAnniversaryHuCheErRules || _coreCardUseLifecycleOperations.LifecycleCardUse(useId)is not { Action: { Type: CardActionType.Use } a } use || !HuCheErWeapon(a.EffectiveKind) || !a.PhysicalCards.Any(c => c.CardId == cardId))
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryHuCheErWeaponClaimedEvent>().Any(e => e.UseId == useId && e.ActionId == a.ActionId && e.Count > 0 && e.Before >= 0 && e.After >= e.Before && e.After <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence && _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryHuCheErStartedEvent>().Count(s => s.FrameId == e.FrameId && s.UseId == useId && s.ActionId == a.ActionId && s.Actor == a.ActorSeat && s.Source.OwnerSeat == e.Recipient && s.Source.SkillId == AnniversaryHuCheErComposition.Daoji && s.Source.BindingId == "first-foreign-weapon" && _contentRegistryOperations._contentRegistry.GetSkill(s.Source.SkillId).Program?.GameplayHash == s.Hash) == 1 && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.CardId == cardId && m.From == CardLocation.Processing && (m.To == CardLocation.Hand(e.Recipient) || m.To == CardLocation.OutsideGame && _coreAdvancedLifecycleOperations.GetAdvancedCard(cardId).IsGeneralWeapon || _coreHappyWeiYanGainDestinationsOperations.IsHappyWeiYanRedirectedGain(m, CardLocation.Hand(e.Recipient))) && m.Reason.Value == HuCheErClaimReason && m.Sequence > e.Before && m.Sequence <= e.After) == 1);
    }

    [ProgramMovementProof(ProgramMovementProofPhase.NativeCost, 49500)]
    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 49500)]
    [ProgramMovementProof(ProgramMovementProofPhase.PrivatePayments, 49500)]
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 49500)]
    internal bool HuCheErMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => f.AnniversaryHuCheEr is { Stage: AnniversaryHuCheErStage.MovementChildren } && e?.Op == SkillProgramEffectOp.ResolveAnniversaryHuCheErDaoji && p is not null && p == f.PendingMovementContinuation && ValidHuCheEr(f);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 49500)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 49500)]
    internal bool RetainHuCheEr(ProgramSkillFrame f) => f.AnniversaryHuCheEr is { Stage: AnniversaryHuCheErStage.MovementChildren } && ValidHuCheEr(f);
    internal bool HuCheErFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.AnniversaryHuCheEr is not { Stage: AnniversaryHuCheErStage.MovementChildren } r || !ValidHuCheEr(f))
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return f.PendingMovementContinuation is { } p && HuCheErMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p) && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.All(r.Moves.Contains);
        if (child is RecoveryReplacementFrame recovery)
            return recovery.ParentFrameId == f.Id && recovery.Return.ResumeFrameId == f.Id && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && state.CharacterStateContinuation == CharacterStateContinuation.Program);
    }

    internal bool HuCheErObserverEdge(int i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]) || _resolutionStackOperations._resolutionStack[i - 1] is ProgramSkillFrame f && HuCheErFirstChild(f, _resolutionStackOperations._resolutionStack[i]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]);
    internal ProgramSkillFrame? HuCheErObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterHuCheErAnniversaryOperations.HuCheErFirstChild(f, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _characterHuCheErAnniversaryOperations.HuCheErObserverEdge(n), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 49500)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 49500)]
    internal bool HuCheErDamageObserver(long id) => HuCheErObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 49500)]
    internal bool HuCheErBeforeDamageObserver(long id) => HuCheErObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 49500)]
    internal bool HuCheErDyingObserver() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && HuCheErObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 49500)]
    internal bool AdvanceHuCheErChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && HuCheErObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
