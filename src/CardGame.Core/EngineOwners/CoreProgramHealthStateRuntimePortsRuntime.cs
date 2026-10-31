#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CoreProgramHealthStateRuntimePortsRuntime : IProgramHealthStateFacts, IProgramHealthStateNativeEntries
{
    private readonly RuntimeEntries.CharacterDaQiaoXiaoQiaoHappyGodEntries _characterDaQiaoXiaoQiaoHappyGodOperations;
    private readonly RuntimeEntries.CharacterSunJianMouOlEntries _characterSunJianMouOlOperations;
    private readonly IKernelProgramPipelineHooksAccess _programPipelineHooksOperations;
    private readonly RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries _coreCharacterStateProgramsOperations;
    private readonly RuntimeEntries.CoreGameEngineCharacterTurnedOverProgramsEntries _coreCharacterTurnedOverProgramsOperations;
    private readonly RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries _coreProgramCardPoliciesOperations;
    private readonly RuntimeEntries.CoreGameEngineQimeiProgramsEntries _coreQimeiProgramsOperations;
    private readonly RuntimeEntries.CoreGameEngineRuleQueriesEntries _coreRuleQueriesOperations;
    private readonly RuntimeEntries.CoreGameEngineWushuangTacticRulesEntries _coreWushuangTacticRulesOperations;
    private readonly RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly RuntimeEntries.CoreNativeRecoveryFramesEntries _coreNativeRecoveryFramesOperations;
    private readonly RuntimeEntries.CoreRecoveryReplacementEntries _coreRecoveryReplacementOperations;
    private readonly RuntimeEntries.CoreShenwuNpcProgramsEntries _coreShenwuNpcProgramsOperations;
    internal CoreProgramHealthStateRuntimePortsRuntime(RuntimeEntries.CharacterDaQiaoXiaoQiaoHappyGodEntries characterDaQiaoXiaoQiaoHappyGodOperations, RuntimeEntries.CharacterSunJianMouOlEntries characterSunJianMouOlOperations, IKernelProgramPipelineHooksAccess programPipelineHooksOperations, RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries coreCharacterStateProgramsOperations, RuntimeEntries.CoreGameEngineCharacterTurnedOverProgramsEntries coreCharacterTurnedOverProgramsOperations, RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations, RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries coreProgramCardPoliciesOperations, RuntimeEntries.CoreGameEngineQimeiProgramsEntries coreQimeiProgramsOperations, RuntimeEntries.CoreGameEngineRuleQueriesEntries coreRuleQueriesOperations, RuntimeEntries.CoreGameEngineWushuangTacticRulesEntries coreWushuangTacticRulesOperations, RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations, RuntimeEntries.CoreNativeRecoveryFramesEntries coreNativeRecoveryFramesOperations, RuntimeEntries.CoreRecoveryReplacementEntries coreRecoveryReplacementOperations, RuntimeEntries.CoreShenwuNpcProgramsEntries coreShenwuNpcProgramsOperations)
    {
        this._characterDaQiaoXiaoQiaoHappyGodOperations = characterDaQiaoXiaoQiaoHappyGodOperations;
        this._characterSunJianMouOlOperations = characterSunJianMouOlOperations;
        this._programPipelineHooksOperations = programPipelineHooksOperations;
        this._coreCharacterStateProgramsOperations = coreCharacterStateProgramsOperations;
        this._coreCharacterTurnedOverProgramsOperations = coreCharacterTurnedOverProgramsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreQimeiProgramsOperations = coreQimeiProgramsOperations;
        this._coreRuleQueriesOperations = coreRuleQueriesOperations;
        this._coreWushuangTacticRulesOperations = coreWushuangTacticRulesOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativeRecoveryFramesOperations = coreNativeRecoveryFramesOperations;
        this._coreRecoveryReplacementOperations = coreRecoveryReplacementOperations;
        this._coreShenwuNpcProgramsOperations = coreShenwuNpcProgramsOperations;
    }
}
