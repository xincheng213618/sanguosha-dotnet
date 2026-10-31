#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineSourceExecutionerYishiRuntime : global::CardGame.Core.ISourceExecutionerHost
{
    private readonly ICoreSourceExecutionerYishiCardZonesOperations _cardZonesOperations;
    private readonly ICoreSourceExecutionerYishiResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreSourceExecutionerYishiCoreDamagePreventionProgramsOperations _coreDamagePreventionProgramsOperations;
    private readonly ICoreSourceExecutionerYishiCoreProgramAttackAttemptOperations _coreProgramAttackAttemptOperations;
    private readonly ICoreSourceExecutionerYishiCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreSourceExecutionerYishiCoreSourceExecutionerHuanfuOperations _coreSourceExecutionerHuanfuOperations;
    private readonly ICoreSourceExecutionerYishiCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICoreSourceExecutionerYishiKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionOptionsAccess _coreMatchDefinitionModeDefinitionOptionsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineSourceExecutionerYishiRuntime(ICoreSourceExecutionerYishiCardZonesOperations cardZonesOperations, ICoreSourceExecutionerYishiResolutionStackOperations resolutionStackOperations, ICoreSourceExecutionerYishiCoreDamagePreventionProgramsOperations coreDamagePreventionProgramsOperations, ICoreSourceExecutionerYishiCoreProgramAttackAttemptOperations coreProgramAttackAttemptOperations, ICoreSourceExecutionerYishiCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreSourceExecutionerYishiCoreSourceExecutionerHuanfuOperations coreSourceExecutionerHuanfuOperations, ICoreSourceExecutionerYishiCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICoreSourceExecutionerYishiKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionOptionsAccess coreMatchDefinitionModeDefinitionOptionsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDamagePreventionProgramsOperations = coreDamagePreventionProgramsOperations;
        this._coreProgramAttackAttemptOperations = coreProgramAttackAttemptOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreSourceExecutionerHuanfuOperations = coreSourceExecutionerHuanfuOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchDefinitionModeDefinitionOptionsAccess = coreMatchDefinitionModeDefinitionOptionsAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
