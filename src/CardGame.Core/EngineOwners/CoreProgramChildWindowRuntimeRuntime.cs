#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreProgramChildWindowRuntimeRuntime : global::CardGame.Core.IProgramChildWindowEntries
{
    private readonly ICoreProgramChildWindowRuntimeCoreCharacterStateProgramsOperations _coreCharacterStateProgramsOperations;
    private readonly ICoreProgramChildWindowRuntimeCoreHpChangeProgramsOperations _coreHpChangeProgramsOperations;
    private readonly ICoreProgramChildWindowRuntimeCoreRecoveryReplacementOperations _coreRecoveryReplacementOperations;
    internal CoreProgramChildWindowRuntimeRuntime(ICoreProgramChildWindowRuntimeCoreCharacterStateProgramsOperations coreCharacterStateProgramsOperations, ICoreProgramChildWindowRuntimeCoreHpChangeProgramsOperations coreHpChangeProgramsOperations, ICoreProgramChildWindowRuntimeCoreRecoveryReplacementOperations coreRecoveryReplacementOperations)
    {
        this._coreCharacterStateProgramsOperations = coreCharacterStateProgramsOperations;
        this._coreHpChangeProgramsOperations = coreHpChangeProgramsOperations;
        this._coreRecoveryReplacementOperations = coreRecoveryReplacementOperations;
    }
}
