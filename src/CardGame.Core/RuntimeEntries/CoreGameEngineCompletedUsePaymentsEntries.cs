#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineCompletedUsePaymentsRuntime))]
internal sealed partial class CoreGameEngineCompletedUsePaymentsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineCompletedUsePaymentsRuntime> _resolve;
    private CoreGameEngineCompletedUsePaymentsEntries(Func<EngineOwners.CoreGameEngineCompletedUsePaymentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCompletedUsePaymentsEntries Create(Func<EngineOwners.CoreGameEngineCompletedUsePaymentsRuntime> resolve) => new(resolve);
}
