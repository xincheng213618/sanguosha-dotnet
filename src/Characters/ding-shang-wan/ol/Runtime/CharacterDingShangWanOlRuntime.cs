#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterDingShangWanOlRuntime : global::CardGame.Core.IDingShangWanNativeFacts, global::CardGame.Core.IDingShangWanNativeOperations
{
    private readonly ICharacterDingShangWanOlCharacterHuangQuanMouOperations _characterHuangQuanMouOperations;
    private readonly ICharacterDingShangWanOlCharacterYanPuOlOperations _characterYanPuOlOperations;
    private readonly ICharacterDingShangWanOlCoreDyingCursorRuntimeOperations _coreDyingCursorRuntimeOperations;
    private readonly ICharacterDingShangWanOlCoreAfterTurnEndedProgramsOperations _coreAfterTurnEndedProgramsOperations;
    private readonly ICharacterDingShangWanOlCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICharacterDingShangWanOlCoreObserverSubtreesOperations _coreObserverSubtreesOperations;
    private readonly ICharacterDingShangWanOlCorePrivateOfferReturnsOperations _corePrivateOfferReturnsOperations;
    private readonly ICharacterDingShangWanOlCoreProgramAtomicCardMovesOperations _coreProgramAtomicCardMovesOperations;
    private readonly ICharacterDingShangWanOlCoreProgramAttackAttemptOperations _coreProgramAttackAttemptOperations;
    private readonly ICharacterDingShangWanOlCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterDingShangWanOlCoreRecoveryReplacementDamageRidesOperations _coreRecoveryReplacementDamageRidesOperations;
    private readonly ICharacterDingShangWanOlCoreResponseCompletionOperations _coreResponseCompletionOperations;
    private readonly ICharacterDingShangWanOlCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess _coreCardMovementJournalMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterDingShangWanOlRuntime(ICharacterDingShangWanOlCharacterHuangQuanMouOperations characterHuangQuanMouOperations, ICharacterDingShangWanOlCharacterYanPuOlOperations characterYanPuOlOperations, ICharacterDingShangWanOlCoreDyingCursorRuntimeOperations coreDyingCursorRuntimeOperations, ICharacterDingShangWanOlCoreAfterTurnEndedProgramsOperations coreAfterTurnEndedProgramsOperations, ICharacterDingShangWanOlCoreNationalSkillsOperations coreNationalSkillsOperations, ICharacterDingShangWanOlCoreObserverSubtreesOperations coreObserverSubtreesOperations, ICharacterDingShangWanOlCorePrivateOfferReturnsOperations corePrivateOfferReturnsOperations, ICharacterDingShangWanOlCoreProgramAtomicCardMovesOperations coreProgramAtomicCardMovesOperations, ICharacterDingShangWanOlCoreProgramAttackAttemptOperations coreProgramAttackAttemptOperations, ICharacterDingShangWanOlCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterDingShangWanOlCoreRecoveryReplacementDamageRidesOperations coreRecoveryReplacementDamageRidesOperations, ICharacterDingShangWanOlCoreResponseCompletionOperations coreResponseCompletionOperations, ICharacterDingShangWanOlCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess coreCardMovementJournalMovementSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterHuangQuanMouOperations = characterHuangQuanMouOperations;
        this._characterYanPuOlOperations = characterYanPuOlOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreAfterTurnEndedProgramsOperations = coreAfterTurnEndedProgramsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreObserverSubtreesOperations = coreObserverSubtreesOperations;
        this._corePrivateOfferReturnsOperations = corePrivateOfferReturnsOperations;
        this._coreProgramAtomicCardMovesOperations = coreProgramAtomicCardMovesOperations;
        this._coreProgramAttackAttemptOperations = coreProgramAttackAttemptOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRecoveryReplacementDamageRidesOperations = coreRecoveryReplacementDamageRidesOperations;
        this._coreResponseCompletionOperations = coreResponseCompletionOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreCardMovementJournalMovementSequenceAccess = coreCardMovementJournalMovementSequenceAccess;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
