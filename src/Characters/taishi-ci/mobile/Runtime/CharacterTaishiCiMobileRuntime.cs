#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CharacterTaishiCiMobileRuntime
{
    private readonly RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramPersistentStateEntries _coreProgramPersistentStateOperations;
    private readonly RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterTaishiCiMobileRuntime(RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations, RuntimeEntries.CoreGameEngineProgramPersistentStateEntries coreProgramPersistentStateOperations, RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramPersistentStateOperations = coreProgramPersistentStateOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
