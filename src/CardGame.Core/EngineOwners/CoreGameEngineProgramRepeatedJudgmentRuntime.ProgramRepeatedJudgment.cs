#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramRepeatedJudgmentRuntime
{
    internal SkillProgramStepOutcome BeginProgramRepeatedJudgment(ProgramSkillFrame frame, string reason, string resultBind, IReadOnlyList<Suit> successSuits)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.RepeatedJudgment is not null || successSuits.Count == 0 || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive)
            throw new InvalidOperationException("Repeated judgment requires one active owner and success suit set.");
        active = active with
        {
            RepeatedJudgment = new ProgramRepeatedJudgment(reason, resultBind, Array.AsReadOnly(successSuits.ToArray()), 0)
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(active);
        return _coreProgramLifecycleOperations.StartProgramJudgment(active, frame.OwnerSeat, reason, resultBind, SkillProgramCardSetVisibility.Public, frame.OwnerSeat);
    }

    internal void ResumeProgramRepeatedJudgment(JudgmentFrame pending)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(pending.ParentFrameId);
        var state = frame.RepeatedJudgment ?? throw new InvalidOperationException("Repeated judgment lost its program state.");
        if (state.ClaimHandLimitExemption is not null)
        {
            _coreExactRepeatedJudgmentOperations.ResumeProgramExactRepeatedJudgment(pending, frame, state);
            return;
        }

        var card = _coreNativeDamageHandlesOperations.GetJudgmentCard(pending);
        if (card is null || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { RepeatedJudgment = null });
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
            return;
        }

        var matched = state.SuccessSuits.Contains(_characterZhangQiYingOlOperations.EffectiveZhenyiJudgmentSuit(pending, _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[pending.TargetSeat], card)));
        var location = _cardZonesOperations._cardZones.GetLocation(card.Id);
        if (location != CardLocation.Judgment(pending.TargetSeat))
            throw new InvalidOperationException("Repeated judgment card left its resolution zone.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { RepeatedJudgment = state with { CompletedCount = checked(state.CompletedCount + 1), LastMatched = matched }, PendingMovementContinuation = new ProgramMovementContinuation(frame.OwnerSeat, 0, null) });
        _corePhysicalMovementFacadesOperations.MoveCard(card, location, matched ? CardLocation.Hand(frame.OwnerSeat) : CardLocation.DiscardPile, new CardMoveReason($"skill-program.{(frame.SkillId)}.repeatJudgment"));
        if (matched)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramJudgmentCardClaimedEvent(pending.Id, frame.SkillId, frame.OwnerSeat, card.Id, card.Kind));
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.ReturnRuntimeProgramMovement(frame.Id);
    }

    internal void ContinueProgramRepeatedJudgmentAfterMovement(long frameId)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
        var state = frame.RepeatedJudgment ?? throw new InvalidOperationException("Repeated judgment lost its movement result.");
        if (state.LastMatched != true)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { RepeatedJudgment = null });
            _coreRuntimeOperations.AdvanceRuntimeProgram(frameId);
            return;
        }

        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramRepeatJudgment, frame.OwnerSeat, "判定符合继续条件，是否再次判定？", [], [])
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = frame.OwnerSeat,
            Choices = [new PromptChoice(new ChoiceId($"program-repeat-judgment.{(frameId)}.continue"), "继续判定。", [], [], new Dictionary<string, string> { ["action"] = "continue" }), new PromptChoice(new ChoiceId($"program-repeat-judgment.{(frameId)}.stop"), "停止判定。", [], [], new Dictionary<string, string> { ["action"] = "stop" })]
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    internal CommandResult SubmitProgramRepeatJudgmentAnswer(PromptChoice choice) => _kernelCoreMainOperations.Accept(() =>
    {
        ResolveProgramRepeatJudgmentChoice(choice);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        if (_coreMatchDefinitionOptionsAccess._options.AdvanceAfterHumanCommands)
            _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
    });
    internal void ResolveProgramRepeatJudgmentChoice(PromptChoice choice)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Repeated judgment has no active program frame.");
        var state = frame.RepeatedJudgment;
        if (state?.LastMatched != true || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramRepeatJudgment, PlayerSeat: var seat } decision || seat != frame.OwnerSeat || !decision.Choices.Any(item => item.Id == choice.Id))
            throw new InvalidOperationException("Repeated judgment choice is no longer current.");
        var action = choice.Parameters.GetValueOrDefault("action");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (action == "stop")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { RepeatedJudgment = null });
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
            return;
        }

        if (action != "continue")
            throw new InvalidOperationException("Repeated judgment choice is unsupported.");
        frame = frame with
        {
            RepeatedJudgment = state.ClaimHandLimitExemption is null ? state with
            {
                LastMatched = null
            }

            : state with
            {
                LastMatched = null,
                FinalOutcome = null,
                LastClaimReceipt = null
            }
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame);
        _ = _coreProgramLifecycleOperations.StartProgramJudgment(frame, frame.OwnerSeat, state.Reason, state.ResultBind, SkillProgramCardSetVisibility.Public, frame.OwnerSeat);
    }

    internal bool IsAiProgramRepeatJudgmentPending() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is { Kind: DecisionKind.ProgramRepeatJudgment, PlayerSeat: var seat } && !_coreMatchParticipantsPlayersAccess._players[seat].IsHuman;
    internal void ResolvePendingAiProgramRepeatJudgment()
    {
        var decision = _corePendingDecisionPolicyPipelineOperations._pendingDecision ?? throw new InvalidOperationException("AI repeated judgment has no prompt.");
        ResolveProgramRepeatJudgmentChoice(decision.Choices[0]);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }
}
