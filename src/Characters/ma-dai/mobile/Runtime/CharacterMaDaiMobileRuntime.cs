#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterMaDaiMobileRuntime : global::CardGame.Core.IMobileMaDaiProgramHost
{
    private readonly ICharacterMaDaiMobileCharacterDaQiaoXiaoQiaoHappyGodOperations _characterDaQiaoXiaoQiaoHappyGodOperations;
    private readonly ICharacterMaDaiMobileResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterMaDaiMobileCoreProgramAttackAttemptOperations _coreProgramAttackAttemptOperations;
    private readonly ICharacterMaDaiMobileCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICharacterMaDaiMobileCoreProgramJudgmentOutcomesOperations _coreProgramJudgmentOutcomesOperations;
    private readonly ICharacterMaDaiMobileCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterMaDaiMobileCoreWushuangTacticRulesOperations _coreWushuangTacticRulesOperations;
    private readonly ICharacterMaDaiMobileCoreNativePlayerDeathOperations _coreNativePlayerDeathOperations;
    private readonly ICharacterMaDaiMobileCoreNativeRuleQueriesOperations _coreNativeRuleQueriesOperations;
    private readonly ICharacterMaDaiMobileKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterMaDaiMobileRuntime(ICharacterMaDaiMobileCharacterDaQiaoXiaoQiaoHappyGodOperations characterDaQiaoXiaoQiaoHappyGodOperations, ICharacterMaDaiMobileResolutionStackOperations resolutionStackOperations, ICharacterMaDaiMobileCoreProgramAttackAttemptOperations coreProgramAttackAttemptOperations, ICharacterMaDaiMobileCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICharacterMaDaiMobileCoreProgramJudgmentOutcomesOperations coreProgramJudgmentOutcomesOperations, ICharacterMaDaiMobileCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterMaDaiMobileCoreWushuangTacticRulesOperations coreWushuangTacticRulesOperations, ICharacterMaDaiMobileCoreNativePlayerDeathOperations coreNativePlayerDeathOperations, ICharacterMaDaiMobileCoreNativeRuleQueriesOperations coreNativeRuleQueriesOperations, ICharacterMaDaiMobileKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterDaQiaoXiaoQiaoHappyGodOperations = characterDaQiaoXiaoQiaoHappyGodOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreProgramAttackAttemptOperations = coreProgramAttackAttemptOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramJudgmentOutcomesOperations = coreProgramJudgmentOutcomesOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreWushuangTacticRulesOperations = coreWushuangTacticRulesOperations;
        this._coreNativePlayerDeathOperations = coreNativePlayerDeathOperations;
        this._coreNativeRuleQueriesOperations = coreNativeRuleQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
