#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramPhaseSchedulingRuntime))]
internal sealed class CoreActualPhaseEndRuntimePortsProgramPhaseSchedulingAdapter(global::System.Func<global::CardGame.Core.ProgramPhaseSchedulingRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualPhaseEndRuntimePortsProgramPhaseSchedulingOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramPhaseSchedulingRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramPhaseSchedulingRuntime Get() => this._targetFactory();
}
