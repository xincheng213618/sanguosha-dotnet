#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeCoreInvariantsRuntime
{
    internal void AssertCoreInvariants()
    {
        _coreColdDamageOperations.AssertColdDamageReplacementFrames();
        _coreShanheS5YimieOperations.AssertShanheS5YimieRecoveryFrames();
        _characterHuaTuoAnniversaryGodOperations.AssertSkillInvocations();
        _characterCaoPiAnniversaryGodOperations.AssertKemingEchoFrames();
        _coreNativeEquipmentInvocationsOperations.AssertNativeEquipmentInvocations();
        _coreWenyiDamageHealthOperations.AssertWenyiDamageHealth();
        _characterDaQiaoXiaoQiaoHappyGodOperations.AssertHappyQiaoDamageHealth();
        _characterDaQiaoXiaoQiaoHappyGodOperations.AssertHappyQiaoCardMaximumHealth();
        _characterHuanShuJunOlOperations.AssertHuanpeiRequests();
        _characterZhaoZhongOlOperations.AssertYintianDamageStates();
        _characterLuXunWumiaoOperations.AssertWuXiongmuDamageStates();
        _characterSharedGameEngineOlMoRumoProofsOperations.AssertOlMoRumoStates();
        _coreHulaoCardsOperations.AssertHulaoCardState();
        _coreShenwuCardsOperations.AssertShenwuCardState();
        _coreMantianGuohaiOperations.AssertMantianGuohaiState();
        _coreHellSoulCardsOperations.AssertHellSoulCardState();
        _coreFriendDuelResponsesOperations.AssertFriendEventFrames();
        _coreRequestedDeckBasicOwnershipOperations.AssertRequestedDeckBasicMaterials();
        _coreDrawFundedDistinctBasicObserversOperations.AssertDrawFundedDistinctBasicFramesAndUses();
        _coreChainedStateBasicReturnsOperations.AssertChainedStateBasicFramesAndUses();
        _characterGuoTuOlOperations.AssertWeijieRequests();
        _characterLiuZhangOlOperations.AssertZonghuRequests();
        _coreForeignSelectedCardSlashReturnsOperations.AssertSelectedForeignCardSlashes();
        var coreDecision = _characterHuanShuJunOlOperations.HuanpeiInvariantDecision(_characterLiuZhangOlOperations.ZonghuInvariantDecision(_characterGuoTuOlOperations.WeijieInvariantDecision((_characterChengJiChengCuiOlOperations.TousuiNativeOriginalDecision(_coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade?.Attack.ResolutionId ?? 0) ?? _characterChengJiChengCuiOlOperations.TousuiNativeOriginalDecision(_coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword?.ResolutionId ?? 0) ?? _characterMaXiuMaTieOlOperations.KenshangNativeOriginalDecision(_coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade?.Attack.ResolutionId ?? 0) ?? _characterMaXiuMaTieOlOperations.KenshangNativeOriginalDecision(_coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword?.ResolutionId ?? 0)) ?? _coreChainedStateBasicReturnsOperations.ChainedStateBasicInvariantDecision(_coreDrawFundedDistinctBasicObserversOperations.DrawFundedDistinctBasicInvariantDecision(_coreRequestedDeckBasicOwnershipOperations.RequestedDeckBasicInvariantDecision())))));
        var coreTop = _characterHuanShuJunOlOperations.HuanpeiInvariantTop(_characterLiuZhangOlOperations.ZonghuInvariantTop(_characterGuoTuOlOperations.WeijieInvariantTop(_coreChainedStateBasicReturnsOperations.ChainedStateBasicInvariantTop(_coreDrawFundedDistinctBasicObserversOperations.DrawFundedDistinctBasicInvariantTop(_coreRequestedDeckBasicOwnershipOperations.RequestedDeckBasicInvariantTop())))));
        coreDecision = _characterLiuFengBoundaryOperations.BoundaryLiuFengInvariantDecision(coreDecision);
        coreTop = _characterLiuFengBoundaryOperations.BoundaryLiuFengInvariantTop(coreTop);
        _characterLiuFengBoundaryOperations.AssertBoundaryLiuFengRequests();
        var responseHealthCursor = _coreTypedRecoveryDamageCursorOperations.ProjectTypedResponseCompletionHealthCursor();
        if (responseHealthCursor.IsMalformed)
            throw new InvalidOperationException("A native response health child lost its exact paid return.");
        var committedPaymentCursor = _coreTypedRecoveryDamageCursorOperations.ProjectTypedCommittedSlashPaymentCursor();
        if (committedPaymentCursor.IsMalformed)
            throw new InvalidOperationException("A committed Slash card-use payment child lost its exact paid return.");
        var damageCursorProjection = _coreDamageCursorRuntimeOperations.ActiveDamageTrigger is { } damageCursorWindow ? _coreDamageObserverCursorQueriesOperations.ResolveDamageCursor(damageCursorWindow) : default;
        var damageProgramDying = damageCursorProjection.OwnedDyingId is { } ownedDyingId && _coreDyingCursorRuntimeOperations.ActiveDying?.Id == ownedDyingId || responseHealthCursor.Owner is not null && responseHealthCursor.OwnedDyingId is { } responseDyingId && _coreDyingCursorRuntimeOperations.ActiveDying?.Id == responseDyingId || committedPaymentCursor.Owner is not null && committedPaymentCursor.OwnedDyingId is { } paymentDyingId && _coreDyingCursorRuntimeOperations.ActiveDying?.Id == paymentDyingId;
        if (_coreDyingCursorRuntimeOperations.ActiveDying is not null && !damageProgramDying)
        {
            damageProgramDying = _coreProgramRuntimeHooksOperations.RuntimeHooks.IsDying(ProgramDyingProofPhase.Invariant);
        }

        _turnCardUseEffectsOperations._turnCardUseEffects.AssertInvariants();
        _coreJudgedRankSlashPoliciesOperations.AssertJudgedRankSlashPolicies();
        _coreFixedDistanceOneDebtOperations.AssertFixedDistanceOnePolicies();
        _corePaidHpLossProgramsOperations.AssertPaidHpLossModifiers();
        _coreAlcoholSlashSuppressionsOperations.AssertCurrentTurnOwnSkillSuppressions();
        _characterZhugeGuoLeOperations.AssertLeChengyanEffects();
        _coreUnexpectedAssaultOperations.AssertUnexpectedAssaultReceipts();
        foreach (var floodUse in _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>())
            _characterGuanYuWumiaoOperations.AssertWumiaoFlood(floodUse);
        foreach (var qizhengUse in _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>())
            _characterXunYuMobileGodOperations.AssertQizheng(qizhengUse);
        foreach (var leizhenUse in _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>())
            _coreShanheLeizhenOperations.AssertShanheLeizhenUse(leizhenUse);
        _coreExtraCardEffectRoundsOperations.AssertExtraCardEffectReceipts();
        _characterHuangYueyingOlMouOperations.AssertMouBingcaiExtraEffectRounds();
        _characterSunQuanOlMagicOperations.AssertMoQuanyuSlashUses();
        _characterFeiYiOlOperations.AssertFeiYiEffectRounds();
        foreach (var use in _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>())
            _coreNoDamageSkillDisableOperations.AssertNoDamageSkillDisablePolicy(use);
        _coreHpChangeProgramsOperations.AssertPostEventProgramInvariants();
        _coreNativeDrawAttemptsOperations.AssertNativeDrawAttempts();
        foreach (var use in _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>())
        {
            _coreNativeCardEffectDrawsOperations.AssertNativeCardEffectDraw(use);
            _characterGuanHaiOlOperations.AssertXieweiPayments(use);
        }

        _coreWenheCardsOperations.AssertWenheCardState();
        _coreGuanduCardsOperations.AssertGuanduCardState();
        _coreDragonBoatCardsOperations.AssertDragonBoatCardState();
        _coreRecoveryReplacementOperations.AssertRecoveryReplacementInvariants();
        _characterDaQiaoXiaoQiaoHappyGodOperations.AssertHappyQiaoHealthFrames();
        foreach (var frame in _resolutionStackOperations._resolutionStack)
            _coreHappyWeiYanGainDestinationsOperations.AssertHappyWeiYanRetainedGainSettlements(frame);
        _characterPangJiOlOperations.AssertDouyuWindows();
        _characterLuKangWumiaoOperations.AssertWuLuKangWindows();
        _corePindianOperations.AssertPindianInvariant();
        _corePindianRevealWindowsOperations.AssertPindianRevealWindows();
        _coreCardActionsOperations.AssertProgramCardWindowState();
        _coreCardEffectProgramsOperations.AssertCardEffectProgramState();
        _coreJudgmentProgramsOperations.AssertProgramJudgmentWindowState();
        _coreDiscardOperations.AssertDiscardPromptInvariant();
        _characterLuMengMouOperations.AssertYingboInvariant();
        _coreGrantedEntityDistanceInvariantsOperations.AssertGrantedEntityDistancePolicies();
        _coreSharedSlashOffersOperations.AssertSharedSlashUseReturns();
        _cardZonesOperations._cardZones.AssertInvariants(_corePhysicalCardPopulationInitialCardCountAccess._initialCardCount + _coreGameEngineAdvancedWeaponsGeneratedPhysicalCardIdsAccess._generatedPhysicalCardIds.Count);
        foreach (var player in _coreMatchParticipantsPlayersAccess._players)
        {
            var equipment = _corePhysicalOwnedCardQueriesOperations.GetEquipment(player);
            if (equipment.Any(card => !_coreEquipmentContentOperations.EquipmentRules.IsEquipment(card.Kind) && !_characterWeiYanHappyGodOperations.HasExactHappyBonePlacement(card, player.Seat)))
            {
                throw new InvalidOperationException($"Player {(player.Seat)} equipment zone contains a non-equipment card.");
            }

            if (equipment.GroupBy(card => _coreFieldEquipmentOperations.FieldEquipmentSlot(card)).Any(group => group.Count() > player.EquipmentSlotCapacity(group.Key)))
            {
                throw new InvalidOperationException($"Player {(player.Seat)} exceeds an equipment slot's capacity.");
            }
        }

        foreach (var(cardId, effectiveKind)in _coreJudgmentEffectiveCardsJudgmentEffectiveCardKindsAccess._judgmentEffectiveCardKinds)
        {
            if (_cardZonesOperations._cardZones.GetLocation(cardId).Zone != CardZoneKind.Judgment || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsDelayedCard(effectiveKind))
            {
                throw new InvalidOperationException($"Judgment effective card {(cardId)}/{(effectiveKind)} is outside a delayed judgment zone.");
            }
        }

        _coreRecipientScopedDamageOperations.AssertRecipientScopedDamageStates();
        _coreCardDamageMaximumHpReplacementOperations.AssertMaximumHpDamageReplacementReceipts();
        _coreDamageArmorPaymentsOperations.AssertDamageArmorPayments();
        _coreOwnedDeathBenefitObserversOperations.AssertOwnedDeathBenefitReturns();
        _coreDyingSuitsAndHistoricalUseObserversOperations.AssertDyingSuitsSubtree();
        _coreRecipientConsequencesObserversOperations.AssertRecipientConsequencesSubtrees();
        if (_resolutionStackOperations._resolutionStack.OfType<DeathFrame>().LastOrDefault()is { } currentDeath)
        {
            _coreDeathProgramsOperations.GetCurrentDeathFrame(currentDeath.Id);
            if (_characterDianWeiHappyGodOperations.TryAssertHappyDianWeiDeathRemoval(currentDeath))
                return;
            if (_resolutionStackOperations._resolutionStack.OfType<ProgramKillTriggerWindowFrame>().LastOrDefault()is not null || _resolutionStackOperations._resolutionStack.Any(frame => frame is ProgramSkillFrame program && program.WindowContext is { Window: SkillProgramTriggerWindow.CharacterDied }))
            {
                _coreDeathProgramsOperations.AssertKillDiedProgramInvariant();
                return;
            }

            _coreDeathProgramsOperations.AssertOwnerDiedProgramInvariant();
            return;
        }

        _coreColorFireAttacksOperations.AssertColorFireAttackReceipts();
        _coreDualColorDuelProgramsOperations.AssertDualColorDuels();
        _coreConditionalDiscardDuelReturnsOperations.AssertConditionalDiscardDuels();
        _coreKuangfuReceiptsOperations.AssertKuangfuPrograms();
        _coreInspectedHandSlashReturnsOperations.AssertInspectedHandSlashes();
        _corePlacedEquipmentBenefitObserversOperations.AssertPlacedEquipmentBenefits();
        _coreDamageColorDuelProgramsOperations.AssertSelectedActorDuels();
        _characterBaoSanNiangOlOperations.AssertChosenAssaults();
        _characterTianChouOlOperations.AssertTianChouNativeUses();
        _characterMaXiuMaTieOlOperations.AssertKenshangRequests();
        _characterChengJiChengCuiOlOperations.AssertTousuiRequests();
        _coreNonBasicSlashProfileOperations.AssertNonBasicSlashPolicies();
        _coreVirtualOrdinaryTrickUsesOperations.AssertProgramVirtualOrdinaryTricks();
        _coreDamageTargetObtainProgramsOperations.AssertDamageTargetDuels();
        _coreQinglongInsertionOperations.AssertQinglongInsertions();
        var processing = _coreProgramViewContributorsOperations.ViewContributors.FilterProcessing(ProgramViewContribution.InvariantProcessing, _cardZonesOperations._cardZones.CardsAt(CardLocation.Processing));
        var hasActiveCardResolution = (_characterDianWeiHappyGodOperations.HasHappyDianWeiGuardPrompt() || _characterPangTongHappyGodOperations.HasHappyPangTongPrompt() || _characterDiaoChanHappyGodOperations.HasHappyDiaoChanPrompt() || _characterDaQiaoXiaoQiaoHappyGodOperations.HasHappyQiaoHealthPrompt() || _characterZuoCiHappyGodOperations.HasHappyZuoCiPrompt() || _characterHuZhaoAnniversaryOperations.HasAnniversaryHuZhaoPrompt() || _characterXuJingAnniversaryOperations.HasAnniversaryXuJingPrompt() || _characterDuKuiAnniversaryOperations.HasDuKuiPrompt() || _characterLiuYongAnniversaryOperations.HasAnniversaryLiuYongPrompt() || _characterLiuLiAnniversaryOperations.HasAnniversaryLiuLiPrompt() || _characterBaiLingYunAnniversaryOperations.HasAnniversaryBaiLingYunPrompt() || _characterZhangFenAnniversaryOperations.HasZhangFenPrompt() || _characterYangBiaoAnniversaryOperations.HasAnniversaryYangBiaoPrompt() || _characterHeYanAnniversaryOperations.HasAnniversaryHeYanPrompt() || _characterGuoZhaoAnniversaryOperations.HasAnniversaryGuoZhaoPrompt() || _characterFengXiAnniversaryOperations.HasAnniversaryFengXiPrompt() || _characterMangYaChangAnniversaryOperations.HasAnniversaryJiedaoPrompt() || _characterFuRongFuQianAnniversaryOperations.HasAnniversaryFuRongFuQianPrompt() || _characterGaoXiangAnniversaryOperations.HasAnniversaryGaoXiangPrompt() || _characterZhangWenAnniversaryOperations.HasAnniversaryZhangWenPrompt() || _characterZhangXunAnniversaryOperations.HasAnniversaryZhangXunPrompt() || (_characterSunHuanAnniversaryOperations.HasAnniversarySunHuanPrompt() || _characterGuoYuanMobileShiOperations.HasShiGyPrompt() || _characterLuSuMobileShiOperations.HasShiLsPrompt()) || _characterMuShunAnniversaryOperations.HasAnniversaryMuShunPrompt() || _characterXingDaoRongAnniversaryOperations.HasAnniversaryXingDaoRongPrompt() || _characterHanSongAnniversaryOperations.HasAnniversaryHanSongPrompt() || _characterXiahouHuiAnniversaryOperations.HasAnniversaryXiahouHuiPrompt() || _characterXunXuAnniversaryOperations.HasAnniversaryXunXuPrompt() || _characterZhangHuanAnniversaryOperations.HasAnniversaryZhangHuanPrompt() || _characterCheZhouAnniversaryOperations.HasAnniversaryCheZhouPrompt() || _characterWangChangAnniversaryOperations.HasAnniversaryWangChangPrompt() || _characterWuFuAnniversaryOperations.HasAnniversaryWuFuPrompt() || _characterMaTieAnniversaryOperations.HasAnniversaryMaTiePrompt() || _characterYanXiangAnniversaryOperations.HasAnniversaryYanXiangPrompt() || _characterCuiLieAnniversaryOperations.HasAnniversaryCuiLiePrompt() || _characterHouChengAnniversaryOperations.HasAnniversaryHouChengPrompt() || _characterMaMiaoAnniversaryOperations.HasAnniversaryMaMiaoPrompt() || _characterYuanYinAnniversaryOperations.HasAnniversaryYuanYinPrompt() || _characterQiaoRuiAnniversaryOperations.HasAnniversaryQiaoRuiPrompt() || _characterHuCheErAnniversaryOperations.HasAnniversaryHuCheErPrompt() || _characterZhangHengAnniversaryOperations.HasAnniversaryZhangHengPrompt() || _characterLeiBoAnniversaryOperations.HasAnniversaryLeiBoPrompt() || _characterHaoMengAnniversaryOperations.HasAnniversaryHaoMengPrompt() || _characterHeQiMobileOperations.HasMobileHeQiPrompt() || _characterLiuFengBoundaryOperations.HasBoundaryLiuFengPrompt()) || _characterZhugeGuoLeOperations.HasLeChengyanBorrowedPrompt() || _coreWenhePoisonOperations.HasWenhePoisonFireAttackPrompt() || _coreHulaoCardsOperations.HasHulaoAllianceFeastPrompt() || _coreShenwuCardsOperations.HasShenwuTrickPrompt() || _coreMantianGuohaiOperations.HasMantianGuohaiPrompt() || _coreHellSoulCardsOperations.HasHellSoulPrompt() || _coreWenheChaosOperations.HasWenheChaosPrompt() || _characterGuanYuWumiaoOperations.HasWumiaoFloodDiscardPrompt() || _characterXunYuMobileGodOperations.HasQizhengPrompt() || _coreDragonBoatCardsOperations.HasDragonBoatDiscardPrompt() || _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Any(frame => frame.ColorFireAttack?.PaidCardId is not null) || _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not null || _coreNativeCardContinuationCursorsOperations.ActiveQinglongFollowup is not null || _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword is not null || _coreNativeCardContinuationCursorsOperations.ActiveGroupCard is not null || _coreNativeCardContinuationCursorsOperations.ActiveFireAttack is not null || _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow is not null || _characterLuMengMouOperations.ActiveYingboGift is not null || _coreNativeCardContinuationCursorsOperations.ActiveTargetCardSelection is not null || _coreJudgmentCursorRuntimeOperations.ActiveJudgment is not null || _coreProgramStateCardContinuationOperations.HasCharacterStateCardUseContinuation() || _coreSoleIncomingRangeDrawOperations.HasSoleIncomingRangeDrawProcessing(processing) || _coreNativeCardEffectDrawsOperations.HasNativeCardEffectDrawProcessing(processing) || _coreWenheCardsOperations.HasWenheCardEffectProcessing(processing) || _coreGuanduCardsOperations.HasGuanduCardEffectProcessing(processing) || _characterGuanYuWumiaoOperations.HasWumiaoFloodProcessing(processing) || _coreDragonBoatCardsOperations.HasDragonBoatCardEffectProcessing(processing) || _characterXunYuMobileGodOperations.HasQizhengProcessing(processing) || _coreOwnTrickDrawTargetsOperations.HasOwnTrickDrawOfferProcessing(processing) || _coreProgramCardUsePaymentsOperations.CardUsePayments.AllowsProcessing(processing) || _resolutionStackOperations._resolutionStack.Any(frame => frame is CardEffectBeforeApplyFrame or ProgramSkillFrame or ProgramCardTriggerWindowFrame or PindianFrame or HpChangedTriggerWindowFrame or RecoveryReplacementFrame or CardDeclarationFrame);
        if (!hasActiveCardResolution && processing.Count != 0)
        {
            throw new InvalidOperationException("Processing contains cards without an active resolution.");
        }

        if (_coreCardDeclarationsOperations.AssertCardDeclarationInvariant())
            return;
        if (_coreActiveNativeAttackQueriesOperations.ActiveCardAttack is { } attack && !IsActiveAttackCardConsistent(attack, processing))
        {
            throw new InvalidOperationException($"The active card resolution and Processing zone are inconsistent " + $"(attack={(attack.ResolutionId)}/{(attack.EffectiveCardKind)}, physical=[{(string.Join(',', attack.PhysicalCards.Select(card => card.Id)))}], " + $"processing=[{(string.Join(',', processing.Select(card => card.Id)))}], source={(attack.SourceSeat)},target={(attack.TargetSeat)},program={(attack.ProgramSkillFrameId)},duel={(_coreNativeCardContinuationCursorsOperations.ActiveDuel?.ResolutionId)},stack={(string.Join(';', _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Select(f => $"{(f.Id)}:{(string.Join(',', f.SelectedTargetSeats))}")))}).");
        }

        if (_coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword is { } borrowedSword)
        {
            var parentFrame = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().FirstOrDefault(frame => frame.Id == borrowedSword.ResolutionId);
            var parentMaterialAccountedFor = _characterZhugeLiangMouOperations.IsZeroEntityProcessingParent(borrowedSword.ResolutionId, borrowedSword.Card.Id) || _coreCurrentUsePhysicalClaimsOperations.IsClaimedUseCardEntity(borrowedSword.ResolutionId, borrowedSword.Card.Id) || processing.Any(card => card.Id == borrowedSword.Card.Id);
            var responseWindow = _resolutionStackOperations._resolutionStack.OfType<ResponseWindowFrame>().LastOrDefault(frame => frame.ParentFrameId == borrowedSword.ResolutionId);
            var paidOwnerChoice = _coreDrawFundedDistinctBasicObserversOperations.DrawFundedDistinctBasicObserverRoot(borrowedSword.ResolutionId)is { } paidBorrowed && paidBorrowed.Payment is { Intent: DrawFundedDistinctBasicIntent.BorrowedSword, Cursor: 0 } payment && payment.Source.OwnerSeat == borrowedSword.WeaponOwnerSeat && payment.TargetSeat == borrowedSword.SlashTargetSeat && payment.ParentFrameId == borrowedSword.ResolutionId && payment.RequestFrameId == responseWindow?.Id && payment.ParentActionId == parentFrame?.Action?.ActionId && paidBorrowed.TargetSeats.SequenceEqual([borrowedSword.SlashTargetSeat]) && paidBorrowed.OriginalDecision is { Kind: DecisionKind.RespondSlash } originalBorrowed && originalBorrowed.PlayerSeat == borrowedSword.WeaponOwnerSeat && originalBorrowed.PromptId == payment.OriginalPromptId;
            paidOwnerChoice |= _characterWoLongFengChuOlOperations.HasWoLongFengChuBorrowedPaymentRide(borrowedSword);
            paidOwnerChoice |= _characterHuanShuJunOlOperations.HasHuanpeiBorrowedPaymentRide(borrowedSword);
            var awaitingOwnerChoice = borrowedSword.AwaitingSlashChoice && responseWindow is not null && responseWindow.ResponderSeat == borrowedSword.WeaponOwnerSeat && (coreDecision is { Kind: DecisionKind.RespondSlash } borrowedDecision && borrowedDecision.PlayerSeat == borrowedSword.WeaponOwnerSeat || paidOwnerChoice);
            var awaitingFactionSlash = _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is { IsBorrowedSwordUse: true } borrowedFactionSlash && (borrowedFactionSlash.AwaitingProviders || borrowedFactionSlash.AwaitingZhuqueFanChoice) && global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameContinuationOwner(borrowedFactionSlash.BorrowedSword, borrowedSword);
            var resolvingSlash = borrowedSword.ActiveAttack is { } borrowedAttack && (global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, borrowedAttack) || _characterLiuFengBoundaryOperations.HasBoundaryLiuFengBorrowedSettlementRide(borrowedSword));
            var unansweredRenjie = _characterSimaYiMobileGodOperations.HasMobileSimaRenjieNativeSubtree(borrowedSword.ResolutionId);
            if (parentFrame is null || global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(parentFrame) != CardKind.BorrowedSword || !parentMaterialAccountedFor || (awaitingOwnerChoice ? 1 : 0) + (awaitingFactionSlash ? 1 : 0) + (resolvingSlash ? 1 : 0) + (unansweredRenjie ? 1 : 0) != 1)
            {
                throw new InvalidOperationException("A Borrowed Sword continuation must retain its parent card and exactly one response or Slash continuation.");
            }
        }

        IReadOnlyList<Card> groupProcessing = _coreNativeCardContinuationCursorsOperations.ActiveGroupCard is { } nativeGroup && (_characterDengAiAnniversaryGodOperations.ShenDaCuixinAncestorProcessing(nativeGroup.ResolutionId) ?? _characterZhouYuMouOperations.MouZyNativeAncestorProcessing(nativeGroup.ResolutionId) ?? _characterXiaoQiaoMouOlOperations.MouXqNativeAncestorProcessing(nativeGroup.ResolutionId) ?? _characterDiaoChanLeOperations.DiouNativeAncestorProcessing(nativeGroup.ResolutionId))is { } diouGroupAncestors ? processing.Where(card => !diouGroupAncestors.Contains(card.Id)).ToArray() : processing;
        if (_coreNativeCardContinuationCursorsOperations.ActiveGroupCard is { Effect: GroupCardEffect.Recovery } recoveryGroup && (_characterZhugeLiangMouOperations.IsZeroEntityUse(recoveryGroup.ResolutionId) || _coreCurrentUsePhysicalClaimsOperations.IsClaimedUseCardEntity(recoveryGroup.ResolutionId, recoveryGroup.Card.Id) || _coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial(recoveryGroup.ResolutionId, recoveryGroup.Card.Id) ? groupProcessing.Count != 0 : groupProcessing.Count != 1 || groupProcessing[0].Id != recoveryGroup.Card.Id))
        {
            throw new InvalidOperationException("The active group recovery and Processing zone are inconsistent.");
        }

        if (_coreNativeCardContinuationCursorsOperations.ActiveGroupCard is { Effect: GroupCardEffect.PublicDraft } publicDraft && (groupProcessing.Count != (_characterZhugeLiangMouOperations.IsZeroEntityUse(publicDraft.ResolutionId) || _coreCurrentUsePhysicalClaimsOperations.IsClaimedUseCardEntity(publicDraft.ResolutionId, publicDraft.Card.Id) || _coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial(publicDraft.ResolutionId, publicDraft.Card.Id) ? 0 : 1) + publicDraft.RevealedCardIds.Count || !_characterZhugeLiangMouOperations.IsZeroEntityUse(publicDraft.ResolutionId) && !_coreCurrentUsePhysicalClaimsOperations.IsClaimedUseCardEntity(publicDraft.ResolutionId, publicDraft.Card.Id) && !_coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial(publicDraft.ResolutionId, publicDraft.Card.Id) && groupProcessing.All(card => card.Id != publicDraft.Card.Id) || publicDraft.RevealedCardIds.Any(cardId => groupProcessing.All(card => card.Id != cardId))))
        {
            throw new InvalidOperationException("The active FiveGrains draft and Processing zone are inconsistent.");
        }

        if (_coreNativeCardContinuationCursorsOperations.ActiveFireAttack is { } fireAttack)
        {
            var retainedFireAttack = !_characterZhugeLiangMouOperations.IsZeroEntityUse(fireAttack.Id) && !_coreCurrentUsePhysicalClaimsOperations.IsClaimedUseCardEntity(fireAttack.Id, fireAttack.CardId) && (!_coreHellSoulCardsOperations.IsHellDestroyedSoulMaterial(fireAttack.Id, fireAttack.CardId) && !(_characterHuBanOlOperations.IsHuiyunOutsideGeneralWeaponMaterial(fireAttack.Id, fireAttack.CardId) || _characterMiHengLeOperations.IsSiruiOutsideGeneralWeaponMaterial(fireAttack.Id, fireAttack.CardId)) && !(_coreGuanduEquipmentOperations.IsGuanduDestroyedChariotMaterial(fireAttack.Id, fireAttack.CardId) || _coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial(fireAttack.Id, fireAttack.CardId)));
            var expectedProcessingCount = retainedFireAttack ? 1 : 0;
            IReadOnlyList<Card> fireProcessing = (_characterDengAiAnniversaryGodOperations.ShenDaCuixinAncestorProcessing(fireAttack.Id) ?? _characterZhouYuMouOperations.MouZyNativeAncestorProcessing(fireAttack.Id) ?? _characterXiaoQiaoMouOlOperations.MouXqNativeAncestorProcessing(fireAttack.Id) ?? _characterDiaoChanLeOperations.DiouNativeAncestorProcessing(fireAttack.Id))is { } diouFireAncestors ? processing.Where(card => !diouFireAncestors.Contains(card.Id)).ToArray() : processing;
            if (fireProcessing.Count != expectedProcessingCount || retainedFireAttack && fireProcessing.All(card => card.Id != fireAttack.CardId) || (fireAttack.FireAttackSelection?.RevealedCardId is { } revealedId && fireAttack.ColorFireAttack?.PaidCardId is null && fireAttack.MouZhitianPayment is null && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[global::CardGame.Core.EngineOwners.CoreNativeFireAttackRuntime.GetFireAttackTargetSeat(fireAttack)]).All(card => card.Id != revealedId)))
            {
                throw new InvalidOperationException("The active FireAttack and Processing zone are inconsistent.");
            }

            if (_coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not null || _coreNativeCardContinuationCursorsOperations.ActiveDuel is not null || _coreNativeCardContinuationCursorsOperations.ActiveGroupCard is not null || _coreDyingCursorRuntimeOperations.ActiveDying is not null || _coreDamageCursorRuntimeOperations.ActiveDamageTrigger is not null || _coreNativeCardContinuationCursorsOperations.ActiveTargetCardSelection is not null)
            {
                throw new InvalidOperationException("A FireAttack selection cannot coexist with another card continuation.");
            }

            if (coreTop is not CardUseFrame cardUse || !ReferenceEquals(cardUse, fireAttack) || global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(cardUse) != CardKind.FireAttack)
            {
                throw new InvalidOperationException("A FireAttack selection must retain its CardUse frame as the stack top.");
            }

            var expectedKind = fireAttack.FireAttackSelection?.RevealedCardId is null ? DecisionKind.FireAttackReveal : DecisionKind.FireAttackDiscard;
            if (coreDecision is not { } fireDecision || fireDecision.Kind != expectedKind || fireDecision.PlayerSeat != (expectedKind == DecisionKind.FireAttackReveal ? global::CardGame.Core.EngineOwners.CoreNativeFireAttackRuntime.GetFireAttackTargetSeat(fireAttack) : fireAttack.SourceSeat))
            {
                throw new InvalidOperationException("A FireAttack selection must retain a prompt for the current picker.");
            }

            var expectedFireStatus = _coreMatchParticipantsPlayersAccess._players[fireDecision.PlayerSeat].IsHuman ? EngineStatus.AwaitingHumanCardSelection : EngineStatus.Running;
            if (_coreMatchOutcomeStatusAccess._status != expectedFireStatus)
            {
                throw new InvalidOperationException("A FireAttack selection status does not match its current picker.");
            }
        }

        if (_coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow is { } nullification)
        {
            var responseWindow = _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().LastOrDefault(frame => global::CardGame.Core.EngineOwners.CoreGameEngineResponseUseCompletionRuntime.IsNullificationResponseProgramWindow(frame) && frame.ParentFrameId == nullification.Id);
            var nullificationCardUse = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(frame => frame.Id == nullification.ParentFrameId);
            var completedResponseDying = _coreResponseUseCompletionOperations.TryGetCompletedNullificationDyingCosts(responseWindow, out var rescueCosts) || _coreResponseEntityExchangeOperations.TryGetResponseExchangeMovementDyingCosts(responseWindow, out rescueCosts);
            var counterspellPaymentRide = _coreUnrespondableCounterspellsOperations.TryGetPolicyCounterspellPaymentRide(nullification, out var paymentRescueCosts);
            var shanheClaimRide = _coreShanheNullifiedClaimObserversOperations.TryGetShanheNullifiedClaimRide(nullification, out var claimChildCosts);
            var slotNamedPaymentRide = _characterWoLongFengChuOlOperations.HasWoLongFengChuUseObserver(nullification.ParentFrameId);
            completedResponseDying |= counterspellPaymentRide && _coreDyingCursorRuntimeOperations.ActiveDying is not null;
            completedResponseDying |= shanheClaimRide && _coreDyingCursorRuntimeOperations.ActiveDying is not null;
            completedResponseDying |= slotNamedPaymentRide && _coreDyingCursorRuntimeOperations.ActiveDying is not null;
            if (counterspellPaymentRide)
                rescueCosts = paymentRescueCosts;
            if (shanheClaimRide)
                rescueCosts = rescueCosts.Concat(claimChildCosts).Distinct().ToArray();
            var expectedPhysicalCardIds = nullification.DelayedReturn is not null || _characterZhugeLiangMouOperations.IsZeroEntityUse(nullification.ParentFrameId) || _coreNativeSlashTargetDuelOperations.IsNativeSlashTargetDuelZeroUse(nullification.ParentFrameId) || _coreVirtualOrdinaryTrickUsesOperations.IsProgramVirtualOrdinaryTrickUse(nullification.ParentFrameId) ? Array.Empty<int>() : _coreDamageTargetObtainProgramsOperations.IsDamageTargetDuelUse(nullification.ParentFrameId) ? _coreDamageTargetObtainProgramsOperations.DamageTargetDuelOuterProcessing(nullification.ParentFrameId) : (_coreDamageColorDuelProgramsOperations.IsSelectedActorDuelUse(nullification.ParentFrameId) || _coreDualColorDuelProgramsOperations.IsDualColorDuelUse(nullification.ParentFrameId) || _coreConditionalDiscardDuelReturnsOperations.IsConditionalDiscardDuelUse(nullification.ParentFrameId) || _coreFriendEventsOperations.IsFriendRecastDuelUse(nullification.ParentFrameId) || _characterHaoPuOlOperations.IsZhenyingDuelUse(nullification.ParentFrameId) || _characterHeJinOlOperations.IsHeJinMouzhuDuelUse(nullification.ParentFrameId) || _characterWuAnGuoOlOperations.IsWuAnGuoLiyongDuelUse(nullification.ParentFrameId) || _characterFanJiangZhangDaOlOperations.IsJueshengDuelUse(nullification.ParentFrameId)) ? Array.Empty<int>() : nullificationCardUse?.PhysicalCardIds is { Count: > 0 } physicalCardIds ? physicalCardIds : [nullification.EffectCardId];
            var lixianParentProcessing = _characterDengAiAnniversaryGodOperations.ShenDaCuixinAncestorProcessing(nullification.ParentFrameId) ?? _characterHuangYueyingOlMouOperations.MouLixianNativeAttackAncestorProcessing(nullification.ParentFrameId) ?? (_characterZhouYuMouOperations.MouZyNativeAncestorProcessing(nullification.ParentFrameId) ?? _characterXiaoQiaoMouOlOperations.MouXqNativeAncestorProcessing(nullification.ParentFrameId) ?? _characterDiaoChanLeOperations.DiouNativeAncestorProcessing(nullification.ParentFrameId)) ?? _characterLiuChengMouMobileOperations.MouLiuChengNativeAncestorProcessing(nullification.ParentFrameId) ?? _characterChengYuMouOlOperations.MouDanchiAncestorProcessing(nullification.ParentFrameId) ?? _characterLvYiMobileShiOperations.ShiLySongyanAncestorProcessing(nullification.ParentFrameId);
            expectedPhysicalCardIds = expectedPhysicalCardIds.Where(id => !_coreCurrentUsePhysicalClaimsOperations.IsClaimedUseCardEntity(nullification.ParentFrameId, id) && (!_coreHellSoulCardsOperations.IsHellDestroyedSoulMaterial(nullification.ParentFrameId, id) && !(_characterHuBanOlOperations.IsHuiyunOutsideGeneralWeaponMaterial(nullification.ParentFrameId, id) || (_characterMiHengLeOperations.IsSiruiOutsideGeneralWeaponMaterial(nullification.ParentFrameId, id) || _characterHuangYueyingOlMouOperations.IsMouLixianRemovedMaterial(nullification.ParentFrameId, id))) && !(_coreGuanduEquipmentOperations.IsGuanduDestroyedChariotMaterial(nullification.ParentFrameId, id) || _coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial(nullification.ParentFrameId, id)))).ToArray();
            var groupReveal = nullification.GroupTargetReturn is not null && _coreNativeCardContinuationCursorsOperations.ActiveGroupCard is { } nullificationGroup ? nullificationGroup.RevealedCardIds : [];
            if (!processing.Select(card => card.Id).Order().SequenceEqual(expectedPhysicalCardIds.Concat(groupReveal).Concat(rescueCosts).Concat(lixianParentProcessing ?? []).Order()))
            {
                throw new InvalidOperationException("The active Nullification window and Processing zone are inconsistent.");
            }

            if (lixianParentProcessing is null && (_coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not null || _coreNativeCardContinuationCursorsOperations.ActiveDuel is not null || _coreNativeCardContinuationCursorsOperations.ActiveGroupCard is not null && !_coreGroupTargetNullificationOperations.IsGroupTargetNullification(nullification) || _coreNativeCardContinuationCursorsOperations.ActiveFireAttack is not null) || _coreDyingCursorRuntimeOperations.ActiveDying is not null && !completedResponseDying || _coreDamageCursorRuntimeOperations.ActiveDamageTrigger is not null && !slotNamedPaymentRide && !shanheClaimRide && lixianParentProcessing is null || _coreNativeCardContinuationCursorsOperations.ActiveTargetCardSelection is not null)
            {
                throw new InvalidOperationException("A Nullification window cannot coexist with another card continuation.");
            }

            if (!_coreDelayedNullificationOperations.HasNullificationEffectOwner(nullification) || nullification.CandidateIndex < 0 || nullification.CandidateIndex > nullification.CandidateSeats.Count || nullification.ChainDepth < 0 || nullification.Step != ResolutionFrameStep.AwaitingResponse)
            {
                throw new InvalidOperationException("A Nullification window must retain its response cursor below any response trigger.");
            }

            if (responseWindow is null && !counterspellPaymentRide && !shanheClaimRide && !slotNamedPaymentRide && !_characterSimaYiMobileGodOperations.HasMobileSimaRenjieNativeSubtree(nullification.ParentFrameId))
            {
                var currentNullificationCards = nullification.CandidateIndex < nullification.CandidateSeats.Count ? _coreNativeNullificationOperations.GetNullificationCards(_coreMatchParticipantsPlayersAccess._players[nullification.CandidateSeats[nullification.CandidateIndex]]).Select(card => card.Id).ToArray() : Array.Empty<int>();
                if (coreDecision is not { Kind: DecisionKind.Nullification } nullificationDecision || nullification.CandidateIndex >= nullification.CandidateSeats.Count || nullificationDecision.PlayerSeat != nullification.CandidateSeats[nullification.CandidateIndex] || nullificationDecision.SourceSeat != nullification.SourceSeat || nullificationDecision.IncomingCard != nullification.EffectCardKind || nullificationDecision.RequiredCardKind != CardKind.Nullification || !nullificationDecision.ValidCardIds.SequenceEqual(currentNullificationCards))
                {
                    throw new InvalidOperationException("A Nullification window must retain a prompt for its current responder.");
                }

                var expectedNullificationStatus = _coreMatchParticipantsPlayersAccess._players[nullificationDecision.PlayerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
                if (_coreMatchOutcomeStatusAccess._status != expectedNullificationStatus)
                {
                    throw new InvalidOperationException("A Nullification prompt status does not match its current responder.");
                }
            }
        }

        if (_coreNativeCardContinuationCursorsOperations.ActiveTargetCardSelection is { } targetCardSelection && !_coreExtraEffectForeignSelectionOperations.AssertExtraForeignSelection(targetCardSelection, coreDecision, processing))
        {
            var target = _coreMatchParticipantsPlayersAccess._players[targetCardSelection.TargetSeat];
            var expectedSlots = Enumerable.Range(0, targetCardSelection.CandidateSlots.Count).ToArray();
            var lixianParentProcessing = _characterDengAiAnniversaryGodOperations.ShenDaCuixinAncestorProcessing(targetCardSelection.ParentFrameId) ?? _characterHuangYueyingOlMouOperations.MouLixianNativeAttackAncestorProcessing(targetCardSelection.ParentFrameId) ?? (_characterZhouYuMouOperations.MouZyNativeAncestorProcessing(targetCardSelection.ParentFrameId) ?? _characterXiaoQiaoMouOlOperations.MouXqNativeAncestorProcessing(targetCardSelection.ParentFrameId) ?? _characterDiaoChanLeOperations.DiouNativeAncestorProcessing(targetCardSelection.ParentFrameId)) ?? _characterLiuChengMouMobileOperations.MouLiuChengNativeAncestorProcessing(targetCardSelection.ParentFrameId) ?? _characterChengYuMouOlOperations.MouDanchiAncestorProcessing(targetCardSelection.ParentFrameId);
            var expectedSelectionProcessing = _characterZhugeLiangMouOperations.IsZeroEntityProcessingParent(targetCardSelection.ParentFrameId, targetCardSelection.EffectCardId) || targetCardSelection.EffectCardId == 0 && _coreVirtualOrdinaryTrickUsesOperations.IsProgramVirtualOrdinaryTrickUse(targetCardSelection.ParentFrameId) || _coreCurrentUsePhysicalClaimsOperations.IsClaimedUseCardEntity(targetCardSelection.ParentFrameId, targetCardSelection.EffectCardId) || _characterHuangYueyingOlMouOperations.IsMouLixianRemovedMaterial(targetCardSelection.ParentFrameId, targetCardSelection.EffectCardId) || _coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial(targetCardSelection.ParentFrameId, targetCardSelection.EffectCardId) ? Array.Empty<int>() : new[]
            {
                targetCardSelection.EffectCardId
            };
            if (!processing.Select(c => c.Id).Order().SequenceEqual(expectedSelectionProcessing.Concat(lixianParentProcessing ?? []).Order()))
            {
                throw new InvalidOperationException("The active hidden target-card selection and Processing zone are inconsistent.");
            }

            if (lixianParentProcessing is null && (_coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not null || _coreNativeCardContinuationCursorsOperations.ActiveDuel is not null || _coreNativeCardContinuationCursorsOperations.ActiveGroupCard is not null || _coreNativeCardContinuationCursorsOperations.ActiveFireAttack is not null || _coreDamageCursorRuntimeOperations.ActiveDamageTrigger is not null) || _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow is not null || _coreDyingCursorRuntimeOperations.ActiveDying is not null || _coreJudgmentCursorRuntimeOperations.ActiveJudgment is not null)
            {
                throw new InvalidOperationException("A hidden target-card selection cannot coexist with another continuation.");
            }

            if (!ReferenceEquals(coreTop, targetCardSelection) || targetCardSelection.Step != ResolutionFrameStep.AwaitingResponse || _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault(use => use.Id == targetCardSelection.ParentFrameId)is null || !targetCardSelection.CandidateSlots.SequenceEqual(expectedSlots))
            {
                throw new InvalidOperationException("A hidden target-card selection must retain only its opaque slot cursor as the stack top.");
            }

            var expectedDecision = coreDecision;
            if (expectedDecision is not { Kind: DecisionKind.SelectTargetCard } decision || decision.PlayerSeat != targetCardSelection.SourceSeat || decision.SourceSeat != targetCardSelection.SourceSeat || decision.TargetSeat != targetCardSelection.TargetSeat || !decision.ValidCardIds.SequenceEqual([]) || !decision.ValidTargetSeats.SequenceEqual([targetCardSelection.TargetSeat]) || decision.Choices.Count != targetCardSelection.CandidateSlots.Count || decision.Choices.Any(choice => choice.Cards.Count != 0 || !choice.Targets.SequenceEqual([targetCardSelection.TargetSeat]) || choice.Parameters.GetValueOrDefault("action") != "target-card-slot" || !choice.Parameters.TryGetValue("slot-index", out var slotText) || !int.TryParse(slotText, System.Globalization.NumberStyles.None, System.Globalization.CultureInfo.InvariantCulture, out var slot) || slot < 0 || slot >= targetCardSelection.CandidateSlots.Count))
            {
                throw new InvalidOperationException("A hidden target-card selection must retain a private opaque-slot prompt.");
            }

            if (targetCardSelection.CandidateSlots.Count != _corePhysicalOwnedCardQueriesOperations.GetHand(target).Count || decision.Choices.Select(choice => int.Parse(choice.Parameters["slot-index"], System.Globalization.CultureInfo.InvariantCulture)).OrderBy(slot => slot).SequenceEqual(expectedSlots)is false)
            {
                throw new InvalidOperationException("A hidden target-card selection cursor no longer matches the target hand count.");
            }

            var expectedSelectionStatus = _coreMatchParticipantsPlayersAccess._players[decision.PlayerSeat].IsHuman ? EngineStatus.AwaitingHumanCardSelection : EngineStatus.Running;
            if (_coreMatchOutcomeStatusAccess._status != expectedSelectionStatus)
            {
                throw new InvalidOperationException("A hidden target-card prompt status does not match its source.");
            }
        }

        if (_coreNativeCardContinuationCursorsOperations.ActiveStoneAxe is { } stoneAxe)
        {
            var stoneAxeAttack = stoneAxe.Attack;
            var decision = coreDecision;
            var source = _coreMatchParticipantsPlayersAccess._players[stoneAxeAttack.SourceSeat];
            var target = _coreMatchParticipantsPlayersAccess._players[stoneAxeAttack.TargetSeat];
            var currentCandidates = _coreNativeStoneAxeOperations.GetStoneAxeDiscardCards(source).Select(card => card.Id).ToArray();
            var expectedChoiceCount = currentCandidates.Length * (currentCandidates.Length - 1) / 2 + 1;
            if (!_coreNativeDamageHandlesOperations.UsesFormalStoneAxe || !global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, stoneAxeAttack) || !source.IsAlive || !target.IsAlive || !_coreAdvancedWeaponsOperations.HasWeaponAbility(source, CardKind.StoneAxe) || stoneAxeAttack.SuccessfulDodgeResponses != stoneAxeAttack.RequiredDodgeResponses || !stoneAxe.CandidateCardIds.SequenceEqual(currentCandidates) || coreTop is not CardUseFrame cardUse || cardUse.Id != stoneAxeAttack.ResolutionId || cardUse.SourceSeat != stoneAxeAttack.SourceSeat || global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(cardUse) != stoneAxeAttack.EffectiveCardKind || cardUse.Step != ResolutionFrameStep.AwaitingResponse || decision is not { Kind: DecisionKind.StoneAxe, IsPrivate: true } || decision.PlayerSeat != stoneAxeAttack.SourceSeat || decision.SourceSeat != stoneAxeAttack.SourceSeat || decision.TargetSeat != stoneAxeAttack.TargetSeat || decision.IncomingCard != stoneAxeAttack.EffectiveCardKind || !decision.ValidCardIds.SequenceEqual(currentCandidates) || decision.Choices.Count != expectedChoiceCount || decision.Choices.Count(choice => choice.Parameters.GetValueOrDefault("action") == "stone-axe-skip" && choice.Cards.Count == 0 && choice.Targets.Count == 0) != 1 || decision.Choices.Any(choice => choice.Parameters.GetValueOrDefault("action") == "stone-axe-use" && (choice.Cards.Count != 2 || choice.Cards.Distinct().Count() != 2 || choice.Cards.Any(id => !currentCandidates.Contains(id)) || choice.Targets.Count != 0)))
            {
                throw new InvalidOperationException("A Stone Axe choice must retain its private two-card cost prompt and completed Dodge continuation.");
            }

            var expectedStoneAxeStatus = source.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
            if (_coreMatchOutcomeStatusAccess._status != expectedStoneAxeStatus)
            {
                throw new InvalidOperationException("A Stone Axe prompt status does not match its owner.");
            }
        }

        if (_coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade is { } qinglong && _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest?.IsQinglongCrescentBladeUse != true)
        {
            var qinglongAttack = qinglong.Attack;
            var decision = coreDecision;
            var source = _coreMatchParticipantsPlayersAccess._players[qinglongAttack.SourceSeat];
            var target = _coreMatchParticipantsPlayersAccess._players[qinglongAttack.TargetSeat];
            var paidQinglongPaused = _coreDrawFundedDistinctBasicObserversOperations.DrawFundedDistinctBasicObserverRoot(qinglongAttack.ResolutionId)is { } paidQinglong && paidQinglong.Payment is { Intent: DrawFundedDistinctBasicIntent.Qinglong, Cursor: 0 } qinglongPayment && _coreNativeDamageHandlesOperations.UsesFormalQinglongCrescentBlade && global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, qinglongAttack) && qinglongAttack.SuccessfulDodgeResponses == qinglongAttack.RequiredDodgeResponses && _coreCardUseLifecycleOperations.LifecycleCardUse(qinglongAttack.ResolutionId)is { Step: ResolutionFrameStep.AwaitingResponse } originalQinglong && originalQinglong.SourceSeat == source.Seat && originalQinglong.CardKind == qinglongAttack.EffectiveCardKind && originalQinglong.Action is { } originalQinglongAction && qinglongPayment.Source.OwnerSeat == source.Seat && qinglongPayment.TargetSeat == target.Seat && qinglongPayment.ParentFrameId == originalQinglong.Id && qinglongPayment.RequestFrameId == originalQinglong.Id && qinglongPayment.ParentActionId == originalQinglongAction.ActionId && paidQinglong.TargetSeats.SequenceEqual([target.Seat]) && paidQinglong.OriginalDecision is { Kind: DecisionKind.QinglongCrescentBlade, IsPrivate: true } originalQinglongDecision && originalQinglongDecision.PlayerSeat == source.Seat && originalQinglongDecision.SourceSeat == source.Seat && originalQinglongDecision.TargetSeat == target.Seat && originalQinglongDecision.IncomingCard == qinglongAttack.EffectiveCardKind && originalQinglongDecision.ValidTargetSeats.SequenceEqual([target.Seat]) && originalQinglongDecision.PromptId == qinglongPayment.OriginalPromptId && originalQinglongDecision.Revision == qinglongPayment.OriginalRevision;
            paidQinglongPaused |= _characterWoLongFengChuOlOperations.HasWoLongFengChuQinglongPaymentRide(qinglongAttack);
            paidQinglongPaused |= _characterGuoTuOlOperations.HasWeijieQinglongPaymentRide(qinglongAttack);
            paidQinglongPaused |= _characterLiuFengBoundaryOperations.HasBoundaryLiuFengQinglongPaymentRide(qinglongAttack);
            paidQinglongPaused |= _characterLiuZhangOlOperations.HasZonghuQinglongPaymentRide(qinglongAttack);
            paidQinglongPaused |= _characterHuanShuJunOlOperations.HasHuanpeiQinglongPaymentRide(qinglongAttack);
            paidQinglongPaused |= _characterXiaoQiaoMouOlOperations.HasMouXqJueyanResponseContinuation(qinglongAttack.ResolutionId);
            if (!paidQinglongPaused)
            {
                var currentSlashCards = _coreNativeQinglongOperations.GetQinglongCrescentBladeSlashCards(source, target);
                var currentCandidates = currentSlashCards.Select(card => card.Id).ToArray();
                var physicalSlashChoices = _coreNativeQinglongOperations.QinglongPhysicalSlashChoices(qinglong, currentSlashCards);
                var canRequestFactionSlash = _coreNativeQinglongOperations.CanRequestQinglongCrescentBladeFactionSlash(qinglong);
                if (!_coreNativeDamageHandlesOperations.UsesFormalQinglongCrescentBlade || !global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, qinglongAttack) || qinglongAttack.SuccessfulDodgeResponses != qinglongAttack.RequiredDodgeResponses || coreTop is not CardUseFrame cardUse || cardUse.Id != qinglongAttack.ResolutionId || cardUse.SourceSeat != qinglongAttack.SourceSeat || global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(cardUse) != qinglongAttack.EffectiveCardKind || cardUse.Step != ResolutionFrameStep.AwaitingResponse || decision is not { Kind: DecisionKind.QinglongCrescentBlade, IsPrivate: true } || decision.PlayerSeat != qinglongAttack.SourceSeat || decision.SourceSeat != qinglongAttack.SourceSeat || decision.TargetSeat != qinglongAttack.TargetSeat || decision.IncomingCard != qinglongAttack.EffectiveCardKind || !decision.ValidCardIds.SequenceEqual(currentCandidates) || !decision.ValidTargetSeats.SequenceEqual([target.Seat]) || decision.Choices.Count != physicalSlashChoices.Count + 1 + (canRequestFactionSlash ? 1 : 0) + _coreTieredRoundZeroUsesOperations.TieredRoundZeroForcedSlashChoices(source, target, qinglongAttack.ResolutionId, true).Count + _characterXiaoQiaoMouOlOperations.MouXqForcedSlashChoices(source, target, qinglongAttack.ResolutionId, true).Count + _coreRoundOnceCardlessNativeUsesOperations.RoundOnceCardlessForcedSlashChoices(source, target, qinglongAttack.ResolutionId, true).Count + _characterMaXiuMaTieOlOperations.KenshangQinglongSlashChoices(qinglong).Count + _characterChengJiChengCuiOlOperations.TousuiQinglongSlashChoices(qinglong).Count || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(decision.Choices.Where(c => c.Parameters.GetValueOrDefault("action") == "kenshang-request-open").ToArray(), _characterMaXiuMaTieOlOperations.KenshangQinglongSlashChoices(qinglong)) || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(decision.Choices.Where(c => c.Parameters.GetValueOrDefault("action") == "tousui-request-open").ToArray(), _characterChengJiChengCuiOlOperations.TousuiQinglongSlashChoices(qinglong)) || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(decision.Choices.Where(c => c.Parameters.GetValueOrDefault("action") == "tiered-round-zero-forced-slash").ToArray(), _coreTieredRoundZeroUsesOperations.TieredRoundZeroForcedSlashChoices(source, target, qinglongAttack.ResolutionId, true)) || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(decision.Choices.Where(c => c.Parameters.GetValueOrDefault("action") == "round-once-cardless-forced-slash").ToArray(), _coreRoundOnceCardlessNativeUsesOperations.RoundOnceCardlessForcedSlashChoices(source, target, qinglongAttack.ResolutionId, true)) || decision.Choices.Count(choice => choice.Parameters.GetValueOrDefault("action") == "qinglong-skip" && choice.Cards.Count == 0 && choice.Targets.Count == 0) != 1 || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(decision.Choices.Where(choice => choice.Parameters.GetValueOrDefault("action") == "qinglong-slash").ToArray(), physicalSlashChoices) || decision.Choices.Where(choice => choice.Parameters.GetValueOrDefault("action") == "qinglong-slash").Any(choice => choice.Cards.Count != 1 || !choice.Targets.SequenceEqual([target.Seat])) || decision.Choices.Count(choice => choice.Parameters.GetValueOrDefault("action") == "qinglong-jijiang" && choice.Cards.Count == 0 && choice.Targets.SequenceEqual([target.Seat])) != (canRequestFactionSlash ? 1 : 0))
                {
                    throw new InvalidOperationException("A Qinglong Crescent Blade choice must retain its private exact-Slash prompt and completed Dodge continuation.");
                }

                var expectedQinglongStatus = source.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
                if (_coreMatchOutcomeStatusAccess._status != expectedQinglongStatus)
                {
                    throw new InvalidOperationException("A Qinglong Crescent Blade prompt status does not match its owner.");
                }
            }
        }

        if (_coreNativeCardContinuationCursorsOperations.ActiveIceSword is { } iceSword)
        {
            _coreNativeIceSwordOperations.AssertIceSwordLogicalDiscard(iceSword);
            var iceSwordAttack = iceSword.Attack;
            var source = _coreMatchParticipantsPlayersAccess._players[iceSwordAttack.SourceSeat];
            var target = _coreMatchParticipantsPlayersAccess._players[iceSwordAttack.TargetSeat];
            var targetHand = _corePhysicalOwnedCardQueriesOperations.GetHand(target);
            var legalHandSlots = Enumerable.Range(0, targetHand.Count).Where(slot => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(iceSwordAttack.SourceSeat, targetHand[slot], CardLocation.Hand(target.Seat), OwnedCardMoveIntent.Discard)).ToArray();
            var equipmentIds = _corePhysicalOwnedCardQueriesOperations.GetEquipment(target).Where(card => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(iceSwordAttack.SourceSeat, card, CardLocation.Equipment(target.Seat), OwnedCardMoveIntent.Discard)).Select(card => card.Id).ToArray();
            var decision = coreDecision;
            var discardChoices = decision?.Choices.Where(choice => choice.Parameters.GetValueOrDefault("action") == "ice-sword-discard").ToArray() ?? [];
            var expectedSkipCount = iceSword.Activated ? 0 : 1;
            var expectedHandCount = iceSword.Activated ? legalHandSlots.Length : 0;
            var expectedEquipmentIds = iceSword.Activated ? equipmentIds : [];
            var handSlots = discardChoices.Where(choice => choice.Parameters.GetValueOrDefault("target-zone") == "hand").Select(choice => choice.Parameters.GetValueOrDefault("slot-index")).ToArray();
            if (!_coreNativeDamageHandlesOperations.UsesFormalIceSword || !global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, iceSwordAttack) || !iceSwordAttack.IceSwordAttempted || !iceSwordAttack.DamageAmountFinalized || iceSwordAttack.IsChainPropagation || !_coreAdvancedWeaponsOperations.HasWeaponAbility(source, CardKind.IceSword) || coreTop is not CardUseFrame cardUse || cardUse.Id != iceSwordAttack.ResolutionId || cardUse.Step != ResolutionFrameStep.AwaitingResponse || decision is not { Kind: DecisionKind.IceSword, IsPrivate: true } || decision.PlayerSeat != iceSwordAttack.SourceSeat || decision.SourceSeat != iceSwordAttack.SourceSeat || decision.TargetSeat != iceSwordAttack.TargetSeat || !decision.ValidCardIds.SequenceEqual(expectedEquipmentIds) || !decision.ValidTargetSeats.SequenceEqual(iceSword.Activated ? [target.Seat] : []) || discardChoices.Length != expectedHandCount + expectedEquipmentIds.Length || decision.Choices.Count != discardChoices.Length + expectedSkipCount * 2 || handSlots.Length != expectedHandCount || !handSlots.SequenceEqual((iceSword.Activated ? legalHandSlots : []).Select(slot => slot.ToString(System.Globalization.CultureInfo.InvariantCulture))) || discardChoices.Any(choice => !choice.Targets.SequenceEqual([target.Seat])) || decision.Choices.Count(choice => choice.Parameters.GetValueOrDefault("action") == "ice-sword-damage" && choice.Cards.Count == 0 && choice.Targets.Count == 0) != expectedSkipCount || decision.Choices.Count(choice => choice.Parameters.GetValueOrDefault("action") == "ice-sword-activate" && choice.Cards.Count == 0 && choice.Targets.Count == 0) != expectedSkipCount || (iceSword.Activated ? iceSword.DiscardedCardIds.Count > 1 : iceSword.DiscardedCardIds.Count != 0))
            {
                throw new InvalidOperationException("An Ice Sword choice must retain its private staged target-card prompt before Slash damage.");
            }

            var expectedIceSwordStatus = source.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
            if (_coreMatchOutcomeStatusAccess._status != expectedIceSwordStatus)
            {
                throw new InvalidOperationException("An Ice Sword prompt status does not match its owner.");
            }
        }

        if (_coreNativeCardContinuationCursorsOperations.ActiveQilinBow is { } qilinBow)
        {
            var qilinAttack = qilinBow.Attack;
            var source = _coreMatchParticipantsPlayersAccess._players[qilinAttack.SourceSeat];
            var target = _coreMatchParticipantsPlayersAccess._players[qilinAttack.TargetSeat];
            var mountIds = _coreNativeQilinBowOperations.GetQilinBowTargetMounts(target).Select(card => card.Id).ToArray();
            var decision = coreDecision;
            var discardChoices = decision?.Choices.Where(choice => choice.Parameters.GetValueOrDefault("action") == "qilin-bow-discard").ToArray() ?? [];
            if (!_coreNativeDamageHandlesOperations.UsesFormalQilinBow || !global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, qilinAttack) || !qilinAttack.QilinBowAttempted || !qilinAttack.DamageAmountFinalized || qilinAttack.IsChainPropagation || !_coreAdvancedWeaponsOperations.HasWeaponAbility(source, CardKind.QilinBow) || mountIds.Length == 0 || coreTop is not CardUseFrame cardUse || cardUse.Id != qilinAttack.ResolutionId || cardUse.Step != ResolutionFrameStep.AwaitingResponse || decision is not { Kind: DecisionKind.QilinBow, IsPrivate: true } || decision.PlayerSeat != qilinAttack.SourceSeat || decision.SourceSeat != qilinAttack.SourceSeat || decision.TargetSeat != qilinAttack.TargetSeat || !decision.ValidCardIds.SequenceEqual(mountIds) || !decision.ValidTargetSeats.SequenceEqual([target.Seat]) || !discardChoices.SelectMany(choice => choice.Cards).SequenceEqual(mountIds) || discardChoices.Any(choice => choice.Cards.Count != 1 || !choice.Targets.SequenceEqual([target.Seat])) || decision.Choices.Count != mountIds.Length + 1 || decision.Choices.Count(choice => choice.Parameters.GetValueOrDefault("action") == "qilin-bow-skip" && choice.Cards.Count == 0 && choice.Targets.Count == 0) != 1)
            {
                throw new InvalidOperationException("A Qilin Bow choice must retain its private exact-mount prompt before Slash damage.");
            }

            var expectedQilinStatus = source.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
            if (_coreMatchOutcomeStatusAccess._status != expectedQilinStatus)
            {
                throw new InvalidOperationException("A Qilin Bow prompt status does not match its owner.");
            }
        }

        if (_coreNativeCardContinuationCursorsOperations.ActiveCixiongDoubleSwords is { } cixiong && !_characterCaoPiAnniversaryGodOperations.HasKemingCixiongContinuation(cixiong))
        {
            var cixiongAttack = cixiong.Attack;
            var decision = coreDecision;
            var source = _coreMatchParticipantsPlayersAccess._players[cixiongAttack.SourceSeat];
            var target = _coreMatchParticipantsPlayersAccess._players[cixiongAttack.TargetSeat];
            var targetHandIds = _corePhysicalOwnedCardQueriesOperations.GetHand(target).OrderBy(card => card.Id).Select(card => card.Id).ToArray();
            var sourceActivationPromptMatches = cixiong.Stage == CixiongDoubleSwordsStage.SourceActivation && decision?.PlayerSeat == source.Seat && decision.ValidCardIds.Count == 0 && decision.ValidTargetSeats.Count == 0 && decision.Choices.Count == 2 && decision.Choices.All(choice => choice.Cards.Count == 0 && choice.Targets.Count == 0) && decision.Choices.Count(choice => choice.Parameters.GetValueOrDefault("action") == "cixiong-use") == 1 && decision.Choices.Count(choice => choice.Parameters.GetValueOrDefault("action") == "cixiong-skip") == 1;
            var targetChoicePromptMatches = cixiong.Stage == CixiongDoubleSwordsStage.TargetChoice && decision?.PlayerSeat == target.Seat && decision.ValidCardIds.SequenceEqual(targetHandIds) && decision.ValidTargetSeats.Count == 0 && decision.Choices.Count == targetHandIds.Length + 1 && decision.Choices.Count(choice => choice.Parameters.GetValueOrDefault("action") == "cixiong-draw" && choice.Cards.Count == 0 && choice.Targets.Count == 0) == 1 && decision.Choices.Where(choice => choice.Parameters.GetValueOrDefault("action") == "cixiong-discard").SelectMany(choice => choice.Cards).OrderBy(id => id).SequenceEqual(targetHandIds) && decision.Choices.Where(choice => choice.Parameters.GetValueOrDefault("action") == "cixiong-discard").All(choice => choice.Cards.Count == 1 && choice.Targets.Count == 0);
            if (!_coreNativeDamageHandlesOperations.UsesFormalCixiongDoubleSwords || !global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, cixiongAttack) || !source.IsAlive || !target.IsAlive || source.Gender == target.Gender || !_coreAdvancedWeaponsOperations.HasWeaponAbility(source, CardKind.CixiongDoubleSwords) || cixiongAttack.CixiongDoubleSwordsResolved || coreTop is not CardUseFrame cardUse || cardUse.Id != cixiongAttack.ResolutionId || cardUse.SourceSeat != cixiongAttack.SourceSeat || global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(cardUse) != cixiongAttack.EffectiveCardKind || cardUse.Step != ResolutionFrameStep.Declared || cardUse.TargetIndex < 0 || cardUse.TargetIndex >= cardUse.TargetSeats.Count || cardUse.TargetSeats[cardUse.TargetIndex] != cixiongAttack.TargetSeat || decision is not { Kind: DecisionKind.CixiongDoubleSwords, IsPrivate: true } || decision.SourceSeat != cixiongAttack.SourceSeat || decision.TargetSeat != cixiongAttack.TargetSeat || decision.IncomingCard != cixiongAttack.EffectiveCardKind || !(sourceActivationPromptMatches || targetChoicePromptMatches))
            {
                throw new InvalidOperationException("A Cixiong Double Swords choice must retain its private staged prompt and exact Slash continuation.");
            }

            var responder = cixiong.Stage == CixiongDoubleSwordsStage.SourceActivation ? source : target;
            var expectedCixiongStatus = responder.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
            if (_coreMatchOutcomeStatusAccess._status != expectedCixiongStatus)
            {
                throw new InvalidOperationException("A Cixiong Double Swords prompt status does not match its responder.");
            }
        }

        if (coreDecision is { Kind: DecisionKind.SkipDiscardPolicy } kejiDecision && !_characterLuMengMobileOperations.AssertPreEntryDiscardPolicyChoice(kejiDecision))
        {
            var validPolicy = kejiDecision.Choices.Count == 2 && kejiDecision.Choices[0].Parameters.TryGetValue("skill-id", out var skillId) && kejiDecision.Choices[0].Parameters.TryGetValue("policy-id", out var policyId) && kejiDecision.Choices.All(choice => choice.Parameters.GetValueOrDefault("skill-id") == skillId && choice.Parameters.GetValueOrDefault("policy-id") == policyId) && _coreProgramCardPoliciesOperations.CardPolicies(_coreMatchParticipantsPlayersAccess._players[kejiDecision.PlayerSeat], SkillProgramCardPolicyKind.OfferSkipDiscard).Any(item => item.Policy.BeforePhaseEntry != true && item.Source.SkillId == skillId && item.Policy.Id == policyId);
            if (_coreActualTurnProgressCurrentSeatPhaseAccess._phase != TurnPhase.Discard || _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat != kejiDecision.PlayerSeat || !_coreMatchParticipantsPlayersAccess._players[kejiDecision.PlayerSeat].IsAlive || !validPolicy || _coreCurrentTurnStatisticsUsedOrPlayedSlashDuringPlayPhaseAccess._usedOrPlayedSlashDuringPlayPhase || kejiDecision.Choices.Count != 2 || kejiDecision.Choices.Any(choice => choice.Cards.Count != 0 || choice.Targets.Count != 0) || _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not null || _coreNativeCardContinuationCursorsOperations.ActiveDuel is not null || _coreNativeCardContinuationCursorsOperations.ActiveGroupCard is not null || _coreNativeCardContinuationCursorsOperations.ActiveFireAttack is not null || _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow is not null || _coreNativeCardContinuationCursorsOperations.ActiveTargetCardSelection is not null || _coreDyingCursorRuntimeOperations.ActiveDying is not null || _coreDamageCursorRuntimeOperations.ActiveDamageTrigger is not null || _coreJudgmentCursorRuntimeOperations.ActiveJudgment is not null || _resolutionStackOperations._resolutionStack.Count != 0)
            {
                throw new InvalidOperationException("A discard policy must retain its exact prompt at a clean discard-phase boundary.");
            }

            var expectedKejiStatus = _coreMatchParticipantsPlayersAccess._players[kejiDecision.PlayerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
            if (_coreMatchOutcomeStatusAccess._status != expectedKejiStatus)
            {
                throw new InvalidOperationException("A discard-policy prompt status does not match its owner.");
            }
        }

        if (!_coreMatchSetupSetupCompleteAccess._setupComplete && _resolutionStackOperations._resolutionStack.Count != 0)
        {
            throw new InvalidOperationException("Setup cannot retain an in-flight card resolution.");
        }

        if (!hasActiveCardResolution && _resolutionStackOperations._resolutionStack.Count != 0 && !_characterHuaTuoAnniversaryGodOperations.HasExactBoundarySkillInvocation() && !_characterLuMengMobileOperations.HasNativeDiscardEntryBoundary() && !_characterBeiMiHuOlOperations.HasBeiMiHuTurnPreludeBoundary() && !_characterQiaoZhouOlOperations.HasQiaoZhouPreparationBoundaryFrame() && !_coreForeignTurnContestsOperations.HasForeignActualTurnStartBoundary() && !(_coreActualPhaseEndsOperations.HasActualPhaseEndBoundaryFrame() || _coreRoundEndingOperations.HasRoundEndingBoundaryFrame()) && !_coreProgramLifecycleOperations.HasProgramLifecycleBoundaryFrame() && !_coreDrawPhaseObligationsOperations.HasDrawPhaseObligationBoundaryFrame() && !_coreDiscardOperations.HasManualDiscardBoundaryFrame() && !_coreProgramLifecycleOperations.HasTurnEndingBoundaryFrame() && !_coreDeferredHandAlignmentsOperations.HasDeferredTurnEndBoundaryFrame() && !_coreProgramLifecycleOperations.HasPlayPhaseStartingBoundaryFrame() && !_coreEquipmentRecastsOperations.HasEquipmentRecastNativeDrawBoundary() && !_coreCardMovementProgramsOperations.HasCardsMovedProgramBoundaryFrame() && _coreDrawFundedDistinctBasicObserversOperations.DrawFundedDistinctBasicObserverRoot()is null && !_coreWushuangOperations.HasWushuangIntermissionBoundary() && !_coreShanheMarketOperations.HasShanheMarketBoundary())
        {
            throw new InvalidOperationException($"A completed card resolution left frames on the stack: {(string.Join(", ", _resolutionStackOperations._resolutionStack.Select(frame => $"{(frame.Id)}/{(frame.Kind)}/{(frame.Step)}")))}.");
        }

        if (_coreActiveNativeAttackQueriesOperations.ActiveCardAttack is { } pendingAttack)
        {
            if (!pendingAttack.IsDelayedJudgmentDamage && !pendingAttack.IsProgramJudgmentDamage && !pendingAttack.IsProgramSkillDamage && !_resolutionStackOperations._resolutionStack.Any(frame => frame is CardUseFrame cardUse && cardUse.Id == pendingAttack.ResolutionId))
            {
                throw new InvalidOperationException("An active Slash continuation has no parent CardUse frame.");
            }

            if (pendingAttack.IsProgramSkillDamage && !_resolutionStackOperations._resolutionStack.Any(frame => frame is ProgramSkillFrame programSkill && programSkill.Id == pendingAttack.ProgramSkillFrameId))
            {
                throw new InvalidOperationException("An active-program damage continuation has no parent ProgramSkill frame.");
            }

            if (_coreProgramAttackAttemptOperations.IsCardAttackSuspendedByProgramDamage)
            {
            // The retained card owner is suspended beneath the program damage owner.
            }
            else if (_coreIdentityAppearanceInvariantsOperations.HasIdentityAppearanceAttack(pendingAttack))
            {
            // The exact appearance invoice owns this attack while its native children finish.
            }
            else if (_characterZhangLiaoMobileGodOperations.HasMobileZhitiDuelContinuation(pendingAttack))
            {
            // The real failed Duel response owns its typed slot-restoration child.
            }
            else if (_coreDyingSuitsAndHistoricalUseObserversOperations.HasDyingSuitsSuspendedAttack(pendingAttack.ResolutionId))
            {
                _coreDyingSuitsAndHistoricalUseObserversOperations.AssertDyingSuitsSubtree();
            }
            else if (_coreSlashTargetPenaltyObserversOperations.HasTargetPenaltyUseObserver(pendingAttack.ResolutionId))
            {
                _coreSlashTargetPenaltyObserversOperations.AssertTargetPenaltyWindows();
            }
            else if (_coreSlashTargetBenefitObserversOperations.HasSlashTargetBenefitUseObserver(pendingAttack.ResolutionId))
            {
                _coreSlashTargetBenefitObserversOperations.AssertSlashTargetBenefitWindows();
            }
            else if (_coreFirstSlashDuelLegacyWindowsOperations.HasFirstSlashDuelLegacyUseObserver(pendingAttack.ResolutionId))
            {
                _coreFirstSlashDuelLegacyWindowsOperations.AssertFirstSlashDuelLegacyWindows();
            }
            else if (_characterWoLongFengChuOlOperations.HasWoLongFengChuUseObserver(pendingAttack.ResolutionId))
            {
            // The exact slot-payment frame owns this original native response.
            }
            else if (_corePaidOwnTargetObserversOperations.HasActualUseTargetObserver(pendingAttack.ResolutionId))
            {
                _corePaidOwnTargetObserversOperations.AssertActualUseTargetPrograms();
            }
            else if ((_coreChainedStateBasicReturnsOperations.HasChainedStateBasicObserver(pendingAttack.ResolutionId) || _coreDrawFundedDistinctBasicObserversOperations.HasDrawFundedDistinctBasicObserver(pendingAttack.ResolutionId)))
            {
                _coreChainedStateBasicReturnsOperations.AssertChainedStateBasicFramesAndUses();
                _coreDrawFundedDistinctBasicObserversOperations.AssertDrawFundedDistinctBasicFramesAndUses();
            }
            else if (_coreWenyiDamageHealthOperations.HasWenyiDamageHealthContinuation(pendingAttack.ResolutionId))
            {
                _coreWenyiDamageHealthOperations.AssertWenyiDamageHealth();
            }
            else if (_characterDaQiaoXiaoQiaoHappyGodOperations.HasHappyQiaoDamageHealthContinuation(pendingAttack.ResolutionId))
            {
                _characterDaQiaoXiaoQiaoHappyGodOperations.AssertHappyQiaoDamageHealth();
            }
            else if (_characterDaQiaoXiaoQiaoHappyGodOperations.HasHappyQiaoCardMaximumHealthContinuation(pendingAttack.ResolutionId))
            {
                _characterDaQiaoXiaoQiaoHappyGodOperations.AssertHappyQiaoCardMaximumHealth();
            }
            else if (_coreHealthQueriesOperations.HasHappyQiaoNativeDamageChoice(pendingAttack.ResolutionId))
            {
                _characterDaQiaoXiaoQiaoHappyGodOperations.AssertHappyQiaoHealthFrames();
            }
            else if (_characterHuanShuJunOlOperations.HasHuanpeiNativeRequestContinuation(pendingAttack.ResolutionId))
            {
                _characterHuanShuJunOlOperations.AssertHuanpeiRequests();
            }
            else if (_characterLiuZhangOlOperations.HasZonghuNativeRequestContinuation(pendingAttack.ResolutionId))
            {
                _characterLiuZhangOlOperations.AssertZonghuRequests();
            }
            else if (_characterGuoTuOlOperations.HasWeijieNativeRequestContinuation(pendingAttack.ResolutionId))
            {
                _characterGuoTuOlOperations.AssertWeijieRequests();
            }
            else if (_characterLiuFengBoundaryOperations.HasBoundaryLiuFengNativeRequestContinuation(pendingAttack.ResolutionId))
            {
                _characterLiuFengBoundaryOperations.AssertBoundaryLiuFengRequests();
            }
            else if (_coreFriendDuelResponsesOperations.HasFriendDuelResponseObserver(pendingAttack.ResolutionId))
            {
                _coreFriendDuelResponsesOperations.AssertFriendEventFrames();
            }
            else if (_characterLuXunMouMobileOperations.HasNativeGroupEffectAttackObserver(pendingAttack))
            {
                _coreCardEffectProgramsOperations.AssertCardEffectProgramState();
            }
            else if (_coreCardActionsOperations.ProgramCardAttack is not null)
            {
                // The generic window owns the pending attack until all effects finish.
                _coreCardActionsOperations.AssertProgramCardWindowState();
                _coreCardEffectProgramsOperations.AssertCardEffectProgramState();
            }
            else if (_coreFactionRequestCostRecoveriesOperations.HasPaidFactionRequestCostRecovery(_coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest))
            {
            // The exact paid request owns the response while its recovery and observer children finish.
            }
            else if (_coreRecoveryReplacementOperations.HasRecoveryPaidCardUseObserver(pendingAttack.ResolutionId))
            {
                if ((_coreDamageObserverCursorQueriesOperations.DamageCursorEffectiveTop(includeNestedObservers: true)is not CardUseFrame paidUse || paidUse.Id != pendingAttack.ResolutionId || paidUse.RecoveryPaidContinuation is null) && committedPaymentCursor.Owner?.Id != pendingAttack.ResolutionId && !_coreRecoveryReplacementDamageRidesOperations.IsRecoveryReplacementProgramDying(pendingAttack.ResolutionId))
                    throw new InvalidOperationException("An active paid attack recovery must retain its exact card-use continuation.");
            }
            else if (_coreNativeEquipmentInvocationsOperations.FindNativeEquipmentInvocation(pendingAttack.ResolutionId)is not null || _coreNativeSkillInvocationObserversOperations.HasNativeFactionSkillInvocation(pendingAttack.ResolutionId))
            {
            // The accepted equipment receipt owns a proven invocation subtree until its typed return.
            }
            else if (_coreNativeCardContinuationCursorsOperations.ActiveStoneAxe is { } stoneAxeContinuation)
            {
                if (!global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(stoneAxeContinuation.Attack, pendingAttack) || coreTop is not CardUseFrame stoneAxeCardUse || stoneAxeCardUse.Id != pendingAttack.ResolutionId || stoneAxeCardUse.Step != ResolutionFrameStep.AwaitingResponse)
                {
                    throw new InvalidOperationException("An active Stone Axe choice must retain its Slash frame as the stack top.");
                }
            }
            else if (_coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade is { } qinglongContinuation)
            {
                if (!global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(qinglongContinuation.Attack, pendingAttack) || coreTop is not CardUseFrame qinglongCardUse || qinglongCardUse.Id != pendingAttack.ResolutionId || qinglongCardUse.Step != ResolutionFrameStep.AwaitingResponse)
                {
                    throw new InvalidOperationException("An active Qinglong Crescent Blade choice must retain its Slash frame as the stack top.");
                }
            }
            else if (_coreNativeCardContinuationCursorsOperations.ActiveIceSword is { } iceSwordContinuation)
            {
                if (!global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(iceSwordContinuation.Attack, pendingAttack) || coreTop is not CardUseFrame iceSwordCardUse || iceSwordCardUse.Id != pendingAttack.ResolutionId || iceSwordCardUse.Step != ResolutionFrameStep.AwaitingResponse)
                {
                    throw new InvalidOperationException("An active Ice Sword choice must retain its Slash frame as the stack top.");
                }
            }
            else if (_coreNativeCardContinuationCursorsOperations.ActiveQilinBow is { } qilinContinuation)
            {
                if (!global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(qilinContinuation.Attack, pendingAttack) || coreTop is not CardUseFrame qilinCardUse || qilinCardUse.Id != pendingAttack.ResolutionId || qilinCardUse.Step != ResolutionFrameStep.AwaitingResponse)
                {
                    throw new InvalidOperationException("An active Qilin Bow choice must retain its Slash frame as the stack top.");
                }
            }
            else if (_coreNativeCardContinuationCursorsOperations.ActiveCixiongDoubleSwords is { } kemingCixiong && global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(kemingCixiong.Attack, pendingAttack) && _characterCaoPiAnniversaryGodOperations.HasKemingCixiongContinuation(kemingCixiong))
            {
            // The completed native draw owns an exact typed echo child.
            }
            else if (_coreNativeCardContinuationCursorsOperations.ActiveCixiongDoubleSwords is { } cixiongContinuation)
            {
                if (!global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(cixiongContinuation.Attack, pendingAttack) || coreTop is not CardUseFrame cixiongCardUse || cixiongCardUse.Id != pendingAttack.ResolutionId || cixiongCardUse.Step != ResolutionFrameStep.Declared)
                {
                    throw new InvalidOperationException("An active Cixiong Double Swords choice must retain its declared Slash frame as the stack top.");
                }
            }
            else if ((_coreDynamicDiscardDamageObserversOperations.HasDynamicDiscardDamageAttackObserver(pendingAttack) || _coreSignedDamagePaymentObserversOperations.HasSignedDamagePaymentAttackObserver(pendingAttack)))
            {
            // Only this new paid root proves the complete contiguous child pipeline.
            }
            else if (_characterSimaYiMobileGodOperations.HasMobileSimaRenjieNativeSubtree(pendingAttack.ResolutionId))
            {
            // The exact unanswered response retains its original native attack.
            }
            else if (pendingAttack.IsProgramJudgmentDamage)
            {
                if (pendingAttack.ProgramJudgmentFrameId is not { } programFrameId || _coreJudgmentCursorRuntimeOperations.ActiveJudgment is null || !_resolutionStackOperations._resolutionStack.OfType<ProgramJudgmentTriggerWindowFrame>().Any(frame => frame.Id == programFrameId && frame.ParentFrameId == _coreJudgmentCursorRuntimeOperations.ActiveJudgment.Id))
                {
                    throw new InvalidOperationException("Configured judgment damage must retain its judgment trigger parent.");
                }
            }
            else if (_coreJudgmentCursorRuntimeOperations.ActiveJudgment is { } judgmentContinuation)
            {
                var judgmentFrame = _resolutionStackOperations._resolutionStack.OfType<JudgmentFrame>().LastOrDefault();
                var judgmentIsActive = coreTop switch
                {
                    JudgmentFrame current => current.Id == judgmentContinuation.Id,
                    ProgramJudgmentTriggerWindowFrame program => program.ParentFrameId == judgmentContinuation.Id,
                    ProgramSkillFrame program => program.WindowContext?.Judgment?.JudgmentFrameId == judgmentContinuation.Id || program.WindowContext?.JudgmentReplacement?.JudgmentFrameId == judgmentContinuation.Id,
                    _ => false
                };
                judgmentIsActive |= _coreJudgmentOutcomeDamageEdgesOperations.HasExactJudgmentOutcomeSubtree(judgmentContinuation.Id);
                judgmentIsActive |= _coreYaodaoChitongOperations.HasExactYaodaoChitongJudgmentSubtree(judgmentContinuation.Id);
                if (!global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreNativeDamageHandlesOperations.GetJudgmentAttack(judgmentContinuation), pendingAttack) || judgmentFrame is null || !judgmentIsActive && !_coreOwnedDamagePointJudgmentsOperations.HasOwnedDamagePointJudgmentRide(judgmentContinuation.Id) && !_coreDamageJudgmentSuitPaymentObserversOperations.HasDamageJudgmentSuitPaymentRide(judgmentContinuation.Id) || judgmentFrame.Id != judgmentContinuation.Id || judgmentFrame.ParentFrameId != judgmentContinuation.ParentFrameId)
                {
                    throw new InvalidOperationException("An active judgment continuation must retain its Judgment frame and active child window.");
                }
            }
            else if (_characterLuMengMouOperations.ActiveYingboGift is { Continuation: YingboGiftContinuation.Attack, Attack: { } giftAttack })
            {
                if (!global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(giftAttack, pendingAttack) || coreTop is not CardUseFrame giftCardUse || giftCardUse.Id != pendingAttack.ResolutionId)
                {
                    throw new InvalidOperationException("An active Yingbo gift must retain its Slash frame as the stack top.");
                }
            }
            else if (_coreColdDamageOperations.HasColdDamageReplacement(pendingAttack.ResolutionId) || _coreShanheS5YimieOperations.HasShanheS5YimieRecovery(pendingAttack.ResolutionId))
            {
            // The exact direct-damage parent and every paid child are checked
            // by AssertColdDamageReplacementFrames.
            }
            else if (_coreDyingCursorRuntimeOperations.ActiveDying is null && _coreDamageCursorRuntimeOperations.ActiveDamageTrigger is null && !_resolutionStackOperations._resolutionStack.OfType<BeforeDamageProgramWindowFrame>().Any() && !_coreColdDamageOperations.HasColdDamageReplacement(pendingAttack.ResolutionId) && !_coreShanheS5YimieOperations.HasShanheS5YimieRecovery(pendingAttack.ResolutionId))
            {
                if ((coreTop is not ResponseWindowFrame response || response.ParentFrameId != pendingAttack.ResolutionId) && !_coreProgramDodgeResponsesOperations.HasProgramDodgeResponseContinuation(pendingAttack) && !_characterXunYuMobileGodOperations.HasQizhengResponseContinuation(pendingAttack) && !_characterXiaoQiaoMouOlOperations.HasMouXqJueyanResponseContinuation(pendingAttack.ResolutionId) && !_coreOriginalUseLightningOperations.HasNativeDelayedAttackChild(pendingAttack.ResolutionId))
                {
                    throw new InvalidOperationException("An active Slash must retain its response window as the stack top.");
                }
            }
            else if (_resolutionStackOperations._resolutionStack.OfType<BeforeDamageProgramWindowFrame>().LastOrDefault()is { Continuation: BeforeDamageProgramContinuation.Attack } beforeDamage)
            {
                var effectiveTop = _coreDamageObserverCursorQueriesOperations.DamageCursorEffectiveTop(includeNestedObservers: true);
                var topMatchesWindow = effectiveTop is BeforeDamageProgramWindowFrame topWindow && topWindow.Id == beforeDamage.Id;
                var topMatchesProgram = effectiveTop is ProgramSkillFrame topProgram && topProgram.WindowContext is { Window: SkillProgramTriggerWindow.BeforeDamageApplied, ParentFrameId: var parentFrameId } && parentFrameId == beforeDamage.Id;
                topMatchesProgram |= _coreJudgmentOutcomeDamageEdgesOperations.HasExactBeforeDamageJudgmentSubtree(beforeDamage.Id);
                topMatchesProgram |= _coreProgramRuntimeHooksOperations.RuntimeHooks.MatchesObserver(ProgramObserverProofPhase.BeforeDamagePrelude, beforeDamage.Id);
                if (beforeDamage.SourceSeat != pendingAttack.SourceSeat || (beforeDamage.RedirectedTargetSeat ?? beforeDamage.TargetSeat) != pendingAttack.TargetSeat || !topMatchesWindow && !topMatchesProgram && !_coreProgramRuntimeHooksOperations.RuntimeHooks.MatchesObserver(ProgramObserverProofPhase.BeforeDamage, beforeDamage.Id))
                {
                    throw new InvalidOperationException("An active before-damage program must retain its damage and parent window.");
                }
            }
            else if (_coreDamageCursorRuntimeOperations.ActiveDamageTrigger is { } triggerContinuation)
            {
                var effectiveTop = damageCursorProjection.Top;
                var topMatchesWindow = effectiveTop is DamageTriggerWindowFrame frame && frame.Id == triggerContinuation.Id && frame.ParentFrameId == triggerContinuation.ParentFrameId && frame.CandidateIndex == triggerContinuation.CandidateIndex;
                var topMatchesProgram = effectiveTop is ProgramSkillFrame program && program.WindowContext is { Window: SkillProgramTriggerWindow.DamageAppliedBeforeDying or SkillProgramTriggerWindow.AfterDamageApplied, ParentFrameId: var parentFrameId } && parentFrameId == triggerContinuation.Id && _coreDamageObserverCursorQueriesOperations.DamageProgramMatchesWindow(program, triggerContinuation);
                var topMatchesProgramPindian = coreTop is PindianFrame pindian && _resolutionStackOperations._resolutionStack.Count >= 2 && _resolutionStackOperations._resolutionStack[^2] is ProgramSkillFrame pindianProgram && pindian.ParentFrameId == pindianProgram.Id && pindianProgram.WindowContext is { Window: SkillProgramTriggerWindow.AfterDamageApplied, ParentFrameId: var pindianWindowId } && pindianWindowId == triggerContinuation.Id && _coreDamageObserverCursorQueriesOperations.DamageProgramMatchesWindow(pindianProgram, triggerContinuation);
                if (!global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreNativeDamageHandlesOperations.GetDamageTriggerAttack(triggerContinuation), pendingAttack) || !topMatchesWindow && !topMatchesProgram && !topMatchesProgramPindian && !damageProgramDying)
                {
                    throw new InvalidOperationException("An active damage trigger window must retain its ordered cursor as the stack top.");
                }
            }
            else if (_coreDyingCursorRuntimeOperations.ActiveDying is { } dyingContinuation)
            {
                if (!_coreWenhePoisonOperations.IsCurrentWenhePoisonDying() && !global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreNativeDamageHandlesOperations.GetDyingAttack(dyingContinuation), pendingAttack))
                {
                    throw new InvalidOperationException("The dying continuation does not belong to the active Slash.");
                }

                var topMatchesDying = coreTop is DyingFrame dying && dying.Id == dyingContinuation.FrameId && dying.ParentFrameId == dyingContinuation.ParentFrameId;
                var topMatchesDyingProgram = coreTop is ProgramSkillFrame { WindowContext: { Window: SkillProgramTriggerWindow.DyingResponse or SkillProgramTriggerWindow.SelfDyingResponse, ParentFrameId: var dyingParentId } } && dyingParentId == dyingContinuation.FrameId;
                // A dying rescue card use legitimately nests its own program card-trigger
                // windows between the rescue use and the Dying frame, the same way damage
                // triggers nest above their Damage frame. Walk the contiguous window chain
                // down and require it to anchor on the active dying continuation, either
                // directly on the Dying frame or through that rescue card use.
                var topMatchesDyingCardWindow = false;
                if (coreTop is ProgramCardTriggerWindowFrame topCardWindow)
                {
                    var windowIndex = _resolutionStackOperations._resolutionStack.Count - 1;
                    var currentWindow = topCardWindow;
                    while (windowIndex > 0 && _resolutionStackOperations._resolutionStack[windowIndex - 1] is ProgramCardTriggerWindowFrame beneath && beneath.Id == currentWindow.ParentFrameId)
                    {
                        windowIndex--;
                        currentWindow = beneath;
                    }

                    topMatchesDyingCardWindow = windowIndex > 0 && _resolutionStackOperations._resolutionStack[windowIndex - 1] switch
                    {
                        DyingFrame directDying => currentWindow.ParentFrameId == directDying.Id && directDying.Id == dyingContinuation.FrameId,
                        CardUseFrame { DyingResponse: not null } rescue => currentWindow.ParentFrameId == rescue.Id && rescue.DyingResponse!.ResolutionId == dyingContinuation.FrameId && windowIndex >= 2 && _resolutionStackOperations._resolutionStack[windowIndex - 2] is DyingFrame chainedDying && chainedDying.Id == dyingContinuation.FrameId,
                        _ => false
                    };
                }

                var dyingFrameIndex = _resolutionStackOperations._resolutionStack.FindLastIndex(frame => frame is DyingFrame dying && dying.Id == dyingContinuation.FrameId && dying.ParentFrameId == dyingContinuation.ParentFrameId);
                var nestedResponseUseOnDying = dyingFrameIndex >= 0 && _resolutionStackOperations._resolutionStack.Skip(dyingFrameIndex + 1).All(frame => frame is CardUseFrame or ProgramCardTriggerWindowFrame or ProgramSkillFrame or HpChangedTriggerWindowFrame);
                var nestedDyingEntry = dyingFrameIndex >= 0 && dyingFrameIndex + 1 < _resolutionStackOperations._resolutionStack.Count && _resolutionStackOperations._resolutionStack[dyingFrameIndex + 1] is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.DyingEntering, Continuation: ProgramLifecycleContinuation.ResumeDyingEntry } entry && entry.ResumeDyingFrameId == dyingContinuation.FrameId && _resolutionStackOperations._resolutionStack.Skip(dyingFrameIndex + 2).All(frame => frame is ProgramSkillFrame or HpChangedTriggerWindowFrame or CardsMovedTriggerWindowFrame);
                if (!topMatchesDying && !topMatchesDyingProgram && !topMatchesDyingCardWindow && !nestedResponseUseOnDying && !nestedDyingEntry && !_characterCaiYongLeOperations.HasLeCaiYongFeibaiRescueContinuation(dyingContinuation.FrameId) && !_characterZhugeLiangWumiaoOperations.HasWuZglRescueContinuation(dyingContinuation.FrameId) && !_characterCaoZhiLeOperations.HasLeCaoZhiWenlanRescueContinuation(dyingContinuation.FrameId) && !_coreOtherDyingRecoveryRidesOperations.IsExactOtherDyingRecoveryRide(dyingContinuation.FrameId) && !_coreEquipmentPairDyingCardObserversOperations.IsExactDyingOwnedCardRide(dyingContinuation.FrameId) && !(dyingFrameIndex >= 0 && (_coreRoundPricedPileAlcoholOperations.IsRoundPricedPileAlcoholRide(dyingFrameIndex, (DyingFrame)_resolutionStackOperations._resolutionStack[dyingFrameIndex]) || _characterLiuBeiSpOl664Operations.IsOlSpZhujiuDyingRide(dyingFrameIndex, (DyingFrame)_resolutionStackOperations._resolutionStack[dyingFrameIndex]))))
                {
                    var top = coreTop;
                    var topShape = top is ProgramSkillFrame diagnosticProgram ? $"ProgramSkillFrame({(diagnosticProgram.SkillId)}, window {(diagnosticProgram.WindowContext?.Window.ToString() ?? "none")}, owner {(diagnosticProgram.OwnerSeat)})" : top?.GetType().Name ?? "empty";
                    throw new InvalidOperationException("An active dying continuation must retain its Dying frame as the stack top " + $"(found {(topShape)}, continuation frame {(dyingContinuation.FrameId)}).");
                }
            }
            else
            {
                throw new InvalidOperationException("An active Slash continuation has an unsupported pending state.");
            }
        }

        _coreSkillProgramsOperations.AssertProgramSkillState();
        _corePaidOwnTargetObserversOperations.AssertActualUseTargetPrograms();
        _coreSlashTargetBenefitObserversOperations.AssertSlashTargetBenefitWindows();
        _coreFirstSlashDuelLegacyWindowsOperations.AssertFirstSlashDuelLegacyWindows();
        _coreSlashTargetPenaltyObserversOperations.AssertTargetPenaltyWindows();
        _coreHpLossSlashMaterialsOperations.AssertHpLossMaterialSlashes();
        _coreLostHpOwnedGiftsOperations.AssertLostHpOwnedGifts();
        _coreSourceCurseObserversOperations.AssertSourceCurseState();
        _corePrivateOfferObserversOperations.AssertPrivateOfferPrograms();
        _corePhaseHandSeizureReceiptsOperations.AssertPhaseHandSeizurePrograms();
        _coreEquipmentDonationObserversOperations.AssertEquipmentDonationPrograms();
        _coreActualDiscardRecoveryObserversOperations.AssertActualEquipmentOrDiscardPrograms();
        _coreShownEntityTurnPoliciesOperations.AssertShownEntityTurnPrograms();
        foreach (var use in _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>())
            _coreGiftedSlashUseRewardsOperations.AssertGiftedSlashUseBenefit(use);
        foreach (var use in _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>())
        {
            _coreJoinedTrickDamageRewardsOperations.AssertJoinedTrickDamageBenefits(use);
            _coreRoundGainedSourceUsesOperations.AssertRoundGainedUseQualifications(use);
        }

        _corePaidColorDamageClaimObserversOperations.AssertPaidColorDamageClaims();
        _coreProgramAttackAttemptOperations.AssertProgramAttackState();
        _coreFinalTargetSlashOperations.AssertFinalTargetSlashReceipts();
        _coreRecordedSuitSlashOperations.AssertRecordedSuitSlashReceipts();
        _coreCurrentUsePhysicalClaimsOperations.AssertCurrentUsePhysicalClaims();
        _coreSequentialTrickTargetsOperations.AssertSequentialTrickTargets();
        if (_activeCardMovementBatchIdsOperations._activeCardMovementBatchIds.Count != 0)
            throw new InvalidOperationException("An atomic card-movement batch escaped its operation boundary.");
        if (_suppressedCardMovedEventBatchIdsOperations._suppressedCardMovedEventBatchIds.Count != 0)
            throw new InvalidOperationException("A card-movement fact suppression escaped its operation boundary.");
        if (_coreDyingCursorRuntimeOperations.ActiveDying is not null && _coreProgramAttackAttemptOperations.CurrentDamageAttempt is null && !_coreDyingCursorRuntimeOperations.ActiveDying.ResumesProgramSkill && !_coreWenhePoisonOperations.IsCurrentWenhePoisonDying() && !_characterHuaTuoAnniversaryGodOperations.IsExactSkillInvocationDying(_coreDyingCursorRuntimeOperations.ActiveDying))
        {
            throw new InvalidOperationException("A dying continuation must retain its active card resolution.");
        }

        _coreFinalizedJudgmentDamageChildrenOperations.AssertFinalizedJudgmentDamageChildren();
        if (_coreJudgmentCursorRuntimeOperations.ActiveJudgment is { } pendingJudgment)
        {
            var programJudgmentFrame = _resolutionStackOperations._resolutionStack.OfType<ProgramJudgmentTriggerWindowFrame>().LastOrDefault();
            var belongsToActiveAttack = _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is { } activeAttack && _coreNativeDamageHandlesOperations.GetJudgmentAttack(pendingJudgment)is { } judgmentAttack && global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(judgmentAttack, activeAttack);
            var belongsToProgramJudgmentDamage = programJudgmentFrame is not null && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(child => child.AttackAttempt?.ProgramJudgmentWindowId == programJudgmentFrame.Id) && programJudgmentFrame.ParentFrameId == pendingJudgment.Id;
            var programSkillFrame = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().LastOrDefault(frame => frame.Id == pendingJudgment.ParentFrameId);
            var belongsToProgramSkill = _coreProgramLifecycleOperations.IsValidProgramJudgmentContinuation(pendingJudgment, programSkillFrame);
            var belongsToDelayedCard = global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime.IsDelayedJudgmentContinuation(pendingJudgment.Continuation) && _coreNativeDamageHandlesOperations.GetJudgmentAttack(pendingJudgment)is null && (_coreOriginalUseLightningOperations.IsOriginalUseLightningJudgment(pendingJudgment) || _coreNativeDamageHandlesOperations.GetDelayedJudgmentCard(pendingJudgment)is { } delayedCard && _cardZonesOperations._cardZones.GetLocation(delayedCard.Id) == CardLocation.Judgment(pendingJudgment.TargetSeat));
            if ((!belongsToActiveAttack && !belongsToProgramJudgmentDamage && !belongsToProgramSkill && !belongsToDelayedCard) || _resolutionStackOperations._resolutionStack.OfType<JudgmentFrame>().LastOrDefault()is not { } judgmentFrame || judgmentFrame.Id != pendingJudgment.Id || judgmentFrame.ParentFrameId != pendingJudgment.ParentFrameId || judgmentFrame.TargetSeat != pendingJudgment.TargetSeat || judgmentFrame.ReplacementCandidateIndex < 0 || judgmentFrame.ReplacementCandidateIndex > judgmentFrame.ReplacementCandidates.Count || judgmentFrame.OutcomeClaim is not null && !_coreJudgmentOutcomeClaimsOperations.HasExactJudgmentOutcomeClaimDisposition(judgmentFrame) || _coreNativeDamageHandlesOperations.GetJudgmentCard(pendingJudgment)is not { } currentJudgmentCard || (_cardZonesOperations._cardZones.GetLocation(currentJudgmentCard.Id) != CardLocation.Judgment(pendingJudgment.TargetSeat) && !_coreJudgmentOutcomeClaimsOperations.HasExactJudgmentOutcomeClaimDisposition(pendingJudgment) && !_characterZhangFeiAnniversaryGodOperations.HasShenZfJudgmentGainDisposition(pendingJudgment) && !(programJudgmentFrame is not null && _cardZonesOperations._cardZones.GetLocation(currentJudgmentCard.Id) == CardLocation.Hand(pendingJudgment.TargetSeat) && programJudgmentFrame.Candidates.Take(programJudgmentFrame.CandidateIndex + 1).Any(candidate => _contentRegistryOperations._contentRegistry.Skills[candidate.SkillId].Program!.Triggers.Single(trigger => trigger.Id == candidate.TriggerId).Effects.Any(effect => effect.Op == SkillProgramEffectOp.ClaimJudgmentCard)))))
            {
                throw new InvalidOperationException("A judgment continuation must retain its public card and ordered cursor.");
            }

            var activeJudgmentProgram = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().LastOrDefault(item => item.WindowContext?.Judgment?.JudgmentFrameId == pendingJudgment.Id || item.WindowContext?.JudgmentReplacement?.JudgmentFrameId == pendingJudgment.Id);
            var isProgramJudgmentChoice = programJudgmentFrame is not null && ReferenceEquals(coreTop, programJudgmentFrame) && !belongsToProgramJudgmentDamage;
            var programReplacementCandidate = activeJudgmentProgram is null && !isProgramJudgmentChoice && pendingJudgment.Succeeded is null && _coreNativeJudgmentOperations.CurrentJudgmentCandidate(pendingJudgment)is { } candidate ? candidate : null;
            var programReplacementTrigger = programReplacementCandidate is not null ? _coreJudgmentReplacementProgramsOperations.GetProgramJudgmentReplacement(programReplacementCandidate).Trigger : null;
            var isProgramReplacementChoice = programReplacementTrigger is not null;
            var expectedJudgmentOwner = isProgramJudgmentChoice ? programJudgmentFrame!.Candidates[programJudgmentFrame.CandidateIndex].OwnerSeat : isProgramReplacementChoice ? pendingJudgment.DelegatedReplacement is { Stage: DelegatedJudgmentStage.Choosing } delegated ? delegated.SubjectSeat : programReplacementCandidate!.OwnerSeat : -1;
            var promptMatches = activeJudgmentProgram is not null || belongsToProgramJudgmentDamage ? true : isProgramJudgmentChoice ? coreDecision is null || _coreJudgmentProgramsOperations.IsProgramJudgmentPromptValid(programJudgmentFrame!) : isProgramReplacementChoice ? _coreDelegatedJudgmentChoicesOperations.IsDelegatedJudgment(programReplacementTrigger!) ? _coreDelegatedJudgmentChoicesOperations.IsDelegatedJudgmentPromptValid(pendingJudgment, coreDecision) : global::CardGame.Core.EngineOwners.CharacterWangYiMobileRuntime.IsDeckTopJudgmentReplacement(programReplacementTrigger!) ? _characterWangYiMobileOperations.IsDeckTopJudgmentReplacementPromptValid(pendingJudgment, coreDecision) : coreDecision is { Kind: DecisionKind.ProgramJudgmentReplacement } replacementDecision && replacementDecision.PlayerSeat == expectedJudgmentOwner && replacementDecision.ValidCardIds.SequenceEqual(_coreJudgmentReplacementProgramsOperations.GetProgramJudgmentReplacementCards(_coreMatchParticipantsPlayersAccess._players[expectedJudgmentOwner], programReplacementTrigger!, pendingJudgment.TargetSeat, pendingJudgment.Reason).Select(card => card.Id)) && replacementDecision.Choices.Count == replacementDecision.ValidCardIds.Count + (programReplacementTrigger!.Optional ? 1 : 0) : false;
            if (!promptMatches)
            {
                throw new InvalidOperationException("An active judgment continuation must retain its private replacement or result prompt.");
            }

            if (activeJudgmentProgram is null && !belongsToProgramJudgmentDamage)
            {
                var expectedJudgmentStatus = isProgramJudgmentChoice && coreDecision is null ? EngineStatus.Running : _coreMatchParticipantsPlayersAccess._players[expectedJudgmentOwner].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
                if (_coreMatchOutcomeStatusAccess._status != expectedJudgmentStatus)
                    throw new InvalidOperationException("A judgment prompt status does not match its current owner.");
            }
        }

        if (_coreDyingCursorRuntimeOperations.ActiveDying is not null && _coreDamageCursorRuntimeOperations.ActiveDamageTrigger is not null && !damageProgramDying)
        {
            throw new InvalidOperationException("A damage trigger window cannot coexist with a dying continuation.");
        }

        if (_coreDamageCursorRuntimeOperations.ActiveDamageTrigger is { } pendingDamageTrigger)
        {
            if (_resolutionStackOperations._resolutionStack.All(frame => frame.Id != pendingDamageTrigger.Id) || pendingDamageTrigger.CandidateIndex < 0 || pendingDamageTrigger.CandidateIndex > pendingDamageTrigger.Candidates.Count)
            {
                throw new InvalidOperationException("A damage trigger continuation must retain a valid trigger window frame.");
            }

            var damageCursorTop = damageCursorProjection.Top;
            var activeDamageProgram = damageCursorTop is ProgramSkillFrame programFrame && programFrame.WindowContext is { Window: SkillProgramTriggerWindow.DamageAppliedBeforeDying or SkillProgramTriggerWindow.AfterDamageApplied, ParentFrameId: var programParentId } && programParentId == pendingDamageTrigger.Id && _coreDamageObserverCursorQueriesOperations.DamageProgramMatchesWindow(programFrame, pendingDamageTrigger);
            var activeDamageProgramPindian = coreTop is PindianFrame pindianFrame && _resolutionStackOperations._resolutionStack.Count >= 2 && _resolutionStackOperations._resolutionStack[^2] is ProgramSkillFrame pindianParent && pindianParent.Id == pindianFrame.ParentFrameId && pindianParent.WindowContext is { Window: SkillProgramTriggerWindow.AfterDamageApplied, ParentFrameId: var pindianWindowId } && pindianWindowId == pendingDamageTrigger.Id && _coreDamageObserverCursorQueriesOperations.DamageProgramMatchesWindow(pindianParent, pendingDamageTrigger);
            var activeDamageProgramJudgment = _coreJudgmentCursorRuntimeOperations.ActiveJudgment is { } nestedJudgment && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(parent => parent.Id == nestedJudgment.ParentFrameId && parent.WindowContext is { Window: SkillProgramTriggerWindow.AfterDamageApplied, ParentFrameId: var damageWindowId } && damageWindowId == pendingDamageTrigger.Id && _coreDamageObserverCursorQueriesOperations.DamageProgramMatchesWindow(parent, pendingDamageTrigger));
            var awaitingDamageProgramPrompt = coreDecision is { Kind: DecisionKind.ProgramTrigger } && pendingDamageTrigger.CandidateIndex < pendingDamageTrigger.Candidates.Count;
            var expectedTop = damageCursorProjection.HasPaidObserverSubtree || damageProgramDying || activeDamageProgram || activeDamageProgramPindian || activeDamageProgramJudgment || damageCursorTop is DamageTriggerWindowFrame triggerFrame && triggerFrame.Id == pendingDamageTrigger.Id && triggerFrame.CandidateIndex == pendingDamageTrigger.CandidateIndex;
            if (!expectedTop)
            {
                throw new InvalidOperationException("A damage trigger continuation must retain its window or skill frame at the stack top.");
            }

            if (!activeDamageProgram && !activeDamageProgramPindian && !activeDamageProgramJudgment && !awaitingDamageProgramPrompt && !damageProgramDying && _coreMatchOutcomeStatusAccess._status != EngineStatus.Running)
            {
                throw new InvalidOperationException("An automatic damage trigger cursor must remain in the running state.");
            }
        }

        if (_coreNativeCardContinuationCursorsOperations.ActiveDuel is not null && (_coreActiveNativeAttackQueriesOperations.ActiveCardAttack is null || (!global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreNativeCardContinuationCursorsOperations.ActiveDuel.Attack, _coreActiveNativeAttackQueriesOperations.ActiveCardAttack) && !(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack.IsProgramJudgmentDamage && global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreJudgmentCursorRuntimeOperations.ActiveJudgment is { } duelJudgment ? _coreNativeDamageHandlesOperations.GetJudgmentAttack(duelJudgment) : null, _coreNativeCardContinuationCursorsOperations.ActiveDuel.Attack)) && !_coreProgramAttackAttemptOperations.IsCardAttackSuspendedByProgramDamage)))
        {
            throw new InvalidOperationException("A Duel continuation must retain its active card resolution.");
        }

        if (_coreNativeCardContinuationCursorsOperations.ActiveGroupCard is { Effect: GroupCardEffect.ResponseAttack } group && !(_coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow is { } groupWindow && _coreGroupTargetNullificationOperations.IsGroupTargetNullification(groupWindow)) && !(_characterLuMengMouOperations.ActiveYingboGift is { Continuation: YingboGiftContinuation.GroupAttack, Attack: null, Group: { } yingboGroup } && ReferenceEquals(yingboGroup, group) && group.CurrentAttack is null && _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is null) && (_coreActiveNativeAttackQueriesOperations.ActiveCardAttack is null || (!global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(group.CurrentAttack, _coreActiveNativeAttackQueriesOperations.ActiveCardAttack) && !(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack.IsProgramJudgmentDamage && global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreJudgmentCursorRuntimeOperations.ActiveJudgment is { } groupJudgment ? _coreNativeDamageHandlesOperations.GetJudgmentAttack(groupJudgment) : null, group.CurrentAttack)) && !_coreProgramAttackAttemptOperations.IsCardAttackSuspendedByProgramDamage)))
        {
            throw new InvalidOperationException("A group continuation must retain its current target attack or pending Yingbo gift choice.");
        }

        if (_coreNativeCardContinuationCursorsOperations.ActiveFactionDefense is { } hujia)
        {
            var responseWindow = _resolutionStackOperations._resolutionStack.OfType<ResponseWindowFrame>().LastOrDefault();
            var awaitingBaguaJudgment = _coreJudgmentCursorRuntimeOperations.ActiveJudgment is { Continuation: JudgmentContinuationKind.FactionDefenseBagua } judgment && global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreNativeDamageHandlesOperations.GetJudgmentAttack(judgment), hujia.Attack) && judgment.TargetSeat == hujia.CurrentCandidateSeat;
            var awaitingEquipmentInvocation = _coreNativeEquipmentInvocationsOperations.HasFactionDefenseEquipmentInvocation(hujia);
            var awaitingSkillInvocation = _coreNativeSkillInvocationObserversOperations.HasFactionDefenseSkillInvocation(hujia);
            var awaitingProvider = coreDecision is { Kind: DecisionKind.RespondDodge } hujiaDecision && hujiaDecision.PlayerSeat == hujia.CurrentCandidateSeat || _coreProgramDodgeResponsesOperations.HasProgramDodgeResponseContinuation(hujia.Attack);
            if (_coreActiveNativeAttackQueriesOperations.ActiveCardAttack is null || !global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, hujia.Attack) || hujia.OwnerSeat != hujia.Attack.TargetSeat || !hujia.Attack.FactionDefenseAttempted || hujia.CandidateIndex < 0 || hujia.CandidateIndex >= hujia.CandidateSeats.Count || responseWindow is null || responseWindow.ParentFrameId != hujia.Attack.ResolutionId || responseWindow.ResponderSeat != hujia.OwnerSeat || (awaitingBaguaJudgment ? 1 : 0) + (awaitingProvider ? 1 : 0) + (awaitingEquipmentInvocation ? 1 : 0) + (awaitingSkillInvocation ? 1 : 0) != 1)
            {
                throw new InvalidOperationException("A FactionDefense continuation must retain its attack, ordered Wei cursor and one private response.");
            }
        }

        if (_coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is { } jijiang)
        {
            var candidateCursorValid = jijiang.CandidateIndex >= 0 && jijiang.CandidateIndex < jijiang.CandidateSeats.Count;
            var providerPromptMatches = _coreNativeSkillInvocationObserversOperations.HasFactionSlashSkillInvocation(jijiang) || _coreFactionRequestPaymentsOperations.IsFactionRequestCostPrompt(jijiang) || _coreFactionRequestCostRecoveriesOperations.HasPaidFactionRequestCostRecovery(jijiang) || coreDecision is { Kind: DecisionKind.RespondSlash } jijiangDecision && jijiangDecision.PlayerSeat == jijiang.CurrentCandidateSeat;
            var zhuquePromptMatches = coreDecision is { Kind: DecisionKind.ZhuqueFan } zhuqueDecision && zhuqueDecision.PlayerSeat == jijiang.OwnerSeat && jijiang.ZhuqueFanProviderSeat == jijiang.CurrentCandidateSeat && jijiang.ZhuqueFanPhysicalCards.Count > 0;
            if ((!jijiang.IsProgramSkillUse && jijiang.PolicySource is null) || jijiang.RequiredKind != CardKind.Slash || string.IsNullOrWhiteSpace(jijiang.ProviderFactionId) || jijiang.CandidateSeats.Any(seat => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) || seat == jijiang.OwnerSeat) || !candidateCursorValid || jijiang.AwaitingProviders && jijiang.AwaitingZhuqueFanChoice || jijiang.AwaitingZhuqueFanChoice != zhuquePromptMatches || jijiang.AwaitingZhuqueFanChoice && (!_coreNativeDamageHandlesOperations.UsesFormalZhuqueFan || !_coreNativeTargetLegalityOperations.HasZhuqueFan(_coreMatchParticipantsPlayersAccess._players[jijiang.OwnerSeat])) || !jijiang.AwaitingZhuqueFanChoice && (jijiang.ZhuqueFanProviderSeat is not null || jijiang.ZhuqueFanPhysicalCards.Count != 0))
            {
                throw new InvalidOperationException("A faction-card request must retain its ordered provider cursor.");
            }

            if (jijiang.IsBorrowedSwordUse)
            {
                var borrowedParent = jijiang.BorrowedSword;
                var responseWindow = _resolutionStackOperations._resolutionStack.OfType<ResponseWindowFrame>().LastOrDefault(frame => borrowedParent is not null && frame.ParentFrameId == borrowedParent.ResolutionId);
                var activeAttackMatches = !jijiang.AwaitingProviders && !jijiang.AwaitingZhuqueFanChoice && jijiang.ActiveAttack is { } activeAttack && global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, activeAttack) && global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(borrowedParent?.ActiveAttack, activeAttack);
                var awaitingZhuqueMatches = jijiang.AwaitingZhuqueFanChoice && responseWindow is not null && zhuquePromptMatches;
                if (borrowedParent is null || !global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword, borrowedParent) || (jijiang.AwaitingProviders ? responseWindow is null || !providerPromptMatches : !awaitingZhuqueMatches && !activeAttackMatches))
                {
                    throw new InvalidOperationException("A Borrowed Sword FactionSlash continuation must retain its parent response or nested Slash.");
                }
            }
            else if (jijiang.IsQinglongCrescentBladeUse)
            {
                var qinglongParent = jijiang.QinglongCrescentBlade;
                var parentAttack = qinglongParent?.Attack;
                var activeAttackMatches = !jijiang.AwaitingProviders && !jijiang.AwaitingZhuqueFanChoice && jijiang.ActiveAttack is { } activeAttack && global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, activeAttack) && (_coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade is null || global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade.Attack, activeAttack));
                var awaitingProviderMatches = jijiang.AwaitingProviders && global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade, qinglongParent) && global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, parentAttack) && providerPromptMatches;
                if (qinglongParent is null || jijiang.TargetSeat != parentAttack?.TargetSeat || (!awaitingProviderMatches && !activeAttackMatches && !_coreNativeEquipmentInvocationsOperations.HasQinglongFactionEquipmentInvocation(jijiang)))
                {
                    throw new InvalidOperationException("A Qinglong Crescent Blade FactionSlash continuation must retain its parent choice or nested Slash.");
                }
            }
            else if (jijiang.IsProgramSkillUse)
            {
                var programFrame = jijiang.ProgramSkillFrameId is { } programFrameId ? _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().LastOrDefault(frame => frame.Id == programFrameId) : null;
                var activeAttackMatches = !jijiang.AwaitingProviders && !jijiang.AwaitingZhuqueFanChoice && jijiang.ActiveAttack is { } activeAttack && global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, activeAttack);
                if (programFrame is null || (!jijiang.IsAssistedProgramUse && programFrame.OwnerSeat != jijiang.OwnerSeat) || jijiang.TargetSeat is not { } targetSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(targetSeat) || (jijiang.AwaitingProviders ? !providerPromptMatches : jijiang.AwaitingZhuqueFanChoice ? !zhuquePromptMatches : !activeAttackMatches))
                {
                    throw new InvalidOperationException("A program faction-card continuation must retain either a provider prompt or its Slash attack.");
                }

                if (jijiang.IsAssistedProgramUse)
                    _coreAssistedFactionSlashOperations.ValidateAssistedFactionSlashParent(jijiang, programFrame);
            }
            else
            {
                var responseWindow = _resolutionStackOperations._resolutionStack.OfType<ResponseWindowFrame>().LastOrDefault();
                if (!jijiang.AwaitingProviders || jijiang.ResponseAttack is not { } responseAttack || !global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, responseAttack) || responseWindow is null || responseWindow.ParentFrameId != responseAttack.ResolutionId || responseWindow.ResponderSeat != jijiang.OwnerSeat || !providerPromptMatches)
                {
                    throw new InvalidOperationException("A response FactionSlash continuation must retain its attack, response window and one private provider prompt.");
                }
            }
        }

        if (_coreNativeCardContinuationCursorsOperations.ActiveGroupCard is { Effect: GroupCardEffect.Recovery } recovery && (_coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not null || recovery.CurrentAttack is not null))
        {
            throw new InvalidOperationException("A group recovery cannot retain an attack continuation.");
        }

        if (_coreNativeCardContinuationCursorsOperations.ActiveGroupCard is { Effect: GroupCardEffect.PublicDraft } draft && !(_coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow is { } draftWindow && _coreGroupTargetNullificationOperations.IsGroupTargetNullification(draftWindow)))
        {
            if (_coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not null || draft.CurrentAttack is not null)
            {
                throw new InvalidOperationException("A FiveGrains draft cannot retain an attack continuation.");
            }

            if (draft.TargetIndex >= draft.TargetSeats.Count)
            {
                throw new InvalidOperationException("A FiveGrains draft must finish before its target cursor reaches the end.");
            }

            var expectedPicker = draft.TargetSeats[draft.TargetIndex];
            if (coreDecision is not { Kind: DecisionKind.SelectHarvestCard } harvestDecision || harvestDecision.PlayerSeat != expectedPicker || !harvestDecision.ValidCardIds.SequenceEqual(draft.RevealedCardIds))
            {
                throw new InvalidOperationException("A FiveGrains draft must retain a prompt for its current picker.");
            }

            var expectedStatus = _coreMatchParticipantsPlayersAccess._players[expectedPicker].IsHuman ? EngineStatus.AwaitingHumanCardSelection : EngineStatus.Running;
            if (_coreMatchOutcomeStatusAccess._status != expectedStatus)
            {
                throw new InvalidOperationException("A FiveGrains draft status does not match its current picker.");
            }
        }

        if (coreDecision?.Kind == DecisionKind.SelectHarvestCard && _coreNativeCardContinuationCursorsOperations.ActiveGroupCard is not { Effect: GroupCardEffect.PublicDraft })
        {
            throw new InvalidOperationException("A harvest selection cannot exist without a public draft.");
        }

        if (coreDecision?.Kind is DecisionKind.FireAttackReveal or DecisionKind.FireAttackDiscard && _coreNativeCardContinuationCursorsOperations.ActiveFireAttack is null)
        {
            throw new InvalidOperationException("A FireAttack selection cannot exist without a FireAttack resolution.");
        }

        if (coreDecision?.Kind == DecisionKind.ProgramJudgmentReplacement && _coreJudgmentCursorRuntimeOperations.ActiveJudgment is null)
        {
            throw new InvalidOperationException("A configured replacement prompt cannot exist without a judgment continuation.");
        }

        if (coreDecision?.Kind == DecisionKind.StoneAxe && _coreNativeCardContinuationCursorsOperations.ActiveStoneAxe is null)
        {
            throw new InvalidOperationException("A Stone Axe prompt cannot exist without its Slash continuation.");
        }

        if (coreDecision?.Kind == DecisionKind.CixiongDoubleSwords && _coreNativeCardContinuationCursorsOperations.ActiveCixiongDoubleSwords is null)
        {
            throw new InvalidOperationException("A Cixiong Double Swords prompt cannot exist without its Slash continuation.");
        }

        if (coreDecision?.Kind == DecisionKind.QinglongCrescentBlade && _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade is null)
        {
            throw new InvalidOperationException("A Qinglong Crescent Blade prompt cannot exist without its Slash continuation.");
        }

        if (coreDecision?.Kind == DecisionKind.IceSword && _coreNativeCardContinuationCursorsOperations.ActiveIceSword is null)
        {
            throw new InvalidOperationException("An Ice Sword prompt cannot exist without its Slash continuation.");
        }

        if (coreDecision?.Kind == DecisionKind.QilinBow && _coreNativeCardContinuationCursorsOperations.ActiveQilinBow is null)
        {
            throw new InvalidOperationException("A Qilin Bow prompt cannot exist without its Slash continuation.");
        }

        if (coreDecision?.Kind == DecisionKind.Yingbo && _characterLuMengMouOperations.ActiveYingboGift is null)
        {
            throw new InvalidOperationException("A Yingbo prompt cannot exist without its card continuation.");
        }

        if (coreDecision?.Kind == DecisionKind.ZhuqueFan && _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is not { AwaitingZhuqueFanChoice: true })
        {
            throw new InvalidOperationException("A Zhuque Fan prompt cannot exist without its FactionSlash continuation.");
        }

        var hasCardProgramPrompt = coreDecision is { Kind: DecisionKind.ProgramTrigger } && (_resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().Any() || _coreColdDamageOperations.HasColdDamagePrompt() || _resolutionStackOperations._resolutionStack.OfType<CardEffectBeforeApplyFrame>().Any() || _coreHulaoCardsOperations.HasHulaoAllianceFeastPrompt() || _coreShenwuCardsOperations.HasShenwuTrickPrompt() || _coreMantianGuohaiOperations.HasMantianGuohaiPrompt() || _coreHellSoulCardsOperations.HasHellSoulPrompt() || _coreWenheChaosOperations.HasWenheChaosPrompt() || _characterGuanYuWumiaoOperations.HasWumiaoFloodDiscardPrompt() || _characterXunYuMobileGodOperations.HasQizhengPrompt() || _coreDragonBoatCardsOperations.HasDragonBoatDiscardPrompt() || _characterCaoPiAnniversaryGodOperations.HasKemingEchoPrompt() || _characterZhangLiaoMobileGodOperations.HasMobileZhitiContestPrompt() || (_characterDianWeiHappyGodOperations.HasHappyDianWeiGuardPrompt() || _characterPangTongHappyGodOperations.HasHappyPangTongPrompt() || _characterDiaoChanHappyGodOperations.HasHappyDiaoChanPrompt() || _characterDaQiaoXiaoQiaoHappyGodOperations.HasHappyQiaoHealthPrompt() || _characterZuoCiHappyGodOperations.HasHappyZuoCiPrompt() || _characterHuZhaoAnniversaryOperations.HasAnniversaryHuZhaoPrompt() || _characterXuJingAnniversaryOperations.HasAnniversaryXuJingPrompt() || _characterDuKuiAnniversaryOperations.HasDuKuiPrompt() || _characterLiuYongAnniversaryOperations.HasAnniversaryLiuYongPrompt() || _characterLiuLiAnniversaryOperations.HasAnniversaryLiuLiPrompt() || _characterBaiLingYunAnniversaryOperations.HasAnniversaryBaiLingYunPrompt() || _characterZhangFenAnniversaryOperations.HasZhangFenPrompt() || _characterYangBiaoAnniversaryOperations.HasAnniversaryYangBiaoPrompt() || _characterHeYanAnniversaryOperations.HasAnniversaryHeYanPrompt() || _characterGuoZhaoAnniversaryOperations.HasAnniversaryGuoZhaoPrompt() || _characterFengXiAnniversaryOperations.HasAnniversaryFengXiPrompt() || _characterMangYaChangAnniversaryOperations.HasAnniversaryJiedaoPrompt() || _characterFuRongFuQianAnniversaryOperations.HasAnniversaryFuRongFuQianPrompt() || _characterGaoXiangAnniversaryOperations.HasAnniversaryGaoXiangPrompt() || _characterZhangWenAnniversaryOperations.HasAnniversaryZhangWenPrompt() || _characterZhangXunAnniversaryOperations.HasAnniversaryZhangXunPrompt() || (_characterSunHuanAnniversaryOperations.HasAnniversarySunHuanPrompt() || _characterGuoYuanMobileShiOperations.HasShiGyPrompt() || _characterLuSuMobileShiOperations.HasShiLsPrompt()) || _characterMuShunAnniversaryOperations.HasAnniversaryMuShunPrompt() || _characterXingDaoRongAnniversaryOperations.HasAnniversaryXingDaoRongPrompt() || _characterHanSongAnniversaryOperations.HasAnniversaryHanSongPrompt() || _characterXiahouHuiAnniversaryOperations.HasAnniversaryXiahouHuiPrompt() || _characterXunXuAnniversaryOperations.HasAnniversaryXunXuPrompt() || _characterZhangHuanAnniversaryOperations.HasAnniversaryZhangHuanPrompt() || _characterCheZhouAnniversaryOperations.HasAnniversaryCheZhouPrompt() || _characterWangChangAnniversaryOperations.HasAnniversaryWangChangPrompt() || _characterWuFuAnniversaryOperations.HasAnniversaryWuFuPrompt() || _characterMaTieAnniversaryOperations.HasAnniversaryMaTiePrompt() || _characterYanXiangAnniversaryOperations.HasAnniversaryYanXiangPrompt() || _characterCuiLieAnniversaryOperations.HasAnniversaryCuiLiePrompt() || _characterHouChengAnniversaryOperations.HasAnniversaryHouChengPrompt() || _characterMaMiaoAnniversaryOperations.HasAnniversaryMaMiaoPrompt() || _characterYuanYinAnniversaryOperations.HasAnniversaryYuanYinPrompt() || _characterQiaoRuiAnniversaryOperations.HasAnniversaryQiaoRuiPrompt() || _characterHuCheErAnniversaryOperations.HasAnniversaryHuCheErPrompt() || _characterZhangHengAnniversaryOperations.HasAnniversaryZhangHengPrompt() || _characterLeiBoAnniversaryOperations.HasAnniversaryLeiBoPrompt() || _characterHaoMengAnniversaryOperations.HasAnniversaryHaoMengPrompt() || _characterHeQiMobileOperations.HasMobileHeQiPrompt() || _characterLiuFengBoundaryOperations.HasBoundaryLiuFengPrompt())) || _characterZhugeGuoLeOperations.HasLeChengyanBorrowedPrompt() || _coreCardActionContestsOperations.HasCardActionPindianContinuation() || _characterPangJiOlOperations.HasBiguoPindianContinuation();
        var hasBeforeDamageProgramPrompt = coreDecision is { Kind: DecisionKind.ProgramTrigger } && _resolutionStackOperations._resolutionStack.OfType<BeforeDamageProgramWindowFrame>().Any();
        var awaitingHumanResponse = (coreDecision?.Kind is DecisionKind.RespondDodge or DecisionKind.RespondSlash or DecisionKind.StoneAxe or DecisionKind.CixiongDoubleSwords or DecisionKind.QinglongCrescentBlade or DecisionKind.IceSword or DecisionKind.QilinBow or DecisionKind.ZhuqueFan or DecisionKind.ProgramJudgmentTrigger or DecisionKind.ProgramJudgmentReplacement || hasCardProgramPrompt || hasBeforeDamageProgramPrompt) && _coreMatchOutcomeStatusAccess._status == EngineStatus.AwaitingHumanResponse;
        var awaitingHumanNullification = coreDecision?.Kind == DecisionKind.Nullification && _coreMatchOutcomeStatusAccess._status == EngineStatus.AwaitingHumanResponse;
        var awaitingHumanDying = coreDecision?.Kind == DecisionKind.RescueDying && _coreMatchOutcomeStatusAccess._status == EngineStatus.AwaitingHumanDying;
        var awaitingAiResponse = _coreNativePromptBoundaryOperations.IsAiResponsePending() || (_resolutionStackOperations._resolutionStack.LastOrDefault() is RequestedDeckBasicFrame && coreDecision is { Kind: DecisionKind.RespondDodge or DecisionKind.RespondSlash or DecisionKind.QinglongCrescentBlade } originalNeed && !_coreMultiplayerOperations.IsHumanControlledSeat(originalNeed.PlayerSeat)) || (coreDecision is { } programDecision && (hasCardProgramPrompt || hasBeforeDamageProgramPrompt) && !_coreMatchParticipantsPlayersAccess._players[programDecision.PlayerSeat].IsHuman) || (_characterChengJiChengCuiOlOperations.TousuiNativeOriginalDecision(_coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade?.Attack.ResolutionId ?? 0) ?? _characterChengJiChengCuiOlOperations.TousuiNativeOriginalDecision(_coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword?.ResolutionId ?? 0) ?? _characterMaXiuMaTieOlOperations.KenshangNativeOriginalDecision(_coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade?.Attack.ResolutionId ?? 0) ?? _characterMaXiuMaTieOlOperations.KenshangNativeOriginalDecision(_coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword?.ResolutionId ?? 0))is { } kenshangOriginal && !_coreMatchParticipantsPlayersAccess._players[kenshangOriginal.PlayerSeat].IsHuman || _coreJudgmentReplacementProgramsOperations.IsAiProgramJudgmentReplacementPending() || _coreJudgmentProgramsOperations.IsAiProgramJudgmentPending();
        var awaitingAiNullification = _coreNativePromptBoundaryOperations.IsAiNullificationPending();
        var awaitingNullificationProgram = _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().Any(frame => global::CardGame.Core.EngineOwners.CoreGameEngineResponseUseCompletionRuntime.IsNullificationResponseProgramWindow(frame) && _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow?.Id == frame.ParentFrameId) || _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow is { } jueyanNullification && _characterXiaoQiaoMouOlOperations.HasMouXqJueyanResponseContinuation(jueyanNullification.Id);
        var awaitingNullificationPayment = _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow is { } paymentWindow && (_coreUnrespondableCounterspellsOperations.TryGetPolicyCounterspellPaymentRide(paymentWindow, out _) || _coreShanheNullifiedClaimObserversOperations.TryGetShanheNullifiedClaimRide(paymentWindow, out _) || _characterWoLongFengChuOlOperations.HasWoLongFengChuUseObserver(paymentWindow.ParentFrameId) || _characterSimaYiMobileGodOperations.HasMobileSimaRenjieNativeSubtree(paymentWindow.ParentFrameId));
        if (!(_characterDianWeiHappyGodOperations.HasHappyDianWeiGuardPrompt() || _characterPangTongHappyGodOperations.HasHappyPangTongPrompt() || _characterDiaoChanHappyGodOperations.HasHappyDiaoChanPrompt() || _characterDaQiaoXiaoQiaoHappyGodOperations.HasHappyQiaoHealthPrompt() || _characterZuoCiHappyGodOperations.HasHappyZuoCiPrompt() || _characterHuZhaoAnniversaryOperations.HasAnniversaryHuZhaoPrompt() || _characterXuJingAnniversaryOperations.HasAnniversaryXuJingPrompt() || _characterDuKuiAnniversaryOperations.HasDuKuiPrompt() || _characterLiuYongAnniversaryOperations.HasAnniversaryLiuYongPrompt() || _characterLiuLiAnniversaryOperations.HasAnniversaryLiuLiPrompt() || _characterBaiLingYunAnniversaryOperations.HasAnniversaryBaiLingYunPrompt() || _characterZhangFenAnniversaryOperations.HasZhangFenPrompt() || _characterYangBiaoAnniversaryOperations.HasAnniversaryYangBiaoPrompt() || _characterHeYanAnniversaryOperations.HasAnniversaryHeYanPrompt() || _characterGuoZhaoAnniversaryOperations.HasAnniversaryGuoZhaoPrompt() || _characterFengXiAnniversaryOperations.HasAnniversaryFengXiPrompt() || _characterMangYaChangAnniversaryOperations.HasAnniversaryJiedaoPrompt() || _characterFuRongFuQianAnniversaryOperations.HasAnniversaryFuRongFuQianPrompt() || _characterGaoXiangAnniversaryOperations.HasAnniversaryGaoXiangPrompt() || _characterZhangWenAnniversaryOperations.HasAnniversaryZhangWenPrompt() || _characterZhangXunAnniversaryOperations.HasAnniversaryZhangXunPrompt() || (_characterSunHuanAnniversaryOperations.HasAnniversarySunHuanPrompt() || _characterGuoYuanMobileShiOperations.HasShiGyPrompt() || _characterLuSuMobileShiOperations.HasShiLsPrompt()) || _characterMuShunAnniversaryOperations.HasAnniversaryMuShunPrompt() || _characterXingDaoRongAnniversaryOperations.HasAnniversaryXingDaoRongPrompt() || _characterHanSongAnniversaryOperations.HasAnniversaryHanSongPrompt() || _characterXiahouHuiAnniversaryOperations.HasAnniversaryXiahouHuiPrompt() || _characterXunXuAnniversaryOperations.HasAnniversaryXunXuPrompt() || _characterZhangHuanAnniversaryOperations.HasAnniversaryZhangHuanPrompt() || _characterCheZhouAnniversaryOperations.HasAnniversaryCheZhouPrompt() || _characterWangChangAnniversaryOperations.HasAnniversaryWangChangPrompt() || _characterWuFuAnniversaryOperations.HasAnniversaryWuFuPrompt() || _characterMaTieAnniversaryOperations.HasAnniversaryMaTiePrompt() || _characterYanXiangAnniversaryOperations.HasAnniversaryYanXiangPrompt() || _characterCuiLieAnniversaryOperations.HasAnniversaryCuiLiePrompt() || _characterHouChengAnniversaryOperations.HasAnniversaryHouChengPrompt() || _characterMaMiaoAnniversaryOperations.HasAnniversaryMaMiaoPrompt() || _characterYuanYinAnniversaryOperations.HasAnniversaryYuanYinPrompt() || _characterQiaoRuiAnniversaryOperations.HasAnniversaryQiaoRuiPrompt() || _characterHuCheErAnniversaryOperations.HasAnniversaryHuCheErPrompt() || _characterZhangHengAnniversaryOperations.HasAnniversaryZhangHengPrompt() || _characterLeiBoAnniversaryOperations.HasAnniversaryLeiBoPrompt() || _characterHaoMengAnniversaryOperations.HasAnniversaryHaoMengPrompt() || _characterHeQiMobileOperations.HasMobileHeQiPrompt() || _characterLiuFengBoundaryOperations.HasBoundaryLiuFengPrompt()) && !_characterZhugeGuoLeOperations.HasLeChengyanBorrowedPrompt() && !_coreWenhePoisonOperations.HasExactWenhePoisonResponseContinuation() && !_coreHulaoCardsOperations.HasHulaoAllianceFeastPrompt() && !_coreShenwuCardsOperations.HasShenwuTrickPrompt() && !_coreMantianGuohaiOperations.HasMantianGuohaiPrompt() && !_coreHellSoulCardsOperations.HasHellSoulPrompt() && !_coreWenheChaosOperations.HasWenheChaosPrompt() && !_characterGuanYuWumiaoOperations.HasWumiaoFloodDiscardPrompt() && !_characterXunYuMobileGodOperations.HasQizhengPrompt() && !_coreDragonBoatCardsOperations.HasDragonBoatDiscardPrompt() && !_resolutionStackOperations._resolutionStack.OfType<CardEffectBeforeApplyFrame>().Any() && _coreProgramAttackAttemptOperations.CurrentDamageAttempt is null && _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow is null && _coreJudgmentCursorRuntimeOperations.ActiveJudgment is null && !_characterCaoPiAnniversaryGodOperations.HasKemingEchoPrompt() && !_characterZhangLiaoMobileGodOperations.HasMobileZhitiContestPrompt() && _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().All(frame => !_characterZhangYanMobileShiOperations.HasShiZySuspendedCompletedParent(frame) && !_characterDengAiAnniversaryGodOperations.HasShenDaSuspendedCompletedParent(frame) && !_characterSimaYiMouOperations.ValidMouSmPlayedWindow(frame) && !_characterLuSuMouOperations.ValidMouLuSuPlayedWindow(frame) && !_coreDelayedPlacementTargetProgramsOperations.IsExactFinalizedDelayedPlacementWindow(frame) && !_characterChengYuMouOlOperations.HasMouDanchiSuspendedCompletedParent(frame) && !(_coreCardUseLifecycleOperations.LifecycleCardUse(frame.ParentFrameId)is { } diouParent && (_characterDiaoChanLeOperations.IsDiouSuspendedCompletedAttack(diouParent, frame.Id) || _characterLiuChengMouMobileOperations.IsMouLiuChengSuspendedCompletedAttack(diouParent, frame.Id))) && frame.Continuation is not (ProgramCardContinuation.DelayedCard or ProgramCardContinuation.BeforeTrickTargetEffects or ProgramCardContinuation.BeforeSimpleTargetEffects or ProgramCardContinuation.FinalizedTrick or ProgramCardContinuation.FinalizedSimpleCard or ProgramCardContinuation.CommittedTrick or ProgramCardContinuation.CommittedSimpleCard or ProgramCardContinuation.CompletedCard)) && _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest?.IsProgramSkillUse != true && _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest?.IsBorrowedSwordUse != true && _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword is null && _coreDyingCursorRuntimeOperations.ActiveDying?.ResumesProgramSkill != true && !_characterHuaTuoAnniversaryGodOperations.IsExactSkillInvocationDying(_coreDyingCursorRuntimeOperations.ActiveDying) && (awaitingHumanResponse || awaitingHumanDying || awaitingAiResponse) && !awaitingNullificationProgram)
        {
            throw new InvalidOperationException("A response continuation exists without an active Slash.");
        }

        if (_coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow is not null && (awaitingHumanResponse || awaitingHumanDying || awaitingAiResponse) && !awaitingNullificationProgram && !awaitingNullificationPayment)
        {
            throw new InvalidOperationException("A Nullification window cannot retain a non-Nullification prompt.");
        }

        if (_coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow is not null && !awaitingHumanNullification && !awaitingAiNullification && !awaitingNullificationProgram && !awaitingNullificationPayment)
        {
            throw new InvalidOperationException("An active Nullification window must retain exactly one response continuation.");
        }

        if (_coreDyingCursorRuntimeOperations.ActiveDying is null && _coreDamageCursorRuntimeOperations.ActiveDamageTrigger is null && _coreJudgmentCursorRuntimeOperations.ActiveJudgment is null && _characterLuMengMouOperations.ActiveYingboGift is null && _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not null && awaitingHumanResponse == awaitingAiResponse && !_coreIdentityAppearanceInvariantsOperations.HasIdentityAppearanceAttack(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack) && !_coreNativeEquipmentInvocationsOperations.HasNativeEquipmentInvocation(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack.ResolutionId) && !_characterZhangLiaoMobileGodOperations.HasMobileZhitiDuelContinuation(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack) && !_characterSimaYiMobileGodOperations.HasMobileSimaRenjieNativeSubtree(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack.ResolutionId) && !(_coreNativeCardContinuationCursorsOperations.ActiveCixiongDoubleSwords is { } kemingCixiongContinuation && _characterCaoPiAnniversaryGodOperations.HasKemingCixiongContinuation(kemingCixiongContinuation)) && !_coreNativeSkillInvocationObserversOperations.HasNativeFactionSkillInvocation(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack.ResolutionId) && !_coreOriginalUseLightningOperations.HasNativeDelayedAttackChild(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack.ResolutionId) && !_coreProgramDodgeResponsesOperations.HasProgramDodgeResponseContinuation(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack) && !_characterXiaoQiaoMouOlOperations.HasMouXqJueyanResponseContinuation(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack.ResolutionId) && !_corePaidOwnTargetObserversOperations.HasActualUseTargetObserver(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack.ResolutionId) && !_characterLvKuangLvXiangOlOperations.HasLvKuangLvXiangCardObserver(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack.ResolutionId) && !_characterXinPiOlOperations.HasXinPiCardObserver(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack.ResolutionId) && !_characterHuaManOlOperations.HasHuaManCardContinuation(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack.ResolutionId) && !_coreSlashTargetBenefitObserversOperations.HasSlashTargetBenefitUseObserver(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack.ResolutionId) && !_coreFirstSlashDuelLegacyWindowsOperations.HasFirstSlashDuelLegacyUseObserver(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack.ResolutionId) && !_characterWoLongFengChuOlOperations.HasWoLongFengChuUseObserver(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack.ResolutionId) && !_coreSlashTargetPenaltyObserversOperations.HasTargetPenaltyUseObserver(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack.ResolutionId) && !_coreRecoveryReplacementOperations.HasRecoveryPaidCardUseObserver(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack.ResolutionId) && !_coreShanheS5YimieObserversOperations.HasShanheS5YimieRecoveryContinuation(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack.ResolutionId) && !(_characterHuBanOlOperations.HasHuiyunCompletedUseObserver(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack.ResolutionId) || _characterMaChengOlOperations.HasChenglieCompletedUseObserver(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack.ResolutionId) || _characterZhouCangBoundaryMobileOperations.HasBoundaryZhouCangCompletedUseObserver(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack.ResolutionId) || _characterGuoYuanMobileShiOperations.HasShiGyCompletedUseObserver(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack.ResolutionId)) && !(_coreChainedStateBasicReturnsOperations.HasChainedStateBasicObserver(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack.ResolutionId) || _coreDrawFundedDistinctBasicObserversOperations.HasDrawFundedDistinctBasicObserver(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack.ResolutionId)) && !_characterGuoTuOlOperations.HasWeijieNativeRequestContinuation(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack.ResolutionId) && !_characterLiuFengBoundaryOperations.HasBoundaryLiuFengNativeRequestContinuation(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack.ResolutionId) && !_characterLiuZhangOlOperations.HasZonghuNativeRequestContinuation(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack.ResolutionId) && !_characterHuanShuJunOlOperations.HasHuanpeiNativeRequestContinuation(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack.ResolutionId) && !_coreWenyiDamageHealthOperations.HasWenyiDamageHealthContinuation(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack.ResolutionId) && !_characterDaQiaoXiaoQiaoHappyGodOperations.HasHappyQiaoDamageHealthContinuation(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack.ResolutionId) && !_characterDaQiaoXiaoQiaoHappyGodOperations.HasHappyQiaoCardMaximumHealthContinuation(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack.ResolutionId) && !_coreHealthQueriesOperations.HasHappyQiaoNativeDamageChoice(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack.ResolutionId) && !_coreFriendDuelResponsesOperations.HasFriendDuelResponseObserver(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack.ResolutionId) && !_characterZhangFeiWumiaoOperations.HasWuZhangFeiUseContinuation(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack.ResolutionId) && !_characterCaiYongLeOperations.HasLeCaiYongFeibaiUseContinuation(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack.ResolutionId) && !_characterSimaYiMouOperations.HasMouSmPrivateContinuation(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack.ResolutionId) && !_characterZhugeLiangWumiaoOperations.HasWuZglUseContinuation(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack.ResolutionId) && !_characterCaoZhiLeOperations.HasLeCaoZhiWenlanUseContinuation(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack.ResolutionId) && !_characterLuZhiMouOlOperations.HasMouLzDirectUseContinuation(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack.ResolutionId) && !_coreFactionRequestCostRecoveriesOperations.HasPaidFactionRequestCostRecovery(_coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest) && !_coreDyingSuitsAndHistoricalUseObserversOperations.HasDyingSuitsSuspendedAttack(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack.ResolutionId))
        {
            throw new InvalidOperationException("An active card resolution must have exactly one response continuation.");
        }

        if (_coreMatchOutcomeStatusAccess._status == EngineStatus.Completed && (coreDecision is not null || _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not null || _coreNativeCardContinuationCursorsOperations.ActiveDuel is not null || _coreNativeCardContinuationCursorsOperations.ActiveGroupCard is not null || _coreNativeCardContinuationCursorsOperations.ActiveFireAttack is not null || _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow is not null || _coreNativeCardContinuationCursorsOperations.ActiveTargetCardSelection is not null || _coreDyingCursorRuntimeOperations.ActiveDying is not null || _coreDamageCursorRuntimeOperations.ActiveDamageTrigger is not null || _coreJudgmentCursorRuntimeOperations.ActiveJudgment is not null || _characterLuMengMouOperations.ActiveYingboGift is not null || _coreNativeCardContinuationCursorsOperations.ActiveFactionDefense is not null || _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is not null || _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword is not null || _coreNativeCardContinuationCursorsOperations.ActiveStoneAxe is not null || _coreNativeCardContinuationCursorsOperations.ActiveCixiongDoubleSwords is not null || _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade is not null || _coreNativeCardContinuationCursorsOperations.ActiveIceSword is not null || _coreNativeCardContinuationCursorsOperations.ActiveQilinBow is not null || _coreNativeCardContinuationCursorsOperations.ActiveFangtianHalberd is not null || processing.Count != 0))
        {
            throw new InvalidOperationException("A completed game cannot retain pending resolution state.");
        }
    }

    internal bool IsActiveAttackCardConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, IReadOnlyList<Card> processing)
    {
        if (_characterDengAiAnniversaryGodOperations.IsShenDaCuixinAttackConsistent(attack, processing))
            return true;
        processing = _coreWenyiDamageHealthOperations.SeparateWenyiDamageHealthChildProcessing(attack, processing);
        processing = _characterDaQiaoXiaoQiaoHappyGodOperations.SeparateHappyQiaoDamageHealthChildProcessing(attack, processing);
        processing = _characterDaQiaoXiaoQiaoHappyGodOperations.SeparateHappyQiaoCardMaximumHealthChildProcessing(attack, processing);
        processing = _characterHuangYueyingOlMouOperations.SeparateMouLixianDamageChildProcessing(attack, processing);
        processing = _coreProgramShanheSourceWarElephantOperations.SeparateShanheWarElephantCounterParentProcessing(attack, processing);
        if (_characterZhouYuMouOperations.MouZyNativeAncestorProcessing(attack.ResolutionId)is { } mouZyAncestors)
            processing = processing.Where(card => !mouZyAncestors.Contains(card.Id)).ToArray();
        if (_characterZhouYuMouOperations.IsMouZyZeroFireAttackConsistent(attack, processing))
            return true;
        if (_coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword is { ActiveAttack: { } issuedBorrowedAttack } diouBorrowed && global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(issuedBorrowedAttack, attack) && _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is { Action: { } borrowedAction } borrowedChild && borrowedAction.ParentActionId == _coreCardUseLifecycleOperations.LifecycleCardUse(diouBorrowed.ResolutionId)?.Action?.ActionId && borrowedAction.ProviderSeat == diouBorrowed.WeaponOwnerSeat && borrowedChild.SourceSeat == borrowedAction.ActorSeat && borrowedAction.EffectiveKind == attack.EffectiveCardKind && (_characterDengAiAnniversaryGodOperations.ShenDaCuixinAncestorProcessing(diouBorrowed.ResolutionId) ?? _characterXiaoQiaoMouOlOperations.MouXqNativeAncestorProcessing(diouBorrowed.ResolutionId) ?? _characterDiaoChanLeOperations.DiouNativeAncestorProcessing(diouBorrowed.ResolutionId))is { } diouBorrowedAncestors)
            processing = processing.Where(card => !diouBorrowedAncestors.Contains(card.Id)).ToArray();
        var qinglongAncestors = _coreQinglongInsertionOperations.QinglongAncestorProcessing(attack.ResolutionId);
        processing = processing.Where(card => !qinglongAncestors.Contains(card.Id)).ToArray();
        // A rescue card can be awaiting its own use triggers while the attack
        // remains suspended below the dying frame. Its costs belong to that
        // child use and are checked by the card-window invariant separately.
        var rescueCosts = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Where(frame => frame.Id != attack.ResolutionId && frame.DyingResponse is { } response && response.ResolutionId == _coreDyingCursorRuntimeOperations.ActiveDying?.FrameId).SelectMany(frame => frame.Action?.PhysicalCards ?? []).Select(cost => cost.CardId).ToHashSet();
        if (attack.PhysicalCards.Any(card => rescueCosts.Contains(card.Id)))
            return false;
        processing = processing.Where(card => !rescueCosts.Contains(card.Id)).ToArray();
        var activePindian = _resolutionStackOperations._resolutionStack.OfType<PindianFrame>().SingleOrDefault();
        if (activePindian is { } pindian && (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == pindian.Id || _characterPangJiOlOperations.IsDouyuPairSubtree(pindian) || _characterLuKangWumiaoOperations.IsWuLuKangPairSubtree(pindian) || _characterZhangLiaoMobileGodOperations.IsMobileZhitiContestSubtree(pindian)))
        {
            var contestIds = _corePindianPoliciesOperations.CurrentPindianProcessingIds(pindian);
            if (contestIds.Any(id => (pindian.ParentProcessingCardIds ?? []).Contains(id)))
                return false;
            processing = processing.Where(card => !contestIds.Contains(card.Id)).ToArray();
        }

        if (_coreProgramLifecycleOperations.HasPendingProgramBoundCards)
        {
            return _coreProgramLifecycleOperations.IsProgramProcessingConsistent(attack, processing);
        }

        if (_resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().LastOrDefault()is { } programFrame && programFrame.Action.Type == CardActionType.Response)
        {
            var alternativeResponseIds = programFrame.Action.PhysicalCards.Where(cost => _coreAlternativeViewAsCostsOperations.IsProgramAlternativeCost(programFrame.Action, cost.CardId)).Select(cost => cost.CardId).ToHashSet();
            if (processing.Any(card => alternativeResponseIds.Contains(card.Id)))
                return false;
            var destroyedResponseIds = programFrame.Action.PhysicalCards.Where(cost => _coreGuanduChariotResponsesOperations.GuanduChariotResponsePayment(programFrame.Action, programFrame.ParentFrameId, cost, programFrame.Continuation)is not null).Select(cost => cost.CardId).ToHashSet();
            if (processing.Any(card => destroyedResponseIds.Contains(card.Id)))
                return false;
            var responseIds = programFrame.Action.PhysicalCards.Where(cost => !alternativeResponseIds.Contains(cost.CardId) && !destroyedResponseIds.Contains(cost.CardId) && !_coreResponseEntityExchangeOperations.IsExchangedCardClaim(programFrame.Action.ActionId, cost.CardId)).Select(cost => cost.CardId).ToHashSet();
            if (programFrame.MouSimaYiPlayed is { Window: SkillProgramTriggerWindow.CardResponseCompleted } && _characterSimaYiMouOperations.ValidMouSmPlayedWindow(programFrame) && programFrame.Action.PhysicalCards.Where(cost => responseIds.Contains(cost.CardId)).All(cost => _characterSimaYiMouOperations.IsMouSmPlayedCostFinished(programFrame, cost)) || programFrame.MouLuSuAnniversaryPlayed is { Window: SkillProgramTriggerWindow.CardResponseCompleted } && _characterLuSuMouOperations.ValidMouLuSuPlayedWindow(programFrame) && programFrame.Action.PhysicalCards.Where(cost => responseIds.Contains(cost.CardId)).All(cost => _characterLuSuMouOperations.IsMouLuSuPlayedCostFinished(programFrame, cost)) || programFrame.ResponseCompletion is not null && _coreResponseCompletionOperations.ValidResponseCompletionWindow(programFrame) || programFrame.CompletedResponseReturn is { IsCommitted: false } || programFrame.CompletedResponseReturn is { IsCommitted: true, Kind: ProgramCompletedResponseKind.Nullification })
            {
                if (processing.Any(card => responseIds.Contains(card.Id)))
                    return false;
            }
            else if (responseIds.Any(id => processing.All(card => card.Id != id)))
                return false;
            processing = processing.Where(card => !responseIds.Contains(card.Id)).ToArray();
        }

        if (_coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is { CardId: 0, CardKind: CardKind.QizhengXiangsheng, Qizheng: not null, VirtualOrdinaryTrickOrigin: not null })
            return _characterXunYuMobileGodOperations.IsVirtualQizhengAttackConsistent(attack, processing);
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.HeJinMouzhu is not null)
            return _characterHeJinOlOperations.IsHeJinMouzhuAttackConsistent(attack, processing);
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.WuAnGuoLiyong is not null)
            return _characterWuAnGuoOlOperations.IsWuAnGuoLiyongAttackConsistent(attack, processing);
        if (_coreHpLossSlashMaterialsOperations.IsExactHpLossMaterialSlash(attack, processing))
            return true;
        if (_coreNativeMaterialSlashOperations.IsExactNativeMaterialSlashExpansion(attack, processing))
            return true;
        if (_coreRoundOnceCardlessReturnsOperations.IsRoundOnceCardlessProgramRequestAttackConsistent(attack, processing))
            return true;
        if (_characterGuoTuOlOperations.IsWeijieProgramRequestAttackConsistent(attack, processing))
            return true;
        if (_characterLiuFengBoundaryOperations.IsBoundaryLiuFengNativeAttackConsistent(attack, processing))
            return true;
        if (_characterLiuZhangOlOperations.IsZonghuProgramRequestAttackConsistent(attack, processing))
            return true;
        if (_characterWenChouMouOlOperations.IsMouWenChouDuelAttackConsistent(attack, processing) || _characterXuHuangMouMobileOperations.IsMouXuHuangDuelAttackConsistent(attack, processing) || (_characterChengYuMouOlOperations.IsMouDanchiAttackConsistent(attack, processing) || _characterLiuChengMouMobileOperations.IsMouLiuChengAttackConsistent(attack, processing)) || _characterDiaoChanLeOperations.IsDiouAttackConsistent(attack, processing) || _characterXiaoQiaoMouOlOperations.IsMouXqJueyanAttackConsistent(attack, processing) || _characterHuanShuJunOlOperations.IsHuanpeiAttackConsistent(attack, processing))
            return true;
        if (_coreSoleTargetProtectionObserversOperations.IsSoleTargetQuarantinedAttackConsistent(attack, processing))
            return true;
        if (_characterAHuiNanOlOperations.IsAHuiNanJuemanAttackConsistent(attack, processing) || _characterQinLangOlOperations.IsQinLangXianyingAttackConsistent(attack, processing) || _characterQinLangOlOperations.IsQinLangXianyingBorrowedAttackConsistent(attack, processing) || _characterTianChouOlOperations.IsTianChouShandaoAttackConsistent(attack, processing) || _characterBaoSanNiangOlOperations.IsChosenAssaultAttackConsistent(attack, processing) || _characterZhugeLiangMouOperations.IsMouZhijueZeroAttackConsistent(attack, processing) || _characterZhugeLiangMouOperations.IsMouZhijueBorrowedAttackConsistent(attack, processing) || _coreTieredRoundZeroReturnsOperations.IsPaidZeroMaterialAttackConsistent(attack, processing) || _coreTieredRoundZeroReturnsOperations.IsTieredRoundZeroBorrowedAttackConsistent(attack, processing))
            return true;
        if (_characterDongChengMouOperations.IsMouDcZeroAttackConsistent(attack, processing))
            return true;
        if (_coreChainedStateBasicReturnsOperations.IsChainedStateBasicAttackConsistent(attack, processing))
            return true;
        if (_coreDrawFundedDistinctBasicReturnsOperations.IsDrawFundedDistinctBasicAttackConsistent(attack, processing))
            return true;
        if (_coreDragonBoatCardsOperations.IsDragonBoatZeroAgainstCurrentAttackConsistent(attack, processing))
            return true;
        if (_coreVirtualOrdinaryTrickUsesOperations.IsProgramVirtualAreaAttackConsistent(attack, processing))
            return true;
        if (_characterJiangWeiAnniversaryGodOperations.IsShenJwAttackConsistent(attack, processing))
            return true;
        if (_characterLuShiOlOperations.IsOlLuShiLeijieAttackConsistent(attack, processing))
            return true;
        if (_characterLiuYongAnniversaryOperations.IsAnniversaryLiuYongSlashConsistent(attack, processing))
            return true;
        if (_coreShenwuCardsOperations.IsShenwuTransplantAttackConsistent(attack, processing))
            return true;
        if (_coreWenheCardsOperations.IsWenheChaosAttackConsistent(attack, processing))
            return true;
        if (_coreAlternativeViewAsCostsOperations.TryValidateProgramAlternativeCostAttack(attack, processing, out var alternativeCostValid))
            return alternativeCostValid;
        if (attack.IsProgramJudgmentDamage)
        {
            var retainedParentCardIds = (_coreJudgmentCursorRuntimeOperations.ActiveJudgment is { } parentJudgment ? _coreNativeDamageHandlesOperations.GetJudgmentAttack(parentJudgment) : null)?.PhysicalCards.Where(card => _cardZonesOperations._cardZones.GetLocation(card.Id) == CardLocation.Processing).Select(card => card.Id).Order().ToArray() ?? [];
            return attack.Card is null && processing.Select(card => card.Id).Order().SequenceEqual(retainedParentCardIds) && attack.ProgramJudgmentFrameId is { } frameId && _resolutionStackOperations._resolutionStack.OfType<ProgramJudgmentTriggerWindowFrame>().Any(frame => frame.Id == frameId);
        }

        if (attack.Card is null && (attack.EffectiveCardKind == CardKind.Slash || _coreNativeSlashTargetDuelOperations.IsNativeSlashTargetDuelAttack(attack) || _coreShanheS5XuncanOperations.IsShanheS5YunshenUse(attack.ResolutionId) || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is { } fireUse && _coreCurrentSlashFireOffersOperations.IsCurrentSlashFireChangedUse(fireUse) || global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(attack.EffectiveCardKind ?? CardKind.Slash) && _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.VirtualBasicReturn is not null) && (attack.ProgramSkillCardUseFrameId is not null || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.VirtualBasicReturn is not null || _coreForeignPublicPileSlashesOperations.IsForeignPublicPileSlashUse(attack.ResolutionId)))
        {
            return processing.Count == 0 && _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Any(frame => frame.Id == attack.ResolutionId && frame.CardId == 0 && frame.PhysicalCardIds?.Count is 0);
        }

        if (attack.Card is null && attack.EffectiveCardKind == CardKind.Duel && _characterFanJiangZhangDaOlOperations.IsJueshengDuelUse(attack.ResolutionId))
            return _characterFanJiangZhangDaOlOperations.IsJueshengDuelAttackConsistent(attack, processing);
        if (attack.Card is null && attack.EffectiveCardKind == CardKind.Duel && _characterHaoPuOlOperations.IsZhenyingDuelUse(attack.ResolutionId))
            return _characterHaoPuOlOperations.IsZhenyingDuelAttackConsistent(attack, processing);
        if (attack.Card is null && attack.EffectiveCardKind == CardKind.Duel && _coreDamageTargetObtainProgramsOperations.IsDamageTargetDuelUse(attack.ResolutionId))
            return processing.Select(c => c.Id).Order().SequenceEqual(_coreDamageTargetObtainProgramsOperations.DamageTargetDuelOuterProcessing(attack.ResolutionId).Order()) && _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.DamageTargetDuelOrigin is { AttackStarted: true } obtained && attack.ProgramSkillCardUseFrameId == obtained.ParentProgramFrameId;
        if (attack.Card is null && attack.EffectiveCardKind == CardKind.Duel && _coreConditionalDiscardDuelReturnsOperations.IsConditionalDiscardDuelUse(attack.ResolutionId))
            return processing.Count == 0 && _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.ConditionalDiscardDuelOrigin is { AttackStarted: true } conditional && attack.ProgramSkillCardUseFrameId == conditional.ParentProgramFrameId;
        if (attack.Card is null && attack.EffectiveCardKind == CardKind.Duel && _characterHuangQuanMobileShiOperations.IsShiQuanDuelUse(attack.ResolutionId))
            return processing.Count == 0 && _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.ShiQuanDuelOrigin is { AttackStarted: true } shiQuanOrigin && attack.ProgramSkillCardUseFrameId == shiQuanOrigin.ParentProgramFrameId;
        if (attack.Card is null && attack.EffectiveCardKind == CardKind.Duel && _coreDualColorDuelProgramsOperations.IsDualColorDuelUse(attack.ResolutionId))
            return processing.Count == 0 && _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.DualColorDuelOrigin is { AttackStarted: true } dualColor && attack.ProgramSkillCardUseFrameId == dualColor.ParentProgramFrameId;
        if (attack.Card is null && attack.EffectiveCardKind == CardKind.Duel && _coreDamageColorDuelProgramsOperations.IsSelectedActorDuelUse(attack.ResolutionId))
            return processing.Count == 0 && _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.SelectedActorDuelOrigin is { AttackStarted: true } origin && attack.ProgramSkillCardUseFrameId == origin.ParentProgramFrameId;
        if (attack.Card is null && attack.EffectiveCardKind == CardKind.Duel && _coreFriendEventsOperations.IsFriendRecastDuelUse(attack.ResolutionId))
            return processing.Count == 0 && attack.PhysicalCards.Count == 0 && _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.FriendRecastDuel is { } friendOrigin && attack.CardUserSeat == friendOrigin.OwnerSeat && attack.ProgramSkillCardUseFrameId == friendOrigin.ProgramFrameId;
        if (attack.Card is null && attack.EffectiveCardKind == CardKind.Duel && attack.ProgramSkillFrameId == attack.ResolutionId && _coreNativeCardContinuationCursorsOperations.ActiveDuel is { } virtualDuel && virtualDuel.Attack.ResolutionId == attack.ResolutionId)
            return _coreProgramVirtualDuelOperations.IsProgramVirtualDuelAttackConsistent(attack, processing);
        if (attack.IsDelayedJudgmentDamage && _resolutionStackOperations._resolutionStack.OfType<JudgmentFrame>().Any(j => j.Id == attack.ResolutionId && j.OriginalUseLightning is not null))
            return _coreOriginalUseLightningOperations.IsOriginalUseLightningAttackConsistent(attack, processing);
        var attackCard = attack.Card;
        if (attackCard is null)
        {
            return false;
        }

        if (attack.IsDelayedJudgmentDamage)
        {
            var delayedLocation = _cardZonesOperations._cardZones.GetLocation(attackCard.Id);
            return delayedLocation.Zone is CardZoneKind.Judgment or CardZoneKind.Hand or CardZoneKind.DiscardPile;
        }

        if (_coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword is { ActiveAttack: { } borrowedAttack } borrowedSword && global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(borrowedAttack, attack))
        {
            var retainedParent = !_characterZhugeLiangMouOperations.IsZeroEntityProcessingParent(borrowedSword.ResolutionId, borrowedSword.Card.Id) && !_coreCurrentUsePhysicalClaimsOperations.IsClaimedUseCardEntity(borrowedSword.ResolutionId, borrowedSword.Card.Id);
            var parentCount = retainedParent ? 1 : 0;
            if (retainedParent && processing.All(card => card.Id != borrowedSword.Card.Id))
            {
                return false;
            }

            var allowedIds = attack.PhysicalCards.Select(card => card.Id).Concat(retainedParent ? new[] { borrowedSword.Card.Id } : Array.Empty<int>()).ToHashSet();
            var retainedClaimCosts = attack.PhysicalCards.Where(card => !_coreCurrentUsePhysicalClaimsOperations.IsClaimedUseCardEntity(attack.ResolutionId, card.Id) && !((_coreKenshangPaymentsOperations.IsKenshangRemovedMaterial(attack.ResolutionId, card.Id) || _characterGuanHaiOlOperations.IsXieweiRemovedMaterial(attack.ResolutionId, card.Id)) || (_characterWuJingOlOperations.IsWuJingHejiRemovedMaterial(attack.ResolutionId, card.Id) || _characterSunYuanOlOperations.IsSayingRemovedMaterial(attack.ResolutionId, card.Id)))).Select(card => card.Id).ToHashSet();
            if (retainedClaimCosts.Count != attack.PhysicalCards.Count && processing.Count == retainedClaimCosts.Count + parentCount && processing.All(card => retainedParent && card.Id == borrowedSword.Card.Id || retainedClaimCosts.Contains(card.Id)))
                return true;
            var allAttackCardsInProcessing = attack.PhysicalCards.All(card => _cardZonesOperations._cardZones.GetLocation(card.Id) == CardLocation.Processing);
            if (allAttackCardsInProcessing)
            {
                return processing.Count == attack.PhysicalCards.Count + parentCount && processing.All(card => allowedIds.Contains(card.Id));
            }

            if (_resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().LastOrDefault()is { Continuation: ProgramCardContinuation.CompletedSlash } completed && completed.Candidates.Any(candidate => _contentRegistryOperations._contentRegistry.GetSkill(candidate.SkillId).Program?.Triggers.Where(trigger => trigger.Id == candidate.TriggerId).SelectMany(trigger => trigger.Effects).Any(effect => effect.Op == SkillProgramEffectOp.StoreTopCardInPublicPile) == true) && completed.ParentFrameId == attack.ResolutionId && _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(frame => frame.Id == attack.ResolutionId)is { Step: ResolutionFrameStep.Completed, Action: { } finishedAction } && completed.Action.ActionId == finishedAction.ActionId && completed.Action.ActorSeat == attack.CardUserSeat && completed.Action.PhysicalCards.Select(cost => cost.CardId).SequenceEqual(attack.PhysicalCards.Select(card => card.Id)) && _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Any(frame => frame.Id == borrowedSword.ResolutionId && frame.CardKind == CardKind.BorrowedSword && frame.CardId == borrowedSword.Card.Id))
                return processing.Count == parentCount && (!retainedParent || processing[0].Id == borrowedSword.Card.Id) && attack.PhysicalCards.All(card => ((_coreKenshangPaymentsOperations.IsKenshangRemovedMaterial(attack.ResolutionId, card.Id) || _characterGuanHaiOlOperations.IsXieweiRemovedMaterial(attack.ResolutionId, card.Id)) || (_characterWuJingOlOperations.IsWuJingHejiRemovedMaterial(attack.ResolutionId, card.Id) || _characterSunYuanOlOperations.IsSayingRemovedMaterial(attack.ResolutionId, card.Id))) || _cardZonesOperations._cardZones.GetLocation(card.Id).Zone is CardZoneKind.DiscardPile or CardZoneKind.Hand or CardZoneKind.DrawPile);
            return _coreProgramAttackAttemptOperations.HasDamageTriggerForAttack(attack) && processing.All(card => allowedIds.Contains(card.Id)) && attack.PhysicalCards.All(card => ((_coreKenshangPaymentsOperations.IsKenshangRemovedMaterial(attack.ResolutionId, card.Id) || _characterGuanHaiOlOperations.IsXieweiRemovedMaterial(attack.ResolutionId, card.Id)) || (_characterWuJingOlOperations.IsWuJingHejiRemovedMaterial(attack.ResolutionId, card.Id) || _characterSunYuanOlOperations.IsSayingRemovedMaterial(attack.ResolutionId, card.Id))) || _cardZonesOperations._cardZones.GetLocation(card.Id).Zone is CardZoneKind.Processing or CardZoneKind.DrawPile or CardZoneKind.Hand or CardZoneKind.DiscardPile);
        }

        if (_coreNativeCardContinuationCursorsOperations.ActiveFangtianHalberd is { CurrentAttack: { } fangtianAttack } fangtian && global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(fangtianAttack, attack))
        {
            if (_coreNextActualUseTargetAdjustmentsOperations.IsExactNextActualUseMaterialSlash(fangtian, attack, processing))
                return true;
            if (_coreGuanHaiBridgesOperations.IsXieweiMultiTargetAttack(fangtian, attack, processing))
                return true;
            if (_characterHaoMengAnniversaryOperations.IsHaoMengMaterialSlashAttack(fangtian, attack, processing))
                return true;
            var fangtianCardLocation = _cardZonesOperations._cardZones.GetLocation(fangtian.Card.Id);
            return attack.ResolutionId == fangtian.ResolutionId && (attack.SourceSeat == fangtian.SourceSeat && attack.EffectiveCardKind == fangtian.EffectiveCardKind || _coreNativeSlashTargetDuelOperations.IsNativeSlashTargetDuelAttack(attack)) && attack.PhysicalCards.Count == 1 && attack.PhysicalCards[0].Id == fangtian.Card.Id && (fangtianCardLocation.Zone is CardZoneKind.Processing or CardZoneKind.Hand or CardZoneKind.DrawPile or CardZoneKind.DiscardPile || _coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial(attack.ResolutionId, fangtian.Card.Id)) && processing.All(card => card.Id == fangtian.Card.Id);
        }

        var attackCardIds = attack.PhysicalCards.Select(card => card.Id).ToHashSet();
        if (_resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().LastOrDefault()is { Continuation: ProgramCardContinuation.CompletedSlash, Action.ActionId: var completedActionId } && _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault(frame => frame.Id == attack.ResolutionId)?.Action?.ActionId == completedActionId)
        {
            if (_characterLvKuangLvXiangOlOperations.IsQigongCompletedAttackProcessing(attack, processing))
                return true;
            return processing.Count == 0 && attack.PhysicalCards.All(card => ((_coreHellSoulCardsOperations.IsHellDestroyedSoulMaterial(attack.ResolutionId, card.Id) || (_characterHuBanOlOperations.IsHuiyunOutsideGeneralWeaponMaterial(attack.ResolutionId, card.Id) || _characterMiHengLeOperations.IsSiruiOutsideGeneralWeaponMaterial(attack.ResolutionId, card.Id)) || (_coreGuanduEquipmentOperations.IsGuanduDestroyedChariotMaterial(attack.ResolutionId, card.Id) || _coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial(attack.ResolutionId, card.Id))) || _coreForeignSelectedCardSlashReturnsOperations.IsSelectedForeignCardSlashRemovedMaterial(attack.ResolutionId, card.Id) || _characterZhangYanOlOperations.IsZhangYanSujiRemovedMaterial(attack.ResolutionId, card.Id) || ((_coreKenshangPaymentsOperations.IsKenshangRemovedMaterial(attack.ResolutionId, card.Id) || _characterGuanHaiOlOperations.IsXieweiRemovedMaterial(attack.ResolutionId, card.Id)) || (_characterWuJingOlOperations.IsWuJingHejiRemovedMaterial(attack.ResolutionId, card.Id) || _characterSunYuanOlOperations.IsSayingRemovedMaterial(attack.ResolutionId, card.Id)))) || _cardZonesOperations._cardZones.GetLocation(card.Id).Zone is CardZoneKind.DiscardPile or CardZoneKind.Hand or CardZoneKind.DrawPile);
        }

        // A completed gift can reuse the original use's entity in a distinct
        // recipient use. Its exact typed producer owns that current payment.
        if (_resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().Any(window => _coreCompletedGiftQinglongRidesOperations.TryGetCompletedGiftRecipientSlashRide(window, out var recipientUse) && recipientUse.Id == attack.ResolutionId) && processing.Count == attack.PhysicalCards.Count && processing.All(card => attackCardIds.Contains(card.Id)) && attack.PhysicalCards.All(card => _cardZonesOperations._cardZones.GetLocation(card.Id) == CardLocation.Processing))
            return true;
        // An outer multi-target use keeps its own card in Processing for the
        // whole resolution while an inner triggered resolution (such as a
        // nested attack) runs above its response window; its live frame on the
        // stack proves the card is held, not leaked. Once that frame pops, any
        // residue fails this invariant again.
        var heldByOuterUses = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Where(frame => frame.Id != attack.ResolutionId && frame.CardId != 0).Select(frame => frame.CardId).ToHashSet();
        var toleratedProcessing = processing.Where(card => !heldByOuterUses.Contains(card.Id)).ToArray();
        var retainedAttackIds = attack.PhysicalCards.Where(card => !_coreCurrentUsePhysicalClaimsOperations.IsClaimedUseCardEntity(attack.ResolutionId, card.Id) && (!_coreHellSoulCardsOperations.IsHellDestroyedSoulMaterial(attack.ResolutionId, card.Id) && !(_characterHuBanOlOperations.IsHuiyunOutsideGeneralWeaponMaterial(attack.ResolutionId, card.Id) || _characterMiHengLeOperations.IsSiruiOutsideGeneralWeaponMaterial(attack.ResolutionId, card.Id)) && !(_coreGuanduEquipmentOperations.IsGuanduDestroyedChariotMaterial(attack.ResolutionId, card.Id) || _coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial(attack.ResolutionId, card.Id))) && !(_coreForeignSelectedCardSlashReturnsOperations.IsSelectedForeignCardSlashRemovedMaterial(attack.ResolutionId, card.Id) || _characterZhangYanOlOperations.IsZhangYanSujiRemovedMaterial(attack.ResolutionId, card.Id) || ((_coreKenshangPaymentsOperations.IsKenshangRemovedMaterial(attack.ResolutionId, card.Id) || _characterGuanHaiOlOperations.IsXieweiRemovedMaterial(attack.ResolutionId, card.Id)) || (_characterWuJingOlOperations.IsWuJingHejiRemovedMaterial(attack.ResolutionId, card.Id) || _characterSunYuanOlOperations.IsSayingRemovedMaterial(attack.ResolutionId, card.Id))))).Select(card => card.Id).ToHashSet();
        if (retainedAttackIds.Count != attack.PhysicalCards.Count && toleratedProcessing.Select(card => card.Id).ToHashSet().SetEquals(retainedAttackIds))
            return true;
        if (toleratedProcessing.Length == attack.PhysicalCards.Count && toleratedProcessing.All(card => attackCardIds.Contains(card.Id)))
        {
            return true;
        }

        if (_coreNativeCardContinuationCursorsOperations.ActiveQinglongFollowup is { } qinglongSuspension && qinglongSuspension.OuterResolutionId != attack.ResolutionId)
        {
            // A Qinglong Crescent Blade follow-up runs nested while its parent
            // attack is suspended with targets still awaiting their dodge
            // windows, so the parent's physical cards legitimately remain in
            // Processing until the parent resumes.
            var suspendedParentIds = qinglongSuspension.NextAttack?.PhysicalCards.Select(card => card.Id).ToHashSet() ?? new HashSet<int>();
            return processing.All(card => attackCardIds.Contains(card.Id) || suspendedParentIds.Contains(card.Id)) && attack.PhysicalCards.All(card => ((_coreKenshangPaymentsOperations.IsKenshangRemovedMaterial(attack.ResolutionId, card.Id) || _characterGuanHaiOlOperations.IsXieweiRemovedMaterial(attack.ResolutionId, card.Id)) || (_characterWuJingOlOperations.IsWuJingHejiRemovedMaterial(attack.ResolutionId, card.Id) || _characterSunYuanOlOperations.IsSayingRemovedMaterial(attack.ResolutionId, card.Id))) || _cardZonesOperations._cardZones.GetLocation(card.Id).Zone is CardZoneKind.Processing or CardZoneKind.DrawPile or CardZoneKind.Hand or CardZoneKind.DiscardPile);
        }

        if (_coreProgramAttackAttemptOperations.HasDamageTriggerForAttack(attack))
        {
            return processing.All(card => attackCardIds.Contains(card.Id)) && attack.PhysicalCards.All(card => _cardZonesOperations._cardZones.GetLocation(card.Id).Zone is CardZoneKind.Processing or CardZoneKind.DrawPile or CardZoneKind.Hand or CardZoneKind.DiscardPile);
        }

        if (attack.IsChainPropagation)
        {
            var propagatedLocation = _cardZonesOperations._cardZones.GetLocation(attackCard.Id);
            return propagatedLocation.Zone is CardZoneKind.DrawPile or CardZoneKind.Hand or CardZoneKind.DiscardPile;
        }

        if (_coreDyingCursorRuntimeOperations.ActiveDying is not null)
        {
            var dyingLocation = _cardZonesOperations._cardZones.GetLocation(attackCard.Id);
            return dyingLocation.Zone is CardZoneKind.DrawPile or CardZoneKind.Hand or CardZoneKind.DiscardPile;
        }

        if (_coreNativeCardContinuationCursorsOperations.ActiveGroupCard is not { Effect: GroupCardEffect.ResponseAttack } group || processing.Count != 0)
        {
            return false;
        }

        var location = _cardZonesOperations._cardZones.GetLocation(attackCard.Id);
        // The effect continues after a target claims its physical card. That target can
        // subsequently die, discard it, or move it again before the next target responds.
        return group.DamageClaimedPhysicalCardIds.Contains(attackCard.Id) && location.Zone is CardZoneKind.Hand or CardZoneKind.DiscardPile or CardZoneKind.DrawPile;
    }

    internal static bool IsResolvedGroupPhysicalCardDestinationAllowed(CardLocation location, bool damageCardClaimed, IReadOnlyList<int> targetSeats) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_CommonRules.IsResolvedGroupPhysicalCardDestinationAllowed(location, damageCardClaimed, targetSeats);
    internal static bool RecordClaimedGroupPhysicalCard(ISet<int> claimedCardIds, long groupResolutionId, IReadOnlyList<Card> groupPhysicalCards, long attackResolutionId, int claimedCardId) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_CommonRules.RecordClaimedGroupPhysicalCard(claimedCardIds, groupResolutionId, groupPhysicalCards, attackResolutionId, claimedCardId);
}
