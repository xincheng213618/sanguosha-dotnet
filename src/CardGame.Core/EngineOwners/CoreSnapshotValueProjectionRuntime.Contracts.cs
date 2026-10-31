#nullable enable

namespace CardGame.Core.EngineOwners;
internal interface ICoreSnapshotValueProjectionCardSnapshotProjectorOperations : IProgramRuntimeOperations
{
    CardSnapshotProjector Get();
}
