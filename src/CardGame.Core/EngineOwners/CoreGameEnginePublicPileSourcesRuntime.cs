#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEnginePublicPileSourcesRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCaoXianOlEntries _characterCaoXianOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDengAiMouOlEntries _characterDengAiMouOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterGanNingMouMobileEntries _characterGanNingMouMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterGongsunZanMouMobileEntries _characterGongsunZanMouMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterGuanHaiOlEntries _characterGuanHaiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterGuoHuaiMouMobileEntries _characterGuoHuaiMouMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLiFengOlEntries _characterLiFengOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterSunShangxiangMouMobileEntries _characterSunShangxiangMouMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterXiahouDunMouMobileEntries _characterXiahouDunMouMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhenJiHappyGodEntries _characterZhenJiHappyGodOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRicePileEntries _coreRicePileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreSnapshotValueProjectionEntries _coreSnapshotValueProjectionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEnginePublicPileSourcesPublicPersistentPilesAccess _coreGameEnginePublicPileSourcesPublicPersistentPilesAccess;
    internal CoreGameEnginePublicPileSourcesRuntime(global::CardGame.Core.RuntimeEntries.CharacterCaoXianOlEntries characterCaoXianOlOperations, global::CardGame.Core.RuntimeEntries.CharacterDengAiMouOlEntries characterDengAiMouOlOperations, global::CardGame.Core.RuntimeEntries.CharacterGanNingMouMobileEntries characterGanNingMouMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterGongsunZanMouMobileEntries characterGongsunZanMouMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterGuanHaiOlEntries characterGuanHaiOlOperations, global::CardGame.Core.RuntimeEntries.CharacterGuoHuaiMouMobileEntries characterGuoHuaiMouMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterLiFengOlEntries characterLiFengOlOperations, global::CardGame.Core.RuntimeEntries.CharacterSunShangxiangMouMobileEntries characterSunShangxiangMouMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterXiahouDunMouMobileEntries characterXiahouDunMouMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterZhenJiHappyGodEntries characterZhenJiHappyGodOperations, global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRicePileEntries coreRicePileOperations, global::CardGame.Core.RuntimeEntries.CoreSnapshotValueProjectionEntries coreSnapshotValueProjectionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEnginePublicPileSourcesPublicPersistentPilesAccess coreGameEnginePublicPileSourcesPublicPersistentPilesAccess)
    {
        this._characterCaoXianOlOperations = characterCaoXianOlOperations;
        this._characterDengAiMouOlOperations = characterDengAiMouOlOperations;
        this._characterGanNingMouMobileOperations = characterGanNingMouMobileOperations;
        this._characterGongsunZanMouMobileOperations = characterGongsunZanMouMobileOperations;
        this._characterGuanHaiOlOperations = characterGuanHaiOlOperations;
        this._characterGuoHuaiMouMobileOperations = characterGuoHuaiMouMobileOperations;
        this._characterLiFengOlOperations = characterLiFengOlOperations;
        this._characterSunShangxiangMouMobileOperations = characterSunShangxiangMouMobileOperations;
        this._characterXiahouDunMouMobileOperations = characterXiahouDunMouMobileOperations;
        this._characterZhenJiHappyGodOperations = characterZhenJiHappyGodOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreRicePileOperations = coreRicePileOperations;
        this._coreSnapshotValueProjectionOperations = coreSnapshotValueProjectionOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreGameEnginePublicPileSourcesPublicPersistentPilesAccess = coreGameEnginePublicPileSourcesPublicPersistentPilesAccess;
    }
}
