#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MatchStepDecisions(Func<RuntimeEntries.CorePendingDecisionPolicyPipelineEntries> getCorePendingDecisionPolicyPipelineRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreGameEngineProgramTopReorderEntries> getCoreGameEngineProgramTopReorderRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineJudgmentProgramsRuntime>> getCoreGameEngineJudgmentProgramsRuntime, Func<RuntimeEntries.CoreGameEngineProgramRepeatedJudgmentEntries> getCoreGameEngineProgramRepeatedJudgmentRuntime, Func<RuntimeEntries.CoreGameEngineJudgmentReplacementProgramsEntries> getCoreGameEngineJudgmentReplacementProgramsRuntime, Func<RuntimeEntries.CharacterLuMengMouEntries> getCharacterLuMengMouRuntime, Func<RuntimeEntries.CoreNativePromptBoundaryEntries> getCoreNativePromptBoundaryRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineSkillPromptsRuntime>> getCoreGameEngineSkillPromptsRuntime, Func<RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, Func<RuntimeEntries.CoreNativeAiDecisionsEntries> getCoreNativeAiDecisionsRuntime, Func<RuntimeEntries.CoreGameEnginePindianEntries> getCoreGameEnginePindianRuntime, Func<RuntimeEntries.CharacterHuanShuJunOlEntries> getCharacterHuanShuJunOlRuntime, Func<RuntimeEntries.CharacterLiuZhangOlEntries> getCharacterLiuZhangOlRuntime, Func<RuntimeEntries.CharacterLiuFengBoundaryEntries> getCharacterLiuFengBoundaryRuntime, Func<RuntimeEntries.CharacterGuoTuOlEntries> getCharacterGuoTuOlRuntime, Func<RuntimeEntries.CoreGameEngineRequestedDeckBasicsEntries> getCoreGameEngineRequestedDeckBasicsRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineDrawFundedDistinctBasicResponsesRuntime>> getCoreGameEngineDrawFundedDistinctBasicResponsesRuntime, Func<RuntimeEntries.CoreGameEngineChainedStateBasicConversionsEntries> getCoreGameEngineChainedStateBasicConversionsRuntime, Func<RuntimeEntries.CoreGameEngineCardDeclarationsEntries> getCoreGameEngineCardDeclarationsRuntime) : IMatchStepDecisions
{
    public bool Pending(MatchAutomaticDecision kind) => kind switch
    {
        MatchAutomaticDecision.RecoveryReplacement => getCorePendingDecisionPolicyPipelineRuntime()._pendingDecision is { Kind: DecisionKind.RecoveryReplacement } recovery && !matchParticipantsState.ReferenceToPlayers[recovery.PlayerSeat].IsHuman,
        MatchAutomaticDecision.ProgramTrigger => getCorePendingDecisionPolicyPipelineRuntime()._pendingDecision is { Kind: DecisionKind.ProgramTrigger } trigger && !matchParticipantsState.ReferenceToPlayers[trigger.PlayerSeat].IsHuman,
        MatchAutomaticDecision.ProgramTopReorder => getCoreGameEngineProgramTopReorderRuntime().IsAiProgramTopReorderPending(),
        MatchAutomaticDecision.ProgramJudgment => getCoreGameEngineJudgmentProgramsRuntime().Value.IsAiProgramJudgmentPending(),
        MatchAutomaticDecision.ProgramRepeatJudgment => getCoreGameEngineProgramRepeatedJudgmentRuntime().IsAiProgramRepeatJudgmentPending(),
        MatchAutomaticDecision.ProgramJudgmentReplacement => getCoreGameEngineJudgmentReplacementProgramsRuntime().IsAiProgramJudgmentReplacementPending(),
        MatchAutomaticDecision.Yingbo => getCharacterLuMengMouRuntime().IsAiYingboPending(),
        MatchAutomaticDecision.SkipDiscardPolicy => getCoreNativePromptBoundaryRuntime().IsAiSkipDiscardPolicyPending(),
        MatchAutomaticDecision.Pindian => getCoreGameEngineSkillPromptsRuntime().Value.IsAiPindianPending(),
        MatchAutomaticDecision.TargetCardSelection => getCoreNativePromptBoundaryRuntime().IsAiTargetCardSelectionPending(),
        MatchAutomaticDecision.FireAttack => getCoreNativePromptBoundaryRuntime().IsAiFireAttackPending(),
        MatchAutomaticDecision.Nullification => getCoreNativePromptBoundaryRuntime().IsAiNullificationPending(),
        MatchAutomaticDecision.Response => getCoreNativePromptBoundaryRuntime().IsAiResponsePending(),
        MatchAutomaticDecision.DyingResponse => getCoreNativePromptBoundaryRuntime().IsAiDyingResponsePending(),
        MatchAutomaticDecision.Harvest => getCoreNativePromptBoundaryRuntime().IsAiHarvestPending(),
        _ => throw new ArgumentOutOfRangeException(nameof(kind))};
    public void Resolve(MatchAutomaticDecision kind)
    {
        switch (kind)
        {
            case MatchAutomaticDecision.RecoveryReplacement:
                getCoreRecoveryReplacementRuntime().ResolveAiRecoveryReplacement();
                break;
            case MatchAutomaticDecision.ProgramTrigger:
                getCoreGameEngineProgramLifecycleRuntime().ResolvePendingAiProgramTrigger();
                break;
            case MatchAutomaticDecision.ProgramTopReorder:
                getCoreGameEngineProgramTopReorderRuntime().ResolvePendingAiProgramTopReorder();
                break;
            case MatchAutomaticDecision.ProgramJudgment:
                getCoreGameEngineJudgmentProgramsRuntime().Value.ResolvePendingAiProgramJudgment();
                break;
            case MatchAutomaticDecision.ProgramRepeatJudgment:
                getCoreGameEngineProgramRepeatedJudgmentRuntime().ResolvePendingAiProgramRepeatJudgment();
                break;
            case MatchAutomaticDecision.ProgramJudgmentReplacement:
                getCoreGameEngineJudgmentReplacementProgramsRuntime().ResolvePendingAiProgramJudgmentReplacement();
                break;
            case MatchAutomaticDecision.Yingbo:
                getCharacterLuMengMouRuntime().ResolvePendingAiYingbo();
                break;
            case MatchAutomaticDecision.SkipDiscardPolicy:
                getCoreNativeAiDecisionsRuntime().ResolvePendingAiSkipDiscardPolicy();
                break;
            case MatchAutomaticDecision.Pindian:
                getCoreGameEnginePindianRuntime().ResolvePendingAiPindian();
                break;
            case MatchAutomaticDecision.TargetCardSelection:
                getCoreNativeAiDecisionsRuntime().ResolvePendingAiTargetCardSelection();
                break;
            case MatchAutomaticDecision.FireAttack:
                getCoreNativeAiDecisionsRuntime().ResolvePendingAiFireAttack();
                break;
            case MatchAutomaticDecision.Nullification:
                getCoreNativeAiDecisionsRuntime().ResolvePendingAiNullification();
                break;
            case MatchAutomaticDecision.Response:
                getCoreNativeAiDecisionsRuntime().ResolvePendingAiResponse();
                break;
            case MatchAutomaticDecision.Harvest:
                getCoreNativeAiDecisionsRuntime().ResolvePendingAiHarvest();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(kind));
        }
    }

    public bool TryAdvanceRequest(MatchSpecialRequest kind) => kind switch
    {
        MatchSpecialRequest.Huanpei => getCharacterHuanShuJunOlRuntime().TryAdvanceHuanpeiAi(),
        MatchSpecialRequest.Zonghu => getCharacterLiuZhangOlRuntime().TryAdvanceZonghuAi(),
        MatchSpecialRequest.BoundaryLiuFeng => getCharacterLiuFengBoundaryRuntime().TryAdvanceBoundaryLiuFengAi(),
        MatchSpecialRequest.Weijie => getCharacterGuoTuOlRuntime().TryAdvanceWeijieAi(),
        MatchSpecialRequest.RequestedDeckBasic => getCoreGameEngineRequestedDeckBasicsRuntime().TryAdvanceRequestedDeckBasicAi(),
        MatchSpecialRequest.DrawFundedDistinctBasic => getCoreGameEngineDrawFundedDistinctBasicResponsesRuntime().Value.TryAdvanceDrawFundedDistinctBasicAi(),
        MatchSpecialRequest.ChainedStateBasic => getCoreGameEngineChainedStateBasicConversionsRuntime().TryAdvanceChainedStateBasicAi(),
        _ => throw new ArgumentOutOfRangeException(nameof(kind))};
    public void ContinueDeclaration(long frameId) => getCoreGameEngineCardDeclarationsRuntime().ContinueCardDeclarationChallenge(frameId);
    public void AnswerDeclaration(PromptChoice choice) => getCoreGameEngineCardDeclarationsRuntime().AnswerCardDeclaration(choice);
}
