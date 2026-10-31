#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CharacterTangZiOlRuntime : ISelectionWoundedQueries
{
    private readonly RuntimeEntries.CoreGameEngineTreatedWoundedEntries _coreTreatedWoundedOperations;
    private readonly ICharacterTangZiOlCoreProgramSelectionModulesOperations _coreProgramSelectionModulesOperations;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterTangZiOlRuntime(RuntimeEntries.CoreGameEngineTreatedWoundedEntries coreTreatedWoundedOperations, ICharacterTangZiOlCoreProgramSelectionModulesOperations coreProgramSelectionModulesOperations, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._coreTreatedWoundedOperations = coreTreatedWoundedOperations;
        this._coreProgramSelectionModulesOperations = coreProgramSelectionModulesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
