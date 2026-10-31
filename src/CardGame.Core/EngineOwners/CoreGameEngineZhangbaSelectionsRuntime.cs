#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineZhangbaSelectionsRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingSuitAndCompletedTopEntries _coreAlternatingSuitAndCompletedTopOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineFinalTargetSlashEntries _coreFinalTargetSlashOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTurnRedSlashBenefitsEntries _coreTurnRedSlashBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeResponseMaterialQueriesEntries _coreNativeResponseMaterialQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeTargetLegalityEntries _coreNativeTargetLegalityOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreProgramParticipantPhaseOperationsEntries _coreProgramParticipantPhaseOperationsOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineZhangbaSelectionsRuntime(global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingSuitAndCompletedTopEntries coreAlternatingSuitAndCompletedTopOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineFinalTargetSlashEntries coreFinalTargetSlashOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineTurnRedSlashBenefitsEntries coreTurnRedSlashBenefitsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeResponseMaterialQueriesEntries coreNativeResponseMaterialQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreNativeTargetLegalityEntries coreNativeTargetLegalityOperations, global::CardGame.Core.RuntimeEntries.CoreProgramParticipantPhaseOperationsEntries coreProgramParticipantPhaseOperationsOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._coreAlternatingSuitAndCompletedTopOperations = coreAlternatingSuitAndCompletedTopOperations;
        this._coreFinalTargetSlashOperations = coreFinalTargetSlashOperations;
        this._coreTurnRedSlashBenefitsOperations = coreTurnRedSlashBenefitsOperations;
        this._coreNativeResponseMaterialQueriesOperations = coreNativeResponseMaterialQueriesOperations;
        this._coreNativeTargetLegalityOperations = coreNativeTargetLegalityOperations;
        this._coreProgramParticipantPhaseOperationsOperations = coreProgramParticipantPhaseOperationsOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
