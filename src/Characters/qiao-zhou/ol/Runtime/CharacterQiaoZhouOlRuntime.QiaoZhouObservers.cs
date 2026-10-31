#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterQiaoZhouOlRuntime
{
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 26220)]
    internal void AssertQiaoZhouForecast(ProgramSkillFrame f)
    {
        if (f.QiaoZhouForecast is not { } r)
            return;
        if (f.SkillId != "ol:xingbu" || global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f) != "forecast" || r.Source != QiaoZhouSource(f) || r.GameplayHash != f.GameplayHash || r.InstructionIndex != f.InstructionIndex || f.WindowContext is not { Window: SkillProgramTriggerWindow.TurnEnding } context || r.EndingWindowId != context.ParentFrameId || _resolutionStackOperations._resolutionStack.OfType<TurnEndingBoundaryFrame>().SingleOrDefault(w => w.Id == r.EndingWindowId)is not { } ending || ending.OwnerSeat != f.OwnerSeat || ending.TurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || ending.ItemIndex >= ending.Items.Count || ending.Items[ending.ItemIndex].Candidate is not { } candidate || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, candidate) || !Enum.IsDefined(r.Stage) || r.SequenceBefore < 0 || r.SequenceAfter < r.SequenceBefore || r.SequenceAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.Cards.Count > 3 || r.Cards.Select(c => c.CardId).Distinct().Count() != r.Cards.Count || r.RedCount != r.Cards.Count(c => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsRedSuit(c.Suit)) || r.Cards.Any(c => c.MovementSequence <= 0 || c.MovementSequence > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == c.MovementSequence && m.CardId == c.CardId && m.From == CardLocation.DrawPile && m.To == CardLocation.Processing && m.Reason.Value == QiaoZhouRevealReason) != 1) || _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramCardsRevealedEvent>().Count(e => e.FrameId == f.Id && e.SkillId == f.SkillId && e.Bind == QiaoZhouTableBind && e.Cards.Select(c => c.Id).SequenceEqual(r.Cards.Select(c => c.CardId)) && e.Cards.Select(c => c.Suit).SequenceEqual(r.Cards.Select(c => c.Suit))) != 1)
            throw new InvalidOperationException("Xingbu lost its frozen native reveal, colors, ending candidate or movement provenance.");
    }

    internal bool QiaoZhouFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (root.QiaoZhouForecast is not { } r)
            return false;
        AssertQiaoZhouForecast(root);
        return child is CardsMovedTriggerWindowFrame moved && moved.ResumeProgramFrameId == root.Id && moved.Batch.ParentFrameId == root.Id && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.SequenceBefore && m.Sequence <= r.SequenceAfter && (r.Stage == QiaoZhouForecastStage.RevealChildren && (m.Reason.Value == QiaoZhouRevealReason || m.Reason == CardMoveReasons.Reshuffle && m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile) || r.Stage == QiaoZhouForecastStage.CleanupChildren && m.Reason.Value == QiaoZhouCleanupReason && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile));
    }

    internal ResolutionFrame? QiaoZhouObserverRoot()
    {
        var root = _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterQiaoZhouOlOperations.QiaoZhouFirstChild(f, c), (i) => _coreObserverSubtreesOperations2.HasMovementObserverSuffix(i));
        if (root is not null)
            return root;
        if (_resolutionStackOperations._resolutionStack.FirstOrDefault()is not QiaoZhouPreparationFrame prep || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[1] is not CardsMovedTriggerWindowFrame moved || prep.ActiveChildFrameId != moved.Id || !IsQiaoZhouPreparationMovement(prep, moved))
            return null;
        AssertQiaoZhouPreparation(prep);
        return _coreObserverSubtreesOperations.HasMovementObserverSuffix(0) ? prep : null;
    }

    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 380)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 490)]
    internal bool HasQiaoZhouDamageObserver(long id) => QiaoZhouObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is BeforeDamageProgramWindowFrame or DamageTriggerWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 350)]
    internal bool IsQiaoZhouDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && QiaoZhouObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 26220)]
    internal bool AdvanceQiaoZhouSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && QiaoZhouObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
}
