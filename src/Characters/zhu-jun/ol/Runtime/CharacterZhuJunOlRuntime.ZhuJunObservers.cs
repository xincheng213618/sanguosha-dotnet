#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhuJunOlRuntime
{

    internal bool IsZhuJunDrawMovement(ProgramSkillFrame f, CardMovementRecord m) => f.ZhuJunCuipoDraw is { } r && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After && (m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == ZhuJunDrawReason || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle);
    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 240)]
    internal bool IsZhuJunMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => f.ZhuJunCuipoDraw is not null && effect?.Op == SkillProgramEffectOp.ResolveCuipoActualTurnUse && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    internal bool ZhuJunFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.ZhuJunCuipoDraw is not { } r || f.PendingMovementContinuation is null)
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => IsZhuJunDrawMovement(f, m));
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        return child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && state.ResumeProgramFrameId == f.Id && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == state.Window);
    }

    internal ProgramSkillFrame? ZhuJunObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterZhuJunOlOperations.ZhuJunFirstChild(f, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (child) => _coreObserverSubtreesOperations.IsMovementObserverEdge(child) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(child) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[child], _resolutionStackOperations2._resolutionStack()[child - 1]), (child, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(child, dying)));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 600)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 710)]
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 740)]
    internal bool HasZhuJunDamageObserver(long id) => ZhuJunObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame or BeforeDamageProgramWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27030)]
    internal bool AdvanceZhuJunSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && ZhuJunObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
