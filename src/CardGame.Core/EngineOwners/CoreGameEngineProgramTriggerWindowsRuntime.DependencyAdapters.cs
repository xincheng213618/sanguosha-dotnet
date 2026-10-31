#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramPhaseBoundaryRuntime))]
internal sealed class CoreProgramTriggerWindowsProgramPhaseBoundaryAdapter(global::System.Func<global::CardGame.Core.ProgramPhaseBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTriggerWindowsProgramPhaseBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramPhaseBoundaryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramPhaseBoundaryRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramLifecycleWindowRuntime))]
internal sealed class CoreProgramTriggerWindowsProgramLifecycleWindowAdapter(global::System.Func<global::CardGame.Core.ProgramLifecycleWindowRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTriggerWindowsProgramLifecycleWindowOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramLifecycleWindowRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramLifecycleWindowRuntime Get() => this._targetFactory();
}
