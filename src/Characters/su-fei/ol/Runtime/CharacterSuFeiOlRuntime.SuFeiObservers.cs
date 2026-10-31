#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSuFeiOlRuntime
{
    [ProgramTargetWindow(ProgramTargetWindowStage.FinalizedAfterCategory, 60)]
    internal bool SuFeiFinalizedAfterCategoryTargetWindow(ProgramTargetCardWindowQuery query) => HasSuFeiCapability;
    [ProgramTargetWindow(ProgramTargetWindowStage.NormalizeImplicitDrawTwo, 70)]
    internal bool SuFeiNormalizeImplicitDrawTwoTargetWindow(ProgramTargetCardWindowQuery query) => HasSuFeiCapability;
    internal bool HasSuFeiCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.DrawThenOfferActualConsecutiveTargetGift);

    internal bool SuFeiOwnerQualified(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive && _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[seat]).GetInstanceTriggers(SkillProgramTriggerWindow.CardUseTargetsFinalized).Any(b => ProgramInstructionResolver.Default.Features(b.Trigger).HasOperation(SkillProgramEffectOp.DrawThenOfferActualConsecutiveTargetGift));
    internal int[] SuFeiPrimaryTargets(CardActionContext a)
    {
        if (a.EffectiveKind == CardKind.BorrowedSword)
        {
            // Some native conversion producers already compress DesignatedTargetSeats
            // to weapon holders. Only TargetSeats retains the complete ordered pairs.
            var pairs = a.TargetSeats;
            if (pairs.Count == 0 || pairs.Count % 2 != 0)
                return[];
            return pairs.Where((_, i) => i % 2 == 0).Distinct().ToArray();
        }

        // NormalizeImplicitDrawTwoUseTarget historically updates TargetSeats while
        // retaining an explicitly empty designated list. Recognize only that exact
        // native implicit-self shape here; other empty designation stays empty.
        if (a.EffectiveKind == CardKind.DrawTwo && a.EffectiveDesignatedTargetSeats.Count == 0 && a.TargetSeats.Count == 1 && a.TargetSeats[0] == a.ActorSeat)
            return[a.ActorSeat];
        return a.EffectiveDesignatedTargetSeats.Distinct().ToArray();
    }

    internal void CaptureSuFeiFinalizedUse(long nativeUseId, CardActionContext action)
    {
        if (!HasSuFeiCapability || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber < 1 || action.Type != CardActionType.Use || action.ActorSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || !SuFeiOwnerQualified(action.ActorSeat) && _coreMatchParticipantsPlayersAccess._players[action.ActorSeat].SuFeiLastUse is null)
            return;
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(nativeUseId)is not { Action: { } actual } use || actual.ActionId != action.ActionId || !global::CardGame.Core.EngineOwners.CoreGameEngineOverflowTargetCancellationObserversRuntime.SameOverflowAction(actual, action) || !_coreCompletedCategoryUseProofOperations.HasExactAcceptedActualHandGainUse(use, action))
            throw new InvalidOperationException("Lianpian requires the exact accepted native use and finalized target action.");
        if (use.SuFeiFinalizedUse is not null)
            return;
        var owner = _coreMatchParticipantsPlayersAccess._players[action.ActorSeat];
        var last = owner.SuFeiLastUse;
        if (last?.NativeUseId == nativeUseId)
            return;
        var current = new SuFeiActualUse(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, use.Id, action, SuFeiPrimaryTargets(action));
        SuFeiConsecutiveQualification? q = null;
        if (last is not null && last.ActualTurn == current.ActualTurn && last.PhaseInstance == current.PhaseInstance && last.Action.ActorSeat == action.ActorSeat && last.NativeUseId != use.Id)
        {
            var common = last.PrimaryTargets.Intersect(current.PrimaryTargets).Where(s => _coreTableSeatQueriesOperations.IsValidPlayerSeat(s)).ToArray();
            if (common.Length > 0)
                q = new(last, current, common);
        }

        owner.SuFeiLastUse = current;
        _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with { SuFeiQualification = q, SuFeiFinalizedUse = current });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SuFeiActualUseRecordedEvent(current));
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 490)]
    internal void ObserveSuFeiTurnOrDeath(IGameEvent payload)
    {
        if (payload is TurnEndedEvent ended && _coreMatchParticipantsPlayersAccess._players[ended.ActorSeat].SuFeiLastUse is not null)
            _coreMatchParticipantsPlayersAccess._players[ended.ActorSeat].SuFeiLastUse = null;
        else if (payload is PlayerDiedEvent dead)
            _coreMatchParticipantsPlayersAccess._players[dead.VictimSeat].SuFeiLastUse = null;
    }

    internal bool SuFeiUseIssued(SuFeiActualUse u) => u.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && u.PhaseInstance == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId && u.Action.Type == CardActionType.Use && u.Action.ActorSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && u.PrimaryTargets.SequenceEqual(SuFeiPrimaryTargets(u.Action)) && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<SuFeiActualUseRecordedEvent>().Count(e => e.Use.NativeUseId == u.NativeUseId && e.Use.ActualTurn == u.ActualTurn && e.Use.PhaseInstance == u.PhaseInstance && global::CardGame.Core.EngineOwners.CoreGameEngineOverflowTargetCancellationObserversRuntime.SameOverflowAction(e.Use.Action, u.Action) && e.Use.PrimaryTargets.SequenceEqual(u.PrimaryTargets)) == 1;
    internal bool ValidSuFeiQualification(SuFeiConsecutiveQualification q, int owner) => q.Current.Action.ActorSeat == owner && q.Previous.Action.ActorSeat == owner && q.Current.NativeUseId != q.Previous.NativeUseId && SuFeiUseIssued(q.Previous) && SuFeiUseIssued(q.Current) && q.CommonTargets.Count > 0 && q.CommonTargets.SequenceEqual(q.Previous.PrimaryTargets.Intersect(q.Current.PrimaryTargets).Where(_coreTableSeatQueriesOperations.IsValidPlayerSeat));
    internal bool SuFeiFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (!CanContinueSuFei(root) || root.SuFei is not { } r || child is not CardsMovedTriggerWindowFrame moved)
            return false;
        var before = r.Stage == SuFeiStage.DrawChildren ? r.DrawBefore : r.GiftBefore;
        var after = r.Stage == SuFeiStage.DrawChildren ? r.DrawAfter : r.GiftAfter;
        return r.Stage is SuFeiStage.DrawChildren or SuFeiStage.GiftChildren && moved.Batch.ParentFrameId == root.Id && moved.Batch.AwaitingProgramFrameId == root.Id && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > before && m.Sequence <= after && (r.Stage == SuFeiStage.DrawChildren ? m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(root.OwnerSeat) && m.Reason.Value == SuFeiDrawReason : m.CardId == r.DrawnId && m.From == CardLocation.Hand(root.OwnerSeat) && m.To == CardLocation.Hand(r.GiftTarget!.Value) && m.Reason.Value == SuFeiGiftReason));
    }

    internal ProgramSkillFrame? SuFeiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((r, c) => this.SuFeiFirstChild(r, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 290)]
    internal bool TryAdvanceSuFeiSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && SuFeiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
}
