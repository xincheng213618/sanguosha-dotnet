#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineCompletedUseChildReturnsRuntime))]
internal sealed partial class CoreGameEngineCompletedUseChildReturnsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineCompletedUseChildReturnsRuntime> _resolve;
    private CoreGameEngineCompletedUseChildReturnsEntries(Func<EngineOwners.CoreGameEngineCompletedUseChildReturnsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCompletedUseChildReturnsEntries Create(Func<EngineOwners.CoreGameEngineCompletedUseChildReturnsRuntime> resolve) => new(resolve);
}
