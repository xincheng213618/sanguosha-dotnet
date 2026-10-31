#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterCaiMaoOlRuntime : global::CardGame.Core.ICaiMaoHost
{
    private readonly ICharacterCaiMaoOlCharacterLuoXianOlOperations _characterLuoXianOlOperations;
    private readonly ICharacterCaiMaoOlCardZonesOperations _cardZonesOperations;
    private readonly ICharacterCaiMaoOlContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterCaiMaoOlRandomOperations _randomOperations;
    private readonly ICharacterCaiMaoOlResolutionStackOperations2 _resolutionStackOperations;
    private readonly ICharacterCaiMaoOlCoreDyingCursorRuntimeOperations _coreDyingCursorRuntimeOperations;
    private readonly ICharacterCaiMaoOlCoreCardMovementProgramsOperations _coreCardMovementProgramsOperations;
    private readonly ICharacterCaiMaoOlCoreCharacterStateProgramsOperations _coreCharacterStateProgramsOperations;
    private readonly ICharacterCaiMaoOlCoreHpChangeProgramsOperations _coreHpChangeProgramsOperations;
    private readonly ICharacterCaiMaoOlCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICharacterCaiMaoOlCoreObserverSubtreesOperations _coreObserverSubtreesOperations;
    private readonly ICharacterCaiMaoOlCorePrivateOfferReturnsOperations _corePrivateOfferReturnsOperations;
    private readonly ICharacterCaiMaoOlCoreProgramAtomicCardMovesOperations _coreProgramAtomicCardMovesOperations;
    private readonly ICharacterCaiMaoOlCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICharacterCaiMaoOlCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterCaiMaoOlCoreProgramRevealsOperations _coreProgramRevealsOperations;
    private readonly ICharacterCaiMaoOlCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICharacterCaiMaoOlCoreSkillOwnershipWindowsOperations _coreSkillOwnershipWindowsOperations;
    private readonly ICharacterCaiMaoOlCoreSkillProgramsOperations _coreSkillProgramsOperations;
    private readonly ICharacterCaiMaoOlCoreTableOrderOperations _coreTableOrderOperations;
    private readonly ICharacterCaiMaoOlCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICharacterCaiMaoOlCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICharacterCaiMaoOlCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICharacterCaiMaoOlCorePlayerViewProjectionOperations _corePlayerViewProjectionOperations;
    private readonly ICharacterCaiMaoOlCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICharacterCaiMaoOlCharacterCaiMaoOlOperations _characterCaiMaoOlOperations;
    private readonly ICharacterCaiMaoOlResolutionStackOperations _resolutionStackOperations2;
    private readonly ICharacterCaiMaoOlCoreObserverSubtreesOperations2 _coreObserverSubtreesOperations2;
    private readonly ICharacterCaiMaoOlCoreRecoveryReplacementDamageRidesOperations _coreRecoveryReplacementDamageRidesOperations;
    private readonly ICharacterCaiMaoOlKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess _coreAiBrainsAiBrainsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess _coreCardMovementJournalCardMovementsMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterCaiMaoOlRuntime(ICharacterCaiMaoOlCharacterLuoXianOlOperations characterLuoXianOlOperations, ICharacterCaiMaoOlCardZonesOperations cardZonesOperations, ICharacterCaiMaoOlContentRegistryOperations contentRegistryOperations, ICharacterCaiMaoOlRandomOperations randomOperations, ICharacterCaiMaoOlResolutionStackOperations2 resolutionStackOperations, ICharacterCaiMaoOlCoreDyingCursorRuntimeOperations coreDyingCursorRuntimeOperations, ICharacterCaiMaoOlCoreCardMovementProgramsOperations coreCardMovementProgramsOperations, ICharacterCaiMaoOlCoreCharacterStateProgramsOperations coreCharacterStateProgramsOperations, ICharacterCaiMaoOlCoreHpChangeProgramsOperations coreHpChangeProgramsOperations, ICharacterCaiMaoOlCoreNationalSkillsOperations coreNationalSkillsOperations, ICharacterCaiMaoOlCoreObserverSubtreesOperations coreObserverSubtreesOperations, ICharacterCaiMaoOlCorePrivateOfferReturnsOperations corePrivateOfferReturnsOperations, ICharacterCaiMaoOlCoreProgramAtomicCardMovesOperations coreProgramAtomicCardMovesOperations, ICharacterCaiMaoOlCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICharacterCaiMaoOlCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterCaiMaoOlCoreProgramRevealsOperations coreProgramRevealsOperations, ICharacterCaiMaoOlCoreRuntimeOperations coreRuntimeOperations, ICharacterCaiMaoOlCoreSkillOwnershipWindowsOperations coreSkillOwnershipWindowsOperations, ICharacterCaiMaoOlCoreSkillProgramsOperations coreSkillProgramsOperations, ICharacterCaiMaoOlCoreTableOrderOperations coreTableOrderOperations, ICharacterCaiMaoOlCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICharacterCaiMaoOlCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICharacterCaiMaoOlCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICharacterCaiMaoOlCorePlayerViewProjectionOperations corePlayerViewProjectionOperations, ICharacterCaiMaoOlCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICharacterCaiMaoOlCharacterCaiMaoOlOperations characterCaiMaoOlOperations, ICharacterCaiMaoOlResolutionStackOperations resolutionStackOperations2, ICharacterCaiMaoOlCoreObserverSubtreesOperations2 coreObserverSubtreesOperations2, ICharacterCaiMaoOlCoreRecoveryReplacementDamageRidesOperations coreRecoveryReplacementDamageRidesOperations, ICharacterCaiMaoOlKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess, global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess coreAiBrainsAiBrainsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess coreCardMovementJournalCardMovementsMovementSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterLuoXianOlOperations = characterLuoXianOlOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._randomOperations = randomOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreCharacterStateProgramsOperations = coreCharacterStateProgramsOperations;
        this._coreHpChangeProgramsOperations = coreHpChangeProgramsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreObserverSubtreesOperations = coreObserverSubtreesOperations;
        this._corePrivateOfferReturnsOperations = corePrivateOfferReturnsOperations;
        this._coreProgramAtomicCardMovesOperations = coreProgramAtomicCardMovesOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreProgramRevealsOperations = coreProgramRevealsOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSkillOwnershipWindowsOperations = coreSkillOwnershipWindowsOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._coreTableOrderOperations = coreTableOrderOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._corePlayerViewProjectionOperations = corePlayerViewProjectionOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._characterCaiMaoOlOperations = characterCaiMaoOlOperations;
        this._resolutionStackOperations2 = resolutionStackOperations2;
        this._coreObserverSubtreesOperations2 = coreObserverSubtreesOperations2;
        this._coreRecoveryReplacementDamageRidesOperations = coreRecoveryReplacementDamageRidesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess = coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess;
        this._coreAiBrainsAiBrainsAccess = coreAiBrainsAiBrainsAccess;
        this._coreCardMovementJournalCardMovementsMovementSequenceAccess = coreCardMovementJournalCardMovementsMovementSequenceAccess;
        this._coreMatchOutcomeStatusAccess = coreMatchOutcomeStatusAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
