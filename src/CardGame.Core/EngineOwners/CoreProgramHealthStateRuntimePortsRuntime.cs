#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreProgramHealthStateRuntimePortsRuntime : global::CardGame.Core.IProgramHealthStateFacts, global::CardGame.Core.IProgramHealthStateNativeEntries
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDaQiaoXiaoQiaoHappyGodEntries _characterDaQiaoXiaoQiaoHappyGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterSunJianMouOlEntries _characterSunJianMouOlOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelProgramPipelineHooksAccess _programPipelineHooksOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries _coreCharacterStateProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCharacterTurnedOverProgramsEntries _coreCharacterTurnedOverProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries _coreProgramCardPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineQimeiProgramsEntries _coreQimeiProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuleQueriesEntries _coreRuleQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangTacticRulesEntries _coreWushuangTacticRulesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeRecoveryFramesEntries _coreNativeRecoveryFramesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreRecoveryReplacementEntries _coreRecoveryReplacementOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreShenwuNpcProgramsEntries _coreShenwuNpcProgramsOperations;
    internal CoreProgramHealthStateRuntimePortsRuntime(global::CardGame.Core.RuntimeEntries.CharacterDaQiaoXiaoQiaoHappyGodEntries characterDaQiaoXiaoQiaoHappyGodOperations, global::CardGame.Core.RuntimeEntries.CharacterSunJianMouOlEntries characterSunJianMouOlOperations, global::CardGame.Core.EngineOwners.IKernelProgramPipelineHooksAccess programPipelineHooksOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries coreCharacterStateProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCharacterTurnedOverProgramsEntries coreCharacterTurnedOverProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries coreProgramCardPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineQimeiProgramsEntries coreQimeiProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRuleQueriesEntries coreRuleQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangTacticRulesEntries coreWushuangTacticRulesOperations, global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations, global::CardGame.Core.RuntimeEntries.CoreNativeRecoveryFramesEntries coreNativeRecoveryFramesOperations, global::CardGame.Core.RuntimeEntries.CoreRecoveryReplacementEntries coreRecoveryReplacementOperations, global::CardGame.Core.RuntimeEntries.CoreShenwuNpcProgramsEntries coreShenwuNpcProgramsOperations)
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
