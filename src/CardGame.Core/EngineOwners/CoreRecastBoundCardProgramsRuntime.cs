#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreRecastBoundCardProgramsRuntime : global::CardGame.Core.IRecastBoundCardProgramEffectHost
{
    private readonly ICoreRecastBoundCardProgramsCardZonesOperations _cardZonesOperations;
    private readonly ICoreRecastBoundCardProgramsContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreRecastBoundCardProgramsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreRecastBoundCardProgramsCoreCardMovementProgramsOperations _coreCardMovementProgramsOperations;
    private readonly ICoreRecastBoundCardProgramsCoreHpChangeProgramsOperations _coreHpChangeProgramsOperations;
    private readonly ICoreRecastBoundCardProgramsCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICoreRecastBoundCardProgramsCoreNativeRecastInvocationsOperations _coreNativeRecastInvocationsOperations;
    private readonly ICoreRecastBoundCardProgramsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreRecastBoundCardProgramsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreRecastBoundCardProgramsCorePhysicalDrawFacadesOperations _corePhysicalDrawFacadesOperations;
    private readonly ICoreRecastBoundCardProgramsCorePhysicalMovementFacadesOperations _corePhysicalMovementFacadesOperations;
    private readonly ICoreRecastBoundCardProgramsCoreRecoveryReplacementOperations _coreRecoveryReplacementOperations;
    private readonly ICoreRecastBoundCardProgramsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreRecastBoundCardProgramsRuntime(ICoreRecastBoundCardProgramsCardZonesOperations cardZonesOperations, ICoreRecastBoundCardProgramsContentRegistryOperations contentRegistryOperations, ICoreRecastBoundCardProgramsResolutionStackOperations resolutionStackOperations, ICoreRecastBoundCardProgramsCoreCardMovementProgramsOperations coreCardMovementProgramsOperations, ICoreRecastBoundCardProgramsCoreHpChangeProgramsOperations coreHpChangeProgramsOperations, ICoreRecastBoundCardProgramsCoreNationalSkillsOperations coreNationalSkillsOperations, ICoreRecastBoundCardProgramsCoreNativeRecastInvocationsOperations coreNativeRecastInvocationsOperations, ICoreRecastBoundCardProgramsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreRecastBoundCardProgramsCoreRuntimeOperations coreRuntimeOperations, ICoreRecastBoundCardProgramsCorePhysicalDrawFacadesOperations corePhysicalDrawFacadesOperations, ICoreRecastBoundCardProgramsCorePhysicalMovementFacadesOperations corePhysicalMovementFacadesOperations, ICoreRecastBoundCardProgramsCoreRecoveryReplacementOperations coreRecoveryReplacementOperations, ICoreRecastBoundCardProgramsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreHpChangeProgramsOperations = coreHpChangeProgramsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreNativeRecastInvocationsOperations = coreNativeRecastInvocationsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._corePhysicalDrawFacadesOperations = corePhysicalDrawFacadesOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._coreRecoveryReplacementOperations = coreRecoveryReplacementOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
