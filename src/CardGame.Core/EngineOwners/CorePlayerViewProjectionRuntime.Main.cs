#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CorePlayerViewProjectionRuntime
{
    internal GameSnapshot State => CreateSnapshot(_coreMatchDefinitionOptionsAccess._options.HumanSeat);

    internal GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => _corePlayerViewProjectionQueriesOperations.Value.ProjectPlayerView(viewerSeat, revealAll);
    internal string SerializeState(bool revealAll = false) => SnapshotJson.Serialize(CreateSnapshot(_coreMatchDefinitionOptionsAccess._options.HumanSeat, revealAll));
}
