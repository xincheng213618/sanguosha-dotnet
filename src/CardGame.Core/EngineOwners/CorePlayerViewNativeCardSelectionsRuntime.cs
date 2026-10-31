#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CorePlayerViewNativeCardSelectionsRuntime
{
    private readonly RuntimeEntries.CharacterLiYiOlEntries _characterLiYiOlOperations;
    private readonly RuntimeEntries.CharacterLuSuMobileShiEntries _characterLuSuMobileShiOperations;
    private readonly RuntimeEntries.CharacterQinLangOlEntries _characterQinLangOlOperations;
    private readonly RuntimeEntries.CharacterSunChenMobileShiEntries _characterSunChenMobileShiOperations;
    private readonly RuntimeEntries.CharacterSunJianMouOlEntries _characterSunJianMouOlOperations;
    private readonly RuntimeEntries.CharacterSunJunMobileShiEntries _characterSunJunMobileShiOperations;
    private readonly RuntimeEntries.CharacterZhangYanMobileShiEntries _characterZhangYanMobileShiOperations;
    private readonly IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CorePlayerViewNativeCardSelectionsRuntime(RuntimeEntries.CharacterLiYiOlEntries characterLiYiOlOperations, RuntimeEntries.CharacterLuSuMobileShiEntries characterLuSuMobileShiOperations, RuntimeEntries.CharacterQinLangOlEntries characterQinLangOlOperations, RuntimeEntries.CharacterSunChenMobileShiEntries characterSunChenMobileShiOperations, RuntimeEntries.CharacterSunJianMouOlEntries characterSunJianMouOlOperations, RuntimeEntries.CharacterSunJunMobileShiEntries characterSunJunMobileShiOperations, RuntimeEntries.CharacterZhangYanMobileShiEntries characterZhangYanMobileShiOperations, IKernelCardZoneStoreAccess cardZonesOperations, IKernelContentRegistryAccess contentRegistryOperations, IKernelFrameStoreAccess resolutionStackOperations, RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations, RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterLiYiOlOperations = characterLiYiOlOperations;
        this._characterLuSuMobileShiOperations = characterLuSuMobileShiOperations;
        this._characterQinLangOlOperations = characterQinLangOlOperations;
        this._characterSunChenMobileShiOperations = characterSunChenMobileShiOperations;
        this._characterSunJianMouOlOperations = characterSunJianMouOlOperations;
        this._characterSunJunMobileShiOperations = characterSunJunMobileShiOperations;
        this._characterZhangYanMobileShiOperations = characterZhangYanMobileShiOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
