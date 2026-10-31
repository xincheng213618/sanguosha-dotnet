#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineShanheS5SimayiRuntime : global::CardGame.Core.IShanheS5BaiyinHost
{
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramDiscardOriginsEntries _coreProgramDiscardOriginsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramSkillChangesEntries _coreProgramSkillChangesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess _coreActualTurnProgressTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineShanheS5SimayiRuntime(global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramDiscardOriginsEntries coreProgramDiscardOriginsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramSkillChangesEntries coreProgramSkillChangesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess coreActualTurnProgressTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
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
