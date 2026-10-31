#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterQiaoZhouOlRuntime
{

    internal bool QiaoZhouFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (root.QiaoZhouForecast is not { } r)
            return false;
        return child is CardsMovedTriggerWindowFrame moved && moved.ResumeProgramFrameId == root.Id && moved.Batch.ParentFrameId == root.Id && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.SequenceBefore && m.Sequence <= r.SequenceAfter && (r.Stage == QiaoZhouForecastStage.RevealChildren && (m.Reason.Value == QiaoZhouRevealReason || m.Reason == CardMoveReasons.Reshuffle && m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile) || r.Stage == QiaoZhouForecastStage.CleanupChildren && m.Reason.Value == QiaoZhouCleanupReason && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile));
    }

    internal ResolutionFrame? QiaoZhouObserverRoot()
    {
        var root = _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterQiaoZhouOlOperations.QiaoZhouFirstChild(f, c), (i) => _coreObserverSubtreesOperations.HasMovementObserverSuffix(i));
        if (root is not null)
            return root;
        if (_resolutionStackOperations._resolutionStack.FirstOrDefault()is not QiaoZhouPreparationFrame prep || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[1] is not CardsMovedTriggerWindowFrame moved || prep.ActiveChildFrameId != moved.Id || !IsQiaoZhouPreparationMovement(prep, moved))
            return null;
        AssertQiaoZhouPreparation(prep);
        return _coreObserverSubtreesOperations.HasMovementObserverSuffix(0) ? prep : null;
    }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 26220)]
    internal bool AdvanceQiaoZhouSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && QiaoZhouObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
}
