#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CharacterGuanYuHappyGodRuntime
{
    private readonly ICharacterGuanYuHappyGodContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterGuanYuHappyGodResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterGuanYuHappyGodCoreCardUseLifecycleOperations _coreCardUseLifecycleOperations;
    private readonly ICharacterGuanYuHappyGodCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICharacterGuanYuHappyGodCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess _coreActualRoundProgressRoundNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterGuanYuHappyGodRuntime(ICharacterGuanYuHappyGodContentRegistryOperations contentRegistryOperations, ICharacterGuanYuHappyGodResolutionStackOperations resolutionStackOperations, ICharacterGuanYuHappyGodCoreCardUseLifecycleOperations coreCardUseLifecycleOperations, ICharacterGuanYuHappyGodCoreNationalSkillsOperations coreNationalSkillsOperations, ICharacterGuanYuHappyGodCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess coreActualRoundProgressRoundNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreActualRoundProgressRoundNumberAccess = coreActualRoundProgressRoundNumberAccess;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
