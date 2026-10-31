#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineSignedDamagePaymentsRuntime))]
internal sealed partial class CoreGameEngineSignedDamagePaymentsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSignedDamagePaymentsRuntime> _resolve;
    private CoreGameEngineSignedDamagePaymentsEntries(Func<EngineOwners.CoreGameEngineSignedDamagePaymentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSignedDamagePaymentsEntries Create(Func<EngineOwners.CoreGameEngineSignedDamagePaymentsRuntime> resolve) => new(resolve);
}
