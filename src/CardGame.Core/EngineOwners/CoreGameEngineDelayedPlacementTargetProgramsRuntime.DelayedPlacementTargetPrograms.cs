#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDelayedPlacementTargetProgramsRuntime
{
    internal static LegalActionKind DelayedPlacementActionKind(CardKind kind) => RuntimeSkills.RuntimeRules_Core_DelayedPlacementTargetPrograms.DelayedPlacementActionKind(kind);
    internal bool TryBeginFinalizedDelayedPlacementPrograms(long frameId, Card card, LegalActionKind actionKind)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(frameId) is { OriginalUseLightning: not null })
            return _coreOriginalUseLightningOperations.BeginOriginalUseLightningFinalized(frameId, card, actionKind);
        // This opt-in bridge is shared by physical play and the existing real
        // discarded-card and owned-card conversion producers. Their paid
        // movement children and committed-use observers have drained before
        // entering this method, including when protection will end the use.
        if (!(_characterCuiLieAnniversaryOperations.HasAnniversaryCuiLieCapability || _characterXunYuMobileGodOperations.HasDinghanCapability || _characterChenLinMouOperations.HasMouChenLin || _characterSimaShiMouOperations.HasMouSimaShi || _characterHuangQuanMouOperations.HasMouHuangQuan || _characterXuHuangMouMobileOperations.HasMouXuHuangCapability || _characterDiaoChanLeOperations.HasDiouCapability || _characterLiuYanOlOperations.HasLiuYanCapability || _characterBianFuRenOlOperations.HasBianFuRenCapability || _characterCaoChunOlOperations.HasCaoChunCapability || _characterYuanTanYuanShangOlOperations.HasNeifaCapability || _coreSoleTargetProtectionOperations.Value.HasSoleTargetProtectionCapability || _actualUseHistoryContent.RequiresFact(ProgramTrackedFact.ActualCardUseHistory) || _coreJianjiDeclarationsOperations.Value.HasJianjiNativeUseCapability || _characterFanChouOlOperations.Value.HasFanChouCapability) || _coreCardUseLifecycleOperations.LifecycleCardUse(frameId)is not { } use || !CoreNativeCardClassificationsRuntime.IsDelayedCard(use.CardKind) || use.FinalizedTrickProgramsStarted)
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != frameId || use.Action is not { } action || actionKind != DelayedPlacementActionKind(use.CardKind) || action.TargetSeats is not [var target] || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || !action.TargetSeats.SequenceEqual(use.TargetSeats))
            throw new InvalidOperationException("A delayed placement lost its exact paid use and target.");
        var continuation = new ProgramTrickContinuation(card.Id, actionKind);
        _ = _nativeCardRepresentations.Get(frameId, card.Id, requireProcessing: true);
        _coreCardUseLifecycleOperations.TryMarkProgramUseAccepted(frameId);
        if (!_coreCardUseLifecycleOperations.TryMarkFinalizedTrickProgramsStarted(frameId))
            return false;
        _nativeTrickContinuations.ValidateIssuance(frameId, action, ProgramCardContinuation.FinalizedDelayedPlacement, continuation);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardActionAcceptedEvent(action));
        return _coreCardActionsOperations.TryBeginProgramCardWindow(null, action, SkillProgramTriggerWindow.CardUseTargetsFinalized, use.TargetSeats, ProgramCardContinuation.FinalizedDelayedPlacement, trickContinuation: continuation);
    }

    internal bool IsExactFinalizedDelayedPlacementWindow(ProgramCardTriggerWindowFrame window) =>
        window.Continuation == ProgramCardContinuation.FinalizedDelayedPlacement &&
        window.TrickContinuation is { TargetCardId: null, RequiredCardKind: null } continuation &&
        _nativeTrickContinuations.Matches(window) &&
        _coreCardUseLifecycleOperations.LifecycleCardUse(window.ParentFrameId) is { } use &&
        continuation.ActionKind == DelayedPlacementActionKind(use.CardKind);

    internal void ContinueFinalizedDelayedPlacement(ProgramCardTriggerWindowFrame completedWindow)
    {
        if (_coreOriginalUseLightningOperations.IsOriginalUseLightningFinalizedWindow(completedWindow))
        {
            _coreOriginalUseLightningOperations.ContinueOriginalUseLightningFinalized(completedWindow);
            return;
        }

        if (!IsExactFinalizedDelayedPlacementWindow(completedWindow) || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != completedWindow.ParentFrameId)
            throw new InvalidOperationException("A finalized delayed placement lost its native typed return.");
        if (_characterCuiLieAnniversaryOperations.TryBeginAnniversaryCuiLieDelayedPlacement(completedWindow))
            return;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(completedWindow.ParentFrameId)!;
        var targetSeat = use.TargetSeats.Single();
        // Quarantine owns the material's outside-game movement and later
        // return. Its exact helper completes this use without placing or
        // discarding those already isolated physical entities.
        if (_coreSoleTargetQuarantineOperations.TryCompleteSoleTargetQuarantinedUse(use.Id))
            return;
        var returned = _nativeTrickContinuations.Read(completedWindow);
        use = returned.Use;
        var action = use.Action!;
        var continuation = returned.Continuation;
        var card = returned.Card;
        var target = _coreMatchParticipantsPlayersAccess._players[targetSeat];
        var forbidden = _coreProgramCardPoliciesOperations.HasCardPolicy(target, SkillProgramCardPolicyKind.ProhibitDelayedTrickTarget);
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || _invalidations.IsUseIneffective(use) || use.IneffectiveTargetSeats?.Contains(targetSeat) == true || !target.IsAlive || target.JudgmentAreaAbolished || forbidden || _coreActualTurnsOperations.HasJudgmentEffectiveCard(target, use.CardKind) || use.CardKind != CardKind.Lightning && targetSeat == use.SourceSeat && !_characterLiuYanOlOperations.IsLiuYanLimuUse(use.Id))
        {
            _coreProgramCardTargetEffectsOperations.MarkCardEffectIneffective(use.Id, targetSeat);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardEffectSkippedEvent(use.Id, use.SourceSeat, targetSeat, use.CardKind, forbidden || use.IneffectiveTargetSeats?.Contains(targetSeat) == true ? CardEffectSkipReason.SkillNullified : CardEffectSkipReason.PublicTargetMissing));
            _coreMatchLogOperations.AddLog("CardEffect", $"【{(CardCatalog.Get(use.CardKind).DisplayName)}】的目标已不能承接此延时牌，本次放置跳过。", use.SourceSeat, targetSeat);
            _coreNativeCardUseCompletionOperations.FinishCardUse(use.Id, card, use.CardKind);
            return;
        }

        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(use.Id, card, action.ActorSeat, use.TargetSeats, continuation.ActionKind, playedCardKind: use.CardKind);
    }
}
