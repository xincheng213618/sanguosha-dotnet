#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineHellEncounterHostRuntime
{
    internal IHellEncounterHost CreateHellEncounterHost() => _kernelCoreGameEngineHellEncounterHostOperations.CreateHellEncounterHost();
    internal IHellEncounterFeature? HellFeature => _modeRuntimeOperations._modeRuntime.Find<IHellEncounterFeature>();
}
