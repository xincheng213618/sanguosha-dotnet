#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CorePindianOwnedPileSourcesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePindianOwnedPileSourcesRuntime> _resolve;
    private CorePindianOwnedPileSourcesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CorePindianOwnedPileSourcesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CorePindianOwnedPileSourcesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CorePindianOwnedPileSourcesRuntime> resolve) => new(resolve);
}
