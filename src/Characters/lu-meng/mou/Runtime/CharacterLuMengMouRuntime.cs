#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CharacterLuMengMouRuntime
{
    private readonly ICharacterLuMengMouCharacterHuangQuanMouOperations _characterHuangQuanMouOperations;
    private readonly ICharacterLuMengMouCardZonesOperations _cardZonesOperations;
    private readonly ICharacterLuMengMouSkillRuntimeStateOperations _skillRuntimeStateOperations;
    private readonly ICharacterLuMengMouCoreCardAttackStateOperations _coreCardAttackStateOperations;
    private readonly ICharacterLuMengMouCoreCardUseLifecycleOperations _coreCardUseLifecycleOperations;
    private readonly ICharacterLuMengMouCoreMouLuMengOperations _coreMouLuMengOperations;
    private readonly ICharacterLuMengMouCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICharacterLuMengMouCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICharacterLuMengMouCoreMatchLogOperations _coreMatchLogOperations;
    private readonly ICharacterLuMengMouCoreNativeAttackCompletionOperations _coreNativeAttackCompletionOperations;
    private readonly ICharacterLuMengMouCoreNativeGroupCompletionOperations _coreNativeGroupCompletionOperations;
    private readonly ICharacterLuMengMouCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICharacterLuMengMouCoreNativeRecoveryFramesOperations _coreNativeRecoveryFramesOperations;
    private readonly ICharacterLuMengMouCoreNativeSlashCompletionOperations _coreNativeSlashCompletionOperations;
    private readonly ICharacterLuMengMouCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICharacterLuMengMouCorePhysicalMovementFacadesOperations _corePhysicalMovementFacadesOperations;
    private readonly ICharacterLuMengMouCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICharacterLuMengMouKernelCharacterLuMengMouOperations _kernelCharacterLuMengMouOperations;
    private readonly ICharacterLuMengMouKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly ICharacterLuMengMouKernelCoreGameEngineRuntimeOperations _kernelCoreGameEngineRuntimeOperations;
    private readonly ICharacterLuMengMouKernelCoreMainOperations _kernelCoreMainOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess _coreCardMovementJournalMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionOptionsAccess _coreMatchDefinitionOptionsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterLuMengMouRuntime(ICharacterLuMengMouCharacterHuangQuanMouOperations characterHuangQuanMouOperations, ICharacterLuMengMouCardZonesOperations cardZonesOperations, ICharacterLuMengMouSkillRuntimeStateOperations skillRuntimeStateOperations, ICharacterLuMengMouCoreCardAttackStateOperations coreCardAttackStateOperations, ICharacterLuMengMouCoreCardUseLifecycleOperations coreCardUseLifecycleOperations, ICharacterLuMengMouCoreMouLuMengOperations coreMouLuMengOperations, ICharacterLuMengMouCoreNationalSkillsOperations coreNationalSkillsOperations, ICharacterLuMengMouCoreRuntimeOperations coreRuntimeOperations, ICharacterLuMengMouCoreMatchLogOperations coreMatchLogOperations, ICharacterLuMengMouCoreNativeAttackCompletionOperations coreNativeAttackCompletionOperations, ICharacterLuMengMouCoreNativeGroupCompletionOperations coreNativeGroupCompletionOperations, ICharacterLuMengMouCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICharacterLuMengMouCoreNativeRecoveryFramesOperations coreNativeRecoveryFramesOperations, ICharacterLuMengMouCoreNativeSlashCompletionOperations coreNativeSlashCompletionOperations, ICharacterLuMengMouCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICharacterLuMengMouCorePhysicalMovementFacadesOperations corePhysicalMovementFacadesOperations, ICharacterLuMengMouCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICharacterLuMengMouKernelCharacterLuMengMouOperations kernelCharacterLuMengMouOperations, ICharacterLuMengMouKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, ICharacterLuMengMouKernelCoreGameEngineRuntimeOperations kernelCoreGameEngineRuntimeOperations, ICharacterLuMengMouKernelCoreMainOperations kernelCoreMainOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess coreCardMovementJournalMovementSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionOptionsAccess coreMatchDefinitionOptionsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
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
