#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineGroupTargetNullificationRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLuMengMouEntries _characterLuMengMouOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhugeGuoLeEntries _characterZhugeGuoLeOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentCardEnhancementsEntries _coreCurrentCardEnhancementsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineIssuedPlayUsePoliciesEntries _coreIssuedPlayUsePoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineOrderedSelectedActorDuelsEntries _coreOrderedSelectedActorDuelsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineUnnullifiableOrdinaryTricksEntries _coreUnnullifiableOrdinaryTricksOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries _coreNativeCardContinuationCursorsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardUseFramesEntries _coreNativeCardUseFramesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeGroupAttackEntries _coreNativeGroupAttackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeGroupCompletionEntries _coreNativeGroupCompletionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeHarvestEntries _coreNativeHarvestOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeNullificationEntries _coreNativeNullificationOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreResolutionIdentityBoundaryEntries _coreResolutionIdentityBoundaryOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineGroupTargetNullificationRuntime(global::CardGame.Core.RuntimeEntries.CharacterLuMengMouEntries characterLuMengMouOperations, global::CardGame.Core.RuntimeEntries.CharacterZhugeGuoLeEntries characterZhugeGuoLeOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentCardEnhancementsEntries coreCurrentCardEnhancementsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineIssuedPlayUsePoliciesEntries coreIssuedPlayUsePoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineOrderedSelectedActorDuelsEntries coreOrderedSelectedActorDuelsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineUnnullifiableOrdinaryTricksEntries coreUnnullifiableOrdinaryTricksOperations, global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries coreNativeCardContinuationCursorsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardUseFramesEntries coreNativeCardUseFramesOperations, global::CardGame.Core.RuntimeEntries.CoreNativeGroupAttackEntries coreNativeGroupAttackOperations, global::CardGame.Core.RuntimeEntries.CoreNativeGroupCompletionEntries coreNativeGroupCompletionOperations, global::CardGame.Core.RuntimeEntries.CoreNativeHarvestEntries coreNativeHarvestOperations, global::CardGame.Core.RuntimeEntries.CoreNativeNullificationEntries coreNativeNullificationOperations, global::CardGame.Core.RuntimeEntries.CoreResolutionIdentityBoundaryEntries coreResolutionIdentityBoundaryOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterLuMengMouOperations = characterLuMengMouOperations;
        this._characterZhugeGuoLeOperations = characterZhugeGuoLeOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreCurrentCardEnhancementsOperations = coreCurrentCardEnhancementsOperations;
        this._coreIssuedPlayUsePoliciesOperations = coreIssuedPlayUsePoliciesOperations;
        this._coreOrderedSelectedActorDuelsOperations = coreOrderedSelectedActorDuelsOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreUnnullifiableOrdinaryTricksOperations = coreUnnullifiableOrdinaryTricksOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativeCardContinuationCursorsOperations = coreNativeCardContinuationCursorsOperations;
        this._coreNativeCardUseFramesOperations = coreNativeCardUseFramesOperations;
        this._coreNativeGroupAttackOperations = coreNativeGroupAttackOperations;
        this._coreNativeGroupCompletionOperations = coreNativeGroupCompletionOperations;
        this._coreNativeHarvestOperations = coreNativeHarvestOperations;
        this._coreNativeNullificationOperations = coreNativeNullificationOperations;
        this._coreResolutionIdentityBoundaryOperations = coreResolutionIdentityBoundaryOperations;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
