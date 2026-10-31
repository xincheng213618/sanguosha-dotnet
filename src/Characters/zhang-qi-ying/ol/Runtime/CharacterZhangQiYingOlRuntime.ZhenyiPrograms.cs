#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangQiYingOlRuntime
{
    internal IReadOnlyList<PromptChoice> ZhenyiJudgmentChoices(ProgramSkillFrame f) => Array.AsReadOnly(new[] { Suit.Spade, Suit.Heart }.Select(s => new PromptChoice(new($"zhenyi.{(f.Id)}.{(s)}"), s == Suit.Spade ? "黑桃5" : "红桃5", [], [], new Dictionary<string, string> { { "program-action", "zhenyi-judgment" }, { "suit", s.ToString() } })).ToArray());
    internal void PublishZhenyiJudgment(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "将实际判定结果改为黑桃5或红桃5", [], [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = false,
            Choices = ZhenyiJudgmentChoices(f)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAiChoice(SkillProgramEffectOp.ChooseFinalJudgmentFiveByZiwei)]
    internal PromptChoice ZhenyiAiJudgment(ProgramSkillFrame f) => ZhenyiJudgmentChoices(f).First();
    [ProgramAnswer("zhenyi-judgment")]
    internal void ResolveZhenyiJudgment(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ZhangQiYing: { Stage: "judgment-choice" } } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], ZhenyiJudgmentChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Zhenyi judgment lost its exact actual chooser.");
        AssertZhangQiYing(f);
        var j = f.WindowContext!.Judgment!;
        var pending = _coreJudgmentCursorRuntimeOperations.ActiveJudgment!;
        if (pending.Id != j.JudgmentFrameId || _coreNativeDamageHandlesOperations.GetJudgmentCard(pending)?.Id != j.CardId || _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Markers.GetValueOrDefault(PlayerMarkerKind.Ziwei) != 1)
            throw new InvalidOperationException("Zhenyi judgment source/result changed before payment.");
        var receipt = new ZhenyiJudgmentOverride(f.Id, pending.Id, ZhangQiYingSource(f), f.GameplayHash, j.CardId, Enum.Parse<Suit>(choice.Parameters["suit"]), 5);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        SetFaluToken(f, PlayerMarkerKind.Ziwei, 0, "ziwei");
        _coreRuntimeOperations.ReplaceRuntimeFrame(pending.Id, pending with { ZhenyiOverride = receipt, Suit = receipt.Suit });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhenyiJudgmentOverriddenEvent(receipt));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhangQiYing = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }
}
