#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreProgramDamageInsertionRuntimeRuntime : global::CardGame.Core.IProgramDamageInsertionFacts
{
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries _coreDyingCursorRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries _coreProgramAttackAttemptOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramChildResolutionEntries _coreProgramChildResolutionOperations;
    private readonly ICoreProgramDamageInsertionRuntimeProgramDamageInsertionOperations _programDamageInsertionOperations;
    internal CoreProgramDamageInsertionRuntimeRuntime(global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries coreDyingCursorRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries coreProgramAttackAttemptOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramChildResolutionEntries coreProgramChildResolutionOperations, ICoreProgramDamageInsertionRuntimeProgramDamageInsertionOperations programDamageInsertionOperations)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreProgramAttackAttemptOperations = coreProgramAttackAttemptOperations;
        this._coreProgramChildResolutionOperations = coreProgramChildResolutionOperations;
        this._programDamageInsertionOperations = programDamageInsertionOperations;
    }
}
