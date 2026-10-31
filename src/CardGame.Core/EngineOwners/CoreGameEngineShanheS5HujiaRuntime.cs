#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineShanheS5HujiaRuntime : global::CardGame.Core.IShanheS5HujiaHost, global::CardGame.Core.IShanheS5JianxiongHost
{
    private readonly ICoreShanheS5HujiaContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreShanheS5HujiaResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreShanheS5HujiaCoreDamageCursorRuntimeOperations _coreDamageCursorRuntimeOperations;
    private readonly ICoreShanheS5HujiaCoreLordSkillProjectionsOperations _coreLordSkillProjectionsOperations;
    private readonly ICoreShanheS5HujiaCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreShanheS5HujiaCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreShanheS5HujiaCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreShanheS5HujiaCoreProgramFactionActionRewardOperations _coreProgramFactionActionRewardOperations;
    private readonly ICoreShanheS5HujiaKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineShanheS5HujiaRuntime(ICoreShanheS5HujiaContentRegistryOperations contentRegistryOperations, ICoreShanheS5HujiaResolutionStackOperations resolutionStackOperations, ICoreShanheS5HujiaCoreDamageCursorRuntimeOperations coreDamageCursorRuntimeOperations, ICoreShanheS5HujiaCoreLordSkillProjectionsOperations coreLordSkillProjectionsOperations, ICoreShanheS5HujiaCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreShanheS5HujiaCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreShanheS5HujiaCoreRuntimeOperations coreRuntimeOperations, ICoreShanheS5HujiaCoreProgramFactionActionRewardOperations coreProgramFactionActionRewardOperations, ICoreShanheS5HujiaKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDamageCursorRuntimeOperations = coreDamageCursorRuntimeOperations;
        this._coreLordSkillProjectionsOperations = coreLordSkillProjectionsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreProgramFactionActionRewardOperations = coreProgramFactionActionRewardOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
