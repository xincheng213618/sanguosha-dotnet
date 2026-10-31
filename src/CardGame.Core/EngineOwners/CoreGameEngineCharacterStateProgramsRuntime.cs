#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineCharacterStateProgramsRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCaoPiAnniversaryGodEntries _characterCaoPiAnniversaryGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterGuanYuWumiaoEntries _characterGuanYuWumiaoOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhaoZhongOlEntries _characterZhaoZhongOlOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreActualTurnsEntries _coreActualTurnsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineChainedStateBasicConversionsEntries _coreChainedStateBasicConversionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineConvertingGiftsEntries _coreConvertingGiftsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageColorDuelProgramsEntries _coreDamageColorDuelProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDeferredHandAlignmentsEntries _coreDeferredHandAlignmentsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDragonBoatCardsEntries _coreDragonBoatCardsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineGuanduCardsEntries _coreGuanduCardsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSoleIncomingRangeDrawEntries _coreSoleIncomingRangeDrawOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTableOrderEntries _coreTableOrderOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineVirtualBasicOffersEntries _coreVirtualBasicOffersOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineWenheCardsEntries _coreWenheCardsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardEffectDrawsEntries _coreNativeCardEffectDrawsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardUseCompletionEntries _coreNativeCardUseCompletionOperations;
    private readonly ICoreCharacterStateProgramsCoreNativeRuleQueriesOperations _coreNativeRuleQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreResolutionIdentityBoundaryEntries _coreResolutionIdentityBoundaryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupSetupCompleteAccess _coreMatchSetupSetupCompleteAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCharacterStateProgramsPendingCharacterStateChangesAccess _coreGameEngineCharacterStateProgramsPendingCharacterStateChangesAccess;
    internal CoreGameEngineCharacterStateProgramsRuntime(global::CardGame.Core.RuntimeEntries.CharacterCaoPiAnniversaryGodEntries characterCaoPiAnniversaryGodOperations, global::CardGame.Core.RuntimeEntries.CharacterGuanYuWumiaoEntries characterGuanYuWumiaoOperations, global::CardGame.Core.RuntimeEntries.CharacterZhaoZhongOlEntries characterZhaoZhongOlOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreActualTurnsEntries coreActualTurnsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineChainedStateBasicConversionsEntries coreChainedStateBasicConversionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineConvertingGiftsEntries coreConvertingGiftsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageColorDuelProgramsEntries coreDamageColorDuelProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDeferredHandAlignmentsEntries coreDeferredHandAlignmentsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDragonBoatCardsEntries coreDragonBoatCardsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineGuanduCardsEntries coreGuanduCardsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineSoleIncomingRangeDrawEntries coreSoleIncomingRangeDrawOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineTableOrderEntries coreTableOrderOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineVirtualBasicOffersEntries coreVirtualBasicOffersOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineWenheCardsEntries coreWenheCardsOperations, global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardEffectDrawsEntries coreNativeCardEffectDrawsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardUseCompletionEntries coreNativeCardUseCompletionOperations, ICoreCharacterStateProgramsCoreNativeRuleQueriesOperations coreNativeRuleQueriesOperations, global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations, global::CardGame.Core.RuntimeEntries.CoreResolutionIdentityBoundaryEntries coreResolutionIdentityBoundaryOperations, global::CardGame.Core.EngineOwners.IKernelRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupSetupCompleteAccess coreMatchSetupSetupCompleteAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCharacterStateProgramsPendingCharacterStateChangesAccess coreGameEngineCharacterStateProgramsPendingCharacterStateChangesAccess)
    {
        this._characterCaoPiAnniversaryGodOperations = characterCaoPiAnniversaryGodOperations;
        this._characterGuanYuWumiaoOperations = characterGuanYuWumiaoOperations;
        this._characterZhaoZhongOlOperations = characterZhaoZhongOlOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreActualTurnsOperations = coreActualTurnsOperations;
        this._coreChainedStateBasicConversionsOperations = coreChainedStateBasicConversionsOperations;
        this._coreConvertingGiftsOperations = coreConvertingGiftsOperations;
        this._coreDamageColorDuelProgramsOperations = coreDamageColorDuelProgramsOperations;
        this._coreDeferredHandAlignmentsOperations = coreDeferredHandAlignmentsOperations;
        this._coreDragonBoatCardsOperations = coreDragonBoatCardsOperations;
        this._coreGuanduCardsOperations = coreGuanduCardsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSoleIncomingRangeDrawOperations = coreSoleIncomingRangeDrawOperations;
        this._coreTableOrderOperations = coreTableOrderOperations;
        this._coreVirtualBasicOffersOperations = coreVirtualBasicOffersOperations;
        this._coreWenheCardsOperations = coreWenheCardsOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativeCardEffectDrawsOperations = coreNativeCardEffectDrawsOperations;
        this._coreNativeCardUseCompletionOperations = coreNativeCardUseCompletionOperations;
        this._coreNativeRuleQueriesOperations = coreNativeRuleQueriesOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._coreResolutionIdentityBoundaryOperations = coreResolutionIdentityBoundaryOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess = coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
        this._coreMatchOutcomeStatusWinnerAccess = coreMatchOutcomeStatusWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreMatchSetupSetupCompleteAccess = coreMatchSetupSetupCompleteAccess;
        this._coreGameEngineCharacterStateProgramsPendingCharacterStateChangesAccess = coreGameEngineCharacterStateProgramsPendingCharacterStateChangesAccess;
    }
}
