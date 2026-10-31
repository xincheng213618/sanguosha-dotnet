#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramCapability(typeof(CardSnapshotProjector))]
internal sealed class CoreSnapshotValueProjectionCardSnapshotProjectorAdapter(Func<CardSnapshotProjector> targetFactory) : ICoreSnapshotValueProjectionCardSnapshotProjectorOperations
{
    private readonly Func<CardSnapshotProjector> _targetFactory = targetFactory;
    public CardSnapshotProjector Get() => this._targetFactory();
}
