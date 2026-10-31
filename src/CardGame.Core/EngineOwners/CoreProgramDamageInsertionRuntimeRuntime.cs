#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreProgramDamageInsertionRuntimeRuntime : global::CardGame.Core.IProgramDamageInsertionFacts
{
    private readonly ICoreProgramDamageInsertionRuntimeResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreProgramDamageInsertionRuntimeCoreDyingCursorRuntimeOperations _coreDyingCursorRuntimeOperations;
    private readonly ICoreProgramDamageInsertionRuntimeCoreProgramAttackAttemptOperations _coreProgramAttackAttemptOperations;
    private readonly ICoreProgramDamageInsertionRuntimeCoreProgramChildResolutionOperations _coreProgramChildResolutionOperations;
    private readonly ICoreProgramDamageInsertionRuntimeProgramDamageInsertionOperations _programDamageInsertionOperations;
    internal CoreProgramDamageInsertionRuntimeRuntime(ICoreProgramDamageInsertionRuntimeResolutionStackOperations resolutionStackOperations, ICoreProgramDamageInsertionRuntimeCoreDyingCursorRuntimeOperations coreDyingCursorRuntimeOperations, ICoreProgramDamageInsertionRuntimeCoreProgramAttackAttemptOperations coreProgramAttackAttemptOperations, ICoreProgramDamageInsertionRuntimeCoreProgramChildResolutionOperations coreProgramChildResolutionOperations, ICoreProgramDamageInsertionRuntimeProgramDamageInsertionOperations programDamageInsertionOperations)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreProgramAttackAttemptOperations = coreProgramAttackAttemptOperations;
        this._coreProgramChildResolutionOperations = coreProgramChildResolutionOperations;
        this._programDamageInsertionOperations = programDamageInsertionOperations;
    }
}
