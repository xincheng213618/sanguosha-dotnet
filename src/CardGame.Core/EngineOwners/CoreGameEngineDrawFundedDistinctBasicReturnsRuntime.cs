#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineDrawFundedDistinctBasicReturnsRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCaoChunOlEntries _characterCaoChunOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLiYiOlEntries _characterLiYiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLvKuangLvXiangOlEntries _characterLvKuangLvXiangOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterSunYuanOlEntries _characterSunYuanOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterWuJingOlEntries _characterWuJingOlOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries _coreDyingCursorRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicObserversEntries _coreDrawFundedDistinctBasicObserversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicPaymentEntries _coreDrawFundedDistinctBasicPaymentOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicUsesEntries _coreDrawFundedDistinctBasicUsesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTableOrderEntries _coreTableOrderOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTieredRoundZeroReturnsEntries _coreTieredRoundZeroReturnsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreParticipantHandDamageRidesEntries _coreParticipantHandDamageRidesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionPlayerCountAccess _coreMatchDefinitionPlayerCountAccess;
    internal CoreGameEngineDrawFundedDistinctBasicReturnsRuntime(global::CardGame.Core.RuntimeEntries.CharacterCaoChunOlEntries characterCaoChunOlOperations,global::CardGame.Core.RuntimeEntries.CharacterLiYiOlEntries characterLiYiOlOperations,global::CardGame.Core.RuntimeEntries.CharacterLvKuangLvXiangOlEntries characterLvKuangLvXiangOlOperations,global::CardGame.Core.RuntimeEntries.CharacterSunYuanOlEntries characterSunYuanOlOperations,global::CardGame.Core.RuntimeEntries.CharacterWuJingOlEntries characterWuJingOlOperations,global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations,global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations,global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries coreDyingCursorRuntimeOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicObserversEntries coreDrawFundedDistinctBasicObserversOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicPaymentEntries coreDrawFundedDistinctBasicPaymentOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicUsesEntries coreDrawFundedDistinctBasicUsesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineTableOrderEntries coreTableOrderOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineTieredRoundZeroReturnsEntries coreTieredRoundZeroReturnsOperations,global::CardGame.Core.RuntimeEntries.CoreParticipantHandDamageRidesEntries coreParticipantHandDamageRidesOperations,global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations,global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionPlayerCountAccess coreMatchDefinitionPlayerCountAccess)
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
