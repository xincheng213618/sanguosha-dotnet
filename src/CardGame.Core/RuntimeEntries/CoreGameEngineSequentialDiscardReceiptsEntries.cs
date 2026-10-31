#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineSequentialDiscardReceiptsRuntime))]
internal sealed partial class CoreGameEngineSequentialDiscardReceiptsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSequentialDiscardReceiptsRuntime> _resolve;
    private CoreGameEngineSequentialDiscardReceiptsEntries(Func<EngineOwners.CoreGameEngineSequentialDiscardReceiptsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSequentialDiscardReceiptsEntries Create(Func<EngineOwners.CoreGameEngineSequentialDiscardReceiptsRuntime> resolve) => new(resolve);
}
