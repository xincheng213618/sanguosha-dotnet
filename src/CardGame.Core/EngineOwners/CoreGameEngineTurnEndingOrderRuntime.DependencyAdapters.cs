#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramPhaseBoundaryRuntime))]
internal sealed class CoreTurnEndingOrderProgramPhaseBoundaryAdapter(global::System.Func<global::CardGame.Core.ProgramPhaseBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTurnEndingOrderProgramPhaseBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramPhaseBoundaryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramPhaseBoundaryRuntime Get() => this._targetFactory();
}
