#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineShownBoundGiftsRuntime : global::CardGame.Core.IShownBoundGiftProgramHost
{
    private readonly ICoreShownBoundGiftsCharacterHuangQuanMouOperations _characterHuangQuanMouOperations;
    private readonly ICoreShownBoundGiftsCardZonesOperations _cardZonesOperations;
    private readonly ICoreShownBoundGiftsTurnCardUseEffectsOperations _turnCardUseEffectsOperations;
    private readonly ICoreShownBoundGiftsCoreBoundCardMovementContinuationOperations _coreBoundCardMovementContinuationOperations;
    private readonly ICoreShownBoundGiftsCoreCardUseModulesOperations _coreCardUseModulesOperations;
    private readonly ICoreShownBoundGiftsCoreProgramAtomicCardMovesOperations _coreProgramAtomicCardMovesOperations;
    private readonly ICoreShownBoundGiftsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreShownBoundGiftsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreShownBoundGiftsCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICoreShownBoundGiftsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess _coreCardMovementJournalCardMovementsMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineShownBoundGiftsRuntime(ICoreShownBoundGiftsCharacterHuangQuanMouOperations characterHuangQuanMouOperations, ICoreShownBoundGiftsCardZonesOperations cardZonesOperations, ICoreShownBoundGiftsTurnCardUseEffectsOperations turnCardUseEffectsOperations, ICoreShownBoundGiftsCoreBoundCardMovementContinuationOperations coreBoundCardMovementContinuationOperations, ICoreShownBoundGiftsCoreCardUseModulesOperations coreCardUseModulesOperations, ICoreShownBoundGiftsCoreProgramAtomicCardMovesOperations coreProgramAtomicCardMovesOperations, ICoreShownBoundGiftsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreShownBoundGiftsCoreRuntimeOperations coreRuntimeOperations, ICoreShownBoundGiftsCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICoreShownBoundGiftsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess coreCardMovementJournalCardMovementsMovementSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterHuangQuanMouOperations = characterHuangQuanMouOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._turnCardUseEffectsOperations = turnCardUseEffectsOperations;
        this._coreBoundCardMovementContinuationOperations = coreBoundCardMovementContinuationOperations;
        this._coreCardUseModulesOperations = coreCardUseModulesOperations;
        this._coreProgramAtomicCardMovesOperations = coreProgramAtomicCardMovesOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess = coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess;
        this._coreCardMovementJournalCardMovementsMovementSequenceAccess = coreCardMovementJournalCardMovementsMovementSequenceAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
