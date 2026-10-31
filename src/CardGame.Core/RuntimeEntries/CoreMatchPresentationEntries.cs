#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreMatchPresentationEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchPresentationRuntime> _resolve;
    private CoreMatchPresentationEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchPresentationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreMatchPresentationEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchPresentationRuntime> resolve) => new(resolve);
}
