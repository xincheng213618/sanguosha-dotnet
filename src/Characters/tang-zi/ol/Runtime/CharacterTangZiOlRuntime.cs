#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterTangZiOlRuntime : global::CardGame.Core.ISelectionWoundedQueries
{
    private readonly ICharacterTangZiOlCoreTreatedWoundedOperations _coreTreatedWoundedOperations;
    private readonly ICharacterTangZiOlCoreProgramSelectionModulesOperations _coreProgramSelectionModulesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterTangZiOlRuntime(ICharacterTangZiOlCoreTreatedWoundedOperations coreTreatedWoundedOperations, ICharacterTangZiOlCoreProgramSelectionModulesOperations coreProgramSelectionModulesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._coreTreatedWoundedOperations = coreTreatedWoundedOperations;
        this._coreProgramSelectionModulesOperations = coreProgramSelectionModulesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
