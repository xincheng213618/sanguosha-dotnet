#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaiFuRenOlRuntime
{
    internal const string OrdinaryQietingMoveReason = "skill-program.cai-fu-ren.qieting.move";
    internal const string OrdinaryQietingDrawReason = "skill-program.cai-fu-ren.qieting.draw";
    internal bool CanMoveOrdinaryQietingEquipment(ProgramSkillFrame f, Card card)
    {
        var peer = f.OrdinaryQieting!.Qualification.TurnOwnerSeat;
        return !card.IsGeneralWeapon && _coreEquipmentPlacementAndHpPairOperations.CanPlaceOwnedEquipment(peer, f.OwnerSeat, card, CardLocation.Equipment(peer)) && (CharacterCaiFuRenBoundaryMobileRuntime.IsBoundaryMobileQieting(f) || _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).All(c => _coreEquipmentContentOperations.EquipmentRules.Get(c.Kind).Slot != _coreEquipmentContentOperations.EquipmentRules.Get(card.Kind).Slot));
    }

    internal SkillProgramStepOutcome BeginOrdinaryQieting(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.OrdinaryQieting is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || !CharacterCaiFuRenBoundaryMobileRuntime.IsKnownCaiFuRenQietingBinding(f.SkillId, f.TriggerId) || f.WindowContext is not { OrdinaryQieting: { } q } c || _resolutionStackOperations._resolutionStack.OfType<DeferredTurnEndFrame>().SingleOrDefault(p => p.Id == c.ParentFrameId)is not { } parent || !IsEligibleOrdinaryCaiFuRenQietingCandidate(parent, new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, c.OccurrenceIndex), c))
            throw new InvalidOperationException("Ordinary Qieting requires its exact unused-on-others actual turn and original binding.");
        var r = new OrdinaryQietingReceipt(OrdinaryCaiFuRenSource(f), f.GameplayHash, q, parent.Id, c.OccurrenceIndex, OrdinaryQietingStage.Choice);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { OrdinaryQieting = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OrdinaryQietingStartedEvent(f.Id, r.Source, r.GameplayHash, q, parent.Id, c.OccurrenceIndex));
        PublishOrdinaryQieting(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> OrdinaryQietingChoices(ProgramSkillFrame f)
    {
        if (CharacterCaiFuRenBoundaryMobileRuntime.IsBoundaryMobileQieting(f) && f.OrdinaryQieting?.Stage == OrdinaryQietingStage.Viewing)
            return _characterCaiFuRenBoundaryMobileOperations.BoundaryMobileQietingHandChoices(f);
        if (f.OrdinaryQieting is not { Stage: OrdinaryQietingStage.Choice, Paid: false } r || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return[];
        PromptChoice Choice(string option, string label, IReadOnlyList<int> cards) => new(new($"ordinary-qieting.{(f.Id)}.{(option)}"), label, cards, [f.OwnerSeat], new Dictionary<string, string> { ["program-action"] = "ordinary-qieting", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["option"] = option });
        var choices = _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[r.Qualification.TurnOwnerSeat]).Where(c => CanMoveOrdinaryQietingEquipment(f, c)).OrderBy(c => c.Id).Select(c => Choice($"equipment:{(c.Id)}", CharacterCaiFuRenBoundaryMobileRuntime.IsBoundaryMobileQieting(f) ? $"将【{(c.DisplayName)}】置入你的装备区" : $"将【{(c.DisplayName)}】置入你的空装备栏", [c.Id])).ToList();
        if (CharacterCaiFuRenBoundaryMobileRuntime.IsBoundaryMobileQieting(f) && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.Qualification.TurnOwnerSeat]).Count > 0)
            choices.Add(Choice("hand", "观看其两张手牌，获得其中一张", []));
        choices.Add(Choice("draw", "摸一张牌", []));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishOrdinaryQieting(ProgramSkillFrame f)
    {
        var choices = OrdinaryQietingChoices(f);
        if (choices.Count == 0)
        {
            FinishOrdinaryQieting(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, CharacterCaiFuRenBoundaryMobileRuntime.IsBoundaryMobileQieting(f) ? "窃听：观看两张手牌取一、置入装备，或摸一张牌。" : "窃听：置入其一张空槽装备，或摸一张牌。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [f.OwnerSeat], SourceSeat: f.OrdinaryQieting!.Qualification.TurnOwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = f.OwnerSeat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("ordinary-qieting")]
    internal void AnswerOrdinaryQieting(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame mobile && CharacterCaiFuRenBoundaryMobileRuntime.IsBoundaryMobileQieting(mobile))
        {
            _characterCaiFuRenBoundaryMobileOperations.AnswerBoundaryMobileQieting(mobile, selected);
            return;
        }

        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { OrdinaryQieting: { Stage: OrdinaryQietingStage.Choice, Paid: false } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !OrdinaryQietingChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [selected])))
            throw new InvalidOperationException("Ordinary Qieting requires its current original single option.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var option = selected.Parameters["option"];
        Card? card = option == "draw" ? null : _coreAdvancedLifecycleOperations.GetAdvancedCard(selected.Cards.Single());
        if (card is not null && !CanMoveOrdinaryQietingEquipment(f, card))
            throw new InvalidOperationException("Ordinary Qieting cannot replace equipment or substitute its source entity.");
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect, OrdinaryQieting = r = r with { Paid = true, Option = option == "draw" ? "draw" : "equipment", CardId = card?.Id, PrintedKind = card?.Kind, Stage = card is null ? OrdinaryQietingStage.Draw : OrdinaryQietingStage.MovementChildren, Before = before, After = before, GrainIds = card?.Kind == CardKind.WoodenOx ? _corePhysicalOwnedCardQueriesOperations.GetWoodenOxGrain(_coreMatchParticipantsPlayersAccess._players[r.Qualification.TurnOwnerSeat]).Select(c => c.Id).ToArray() : [] }, PendingMovementContinuation = card is null ? null : new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OrdinaryQietingOptionIssuedEvent(f.Id, r.Option!, card?.Id, card?.Kind, before));
        if (card is not null)
        {
            var from = CardLocation.Equipment(r.Qualification.TurnOwnerSeat);
            _corePhysicalMovementFacadesOperations.MoveCard(card, from, CardLocation.Equipment(f.OwnerSeat), new(OrdinaryQietingMoveReason), movement =>
            {
                var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                _coreRuntimeOperations.ReplaceRuntimeTop(current with { OrdinaryQieting = current.OrdinaryQieting!with { After = movement.Sequence } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OrdinaryQietingEquipmentMovedEvent(f.Id, movement.CardId, movement.CardKind, from, movement.To, movement.Sequence));
            });
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = current with { OrdinaryQieting = current.OrdinaryQieting!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new EquipmentChangedEvent(f.Id, f.OwnerSeat, _coreEquipmentContentOperations.EquipmentRules.Get(card.Kind).Slot, card.Id, card.Kind, null));
            _coreMatchLogOperations.AddLog("EquipmentChanged", $"{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)} 发动窃听，将【{(card.DisplayName)}】置入自己的装备区。", f.OwnerSeat);
        }

        ContinueOrdinaryQieting(f.Id);
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveOrdinaryCaiFuRenQieting, SkillProgramEffectOp.ResolveBoundaryMobileQieting)]
    internal void ReturnOrdinaryQietingDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        if (f.OrdinaryQieting is not { Paid: true, Stage: OrdinaryQietingStage.Draw, Option: "draw" } r || outcome.RecipientSeat != f.OwnerSeat || outcome.RequestedCount != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<OrdinaryQietingDrawReturnedEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Ordinary Qieting lost its one native draw and typed return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { OrdinaryQieting = r with { Stage = OrdinaryQietingStage.DrawChildren, DrawOutcome = outcome, Before = outcome.SequenceBefore, After = outcome.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OrdinaryQietingDrawReturnedEvent(f.Id, outcome.ActualDrawIds.Count, outcome.ReplacementGainedIds.Count, outcome.SequenceBefore, outcome.SequenceAfter, outcome.AttemptFrameId));
        ContinueOrdinaryQieting(f.Id);
    }

    internal bool DrainOrdinaryQieting(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    internal void ContinueOrdinaryQieting(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { OrdinaryQieting: { } r } f || f.Id != id)
            return;
        if (r.Stage is OrdinaryQietingStage.Choice or OrdinaryQietingStage.Viewing)
        {
            if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            {
                FinishOrdinaryQieting(f);
                return;
            }

            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishOrdinaryQieting(f);
            return;
        }

        if (r.Stage == OrdinaryQietingStage.Draw)
        {
            if (f.NativeDraw is not null)
                return;
            if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                FinishOrdinaryQieting(f);
                return;
            }

            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, 1, new(OrdinaryQietingDrawReason));
            return;
        }

        if (r.Stage is OrdinaryQietingStage.MovementChildren or OrdinaryQietingStage.DrawChildren)
        {
            if (DrainOrdinaryQieting(f))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
            FinishOrdinaryQieting(f);
            return;
        }

        throw new InvalidOperationException("Ordinary Qieting returned outside its issued option.");
    }

    internal void FinishOrdinaryQieting(ProgramSkillFrame f)
    {
        var paid = f.OrdinaryQieting?.Paid == true;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OrdinaryQietingCompletedEvent(f.Id, paid));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { OrdinaryQieting = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, paid);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27596)]
    internal bool ResumeOrdinaryQieting(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { OrdinaryQieting: not null } f || f.Id != id)
            return false;
        ContinueOrdinaryQieting(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 27596)]
    internal bool ReturnOrdinaryQietingMovement(ProgramSkillFrame f)
    {
        if (f.OrdinaryQieting is null)
            return false;
        ContinueOrdinaryQieting(f.Id);
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveOrdinaryCaiFuRenQieting)]
    internal PromptChoice ChooseOrdinaryQietingAi(PendingDecision decision, ProgramSkillFrame f) => decision.Choices.OrderByDescending(c => c.Cards.Count == 1 ? CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards[0]), _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]) : 5d).ThenBy(c => c.Cards.FirstOrDefault()).First();
}
