#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEnginePublicPersistentPilesRuntime : global::CardGame.Core.IPublicPersistentPileProgramHost
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterBaoXinOlEntries _characterBaoXinOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterGanNingMouMobileEntries _characterGanNingMouMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterGongsunZanMouMobileEntries _characterGongsunZanMouMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuangQuanMouEntries _characterHuangQuanMouOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuangZuMobileShiEntries _characterHuangZuMobileShiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLiFengOlEntries _characterLiFengOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLvQianOlEntries _characterLvQianOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterSunShangxiangMouMobileEntries _characterSunShangxiangMouMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterXiahouDunMouMobileEntries _characterXiahouDunMouMobileOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedWeaponsEntries _coreAdvancedWeaponsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineBoundPublicPileProgramsEntries _coreBoundPublicPileProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardDistributionPresentationEntries _coreCardDistributionPresentationOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardMovementProgramsEntries _coreCardMovementProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries _coreProgramAtomicCardMovesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePublicPileSourcesEntries _corePublicPileSourcesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativePromptBoundaryEntries _coreNativePromptBoundaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalDrawFacadesEntries _corePhysicalDrawFacadesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries _corePhysicalMovementFacadesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess _coreCardMovementJournalMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEnginePublicPileSourcesPublicPersistentPilesAccess _coreGameEnginePublicPileSourcesPublicPersistentPilesAccess;
    internal CoreGameEnginePublicPersistentPilesRuntime(global::CardGame.Core.RuntimeEntries.CharacterBaoXinOlEntries characterBaoXinOlOperations, global::CardGame.Core.RuntimeEntries.CharacterGanNingMouMobileEntries characterGanNingMouMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterGongsunZanMouMobileEntries characterGongsunZanMouMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterHuangQuanMouEntries characterHuangQuanMouOperations, global::CardGame.Core.RuntimeEntries.CharacterHuangZuMobileShiEntries characterHuangZuMobileShiOperations, global::CardGame.Core.RuntimeEntries.CharacterLiFengOlEntries characterLiFengOlOperations, global::CardGame.Core.RuntimeEntries.CharacterLvQianOlEntries characterLvQianOlOperations, global::CardGame.Core.RuntimeEntries.CharacterSunShangxiangMouMobileEntries characterSunShangxiangMouMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterXiahouDunMouMobileEntries characterXiahouDunMouMobileOperations, global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedWeaponsEntries coreAdvancedWeaponsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineBoundPublicPileProgramsEntries coreBoundPublicPileProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardDistributionPresentationEntries coreCardDistributionPresentationOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardMovementProgramsEntries coreCardMovementProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries coreProgramAtomicCardMovesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePublicPileSourcesEntries corePublicPileSourcesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreNativePromptBoundaryEntries coreNativePromptBoundaryOperations, global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalDrawFacadesEntries corePhysicalDrawFacadesOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries corePhysicalMovementFacadesOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess coreCardMovementJournalMovementSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEnginePublicPileSourcesPublicPersistentPilesAccess coreGameEnginePublicPileSourcesPublicPersistentPilesAccess)
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
