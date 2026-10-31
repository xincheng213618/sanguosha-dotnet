#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineAdjacentDiscardStorageRuntime))]
internal sealed partial class CoreGameEngineAdjacentDiscardStorageEntries
{
    private readonly Func<EngineOwners.CoreGameEngineAdjacentDiscardStorageRuntime> _resolve;
    private CoreGameEngineAdjacentDiscardStorageEntries(Func<EngineOwners.CoreGameEngineAdjacentDiscardStorageRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAdjacentDiscardStorageEntries Create(Func<EngineOwners.CoreGameEngineAdjacentDiscardStorageRuntime> resolve) => new(resolve);
}
