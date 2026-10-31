#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ForeignTurnContestRuntime))]
internal sealed class CoreForeignContestAidObserversForeignTurnContestAdapter(global::System.Func<global::CardGame.Core.ForeignTurnContestRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreForeignContestAidObserversForeignTurnContestOperations
{
    private readonly global::System.Func<global::CardGame.Core.ForeignTurnContestRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ForeignTurnContestRuntime Get() => this._targetFactory();
}
