#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterDingShangWanOlRuntime : global::CardGame.Core.IDingShangWanNativeFacts, global::CardGame.Core.IDingShangWanNativeOperations
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuangQuanMouEntries _characterHuangQuanMouOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterYanPuOlEntries _characterYanPuOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries _coreDyingCursorRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAfterTurnEndedProgramsEntries _coreAfterTurnEndedProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineObserverSubtreesEntries _coreObserverSubtreesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries _coreProgramAtomicCardMovesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries _coreProgramAttackAttemptOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRecoveryReplacementDamageRidesEntries _coreRecoveryReplacementDamageRidesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineResponseCompletionEntries _coreResponseCompletionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess _coreCardMovementJournalMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterDingShangWanOlRuntime(global::CardGame.Core.RuntimeEntries.CharacterHuangQuanMouEntries characterHuangQuanMouOperations,global::CardGame.Core.RuntimeEntries.CharacterYanPuOlEntries characterYanPuOlOperations,global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries coreDyingCursorRuntimeOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineAfterTurnEndedProgramsEntries coreAfterTurnEndedProgramsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineObserverSubtreesEntries coreObserverSubtreesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries coreProgramAtomicCardMovesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries coreProgramAttackAttemptOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineRecoveryReplacementDamageRidesEntries coreRecoveryReplacementDamageRidesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineResponseCompletionEntries coreResponseCompletionOperations,global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations,global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess coreCardMovementJournalMovementSequenceAccess,global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess,global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterHuangQuanMouOperations = characterHuangQuanMouOperations;
        this._characterYanPuOlOperations = characterYanPuOlOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreAfterTurnEndedProgramsOperations = coreAfterTurnEndedProgramsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreObserverSubtreesOperations = coreObserverSubtreesOperations;
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
