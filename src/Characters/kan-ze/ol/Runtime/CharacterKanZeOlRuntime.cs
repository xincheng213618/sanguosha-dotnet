#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterKanZeOlRuntime : global::CardGame.Core.IKanZeProgramHost
{
    private readonly ICharacterKanZeOlCharacterHuangQuanMouOperations _characterHuangQuanMouOperations;
    private readonly ICharacterKanZeOlCharacterXiaoQiaoMouOlOperations _characterXiaoQiaoMouOlOperations;
    private readonly ICharacterKanZeOlCardZonesOperations _cardZonesOperations;
    private readonly ICharacterKanZeOlContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterKanZeOlResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterKanZeOlCoreDyingCursorRuntimeOperations _coreDyingCursorRuntimeOperations;
    private readonly ICharacterKanZeOlCoreAdvancedLifecycleOperations _coreAdvancedLifecycleOperations;
    private readonly ICharacterKanZeOlCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICharacterKanZeOlCoreObserverSubtreesOperations _coreObserverSubtreesOperations;
    private readonly ICharacterKanZeOlCorePrivateOfferReturnsOperations _corePrivateOfferReturnsOperations;
    private readonly ICharacterKanZeOlCoreProgramAtomicCardMovesOperations _coreProgramAtomicCardMovesOperations;
    private readonly ICharacterKanZeOlCoreProgramAttackAttemptOperations _coreProgramAttackAttemptOperations;
    private readonly ICharacterKanZeOlCoreProgramContinuationsOperations _coreProgramContinuationsOperations;
    private readonly ICharacterKanZeOlCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterKanZeOlCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICharacterKanZeOlCoreSkillProgramsOperations _coreSkillProgramsOperations;
    private readonly ICharacterKanZeOlCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICharacterKanZeOlCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICharacterKanZeOlCoreSnapshotValueProjectionOperations _coreSnapshotValueProjectionOperations;
    private readonly ICharacterKanZeOlCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICharacterKanZeOlCharacterKanZeOlOperations _characterKanZeOlOperations;
    private readonly ICharacterKanZeOlCoreObserverSubtreesOperations2 _coreObserverSubtreesOperations2;
    private readonly ICharacterKanZeOlKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess _coreCardMovementJournalCardMovementsMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterKanZeOlRuntime(ICharacterKanZeOlCharacterHuangQuanMouOperations characterHuangQuanMouOperations, ICharacterKanZeOlCharacterXiaoQiaoMouOlOperations characterXiaoQiaoMouOlOperations, ICharacterKanZeOlCardZonesOperations cardZonesOperations, ICharacterKanZeOlContentRegistryOperations contentRegistryOperations, ICharacterKanZeOlResolutionStackOperations resolutionStackOperations, ICharacterKanZeOlCoreDyingCursorRuntimeOperations coreDyingCursorRuntimeOperations, ICharacterKanZeOlCoreAdvancedLifecycleOperations coreAdvancedLifecycleOperations, ICharacterKanZeOlCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICharacterKanZeOlCoreObserverSubtreesOperations coreObserverSubtreesOperations, ICharacterKanZeOlCorePrivateOfferReturnsOperations corePrivateOfferReturnsOperations, ICharacterKanZeOlCoreProgramAtomicCardMovesOperations coreProgramAtomicCardMovesOperations, ICharacterKanZeOlCoreProgramAttackAttemptOperations coreProgramAttackAttemptOperations, ICharacterKanZeOlCoreProgramContinuationsOperations coreProgramContinuationsOperations, ICharacterKanZeOlCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterKanZeOlCoreRuntimeOperations coreRuntimeOperations, ICharacterKanZeOlCoreSkillProgramsOperations coreSkillProgramsOperations, ICharacterKanZeOlCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICharacterKanZeOlCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICharacterKanZeOlCoreSnapshotValueProjectionOperations coreSnapshotValueProjectionOperations, ICharacterKanZeOlCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICharacterKanZeOlCharacterKanZeOlOperations characterKanZeOlOperations, ICharacterKanZeOlCoreObserverSubtreesOperations2 coreObserverSubtreesOperations2, ICharacterKanZeOlKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess coreCardMovementJournalCardMovementsMovementSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterHuangQuanMouOperations = characterHuangQuanMouOperations;
        this._characterXiaoQiaoMouOlOperations = characterXiaoQiaoMouOlOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreAdvancedLifecycleOperations = coreAdvancedLifecycleOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreObserverSubtreesOperations = coreObserverSubtreesOperations;
        this._corePrivateOfferReturnsOperations = corePrivateOfferReturnsOperations;
        this._coreProgramAtomicCardMovesOperations = coreProgramAtomicCardMovesOperations;
        this._coreProgramAttackAttemptOperations = coreProgramAttackAttemptOperations;
        this._coreProgramContinuationsOperations = coreProgramContinuationsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._coreSnapshotValueProjectionOperations = coreSnapshotValueProjectionOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._characterKanZeOlOperations = characterKanZeOlOperations;
        this._coreObserverSubtreesOperations2 = coreObserverSubtreesOperations2;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreCardMovementJournalCardMovementsMovementSequenceAccess = coreCardMovementJournalCardMovementsMovementSequenceAccess;
        this._coreMatchOutcomeStatusAccess = coreMatchOutcomeStatusAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
