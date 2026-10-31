#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreProgramPairedColorResponseAiRuntime
{
    private readonly ICoreProgramPairedColorResponseAiCoreDamageCounterpartAcquisitionOperations _coreDamageCounterpartAcquisitionOperations;
    private readonly ICoreProgramPairedColorResponseAiCorePairedColorResponsesOperations _corePairedColorResponsesOperations;
    private readonly ICoreProgramPairedColorResponseAiCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreProgramPairedColorResponseAiRuntime(ICoreProgramPairedColorResponseAiCoreDamageCounterpartAcquisitionOperations coreDamageCounterpartAcquisitionOperations, ICoreProgramPairedColorResponseAiCorePairedColorResponsesOperations corePairedColorResponsesOperations, ICoreProgramPairedColorResponseAiCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._coreDamageCounterpartAcquisitionOperations = coreDamageCounterpartAcquisitionOperations;
        this._corePairedColorResponsesOperations = corePairedColorResponsesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
