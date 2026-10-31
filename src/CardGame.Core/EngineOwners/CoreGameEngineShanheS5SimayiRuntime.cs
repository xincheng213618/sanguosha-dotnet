#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineShanheS5SimayiRuntime : global::CardGame.Core.IShanheS5BaiyinHost
{
    private readonly ICoreShanheS5SimayiCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICoreShanheS5SimayiCoreProgramDiscardOriginsOperations _coreProgramDiscardOriginsOperations;
    private readonly ICoreShanheS5SimayiCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreShanheS5SimayiCoreProgramSkillChangesOperations _coreProgramSkillChangesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess _coreActualTurnProgressTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineShanheS5SimayiRuntime(ICoreShanheS5SimayiCoreNationalSkillsOperations coreNationalSkillsOperations, ICoreShanheS5SimayiCoreProgramDiscardOriginsOperations coreProgramDiscardOriginsOperations, ICoreShanheS5SimayiCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreShanheS5SimayiCoreProgramSkillChangesOperations coreProgramSkillChangesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess coreActualTurnProgressTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramDiscardOriginsOperations = coreProgramDiscardOriginsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreProgramSkillChangesOperations = coreProgramSkillChangesOperations;
        this._coreActualTurnProgressTurnNumberAccess = coreActualTurnProgressTurnNumberAccess;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
