#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CharacterLuMengMouRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuangQuanMouEntries _characterHuangQuanMouOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelSkillRuntimeStateAccess _skillRuntimeStateOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries _coreCardAttackStateOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineMouLuMengEntries _coreMouLuMengOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeAttackCompletionEntries _coreNativeAttackCompletionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeGroupCompletionEntries _coreNativeGroupCompletionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativePromptBoundaryEntries _coreNativePromptBoundaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeRecoveryFramesEntries _coreNativeRecoveryFramesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeSlashCompletionEntries _coreNativeSlashCompletionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries _corePhysicalMovementFacadesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCreateYingboGiftHandleOperations _kernelCharacterLuMengMouOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceToHumanBoundaryOperations _kernelCoreGameEngineRuntimeOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAcceptAndRejectOperations _kernelCoreMainOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess _coreCardMovementJournalMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionOptionsAccess _coreMatchDefinitionOptionsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterLuMengMouRuntime(global::CardGame.Core.RuntimeEntries.CharacterHuangQuanMouEntries characterHuangQuanMouOperations, global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations, global::CardGame.Core.EngineOwners.IKernelSkillRuntimeStateAccess skillRuntimeStateOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries coreCardAttackStateOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineMouLuMengEntries coreMouLuMengOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations, global::CardGame.Core.RuntimeEntries.CoreNativeAttackCompletionEntries coreNativeAttackCompletionOperations, global::CardGame.Core.RuntimeEntries.CoreNativeGroupCompletionEntries coreNativeGroupCompletionOperations, global::CardGame.Core.RuntimeEntries.CoreNativePromptBoundaryEntries coreNativePromptBoundaryOperations, global::CardGame.Core.RuntimeEntries.CoreNativeRecoveryFramesEntries coreNativeRecoveryFramesOperations, global::CardGame.Core.RuntimeEntries.CoreNativeSlashCompletionEntries coreNativeSlashCompletionOperations, global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries corePhysicalMovementFacadesOperations, global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations, global::CardGame.Core.EngineOwners.IKernelCreateYingboGiftHandleOperations kernelCharacterLuMengMouOperations, global::CardGame.Core.EngineOwners.IKernelRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceToHumanBoundaryOperations kernelCoreGameEngineRuntimeOperations, global::CardGame.Core.EngineOwners.IKernelAcceptAndRejectOperations kernelCoreMainOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess coreCardMovementJournalMovementSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionOptionsAccess coreMatchDefinitionOptionsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterHuangQuanMouOperations = characterHuangQuanMouOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._skillRuntimeStateOperations = skillRuntimeStateOperations;
        this._coreCardAttackStateOperations = coreCardAttackStateOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreMouLuMengOperations = coreMouLuMengOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativeAttackCompletionOperations = coreNativeAttackCompletionOperations;
        this._coreNativeGroupCompletionOperations = coreNativeGroupCompletionOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._coreNativeRecoveryFramesOperations = coreNativeRecoveryFramesOperations;
        this._coreNativeSlashCompletionOperations = coreNativeSlashCompletionOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCharacterLuMengMouOperations = kernelCharacterLuMengMouOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._kernelCoreGameEngineRuntimeOperations = kernelCoreGameEngineRuntimeOperations;
        this._kernelCoreMainOperations = kernelCoreMainOperations;
        this._coreCardMovementJournalMovementSequenceAccess = coreCardMovementJournalMovementSequenceAccess;
        this._coreMatchDefinitionOptionsAccess = coreMatchDefinitionOptionsAccess;
        this._coreMatchOutcomeStatusWinnerAccess = coreMatchOutcomeStatusWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
