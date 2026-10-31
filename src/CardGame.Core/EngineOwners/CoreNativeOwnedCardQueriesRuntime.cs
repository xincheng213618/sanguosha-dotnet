#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreNativeOwnedCardQueriesRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterBaiLingYunAnniversaryEntries _characterBaiLingYunAnniversaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCaoAngMouEntries _characterCaoAngMouOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDuKuiAnniversaryEntries _characterDuKuiAnniversaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDuanJiongOlEntries _characterDuanJiongOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterFuRongOlEntries _characterFuRongOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterGongsunZanMouMobileEntries _characterGongsunZanMouMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLuSuMobileShiEntries _characterLuSuMobileShiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterMaChaoAnniversaryGodEntries _characterMaChaoAnniversaryGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterWeiYanHappyGodEntries _characterWeiYanHappyGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterYaDanOlEntries _characterYaDanOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhangHuanAnniversaryEntries _characterZhangHuanAnniversaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhouFeiBoundaryMobileEntries _characterZhouFeiBoundaryMobileOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardDeclarationsEntries _coreCardDeclarationsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineLastZoneConversionsEntries _coreLastZoneConversionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNativePublicPileOriginalCardsEntries _coreNativePublicPileOriginalCardsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineOwnPublicPileConversionsEntries _coreOwnPublicPileConversionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRequestedDeckBasicsEntries _coreRequestedDeckBasicsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseAccess _coreActualTurnProgressCurrentSeatPhaseAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCurrentTurnStatisticsUsedOrPlayedSlashDuringPlayPhaseAccess _coreCurrentTurnStatisticsUsedOrPlayedSlashDuringPlayPhaseAccess;
    internal CoreNativeOwnedCardQueriesRuntime(global::CardGame.Core.RuntimeEntries.CharacterBaiLingYunAnniversaryEntries characterBaiLingYunAnniversaryOperations, global::CardGame.Core.RuntimeEntries.CharacterCaoAngMouEntries characterCaoAngMouOperations, global::CardGame.Core.RuntimeEntries.CharacterDuKuiAnniversaryEntries characterDuKuiAnniversaryOperations, global::CardGame.Core.RuntimeEntries.CharacterDuanJiongOlEntries characterDuanJiongOlOperations, global::CardGame.Core.RuntimeEntries.CharacterFuRongOlEntries characterFuRongOlOperations, global::CardGame.Core.RuntimeEntries.CharacterGongsunZanMouMobileEntries characterGongsunZanMouMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterLuSuMobileShiEntries characterLuSuMobileShiOperations, global::CardGame.Core.RuntimeEntries.CharacterMaChaoAnniversaryGodEntries characterMaChaoAnniversaryGodOperations, global::CardGame.Core.RuntimeEntries.CharacterWeiYanHappyGodEntries characterWeiYanHappyGodOperations, global::CardGame.Core.RuntimeEntries.CharacterYaDanOlEntries characterYaDanOlOperations, global::CardGame.Core.RuntimeEntries.CharacterZhangHuanAnniversaryEntries characterZhangHuanAnniversaryOperations, global::CardGame.Core.RuntimeEntries.CharacterZhouFeiBoundaryMobileEntries characterZhouFeiBoundaryMobileOperations, global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardDeclarationsEntries coreCardDeclarationsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineLastZoneConversionsEntries coreLastZoneConversionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNativePublicPileOriginalCardsEntries coreNativePublicPileOriginalCardsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineOwnPublicPileConversionsEntries coreOwnPublicPileConversionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRequestedDeckBasicsEntries coreRequestedDeckBasicsOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseAccess coreActualTurnProgressCurrentSeatPhaseAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCurrentTurnStatisticsUsedOrPlayedSlashDuringPlayPhaseAccess coreCurrentTurnStatisticsUsedOrPlayedSlashDuringPlayPhaseAccess)
    {
        this._characterBaiLingYunAnniversaryOperations = characterBaiLingYunAnniversaryOperations;
        this._characterCaoAngMouOperations = characterCaoAngMouOperations;
        this._characterDuKuiAnniversaryOperations = characterDuKuiAnniversaryOperations;
        this._characterDuanJiongOlOperations = characterDuanJiongOlOperations;
        this._characterFuRongOlOperations = characterFuRongOlOperations;
        this._characterGongsunZanMouMobileOperations = characterGongsunZanMouMobileOperations;
        this._characterLuSuMobileShiOperations = characterLuSuMobileShiOperations;
        this._characterMaChaoAnniversaryGodOperations = characterMaChaoAnniversaryGodOperations;
        this._characterWeiYanHappyGodOperations = characterWeiYanHappyGodOperations;
        this._characterYaDanOlOperations = characterYaDanOlOperations;
        this._characterZhangHuanAnniversaryOperations = characterZhangHuanAnniversaryOperations;
        this._characterZhouFeiBoundaryMobileOperations = characterZhouFeiBoundaryMobileOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._coreCardDeclarationsOperations = coreCardDeclarationsOperations;
        this._coreLastZoneConversionsOperations = coreLastZoneConversionsOperations;
        this._coreNativePublicPileOriginalCardsOperations = coreNativePublicPileOriginalCardsOperations;
        this._coreOwnPublicPileConversionsOperations = coreOwnPublicPileConversionsOperations;
        this._coreRequestedDeckBasicsOperations = coreRequestedDeckBasicsOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreActualTurnProgressCurrentSeatPhaseAccess = coreActualTurnProgressCurrentSeatPhaseAccess;
        this._coreCurrentTurnStatisticsUsedOrPlayedSlashDuringPlayPhaseAccess = coreCurrentTurnStatisticsUsedOrPlayedSlashDuringPlayPhaseAccess;
    }
}
