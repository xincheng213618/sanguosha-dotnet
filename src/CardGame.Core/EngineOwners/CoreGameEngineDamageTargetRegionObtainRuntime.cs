#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineDamageTargetRegionObtainRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterFanJiangZhangDaOlEntries _characterFanJiangZhangDaOlOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries _coreAdvancedLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardMovementProgramsEntries _coreCardMovementProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageTargetObtainProgramsEntries _coreDamageTargetObtainProgramsOperations;
    private readonly ICoreDamageTargetRegionObtainCoreEquipmentContentOperations _coreEquipmentContentOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries _coreProgramAtomicCardMovesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheS5ShuangxiongEntries _coreShanheS5ShuangxiongOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreHappyWeiYanGainDestinationsEntries _coreHappyWeiYanGainDestinationsOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess _coreCardMovementJournalCardMovementsMovementSequenceAccess;
    internal CoreGameEngineDamageTargetRegionObtainRuntime(global::CardGame.Core.RuntimeEntries.CharacterFanJiangZhangDaOlEntries characterFanJiangZhangDaOlOperations,global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations,global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations,global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries coreAdvancedLifecycleOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCardMovementProgramsEntries coreCardMovementProgramsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageTargetObtainProgramsEntries coreDamageTargetObtainProgramsOperations,ICoreDamageTargetRegionObtainCoreEquipmentContentOperations coreEquipmentContentOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries coreProgramAtomicCardMovesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheS5ShuangxiongEntries coreShanheS5ShuangxiongOperations,global::CardGame.Core.RuntimeEntries.CoreHappyWeiYanGainDestinationsEntries coreHappyWeiYanGainDestinationsOperations,global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations,global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess coreCardMovementJournalCardMovementsMovementSequenceAccess)
    {
        this._characterFanJiangZhangDaOlOperations = characterFanJiangZhangDaOlOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAdvancedLifecycleOperations = coreAdvancedLifecycleOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreDamageTargetObtainProgramsOperations = coreDamageTargetObtainProgramsOperations;
        this._coreEquipmentContentOperations = coreEquipmentContentOperations;
        this._coreProgramAtomicCardMovesOperations = coreProgramAtomicCardMovesOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreShanheS5ShuangxiongOperations = coreShanheS5ShuangxiongOperations;
        this._coreHappyWeiYanGainDestinationsOperations = coreHappyWeiYanGainDestinationsOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreCardMovementJournalCardMovementsMovementSequenceAccess = coreCardMovementJournalCardMovementsMovementSequenceAccess;
    }
}
