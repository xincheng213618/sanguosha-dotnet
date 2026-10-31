#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineHulaoCardsRuntime
{
    private readonly ICoreHulaoCardsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreHulaoCardsCoreDirectedTurnCardPoliciesOperations _coreDirectedTurnCardPoliciesOperations;
    private readonly ICoreHulaoCardsCoreCardRecoveryModifiersOperations _coreCardRecoveryModifiersOperations;
    private readonly ICoreHulaoCardsCoreCardUseLifecycleOperations _coreCardUseLifecycleOperations;
    private readonly ICoreHulaoCardsCoreCardUseModulesOperations _coreCardUseModulesOperations;
    private readonly ICoreHulaoCardsCoreDamageColorDuelProgramsOperations _coreDamageColorDuelProgramsOperations;
    private readonly ICoreHulaoCardsCoreDeckEquipmentProgramsOperations _coreDeckEquipmentProgramsOperations;
    private readonly ICoreHulaoCardsCoreProgramCardPoliciesOperations _coreProgramCardPoliciesOperations;
    private readonly ICoreHulaoCardsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreHulaoCardsCoreShenwuEquipmentOperations _coreShenwuEquipmentOperations;
    private readonly ICoreHulaoCardsCoreTableOrderOperations _coreTableOrderOperations;
    private readonly ICoreHulaoCardsCoreMatchLogOperations _coreMatchLogOperations;
    private readonly ICoreHulaoCardsCoreNativeCardAppearanceOperations _coreNativeCardAppearanceOperations;
    private readonly ICoreHulaoCardsCoreNativeCardUseOperations _coreNativeCardUseOperations;
    private readonly ICoreHulaoCardsCoreNativeCardUseCompletionOperations _coreNativeCardUseCompletionOperations;
    private readonly ICoreHulaoCardsCoreNativeCardUseFramesOperations _coreNativeCardUseFramesOperations;
    private readonly ICoreHulaoCardsCoreNativeDamagePoliciesOperations _coreNativeDamagePoliciesOperations;
    private readonly ICoreHulaoCardsCoreNativeLegalActionsOperations _coreNativeLegalActionsOperations;
    private readonly ICoreHulaoCardsCoreNativeNullificationOperations _coreNativeNullificationOperations;
    private readonly ICoreHulaoCardsCoreNativeOwnedCardQueriesOperations _coreNativeOwnedCardQueriesOperations;
    private readonly ICoreHulaoCardsCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICoreHulaoCardsCoreNativeRecoveryFramesOperations _coreNativeRecoveryFramesOperations;
    private readonly ICoreHulaoCardsCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICoreHulaoCardsCorePhysicalDrawFacadesOperations _corePhysicalDrawFacadesOperations;
    private readonly ICoreHulaoCardsCorePhysicalMovementFacadesOperations _corePhysicalMovementFacadesOperations;
    private readonly ICoreHulaoCardsCoreRecoveryReplacementOperations _coreRecoveryReplacementOperations;
    private readonly ICoreHulaoCardsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionPlayerCountAccess _coreMatchDefinitionPlayerCountAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineHulaoCardsRuntime(ICoreHulaoCardsResolutionStackOperations resolutionStackOperations, ICoreHulaoCardsCoreDirectedTurnCardPoliciesOperations coreDirectedTurnCardPoliciesOperations, ICoreHulaoCardsCoreCardRecoveryModifiersOperations coreCardRecoveryModifiersOperations, ICoreHulaoCardsCoreCardUseLifecycleOperations coreCardUseLifecycleOperations, ICoreHulaoCardsCoreCardUseModulesOperations coreCardUseModulesOperations, ICoreHulaoCardsCoreDamageColorDuelProgramsOperations coreDamageColorDuelProgramsOperations, ICoreHulaoCardsCoreDeckEquipmentProgramsOperations coreDeckEquipmentProgramsOperations, ICoreHulaoCardsCoreProgramCardPoliciesOperations coreProgramCardPoliciesOperations, ICoreHulaoCardsCoreRuntimeOperations coreRuntimeOperations, ICoreHulaoCardsCoreShenwuEquipmentOperations coreShenwuEquipmentOperations, ICoreHulaoCardsCoreTableOrderOperations coreTableOrderOperations, ICoreHulaoCardsCoreMatchLogOperations coreMatchLogOperations, ICoreHulaoCardsCoreNativeCardAppearanceOperations coreNativeCardAppearanceOperations, ICoreHulaoCardsCoreNativeCardUseOperations coreNativeCardUseOperations, ICoreHulaoCardsCoreNativeCardUseCompletionOperations coreNativeCardUseCompletionOperations, ICoreHulaoCardsCoreNativeCardUseFramesOperations coreNativeCardUseFramesOperations, ICoreHulaoCardsCoreNativeDamagePoliciesOperations coreNativeDamagePoliciesOperations, ICoreHulaoCardsCoreNativeLegalActionsOperations coreNativeLegalActionsOperations, ICoreHulaoCardsCoreNativeNullificationOperations coreNativeNullificationOperations, ICoreHulaoCardsCoreNativeOwnedCardQueriesOperations coreNativeOwnedCardQueriesOperations, ICoreHulaoCardsCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICoreHulaoCardsCoreNativeRecoveryFramesOperations coreNativeRecoveryFramesOperations, ICoreHulaoCardsCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICoreHulaoCardsCorePhysicalDrawFacadesOperations corePhysicalDrawFacadesOperations, ICoreHulaoCardsCorePhysicalMovementFacadesOperations corePhysicalMovementFacadesOperations, ICoreHulaoCardsCoreRecoveryReplacementOperations coreRecoveryReplacementOperations, ICoreHulaoCardsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionPlayerCountAccess coreMatchDefinitionPlayerCountAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDirectedTurnCardPoliciesOperations = coreDirectedTurnCardPoliciesOperations;
        this._coreCardRecoveryModifiersOperations = coreCardRecoveryModifiersOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreCardUseModulesOperations = coreCardUseModulesOperations;
        this._coreDamageColorDuelProgramsOperations = coreDamageColorDuelProgramsOperations;
        this._coreDeckEquipmentProgramsOperations = coreDeckEquipmentProgramsOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreShenwuEquipmentOperations = coreShenwuEquipmentOperations;
        this._coreTableOrderOperations = coreTableOrderOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativeCardAppearanceOperations = coreNativeCardAppearanceOperations;
        this._coreNativeCardUseOperations = coreNativeCardUseOperations;
        this._coreNativeCardUseCompletionOperations = coreNativeCardUseCompletionOperations;
        this._coreNativeCardUseFramesOperations = coreNativeCardUseFramesOperations;
        this._coreNativeDamagePoliciesOperations = coreNativeDamagePoliciesOperations;
        this._coreNativeLegalActionsOperations = coreNativeLegalActionsOperations;
        this._coreNativeNullificationOperations = coreNativeNullificationOperations;
        this._coreNativeOwnedCardQueriesOperations = coreNativeOwnedCardQueriesOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._coreNativeRecoveryFramesOperations = coreNativeRecoveryFramesOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalDrawFacadesOperations = corePhysicalDrawFacadesOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._coreRecoveryReplacementOperations = coreRecoveryReplacementOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchDefinitionPlayerCountAccess = coreMatchDefinitionPlayerCountAccess;
        this._coreMatchOutcomeStatusAccess = coreMatchOutcomeStatusAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
