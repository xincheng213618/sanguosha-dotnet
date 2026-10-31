#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineHellEncounterHostRuntime
{
    internal IHellEncounterHost CreateHellEncounterHost() => _kernelCoreGameEngineHellEncounterHostOperations.CreateHellEncounterHost();
    internal IHellEncounterFeature? HellFeature => _modeRuntimeOperations._modeRuntime.Find<IHellEncounterFeature>();
}
