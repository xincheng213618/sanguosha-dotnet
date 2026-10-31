#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOriginalHandEntitiesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalHandEntitiesRuntime> _resolve;
    private CoreGameEngineOriginalHandEntitiesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalHandEntitiesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOriginalHandEntitiesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalHandEntitiesRuntime> resolve) => new(resolve);
}
