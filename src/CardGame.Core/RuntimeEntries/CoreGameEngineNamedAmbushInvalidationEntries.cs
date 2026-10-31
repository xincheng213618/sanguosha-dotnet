#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNamedAmbushInvalidationEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNamedAmbushInvalidationRuntime> _resolve;
    private CoreGameEngineNamedAmbushInvalidationEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNamedAmbushInvalidationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNamedAmbushInvalidationEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNamedAmbushInvalidationRuntime> resolve) => new(resolve);
}
