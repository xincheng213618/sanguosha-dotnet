#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramHandControlRuntime
{
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 10)]
    internal bool AllowsDrawTurnOwnerThenDiscardMaximumHandForDodgeActivation(ProgramActivationLegalityContext gate)
    {
        var owner = gate.Owner;
        var program = gate.Program;
        var features = gate.Features;
        return !(features.HasOperation(SkillProgramEffectOp.DrawTurnOwnerThenDiscardMaximumHandForDodge) || !_coreLordSkillProjectionsOperations.HasSkillRoleQualification(owner, program.Id, null, Role.Lord) && features.HasOperation(SkillProgramEffectOp.GrantGameFactionAttackRangeTargets));
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeCondition, 60)]
    internal bool CanOfferProgramHandControl(CharacterState owner, SkillProgramTrigger trigger) => !trigger.Effects.Any(effect => effect.Op == SkillProgramEffectOp.ChooseHandCountIntervention) || _coreMatchParticipantsPlayersAccess._players.Any(player => player.IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(player).Count != _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Count);
    SkillProgramStepOutcome global::CardGame.Core.IHandControlProgramHost.ExecuteHandControl(SkillProgramEffect effect, ProgramSkillFrame frame) => ExecuteProgramHandControl(effect, frame);
    internal SkillProgramStepOutcome ExecuteProgramHandControl(SkillProgramEffect effect, ProgramSkillFrame frame)
    {
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        if (effect.Op == SkillProgramEffectOp.LoseOwnerSkillsAndGrant)
            return _coreProgramSkillChangesOperations.ReplaceProgramOwnerSkills(frame, effect);
        if (frame.HandControlDraft is null)
        {
            var stage = effect.Op switch
            {
                SkillProgramEffectOp.ChooseHandCountIntervention => "intervention",
                SkillProgramEffectOp.RevealHandColorDiscardAndTake => "color",
                SkillProgramEffectOp.DrawThenPutOwnedCardOnTopParticipants => "participants",
                SkillProgramEffectOp.DrawTurnOwnerThenDiscardMaximumHandForDodge => "response-draw",
                _ => throw new InvalidOperationException("Unsupported hand-control operation.")};
            var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(owner).ToArray();
            if (stage == "color" && hand.Length == 0)
                return SkillProgramStepOutcome.Continue;
            if (stage == "color")
            {
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCardsRevealedEvent(frame.Id, frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), owner.Seat, "hand-color-cost", hand.Select(_coreSnapshotValueProjectionOperations.ToSnapshot).ToArray()));
                frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
            }

            var maximum = stage == "participants" ? Math.Max(0, effect.BooleanValue == true ? owner.MaxHp : owner.Hp) : 1;
            if (maximum == 0)
                return SkillProgramStepOutcome.Continue;
            frame = frame with
            {
                HandControlDraft = new(effect.Op, stage, maximum, [], RevealedCardIds: stage == "color" ? hand.Select(card => card.Id).ToArray() : null)
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(frame);
            if (stage == "color" && _characterXiaoQiaoMouOlOperations.TryBeginPublicHandRevealWindow(frame.Id, "hand-color-whole-hand"))
                return SkillProgramStepOutcome.AwaitChild;
        }

        var draft = frame.HandControlDraft!;
        if (draft.Stage == "response-draw")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { HandControlDraft = draft with { Stage = "response-compare" }, ReexecuteParticipantInstruction = true });
            if (_coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatAccess._currentSeat].IsAlive)
                _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatAccess._currentSeat], 1, true, new CardMoveReason("program.maximum-hand-dodge.draw"));
            return SkillProgramStepOutcome.Continue;
        }

        if (draft.Stage == "response-compare")
        {
            var maximum = _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive).Max(player => _corePhysicalOwnedCardQueriesOperations.GetHand(player).Count);
            var leaders = _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(player).Count == maximum).Select(player => player.Seat).ToArray();
            if (leaders.SequenceEqual([_coreActualTurnProgressCurrentSeatAccess._currentSeat]))
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(frame with { HandControlDraft = null });
                _coreProgramDodgeResponsesOperations.CompleteProgramMaximumHandDodge(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), succeeded: false);
                return SkillProgramStepOutcome.AwaitChild;
            }

            draft = draft with
            {
                Stage = "response-targets",
                ParticipantSeats = leaders
            };
            frame = frame with
            {
                HandControlDraft = draft
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(frame);
        }

        if (draft.Stage == "response-complete")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { HandControlDraft = null });
            _coreProgramDodgeResponsesOperations.CompleteProgramMaximumHandDodge(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), succeeded: true);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (draft.Stage == "participant-draw")
        {
            while (draft.ParticipantIndex < draft.ParticipantSeats.Count && !_coreMatchParticipantsPlayersAccess._players[draft.ParticipantSeats[draft.ParticipantIndex]].IsAlive)
                draft = draft with
                {
                    ParticipantIndex = draft.ParticipantIndex + 1
                };
            if (draft.ParticipantIndex == draft.ParticipantSeats.Count)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(frame with { HandControlDraft = null });
                return SkillProgramStepOutcome.Continue;
            }

            var seat = draft.ParticipantSeats[draft.ParticipantIndex];
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { HandControlDraft = draft with { Stage = "participant-top", CardOwnerSeat = seat }, ReexecuteParticipantInstruction = true });
            _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[seat], 1, log: true, new CardMoveReason("program.participant-top.draw"));
            return SkillProgramStepOutcome.Continue;
        }

        if (draft.Stage == "participant-top")
        {
            var seat = draft.CardOwnerSeat!.Value;
            var cards = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(seat)).ToArray();
            if (!_coreMatchParticipantsPlayersAccess._players[seat].IsAlive || cards.Length == 0)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(frame with { HandControlDraft = draft with { Stage = "participant-draw", CardOwnerSeat = null, ParticipantIndex = draft.ParticipantIndex + 1 }, ReexecuteParticipantInstruction = true });
                return SkillProgramStepOutcome.Continue;
            }

            draft = draft with
            {
                CandidateCardIds = cards.Select(card => card.Id).ToArray(),
                CandidateLocations = cards.Select(card => _cardZonesOperations._cardZones.GetLocation(card.Id)).ToArray()
            };
            frame = frame with
            {
                HandControlDraft = draft
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(frame);
        }

        if (draft.Stage == "take-targets" && (draft.ParticipantSeats.Count == draft.Maximum || !HandControlTakeTargets(frame).Any()))
            return FinishProgramHandColorTake(frame);
        if (BuildProgramHandControlChoices(frame).Count == 0)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { HandControlDraft = null });
            if (draft.Operation == SkillProgramEffectOp.DrawTurnOwnerThenDiscardMaximumHandForDodge)
            {
                _coreProgramDodgeResponsesOperations.CompleteProgramMaximumHandDodge(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), succeeded: false);
                return SkillProgramStepOutcome.AwaitChild;
            }

            return SkillProgramStepOutcome.Continue;
        }

        PublishProgramHandControlChoice(frame);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IEnumerable<CharacterState> HandControlTakeTargets(ProgramSkillFrame frame) => _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != frame.OwnerSeat && !frame.HandControlDraft!.ParticipantSeats.Contains(player.Seat) && _corePhysicalOwnedCardQueriesOperations.GetHand(player).Count + _corePhysicalOwnedCardQueriesOperations.GetEquipment(player).Count > 0);
    internal IReadOnlyList<PromptChoice> BuildProgramHandControlChoices(ProgramSkillFrame frame)
    {
        var draft = frame.HandControlDraft ?? throw new InvalidOperationException("Missing hand-control draft.");
        var result = new List<PromptChoice>();
        void Add(string action, string label, IReadOnlyList<int>? cards = null, IReadOnlyList<int>? seats = null, string? zone = null, string? slot = null) => result.Add(new(new ChoiceId($"hand-control.{(frame.Id)}.{(draft.Stage)}.{(result.Count)}"), label, cards ?? [], seats ?? [], new Dictionary<string, string> { ["program-action"] = "hand-control", ["hand-control-action"] = action, ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["stage"] = draft.Stage, ["source-zone"] = zone ?? "", ["slot"] = slot ?? "" }));
        if (draft.Stage == "intervention")
        {
            var count = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]).Count;
            foreach (var player in _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive))
            {
                var theirCount = _corePhysicalOwnedCardQueriesOperations.GetHand(player).Count;
                if (theirCount < count)
                    Add("draw", $"令 {(player.Name)} 摸一张牌", seats: [player.Seat]);
                if (theirCount > count)
                    Add("discard-target", $"弃置 {(player.Name)} 一张手牌", seats: [player.Seat]);
            }
        }
        else if (draft.Stage == "color")
        {
            if (_corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(frame)?.StateId == "two-colors" && draft.RevealedCardIds!.Select(id => HandControlIsRed(frame.OwnerSeat, _coreAdvancedLifecycleOperations.GetAdvancedCard(id))).Distinct().Count() != 2)
                return result;
            var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]);
            foreach (var red in new[]
            {
                false,
                true
            }

            )
                if (hand.Any(card => HandControlIsRed(frame.OwnerSeat, card) == red) && !hand.Any(card => HandControlIsRed(frame.OwnerSeat, card) == red && _coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(frame.OwnerSeat, card, CardLocation.Hand(frame.OwnerSeat), OwnedCardMoveIntent.Discard)))
                    Add(red ? "red" : "black", $"弃置全部{((red ? "红色" : "黑色"))}手牌");
        }
        else if (draft.Stage == "participants")
        {
            foreach (var player in _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && !draft.ParticipantSeats.Contains(player.Seat)))
                if (draft.ParticipantSeats.Count < draft.Maximum)
                    Add("participant", $"选择 {(player.Name)}", seats: [player.Seat]);
            Add("finish", "完成目标选择");
        }
        else if (draft.Stage == "take-targets")
        {
            foreach (var player in HandControlTakeTargets(frame))
                Add("take-target", $"获得 {(player.Name)} 一张牌", seats: [player.Seat]);
            Add("finish", "完成取牌");
        }
        else if (draft.Stage == "response-targets")
        {
            var maximum = _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive).Max(player => _corePhysicalOwnedCardQueriesOperations.GetHand(player).Count);
            foreach (var player in _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(player).Count == maximum && (_coreForeignEquipmentDiscardPoliciesOperations.HasDiscardableHeBy(frame.OwnerSeat, player.Seat) || _corePhysicalOwnedCardQueriesOperations.GetJudgment(player).Count > 0)))
                Add("response-target", $"弃置 {(player.Name)} 一张牌", seats: [player.Seat]);
        }
        else if (draft.Stage is "take-card" or "intervention-card" or "participant-top" or "response-card")
        {
            var seat = draft.CardOwnerSeat!.Value;
            var zones = draft.Stage == "intervention-card" ? new[]
            {
                CardZoneKind.Hand
            }

            : draft.Stage == "response-card" ? new[]
            {
                CardZoneKind.Hand,
                CardZoneKind.Equipment,
                CardZoneKind.Judgment
            }

            : new[]
            {
                CardZoneKind.Hand,
                CardZoneKind.Equipment
            };
            foreach (var zone in zones)
            {
                var cards = _cardZonesOperations._cardZones.CardsAt(new(zone, seat));
                for (var slot = 0; slot < cards.Count; slot++)
                {
                    var card = cards[slot];
                    if (draft.Stage == "response-card" && _coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(frame.OwnerSeat, card, new(zone, seat), OwnedCardMoveIntent.Discard))
                        continue;
                    var opaque = zone == CardZoneKind.Hand && seat != frame.OwnerSeat && draft.Stage != "participant-top";
                    Add("card", opaque ? $"选择 {(_coreMatchParticipantsPlayersAccess._players[seat].Name)} 的第{(slot + 1)}个暗置手牌牌位" : $"选择【{(card.DisplayName)}】", opaque ? [] : [card.Id], [seat], zone.ToString(), slot.ToString(System.Globalization.CultureInfo.InvariantCulture));
                }
            }
        }

        return result;
    }

    internal bool HandControlIsRed(int seat, Card card) => _coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(_coreMatchParticipantsPlayersAccess._players[seat], card)is Suit.Heart or Suit.Diamond;
    internal void PublishProgramHandControlChoice(ProgramSkillFrame frame)
    {
        var draft = frame.HandControlDraft!;
        var chooser = draft.Stage == "participant-top" ? draft.CardOwnerSeat!.Value : frame.OwnerSeat;
        var choices = BuildProgramHandControlChoices(frame);
        if (draft.Stage == "color" && choices.Count == 0)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "没有可完整支付的颜色手牌费用。");
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, $"【{(skill.Name)}】请选择", choices.SelectMany(choice => choice.Cards).Distinct().ToArray(), choices.SelectMany(choice => choice.Targets).Distinct().ToArray(), frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = draft.CardOwnerSeat,
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description),
            Choices = choices
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("hand-control")]
    internal void ResolveProgramHandControlChoice(PromptChoice choice)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Missing hand-control frame.");
        var draft = frame.HandControlDraft ?? throw new InvalidOperationException("Missing hand-control draft.");
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } decision || choice.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) || choice.Parameters.GetValueOrDefault("stage") != draft.Stage || !decision.Choices.Any(item => item.Id == choice.Id) || !BuildProgramHandControlChoices(frame).Any(item => item.Id == choice.Id && item.Cards.SequenceEqual(choice.Cards) && item.Targets.SequenceEqual(choice.Targets) && item.Parameters.Count == choice.Parameters.Count && item.Parameters.All(parameter => choice.Parameters.GetValueOrDefault(parameter.Key) == parameter.Value)))
            throw new InvalidOperationException("Hand-control choice lost its physical sources or fresh public condition.");
        var action = choice.Parameters["hand-control-action"];
        if (draft.Stage == "color" && !_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]).Select(card => card.Id).SequenceEqual(draft.RevealedCardIds!))
            throw new InvalidOperationException("The revealed hand changed before color payment.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "技能实例或角色已失效。");
            return;
        }

        if (draft.Stage == "intervention")
        {
            var seat = choice.Targets.Single();
            if (action == "draw")
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(frame with { HandControlDraft = null });
                _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[seat], 1, true, new CardMoveReason("program.hand-count.draw"));
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramHandCountInterventionEvent(frame.Id, frame.SkillId, frame.OwnerSeat, seat, "draw"));
                _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { HandControlDraft = draft with { Stage = "intervention-card", CardOwnerSeat = seat } });
            PublishProgramHandControlChoice(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id));
            return;
        }

        if (draft.Stage == "color")
        {
            var red = action == "red";
            var cards = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]).Where(card => HandControlIsRed(frame.OwnerSeat, card) == red).ToArray();
            if (cards.Length == 0)
                throw new InvalidOperationException("Color cost cannot be empty.");
            if (cards.Any(card => _coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(frame.OwnerSeat, card, CardLocation.Hand(frame.OwnerSeat), OwnedCardMoveIntent.Discard)))
            {
                _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "完整颜色费用包含不可自弃手牌，未支付。");
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { HandControlDraft = draft with { Stage = "take-targets", Maximum = cards.Length }, ReexecuteParticipantInstruction = true });
            _coreNativeLogicalDiscardsOperations.MoveLogicalDiscardCards(frame.OwnerSeat, frame.OwnerSeat, cards, CardLocation.Hand(frame.OwnerSeat), new CardMoveReason("program.hand-color.discard"));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramHandColorDiscardEvent(frame.Id, frame.SkillId, frame.OwnerSeat, red, cards.Select(card => card.Id).ToArray()));
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
            return;
        }

        if (draft.Stage == "participants")
        {
            if (action == "participant")
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(frame with { HandControlDraft = draft with { ParticipantSeats = [..draft.ParticipantSeats, choice.Targets.Single()] } });
                PublishProgramHandControlChoice(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id));
                return;
            }

            var ordered = draft.ParticipantSeats.OrderBy(seat => _coreTableOrderOperations.RelativeTableOrder(frame.OwnerSeat, seat)).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { HandControlDraft = draft with { Stage = "participant-draw", ParticipantSeats = ordered }, ReexecuteParticipantInstruction = true });
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
            return;
        }

        if (draft.Stage == "take-targets")
        {
            if (action == "finish")
            {
                var outcome = FinishProgramHandColorTake(frame);
                if (outcome == SkillProgramStepOutcome.Continue)
                    _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { HandControlDraft = draft with { Stage = "take-card", CardOwnerSeat = choice.Targets.Single() } });
            PublishProgramHandControlChoice(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id));
            return;
        }

        if (draft.Stage == "response-targets")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { HandControlDraft = draft with { Stage = "response-card", CardOwnerSeat = choice.Targets.Single() } });
            PublishProgramHandControlChoice(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id));
            return;
        }

        if (action != "card" || draft.CardOwnerSeat is not { } cardOwner || !Enum.TryParse<CardZoneKind>(choice.Parameters["source-zone"], out var zone) || !int.TryParse(choice.Parameters["slot"], out var slot))
            throw new InvalidOperationException("Malformed hand-control card choice.");
        var source = new CardLocation(zone, cardOwner);
        var card = _cardZonesOperations._cardZones.CardsAt(source)[slot];
        var reason = new CardMoveReason(draft.Stage == "participant-top" ? "program.participant-top.put" : draft.Stage == "take-card" ? "program.hand-color.obtain" : "program.hand-count.discard");
        if (draft.Stage == "participant-top")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { HandControlDraft = draft with { Stage = "participant-draw", ParticipantIndex = draft.ParticipantIndex + 1, CardOwnerSeat = null, CandidateCardIds = null, CandidateLocations = null }, ReexecuteParticipantInstruction = true });
            _corePhysicalMovementFacadesOperations.MoveCard(card, source, CardLocation.DrawPile, reason);
            _cardZonesOperations._cardZones.PlaceDrawPileCardsAtTop([card.Id]);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramParticipantCardPlacedOnTopEvent(frame.Id, frame.SkillId, frame.OwnerSeat, cardOwner, zone));
        }
        else if (draft.Stage == "take-card")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { HandControlDraft = draft with { Stage = "take-targets", ParticipantSeats = [..draft.ParticipantSeats, cardOwner], CardOwnerSeat = null, ObtainedCount = draft.ObtainedCount + 1 }, ReexecuteParticipantInstruction = true });
            _corePhysicalMovementFacadesOperations.MoveCard(card, source, CardLocation.Hand(frame.OwnerSeat), reason);
        }
        else if (draft.Stage == "response-card")
        {
            if (_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(frame.OwnerSeat, card, source, OwnedCardMoveIntent.Discard))
                throw new InvalidOperationException("Response discard is no longer available.");
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { HandControlDraft = draft with { Stage = "response-complete", CardOwnerSeat = null }, ReexecuteParticipantInstruction = true });
            _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(frame.OwnerSeat, frame.OwnerSeat, card, source, new CardMoveReason("program.maximum-hand-dodge.discard"), movement => _characterBianFuRenOlOperations.RecordBianFuRenForeignDiscardProducer(movement, frame.OwnerSeat, frame.Id));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramHandCountInterventionEvent(frame.Id, frame.SkillId, frame.OwnerSeat, cardOwner, "response-discard"));
        }
        else
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { HandControlDraft = null });
            _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(frame.OwnerSeat, frame.OwnerSeat, card, source, reason);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramHandCountInterventionEvent(frame.Id, frame.SkillId, frame.OwnerSeat, cardOwner, "discard"));
        }

        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    internal SkillProgramStepOutcome FinishProgramHandColorTake(ProgramSkillFrame frame)
    {
        var obtained = frame.HandControlDraft!.ObtainedCount;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { HandControlDraft = null });
        return obtained >= 2 ? _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(frame.Id, frame.SkillId, frame.OwnerSeat, 1) : SkillProgramStepOutcome.Continue;
    }

    internal void AssertProgramHandControlDraft(ProgramSkillFrame frame, SkillProgramEffect paused)
    {
        if (frame.HandControlDraft is not { } draft)
            return;
        var stages = draft.Operation switch
        {
            SkillProgramEffectOp.ChooseHandCountIntervention => new[]
            {
                "intervention",
                "intervention-card"
            },
            SkillProgramEffectOp.RevealHandColorDiscardAndTake => ["color", "take-targets", "take-card"],
            SkillProgramEffectOp.DrawThenPutOwnedCardOnTopParticipants => ["participants", "participant-draw", "participant-top"],
            SkillProgramEffectOp.DrawTurnOwnerThenDiscardMaximumHandForDodge => ["response-draw", "response-compare", "response-targets", "response-card", "response-complete"],
            _ => []
        };
        var needsCardOwner = draft.Stage is "intervention-card" or "take-card" or "participant-top" or "response-card";
        if (draft.Operation != paused.Op || draft.Maximum < 1 || draft.ParticipantSeats.Distinct().Count() != draft.ParticipantSeats.Count || draft.ParticipantSeats.Any(seat => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(seat)) || draft.ParticipantIndex < 0 || draft.ParticipantIndex > draft.ParticipantSeats.Count || draft.ObtainedCount < 0 || draft.ObtainedCount > draft.Maximum || draft.CardOwnerSeat is { } seat && !_coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) || needsCardOwner != draft.CardOwnerSeat.HasValue || !stages.Contains(draft.Stage) || draft.Operation is SkillProgramEffectOp.RevealHandColorDiscardAndTake or SkillProgramEffectOp.DrawThenPutOwnedCardOnTopParticipants && draft.ParticipantSeats.Count > draft.Maximum || draft.Operation == SkillProgramEffectOp.RevealHandColorDiscardAndTake && (draft.RevealedCardIds is null || draft.RevealedCardIds.Count == 0 || draft.RevealedCardIds.Distinct().Count() != draft.RevealedCardIds.Count || draft.Maximum > draft.RevealedCardIds.Count || draft.ParticipantSeats.Count != draft.ObtainedCount || draft.ParticipantSeats.Contains(frame.OwnerSeat)) || draft.Operation != SkillProgramEffectOp.RevealHandColorDiscardAndTake && draft.RevealedCardIds is not null || (draft.CandidateCardIds is null) != (draft.CandidateLocations is null) || draft.CandidateCardIds is { } ids && (draft.Stage != "participant-top" || ids.Count != draft.CandidateLocations!.Count || ids.Distinct().Count() != ids.Count || ids.Where((id, index) => _cardZonesOperations._cardZones.GetLocation(id) != draft.CandidateLocations[index]).Any()))
            throw new InvalidOperationException("Invalid hand-control instruction-owned draft.");
        if (ReferenceEquals(frame, _resolutionStackOperations._resolutionStack.LastOrDefault()) && _corePendingDecisionPolicyPipelineOperations._pendingDecision is { Kind: DecisionKind.ProgramTrigger } decision && decision.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "hand-control"))
        {
            var chooser = draft.Stage == "participant-top" ? draft.CardOwnerSeat : frame.OwnerSeat;
            var expected = BuildProgramHandControlChoices(frame);
            if (!decision.IsPrivate || decision.PlayerSeat != chooser || decision.Choices.Count != expected.Count || !decision.Choices.Zip(expected).All(pair => pair.First.Id == pair.Second.Id && pair.First.Cards.SequenceEqual(pair.Second.Cards) && pair.First.Targets.SequenceEqual(pair.Second.Targets) && pair.First.Parameters.Count == pair.Second.Parameters.Count && pair.First.Parameters.All(parameter => pair.Second.Parameters.GetValueOrDefault(parameter.Key) == parameter.Value)))
                throw new InvalidOperationException("Hand-control prompt lost its fresh choices or private chooser.");
        }
    }

    [PublicHandRevealReturn("hand-color-whole-hand")]
    internal void ReturnHandColorPublicHand(ProgramSkillFrame frame)
    {
        if (frame.HandControlDraft is not { Stage: "color" })
            throw new InvalidOperationException("Hand color control lost its original displayed roster.");
        var effect = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
        if (ExecuteProgramHandControl(effect, frame) == SkillProgramStepOutcome.Continue)
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }
}
