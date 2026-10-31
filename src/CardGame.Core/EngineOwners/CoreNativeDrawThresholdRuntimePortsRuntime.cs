#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreNativeDrawThresholdRuntimePortsRuntime : global::CardGame.Core.INativeDrawThresholdNativeOperations
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterWeiYanMobileShiEntries _characterWeiYanMobileShiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries _coreSkillProgramsOperations;
    internal CoreNativeDrawThresholdRuntimePortsRuntime(global::CardGame.Core.RuntimeEntries.CharacterWeiYanMobileShiEntries characterWeiYanMobileShiOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries coreSkillProgramsOperations)
    {
        this._characterWeiYanMobileShiOperations = characterWeiYanMobileShiOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
    }
}
