#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterPangTongFriendRuntime
{
    internal bool HasFriendYangmingQuota(CharacterState owner) => _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { FriendPangTong.Stage: FriendPangTongStage.Use } frame && frame.OwnerSeat == owner.Seat;
    internal static CardLocation FriendYangmingLocation(ProgramSkillFrame frame) => RuntimeSkills.RuntimeRules_pang_tong_friend_FriendYangming.FriendYangmingLocation(frame);
    internal SkillProgramStepOutcome BeginFriendYangming(ProgramSkillFrame frame)
    {
        var suits = _corePlayCardHistoryOperations.EventsSinceLastBoundary(payload => payload is TurnStartedEvent).OfType<CardMovedEvent>().Where(move => move.To == CardLocation.DiscardPile && move.From != CardLocation.DiscardPile).Select(move => _coreCardAttackStateOperations.GetAttackCard(move.CardId).Suit).Where(suit => suit != Suit.None).Distinct().Count();
        var count = suits + (_coreNationalSkillsOperations.HasRuntimeSkill(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], "friend:gongli-pang-tong") && _coreFriendTrioOperations.HasFriendTrioAlly(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], "zhuge-liang") ? 1 : 0);
        if (count > 0)
            _corePhysicalDrawFacadesOperations.EnsureDrawPile();
        var cards = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).TakeLast(count).Reverse().ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { FriendPangTong = new(frame.InstructionIndex, SkillProgramEffectOp.UseFriendDeckSuitsAtPlayEnd, FriendPangTongStage.Placement, cards.Select(card => card.Id).ToArray(), [], []) });
        _cardZonesOperations._cardZones.EnsurePublicPersistentPile(FriendYangmingLocation(frame));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FriendYangmingStoredEvent(frame.Id, frame.OwnerSeat, cards.Length));
        FriendPangTongMutation(frame, FriendPangTongStage.Use, () => _corePhysicalMovementFacadesOperations.MoveCards(cards, CardLocation.DrawPile, FriendYangmingLocation(frame), new("friend.yangming.place")));
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool IsFriendYangmingOriginalUseFrame(ProgramSkillFrame frame, CharacterState owner, Card card, CardKind kind)
    {
        if (card.Id <= 0 || card.Kind != kind || frame.OwnerSeat != owner.Seat || !_coreDeckProgramsOperations.DeckProgramSourceValid(frame) || frame.FriendPangTong is not { Operation: SkillProgramEffectOp.UseFriendDeckSuitsAtPlayEnd, Stage: FriendPangTongStage.Use, UseId: null } receipt || receipt.InstructionIndex != frame.InstructionIndex || !receipt.Cards.Contains(card.Id) || receipt.UsedSuits.Contains(card.Suit) || _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program is not { } program || program.GameplayHash != frame.GameplayHash || _cardZonesOperations._cardZones.GetLocation(card.Id) != FriendYangmingLocation(frame))
            return false;
        var effect = ProgramInstructionResolver.Default.Resolve(frame, program).GetPausedInstruction(frame.InstructionIndex).Effect;
        return effect.Op == SkillProgramEffectOp.UseFriendDeckSuitsAtPlayEnd && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<FriendYangmingStoredEvent>().Count(e => e.ProgramFrameId == frame.Id && e.OwnerSeat == owner.Seat && e.Count == receipt.Cards.Count) == 1 && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(move => move.CardId == card.Id && move.CardKind == card.Kind && move.From == CardLocation.DrawPile && move.To == FriendYangmingLocation(frame) && move.Reason.Value == "friend.yangming.place") == 1;
    }

    internal bool IsFriendYangmingOriginalUseChoice(CharacterState owner, Card card, CardKind kind) => _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame frame && IsFriendYangmingOriginalUseFrame(frame, owner, card, kind);
    internal bool IsFriendYangmingOriginalUseParent(CardUseFrame use)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == use.Id);
        if (index != _resolutionStackOperations._resolutionStack.Count - 1 || index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramSkillFrame parent || use.Action is not { Type: CardActionType.Use, PhysicalCards: [var cost], ConversionChain.Count: 0 } action || action.ActorSeat != parent.OwnerSeat || action.ProviderSeat != parent.OwnerSeat || action.RequesterSeat is not null || use.SourceSeat != parent.OwnerSeat || use.CardId != cost.CardId || use.CardKind != cost.CardKind || action.EffectiveKind != cost.CardKind || cost.From != FriendYangmingLocation(parent) || use.PhysicalCardIds is not [var physicalId] || physicalId != cost.CardId)
            return false;
        return IsFriendYangmingOriginalUseFrame(parent, _coreMatchParticipantsPlayersAccess._players[parent.OwnerSeat], _coreCardAttackStateOperations.GetAttackCard(cost.CardId), cost.CardKind);
    }

    internal IReadOnlyList<LegalAction> FriendYangmingActions(ProgramSkillFrame frame)
    {
        var receipt = frame.FriendPangTong!;
        var actor = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        var actions = new List<LegalAction>();
        if (!actor.IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return[];
        foreach (var card in _cardZonesOperations._cardZones.CardsAt(FriendYangmingLocation(frame)).Where(card => !receipt.UsedSuits.Contains(card.Suit) && receipt.Cards.Contains(card.Id)))
        {
            var kind = card.Kind;
            var suit = _coreNativeCardAppearanceOperations.EffectiveSuit(actor, card);
            var red = CoreGameEnginePublicPileColorsRuntime.SuitColor(suit);
            if (_coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, kind, CardActionType.Use) || _coreResponseEntityExchangeOperations.IsResponseEntityRestricted(actor.Seat, card.Id))
                continue;
            if (CoreNativeCardClassificationsRuntime.IsSlashCard(kind))
                foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(target => _coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, target, card, effectiveKind: kind, physicalCardIds: [card.Id])))
                    actions.Add(new(LegalActionKind.Slash, card.Id, target.Seat, $"使用【{(card.DisplayName)}】对 {(target.Name)}", kind, TargetSeats: [target.Seat]));
            else if (kind == CardKind.Peach && _coreTreatedWoundedOperations.IsRuleWounded(actor) && !_coreHandComparisonOperations.HasSelfCardTargetProhibition(actor.Seat) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(actor, kind, suit, red))
                actions.Add(new(LegalActionKind.Peach, card.Id, null, "使用【桃】", kind));
            else if (kind == CardKind.Alcohol && !actor.HasAlcoholEffect && !_coreHandComparisonOperations.HasSelfCardTargetProhibition(actor.Seat) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(actor, kind, suit, red))
                actions.Add(new(LegalActionKind.Alcohol, card.Id, null, "使用【酒】（无次数限制）", kind));
            else if (_coreEquipmentContentOperations.EquipmentRules.IsEquipment(kind) && actor.EquipmentSlotCapacity(_coreEquipmentContentOperations.EquipmentRules.Get(kind).Slot) > 0 && !_coreHandComparisonOperations.IsSelfTargetForbiddenAction(actor, kind, [actor.Seat]))
                actions.Add(new(LegalActionKind.Equip, card.Id, null, $"使用【{(card.DisplayName)}】", kind));
            else if (CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(kind) && kind != CardKind.Nullification)
                foreach (var option in _coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(actor, kind, suit, enforceUsePermission: true, beneficiaryShieldSuit: suit, actualEffectiveColor: red, hasActualColor: true, physicalCardIds: [card.Id], includeNextActualUseAdjustment: true))
                    actions.Add(new(option.ActionKind, card.Id, option.TargetSeats.Count == 1 ? option.TargetSeats[0] : null, $"使用【{(card.DisplayName)}】：{(option.Description)}", kind, option.TargetCardId, option.TargetSeats));
            else if (kind is CardKind.Indulgence or CardKind.SupplyShortage or CardKind.Lightning)
                foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(target => target.IsAlive && (kind == CardKind.Lightning ? target.Seat == actor.Seat : target.Seat != actor.Seat) && !target.JudgmentAreaAbolished && !_coreActualTurnsOperations.HasJudgmentEffectiveCard(target, kind) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(target, kind, suit, red) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, target.Seat, kind) && (kind != CardKind.SupplyShortage || _coreNativeRuleQueriesOperations.GetSeatDistance(actor.Seat, target.Seat) <= 1 || _coreDirectedCardPolicyQueriesOperations.HasCardDistanceExemption(actor, target, kind))))
                    actions.Add(new(kind switch
                    {
                        CardKind.Indulgence => LegalActionKind.Indulgence,
                        CardKind.SupplyShortage => LegalActionKind.SupplyShortage,
                        _ => LegalActionKind.Lightning
                    }, card.Id, target.Seat, $"对 {(target.Name)} 使用【{(card.DisplayName)}】", kind, TargetSeats: [target.Seat]));
        }

        return actions;
    }

    internal void IssueFriendYangmingUse(ProgramSkillFrame frame, LegalAction action)
    {
        var actor = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        var card = _coreCardAttackStateOperations.GetAttackCard(action.CardId!.Value);
        var kind = card.Kind;
        var receipt = frame.FriendPangTong!;
        GameEngine.ProgramOrdinaryTrickUseOption? option = null;
        if (CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(kind))
            option = _coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(actor, kind, _coreNativeCardAppearanceOperations.EffectiveSuit(actor, card), enforceUsePermission: true, beneficiaryShieldSuit: _coreNativeCardAppearanceOperations.EffectiveSuit(actor, card), actualEffectiveColor: CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(actor, card)), hasActualColor: true, physicalCardIds: [card.Id], includeNextActualUseAdjustment: true).Single(candidate => candidate.ActionKind == action.Kind && candidate.TargetCardId == action.TargetCardId && candidate.TargetSeats.SequenceEqual(action.TargetSeats));
        if (option?.NextActualUseAdjusted == true)
            _coreGameEngineNextCardTargetAdjustmentsSelectedNextCardTargetSeatsAccess._selectedNextCardTargetSeats = action.TargetSeats;
        _coreCardConversionsOperations.SelectUseConversion(action);
        var id = _coreNativeCardUseOperations.BeginCardUse(card, actor.Seat, action.TargetSeats, kind, designatedTargetSeats: kind == CardKind.BorrowedSword ? action.TargetSeats.Take(1).ToArray() : null);
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
        var ret = new FriendYangmingUseReturn(frame.Id, frame.InstructionIndex, card.Id, card.Suit, frame.SkillInstanceId, frame.GameplayHash, use.Action!.ActionId);
        _coreRuntimeOperations.ReplaceRuntimeFrame(frame.Id, frame with { FriendPangTong = receipt with { Stage = FriendPangTongStage.Issued, UseId = id, UsedSuits = receipt.UsedSuits.Append(card.Suit).ToArray() } });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, current => current with { FriendYangmingReturn = ret, UnlimitedUse = true, QuotaOnlyUseLimitExemption = true });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FriendYangmingUseIssuedEvent(id, actor.Seat, ret));
        _corePhysicalMovementFacadesOperations.MoveCard(card, FriendYangmingLocation(frame), CardLocation.Processing, _coreEquipmentContentOperations.EquipmentRules.IsEquipment(kind) ? CardMoveReasons.EquipmentUse : CardMoveReasons.Use);
        if (_coreEquipmentContentOperations.EquipmentRules.IsEquipment(kind))
        {
            if (!_coreFinalizedCardTargetsOperations.TryBeginEquipmentTargetPrograms(actor, card, id))
                _coreNativeEquipmentUseOperations.CompleteEquipmentUse(actor, card, id);
            return;
        }

        if (kind == CardKind.Peach)
        {
            _coreCardUseLifecycleOperations.BeginSimpleCardUse(id, new(card.Id, SimpleCardUseEffect.Recovery));
            return;
        }

        if (kind == CardKind.Alcohol)
        {
            _coreCardUseLifecycleOperations.BeginSimpleCardUse(id, new(card.Id, SimpleCardUseEffect.Alcohol));
            return;
        }

        if (CoreNativeCardClassificationsRuntime.IsSlashCard(kind))
        {
            var target = _coreMatchParticipantsPlayersAccess._players[action.TargetSeats[0]];
            var damage = 1 + actor.AlcoholDamageBonus;
            _coreAlcoholSlashSuppressionsOperations.Value.CaptureProgramAlcoholConsumption(id, actor);
            actor.HasAlcoholEffect = false;
            var attack = _kernelCoreMainOperations.CreateCardAttackHandle(id, actor.Seat, target.Seat, card, damageAmount: damage, playedCardKind: kind, physicalCards: [card], programSkillCardUseFrameId: frame.Id, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, target, kind));
            _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
            _coreCurrentCardEnhancementsOperations.CaptureProgramAdjustedSlashBaseDamage(attack);
            _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actor.Seat, kind);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(card.Id, kind, actor.Seat, target.Seat));
            if (!_coreCardActionsOperations.TryBeginProgramCardWindow(attack, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.Action!, SkillProgramTriggerWindow.CardUseCommitted, action.TargetSeats, ProgramCardContinuation.CommittedSlash))
                _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
            return;
        }

        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, actor.Seat, use.TargetSeats, action.Kind, action.TargetCardId, option?.RequiredCardKind, kind);
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterCleanup, 30)]
    internal bool CompleteFriendYangmingNativeAttackReturn()
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { FriendPangTong.Stage: FriendPangTongStage.Returned } parent)
            return false;
        _coreRuntimeOperations.AdvanceRuntimeProgram(parent.Id);
        return true;
    }

    [NativeUseReturn(310)]
    internal void ReturnFriendYangmingUse(CardUseFrame use)
    {
        if (use.FriendYangmingReturn is not { } ret)
            return;
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(ret.ProgramFrameId);
        var receipt = frame.FriendPangTong;
        if (receipt is not { Stage: FriendPangTongStage.Issued } || receipt.UseId != use.Id || frame.InstructionIndex != ret.InstructionIndex || frame.SkillInstanceId != ret.SkillInstanceId || frame.GameplayHash != ret.GameplayHash || use.CardId != ret.CardId || use.Action?.ActionId != ret.CardActionId || !receipt.UsedSuits.Contains(ret.Suit))
            throw new InvalidOperationException("Yangming lost its once-issued owning typed Use return.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(frame.Id, frame = frame with { FriendPangTong = receipt with { Stage = FriendPangTongStage.Returned, UseId = null } });
        if (use.CardAttack is not { Active: true })
            ContinueFriendPangTong(frame);
    }

    internal void CleanupFriendYangming(ProgramSkillFrame frame, bool reward = true)
    {
        var cards = _cardZonesOperations._cardZones.CardsAt(FriendYangmingLocation(frame)).ToArray();
        FriendPangTongMutation(frame, reward ? FriendPangTongStage.Reward : FriendPangTongStage.Complete, () => _corePhysicalMovementFacadesOperations.MoveCards(cards, FriendYangmingLocation(frame), CardLocation.DiscardPile, new("friend.yangming.remaining")));
    }
}
