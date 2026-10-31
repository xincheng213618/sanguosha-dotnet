#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuBeiSpOl664Runtime
{
    internal const string OlSpZhujiu = "ol:zhujiu-3577", OlSpJinglei = "ol:jinglei-3578", OlSpBrewDisabled = "zhujiu-disabled";
    SkillProgramStepOutcome IOlSpLiuBei664Host.Execute(ProgramSkillFrame frame, SkillProgramEffect effect) => effect.Op switch
    {
        SkillProgramEffectOp.UseOlSpLiuBeiZhujiu => BeginOlSpZhujiu(frame),
        SkillProgramEffectOp.GrantOlSpLiuBeiXudai => GrantOlSpXudai(frame),
        SkillProgramEffectOp.ResolveOlSpLiuBeiJinglei when effect.StateId == "pay" => PayOlSpJinglei(frame),
        _ => BeginOlSpJingleiAdjustment(frame)};
    internal SkillProgramStepOutcome GrantOlSpXudai(ProgramSkillFrame frame)
    {
        if (frame.SkillId != "ol:xudai-3576" || frame.SelectedTargetSeats is not [var recipient] || recipient == frame.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[recipient].IsAlive)
            throw new InvalidOperationException("Xudai requires its original other recipient on the paid limited frame.");
        // Shared mutation and original paid frame; a limited grant is not an awakening.
        _coreProgramSkillChangesOperations.AcquireProgramSkillChange(_coreProgramSkillChangesOperations.PrepareProgramSkillChange(frame)with { Owner = _coreMatchParticipantsPlayersAccess._players[recipient] }, [OlSpZhujiu], announceAwakening: false);
        return SkillProgramStepOutcome.Continue;
    }

    internal int OlSpAlcoholCount(int owner)
    {
        var events = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var start = Array.FindLastIndex(events, item => item is TurnStartedEvent turn && turn.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && turn.ActorSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat);
        // Declaration facts cover physical, converted, quota-exempt and dying Alcohol Uses.
        return start < 0 ? 0 : events.Skip(start + 1).OfType<CardUseDeclaredEvent>().Where(item => item.SourceSeat == owner && item.CardKind == CardKind.Alcohol).Select(item => item.ResolutionId).Distinct().Count();
    }

    internal Card[] OlSpBrewMaterials(int owner) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment2(owner)).Where(card => !card.IsGeneralWeapon && !_coreCardUseModulesOperations.IsTurnHandCardRestricted(_coreMatchParticipantsPlayersAccess._players[owner], card) && _coreProgramTargetRulesOperations.TargetRules.AllowsNativeUseMaterial(new(owner, CardKind.Alcohol, [card.Id], null))).ToArray();
    internal bool CanOlSpBrew(CharacterState owner) => owner.IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat == owner.Seat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && !owner.HasAlcoholEffect && _characterHuangQuanOlOperations.CanUseJianjiOrdinaryAlcohol(owner, null) && !_coreCardUseModulesOperations.IsCardUseForbidden(owner.Seat, CardKind.Alcohol, CardActionType.Use) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(owner.Seat, owner.Seat, CardKind.Alcohol) && _skillRuntimeStateOperations._skillRuntimeState.GetUsage(owner.Seat, OlSpZhujiu, OlSpBrewDisabled, SkillUsageScope.Turn) == 0 && OlSpBrewMaterials(owner.Seat).Length > OlSpAlcoholCount(owner.Seat);
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 5431, SkillProgramEffectOp.UseOlSpLiuBeiZhujiu)]
    internal bool CanOfferOlSpBrew(CharacterState owner) => CanOlSpBrew(owner);
    internal bool CanOlSpDyingBrew(int owner, ProgramSkillWindowContext? context) => _coreDyingCursorRuntimeOperations.Value.ActiveDying is { } dying && context is { Window: SkillProgramTriggerWindow.SelfDyingResponse } && context.ParentFrameId == dying.Id && context.TargetSeat == owner && dying.VictimSeat == owner && dying.ResponderSeat == owner && _coreMatchParticipantsPlayersAccess._players[owner].IsAlive && _coreMatchParticipantsPlayersAccess._players[owner].Hp <= 0 && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && !_characterZhouYuOlMouOperations.MouGuquPhysicalUseForbidden(owner, []) && !_coreAlcoholIdentityPoliciesOperations.IsForeignTurnAlcoholUseForbidden(owner, CardKind.Alcohol, CardActionType.Use) && !_coreCardUseModulesOperations.IsCardUseForbidden(owner, CardKind.Alcohol, CardActionType.Use) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(owner, owner, CardKind.Alcohol) && _skillRuntimeStateOperations._skillRuntimeState.GetUsage(owner, OlSpZhujiu, OlSpBrewDisabled, SkillUsageScope.Turn) == 0 && OlSpBrewMaterials(owner).Length > OlSpAlcoholCount(owner);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 5431)]
    internal bool CanOfferOlSpDyingBrew(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => !trigger.Effects.Any(effect => effect.Op == SkillProgramEffectOp.UseOlSpLiuBeiZhujiu) || candidate.SkillId == OlSpZhujiu && CanOlSpDyingBrew(candidate.OwnerSeat, context);
    internal SkillProgramStepOutcome BeginOlSpZhujiu(ProgramSkillFrame frame)
    {
        var dying = frame.WindowContext?.Window == SkillProgramTriggerWindow.SelfDyingResponse;
        if (frame.OlSpLiuBei664 is not null || frame.SkillId != OlSpZhujiu || !(dying ? CanOlSpDyingBrew(frame.OwnerSeat, frame.WindowContext) : CanOlSpBrew(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat])))
            throw new InvalidOperationException("Zhujiu requires its current unpaid ordinary Alcohol allowance.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { OlSpLiuBei664 = new() { Minimum = checked(OlSpAlcoholCount(frame.OwnerSeat) + 1), TurnNumber = _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, DyingFrameId = dying ? _coreDyingCursorRuntimeOperations.Value.ActiveDying!.Id : null } });
        PublishOlSpLiuBeiChoice(frame);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal bool HasOlSpZhujiu(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive && _coreMatchParticipantsPlayersAccess._players[seat].SkillGrants.Grants.Any(grant => grant.SkillId == OlSpZhujiu);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 5432)]
    internal bool CanOfferOlSpJinglei(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => !trigger.Effects.Any(effect => effect.Op == SkillProgramEffectOp.ResolveOlSpLiuBeiJinglei && effect.StateId == "pay") || candidate.SkillId == OlSpJinglei && _coreDyingCursorRuntimeOperations.Value.ActiveDying is null && !_coreMatchParticipantsPlayersAccess._players.Any(player => player.IsAlive && player.Hp <= 0) && _coreMatchParticipantsPlayersAccess._players.Any(player => HasOlSpZhujiu(player.Seat)) && context.Window == SkillProgramTriggerWindow.CardUseCompleted && _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().Any(window => window.Id == context.ParentFrameId && window.Action.Type == CardActionType.Use && window.Action.EffectiveKind == CardKind.Alcohol);
    internal SkillProgramStepOutcome PayOlSpJinglei(ProgramSkillFrame frame)
    {
        if (frame.SkillId != OlSpJinglei || frame.InstructionIndex != 1 || frame.OlSpLiuBei664 is not null || _coreDyingCursorRuntimeOperations.Value.ActiveDying is not null || _coreMatchParticipantsPlayersAccess._players.Any(player => player.IsAlive && player.Hp <= 0) || frame.WindowContext is not { } context || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(window => window.Id == context.ParentFrameId)is not { } parent || parent.Action.Type != CardActionType.Use || parent.Action.EffectiveKind != CardKind.Alcohol)
            throw new InvalidOperationException("Jinglei requires its actual completed Alcohol and no current dying character.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { OlSpLiuBei664 = new() { Stage = "damage", TurnNumber = _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, UseId = parent.ParentFrameId } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OlSpLiuBeiJingleiPaidEvent(frame.Id, frame.OwnerSeat, parent.ParentFrameId));
        return _coreSkillProgramsOperations.BeginProgramSkillDamage(frame, frame.OwnerSeat, 1, nature: DamageNature.Thunder, sourceLess: true);
    }

    internal SkillProgramStepOutcome BeginOlSpJingleiAdjustment(ProgramSkillFrame frame)
    {
        if (frame.OlSpLiuBei664 is not { Stage: "damage" } || frame.InstructionIndex != 2 || frame.AttackAttempt is not null || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<OlSpLiuBeiJingleiPaidEvent>().Count(item => item.FrameId == frame.Id && item.OwnerSeat == frame.OwnerSeat) != 1)
            throw new InvalidOperationException("Jinglei's adjustment requires its original already-resolved source-less thunder payment.");
        if (!_coreMatchParticipantsPlayersAccess._players.Any(player => HasOlSpZhujiu(player.Seat)))
            return SkillProgramStepOutcome.Continue;
        if (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive)
        {
            BeginOlSpHandAdjustment(frame, _coreMatchParticipantsPlayersAccess._players.First(player => HasOlSpZhujiu(player.Seat)).Seat);
            return SkillProgramStepOutcome.AwaitChild;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { OlSpLiuBei664 = frame.OlSpLiuBei664 with { Stage = "recipient" } });
        PublishOlSpLiuBeiChoice(frame);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> OlSpLiuBeiChoices(ProgramSkillFrame frame)
    {
        var draft = frame.OlSpLiuBei664!;
        PromptChoice Choice(string id, string label, IReadOnlyList<int>? cards = null, IReadOnlyList<int>? targets = null) => new(new($"sp-liubei.{(frame.Id)}.{(id)}"), label, cards ?? [], targets ?? [], new Dictionary<string, string> { ["program-action"] = "ol-sp-liubei-664", ["stage"] = draft.Stage });
        if (draft.Stage == "recipient")
            return _coreMatchParticipantsPlayersAccess._players.Where(player => HasOlSpZhujiu(player.Seat)).Select(player => Choice("recipient." + player.Seat, "令" + player.Name + "调整手牌", targets: [player.Seat])).ToArray();
        if (draft.Stage == "discard")
            return _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[draft.TargetSeat!.Value]).Where(card => !draft.Cards.Contains(card.Id)).Select(card => Choice("discard." + card.Id, "弃置【" + card.DisplayName + $"】（{(draft.Cards.Count + 1)}/{(draft.Minimum)}）", [card.Id])).ToArray();
        if (draft.Stage != "materials")
            throw new InvalidOperationException("SP Liu Bei has no unpaid selection at this stage.");
        var choices = OlSpBrewMaterials(frame.OwnerSeat).Where(card => !draft.Cards.Contains(card.Id)).Select(card => Choice("material." + card.Id, "选择【" + card.DisplayName + "】煮酒", [card.Id])).ToList();
        if (draft.Cards.Count >= draft.Minimum)
            choices.Add(Choice("use", $"以{(draft.Cards.Count)}张牌使用酒"));
        choices.Add(Choice("cancel", "取消煮酒"));
        return choices;
    }

    internal void PublishOlSpLiuBeiChoice(ProgramSkillFrame frame)
    {
        var choices = OlSpLiuBeiChoices(frame);
        var draft = frame.OlSpLiuBei664!;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { OlSpLiuBei664 = draft with { Choices = choices } });
        var chooser = draft.Stage == "discard" ? draft.TargetSeat!.Value : frame.OwnerSeat;
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, draft.Stage == "materials" ? $"煮酒：选择至少{(draft.Minimum)}张牌。" : draft.Stage == "recipient" ? "惊雷：选择一名拥有煮酒的角色。" : "惊雷：弃置多于体力上限的手牌。", choices.SelectMany(choice => choice.Cards).Distinct().ToArray(), choices.SelectMany(choice => choice.Targets).Distinct().ToArray(), frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Choices = choices,
            IsPrivate = draft.Stage != "recipient",
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAiChoice(SkillProgramEffectOp.UseOlSpLiuBeiZhujiu, SkillProgramEffectOp.ResolveOlSpLiuBeiJinglei)]
    internal PromptChoice SelectAiOlSpLiuBei664(PendingDecision decision, ProgramSkillFrame frame)
    {
        if (frame.OlSpLiuBei664 is not { } draft || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != frame.Id || decision.Kind != DecisionKind.ProgramTrigger || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PromptId != decision.PromptId || decision.PlayerSeat != (draft.Stage == "discard" ? draft.TargetSeat : frame.OwnerSeat))
            throw new InvalidOperationException("SP Liu Bei AI lost its actual issued chooser and original program frame.");
        var current = OlSpLiuBeiChoices(frame);
        var legal = decision.Choices.Where(choice => draft.Choices.Any(issued => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([issued], [choice])) && current.Any(candidate => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([candidate], [choice]))).ToArray();
        if (legal.Length == 0)
            throw new InvalidOperationException("SP Liu Bei AI has no currently legal issued choice.");
        if (draft.Stage == "recipient")
        {
            int Benefit(PromptChoice choice)
            {
                var seat = choice.Targets.Single();
                var target = _coreMatchParticipantsPlayersAccess._players[seat];
                var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(target).Count;
                var draw = Math.Max(0, Math.Min(5, target.MaxHp) - hand);
                var discard = Math.Max(0, hand - target.MaxHp);
                return seat == frame.OwnerSeat ? 2 * draw - discard : discard + (_coreTeamActivityProgramsOperations.OtherRevealedPublicTeamAlly(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], target) ? 2 * draw : -2 * draw);
            }

            return legal.OrderByDescending(Benefit).ThenBy(choice => choice.Targets.Single()).First();
        }

        if (draft.Stage == "discard")
            return legal.Where(choice => choice.Cards.Count == 1).OrderBy(choice => CardCatalog.Get(_coreCardAttackStateOperations.GetAttackCard(choice.Cards[0]).Kind).HandKeepValue).ThenBy(choice => choice.Cards[0]).First();
        if (draft.Stage != "materials")
            throw new InvalidOperationException("SP Liu Bei AI cannot choose during a paid native child.");
        var canPay = draft.DyingFrameId is not null ? CanOlSpDyingBrew(frame.OwnerSeat, frame.WindowContext) : CanOlSpBrew(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]);
        var materials = legal.Where(choice => choice.Cards.Count == 1).ToArray();
        if (!canPay || draft.Minimum - draft.Cards.Count > materials.Length)
            return legal.Single(choice => choice.Id.Value.EndsWith(".cancel", StringComparison.Ordinal));
        if (draft.Cards.Count >= draft.Minimum)
            return legal.Single(choice => choice.Id.Value.EndsWith(".use", StringComparison.Ordinal));
        return materials.OrderByDescending(choice => _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], _coreCardAttackStateOperations.GetAttackCard(choice.Cards[0])) == Suit.Club).ThenBy(choice => CardCatalog.Get(_coreCardAttackStateOperations.GetAttackCard(choice.Cards[0]).Kind).HandKeepValue).ThenBy(choice => choice.Cards[0]).First();
    }

    [ProgramAnswer("ol-sp-liubei-664")]
    internal void AnswerOlSpLiuBei(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { OlSpLiuBei664: { } draft } frame || draft.Choices.SingleOrDefault(choice => choice.Id == selected.Id)is not { } choice || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], [selected]) || !OlSpLiuBeiChoices(frame).Any(current => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], [current])))
            throw new InvalidOperationException("SP Liu Bei lost its issued selection and current owned entities.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (draft.Stage == "recipient")
        {
            BeginOlSpHandAdjustment(frame, choice.Targets.Single());
            return;
        }

        if (choice.Cards is [var card])
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { OlSpLiuBei664 = draft with { Cards = draft.Cards.Append(card).ToArray() } });
            if (draft.Stage == "discard" && frame.OlSpLiuBei664!.Cards.Count == draft.Minimum)
            {
                CommitOlSpJingleiDiscard(frame);
                return;
            }

            PublishOlSpLiuBeiChoice(frame);
            return;
        }

        if (choice.Id.Value.EndsWith(".cancel", StringComparison.Ordinal))
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { OlSpLiuBei664 = null });
            _coreSkillProgramsOperations.FinishProgramSkill(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), false);
            return;
        }

        CommitOlSpZhujiu(frame);
    }

    internal void CommitOlSpZhujiu(ProgramSkillFrame frame)
    {
        var draft = frame.OlSpLiuBei664!;
        var actor = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        var dying = draft.DyingFrameId is not null;
        if (!(dying ? CanOlSpDyingBrew(actor.Seat, frame.WindowContext) && _coreDyingCursorRuntimeOperations.Value.ActiveDying?.Id == draft.DyingFrameId : CanOlSpBrew(actor)) || draft.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || draft.Minimum != OlSpAlcoholCount(actor.Seat) + 1 || draft.Cards.Count < draft.Minimum || draft.Cards.Distinct().Count() != draft.Cards.Count || draft.Cards.Any(id => !OlSpBrewMaterials(actor.Seat).Any(card => card.Id == id)))
            throw new InvalidOperationException("Zhujiu's exact materials or ordinary Wine allowance changed before payment.");
        var cards = draft.Cards.Select(id => _coreCardAttackStateOperations.GetAttackCard(id)).ToArray();
        var allClub = cards.All(card => _coreNativeCardAppearanceOperations.EffectiveSuit(actor, card) == Suit.Club);
        var source = new CardConversionSource(frame.SkillId, frame.ActivationId!, actor.Seat, frame.SkillInstanceId);
        var id = _coreNativeCardUseOperations.BeginCardUse(cards[0], actor.Seat, [actor.Seat], CardKind.Alcohol, physicalCardIds: draft.Cards, conversionSource: source);
        _coreRuntimeOperations.ReplaceRuntimeFrame(frame.Id, frame with { OlSpLiuBei664 = draft with { Stage = "wine", AllClub = allClub, UseId = id, SequenceBefore = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Choices = [] } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OlSpLiuBeiZhujiuIssuedEvent(frame.Id, actor.Seat, id, draft.Minimum, cards.Length, allClub, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber));
        foreach (var material in cards)
            _corePhysicalMovementFacadesOperations.MoveCard(material, _cardZonesOperations._cardZones.GetLocation(material.Id), CardLocation.Processing, CardMoveReasons.Use, disclosure: CardMovementDisclosure.FaceUp);
        var paid = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Single(parent => parent.Id == frame.Id);
        _coreRuntimeOperations.ReplaceRuntimeFrame(frame.Id, paid with { OlSpLiuBei664 = paid.OlSpLiuBei664!with { SequenceAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        if (!dying)
            _characterHuangGaiMouMobileOperations.RecordMouZhaJiangAwareAlcoholDebit(id, actor);
        _coreCardUseLifecycleOperations.BeginSimpleCardUse(id, new(cards[0].Id, dying ? SimpleCardUseEffect.Recovery : SimpleCardUseEffect.Alcohol, 1));
    }

    [NativeUseReturn(5431)]
    internal bool ReturnOlSpZhujiu(CardUseFrame use)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { OlSpLiuBei664: { Stage: "wine" } draft } frame || draft.UseId != use.Id)
            return false;
        if (frame.SkillId != OlSpZhujiu || use.CardKind != CardKind.Alcohol || use.SourceSeat != frame.OwnerSeat || use.Action?.Type != CardActionType.Use || !(use.PhysicalCardIds ?? []).SequenceEqual(draft.Cards) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<OlSpLiuBeiZhujiuIssuedEvent>().Count(item => item.FrameId == frame.Id && item.UseId == use.Id) != 1)
            throw new InvalidOperationException("Zhujiu lost its original paid native multi-entity Wine return.");
        if (!draft.AllClub && draft.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber)
        {
            _skillRuntimeStateOperations._skillRuntimeState.TryConsumeUsage(frame.OwnerSeat, OlSpZhujiu, OlSpBrewDisabled, SkillUsageScope.Turn, 1);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OlSpLiuBeiZhujiuDisabledEvent(frame.Id, frame.OwnerSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber));
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OlSpLiuBeiZhujiuReturnedEvent(frame.Id, frame.OwnerSeat, use.Id, draft.DyingFrameId));
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { OlSpLiuBei664 = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        return true;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.ReturnCompletedProgramBinding, 5431)]
    internal bool ReturnOlSpZhujiuDyingBinding(ProgramSkillFrame frame, bool completed)
    {
        if (!completed || frame.SkillId != OlSpZhujiu || frame.WindowContext is not { Window: SkillProgramTriggerWindow.SelfDyingResponse } context || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<OlSpLiuBeiZhujiuReturnedEvent>().Count(item => item.FrameId == frame.Id && item.OwnerSeat == frame.OwnerSeat && item.DyingFrameId == context.ParentFrameId) != 1)
            return false;
        // Reuse the native dying return; only this already-paid, completed conversion
        // can re-enter the same dying occurrence with the now-increased material price.
        _coreProgramLifecycleOperations.CompleteDyingProgramBinding(frame, completed);
        if (_coreDyingCursorRuntimeOperations.Value.ActiveDying is { } dying && dying.Id == context.ParentFrameId && dying.VictimSeat == frame.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].Hp <= 0)
        {
            var key = CoreGameEngineProgramLifecycleRuntime.SelfDyingBindingKey(frame.SkillId, frame.TriggerId!, frame.SkillInstanceId);
            _coreNativeDyingFramesOperations.UpdateDyingFrame(dying.Id, current => current with { AttemptedSelfDyingBindings = current.AttemptedSelfDyingBindings.Where(binding => binding != key).ToArray() });
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreNativeDyingFramesOperations.SetDyingFrameStep(dying.Id, ResolutionFrameStep.AwaitingResponse);
            _coreMatchOutcomeStatusWinnerAccess._status = EngineStatus.Running;
            _coreNativeDyingOperations.ExposeHumanDyingPrompt();
        }

        return true;
    }

    internal void BeginOlSpHandAdjustment(ProgramSkillFrame frame, int target)
    {
        if (!HasOlSpZhujiu(target))
            throw new InvalidOperationException("Jinglei requires its selected living Zhujiu holder.");
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Count;
        var maximum = _coreMatchParticipantsPlayersAccess._players[target].MaxHp;
        var draw = Math.Max(0, Math.Min(5, maximum) - hand);
        if (hand > maximum)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { OlSpLiuBei664 = frame.OlSpLiuBei664!with { Stage = "discard", TargetSeat = target, Minimum = hand - maximum, Cards = [], Choices = [] } });
            PublishOlSpLiuBeiChoice(frame);
            return;
        }

        if (draw == 0)
        {
            CompleteOlSpAdjustment(frame);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { OlSpLiuBei664 = frame.OlSpLiuBei664!with { Stage = "drawing", TargetSeat = target, Minimum = draw, Choices = [] } });
        _coreProgramNativeDrawOperations.BeginOperationProgramDraw(frame, target, draw, new("skill.ol-sp-liu-bei.jinglei.draw"));
    }

    internal void CommitOlSpJingleiDiscard(ProgramSkillFrame frame)
    {
        var draft = frame.OlSpLiuBei664!;
        var target = draft.TargetSeat!.Value;
        if (draft.Cards.Count != draft.Minimum || draft.Cards.Any(id => !_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Any(card => card.Id == id)))
            throw new InvalidOperationException("Jinglei lost its selected actual hand discards.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { OlSpLiuBei664 = draft with { Stage = "discarding", SequenceBefore = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Choices = [] } });
        foreach (var card in draft.Cards.Select(_coreCardAttackStateOperations.GetAttackCard).ToArray())
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Hand(target), CardLocation.DiscardPile, new("skill.ol-sp-liu-bei.jinglei.discard"));
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { OlSpLiuBei664 = frame.OlSpLiuBei664!with { SequenceAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        if (_coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat) == SkillProgramStepOutcome.Continue)
            ContinueOlSpAdjustment(frame.Id);
    }

    internal void ContinueOlSpAdjustment(long id)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var draft = frame.OlSpLiuBei664!;
        if (draft.Stage == "discarding" && draft.TargetSeat != frame.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive)
        {
            if (draft.Cards.Any(card => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(move => move.Sequence > draft.SequenceBefore && move.Sequence <= draft.SequenceAfter && move.CardId == card && move.From == CardLocation.Hand(draft.TargetSeat!.Value) && move.To == CardLocation.DiscardPile && move.Reason.Value == "skill.ol-sp-liu-bei.jinglei.discard") != 1))
                throw new InvalidOperationException("Jinglei can transfer only the exact entities actually discarded by this paid adjustment.");
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { OlSpLiuBei664 = draft with { Stage = "giving" }, PendingMovementContinuation = null });
            foreach (var card in draft.Cards.Where(card => _cardZonesOperations._cardZones.GetLocation(card) == CardLocation.DiscardPile).Select(_coreCardAttackStateOperations.GetAttackCard).ToArray())
                _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DiscardPile, CardLocation.Hand(frame.OwnerSeat), new("skill.ol-sp-liu-bei.jinglei.give"));
            if (_coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat) == SkillProgramStepOutcome.AwaitChild)
                return;
        }

        CompleteOlSpAdjustment(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
    }

    internal void CompleteOlSpAdjustment(ProgramSkillFrame frame)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { OlSpLiuBei664 = null, PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveOlSpLiuBeiJinglei)]
    internal void ReturnOlSpJingleiDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.OlSpLiuBei664 is not { Stage: "drawing" } draft || outcome.RecipientSeat != draft.TargetSeat || outcome.RequestedCount != draft.Minimum)
            throw new InvalidOperationException("Jinglei's draw lost its selected holder and paid bounded refill.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { OlSpLiuBei664 = draft with { Stage = "drawing-return" } });
        if (_coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat) == SkillProgramStepOutcome.Continue)
            CompleteOlSpAdjustment(frame);
    }
    [ProgramContinuation(ProgramContinuationKind.Resume, 5431)]
    internal bool ResumeOlSpJingleiMovement(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { OlSpLiuBei664.Stage: "discarding" or "giving" or "drawing-return" } frame || frame.Id != id)
            return false;
        ContinueOlSpAdjustment(id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 5431)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 5431)]
    internal bool CanContinuePaidOlSpLiuBei664(ProgramSkillFrame frame) => frame.OlSpLiuBei664 is { } draft && (frame.SkillId == OlSpZhujiu && draft.Stage == "wine" && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<OlSpLiuBeiZhujiuIssuedEvent>().Count(item => item.FrameId == frame.Id && item.OwnerSeat == frame.OwnerSeat && item.UseId == draft.UseId && item.PaidCount == draft.Cards.Count) == 1 || frame.SkillId == OlSpJinglei && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<OlSpLiuBeiJingleiPaidEvent>().Count(item => item.FrameId == frame.Id && item.OwnerSeat == frame.OwnerSeat && item.AlcoholUseId == draft.UseId) == 1);
}
