#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineDrawFundedDistinctBasicReturnsRuntime
{
    private readonly RuntimeEntries.CharacterCaoChunOlEntries _characterCaoChunOlOperations;
    private readonly RuntimeEntries.CharacterLiYiOlEntries _characterLiYiOlOperations;
    private readonly RuntimeEntries.CharacterLvKuangLvXiangOlEntries _characterLvKuangLvXiangOlOperations;
    private readonly RuntimeEntries.CharacterSunYuanOlEntries _characterSunYuanOlOperations;
    private readonly RuntimeEntries.CharacterWuJingOlEntries _characterWuJingOlOperations;
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly RuntimeEntries.CoreDyingCursorRuntimeEntries _coreDyingCursorRuntimeOperations;
    private readonly RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicObserversEntries _coreDrawFundedDistinctBasicObserversOperations;
    private readonly RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicPaymentEntries _coreDrawFundedDistinctBasicPaymentOperations;
    private readonly RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicUsesEntries _coreDrawFundedDistinctBasicUsesOperations;
    private readonly RuntimeEntries.CoreGameEngineTableOrderEntries _coreTableOrderOperations;
    private readonly RuntimeEntries.CoreGameEngineTieredRoundZeroReturnsEntries _coreTieredRoundZeroReturnsOperations;
    private readonly RuntimeEntries.CoreParticipantHandDamageRidesEntries _coreParticipantHandDamageRidesOperations;
    private readonly IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly EngineStates.Contracts.ICoreMatchDefinitionPlayerCountAccess _coreMatchDefinitionPlayerCountAccess;
    internal CoreGameEngineDrawFundedDistinctBasicReturnsRuntime(RuntimeEntries.CharacterCaoChunOlEntries characterCaoChunOlOperations,RuntimeEntries.CharacterLiYiOlEntries characterLiYiOlOperations,RuntimeEntries.CharacterLvKuangLvXiangOlEntries characterLvKuangLvXiangOlOperations,RuntimeEntries.CharacterSunYuanOlEntries characterSunYuanOlOperations,RuntimeEntries.CharacterWuJingOlEntries characterWuJingOlOperations,IKernelContentRegistryAccess contentRegistryOperations,IKernelFrameStoreAccess resolutionStackOperations,RuntimeEntries.CoreDyingCursorRuntimeEntries coreDyingCursorRuntimeOperations,RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations,RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations,RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicObserversEntries coreDrawFundedDistinctBasicObserversOperations,RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicPaymentEntries coreDrawFundedDistinctBasicPaymentOperations,RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicUsesEntries coreDrawFundedDistinctBasicUsesOperations,RuntimeEntries.CoreGameEngineTableOrderEntries coreTableOrderOperations,RuntimeEntries.CoreGameEngineTieredRoundZeroReturnsEntries coreTieredRoundZeroReturnsOperations,RuntimeEntries.CoreParticipantHandDamageRidesEntries coreParticipantHandDamageRidesOperations,IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations,EngineStates.Contracts.ICoreMatchDefinitionPlayerCountAccess coreMatchDefinitionPlayerCountAccess)
    {
        this._characterCaoChunOlOperations = characterCaoChunOlOperations;
        this._characterLiYiOlOperations = characterLiYiOlOperations;
        this._characterLvKuangLvXiangOlOperations = characterLvKuangLvXiangOlOperations;
        this._characterSunYuanOlOperations = characterSunYuanOlOperations;
        this._characterWuJingOlOperations = characterWuJingOlOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreDrawFundedDistinctBasicObserversOperations = coreDrawFundedDistinctBasicObserversOperations;
        this._coreDrawFundedDistinctBasicPaymentOperations = coreDrawFundedDistinctBasicPaymentOperations;
        this._coreDrawFundedDistinctBasicUsesOperations = coreDrawFundedDistinctBasicUsesOperations;
        this._coreTableOrderOperations = coreTableOrderOperations;
        this._coreTieredRoundZeroReturnsOperations = coreTieredRoundZeroReturnsOperations;
        this._coreParticipantHandDamageRidesOperations = coreParticipantHandDamageRidesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchDefinitionPlayerCountAccess = coreMatchDefinitionPlayerCountAccess;
    }
}
