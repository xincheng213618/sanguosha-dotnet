#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWangJiBoundaryMobileRuntime
{
    internal const string BoundaryWjQizhi = "boundary:qizhi-mobile", BoundaryWjJinqu = "boundary:jinqu-mobile", BoundaryWjAction = "boundary-wang-ji";
    internal const string BoundaryWjDiscardReason = "skill-program.boundary:qizhi-mobile.discard";
    internal static CardConversionSource BoundaryWjSource(ProgramSkillFrame frame) => new(frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId);
    internal int BoundaryWjCharge(int owner) => _coreMatchParticipantsPlayersAccess._players[owner].Markers.GetValueOrDefault(PlayerMarkerKind.BoundaryWangJiCharge);
    internal bool BoundaryWjCard(CardActionContext action) => _coreFriendTrioOperations.IsFriendActualCardUse(action) && (CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(action.EffectiveKind) == SkillProgramCardCategory.Basic || CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(action.EffectiveKind));
    internal BoundaryWangJiTurnEvent? BoundaryWjTurn() => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<BoundaryWangJiTurnEvent>().LastOrDefault(entry => entry.TurnNumber == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber && entry.OwnerSeat == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat);
    internal int BoundaryWjPeak(int owner)
    {
        var turn = BoundaryWjTurn();
        return turn is null || turn.OwnerSeat != owner ? 0 : _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().SkipWhile(entry => !Equals(entry, turn)).OfType<PlayerMarkerChangedEvent>().Where(entry => entry.PlayerSeat == owner && entry.Marker == PlayerMarkerKind.BoundaryWangJiCharge).Select(entry => entry.Count).Append(turn.InitialCharge).Max();
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 54400)]
    internal void ObserveBoundaryWjTurn(TurnStartedEvent entry)
    {
        if (_contentRegistryOperations._contentRegistry.Skills.ContainsKey(BoundaryWjJinqu) && _coreNationalSkillsOperations.HasRuntimeSkill(_coreMatchParticipantsPlayersAccess._players[entry.ActorSeat], BoundaryWjJinqu))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryWangJiTurnEvent(entry.TurnNumber, entry.ActorSeat, _coreProgramEventHistoryOperations.Value.ProgramEventHistory<TurnStartedEvent>().Count() + 1, BoundaryWjCharge(entry.ActorSeat)));
    }

    internal IReadOnlyList<int> BoundaryWjRecipients(int owner, ActualDiscardRecoveryPhaseKey phase) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<BoundaryWangJiRecipientEvent>().Where(entry => entry.Source.OwnerSeat == owner && entry.Source.SkillId == BoundaryWjQizhi && entry.Phase == phase).Select(entry => entry.TargetSeat).Distinct().OrderBy(seat => _coreTableOrderOperations.RelativeTableOrder(owner, seat)).ToArray();
    internal bool CanSpendBoundaryWj(int owner, int target) => BoundaryWjCharge(owner) > 0 && _coreMatchParticipantsPlayersAccess._players[owner].IsAlive && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } phase && phase.ActorSeat == owner && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<BoundaryWangJiSpentEvent>().Any(entry => entry.Source.OwnerSeat == owner && entry.Phase == phase && entry.TargetSeat == target);
    SkillProgramStepOutcome IBoundaryWangJiHost.Execute(ProgramSkillFrame frame, SkillProgramEffectOp operation) => BeginBoundaryWj(frame, operation);
    bool IBoundaryWangJiSelection.CanSpend(int owner, int target) => CanSpendBoundaryWj(owner, target);
    [ProgramTargetWindow(ProgramTargetWindowStage.FinalizedInstantTrickRequirement, 54400)]
    [ProgramTargetWindow(ProgramTargetWindowStage.ImplicitSelfDrawTwo, 54400)]
    [ProgramTargetWindow(ProgramTargetWindowStage.NormalizeImplicitDrawTwo, 54400)]
    internal bool BoundaryWjTargetWindow(ProgramTargetCardWindowQuery query) => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveBoundaryWangJiQizhi);
    internal bool BoundaryWjCompletedWindow(ProgramSkillWindowContext context, out ProgramCardTriggerWindowFrame window)
    {
        window = null !;
        if (context.Window != SkillProgramTriggerWindow.CardUseCompleted || context.CardUse is not { } identity || _resolutionStackOperations._resolutionStack.SingleOrDefault(frame => frame.Id == context.ParentFrameId)is not ProgramCardTriggerWindowFrame current || CoreGameEngineCardActionsRuntime.GetCardActionWindow(current) != context.Window || current.Action.ActionId != identity.CardActionId || !BoundaryWjCard(current.Action))
            return false;
        if (current.Action.Type == CardActionType.Use)
        {
            if (_coreCardUseLifecycleOperations.LifecycleCardUse(current.ParentFrameId)is not { Action: { } actual, Step: ResolutionFrameStep.Completed } || actual.ActionId != current.Action.ActionId)
                return false;
        }
        else if (current.CompletedResponseReturn is not { IsCommitted: false } typed || !_coreResponseUseCompletionOperations.IsCompletedResponseUse2(current.Action, typed.Kind) || !_coreResponseUseCompletionOperations.IsCompletedResponseParentConsistent(current))
            return false;
        window = current;
        return true;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.ObservesResponseUseCompletion, 54400)]
    internal bool BoundaryWjObservesResponse(CardActionContext action) => BoundaryWjCard(action) && action.ActorSeat == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && _coreNationalSkillsOperations.HasRuntimeSkill(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat], BoundaryWjJinqu);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 54400)]
    internal bool OfferBoundaryWj(ProgramTriggerLegalityContext gate)
    {
        if (gate.SkillId == BoundaryWjQizhi && gate.Trigger.Id == "inspect-recipients")
            return _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } phase && phase.ActorSeat == gate.OwnerSeat && gate.Context.CardUse is { ActorSeat: var actor } && actor == gate.OwnerSeat && BoundaryWjRecipients(gate.OwnerSeat, phase).Any(seat => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Count > 0);
        if (gate.SkillId != BoundaryWjJinqu)
            return true;
        return gate.Trigger.Id == "ending-peak" ? BoundaryWjPeak(gate.OwnerSeat) > 0 : BoundaryWjTurn()is { } turn && turn.OwnerSeat == gate.OwnerSeat && BoundaryWjCompletedWindow(gate.Context, out var window) && window.Action.ActorSeat == gate.OwnerSeat && (!_coreProgramEventHistoryOperations.Value.ProgramEventHistory<BoundaryWangJiUseEvent>().Any(entry => entry.OwnerSeat == gate.OwnerSeat && entry.ActionId == window.Action.ActionId) || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(frame => frame.BoundaryWangJi?.WindowId == window.Id && frame.OwnerSeat == gate.OwnerSeat));
    }

    internal SkillProgramStepOutcome BeginBoundaryWj(ProgramSkillFrame supplied, SkillProgramEffectOp operation)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (operation == SkillProgramEffectOp.InitializeBoundaryWangJi)
        {
            if (frame.SkillId != BoundaryWjQizhi || frame.TriggerId != "initial-charge" || frame.WindowContext?.Window != SkillProgramTriggerWindow.GameStarting)
                throw new InvalidOperationException("Mobile Qizhi initial charge requires its original game-start binding.");
            if (!_coreProgramEventHistoryOperations.Value.ProgramEventHistory<BoundaryWangJiInitializedEvent>().Any(entry => entry.OwnerSeat == frame.OwnerSeat))
            {
                BoundaryWjChangeMarkerCharge(frame.Id, frame.OwnerSeat, PlayerMarkerKind.BoundaryWangJiCharge, 1, 2, BoundaryWjQizhi);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryWangJiInitializedEvent(frame.Id, frame.OwnerSeat));
            }

            return SkillProgramStepOutcome.Continue;
        }

        if (operation == SkillProgramEffectOp.SpendBoundaryWangJiQizhi)
        {
            if (frame.SkillId != BoundaryWjQizhi || frame.TriggerId is not null || frame.ActivationId != "charge-draw" || frame.SelectedTargetSeats is not [var recipient] || !CanSpendBoundaryWj(frame.OwnerSeat, recipient))
                throw new InvalidOperationException("Qizhi requires one fresh phase recipient and one actual charge.");
            var phase = _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()!;
            var draw = new BoundaryWangJiReceipt(BoundaryWjSource(frame), frame.GameplayHash, operation, 0, 0, phase)
            {
                Stage = BoundaryWangJiStage.Drawing,
                Targets = [recipient],
                Requested = 2,
                Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
                After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundaryWangJi = draw });
            BoundaryWjChangeMarkerCharge(frame.Id, frame.OwnerSeat, PlayerMarkerKind.BoundaryWangJiCharge, -1, 2, BoundaryWjQizhi);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryWangJiSpentEvent(frame.Id, draw.Source, frame.GameplayHash, phase, recipient));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryWangJiStartedEvent(frame.Id, draw.Source, frame.GameplayHash, operation, 0, 0, phase));
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(frame, recipient, 2, new("skill-program.boundary:qizhi-mobile.Draw"));
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (frame.BoundaryWangJi is not null || frame.InstructionIndex != 1 || frame.WindowContext is not { } context)
            throw new InvalidOperationException("Wang Ji requires the original native instruction and owning frame.");
        var receipt = new BoundaryWangJiReceipt(BoundaryWjSource(frame), frame.GameplayHash, operation, context.ParentFrameId, context.CardUse?.CardActionId ?? 0, _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase())
        {
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        if (operation == SkillProgramEffectOp.ResolveBoundaryWangJiQizhi)
        {
            if (frame.SkillId != BoundaryWjQizhi || context.Window != SkillProgramTriggerWindow.CardUseTargetsFinalized || receipt.Phase is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase || phase.ActorSeat != frame.OwnerSeat || context.CardUse is not { } identity || _coreCardUseLifecycleOperations.LifecycleCardUse(identity.ParentCardUseFrameId)is not { Action: { Type: CardActionType.Use } action } || action.ActionId != identity.CardActionId || action.ActorSeat != frame.OwnerSeat || !BoundaryWjCard(action))
                throw new InvalidOperationException("Qizhi inspection needs the actual basic/instant-trick target designation in this Play phase.");
            receipt = receipt with
            {
                Stage = BoundaryWangJiStage.Inspect,
                Targets = BoundaryWjRecipients(frame.OwnerSeat, phase)
            };
        }
        else if (context.Window == SkillProgramTriggerWindow.CardUseCompleted)
        {
            if (frame.SkillId != BoundaryWjJinqu || BoundaryWjTurn()is not { } turn || turn.OwnerSeat != frame.OwnerSeat || !BoundaryWjCompletedWindow(context, out var window) || window.Action.ActorSeat != frame.OwnerSeat)
                throw new InvalidOperationException("Jinqu counts only its owner's completed true uses in the owner's actual turn.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryWangJiUseEvent(frame.Id, frame.OwnerSeat, turn.Ordinal, window.Action.ActionId));
            if (_coreProgramEventHistoryOperations.Value.ProgramEventHistory<BoundaryWangJiUseEvent>().Count(entry => entry.OwnerSeat == frame.OwnerSeat && entry.TurnOrdinal == turn.Ordinal) % 2 != 0)
                return SkillProgramStepOutcome.Continue;
            receipt = receipt with
            {
                Stage = BoundaryWangJiStage.Charge
            };
        }
        else
        {
            if (frame.SkillId != BoundaryWjJinqu || frame.TriggerId != "ending-peak" || context.Window != SkillProgramTriggerWindow.TurnEnding)
                throw new InvalidOperationException("Jinqu's highest-charge draw needs its own actual ending.");
            receipt = receipt with
            {
                Stage = BoundaryWangJiStage.Drawing,
                Requested = BoundaryWjPeak(frame.OwnerSeat)
            };
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundaryWangJi = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryWangJiStartedEvent(frame.Id, receipt.Source, receipt.GameplayHash, operation, receipt.WindowId, receipt.ActionId, receipt.Phase));
        if (receipt.Stage == BoundaryWangJiStage.Drawing)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryWangJiPeakFrozenEvent(frame.Id, frame.OwnerSeat, BoundaryWjTurn()!.Ordinal, receipt.Requested));
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(frame, frame.OwnerSeat, receipt.Requested, new("skill-program.boundary:jinqu-mobile.Draw"));
            return SkillProgramStepOutcome.AwaitChild;
        }

        ContinueBoundaryWj(frame.Id);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal PromptChoice BoundaryWjChoice(ProgramSkillFrame frame, string branch, int value, string text, bool card = false) => new(new($"boundary-wang-ji.{(frame.Id)}.{(branch)}.{(value)}"), text, card ? [value] : [], card ? [] : [value], new Dictionary<string, string> { ["program-action"] = BoundaryWjAction, ["frame-id"] = frame.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch });
    internal IReadOnlyList<PromptChoice> BoundaryWjChoices(ProgramSkillFrame frame)
    {
        var receipt = frame.BoundaryWangJi!;
        if (receipt.Stage == BoundaryWangJiStage.Charge)
            return _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive).SelectMany(player => BoundaryWjChargeSources(player.Seat).Select(skill => BoundaryWjChoice(frame, "charge", player.Seat, $"令 {(player.Name)} 的 {(_contentRegistryOperations._contentRegistry.GetSkill(skill).Name)} 获得1点蓄力")with { Id = new($"boundary-wang-ji.{(frame.Id)}.charge.{(player.Seat)}.{(skill)}"), Parameters = new Dictionary<string, string> { ["program-action"] = BoundaryWjAction, ["frame-id"] = frame.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = "charge", ["charge-skill"] = skill } })).ToArray();
        if (receipt.Stage != BoundaryWangJiStage.Inspect || receipt.Cursor >= receipt.Targets.Count)
            return[];
        var target = receipt.Targets[receipt.Cursor];
        return receipt.Viewed.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Hand(target) && !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(target, _coreAdvancedLifecycleOperations.GetAdvancedCard(id), CardLocation.Hand(target), OwnedCardMoveIntent.Discard)).Select(id => BoundaryWjChoice(frame, "discard", id, $"弃置 {(_coreMatchParticipantsPlayersAccess._players[target].Name)} 的 {(_coreAdvancedLifecycleOperations.GetAdvancedCard(id).DisplayName)}", true)).ToArray();
    }

    internal void PublishBoundaryWj(ProgramSkillFrame frame)
    {
        var choices = BoundaryWjChoices(frame);
        if (choices.Count == 0)
        {
            FinishBoundaryWj(frame);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { Step = ResolutionFrameStep.AwaitingResponse });
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, frame.OwnerSeat, frame.BoundaryWangJi!.Stage == BoundaryWangJiStage.Inspect ? "奇制：观看其全部手牌，选择弃置一张。" : "进趋：选择获得蓄力的角色。", choices.SelectMany(choice => choice.Cards).Distinct().ToArray(), choices.SelectMany(choice => choice.Targets).Distinct().ToArray(), frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer(BoundaryWjAction)]
    internal void AnswerBoundaryWj(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { BoundaryWangJi: { } receipt } frame || !ValidBoundaryWj(frame) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != frame.OwnerSeat || !BoundaryWjChoices(frame).Any(candidate => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([candidate], [choice])))
            throw new InvalidOperationException("Wang Ji's private choice changed its original owner, inspected hand or native frame.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { Step = ResolutionFrameStep.ResolvingEffect });
        if (receipt.Stage == BoundaryWangJiStage.Charge)
        {
            GrantBoundaryWjCharge(frame, choice.Targets.Single(), choice.Parameters["charge-skill"]);
            FinishBoundaryWj(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id));
            return;
        }

        var target = receipt.Targets[receipt.Cursor];
        var id = choice.Cards.Single();
        var from = CardLocation.Hand(target);
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(id);
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundaryWangJi = receipt with { Stage = BoundaryWangJiStage.DiscardChildren, Before = before, After = before, Payment = null }, PendingMovementContinuation = new(target, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(frame, frame.OwnerSeat, [id], new(BoundaryWjDiscardReason));
        var movement = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Single(move => move.Sequence > before && move.CardId == id && move.From == from && move.Reason.Value == BoundaryWjDiscardReason);
        var payment = new BoundaryWangJiDiscardedEvent(frame.Id, receipt.Cursor, target, id, card.Kind, from, movement.To, movement.Sequence);
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundaryWangJi = frame.BoundaryWangJi!with { Payment = payment, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(payment);
        ContinueBoundaryWj(frame.Id);
    }

    internal bool DrainBoundaryWj(ProgramSkillFrame frame) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(frame.Id, PostEventContinuation.AwaitedProgramMovement);
    internal void ContinueBoundaryWj(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { BoundaryWangJi: { } receipt } frame || frame.Id != id)
            return;
        if (!ValidBoundaryWj(frame))
            throw new InvalidOperationException("Wang Ji lost its original instruction or paid native continuation.");
        if (receipt.Stage == BoundaryWangJiStage.Drawing)
            return;
        if (receipt.Stage is BoundaryWangJiStage.DiscardChildren or BoundaryWangJiStage.DrawChildren)
        {
            if (DrainBoundaryWj(frame))
                return;
            frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            receipt = frame.BoundaryWangJi!;
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { PendingMovementContinuation = null });
            if (receipt.Stage == BoundaryWangJiStage.DrawChildren)
            {
                FinishBoundaryWj(frame);
                return;
            }

            receipt = receipt with
            {
                Stage = BoundaryWangJiStage.Inspect,
                Cursor = receipt.Cursor + 1,
                Viewed = [],
                Payment = null
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundaryWangJi = receipt });
        }

        if (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            FinishBoundaryWj(frame);
            return;
        }

        if (receipt.Stage == BoundaryWangJiStage.Inspect)
        {
            while (receipt.Cursor < receipt.Targets.Count && (!_coreMatchParticipantsPlayersAccess._players[receipt.Targets[receipt.Cursor]].IsAlive || _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[receipt.Targets[receipt.Cursor]]).Count == 0))
            {
                receipt = receipt with
                {
                    Cursor = receipt.Cursor + 1
                };
                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundaryWangJi = receipt });
            }

            if (receipt.Cursor == receipt.Targets.Count)
            {
                FinishBoundaryWj(frame);
                return;
            }

            if (receipt.Viewed.Count == 0)
            {
                receipt = receipt with
                {
                    Viewed = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[receipt.Targets[receipt.Cursor]]).Select(card => card.Id).ToArray()
                };
                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundaryWangJi = receipt });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryWangJiInspectionEvent(frame.Id, receipt.Cursor, receipt.Targets[receipt.Cursor], receipt.Viewed.Count));
            }

            if (BoundaryWjChoices(frame).Count == 0)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(frame with { BoundaryWangJi = receipt with { Cursor = receipt.Cursor + 1, Viewed = [] } });
                ContinueBoundaryWj(id);
                return;
            }
        }

        PublishBoundaryWj(frame);
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.SpendBoundaryWangJiQizhi, SkillProgramEffectOp.ResolveBoundaryWangJiJinqu)]
    internal void ReturnBoundaryWjDraw(ProgramSkillFrame frame, NativeDrawOutcome outcome)
    {
        if (frame.BoundaryWangJi is not { Stage: BoundaryWangJiStage.Drawing, Draw: null } receipt || !ValidBoundaryWj(frame) || outcome.RecipientSeat != (receipt.Op == SkillProgramEffectOp.SpendBoundaryWangJiQizhi ? receipt.Targets.Single() : frame.OwnerSeat) || outcome.RequestedCount != receipt.Requested)
            throw new InvalidOperationException("Wang Ji must return its original requested native draw and real recipient.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundaryWangJi = receipt with { Stage = BoundaryWangJiStage.DrawChildren, Draw = outcome, Before = outcome.SequenceBefore, After = outcome.SequenceAfter }, PendingMovementContinuation = new(outcome.RecipientSeat, 0, null) });
        if (receipt.Op == SkillProgramEffectOp.SpendBoundaryWangJiQizhi && outcome.ActualDrawIds.Count > 0)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryWangJiRecipientEvent(frame.Id, receipt.Source, receipt.GameplayHash, receipt.Phase!, outcome.RecipientSeat));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryWangJiDrawReturnedEvent(frame.Id, outcome.RequestedCount, outcome.ActualDrawIds.Count, outcome.AttemptFrameId));
        ContinueBoundaryWj(frame.Id);
    }

    internal void FinishBoundaryWj(ProgramSkillFrame frame)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundaryWangJi = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(frame, true);
    }

    [ProgramViewContributor(ProgramViewContribution.PrivateCards, 54400)]
    internal CardSnapshot[] BoundaryWjPrivateHand(int viewer) => _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Where(frame => frame.OwnerSeat == viewer && frame.BoundaryWangJi is { Stage: BoundaryWangJiStage.Inspect } && ValidBoundaryWj(frame)).SelectMany(frame => frame.BoundaryWangJi!.Viewed.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Hand(frame.BoundaryWangJi.Targets[frame.BoundaryWangJi.Cursor])).Select(id => _closedHandCardViewQueriesSource.Get().Project(_coreMatchParticipantsPlayersAccess._players[frame.BoundaryWangJi.Targets[frame.BoundaryWangJi.Cursor]], _coreAdvancedLifecycleOperations.GetAdvancedCard(id)))).ToArray();
    [ProgramAiChoice(SkillProgramEffectOp.ResolveBoundaryWangJiQizhi, SkillProgramEffectOp.ResolveBoundaryWangJiJinqu)]
    internal PromptChoice AiBoundaryWj(PendingDecision decision, ProgramSkillFrame frame) => frame.BoundaryWangJi!.Stage == BoundaryWangJiStage.Charge ? decision.Choices.FirstOrDefault(choice => choice.Targets.Contains(frame.OwnerSeat)) ?? decision.Choices.First() : decision.Choices.OrderByDescending(choice => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(choice.Cards.Single()), _coreMatchParticipantsPlayersAccess._players[frame.BoundaryWangJi.Targets[frame.BoundaryWangJi.Cursor]])).First();
}
