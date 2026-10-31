#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineWushuangDeckCardsRuntime : global::CardGame.Core.IWushuangDeckCardsHost
{
    private readonly ICoreWushuangDeckCardsCardZonesOperations _cardZonesOperations;
    private readonly ICoreWushuangDeckCardsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreWushuangDeckCardsCoreBoundCardMovementContinuationOperations _coreBoundCardMovementContinuationOperations;
    private readonly ICoreWushuangDeckCardsCoreProgramAtomicCardMovesOperations _coreProgramAtomicCardMovesOperations;
    private readonly ICoreWushuangDeckCardsCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreWushuangDeckCardsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreWushuangDeckCardsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreWushuangDeckCardsCorePhysicalDrawFacadesOperations _corePhysicalDrawFacadesOperations;
    private readonly ICoreWushuangDeckCardsCoreWushuangModeProjectionOperations _coreWushuangModeProjectionOperations;
    private readonly ICoreWushuangDeckCardsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess _coreCardMovementJournalCardMovementsMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineWushuangDeckCardsRuntime(ICoreWushuangDeckCardsCardZonesOperations cardZonesOperations, ICoreWushuangDeckCardsResolutionStackOperations resolutionStackOperations, ICoreWushuangDeckCardsCoreBoundCardMovementContinuationOperations coreBoundCardMovementContinuationOperations, ICoreWushuangDeckCardsCoreProgramAtomicCardMovesOperations coreProgramAtomicCardMovesOperations, ICoreWushuangDeckCardsCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreWushuangDeckCardsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreWushuangDeckCardsCoreRuntimeOperations coreRuntimeOperations, ICoreWushuangDeckCardsCorePhysicalDrawFacadesOperations corePhysicalDrawFacadesOperations, ICoreWushuangDeckCardsCoreWushuangModeProjectionOperations coreWushuangModeProjectionOperations, ICoreWushuangDeckCardsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess coreCardMovementJournalCardMovementsMovementSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreBoundCardMovementContinuationOperations = coreBoundCardMovementContinuationOperations;
        this._coreProgramAtomicCardMovesOperations = coreProgramAtomicCardMovesOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._corePhysicalDrawFacadesOperations = corePhysicalDrawFacadesOperations;
        this._coreWushuangModeProjectionOperations = coreWushuangModeProjectionOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreCardMovementJournalCardMovementsMovementSequenceAccess = coreCardMovementJournalCardMovementsMovementSequenceAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
