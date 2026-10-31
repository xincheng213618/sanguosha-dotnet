#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreSnapshotValueProjectionRuntime
{
    private readonly ICoreSnapshotValueProjectionCardSnapshotProjectorOperations _cardSnapshotProjectorOperations;
    internal CoreSnapshotValueProjectionRuntime(ICoreSnapshotValueProjectionCardSnapshotProjectorOperations cardSnapshotProjectorOperations)
    {
        this._cardSnapshotProjectorOperations = cardSnapshotProjectorOperations;
    }
}
