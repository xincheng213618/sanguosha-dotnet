#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CorePlayerViewNativeCardSelectionsRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLiYiOlEntries _characterLiYiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLuSuMobileShiEntries _characterLuSuMobileShiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterQinLangOlEntries _characterQinLangOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterSunChenMobileShiEntries _characterSunChenMobileShiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterSunJianMouOlEntries _characterSunJianMouOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterSunJunMobileShiEntries _characterSunJunMobileShiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhangYanMobileShiEntries _characterZhangYanMobileShiOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CorePlayerViewNativeCardSelectionsRuntime(global::CardGame.Core.RuntimeEntries.CharacterLiYiOlEntries characterLiYiOlOperations, global::CardGame.Core.RuntimeEntries.CharacterLuSuMobileShiEntries characterLuSuMobileShiOperations, global::CardGame.Core.RuntimeEntries.CharacterQinLangOlEntries characterQinLangOlOperations, global::CardGame.Core.RuntimeEntries.CharacterSunChenMobileShiEntries characterSunChenMobileShiOperations, global::CardGame.Core.RuntimeEntries.CharacterSunJianMouOlEntries characterSunJianMouOlOperations, global::CardGame.Core.RuntimeEntries.CharacterSunJunMobileShiEntries characterSunJunMobileShiOperations, global::CardGame.Core.RuntimeEntries.CharacterZhangYanMobileShiEntries characterZhangYanMobileShiOperations, global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
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
