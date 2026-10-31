#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CoreGameEnginePublicPersistentPilesRuntime : IPublicPersistentPileProgramHost
{
    private readonly RuntimeEntries.CharacterBaoXinOlEntries _characterBaoXinOlOperations;
    private readonly RuntimeEntries.CharacterGanNingMouMobileEntries _characterGanNingMouMobileOperations;
    private readonly RuntimeEntries.CharacterGongsunZanMouMobileEntries _characterGongsunZanMouMobileOperations;
    private readonly RuntimeEntries.CharacterHuangQuanMouEntries _characterHuangQuanMouOperations;
    private readonly RuntimeEntries.CharacterHuangZuMobileShiEntries _characterHuangZuMobileShiOperations;
    private readonly RuntimeEntries.CharacterLiFengOlEntries _characterLiFengOlOperations;
    private readonly RuntimeEntries.CharacterLvQianOlEntries _characterLvQianOlOperations;
    private readonly RuntimeEntries.CharacterSunShangxiangMouMobileEntries _characterSunShangxiangMouMobileOperations;
    private readonly RuntimeEntries.CharacterXiahouDunMouMobileEntries _characterXiahouDunMouMobileOperations;
    private readonly IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly RuntimeEntries.CoreGameEngineAdvancedWeaponsEntries _coreAdvancedWeaponsOperations;
    private readonly RuntimeEntries.CoreGameEngineBoundPublicPileProgramsEntries _coreBoundPublicPileProgramsOperations;
    private readonly RuntimeEntries.CoreGameEngineCardDistributionPresentationEntries _coreCardDistributionPresentationOperations;
    private readonly RuntimeEntries.CoreGameEngineCardMovementProgramsEntries _coreCardMovementProgramsOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries _coreProgramAtomicCardMovesOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly RuntimeEntries.CoreGameEnginePublicPileSourcesEntries _corePublicPileSourcesOperations;
    private readonly RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly RuntimeEntries.CoreNativePromptBoundaryEntries _coreNativePromptBoundaryOperations;
    private readonly RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly RuntimeEntries.CorePhysicalDrawFacadesEntries _corePhysicalDrawFacadesOperations;
    private readonly RuntimeEntries.CorePhysicalMovementFacadesEntries _corePhysicalMovementFacadesOperations;
    private readonly RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess _coreCardMovementJournalMovementSequenceAccess;
    private readonly EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly EngineStates.Contracts.ICoreGameEnginePublicPileSourcesPublicPersistentPilesAccess _coreGameEnginePublicPileSourcesPublicPersistentPilesAccess;
    internal CoreGameEnginePublicPersistentPilesRuntime(RuntimeEntries.CharacterBaoXinOlEntries characterBaoXinOlOperations, RuntimeEntries.CharacterGanNingMouMobileEntries characterGanNingMouMobileOperations, RuntimeEntries.CharacterGongsunZanMouMobileEntries characterGongsunZanMouMobileOperations, RuntimeEntries.CharacterHuangQuanMouEntries characterHuangQuanMouOperations, RuntimeEntries.CharacterHuangZuMobileShiEntries characterHuangZuMobileShiOperations, RuntimeEntries.CharacterLiFengOlEntries characterLiFengOlOperations, RuntimeEntries.CharacterLvQianOlEntries characterLvQianOlOperations, RuntimeEntries.CharacterSunShangxiangMouMobileEntries characterSunShangxiangMouMobileOperations, RuntimeEntries.CharacterXiahouDunMouMobileEntries characterXiahouDunMouMobileOperations, IKernelCardZoneStoreAccess cardZonesOperations, IKernelContentRegistryAccess contentRegistryOperations, IKernelFrameStoreAccess resolutionStackOperations, RuntimeEntries.CoreGameEngineAdvancedWeaponsEntries coreAdvancedWeaponsOperations, RuntimeEntries.CoreGameEngineBoundPublicPileProgramsEntries coreBoundPublicPileProgramsOperations, RuntimeEntries.CoreGameEngineCardDistributionPresentationEntries coreCardDistributionPresentationOperations, RuntimeEntries.CoreGameEngineCardMovementProgramsEntries coreCardMovementProgramsOperations, RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries coreProgramAtomicCardMovesOperations, RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, RuntimeEntries.CoreGameEnginePublicPileSourcesEntries corePublicPileSourcesOperations, RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations, RuntimeEntries.CoreNativePromptBoundaryEntries coreNativePromptBoundaryOperations, RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations, RuntimeEntries.CorePhysicalDrawFacadesEntries corePhysicalDrawFacadesOperations, RuntimeEntries.CorePhysicalMovementFacadesEntries corePhysicalMovementFacadesOperations, RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations, EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess coreCardMovementJournalMovementSequenceAccess, EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, EngineStates.Contracts.ICoreGameEnginePublicPileSourcesPublicPersistentPilesAccess coreGameEnginePublicPileSourcesPublicPersistentPilesAccess)
    {
        this._characterBaoXinOlOperations = characterBaoXinOlOperations;
        this._characterGanNingMouMobileOperations = characterGanNingMouMobileOperations;
        this._characterGongsunZanMouMobileOperations = characterGongsunZanMouMobileOperations;
        this._characterHuangQuanMouOperations = characterHuangQuanMouOperations;
        this._characterHuangZuMobileShiOperations = characterHuangZuMobileShiOperations;
        this._characterLiFengOlOperations = characterLiFengOlOperations;
        this._characterLvQianOlOperations = characterLvQianOlOperations;
        this._characterSunShangxiangMouMobileOperations = characterSunShangxiangMouMobileOperations;
        this._characterXiahouDunMouMobileOperations = characterXiahouDunMouMobileOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAdvancedWeaponsOperations = coreAdvancedWeaponsOperations;
        this._coreBoundPublicPileProgramsOperations = coreBoundPublicPileProgramsOperations;
        this._coreCardDistributionPresentationOperations = coreCardDistributionPresentationOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreProgramAtomicCardMovesOperations = coreProgramAtomicCardMovesOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._corePublicPileSourcesOperations = corePublicPileSourcesOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalDrawFacadesOperations = corePhysicalDrawFacadesOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreCardMovementJournalMovementSequenceAccess = coreCardMovementJournalMovementSequenceAccess;
        this._coreMatchOutcomeStatusAccess = coreMatchOutcomeStatusAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreGameEnginePublicPileSourcesPublicPersistentPilesAccess = coreGameEnginePublicPileSourcesPublicPersistentPilesAccess;
    }
}
