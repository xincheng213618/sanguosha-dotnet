#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineActualUseTargetProgramsRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreActiveNativeAttackQueriesEntries _coreActiveNativeAttackQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardActionsEntries _coreCardActionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentUsePhysicalClaimsEntries _coreCurrentUsePhysicalClaimsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageColorDuelProgramsEntries _coreDamageColorDuelProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageJudgmentSuitPaymentsEntries _coreDamageJudgmentSuitPaymentsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHalfHandTargetSupportEntries _coreHalfHandTargetSupportOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineOwnTrickDrawTargetsEntries _coreOwnTrickDrawTargetsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardTargetEffectsEntries _coreProgramCardTargetEffectsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSameTypeActualUseAidEntries _coreSameTypeActualUseAidOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShortRangeSlashTargetsEntries _coreShortRangeSlashTargetsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSlashTargetBenefitWindowsEntries _coreSlashTargetBenefitWindowsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSlashTargetPenaltyWindowsEntries _coreSlashTargetPenaltyWindowsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTableOrderEntries _coreTableOrderOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineUniqueHpTargetAnnouncementsEntries _coreUniqueHpTargetAnnouncementsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeAttackCompletionEntries _coreNativeAttackCompletionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardUseCompletionEntries _coreNativeCardUseCompletionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardUseFramesEntries _coreNativeCardUseFramesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeNullificationEntries _coreNativeNullificationOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeSlashUseEntries _coreNativeSlashUseOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries _corePhysicalMovementFacadesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreResolutionIdentityBoundaryEntries _coreResolutionIdentityBoundaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCompleteGameOperations _kernelCoreMainOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineActualUseTargetProgramsRuntime(global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreActiveNativeAttackQueriesEntries coreActiveNativeAttackQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardActionsEntries coreCardActionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentUsePhysicalClaimsEntries coreCurrentUsePhysicalClaimsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageColorDuelProgramsEntries coreDamageColorDuelProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageJudgmentSuitPaymentsEntries coreDamageJudgmentSuitPaymentsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineHalfHandTargetSupportEntries coreHalfHandTargetSupportOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineOwnTrickDrawTargetsEntries coreOwnTrickDrawTargetsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardTargetEffectsEntries coreProgramCardTargetEffectsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineSameTypeActualUseAidEntries coreSameTypeActualUseAidOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineShortRangeSlashTargetsEntries coreShortRangeSlashTargetsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineSlashTargetBenefitWindowsEntries coreSlashTargetBenefitWindowsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineSlashTargetPenaltyWindowsEntries coreSlashTargetPenaltyWindowsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineTableOrderEntries coreTableOrderOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineUniqueHpTargetAnnouncementsEntries coreUniqueHpTargetAnnouncementsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeAttackCompletionEntries coreNativeAttackCompletionOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardUseCompletionEntries coreNativeCardUseCompletionOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardUseFramesEntries coreNativeCardUseFramesOperations, global::CardGame.Core.RuntimeEntries.CoreNativeNullificationEntries coreNativeNullificationOperations, global::CardGame.Core.RuntimeEntries.CoreNativeSlashUseEntries coreNativeSlashUseOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries corePhysicalMovementFacadesOperations, global::CardGame.Core.RuntimeEntries.CoreResolutionIdentityBoundaryEntries coreResolutionIdentityBoundaryOperations, global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineOwners.IKernelCompleteGameOperations kernelCoreMainOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreActiveNativeAttackQueriesOperations = coreActiveNativeAttackQueriesOperations;
        this._coreCardActionsOperations = coreCardActionsOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreCurrentUsePhysicalClaimsOperations = coreCurrentUsePhysicalClaimsOperations;
        this._coreDamageColorDuelProgramsOperations = coreDamageColorDuelProgramsOperations;
        this._coreDamageJudgmentSuitPaymentsOperations = coreDamageJudgmentSuitPaymentsOperations;
        this._coreHalfHandTargetSupportOperations = coreHalfHandTargetSupportOperations;
        this._coreOwnTrickDrawTargetsOperations = coreOwnTrickDrawTargetsOperations;
        this._coreProgramCardTargetEffectsOperations = coreProgramCardTargetEffectsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSameTypeActualUseAidOperations = coreSameTypeActualUseAidOperations;
        this._coreShortRangeSlashTargetsOperations = coreShortRangeSlashTargetsOperations;
        this._coreSlashTargetBenefitWindowsOperations = coreSlashTargetBenefitWindowsOperations;
        this._coreSlashTargetPenaltyWindowsOperations = coreSlashTargetPenaltyWindowsOperations;
        this._coreTableOrderOperations = coreTableOrderOperations;
        this._coreUniqueHpTargetAnnouncementsOperations = coreUniqueHpTargetAnnouncementsOperations;
        this._coreNativeAttackCompletionOperations = coreNativeAttackCompletionOperations;
        this._coreNativeCardUseCompletionOperations = coreNativeCardUseCompletionOperations;
        this._coreNativeCardUseFramesOperations = coreNativeCardUseFramesOperations;
        this._coreNativeNullificationOperations = coreNativeNullificationOperations;
        this._coreNativeSlashUseOperations = coreNativeSlashUseOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._coreResolutionIdentityBoundaryOperations = coreResolutionIdentityBoundaryOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._kernelCoreMainOperations = kernelCoreMainOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreMatchOutcomeStatusWinnerAccess = coreMatchOutcomeStatusWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
