#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreProgramFactionActionRewardRuntime
{
    private readonly ICoreProgramFactionActionRewardCharacterLuSuMobileShiOperations _characterLuSuMobileShiOperations;
    private readonly ICoreProgramFactionActionRewardContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreProgramFactionActionRewardResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreProgramFactionActionRewardCoreCardUseLifecycleOperations _coreCardUseLifecycleOperations;
    private readonly ICoreProgramFactionActionRewardCoreGouXiangArmorOperations _coreGouXiangArmorOperations;
    private readonly ICoreProgramFactionActionRewardCoreLordSkillProjectionsOperations _coreLordSkillProjectionsOperations;
    private readonly ICoreProgramFactionActionRewardCoreWushuangJinzhiOperations _coreWushuangJinzhiOperations;
    private readonly ICoreProgramFactionActionRewardCoreMatchSetupDefinitionsOperations _coreMatchSetupDefinitionsOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreProgramFactionActionRewardRuntime(ICoreProgramFactionActionRewardCharacterLuSuMobileShiOperations characterLuSuMobileShiOperations, ICoreProgramFactionActionRewardContentRegistryOperations contentRegistryOperations, ICoreProgramFactionActionRewardResolutionStackOperations resolutionStackOperations, ICoreProgramFactionActionRewardCoreCardUseLifecycleOperations coreCardUseLifecycleOperations, ICoreProgramFactionActionRewardCoreGouXiangArmorOperations coreGouXiangArmorOperations, ICoreProgramFactionActionRewardCoreLordSkillProjectionsOperations coreLordSkillProjectionsOperations, ICoreProgramFactionActionRewardCoreWushuangJinzhiOperations coreWushuangJinzhiOperations, ICoreProgramFactionActionRewardCoreMatchSetupDefinitionsOperations coreMatchSetupDefinitionsOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterLuSuMobileShiOperations = characterLuSuMobileShiOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreGouXiangArmorOperations = coreGouXiangArmorOperations;
        this._coreLordSkillProjectionsOperations = coreLordSkillProjectionsOperations;
        this._coreWushuangJinzhiOperations = coreWushuangJinzhiOperations;
        this._coreMatchSetupDefinitionsOperations = coreMatchSetupDefinitionsOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
