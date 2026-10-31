#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoXiOlRuntime
{
    internal bool CaoXiFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.CaoXiJianxuan is not { Stage: CaoXiJianxuanStage.DrawChildren } r || !ValidCaoXiJianxuan(f))
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return moved.Batch.ParentFrameId == f.Id && moved.ResumeProgramFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After && (m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(r.RecipientSeat!.Value) && m.Reason.Value == CaoXiDrawReason || m.Reason == CardMoveReasons.Reshuffle));
        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal ProgramSkillFrame? CaoXiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterCaoXiOlOperations.CaoXiFirstChild(f, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 26810)]
    internal bool AdvanceCaoXiSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && CaoXiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
