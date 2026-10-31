#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMaWanOlRuntime
{

    internal bool MaWanHasMovement(ProgramSkillFrame f, CardsMovedTriggerWindowFrame moved) => f.MaWanHunjiang is { Stage: MaWanHunjiangStage.DrawChildren } r && moved.ResumeProgramFrameId == f.Id && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == MaWanDrawReason && m.Sequence > r.SequenceBefore && m.Sequence <= r.SequenceAfter && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m));
    internal bool MaWanFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.MaWanHunjiang is not { Stage: MaWanHunjiangStage.DrawChildren })
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return MaWanHasMovement(f, moved);
        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal bool MaWanObserverSuffix(int root) => _coreObserverSubtreesOperations.HasObserverSuffix(root, (i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[i], _resolutionStackOperations2._resolutionStack()[i - 1]), (i, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, dying));
    internal ProgramSkillFrame? MaWanObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterMaWanOlOperations.MaWanFirstChild(f, child), (i) => _characterMaWanOlOperations.MaWanObserverSuffix(i));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 26510)]
    internal bool AdvanceMaWanSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MaWanObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
}
