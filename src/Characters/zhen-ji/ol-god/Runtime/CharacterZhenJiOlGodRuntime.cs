#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterZhenJiOlGodRuntime : global::CardGame.Core.IOlShenZhenJiFacts, global::CardGame.Core.IOlShenZhenJiOperations
{
    private readonly ICharacterZhenJiOlGodContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterZhenJiOlGodCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICharacterZhenJiOlGodCoreNativeLogicalDiscardsOperations _coreNativeLogicalDiscardsOperations;
    private readonly ICharacterZhenJiOlGodCoreProgramCardPaymentsOperations _coreProgramCardPaymentsOperations;
    private readonly ICharacterZhenJiOlGodCoreProgramContinuationsOperations _coreProgramContinuationsOperations;
    private readonly ICharacterZhenJiOlGodCoreSkillProgramsOperations _coreSkillProgramsOperations;
    private readonly ICharacterZhenJiOlGodCoreTableOrderOperations _coreTableOrderOperations;
    private readonly ICharacterZhenJiOlGodCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICharacterZhenJiOlGodCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICharacterZhenJiOlGodKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly ICharacterZhenJiOlGodOlShenZhenJiOperations _olShenZhenJiOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess _coreCardMovementJournalMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterZhenJiOlGodRuntime(ICharacterZhenJiOlGodContentRegistryOperations contentRegistryOperations, ICharacterZhenJiOlGodCoreNationalSkillsOperations coreNationalSkillsOperations, ICharacterZhenJiOlGodCoreNativeLogicalDiscardsOperations coreNativeLogicalDiscardsOperations, ICharacterZhenJiOlGodCoreProgramCardPaymentsOperations coreProgramCardPaymentsOperations, ICharacterZhenJiOlGodCoreProgramContinuationsOperations coreProgramContinuationsOperations, ICharacterZhenJiOlGodCoreSkillProgramsOperations coreSkillProgramsOperations, ICharacterZhenJiOlGodCoreTableOrderOperations coreTableOrderOperations, ICharacterZhenJiOlGodCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICharacterZhenJiOlGodCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICharacterZhenJiOlGodKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, ICharacterZhenJiOlGodOlShenZhenJiOperations olShenZhenJiOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess coreCardMovementJournalMovementSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreNativeLogicalDiscardsOperations = coreNativeLogicalDiscardsOperations;
        this._coreProgramCardPaymentsOperations = coreProgramCardPaymentsOperations;
        this._coreProgramContinuationsOperations = coreProgramContinuationsOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._coreTableOrderOperations = coreTableOrderOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._olShenZhenJiOperations = olShenZhenJiOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreCardMovementJournalMovementSequenceAccess = coreCardMovementJournalMovementSequenceAccess;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
