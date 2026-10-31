#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime))]
internal sealed class CoreTableSeatQueriesCoreTableOrderAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTableSeatQueriesCoreTableOrderOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime> _targetFactory = targetFactory;
    public int TableStepSeat(int from, int offset) => this._targetFactory().TableStepSeat(from, offset);
}
