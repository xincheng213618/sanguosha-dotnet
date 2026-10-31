#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineEquipmentDonationObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentDonationObserversRuntime> _resolve;
    private CoreGameEngineEquipmentDonationObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentDonationObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineEquipmentDonationObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentDonationObserversRuntime> resolve) => new(resolve);
}
