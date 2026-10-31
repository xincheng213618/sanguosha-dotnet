#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CorePhysicalOwnedCardQueriesRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterBaiLingYunAnniversaryEntries _characterBaiLingYunAnniversaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDuKuiAnniversaryEntries _characterDuKuiAnniversaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDuanJiongOlEntries _characterDuanJiongOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterFuRongOlEntries _characterFuRongOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterGongsunZanMouMobileEntries _characterGongsunZanMouMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuCheErMouEntries _characterHuCheErMouOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLuSuMobileShiEntries _characterLuSuMobileShiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterYaDanOlEntries _characterYaDanOlOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardDeclarationsEntries _coreCardDeclarationsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRequestedDeckBasicsEntries _coreRequestedDeckBasicsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDamageHandlesEntries _coreNativeDamageHandlesOperations;
    internal CorePhysicalOwnedCardQueriesRuntime(global::CardGame.Core.RuntimeEntries.CharacterBaiLingYunAnniversaryEntries characterBaiLingYunAnniversaryOperations, global::CardGame.Core.RuntimeEntries.CharacterDuKuiAnniversaryEntries characterDuKuiAnniversaryOperations, global::CardGame.Core.RuntimeEntries.CharacterDuanJiongOlEntries characterDuanJiongOlOperations, global::CardGame.Core.RuntimeEntries.CharacterFuRongOlEntries characterFuRongOlOperations, global::CardGame.Core.RuntimeEntries.CharacterGongsunZanMouMobileEntries characterGongsunZanMouMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterHuCheErMouEntries characterHuCheErMouOperations, global::CardGame.Core.RuntimeEntries.CharacterLuSuMobileShiEntries characterLuSuMobileShiOperations, global::CardGame.Core.RuntimeEntries.CharacterYaDanOlEntries characterYaDanOlOperations, global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardDeclarationsEntries coreCardDeclarationsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRequestedDeckBasicsEntries coreRequestedDeckBasicsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeDamageHandlesEntries coreNativeDamageHandlesOperations)
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
