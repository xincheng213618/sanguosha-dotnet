#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSourceHuojieEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceHuojieRuntime> _resolve;
    private CoreGameEngineSourceHuojieEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceHuojieRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSourceHuojieEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceHuojieRuntime> resolve) => new(resolve);
}
