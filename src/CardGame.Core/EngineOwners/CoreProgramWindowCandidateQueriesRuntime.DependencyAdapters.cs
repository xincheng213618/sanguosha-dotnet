#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramTriggerRuntime))]
internal sealed class CoreProgramWindowCandidateQueriesProgramTriggerAdapter(global::System.Func<global::CardGame.Core.ProgramTriggerRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramWindowCandidateQueriesProgramTriggerOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramTriggerRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramTriggerRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramTriggerFactProjector))]
internal sealed class CoreProgramWindowCandidateQueriesProgramTriggerFactProjectorAdapter(global::System.Func<global::CardGame.Core.ProgramTriggerFactProjector> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramWindowCandidateQueriesProgramTriggerFactProjectorOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramTriggerFactProjector> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramTriggerFactProjector Get() => this._targetFactory();
}
