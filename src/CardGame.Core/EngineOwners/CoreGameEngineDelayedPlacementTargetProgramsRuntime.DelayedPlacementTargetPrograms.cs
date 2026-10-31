#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDelayedPlacementTargetProgramsRuntime
{
    internal static LegalActionKind DelayedPlacementActionKind(CardKind kind) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_DelayedPlacementTargetPrograms.DelayedPlacementActionKind(kind);
    internal bool TryBeginFinalizedDelayedPlacementPrograms(long frameId, Card card, LegalActionKind actionKind)
    {
        if (_coreOriginalUseLightningOperations.IsOriginalUseLightning(frameId))
            return _coreOriginalUseLightningOperations.BeginOriginalUseLightningFinalized(frameId, card, actionKind);
        // This opt-in bridge is shared by physical play and the existing real
        // discarded-card and owned-card conversion producers. Their paid
        // movement children and committed-use observers have drained before
        // entering this method, including when protection will end the use.
        if (!(_characterCuiLieAnniversaryOperations.HasAnniversaryCuiLieCapability || _characterXunYuMobileGodOperations.HasDinghanCapability || _characterChenLinMouOperations.HasMouChenLin || _characterSimaShiMouOperations.HasMouSimaShi || _characterHuangQuanMouOperations.HasMouHuangQuan || _characterXuHuangMouMobileOperations.HasMouXuHuangCapability || _characterDiaoChanLeOperations.HasDiouCapability || _characterLiuYanOlOperations.HasLiuYanCapability || _characterBianFuRenOlOperations.HasBianFuRenCapability || _characterCaoChunOlOperations.HasCaoChunCapability || _characterYuanTanYuanShangOlOperations.HasNeifaCapability || _coreSoleTargetProtectionOperations.HasSoleTargetProtectionCapability || _characterSuFeiOlOperations.HasSuFeiCapability || _coreJianjiDeclarationsOperations.HasJianjiNativeUseCapability || _characterFanChouOlOperations.HasFanChouCapability || _characterGuoSiOlOperations.HasGuoSiCapability) || _coreCardUseLifecycleOperations.LifecycleCardUse(frameId)is not { } use || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsDelayedCard(use.CardKind) || use.FinalizedTrickProgramsStarted)
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != frameId || use.Action is not { Type: CardActionType.Use } action || action.ActionId <= 0 || action.ActorSeat != use.SourceSeat || action.EffectiveKind != use.CardKind || use.CardId != card.Id || actionKind != DelayedPlacementActionKind(use.CardKind) || action.TargetSeats is not [var target] || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || !action.TargetSeats.SequenceEqual(use.TargetSeats) || !(action.PhysicalCards.Any(cost => cost.CardId == card.Id) || _characterDiaoChanLeOperations.IsDiouUse(frameId)) || _cardZonesOperations._cardZones.GetLocation(card.Id) != CardLocation.Processing && !(_cardZonesOperations._cardZones.GetLocation(card.Id) == CardLocation.OutsideGame && _coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial3(frameId, card.Id)))
            throw new InvalidOperationException("A delayed placement lost its exact paid use and target.");
        _coreCardUseLifecycleOperations.TryMarkProgramUseAccepted(frameId);
        if (!_coreCardUseLifecycleOperations.TryMarkFinalizedTrickProgramsStarted(frameId))
            return false;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardActionAcceptedEvent(action));
        return _coreCardActionsOperations.TryBeginProgramCardWindow(null, action, SkillProgramTriggerWindow.CardUseTargetsFinalized, use.TargetSeats, ProgramCardContinuation.FinalizedDelayedPlacement, trickContinuation: new(card.Id, actionKind));
    }

    internal bool IsExactFinalizedDelayedPlacementWindow(ProgramCardTriggerWindowFrame window) => _coreOriginalUseLightningOperations.IsOriginalUseLightningFinalizedWindow(window) || (_characterCuiLieAnniversaryOperations.HasAnniversaryCuiLieCapability || _characterXunYuMobileGodOperations.HasDinghanCapability || _characterChenLinMouOperations.HasMouChenLin || _characterSimaShiMouOperations.HasMouSimaShi || _characterHuangQuanMouOperations.HasMouHuangQuan || _characterXuHuangMouMobileOperations.HasMouXuHuangCapability || _characterDiaoChanLeOperations.HasDiouCapability || _characterLiuYanOlOperations.HasLiuYanCapability || _characterBianFuRenOlOperations.HasBianFuRenCapability || _characterCaoChunOlOperations.HasCaoChunCapability || _characterYuanTanYuanShangOlOperations.HasNeifaCapability || _coreSoleTargetProtectionOperations.HasSoleTargetProtectionCapability || _characterSuFeiOlOperations.HasSuFeiCapability || _coreJianjiDeclarationsOperations.HasJianjiNativeUseCapability || _characterFanChouOlOperations.HasFanChouCapability || _characterGuoSiOlOperations.HasGuoSiCapability) && window.Continuation == ProgramCardContinuation.FinalizedDelayedPlacement && window.TrickContinuation is { TargetCardId: null, RequiredCardKind: null } continuation && window.SimpleContinuation is null && window.CompletedResponseReturn is null && window.AttackOwnerFrameId is null && _coreCardUseLifecycleOperations.LifecycleCardUse(window.ParentFrameId)is { FinalizedTrickProgramsStarted: true, ProgramUseAccepted: true, Action: { Type: CardActionType.Use } action } use && window.Action.Type == CardActionType.Use && action.ActionId == window.Action.ActionId && action.ActorSeat == use.SourceSeat && action.EffectiveKind == use.CardKind && window.Action.EffectiveKind == use.CardKind && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsDelayedCard(use.CardKind) && continuation.ActionKind == DelayedPlacementActionKind(use.CardKind) && continuation.EffectCardId == use.CardId && (action.PhysicalCards.Any(cost => cost.CardId == use.CardId) || _characterDiaoChanLeOperations.IsDiouUse(use.Id)) && window.Action.PhysicalCards.SequenceEqual(action.PhysicalCards) && (action.TargetSeats is [var target] && _coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || _characterCuiLieAnniversaryOperations.HasAnniversaryCuiLieDelayedTail(use) || action.TargetSeats.Count == 0 && (_characterXunYuMobileGodOperations.HasDinghanCancellation(use) || _characterCuiLieAnniversaryOperations.HasAnniversaryCuiLieCancellation(use))) && action.TargetSeats.SequenceEqual(use.TargetSeats);
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
        var continuation = completedWindow.TrickContinuation!;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(completedWindow.ParentFrameId)!;
        var action = use.Action!;
        var targetSeat = use.TargetSeats.Single();
        // Quarantine owns the material's outside-game movement and later
        // return. Its exact helper completes this use without placing or
        // discarding those already isolated physical entities.
        if (_coreSoleTargetQuarantineOperations.TryCompleteSoleTargetQuarantinedUse(use.Id))
            return;
        var card = _coreDamageColorDuelProgramsOperations.GetTrickRepresentation(use.Id, continuation.EffectCardId, requireProcessing: true);
        var target = _coreMatchParticipantsPlayersAccess._players[targetSeat];
        var forbidden = _coreProgramCardPoliciesOperations.HasCardPolicy(target, SkillProgramCardPolicyKind.ProhibitDelayedTrickTarget);
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || _coreProgramCardUseNullificationOperations.IsProgramUseIneffective(use) || _coreNamedAmbushInvalidationOperations.IsNamedAmbushUseIneffective(use) || use.IneffectiveTargetSeats?.Contains(targetSeat) == true || !target.IsAlive || target.JudgmentAreaAbolished || forbidden || _coreActualTurnsOperations.HasJudgmentEffectiveCard(target, use.CardKind) || use.CardKind != CardKind.Lightning && targetSeat == use.SourceSeat && !_characterLiuYanOlOperations.IsLiuYanLimuUse(use.Id))
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
