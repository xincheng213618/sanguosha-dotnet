#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOldHandDispositionEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOldHandDispositionRuntime> _resolve;
    private CoreGameEngineOldHandDispositionEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOldHandDispositionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOldHandDispositionEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOldHandDispositionRuntime> resolve) => new(resolve);
}
