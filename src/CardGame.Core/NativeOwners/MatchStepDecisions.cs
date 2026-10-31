#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MatchStepDecisions(global::System.Func<global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries> getCorePendingDecisionPolicyPipelineRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramTopReorderEntries> getCoreGameEngineProgramTopReorderRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineJudgmentProgramsEntries> getCoreGameEngineJudgmentProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramRepeatedJudgmentEntries> getCoreGameEngineProgramRepeatedJudgmentRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineJudgmentReplacementProgramsEntries> getCoreGameEngineJudgmentReplacementProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLuMengMouEntries> getCharacterLuMengMouRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativePromptBoundaryEntries> getCoreNativePromptBoundaryRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillPromptsEntries> getCoreGameEngineSkillPromptsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativeAiDecisionsEntries> getCoreNativeAiDecisionsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePindianEntries> getCoreGameEnginePindianRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHuanShuJunOlEntries> getCharacterHuanShuJunOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLiuZhangOlEntries> getCharacterLiuZhangOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLiuFengBoundaryEntries> getCharacterLiuFengBoundaryRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterGuoTuOlEntries> getCharacterGuoTuOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineRequestedDeckBasicsEntries> getCoreGameEngineRequestedDeckBasicsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicResponsesEntries> getCoreGameEngineDrawFundedDistinctBasicResponsesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineChainedStateBasicConversionsEntries> getCoreGameEngineChainedStateBasicConversionsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardDeclarationsEntries> getCoreGameEngineCardDeclarationsRuntime) : IMatchStepDecisions
{
    public bool Pending(MatchAutomaticDecision kind) => kind switch
    {
        MatchAutomaticDecision.RecoveryReplacement => getCorePendingDecisionPolicyPipelineRuntime()._pendingDecision is { Kind: DecisionKind.RecoveryReplacement } recovery && !matchParticipantsState.ReferenceToPlayers[recovery.PlayerSeat].IsHuman,
        MatchAutomaticDecision.ProgramTrigger => getCorePendingDecisionPolicyPipelineRuntime()._pendingDecision is { Kind: DecisionKind.ProgramTrigger } trigger && !matchParticipantsState.ReferenceToPlayers[trigger.PlayerSeat].IsHuman,
        MatchAutomaticDecision.ProgramTopReorder => getCoreGameEngineProgramTopReorderRuntime().IsAiProgramTopReorderPending(),
        MatchAutomaticDecision.ProgramJudgment => getCoreGameEngineJudgmentProgramsRuntime().IsAiProgramJudgmentPending(),
        MatchAutomaticDecision.ProgramRepeatJudgment => getCoreGameEngineProgramRepeatedJudgmentRuntime().IsAiProgramRepeatJudgmentPending(),
        MatchAutomaticDecision.ProgramJudgmentReplacement => getCoreGameEngineJudgmentReplacementProgramsRuntime().IsAiProgramJudgmentReplacementPending(),
        MatchAutomaticDecision.Yingbo => getCharacterLuMengMouRuntime().IsAiYingboPending(),
        MatchAutomaticDecision.SkipDiscardPolicy => getCoreNativePromptBoundaryRuntime().IsAiSkipDiscardPolicyPending(),
        MatchAutomaticDecision.Pindian => getCoreGameEngineSkillPromptsRuntime().IsAiPindianPending(),
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
                getCoreGameEngineJudgmentProgramsRuntime().ResolvePendingAiProgramJudgment();
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
        MatchSpecialRequest.DrawFundedDistinctBasic => getCoreGameEngineDrawFundedDistinctBasicResponsesRuntime().TryAdvanceDrawFundedDistinctBasicAi(),
        MatchSpecialRequest.ChainedStateBasic => getCoreGameEngineChainedStateBasicConversionsRuntime().TryAdvanceChainedStateBasicAi(),
        _ => throw new ArgumentOutOfRangeException(nameof(kind))};
    public void ContinueDeclaration(long frameId) => getCoreGameEngineCardDeclarationsRuntime().ContinueCardDeclarationChallenge(frameId);
    public void AnswerDeclaration(PromptChoice choice) => getCoreGameEngineCardDeclarationsRuntime().AnswerCardDeclaration(choice);
}
