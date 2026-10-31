#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramRuntimeHooksRuntime))]
internal sealed class CoreDamagePreventionProgramsCoreProgramRuntimeHooksAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramRuntimeHooksRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamagePreventionProgramsCoreProgramRuntimeHooksOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramRuntimeHooksRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramRuntimeHooks RuntimeHooks { get => this._targetFactory().RuntimeHooks; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramWindowCursorRuntime))]
internal sealed class CoreDamagePreventionProgramsProgramWindowCursorAdapter(global::System.Func<global::CardGame.Core.ProgramWindowCursorRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamagePreventionProgramsProgramWindowCursorOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramWindowCursorRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramWindowCursorRuntime Get() => this._targetFactory();
}
