#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreNativeEquipmentUseRuntime
{
    private readonly RuntimeEntries.CharacterHuCheErAnniversaryEntries _characterHuCheErAnniversaryOperations;
    private readonly RuntimeEntries.CharacterWeiYanHappyGodEntries _characterWeiYanHappyGodOperations;
    private readonly RuntimeEntries.CharacterXiangChongOlEntries _characterXiangChongOlOperations;
    private readonly RuntimeEntries.CoreGameEngineAdvancedWeaponsEntries _coreAdvancedWeaponsOperations;
    private readonly RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly ICoreNativeEquipmentUseCoreEquipmentContentOperations _coreEquipmentContentOperations;
    private readonly RuntimeEntries.CoreGameEngineFieldEquipmentEntries _coreFieldEquipmentOperations;
    private readonly RuntimeEntries.CoreGameEngineFinalizedCardTargetsEntries _coreFinalizedCardTargetsOperations;
    private readonly RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly RuntimeEntries.CoreNativeCardUseEntries _coreNativeCardUseOperations;
    private readonly RuntimeEntries.CoreNativeLegalActionsEntries _coreNativeLegalActionsOperations;
    private readonly RuntimeEntries.CoreNativeOwnedCardQueriesEntries _coreNativeOwnedCardQueriesOperations;
    private readonly RuntimeEntries.CorePhysicalMovementFacadesEntries _corePhysicalMovementFacadesOperations;
    private readonly RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly RuntimeEntries.CoreRecoveryReplacementEntries _coreRecoveryReplacementOperations;
    private readonly IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    internal CoreNativeEquipmentUseRuntime(RuntimeEntries.CharacterHuCheErAnniversaryEntries characterHuCheErAnniversaryOperations, RuntimeEntries.CharacterWeiYanHappyGodEntries characterWeiYanHappyGodOperations, RuntimeEntries.CharacterXiangChongOlEntries characterXiangChongOlOperations, RuntimeEntries.CoreGameEngineAdvancedWeaponsEntries coreAdvancedWeaponsOperations, RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations, ICoreNativeEquipmentUseCoreEquipmentContentOperations coreEquipmentContentOperations, RuntimeEntries.CoreGameEngineFieldEquipmentEntries coreFieldEquipmentOperations, RuntimeEntries.CoreGameEngineFinalizedCardTargetsEntries coreFinalizedCardTargetsOperations, RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations, RuntimeEntries.CoreNativeCardUseEntries coreNativeCardUseOperations, RuntimeEntries.CoreNativeLegalActionsEntries coreNativeLegalActionsOperations, RuntimeEntries.CoreNativeOwnedCardQueriesEntries coreNativeOwnedCardQueriesOperations, RuntimeEntries.CorePhysicalMovementFacadesEntries corePhysicalMovementFacadesOperations, RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations, RuntimeEntries.CoreRecoveryReplacementEntries coreRecoveryReplacementOperations, IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations)
    {
        this._characterHuCheErAnniversaryOperations = characterHuCheErAnniversaryOperations;
        this._characterWeiYanHappyGodOperations = characterWeiYanHappyGodOperations;
        this._characterXiangChongOlOperations = characterXiangChongOlOperations;
        this._coreAdvancedWeaponsOperations = coreAdvancedWeaponsOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreEquipmentContentOperations = coreEquipmentContentOperations;
        this._coreFieldEquipmentOperations = coreFieldEquipmentOperations;
        this._coreFinalizedCardTargetsOperations = coreFinalizedCardTargetsOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativeCardUseOperations = coreNativeCardUseOperations;
        this._coreNativeLegalActionsOperations = coreNativeLegalActionsOperations;
        this._coreNativeOwnedCardQueriesOperations = coreNativeOwnedCardQueriesOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreRecoveryReplacementOperations = coreRecoveryReplacementOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
    }
}
