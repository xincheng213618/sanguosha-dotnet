#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSourceBeigongEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceBeigongRuntime> _resolve;
    private CoreGameEngineSourceBeigongEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceBeigongRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSourceBeigongEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceBeigongRuntime> resolve) => new(resolve);
}
