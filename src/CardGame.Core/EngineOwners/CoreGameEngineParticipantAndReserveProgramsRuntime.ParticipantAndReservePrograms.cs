#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineParticipantAndReserveProgramsRuntime
{
    internal bool BeginGameStartingPrograms()
    {
        _coreMatchSetupGameStartingProgramsResolvedAccess._gameStartingProgramsResolved = true;
        _characterZhouYuLeOperations.CaptureGuYinOpeningOrigins();
        var facts = _coreMatchParticipantsPlayersAccess._players.ToDictionary(player => player.Seat, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts);
        var candidates = _coreMatchParticipantsPlayersAccess._players.SelectMany(player => _coreProgramLifecycleOperations.CollectEligibleProgramTriggerCandidates(player, SkillProgramTriggerWindow.GameStarting, facts[player.Seat])).OrderBy(candidate => candidate.OwnerSeat).ThenByDescending(candidate => candidate.Priority).ToArray();
        if (candidates.Length == 0)
            return false;
        _coreRuntimeOperations.PushRuntimeFrame(_characterLuBuOlMagicOperations.CaptureMoLuBuInitialQi(_characterZhugeGuoLeOperations.CaptureLeXidiOpening(_characterCaoZhiLeOperations.FreezeFuleInitialEntities(_characterCaiYongLeOperations.CaptureJiaoweiInitialEntities(_characterDiaoChanLeOperations.CaptureTanbanInitialEntities(_characterZouShiLeOperations.CaptureYunzhengInitialEntities(_characterZhouYuLeOperations.CaptureGuYinInitialEntities(_characterXiaoQiaoLeOperations.CaptureQiqinInitialEntities(_characterCaiWenJiLeOperations.CaptureShuangjiaInitialEntities(_characterZhouFeiLeOperations.CaptureLingkongInitialEntities(_characterMiHengLeOperations.CaptureJiguInitialEntities(new ProgramLifecycleTriggerWindowFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, _coreActualTurnProgressCurrentSeatStartedAccess._currentSeat, SkillProgramTriggerWindow.GameStarting, candidates, ProgramLifecycleContinuation.CompleteGameStarting, facts[_coreActualTurnProgressCurrentSeatStartedAccess._currentSeat]) { ParticipantFacts = facts, FrozenFactionPopulation = _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.GrantFactionPopulationMarker) ? new System.Collections.ObjectModel.ReadOnlyDictionary<string, int>(_coreMatchParticipantsPlayersAccess._players.GroupBy(player => _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(player) ?? "").ToDictionary(group => group.Key, group => group.Count(), StringComparer.Ordinal)) : null }))))))))))));
        _coreRuntimeOperations.AdvanceRuntimeTop<ProgramLifecycleTriggerWindowFrame>();
        return true;
    }

    // Dying entry and exit are mandatory immediate boundaries. Definitions are
    // restricted to automatic draws so the rescue cursor never exposes stale hands.
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 70)]
    internal void ObserveAutomaticDyingTransitions(IGameEvent payload)
    {
        if (_coreActualTurnProgressCurrentSeatStartedAccess._started)
            ResolveAutomaticDyingTransitionPrograms(payload);
    }

    internal void ResolveAutomaticDyingTransitionPrograms(IGameEvent payload)
    {
        var transition = payload switch
        {
            PlayerDyingEvent entered => (entered.VictimSeat, SkillProgramTriggerWindow.DyingEntered),
            DyingResolvedEvent { Survived: true } exited => (exited.VictimSeat, SkillProgramTriggerWindow.DyingExited),
            _ => (-1, SkillProgramTriggerWindow.DyingEntered)};
        if (transition.Item1 < 0)
            return;
        var owner = _coreMatchParticipantsPlayersAccess._players[transition.Item1];
        foreach (var binding in _coreSkillProgramsOperations.EnabledUniqueProgramTriggers(owner, transition.Item2).Where(binding => binding.Trigger.Condition.Evaluate(_coreProgramLifecycleOperations.CaptureProgramTriggerFacts(owner), binding.SkillId, binding.SkillInstanceId)))
            foreach (var effect in binding.Trigger.Effects)
                if (effect.Op == SkillProgramEffectOp.Draw)
                    _corePhysicalDrawFacadesOperations.DrawCards(owner, effect.Amount, log: true, new CardMoveReason("program.dying-transition.draw"));
    }

    SkillProgramStepOutcome IParticipantReserveProgramHost.ExecuteParticipantReserve(SkillProgramEffect effect, ProgramSkillFrame frame) => ExecuteParticipantReserve(effect, frame);
    internal SkillProgramStepOutcome ExecuteParticipantReserve(SkillProgramEffect effect, ProgramSkillFrame frame)
    {
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        if (effect.Op == SkillProgramEffectOp.LoseHpUnclamped)
        {
            if (_coreIdentityConcealmentOperations.TryBeginIdentityAppearanceForProgramHpLoss(frame.Id, frame.SkillId, owner.Seat, effect.Amount))
                return SkillProgramStepOutcome.AwaitChild;
            var before = owner.Hp;
            owner.Hp -= effect.Amount;
            _coreHpChangeProgramsOperations.RecordHpChange(frame.Id, null, owner.Seat, before, owner.Hp, HpChangeKind.Loss);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSkillHpLostEvent(frame.Id, frame.SkillId, owner.Seat, effect.Amount, owner.Hp));
            if (owner.Hp > 0)
                return SkillProgramStepOutcome.Continue;
            _coreSkillProgramsOperations.BeginProgramSkillDying(frame.Id, owner);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (effect.Op == SkillProgramEffectOp.RecoverAllLiving)
        {
            NativeOwners.ProgramSkillHost host = _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost();
            foreach (var player in _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive))
                host.Recover(frame.Id, owner.Seat, player.Seat, effect.Amount, null, null);
            return SkillProgramStepOutcome.Continue;
        }

        if (effect.Op == SkillProgramEffectOp.SpendMarkerOrLoseHp)
        {
            var choices = new List<PromptChoice>();
            if (owner.Markers.GetValueOrDefault(effect.Marker!.Value) >= effect.Amount)
                choices.Add(new(new ChoiceId("marker-payment.spend"), "移去" + PlayerMarkerCatalog.GetDisplayName(effect.Marker!.Value) + "标记", [], [], new Dictionary<string, string> { ["program-action"] = "marker-payment", ["pay"] = "marker" }));
            choices.Add(new(new ChoiceId("marker-payment.hp"), "失去1点体力", [], [], new Dictionary<string, string> { ["program-action"] = "marker-payment", ["pay"] = "hp" }));
            var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
            _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, owner.Seat, skill.Name + "：选择支付方式", [], [], owner.Seat)
            {
                PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
                IsPrivate = true,
                Choices = choices.ToArray(),
                SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
            };
            _coreMatchOutcomeStatusAccess._status = owner.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
            return SkillProgramStepOutcome.AwaitChoice;
        }

        if (effect.Op == SkillProgramEffectOp.InitializePrivatePile)
        {
            var location = new CardLocation(CardZoneKind.PrivateReserve, owner.Seat);
            for (var index = 0; index < effect.Amount && _corePhysicalDrawFacadesOperations.EnsureDrawPile(); index++)
                _corePhysicalMovementFacadesOperations.MoveCard(_cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile)[^1], CardLocation.DrawPile, location, new CardMoveReason("program.private-reserve.initialize"));
            return SkillProgramStepOutcome.Continue;
        }

        if (effect.Op == SkillProgramEffectOp.ExchangePrivatePile)
        {
            var maximum = Math.Min(_corePhysicalOwnedCardQueriesOperations.GetHand(owner).Count, _cardZonesOperations._cardZones.CardsAt(new CardLocation(CardZoneKind.PrivateReserve, owner.Seat)).Count);
            if (maximum == 0)
                return SkillProgramStepOutcome.Continue;
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { PrivateReserveDraft = new(owner.Seat, "exchange-hand", maximum, [], []) });
            PublishPrivateReserveChoice(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id));
            return SkillProgramStepOutcome.AwaitChoice;
        }

        if (effect.Op == SkillProgramEffectOp.GrantAttributedNatureEffect)
        {
            foreach (var seat in frame.SelectedTargetSeats)
                SetAttributedNatureMarker(owner.Seat, seat, effect.Marker!.Value, 1, frame.Id);
            return SkillProgramStepOutcome.Continue;
        }

        // Chaotic Ambition walks the field from the owner's next turn neighbour.
        var seats = frame.WenheChaos is { } wenhe && _coreWenheChaosOperations.IsExactWenheChaos(frame) ? wenhe.ActorSeats.ToArray() : effect.Op is SkillProgramEffectOp.RequestSlashByNearest or SkillProgramEffectOp.RequestLegalSlashByNearest ? Enumerable.Range(1, _coreMatchDefinitionPlayerCountAccess._playerCount - 1).Select(offset => _coreMatchParticipantsPlayersAccess._players[_coreTableOrderOperations.TableStepSeat(owner.Seat, offset)].Seat).ToArray() : effect.TargetKind is { } kind ? _coreMatchParticipantsPlayersAccess._players.Where(player => (kind != SkillProgramTargetKind.OtherLiving || player.Seat != owner.Seat)).Select(player => player.Seat).ToArray() : frame.SelectedTargetSeats.ToArray();
        var key = "participant-cursor-" + (frame.InstructionIndex - 1);
        var cursor = frame.NumberBindings.SingleOrDefault(item => item.Name == key)?.Value ?? 0;
        if (cursor >= seats.Length)
            return SkillProgramStepOutcome.Continue;
        var seatNow = seats[cursor];
        var amount = cursor == 0 ? effect.Amount : effect.MinimumValue;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { NumberBindings = frame.NumberBindings.Where(item => item.Name != key).Append(new ProgramSkillNumberBinding(key, cursor + 1)).ToArray(), ReexecuteParticipantInstruction = true });
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (!_coreMatchParticipantsPlayersAccess._players[seatNow].IsAlive)
            return SkillProgramStepOutcome.Continue;
        if (effect.Op == SkillProgramEffectOp.DamageParticipants)
            return _coreSkillProgramsOperations.BeginProgramSkillDamage(frame, seatNow, amount, nature: effect.DamageNature);
        if (effect.Op == SkillProgramEffectOp.LoseHpParticipants)
            return _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(frame.Id, frame.SkillId, seatNow, amount);
        if (effect.Op == SkillProgramEffectOp.RequestLegalSlashByNearest)
            return _coreNearestLegalSlashRequestsOperations.RequestLegalSlashByNearestActor(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), seatNow, amount);
        if (effect.Op == SkillProgramEffectOp.RequestSlashByNearest)
            return _coreProgramSlashRequestOperations.RequestProgramSlashByNearest(frame, seatNow, amount);
        if (effect.Zones.SequenceEqual([CardZoneKind.Equipment]))
        {
            _corePhysicalMovementFacadesOperations.MoveCards(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[seatNow]).Where(card => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(frame.OwnerSeat, card, CardLocation.Equipment(seatNow), OwnedCardMoveIntent.Discard)).ToArray(), CardLocation.Equipment(seatNow), CardLocation.DiscardPile, new CardMoveReason("program.participants.discard-equipment"), movement => _characterBianFuRenOlOperations.RecordBianFuRenForeignDiscardProducer(movement, frame.OwnerSeat, frame.Id));
            return SkillProgramStepOutcome.Continue;
        }

        var required = Math.Min(amount, _coreTurnHandCategoryRestrictionsOperations.GetSelfDiscardableOwnedHand(_coreMatchParticipantsPlayersAccess._players[seatNow]).Count);
        if (required == 0)
            return SkillProgramStepOutcome.Continue;
        // Restore the committed cursor for private input; the draft resumes this instruction.
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { ReexecuteParticipantInstruction = false, PrivateReserveDraft = new(seatNow, "discard-hand", required, [], []) });
        PublishPrivateReserveChoice(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id));
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void PublishPrivateReserveChoice(ProgramSkillFrame frame)
    {
        var draft = frame.PrivateReserveDraft!;
        var starPhase = draft.Mode == "exchange-stars";
        var location = starPhase ? new CardLocation(CardZoneKind.PrivateReserve, draft.ChooserSeat) : CardLocation.Hand(draft.ChooserSeat);
        var selected = starPhase ? draft.SelectedReserveIds : draft.SelectedHandIds;
        Dictionary<string, string> Parameters(int? cardId = null)
        {
            var values = new Dictionary<string, string>
            {
                ["program-action"] = "private-reserve-choice",
                ["reserve-mode"] = draft.Mode,
                ["reserve-required"] = draft.RequiredCount.ToString(System.Globalization.CultureInfo.InvariantCulture),
                ["reserve-selected-hand"] = string.Join(',', draft.SelectedHandIds),
                ["reserve-selected-stars"] = string.Join(',', draft.SelectedReserveIds)
            };
            if (cardId is { } id)
                values["card-id"] = id.ToString(System.Globalization.CultureInfo.InvariantCulture);
            else
                values["finish"] = "true";
            return values;
        }

        var choices = _cardZonesOperations._cardZones.CardsAt(location).Where(card => !selected.Contains(card.Id) && (draft.Mode != "discard-hand" || !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(draft.ChooserSeat, card, location, OwnedCardMoveIntent.Discard))).Select(card => new PromptChoice(new ChoiceId("private-reserve." + card.Id), card.DisplayName, [card.Id], [], Parameters(card.Id))).ToList();
        if (draft.Mode == "exchange-hand" && selected.Count > 0)
            choices.Add(new(new ChoiceId("private-reserve.finish"), "完成手牌选择", [], [], Parameters()));
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, draft.ChooserSeat, $"【{(skill.Name)}】选择{((starPhase ? "等量星" : "手牌"))}", [], [], frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = draft.ChooserSeat,
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description),
            Choices = choices.ToArray()
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[draft.ChooserSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("private-reserve-choice")]
    internal void ResolvePrivateReserveChoice(PromptChoice choice)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Missing reserve frame.");
        var draft = frame.PrivateReserveDraft ?? throw new InvalidOperationException("Missing reserve draft.");
        var finish = choice.Parameters.GetValueOrDefault("finish") == "true";
        var star = draft.Mode == "exchange-stars";
        var location = star ? new CardLocation(CardZoneKind.PrivateReserve, draft.ChooserSeat) : CardLocation.Hand(draft.ChooserSeat);
        var selected = star ? draft.SelectedReserveIds : draft.SelectedHandIds;
        if (finish && (draft.Mode != "exchange-hand" || selected.Count == 0) || !finish && (choice.Cards.Count != 1 || selected.Contains(choice.Cards[0]) || _cardZonesOperations._cardZones.GetLocation(choice.Cards[0]) != location))
            throw new InvalidOperationException("Invalid private reserve choice.");
        if (draft.Mode == "discard-hand" && choice.Cards.Any(id => _coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(draft.ChooserSeat, _cardZonesOperations._cardZones.CardsAt(location).Single(card => card.Id == id), location, OwnedCardMoveIntent.Discard)))
            throw new InvalidOperationException("A protected hand card cannot be selected for self discard.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var ids = finish ? selected : selected.Append(choice.Cards[0]).ToArray();
        draft = star ? draft with
        {
            SelectedReserveIds = ids
        }

        : draft with
        {
            SelectedHandIds = ids
        };
        if (draft.Mode == "exchange-hand" && (finish || ids.Count == draft.RequiredCount))
            draft = draft with
            {
                Mode = "exchange-stars",
                RequiredCount = ids.Count
            };
        else if (ids.Count == draft.RequiredCount)
        {
            var reason = new CardMoveReason("program.private-reserve.exchange");
            if (draft.Mode == "discard-hand")
            {
                _corePhysicalMovementFacadesOperations.MoveCards(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[draft.ChooserSeat]).Where(card => ids.Contains(card.Id)).ToArray(), location, CardLocation.DiscardPile, new CardMoveReason("program.participants.discard-hand"));
                frame = frame with
                {
                    ReexecuteParticipantInstruction = true
                };
            }
            else
            {
                var reserve = new CardLocation(CardZoneKind.PrivateReserve, draft.ChooserSeat);
                var handCards = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[draft.ChooserSeat]).Where(card => draft.SelectedHandIds.Contains(card.Id)).ToArray();
                var starCards = _cardZonesOperations._cardZones.CardsAt(reserve).Where(card => draft.SelectedReserveIds.Contains(card.Id)).ToArray();
                if (handCards.Length != ids.Count || starCards.Length != ids.Count)
                    throw new InvalidOperationException("Exchange lost physical cards.");
                _corePhysicalMovementFacadesOperations.MoveCards(handCards, CardLocation.Hand(draft.ChooserSeat), CardLocation.Processing, reason);
                _corePhysicalMovementFacadesOperations.MoveCards(starCards, reserve, CardLocation.Hand(draft.ChooserSeat), reason);
                _corePhysicalMovementFacadesOperations.MoveCards(handCards, CardLocation.Processing, reserve, reason);
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { PrivateReserveDraft = null });
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { PrivateReserveDraft = draft });
        PublishPrivateReserveChoice(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id));
    }

    [ProgramAnswer("marker-payment")]
    internal void ResolveMarkerOrHpPayment(PromptChoice choice)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Missing marker payment frame.");
        var effect = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
        if (effect.Op != SkillProgramEffectOp.SpendMarkerOrLoseHp)
            throw new InvalidOperationException("Missing marker payment instruction.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (choice.Parameters.GetValueOrDefault("pay") == "marker")
        {
            _coreProgramMarkerPaymentsOperations.PayProgramMarkerCost(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], new SkillProgramMarkerCost(effect.Marker!.Value, effect.Amount), frame.SkillId, frame.ActivationId);
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        }
        else if (_kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(frame.Id, frame.SkillId, frame.OwnerSeat, effect.Amount) == SkillProgramStepOutcome.Continue)
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    internal void SetAttributedNatureMarker(int source, int target, PlayerMarkerKind marker, int amount, long id)
    {
        var player = _coreMatchParticipantsPlayersAccess._players[target];
        var previous = player.MarkerSourceCounts.GetValueOrDefault((marker, source));
        player.MarkerSourceCounts[(marker, source)] = amount;
        var count = player.Markers.GetValueOrDefault(marker) + amount - previous;
        if (amount == 0)
            player.MarkerSourceCounts.Remove((marker, source));
        if (count == 0)
            player.Markers.Remove(marker);
        else
            player.Markers[marker] = count;
        if (amount != previous)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlayerMarkerChangedEvent(id, target, marker, amount - previous, count, source, "program.nature-effect"));
    }
    internal void ExpireAttributedNatureMarkers(int ownerSeat)
    {
        foreach (var player in _coreMatchParticipantsPlayersAccess._players)
            foreach (var marker in new[]
            {
                PlayerMarkerKind.Gale,
                PlayerMarkerKind.Mist
            }

            )
                if (player.MarkerSourceCounts.ContainsKey((marker, ownerSeat)))
                    SetAttributedNatureMarker(ownerSeat, player.Seat, marker, 0, ++_coreResolutionIdentityBoundaryOperations._resolutionSequence);
    }
}
