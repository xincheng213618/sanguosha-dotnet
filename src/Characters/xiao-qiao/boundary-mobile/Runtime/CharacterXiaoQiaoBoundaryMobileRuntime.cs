#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterXiaoQiaoBoundaryMobileRuntime : global::CardGame.Core.IBoundaryXiaoQiaoMobileHost
{
    private readonly ICharacterXiaoQiaoBoundaryMobileCardZonesOperations _cardZonesOperations;
    private readonly ICharacterXiaoQiaoBoundaryMobileContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterXiaoQiaoBoundaryMobileResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterXiaoQiaoBoundaryMobileCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICharacterXiaoQiaoBoundaryMobileCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICharacterXiaoQiaoBoundaryMobileCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICharacterXiaoQiaoBoundaryMobileCoreSkillProgramsOperations _coreSkillProgramsOperations;
    private readonly ICharacterXiaoQiaoBoundaryMobileCoreJudgmentCursorRuntimeOperations _coreJudgmentCursorRuntimeOperations;
    private readonly ICharacterXiaoQiaoBoundaryMobileCoreNativeDamageHandlesOperations _coreNativeDamageHandlesOperations;
    private readonly ICharacterXiaoQiaoBoundaryMobileCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICharacterXiaoQiaoBoundaryMobileCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICharacterXiaoQiaoBoundaryMobileKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterXiaoQiaoBoundaryMobileRuntime(ICharacterXiaoQiaoBoundaryMobileCardZonesOperations cardZonesOperations, ICharacterXiaoQiaoBoundaryMobileContentRegistryOperations contentRegistryOperations, ICharacterXiaoQiaoBoundaryMobileResolutionStackOperations resolutionStackOperations, ICharacterXiaoQiaoBoundaryMobileCoreNationalSkillsOperations coreNationalSkillsOperations, ICharacterXiaoQiaoBoundaryMobileCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICharacterXiaoQiaoBoundaryMobileCoreRuntimeOperations coreRuntimeOperations, ICharacterXiaoQiaoBoundaryMobileCoreSkillProgramsOperations coreSkillProgramsOperations, ICharacterXiaoQiaoBoundaryMobileCoreJudgmentCursorRuntimeOperations coreJudgmentCursorRuntimeOperations, ICharacterXiaoQiaoBoundaryMobileCoreNativeDamageHandlesOperations coreNativeDamageHandlesOperations, ICharacterXiaoQiaoBoundaryMobileCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICharacterXiaoQiaoBoundaryMobileCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICharacterXiaoQiaoBoundaryMobileKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._coreJudgmentCursorRuntimeOperations = coreJudgmentCursorRuntimeOperations;
        this._coreNativeDamageHandlesOperations = coreNativeDamageHandlesOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchOutcomeStatusAccess = coreMatchOutcomeStatusAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
