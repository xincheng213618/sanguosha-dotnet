#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineActualTurnTrickUsesRuntime
{
    private readonly ICoreActualTurnTrickUsesCharacterLuSuMobileShiOperations _characterLuSuMobileShiOperations;
    private readonly ICoreActualTurnTrickUsesContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreActualTurnTrickUsesResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreActualTurnTrickUsesCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreActualTurnTrickUsesKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess _coreActualTurnProgressTurnNumberAccess;
    internal CoreGameEngineActualTurnTrickUsesRuntime(ICoreActualTurnTrickUsesCharacterLuSuMobileShiOperations characterLuSuMobileShiOperations, ICoreActualTurnTrickUsesContentRegistryOperations contentRegistryOperations, ICoreActualTurnTrickUsesResolutionStackOperations resolutionStackOperations, ICoreActualTurnTrickUsesCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreActualTurnTrickUsesKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess coreActualTurnProgressTurnNumberAccess)
    {
        this._characterLuSuMobileShiOperations = characterLuSuMobileShiOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressTurnNumberAccess = coreActualTurnProgressTurnNumberAccess;
    }
}
