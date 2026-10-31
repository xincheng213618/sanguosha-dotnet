#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CharacterDengAiHappyGodRuntime
{
    private readonly ICharacterDengAiHappyGodCharacterDengAiAnniversaryGodOperations _characterDengAiAnniversaryGodOperations;
    private readonly ICharacterDengAiHappyGodCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICharacterDengAiHappyGodCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICharacterDengAiHappyGodCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterDengAiHappyGodRuntime(ICharacterDengAiHappyGodCharacterDengAiAnniversaryGodOperations characterDengAiAnniversaryGodOperations, ICharacterDengAiHappyGodCoreNationalSkillsOperations coreNationalSkillsOperations, ICharacterDengAiHappyGodCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICharacterDengAiHappyGodCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterDengAiAnniversaryGodOperations = characterDengAiAnniversaryGodOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
