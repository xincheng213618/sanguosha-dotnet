#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRicePileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRicePileRuntime> _resolve;
    private CoreGameEngineRicePileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRicePileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRicePileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRicePileRuntime> resolve) => new(resolve);
}
