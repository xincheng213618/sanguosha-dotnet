#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHeQiOlRuntime
{
    internal bool HeQiFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (!CanContinueHeQi(root) || root.HeQi is not { } r || child is not CardsMovedTriggerWindowFrame moved)
            return false;
        return moved.Batch.ParentFrameId == root.Id && moved.Batch.AwaitingProgramFrameId == root.Id && moved.ResumeProgramFrameId is null && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After && (m.Reason.Value == HeQiReason(r.Stage) && r.Paid.Any(p => p.CardId == m.CardId && p.From == m.From) || r.Stage == HeQiStage.ObtainChildren && r.Paid.Any(p => p.PrintedKind == CardKind.WoodenOx && p.From == CardLocation.Equipment(r.TargetSeat)) && m.From == CardLocation.WoodenOxGrain(r.TargetSeat) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard));
    }

    internal ProgramSkillFrame? HeQiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((r, c) => _characterHeQiOlOperations.HeQiFirstChild(r, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 90)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 200)]
    internal bool HasHeQiDamageObserver(long id) => HeQiObserverRoot()is { } r && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == r.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 170)]
    internal bool TryAdvanceHeQiSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && HeQiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 300)]
    internal bool HasHeQiBeforeDamageObserver(long id) => HeQiObserverRoot()is { } r && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is BeforeDamageProgramWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == r.Id);
}
