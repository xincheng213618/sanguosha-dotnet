#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterZhangChangPuOlRuntime : global::CardGame.Core.IZhangChangPuProgramHost
{
    private readonly ICharacterZhangChangPuOlCardZonesOperations _cardZonesOperations;
    private readonly ICharacterZhangChangPuOlContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterZhangChangPuOlResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterZhangChangPuOlCoreAttributedMarkerEventsOperations _coreAttributedMarkerEventsOperations;
    private readonly ICharacterZhangChangPuOlCoreCardUseModulesOperations _coreCardUseModulesOperations;
    private readonly ICharacterZhangChangPuOlCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICharacterZhangChangPuOlCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterZhangChangPuOlCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICharacterZhangChangPuOlCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICharacterZhangChangPuOlCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICharacterZhangChangPuOlCorePhysicalDrawFacadesOperations _corePhysicalDrawFacadesOperations;
    private readonly ICharacterZhangChangPuOlCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICharacterZhangChangPuOlKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterZhangChangPuOlRuntime(ICharacterZhangChangPuOlCardZonesOperations cardZonesOperations, ICharacterZhangChangPuOlContentRegistryOperations contentRegistryOperations, ICharacterZhangChangPuOlResolutionStackOperations resolutionStackOperations, ICharacterZhangChangPuOlCoreAttributedMarkerEventsOperations coreAttributedMarkerEventsOperations, ICharacterZhangChangPuOlCoreCardUseModulesOperations coreCardUseModulesOperations, ICharacterZhangChangPuOlCoreNationalSkillsOperations coreNationalSkillsOperations, ICharacterZhangChangPuOlCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterZhangChangPuOlCoreRuntimeOperations coreRuntimeOperations, ICharacterZhangChangPuOlCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICharacterZhangChangPuOlCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICharacterZhangChangPuOlCorePhysicalDrawFacadesOperations corePhysicalDrawFacadesOperations, ICharacterZhangChangPuOlCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICharacterZhangChangPuOlKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAttributedMarkerEventsOperations = coreAttributedMarkerEventsOperations;
        this._coreCardUseModulesOperations = coreCardUseModulesOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalDrawFacadesOperations = corePhysicalDrawFacadesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchOutcomeStatusWinnerAccess = coreMatchOutcomeStatusWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
