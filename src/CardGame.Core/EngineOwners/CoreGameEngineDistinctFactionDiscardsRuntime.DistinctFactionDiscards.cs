#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDistinctFactionDiscardsRuntime
{
    SkillProgramStepOutcome IDistinctFactionDiscardHost.BeginDistinctFactionDiscards(ProgramSkillFrame f) => BeginDistinctFactionDiscards(f);
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 20, SkillProgramEffectOp.DiscardDistinctFactionParticipants)]
    internal bool CanStartDistinctFactionDiscard(ProgramActivationLegalityContext gate) => HasPayableDistinctFactionOwnerHe(gate.Owner.Seat, gate.Program.Id, _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(gate.Owner, gate.Program.Id));
    internal bool HasPayableDistinctFactionOwnerHe(int seat, string skillId, string instanceId) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Count > 0 || _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[seat]).Any(c => !_coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(seat, skillId, instanceId, c));
    internal bool DistinctFactionPlanValid(int owner, IReadOnlyList<int> seats) => seats.Distinct().Count() == seats.Count && seats.All(s => _coreTableSeatQueriesOperations.IsValidPlayerSeat(s) && s != owner && _coreMatchParticipantsPlayersAccess._players[s].IsAlive && _coreForeignEquipmentDiscardPoliciesOperations.HasDiscardableHeBy(owner, s)) && seats.Select(s => _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(_coreMatchParticipantsPlayersAccess._players[s])).Distinct(StringComparer.Ordinal).Count() == seats.Count;
    internal bool DistinctFactionOwnerValid(ProgramSkillFrame f) => _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreProgramLifecycleOperations.CanInvokeProgramSkill(f);
    internal void CancelDistinctFactionDiscards(ProgramSkillFrame f, string reason)
    {
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, reason);
    }

    internal SkillProgramStepOutcome BeginDistinctFactionDiscards(ProgramSkillFrame f)
    {
        if (f.DistinctFactionDiscards is not null)
            throw new InvalidOperationException("A participant discard already owns a draft.");
        if (!DistinctFactionOwnerValid(f) || !HasPayableDistinctFactionOwnerHe(f.OwnerSeat, f.SkillId, f.SkillInstanceId))
        {
            CancelDistinctFactionDiscards(f, "弃牌持有人、来源或必付牌已失效，技能剩余步骤取消。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f with { DistinctFactionDiscards = new("select", Array.AsReadOnly(Array.Empty<int>()), Array.AsReadOnly(Array.Empty<int>()), 0, Array.AsReadOnly(Array.Empty<ProgramDiscardReceipt>())) });
        PublishDistinctFactionSelection(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void PublishDistinctFactionSelection(ProgramSkillFrame f)
    {
        var d = f.DistinctFactionDiscards!;
        var choices = new List<PromptChoice>();
        foreach (var p in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat && _coreForeignEquipmentDiscardPoliciesOperations.HasDiscardableHeBy(f.OwnerSeat, p.Seat)))
            if (DistinctFactionPlanValid(f.OwnerSeat, d.SelectedSeats.Append(p.Seat).ToArray()))
                choices.Add(new(new("participant-discard.select." + p.Seat), "选择 " + p.Name, [], [p.Seat], new Dictionary<string, string> { { "program-action", "distinct-faction-select" } }));
        choices.Add(new(new("participant-discard.finish"), "完成选择（可不选择其他角色）", [], [], new Dictionary<string, string> { { "program-action", "distinct-faction-finish" } }));
        PublishDistinctFactionPrompt(f, "选择有效势力互异的其他角色", choices);
    }

    internal void PublishDistinctFactionPrompt(ProgramSkillFrame f, string text, IReadOnlyList<PromptChoice> choices)
    {
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, text, Array.AsReadOnly(choices.SelectMany(c => c.Cards).Distinct().ToArray()), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description),
            Choices = Array.AsReadOnly(choices.ToArray())
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    internal IReadOnlyList<int> FreezeDiscardParticipantOrder(int owner, IReadOnlyList<int> chosen)
    {
        var order = new List<int>
        {
            owner
        };
        var seat = owner;
        for (var i = 0; i < _coreMatchDefinitionPlayerCountAccess._playerCount - 1; i++)
        {
            seat = _coreTableSeatQueriesOperations.FindNextAliveSeat(seat);
            if (seat == owner)
                break;
            if (chosen.Contains(seat))
                order.Add(seat);
        }

        if (order.Count != chosen.Count + 1)
            throw new InvalidOperationException("Participant plan lost its actual action order.");
        return Array.AsReadOnly(order.ToArray());
    }

    [ProgramAnswer("distinct-faction-select", "distinct-faction-finish", "distinct-faction-discard")]
    internal void ResolveDistinctFactionDiscardChoice(PromptChoice choice)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Missing participant discard frame.");
        var d = f.DistinctFactionDiscards ?? throw new InvalidOperationException("Missing participant discard draft.");
        var op = ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).GetPausedInstruction(f.InstructionIndex).Effect.Op;
        if (!DistinctFactionOwnerValid(f) || d.Stage == "select" && !HasPayableDistinctFactionOwnerHe(f.OwnerSeat, f.SkillId, f.SkillInstanceId))
        {
            CancelDistinctFactionDiscards(f, "弃牌持有人、来源或必付牌已失效，技能剩余步骤取消。");
            return;
        }

        if (d.Stage == "select")
        {
            if (choice.Parameters.GetValueOrDefault("program-action") == "distinct-faction-select")
            {
                var selected = d.SelectedSeats.Concat(choice.Targets).ToArray();
                if (choice.Targets.Count != 1 || !DistinctFactionPlanValid(f.OwnerSeat, selected))
                    throw new InvalidOperationException("Participants must be distinct living owners with different effective factions and HE.");
                _coreNativePromptBoundaryOperations.ClearPendingDecision();
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { DistinctFactionDiscards = d with { SelectedSeats = Array.AsReadOnly(selected) } });
                PublishDistinctFactionSelection(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
                return;
            }

            if (choice.Parameters.GetValueOrDefault("program-action") != "distinct-faction-finish" || !DistinctFactionPlanValid(f.OwnerSeat, d.SelectedSeats))
                throw new InvalidOperationException("Discard participant selection became invalid.");
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { DistinctFactionDiscards = d with { Stage = "discard", ParticipantSeats = FreezeDiscardParticipantOrder(f.OwnerSeat, d.SelectedSeats) } });
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return;
        }

        if (d.Stage != "discard" || d.ParticipantIndex >= d.ParticipantSeats.Count)
            throw new InvalidOperationException("Participant cost cursor is unavailable.");
        var owner = d.ParticipantSeats[d.ParticipantIndex];
        if (!_coreMatchParticipantsPlayersAccess._players[owner].IsAlive || !_coreForeignEquipmentDiscardPoliciesOperations.HasDiscardableHeBy(f.OwnerSeat, owner))
        {
            CancelOrSkipDistinctFactionPayment(f, d);
            return;
        }

        if (choice.Parameters.GetValueOrDefault("frame-id") != f.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) || choice.Parameters.GetValueOrDefault("card-owner-seat") != owner.ToString(System.Globalization.CultureInfo.InvariantCulture) || !Enum.TryParse<CardZoneKind>(choice.Parameters.GetValueOrDefault("source-zone"), out var zone) || zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || !int.TryParse(choice.Parameters.GetValueOrDefault("slot-index"), out var slot))
            throw new InvalidOperationException("Participant discard does not match its published opaque source slot.");
        var source = new CardLocation(zone, owner);
        var cards = _cardZonesOperations._cardZones.CardsAt(source);
        if (slot < 0 || slot >= cards.Count)
        {
            CancelOrSkipDistinctFactionPayment(f, d);
            return;
        }

        var card = cards[slot];
        var hidden = zone == CardZoneKind.Hand && owner != f.OwnerSeat;
        if (!hidden && (choice.Cards.Count != 1 || choice.Cards[0] != card.Id))
            throw new InvalidOperationException("Public participant cost identity changed.");
        if (zone == CardZoneKind.Equipment && owner == f.OwnerSeat && _coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(owner, f.SkillId, f.SkillInstanceId, card))
        {
            CancelOrSkipDistinctFactionPayment(f, d);
            return;
        }

        // Capture the source owner's effective suit before the entity leaves its original HE zone.
        if (_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(f.OwnerSeat, card, source, OwnedCardMoveIntent.Discard))
        {
            CancelOrSkipDistinctFactionPayment(f, d);
            return;
        }

        var suit = _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[owner], card);
        var paidDestination = _coreEphemeralCardMaterialsOperations.NormalizeEphemeralCardDestination(card, source, card.IsGeneralWeapon && source.Zone == CardZoneKind.Equipment ? CardLocation.OutsideGame : CardLocation.DiscardPile);
        var paidReason = new CardMoveReason("skill-program." + f.SkillId + ".participant-discard");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { DistinctFactionDiscards = d with { Stage = "discard-children", ParticipantIndex = d.ParticipantIndex + 1 }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        var beforeMovement = _coreCardMovementJournalCardMovementsAccess._cardMovements.Count;
        _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(f.OwnerSeat, f.OwnerSeat, card, source, paidReason, movement => _characterBianFuRenOlOperations.RecordBianFuRenForeignDiscardProducer(movement, f.OwnerSeat, f.Id));
        var movement = _coreCardMovementJournalCardMovementsAccess._cardMovements.Skip(beforeMovement).Single(m => m.CardId == card.Id && m.From == source && m.To == paidDestination && m.Reason == paidReason);
        if (movement.CardId != card.Id || movement.From != source || movement.To != paidDestination || movement.Reason != paidReason)
            throw new InvalidOperationException("A discard receipt requires its actual movement.");
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        d = f.DistinctFactionDiscards!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { DistinctFactionDiscards = d with { Receipts = Array.AsReadOnly(d.Receipts.Append(new ProgramDiscardReceipt(owner, card.Id, source, suit, movement.Sequence, movement.To == CardLocation.DiscardPile ? null : movement.To)).ToArray()) } });
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.ReturnRuntimeProgramMovement(f.Id);
    }

    internal void CancelOrSkipDistinctFactionPayment(ProgramSkillFrame f, ProgramDistinctFactionDiscardDraft d)
    {
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        // Owner payment is mandatory; only later unavailable participants may be skipped.
        if (d.ParticipantIndex == 0)
        {
            CancelDistinctFactionDiscards(f, "本人必付牌已失效，技能剩余步骤取消。");
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f with { DistinctFactionDiscards = d with { ParticipantIndex = d.ParticipantIndex + 1 } });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.DiscardDistinctFactionParticipants)]
    internal PromptChoice SelectAiDistinctFactionDiscard(PendingDecision decision, ProgramSkillFrame f)
    {
        if (f.DistinctFactionDiscards!.Stage == "select")
        {
            var(id, thought) = _coreAiBrainsAiBrainsAccess._aiBrains[decision.PlayerSeat].ChooseHostileHandTargets(_corePlayerViewProjectionOperations.CreateSnapshot(decision.PlayerSeat), decision.Choices, _coreAiDiagnosticsThoughtSequenceAccess._thoughtSequence++);
            _coreNativeAiDecisionsOperations.AddThought(thought);
            return decision.Choices.Single(c => c.Id == id);
        }

        var view = _corePlayerViewProjectionOperations.CreateSnapshot(decision.PlayerSeat);
        var visible = view.Players.SelectMany(p => p.Equipment.Concat(p.Seat == decision.PlayerSeat ? p.Hand : [])).ToDictionary(c => c.Id);
        // Opaque foreign Hand slots share one estimate; only published faces participate in cost ordering.
        return decision.Choices.OrderBy(c => c.Cards.Count == 1 && visible.TryGetValue(c.Cards[0], out var card) ? CardCatalog.Get(card.Kind).HandKeepValue : 0d).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First();
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 870)]
    internal bool ResumeDistinctFactionDiscards(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var d = f.DistinctFactionDiscards;
        if (d is null)
            return false;
        if (d.Stage is "discard-children" or "reward-children")
        {
            if (f.PendingMovementContinuation is not null)
                return true;
            if (_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
                return true;
            d = d with
            {
                Stage = d.Stage == "discard-children" ? "discard" : "reward"
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { DistinctFactionDiscards = d });
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        }

        if (!DistinctFactionOwnerValid(f))
        {
            CancelDistinctFactionDiscards(f, "弃牌持有人或来源已失效，技能剩余步骤取消。");
            return true;
        }

        if (d.Stage == "select")
        {
            if (!HasPayableDistinctFactionOwnerHe(f.OwnerSeat, f.SkillId, f.SkillInstanceId))
                CancelDistinctFactionDiscards(f, "本人必付牌已失效，技能剩余步骤取消。");
            return true;
        }

        while (d.Stage == "discard" && d.ParticipantIndex < d.ParticipantSeats.Count)
        {
            var seat = d.ParticipantSeats[d.ParticipantIndex];
            var costs = _coreMatchParticipantsPlayersAccess._players[seat].IsAlive ? _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(id, f.OwnerSeat, seat, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard) : [];
            if (costs.Count == 0 && d.ParticipantIndex == 0)
            {
                CancelDistinctFactionDiscards(f, "本人必付牌已失效，技能剩余步骤取消。");
                return true;
            }

            if (costs.Count == 0)
            {
                d = d with
                {
                    ParticipantIndex = d.ParticipantIndex + 1
                };
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { DistinctFactionDiscards = d });
                continue;
            }

            PublishDistinctFactionPrompt(f, "依行动顺序弃置 " + _coreMatchParticipantsPlayersAccess._players[seat].Name + " 的一张牌", costs.Select(c => c with { Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = "distinct-faction-discard" } }).ToArray());
            return true;
        }

        if (d.Stage == "discard")
        {
            d = d with
            {
                Stage = "reward"
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { DistinctFactionDiscards = d });
        }

        if (d.Stage == "reward")
        {
            var rewards = d.Receipts.Where(p => p.EffectiveSuit == Suit.Spade).Select(p => p.OwnerSeat).Distinct().ToArray();
            while (d.RewardIndex < rewards.Length)
            {
                var seat = rewards[d.RewardIndex];
                d = d with
                {
                    RewardIndex = d.RewardIndex + 1
                };
                if (!_coreMatchParticipantsPlayersAccess._players[seat].IsAlive)
                {
                    _coreRuntimeOperations.ReplaceRuntimeTop(f with { DistinctFactionDiscards = d });
                    continue;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(f with { DistinctFactionDiscards = d with { Stage = "reward-children" }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
                _coreProgramLifecycleOperations.DrawProgramCards(id, seat, 1, null, null, SkillProgramCardSetVisibility.Private, new("skill-program." + f.SkillId + ".participant-reward"));
                if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
                    _coreRuntimeOperations.ReturnRuntimeProgramMovement(id);
                return true;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f with { DistinctFactionDiscards = null });
            return false;
        }

        return true;
    }
}
