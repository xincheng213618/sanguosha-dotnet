#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineTurnEndDamageMarkerRuntime : global::CardGame.Core.ITurnEndDamageMarkerProgramHost
{
    private readonly ICoreTurnEndDamageMarkerContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreTurnEndDamageMarkerResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreTurnEndDamageMarkerCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreTurnEndDamageMarkerCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICoreTurnEndDamageMarkerCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreTurnEndDamageMarkerKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineTurnEndDamageMarkerRuntime(ICoreTurnEndDamageMarkerContentRegistryOperations contentRegistryOperations, ICoreTurnEndDamageMarkerResolutionStackOperations resolutionStackOperations, ICoreTurnEndDamageMarkerCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreTurnEndDamageMarkerCoreNationalSkillsOperations coreNationalSkillsOperations, ICoreTurnEndDamageMarkerCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreTurnEndDamageMarkerKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
