#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCardDeclarationsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardDeclarationsRuntime> _resolve;
    private CoreGameEngineCardDeclarationsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardDeclarationsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCardDeclarationsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardDeclarationsRuntime> resolve) => new(resolve);
}
