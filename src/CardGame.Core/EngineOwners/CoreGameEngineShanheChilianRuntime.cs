#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineShanheChilianRuntime
{
    private readonly ICoreShanheChilianContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreShanheChilianResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreShanheChilianCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICoreShanheChilianCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreShanheChilianCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineShanheChilianRuntime(ICoreShanheChilianContentRegistryOperations contentRegistryOperations, ICoreShanheChilianResolutionStackOperations resolutionStackOperations, ICoreShanheChilianCoreNationalSkillsOperations coreNationalSkillsOperations, ICoreShanheChilianCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreShanheChilianCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
