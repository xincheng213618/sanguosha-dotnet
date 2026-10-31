#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreProgramDrawRuntimePortsRuntime : global::CardGame.Core.IProgramDrawFacts, global::CardGame.Core.IProgramDrawOperations
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCaoXiOlEntries _characterCaoXiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterWangYiBoundaryMobileEntries _characterWangYiBoundaryMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreActualDiscardPhaseLedgerEntries _coreActualDiscardPhaseLedgerOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentTurnUseKindsEntries _coreCurrentTurnUseKindsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineForcedPhaseAlcoholEntries _coreForcedPhaseAlcoholOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePairBenefitObserversEntries _corePairBenefitObserversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePairObtainShownGiftsEntries _corePairObtainShownGiftsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCategoryTargetLedgerEntries _coreProgramCategoryTargetLedgerOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramOwnedCardsEntries _coreProgramOwnedCardsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreMatchSetupDefinitionsEntries _coreMatchSetupDefinitionsOperations;
    private readonly ICoreProgramDrawRuntimePortsCoreNativeRuleQueriesOperations _coreNativeRuleQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalDrawFacadesEntries _corePhysicalDrawFacadesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreProgramDrawRuntimePortsRuntime(global::CardGame.Core.RuntimeEntries.CharacterCaoXiOlEntries characterCaoXiOlOperations, global::CardGame.Core.RuntimeEntries.CharacterWangYiBoundaryMobileEntries characterWangYiBoundaryMobileOperations, global::CardGame.Core.RuntimeEntries.CoreActualDiscardPhaseLedgerEntries coreActualDiscardPhaseLedgerOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentTurnUseKindsEntries coreCurrentTurnUseKindsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineForcedPhaseAlcoholEntries coreForcedPhaseAlcoholOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePairBenefitObserversEntries corePairBenefitObserversOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePairObtainShownGiftsEntries corePairObtainShownGiftsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCategoryTargetLedgerEntries coreProgramCategoryTargetLedgerOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramOwnedCardsEntries coreProgramOwnedCardsOperations, global::CardGame.Core.RuntimeEntries.CoreMatchSetupDefinitionsEntries coreMatchSetupDefinitionsOperations, ICoreProgramDrawRuntimePortsCoreNativeRuleQueriesOperations coreNativeRuleQueriesOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalDrawFacadesEntries corePhysicalDrawFacadesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
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
