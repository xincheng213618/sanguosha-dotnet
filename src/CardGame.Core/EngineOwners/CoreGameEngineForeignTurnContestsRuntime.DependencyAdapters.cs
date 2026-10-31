#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ForeignActualTurnWindowRuntime))]
internal sealed class CoreForeignTurnContestsForeignActualTurnWindowAdapter(global::System.Func<global::CardGame.Core.ForeignActualTurnWindowRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreForeignTurnContestsForeignActualTurnWindowOperations
{
    private readonly global::System.Func<global::CardGame.Core.ForeignActualTurnWindowRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ForeignActualTurnWindowRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ForeignTurnContestRuntime))]
internal sealed class CoreForeignTurnContestsForeignTurnContestAdapter(global::System.Func<global::CardGame.Core.ForeignTurnContestRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreForeignTurnContestsForeignTurnContestOperations
{
    private readonly global::System.Func<global::CardGame.Core.ForeignTurnContestRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ForeignTurnContestRuntime Get() => this._targetFactory();
}
