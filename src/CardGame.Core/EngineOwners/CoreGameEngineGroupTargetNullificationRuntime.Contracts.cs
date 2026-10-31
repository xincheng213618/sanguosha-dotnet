#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreGroupTargetNullificationCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame);
}

internal interface ICoreGroupTargetNullificationCharacterZhugeGuoLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsLeChengyanEffectUse(long id);
}

internal interface ICoreGroupTargetNullificationCoreCurrentCardEnhancementsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCurrentCardEnhancement(long frameId, global::CardGame.Core.CurrentCardEnhancement flag);
}

internal interface ICoreGroupTargetNullificationCoreNativeHarvestOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginHarvestSelection(global::CardGame.Core.NativeOwners.GroupCardHandle group);
}

internal interface ICoreGroupTargetNullificationCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreGroupTargetNullificationCoreNativeGroupCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishGroupRecovery(global::CardGame.Core.NativeOwners.GroupCardHandle group);
}

internal interface ICoreGroupTargetNullificationCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.GroupCardHandle? ActiveGroupCard { get; set; }

    global::CardGame.Core.NullificationWindowFrame? ActiveNullificationWindow { get; }
}

internal interface ICoreGroupTargetNullificationCoreNativeGroupAttackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginGroupAttackResponse(global::CardGame.Core.NativeOwners.GroupCardHandle group);
}

internal interface ICoreGroupTargetNullificationCoreResolutionIdentityBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long _resolutionSequence { get; set; }
}

internal interface ICoreGroupTargetNullificationCoreNativeNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> BuildNullificationCandidateSeats(int startAfterSeat);
    void ContinueNullificationWindow(global::CardGame.Core.NullificationWindowFrame pending);
}

internal interface ICoreGroupTargetNullificationCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICoreGroupTargetNullificationCoreIssuedPlayUsePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsIssuedCardUnrespondable(long frameId);
}

internal interface ICoreGroupTargetNullificationCoreUnnullifiableOrdinaryTricksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsOriginalOrdinaryTrickCannotNullify(long useId);
}

internal interface ICoreGroupTargetNullificationCoreNativeCardUseFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetCardUseStep(long frameId, global::CardGame.Core.ResolutionFrameStep step);
}

internal interface ICoreGroupTargetNullificationCharacterLuMengMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsYingboUnrespondable(long resolutionId);
}

internal interface ICoreGroupTargetNullificationCoreOrderedSelectedActorDuelsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsOrderedSelectedActorDuelCannotNullify(long useId);
}

internal interface ICoreGroupTargetNullificationResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}
