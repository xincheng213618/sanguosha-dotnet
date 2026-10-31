#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreProgramLostHpChainedPaymentsRuntime))]
internal sealed partial class CoreProgramLostHpChainedPaymentsEntries
{
    private readonly Func<EngineOwners.CoreProgramLostHpChainedPaymentsRuntime> _resolve;
    private CoreProgramLostHpChainedPaymentsEntries(Func<EngineOwners.CoreProgramLostHpChainedPaymentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramLostHpChainedPaymentsEntries Create(Func<EngineOwners.CoreProgramLostHpChainedPaymentsRuntime> resolve) => new(resolve);
}
