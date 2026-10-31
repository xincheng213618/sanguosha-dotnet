#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXuHuangMouMobileRuntime
{
    internal IReadOnlyList<GameEngine.ProgramOrdinaryTrickUseOption> MouXuHuangDuelOptions(ProgramSkillFrame f) => Array.AsReadOnly(_coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], CardKind.Duel, Suit.None, enforceUsePermission: true, beneficiaryShieldSuit: Suit.None, actualEffectiveColor: null, hasActualColor: true, physicalCardIds: [], includeNextActualUseAdjustment: true, conversionSource: f.MouXuHuangDuanliang!.Source).Where(o => o.ActionKind == LegalActionKind.Duel && o.TargetSeats.Count > 0 && o.TargetSeats[0] == f.MouXuHuangDuanliang.TargetSeat).ToArray());
    internal IReadOnlyList<PromptChoice> MouXuHuangDuelChoices(ProgramSkillFrame f) => Array.AsReadOnly(MouXuHuangDuelOptions(f).Select(o => new PromptChoice(new($"mou-xu-huang.{(f.Id)}.duel.{(o.Id.Value)}"), o.Description, [], o.TargetSeats, new Dictionary<string, string> { ["program-action"] = "mou-xu-huang-duel-targets", ["frame-id"] = f.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["option-id"] = o.Id.Value })).ToArray());
    internal void BeginMouXuHuangDuelTargets(ProgramSkillFrame f)
    {
        var options = MouXuHuangDuelOptions(f);
        if (options.Count == 0)
        {
            FinishMouXuHuangDuanliang(f);
            return;
        }

        if (options.Count == 1)
        {
            AcceptMouXuHuangDuelTargets(f, options[0]);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouXuHuangDuanliang = f.MouXuHuangDuanliang!with { Stage = MouXuHuangDuanliangStage.DuelTargets } });
        PublishMouXuHuang(f, f.OwnerSeat, MouXuHuangDuelChoices(f), "擂鼓进军：选择本次原生决斗的目标调整。");
    }

    [ProgramAnswer("mou-xu-huang-duel-targets")]
    internal void AnswerMouXuHuangDuelTargets(PromptChoice selected)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(_resolutionStackOperations._resolutionStack[^1].Id);
        if (!MouXuHuangDuanliangRoot(f) || f.MouXuHuangDuanliang!.Stage != MouXuHuangDuanliangStage.DuelTargets || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], MouXuHuangDuelChoices(f).Where(c => c.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("Duanliang's optional native target adjustment lost its exact current producer.");
        var option = MouXuHuangDuelOptions(f).Single(o => o.Id.Value == selected.Parameters["option-id"]);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        AcceptMouXuHuangDuelTargets(f, option);
    }

    internal void AcceptMouXuHuangDuelTargets(ProgramSkillFrame f, GameEngine.ProgramOrdinaryTrickUseOption option)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouXuHuangDuanliang = f.MouXuHuangDuanliang!with { NativeTargets = option.TargetSeats } });
        foreach (var pair in option.TargetSeats.Select((seat, index) => (seat, index)))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouXuHuangDuelTargetSelectedEvent(f.Id, pair.index, pair.seat, option.NextActualUseAdjusted));
        if (option.NextActualUseAdjusted)
            _coreGameEngineNextCardTargetAdjustmentsSelectedNextCardTargetSeatsAccess._selectedNextCardTargetSeats = option.TargetSeats;
        IssueMouXuHuangUse(f, new(0, CardKind.Duel, Suit.None, 0), CardKind.Duel);
    }

    internal bool CanMouXuHuangDuel(int actor, int target, CardConversionSource source) => _coreMatchParticipantsPlayersAccess._players[actor].IsAlive && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && actor != target && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && !_coreCardUseModulesOperations.HasTurnCardTargetRestriction(actor, SkillProgramCardTargetRestriction.SelfOnly) && !_coreCardUseModulesOperations.IsCardUseForbidden(actor, CardKind.Duel, CardActionType.Use) && _coreProgramTargetRulesOperations.TargetRules.AllowsNativeUseMaterial(new(actor, CardKind.Duel, [], source)) && !_characterCuiYanOlOperations.IsCuiYanNextUseForbidden(actor, [], null) && _coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(_coreMatchParticipantsPlayersAccess._players[actor], CardKind.Duel, Suit.None, enforceUsePermission: true, beneficiaryShieldSuit: Suit.None, actualEffectiveColor: null, hasActualColor: true, physicalCardIds: [], conversionSource: source).Any(p => p.ActionKind == LegalActionKind.Duel && p.TargetSeats.SequenceEqual([target]));
    internal bool CanMouXuHuangSupply(ProgramSkillFrame f, Card card)
    {
        var r = f.MouXuHuangDuanliang!;
        var actor = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[r.TargetSeat];
        var suit = _coreNativeCardAppearanceOperations.EffectiveSuit(actor, card);
        var color = _coreActionDiscardColorsOperations.CapturePhysicalCardColor(actor.Seat, card);
        return actor.IsAlive && target.IsAlive && actor.Seat != target.Seat && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _cardZonesOperations._cardZones.GetLocation(card.Id) == CardLocation.DrawPile && !target.JudgmentAreaAbolished && !_coreActualTurnsOperations.HasJudgmentEffectiveCard(target, CardKind.SupplyShortage) && !_coreProgramCardPoliciesOperations.HasCardPolicy(target, SkillProgramCardPolicyKind.ProhibitDelayedTrickTarget) && !_coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, CardKind.SupplyShortage, CardActionType.Use) && !_coreProgramParticipantPhaseOperationsOperations.IsTurnPhysicalUseForbidden(actor.Seat, [card.Id]) && !_coreProgramParticipantPhaseOperationsOperations.IsTurnSuitUseForbidden(actor.Seat, suit) && _coreProgramTargetRulesOperations.TargetRules.AllowsNativeUseMaterial(new(actor.Seat, CardKind.SupplyShortage, [card.Id], r.Source)) && !_characterCuiYanOlOperations.IsCuiYanNextUseForbidden(actor.Seat, [card.Id], suit) && !_coreCardUseModulesOperations.HasTurnCardTargetRestriction(actor.Seat, SkillProgramCardTargetRestriction.SelfOnly) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, target.Seat, CardKind.SupplyShortage) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(target, CardKind.SupplyShortage, suit, color) && !_coreShenwuEquipmentOperations.IsShenwuRobeTargetProhibited(actor.Seat, target.Seat, CardKind.SupplyShortage) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, target.Seat, suit);
    }

    internal void IssueMouXuHuangTop(ProgramSkillFrame f)
    {
        var card = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).LastOrDefault();
        if (card is null || !CanMouXuHuangSupply(f, card))
        {
            FinishMouXuHuangDuanliang(f);
            return;
        }

        IssueMouXuHuangUse(f, card, CardKind.SupplyShortage);
    }

    internal void IssueMouXuHuangUse(ProgramSkillFrame f, Card card, CardKind kind)
    {
        var r = f.MouXuHuangDuanliang!;
        if (!MouXuHuangDuanliangRoot(f) || !r.Revealed || !r.Success || r.Native is not null || kind == CardKind.Duel && !CanMouXuHuangDuel(f.OwnerSeat, r.TargetSeat, r.Source) || kind == CardKind.SupplyShortage && !CanMouXuHuangSupply(f, card) || kind is not (CardKind.Duel or CardKind.SupplyShortage))
            throw new InvalidOperationException("Duanliang must issue its chosen successful legal native consequence exactly once.");
        var origin = new MouXuHuangNativeOrigin(f.Id, r.Source, f.GameplayHash, _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1, _coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence + 1, f.OwnerSeat, r.TargetSeat, kind, card.Id, card.Kind, card.Id == 0 ? null : CardLocation.DrawPile, card.Id == 0 ? Suit.None : _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], card), card.Rank, card.Id == 0 ? null : _coreActionDiscardColorsOperations.CapturePhysicalCardColor(f.OwnerSeat, card));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouXuHuangDuanliang = r with { Stage = MouXuHuangDuanliangStage.NativeIssuing, Native = origin, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouXuHuangNativeIssuedEvent(origin));
        var id = _coreNativeCardUseOperations.BeginCardUse(card, f.OwnerSeat, kind == CardKind.Duel ? r.NativeTargets : [r.TargetSeat], kind, physicalCardIds: card.Id == 0 ? [] : [card.Id], conversionSource: r.Source);
        if (id != origin.UseId || !IsMouXuHuangNativeUse(id))
            throw new InvalidOperationException("Duanliang changed its signed native Use identity.");
        if (card.Id > 0)
        {
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DrawPile, CardLocation.Processing, CardMoveReasons.Use, m =>
            {
                if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame { MouXuHuangNative: { } signed } live || live.Id != id || signed != origin || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not ProgramSkillFrame { MouXuHuangDuanliang: { Stage: MouXuHuangDuanliangStage.NativeUse, PaymentSequence: null, Native: { } parentOrigin } } parent || parent.Id != f.Id || parentOrigin != origin || !MouXuHuangDuanliangRoot(parent) || !IsMouXuHuangNativeUse(id) || m.CardId != origin.CardId || m.CardKind != origin.PrintedKind || m.From != origin.From || m.To != CardLocation.Processing || m.Reason != CardMoveReasons.Use)
                    throw new InvalidOperationException("Duanliang top must pay its one original real DrawPile entity.");
                _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { MouXuHuangDuanliang = parent.MouXuHuangDuanliang with { PaymentSequence = m.Sequence, After = m.Sequence } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouXuHuangTopPaidEvent(f.Id, id, m.CardId, m.Sequence));
            });
        }

        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, f.OwnerSeat, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.TargetSeats, kind == CardKind.Duel ? LegalActionKind.Duel : LegalActionKind.SupplyShortage, playedCardKind: kind);
    }

    internal CardActionContext PrepareMouXuHuangAction(CardActionContext action)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouXuHuangDuanliang: { Stage: MouXuHuangDuanliangStage.NativeIssuing, Native: { } o } } f)
            return action;
        if (!MouXuHuangDuanliangRoot(f) || action.Type != CardActionType.Use || action.ActionId != o.ActionId || action.ActorSeat != o.ActorSeat || action.ProviderSeat != o.ActorSeat || action.RequesterSeat is not null || action.EffectiveKind != o.Kind || !action.TargetSeats.SequenceEqual(o.Kind == CardKind.Duel ? f.MouXuHuangDuanliang.NativeTargets : [o.TargetSeat]) || !action.ConversionChain.SequenceEqual([o.Source]) || (o.CardId == 0 ? action.PhysicalCards.Count != 0 : action.PhysicalCards is not [var cost] || cost.CardId != o.CardId || cost.CardKind != o.PrintedKind || cost.From != o.From))
            throw new InvalidOperationException("Xu Huang's native declaration must retain its exact chosen source, original top or empty Duel materials.");
        return _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(action.ActionId, action.ParentActionId, action.Type, action.ActorSeat, action.ProviderSeat, action.RequesterSeat, action.ResponderSeat, action.OpponentSeat, action.EffectiveKind, action.TargetSeats, action.PhysicalCards, action.ConversionChain, action.DesignatedTargetSeats, effectiveSuit: o.Suit, effectiveRank: o.Rank, effectiveIsRed: o.EffectiveIsRed));
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 29500)]
    internal void BindMouXuHuangDeclared(IGameEvent fact)
    {
        if (fact is not CardUseDeclaredEvent e || _coreCardUseLifecycleOperations.LifecycleCardUse(e.ResolutionId)is not { } use)
            return;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == use.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramSkillFrame { MouXuHuangDuanliang: { Stage: MouXuHuangDuanliangStage.NativeIssuing, Native: { } o } r } f)
            return;
        if (!MouXuHuangDuanliangRoot(f) || use.Id != o.UseId || use.Action?.ActionId != o.ActionId || use.CardId != o.CardId || use.CardKind != o.Kind)
            throw new InvalidOperationException("Xu Huang cannot bind another producer's native declaration.");
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { MouXuHuangNative = o });
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { MouXuHuangDuanliang = r with { Stage = MouXuHuangDuanliangStage.NativeUse } });
    }

    internal bool IsMouXuHuangNativeUse(long id)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { MouXuHuangNative: { } o, Action: { Type: CardActionType.Use } a } use || o.UseId != id || o.ActionId != a.ActionId || use.CardId != o.CardId || use.CardKind != o.Kind || a.EffectiveKind != o.Kind || a.ProviderSeat != o.ActorSeat || a.RequesterSeat is not null || a.EffectiveSuit != o.Suit || a.EffectiveRank != o.Rank || a.EffectiveIsRed != o.EffectiveIsRed || !a.ConversionChain.SequenceEqual([o.Source]) || !_characterZhouYuOlMouOperations.MouGuquActorLineageMatches(id, a.ActionId, o.ActorSeat, a.ActorSeat, a.ProviderSeat, a.Type) || use.SourceSeat != a.ActorSeat || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == o.ProgramFrameId)is not ProgramSkillFrame { MouXuHuangDuanliang: { Native: { } expected, Stage: MouXuHuangDuanliangStage.NativeUse } } f || expected != o || !MouXuHuangDuanliangRoot(f) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouXuHuangNativeIssuedEvent>().Count(e => e.Origin == o) != 1)
            return false;
        return o.CardId == 0 ? o.Kind == CardKind.Duel && use.PhysicalCardIds is { Count: 0 } && a.PhysicalCards.Count == 0 && a.EffectiveIsRed is null : o.Kind == CardKind.SupplyShortage && use.PhysicalCardIds is [var material] && material == o.CardId && a.PhysicalCards is [var cost] && cost.CardId == o.CardId && cost.CardKind == o.PrintedKind && cost.From == CardLocation.DrawPile;
    }

    internal bool IsMouXuHuangZeroUse(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { CardId: 0 } && IsMouXuHuangNativeUse(id);
    internal bool MatchesMouXuHuangNativeAction(long id, CardActionContext action, int cardId) => cardId == 0 && IsMouXuHuangZeroUse(id) && _coreCardUseLifecycleOperations.LifecycleCardUse(id)?.Action is { } current && action.ActionId == current.ActionId && action.Type == current.Type && action.ActorSeat == current.ActorSeat && action.ProviderSeat == current.ProviderSeat && action.EffectiveKind == current.EffectiveKind && action.PhysicalCards.Count == 0 && action.ConversionChain.SequenceEqual(current.ConversionChain);
    internal Card MouXuHuangRepresentation(long id) => IsMouXuHuangZeroUse(id) ? new(0, CardKind.Duel, Suit.None, 0) : throw new InvalidOperationException("Xu Huang lost its signed zero-material Duel.");
    internal Card ReadMouXuHuangUseAppearance(long id, CardAppearanceReference appearance)
    {
        if (!IsMouXuHuangZeroUse(id))
            return _characterLiuChengMouMobileOperations.ReadMouLiuChengUseAppearance(id, appearance);
        if (appearance.Id != 0 || appearance.Kind != CardKind.Duel || appearance.Suit != Suit.None || appearance.Rank != 0)
            throw new InvalidOperationException("Xu Huang's empty native Duel changed its neutral appearance.");
        return MouXuHuangRepresentation(id);
    }

    internal long? MouXuHuangDuelProgramParent(long id) => IsMouXuHuangZeroUse(id) ? _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.MouXuHuangNative!.ProgramFrameId : null;
    [NativeUseValidation(29500)]
    internal void ValidateMouXuHuangNative(CardUseFrame use)
    {
        if (use.MouXuHuangNative is not null && (!IsMouXuHuangNativeUse(use.Id) || use.Step != ResolutionFrameStep.Completed || use.PendingRecoveryAttempts is { Count: > 0 } || use.RecoveryPaidContinuation is not null))
            throw new InvalidOperationException("Xu Huang cannot return an incomplete or unproven native Use.");
    }

    [NativeUseReturn(29500)]
    internal void ReturnMouXuHuangNative(CardUseFrame use)
    {
        if (use.MouXuHuangNative is not { } o)
            return;
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(o.ProgramFrameId);
        var r = f.MouXuHuangDuanliang!;
        if (!MouXuHuangDuanliangRoot(f) || r.Native != o || r.NativeReturned || _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)is not null || use.Step != ResolutionFrameStep.Completed)
            throw new InvalidOperationException("Xu Huang's completed native Use must return once to its own original strategy.");
        var batches = _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Where(b => b.ParentFrameId == use.Id && b.AwaitingProgramFrameId is null && b.OriginSkillId == f.SkillId && b.OriginSkillInstanceId == f.SkillInstanceId && b.OriginOwnerSeat == f.OwnerSeat).ToArray();
        foreach (var batch in batches)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouXuHuangNativeBatchSignedEvent(f.Id, use.Id, batch.Id, batch.ParentBatchId, batch.Movements.Count));
            foreach (var m in batch.Movements)
                SignMouXuHuangMovement(f, m);
        }

        var attackPending = use.CardAttack is { Active: true };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouXuHuangDuanliang = r with { Stage = MouXuHuangDuanliangStage.NativeChildren, NativeReturned = true, AttackReturned = !attackPending, NativeBatchIds = batches.Select(b => b.Id).ToArray() }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouXuHuangNativeReturnedEvent(f.Id, use.Id, o.ActionId));
        if (!attackPending)
            ContinueMouXuHuangDuanliang(f.Id);
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterBorrowedSword, 29500)]
    internal bool ReturnMouXuHuangNativeAttack(GameEngine.AttackCompletionReceipt completion)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouXuHuangDuanliang: { NativeReturned: true, AttackReturned: false, Native: { } o } r } f || completion.ResolutionId != o.UseId)
            return false;
        if (completion.ProgramFrameId != f.Id || !MouXuHuangDuanliangRoot(f))
            throw new InvalidOperationException("Xu Huang's Duel returned to another owning strategy.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouXuHuangDuanliang = r with { AttackReturned = true } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouXuHuangNativeAttackReturnedEvent(f.Id, o.UseId));
        ContinueMouXuHuangDuanliang(f.Id);
        return true;
    }

    internal bool IsMouXuHuangNativeSettlementBatch(ProgramSkillFrame f, CardMovementBatchContext b) => f.MouXuHuangDuanliang is { Stage: MouXuHuangDuanliangStage.NativeChildren, NativeReturned: true, Native: { } o } r && MouXuHuangDuanliangRoot(f) && r.NativeBatchIds.Contains(b.Id) && b.ParentFrameId == o.UseId && b.AwaitingProgramFrameId is null && b.Movements.Count > 0 && b.OriginSkillId == f.SkillId && b.OriginSkillInstanceId == f.SkillInstanceId && b.OriginOwnerSeat == f.OwnerSeat && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouXuHuangNativeBatchSignedEvent>().Count(e => e == new MouXuHuangNativeBatchSignedEvent(f.Id, o.UseId, b.Id, b.ParentBatchId, b.Movements.Count)) == 1 && b.Movements.All(m => MouXuHuangMovementSigned(f, m));
    [ProgramPipelineHook(ProgramPipelinePhase.IsProgramMovementSettlement, 1600)]
    internal bool WindowSettlement_IsMouXuHuangNativeSettlementBatch(ProgramSkillFrame frame, CardMovementBatchContext batch) => IsMouXuHuangNativeSettlementBatch(frame, batch);
}
