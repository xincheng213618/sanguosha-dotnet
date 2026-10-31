#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheMobilizationEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheMobilizationRuntime> _resolve;
    private CoreGameEngineShanheMobilizationEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheMobilizationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheMobilizationEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheMobilizationRuntime> resolve) => new(resolve);
}
