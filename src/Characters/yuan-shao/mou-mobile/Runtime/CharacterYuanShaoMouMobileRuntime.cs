#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterYuanShaoMouMobileRuntime : global::CardGame.Core.IMouYuanShaoHost
{
    private readonly ICharacterYuanShaoMouMobileContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterYuanShaoMouMobileResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterYuanShaoMouMobileCoreCardActionsOperations _coreCardActionsOperations;
    private readonly ICharacterYuanShaoMouMobileCoreCardUseLifecycleOperations _coreCardUseLifecycleOperations;
    private readonly ICharacterYuanShaoMouMobileCoreLordSkillProjectionsOperations _coreLordSkillProjectionsOperations;
    private readonly ICharacterYuanShaoMouMobileCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICharacterYuanShaoMouMobileCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICharacterYuanShaoMouMobileCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterYuanShaoMouMobileCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICharacterYuanShaoMouMobileCoreMatchSetupDefinitionsOperations _coreMatchSetupDefinitionsOperations;
    private readonly ICharacterYuanShaoMouMobileCoreResolutionIdentityBoundaryOperations _coreResolutionIdentityBoundaryOperations;
    private readonly ICharacterYuanShaoMouMobileCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICharacterYuanShaoMouMobileKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatAccess _coreActualTurnProgressCurrentSeatAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess _coreCardMovementJournalCardMovementsMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterYuanShaoMouMobileRuntime(ICharacterYuanShaoMouMobileContentRegistryOperations contentRegistryOperations, ICharacterYuanShaoMouMobileResolutionStackOperations resolutionStackOperations, ICharacterYuanShaoMouMobileCoreCardActionsOperations coreCardActionsOperations, ICharacterYuanShaoMouMobileCoreCardUseLifecycleOperations coreCardUseLifecycleOperations, ICharacterYuanShaoMouMobileCoreLordSkillProjectionsOperations coreLordSkillProjectionsOperations, ICharacterYuanShaoMouMobileCoreNationalSkillsOperations coreNationalSkillsOperations, ICharacterYuanShaoMouMobileCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICharacterYuanShaoMouMobileCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterYuanShaoMouMobileCoreRuntimeOperations coreRuntimeOperations, ICharacterYuanShaoMouMobileCoreMatchSetupDefinitionsOperations coreMatchSetupDefinitionsOperations, ICharacterYuanShaoMouMobileCoreResolutionIdentityBoundaryOperations coreResolutionIdentityBoundaryOperations, ICharacterYuanShaoMouMobileCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICharacterYuanShaoMouMobileKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatAccess coreActualTurnProgressCurrentSeatAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess coreCardMovementJournalCardMovementsMovementSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardActionsOperations = coreCardActionsOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreLordSkillProjectionsOperations = coreLordSkillProjectionsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreMatchSetupDefinitionsOperations = coreMatchSetupDefinitionsOperations;
        this._coreResolutionIdentityBoundaryOperations = coreResolutionIdentityBoundaryOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatAccess = coreActualTurnProgressCurrentSeatAccess;
        this._coreCardMovementJournalCardMovementsMovementSequenceAccess = coreCardMovementJournalCardMovementsMovementSequenceAccess;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
