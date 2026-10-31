#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramLifeRestorationRuntime
{
    private readonly ICoreProgramLifeRestorationCoreCharacterStateProgramsOperations _coreCharacterStateProgramsOperations;
    private readonly ICoreProgramLifeRestorationCoreCharacterTurnedOverProgramsOperations _coreCharacterTurnedOverProgramsOperations;
    private readonly ICoreProgramLifeRestorationKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    internal CoreGameEngineProgramLifeRestorationRuntime(ICoreProgramLifeRestorationCoreCharacterStateProgramsOperations coreCharacterStateProgramsOperations, ICoreProgramLifeRestorationCoreCharacterTurnedOverProgramsOperations coreCharacterTurnedOverProgramsOperations, ICoreProgramLifeRestorationKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations)
    {
        this._coreCharacterStateProgramsOperations = coreCharacterStateProgramsOperations;
        this._coreCharacterTurnedOverProgramsOperations = coreCharacterTurnedOverProgramsOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
    }
}
