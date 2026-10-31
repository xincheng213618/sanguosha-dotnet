#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CharacterGuanPingBoundaryRuntime
{
    private readonly IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly RuntimeEntries.CoreNativeCardAppearanceEntries _coreNativeCardAppearanceOperations;
    private readonly RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    internal CharacterGuanPingBoundaryRuntime(IKernelCardZoneStoreAccess cardZonesOperations, RuntimeEntries.CoreNativeCardAppearanceEntries coreNativeCardAppearanceOperations, RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._coreNativeCardAppearanceOperations = coreNativeCardAppearanceOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
    }
}
