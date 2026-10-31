#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActualPlayPhaseCardUsesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualPlayPhaseCardUsesRuntime> _resolve;
    private CoreGameEngineActualPlayPhaseCardUsesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualPlayPhaseCardUsesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineActualPlayPhaseCardUsesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualPlayPhaseCardUsesRuntime> resolve) => new(resolve);
}
