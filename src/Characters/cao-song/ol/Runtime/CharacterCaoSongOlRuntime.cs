#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterCaoSongOlRuntime : global::CardGame.Core.ICaoSongProgramHost
{
    private readonly ICharacterCaoSongOlCharacterCaoXiOlOperations _characterCaoXiOlOperations;
    private readonly ICharacterCaoSongOlCharacterHuangQuanMouOperations _characterHuangQuanMouOperations;
    private readonly ICharacterCaoSongOlCharacterSunJianMouOlOperations _characterSunJianMouOlOperations;
    private readonly ICharacterCaoSongOlCardZonesOperations _cardZonesOperations;
    private readonly ICharacterCaoSongOlContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterCaoSongOlResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterCaoSongOlCoreDyingCursorRuntimeOperations _coreDyingCursorRuntimeOperations;
    private readonly ICharacterCaoSongOlCoreCardAttackStateOperations _coreCardAttackStateOperations;
    private readonly ICharacterCaoSongOlCoreHpChangeProgramsOperations _coreHpChangeProgramsOperations;
    private readonly ICharacterCaoSongOlCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICharacterCaoSongOlCoreObserverSubtreesOperations _coreObserverSubtreesOperations;
    private readonly ICharacterCaoSongOlCoreProgramAtomicCardMovesOperations _coreProgramAtomicCardMovesOperations;
    private readonly ICharacterCaoSongOlCoreProgramAttackAttemptOperations _coreProgramAttackAttemptOperations;
    private readonly ICharacterCaoSongOlCoreProgramContinuationsOperations _coreProgramContinuationsOperations;
    private readonly ICharacterCaoSongOlCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICharacterCaoSongOlCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterCaoSongOlCoreQimeiProgramsOperations _coreQimeiProgramsOperations;
    private readonly ICharacterCaoSongOlCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICharacterCaoSongOlCoreSkillProgramsOperations _coreSkillProgramsOperations;
    private readonly ICharacterCaoSongOlCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICharacterCaoSongOlCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICharacterCaoSongOlCorePhysicalDrawFacadesOperations _corePhysicalDrawFacadesOperations;
    private readonly ICharacterCaoSongOlCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICharacterCaoSongOlCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICharacterCaoSongOlCharacterCaoSongOlOperations _characterCaoSongOlOperations;
    private readonly ICharacterCaoSongOlCoreObserverSubtreesOperations2 _coreObserverSubtreesOperations2;
    private readonly ICharacterCaoSongOlKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess _coreCardMovementJournalCardMovementsMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterCaoSongOlRuntime(ICharacterCaoSongOlCharacterCaoXiOlOperations characterCaoXiOlOperations, ICharacterCaoSongOlCharacterHuangQuanMouOperations characterHuangQuanMouOperations, ICharacterCaoSongOlCharacterSunJianMouOlOperations characterSunJianMouOlOperations, ICharacterCaoSongOlCardZonesOperations cardZonesOperations, ICharacterCaoSongOlContentRegistryOperations contentRegistryOperations, ICharacterCaoSongOlResolutionStackOperations resolutionStackOperations, ICharacterCaoSongOlCoreDyingCursorRuntimeOperations coreDyingCursorRuntimeOperations, ICharacterCaoSongOlCoreCardAttackStateOperations coreCardAttackStateOperations, ICharacterCaoSongOlCoreHpChangeProgramsOperations coreHpChangeProgramsOperations, ICharacterCaoSongOlCoreNationalSkillsOperations coreNationalSkillsOperations, ICharacterCaoSongOlCoreObserverSubtreesOperations coreObserverSubtreesOperations, ICharacterCaoSongOlCoreProgramAtomicCardMovesOperations coreProgramAtomicCardMovesOperations, ICharacterCaoSongOlCoreProgramAttackAttemptOperations coreProgramAttackAttemptOperations, ICharacterCaoSongOlCoreProgramContinuationsOperations coreProgramContinuationsOperations, ICharacterCaoSongOlCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICharacterCaoSongOlCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterCaoSongOlCoreQimeiProgramsOperations coreQimeiProgramsOperations, ICharacterCaoSongOlCoreRuntimeOperations coreRuntimeOperations, ICharacterCaoSongOlCoreSkillProgramsOperations coreSkillProgramsOperations, ICharacterCaoSongOlCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICharacterCaoSongOlCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICharacterCaoSongOlCorePhysicalDrawFacadesOperations corePhysicalDrawFacadesOperations, ICharacterCaoSongOlCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICharacterCaoSongOlCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICharacterCaoSongOlCharacterCaoSongOlOperations characterCaoSongOlOperations, ICharacterCaoSongOlCoreObserverSubtreesOperations2 coreObserverSubtreesOperations2, ICharacterCaoSongOlKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess coreCardMovementJournalCardMovementsMovementSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterCaoXiOlOperations = characterCaoXiOlOperations;
        this._characterHuangQuanMouOperations = characterHuangQuanMouOperations;
        this._characterSunJianMouOlOperations = characterSunJianMouOlOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreCardAttackStateOperations = coreCardAttackStateOperations;
        this._coreHpChangeProgramsOperations = coreHpChangeProgramsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreObserverSubtreesOperations = coreObserverSubtreesOperations;
        this._coreProgramAtomicCardMovesOperations = coreProgramAtomicCardMovesOperations;
        this._coreProgramAttackAttemptOperations = coreProgramAttackAttemptOperations;
        this._coreProgramContinuationsOperations = coreProgramContinuationsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreQimeiProgramsOperations = coreQimeiProgramsOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalDrawFacadesOperations = corePhysicalDrawFacadesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._characterCaoSongOlOperations = characterCaoSongOlOperations;
        this._coreObserverSubtreesOperations2 = coreObserverSubtreesOperations2;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreCardMovementJournalCardMovementsMovementSequenceAccess = coreCardMovementJournalCardMovementsMovementSequenceAccess;
        this._coreMatchOutcomeStatusWinnerAccess = coreMatchOutcomeStatusWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
