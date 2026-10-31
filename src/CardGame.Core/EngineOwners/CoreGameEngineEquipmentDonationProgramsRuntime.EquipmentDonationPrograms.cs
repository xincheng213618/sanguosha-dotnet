#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineEquipmentDonationProgramsRuntime
{
    internal const string EquipmentDonationMoveReason = "program.all-equipment-donation.give";
    internal static string EquipmentDonationUsage(string id) => RuntimeSkills.RuntimeRules_Core_EquipmentDonationPrograms.EquipmentDonationUsage(id);
    internal long EquipmentDonationSequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count == 0 ? 0 : _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements[^1].Sequence;

    internal SkillProgramStepOutcome AwaitEquipmentDonationMovements(long frameId)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(frameId, PostEventContinuation.AwaitedProgramMovement) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frameId, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(frameId))
            return SkillProgramStepOutcome.AwaitChild;
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(frameId)with { PendingMovementContinuation = null });
        return SkillProgramStepOutcome.Continue;
    }

    internal bool HasPayableAllEquipmentDonation(int ownerSeat, string skillId, string usageId) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(ownerSeat) && _coreMatchParticipantsPlayersAccess._players[ownerSeat].IsAlive && _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[ownerSeat])is { Count: > 0 } equipment && equipment.All(c => !c.IsGeneralWeapon) && _skillRuntimeStateOperations._skillRuntimeState.GetUsage(ownerSeat, skillId, EquipmentDonationUsage(usageId), SkillUsageScope.Game) == 0;
    internal bool EquipmentDonationSourceValid(ProgramSkillFrame f) => _characterCaiFuRenOlOperations.IsIssuedOrdinaryCaiFuRenXianzhou(f) || _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreProgramLifecycleOperations.CanInvokeProgramSkill(f) && _coreSkillProgramsOperations.EnabledSkillPrograms(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Any(p => p.Id == f.SkillId && p.GameplayHash == f.GameplayHash);
    internal Dictionary<string, string> EquipmentDonationParameters(ProgramSkillFrame f, string action, string option) => new()
    {
        ["program-action"] = action,
        ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
        ["option"] = option
    };
    internal void PublishEquipmentDonationPrompt(ProgramSkillFrame f, int chooser, string text, IReadOnlyList<PromptChoice> choices)
    {
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, text, choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), SourceSeat: f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            TargetSeat = chooser,
            Choices = Array.AsReadOnly(choices.ToArray()),
            IsPrivate = _characterCaiFuRenOlOperations.IsIssuedOrdinaryCaiFuRenXianzhou(f),
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    internal SkillProgramStepOutcome RunAllEquipmentDonation(ProgramSkillFrame frame, string usageId)
    {
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (frame.EquipmentDonation is null)
        {
            if (frame.TriggerId is not null || frame.InstructionIndex != 1 || frame.SelectedCardIds.Count != 0 || frame.SelectedTargetSeats is not [var recipient] || recipient == frame.OwnerSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(recipient) || !_coreMatchParticipantsPlayersAccess._players[recipient].IsAlive || !EquipmentDonationSourceValid(frame) || !HasPayableAllEquipmentDonation(frame.OwnerSeat, frame.SkillId, usageId))
                throw new InvalidOperationException("All-equipment donation requires one current unpaid source and recipient.");
            var cards = _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]).OrderBy(c => c.Id).ToArray();
            var before = EquipmentDonationSequence;
            // One actual source batch, and one actual recipient batch. All entities are paid before any observer resumes.
            _corePhysicalMovementFacadesOperations.MoveCards(cards, CardLocation.Equipment(frame.OwnerSeat), CardLocation.Processing, new(EquipmentDonationMoveReason), movement => _characterBianFuRenOlOperations.RecordBianFuRenOwnedTransferProducer(movement, recipient, frame.Id));
            _corePhysicalMovementFacadesOperations.MoveCards(cards, CardLocation.Processing, CardLocation.Hand(recipient), new(EquipmentDonationMoveReason), _characterHuangQuanMouOperations.NativeGiveCompletion(frame.Id, frame.OwnerSeat, recipient, cards.Select(c => c.Id).ToArray(), _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, null));
            var after = EquipmentDonationSequence;
            var delivered = cards.Count(c => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence > before && m.Sequence <= after && m.CardId == c.Id && m.From == CardLocation.Processing && m.To == CardLocation.Hand(recipient) && m.Reason.Value == EquipmentDonationMoveReason));
            var source = new CardConversionSource(frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId);
            if (!_skillRuntimeStateOperations._skillRuntimeState.TryConsumeUsage(frame.OwnerSeat, frame.SkillId, EquipmentDonationUsage(usageId), SkillUsageScope.Game, 1))
                throw new InvalidOperationException("The paid donation lost its exact unconsumed game-wide opportunity.");
            frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { ReexecuteParticipantInstruction = true, EquipmentDonation = new(0, source, frame.GameplayHash, usageId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, recipient, cards.Select(c => c.Id).ToArray(), delivered, before, after, EquipmentDonationStage.PaidMovement, []) });
            foreach (var(card, index)in cards.Select((card, index) => (card, index)))
            {
                var movement = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Single(m => m.Sequence > before && m.Sequence <= after && m.CardId == card.Id && m.From == CardLocation.Equipment(frame.OwnerSeat) && m.To == CardLocation.Processing && m.Reason.Value == EquipmentDonationMoveReason);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new EquipmentDonationEntityPaidEvent(frame.Id, source, recipient, card.Id, index, movement.Sequence, _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence > before && m.Sequence <= after && m.CardId == card.Id && m.From == CardLocation.Processing && m.To == CardLocation.Hand(recipient) && m.Reason.Value == EquipmentDonationMoveReason)));
            }

            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SkillUsageConsumedEvent(frame.OwnerSeat, frame.SkillId, EquipmentDonationUsage(usageId), SkillUsageScope.Game, 1));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new EquipmentDonationPaidEvent(frame.Id, source, frame.GameplayHash, usageId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, recipient, cards.Length, delivered, before, after));
            return AwaitEquipmentDonationMovements(frame.Id);
        }

        if (!_coreEquipmentDonationObserversOperations.Value.IsValidAllEquipmentDonation(frame))
            throw new InvalidOperationException("Donation lost its frozen actual payment or limited usage.");
        var receipt = frame.EquipmentDonation;
        if (receipt.Stage is EquipmentDonationStage.Complete or EquipmentDonationStage.RecoveryIssued)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { EquipmentDonation = receipt with { Stage = EquipmentDonationStage.Complete } });
            return SkillProgramStepOutcome.Continue;
        }

        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !EquipmentDonationSourceValid(frame) || !_coreMatchParticipantsPlayersAccess._players[receipt.RecipientSeat].IsAlive)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { EquipmentDonation = receipt with { Stage = EquipmentDonationStage.Complete } });
            return SkillProgramStepOutcome.Continue;
        }

        if (receipt.Stage == EquipmentDonationStage.Damaging)
        {
            var cursor = receipt.DamageCursor;
            while (cursor < receipt.DamageTargets.Count && !_coreMatchParticipantsPlayersAccess._players[receipt.DamageTargets[cursor]].IsAlive)
                cursor++;
            if (cursor >= receipt.DamageTargets.Count)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(frame with { EquipmentDonation = receipt with { Stage = EquipmentDonationStage.Complete } });
                return SkillProgramStepOutcome.Continue;
            }

            var target = receipt.DamageTargets[cursor];
            receipt = receipt with
            {
                DamageCursor = cursor + 1
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { EquipmentDonation = receipt, ReexecuteParticipantInstruction = true });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new EquipmentDonationBenefitIssuedEvent(frame.Id, receipt.Source, receipt.RecipientSeat, "damage", receipt.ActualDeliveredCount, target, cursor));
            return _coreSkillProgramsOperations.BeginProgramSkillDamage(frame, target, 1, new(ProgramParticipantRef.SelectedTarget));
        }

        receipt = receipt with
        {
            Stage = receipt.Stage == EquipmentDonationStage.PaidMovement ? EquipmentDonationStage.RecipientChoice : receipt.Stage
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { EquipmentDonation = receipt, ReexecuteParticipantInstruction = true });
        PublishEquipmentDonationPrompt(frame, receipt.RecipientSeat, "选择回复持有者体力，或选择攻击范围内的角色造成伤害。", EquipmentDonationChoices(frame));
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> EquipmentDonationChoices(ProgramSkillFrame f)
    {
        var receipt = f.EquipmentDonation!;
        var choices = new List<PromptChoice>();
        void Add(string option, string label, IReadOnlyList<int> targets) => choices.Add(new(new($"equipment-donation.{(f.Id)}.{(option)}"), label, [], targets, EquipmentDonationParameters(f, "equipment-donation", option)));
        if (receipt.Stage == EquipmentDonationStage.RecipientChoice)
        {
            Add("recover", $"令持有者回复 {(receipt.ActualDeliveredCount)} 点体力。", [f.OwnerSeat]);
            Add("damage", "选择攻击范围内至多 X 名角色。", []);
            return choices;
        }

        if (receipt.Stage != EquipmentDonationStage.SelectingTargets)
            throw new InvalidOperationException("Donation target prompt has no selecting receipt.");
        if (receipt.DamageTargets.Count < receipt.ActualDeliveredCount)
            foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != receipt.RecipientSeat && !receipt.DamageTargets.Contains(p.Seat) && _coreProgramDodgeResponsesOperations.IsWithinAttackRange(receipt.RecipientSeat, p.Seat)).OrderBy(p => p.Seat))
                Add($"target:{(target.Seat)}", $"选择 {(target.Name)}。", [target.Seat]);
        Add("confirm", "确认已选目标；可不选择任何角色。", receipt.DamageTargets);
        return choices;
    }

    internal void ResolveEquipmentDonationChoice(ProgramSkillFrame f, PromptChoice choice)
    {
        if (!_coreEquipmentDonationObserversOperations.Value.IsValidAllEquipmentDonation(f) || f.EquipmentDonation is not { } receipt || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != receipt.RecipientSeat)
            throw new InvalidOperationException("The recipient choice lost its exact paid donation.");
        choice = EquipmentDonationChoices(f).Single(c => c.Id == choice.Id);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!EquipmentDonationSourceValid(f) || !_coreMatchParticipantsPlayersAccess._players[receipt.RecipientSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { EquipmentDonation = receipt with { Stage = EquipmentDonationStage.Complete }, ReexecuteParticipantInstruction = true });
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return;
        }

        switch (choice.Parameters["option"])
        {
            case "recover":
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { EquipmentDonation = receipt with { Stage = EquipmentDonationStage.RecoveryIssued }, ReexecuteParticipantInstruction = true });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new EquipmentDonationBenefitIssuedEvent(f.Id, receipt.Source, receipt.RecipientSeat, "recover", receipt.ActualDeliveredCount, f.OwnerSeat, 0));
                _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, receipt.RecipientSeat, f.OwnerSeat, receipt.ActualDeliveredCount, null, null);
                _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
                return;
            case "damage":
                receipt = receipt with
                {
                    Stage = EquipmentDonationStage.SelectingTargets
                };
                break;
            case "confirm":
                if (receipt.DamageTargets.Any(seat => !_coreMatchParticipantsPlayersAccess._players[seat].IsAlive || !_coreProgramDodgeResponsesOperations.IsWithinAttackRange(receipt.RecipientSeat, seat)))
                    throw new InvalidOperationException("A donation target left the recipient's actual range before confirmation.");
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { EquipmentDonation = receipt with { Stage = EquipmentDonationStage.Damaging }, ReexecuteParticipantInstruction = true });
                _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
                return;
            default:
                if (choice.Targets is not [var seat] || receipt.DamageTargets.Contains(seat) || receipt.DamageTargets.Count >= receipt.ActualDeliveredCount)
                    throw new InvalidOperationException("Donation requires distinct targets within the frozen actual X.");
                receipt = receipt with
                {
                    DamageTargets = receipt.DamageTargets.Append(seat).ToArray()
                };
                break;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { EquipmentDonation = receipt, ReexecuteParticipantInstruction = true });
        PublishEquipmentDonationPrompt(f, receipt.RecipientSeat, $"已选择 {(receipt.DamageTargets.Count)} 名角色。", EquipmentDonationChoices(f));
    }

    [ProgramAiChoice(SkillProgramEffectOp.DonateAllEquipmentAndOfferRecipientBenefits)]
    internal PromptChoice SelectAiEquipmentDonation(PendingDecision decision, ProgramSkillFrame f)
    {
        var r = f.EquipmentDonation!;
        var view = _corePlayerViewProjectionOperations.CreateSnapshot(r.RecipientSeat);
        if (r.Stage == EquipmentDonationStage.RecipientChoice)
        {
            var recover = _coreAiBrainsAiBrainsAccess._aiBrains[r.RecipientSeat].ScoreProgramTarget(view, f.OwnerSeat, new(0, 0, 0, 0, r.ActualDeliveredCount, 0, false, false));
            return decision.Choices.Single(c => c.Parameters["option"] == (recover > 0 && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp < _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp ? "recover" : "damage"));
        }

        return decision.Choices.Where(c => c.Parameters["option"].StartsWith("target:", StringComparison.Ordinal)).Select(c => (Choice: c, Value: _coreAiBrainsAiBrainsAccess._aiBrains[r.RecipientSeat].ScoreProgramTarget(view, c.Targets.Single(), new(0, 0, 0, 0, 0, 1, false, false)))).Where(c => c.Value > 0).OrderByDescending(c => c.Value).ThenBy(c => c.Choice.Targets.Single()).Select(c => c.Choice).FirstOrDefault() ?? decision.Choices.Single(c => c.Parameters["option"] == "confirm");
    }

    SkillProgramStepOutcome IEquipmentDonationProgramHost.DonateAllEquipmentAndOfferRecipientBenefits(ProgramSkillFrame f, string usageId) => RunAllEquipmentDonation(f, usageId);
    SkillProgramStepOutcome IEquipmentDonationProgramHost.ChooseEquipmentOrDrawAfterOtherActualTurn(ProgramSkillFrame f) => _coreActualEndedTurnEquipmentOperations.RunActualEndedTurnEquipment(f);
}
