#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreProgramCardUsePaymentsRuntime))]
internal sealed partial class CoreProgramCardUsePaymentsEntries
{
    private readonly Func<EngineOwners.CoreProgramCardUsePaymentsRuntime> _resolve;
    private CoreProgramCardUsePaymentsEntries(Func<EngineOwners.CoreProgramCardUsePaymentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramCardUsePaymentsEntries Create(Func<EngineOwners.CoreProgramCardUsePaymentsRuntime> resolve) => new(resolve);
}
