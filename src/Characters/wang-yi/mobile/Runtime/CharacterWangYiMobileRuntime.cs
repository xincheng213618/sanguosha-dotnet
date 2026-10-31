#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CharacterWangYiMobileRuntime
{
    private readonly ICharacterWangYiMobileCardZonesOperations _cardZonesOperations;
    private readonly ICharacterWangYiMobileContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterWangYiMobileResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterWangYiMobileCoreJudgmentReplacementProgramsOperations _coreJudgmentReplacementProgramsOperations;
    private readonly ICharacterWangYiMobileCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICharacterWangYiMobileCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterWangYiMobileCoreJudgmentCursorRuntimeOperations _coreJudgmentCursorRuntimeOperations;
    private readonly ICharacterWangYiMobileCoreNativeCardAppearanceOperations _coreNativeCardAppearanceOperations;
    private readonly ICharacterWangYiMobileCoreNativeDamageHandlesOperations _coreNativeDamageHandlesOperations;
    private readonly ICharacterWangYiMobileCoreNativeJudgmentOperations _coreNativeJudgmentOperations;
    private readonly ICharacterWangYiMobileCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICharacterWangYiMobileCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICharacterWangYiMobileCorePhysicalDrawFacadesOperations _corePhysicalDrawFacadesOperations;
    private readonly ICharacterWangYiMobileCoreSnapshotValueProjectionOperations _coreSnapshotValueProjectionOperations;
    private readonly ICharacterWangYiMobileKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterWangYiMobileRuntime(ICharacterWangYiMobileCardZonesOperations cardZonesOperations, ICharacterWangYiMobileContentRegistryOperations contentRegistryOperations, ICharacterWangYiMobileResolutionStackOperations resolutionStackOperations, ICharacterWangYiMobileCoreJudgmentReplacementProgramsOperations coreJudgmentReplacementProgramsOperations, ICharacterWangYiMobileCoreNationalSkillsOperations coreNationalSkillsOperations, ICharacterWangYiMobileCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterWangYiMobileCoreJudgmentCursorRuntimeOperations coreJudgmentCursorRuntimeOperations, ICharacterWangYiMobileCoreNativeCardAppearanceOperations coreNativeCardAppearanceOperations, ICharacterWangYiMobileCoreNativeDamageHandlesOperations coreNativeDamageHandlesOperations, ICharacterWangYiMobileCoreNativeJudgmentOperations coreNativeJudgmentOperations, ICharacterWangYiMobileCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICharacterWangYiMobileCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICharacterWangYiMobileCorePhysicalDrawFacadesOperations corePhysicalDrawFacadesOperations, ICharacterWangYiMobileCoreSnapshotValueProjectionOperations coreSnapshotValueProjectionOperations, ICharacterWangYiMobileKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreJudgmentReplacementProgramsOperations = coreJudgmentReplacementProgramsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreJudgmentCursorRuntimeOperations = coreJudgmentCursorRuntimeOperations;
        this._coreNativeCardAppearanceOperations = coreNativeCardAppearanceOperations;
        this._coreNativeDamageHandlesOperations = coreNativeDamageHandlesOperations;
        this._coreNativeJudgmentOperations = coreNativeJudgmentOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalDrawFacadesOperations = corePhysicalDrawFacadesOperations;
        this._coreSnapshotValueProjectionOperations = coreSnapshotValueProjectionOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchOutcomeStatusAccess = coreMatchOutcomeStatusAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
