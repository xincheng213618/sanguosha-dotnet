#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineDirectedCardPolicyQueriesRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterFanJiangZhangDaOlEntries _characterFanJiangZhangDaOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterGaoShunBoundaryMobileEntries _characterGaoShunBoundaryMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuaXiongMouMobileEntries _characterHuaXiongMouMobileOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreDirectedTurnCardPoliciesEntries _coreDirectedTurnCardPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseModulesEntries _coreCardUseModulesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineExtendedViewAsEntries _coreExtendedViewAsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineFinalTargetSlashEntries _coreFinalTargetSlashOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineJudgedRankSlashPoliciesEntries _coreJudgedRankSlashPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineOriginalTargetAdditionsEntries _coreOriginalTargetAdditionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries _coreProgramCardPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuleQueriesEntries _coreRuleQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeEquipmentAndFactionQueriesEntries _coreNativeEquipmentAndFactionQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreProgramAllHandBonusOperationEntries _coreProgramAllHandBonusOperationOperations;
    private readonly ICoreDirectedCardPolicyQueriesCoreProgramDistanceRulesOperations _coreProgramDistanceRulesOperations;
    private readonly ICoreDirectedCardPolicyQueriesCoreProgramTargetRulesOperations _coreProgramTargetRulesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineDirectedCardPolicyQueriesRuntime(global::CardGame.Core.RuntimeEntries.CharacterFanJiangZhangDaOlEntries characterFanJiangZhangDaOlOperations, global::CardGame.Core.RuntimeEntries.CharacterGaoShunBoundaryMobileEntries characterGaoShunBoundaryMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterHuaXiongMouMobileEntries characterHuaXiongMouMobileOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreDirectedTurnCardPoliciesEntries coreDirectedTurnCardPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseModulesEntries coreCardUseModulesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineExtendedViewAsEntries coreExtendedViewAsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineFinalTargetSlashEntries coreFinalTargetSlashOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineJudgedRankSlashPoliciesEntries coreJudgedRankSlashPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineOriginalTargetAdditionsEntries coreOriginalTargetAdditionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries coreProgramCardPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRuleQueriesEntries coreRuleQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreNativeEquipmentAndFactionQueriesEntries coreNativeEquipmentAndFactionQueriesOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreProgramAllHandBonusOperationEntries coreProgramAllHandBonusOperationOperations, ICoreDirectedCardPolicyQueriesCoreProgramDistanceRulesOperations coreProgramDistanceRulesOperations, ICoreDirectedCardPolicyQueriesCoreProgramTargetRulesOperations coreProgramTargetRulesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterFanJiangZhangDaOlOperations = characterFanJiangZhangDaOlOperations;
        this._characterGaoShunBoundaryMobileOperations = characterGaoShunBoundaryMobileOperations;
        this._characterHuaXiongMouMobileOperations = characterHuaXiongMouMobileOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDirectedTurnCardPoliciesOperations = coreDirectedTurnCardPoliciesOperations;
        this._coreCardUseModulesOperations = coreCardUseModulesOperations;
        this._coreExtendedViewAsOperations = coreExtendedViewAsOperations;
        this._coreFinalTargetSlashOperations = coreFinalTargetSlashOperations;
        this._coreJudgedRankSlashPoliciesOperations = coreJudgedRankSlashPoliciesOperations;
        this._coreOriginalTargetAdditionsOperations = coreOriginalTargetAdditionsOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreRuleQueriesOperations = coreRuleQueriesOperations;
        this._coreNativeEquipmentAndFactionQueriesOperations = coreNativeEquipmentAndFactionQueriesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreProgramAllHandBonusOperationOperations = coreProgramAllHandBonusOperationOperations;
        this._coreProgramDistanceRulesOperations = coreProgramDistanceRulesOperations;
        this._coreProgramTargetRulesOperations = coreProgramTargetRulesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
