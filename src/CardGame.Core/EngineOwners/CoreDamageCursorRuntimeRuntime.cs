#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreDamageCursorRuntimeRuntime
{
    private readonly ICoreDamageCursorRuntimeResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreDamageCursorRuntimeCoreProgramAttackAttemptOperations _coreProgramAttackAttemptOperations;
    internal CoreDamageCursorRuntimeRuntime(ICoreDamageCursorRuntimeResolutionStackOperations resolutionStackOperations, ICoreDamageCursorRuntimeCoreProgramAttackAttemptOperations coreProgramAttackAttemptOperations)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreProgramAttackAttemptOperations = coreProgramAttackAttemptOperations;
    }
}
