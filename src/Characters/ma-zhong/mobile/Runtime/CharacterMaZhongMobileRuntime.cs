#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterMaZhongMobileRuntime : global::CardGame.Core.IMobileMaZhongHost
{
    private readonly ICharacterMaZhongMobileCardZonesOperations _cardZonesOperations;
    private readonly ICharacterMaZhongMobileContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterMaZhongMobileResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterMaZhongMobileSkillRuntimeStateOperations _skillRuntimeStateOperations;
    private readonly ICharacterMaZhongMobileCoreAdvancedLifecycleOperations _coreAdvancedLifecycleOperations;
    private readonly ICharacterMaZhongMobileCoreCardActionsOperations _coreCardActionsOperations;
    private readonly ICharacterMaZhongMobileCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICharacterMaZhongMobileCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICharacterMaZhongMobileCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterMaZhongMobileCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICharacterMaZhongMobileKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess _coreCardMovementJournalCardMovementsMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterMaZhongMobileRuntime(ICharacterMaZhongMobileCardZonesOperations cardZonesOperations, ICharacterMaZhongMobileContentRegistryOperations contentRegistryOperations, ICharacterMaZhongMobileResolutionStackOperations resolutionStackOperations, ICharacterMaZhongMobileSkillRuntimeStateOperations skillRuntimeStateOperations, ICharacterMaZhongMobileCoreAdvancedLifecycleOperations coreAdvancedLifecycleOperations, ICharacterMaZhongMobileCoreCardActionsOperations coreCardActionsOperations, ICharacterMaZhongMobileCoreNationalSkillsOperations coreNationalSkillsOperations, ICharacterMaZhongMobileCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICharacterMaZhongMobileCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterMaZhongMobileCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICharacterMaZhongMobileKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess coreCardMovementJournalCardMovementsMovementSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._skillRuntimeStateOperations = skillRuntimeStateOperations;
        this._coreAdvancedLifecycleOperations = coreAdvancedLifecycleOperations;
        this._coreCardActionsOperations = coreCardActionsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess = coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
        this._coreCardMovementJournalCardMovementsMovementSequenceAccess = coreCardMovementJournalCardMovementsMovementSequenceAccess;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
