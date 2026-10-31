#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CorePlayerViewProjectionRuntime
{
    internal GameSnapshot State => CreateSnapshot(_coreMatchDefinitionOptionsAccess._options.HumanSeat);

    internal GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => _corePlayerViewProjectionQueriesOperations.ProjectPlayerView(viewerSeat, revealAll);
    internal string SerializeState(bool revealAll = false) => SnapshotJson.Serialize(CreateSnapshot(_coreMatchDefinitionOptionsAccess._options.HumanSeat, revealAll));
}
