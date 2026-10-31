#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineStrategicProgramsRuntime
{
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.AfterDyingCheck, 10, SkillProgramEffectOp.SuppressGeneralSkill)]
    internal bool AllowsSuppressGeneralSkillTrigger(ProgramTriggerLegalityContext gate)
    {
        var owner = gate.Owner;
        var features = gate.Features;
        var context = gate.Context;
        return !(features.HasOperation(SkillProgramEffectOp.SuppressGeneralSkill) && (_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || owner.Seat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || context.TargetSeat is not { } target || _coreGameEngineStrategicProgramsProgramSuppressionsStrategicEndPlayTurnStrategicHandLimitPenaltyTurnsAccess._programSuppressions.Any(item => item.SourceSeat == owner.Seat && item.TargetSeat == target)));
    }

    internal bool UsesStrategicTriggerValue(SkillProgramTriggerValueKind kind) => _contentRegistryOperations._contentRegistry.ProgramDependencies.UsesTriggerValue(kind);
    internal IReadOnlyDictionary<string, bool>? GetPublicProgramBooleanStates(CharacterState player)
    {
        var values = new Dictionary<string, bool>(StringComparer.Ordinal);
        foreach (var grant in player.SkillGrants.Grants.Where(grant => grant.IsEnabled))
        {
            var program = _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(grant.SkillId)?.Program;
            if (program is null)
                continue;
            foreach (var definition in program.BooleanStates.Where(state => state.Visibility == SkillProgramStateVisibility.Public))
                values[definition.Id] = _coreProgramPersistentStateOperations.GetProgramBooleanState2(player.Seat, grant.SkillId, grant.SkillInstanceId, definition.Id);
        }

        return values.Count > 0 ? values : null;
    }

    internal IReadOnlyDictionary<string, int>? GetProgramPublicCounters(CharacterState player)
    {
        var values = new Dictionary<string, int>(StringComparer.Ordinal);
        if (_contentRegistryOperations._contentRegistry.ProgramDependencies.HasCardPolicyKindAtOrAbove(450))
        {
            values["junlue"] = player.Markers.GetValueOrDefault(PlayerMarkerKind.Junlue);
            values["camp"] = player.Markers.GetValueOrDefault(PlayerMarkerKind.Camp);
            values["woundedPlayers"] = _coreMatchParticipantsPlayersAccess._players.Count(other => other.IsAlive && _coreTreatedWoundedOperations.IsRuleWounded(other));
        }

        foreach (var item in _coreGameEngineConfiguredConversionsConfiguredConversionTiersAccess._configuredConversionTiers.Where(item => item.Key.Seat == player.Seat))
            values[item.Key.State] = item.Value;
        return values.Count > 0 ? values : null;
    }

    internal void ResetPlayPhaseProgramStates(int ownerSeat)
    {
        foreach (var key in _coreGameEngineProgramPersistentStateProgramBooleanStatesAccess._programBooleanStates.Keys.Where(key => key.OwnerSeat == ownerSeat).ToArray())
        {
            var definition = _coreProgramPersistentStateOperations.GetProgramBooleanStateDefinition2(key.SkillId, key.StateId);
            if (definition.ResetScope == SkillProgramStateResetScope.PlayPhase)
                _coreGameEngineProgramPersistentStateProgramBooleanStatesAccess._programBooleanStates[key] = definition.InitialValue;
        }
    }

    internal IEnumerable<RuleQueryContribution> CollectStrategicRuleContributions(CharacterState player, SkillRuleQuery query)
    {
        if (query == SkillRuleQuery.HandLimit && _coreGameEngineStrategicProgramsProgramSuppressionsStrategicEndPlayTurnStrategicHandLimitPenaltyTurnsAccess._strategicHandLimitPenaltyTurns.GetValueOrDefault(player.Seat, -1) == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber)
            yield return new FiniteRuleQueryContribution($"turn:{(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber)}:hand-discard-share:{(player.Seat)}", SkillRuleOperation.Add, -1);
        int? wounded = null;
        int WoundedCount() => wounded ??= _coreMatchParticipantsPlayersAccess._players.Count(other => other.IsAlive && _coreTreatedWoundedOperations.IsRuleWounded(other));
        foreach (var source in _coreMatchParticipantsPlayersAccess._players.Where(source => source.IsAlive))
            foreach (var instance in _coreNationalSkillsOperations.GetSkillBindingShard(source).ProgramInstances)
                foreach (var policy in instance.Program.CardPolicies)
                {
                    if (policy.Kind switch
                    {
                        SkillProgramCardPolicyKind.ChainedHandLimitAura or SkillProgramCardPolicyKind.WoundedInRangeHandLimitPenalty => query != SkillRuleQuery.HandLimit,
                        SkillProgramCardPolicyKind.MarkerTurnBonuses => query is not (SkillRuleQuery.DrawCount or SkillRuleQuery.SlashLimit or SkillRuleQuery.HandLimit),
                        SkillProgramCardPolicyKind.WoundedPopulationBonuses => query is not (SkillRuleQuery.DrawCount or SkillRuleQuery.HandLimit),
                        _ => true
                    })
                        continue;
                    if (!policy.Condition.Evaluate(_coreNativeSkillContextQueriesOperations.CreateSkillContext(source)))
                        continue;
                    var value = policy.Kind switch
                    {
                        SkillProgramCardPolicyKind.ChainedHandLimitAura when query == SkillRuleQuery.HandLimit && player.IsChained => 2,
                        SkillProgramCardPolicyKind.MarkerTurnBonuses when player.MarkerSourceCounts.GetValueOrDefault((PlayerMarkerKind.Camp, source.Seat)) > 0 && query is SkillRuleQuery.DrawCount or SkillRuleQuery.SlashLimit or SkillRuleQuery.HandLimit => 1,
                        SkillProgramCardPolicyKind.WoundedPopulationBonuses when source.Seat == player.Seat && query == SkillRuleQuery.HandLimit && WoundedCount() >= 1 => 1,
                        SkillProgramCardPolicyKind.WoundedPopulationBonuses when source.Seat == player.Seat && query == SkillRuleQuery.DrawCount && WoundedCount() >= 3 => 1,
                        SkillProgramCardPolicyKind.WoundedInRangeHandLimitPenalty when query == SkillRuleQuery.HandLimit && source.Seat != player.Seat && _coreTreatedWoundedOperations.IsRuleWounded(player) && _coreProgramDodgeResponsesOperations.IsWithinAttackRange(source.Seat, player.Seat) => -1,
                        _ => 0
                    };
                    if (value != 0)
                        yield return new FiniteRuleQueryContribution($"aura:{(source.Seat)}:{(instance.SkillId)}:{(instance.SkillInstanceId)}:{(policy.Id)}", SkillRuleOperation.Add, value);
                }
    }

    internal void SetStrategicMarker(int seat, PlayerMarkerKind marker, int sourceSeat, int count, ProgramSkillFrame frame)
    {
        var player = _coreMatchParticipantsPlayersAccess._players[seat];
        var old = player.Markers.GetValueOrDefault(marker);
        foreach (var key in player.MarkerSourceCounts.Keys.Where(key => key.Marker == marker).ToArray())
            player.MarkerSourceCounts.Remove(key);
        if (count == 0)
            player.Markers.Remove(marker);
        else
        {
            player.Markers[marker] = count;
            player.MarkerSourceCounts[(marker, sourceSeat)] = count;
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlayerMarkerChangedEvent(frame.Id, seat, marker, count - old, count, sourceSeat, $"skill-program.{(frame.SkillId)}.marker"));
    }

    internal SkillProgramStepOutcome ExecuteStrategicProgramEffect(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat)
    {
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        var host = _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost();
        var reason = new CardMoveReason($"skill-program.{(frame.SkillId)}.{(effect.Op)}");
        switch (effect.Op)
        {
            case SkillProgramEffectOp.SetMarkerAmount:
                var marker = effect.Marker ?? throw new InvalidOperationException("Marker operation requires a marker.");
                if (effect.Amount < 0)
                    SetStrategicMarker(targetSeat, marker, frame.OwnerSeat, 0, frame);
                else if (effect.Amount > 0)
                {
                    var holder = _coreMatchParticipantsPlayersAccess._players[targetSeat];
                    var source = (marker, frame.OwnerSeat);
                    var aggregate = checked(holder.Markers.GetValueOrDefault(marker) + effect.Amount);
                    holder.MarkerSourceCounts[source] = checked(holder.MarkerSourceCounts.GetValueOrDefault(source) + effect.Amount);
                    holder.Markers[marker] = aggregate;
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlayerMarkerChangedEvent(frame.Id, targetSeat, marker, effect.Amount, aggregate, frame.OwnerSeat, $"skill-program.{(frame.SkillId)}.marker"));
                }

                return SkillProgramStepOutcome.Continue;
            case SkillProgramEffectOp.MoveUniqueMarker:
                marker = effect.Marker ?? throw new InvalidOperationException("Marker transfer requires a marker.");
                if (!owner.Markers.ContainsKey(marker))
                    return SkillProgramStepOutcome.Continue;
                foreach (var holder in _coreMatchParticipantsPlayersAccess._players.Where(player => player.Markers.GetValueOrDefault(marker) > 0).ToArray())
                    SetStrategicMarker(holder.Seat, marker, frame.OwnerSeat, 0, frame);
                SetStrategicMarker(targetSeat, marker, frame.OwnerSeat, 1, frame);
                return SkillProgramStepOutcome.Continue;
            case SkillProgramEffectOp.ClaimMarkedHand:
                marker = effect.Marker ?? throw new InvalidOperationException("Marked hand claim requires a marker.");
                var turnOwner = _coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat];
                if (turnOwner.Seat != owner.Seat && turnOwner.MarkerSourceCounts.GetValueOrDefault((marker, owner.Seat)) > 0)
                {
                    _corePhysicalMovementFacadesOperations.MoveCards(_corePhysicalOwnedCardQueriesOperations.GetHand(turnOwner).ToArray(), CardLocation.Hand(turnOwner.Seat), CardLocation.Hand(owner.Seat), reason);
                    SetStrategicMarker(turnOwner.Seat, marker, owner.Seat, 0, frame);
                    SetStrategicMarker(owner.Seat, marker, owner.Seat, 1, frame);
                }

                return SkillProgramStepOutcome.Continue;
            case SkillProgramEffectOp.EndCurrentPlay:
                _coreGameEngineStrategicProgramsProgramSuppressionsStrategicEndPlayTurnStrategicHandLimitPenaltyTurnsAccess._strategicEndPlayTurn = _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber;
                return SkillProgramStepOutcome.Continue;
            case SkillProgramEffectOp.DiscardTargetEquipment:
                var equipmentDiscard = _coreNativeLogicalDiscardsOperations.CaptureNativeLogicalDiscardInvocation(frame.OwnerSeat, frame.OwnerSeat, frame.SelectedTargetSeats.SelectMany(seat => _corePhysicalOwnedCardQueriesOperations.GetEquipment2(seat).Where(card => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(frame.OwnerSeat, card, CardLocation.Equipment(seat), OwnedCardMoveIntent.Discard)).Select(card => card.Id)).ToArray(), reason, frame.Id);
                foreach (var seat in frame.SelectedTargetSeats)
                    _corePhysicalMovementFacadesOperations.MoveCards(_corePhysicalOwnedCardQueriesOperations.GetEquipment2(seat).Where(card => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(frame.OwnerSeat, card, CardLocation.Equipment(seat), OwnedCardMoveIntent.Discard)).ToArray(), CardLocation.Equipment(seat), CardLocation.DiscardPile, reason, movement => _characterBianFuRenOlOperations.RecordBianFuRenForeignDiscardProducer(movement, frame.OwnerSeat, frame.Id));
                _coreNativeLogicalDiscardsOperations.CompleteNativeLogicalDiscardInvocation(equipmentDiscard);
                return SkillProgramStepOutcome.Continue;
            case SkillProgramEffectOp.DamageOtherLiving:
                var batch = frame.StrategicDamageBatch;
                if (batch is null)
                    batch = new(frame.InstructionIndex, Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != owner.Seat).Select(player => player.Seat).ToArray()));
                if (batch.InstructionIndex != frame.InstructionIndex)
                    throw new InvalidOperationException("The strategic damage batch belongs to another instruction.");
                var remaining = batch.RemainingTargetSeats.SkipWhile(seat => !_coreMatchParticipantsPlayersAccess._players[seat].IsAlive).ToArray();
                if (remaining.Length == 0)
                {
                    _coreRuntimeOperations.ReplaceRuntimeTop(frame with { StrategicDamageBatch = null });
                    return SkillProgramStepOutcome.Continue;
                }

                var damageSeat = remaining[0];
                // The automatic instruction repeats until all frozen seats have resolved their complete damage windows.
                _coreRuntimeOperations.ReplaceRuntimeTop(frame with { ReexecuteParticipantInstruction = true, StrategicDamageBatch = batch with { RemainingTargetSeats = Array.AsReadOnly(remaining[1..]) } });
                return _coreSkillProgramsOperations.BeginProgramSkillDamage(frame, damageSeat, effect.Amount == 0 ? 1 : effect.Amount);
            case SkillProgramEffectOp.SelectDistinctSuitHandDiscards:
                var candidates = _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Where(card => !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(owner.Seat, card, CardLocation.Hand(owner.Seat), OwnedCardMoveIntent.Discard)).Concat(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[targetSeat])).Select(card => card.Id).ToArray();
                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { StrategicDraft = new(effect.Op, targetSeat, effect.ResultBind, effect.Amount == 3 ? 3 : 4) { Candidates = candidates } });
                PublishStrategicPrompt(frame);
                return SkillProgramStepOutcome.AwaitChoice;
            case SkillProgramEffectOp.ApplyHandDiscardShare:
                var binding = _coreProgramLifecycleOperations.GetProgramCardSet(frame, effect.SourceBind!);
                if (binding.CardIds.Count != 4 || binding.CardIds.Select((id, i) => _coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(frame.OwnerSeat, _cardZonesOperations._cardZones.CardsAt(binding.SourceLocations[i]).Single(card => card.Id == id), binding.SourceLocations[i], OwnedCardMoveIntent.Discard)).Any(blocked => blocked))
                    return SkillProgramStepOutcome.Continue;
                var mine = binding.SourceLocations.Count(location => location.OwnerSeat == frame.OwnerSeat);
                var sharedDiscard = _coreNativeLogicalDiscardsOperations.CaptureNativeLogicalDiscardInvocation(frame.OwnerSeat, frame.OwnerSeat, binding.CardIds, reason, frame.Id);
                foreach (var group in binding.CardIds.Select((id, index) => (id, Location: binding.SourceLocations[index])).GroupBy(item => item.Location))
                    _corePhysicalMovementFacadesOperations.MoveCards(group.Select(item => _cardZonesOperations._cardZones.CardsAt(item.Location).Single(card => card.Id == item.id)).ToArray(), group.Key, CardLocation.DiscardPile, reason, movement => _characterBianFuRenOlOperations.RecordBianFuRenForeignDiscardProducer(movement, frame.OwnerSeat, frame.Id));
                _coreNativeLogicalDiscardsOperations.CompleteNativeLogicalDiscardInvocation(sharedDiscard);
                if (mine == 0)
                    host.ChangeMaximumHp(frame, -1);
                if (mine == 1)
                {
                    _coreGameEngineStrategicProgramsProgramSuppressionsStrategicEndPlayTurnStrategicHandLimitPenaltyTurnsAccess._strategicHandLimitPenaltyTurns[owner.Seat] = _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber;
                    _coreGameEngineStrategicProgramsProgramSuppressionsStrategicEndPlayTurnStrategicHandLimitPenaltyTurnsAccess._strategicEndPlayTurn = _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber;
                }

                if (mine == 3)
                    host.Recover(frame.Id, owner.Seat, owner.Seat, 1, null, null);
                if (mine == 4)
                    host.Draw(frame.Id, owner.Seat, owner.Seat, 4, null, null, SkillProgramCardSetVisibility.Private, reason);
                return SkillProgramStepOutcome.Continue;
            case SkillProgramEffectOp.SuppressGeneralSkill:
                if (_coreGameEngineStrategicProgramsProgramSuppressionsStrategicEndPlayTurnStrategicHandLimitPenaltyTurnsAccess._programSuppressions.Any(item => item.SourceSeat == owner.Seat && item.TargetSeat == targetSeat))
                    return SkillProgramStepOutcome.Continue;
                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { StrategicDraft = new(effect.Op, targetSeat, null, 1) });
                PublishStrategicPrompt(frame);
                return _corePendingDecisionPolicyPipelineOperations._pendingDecision is null ? SkillProgramStepOutcome.Continue : SkillProgramStepOutcome.AwaitChoice;
            case SkillProgramEffectOp.SelectChainedByMarker:
                var count = owner.Markers.GetValueOrDefault(effect.Marker!.Value);
                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { StrategicDraft = new(effect.Op, -1, null, count) { Candidates = _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.IsChained).Select(player => player.Seat).ToArray() } });
                SetStrategicMarker(owner.Seat, effect.Marker.Value, owner.Seat, 0, frame);
                PublishStrategicPrompt(frame);
                return SkillProgramStepOutcome.AwaitChoice;
            case SkillProgramEffectOp.SelectOneSelectedTarget:
                if (_coreNamedAcquisitionObserversOperations.HasLostHpTargetSelection(frame))
                {
                    var live = frame.SelectedTargetSeats.Where(seat => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive).ToArray();
                    if (live.Length == 0)
                    {
                        _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "原连环目标均已死亡，未发行火焰伤害已取消。");
                        return SkillProgramStepOutcome.AwaitChild;
                    }

                    _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { SelectedTargetSeats = Array.AsReadOnly(live) });
                }

                if (frame.SelectedTargetSeats.Count == 0)
                    return SkillProgramStepOutcome.Continue;
                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { StrategicDraft = new(effect.Op, -1, null, 1) { Candidates = frame.SelectedTargetSeats } });
                PublishStrategicPrompt(frame);
                return SkillProgramStepOutcome.AwaitChoice;
            default:
                throw new InvalidOperationException("Unknown strategic operation.");
        }
    }

    internal void PublishStrategicPrompt(ProgramSkillFrame frame)
    {
        var draft = frame.StrategicDraft ?? throw new InvalidOperationException("Strategic choice lost its draft.");
        var choices = new List<PromptChoice>();
        PromptChoice Choice(string id, string label, IReadOnlyList<int> cards, IReadOnlyList<int> targets) => new(new ChoiceId($"strategic.frame-{(frame.Id)}.{(draft.Selected.Count)}.{(id)}"), label, cards, targets, new Dictionary<string, string> { ["program-action"] = "strategic-choice", ["frame-id"] = frame.Id.ToString(), ["selection-index"] = draft.Selected.Count.ToString(), ["choice"] = id });
        if (draft.Op == SkillProgramEffectOp.SelectDistinctSuitHandDiscards)
        {
            var used = draft.Selected.Select(id => _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(id)).Single(card => card.Id == id).Suit).ToHashSet();
            foreach (var id in draft.Candidates.Where(id => !draft.Selected.Contains(id)))
            {
                var card = _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(id)).Single(card => card.Id == id);
                if (used.Contains(card.Suit))
                    continue;
                var seat = _cardZonesOperations._cardZones.GetLocation(id).OwnerSeat!.Value;
                choices.Add(Choice($"card-{(id)}", $"{(_coreMatchParticipantsPlayersAccess._players[seat].Name)}：{(card.DisplayName)} {(card.RankText)}", [id], []));
            }

            choices.Add(Choice("decline", "不弃置牌", [], []));
        }
        else if (draft.Op is SkillProgramEffectOp.DiscardHandOrUseEquipment or SkillProgramEffectOp.MoveFieldEquipment)
        {
            // Both costs are mandatory once the owning skill activated; the draft only ever lists pay options.
            foreach (var cardId in draft.Candidates.Where(id => !draft.Selected.Contains(id)))
            {
                var location = _cardZonesOperations._cardZones.GetLocation(cardId);
                var card = _cardZonesOperations._cardZones.CardsAt(location).Single(item => item.Id == cardId);
                var holder = location.OwnerSeat is { } seat ? _coreMatchParticipantsPlayersAccess._players[seat].Name : "牌堆";
                choices.Add(Choice($"card-{(cardId)}", $"{(holder)}：{(card.DisplayName)}", [cardId], []));
            }
        }
        else if (draft.Op == SkillProgramEffectOp.SuppressGeneralSkill)
        {
            var target = _coreMatchParticipantsPlayersAccess._players[draft.TargetSeat];
            foreach (var skill in target.General.Skills.Concat(target.SecondaryGeneral?.Skills ?? []).Where(skill => skill.ContentId is not null).DistinctBy(skill => skill.ContentId))
                if (target.SkillGrants.Grants.Any(grant => grant.SkillId == skill.ContentId && grant.IsEnabled && !grant.IsModeRule))
                    choices.Add(Choice(skill.ContentId!, $"令【{(skill.Name)}】失效", [], [target.Seat]));
            if (choices.Count == 0)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(frame with { StrategicDraft = null });
                return;
            }
        }
        else
        {
            if (draft.Selected.Count < draft.Maximum)
                foreach (var seat in draft.Candidates.Where(seat => !draft.Selected.Contains(seat)))
                    choices.Add(Choice($"seat-{(seat)}", $"选择 {(_coreMatchParticipantsPlayersAccess._players[seat].Name)}", [], [seat]));
            if (draft.Op != SkillProgramEffectOp.SelectOneSelectedTarget)
                choices.Add(Choice("finish", "完成选择", [], []));
        }

        var skillDefinition = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, frame.OwnerSeat, $"【{(skillDefinition.Name)}】请选择。", [], [], frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = draft.Op == SkillProgramEffectOp.SelectDistinctSuitHandDiscards,
            TargetSeat = draft.TargetSeat >= 0 ? draft.TargetSeat : frame.OwnerSeat,
            SkillPrompt = new(frame.SkillId, skillDefinition.Name, skillDefinition.Name, skillDefinition.Description),
            Choices = choices.AsReadOnly()
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("strategic-choice")]
    internal void ResolveStrategicProgramChoice(PromptChoice choice)
    {
        var frame = _resolutionStackOperations._resolutionStack.Last() as ProgramSkillFrame ?? throw new InvalidOperationException("Strategic choice lost its frame.");
        var draft = frame.StrategicDraft ?? throw new InvalidOperationException("Strategic choice lost its draft.");
        if (choice.Parameters["frame-id"] != frame.Id.ToString() || choice.Parameters["selection-index"] != draft.Selected.Count.ToString())
            throw new InvalidOperationException("Strategic selection is stale.");
        var id = choice.Parameters["choice"];
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (draft.Op == SkillProgramEffectOp.SelectDistinctSuitHandDiscards)
        {
            if (id == "decline")
                _coreProgramLifecycleOperations.SetProgramCardSet(frame.Id, draft.Bind!, [], SkillProgramCardSetVisibility.Private, []);
            else
            {
                var cardId = choice.Cards.Single();
                var location = _cardZonesOperations._cardZones.GetLocation(cardId);
                if (!draft.Candidates.Contains(cardId) || location.Zone != CardZoneKind.Hand || location.OwnerSeat is not { } seat || seat != frame.OwnerSeat && seat != draft.TargetSeat)
                    throw new InvalidOperationException("Discard selection no longer owns the card.");
                var selected = draft.Selected.Append(cardId).ToArray();
                if (selected.Select(card => _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(card)).Single(item => item.Id == card).Suit).Distinct().Count() != selected.Length)
                    throw new InvalidOperationException("Discard selection repeats a suit.");
                if (selected.Length < draft.Maximum)
                {
                    _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { StrategicDraft = draft with { Selected = selected } });
                    PublishStrategicPrompt(frame);
                    return;
                }

                _coreProgramLifecycleOperations.SetProgramCardSet(frame.Id, draft.Bind!, selected, SkillProgramCardSetVisibility.Private, selected.Select(card => _cardZonesOperations._cardZones.GetLocation(card)).ToArray());
            }
        }
        else if (draft.Op is SkillProgramEffectOp.DiscardHandOrUseEquipment or SkillProgramEffectOp.MoveFieldEquipment)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { StrategicDraft = null });
            var cardId = choice.Cards.Single();
            if (draft.Op == SkillProgramEffectOp.DiscardHandOrUseEquipment)
                _characterCaoRenBoundaryOperations.ResolveDiscardHandOrUseEquipment(frame, cardId);
            else
                _characterCaoRenBoundaryOperations.ResolveMoveFieldEquipment(frame, cardId);
            return;
        }
        else if (draft.Op == SkillProgramEffectOp.SuppressGeneralSkill)
        {
            var target = _coreMatchParticipantsPlayersAccess._players[draft.TargetSeat];
            var grants = target.SkillGrants.Grants.Where(grant => grant.IsEnabled && !grant.IsModeRule && grant.SkillId == id).Select(grant => grant.GrantId).ToArray();
            if (grants.Length == 0)
                throw new InvalidOperationException("Suppressed skill is no longer owned.");
            foreach (var grant in grants)
                target.SkillGrants.SetEnabled(grant, false);
            _coreGameEngineStrategicProgramsProgramSuppressionsStrategicEndPlayTurnStrategicHandLimitPenaltyTurnsAccess._programSuppressions.Add(new(frame.OwnerSeat, target.Seat, id, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, grants));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSkillSuppressedEvent(frame.OwnerSeat, target.Seat, id, true));
            _coreGameEngineStrategicProgramsProgramSuppressionsStrategicEndPlayTurnStrategicHandLimitPenaltyTurnsAccess._strategicEndPlayTurn = _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber;
        }
        else
        {
            if (id != "finish")
            {
                var seat = choice.Targets.Single();
                if (!draft.Candidates.Contains(seat) || !_coreMatchParticipantsPlayersAccess._players[seat].IsAlive || draft.Op == SkillProgramEffectOp.SelectChainedByMarker && !_coreMatchParticipantsPlayersAccess._players[seat].IsChained || draft.Selected.Contains(seat) || draft.Selected.Count >= draft.Maximum)
                    throw new InvalidOperationException("Bound target selection is unavailable.");
                if (draft.Op == SkillProgramEffectOp.SelectOneSelectedTarget)
                {
                    _coreRuntimeOperations.ReplaceRuntimeTop(frame with { SelectedTargetSeats = [seat], StrategicDraft = null });
                    _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
                    return;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { StrategicDraft = draft with { Selected = draft.Selected.Append(seat).ToArray() } });
                PublishStrategicPrompt(frame);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { SelectedTargetSeats = draft.Selected });
        }

        // Card-set binding and target selection can already have replaced this frame.
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { StrategicDraft = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    internal void ExpireProgramSuppressions(int turnOwnerSeat)
    {
        foreach (var suppression in _coreGameEngineStrategicProgramsProgramSuppressionsStrategicEndPlayTurnStrategicHandLimitPenaltyTurnsAccess._programSuppressions.Where(item => item.TargetSeat == turnOwnerSeat && item.CreatedTurn < _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber).ToArray())
        {
            foreach (var id in suppression.GrantIds)
                if (_coreMatchParticipantsPlayersAccess._players[turnOwnerSeat].SkillGrants.Grants.Any(grant => grant.GrantId == id))
                    _coreMatchParticipantsPlayersAccess._players[turnOwnerSeat].SkillGrants.SetEnabled(id, true);
            _coreGameEngineStrategicProgramsProgramSuppressionsStrategicEndPlayTurnStrategicHandLimitPenaltyTurnsAccess._programSuppressions.Remove(suppression);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSkillSuppressedEvent(suppression.SourceSeat, turnOwnerSeat, suppression.SkillId, false));
        }
    }
}
