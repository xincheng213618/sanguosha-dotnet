#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePublicPersistentPilesRuntime
{
    SkillProgramStepOutcome IPublicPersistentPileProgramHost.ExecutePublicPile(SkillProgramEffect effect, ProgramSkillFrame frame) => ExecutePublicPile(effect, frame);
    internal SkillProgramStepOutcome ExecutePublicPile(SkillProgramEffect effect, ProgramSkillFrame frame)
    {
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        if (!owner.IsAlive)
            return SkillProgramStepOutcome.Continue;
        if (effect.Op == SkillProgramEffectOp.StoreTopCardInPublicPile)
        {
            var source = _coreGameEnginePublicPileSourcesPublicPersistentPilesAccess._publicPersistentPiles.GetValueOrDefault((frame.OwnerSeat, frame.SkillId, frame.SkillInstanceId));
            if (source is not null && source.Capacity != effect.Amount || !_corePublicPileSourcesOperations.SupportsMultiplePublicPiles && source is null && _corePublicPileSourcesOperations.PublicPileSources(owner.Seat).Any())
                throw new InvalidOperationException("A public pile cannot mix source skill instances or capacities.");
            if (source is not null && _corePublicPileSourcesOperations.PublicPileCards(source).Count >= effect.Amount)
                return SkillProgramStepOutcome.Continue;
            var card = _corePhysicalDrawFacadesOperations.DrawOneToProcessing(new("skill-program.public-pile.top"));
            if (card is null)
                return SkillProgramStepOutcome.Continue;
            source ??= _corePublicPileSourcesOperations.EnsurePublicPileSource(frame, effect.Amount);
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { PendingMovementContinuation = new(owner.Seat, 0, null) });
            _coreAdvancedWeaponsOperations.MoveProcessingCardUnlessDestroyed(card, source.Location, new("skill-program.public-pile.store"));
            if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
                _coreRuntimeOperations.ReturnRuntimeProgramMovement(frame.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        var sources = _corePublicPileSourcesOperations.ReferencedPublicPileSources(owner.Seat, effect.SkillIds.Single(), frame.SkillInstanceId).Where(s => effect.Op == SkillProgramEffectOp.ExchangePublicPileHand || s.Capacity == effect.Amount).Where(s => _corePublicPileSourcesOperations.PublicPileCards(s).Count > 0).Where(s => PublicPileSourceSatisfiesFrozenTrigger(frame, s)).ToArray();
        if (sources.Length == 0)
            return SkillProgramStepOutcome.Continue;
        if (frame.PublicPileDraft is null)
        {
            if (sources.Length > 1)
                frame = frame with
                {
                    PublicPileDraft = new(owner.Seat, effect.SkillIds.Single(), "source", -1, [], [], [], [])
                };
            else
            {
                frame = frame with
                {
                    PublicPileDraft = CreatePublicPileDraft(frame, effect, sources[0])
                };
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(frame);
        }

        PublishPublicPileChoice(frame);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal ProgramPublicPileDraft CreatePublicPileDraft(ProgramSkillFrame frame, SkillProgramEffect effect, PublicPersistentPileSource source) => new(source.OwnerSeat, source.SkillId, "owned", source.Capacity, [], [], [], [], _corePublicPileSourcesOperations.SupportsMultiplePublicPiles ? source.SkillInstanceId : null, _corePublicPileSourcesOperations.SupportsMultiplePublicPiles ? source.Location : null);
    internal bool PublicPileSourceSatisfiesFrozenTrigger(ProgramSkillFrame frame, PublicPersistentPileSource source)
    {
        if (frame.WindowContext?.Facts is not { } facts || frame.TriggerId is not { } triggerId)
            return true;
        var trigger = ProgramInstructionResolver.Default.FindTrigger(_contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!, triggerId)!;
        var count = facts.PublicPersistentPileCounts?.GetValueOrDefault(CoreGameEnginePublicPileSourcesRuntime.PublicPileIdentity(source.SkillId, source.SkillInstanceId)) ?? _corePublicPileSourcesOperations.PublicPileCards(source).Count;
        return trigger.Condition.Evaluate(facts with { PublicPersistentPileCounts = null, OwnedZoneCounts = facts.OwnedZoneCounts with { PublicPersistentPile = count } }, frame.SkillId, frame.SkillInstanceId);
    }

    internal static string PublicPileCardLabel(Card card) => RuntimeSkills.RuntimeRules_Core_PublicPersistentPiles.PublicPileCardLabel(card);
    internal void PublishPublicPileChoice(ProgramSkillFrame frame)
    {
        var draft = frame.PublicPileDraft!;
        var handOnly = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect.Op == SkillProgramEffectOp.ExchangePublicPileHand;
        var choices = new List<PromptChoice>();
        void Add(string token, string label, IReadOnlyList<int> cards, IReadOnlyList<int> seats) => choices.Add(new(new ChoiceId($"public-pile.frame-{(frame.Id)}.{(draft.Stage)}.{(token)}"), label, cards, seats, new Dictionary<string, string> { ["program-action"] = "public-pile", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["stage"] = draft.Stage, ["token"] = token }));
        if (draft.Stage == "source")
        {
            var paused = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
            foreach (var source in _corePublicPileSourcesOperations.ReferencedPublicPileSources(draft.OwnerSeat, draft.SourceSkillId, frame.SkillInstanceId).Where(s => (paused.Op == SkillProgramEffectOp.ExchangePublicPileHand || s.Capacity == paused.Amount) && _corePublicPileSourcesOperations.PublicPileCards(s).Count > 0 && PublicPileSourceSatisfiesFrozenTrigger(frame, s)))
                Add("source-" + CoreGameEnginePublicPileSourcesRuntime.PublicPileIdentity(source.SkillId, source.SkillInstanceId), (_contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).ProgramPresentation?.AuthorityName ?? "公开牌堆") + " · " + string.Join("、", _corePublicPileSourcesOperations.PublicPileCards(source).Take(3).Select(PublicPileCardLabel)) + " · " + _corePublicPileSourcesOperations.PublicPileCards(source).Count + "张", [], []);
        }
        else if (draft.Stage == "owned")
        {
            if (draft.OwnedIds.Count < _corePublicPileSourcesOperations.PublicPileDraftCards(draft).Count)
                foreach (var card in PublicPileExchangeOwnedCards(frame).Where(card => !draft.OwnedIds.Contains(card.Id)))
                    Add("card-" + card.Id, "换入【" + card.DisplayName + " " + CoreNativeCardClassificationsRuntime.GetSuitDisplayName(card.Suit) + card.RankText + "】", [card.Id], []);
            Add("finish", draft.OwnedIds.Count == 0 ? (handOnly ? "不交换" : "不交换，检查公开牌堆花色") : "完成换入牌选择", [], []);
        }
        else if (draft.Stage == "pile")
            foreach (var card in _corePublicPileSourcesOperations.PublicPileDraftCards(draft).Where(card => !draft.PileIds.Contains(card.Id)))
                Add("card-" + card.Id, "取出【" + card.DisplayName + " " + CoreNativeCardClassificationsRuntime.GetSuitDisplayName(card.Suit) + card.RankText + "】", [card.Id], []);
        else
            foreach (var id in draft.RemainingIds)
                foreach (var seat in _coreBoundPublicPileProgramsOperations.PublicPileDistributionTargets(frame))
                    Add($"card-{(id)}.target-{(seat)}", "将【" + PublicPileCardLabel(_corePublicPileSourcesOperations.PublicPileDraftCards(draft).Single(card => card.Id == id)) + "】交给 " + _coreMatchParticipantsPlayersAccess._players[seat].Name, [id], [seat]);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, draft.OwnerSeat, draft.Stage == "owned" ? (handOnly ? "选择任意张手牌，与等量公开牌堆牌交换。" : "选择任意张手牌或装备牌，与等量公开牌堆牌交换。") : draft.Stage == "pile" ? "选择等量公开牌堆牌取回手牌。" : "将全部公开牌堆牌分配给其他角色。", choices.SelectMany(choice => choice.Cards).Distinct().ToArray(), choices.SelectMany(choice => choice.Targets).Distinct().ToArray(), draft.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = draft.Stage == "owned",
            TargetSeat = draft.OwnerSeat,
            Choices = draft.Stage == "distribute" ? _coreCardDistributionPresentationOperations.PresentCardDistribution(frame, choices, "public-pile") : choices.ToArray(),
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[draft.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanCardSelection : EngineStatus.Running;
    }

    [ProgramAnswer("public-pile")]
    internal void ResolvePublicPileChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Missing public pile program.");
        var effect = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
        AssertPublicPileDraft(frame, effect);
        var draft = frame.PublicPileDraft!;
        if (selected.Parameters.GetValueOrDefault("stage") != draft.Stage || selected.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture))
            throw new InvalidOperationException("Public pile choice lost its suspended stage.");
        if (draft.Stage == "source")
        {
            var source = _corePublicPileSourcesOperations.ReferencedPublicPileSources(draft.OwnerSeat, draft.SourceSkillId, frame.SkillInstanceId).Single(s => "source-" + CoreGameEnginePublicPileSourcesRuntime.PublicPileIdentity(s.SkillId, s.SkillInstanceId) == selected.Parameters.GetValueOrDefault("token"));
            if (!PublicPileSourceSatisfiesFrozenTrigger(frame, source) || _corePublicPileSourcesOperations.PublicPileCards(source).Count == 0 || selected.Cards.Count != 0 || selected.Targets.Count != 0)
                throw new InvalidOperationException("Public pile source selection lost its frozen qualification.");
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            if (effect.Op == SkillProgramEffectOp.SelectPublicPile)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(frame with { PublicPileDraft = null });
                _coreBoundPublicPileProgramsOperations.BindSelectedPublicPile(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), effect, source);
                _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
                return;
            }

            frame = frame with
            {
                PublicPileDraft = CreatePublicPileDraft(frame, effect, source)
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(frame);
            PublishPublicPileChoice(frame);
            return;
        }

        var finish = selected.Parameters.GetValueOrDefault("token") == "finish";
        if (finish)
        {
            if (draft.Stage != "owned" || selected.Cards.Count != 0 || selected.Targets.Count != 0)
                throw new InvalidOperationException("Only the owned selection may finish early.");
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            if (draft.OwnedIds.Count == 0)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(frame with { PublicPileDraft = null });
                _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
                return;
            }

            draft = draft with
            {
                Stage = "pile"
            };
        }
        else
        {
            if (selected.Cards.Count != 1)
                throw new InvalidOperationException("One distinct physical card is required.");
            var id = selected.Cards.Single();
            var location = _cardZonesOperations._cardZones.GetLocation(id);
            if (draft.Stage == "owned")
            {
                if (selected.Targets.Count != 0 || draft.OwnedIds.Contains(id) || draft.OwnedIds.Count >= _corePublicPileSourcesOperations.PublicPileDraftCards(draft).Count || location.OwnerSeat != draft.OwnerSeat || location.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || effect.Op == SkillProgramEffectOp.ExchangePublicPileHand && location.Zone != CardZoneKind.Hand)
                    throw new InvalidOperationException("Invalid owned public-pile exchange payment.");
                _coreNativePromptBoundaryOperations.ClearPendingDecision();
                draft = draft with
                {
                    OwnedIds = draft.OwnedIds.Append(id).ToArray(),
                    OwnedLocations = draft.OwnedLocations.Append(location).ToArray()
                };
                if (draft.OwnedIds.Count == _corePublicPileSourcesOperations.PublicPileDraftCards(draft).Count)
                    draft = draft with
                    {
                        Stage = "pile"
                    };
            }
            else if (draft.Stage == "pile")
            {
                if (selected.Targets.Count != 0 || location != _corePublicPileSourcesOperations.PublicPileDraftLocation(draft) || draft.PileIds.Contains(id))
                    throw new InvalidOperationException("Invalid public-pile withdrawal.");
                var ids = draft.PileIds.Append(id).ToArray();
                _coreNativePromptBoundaryOperations.ClearPendingDecision();
                draft = draft with
                {
                    PileIds = ids
                };
                if (ids.Length == draft.OwnedIds.Count)
                {
                    // Validate both sides before the first move; settle the entire swap before observers run.
                    if (draft.OwnedIds.Where((cardId, index) => _cardZonesOperations._cardZones.GetLocation(cardId) != draft.OwnedLocations[index]).Any())
                        throw new InvalidOperationException("Exchange payment moved before commit.");
                    _coreRuntimeOperations.ReplaceRuntimeTop(frame with { PublicPileDraft = null, PendingMovementContinuation = new(draft.OwnerSeat, 0, null) });
                    var entering = draft.OwnedIds.Select(cardId => _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(cardId)).Single(card => card.Id == cardId)).ToArray();
                    _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(draft.OwnedIds, CardLocation.Processing, new("skill-program.public-pile.exchange-payment"), disclosure: CardMovementDisclosure.FaceUp);
                    _corePhysicalMovementFacadesOperations.MoveCards(_corePublicPileSourcesOperations.PublicPileDraftCards(draft).Where(card => ids.Contains(card.Id)).ToArray(), _corePublicPileSourcesOperations.PublicPileDraftLocation(draft), CardLocation.Hand(draft.OwnerSeat), new("skill-program.public-pile.exchange-obtain"));
                    foreach (var card in entering)
                        _coreAdvancedWeaponsOperations.MoveProcessingCardUnlessDestroyed(card, _corePublicPileSourcesOperations.PublicPileDraftLocation(draft), new("skill-program.public-pile.exchange-deposit"));
                    if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
                        _coreRuntimeOperations.ReturnRuntimeProgramMovement(frame.Id);
                    return;
                }
            }
            else
            {
                if (selected.Targets.Count != 1 || !_coreBoundPublicPileProgramsOperations.PublicPileDistributionTargets(frame).Contains(selected.Targets.Single()) || !draft.RemainingIds.Contains(id) || location != _corePublicPileSourcesOperations.PublicPileDraftLocation(draft))
                    throw new InvalidOperationException("Public pile requires all frozen cards to living other recipients.");
                var card = _corePublicPileSourcesOperations.PublicPileDraftCards(draft).Single(card => card.Id == id);
                _coreNativePromptBoundaryOperations.ClearPendingDecision();
                _coreRuntimeOperations.ReplaceRuntimeTop(frame with { PublicPileDraft = draft with { RemainingIds = draft.RemainingIds.Where(cardId => cardId != id).ToArray() }, PendingMovementContinuation = new(draft.OwnerSeat, 0, null) });
                _corePhysicalMovementFacadesOperations.MoveCard(card, location, CardLocation.Hand(selected.Targets.Single()), new("skill-program.public-pile.distribute"), _characterHuangQuanMouOperations.NativeGiveCompletion(frame.Id, draft.OwnerSeat, selected.Targets.Single(), [id], _coreCardMovementJournalMovementSequenceAccess._movementSequence, null));
                if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
                    _coreRuntimeOperations.ReturnRuntimeProgramMovement(frame.Id);
                return;
            }
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { PublicPileDraft = draft });
        PublishPublicPileChoice(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id));
    }

    internal IReadOnlyList<Card> PublicPileExchangeOwnedCards(ProgramSkillFrame frame)
    {
        var e = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
        return e.Op == SkillProgramEffectOp.ExchangePublicPileHand ? _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]) : _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat])).ToArray();
    }

    [ProgramAiChoice(SkillProgramEffectOp.ExchangePublicPileHand, SkillProgramEffectOp.ExchangePublicPile, SkillProgramEffectOp.SelectPublicPile, SkillProgramEffectOp.DistributeBoundCards)]
    internal PromptChoice SelectAiPublicPileChoice(PendingDecision decision, ProgramSkillFrame frame)
    {
        var draft = frame.PublicPileDraft!;
        var owner = _coreMatchParticipantsPlayersAccess._players[draft.OwnerSeat];
        if (draft.Stage == "source")
            return decision.Choices[0];
        if (draft.Stage == "owned")
        {
            var available = decision.Choices.Where(choice => choice.Cards.Count == 1).OrderBy(choice => CoreNativeAiDecisionsRuntime.GetKeepValue(PublicPileExchangeOwnedCards(frame).Single(card => card.Id == choice.Cards[0]), owner)).ThenBy(choice => choice.Cards[0]).FirstOrDefault();
            if (available is null || draft.OwnedIds.Count > 0)
                return decision.Choices.Single(choice => choice.Cards.Count == 0);
            var payment = PublicPileExchangeOwnedCards(frame).Single(card => card.Id == available.Cards[0]);
            return _corePublicPileSourcesOperations.PublicPileDraftCards(draft).Any(card => CoreNativeAiDecisionsRuntime.GetKeepValue(card, owner) > CoreNativeAiDecisionsRuntime.GetKeepValue(payment, owner)) ? available : decision.Choices.Single(choice => choice.Cards.Count == 0);
        }

        if (draft.Stage == "pile")
            return decision.Choices.OrderByDescending(choice => CoreNativeAiDecisionsRuntime.GetKeepValue(_corePublicPileSourcesOperations.PublicPileDraftCards(draft).Single(card => card.Id == choice.Cards[0]), owner)).ThenBy(choice => choice.Cards[0]).First();
        return decision.Choices.OrderByDescending(choice => CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(owner, _coreMatchParticipantsPlayersAccess._players[choice.Targets[0]])).ThenBy(choice => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[choice.Targets[0]]).Count).ThenBy(choice => choice.Targets[0]).ThenBy(choice => choice.Cards[0]).First();
    }

    internal void AssertPublicPileDraft(ProgramSkillFrame frame, SkillProgramEffect paused)
    {
        if (frame.PublicPileDraft is not { } draft)
            return;
        if (draft.Stage == "source")
        {
            if (paused.Op is not (SkillProgramEffectOp.SelectPublicPile or SkillProgramEffectOp.ExchangePublicPile or SkillProgramEffectOp.ExchangePublicPileHand) || draft.OwnerSeat != frame.OwnerSeat || draft.SourceSkillId != paused.SkillIds.SingleOrDefault() || draft.SourceLocation is not null || draft.SourceSkillInstanceId is not null || draft.Capacity != -1 || draft.OwnedIds.Count + draft.PileIds.Count + draft.RemainingIds.Count != 0)
                throw new InvalidOperationException("Public pile source draft is malformed.");
            return;
        }

        var source = _corePublicPileSourcesOperations.PublicPileDraftSource(draft);
        var distributing = paused.Op == SkillProgramEffectOp.DistributeBoundCards;
        if (draft.OwnerSeat != frame.OwnerSeat || source is null || source.SkillId != draft.SourceSkillId || source.Capacity != draft.Capacity || (!distributing && ((paused.Op == SkillProgramEffectOp.ExchangePublicPileHand ? draft.Capacity < 0 : draft.Capacity is < 1 or > 16 || paused.Amount != draft.Capacity) || paused.SkillIds.SingleOrDefault() != draft.SourceSkillId)) || (draft.Stage == "distribute" ? !distributing : paused.Op is not (SkillProgramEffectOp.ExchangePublicPile or SkillProgramEffectOp.ExchangePublicPileHand)) || draft.Stage is not ("owned" or "pile" or "distribute") || draft.OwnedIds.Count > (draft.Capacity == 0 ? _corePublicPileSourcesOperations.PublicPileDraftCards(draft).Count : draft.Capacity) || draft.OwnedIds.Count != draft.OwnedLocations.Count || draft.OwnedIds.Distinct().Count() != draft.OwnedIds.Count || draft.PileIds.Distinct().Count() != draft.PileIds.Count || draft.PileIds.Count > draft.OwnedIds.Count || draft.RemainingIds.Distinct().Count() != draft.RemainingIds.Count || draft.RemainingIds.Count > (draft.Capacity == 0 ? _corePublicPileSourcesOperations.PublicPileDraftCards(draft).Count : draft.Capacity) || draft.OwnedLocations.Any(location => paused.Op == SkillProgramEffectOp.ExchangePublicPileHand && location.Zone != CardZoneKind.Hand) || draft.OwnedIds.Where((id, index) => draft.OwnedLocations[index].OwnerSeat != frame.OwnerSeat || draft.OwnedLocations[index].Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || _cardZonesOperations._cardZones.GetLocation(id) != draft.OwnedLocations[index]).Any() || draft.PileIds.Concat(draft.RemainingIds).Any(id => _cardZonesOperations._cardZones.GetLocation(id) != source.Location))
            throw new InvalidOperationException("Public pile draft lost capacity, physical ownership, equal cost or instruction provenance.");
        if (distributing)
        {
            var binding = _coreProgramLifecycleOperations.GetProgramCardSet(frame, paused.SourceBind!);
            if (binding.Visibility != SkillProgramCardSetVisibility.Public || binding.CardIds.Count != binding.SourceLocations.Count || binding.SourceLocations.Any(location => location != source.Location) || draft.RemainingIds.Any(id => !binding.CardIds.Contains(id)))
                throw new InvalidOperationException("A distribution lost its original selected source binding.");
        }

        if (ReferenceEquals(frame, _resolutionStackOperations._resolutionStack.LastOrDefault()) && frame.PendingMovementContinuation is null && (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } prompt || prompt.PlayerSeat != draft.OwnerSeat || prompt.IsPrivate != (draft.Stage == "owned")))
            throw new InvalidOperationException("Public pile draft lost its chooser and visibility.");
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish, 70)]
    internal void CleanupLostPublicPersistentPiles()
    {
        foreach (var source in _coreGameEnginePublicPileSourcesPublicPersistentPilesAccess._publicPersistentPiles.Values.Where(source => !(_coreMatchParticipantsPlayersAccess._players[source.OwnerSeat].IsAlive && _characterGongsunZanMouMobileOperations.IsIssuedMouGszHuPile(source)) && !_characterHuangZuMobileShiOperations.IsIssuedShiHzPile(source) && !_characterGanNingMouMobileOperations.Value.IsIssuedMouGanNingPile(source) && !_characterXiahouDunMouMobileOperations.Value.IsIssuedMouXiahouDunPile(source) && !_characterBaoXinOlOperations.IsIssuedBaoXinMutaoPile(source) && !_characterSunShangxiangMouMobileOperations.IsIssuedMouSsxZhuang(source) && (!_coreMatchParticipantsPlayersAccess._players[source.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[source.OwnerSeat].SkillGrants.Grants.Any(grant => grant.SkillId == source.SkillId && grant.SkillInstanceId == source.SkillInstanceId) && !_characterLvQianOlOperations.IsDurableZengdaoPublicPile(source))).ToArray())
        {
            var retainMetadata = _characterLiFengOlOperations.RetainIssuedJiyunPile(source);
            if (!retainMetadata)
                _coreGameEnginePublicPileSourcesPublicPersistentPilesAccess._publicPersistentPiles.Remove((source.OwnerSeat, source.SkillId, source.SkillInstanceId));
            for (var index = 0; index < _resolutionStackOperations._resolutionStack.Count; index++)
                if (_resolutionStackOperations._resolutionStack[index] is ProgramSkillFrame { PublicPileDraft: { } draft } active && draft.OwnerSeat == source.OwnerSeat && draft.SourceSkillId == source.SkillId && (draft.SourceSkillInstanceId is null || draft.SourceSkillInstanceId == source.SkillInstanceId))
                    _coreRuntimeOperations.ReplaceRuntimeFrame(_resolutionStackOperations._resolutionStack[index].Id, active with { PublicPileDraft = null });
            var cleanup = _corePublicPileSourcesOperations.PublicPileCards(source).Where(card => !retainMetadata || !_characterLiFengOlOperations.IsIssuedJiyunRetainedEntity(source, card.Id)).ToArray();
            if (cleanup.Length > 0)
                _corePhysicalMovementFacadesOperations.MoveCards(cleanup, source.Location, CardLocation.DiscardPile, new("skill-program.public-pile.source-lost"));
        }
    }
}
