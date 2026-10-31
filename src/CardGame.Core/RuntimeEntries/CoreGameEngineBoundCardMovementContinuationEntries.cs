#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineBoundCardMovementContinuationEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBoundCardMovementContinuationRuntime> _resolve;
    private CoreGameEngineBoundCardMovementContinuationEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBoundCardMovementContinuationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineBoundCardMovementContinuationEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBoundCardMovementContinuationRuntime> resolve) => new(resolve);
}
