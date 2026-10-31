#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineShanheS5YuanshuRuntime
{
    private readonly ICoreShanheS5YuanshuContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreShanheS5YuanshuCoreProgramCardPoliciesOperations _coreProgramCardPoliciesOperations;
    private readonly ICoreShanheS5YuanshuCoreProgramDodgeResponsesOperations _coreProgramDodgeResponsesOperations;
    private readonly ICoreShanheS5YuanshuCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICoreShanheS5YuanshuCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineShanheS5YuanshuRuntime(ICoreShanheS5YuanshuContentRegistryOperations contentRegistryOperations, ICoreShanheS5YuanshuCoreProgramCardPoliciesOperations coreProgramCardPoliciesOperations, ICoreShanheS5YuanshuCoreProgramDodgeResponsesOperations coreProgramDodgeResponsesOperations, ICoreShanheS5YuanshuCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICoreShanheS5YuanshuCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreProgramDodgeResponsesOperations = coreProgramDodgeResponsesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
