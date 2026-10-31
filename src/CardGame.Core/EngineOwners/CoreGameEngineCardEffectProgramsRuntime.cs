#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineCardEffectProgramsRuntime
{
    private readonly RuntimeEntries.CharacterLuXunMouMobileEntries _characterLuXunMouMobileOperations;
    private readonly IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly RuntimeEntries.CoreActualTurnsEntries _coreActualTurnsOperations;
    private readonly RuntimeEntries.CoreGameEngineCardActionsEntries _coreCardActionsOperations;
    private readonly RuntimeEntries.CoreGameEngineOriginalUseLightningEntries _coreOriginalUseLightningOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramCardTargetEffectsEntries _coreProgramCardTargetEffectsOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly RuntimeEntries.CoreGameEngineTableOrderEntries _coreTableOrderOperations;
    private readonly RuntimeEntries.CoreNativeJudgmentEntries _coreNativeJudgmentOperations;
    private readonly RuntimeEntries.CoreNativeNullificationEntries _coreNativeNullificationOperations;
    private readonly RuntimeEntries.CoreNativeSlashTargetDuelEntries _coreNativeSlashTargetDuelOperations;
    private readonly RuntimeEntries.CoreResolutionIdentityBoundaryEntries _coreResolutionIdentityBoundaryOperations;
    private readonly IKernelAdvanceRulesAndPublishStateOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
    private readonly EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineCardEffectProgramsRuntime(RuntimeEntries.CharacterLuXunMouMobileEntries characterLuXunMouMobileOperations,IKernelCardZoneStoreAccess cardZonesOperations,IKernelContentRegistryAccess contentRegistryOperations,IKernelFrameStoreAccess resolutionStackOperations,RuntimeEntries.CoreActualTurnsEntries coreActualTurnsOperations,RuntimeEntries.CoreGameEngineCardActionsEntries coreCardActionsOperations,RuntimeEntries.CoreGameEngineOriginalUseLightningEntries coreOriginalUseLightningOperations,RuntimeEntries.CoreGameEngineProgramCardTargetEffectsEntries coreProgramCardTargetEffectsOperations,RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations,RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations,RuntimeEntries.CoreGameEngineTableOrderEntries coreTableOrderOperations,RuntimeEntries.CoreNativeJudgmentEntries coreNativeJudgmentOperations,RuntimeEntries.CoreNativeNullificationEntries coreNativeNullificationOperations,RuntimeEntries.CoreNativeSlashTargetDuelEntries coreNativeSlashTargetDuelOperations,RuntimeEntries.CoreResolutionIdentityBoundaryEntries coreResolutionIdentityBoundaryOperations,IKernelAdvanceRulesAndPublishStateOperations kernelCoreGameEngineRuleProgressionOperations,EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess,EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess,EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
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
        this._coreResolutionIdentityBoundaryOperations = coreResolutionIdentityBoundaryOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess = coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
