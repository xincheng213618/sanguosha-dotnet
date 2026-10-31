#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineRuleQueriesRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDaQiaoXiaoQiaoHappyGodEntries _characterDaQiaoXiaoQiaoHappyGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreDirectedTurnCardPoliciesEntries _coreDirectedTurnCardPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineActionQueriesEntries _coreActionQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseModulesEntries _coreCardUseModulesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDeckEquipmentProgramsEntries _coreDeckEquipmentProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreMatchSetupDefinitionsEntries _coreMatchSetupDefinitionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardAppearanceEntries _coreNativeCardAppearanceOperations;
    private readonly ICoreRuleQueriesCoreProgramTargetRulesOperations _coreProgramTargetRulesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelRuleQueryFactFactories _kernelCoreGameEngineRuleQueriesOperations;
    private readonly ICoreRuleQueriesMatchCardTargetQueriesOperations _matchCardTargetQueriesOperations;
    private readonly ICoreRuleQueriesMatchDistanceQueriesOperations _matchDistanceQueriesOperations;
    private readonly ICoreRuleQueriesMatchHandLimitQueriesOperations _matchHandLimitQueriesOperations;
    private readonly ICoreRuleQueriesMatchTurnNumericQueriesOperations _matchTurnNumericQueriesOperations;
    private readonly ICoreRuleQueriesProgramNumericContributionOperations _programNumericContributionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseAccess _coreActualTurnProgressCurrentSeatPhaseAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCurrentTurnStatisticsSlashCountThisTurnAccess _coreCurrentTurnStatisticsSlashCountThisTurnAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineRuleQueriesRuntime(global::CardGame.Core.RuntimeEntries.CharacterDaQiaoXiaoQiaoHappyGodEntries characterDaQiaoXiaoQiaoHappyGodOperations, global::CardGame.Core.RuntimeEntries.CoreDirectedTurnCardPoliciesEntries coreDirectedTurnCardPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineActionQueriesEntries coreActionQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseModulesEntries coreCardUseModulesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDeckEquipmentProgramsEntries coreDeckEquipmentProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreMatchSetupDefinitionsEntries coreMatchSetupDefinitionsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardAppearanceEntries coreNativeCardAppearanceOperations, ICoreRuleQueriesCoreProgramTargetRulesOperations coreProgramTargetRulesOperations, global::CardGame.Core.EngineOwners.IKernelRuleQueryFactFactories kernelCoreGameEngineRuleQueriesOperations, ICoreRuleQueriesMatchCardTargetQueriesOperations matchCardTargetQueriesOperations, ICoreRuleQueriesMatchDistanceQueriesOperations matchDistanceQueriesOperations, ICoreRuleQueriesMatchHandLimitQueriesOperations matchHandLimitQueriesOperations, ICoreRuleQueriesMatchTurnNumericQueriesOperations matchTurnNumericQueriesOperations, ICoreRuleQueriesProgramNumericContributionOperations programNumericContributionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseAccess coreActualTurnProgressCurrentSeatPhaseAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCurrentTurnStatisticsSlashCountThisTurnAccess coreCurrentTurnStatisticsSlashCountThisTurnAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterDaQiaoXiaoQiaoHappyGodOperations = characterDaQiaoXiaoQiaoHappyGodOperations;
        this._coreDirectedTurnCardPoliciesOperations = coreDirectedTurnCardPoliciesOperations;
        this._coreActionQueriesOperations = coreActionQueriesOperations;
        this._coreCardUseModulesOperations = coreCardUseModulesOperations;
        this._coreDeckEquipmentProgramsOperations = coreDeckEquipmentProgramsOperations;
        this._coreMatchSetupDefinitionsOperations = coreMatchSetupDefinitionsOperations;
        this._coreNativeCardAppearanceOperations = coreNativeCardAppearanceOperations;
        this._coreProgramTargetRulesOperations = coreProgramTargetRulesOperations;
        this._kernelCoreGameEngineRuleQueriesOperations = kernelCoreGameEngineRuleQueriesOperations;
        this._matchCardTargetQueriesOperations = matchCardTargetQueriesOperations;
        this._matchDistanceQueriesOperations = matchDistanceQueriesOperations;
        this._matchHandLimitQueriesOperations = matchHandLimitQueriesOperations;
        this._matchTurnNumericQueriesOperations = matchTurnNumericQueriesOperations;
        this._programNumericContributionOperations = programNumericContributionOperations;
        this._coreActualTurnProgressCurrentSeatPhaseAccess = coreActualTurnProgressCurrentSeatPhaseAccess;
        this._coreCurrentTurnStatisticsSlashCountThisTurnAccess = coreCurrentTurnStatisticsSlashCountThisTurnAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
