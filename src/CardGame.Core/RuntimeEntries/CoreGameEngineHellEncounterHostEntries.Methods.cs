#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHellEncounterHostEntries
{
    internal IHellEncounterFeature? HellFeature { get => _resolve().HellFeature; }
}
