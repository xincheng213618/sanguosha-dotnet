#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CoreForeignEquipmentDiscardPoliciesRuntime : IOwnedCardMoveLegacyFacts
{
    private readonly RuntimeEntries.CharacterBianFuRenOlEntries _characterBianFuRenOlOperations;
    private readonly RuntimeEntries.CharacterDongBaiOlEntries _characterDongBaiOlOperations;
    private readonly RuntimeEntries.CharacterPangTongAnniversaryGodEntries _characterPangTongAnniversaryGodOperations;
    private readonly RuntimeEntries.CharacterZhangFenAnniversaryEntries _characterZhangFenAnniversaryOperations;
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries _coreProgramCardPoliciesOperations;
    private readonly RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries _coreTurnHandCategoryRestrictionsOperations;
    private readonly RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreForeignEquipmentDiscardPoliciesRuntime(RuntimeEntries.CharacterBianFuRenOlEntries characterBianFuRenOlOperations, RuntimeEntries.CharacterDongBaiOlEntries characterDongBaiOlOperations, RuntimeEntries.CharacterPangTongAnniversaryGodEntries characterPangTongAnniversaryGodOperations, RuntimeEntries.CharacterZhangFenAnniversaryEntries characterZhangFenAnniversaryOperations, IKernelContentRegistryAccess contentRegistryOperations, RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries coreProgramCardPoliciesOperations, RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries coreTurnHandCategoryRestrictionsOperations, RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, RuntimeAccess.IClosedOwnedCardMoveQueriesSource _closedOwnedCardMoveQueriesSource)
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

    private readonly RuntimeAccess.IClosedOwnedCardMoveQueriesSource _closedOwnedCardMoveQueriesSource;
}
