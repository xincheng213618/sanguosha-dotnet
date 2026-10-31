#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterJiangGanOlRuntime : global::CardGame.Core.IJiangGanProgramHost
{
    private readonly ICharacterJiangGanOlCharacterBianFuRenOlOperations _characterBianFuRenOlOperations;
    private readonly ICharacterJiangGanOlCharacterHuangQuanMouOperations _characterHuangQuanMouOperations;
    private readonly ICharacterJiangGanOlCardZonesOperations _cardZonesOperations;
    private readonly ICharacterJiangGanOlContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterJiangGanOlRandomOperations _randomOperations;
    private readonly ICharacterJiangGanOlResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterJiangGanOlCoreCardMovementProgramsOperations _coreCardMovementProgramsOperations;
    private readonly ICharacterJiangGanOlCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICharacterJiangGanOlCoreProgramCardPoliciesOperations _coreProgramCardPoliciesOperations;
    private readonly ICharacterJiangGanOlCoreProgramCategoryChallengesOperations _coreProgramCategoryChallengesOperations;
    private readonly ICharacterJiangGanOlCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterJiangGanOlCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICharacterJiangGanOlCoreSkillProgramsOperations _coreSkillProgramsOperations;
    private readonly ICharacterJiangGanOlCoreMatchLogOperations _coreMatchLogOperations;
    private readonly ICharacterJiangGanOlCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICharacterJiangGanOlCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICharacterJiangGanOlCorePhysicalMovementFacadesOperations _corePhysicalMovementFacadesOperations;
    private readonly ICharacterJiangGanOlCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICharacterJiangGanOlKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess _coreCardMovementJournalMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineSkillProgramsProgramPhaseUsesProgramUsesAccess _coreGameEngineSkillProgramsProgramPhaseUsesProgramUsesAccess;
    internal CharacterJiangGanOlRuntime(ICharacterJiangGanOlCharacterBianFuRenOlOperations characterBianFuRenOlOperations, ICharacterJiangGanOlCharacterHuangQuanMouOperations characterHuangQuanMouOperations, ICharacterJiangGanOlCardZonesOperations cardZonesOperations, ICharacterJiangGanOlContentRegistryOperations contentRegistryOperations, ICharacterJiangGanOlRandomOperations randomOperations, ICharacterJiangGanOlResolutionStackOperations resolutionStackOperations, ICharacterJiangGanOlCoreCardMovementProgramsOperations coreCardMovementProgramsOperations, ICharacterJiangGanOlCoreNationalSkillsOperations coreNationalSkillsOperations, ICharacterJiangGanOlCoreProgramCardPoliciesOperations coreProgramCardPoliciesOperations, ICharacterJiangGanOlCoreProgramCategoryChallengesOperations coreProgramCategoryChallengesOperations, ICharacterJiangGanOlCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterJiangGanOlCoreRuntimeOperations coreRuntimeOperations, ICharacterJiangGanOlCoreSkillProgramsOperations coreSkillProgramsOperations, ICharacterJiangGanOlCoreMatchLogOperations coreMatchLogOperations, ICharacterJiangGanOlCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICharacterJiangGanOlCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICharacterJiangGanOlCorePhysicalMovementFacadesOperations corePhysicalMovementFacadesOperations, ICharacterJiangGanOlCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICharacterJiangGanOlKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess coreCardMovementJournalMovementSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineSkillProgramsProgramPhaseUsesProgramUsesAccess coreGameEngineSkillProgramsProgramPhaseUsesProgramUsesAccess)
    {
        this._characterBianFuRenOlOperations = characterBianFuRenOlOperations;
        this._characterHuangQuanMouOperations = characterHuangQuanMouOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._randomOperations = randomOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreProgramCategoryChallengesOperations = coreProgramCategoryChallengesOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreCardMovementJournalMovementSequenceAccess = coreCardMovementJournalMovementSequenceAccess;
        this._coreMatchOutcomeStatusAccess = coreMatchOutcomeStatusAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreGameEngineSkillProgramsProgramPhaseUsesProgramUsesAccess = coreGameEngineSkillProgramsProgramPhaseUsesProgramUsesAccess;
    }
}
