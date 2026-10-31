#nullable enable

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
    internal Card GetTrickRepresentation(long frameId, int cardId, bool requireProcessing = false) => cardId == 0 && _characterLvYiMobileShiOperations.Value.IsShiLySongyanDrawTwo(frameId) ? new Card(0, CardKind.DrawTwo, Suit.None, 0) : cardId == 0 && _coreOriginalUseLightningOperations.IsOriginalUseLightning(frameId) ? new Card(0, CardKind.Lightning, Suit.None, 0) : cardId == 0 && _characterDongChengMouOperations.IsMouDcZeroUse(frameId) ? new Card(0, _coreCardUseLifecycleOperations.LifecycleCardUse(frameId)!.CardKind, Suit.None, 0) : cardId == 0 && _characterZhouYuMouOperations.IsMouZyZeroFireUse(frameId) ? new Card(0, CardKind.FireAttack, Suit.None, 0) : cardId == 0 && _characterDongZhuoMouOlOperations.IsMouDongZhuoOlWine(frameId) ? new Card(0, CardKind.Alcohol, Suit.None, 0) : cardId == 0 && _coreShanheSourceMouDongZhuoWineOperations.IsShanheSourceMouWine(frameId) ? new Card(0, CardKind.Alcohol, Suit.None, 0) : cardId == 0 && _coreWushuangSkillsBOperations.IsWushuangBZeroUse(frameId) ? new Card(0, _coreCardUseLifecycleOperations.LifecycleCardUse(frameId)!.CardKind, Suit.None, 0) : cardId == 0 && _characterXuHuangMouMobileOperations.IsMouXuHuangZeroUse(frameId) ? _characterXuHuangMouMobileOperations.MouXuHuangRepresentation(frameId) : cardId == 0 && _characterLiuChengMouMobileOperations.IsMouLiuChengUse(frameId) ? _characterLiuChengMouMobileOperations.MouLiuChengRepresentation(frameId) : cardId == 0 && _characterXiaoQiaoMouOlOperations.IsMouXqJueyanUse(frameId) ? _characterXiaoQiaoMouOlOperations.MouXqJueyanRepresentation(frameId) : cardId == 0 && _characterDiaoChanLeOperations.IsDiouUse(frameId) ? _characterDiaoChanLeOperations.DiouRepresentation(frameId) : cardId == 0 && _characterHuanShuJunOlOperations.IsHuanpeiUse(frameId) ? _characterHuanShuJunOlOperations.HuanpeiRepresentation(frameId) : cardId == 0 && _characterLiuZhangOlOperations.IsZonghuUse(frameId) ? _characterLiuZhangOlOperations.ZonghuRepresentation(frameId) : cardId == 0 && _characterGuoTuOlOperations.IsWeijieUse(frameId) ? _characterGuoTuOlOperations.WeijieRepresentation(frameId) : cardId == 0 && _characterLuKangWumiaoOperations.IsWuLuKangKegouUse(frameId) ? _characterLuKangWumiaoOperations.WuLuKangKegouRepresentation(frameId) : cardId == 0 && _characterHuCheErMouOperations.IsMouHuCheErUse(frameId) ? _characterHuCheErMouOperations.MouHuCheErRepresentation(frameId) : cardId == 0 && _characterZhangXiuMouOperations.IsMouZhangXiuAnniversaryUse(frameId) ? _characterZhangXiuMouOperations.MouZhangXiuAnniversaryRepresentation(frameId) : cardId == 0 && _characterHuLieMouOperations.IsMouHuLieUse(frameId) ? _characterHuLieMouOperations.MouHuLieRepresentation(frameId) : cardId == 0 && _characterGuanHaiOlOperations.IsYouqueUse(frameId) ? _characterGuanHaiOlOperations.YouqueRepresentation(frameId) : cardId == 0 && _characterTaoQianOlOperations.IsZongluanUse(frameId) ? _characterTaoQianOlOperations.ZongluanRepresentation(frameId) : cardId == 0 && (_characterLiuYanOlOperations.IsLiuYanLegacyAlcoholUse(frameId) || _characterCuiYanOlOperations.IsCuiYanLegacyAlcoholUse(frameId) || _characterHuangYueyingOlMouOperations.IsMouBingcaiLegacyAlcoholUse(frameId)) ? new Card(0, CardKind.Alcohol, Suit.None, 0) : cardId == 0 && _coreCardUseLifecycleOperations.LifecycleCardUse(frameId)is { CardKind: CardKind.Alcohol, LegacyDyingAlcoholReturn: not null } legacyWine && _coreActualTurnLegacyCompletionOperations.IsExactLegacyActualUseCompletion(legacyWine) ? new Card(0, CardKind.Alcohol, Suit.None, 0) : cardId == 0 && _coreCardUseLifecycleOperations.LifecycleCardUse(frameId)is { DyingSuitsPeachReturn: { } dyingReturn } dyingUse && _coreDyingSuitsAndHistoricalUseObserversOperations.ValidDyingSuitsPeachUse(dyingUse, dyingReturn) ? new Card(0, CardKind.Peach, Suit.None, 0) : cardId == 0 && _characterBaoSanNiangOlOperations.IsChosenAssaultUse(frameId) ? new Card(0, CardKind.BarbarianAssault, Suit.None, 0) : cardId == 0 && _characterAHuiNanOlOperations.Value.IsAHuiNanJuemanUse(frameId) ? new Card(0, _coreCardUseLifecycleOperations.LifecycleCardUse(frameId)!.CardKind, Suit.None, 0) : cardId == 0 && _characterQinLangOlOperations.IsQinLangXianyingUse(frameId) ? new Card(0, _coreCardUseLifecycleOperations.LifecycleCardUse(frameId)!.CardKind, Suit.None, 0) : cardId == 0 && _characterZhugeLiangMouOperations.IsMouZhijueZeroUse(frameId) ? new Card(0, _coreCardUseLifecycleOperations.LifecycleCardUse(frameId)!.CardKind, Suit.None, 0) : cardId == 0 && _coreTieredRoundZeroUsesOperations.IsTieredRoundZeroUse(frameId) ? _coreTieredRoundZeroUsesOperations.TieredRoundZeroRepresentation(frameId) : cardId == 0 && _coreChainedStateBasicReturnsOperations.IsChainedStateBasicUse(frameId) ? _coreChainedStateBasicReturnsOperations.ChainedStateBasicRepresentation(frameId) : cardId == 0 && _coreDrawFundedDistinctBasicUsesOperations.IsDrawFundedDistinctBasicUse(frameId) ? _coreDrawFundedDistinctBasicUsesOperations.DrawFundedDistinctBasicRepresentation(frameId) : cardId == 0 && _characterTianChouOlOperations.IsTianChouShandaoUse(frameId) ? new Card(0, _coreCardUseLifecycleOperations.LifecycleCardUse(frameId)!.CardKind, Suit.None, 0) : cardId == 0 && _coreVirtualOrdinaryTrickUsesOperations.IsProgramVirtualOrdinaryTrickUse(frameId) ? new Card(0, _coreCardUseLifecycleOperations.LifecycleCardUse(frameId)!.CardKind, Suit.None, 0) : cardId == 0 && _coreDamageTargetObtainProgramsOperations.IsIssuedZeroEntityDuel(frameId) ? new Card(0, CardKind.Duel, Suit.None, 0) : cardId > 0 && _coreCardUseLifecycleOperations.LifecycleCardUse(frameId)is { DiouOrigin: { } diou } && diou.CarrierId == cardId && _characterDiaoChanLeOperations.IsDiouUse(frameId) ? _coreResponseEntityExchangeOperations.EntityAtCurrentLocation(cardId) : (_coreCurrentUsePhysicalClaimsOperations.IsClaimedUseCardEntity(frameId, cardId) || (_characterHuBanOlOperations.IsHuiyunOutsideGeneralWeaponMaterial(frameId, cardId) || (_characterMiHengLeOperations.IsSiruiOutsideGeneralWeaponMaterial(frameId, cardId) || _characterHuangYueyingOlMouOperations.IsMouLixianRemovedMaterial(frameId, cardId))) || _coreGuanduEquipmentOperations.IsGuanduDestroyedChariotMaterial(frameId, cardId) || _coreHellSoulCardsOperations.IsHellDestroyedSoulMaterial(frameId, cardId) || _coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial3(frameId, cardId)) ? _coreResponseEntityExchangeOperations.EntityAtCurrentLocation(cardId) : cardId == 0 ? UnrepresentedZeroTrick(frameId) : _cardZonesOperations._cardZones.CardsAt(requireProcessing ? CardLocation.Processing : _cardZonesOperations._cardZones.GetLocation(cardId)).Single(c => c.Id == cardId);
    internal Card UnrepresentedZeroTrick(long frameId)
    {
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(frameId);
        var origin = use?.JueshengDuel;
        var action = use?.Action;
        var parent = origin is null ? null : _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(frame => frame.Id == origin.ProgramFrameId);
        var at = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == frameId);
        throw new InvalidOperationException("A zero-entity trick lost its native representation. " + System.Text.Json.JsonSerializer.Serialize(new { UseId = frameId, use?.CardId, use?.CardKind, PhysicalCount = use?.PhysicalCardIds?.Count, use?.SourceSeat, Origin = origin, CurrentTurn = _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, Phase = _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase.ToString(), Winner = _coreMatchOutcomeWinnerAccess._winner.ToString(), Action = action is null ? null : new { action.ActionId, action.Type, action.ActorSeat, action.ProviderSeat, action.EffectiveKind, action.EffectiveSuit, action.EffectiveRank, action.EffectiveIsRed, PhysicalCount = action.PhysicalCards.Count, ConversionCount = action.ConversionChain.Count }, Parent = parent is null ? null : new { parent.Id, parent.OwnerSeat, parent.SkillId, parent.SkillInstanceId, parent.GameplayHash, parent.InstructionIndex, Receipt = parent.Juesheng }, ImmediateParentMatches = at > 0 && _resolutionStackOperations._resolutionStack[at - 1].Id == parent?.Id, OriginUseMatches = origin?.UseId == frameId, ActorMatches = use is not null && origin is not null && _characterFanJiangZhangDaOlOperations.JueshengActorMatches(use, origin), IssuedCount = origin is null ? 0 : _coreProgramEventHistoryOperations.Value.ProgramEventHistory<JueshengDuelIssuedEvent>().Count(e => e.Origin == origin && e.InstructionIndex == 1), HuangFuSongRankCapture = _characterHuangfuSongWumiaoOperations.HasWuHuangFuSongCapability }));
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

    internal SkillProgramStepOutcome BeginDamageAppearanceDraw(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.DamageAppearanceReceipt is not null || f.InstructionIndex != 1 || f.WindowContext is not { Window: SkillProgramTriggerWindow.DamageAppliedBeforeDying } context || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not DamageTriggerWindowFrame window || window.Id != context.ParentFrameId || window.TriggerWindow != context.Window || window.ParentFrameId != context.DamageFrameId || window.TargetSeat != f.OwnerSeat || context.TargetSeat != f.OwnerSeat)
            throw new InvalidOperationException("Damage appearance Draw requires the exact applied damage target window.");
        var attempt = _coreNativeDamageHandlesOperations.Value.GetDamageTriggerAttack(window);
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

    SkillProgramStepOutcome IDamageColorDuelProgramHost.DrawByDamageCardColor(ProgramSkillFrame frame) => BeginDamageAppearanceDraw(frame);
    SkillProgramStepOutcome IDamageColorDuelProgramHost.UseSelectedActorDuel(ProgramSkillFrame frame) => BeginSelectedActorDuel(frame);
}
