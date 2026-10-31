#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePrepDiscardPaymentsRuntime))]
internal sealed partial class CoreGameEnginePrepDiscardPaymentsEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePrepDiscardPaymentsRuntime> _resolve;
    private CoreGameEnginePrepDiscardPaymentsEntries(Func<EngineOwners.CoreGameEnginePrepDiscardPaymentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePrepDiscardPaymentsEntries Create(Func<EngineOwners.CoreGameEnginePrepDiscardPaymentsRuntime> resolve) => new(resolve);
}
