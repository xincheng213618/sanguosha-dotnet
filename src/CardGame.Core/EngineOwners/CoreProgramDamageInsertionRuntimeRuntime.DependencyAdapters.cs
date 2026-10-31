#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramDamageInsertionRuntime))]
internal sealed class CoreProgramDamageInsertionRuntimeProgramDamageInsertionAdapter(global::System.Func<global::CardGame.Core.ProgramDamageInsertionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDamageInsertionRuntimeProgramDamageInsertionOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramDamageInsertionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramDamageInsertionRuntime Get() => this._targetFactory();
}
