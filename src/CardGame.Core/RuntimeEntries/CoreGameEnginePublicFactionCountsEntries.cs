#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePublicFactionCountsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicFactionCountsRuntime> _resolve;
    private CoreGameEnginePublicFactionCountsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicFactionCountsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePublicFactionCountsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicFactionCountsRuntime> resolve) => new(resolve);
}
