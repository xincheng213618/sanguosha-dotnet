#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineCardUsePaymentsRuntime
{
    private readonly ICoreCardUsePaymentsCharacterDiaoChanLeOperations _characterDiaoChanLeOperations;
    private readonly ICoreCardUsePaymentsCardZonesOperations _cardZonesOperations;
    private readonly ICoreCardUsePaymentsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreCardUsePaymentsCoreAdvancedLifecycleOperations _coreAdvancedLifecycleOperations;
    private readonly ICoreCardUsePaymentsCoreAlternativeViewAsCostsOperations _coreAlternativeViewAsCostsOperations;
    private readonly ICoreCardUsePaymentsCoreCardUseLifecycleOperations _coreCardUseLifecycleOperations;
    private readonly ICoreCardUsePaymentsCoreEphemeralCardMaterialsOperations _coreEphemeralCardMaterialsOperations;
    private readonly ICoreCardUsePaymentsCoreProgramAtomicCardMovesOperations _coreProgramAtomicCardMovesOperations;
    private readonly ICoreCardUsePaymentsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreCardUsePaymentsCoreWushuangJinzhiOperations _coreWushuangJinzhiOperations;
    private readonly ICoreCardUsePaymentsCoreNativeOwnedCardQueriesOperations _coreNativeOwnedCardQueriesOperations;
    private readonly ICoreCardUsePaymentsCorePhysicalMovementFacadesOperations _corePhysicalMovementFacadesOperations;
    private readonly ICoreCardUsePaymentsCoreProgramCardUsePaymentsOperations _coreProgramCardUsePaymentsOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineCardUsePaymentsRuntime(ICoreCardUsePaymentsCharacterDiaoChanLeOperations characterDiaoChanLeOperations, ICoreCardUsePaymentsCardZonesOperations cardZonesOperations, ICoreCardUsePaymentsResolutionStackOperations resolutionStackOperations, ICoreCardUsePaymentsCoreAdvancedLifecycleOperations coreAdvancedLifecycleOperations, ICoreCardUsePaymentsCoreAlternativeViewAsCostsOperations coreAlternativeViewAsCostsOperations, ICoreCardUsePaymentsCoreCardUseLifecycleOperations coreCardUseLifecycleOperations, ICoreCardUsePaymentsCoreEphemeralCardMaterialsOperations coreEphemeralCardMaterialsOperations, ICoreCardUsePaymentsCoreProgramAtomicCardMovesOperations coreProgramAtomicCardMovesOperations, ICoreCardUsePaymentsCoreRuntimeOperations coreRuntimeOperations, ICoreCardUsePaymentsCoreWushuangJinzhiOperations coreWushuangJinzhiOperations, ICoreCardUsePaymentsCoreNativeOwnedCardQueriesOperations coreNativeOwnedCardQueriesOperations, ICoreCardUsePaymentsCorePhysicalMovementFacadesOperations corePhysicalMovementFacadesOperations, ICoreCardUsePaymentsCoreProgramCardUsePaymentsOperations coreProgramCardUsePaymentsOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterDiaoChanLeOperations = characterDiaoChanLeOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAdvancedLifecycleOperations = coreAdvancedLifecycleOperations;
        this._coreAlternativeViewAsCostsOperations = coreAlternativeViewAsCostsOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreEphemeralCardMaterialsOperations = coreEphemeralCardMaterialsOperations;
        this._coreProgramAtomicCardMovesOperations = coreProgramAtomicCardMovesOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreWushuangJinzhiOperations = coreWushuangJinzhiOperations;
        this._coreNativeOwnedCardQueriesOperations = coreNativeOwnedCardQueriesOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._coreProgramCardUsePaymentsOperations = coreProgramCardUsePaymentsOperations;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
