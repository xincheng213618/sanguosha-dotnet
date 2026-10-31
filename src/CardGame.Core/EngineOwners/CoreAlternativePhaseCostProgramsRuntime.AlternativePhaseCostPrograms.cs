#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreAlternativePhaseCostProgramsRuntime
{
    SkillProgramStepOutcome IAlternativePhaseCostProgramHost.PayOwnedCardOrMarker(ProgramSkillFrame frame, IReadOnlyList<CardZoneKind> zones, PlayerMarkerKind marker) => BeginAlternativePhaseCost(frame, zones, marker);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.AfterCondition, 20)]
    internal bool CanOfferAlternativePhaseCost(CharacterState owner, SkillProgramTrigger trigger, ProgramTriggerCandidate candidate)
    {
        var effect = trigger.Effects.FirstOrDefault(e => e.Op == SkillProgramEffectOp.PayOwnedCardOrMarker);
        if (effect is null)
            return true;
        if (owner.Markers.GetValueOrDefault(effect.Marker!.Value) >= 1)
            return true;
        return effect.Zones.Any(zone => _cardZonesOperations._cardZones.CardsAt(new CardLocation(zone, owner.Seat)).Any(card => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(owner.Seat, card, new(zone, owner.Seat), OwnedCardMoveIntent.Discard) && !(zone == CardZoneKind.Equipment && _coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(owner.Seat, candidate.SkillId, candidate.SkillInstanceId, card))));
    }

    internal IReadOnlyList<PromptChoice> BuildAlternativePhaseCostChoices(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var choices = _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(frame.Id, frame.OwnerSeat, frame.OwnerSeat, effect.Zones, OwnedCardMoveIntent.Discard).Select(choice => choice with { Parameters = new System.Collections.ObjectModel.ReadOnlyDictionary<string, string>(choice.Parameters.ToDictionary(pair => pair.Key, pair => pair.Key == "program-action" ? "alternative-phase-cost-card" : pair.Value)) }).ToList();
        if (_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].Markers.GetValueOrDefault(effect.Marker!.Value) >= 1)
            choices.Add(new(new($"program-phase-cost.frame-{(frame.Id)}.marker"), $"移除1枚“{(PlayerMarkerCatalog.GetDisplayName(effect.Marker.Value))}”", [], [], new System.Collections.ObjectModel.ReadOnlyDictionary<string, string>(new Dictionary<string, string> { ["program-action"] = "alternative-phase-cost-marker", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) })));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal SkillProgramStepOutcome BeginAlternativePhaseCost(ProgramSkillFrame frame, IReadOnlyList<CardZoneKind> zones, PlayerMarkerKind marker)
    {
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (frame.AlternativePhaseCost is not null || frame.WindowContext is not { } context || context.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || frame.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not ProgramLifecycleTriggerWindowFrame parent || parent.Id != context.ParentFrameId || parent.Window != context.Window)
            throw new InvalidOperationException("An alternative phase cost requires its unpaid exact lifecycle owner.");
        var effect = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
        if (effect.Op != SkillProgramEffectOp.PayOwnedCardOrMarker || effect.Marker != marker || !effect.Zones.SequenceEqual(zones))
            throw new InvalidOperationException("The alternative phase cost does not match its instruction.");
        var choices = BuildAlternativePhaseCostChoices(frame, effect);
        if (choices.Count == 0)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "没有可弃置的牌或可移除的标记，阶段照常继续。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, frame.OwnerSeat, $"【{(skill.Name)}】弃置一张自有牌，或移除1枚“{(PlayerMarkerCatalog.GetDisplayName(marker))}”。", Array.AsReadOnly(choices.SelectMany(c => c.Cards).Distinct().Order().ToArray()), [], frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = frame.OwnerSeat,
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name + " · 选择费用", skill.Description),
            Choices = choices
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal bool IsAlternativePhaseCostChoiceLegal(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame frame || frame.AlternativePhaseCost is not null || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId))
            return false;
        var effect = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
        return effect.Op == SkillProgramEffectOp.PayOwnedCardOrMarker && BuildAlternativePhaseCostChoices(frame, effect).Any(current => current.Id == choice.Id && current.Cards.SequenceEqual(choice.Cards) && current.Targets.SequenceEqual(choice.Targets) && current.Parameters.Count == choice.Parameters.Count && current.Parameters.All(pair => choice.Parameters.GetValueOrDefault(pair.Key) == pair.Value));
    }

    [ProgramAnswer("alternative-phase-cost-card", "alternative-phase-cost-marker")]
    internal void ResolveAlternativePhaseCost(PromptChoice choice)
    {
        if (!IsAlternativePhaseCostChoiceLegal(choice))
            throw new InvalidOperationException("The published alternative cost is no longer payable.");
        var frame = (ProgramSkillFrame)_resolutionStackOperations._resolutionStack[^1];
        var effect = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
        var marker = effect.Marker!.Value;
        var markerPayment = choice.Parameters["program-action"] == "alternative-phase-cost-marker";
        Card? card = null;
        CardLocation? from = null;
        if (!markerPayment)
        {
            var zone = Enum.Parse<CardZoneKind>(choice.Parameters["source-zone"]);
            from = new(zone, frame.OwnerSeat);
            card = _cardZonesOperations._cardZones.CardsAt(from.Value).Single(c => c.Id == choice.Cards.Single());
        }

        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var receipt = new ProgramAlternativePhaseCostReceipt(frame.InstructionIndex, markerPayment ? ProgramAlternativePhaseCostKind.RemoveMarker : ProgramAlternativePhaseCostKind.DiscardCard, marker, card?.Id, from);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { AlternativePhaseCost = receipt });
        if (markerPayment)
            _coreProgramMarkerPaymentsOperations.PayProgramMarkerCost(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], new(marker, 1), frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame));
        else
            _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(frame.OwnerSeat, frame.OwnerSeat, card!, from!.Value, new($"skill-program.{(frame.SkillId)}.{(effect.Op)}"));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramAlternativePhaseCostPaidEvent(frame.Id, frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.SkillInstanceId, frame.OwnerSeat, receipt.Kind, marker, card?.Id, from));
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 720)]
    internal bool ResumeAlternativePhaseCost(long frameId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame frame || frame.Id != frameId || frame.AlternativePhaseCost is null)
            return false;
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(frame.Id, PostEventContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frame.Id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(frame.Id))
            return true;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { AlternativePhaseCost = null });
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), "已付费用的回复和移牌结清后，剩余阶段替代结算取消。");
            return true;
        }

        return false;
    }

    [ProgramAiChoice(SkillProgramEffectOp.PayOwnedCardOrMarker)]
    internal PromptChoice SelectAiAlternativePhaseCost(PendingDecision decision, ProgramSkillFrame frame)
    {
        // The owner may inspect its own materials; another player's private hand is never read.
        var marker = decision.Choices.SingleOrDefault(c => c.Parameters.GetValueOrDefault("program-action") == "alternative-phase-cost-marker");
        if (marker is not null)
            return marker;
        return decision.Choices.OrderBy(choice => CoreNativeAiDecisionsRuntime.GetKeepValue(_cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(choice.Cards.Single())).Single(card => card.Id == choice.Cards[0]), _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat])).ThenBy(choice => choice.Id.Value, StringComparer.Ordinal).First();
    }

    internal void RecordAlternativePhaseSkip(ProgramSkillFrame frame, IReadOnlyList<SkillProgramTurnPhase> phases)
    {
        var plan = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!);
        var paidSubstitution = plan.Instructions.Any(effect => effect.Op == SkillProgramEffectOp.PayOwnedCardOrMarker);
        // A choice made after a public reveal can skip the current Draw without
        // a separate card/marker cost. It still owes the original lifecycle
        // parent an actual substitution receipt; a future-phase flag is too late.
        var currentDraw = frame.WindowContext?.Window == SkillProgramTriggerWindow.DrawPhaseStarting && phases.SequenceEqual([SkillProgramTurnPhase.Draw]);
        if (!paidSubstitution && !currentDraw)
            return;
        if (frame.WindowContext is not { } context || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not ProgramLifecycleTriggerWindowFrame parent || parent.Id != context.ParentFrameId || parent.Window != context.Window || phases.Count != 1)
            throw new InvalidOperationException("An actual phase skip lost its starting parent.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { ActualPhaseSubstitution = new(parent.Id, frame.Id, frame.OwnerSeat, frame.SkillId, frame.SkillInstanceId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, phases[0]) });
        foreach (var phase in phases)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramTurnPhaseSubstitutedEvent(frame.Id, frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.SkillInstanceId, frame.OwnerSeat, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, frame.WindowContext!.Window, phase));
    }

    internal void CompleteActualJudgmentStarting(ProgramLifecycleTriggerWindowFrame frame)
    {
        var current = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        if (frame.ActualPhaseSubstitution is { SkippedPhase: SkillProgramTurnPhase.Judgment } receipt && receipt.ParentFrameId == frame.Id)
            _coreActualTurnsOperations.CompleteTurnStart(current, _coreDelayedTurnEffectsPendingTurnDelayedEffectsAccess._pendingTurnDelayedEffects, skippedActualJudgment: true);
        else
            _coreActualTurnsOperations.BeginDelayedJudgmentOrTurnStart(current);
    }

    internal bool IsConfirmedActualStartingSubstitution(ProgramSkillWindowContext context) => _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().Any(frame => frame.Id == context.ParentFrameId && frame.Window == context.Window && frame.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && frame.ActualPhaseSubstitution is { } receipt && receipt.ParentFrameId == frame.Id && receipt.OwnerSeat == frame.OwnerSeat && receipt.ActualTurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && (frame.Window == SkillProgramTriggerWindow.JudgmentPhaseStarting && receipt.SkippedPhase == SkillProgramTurnPhase.Judgment || frame.Window == SkillProgramTriggerWindow.DrawPhaseStarting && receipt.SkippedPhase == SkillProgramTurnPhase.Draw));
    [ProgramAnswer("decline-owned-field-move")]
    internal void ResolveDeclinedOwnedFieldMove(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame frame || choice.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) || choice.Cards.Count != 0 || choice.Targets.Count != 0)
            throw new InvalidOperationException("A declined field move lost its owning choice.");
        var effect = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
        if (effect.Op != SkillProgramEffectOp.SelectAndMoveOwnedCard || !effect.AllowDecline || !effect.AwaitMovementTriggers || effect.Destination != SkillProgramCardDestination.SelectedTargetCorrespondingZone)
            throw new InvalidOperationException("Only an explicitly optional public field move may be declined.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }
}
