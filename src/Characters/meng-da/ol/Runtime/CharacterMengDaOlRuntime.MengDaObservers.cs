#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMengDaOlRuntime
{

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27240)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27240)]
    internal bool ContinuePaidMengDa(ProgramSkillFrame f)
    {
        if (f.Goude is not { Stage: GoudeStage.MovementChildren or GoudeStage.NativeSlash or GoudeStage.NativeChildren })
            return false;
        return true;
    }

    internal bool MengDaFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.Goude is not { } r)
            return false;
        if (child is CardUseFrame use)
            return r.Stage == GoudeStage.NativeSlash && use.GoudeSlash == r.NativeOrigin && IsGoudeSlashUse(use.Id);
        if (child is CardsMovedTriggerWindowFrame moved)
            return r.Stage == GoudeStage.MovementChildren && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After && (r.Option == GoudeOption.Draw ? m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == GoudeDrawReason || m.Reason == CardMoveReasons.Reshuffle : r.PaidMaterial is { } material && m.CardId == material.CardId && m.From == CardLocation.Hand(material.HolderSeat) && m.To == CardLocation.DiscardPile && m.Reason.Value == GoudeDiscardReason));
        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal ProgramSkillFrame? MengDaObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => this.MengDaFirstChild(f, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27240)]
    internal bool AdvanceMengDaSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MengDaObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
