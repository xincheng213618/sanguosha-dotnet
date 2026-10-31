#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineOwnPublicPileConversionsRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterGongsunZanMouMobileEntries _characterGongsunZanMouMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterGuanYuWumiaoEntries _characterGuanYuWumiaoOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuangZuMobileShiEntries _characterHuangZuMobileShiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLuBuOlMagicEntries _characterLuBuOlMagicOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterPangTongFriendEntries _characterPangTongFriendOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineActionQueriesEntries _coreActionQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardActionsEntries _coreCardActionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries _coreCardAttackStateOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNativePublicPileOriginalCardsEntries _coreNativePublicPileOriginalCardsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePlayCardHistoryEntries _corePlayCardHistoryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePublicPileSourcesEntries _corePublicPileSourcesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSourceDingxiNativeEntries _coreSourceDingxiNativeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTableOrderEntries _coreTableOrderOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEnginePublicPileSourcesPublicPersistentPilesAccess _coreGameEnginePublicPileSourcesPublicPersistentPilesAccess;
    internal CoreGameEngineOwnPublicPileConversionsRuntime(global::CardGame.Core.RuntimeEntries.CharacterGongsunZanMouMobileEntries characterGongsunZanMouMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterGuanYuWumiaoEntries characterGuanYuWumiaoOperations, global::CardGame.Core.RuntimeEntries.CharacterHuangZuMobileShiEntries characterHuangZuMobileShiOperations, global::CardGame.Core.RuntimeEntries.CharacterLuBuOlMagicEntries characterLuBuOlMagicOperations, global::CardGame.Core.RuntimeEntries.CharacterPangTongFriendEntries characterPangTongFriendOperations, global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineActionQueriesEntries coreActionQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardActionsEntries coreCardActionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries coreCardAttackStateOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNativePublicPileOriginalCardsEntries coreNativePublicPileOriginalCardsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePlayCardHistoryEntries corePlayCardHistoryOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePublicPileSourcesEntries corePublicPileSourcesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineSourceDingxiNativeEntries coreSourceDingxiNativeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineTableOrderEntries coreTableOrderOperations, global::CardGame.Core.EngineStates.Contracts.ICoreGameEnginePublicPileSourcesPublicPersistentPilesAccess coreGameEnginePublicPileSourcesPublicPersistentPilesAccess)
    {
        this._characterGongsunZanMouMobileOperations = characterGongsunZanMouMobileOperations;
        this._characterGuanYuWumiaoOperations = characterGuanYuWumiaoOperations;
        this._characterHuangZuMobileShiOperations = characterHuangZuMobileShiOperations;
        this._characterLuBuOlMagicOperations = characterLuBuOlMagicOperations;
        this._characterPangTongFriendOperations = characterPangTongFriendOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreActionQueriesOperations = coreActionQueriesOperations;
        this._coreCardActionsOperations = coreCardActionsOperations;
        this._coreCardAttackStateOperations = coreCardAttackStateOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreNativePublicPileOriginalCardsOperations = coreNativePublicPileOriginalCardsOperations;
        this._corePlayCardHistoryOperations = corePlayCardHistoryOperations;
        this._corePublicPileSourcesOperations = corePublicPileSourcesOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSourceDingxiNativeOperations = coreSourceDingxiNativeOperations;
        this._coreTableOrderOperations = coreTableOrderOperations;
        this._coreGameEnginePublicPileSourcesPublicPersistentPilesAccess = coreGameEnginePublicPileSourcesPublicPersistentPilesAccess;
    }
}
