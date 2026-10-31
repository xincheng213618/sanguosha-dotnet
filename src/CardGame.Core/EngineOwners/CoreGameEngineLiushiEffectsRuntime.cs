#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineLiushiEffectsRuntime : global::CardGame.Core.ILiushiEffectHost
{
    private readonly ICoreLiushiEffectsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreLiushiEffectsCoreAttributedMarkerEventsOperations _coreAttributedMarkerEventsOperations;
    private readonly ICoreLiushiEffectsCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICoreLiushiEffectsCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreLiushiEffectsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreLiushiEffectsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreLiushiEffectsCoreNativeDamageHandlesOperations _coreNativeDamageHandlesOperations;
    private readonly ICoreLiushiEffectsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineLiushiEffectsRuntime(ICoreLiushiEffectsResolutionStackOperations resolutionStackOperations, ICoreLiushiEffectsCoreAttributedMarkerEventsOperations coreAttributedMarkerEventsOperations, ICoreLiushiEffectsCoreNationalSkillsOperations coreNationalSkillsOperations, ICoreLiushiEffectsCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreLiushiEffectsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreLiushiEffectsCoreRuntimeOperations coreRuntimeOperations, ICoreLiushiEffectsCoreNativeDamageHandlesOperations coreNativeDamageHandlesOperations, ICoreLiushiEffectsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAttributedMarkerEventsOperations = coreAttributedMarkerEventsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreNativeDamageHandlesOperations = coreNativeDamageHandlesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
