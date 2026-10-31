#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunJunMobileShiRuntime
{
    internal bool ValidShiSj(ProgramSkillFrame f) => f.ShiSunJun is { } r && f.SkillId == ShiSjXiongtu && f.InstructionIndex == 1 && r.Source == ShiSjSource(f) && r.Hash == f.GameplayHash && r.Turn == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber && Enum.IsDefined(r.Stage) && r.Before >= 0 && r.After >= r.Before && r.After <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence && _coreTableSeatQueriesOperations.IsValidPlayerSeat(r.Target) && _coreProgramEventHistoryOperations.ProgramEventHistory<ShiSunJunStartedEvent>().Count(e => e == new ShiSunJunStartedEvent(f.Id, r.Source, r.Hash, r.Turn, r.Target)) == 1;
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 49200)]
    internal void AssertShiSj(ProgramSkillFrame f)
    {
        if (f.ShiSunJun is not { } r)
            return;
        if (!ValidShiSj(f) || r.Required is < 0 or > 4 || r.Payment.Count > r.Required || r.Payment.Distinct().Count() != r.Payment.Count || r.Hand.Distinct().Count() != r.Hand.Count || r.Revealed > 0 && !r.Hand.Contains(r.Revealed) || f.TriggerId is null && (f.ActivationId != "show-hand" || r.Phase is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase || phase.ActorSeat != f.OwnerSeat || f.SelectedTargetSeats is not [var target] || target != r.Target) || f.TriggerId is not null && (f.TriggerId != "other-damage" || f.WindowContext?.DamageFrameId != r.DamageId || r.Target != f.OwnerSeat))
            throw new InvalidOperationException("Shi Sun Jun lost the original frame source, actual phase, frozen branch or native damage receipt.");
        if (r.Stage == ShiSunJunStage.Hand && !_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.Target]).Select(c => c.Id).SequenceEqual(r.Hand) || r.Stage == ShiSunJunStage.Payment && r.Payment.Any(id => !ShiSjOwnDiscardChoices(f).Any(c => c.Cards.SequenceEqual([id]))))
            throw new InvalidOperationException("Xiongtu lost its unpaid original Hand slot or owned cost entity.");
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == f.Id && r.Stage is ShiSunJunStage.Hand or ShiSunJunStage.Branch or ShiSunJunStage.Payment && (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { } p || p.PlayerSeat != f.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(p.Choices, ShiSjChoices(f))))
            throw new InvalidOperationException("Xiongtu lost its originally published private choice set.");
    }

    [ProgramMovementProof(ProgramMovementProofPhase.NativeCost, 49200)]
    [ProgramMovementProof(ProgramMovementProofPhase.PrivatePayments, 49200)]
    internal bool ShiSjMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => f.ShiSunJun is { } r && ValidShiSj(f) && r.Stage is ShiSunJunStage.DiscardChildren or ShiSunJunStage.CostChildren or ShiSunJunStage.DrawChildren && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null && p == f.PendingMovementContinuation && e?.Op is SkillProgramEffectOp.ResolveMobileShiXiongtu or SkillProgramEffectOp.ResolveMobileShiXiongtuDamageBenefit;
    internal bool ShiSjFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidShiSj(f) || f.ShiSunJun is not { } r)
            return false;
        if (global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f))
            return r.Stage == ShiSunJunStage.Drawing;
        if (child is PublicHandRevealWindowFrame reveal)
            return r.Stage == ShiSunJunStage.RevealChildren && reveal.ParentFrameId == f.Id && reveal.ReturnKind == PublicHandRevealReturnKind.Registered && reveal.ReturnKey == "mobile-shi-sun-jun-hand";
        if (child is CardsMovedTriggerWindowFrame moved && f.PendingMovementContinuation is { } pending && ShiSjMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), pending))
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
        if (child is RecoveryReplacementFrame rec)
            return rec.ParentFrameId == f.Id && rec.Return.ResumeFrameId == f.Id && rec.Return.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && state.CharacterStateContinuation == CharacterStateContinuation.Program);
    }

    internal bool ShiSjEdge(int i) => _characterHuaTuoAnniversaryGodOperations.SkillInvocationStructuralEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]) || _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]) || _resolutionStackOperations._resolutionStack[i - 1] is ProgramSkillFrame f && ShiSjFirstChild(f, _resolutionStackOperations._resolutionStack[i]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]);
    internal bool ShiSjSuffix(int n) => _resolutionStackOperations._resolutionStack[n + 1] is DyingFrame d && n + 2 < _resolutionStackOperations._resolutionStack.Count && _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n + 1, d) || _coreObserverSubtreesOperations.HasObserverSuffix(n, (i) => _characterSunJunMobileShiOperations.ShiSjEdge(i), (i, dying) => _coreObserverSubtreesOperations2.IsSuspendedOrPaidRescueObserverSuffix(i, dying));
    internal ProgramSkillFrame? ShiSjObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterSunJunMobileShiOperations.ShiSjFirstChild(f, c), (n) => _characterSunJunMobileShiOperations.ShiSjSuffix(n));
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 49200)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 49200)]
    internal bool ShiSjDyingChild() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && ShiSjObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 49200)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 49200)]
    internal bool ShiSjDamageChild(long id) => ShiSjObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 49200)]
    internal bool ShiSjBeforeDamageChild(long id) => ShiSjObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 49200)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 49200)]
    internal bool ShiSjRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (ShiSjFirstChild(f, recovery))
            return true;
        var n = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var c = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return n >= 0 && c > n + 1 && ShiSjFirstChild(f, _resolutionStackOperations._resolutionStack[n + 1]) && ShiSjSuffix(n) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[c - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 49200)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 49200)]
    internal bool KeepShiSjPaid(ProgramSkillFrame f) => f.ShiSunJun is { Stage: not ShiSunJunStage.Hand } && ValidShiSj(f);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 49200)]
    internal bool AdvanceShiSjChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && ShiSjObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
