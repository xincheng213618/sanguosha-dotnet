#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePublicPersistentPilesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicPersistentPilesRuntime> _resolve;
    private CoreGameEnginePublicPersistentPilesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicPersistentPilesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePublicPersistentPilesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicPersistentPilesRuntime> resolve) => new(resolve);
}
