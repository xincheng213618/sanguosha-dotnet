#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineDyingOwnerSkillReplacementRuntime
{
    private readonly ICoreDyingOwnerSkillReplacementContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreDyingOwnerSkillReplacementResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreDyingOwnerSkillReplacementCoreDyingCursorRuntimeOperations _coreDyingCursorRuntimeOperations;
    private readonly ICoreDyingOwnerSkillReplacementCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreDyingOwnerSkillReplacementCoreConversionStatesOperations _coreConversionStatesOperations;
    private readonly ICoreDyingOwnerSkillReplacementCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICoreDyingOwnerSkillReplacementCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreDyingOwnerSkillReplacementCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreDyingOwnerSkillReplacementKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineDyingOwnerSkillReplacementRuntime(ICoreDyingOwnerSkillReplacementContentRegistryOperations contentRegistryOperations, ICoreDyingOwnerSkillReplacementResolutionStackOperations resolutionStackOperations, ICoreDyingOwnerSkillReplacementCoreDyingCursorRuntimeOperations coreDyingCursorRuntimeOperations, ICoreDyingOwnerSkillReplacementCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreDyingOwnerSkillReplacementCoreConversionStatesOperations coreConversionStatesOperations, ICoreDyingOwnerSkillReplacementCoreNationalSkillsOperations coreNationalSkillsOperations, ICoreDyingOwnerSkillReplacementCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreDyingOwnerSkillReplacementCoreRuntimeOperations coreRuntimeOperations, ICoreDyingOwnerSkillReplacementKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreConversionStatesOperations = coreConversionStatesOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
