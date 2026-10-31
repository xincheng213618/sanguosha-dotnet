#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.PlayerViewProjector))]
internal sealed class CorePlayerViewProjectionQueriesPlayerViewProjectorAdapter(global::System.Func<global::CardGame.Core.PlayerViewProjector> targetFactory) : global::CardGame.Core.EngineOwners.ICorePlayerViewProjectionQueriesPlayerViewProjectorOperations
{
    private readonly global::System.Func<global::CardGame.Core.PlayerViewProjector> _targetFactory = targetFactory;
    public global::CardGame.Core.PlayerViewProjector Get() => this._targetFactory();
}
