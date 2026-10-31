#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActualTurnLegacyCompletionEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualTurnLegacyCompletionRuntime> _resolve;
    private CoreGameEngineActualTurnLegacyCompletionEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualTurnLegacyCompletionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineActualTurnLegacyCompletionEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualTurnLegacyCompletionRuntime> resolve) => new(resolve);
}
