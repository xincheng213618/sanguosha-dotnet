#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreProgramBoundCardRuntimePortsRuntime : global::CardGame.Core.IProgramBoundCardFacts, global::CardGame.Core.IProgramBoundCardMovementOperations, global::CardGame.Core.IProgramBoundCardPaymentOperations, global::CardGame.Core.IProgramBoundInstructionFacts
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuangQuanMouEntries _characterHuangQuanMouOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterWuGuoTaiBoundaryMobileEntries _characterWuGuoTaiBoundaryMobileOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreForeignEquipmentDiscardPoliciesEntries _coreForeignEquipmentDiscardPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardDistributionPresentationEntries _coreCardDistributionPresentationOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDeckProgramsEntries _coreDeckProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineExactRepeatedJudgmentEntries _coreExactRepeatedJudgmentOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineFieldEquipmentEntries _coreFieldEquipmentOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineForcedPhaseAlcoholEntries _coreForcedPhaseAlcoholOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineLiejieSourceDiscardsEntries _coreLiejieSourceDiscardsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePindianHandReturnsEntries _corePindianHandReturnsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries _coreProgramAtomicCardMovesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPaymentsEntries _coreProgramCardPaymentsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSequentialDiscardReceiptsEntries _coreSequentialDiscardReceiptsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreHellMengpoProgramsEntries _coreHellMengpoProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardAppearanceEntries _coreNativeCardAppearanceOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreParticipantHandProgramsEntries _coreParticipantHandProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalDrawFacadesEntries _corePhysicalDrawFacadesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries _corePhysicalMovementFacadesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreProgramLostHpChainedPaymentsEntries _coreProgramLostHpChainedPaymentsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess _coreCardMovementJournalCardMovementsMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreProgramBoundCardRuntimePortsRuntime(global::CardGame.Core.RuntimeEntries.CharacterHuangQuanMouEntries characterHuangQuanMouOperations, global::CardGame.Core.RuntimeEntries.CharacterWuGuoTaiBoundaryMobileEntries characterWuGuoTaiBoundaryMobileOperations, global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.RuntimeEntries.CoreForeignEquipmentDiscardPoliciesEntries coreForeignEquipmentDiscardPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardDistributionPresentationEntries coreCardDistributionPresentationOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDeckProgramsEntries coreDeckProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineExactRepeatedJudgmentEntries coreExactRepeatedJudgmentOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineFieldEquipmentEntries coreFieldEquipmentOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineForcedPhaseAlcoholEntries coreForcedPhaseAlcoholOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineLiejieSourceDiscardsEntries coreLiejieSourceDiscardsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePindianHandReturnsEntries corePindianHandReturnsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries coreProgramAtomicCardMovesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPaymentsEntries coreProgramCardPaymentsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineSequentialDiscardReceiptsEntries coreSequentialDiscardReceiptsOperations, global::CardGame.Core.RuntimeEntries.CoreHellMengpoProgramsEntries coreHellMengpoProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardAppearanceEntries coreNativeCardAppearanceOperations, global::CardGame.Core.RuntimeEntries.CoreParticipantHandProgramsEntries coreParticipantHandProgramsOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalDrawFacadesEntries corePhysicalDrawFacadesOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries corePhysicalMovementFacadesOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreProgramLostHpChainedPaymentsEntries coreProgramLostHpChainedPaymentsOperations, global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess coreCardMovementJournalCardMovementsMovementSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterHuangQuanMouOperations = characterHuangQuanMouOperations;
        this._characterWuGuoTaiBoundaryMobileOperations = characterWuGuoTaiBoundaryMobileOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreForeignEquipmentDiscardPoliciesOperations = coreForeignEquipmentDiscardPoliciesOperations;
        this._coreCardDistributionPresentationOperations = coreCardDistributionPresentationOperations;
        this._coreDeckProgramsOperations = coreDeckProgramsOperations;
        this._coreExactRepeatedJudgmentOperations = coreExactRepeatedJudgmentOperations;
        this._coreFieldEquipmentOperations = coreFieldEquipmentOperations;
        this._coreForcedPhaseAlcoholOperations = coreForcedPhaseAlcoholOperations;
        this._coreLiejieSourceDiscardsOperations = coreLiejieSourceDiscardsOperations;
        this._corePindianHandReturnsOperations = corePindianHandReturnsOperations;
        this._coreProgramAtomicCardMovesOperations = coreProgramAtomicCardMovesOperations;
        this._coreProgramCardPaymentsOperations = coreProgramCardPaymentsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreSequentialDiscardReceiptsOperations = coreSequentialDiscardReceiptsOperations;
        this._coreHellMengpoProgramsOperations = coreHellMengpoProgramsOperations;
        this._coreNativeCardAppearanceOperations = coreNativeCardAppearanceOperations;
        this._coreParticipantHandProgramsOperations = coreParticipantHandProgramsOperations;
        this._corePhysicalDrawFacadesOperations = corePhysicalDrawFacadesOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreProgramLostHpChainedPaymentsOperations = coreProgramLostHpChainedPaymentsOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreCardMovementJournalCardMovementsMovementSequenceAccess = coreCardMovementJournalCardMovementsMovementSequenceAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
