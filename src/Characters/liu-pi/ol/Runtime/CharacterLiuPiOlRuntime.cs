#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterLiuPiOlRuntime : global::CardGame.Core.ILiuPiProgramHost
{
    private readonly ICharacterLiuPiOlCardZonesOperations _cardZonesOperations;
    private readonly ICharacterLiuPiOlContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterLiuPiOlResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterLiuPiOlCoreDyingCursorRuntimeOperations _coreDyingCursorRuntimeOperations;
    private readonly ICharacterLiuPiOlCoreAdvancedLifecycleOperations _coreAdvancedLifecycleOperations;
    private readonly ICharacterLiuPiOlCoreObserverSubtreesOperations2 _coreObserverSubtreesOperations;
    private readonly ICharacterLiuPiOlCoreProgramAtomicCardMovesOperations _coreProgramAtomicCardMovesOperations;
    private readonly ICharacterLiuPiOlCoreProgramContinuationsOperations _coreProgramContinuationsOperations;
    private readonly ICharacterLiuPiOlCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICharacterLiuPiOlCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterLiuPiOlCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICharacterLiuPiOlCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICharacterLiuPiOlCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICharacterLiuPiOlCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICharacterLiuPiOlCoreSnapshotValueProjectionOperations _coreSnapshotValueProjectionOperations;
    private readonly ICharacterLiuPiOlCharacterLiuPiOlOperations _characterLiuPiOlOperations;
    private readonly ICharacterLiuPiOlCoreObserverSubtreesOperations _coreObserverSubtreesOperations2;
    private readonly ICharacterLiuPiOlKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess _coreCardMovementJournalCardMovementsMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterLiuPiOlRuntime(ICharacterLiuPiOlCardZonesOperations cardZonesOperations, ICharacterLiuPiOlContentRegistryOperations contentRegistryOperations, ICharacterLiuPiOlResolutionStackOperations resolutionStackOperations, ICharacterLiuPiOlCoreDyingCursorRuntimeOperations coreDyingCursorRuntimeOperations, ICharacterLiuPiOlCoreAdvancedLifecycleOperations coreAdvancedLifecycleOperations, ICharacterLiuPiOlCoreObserverSubtreesOperations2 coreObserverSubtreesOperations, ICharacterLiuPiOlCoreProgramAtomicCardMovesOperations coreProgramAtomicCardMovesOperations, ICharacterLiuPiOlCoreProgramContinuationsOperations coreProgramContinuationsOperations, ICharacterLiuPiOlCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICharacterLiuPiOlCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterLiuPiOlCoreRuntimeOperations coreRuntimeOperations, ICharacterLiuPiOlCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICharacterLiuPiOlCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICharacterLiuPiOlCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICharacterLiuPiOlCoreSnapshotValueProjectionOperations coreSnapshotValueProjectionOperations, ICharacterLiuPiOlCharacterLiuPiOlOperations characterLiuPiOlOperations, ICharacterLiuPiOlCoreObserverSubtreesOperations coreObserverSubtreesOperations2, ICharacterLiuPiOlKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess coreCardMovementJournalCardMovementsMovementSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreAdvancedLifecycleOperations = coreAdvancedLifecycleOperations;
        this._coreObserverSubtreesOperations = coreObserverSubtreesOperations;
        this._coreProgramAtomicCardMovesOperations = coreProgramAtomicCardMovesOperations;
        this._coreProgramContinuationsOperations = coreProgramContinuationsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreSnapshotValueProjectionOperations = coreSnapshotValueProjectionOperations;
        this._characterLiuPiOlOperations = characterLiuPiOlOperations;
        this._coreObserverSubtreesOperations2 = coreObserverSubtreesOperations2;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreCardMovementJournalCardMovementsMovementSequenceAccess = coreCardMovementJournalCardMovementsMovementSequenceAccess;
        this._coreMatchOutcomeStatusWinnerAccess = coreMatchOutcomeStatusWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
