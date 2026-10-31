#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CoreProgramDrawRuntimePortsRuntime : IProgramDrawFacts, IProgramDrawOperations
{
    private readonly RuntimeEntries.CharacterCaoXiOlEntries _characterCaoXiOlOperations;
    private readonly RuntimeEntries.CharacterWangYiBoundaryMobileEntries _characterWangYiBoundaryMobileOperations;
    private readonly RuntimeEntries.CoreActualDiscardPhaseLedgerEntries _coreActualDiscardPhaseLedgerOperations;
    private readonly RuntimeEntries.CoreGameEngineCurrentTurnUseKindsEntries _coreCurrentTurnUseKindsOperations;
    private readonly RuntimeEntries.CoreGameEngineForcedPhaseAlcoholEntries _coreForcedPhaseAlcoholOperations;
    private readonly RuntimeEntries.CoreGameEnginePairBenefitObserversEntries _corePairBenefitObserversOperations;
    private readonly RuntimeEntries.CoreGameEnginePairObtainShownGiftsEntries _corePairObtainShownGiftsOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramCategoryTargetLedgerEntries _coreProgramCategoryTargetLedgerOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramOwnedCardsEntries _coreProgramOwnedCardsOperations;
    private readonly RuntimeEntries.CoreMatchSetupDefinitionsEntries _coreMatchSetupDefinitionsOperations;
    private readonly ICoreProgramDrawRuntimePortsCoreNativeRuleQueriesOperations _coreNativeRuleQueriesOperations;
    private readonly RuntimeEntries.CorePhysicalDrawFacadesEntries _corePhysicalDrawFacadesOperations;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreProgramDrawRuntimePortsRuntime(RuntimeEntries.CharacterCaoXiOlEntries characterCaoXiOlOperations, RuntimeEntries.CharacterWangYiBoundaryMobileEntries characterWangYiBoundaryMobileOperations, RuntimeEntries.CoreActualDiscardPhaseLedgerEntries coreActualDiscardPhaseLedgerOperations, RuntimeEntries.CoreGameEngineCurrentTurnUseKindsEntries coreCurrentTurnUseKindsOperations, RuntimeEntries.CoreGameEngineForcedPhaseAlcoholEntries coreForcedPhaseAlcoholOperations, RuntimeEntries.CoreGameEnginePairBenefitObserversEntries corePairBenefitObserversOperations, RuntimeEntries.CoreGameEnginePairObtainShownGiftsEntries corePairObtainShownGiftsOperations, RuntimeEntries.CoreGameEngineProgramCategoryTargetLedgerEntries coreProgramCategoryTargetLedgerOperations, RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, RuntimeEntries.CoreGameEngineProgramOwnedCardsEntries coreProgramOwnedCardsOperations, RuntimeEntries.CoreMatchSetupDefinitionsEntries coreMatchSetupDefinitionsOperations, ICoreProgramDrawRuntimePortsCoreNativeRuleQueriesOperations coreNativeRuleQueriesOperations, RuntimeEntries.CorePhysicalDrawFacadesEntries corePhysicalDrawFacadesOperations, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterCaoXiOlOperations = characterCaoXiOlOperations;
        this._characterWangYiBoundaryMobileOperations = characterWangYiBoundaryMobileOperations;
        this._coreActualDiscardPhaseLedgerOperations = coreActualDiscardPhaseLedgerOperations;
        this._coreCurrentTurnUseKindsOperations = coreCurrentTurnUseKindsOperations;
        this._coreForcedPhaseAlcoholOperations = coreForcedPhaseAlcoholOperations;
        this._corePairBenefitObserversOperations = corePairBenefitObserversOperations;
        this._corePairObtainShownGiftsOperations = corePairObtainShownGiftsOperations;
        this._coreProgramCategoryTargetLedgerOperations = coreProgramCategoryTargetLedgerOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreProgramOwnedCardsOperations = coreProgramOwnedCardsOperations;
        this._coreMatchSetupDefinitionsOperations = coreMatchSetupDefinitionsOperations;
        this._coreNativeRuleQueriesOperations = coreNativeRuleQueriesOperations;
        this._corePhysicalDrawFacadesOperations = corePhysicalDrawFacadesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
