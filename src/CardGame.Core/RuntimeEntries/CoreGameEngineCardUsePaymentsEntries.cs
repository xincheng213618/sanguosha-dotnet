#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineCardUsePaymentsRuntime))]
internal sealed partial class CoreGameEngineCardUsePaymentsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineCardUsePaymentsRuntime> _resolve;
    private CoreGameEngineCardUsePaymentsEntries(Func<EngineOwners.CoreGameEngineCardUsePaymentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCardUsePaymentsEntries Create(Func<EngineOwners.CoreGameEngineCardUsePaymentsRuntime> resolve) => new(resolve);
}
