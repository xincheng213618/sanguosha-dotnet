#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramIronChainTargetsRuntime
{
    private readonly RuntimeEntries.CharacterPangTongMouMobileEntries _characterPangTongMouMobileOperations;
    private readonly RuntimeEntries.CoreGameEngineRuleQueriesEntries _coreRuleQueriesOperations;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineProgramIronChainTargetsRuntime(RuntimeEntries.CharacterPangTongMouMobileEntries characterPangTongMouMobileOperations, RuntimeEntries.CoreGameEngineRuleQueriesEntries coreRuleQueriesOperations, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterPangTongMouMobileOperations = characterPangTongMouMobileOperations;
        this._coreRuleQueriesOperations = coreRuleQueriesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
