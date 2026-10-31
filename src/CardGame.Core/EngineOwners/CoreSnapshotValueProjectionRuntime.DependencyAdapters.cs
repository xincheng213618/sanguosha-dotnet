#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.CardSnapshotProjector))]
internal sealed class CoreSnapshotValueProjectionCardSnapshotProjectorAdapter(global::System.Func<global::CardGame.Core.CardSnapshotProjector> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSnapshotValueProjectionCardSnapshotProjectorOperations
{
    private readonly global::System.Func<global::CardGame.Core.CardSnapshotProjector> _targetFactory = targetFactory;
    public global::CardGame.Core.CardSnapshotProjector Get() => this._targetFactory();
}
