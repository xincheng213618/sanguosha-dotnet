#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDamageArmorPaymentsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageArmorPaymentsRuntime> _resolve;
    private CoreGameEngineDamageArmorPaymentsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageArmorPaymentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDamageArmorPaymentsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageArmorPaymentsRuntime> resolve) => new(resolve);
}
