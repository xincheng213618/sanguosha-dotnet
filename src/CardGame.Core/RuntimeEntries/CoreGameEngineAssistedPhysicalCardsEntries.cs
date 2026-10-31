#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime))]
internal sealed partial class CoreGameEngineAssistedPhysicalCardsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime> _resolve;
    private CoreGameEngineAssistedPhysicalCardsEntries(Func<EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAssistedPhysicalCardsEntries Create(Func<EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime> resolve) => new(resolve);
}
