#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineEquipmentDonationObserversRuntime))]
internal sealed partial class CoreGameEngineEquipmentDonationObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineEquipmentDonationObserversRuntime> _resolve;
    private CoreGameEngineEquipmentDonationObserversEntries(Func<EngineOwners.CoreGameEngineEquipmentDonationObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineEquipmentDonationObserversEntries Create(Func<EngineOwners.CoreGameEngineEquipmentDonationObserversRuntime> resolve) => new(resolve);
}
