#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunYuanOlRuntime
{
    internal void PublishSaying(ProgramSkillFrame supplied, bool keepCurrentRequest = false)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.SunYuan!;
        if (r.Stage is SunYuanStage.SlashIssuing or SunYuanStage.NativeUse or SunYuanStage.NativeChildren)
            return;
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || r.Stage == SunYuanStage.Request && !_coreMatchParticipantsPlayersAccess._players[r.Holder].IsAlive)
        {
            FinishSunYuan(f, r.Paid);
            return;
        }

        if (!r.Paid && !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
        {
            FinishSunYuan(f, false);
            return;
        }

        var choices = SunYuanChoices(f);
        if (choices.Count == 0)
        {
            FinishSunYuan(f, r.Paid);
            return;
        }

        var chooser = r.Stage == SunYuanStage.Request ? r.Holder : f.OwnerSeat;
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, r.Stage == SunYuanStage.Request ? "飒影：可对孙鸢使用一张杀。" : r.Stage == SunYuanStage.Target ? "飒影：选择一名其他角色。" : r.Stage == SunYuanStage.ObtainCard ? "飒影：获得一张区域内的牌。" : $"恫心：选择第 {(r.Selected.Count + 1)}/{(r.Required)} 张弃置牌。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = r.Holder,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }
}
