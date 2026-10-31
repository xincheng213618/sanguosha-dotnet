#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreForeignEquipmentDiscardPoliciesRuntime : global::CardGame.Core.IOwnedCardMoveLegacyFacts
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterBianFuRenOlEntries _characterBianFuRenOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDongBaiOlEntries _characterDongBaiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterPangTongAnniversaryGodEntries _characterPangTongAnniversaryGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhangFenAnniversaryEntries _characterZhangFenAnniversaryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries _coreProgramCardPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries _coreTurnHandCategoryRestrictionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreForeignEquipmentDiscardPoliciesRuntime(global::CardGame.Core.RuntimeEntries.CharacterBianFuRenOlEntries characterBianFuRenOlOperations, global::CardGame.Core.RuntimeEntries.CharacterDongBaiOlEntries characterDongBaiOlOperations, global::CardGame.Core.RuntimeEntries.CharacterPangTongAnniversaryGodEntries characterPangTongAnniversaryGodOperations, global::CardGame.Core.RuntimeEntries.CharacterZhangFenAnniversaryEntries characterZhangFenAnniversaryOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries coreProgramCardPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries coreTurnHandCategoryRestrictionsOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.RuntimeAccess.IClosedOwnedCardMoveQueriesSource _closedOwnedCardMoveQueriesSource)
    {
        this._characterBianFuRenOlOperations = characterBianFuRenOlOperations;
        this._characterDongBaiOlOperations = characterDongBaiOlOperations;
        this._characterPangTongAnniversaryGodOperations = characterPangTongAnniversaryGodOperations;
        this._characterZhangFenAnniversaryOperations = characterZhangFenAnniversaryOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreTurnHandCategoryRestrictionsOperations = coreTurnHandCategoryRestrictionsOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._closedOwnedCardMoveQueriesSource = _closedOwnedCardMoveQueriesSource;
    }

    private readonly global::CardGame.Core.RuntimeAccess.IClosedOwnedCardMoveQueriesSource _closedOwnedCardMoveQueriesSource;
}
