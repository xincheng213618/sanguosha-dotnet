#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOriginalHandAwakeningEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalHandAwakeningRuntime> _resolve;
    private CoreGameEngineOriginalHandAwakeningEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalHandAwakeningRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOriginalHandAwakeningEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalHandAwakeningRuntime> resolve) => new(resolve);
}
