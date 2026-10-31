#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActualTurnViewAsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualTurnViewAsRuntime> _resolve;
    private CoreGameEngineActualTurnViewAsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualTurnViewAsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineActualTurnViewAsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualTurnViewAsRuntime> resolve) => new(resolve);
}
