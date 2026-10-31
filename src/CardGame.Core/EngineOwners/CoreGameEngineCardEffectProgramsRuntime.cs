#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineCardEffectProgramsRuntime
{
    private readonly ICoreCardEffectProgramsCharacterLuXunMouMobileOperations _characterLuXunMouMobileOperations;
    private readonly ICoreCardEffectProgramsCardZonesOperations _cardZonesOperations;
    private readonly ICoreCardEffectProgramsContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreCardEffectProgramsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreCardEffectProgramsCoreActualTurnsOperations _coreActualTurnsOperations;
    private readonly ICoreCardEffectProgramsCoreCardActionsOperations _coreCardActionsOperations;
    private readonly ICoreCardEffectProgramsCoreOriginalUseLightningOperations _coreOriginalUseLightningOperations;
    private readonly ICoreCardEffectProgramsCoreProgramCardTargetEffectsOperations _coreProgramCardTargetEffectsOperations;
    private readonly ICoreCardEffectProgramsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreCardEffectProgramsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreCardEffectProgramsCoreTableOrderOperations _coreTableOrderOperations;
    private readonly ICoreCardEffectProgramsCoreNativeJudgmentOperations _coreNativeJudgmentOperations;
    private readonly ICoreCardEffectProgramsCoreNativeNullificationOperations _coreNativeNullificationOperations;
    private readonly ICoreCardEffectProgramsCoreNativeSlashTargetDuelOperations _coreNativeSlashTargetDuelOperations;
    private readonly ICoreCardEffectProgramsCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICoreCardEffectProgramsCoreResolutionIdentityBoundaryOperations _coreResolutionIdentityBoundaryOperations;
    private readonly ICoreCardEffectProgramsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineCardEffectProgramsRuntime(ICoreCardEffectProgramsCharacterLuXunMouMobileOperations characterLuXunMouMobileOperations, ICoreCardEffectProgramsCardZonesOperations cardZonesOperations, ICoreCardEffectProgramsContentRegistryOperations contentRegistryOperations, ICoreCardEffectProgramsResolutionStackOperations resolutionStackOperations, ICoreCardEffectProgramsCoreActualTurnsOperations coreActualTurnsOperations, ICoreCardEffectProgramsCoreCardActionsOperations coreCardActionsOperations, ICoreCardEffectProgramsCoreOriginalUseLightningOperations coreOriginalUseLightningOperations, ICoreCardEffectProgramsCoreProgramCardTargetEffectsOperations coreProgramCardTargetEffectsOperations, ICoreCardEffectProgramsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreCardEffectProgramsCoreRuntimeOperations coreRuntimeOperations, ICoreCardEffectProgramsCoreTableOrderOperations coreTableOrderOperations, ICoreCardEffectProgramsCoreNativeJudgmentOperations coreNativeJudgmentOperations, ICoreCardEffectProgramsCoreNativeNullificationOperations coreNativeNullificationOperations, ICoreCardEffectProgramsCoreNativeSlashTargetDuelOperations coreNativeSlashTargetDuelOperations, ICoreCardEffectProgramsCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICoreCardEffectProgramsCoreResolutionIdentityBoundaryOperations coreResolutionIdentityBoundaryOperations, ICoreCardEffectProgramsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterLuXunMouMobileOperations = characterLuXunMouMobileOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreActualTurnsOperations = coreActualTurnsOperations;
        this._coreCardActionsOperations = coreCardActionsOperations;
        this._coreOriginalUseLightningOperations = coreOriginalUseLightningOperations;
        this._coreProgramCardTargetEffectsOperations = coreProgramCardTargetEffectsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreTableOrderOperations = coreTableOrderOperations;
        this._coreNativeJudgmentOperations = coreNativeJudgmentOperations;
        this._coreNativeNullificationOperations = coreNativeNullificationOperations;
        this._coreNativeSlashTargetDuelOperations = coreNativeSlashTargetDuelOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._coreResolutionIdentityBoundaryOperations = coreResolutionIdentityBoundaryOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess = coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
