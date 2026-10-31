#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreProgramPairedColorResponseAiRuntime
{
    private readonly RuntimeEntries.CoreGameEngineDamageCounterpartAcquisitionEntries _coreDamageCounterpartAcquisitionOperations;
    private readonly RuntimeEntries.CoreGameEnginePairedColorResponsesEntries _corePairedColorResponsesOperations;
    private readonly RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreProgramPairedColorResponseAiRuntime(RuntimeEntries.CoreGameEngineDamageCounterpartAcquisitionEntries coreDamageCounterpartAcquisitionOperations, RuntimeEntries.CoreGameEnginePairedColorResponsesEntries corePairedColorResponsesOperations, RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._coreDamageCounterpartAcquisitionOperations = coreDamageCounterpartAcquisitionOperations;
        this._corePairedColorResponsesOperations = corePairedColorResponsesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
