#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePrepDiscardReceiptsRuntime))]
internal sealed partial class CoreGameEnginePrepDiscardReceiptsEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePrepDiscardReceiptsRuntime> _resolve;
    private CoreGameEnginePrepDiscardReceiptsEntries(Func<EngineOwners.CoreGameEnginePrepDiscardReceiptsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePrepDiscardReceiptsEntries Create(Func<EngineOwners.CoreGameEnginePrepDiscardReceiptsRuntime> resolve) => new(resolve);
}
