#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterQinLangOlRuntime
{
    internal bool IsQinLangDiscardMovement(QinLangXianyingReceipt r, CardMovementRecord m) => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After && (r.Selected.Any(c => c.CardId == m.CardId && c.Kind == m.CardKind && c.From == m.From) && m.To == (m.CardKind == CardKind.GeneralWeapon && m.From.Zone == CardZoneKind.Equipment ? CardLocation.OutsideGame : CardLocation.DiscardPile) && m.Reason.Value == QinLangDiscardReason || r.Auxiliaries.Any(c => c.CardId == m.CardId && c.Kind == m.CardKind && c.From == m.From) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 860)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 800)]
    internal bool IsQinLangRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => f.QinLangXianying is { Stage: QinLangXianyingStage.DiscardChildren } r && ValidQinLang(f) && r.Selected.Any(c => c.Kind == CardKind.SilverLion && c.From == CardLocation.Equipment(f.OwnerSeat)) && recovery.ParentFrameId == f.Id && recovery.Return == new RecoveryReplacementReturn(PostEventContinuation.Program, f.Id) && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == QinLangDiscardReason && recovery.Attempt.SourceSeat == f.OwnerSeat && recovery.Attempt.TargetSeat == f.OwnerSeat && recovery.Attempt.Amount == 1;
    internal bool QinLangFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.QinLangXianying is not { } r || !ValidQinLang(f))
            return false;
        if (child is CardUseFrame use)
            return r.Stage == QinLangXianyingStage.NativeUse && use.QinLangXianying is { } o && MatchesQinLangXianying(f, o) && IsQinLangXianyingUse(use.Id);
        if (child is RecoveryReplacementFrame recovery)
            return IsQinLangRecoveryParent(f, recovery);
        if (child is CardsMovedTriggerWindowFrame moved)
        {
            if (moved.ResumeProgramFrameId != f.Id || moved.Batch.ParentFrameId != f.Id || moved.Batch.OriginOwnerSeat != f.OwnerSeat || moved.Batch.OriginSkillId != f.SkillId || moved.Batch.OriginSkillInstanceId != f.SkillInstanceId || moved.Batch.Movements.Count == 0)
                return false;
            if (r.Stage == QinLangXianyingStage.DrawChildren)
                return moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After && (m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == QinLangDrawReason || m.Reason == CardMoveReasons.Reshuffle));
            if (r.Stage == QinLangXianyingStage.DiscardChildren)
                return moved.Batch.Movements.All(m => IsQinLangDiscardMovement(r, m)) && (moved.Batch.Id == r.PaidBatchId || moved.Batch.ParentBatchId == r.PaidBatchId && moved.Batch.Movements.All(m => r.Auxiliaries.Any(c => c.CardId == m.CardId)));
            return false;
        }

        if (child is HpChangedTriggerWindowFrame hp)
            return r.Stage == QinLangXianyingStage.DiscardChildren && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.Program && hp.Change.ParentFrameId == f.Id && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == f.OwnerSeat && hp.Change.TargetSeat == f.OwnerSeat && hp.Change.Amount == 1 && r.Selected.Any(c => c.Kind == CardKind.SilverLion && c.From == CardLocation.Equipment(f.OwnerSeat));
        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal ProgramSkillFrame? QinLangObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterQinLangOlOperations.QinLangFirstChild(f, c), (i) => _coreObserverSubtreesOperations2.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations2.IsMovementObserverEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations2.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 1450)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 1560)]
    internal bool HasQinLangDamageObserver(long id) => QinLangObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame or BeforeDamageProgramWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 1580)]
    internal bool HasQinLangBeforeDamageObserver(long id) => HasQinLangDamageObserver(id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 1410)]
    internal bool IsQinLangDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && QinLangObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27170)]
    internal bool AdvanceQinLangChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && QinLangObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveXianyingDrawDiscard, SkillProgramEffectOp.ResolveXianyingTurnEndUses)]
    internal PromptChoice AiQinLang(PendingDecision p, ProgramSkillFrame f)
    {
        var r = f.QinLangXianying!;
        if (r.Stage == QinLangXianyingStage.EndChoice)
        {
            var view = _corePlayerViewProjectionOperations.CreateSnapshot(f.OwnerSeat);
            var benefit = new SkillProgramAiHint(0, 0, 0, 1, 0, 0, false, false);
            var harm = new SkillProgramAiHint(1, 0, 0, 0, 0, 0, false, false);
            var scored = p.Choices.Where(c => c.Parameters.GetValueOrDefault("branch") == "use").Select(c =>
            {
                var grant = r.Grants.Single(g => g.FrameId == long.Parse(c.Parameters["grant-id"], System.Globalization.CultureInfo.InvariantCulture));
                var friendly = grant.Name is CardKind.Peach or CardKind.Alcohol or CardKind.DrawTwo or CardKind.PeachGarden or CardKind.FiveGrains;
                var score = c.Targets.Count == 0 ? 1 : c.Targets.Sum(t => _coreAiBrainsAiBrainsAccess._aiBrains[f.OwnerSeat].ScoreProgramTarget(view, t, friendly ? benefit : harm));
                return (Choice: c, Score: score);
            }).OrderByDescending(x => x.Score).ThenBy(x => x.Choice.Id.Value, StringComparer.Ordinal).FirstOrDefault();
            return scored.Choice is not null && scored.Score >= 0 ? scored.Choice : p.Choices.Last();
        }

        var group = r.Cards.GroupBy(c => c.EffectiveName ?? QinLangName(c.Kind)).Where(g => QinLangImitable(g.Key)).SelectMany(g => Enumerable.Range(1, g.Count()).Where(n => !QinLangCountUsed(f.OwnerSeat, r.RoundNumber, n)).Select(n => g.OrderBy(c => global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.CardId), _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])).Take(n).ToArray())).OrderBy(g => g.Sum(c => global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.CardId), _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])) - 3).ThenBy(g => g.Length).FirstOrDefault();
        if (group is null)
        {
            var count = Enumerable.Range(0, r.Cards.Count + 1).First(n => !QinLangCountUsed(f.OwnerSeat, r.RoundNumber, n));
            group = r.Cards.OrderBy(c => global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.CardId), _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])).Take(count).ToArray();
        }

        var remove = r.Selected.FirstOrDefault(c => !group.Contains(c));
        var add = group.FirstOrDefault(c => !r.Selected.Contains(c));
        return remove is not null ? p.Choices.Single(c => c.Cards.SequenceEqual([remove.CardId])) : add is not null ? p.Choices.Single(c => c.Cards.SequenceEqual([add.CardId])) : p.Choices.Single(c => c.Parameters["branch"] == "pay");
    }
}
