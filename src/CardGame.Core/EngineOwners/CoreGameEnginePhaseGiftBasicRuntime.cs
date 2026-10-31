#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEnginePhaseGiftBasicRuntime : global::CardGame.Core.IPhaseGiftBasicProgramHost
{
    private readonly ICorePhaseGiftBasicCardZonesOperations _cardZonesOperations;
    private readonly ICorePhaseGiftBasicSkillRuntimeStateOperations _skillRuntimeStateOperations;
    private readonly ICorePhaseGiftBasicCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICorePhaseGiftBasicCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICorePhaseGiftBasicCoreVirtualBasicOffersOperations _coreVirtualBasicOffersOperations;
    private readonly ICorePhaseGiftBasicCoreHappyWeiYanGainDestinationsOperations _coreHappyWeiYanGainDestinationsOperations;
    private readonly ICorePhaseGiftBasicKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess;
    internal CoreGameEnginePhaseGiftBasicRuntime(ICorePhaseGiftBasicCardZonesOperations cardZonesOperations, ICorePhaseGiftBasicSkillRuntimeStateOperations skillRuntimeStateOperations, ICorePhaseGiftBasicCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICorePhaseGiftBasicCoreRuntimeOperations coreRuntimeOperations, ICorePhaseGiftBasicCoreVirtualBasicOffersOperations coreVirtualBasicOffersOperations, ICorePhaseGiftBasicCoreHappyWeiYanGainDestinationsOperations coreHappyWeiYanGainDestinationsOperations, ICorePhaseGiftBasicKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._skillRuntimeStateOperations = skillRuntimeStateOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreVirtualBasicOffersOperations = coreVirtualBasicOffersOperations;
        this._coreHappyWeiYanGainDestinationsOperations = coreHappyWeiYanGainDestinationsOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess = coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess;
    }
}
