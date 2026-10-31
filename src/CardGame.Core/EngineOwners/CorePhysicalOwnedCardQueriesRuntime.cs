#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CorePhysicalOwnedCardQueriesRuntime
{
    private readonly RuntimeEntries.CharacterBaiLingYunAnniversaryEntries _characterBaiLingYunAnniversaryOperations;
    private readonly RuntimeEntries.CharacterDuKuiAnniversaryEntries _characterDuKuiAnniversaryOperations;
    private readonly RuntimeEntries.CharacterDuanJiongOlEntries _characterDuanJiongOlOperations;
    private readonly RuntimeEntries.CharacterFuRongOlEntries _characterFuRongOlOperations;
    private readonly RuntimeEntries.CharacterGongsunZanMouMobileEntries _characterGongsunZanMouMobileOperations;
    private readonly RuntimeEntries.CharacterHuCheErMouEntries _characterHuCheErMouOperations;
    private readonly RuntimeEntries.CharacterLuSuMobileShiEntries _characterLuSuMobileShiOperations;
    private readonly RuntimeEntries.CharacterYaDanOlEntries _characterYaDanOlOperations;
    private readonly IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly RuntimeEntries.CoreGameEngineCardDeclarationsEntries _coreCardDeclarationsOperations;
    private readonly RuntimeEntries.CoreGameEngineRequestedDeckBasicsEntries _coreRequestedDeckBasicsOperations;
    private readonly RuntimeEntries.CoreNativeDamageHandlesEntries _coreNativeDamageHandlesOperations;
    internal CorePhysicalOwnedCardQueriesRuntime(RuntimeEntries.CharacterBaiLingYunAnniversaryEntries characterBaiLingYunAnniversaryOperations, RuntimeEntries.CharacterDuKuiAnniversaryEntries characterDuKuiAnniversaryOperations, RuntimeEntries.CharacterDuanJiongOlEntries characterDuanJiongOlOperations, RuntimeEntries.CharacterFuRongOlEntries characterFuRongOlOperations, RuntimeEntries.CharacterGongsunZanMouMobileEntries characterGongsunZanMouMobileOperations, RuntimeEntries.CharacterHuCheErMouEntries characterHuCheErMouOperations, RuntimeEntries.CharacterLuSuMobileShiEntries characterLuSuMobileShiOperations, RuntimeEntries.CharacterYaDanOlEntries characterYaDanOlOperations, IKernelCardZoneStoreAccess cardZonesOperations, RuntimeEntries.CoreGameEngineCardDeclarationsEntries coreCardDeclarationsOperations, RuntimeEntries.CoreGameEngineRequestedDeckBasicsEntries coreRequestedDeckBasicsOperations, RuntimeEntries.CoreNativeDamageHandlesEntries coreNativeDamageHandlesOperations)
    {
        this._characterBaiLingYunAnniversaryOperations = characterBaiLingYunAnniversaryOperations;
        this._characterDuKuiAnniversaryOperations = characterDuKuiAnniversaryOperations;
        this._characterDuanJiongOlOperations = characterDuanJiongOlOperations;
        this._characterFuRongOlOperations = characterFuRongOlOperations;
        this._characterGongsunZanMouMobileOperations = characterGongsunZanMouMobileOperations;
        this._characterHuCheErMouOperations = characterHuCheErMouOperations;
        this._characterLuSuMobileShiOperations = characterLuSuMobileShiOperations;
        this._characterYaDanOlOperations = characterYaDanOlOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._coreCardDeclarationsOperations = coreCardDeclarationsOperations;
        this._coreRequestedDeckBasicsOperations = coreRequestedDeckBasicsOperations;
        this._coreNativeDamageHandlesOperations = coreNativeDamageHandlesOperations;
    }
}
