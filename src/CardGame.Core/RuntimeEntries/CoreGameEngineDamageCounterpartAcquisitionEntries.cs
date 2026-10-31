#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDamageCounterpartAcquisitionEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageCounterpartAcquisitionRuntime> _resolve;
    private CoreGameEngineDamageCounterpartAcquisitionEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageCounterpartAcquisitionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDamageCounterpartAcquisitionEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageCounterpartAcquisitionRuntime> resolve) => new(resolve);
}
