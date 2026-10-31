#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNativeGroupRecoveryKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceRulesAndPublishState();
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreNativeGroupRecoveryCoreGroupTargetNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginGroupTargetNullification(global::CardGame.Core.NativeOwners.GroupCardHandle group);
}

internal interface ICoreNativeGroupRecoveryCoreProgramCardTargetEffectsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardEffectIneffective(long resolutionId, int targetSeat);
}

internal interface ICoreNativeGroupRecoveryCharacterDaQiaoXiaoQiaoHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HappyQiaoAnyWounded(int seat);
    bool HasHappyQiaoHealth(int seat);
}

internal interface ICoreNativeGroupRecoveryCoreWushuangTacticRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWushuangTactic(int seat, int id);
}

internal interface ICoreNativeGroupRecoveryCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreNativeGroupRecoveryCoreShanheAlliedTricksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int ShanheAlliedPeachGardenRecovery(global::CardGame.Core.NativeOwners.GroupCardHandle group, int targetSeat, int original);
}

internal interface ICoreNativeGroupRecoveryCoreHpChangeProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginHpChangedProgramWindow(long? resumeFrameId = default, global::CardGame.Core.PostEventContinuation continuation = (global::CardGame.Core.PostEventContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICoreNativeGroupRecoveryCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.GroupCardHandle? ActiveGroupCard { get; set; }
}

internal interface ICoreNativeGroupRecoveryCoreNativeNullificationFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetCardUseTargetIndex(long frameId, int targetIndex);
}

internal interface ICoreNativeGroupRecoveryCoreRecoveryReplacementOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryQueueRecoveryReplacement(long parentFrameId, int sourceSeat, int targetSeat, int amount, global::CardGame.Core.RecoveryAttemptCompletion completion, global::CardGame.Core.HappyQiaoRecoverToOrigin? happyQiaoRecoverToOrigin = default);
}

internal interface ICoreNativeGroupRecoveryCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind);
}

internal interface ICoreNativeGroupRecoveryCoreNativeGroupCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishGroupRecovery(global::CardGame.Core.NativeOwners.GroupCardHandle group);
}

internal interface ICoreNativeGroupRecoveryCoreNativeRecoveryFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long BeginRecovery(long parentFrameId, int sourceSeat, int targetSeat, int amount);
}

internal interface ICoreNativeGroupRecoveryCharacterLuXunMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginMouLuXunGroupEffect(global::CardGame.Core.NativeOwners.GroupCardHandle group);
}
