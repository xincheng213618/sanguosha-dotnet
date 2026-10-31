#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramWindowReturnRuntime))]
internal sealed class CoreProgramWindowReturnsProgramWindowReturnAdapter(global::System.Func<global::CardGame.Core.ProgramWindowReturnRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramWindowReturnsProgramWindowReturnOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramWindowReturnRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramWindowReturnRuntime Get() => this._targetFactory();
}
