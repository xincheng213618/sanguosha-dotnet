#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActualTurnTrickUsesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualTurnTrickUsesRuntime> _resolve;
    private CoreGameEngineActualTurnTrickUsesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualTurnTrickUsesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineActualTurnTrickUsesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualTurnTrickUsesRuntime> resolve) => new(resolve);
}
