#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterCaoPiOlGodRuntime : global::CardGame.Core.IOlShenCaoPiFacts, global::CardGame.Core.IOlShenCaoPiOperations
{
    private readonly ICharacterCaoPiOlGodContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterCaoPiOlGodObservedSkillGrantRevisionsOperations _observedSkillGrantRevisionsOperations;
    private readonly ICharacterCaoPiOlGodCoreGameActivationAwakeningOperations _coreGameActivationAwakeningOperations;
    private readonly ICharacterCaoPiOlGodCoreProgramContinuationsOperations _coreProgramContinuationsOperations;
    private readonly ICharacterCaoPiOlGodCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICharacterCaoPiOlGodCoreProgramSkillChangesOperations _coreProgramSkillChangesOperations;
    private readonly ICharacterCaoPiOlGodCorePublicPileCashOutOperations _corePublicPileCashOutOperations;
    private readonly ICharacterCaoPiOlGodCorePublicPileSourcesOperations _corePublicPileSourcesOperations;
    private readonly ICharacterCaoPiOlGodCorePhysicalMovementFacadesOperations _corePhysicalMovementFacadesOperations;
    private readonly ICharacterCaoPiOlGodCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICharacterCaoPiOlGodCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICharacterCaoPiOlGodKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess _coreCardMovementJournalCardMovementsMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterCaoPiOlGodRuntime(ICharacterCaoPiOlGodContentRegistryOperations contentRegistryOperations, ICharacterCaoPiOlGodObservedSkillGrantRevisionsOperations observedSkillGrantRevisionsOperations, ICharacterCaoPiOlGodCoreGameActivationAwakeningOperations coreGameActivationAwakeningOperations, ICharacterCaoPiOlGodCoreProgramContinuationsOperations coreProgramContinuationsOperations, ICharacterCaoPiOlGodCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICharacterCaoPiOlGodCoreProgramSkillChangesOperations coreProgramSkillChangesOperations, ICharacterCaoPiOlGodCorePublicPileCashOutOperations corePublicPileCashOutOperations, ICharacterCaoPiOlGodCorePublicPileSourcesOperations corePublicPileSourcesOperations, ICharacterCaoPiOlGodCorePhysicalMovementFacadesOperations corePhysicalMovementFacadesOperations, ICharacterCaoPiOlGodCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICharacterCaoPiOlGodCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICharacterCaoPiOlGodKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess coreCardMovementJournalCardMovementsMovementSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._observedSkillGrantRevisionsOperations = observedSkillGrantRevisionsOperations;
        this._coreGameActivationAwakeningOperations = coreGameActivationAwakeningOperations;
        this._coreProgramContinuationsOperations = coreProgramContinuationsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramSkillChangesOperations = coreProgramSkillChangesOperations;
        this._corePublicPileCashOutOperations = corePublicPileCashOutOperations;
        this._corePublicPileSourcesOperations = corePublicPileSourcesOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreCardMovementJournalCardMovementsMovementSequenceAccess = coreCardMovementJournalCardMovementsMovementSequenceAccess;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
