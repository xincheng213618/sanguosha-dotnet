#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineModeEncountersRuntime
{
    internal IModeEncounterHost CreateModeEncounterHost() => _kernelCoreGameEngineModeEncountersOperations.CreateModeEncounterHost();
}
