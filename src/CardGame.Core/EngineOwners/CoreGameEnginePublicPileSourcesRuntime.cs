#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEnginePublicPileSourcesRuntime
{
    private readonly RuntimeEntries.CharacterCaoXianOlEntries _characterCaoXianOlOperations;
    private readonly RuntimeEntries.CharacterDengAiMouOlEntries _characterDengAiMouOlOperations;
    private readonly RuntimeEntries.CharacterGanNingMouMobileEntries _characterGanNingMouMobileOperations;
    private readonly RuntimeEntries.CharacterGongsunZanMouMobileEntries _characterGongsunZanMouMobileOperations;
    private readonly RuntimeEntries.CharacterGuanHaiOlEntries _characterGuanHaiOlOperations;
    private readonly RuntimeEntries.CharacterGuoHuaiMouMobileEntries _characterGuoHuaiMouMobileOperations;
    private readonly RuntimeEntries.CharacterLiFengOlEntries _characterLiFengOlOperations;
    private readonly RuntimeEntries.CharacterSunShangxiangMouMobileEntries _characterSunShangxiangMouMobileOperations;
    private readonly RuntimeEntries.CharacterXiahouDunMouMobileEntries _characterXiahouDunMouMobileOperations;
    private readonly RuntimeEntries.CharacterZhenJiHappyGodEntries _characterZhenJiHappyGodOperations;
    private readonly IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly RuntimeEntries.CoreGameEngineRicePileEntries _coreRicePileOperations;
    private readonly RuntimeEntries.CoreSnapshotValueProjectionEntries _coreSnapshotValueProjectionOperations;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly EngineStates.Contracts.ICoreGameEnginePublicPileSourcesPublicPersistentPilesAccess _coreGameEnginePublicPileSourcesPublicPersistentPilesAccess;
    internal CoreGameEnginePublicPileSourcesRuntime(RuntimeEntries.CharacterCaoXianOlEntries characterCaoXianOlOperations, RuntimeEntries.CharacterDengAiMouOlEntries characterDengAiMouOlOperations, RuntimeEntries.CharacterGanNingMouMobileEntries characterGanNingMouMobileOperations, RuntimeEntries.CharacterGongsunZanMouMobileEntries characterGongsunZanMouMobileOperations, RuntimeEntries.CharacterGuanHaiOlEntries characterGuanHaiOlOperations, RuntimeEntries.CharacterGuoHuaiMouMobileEntries characterGuoHuaiMouMobileOperations, RuntimeEntries.CharacterLiFengOlEntries characterLiFengOlOperations, RuntimeEntries.CharacterSunShangxiangMouMobileEntries characterSunShangxiangMouMobileOperations, RuntimeEntries.CharacterXiahouDunMouMobileEntries characterXiahouDunMouMobileOperations, RuntimeEntries.CharacterZhenJiHappyGodEntries characterZhenJiHappyGodOperations, IKernelCardZoneStoreAccess cardZonesOperations, IKernelContentRegistryAccess contentRegistryOperations, RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations, RuntimeEntries.CoreGameEngineRicePileEntries coreRicePileOperations, RuntimeEntries.CoreSnapshotValueProjectionEntries coreSnapshotValueProjectionOperations, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, EngineStates.Contracts.ICoreGameEnginePublicPileSourcesPublicPersistentPilesAccess coreGameEnginePublicPileSourcesPublicPersistentPilesAccess)
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
