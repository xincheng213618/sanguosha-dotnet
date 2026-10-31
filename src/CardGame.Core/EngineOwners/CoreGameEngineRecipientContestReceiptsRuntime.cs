#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineRecipientContestReceiptsRuntime
{
    private readonly RuntimeEntries.CharacterLiuPanOlEntries _characterLiuPanOlOperations;
    private readonly RuntimeEntries.CharacterMaChengOlEntries _characterMaChengOlOperations;
    private readonly RuntimeEntries.CharacterMaWanOlEntries _characterMaWanOlOperations;
    private readonly RuntimeEntries.CharacterMaXiuMaTieOlEntries _characterMaXiuMaTieOlOperations;
    private readonly RuntimeEntries.CharacterQianZhaoOlEntries _characterQianZhaoOlOperations;
    private readonly RuntimeEntries.CharacterYangFuOlEntries _characterYangFuOlOperations;
    private readonly RuntimeEntries.CharacterZhangYanOlEntries _characterZhangYanOlOperations;
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly RuntimeEntries.CoreGameEngineActualTurnDamageClaimsEntries _coreActualTurnDamageClaimsOperations;
    private readonly RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries _coreAdvancedLifecycleOperations;
    private readonly RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly RuntimeEntries.CoreGameEngineCurrentSlashFireOffersEntries _coreCurrentSlashFireOffersOperations;
    private readonly RuntimeEntries.CoreGameEngineEphemeralCardMaterialsEntries _coreEphemeralCardMaterialsOperations;
    private readonly RuntimeEntries.CoreGameEngineOriginalTargetAdditionsEntries _coreOriginalTargetAdditionsOperations;
    private readonly RuntimeEntries.CoreGameEngineRecipientContestsEntries _coreRecipientContestsOperations;
    private readonly RuntimeEntries.CoreGameEngineSameTypeActualUseAidEntries _coreSameTypeActualUseAidOperations;
    private readonly RuntimeEntries.CoreGameEngineShortRangeSlashTargetsEntries _coreShortRangeSlashTargetsOperations;
    private readonly RuntimeEntries.CoreGameEngineUniqueHpPeerProgramsEntries _coreUniqueHpPeerProgramsOperations;
    private readonly RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess;
    private readonly EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    internal CoreGameEngineRecipientContestReceiptsRuntime(RuntimeEntries.CharacterLiuPanOlEntries characterLiuPanOlOperations, RuntimeEntries.CharacterMaChengOlEntries characterMaChengOlOperations, RuntimeEntries.CharacterMaWanOlEntries characterMaWanOlOperations, RuntimeEntries.CharacterMaXiuMaTieOlEntries characterMaXiuMaTieOlOperations, RuntimeEntries.CharacterQianZhaoOlEntries characterQianZhaoOlOperations, RuntimeEntries.CharacterYangFuOlEntries characterYangFuOlOperations, RuntimeEntries.CharacterZhangYanOlEntries characterZhangYanOlOperations, IKernelContentRegistryAccess contentRegistryOperations, IKernelFrameStoreAccess resolutionStackOperations, RuntimeEntries.CoreGameEngineActualTurnDamageClaimsEntries coreActualTurnDamageClaimsOperations, RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries coreAdvancedLifecycleOperations, RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations, RuntimeEntries.CoreGameEngineCurrentSlashFireOffersEntries coreCurrentSlashFireOffersOperations, RuntimeEntries.CoreGameEngineEphemeralCardMaterialsEntries coreEphemeralCardMaterialsOperations, RuntimeEntries.CoreGameEngineOriginalTargetAdditionsEntries coreOriginalTargetAdditionsOperations, RuntimeEntries.CoreGameEngineRecipientContestsEntries coreRecipientContestsOperations, RuntimeEntries.CoreGameEngineSameTypeActualUseAidEntries coreSameTypeActualUseAidOperations, RuntimeEntries.CoreGameEngineShortRangeSlashTargetsEntries coreShortRangeSlashTargetsOperations, RuntimeEntries.CoreGameEngineUniqueHpPeerProgramsEntries coreUniqueHpPeerProgramsOperations, RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations, RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations, EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess, EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess)
    {
        this._characterLiuPanOlOperations = characterLiuPanOlOperations;
        this._characterMaChengOlOperations = characterMaChengOlOperations;
        this._characterMaWanOlOperations = characterMaWanOlOperations;
        this._characterMaXiuMaTieOlOperations = characterMaXiuMaTieOlOperations;
        this._characterQianZhaoOlOperations = characterQianZhaoOlOperations;
        this._characterYangFuOlOperations = characterYangFuOlOperations;
        this._characterZhangYanOlOperations = characterZhangYanOlOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreActualTurnDamageClaimsOperations = coreActualTurnDamageClaimsOperations;
        this._coreAdvancedLifecycleOperations = coreAdvancedLifecycleOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreCurrentSlashFireOffersOperations = coreCurrentSlashFireOffersOperations;
        this._coreEphemeralCardMaterialsOperations = coreEphemeralCardMaterialsOperations;
        this._coreOriginalTargetAdditionsOperations = coreOriginalTargetAdditionsOperations;
        this._coreRecipientContestsOperations = coreRecipientContestsOperations;
        this._coreSameTypeActualUseAidOperations = coreSameTypeActualUseAidOperations;
        this._coreShortRangeSlashTargetsOperations = coreShortRangeSlashTargetsOperations;
        this._coreUniqueHpPeerProgramsOperations = coreUniqueHpPeerProgramsOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess = coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
    }
}
