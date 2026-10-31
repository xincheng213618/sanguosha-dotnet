#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDamageColorDuelProgramsRuntime
{
    internal bool CanIssueSelectedActorDuel(int ownerSeat, int actorSeat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(ownerSeat) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(actorSeat) && ownerSeat != actorSeat && _coreMatchParticipantsPlayersAccess._players[ownerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[actorSeat].IsAlive && !_coreCardUseModulesOperations.IsCardUseForbidden(actorSeat, CardKind.Duel, CardActionType.Use) && _coreProgramTargetRulesOperations.TargetRules.AllowsNativeUseMaterial(new(actorSeat, CardKind.Duel, [])) && !_coreCardUseModulesOperations.HasTurnCardTargetRestriction(actorSeat, SkillProgramCardTargetRestriction.SelfOnly) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[ownerSeat], CardKind.Duel, Suit.None) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actorSeat, ownerSeat, CardKind.Duel);
    internal SkillProgramStepOutcome BeginSelectedActorDuel(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        var plan = ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!);
        if (_coreOrderedSelectedActorDuelsOperations.IsOrderedSelectedActorDuelProgram(f))
            return _coreOrderedSelectedActorDuelsOperations.BeginOrderedSelectedActorDuel(f);
        var forward = plan.Instructions.Count == 1 && OrderedSelectedActorDuelContract.IsOwnerToSelected(plan.Instructions[0]);
        if (f.TriggerId is not null || f.WindowContext is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats is not [var actor] || plan.Instructions.Count != 1 || plan.Instructions[0].Op != SkillProgramEffectOp.UseSelectedActorDuel || plan.Activation is not { MinCards: 0, MaxCards: 0, MinTargets: 1, MaxTargets: 1, UsesPerTurn: null } activation || (forward ? activation.UsesPerPhaseExpression != SkillProgramPhaseUsageExpression.CurrentRound || activation.UsesPerPhase is not null || _coreActualRoundProgressRoundNumberAccess._roundNumber < 1 : activation.UsesPerPhase != 2) || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat != f.OwnerSeat || _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not null || _coreNativeCardContinuationCursorsOperations.ActiveDuel is not null)
            throw new InvalidOperationException("Selected actor Duel lost its exact owning zero-card Play activation.");
        var target = forward ? actor : f.OwnerSeat;
        if (forward)
            actor = f.OwnerSeat;
        if (!CanIssueSelectedActorDuel(target, actor))
            return SkillProgramStepOutcome.Continue;
        var origin = new ProgramSelectedActorDuelOrigin(f.Id, f.InstructionIndex, f.OwnerSeat, actor, f.SkillId, f.ActivationId, f.SkillInstanceId, f.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber)
        {
            ParticipantMode = forward ? SelectedActorDuelParticipantMode.OwnerToSelectedTarget : null,
            InitialTargetSeat = forward ? target : null
        };
        var card = new Card(0, CardKind.Duel, Suit.None, 0);
        var id = _coreNativeCardUseOperations.BeginCardUse(card, actor, [target], CardKind.Duel, physicalCardIds: [], selectedActorDuelOrigin: origin);
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, actor, [target], LegalActionKind.Duel, playedCardKind: CardKind.Duel);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool IsSelectedActorDuelUse(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { SelectedActorDuelOrigin: not null, CardId: 0, CardKind: CardKind.Duel, PhysicalCardIds.Count: 0 };
    internal Card GetTrickRepresentation(long frameId, int cardId, bool requireProcessing = false) => cardId == 0 && _characterLvYiMobileShiOperations.IsShiLySongyanDrawTwo(frameId) ? new Card(0, CardKind.DrawTwo, Suit.None, 0) : cardId == 0 && _coreOriginalUseLightningOperations.IsOriginalUseLightning(frameId) ? new Card(0, CardKind.Lightning, Suit.None, 0) : cardId == 0 && _characterDongChengMouOperations.IsMouDcZeroUse(frameId) ? new Card(0, _coreCardUseLifecycleOperations.LifecycleCardUse(frameId)!.CardKind, Suit.None, 0) : cardId == 0 && _characterZhouYuMouOperations.IsMouZyZeroFireUse(frameId) ? new Card(0, CardKind.FireAttack, Suit.None, 0) : cardId == 0 && _characterDongZhuoMouOlOperations.IsMouDongZhuoOlWine(frameId) ? new Card(0, CardKind.Alcohol, Suit.None, 0) : cardId == 0 && _coreShanheSourceMouDongZhuoWineOperations.IsShanheSourceMouWine(frameId) ? new Card(0, CardKind.Alcohol, Suit.None, 0) : cardId == 0 && _coreWushuangSkillsBOperations.IsWushuangBZeroUse(frameId) ? new Card(0, _coreCardUseLifecycleOperations.LifecycleCardUse(frameId)!.CardKind, Suit.None, 0) : cardId == 0 && _characterXuHuangMouMobileOperations.IsMouXuHuangZeroUse(frameId) ? _characterXuHuangMouMobileOperations.MouXuHuangRepresentation(frameId) : cardId == 0 && _characterLiuChengMouMobileOperations.IsMouLiuChengUse(frameId) ? _characterLiuChengMouMobileOperations.MouLiuChengRepresentation(frameId) : cardId == 0 && _characterXiaoQiaoMouOlOperations.IsMouXqJueyanUse(frameId) ? _characterXiaoQiaoMouOlOperations.MouXqJueyanRepresentation(frameId) : cardId == 0 && _characterDiaoChanLeOperations.IsDiouUse(frameId) ? _characterDiaoChanLeOperations.DiouRepresentation(frameId) : cardId == 0 && _characterHuanShuJunOlOperations.IsHuanpeiUse(frameId) ? _characterHuanShuJunOlOperations.HuanpeiRepresentation(frameId) : cardId == 0 && _characterLiuZhangOlOperations.IsZonghuUse(frameId) ? _characterLiuZhangOlOperations.ZonghuRepresentation(frameId) : cardId == 0 && _characterGuoTuOlOperations.IsWeijieUse(frameId) ? _characterGuoTuOlOperations.WeijieRepresentation(frameId) : cardId == 0 && _characterLuKangWumiaoOperations.IsWuLuKangKegouUse(frameId) ? _characterLuKangWumiaoOperations.WuLuKangKegouRepresentation(frameId) : cardId == 0 && _characterHuCheErMouOperations.IsMouHuCheErUse(frameId) ? _characterHuCheErMouOperations.MouHuCheErRepresentation(frameId) : cardId == 0 && _characterZhangXiuMouOperations.IsMouZhangXiuAnniversaryUse(frameId) ? _characterZhangXiuMouOperations.MouZhangXiuAnniversaryRepresentation(frameId) : cardId == 0 && _characterHuLieMouOperations.IsMouHuLieUse(frameId) ? _characterHuLieMouOperations.MouHuLieRepresentation(frameId) : cardId == 0 && _characterGuanHaiOlOperations.IsYouqueUse(frameId) ? _characterGuanHaiOlOperations.YouqueRepresentation(frameId) : cardId == 0 && _characterTaoQianOlOperations.IsZongluanUse(frameId) ? _characterTaoQianOlOperations.ZongluanRepresentation(frameId) : cardId == 0 && (_characterLiuYanOlOperations.IsLiuYanLegacyAlcoholUse(frameId) || _characterCuiYanOlOperations.IsCuiYanLegacyAlcoholUse(frameId) || _characterHuangYueyingOlMouOperations.IsMouBingcaiLegacyAlcoholUse(frameId)) ? new Card(0, CardKind.Alcohol, Suit.None, 0) : cardId == 0 && _coreCardUseLifecycleOperations.LifecycleCardUse(frameId)is { CardKind: CardKind.Alcohol, LegacyDyingAlcoholReturn: not null } legacyWine && _coreActualTurnLegacyCompletionOperations.IsExactLegacyActualUseCompletion(legacyWine) ? new Card(0, CardKind.Alcohol, Suit.None, 0) : cardId == 0 && _coreCardUseLifecycleOperations.LifecycleCardUse(frameId)is { DyingSuitsPeachReturn: { } dyingReturn } dyingUse && _coreDyingSuitsAndHistoricalUseObserversOperations.ValidDyingSuitsPeachUse(dyingUse, dyingReturn) ? new Card(0, CardKind.Peach, Suit.None, 0) : cardId == 0 && _characterBaoSanNiangOlOperations.IsChosenAssaultUse(frameId) ? new Card(0, CardKind.BarbarianAssault, Suit.None, 0) : cardId == 0 && _characterAHuiNanOlOperations.IsAHuiNanJuemanUse(frameId) ? new Card(0, _coreCardUseLifecycleOperations.LifecycleCardUse(frameId)!.CardKind, Suit.None, 0) : cardId == 0 && _characterQinLangOlOperations.IsQinLangXianyingUse(frameId) ? new Card(0, _coreCardUseLifecycleOperations.LifecycleCardUse(frameId)!.CardKind, Suit.None, 0) : cardId == 0 && _characterZhugeLiangMouOperations.IsMouZhijueZeroUse(frameId) ? new Card(0, _coreCardUseLifecycleOperations.LifecycleCardUse(frameId)!.CardKind, Suit.None, 0) : cardId == 0 && _coreTieredRoundZeroUsesOperations.IsTieredRoundZeroUse(frameId) ? _coreTieredRoundZeroUsesOperations.TieredRoundZeroRepresentation(frameId) : cardId == 0 && _coreChainedStateBasicReturnsOperations.IsChainedStateBasicUse(frameId) ? _coreChainedStateBasicReturnsOperations.ChainedStateBasicRepresentation(frameId) : cardId == 0 && _coreDrawFundedDistinctBasicUsesOperations.IsDrawFundedDistinctBasicUse(frameId) ? _coreDrawFundedDistinctBasicUsesOperations.DrawFundedDistinctBasicRepresentation(frameId) : cardId == 0 && _characterTianChouOlOperations.IsTianChouShandaoUse(frameId) ? new Card(0, _coreCardUseLifecycleOperations.LifecycleCardUse(frameId)!.CardKind, Suit.None, 0) : cardId == 0 && _coreVirtualOrdinaryTrickUsesOperations.IsProgramVirtualOrdinaryTrickUse(frameId) ? new Card(0, _coreCardUseLifecycleOperations.LifecycleCardUse(frameId)!.CardKind, Suit.None, 0) : cardId == 0 && _coreDamageTargetObtainProgramsOperations.IsIssuedZeroEntityDuel(frameId) ? new Card(0, CardKind.Duel, Suit.None, 0) : cardId > 0 && _coreCardUseLifecycleOperations.LifecycleCardUse(frameId)is { DiouOrigin: { } diou } && diou.CarrierId == cardId && _characterDiaoChanLeOperations.IsDiouUse(frameId) ? _coreResponseEntityExchangeOperations.EntityAtCurrentLocation(cardId) : (_coreCurrentUsePhysicalClaimsOperations.IsClaimedUseCardEntity(frameId, cardId) || (_characterHuBanOlOperations.IsHuiyunOutsideGeneralWeaponMaterial(frameId, cardId) || (_characterMiHengLeOperations.IsSiruiOutsideGeneralWeaponMaterial(frameId, cardId) || _characterHuangYueyingOlMouOperations.IsMouLixianRemovedMaterial(frameId, cardId))) || _coreGuanduEquipmentOperations.IsGuanduDestroyedChariotMaterial(frameId, cardId) || _coreHellSoulCardsOperations.IsHellDestroyedSoulMaterial(frameId, cardId) || _coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial(frameId, cardId)) ? _coreResponseEntityExchangeOperations.EntityAtCurrentLocation(cardId) : cardId == 0 ? UnrepresentedZeroTrick(frameId) : _cardZonesOperations._cardZones.CardsAt(requireProcessing ? CardLocation.Processing : _cardZonesOperations._cardZones.GetLocation(cardId)).Single(c => c.Id == cardId);
    internal Card UnrepresentedZeroTrick(long frameId)
    {
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(frameId);
        var origin = use?.JueshengDuel;
        var action = use?.Action;
        var parent = origin is null ? null : _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(frame => frame.Id == origin.ProgramFrameId);
        var at = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == frameId);
        throw new InvalidOperationException("A zero-entity trick lost its native representation. " + System.Text.Json.JsonSerializer.Serialize(new { UseId = frameId, use?.CardId, use?.CardKind, PhysicalCount = use?.PhysicalCardIds?.Count, use?.SourceSeat, Origin = origin, CurrentTurn = _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, Phase = _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase.ToString(), Winner = _coreMatchOutcomeWinnerAccess._winner.ToString(), Action = action is null ? null : new { action.ActionId, action.Type, action.ActorSeat, action.ProviderSeat, action.EffectiveKind, action.EffectiveSuit, action.EffectiveRank, action.EffectiveIsRed, PhysicalCount = action.PhysicalCards.Count, ConversionCount = action.ConversionChain.Count }, Parent = parent is null ? null : new { parent.Id, parent.OwnerSeat, parent.SkillId, parent.SkillInstanceId, parent.GameplayHash, parent.InstructionIndex, Receipt = parent.Juesheng }, ImmediateParentMatches = at > 0 && _resolutionStackOperations._resolutionStack[at - 1].Id == parent?.Id, OriginUseMatches = origin?.UseId == frameId, ActorMatches = use is not null && origin is not null && _characterFanJiangZhangDaOlOperations.JueshengActorMatches(use, origin), IssuedCount = origin is null ? 0 : _coreProgramEventHistoryOperations.ProgramEventHistory<JueshengDuelIssuedEvent>().Count(e => e.Origin == origin && e.InstructionIndex == 1), HuangFuSongRankCapture = _characterHuangfuSongWumiaoOperations.HasWuHuangFuSongCapability }));
    }

    internal bool MatchesSelectedActorDuelAction(long frameId, CardActionContext action, int cardId) => cardId == 0 && _coreDamageTargetObtainProgramsOperations.IsIssuedZeroEntityDuel(frameId) && _coreCardUseLifecycleOperations.LifecycleCardUse(frameId)?.Action?.ActionId == action.ActionId && action.Type == CardActionType.Use && action.EffectiveKind == CardKind.Duel && action.PhysicalCards.Count == 0 && action.ConversionChain.Count == 0;
    [NativeUseReturn(200)]
    internal void ReturnSelectedActorDuel(CardUseFrame use)
    {
        if (use.SelectedActorDuelOrigin is not { AttackStarted: false } origin || origin.MouDiaoChanRing is not null)
            return;
        if (origin.OrderedPairPayment is not null)
        {
            if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame orderedParent)
                throw new InvalidOperationException("Ordered Duel lost its original paid parent.");
            _coreOrderedSelectedActorDuelsOperations.CompleteOrderedSelectedActorDuel(orderedParent, origin, use.Id);
            return;
        }

        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != origin.ParentProgramFrameId || f.InstructionIndex != origin.InstructionIndex || f.OwnerSeat != origin.OwnerSeat || f.SkillId != origin.SkillId || f.ActivationId != origin.ActivationId || f.SkillInstanceId != origin.SkillInstanceId || f.GameplayHash != origin.GameplayHash)
            throw new InvalidOperationException("Selected actor Duel lost its typed Program return.");
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal void AssertSelectedActorDuels()
    {
        foreach (var use in _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Where(u => u.SelectedActorDuelOrigin is not null))
        {
            var o = use.SelectedActorDuelOrigin!;
            if (o.MouDiaoChanRing is not null)
            {
                _characterDiaoChanMouMobileOperations.AssertMouDiaoChanRingUse(use, o);
                continue;
            }

            if (o.XiahouEnShiwu is not null)
            {
                _characterXiahouEnAnniversaryOperations.AssertXiahouEnShiwuDuelUse(use, o);
                continue;
            }

            if (o.CaiYangXunji is not null)
            {
                _characterCaiYangAnniversaryOperations.AssertCaiYangXunjiDuelUse(use, o);
                continue;
            }

            if (o.ShanheSourceXiechan is not null)
            {
                _coreShanheSourceLiuYanTrioOperations.AssertSourceXiechanDuelUse(use, o);
                continue;
            }

            var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == use.Id);
            if (o.OrderedPairPayment is not null)
            {
                if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramSkillFrame orderedParent)
                    throw new InvalidOperationException("Ordered Duel lost its direct owning program edge.");
                _coreOrderedSelectedActorDuelsOperations.AssertOrderedSelectedActorDuelUse(use, orderedParent, o);
                continue;
            }

            if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramSkillFrame f || f.Id != o.ParentProgramFrameId || f.OwnerSeat != o.OwnerSeat || f.SkillId != o.SkillId || f.ActivationId != o.ActivationId || f.SkillInstanceId != o.SkillInstanceId || f.GameplayHash != o.GameplayHash || f.InstructionIndex != o.InstructionIndex || o.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats is not [var selectedParticipant] || (o.ParticipantMode == SelectedActorDuelParticipantMode.OwnerToSelectedTarget ? o.InitialActorSeat != f.OwnerSeat || o.InitialTargetSeat != selectedParticipant || selectedParticipant == f.OwnerSeat || !OrderedSelectedActorDuelContract.IsOwnerToSelected(ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).Instructions[0]) : o.ParticipantMode is not null || o.InitialTargetSeat is not null || selectedParticipant != o.InitialActorSeat || o.OwnerSeat == o.InitialActorSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(o.InitialActorSeat) || o.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || use.CardId != 0 || use.CardKind != CardKind.Duel || use.PhysicalCardIds?.Count != 0 || use.Action is not { Type: CardActionType.Use, EffectiveKind: CardKind.Duel, PhysicalCards.Count: 0, ConversionChain.Count: 0, EffectiveSuit: Suit.None, EffectiveIsRed: false } || ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).Instructions is not [{ Op: SkillProgramEffectOp.UseSelectedActorDuel }])
                throw new InvalidOperationException("Selected actor Duel has an invalid issued origin or typed owner.");
        // Initial actor/target are issuance facts. Mature action rewrites need not keep these initial seats.
        }
    }

    internal SkillProgramStepOutcome BeginDamageAppearanceDraw(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.DamageAppearanceReceipt is not null || f.InstructionIndex != 1 || f.WindowContext is not { Window: SkillProgramTriggerWindow.DamageAppliedBeforeDying } context || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not DamageTriggerWindowFrame window || window.Id != context.ParentFrameId || window.TriggerWindow != context.Window || window.ParentFrameId != context.DamageFrameId || window.TargetSeat != f.OwnerSeat || context.TargetSeat != f.OwnerSeat)
            throw new InvalidOperationException("Damage appearance Draw requires the exact applied damage target window.");
        var attempt = _coreNativeDamageHandlesOperations.GetDamageTriggerAttack(window);
        if (!attempt.DamageWasApplied || attempt.DamageAmount <= 0 || attempt.TargetSeat != f.OwnerSeat)
            throw new InvalidOperationException("Damage appearance Draw requires positive actual applied damage.");
        var action = _coreCardUseLifecycleOperations.LifecycleCardUse(attempt.ResolutionId)?.Action;
        var hasCard = action is not null || attempt.EffectiveCardKind is not null || attempt.Card is not null;
        var red = hasCard && (action is not null ? action.EffectiveIsRed ?? (action.EffectiveSuit is Suit.Heart or Suit.Diamond) : attempt.Card?.Suit is Suit.Heart or Suit.Diamond);
        var source = attempt.IsSourceLess || attempt.IsDelayedJudgmentDamage ? (int? )null : attempt.SourceSeat;
        var recipient = hasCard ? red ? source : f.OwnerSeat : null;
        var draw = recipient is { } seat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { DamageAppearanceReceipt = new(f.InstructionIndex, window.Id, window.ParentFrameId, f.OwnerSeat, source, action?.EffectiveKind ?? attempt.EffectiveCardKind ?? attempt.Card?.Kind, hasCard, red, recipient, draw) });
        if (draw)
            _coreProgramLifecycleOperations.DrawProgramCards(f.Id, recipient!.Value, 1, null, null, SkillProgramCardSetVisibility.Private, CardMoveReasons.Draw);
        return SkillProgramStepOutcome.Continue;
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 600)]
    internal void AssertDamageAppearanceReceipt(ProgramSkillFrame f, ProgramExecutionPlan plan)
    {
        if (f.DamageAppearanceReceipt is not { } p)
            return;
        if (f.TriggerId is null || plan.Instructions is not [{ Op: SkillProgramEffectOp.DrawByDamageCardColor }] || f.InstructionIndex != 1 || p.InstructionIndex != 1 || p.OwnerSeat != f.OwnerSeat || f.WindowContext is not { Window: SkillProgramTriggerWindow.DamageAppliedBeforeDying } c || c.ParentFrameId != p.DamageWindowFrameId || c.DamageFrameId != p.DamageFrameId || c.OwnerSeat != p.OwnerSeat || c.TargetSeat != p.OwnerSeat || c.SourceSeat != p.SourceSeat || c.Amount <= 0 || !_resolutionStackOperations._resolutionStack.OfType<DamageTriggerWindowFrame>().Any(w => w.Id == p.DamageWindowFrameId && w.ParentFrameId == p.DamageFrameId && w.TargetSeat == p.OwnerSeat && w.TriggerWindow == c.Window) || p.HasCard != (p.EffectiveCardKind is not null) || p.IsRed && !p.HasCard || p.RecipientSeat != (p.HasCard ? p.IsRed ? p.SourceSeat : p.OwnerSeat : null) || p.SourceSeat is { } source && !_coreTableSeatQueriesOperations.IsValidPlayerSeat(source) || p.DrawIssued && p.RecipientSeat is null)
            throw new InvalidOperationException("Damage appearance receipt lost its exact producer, source or finite claim.");
    }

    internal ProgramSkillFrame? DamageAppearanceDrawObserverRoot(long windowId)
    {
        for (var index = 1; index + 1 < _resolutionStackOperations._resolutionStack.Count; index++)
        {
            if (_resolutionStackOperations._resolutionStack[index] is not ProgramSkillFrame f || f.DamageAppearanceReceipt is not { DrawIssued: true, RecipientSeat: { } recipient } p || f.WindowContext is not { Window: SkillProgramTriggerWindow.DamageAppliedBeforeDying } c || c.ParentFrameId != windowId || p.DamageWindowFrameId != windowId || p.DamageFrameId != c.DamageFrameId || p.OwnerSeat != f.OwnerSeat || c.TargetSeat != f.OwnerSeat || _resolutionStackOperations._resolutionStack[index - 1] is not DamageTriggerWindowFrame w || w.Id != windowId || w.ParentFrameId != p.DamageFrameId || w.TargetSeat != f.OwnerSeat || f.InstructionIndex != 1 || p.InstructionIndex != 1 || ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).Instructions is not [{ Op: SkillProgramEffectOp.DrawByDamageCardColor }] || _resolutionStackOperations._resolutionStack[index + 1] is not CardsMovedTriggerWindowFrame movement || movement.Batch.ParentFrameId != f.Id || movement.Batch.AwaitingProgramFrameId is { } awaiting && awaiting != f.Id || movement.Batch.OriginOwnerSeat != f.OwnerSeat || movement.Batch.OriginSkillId != f.SkillId || movement.Batch.OriginSkillInstanceId != f.SkillInstanceId || movement.Batch.Movements is not [var actual] || actual.From != CardLocation.DrawPile || actual.To != CardLocation.Hand(recipient) || actual.Reason != CardMoveReasons.Draw || !_coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m == actual))
                continue;
            return f;
        }

        return null;
    }

    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 50)]
    internal bool HasDamageAppearanceDrawObserver(long windowId)
    {
        if (DamageAppearanceDrawObserverRoot(windowId)is not { } root)
            return false;
        var index = _resolutionStackOperations._resolutionStack.Count - 1;
        while (index >= 1 && _resolutionStackOperations._resolutionStack[index].Id != root.Id && (_coreDamageObserverCursorQueriesOperations.DamageFrameRidesOn(_resolutionStackOperations._resolutionStack[index], _resolutionStackOperations._resolutionStack[index - 1]) || global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.DamageObserverRidesOn(_resolutionStackOperations._resolutionStack[index], _resolutionStackOperations._resolutionStack[index - 1]) || _corePublicPileEquipmentSequencesOperations.PileEquipmentFrameRidesOn(_resolutionStackOperations._resolutionStack[index], _resolutionStackOperations._resolutionStack[index - 1]) || _coreDeckEquipmentProgramsOperations.RandomEquipmentFrameRidesOn(_resolutionStackOperations._resolutionStack[index], _resolutionStackOperations._resolutionStack[index - 1])))
            index--;
        return _resolutionStackOperations._resolutionStack[index].Id == root.Id;
    }

    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 320, group: 10)]
    internal bool IsDamageAppearanceDrawProgramDying()
    {
        if (_coreDamageCursorRuntimeOperations.ActiveDamageTrigger is not { } trigger || _coreDyingCursorRuntimeOperations.ActiveDying is not { ResumesProgramSkill: true } dying || DamageAppearanceDrawObserverRoot(trigger.Id)is not { } root || _resolutionStackOperations._resolutionStack.OfType<DyingFrame>().SingleOrDefault(frame => frame.Id == dying.FrameId)is not { } dyingFrame || dyingFrame.ParentFrameId != dying.ParentFrameId)
            return false;
        var parentIndex = -1;
        for (var index = 0; index < _resolutionStackOperations._resolutionStack.Count; index++)
            if (_resolutionStackOperations._resolutionStack[index] is ProgramSkillFrame parent && parent.Id == dying.ParentFrameId)
                parentIndex = index;
        if (parentIndex < 0 || parentIndex + 1 >= _resolutionStackOperations._resolutionStack.Count || _resolutionStackOperations._resolutionStack[parentIndex + 1].Id != dyingFrame.Id)
            return false;
        var top = _resolutionStackOperations._resolutionStack.LastOrDefault();
        if (!(top is DyingFrame topDying && topDying.Id == dyingFrame.Id || top is ProgramSkillFrame { WindowContext: { } response } && response.ParentFrameId == dyingFrame.Id && response.Window is SkillProgramTriggerWindow.DyingResponse or SkillProgramTriggerWindow.SelfDyingResponse || _coreAvailableBoundSubsetProgramsOperations.IsAvailableBoundPeachRescueRide(parentIndex + 1, dyingFrame)))
            return false;
        while (parentIndex >= 1 && (_coreDamageObserverCursorQueriesOperations.DamageFrameRidesOn(_resolutionStackOperations._resolutionStack[parentIndex], _resolutionStackOperations._resolutionStack[parentIndex - 1]) || global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.DamageObserverRidesOn(_resolutionStackOperations._resolutionStack[parentIndex], _resolutionStackOperations._resolutionStack[parentIndex - 1]) || _corePublicPileEquipmentSequencesOperations.PileEquipmentFrameRidesOn(_resolutionStackOperations._resolutionStack[parentIndex], _resolutionStackOperations._resolutionStack[parentIndex - 1]) || _coreDeckEquipmentProgramsOperations.RandomEquipmentFrameRidesOn(_resolutionStackOperations._resolutionStack[parentIndex], _resolutionStackOperations._resolutionStack[parentIndex - 1])))
            parentIndex--;
        return _resolutionStackOperations._resolutionStack[parentIndex].Id == root.Id;
    }

    SkillProgramStepOutcome global::CardGame.Core.IDamageColorDuelProgramHost.DrawByDamageCardColor(ProgramSkillFrame frame) => BeginDamageAppearanceDraw(frame);
    SkillProgramStepOutcome global::CardGame.Core.IDamageColorDuelProgramHost.UseSelectedActorDuel(ProgramSkillFrame frame) => BeginSelectedActorDuel(frame);
}
