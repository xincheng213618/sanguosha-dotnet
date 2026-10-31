#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreProgramPairedColorResponseAiRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageCounterpartAcquisitionEntries _coreDamageCounterpartAcquisitionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePairedColorResponsesEntries _corePairedColorResponsesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreProgramPairedColorResponseAiRuntime(global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageCounterpartAcquisitionEntries coreDamageCounterpartAcquisitionOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePairedColorResponsesEntries corePairedColorResponsesOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._coreDamageCounterpartAcquisitionOperations = coreDamageCounterpartAcquisitionOperations;
        this._corePairedColorResponsesOperations = corePairedColorResponsesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
