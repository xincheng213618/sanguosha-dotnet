#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineFrozenHandRosterRuntime : global::CardGame.Core.IFrozenHandRosterHost
{
    private readonly ICoreFrozenHandRosterContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreFrozenHandRosterResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreFrozenHandRosterCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreFrozenHandRosterCoreCardMovementProgramsOperations _coreCardMovementProgramsOperations;
    private readonly ICoreFrozenHandRosterCoreFireTargetBenefitsOperations _coreFireTargetBenefitsOperations;
    private readonly ICoreFrozenHandRosterCoreGainGiftProgramsOperations _coreGainGiftProgramsOperations;
    private readonly ICoreFrozenHandRosterCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICoreFrozenHandRosterCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreFrozenHandRosterCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreFrozenHandRosterCoreSkillProgramsOperations _coreSkillProgramsOperations;
    private readonly ICoreFrozenHandRosterCoreTableOrderOperations _coreTableOrderOperations;
    private readonly ICoreFrozenHandRosterCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICoreFrozenHandRosterCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICoreFrozenHandRosterCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICoreFrozenHandRosterCorePlayerViewProjectionOperations _corePlayerViewProjectionOperations;
    private readonly ICoreFrozenHandRosterCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICoreFrozenHandRosterKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess _coreAiBrainsAiBrainsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess _coreCardMovementJournalCardMovementsMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineFrozenHandRosterRuntime(ICoreFrozenHandRosterContentRegistryOperations contentRegistryOperations, ICoreFrozenHandRosterResolutionStackOperations resolutionStackOperations, ICoreFrozenHandRosterCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreFrozenHandRosterCoreCardMovementProgramsOperations coreCardMovementProgramsOperations, ICoreFrozenHandRosterCoreFireTargetBenefitsOperations coreFireTargetBenefitsOperations, ICoreFrozenHandRosterCoreGainGiftProgramsOperations coreGainGiftProgramsOperations, ICoreFrozenHandRosterCoreNationalSkillsOperations coreNationalSkillsOperations, ICoreFrozenHandRosterCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreFrozenHandRosterCoreRuntimeOperations coreRuntimeOperations, ICoreFrozenHandRosterCoreSkillProgramsOperations coreSkillProgramsOperations, ICoreFrozenHandRosterCoreTableOrderOperations coreTableOrderOperations, ICoreFrozenHandRosterCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICoreFrozenHandRosterCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICoreFrozenHandRosterCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICoreFrozenHandRosterCorePlayerViewProjectionOperations corePlayerViewProjectionOperations, ICoreFrozenHandRosterCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICoreFrozenHandRosterKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess coreAiBrainsAiBrainsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess coreCardMovementJournalCardMovementsMovementSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreFireTargetBenefitsOperations = coreFireTargetBenefitsOperations;
        this._coreGainGiftProgramsOperations = coreGainGiftProgramsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._coreTableOrderOperations = coreTableOrderOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._corePlayerViewProjectionOperations = corePlayerViewProjectionOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreAiBrainsAiBrainsAccess = coreAiBrainsAiBrainsAccess;
        this._coreCardMovementJournalCardMovementsMovementSequenceAccess = coreCardMovementJournalCardMovementsMovementSequenceAccess;
        this._coreMatchOutcomeStatusWinnerAccess = coreMatchOutcomeStatusWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
