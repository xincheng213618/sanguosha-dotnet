#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWangYiBoundaryMobileRuntime
{
    SkillProgramStepOutcome IBoundaryWangYiHost.ChooseMijiCount(ProgramSkillFrame frame) => ChooseBoundaryWangYiMijiCount(frame);
    internal SkillProgramStepOutcome ChooseBoundaryWangYiMijiCount(ProgramSkillFrame frame)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (f.SkillId != BoundaryWangYiMobile.Miji || f.BoundaryWangYiMijiCount is not null || f.WindowContext?.Window != SkillProgramTriggerWindow.TurnEnding || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].HasHpValue)
            throw new InvalidOperationException("Mobile Miji count selection requires its original own Ending frame.");
        var maximum = Math.Max(0, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp - _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryWangYiMijiCount = new(maximum) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryWangYiMijiCountIssuedEvent(f.Id, new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, maximum));
        var choices = Enumerable.Range(0, checked(maximum + 1)).Select(count => new PromptChoice(new ChoiceId($"mobile-miji.{(f.Id)}.{(count)}"), count == 0 ? "不摸牌" : $"摸{(count)}张牌", [], [], new Dictionary<string, string> { ["program-action"] = "mobile-miji-count", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["count"] = count.ToString(CultureInfo.InvariantCulture) })).ToArray();
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, f.OwnerSeat, $"【秘计】选择摸牌数（至多{(maximum)}张）。", [], [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Choices = Array.AsReadOnly(choices),
            SkillPrompt = new(f.SkillId, skill.Name, "秘计 · 摸牌数", skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    [ProgramAnswer("mobile-miji-count")]
    internal void AnswerBoundaryWangYiMijiCount(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { SkillId: BoundaryWangYiMobile.Miji, BoundaryWangYiMijiCount: { Count: null } r } f || !ValidBoundaryWangYiMijiCount(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).GetPausedInstruction(f.InstructionIndex).Effect.Op != SkillProgramEffectOp.ChooseBoundaryWangYiMijiCount || choice.Parameters.GetValueOrDefault("frame-id") != f.Id.ToString(CultureInfo.InvariantCulture) || choice.Cards.Count != 0 || choice.Targets.Count != 0 || !int.TryParse(choice.Parameters.GetValueOrDefault("count"), out var count) || count < 0 || count > r.Maximum)
            throw new InvalidOperationException("The mobile Miji count lost its exact original frame or frozen maximum.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { BoundaryWangYiMijiCount = r with { Count = count } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryWangYiMijiCountChosenEvent(f.Id, f.OwnerSeat, count));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ChooseBoundaryWangYiMijiCount)]
    internal static PromptChoice ChooseAiBoundaryWangYiMijiCount(PendingDecision decision, ProgramSkillFrame frame) => decision.Choices[^1];
    internal int BoundaryWangYiMijiDrawCount(ProgramSkillFrame f) => ValidBoundaryWangYiMijiCount(f) && f.BoundaryWangYiMijiCount?.Count is { } count ? count : throw new InvalidOperationException("Mobile Miji draw requires its committed original count selection.");
    internal bool ValidBoundaryWangYiMijiCount(ProgramSkillFrame f) => f.SkillId == BoundaryWangYiMobile.Miji && f.BoundaryWangYiMijiCount is { Maximum: >= 0 } r && f.TriggerId == "natural-ending" && f.WindowContext?.Window == SkillProgramTriggerWindow.TurnEnding && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<BoundaryWangYiMijiCountIssuedEvent>().Count(e => e.FrameId == f.Id && e.Source == new CardConversionSource(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId) && e.GameplayHash == f.GameplayHash && e.Maximum == r.Maximum) == 1 && (r.Count is null ? _coreProgramEventHistoryOperations.Value.ProgramEventHistory<BoundaryWangYiMijiCountChosenEvent>().All(e => e.FrameId != f.Id) && f.InstructionIndex == 1 : r.Count >= 0 && r.Count <= r.Maximum && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<BoundaryWangYiMijiCountChosenEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.Count == r.Count) == 1);
}
