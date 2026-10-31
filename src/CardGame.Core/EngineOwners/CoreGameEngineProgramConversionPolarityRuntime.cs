#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineProgramConversionPolarityRuntime : global::CardGame.Core.IConversionPolarityProgramHost
{
    private readonly ICoreProgramConversionPolarityResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreProgramConversionPolaritySkillRuntimeStateOperations _skillRuntimeStateOperations;
    private readonly ICoreProgramConversionPolarityCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreProgramConversionPolarityCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreProgramConversionPolarityKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess _coreActualTurnProgressTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess;
    internal CoreGameEngineProgramConversionPolarityRuntime(ICoreProgramConversionPolarityResolutionStackOperations resolutionStackOperations, ICoreProgramConversionPolaritySkillRuntimeStateOperations skillRuntimeStateOperations, ICoreProgramConversionPolarityCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreProgramConversionPolarityCoreRuntimeOperations coreRuntimeOperations, ICoreProgramConversionPolarityKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess coreActualTurnProgressTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._skillRuntimeStateOperations = skillRuntimeStateOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressTurnNumberAccess = coreActualTurnProgressTurnNumberAccess;
        this._coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess = coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess;
    }
}
