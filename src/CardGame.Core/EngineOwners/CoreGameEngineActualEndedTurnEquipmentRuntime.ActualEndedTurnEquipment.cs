#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineActualEndedTurnEquipmentRuntime
{
    internal const string ActualEndedEquipmentMoveReason = "program.actual-ended-turn.equipment";
    internal const string ActualEndedEquipmentDrawReason = "program.actual-ended-turn.draw";

    internal bool IsActualEndedEquipmentTrigger(SkillProgramTrigger trigger) => trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.ChooseEquipmentOrDrawAfterOtherActualTurn);
    internal ActualEndedTurnEquipmentContext? CaptureActualEndedEquipmentContext(DeferredTurnEndFrame parent,
        ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (!_coreAfterTurnEndedProgramsOperations.IsActualAfterTurnEndedParent(parent))
            return null;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var end = Array.FindLastIndex(history, e => e is TurnEndedEvent ended && ended.TurnNumber == parent.TurnNumber && ended.ActorSeat == parent.OwnerSeat);
        if (end < 0)
            return null;
        var start = Array.FindLastIndex(history, end, e => e is TurnStartedEvent begun && begun.TurnNumber == parent.TurnNumber && begun.ActorSeat == parent.OwnerSeat);
        if (start < 0 || history.Skip(start + 1).Take(end - start - 1).Any(e => e is TurnStartedEvent or TurnEndedEvent))
            return null;
        var actual = history.Skip(start + 1).Take(end - start - 1).ToArray();
        var damaged = actual.OfType<DamageAppliedEvent>().Any(e => !e.SourceLess && e.Amount > 0 && e.SourceSeat == parent.OwnerSeat && e.TargetSeat != parent.OwnerSeat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(e.TargetSeat));
        var effect = _coreProgramLifecycleOperations.GetProgramTrigger2(candidate).Effects[0];
        var endedPoint = new CardUseHistoryPoint(parent.TurnNumber, parent.OwnerSeat, end + 1L);
        var world = _expressions.CreateTriggerExpressionWorld(candidate, context with { HistoryPoint = endedPoint }, context.Facts);
        if (ProgramExpressions.EvaluateParticipantSet(effect.TargetSetExpression!, world) is not { } targets)
            return null;
        var used = targets.Any(target => target != parent.OwnerSeat);
        return new(parent.TurnNumber, parent.OwnerSeat, damaged, used, !used ? 2 : !damaged ? 1 : 0);
    }

    internal bool MatchesActualEndedEquipmentContext(DeferredTurnEndFrame parent, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context, SkillProgramTrigger trigger, bool requireLivingTurnOwner = true)
    {
        var frozen = context.ActualEndedEquipment;
        return IsActualEndedEquipmentTrigger(trigger) && frozen is not null && frozen.TurnNumber == parent.TurnNumber && frozen.TurnOwnerSeat == parent.OwnerSeat && frozen.MaximumDistinctOptions > 0 && trigger.Window == SkillProgramTriggerWindow.AfterTurnEnded && trigger.TurnOwnerScope == SkillProgramTurnOwnerScope.OtherLiving && context.Window == trigger.Window && context.ParentFrameId == parent.Id && candidate.OwnerSeat != parent.OwnerSeat && (!requireLivingTurnOwner || _coreMatchParticipantsPlayersAccess._players[parent.OwnerSeat].IsAlive) && context.OwnerSeat == candidate.OwnerSeat && context.SourceSeat == parent.OwnerSeat && context.TargetSeat == parent.OwnerSeat && context.OccurrenceIndex == candidate.OccurrenceIndex && _coreAfterTurnEndedProgramsOperations.AfterTurnEndedCandidate(parent) == candidate;
    }

    internal bool MatchesActualEndedEquipmentRoot(ProgramSkillFrame f)
    {
        if (f.WindowContext is not { Window: SkillProgramTriggerWindow.AfterTurnEnded, ActualEndedEquipment: not null } context || _resolutionStackOperations._resolutionStack.OfType<DeferredTurnEndFrame>().SingleOrDefault(p => p.Id == context.ParentFrameId)is not { } parent || parent.AfterTurnEnded?.CurrentChild is not { } child || child.FrameId != f.Id || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, child.Candidate))
            return false;
        return MatchesActualEndedEquipmentContext(parent, child.Candidate, context, _coreProgramLifecycleOperations.GetProgramTrigger2(child.Candidate), requireLivingTurnOwner: false);
    }

    internal SkillProgramStepOutcome RunActualEndedTurnEquipment(ProgramSkillFrame f)
    {
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        if (!MatchesActualEndedEquipmentRoot(f))
            throw new InvalidOperationException("Equipment options require the original completed actual turn and exact current candidate.");
        if (f.ActualEndedEquipment is null)
        {
            var context = f.WindowContext!;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ActualEndedEquipment = new(0, new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, context.ParentFrameId, context.OccurrenceIndex, context.ActualEndedEquipment!, ActualEndedTurnEquipmentStage.Offering) });
        }

        if (!_coreEquipmentDonationObserversOperations.Value.IsValidActualEndedEquipment(f))
            throw new InvalidOperationException("Equipment options lost their issued actual-turn draw/placement ledger.");
        var r = f.ActualEndedEquipment!;
        if (r.Stage == ActualEndedTurnEquipmentStage.Complete)
            return SkillProgramStepOutcome.Continue;
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreEquipmentDonationProgramsOperations.EquipmentDonationSourceValid(f) || !_coreMatchParticipantsPlayersAccess._players[r.Qualification.TurnOwnerSeat].IsAlive || (r.EquipmentIssued ? 1 : 0) + (r.DrawIssued ? 1 : 0) >= r.Qualification.MaximumDistinctOptions)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { ActualEndedEquipment = r with { Stage = ActualEndedTurnEquipmentStage.Complete } });
            return SkillProgramStepOutcome.Continue;
        }

        var choices = ActualEndedEquipmentChoices(f);
        if (choices.Count == 0)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { ActualEndedEquipment = r with { Stage = ActualEndedTurnEquipmentStage.Complete } });
            return SkillProgramStepOutcome.Continue;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ActualEndedEquipment = r with { Stage = ActualEndedTurnEquipmentStage.Offering }, ReexecuteParticipantInstruction = true });
        _coreEquipmentDonationProgramsOperations.PublishEquipmentDonationPrompt(f, f.OwnerSeat, $"原实际回合可选择至多 {(r.Qualification.MaximumDistinctOptions)} 个不同选项。", choices);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> ActualEndedEquipmentChoices(ProgramSkillFrame f)
    {
        var r = f.ActualEndedEquipment!;
        var choices = new List<PromptChoice>();
        if (!r.DrawIssued)
            choices.Add(new(new($"actual-ended-equipment.{(f.Id)}.draw"), "摸一张牌。", [], [], _coreEquipmentDonationProgramsOperations.EquipmentDonationParameters(f, "actual-ended-equipment", "draw")));
        if (!r.EquipmentIssued)
            foreach (var card in _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[r.Qualification.TurnOwnerSeat]).Where(c => !c.IsGeneralWeapon && _coreEquipmentPlacementAndHpPairOperations.CanPlaceOwnedEquipment(r.Qualification.TurnOwnerSeat, f.OwnerSeat, c, CardLocation.Equipment(r.Qualification.TurnOwnerSeat))).OrderBy(c => c.Id))
                choices.Add(new(new($"actual-ended-equipment.{(f.Id)}.equipment.{(card.Id)}"), $"将【{(card.DisplayName)}】置入自己的装备区。", [card.Id], [f.OwnerSeat], _coreEquipmentDonationProgramsOperations.EquipmentDonationParameters(f, "actual-ended-equipment", "equipment")));
        return choices;
    }

    internal void ResolveActualEndedEquipmentChoice(ProgramSkillFrame f, PromptChoice choice)
    {
        if (!_coreEquipmentDonationObserversOperations.Value.IsValidActualEndedEquipment(f) || f.ActualEndedEquipment is not { } r || r.Stage != ActualEndedTurnEquipmentStage.Offering || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat)
            throw new InvalidOperationException("An ended-turn equipment choice lost its exact original turn.");
        choice = ActualEndedEquipmentChoices(f).Single(c => c.Id == choice.Id);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!_coreEquipmentDonationProgramsOperations.EquipmentDonationSourceValid(f) || _coreMatchOutcomeWinnerAccess._winner != Winner.None)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { ActualEndedEquipment = r with { Stage = ActualEndedTurnEquipmentStage.Complete }, ReexecuteParticipantInstruction = true });
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return;
        }

        var before = _coreEquipmentDonationProgramsOperations.EquipmentDonationSequence;
        int? id = null, replacedId = null;
        var replacedGeneral = false;
        var draw = 0;
        var option = choice.Parameters["option"];
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        if (option == "draw")
        {
            if (r.DrawIssued || choice.Cards.Count != 0 || choice.Targets.Count != 0)
                throw new InvalidOperationException("A draw option cannot repeat or select equipment.");
            _coreProgramLifecycleOperations.DrawProgramCards(f.Id, f.OwnerSeat, 1, null, null, SkillProgramCardSetVisibility.Private, new(ActualEndedEquipmentDrawReason));
            draw = _coreCardMovementJournalCardMovementsAccess._cardMovements.Count(m => m.Sequence > before && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == ActualEndedEquipmentDrawReason);
            r = r with
            {
                DrawIssued = true
            };
        }
        else
        {
            if (option != "equipment" || r.EquipmentIssued || choice.Cards is not [var cardId] || !choice.Targets.SequenceEqual([f.OwnerSeat]))
                throw new InvalidOperationException("An equipment option requires one original turn owner's current actual equipment.");
            var from = CardLocation.Equipment(r.Qualification.TurnOwnerSeat);
            var card = _cardZonesOperations._cardZones.CardsAt(from).Single(c => c.Id == cardId);
            if (card.IsGeneralWeapon || !_coreEquipmentPlacementAndHpPairOperations.CanPlaceOwnedEquipment(r.Qualification.TurnOwnerSeat, f.OwnerSeat, card, from))
                throw new InvalidOperationException("The current equipment cannot survive actual placement.");
            var slot = _coreFieldEquipmentOperations.FieldEquipmentSlot(card);
            var equipped = _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Where(c => _coreFieldEquipmentOperations.FieldEquipmentSlot(c) == slot).ToArray();
            var replaced = equipped.Length >= _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].EquipmentSlotCapacity(slot) ? equipped[0] : null;
            replacedId = replaced?.Id;
            replacedGeneral = replaced?.IsGeneralWeapon == true;
            if (replaced is not null)
            {
                _coreAdvancedWeaponsOperations.CopyFirstReplacedWeapon(card, replaced);
                _corePhysicalMovementFacadesOperations.MoveCard(replaced, CardLocation.Equipment(f.OwnerSeat), CardLocation.DiscardPile, CardMoveReasons.EquipmentReplace);
            }

            _corePhysicalMovementFacadesOperations.MoveCard(card, from, CardLocation.Equipment(f.OwnerSeat), new(ActualEndedEquipmentMoveReason));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new EquipmentChangedEvent(f.Id, f.OwnerSeat, slot, card.Id, card.Kind, replaced?.Id));
            id = card.Id;
            r = r with
            {
                EquipmentIssued = true
            };
        }

        var after = _coreEquipmentDonationProgramsOperations.EquipmentDonationSequence;
        r = r with
        {
            Stage = ActualEndedTurnEquipmentStage.Moving,
            LastPayment = new(option, id, before, after, draw, replacedId, replacedGeneral)
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { ActualEndedEquipment = r, ReexecuteParticipantInstruction = true });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ActualEndedTurnEquipmentOptionIssuedEvent(f.Id, r.Source, r.GameplayHash, r.Qualification.TurnNumber, r.Qualification.TurnOwnerSeat, r.Qualification.MaximumDistinctOptions, option, id, before, after, draw, replacedId, replacedGeneral));
        if (_coreEquipmentDonationProgramsOperations.AwaitEquipmentDonationMovements(f.Id) == SkillProgramStepOutcome.Continue)
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ChooseEquipmentOrDrawAfterOtherActualTurn)]
    internal PromptChoice SelectAiActualEndedEquipment(PendingDecision decision, ProgramSkillFrame f) => decision.Choices.OrderByDescending(c => c.Parameters["option"] == "equipment" ? CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single()), _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]) : 5d).ThenBy(c => c.Cards.FirstOrDefault()).First();
}
