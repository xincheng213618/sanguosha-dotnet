#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CharacterLiangXingOlRuntime : ILiangXingProgramHost, ILuelveCounterSlashProgramHost
{
    private readonly RuntimeEntries.CharacterHuangQuanMouEntries _characterHuangQuanMouOperations;
    private readonly IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly RuntimeEntries.CoreActiveNativeAttackQueriesEntries _coreActiveNativeAttackQueriesOperations;
    private readonly RuntimeEntries.CoreDirectedTurnCardPoliciesEntries _coreDirectedTurnCardPoliciesOperations;
    private readonly RuntimeEntries.CoreGameEngineCardActionsEntries _coreCardActionsOperations;
    private readonly RuntimeEntries.CoreGameEngineCardMovementProgramsEntries _coreCardMovementProgramsOperations;
    private readonly RuntimeEntries.CoreGameEnginePlayCardHistoryEntries _corePlayCardHistoryOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly RuntimeEntries.CoreNativeCardContinuationCursorsEntries _coreNativeCardContinuationCursorsOperations;
    private readonly RuntimeEntries.CoreNativeDamagePoliciesEntries _coreNativeDamagePoliciesOperations;
    private readonly ICharacterLiangXingOlCoreNativeRuleQueriesOperations _coreNativeRuleQueriesOperations;
    private readonly RuntimeEntries.CoreNativeSlashUseEntries _coreNativeSlashUseOperations;
    private readonly RuntimeEntries.CorePhysicalMovementFacadesEntries _corePhysicalMovementFacadesOperations;
    private readonly RuntimeEntries.CoreProgramFactionActionRewardEntries _coreProgramFactionActionRewardOperations;
    private readonly RuntimeEntries.CoreResolutionIdentityBoundaryEntries _coreResolutionIdentityBoundaryOperations;
    private readonly IKernelCardAttackHandleFactory _kernelCoreMainOperations;
    private readonly IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess _coreCardMovementJournalMovementSequenceAccess;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterLiangXingOlRuntime(RuntimeEntries.CharacterHuangQuanMouEntries characterHuangQuanMouOperations, IKernelCardZoneStoreAccess cardZonesOperations, IKernelFrameStoreAccess resolutionStackOperations, RuntimeEntries.CoreActiveNativeAttackQueriesEntries coreActiveNativeAttackQueriesOperations, RuntimeEntries.CoreDirectedTurnCardPoliciesEntries coreDirectedTurnCardPoliciesOperations, RuntimeEntries.CoreGameEngineCardActionsEntries coreCardActionsOperations, RuntimeEntries.CoreGameEngineCardMovementProgramsEntries coreCardMovementProgramsOperations, RuntimeEntries.CoreGameEnginePlayCardHistoryEntries corePlayCardHistoryOperations, RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations, RuntimeEntries.CoreNativeCardContinuationCursorsEntries coreNativeCardContinuationCursorsOperations, RuntimeEntries.CoreNativeDamagePoliciesEntries coreNativeDamagePoliciesOperations, ICharacterLiangXingOlCoreNativeRuleQueriesOperations coreNativeRuleQueriesOperations, RuntimeEntries.CoreNativeSlashUseEntries coreNativeSlashUseOperations, RuntimeEntries.CorePhysicalMovementFacadesEntries corePhysicalMovementFacadesOperations, RuntimeEntries.CoreProgramFactionActionRewardEntries coreProgramFactionActionRewardOperations, RuntimeEntries.CoreResolutionIdentityBoundaryEntries coreResolutionIdentityBoundaryOperations, IKernelCardAttackHandleFactory kernelCoreMainOperations, IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess coreCardMovementJournalMovementSequenceAccess, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterHuangQuanMouOperations = characterHuangQuanMouOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreActiveNativeAttackQueriesOperations = coreActiveNativeAttackQueriesOperations;
        this._coreDirectedTurnCardPoliciesOperations = coreDirectedTurnCardPoliciesOperations;
        this._coreCardActionsOperations = coreCardActionsOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._corePlayCardHistoryOperations = corePlayCardHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreNativeCardContinuationCursorsOperations = coreNativeCardContinuationCursorsOperations;
        this._coreNativeDamagePoliciesOperations = coreNativeDamagePoliciesOperations;
        this._coreNativeRuleQueriesOperations = coreNativeRuleQueriesOperations;
        this._coreNativeSlashUseOperations = coreNativeSlashUseOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._coreProgramFactionActionRewardOperations = coreProgramFactionActionRewardOperations;
        this._coreResolutionIdentityBoundaryOperations = coreResolutionIdentityBoundaryOperations;
        this._kernelCoreMainOperations = kernelCoreMainOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreCardMovementJournalMovementSequenceAccess = coreCardMovementJournalMovementSequenceAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
