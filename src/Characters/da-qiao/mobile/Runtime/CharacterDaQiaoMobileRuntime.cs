#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CharacterDaQiaoMobileRuntime
{
    private readonly RuntimeEntries.CharacterSunYuanOlEntries _characterSunYuanOlOperations;
    private readonly RuntimeEntries.CharacterTaishiCiMobileGodEntries _characterTaishiCiMobileGodOperations;
    private readonly IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly RuntimeEntries.CoreActiveNativeAttackQueriesEntries _coreActiveNativeAttackQueriesOperations;
    private readonly RuntimeEntries.CoreGameEngineActionQueriesEntries _coreActionQueriesOperations;
    private readonly RuntimeEntries.CoreGameEngineCardActionsEntries _coreCardActionsOperations;
    private readonly RuntimeEntries.CoreGameEngineCardAttackStateEntries _coreCardAttackStateOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramAttackEffectsEntries _coreProgramAttackEffectsOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramCardPaymentsEntries _coreProgramCardPaymentsOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramOwnedCardsEntries _coreProgramOwnedCardsOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramPersistentCombatPoliciesEntries _coreProgramPersistentCombatPoliciesOperations;
    private readonly RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries _coreTurnHandCategoryRestrictionsOperations;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterDaQiaoMobileRuntime(RuntimeEntries.CharacterSunYuanOlEntries characterSunYuanOlOperations, RuntimeEntries.CharacterTaishiCiMobileGodEntries characterTaishiCiMobileGodOperations, IKernelCardZoneStoreAccess cardZonesOperations, IKernelContentRegistryAccess contentRegistryOperations, IKernelFrameStoreAccess resolutionStackOperations, RuntimeEntries.CoreActiveNativeAttackQueriesEntries coreActiveNativeAttackQueriesOperations, RuntimeEntries.CoreGameEngineActionQueriesEntries coreActionQueriesOperations, RuntimeEntries.CoreGameEngineCardActionsEntries coreCardActionsOperations, RuntimeEntries.CoreGameEngineCardAttackStateEntries coreCardAttackStateOperations, RuntimeEntries.CoreGameEngineProgramAttackEffectsEntries coreProgramAttackEffectsOperations, RuntimeEntries.CoreGameEngineProgramCardPaymentsEntries coreProgramCardPaymentsOperations, RuntimeEntries.CoreGameEngineProgramOwnedCardsEntries coreProgramOwnedCardsOperations, RuntimeEntries.CoreGameEngineProgramPersistentCombatPoliciesEntries coreProgramPersistentCombatPoliciesOperations, RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries coreTurnHandCategoryRestrictionsOperations, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterSunYuanOlOperations = characterSunYuanOlOperations;
        this._characterTaishiCiMobileGodOperations = characterTaishiCiMobileGodOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreActiveNativeAttackQueriesOperations = coreActiveNativeAttackQueriesOperations;
        this._coreActionQueriesOperations = coreActionQueriesOperations;
        this._coreCardActionsOperations = coreCardActionsOperations;
        this._coreCardAttackStateOperations = coreCardAttackStateOperations;
        this._coreProgramAttackEffectsOperations = coreProgramAttackEffectsOperations;
        this._coreProgramCardPaymentsOperations = coreProgramCardPaymentsOperations;
        this._coreProgramOwnedCardsOperations = coreProgramOwnedCardsOperations;
        this._coreProgramPersistentCombatPoliciesOperations = coreProgramPersistentCombatPoliciesOperations;
        this._coreTurnHandCategoryRestrictionsOperations = coreTurnHandCategoryRestrictionsOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
