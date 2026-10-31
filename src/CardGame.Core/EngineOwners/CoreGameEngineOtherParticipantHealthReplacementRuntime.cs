#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineOtherParticipantHealthReplacementRuntime
{
    private readonly ICoreOtherParticipantHealthReplacementContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreOtherParticipantHealthReplacementCoreGuanduGeneralQualificationOperations _coreGuanduGeneralQualificationOperations;
    private readonly ICoreOtherParticipantHealthReplacementCoreProgramCardPoliciesOperations _coreProgramCardPoliciesOperations;
    private readonly ICoreOtherParticipantHealthReplacementCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineOtherParticipantHealthReplacementRuntime(ICoreOtherParticipantHealthReplacementContentRegistryOperations contentRegistryOperations, ICoreOtherParticipantHealthReplacementCoreGuanduGeneralQualificationOperations coreGuanduGeneralQualificationOperations, ICoreOtherParticipantHealthReplacementCoreProgramCardPoliciesOperations coreProgramCardPoliciesOperations, ICoreOtherParticipantHealthReplacementCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreGuanduGeneralQualificationOperations = coreGuanduGeneralQualificationOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
