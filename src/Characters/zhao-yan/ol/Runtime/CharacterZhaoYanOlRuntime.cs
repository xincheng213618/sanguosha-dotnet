#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterZhaoYanOlRuntime : global::CardGame.Core.IZhaoYanProgramHost
{
    private readonly ICharacterZhaoYanOlCharacterYanJunOlOperations _characterYanJunOlOperations;
    private readonly ICharacterZhaoYanOlCardZonesOperations _cardZonesOperations;
    private readonly ICharacterZhaoYanOlContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterZhaoYanOlResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterZhaoYanOlCoreDyingCursorRuntimeOperations _coreDyingCursorRuntimeOperations;
    private readonly ICharacterZhaoYanOlCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICharacterZhaoYanOlCoreDamagePreventionProgramsOperations _coreDamagePreventionProgramsOperations;
    private readonly ICharacterZhaoYanOlCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICharacterZhaoYanOlCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterZhaoYanOlCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICharacterZhaoYanOlCoreTableOrderOperations _coreTableOrderOperations;
    private readonly ICharacterZhaoYanOlCoreMatchLogOperations _coreMatchLogOperations;
    private readonly ICharacterZhaoYanOlCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICharacterZhaoYanOlCoreNativeSlashUseOperations _coreNativeSlashUseOperations;
    private readonly ICharacterZhaoYanOlCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICharacterZhaoYanOlCorePhysicalDrawFacadesOperations _corePhysicalDrawFacadesOperations;
    private readonly ICharacterZhaoYanOlCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICharacterZhaoYanOlCorePlayerViewProjectionOperations _corePlayerViewProjectionOperations;
    private readonly ICharacterZhaoYanOlCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICharacterZhaoYanOlKernelCoreGameEngineAdvancedLifecycleHostOperations _kernelCoreGameEngineAdvancedLifecycleHostOperations;
    private readonly ICharacterZhaoYanOlKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess _coreAiBrainsAiBrainsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterZhaoYanOlRuntime(ICharacterZhaoYanOlCharacterYanJunOlOperations characterYanJunOlOperations, ICharacterZhaoYanOlCardZonesOperations cardZonesOperations, ICharacterZhaoYanOlContentRegistryOperations contentRegistryOperations, ICharacterZhaoYanOlResolutionStackOperations resolutionStackOperations, ICharacterZhaoYanOlCoreDyingCursorRuntimeOperations coreDyingCursorRuntimeOperations, ICharacterZhaoYanOlCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICharacterZhaoYanOlCoreDamagePreventionProgramsOperations coreDamagePreventionProgramsOperations, ICharacterZhaoYanOlCoreNationalSkillsOperations coreNationalSkillsOperations, ICharacterZhaoYanOlCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterZhaoYanOlCoreRuntimeOperations coreRuntimeOperations, ICharacterZhaoYanOlCoreTableOrderOperations coreTableOrderOperations, ICharacterZhaoYanOlCoreMatchLogOperations coreMatchLogOperations, ICharacterZhaoYanOlCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICharacterZhaoYanOlCoreNativeSlashUseOperations coreNativeSlashUseOperations, ICharacterZhaoYanOlCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICharacterZhaoYanOlCorePhysicalDrawFacadesOperations corePhysicalDrawFacadesOperations, ICharacterZhaoYanOlCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICharacterZhaoYanOlCorePlayerViewProjectionOperations corePlayerViewProjectionOperations, ICharacterZhaoYanOlCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICharacterZhaoYanOlKernelCoreGameEngineAdvancedLifecycleHostOperations kernelCoreGameEngineAdvancedLifecycleHostOperations, ICharacterZhaoYanOlKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess coreAiBrainsAiBrainsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterYanJunOlOperations = characterYanJunOlOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreDamagePreventionProgramsOperations = coreDamagePreventionProgramsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreTableOrderOperations = coreTableOrderOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._coreNativeSlashUseOperations = coreNativeSlashUseOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalDrawFacadesOperations = corePhysicalDrawFacadesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._corePlayerViewProjectionOperations = corePlayerViewProjectionOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineAdvancedLifecycleHostOperations = kernelCoreGameEngineAdvancedLifecycleHostOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreAiBrainsAiBrainsAccess = coreAiBrainsAiBrainsAccess;
        this._coreMatchOutcomeStatusWinnerAccess = coreMatchOutcomeStatusWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
