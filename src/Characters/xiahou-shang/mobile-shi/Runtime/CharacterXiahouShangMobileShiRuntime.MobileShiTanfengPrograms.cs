#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXiahouShangMobileShiRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.IMobileShiTanfengHost.SelectTarget(ProgramSkillFrame frame, SkillProgramEffect effect) => SelectMobileShiTanfengTarget(frame, effect);
    internal string MobileShiTanfengMode(ProgramSkillFrame frame) => frame.ChoiceBindings.Single(binding => binding.Name == "tanfeng-mode").OptionId;
    internal SkillProgramStepOutcome SelectMobileShiTanfengTarget(ProgramSkillFrame supplied, SkillProgramEffect effect)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var mode = MobileShiTanfengMode(frame);
        if (effect.StateId == "initial-target" && mode == "cancel")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { SelectedTargetSeats = [] });
            return SkillProgramStepOutcome.Continue;
        }

        if (mode is not ("discard" or "fire" or "both") || effect.StateId == "fire-target" && mode != "both")
            throw new InvalidOperationException("Tanfeng lost its original preselected branch.");
        var stage = effect.StateId == "fire-target" || mode == "fire" ? "火焰伤害" : "弃置牌";
        var choices = _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive).Select(player => new PromptChoice(new ChoiceId($"shi-tanfeng.frame-{(frame.Id)}.{(effect.StateId)}.seat-{(player.Seat)}"), $"选择 {(player.Name)}（{(stage)}）。", [], [player.Seat], new Dictionary<string, string> { ["program-action"] = "shi-tanfeng-target", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["target-role"] = effect.StateId! })).ToArray();
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, frame.OwnerSeat, $"【探锋】请选择{(stage)}的目标（可以选择自己）。", [], choices.SelectMany(choice => choice.Targets).ToArray(), frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            TargetSeat = frame.OwnerSeat,
            SkillPrompt = new(frame.SkillId, skill.Name, $"探锋 · {(stage)}目标", skill.Description),
            Choices = Array.AsReadOnly(choices)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    [ProgramAnswer("shi-tanfeng-target")]
    internal void ResolveMobileShiTanfengTarget(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Tanfeng target lost its original frame.");
        var effect = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
        if (effect.Op != SkillProgramEffectOp.ResolveMobileShiXiahouShangTanfeng || frame.SkillId != "mobile:shi-tanfeng" || selected.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) || selected.Parameters.GetValueOrDefault("target-role") != effect.StateId || selected.Cards.Count != 0 || selected.Targets is not [var target] || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } prompt || prompt.PlayerSeat != frame.OwnerSeat || MobileShiTanfengMode(frame)is not ("discard" or "fire" or "both") || effect.StateId == "fire-target" && MobileShiTanfengMode(frame) != "both")
            throw new InvalidOperationException("Tanfeng target changed its issued branch, live participant or original frame.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { SelectedTargetSeats = Array.AsReadOnly(new[] { target }) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileShiTanfengTargetSelectedEvent(frame.Id, frame.SkillInstanceId, effect.StateId!, target, MobileShiTanfengMode(frame)));
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    internal bool HasExactMobileShiTanfengParticipantSelection(ProgramSkillFrame frame)
    {
        if (frame.SkillId != "mobile:shi-tanfeng" || frame.TriggerId != "preselect-options" || frame.ActivationId != frame.TriggerId || frame.SelectedCardIds.Count != 0 || frame.SelectedTargetSeats is not [var target] || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || frame.InstructionIndex is < 3 or > 9 || frame.OwnerSeat != _coreActualTurnProgressCurrentSeatAccess._currentSeat || frame.WindowContext is not { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow } context || context.OwnerSeat != frame.OwnerSeat || context.SourceSeat != frame.OwnerSeat || context.TargetSeat != frame.OwnerSeat)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(item => item.Id == frame.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramLifecycleTriggerWindowFrame parent || parent.Id != context.ParentFrameId || parent.OwnerSeat != frame.OwnerSeat || parent.Window != context.Window || parent.Continuation != ProgramLifecycleContinuation.NormalTurnStart || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || parent.Candidates[parent.CandidateIndex] is not { } candidate || candidate.SkillId != frame.SkillId || candidate.BindingId != frame.TriggerId || candidate.SkillInstanceId != frame.SkillInstanceId || candidate.OwnerSeat != frame.OwnerSeat || candidate.GameplayHash != frame.GameplayHash || candidate.OccurrenceIndex != context.OccurrenceIndex)
            return false;
        var mode = frame.ChoiceBindings.SingleOrDefault(binding => binding.Name == "tanfeng-mode");
        if (mode is null || mode.ChooserSeat != frame.OwnerSeat || mode.OptionId is not ("discard" or "fire" or "both"))
            return false;
        var selections = _coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiTanfengTargetSelectedEvent>().Where(item => item.ProgramFrameId == frame.Id).ToArray();
        return selections.Length is >= 1 and <= 2 && selections[0].TargetRole == "initial-target" && selections.All(item => item.SkillInstanceId == frame.SkillInstanceId && item.Mode == mode.OptionId && _coreTableSeatQueriesOperations.IsValidPlayerSeat(item.TargetSeat)) && selections[^1].TargetSeat == target && (selections.Length == 1 || mode.OptionId == "both" && selections[1].TargetRole == "fire-target" && frame.InstructionIndex >= 8);
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 47200)]
    internal void AssertMobileShiTanfengTarget(ProgramSkillFrame frame)
    {
        if (frame.SkillId != "mobile:shi-tanfeng")
            return;
        if (frame.SelectedTargetSeats.Count > 0 && !HasExactMobileShiTanfengParticipantSelection(frame))
            throw new InvalidOperationException("Tanfeng lost its issued participant, original preselection or actual preparation parent.");
        var paused = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
        if (paused.Op != SkillProgramEffectOp.ResolveMobileShiXiahouShangTanfeng || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != frame.Id)
            return;
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } prompt || prompt.PlayerSeat != frame.OwnerSeat || prompt.SkillPrompt?.SkillId != frame.SkillId || prompt.Choices.Count != _coreMatchParticipantsPlayersAccess._players.Count(player => player.IsAlive) || prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") != "shi-tanfeng-target" || choice.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) || choice.Parameters.GetValueOrDefault("target-role") != paused.StateId || choice.Cards.Count != 0 || choice.Targets is not [var target] || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive) || prompt.Choices.Select(choice => choice.Targets[0]).Distinct().Count() != prompt.Choices.Count)
            throw new InvalidOperationException("Tanfeng lost its exact original target prompt.");
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveMobileShiXiahouShangTanfeng)]
    internal PromptChoice SelectMobileShiTanfengAiTarget(PendingDecision decision) => _coreProgramLifecycleOperations.SelectAiProgramTargets(decision, SkillProgramTargetAiOrder.HostileThenHandCount);
}
