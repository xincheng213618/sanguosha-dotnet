#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramRandomSeatAcquisitionRuntime))]
internal sealed class CoreProgramRandomSeatAcquisitionRuntimeProgramRandomSeatAcquisitionAdapter(global::System.Func<global::CardGame.Core.ProgramRandomSeatAcquisitionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramRandomSeatAcquisitionRuntimeProgramRandomSeatAcquisitionOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramRandomSeatAcquisitionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramRandomSeatAcquisitionRuntime Get() => this._targetFactory();
}
