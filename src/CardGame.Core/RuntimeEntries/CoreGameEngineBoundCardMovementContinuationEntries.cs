#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineBoundCardMovementContinuationRuntime))]
internal sealed partial class CoreGameEngineBoundCardMovementContinuationEntries
{
    private readonly Func<EngineOwners.CoreGameEngineBoundCardMovementContinuationRuntime> _resolve;
    private CoreGameEngineBoundCardMovementContinuationEntries(Func<EngineOwners.CoreGameEngineBoundCardMovementContinuationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineBoundCardMovementContinuationEntries Create(Func<EngineOwners.CoreGameEngineBoundCardMovementContinuationRuntime> resolve) => new(resolve);
}
