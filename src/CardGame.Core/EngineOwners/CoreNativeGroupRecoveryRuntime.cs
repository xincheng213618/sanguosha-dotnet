#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreNativeGroupRecoveryRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDaQiaoXiaoQiaoHappyGodEntries _characterDaQiaoXiaoQiaoHappyGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLuXunMouMobileEntries _characterLuXunMouMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineGroupTargetNullificationEntries _coreGroupTargetNullificationOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries _coreHpChangeProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardTargetEffectsEntries _coreProgramCardTargetEffectsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheAlliedTricksEntries _coreShanheAlliedTricksOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangTacticRulesEntries _coreWushuangTacticRulesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries _coreNativeCardContinuationCursorsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeGroupCompletionEntries _coreNativeGroupCompletionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeNullificationFramesEntries _coreNativeNullificationFramesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeRecoveryFramesEntries _coreNativeRecoveryFramesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreRecoveryReplacementEntries _coreRecoveryReplacementOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreNativeGroupRecoveryRuntime(global::CardGame.Core.RuntimeEntries.CharacterDaQiaoXiaoQiaoHappyGodEntries characterDaQiaoXiaoQiaoHappyGodOperations, global::CardGame.Core.RuntimeEntries.CharacterLuXunMouMobileEntries characterLuXunMouMobileOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineGroupTargetNullificationEntries coreGroupTargetNullificationOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries coreHpChangeProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardTargetEffectsEntries coreProgramCardTargetEffectsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheAlliedTricksEntries coreShanheAlliedTricksOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangTacticRulesEntries coreWushuangTacticRulesOperations, global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries coreNativeCardContinuationCursorsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeGroupCompletionEntries coreNativeGroupCompletionOperations, global::CardGame.Core.RuntimeEntries.CoreNativeNullificationFramesEntries coreNativeNullificationFramesOperations, global::CardGame.Core.RuntimeEntries.CoreNativeRecoveryFramesEntries coreNativeRecoveryFramesOperations, global::CardGame.Core.RuntimeEntries.CoreRecoveryReplacementEntries coreRecoveryReplacementOperations, global::CardGame.Core.EngineOwners.IKernelRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterDaQiaoXiaoQiaoHappyGodOperations = characterDaQiaoXiaoQiaoHappyGodOperations;
        this._characterLuXunMouMobileOperations = characterLuXunMouMobileOperations;
        this._coreGroupTargetNullificationOperations = coreGroupTargetNullificationOperations;
        this._coreHpChangeProgramsOperations = coreHpChangeProgramsOperations;
        this._coreProgramCardTargetEffectsOperations = coreProgramCardTargetEffectsOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreShanheAlliedTricksOperations = coreShanheAlliedTricksOperations;
        this._coreWushuangTacticRulesOperations = coreWushuangTacticRulesOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativeCardContinuationCursorsOperations = coreNativeCardContinuationCursorsOperations;
        this._coreNativeGroupCompletionOperations = coreNativeGroupCompletionOperations;
        this._coreNativeNullificationFramesOperations = coreNativeNullificationFramesOperations;
        this._coreNativeRecoveryFramesOperations = coreNativeRecoveryFramesOperations;
        this._coreRecoveryReplacementOperations = coreRecoveryReplacementOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
