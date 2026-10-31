#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePlacedEquipmentBenefitsRuntime
{
    internal const string PlacedEquipmentDrawReason = "skill-program.placed-equipment-benefit.draw";
    internal const string PlacedEquipmentDiscardReason = "skill-program.placed-equipment-benefit.discard";
    internal bool PlacedEquipmentSourceCurrent(ProgramSkillFrame f) => _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId);
    internal bool CanPlaceBenefitEquipment(int owner, Card card, CardLocation from) => !card.IsGeneralWeapon && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && _coreEquipmentPlacementAndHpPairOperations.CanPlaceOwnedEquipment(owner, p.Seat, card, from));
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 780)]
    internal bool CanRunPlacedEquipmentBenefit(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => !t.Effects.Any(e => e.Op == SkillProgramEffectOp.PlaceOwnedEquipmentThenResolveSlotBenefit) || context.Window == SkillProgramTriggerWindow.TurnEnding && c.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat])).Any(card => _coreEquipmentContentOperations.EquipmentRules.IsEquipment(card.Kind) && !_coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(c.OwnerSeat, c.SkillId, c.SkillInstanceId, card) && CanPlaceBenefitEquipment(c.OwnerSeat, card, _cardZonesOperations._cardZones.GetLocation(card.Id)));
    internal bool PlacedEquipmentParentMatches(ProgramSkillFrame f)
    {
        if (f.WindowContext is not { Window: SkillProgramTriggerWindow.TurnEnding } context || f.TriggerId is null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || context.OwnerSeat != f.OwnerSeat || context.SourceSeat != f.OwnerSeat || context.TargetSeat != f.OwnerSeat || f.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || _resolutionStackOperations._resolutionStack.OfType<TurnEndingBoundaryFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } ending || ending.OwnerSeat != f.OwnerSeat || ending.TurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || ending.ItemIndex < 0 || ending.ItemIndex >= ending.Items.Count || ending.Items[ending.ItemIndex].Candidate is not { } candidate || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, candidate) || context.OccurrenceIndex != candidate.OccurrenceIndex || ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).Instructions is not [{ Op: SkillProgramEffectOp.PlaceOwnedEquipmentThenResolveSlotBenefit }])
            return false;
        return true; // No whole Facts equality: dictionary/list reference identity is not an owning proof.
    }

    internal Dictionary<string, string> PlacedEquipmentArgs(ProgramSkillFrame f, string step) => new()
    {
        ["program-action"] = "placed-equipment-benefit",
        ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
        ["step"] = step
    };
    internal IReadOnlyList<PromptChoice> PlacedEquipmentChoices(ProgramSkillFrame f)
    {
        var r = f.PlacedEquipmentBenefit!;
        var choices = new List<PromptChoice>();
        void Add(string step, string id, string label, IReadOnlyList<int> cards, IReadOnlyList<int> targets) => choices.Add(new(new($"placed-equipment.{(f.Id)}.{(id)}"), label, cards, targets, PlacedEquipmentArgs(f, step)));
        void AddOwned(IReadOnlyList<PromptChoice> owned, string step)
        {
            foreach (var c in owned)
            {
                var args = c.Parameters.ToDictionary(k => k.Key, k => k.Value, StringComparer.Ordinal);
                args["program-action"] = "placed-equipment-benefit";
                args["step"] = step;
                choices.Add(new(c.Id, c.Description, c.Cards, c.Targets, args));
            }
        }

        if (r.Stage == PlacedEquipmentBenefitStage.EquipmentChoice)
            AddOwned(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, f.OwnerSeat, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Transfer, canSelect: (_, c) => _coreEquipmentContentOperations.EquipmentRules.IsEquipment(c.Kind) && CanPlaceBenefitEquipment(f.OwnerSeat, c, _cardZonesOperations._cardZones.GetLocation(c.Id))), "equipment");
        else if (r.Stage == PlacedEquipmentBenefitStage.RecipientChoice && r.SelectedCardId is { } id && r.SelectedFrom is { } from)
            foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && _coreEquipmentPlacementAndHpPairOperations.CanPlaceOwnedEquipment(f.OwnerSeat, p.Seat, _coreAdvancedLifecycleOperations.GetAdvancedCard(id), from)).OrderBy(p => p.Seat))
                Add("recipient", $"recipient.{(target.Seat)}", $"置入 {(target.Name)} 的装备区。", [], [target.Seat]);
        else if (r.Stage == PlacedEquipmentBenefitStage.WeaponTarget)
            foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && _coreNativeRuleQueriesOperations.GetCombatDistance(r.Placement!.RecipientSeat, p.Seat) == 1 && _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, p.Seat, [CardZoneKind.Hand, CardZoneKind.Equipment, CardZoneKind.Judgment], OwnedCardMoveIntent.Discard).Count > 0).OrderBy(p => p.Seat))
                Add("weapon-target", $"target.{(target.Seat)}", $"弃置 {(target.Name)} 区域里的一张牌。", [], [target.Seat]);
        else if (r.Stage == PlacedEquipmentBenefitStage.WeaponCard && r.WeaponTargetSeat is { } seat)
            AddOwned(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, seat, [CardZoneKind.Hand, CardZoneKind.Equipment, CardZoneKind.Judgment], OwnedCardMoveIntent.Discard), "weapon-card");
        if (r.Placement is null)
            Add("cancel", "cancel", "不支付装备，取消。", [], []);
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishPlacedEquipmentChoice(ProgramSkillFrame f)
    {
        var r = f.PlacedEquipmentBenefit!;
        if (r.Placement is null && !PlacedEquipmentSourceCurrent(f) || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || r.Placement is { } paid && !_coreMatchParticipantsPlayersAccess._players[paid.RecipientSeat].IsAlive)
        {
            FinishPlacedEquipmentBenefit(f);
            return;
        }

        var choices = PlacedEquipmentChoices(f);
        if (choices.Count == 0)
        {
            FinishPlacedEquipmentBenefit(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "选择装备置入及其真实后续收益。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = f.OwnerSeat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    internal SkillProgramStepOutcome BeginPlacedEquipmentBenefit(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (!PlacedEquipmentParentMatches(f) || f.PlacedEquipmentBenefit is not null || !PlacedEquipmentSourceCurrent(f))
            throw new InvalidOperationException("Equipment placement requires its exact optional own Ending candidate.");
        var r = new ProgramPlacedEquipmentBenefitReceipt(f.InstructionIndex, new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, f.WindowContext!.ParentFrameId, f.WindowContext.OccurrenceIndex, PlacedEquipmentBenefitStage.EquipmentChoice);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PlacedEquipmentBenefit = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlacedEquipmentBenefitStartedEvent(f.Id, r.Source, r.GameplayHash, r.TurnNumber, r.TurnOwnerSeat, r.EndingFrameId, r.OccurrenceIndex));
        PublishPlacedEquipmentChoice(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    [ProgramAnswer("placed-equipment-benefit")]
    internal void ResolvePlacedEquipmentChoice(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || !_corePlacedEquipmentBenefitObserversOperations.ValidPlacedEquipmentBenefit(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } prompt || prompt.PlayerSeat != f.OwnerSeat || !PlacedEquipmentChoices(f).Any(c => c.Id == selected.Id && c.Cards.SequenceEqual(selected.Cards) && c.Targets.SequenceEqual(selected.Targets)))
            throw new InvalidOperationException("Equipment placement requires the exact published private choice.");
        var r = f.PlacedEquipmentBenefit!;
        var step = selected.Parameters["step"];
        if (r.Placement is null && !PlacedEquipmentSourceCurrent(f) || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            FinishPlacedEquipmentBenefit(f);
            return;
        }

        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (step == "cancel")
        {
            FinishPlacedEquipmentBenefit(f);
            return;
        }

        if (step is "equipment" or "weapon-card")
        {
            var owner = step == "equipment" ? f.OwnerSeat : r.WeaponTargetSeat!.Value;
            if (!Enum.TryParse<CardZoneKind>(selected.Parameters.GetValueOrDefault("source-zone"), out var zone) || !int.TryParse(selected.Parameters.GetValueOrDefault("slot-index"), out var index))
                throw new InvalidOperationException("Missing real card slot.");
            var from = new CardLocation(zone, owner);
            var cards = _cardZonesOperations._cardZones.CardsAt(from);
            if (index < 0 || index >= cards.Count)
                throw new InvalidOperationException("The published slot no longer exists.");
            var card = cards[index];
            // A foreign hand choice is opaque. Only its original slot identifies its entity after accepted selection.
            if (zone != CardZoneKind.Hand || owner == f.OwnerSeat)
                if (selected.Cards is not [var visible] || visible != card.Id)
                    throw new InvalidOperationException("Changed visible entity.");
            if (step == "equipment")
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PlacedEquipmentBenefit = r with { SelectedCardId = card.Id, SelectedFrom = from, Stage = PlacedEquipmentBenefitStage.RecipientChoice } });
                PublishPlacedEquipmentChoice(f);
                return;
            }

            if (!_coreMatchParticipantsPlayersAccess._players[owner].IsAlive || _coreNativeRuleQueriesOperations.GetCombatDistance(r.Placement!.RecipientSeat, owner) != 1 || _coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(f.OwnerSeat, card, from, OwnedCardMoveIntent.Discard))
                throw new InvalidOperationException("Weapon benefit lost its current directed-distance or discard eligibility.");
            var before = _coreCardMovementJournalMovementSequenceAccess._movementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(f.OwnerSeat, f.OwnerSeat, card, from, new(PlacedEquipmentDiscardReason), movement => _characterBianFuRenOlOperations.RecordBianFuRenForeignDiscardProducer(movement, f.OwnerSeat, f.Id));
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var payment = new PlacedEquipmentDiscard(owner, card.Id, card.Kind, from, card.IsGeneralWeapon, before, _coreCardMovementJournalMovementSequenceAccess._movementSequence);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PlacedEquipmentBenefit = r with { Discard = payment, Stage = PlacedEquipmentBenefitStage.DiscardChildren } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlacedEquipmentBenefitDiscardedEvent(f.Id, payment));
            ContinuePlacedEquipmentMovement(f);
            return;
        }

        if (step == "weapon-target")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PlacedEquipmentBenefit = r with { WeaponTargetSeat = selected.Targets.Single(), Stage = PlacedEquipmentBenefitStage.WeaponCard } });
            PublishPlacedEquipmentChoice(f);
            return;
        }

        if (step != "recipient" || selected.Targets is not [var recipient] || r.SelectedCardId is not { } cardId || r.SelectedFrom is not { } source)
            throw new InvalidOperationException("Equipment placement lost its actual selected recipient.");
        var equipment = _coreAdvancedLifecycleOperations.GetAdvancedCard(cardId);
        if (equipment.IsGeneralWeapon || !_coreEquipmentPlacementAndHpPairOperations.CanPlaceOwnedEquipment(f.OwnerSeat, recipient, equipment, source))
            throw new InvalidOperationException("The chosen equipment cannot actually enter this recipient's slot.");
        var slot = _coreFieldEquipmentOperations.FieldEquipmentSlot(equipment);
        var equipped = _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[recipient]).Where(c => _coreFieldEquipmentOperations.FieldEquipmentSlot(c) == slot).ToArray();
        var replaced = equipped.Length >= _coreMatchParticipantsPlayersAccess._players[recipient].EquipmentSlotCapacity(slot) ? equipped[0] : null;
        var seq = _coreCardMovementJournalMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        if (replaced is not null)
        {
            _coreAdvancedWeaponsOperations.CopyFirstReplacedWeapon(equipment, replaced);
            _corePhysicalMovementFacadesOperations.MoveCard(replaced, CardLocation.Equipment(recipient), CardLocation.DiscardPile, CardMoveReasons.EquipmentReplace);
        }

        _corePhysicalMovementFacadesOperations.MoveCard(equipment, source, CardLocation.Equipment(recipient), CardMoveReasons.EquipmentEnter);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id); // Preserve real queued Silver Lion / recovery-replacement fields after both moves.
        var placement = new PlacedEquipmentPayment(cardId, equipment.Kind, source, recipient, slot, replaced?.Id, replaced?.IsGeneralWeapon == true, seq, _coreCardMovementJournalMovementSequenceAccess._movementSequence);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PlacedEquipmentBenefit = r with { Placement = placement, Stage = PlacedEquipmentBenefitStage.PlacementChildren } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new EquipmentChangedEvent(f.Id, recipient, slot, cardId, equipment.Kind, replaced?.Id));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlacedEquipmentBenefitPaidEvent(f.Id, placement));
        ContinuePlacedEquipmentMovement(f);
    }

    internal void ContinuePlacedEquipmentMovement(ProgramSkillFrame f)
    {
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id))
            return;
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 390)]
    internal bool ReturnPlacedEquipmentMovement(ProgramSkillFrame f)
    {
        if (f.PlacedEquipmentBenefit is null)
            return false;
        if (f.PendingMovementContinuation is null || !_corePlacedEquipmentBenefitObserversOperations.ValidPlacedEquipmentBenefit(f))
            throw new InvalidOperationException("Equipment benefit lost its paid movement.");
        ContinuePlacedEquipmentMovement(f);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 700)]
    internal bool ResumePlacedEquipmentBenefit(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id || f.PlacedEquipmentBenefit is not { } r)
            return false;
        if (!_corePlacedEquipmentBenefitObserversOperations.ValidPlacedEquipmentBenefit(f))
            throw new InvalidOperationException("Equipment benefit lost its exact original candidate and payment.");
        if (f.PendingMovementContinuation is not null)
        {
            ContinuePlacedEquipmentMovement(f);
            return true;
        }

        if (r.Stage == PlacedEquipmentBenefitStage.RecoveryChildren)
        {
            if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
                return true;
            FinishPlacedEquipmentBenefit(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
            return true;
        }

        if (r.Stage is PlacedEquipmentBenefitStage.DrawChildren or PlacedEquipmentBenefitStage.DiscardChildren)
        {
            FinishPlacedEquipmentBenefit(f);
            return true;
        }

        if (r.Stage != PlacedEquipmentBenefitStage.PlacementChildren)
        {
            PublishPlacedEquipmentChoice(f);
            return true;
        }

        var payment = r.Placement!;
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[payment.RecipientSeat].IsAlive)
        {
            FinishPlacedEquipmentBenefit(f);
            return true;
        }

        if (payment.Slot == EquipmentSlot.Weapon)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PlacedEquipmentBenefit = r with { Stage = PlacedEquipmentBenefitStage.WeaponTarget } });
            PublishPlacedEquipmentChoice(f);
            return true;
        }

        if (payment.Slot == EquipmentSlot.Armor)
        {
            var before = _coreCardMovementJournalMovementSequenceAccess._movementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            var actual = _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[payment.RecipientSeat], 1, true, new(PlacedEquipmentDrawReason)).Count;
            var invoice = new ProgramOneCardDrawInvoice(payment.RecipientSeat, before, _coreCardMovementJournalMovementSequenceAccess._movementSequence, actual);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { PlacedEquipmentBenefit = r with { Stage = PlacedEquipmentBenefitStage.DrawChildren, Draw = invoice } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlacedEquipmentBenefitDrawnEvent(id, invoice));
            ContinuePlacedEquipmentMovement(f);
            return true;
        }

        if (payment.Slot is EquipmentSlot.OffensiveHorse or EquipmentSlot.DefensiveHorse)
        {
            var recipient = _coreMatchParticipantsPlayersAccess._players[payment.RecipientSeat];
            var amount = recipient.HasHpValue ? Math.Min(1, recipient.MaxHp - recipient.Hp) : 0;
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { PlacedEquipmentBenefit = r with { Stage = PlacedEquipmentBenefitStage.RecoveryChildren, RecoveryIssued = true, ActualRecoveryRequest = amount } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlacedEquipmentBenefitRecoveryIssuedEvent(id, payment.RecipientSeat, amount));
            _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(id, f.OwnerSeat, payment.RecipientSeat, 1, null, null);
            _coreRuntimeOperations.AdvanceRuntimeProgram(id);
            return true;
        }

        FinishPlacedEquipmentBenefit(f);
        return true; // Treasure has no listed slot benefit in this current definition.
    }

    internal void FinishPlacedEquipmentBenefit(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlacedEquipmentBenefitFinishedEvent(f.Id, f.PlacedEquipmentBenefit!.Placement is not null));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { PlacedEquipmentBenefit = null, PendingMovementContinuation = null });
        // The sole opt-in instruction owns paid completion even after source invalidation. Unpaid cancellation pays nothing.
        _coreSkillProgramsOperations.FinishProgramSkill(f, completed: true);
    }

    [ProgramAiChoice(SkillProgramEffectOp.PlaceOwnedEquipmentThenResolveSlotBenefit)]
    internal PromptChoice SelectAiPlacedEquipmentBenefit(PendingDecision decision, ProgramSkillFrame f)
    {
        var r = f.PlacedEquipmentBenefit!;
        var view = _corePlayerViewProjectionOperations.CreateSnapshot(f.OwnerSeat);
        if (r.Stage is PlacedEquipmentBenefitStage.RecipientChoice or PlacedEquipmentBenefitStage.WeaponTarget)
            return decision.Choices.Where(c => c.Parameters["step"] != "cancel").Select(c => (Choice: c, Score: _coreAiBrainsAiBrainsAccess._aiBrains[f.OwnerSeat].ScoreProgramTarget(view, c.Targets.Single(), r.Stage == PlacedEquipmentBenefitStage.RecipientChoice ? new(0, 0, 0, 0, 0, 0, true, false) : new(0, 0, 0, 0, 0, 0, false, true)))).OrderByDescending(c => c.Score).ThenBy(c => c.Choice.Id.Value, StringComparer.Ordinal).Select(c => c.Choice).FirstOrDefault() ?? decision.Choices.Single(c => c.Parameters["step"] == "cancel");
        // The owner sees its own material. Foreign hand choices carry no identity; no hidden card is inspected for scoring.
        return decision.Choices.Where(c => c.Parameters["step"] != "cancel").OrderBy(c => c.Parameters.GetValueOrDefault("source-zone") == "Hand" ? 0 : 1).ThenBy(c => c.Id.Value, StringComparer.Ordinal).FirstOrDefault() ?? decision.Choices[0];
    }

    SkillProgramStepOutcome global::CardGame.Core.IPlacedEquipmentBenefitProgramHost.PlaceOwnedEquipmentThenResolveSlotBenefit(ProgramSkillFrame f) => BeginPlacedEquipmentBenefit(f);
}
