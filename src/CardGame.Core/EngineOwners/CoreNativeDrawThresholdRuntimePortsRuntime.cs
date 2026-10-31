#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreNativeDrawThresholdRuntimePortsRuntime : global::CardGame.Core.INativeDrawThresholdNativeOperations
{
    private readonly ICoreNativeDrawThresholdRuntimePortsCharacterWeiYanMobileShiOperations _characterWeiYanMobileShiOperations;
    private readonly ICoreNativeDrawThresholdRuntimePortsCoreSkillProgramsOperations _coreSkillProgramsOperations;
    internal CoreNativeDrawThresholdRuntimePortsRuntime(ICoreNativeDrawThresholdRuntimePortsCharacterWeiYanMobileShiOperations characterWeiYanMobileShiOperations, ICoreNativeDrawThresholdRuntimePortsCoreSkillProgramsOperations coreSkillProgramsOperations)
    {
        this._characterWeiYanMobileShiOperations = characterWeiYanMobileShiOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
    }
}
