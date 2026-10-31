#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramOwnedCardsRuntime
{
    internal SkillProgramEffect GetOwnedSelectionEffect(ProgramSkillFrame frame) => _coreHalfHandPhaseDebtProgramsOperations.PhaseHandDebtOwnedSelectionEffect(frame, _coreEquipmentPairPaymentsOperations.EquipmentPairOwnedSelectionEffect(frame, _coreTurnDrawDebtsOperations.TurnDrawDebtOwnedSelectionEffect(frame, ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect)));
    internal int GetProgramOwnerLostHp(ProgramSkillFrame frame)
    {
        // Lifecycle facts are frozen before earlier candidates can change HP.
        var facts = frame.WindowContext?.Facts;
        if (facts?.UnselectedOwnerHp is { } dual && _characterDaQiaoXiaoQiaoHappyGodOperations.CurrentHappyQiaoPool(frame.OwnerSeat)is { } pool)
            return Math.Max(0, dual.MaximumHp - (pool == HappyQiaoPool.DaQiao ? dual.FirstHp : dual.SecondHp));
        if (facts?.OwnerHpUndefined == true || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].HasHpValue)
            throw new InvalidOperationException("Undefined HP cannot supply a numeric program operand.");
        return facts is not null ? Math.Max(0, facts.CurrentMaxHp - facts.CurrentHp) : Math.Max(0, _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].MaxHp - _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].Hp);
    }

    internal SkillProgramStepOutcome SelectProgramOwnedCards(ProgramSkillFrame frame, int cardOwnerSeat, int amount, SkillProgramNumberExpression? expression, IReadOnlyList<CardZoneKind> zones, string resultBind, int minimumCards, int maximumCards, IReadOnlyList<CardKind> cardKinds, IReadOnlyList<Suit> suits)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (expression == SkillProgramNumberExpression.OwnerLostHp && (active.WindowContext?.Facts is { OwnerHpUndefined: true, UnselectedOwnerHp: null } || !_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].HasHpValue) || expression == SkillProgramNumberExpression.LivingPlayersMinHp && !_coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.HasHpValue))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(active, "体力值未定义，无法计算选牌数量。");
            return SkillProgramStepOutcome.Continue;
        }

        if (active.OwnedCardSelection is not null || active.CardSetBindings.Any(binding => binding.Name == resultBind))
            throw new InvalidOperationException("An owned-card selection cannot overwrite an existing draft or binding.");
        var candidates = zones.SelectMany(zone =>
        {
            var location = new CardLocation(zone, cardOwnerSeat);
            return _cardZonesOperations._cardZones.CardsAt(location).Where(card => _coreTurnHandCategoryRestrictionsOperations.CanSelectSelfDiscardBinding(active, resultBind, cardOwnerSeat, card, location)).Where(card => _coreTriggeredCardConversionsOperations.MatchesTriggeredSlashMaterial(active, resultBind, cardOwnerSeat, card, location)).Where(card => _coreEquipmentPairPaymentsOperations.MatchesEquipmentPairOwnedCost(active, resultBind, card, location)).Where(card => _coreHalfHandPhaseDebtProgramsOperations.MatchesDeferredHandDebtCost(active, resultBind, card, location)).Where(card => _corePublicPileCashOutOperations.MatchesPublicPileStorageSelection(active, resultBind, card, location)).Where(card => _coreSharedSlashOffersOperations.MatchesProgramOwnedSelectionKind(active, resultBind, _coreMatchParticipantsPlayersAccess._players[cardOwnerSeat], card, location, cardKinds)).Where(card => suits.Count == 0 || suits.Contains(_coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(_coreMatchParticipantsPlayersAccess._players[cardOwnerSeat], card))).Select(card => (card.Id, Location: location));
        }).ToArray();
        var requested = maximumCards > 0 ? expression == SkillProgramNumberExpression.LivingPlayerCount ? Math.Min(maximumCards, _coreMatchParticipantsPlayersAccess._players.Count(player => player.IsAlive)) : maximumCards : expression switch
        {
            null => amount,
            SkillProgramNumberExpression.CategoryTargetTurnUsage => _coreProgramCategoryTargetLedgerOperations.GetProgramCategoryTargetTurnUsage(active),
            SkillProgramNumberExpression.OwnerLostHp => GetProgramOwnerLostHp(active),
            SkillProgramNumberExpression.AllOwnedZoneCards => candidates.Length,
            SkillProgramNumberExpression.HandHalfFloor => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[cardOwnerSeat]).Count / 2,
            SkillProgramNumberExpression.HandCountMinusFour => Math.Max(0, _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[cardOwnerSeat]).Count - 4),
            SkillProgramNumberExpression.LivingPlayersMinHp => _coreRuleQueriesOperations.GetLivingPlayersMinHp(),
            SkillProgramNumberExpression.LivingFactionCount => _coreRuleQueriesOperations.GetLivingFactionCount(),
            SkillProgramNumberExpression.LivingPlayerCount => _coreMatchParticipantsPlayersAccess._players.Count(player => player.IsAlive),
            SkillProgramNumberExpression.SelectedPairHandDifference => active.SelectedTargetSeats is { Count: 2 } pair ? Math.Abs(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[pair[0]]).Count - _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[pair[1]]).Count) : throw new InvalidOperationException("A selected-pair hand difference requires two resolved program targets."),
            _ => throw new InvalidOperationException("Unsupported owned-card selection amount.")};
        _corePindianHandReturnsOperations.FreezePindianHalfReturn(active, requested);
        active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var exact = GetOwnedSelectionEffect(active).RequireExactCount == true;
        if (exact && requested > candidates.Length)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(active, "没有足额的合法区域牌，未支付缩水费用。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        var count = exact ? requested : Math.Min(requested, candidates.Length);
        if (minimumCards > count)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(active, "没有足够的合法区域牌，技能结算已取消。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (count <= 0)
        {
            _coreProgramLifecycleOperations.SetProgramCardSet(frame.Id, resultBind, [], SkillProgramCardSetVisibility.Private, []);
            _coreOwnedExactCountAndNamedTargetsOperations.FreezeExactOwnedSelectionSuits(frame.Id, GetOwnedSelectionEffect(active), [], []);
            return SkillProgramStepOutcome.Continue;
        }

        if (expression == SkillProgramNumberExpression.AllOwnedZoneCards)
        {
            _coreProgramLifecycleOperations.SetProgramCardSet(frame.Id, resultBind, candidates.Select(item => item.Id).ToArray(), SkillProgramCardSetVisibility.Private, candidates.Select(item => item.Location).ToArray(), selectionActorSeat: cardOwnerSeat);
            return SkillProgramStepOutcome.Continue;
        }

        active = active with
        {
            OwnedCardSelection = new(cardOwnerSeat, resultBind, count, Array.AsReadOnly(candidates.Select(item => item.Id).ToArray()), Array.AsReadOnly(candidates.Select(item => item.Location).ToArray()), [], minimumCards, maximumCards > 0)
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(active);
        PublishProgramOwnedCardSelection(active);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void PublishProgramOwnedCardSelection(ProgramSkillFrame frame)
    {
        var draft = frame.OwnedCardSelection ?? throw new InvalidOperationException("Missing owned-card draft.");
        var selected = draft.SelectedCardIds.ToHashSet();
        var remaining = draft.RequiredCount - selected.Count;
        var choices = draft.CandidateCardIds.Select((id, index) => (Id: id, Location: draft.CandidateLocations[index])).Where(item => !selected.Contains(item.Id)).Select(item =>
        {
            var card = _cardZonesOperations._cardZones.CardsAt(item.Location).Single(card => card.Id == item.Id);
            var zoneName = item.Location.Zone switch
            {
                CardZoneKind.Hand => "手牌",
                CardZoneKind.Equipment => "装备",
                CardZoneKind.Judgment => "判定牌",
                CardZoneKind.PrivateReserve => "星",
                _ => throw new InvalidOperationException("Unsupported owned-card selection zone.")};
            return new PromptChoice(new ChoiceId($"program-owned-set.frame-{(frame.Id)}.pick-{(selected.Count)}.card-{(item.Id)}"), $"选择{(zoneName)}【{(card.DisplayName)}】（{(card.RankText)}）", [item.Id], [], new Dictionary<string, string> { ["program-action"] = "select-owned-cards", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["result-bind"] = draft.ResultBind, ["selection-index"] = selected.Count.ToString(System.Globalization.CultureInfo.InvariantCulture) });
        }).ToList();
        if (draft.AllowEarlyFinish && selected.Count >= draft.MinimumCount || GetOwnedSelectionEffect(frame).AllowDecline)
        {
            choices.Add(new PromptChoice(new ChoiceId($"program-owned-set.frame-{(frame.Id)}.finish-{(selected.Count)}"), $"完成选择（已选 {(selected.Count)} 张）。", [], [], new Dictionary<string, string> { ["program-action"] = "finish-owned-cards", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["result-bind"] = draft.ResultBind, ["selection-index"] = selected.Count.ToString(System.Globalization.CultureInfo.InvariantCulture) }));
        }

        var skill = _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, draft.CardOwnerSeat, draft.MinimumCount > 0 ? $"请选择自己的区域牌（至少 {(draft.MinimumCount)} 张，已选 {(selected.Count)} 张，最多还可选 {(remaining)} 张；完成前不移动牌）。" : $"请选择自己的区域牌（还需选择 {(remaining)} 张，选定前不移动牌）。", choices.SelectMany(choice => choice.Cards).ToArray(), [], SourceSeat: frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = draft.CardOwnerSeat,
            SkillPrompt = new(frame.SkillId, skill.Name, $"{(skill.Name)} · 选择区域牌", skill.Description),
            Choices = Array.AsReadOnly(choices.ToArray())
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[draft.CardOwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    internal void ResolveProgramOwnedCardSelection(ProgramSkillFrame frame, SkillProgramEffect effect, PromptChoice selected)
    {
        if (_coreForeignEquipmentDiscardPoliciesOperations.HasForeignDiscardCapability && frame.OwnedCardSelection is { } owned && _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != owned.CardOwnerSeat)
            throw new InvalidOperationException("Owned selection lost its actual choosing participant.");
        var deferred = effect.Op == SkillProgramEffectOp.ResolveDeferredHandAlignment;
        if (deferred)
            effect = _coreDeferredHandAlignmentsOperations.DeferredOwnedSelectionEffect(frame, effect);
        effect = _coreHalfHandPhaseDebtProgramsOperations.PhaseHandDebtOwnedSelectionEffect(frame, _coreEquipmentPairPaymentsOperations.EquipmentPairOwnedSelectionEffect(frame, _coreTurnDrawDebtsOperations.TurnDrawDebtOwnedSelectionEffect(frame, effect)));
        var draft = frame.OwnedCardSelection ?? throw new InvalidOperationException("Missing owned-card draft.");
        var finish = selected.Parameters.GetValueOrDefault("program-action") == "finish-owned-cards";
        if (effect.Op != SkillProgramEffectOp.SelectOwnedCards || draft.ResultBind != effect.ResultBind || draft.CardOwnerSeat != (effect.TargetReference is { } cardOwner ? _coreProgramCardPaymentsOperations.ResolveProgramParticipant(frame, cardOwner) : CoreGameEngineProgramChoicesRuntime.ResolveProgramEffectTarget(frame, effect.Target)) || selected.Parameters.GetValueOrDefault("result-bind") != draft.ResultBind || selected.Parameters.GetValueOrDefault("selection-index") != draft.SelectedCardIds.Count.ToString(System.Globalization.CultureInfo.InvariantCulture) || selected.Targets.Count != 0 || (finish ? !draft.AllowEarlyFinish && !effect.AllowDecline || draft.SelectedCardIds.Count < draft.MinimumCount && !effect.AllowDecline || selected.Cards.Count != 0 : selected.Parameters.GetValueOrDefault("program-action") != "select-owned-cards" || selected.Cards.Count != 1 || !draft.CandidateCardIds.Contains(selected.Cards[0]) || draft.SelectedCardIds.Contains(selected.Cards[0])))
            throw new InvalidOperationException("The owned-card choice does not match its suspended instruction.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[draft.CardOwnerSeat].IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(frame) && !_corePindianHandReturnsOperations.HasPaidPindianHandReturn(frame) || draft.CandidateCardIds.Where((id, index) => _cardZonesOperations._cardZones.GetLocation(id) != draft.CandidateLocations[index]).Any())
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "选牌参与者、技能实例或冻结来源已失效，未移动任何所选牌。");
            return;
        }

        var ids = finish ? draft.SelectedCardIds.ToArray() : draft.SelectedCardIds.Append(selected.Cards[0]).ToArray();
        if (_coreForcedPhaseAlcoholOperations.HasPhaseAlcoholChain(frame) && !_coreForcedPhaseAlcoholOperations.CanUsePhaseOwnerAlcohol(frame.WindowContext))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "该出牌阶段角色当前不能使用酒，未支付弃牌费用。");
            return;
        }

        if (ids.Any(id => !_coreTriggeredCardConversionsOperations.MatchesTriggeredSlashMaterial(frame, draft.ResultBind, draft.CardOwnerSeat, _coreCardAttackStateOperations.GetAttackCard(id), _cardZonesOperations._cardZones.GetLocation(id))))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "所选实体牌无法合法转化使用，未支付实体牌。");
            return;
        }

        if (ids.Any(id => !_coreTurnHandCategoryRestrictionsOperations.CanSelectSelfDiscardBinding(frame, draft.ResultBind, draft.CardOwnerSeat, _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(id)).Single(c => c.Id == id), _cardZonesOperations._cardZones.GetLocation(id))))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "所选手牌不能自行弃置，未移动费用。");
            return;
        }

        if (!finish && ids.Length < draft.RequiredCount)
        {
            frame = frame with
            {
                OwnedCardSelection = draft with
                {
                    SelectedCardIds = Array.AsReadOnly(ids)
                }
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(frame);
            PublishProgramOwnedCardSelection(frame);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { OwnedCardSelection = null });
        var locations = ids.Select(id => draft.CandidateLocations[draft.CandidateCardIds.ToList().IndexOf(id)]).ToArray();
        if (deferred)
        {
            _coreDeferredHandAlignmentsOperations.CompleteDeferredAlignmentDiscard(frame.Id, ids, locations);
            return;
        }

        _coreProgramLifecycleOperations.SetProgramCardSet(frame.Id, draft.ResultBind, ids, SkillProgramCardSetVisibility.Private, locations, ids.Length == 1 ? _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[draft.CardOwnerSeat], _cardZonesOperations._cardZones.CardsAt(locations[0]).Single(c => c.Id == ids[0])) : null, selectionActorSeat: draft.CardOwnerSeat);
        _coreOwnedExactCountAndNamedTargetsOperations.FreezeExactOwnedSelectionSuits(frame.Id, effect, ids, locations);
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }
}
