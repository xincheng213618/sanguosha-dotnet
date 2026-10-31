#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreSnapshotValueProjectionRuntime
{
    private readonly ICoreSnapshotValueProjectionCardSnapshotProjectorOperations _cardSnapshotProjectorOperations;
    internal CoreSnapshotValueProjectionRuntime(ICoreSnapshotValueProjectionCardSnapshotProjectorOperations cardSnapshotProjectorOperations)
    {
        this._cardSnapshotProjectorOperations = cardSnapshotProjectorOperations;
    }
}
