#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineExtraEffectForeignSelectionRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhugeGuoLeEntries _characterZhugeGuoLeOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreForeignEquipmentDiscardPoliciesEntries _coreForeignEquipmentDiscardPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries _coreCardAttackStateOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageColorDuelProgramsEntries _coreDamageColorDuelProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineExtraCardEffectRoundsEntries _coreExtraCardEffectRoundsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineMantianGuohaiEntries _coreMantianGuohaiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangCombatTacticsEntries _coreWushuangCombatTacticsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangTargetCardsEntries _coreWushuangTargetCardsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardUseFramesEntries _coreNativeCardUseFramesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativePromptBoundaryEntries _coreNativePromptBoundaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeTrickEffectsEntries _coreNativeTrickEffectsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreResolutionIdentityBoundaryEntries _coreResolutionIdentityBoundaryOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineExtraEffectForeignSelectionRuntime(global::CardGame.Core.RuntimeEntries.CharacterZhugeGuoLeEntries characterZhugeGuoLeOperations,global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations,global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations,global::CardGame.Core.RuntimeEntries.CoreForeignEquipmentDiscardPoliciesEntries coreForeignEquipmentDiscardPoliciesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries coreCardAttackStateOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageColorDuelProgramsEntries coreDamageColorDuelProgramsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineExtraCardEffectRoundsEntries coreExtraCardEffectRoundsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineMantianGuohaiEntries coreMantianGuohaiOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangCombatTacticsEntries coreWushuangCombatTacticsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangTargetCardsEntries coreWushuangTargetCardsOperations,global::CardGame.Core.RuntimeEntries.CoreNativeCardUseFramesEntries coreNativeCardUseFramesOperations,global::CardGame.Core.RuntimeEntries.CoreNativePromptBoundaryEntries coreNativePromptBoundaryOperations,global::CardGame.Core.RuntimeEntries.CoreNativeTrickEffectsEntries coreNativeTrickEffectsOperations,global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations,global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations,global::CardGame.Core.RuntimeEntries.CoreResolutionIdentityBoundaryEntries coreResolutionIdentityBoundaryOperations,global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess,global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterZhugeGuoLeOperations = characterZhugeGuoLeOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreForeignEquipmentDiscardPoliciesOperations = coreForeignEquipmentDiscardPoliciesOperations;
        this._coreCardAttackStateOperations = coreCardAttackStateOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreDamageColorDuelProgramsOperations = coreDamageColorDuelProgramsOperations;
        this._coreExtraCardEffectRoundsOperations = coreExtraCardEffectRoundsOperations;
        this._coreMantianGuohaiOperations = coreMantianGuohaiOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreWushuangCombatTacticsOperations = coreWushuangCombatTacticsOperations;
        this._coreWushuangTargetCardsOperations = coreWushuangTargetCardsOperations;
        this._coreNativeCardUseFramesOperations = coreNativeCardUseFramesOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._coreNativeTrickEffectsOperations = coreNativeTrickEffectsOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreResolutionIdentityBoundaryOperations = coreResolutionIdentityBoundaryOperations;
        this._coreMatchOutcomeStatusAccess = coreMatchOutcomeStatusAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
