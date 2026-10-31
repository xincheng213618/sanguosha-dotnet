#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineAtomicCardMovementRuntime))]
internal sealed partial class CoreGameEngineAtomicCardMovementEntries
{
    private readonly Func<EngineOwners.CoreGameEngineAtomicCardMovementRuntime> _resolve;
    private CoreGameEngineAtomicCardMovementEntries(Func<EngineOwners.CoreGameEngineAtomicCardMovementRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAtomicCardMovementEntries Create(Func<EngineOwners.CoreGameEngineAtomicCardMovementRuntime> resolve) => new(resolve);
}
