#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramChildWindowRuntime))]
internal sealed class CoreProgramContinuationsProgramChildWindowAdapter(global::System.Func<global::CardGame.Core.ProgramChildWindowRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramContinuationsProgramChildWindowOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramChildWindowRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramChildWindowRuntime Get() => this._targetFactory();
}
