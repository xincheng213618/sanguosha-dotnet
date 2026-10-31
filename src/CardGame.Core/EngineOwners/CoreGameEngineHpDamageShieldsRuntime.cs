#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineHpDamageShieldsRuntime : global::CardGame.Core.IHpDamageShieldProgramHost
{
    private readonly ICoreHpDamageShieldsContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreHpDamageShieldsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreHpDamageShieldsCoreCardMovementProgramsOperations _coreCardMovementProgramsOperations;
    private readonly ICoreHpDamageShieldsCoreCharacterStateProgramsOperations _coreCharacterStateProgramsOperations;
    private readonly ICoreHpDamageShieldsCoreHpChangeProgramsOperations _coreHpChangeProgramsOperations;
    private readonly ICoreHpDamageShieldsCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICoreHpDamageShieldsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreHpDamageShieldsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreHpDamageShieldsCoreSkillOwnershipWindowsOperations _coreSkillOwnershipWindowsOperations;
    private readonly ICoreHpDamageShieldsCoreSkillProgramsOperations _coreSkillProgramsOperations;
    private readonly ICoreHpDamageShieldsCoreMatchLogOperations _coreMatchLogOperations;
    private readonly ICoreHpDamageShieldsCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICoreHpDamageShieldsCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICoreHpDamageShieldsCorePlayerViewProjectionOperations _corePlayerViewProjectionOperations;
    private readonly ICoreHpDamageShieldsCoreRecoveryReplacementOperations _coreRecoveryReplacementOperations;
    private readonly ICoreHpDamageShieldsCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICoreHpDamageShieldsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess _coreAiBrainsAiBrainsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineHpChangeProgramsPendingHpChangesAccess _coreGameEngineHpChangeProgramsPendingHpChangesAccess;
    internal CoreGameEngineHpDamageShieldsRuntime(ICoreHpDamageShieldsContentRegistryOperations contentRegistryOperations, ICoreHpDamageShieldsResolutionStackOperations resolutionStackOperations, ICoreHpDamageShieldsCoreCardMovementProgramsOperations coreCardMovementProgramsOperations, ICoreHpDamageShieldsCoreCharacterStateProgramsOperations coreCharacterStateProgramsOperations, ICoreHpDamageShieldsCoreHpChangeProgramsOperations coreHpChangeProgramsOperations, ICoreHpDamageShieldsCoreNationalSkillsOperations coreNationalSkillsOperations, ICoreHpDamageShieldsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreHpDamageShieldsCoreRuntimeOperations coreRuntimeOperations, ICoreHpDamageShieldsCoreSkillOwnershipWindowsOperations coreSkillOwnershipWindowsOperations, ICoreHpDamageShieldsCoreSkillProgramsOperations coreSkillProgramsOperations, ICoreHpDamageShieldsCoreMatchLogOperations coreMatchLogOperations, ICoreHpDamageShieldsCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICoreHpDamageShieldsCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICoreHpDamageShieldsCorePlayerViewProjectionOperations corePlayerViewProjectionOperations, ICoreHpDamageShieldsCoreRecoveryReplacementOperations coreRecoveryReplacementOperations, ICoreHpDamageShieldsCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICoreHpDamageShieldsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess coreAiBrainsAiBrainsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineHpChangeProgramsPendingHpChangesAccess coreGameEngineHpChangeProgramsPendingHpChangesAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreCharacterStateProgramsOperations = coreCharacterStateProgramsOperations;
        this._coreHpChangeProgramsOperations = coreHpChangeProgramsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSkillOwnershipWindowsOperations = coreSkillOwnershipWindowsOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePlayerViewProjectionOperations = corePlayerViewProjectionOperations;
        this._coreRecoveryReplacementOperations = coreRecoveryReplacementOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreAiBrainsAiBrainsAccess = coreAiBrainsAiBrainsAccess;
        this._coreMatchOutcomeStatusWinnerAccess = coreMatchOutcomeStatusWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreGameEngineHpChangeProgramsPendingHpChangesAccess = coreGameEngineHpChangeProgramsPendingHpChangesAccess;
    }
}
