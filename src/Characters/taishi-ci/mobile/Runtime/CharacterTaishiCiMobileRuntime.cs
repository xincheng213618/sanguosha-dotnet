#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CharacterTaishiCiMobileRuntime
{
    private readonly ICharacterTaishiCiMobileCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICharacterTaishiCiMobileCoreProgramPersistentStateOperations _coreProgramPersistentStateOperations;
    private readonly ICharacterTaishiCiMobileCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterTaishiCiMobileRuntime(ICharacterTaishiCiMobileCoreNationalSkillsOperations coreNationalSkillsOperations, ICharacterTaishiCiMobileCoreProgramPersistentStateOperations coreProgramPersistentStateOperations, ICharacterTaishiCiMobileCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramPersistentStateOperations = coreProgramPersistentStateOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
