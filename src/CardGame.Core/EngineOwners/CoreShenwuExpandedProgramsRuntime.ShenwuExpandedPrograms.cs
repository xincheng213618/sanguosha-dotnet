#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreShenwuExpandedProgramsRuntime
{
    SkillProgramStepOutcome IShenwuExpandedProgramHost.ExecuteShenwuExpanded(ProgramSkillFrame frame, SkillProgramEffectOp op) => ExecuteShenwuExpanded(frame, op);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 27720)]
    internal bool AllowsShenwuExpandedTrigger(ProgramTriggerLegalityContext gate)
    {
        if (gate.SkillId is "shenwu:enemy-tier" or "shenwu:player-tier-2020")
            return _coreShenwuActivitiesOperations.Value.ShenwuBattleRules?.AllowsTierTrigger(gate.OwnerSeat, gate.SkillId) ?? false;
        if (gate.SkillId is "shenwu:xunyou" or "shenwu:yanyu")
            return gate.WindowContext is null || gate.Context.TargetSeat is { } other && other != gate.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[other].IsAlive;
        if (gate.SkillId == "shenwu:poison")
            return gate.Owner.Markers.GetValueOrDefault(PlayerMarkerKind.ShenwuPoison) > 0;
        if (gate.SkillId == "shenwu:zhaohuo")
            return _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { } damage && _coreNativeCardClassificationsOperations.GetDamageNature(damage) == DamageNature.Fire;
        if (gate.SkillId != "shenwu:bingxian")
            return true;
        if (gate.Context.TargetSeat is not { } actor || actor == gate.OwnerSeat)
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var end = Array.FindLastIndex(history, fact => fact is TurnEndedEvent turn && turn.ActorSeat == actor && turn.TurnNumber == _coreActualTurnProgressTurnNumberAccess._turnNumber);
        var start = end < 0 ? -1 : Array.FindLastIndex(history, end, fact => fact is TurnStartedEvent turn && turn.ActorSeat == actor && turn.TurnNumber == _coreActualTurnProgressTurnNumberAccess._turnNumber);
        return start >= 0 && !history.Skip(start + 1).Take(end - start - 1).OfType<CardUseDeclaredEvent>().Any(use => use.SourceSeat == actor && CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind));
    }

    internal SkillProgramStepOutcome ExecuteShenwuExpanded(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        switch (op)
        {
            case SkillProgramEffectOp.AddShenwuDamagePoison:
                if (frame.WindowContext is { SourceSeat: { } source } && source != owner.Seat)
                    AddShenwuPoison(source, owner.Seat, frame.Id);
                return SkillProgramStepOutcome.Continue;
            case SkillProgramEffectOp.ResolveShenwuPoison:
                return ResolveShenwuPoison(frame);
            case SkillProgramEffectOp.ResolveShenwuHonglian:
            case SkillProgramEffectOp.ResolveShenwuXushi:
                return ResolveShenwuRandomDamage(frame, op);
            case SkillProgramEffectOp.ResolveShenwuXunyou:
                return ResolveShenwuXunyou(frame);
            case SkillProgramEffectOp.BindShenwuTurnOwner:
            case SkillProgramEffectOp.BindShenwuEndedTurnOwner:
                if (frame.WindowContext?.TargetSeat is not { } target || target == frame.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
                    throw new InvalidOperationException("Yanyu requires the exact other actual turn owner.");
                _coreRuntimeOperations.ReplaceRuntimeTop(frame with { SelectedTargetSeats = Array.AsReadOnly(new[] { target }) });
                return SkillProgramStepOutcome.Continue;
            case SkillProgramEffectOp.GateShenwuYanyu:
                if (frame.JudgmentOutcomeBindings?.LastOrDefault()is not { Suit: Suit.Heart or Suit.Diamond } || frame.SelectedTargetSeats is not [var subject] || !_coreMatchParticipantsPlayersAccess._players[subject].IsAlive)
                    _coreRuntimeOperations.ReplaceRuntimeTop(frame with { InstructionIndex = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).Instructions.Count });
                return SkillProgramStepOutcome.Continue;
            case SkillProgramEffectOp.RecoverShenwu2020Rebirth:
                if (frame.ShenwuExpanded is null)
                {
                    _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShenwuExpanded = new("rebirth-recover"), ReexecuteParticipantInstruction = true });
                    _coreProgramLifecycleOperations.DrawProgramCards(frame.Id, owner.Seat, Math.Max(0, Math.Min(owner.MaxHp, 5) - _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Count), null, null, SkillProgramCardSetVisibility.Private, new("shenwu.2020.rebirth-refill"));
                    var outcome = _coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(frame.Id, owner.Seat);
                    if (outcome != SkillProgramStepOutcome.Continue)
                        return outcome;
                    frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(frame with { ShenwuExpanded = null, ReexecuteParticipantInstruction = false });
                _coreProgramLifecycleOperations.RecoverProgramTargetTo(frame.Id, owner.Seat, owner.Seat, SkillProgramNumberExpression.IntegerConstant, owner.MaxHp, true);
                return SkillProgramStepOutcome.Continue;
            default:
                throw new InvalidOperationException("Unsupported expanded Shenwu operation.");
        }
    }

    internal void AddShenwuPoison(int seat, int source, long frameId)
    {
        var target = _coreMatchParticipantsPlayersAccess._players[seat];
        if (!target.IsAlive || _coreShenwuNpcProgramsOperations.ShenwuEnabled(target, "shenwu:duqu"))
            return;
        var key = (PlayerMarkerKind.ShenwuPoison, source);
        target.MarkerSourceCounts[key] = target.MarkerSourceCounts.GetValueOrDefault(key) + 1;
        var count = target.Markers[PlayerMarkerKind.ShenwuPoison] = target.Markers.GetValueOrDefault(PlayerMarkerKind.ShenwuPoison) + 1;
        const string grant = "activity:shenwu:poison";
        target.SkillGrants.Grant(new(grant, "shenwu:poison", grant, grant));
        _skillBindingIndexOperations._skillBindingIndex.Invalidate(seat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlayerMarkerChangedEvent(frameId, seat, PlayerMarkerKind.ShenwuPoison, 1, count, source, "shenwu.poison"));
    }

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 27720)]
    internal void ObserveShenwuExpandedFacts(IGameEvent fact)
    {
        if (fact is CardUseDeclaredEvent use && use.CardKind is CardKind.Peach or CardKind.Alcohol)
            foreach (var holder in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != use.SourceSeat && _coreProgramCardPoliciesOperations.HasCardPolicy(p, SkillProgramCardPolicyKind.ShenwuPoisonOnPeachWine)).ToArray())
                AddShenwuPoison(use.SourceSeat, holder.Seat, use.ResolutionId);
        if (fact is TurnEndedEvent turn && _coreMatchParticipantsPlayersAccess._players[turn.ActorSeat].SkillGrants.Grants.Any(grant => grant.SkillId == "shenwu:juyuan"))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenwuTurnEndHealthEvent(turn.TurnNumber, turn.ActorSeat, _coreMatchParticipantsPlayersAccess._players[turn.ActorSeat].Hp));
    }

    internal SkillProgramStepOutcome ResolveShenwuRandomDamage(ProgramSkillFrame frame, SkillProgramEffectOp op)
    {
        if (frame.ShenwuExpanded is null)
        {
            var others = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != frame.OwnerSeat).Select(p => p.Seat).ToList();
            var roll = op == SkillProgramEffectOp.ResolveShenwuHonglian ? _randomOperations._random.Next(4) : 0;
            var cards = op == SkillProgramEffectOp.ResolveShenwuHonglian ? _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Where(card => CoreNativeCardClassificationsRuntime.IsRedSuit(_coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], card))).ToList() : [];
            _randomOperations._random.Shuffle(cards);
            var selected = cards.Take(roll).ToArray();
            if (op == SkillProgramEffectOp.ResolveShenwuHonglian)
                _randomOperations._random.Shuffle(others);
            var damages = (op == SkillProgramEffectOp.ResolveShenwuHonglian ? others.Take(3 - roll) : others).Select(seat => new ShenwuExpandedDamage(seat, op == SkillProgramEffectOp.ResolveShenwuXushi ? 1 + _randomOperations._random.Next(2) : 1)).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShenwuExpanded = new("damage", Damage: Array.AsReadOnly(damages)), ReexecuteParticipantInstruction = true });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenwuExpandedRandomEvent(frame.Id, frame.SkillId, roll, selected.Length, Array.AsReadOnly(damages)));
            foreach (var card in selected)
                _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DrawPile, CardLocation.Hand(frame.OwnerSeat), new("shenwu.honglian.obtain"));
            var outcome = _coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat);
            if (outcome != SkillProgramStepOutcome.Continue)
                return outcome;
            frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        }

        var resolution = frame.ShenwuExpanded!;
        while (resolution.Cursor < resolution.Damage!.Count)
        {
            var next = resolution.Damage[resolution.Cursor];
            resolution = resolution with
            {
                Cursor = resolution.Cursor + 1
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShenwuExpanded = resolution, ReexecuteParticipantInstruction = true });
            if (_coreMatchParticipantsPlayersAccess._players[next.Seat].IsAlive)
                return _coreSkillProgramsOperations.BeginProgramSkillDamage(frame, next.Seat, next.Amount, nature: op == SkillProgramEffectOp.ResolveShenwuHonglian ? DamageNature.Fire : DamageNature.Normal);
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { ShenwuExpanded = null, ReexecuteParticipantInstruction = false });
        return SkillProgramStepOutcome.Continue;
    }

    internal SkillProgramStepOutcome ResolveShenwuXunyou(ProgramSkillFrame frame)
    {
        if (frame.ShenwuExpanded is null)
        {
            var candidates = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != frame.OwnerSeat).Select(p => (Player: p, Cards: _corePhysicalOwnedCardQueriesOperations.GetHand(p).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(p)).Concat(_corePhysicalOwnedCardQueriesOperations.GetJudgment(p)).ToArray())).Where(entry => entry.Cards.Length > 0).ToArray();
            if (candidates.Length == 0)
                return SkillProgramStepOutcome.Continue;
            var other = candidates[_randomOperations._random.Next(candidates.Length)];
            var card = other.Cards[_randomOperations._random.Next(other.Cards.Length)];
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShenwuExpanded = new("equip", CardId: card.Id), ReexecuteParticipantInstruction = true });
            _corePhysicalMovementFacadesOperations.MoveCard(card, _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(other.Player, card), CardLocation.Hand(frame.OwnerSeat), new("shenwu.xunyou.obtain"));
            var outcome = _coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat);
            if (outcome != SkillProgramStepOutcome.Continue)
                return outcome;
            frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        }

        var r = frame.ShenwuExpanded!;
        if (r.Stage == "equip" && r.CardId is { } id && _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Hand(frame.OwnerSeat))
        {
            var card = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]).Single(c => c.Id == id);
            if (EquipmentCatalog.IsEquipment(card.Kind) && _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].EquipmentSlotCapacity(EquipmentCatalog.Get(card.Kind).Slot) > 0 && !_coreCardUseModulesOperations.IsCardUseForbidden(frame.OwnerSeat, card.Kind, CardActionType.Use) && !_coreHandComparisonOperations.IsSelfTargetForbiddenAction(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], card.Kind, [frame.OwnerSeat]))
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShenwuExpanded = r with { Stage = "complete" }, ReexecuteParticipantInstruction = true });
                var source = new CardConversionSource(frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId);
                var use = _coreNativeCardUseOperations.BeginCardUse(card, frame.OwnerSeat, [], conversionSource: source);
                _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Hand(frame.OwnerSeat), CardLocation.Processing, CardMoveReasons.EquipmentUse);
                if (!_coreFinalizedCardTargetsOperations.TryBeginEquipmentTargetPrograms(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], card, use))
                    _coreNativeEquipmentUseOperations.CompleteEquipmentUse(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], card, use);
                return SkillProgramStepOutcome.AwaitChild;
            }
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { ShenwuExpanded = null, ReexecuteParticipantInstruction = false });
        return SkillProgramStepOutcome.Continue;
    }

    [NativeUseReturn(27720)]
    internal void ContinueShenwuXunyouEquipment()
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { ShenwuExpanded.Stage: "complete", SkillId: "shenwu:xunyou" } frame)
        {
            if (_coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat) == SkillProgramStepOutcome.Continue)
                _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        }
    }

    internal SkillProgramStepOutcome ResolveShenwuPoison(ProgramSkillFrame frame)
    {
        var r = frame.ShenwuExpanded;
        if (r is null)
        {
            var count = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].Markers.GetValueOrDefault(PlayerMarkerKind.ShenwuPoison);
            if (count == 0)
                return SkillProgramStepOutcome.Continue;
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShenwuExpanded = r = new("offer", count, SelectedIds: []) });
        }

        if (r.Stage is "offer" or "select")
        {
            PublishShenwuPoisonChoice(frame);
            return SkillProgramStepOutcome.AwaitChoice;
        }

        if (r.Stage == "lose-hp")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShenwuExpanded = r with { Stage = "remove" }, ReexecuteParticipantInstruction = true });
            var outcome = _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(frame.Id, frame.SkillId, frame.OwnerSeat, r.Amount);
            if (outcome != SkillProgramStepOutcome.Continue)
                return outcome;
            frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        }

        _coreAttributedMarkerEventsOperations.MutateParticipantMarker(frame, frame.OwnerSeat, PlayerMarkerKind.ShenwuPoison, -1);
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { ShenwuExpanded = null, ReexecuteParticipantInstruction = false });
        return SkillProgramStepOutcome.Continue;
    }

    internal Card[] ShenwuPoisonPayable(ProgramSkillFrame frame) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat])).Where(card => !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(frame.OwnerSeat, card, _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], card), OwnedCardMoveIntent.Discard)).ToArray();
    internal IReadOnlyList<PromptChoice> ShenwuPoisonChoices(ProgramSkillFrame frame)
    {
        var r = frame.ShenwuExpanded!;
        PromptChoice Choice(string action, string text, int? card = null) => new(new ChoiceId($"shenwu-poison:{(frame.Id)}:{(action)}:{(card)}"), text, card is { } id ? [id] : [], [], new Dictionary<string, string> { ["program-action"] = "shenwu-poison", ["poison-action"] = action, ["frame-id"] = frame.Id.ToString() });
        if (r.Stage == "offer")
            return (ShenwuPoisonPayable(frame).Length >= r.Amount ? new[]
            {
                Choice("discard", $"弃置{(r.Amount)}张牌")
            }

            : []).Append(Choice("lose-hp", $"失去{(r.Amount)}点体力")).ToArray();
        return ShenwuPoisonPayable(frame).Where(card => !r.SelectedIds!.Contains(card.Id)).Select(card => Choice("card", $"弃置【{(card.DisplayName)}】（{(r.SelectedIds!.Count)}/{(r.Amount)}）", card.Id)).ToArray();
    }

    internal void PublishShenwuPoisonChoice(ProgramSkillFrame frame)
    {
        var choices = ShenwuPoisonChoices(frame);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, frame.OwnerSeat, skill.Description, choices.SelectMany(c => c.Cards).ToArray(), [], frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("shenwu-poison")]
    internal void AnswerShenwuPoison(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShenwuExpanded: { } r } frame || !ShenwuPoisonChoices(frame).Any(current => current.Id == choice.Id))
            throw new InvalidOperationException("Stale snake poison payment.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var action = choice.Parameters["poison-action"];
        if (action == "discard")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShenwuExpanded = r with { Stage = "select" } });
            PublishShenwuPoisonChoice(frame);
            return;
        }

        if (action == "lose-hp")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { ShenwuExpanded = r with { Stage = "lose-hp" }, ReexecuteParticipantInstruction = true });
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
            return;
        }

        var ids = r.SelectedIds!.Append(choice.Cards.Single()).ToArray();
        if (ids.Length < r.Amount)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShenwuExpanded = r with { SelectedIds = Array.AsReadOnly(ids) } });
            PublishShenwuPoisonChoice(frame);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShenwuExpanded = r with { Stage = "remove", SelectedIds = Array.AsReadOnly(ids) }, ReexecuteParticipantInstruction = true });
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(frame, frame.OwnerSeat, ids, new("shenwu.poison.discard"));
        if (_coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat) == SkillProgramStepOutcome.Continue)
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveShenwuPoison)]
    internal PromptChoice ChooseAiShenwuPoison(PendingDecision decision, ProgramSkillFrame frame, SkillProgramEffect effect) => decision.Choices.First();
}
