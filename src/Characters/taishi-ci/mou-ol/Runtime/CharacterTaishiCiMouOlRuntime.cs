#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CharacterTaishiCiMouOlRuntime
{
    private readonly ICharacterTaishiCiMouOlContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterTaishiCiMouOlCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterTaishiCiMouOlRuntime(ICharacterTaishiCiMouOlContentRegistryOperations contentRegistryOperations, ICharacterTaishiCiMouOlCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
