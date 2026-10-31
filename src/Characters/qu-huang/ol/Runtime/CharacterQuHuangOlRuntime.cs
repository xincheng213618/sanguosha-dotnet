#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterQuHuangOlRuntime : global::CardGame.Core.IQuHuangFacts, global::CardGame.Core.IQuHuangNativeOperations, global::CardGame.Core.IQuHuangNativeQueries
{
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreActualTurnsEntries _coreActualTurnsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreDirectedTurnCardPoliciesEntries _coreDirectedTurnCardPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries _coreDyingCursorRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries _coreAdvancedLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternativeViewAsCostsEntries _coreAlternativeViewAsCostsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardMovementProgramsEntries _coreCardMovementProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseModulesEntries _coreCardUseModulesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentUsePhysicalClaimsEntries _coreCurrentUsePhysicalClaimsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDeckEquipmentProgramsEntries _coreDeckEquipmentProgramsOperations;
    private readonly ICharacterQuHuangOlCoreEquipmentContentOperations _coreEquipmentContentOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineEquipmentSkillsEntries _coreEquipmentSkillsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHandComparisonEntries _coreHandComparisonOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineObserverSubtreesEntries _coreObserverSubtreesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries _coreProgramCardPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRecoveryReplacementDamageRidesEntries _coreRecoveryReplacementDamageRidesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShownEntityTurnPoliciesEntries _coreShownEntityTurnPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardUseEntries _coreNativeCardUseOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDamagePoliciesEntries _coreNativeDamagePoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeNullificationEntries _coreNativeNullificationOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeTrickEffectsEntries _coreNativeTrickEffectsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreProgramParticipantPhaseOperationsEntries _coreProgramParticipantPhaseOperationsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreResolutionIdentityBoundaryEntries _coreResolutionIdentityBoundaryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceRulesAndPublishStateOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly ICharacterQuHuangOlQuHuangPhaseEndLedgerOperations _quHuangPhaseEndLedgerOperations;
    private readonly ICharacterQuHuangOlQuHuangOperations _quHuangOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess _coreActualRoundProgressRoundNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberTurnProgressionAccess _coreActualTurnProgressTurnNumberTurnProgressionAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess _coreCardMovementJournalMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardActionsCardActionSequenceAccess _coreGameEngineCardActionsCardActionSequenceAccess;
    internal CharacterQuHuangOlRuntime(global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations,global::CardGame.Core.RuntimeEntries.CoreActualTurnsEntries coreActualTurnsOperations,global::CardGame.Core.RuntimeEntries.CoreDirectedTurnCardPoliciesEntries coreDirectedTurnCardPoliciesOperations,global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries coreDyingCursorRuntimeOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries coreAdvancedLifecycleOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternativeViewAsCostsEntries coreAlternativeViewAsCostsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCardMovementProgramsEntries coreCardMovementProgramsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseModulesEntries coreCardUseModulesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentUsePhysicalClaimsEntries coreCurrentUsePhysicalClaimsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineDeckEquipmentProgramsEntries coreDeckEquipmentProgramsOperations,ICharacterQuHuangOlCoreEquipmentContentOperations coreEquipmentContentOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineEquipmentSkillsEntries coreEquipmentSkillsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineHandComparisonEntries coreHandComparisonOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineObserverSubtreesEntries coreObserverSubtreesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries coreProgramCardPoliciesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineRecoveryReplacementDamageRidesEntries coreRecoveryReplacementDamageRidesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineShownEntityTurnPoliciesEntries coreShownEntityTurnPoliciesOperations,global::CardGame.Core.RuntimeEntries.CoreNativeCardUseEntries coreNativeCardUseOperations,global::CardGame.Core.RuntimeEntries.CoreNativeDamagePoliciesEntries coreNativeDamagePoliciesOperations,global::CardGame.Core.RuntimeEntries.CoreNativeNullificationEntries coreNativeNullificationOperations,global::CardGame.Core.RuntimeEntries.CoreNativeTrickEffectsEntries coreNativeTrickEffectsOperations,global::CardGame.Core.RuntimeEntries.CoreProgramParticipantPhaseOperationsEntries coreProgramParticipantPhaseOperationsOperations,global::CardGame.Core.RuntimeEntries.CoreResolutionIdentityBoundaryEntries coreResolutionIdentityBoundaryOperations,global::CardGame.Core.EngineOwners.IKernelAdvanceRulesAndPublishStateOperations kernelCoreGameEngineRuleProgressionOperations,ICharacterQuHuangOlQuHuangPhaseEndLedgerOperations quHuangPhaseEndLedgerOperations,ICharacterQuHuangOlQuHuangOperations quHuangOperations,global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess coreActualRoundProgressRoundNumberAccess,global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberTurnProgressionAccess coreActualTurnProgressTurnNumberTurnProgressionAccess,global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess coreCardMovementJournalMovementSequenceAccess,global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess,global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess,global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardActionsCardActionSequenceAccess coreGameEngineCardActionsCardActionSequenceAccess)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreActualTurnsOperations = coreActualTurnsOperations;
        this._coreDirectedTurnCardPoliciesOperations = coreDirectedTurnCardPoliciesOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreAdvancedLifecycleOperations = coreAdvancedLifecycleOperations;
        this._coreAlternativeViewAsCostsOperations = coreAlternativeViewAsCostsOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreCardUseModulesOperations = coreCardUseModulesOperations;
        this._coreCurrentUsePhysicalClaimsOperations = coreCurrentUsePhysicalClaimsOperations;
        this._coreDeckEquipmentProgramsOperations = coreDeckEquipmentProgramsOperations;
        this._coreEquipmentContentOperations = coreEquipmentContentOperations;
        this._coreEquipmentSkillsOperations = coreEquipmentSkillsOperations;
        this._coreHandComparisonOperations = coreHandComparisonOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreObserverSubtreesOperations = coreObserverSubtreesOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRecoveryReplacementDamageRidesOperations = coreRecoveryReplacementDamageRidesOperations;
        this._coreShownEntityTurnPoliciesOperations = coreShownEntityTurnPoliciesOperations;
        this._coreNativeCardUseOperations = coreNativeCardUseOperations;
        this._coreNativeDamagePoliciesOperations = coreNativeDamagePoliciesOperations;
        this._coreNativeNullificationOperations = coreNativeNullificationOperations;
        this._coreNativeTrickEffectsOperations = coreNativeTrickEffectsOperations;
        this._coreProgramParticipantPhaseOperationsOperations = coreProgramParticipantPhaseOperationsOperations;
        this._coreResolutionIdentityBoundaryOperations = coreResolutionIdentityBoundaryOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._quHuangPhaseEndLedgerOperations = quHuangPhaseEndLedgerOperations;
        this._quHuangOperations = quHuangOperations;
        this._coreActualRoundProgressRoundNumberAccess = coreActualRoundProgressRoundNumberAccess;
        this._coreActualTurnProgressTurnNumberTurnProgressionAccess = coreActualTurnProgressTurnNumberTurnProgressionAccess;
        this._coreCardMovementJournalMovementSequenceAccess = coreCardMovementJournalMovementSequenceAccess;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreGameEngineCardActionsCardActionSequenceAccess = coreGameEngineCardActionsCardActionSequenceAccess;
    }
}
