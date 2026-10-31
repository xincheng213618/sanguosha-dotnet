#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWuAnGuoOlRuntime
{
    internal bool WuAnGuoFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.WuAnGuoLiyong is not { Stage: WuAnGuoLiyongStage.GainChildren } r || !ValidWuAnGuoLiyong(f))
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return moved.Batch.ParentFrameId == f.Id && moved.ResumeProgramFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements is [var m] && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After && m.CardId == r.ObtainedCardId && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == LiyongGainReason;
        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal ProgramSkillFrame? WuAnGuoObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterWuAnGuoOlOperations.WuAnGuoFirstChild(f, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27010)]
    internal bool AdvanceWuAnGuoSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && WuAnGuoObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
