#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterWangLingMouRuntime : global::CardGame.Core.IMouWangLingFacts, global::CardGame.Core.IMouWangLingOperations
{
    private readonly ICharacterWangLingMouCharacterHuangQuanMouOperations _characterHuangQuanMouOperations;
    private readonly ICharacterWangLingMouCardZonesOperations _cardZonesOperations;
    private readonly ICharacterWangLingMouContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterWangLingMouResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterWangLingMouCoreAdvancedLifecycleOperations _coreAdvancedLifecycleOperations;
    private readonly ICharacterWangLingMouCoreAfterTurnEndedProgramsOperations _coreAfterTurnEndedProgramsOperations;
    private readonly ICharacterWangLingMouCoreCardMovementProgramsOperations _coreCardMovementProgramsOperations;
    private readonly ICharacterWangLingMouCoreCharacterStateProgramsOperations _coreCharacterStateProgramsOperations;
    private readonly ICharacterWangLingMouCoreHpChangeProgramsOperations _coreHpChangeProgramsOperations;
    private readonly ICharacterWangLingMouCoreNativeLogicalDiscardsOperations _coreNativeLogicalDiscardsOperations;
    private readonly ICharacterWangLingMouCoreProgramAtomicCardMovesOperations _coreProgramAtomicCardMovesOperations;
    private readonly ICharacterWangLingMouCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICharacterWangLingMouCoreSkillOwnershipWindowsOperations _coreSkillOwnershipWindowsOperations;
    private readonly ICharacterWangLingMouCoreSkillProgramsOperations _coreSkillProgramsOperations;
    private readonly ICharacterWangLingMouCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICharacterWangLingMouCorePlayerViewProjectionOperations _corePlayerViewProjectionOperations;
    private readonly ICharacterWangLingMouCoreRecoveryReplacementOperations _coreRecoveryReplacementOperations;
    private readonly ICharacterWangLingMouCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess _coreAiBrainsAiBrainsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess _coreCardMovementJournalCardMovementsMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionPlayerCountAccess _coreMatchDefinitionPlayerCountAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterWangLingMouRuntime(ICharacterWangLingMouCharacterHuangQuanMouOperations characterHuangQuanMouOperations, ICharacterWangLingMouCardZonesOperations cardZonesOperations, ICharacterWangLingMouContentRegistryOperations contentRegistryOperations, ICharacterWangLingMouResolutionStackOperations resolutionStackOperations, ICharacterWangLingMouCoreAdvancedLifecycleOperations coreAdvancedLifecycleOperations, ICharacterWangLingMouCoreAfterTurnEndedProgramsOperations coreAfterTurnEndedProgramsOperations, ICharacterWangLingMouCoreCardMovementProgramsOperations coreCardMovementProgramsOperations, ICharacterWangLingMouCoreCharacterStateProgramsOperations coreCharacterStateProgramsOperations, ICharacterWangLingMouCoreHpChangeProgramsOperations coreHpChangeProgramsOperations, ICharacterWangLingMouCoreNativeLogicalDiscardsOperations coreNativeLogicalDiscardsOperations, ICharacterWangLingMouCoreProgramAtomicCardMovesOperations coreProgramAtomicCardMovesOperations, ICharacterWangLingMouCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICharacterWangLingMouCoreSkillOwnershipWindowsOperations coreSkillOwnershipWindowsOperations, ICharacterWangLingMouCoreSkillProgramsOperations coreSkillProgramsOperations, ICharacterWangLingMouCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICharacterWangLingMouCorePlayerViewProjectionOperations corePlayerViewProjectionOperations, ICharacterWangLingMouCoreRecoveryReplacementOperations coreRecoveryReplacementOperations, ICharacterWangLingMouCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess coreAiBrainsAiBrainsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess coreCardMovementJournalCardMovementsMovementSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionPlayerCountAccess coreMatchDefinitionPlayerCountAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterHuangQuanMouOperations = characterHuangQuanMouOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAdvancedLifecycleOperations = coreAdvancedLifecycleOperations;
        this._coreAfterTurnEndedProgramsOperations = coreAfterTurnEndedProgramsOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreCharacterStateProgramsOperations = coreCharacterStateProgramsOperations;
        this._coreHpChangeProgramsOperations = coreHpChangeProgramsOperations;
        this._coreNativeLogicalDiscardsOperations = coreNativeLogicalDiscardsOperations;
        this._coreProgramAtomicCardMovesOperations = coreProgramAtomicCardMovesOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreSkillOwnershipWindowsOperations = coreSkillOwnershipWindowsOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._corePlayerViewProjectionOperations = corePlayerViewProjectionOperations;
        this._coreRecoveryReplacementOperations = coreRecoveryReplacementOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreAiBrainsAiBrainsAccess = coreAiBrainsAiBrainsAccess;
        this._coreCardMovementJournalCardMovementsMovementSequenceAccess = coreCardMovementJournalCardMovementsMovementSequenceAccess;
        this._coreMatchDefinitionPlayerCountAccess = coreMatchDefinitionPlayerCountAccess;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
