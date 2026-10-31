#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShenwuCardsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShenwuCardsRuntime> _resolve;
    private CoreGameEngineShenwuCardsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShenwuCardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShenwuCardsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShenwuCardsRuntime> resolve) => new(resolve);
}
