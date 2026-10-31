#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePlayerViewProjectionQueriesRuntime))]
internal sealed class CorePlayerViewProjectionCorePlayerViewProjectionQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayerViewProjectionQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePlayerViewProjectionCorePlayerViewProjectionQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayerViewProjectionQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.GameSnapshot ProjectPlayerView(int viewerSeat, bool revealAll) => this._targetFactory().ProjectPlayerView(viewerSeat, revealAll);
}
