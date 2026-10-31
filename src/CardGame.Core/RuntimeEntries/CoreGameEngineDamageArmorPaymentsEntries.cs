#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDamageArmorPaymentsRuntime))]
internal sealed partial class CoreGameEngineDamageArmorPaymentsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDamageArmorPaymentsRuntime> _resolve;
    private CoreGameEngineDamageArmorPaymentsEntries(Func<EngineOwners.CoreGameEngineDamageArmorPaymentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDamageArmorPaymentsEntries Create(Func<EngineOwners.CoreGameEngineDamageArmorPaymentsRuntime> resolve) => new(resolve);
}
