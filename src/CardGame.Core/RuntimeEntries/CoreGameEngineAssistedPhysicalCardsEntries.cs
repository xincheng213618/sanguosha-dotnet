#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAssistedPhysicalCardsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime> _resolve;
    private CoreGameEngineAssistedPhysicalCardsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAssistedPhysicalCardsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime> resolve) => new(resolve);
}
