#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineDirectedCardPolicyQueriesRuntime
{
    private readonly RuntimeEntries.CharacterFanJiangZhangDaOlEntries _characterFanJiangZhangDaOlOperations;
    private readonly RuntimeEntries.CharacterGaoShunBoundaryMobileEntries _characterGaoShunBoundaryMobileOperations;
    private readonly RuntimeEntries.CharacterHuaXiongMouMobileEntries _characterHuaXiongMouMobileOperations;
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly RuntimeEntries.CoreDirectedTurnCardPoliciesEntries _coreDirectedTurnCardPoliciesOperations;
    private readonly RuntimeEntries.CoreGameEngineCardUseModulesEntries _coreCardUseModulesOperations;
    private readonly RuntimeEntries.CoreGameEngineExtendedViewAsEntries _coreExtendedViewAsOperations;
    private readonly RuntimeEntries.CoreGameEngineFinalTargetSlashEntries _coreFinalTargetSlashOperations;
    private readonly RuntimeEntries.CoreGameEngineJudgedRankSlashPoliciesEntries _coreJudgedRankSlashPoliciesOperations;
    private readonly RuntimeEntries.CoreGameEngineOriginalTargetAdditionsEntries _coreOriginalTargetAdditionsOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries _coreProgramCardPoliciesOperations;
    private readonly RuntimeEntries.CoreGameEngineRuleQueriesEntries _coreRuleQueriesOperations;
    private readonly RuntimeEntries.CoreNativeEquipmentAndFactionQueriesEntries _coreNativeEquipmentAndFactionQueriesOperations;
    private readonly RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly RuntimeEntries.CoreProgramAllHandBonusOperationEntries _coreProgramAllHandBonusOperationOperations;
    private readonly ICoreDirectedCardPolicyQueriesCoreProgramDistanceRulesOperations _coreProgramDistanceRulesOperations;
    private readonly ICoreDirectedCardPolicyQueriesCoreProgramTargetRulesOperations _coreProgramTargetRulesOperations;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineDirectedCardPolicyQueriesRuntime(RuntimeEntries.CharacterFanJiangZhangDaOlEntries characterFanJiangZhangDaOlOperations, RuntimeEntries.CharacterGaoShunBoundaryMobileEntries characterGaoShunBoundaryMobileOperations, RuntimeEntries.CharacterHuaXiongMouMobileEntries characterHuaXiongMouMobileOperations, IKernelContentRegistryAccess contentRegistryOperations, IKernelFrameStoreAccess resolutionStackOperations, RuntimeEntries.CoreDirectedTurnCardPoliciesEntries coreDirectedTurnCardPoliciesOperations, RuntimeEntries.CoreGameEngineCardUseModulesEntries coreCardUseModulesOperations, RuntimeEntries.CoreGameEngineExtendedViewAsEntries coreExtendedViewAsOperations, RuntimeEntries.CoreGameEngineFinalTargetSlashEntries coreFinalTargetSlashOperations, RuntimeEntries.CoreGameEngineJudgedRankSlashPoliciesEntries coreJudgedRankSlashPoliciesOperations, RuntimeEntries.CoreGameEngineOriginalTargetAdditionsEntries coreOriginalTargetAdditionsOperations, RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries coreProgramCardPoliciesOperations, RuntimeEntries.CoreGameEngineRuleQueriesEntries coreRuleQueriesOperations, RuntimeEntries.CoreNativeEquipmentAndFactionQueriesEntries coreNativeEquipmentAndFactionQueriesOperations, RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations, RuntimeEntries.CoreProgramAllHandBonusOperationEntries coreProgramAllHandBonusOperationOperations, ICoreDirectedCardPolicyQueriesCoreProgramDistanceRulesOperations coreProgramDistanceRulesOperations, ICoreDirectedCardPolicyQueriesCoreProgramTargetRulesOperations coreProgramTargetRulesOperations, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
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
