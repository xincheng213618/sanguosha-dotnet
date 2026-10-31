#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNativeAttackCompletionCoreRoundOnceCardlessReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RoundOnceCardlessRequestedOwnerWasDamaged(global::CardGame.Core.ProgramSkillFrame parent, global::CardGame.Core.GameEngine.AttackCompletionReceipt completion);
}

internal interface ICoreNativeAttackCompletionCoreOriginalUseLightningOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PreserveNativeDelayedAttackCompletion(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion);
}

internal interface ICoreNativeAttackCompletionCoreNativeGroupAttackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginGroupAttackResponse(global::CardGame.Core.NativeOwners.GroupCardHandle group);
}

internal interface ICoreNativeAttackCompletionCoreProgramNativeReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramNativeReturns NativeReturns { get; }
}

internal interface ICoreNativeAttackCompletionCharacterXunYuMobileGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryCompleteQizhengAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNativeAttackCompletionCoreNativeSlashUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginNextFangtianHalberdTarget(global::CardGame.Core.NativeOwners.FangtianHalberdHandle pending);
}

internal interface ICoreNativeAttackCompletionCoreNativeBorrowedSwordOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteBorrowedSwordAfterSlash(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending);
}

internal interface ICoreNativeAttackCompletionCoreProgramVirtualDuelOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int? DamageProgramUseInstructionIndex(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreNativeAttackCompletionCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreNativeAttackCompletionCoreCardAttackStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameEngine.AttackCompletionReceipt CaptureAttackCompletion(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNativeAttackCompletionCoreSequentialTrickTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryContinueSequentialTrick(long frameId, bool afterAttack = false);
}

internal interface ICoreNativeAttackCompletionCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICoreNativeAttackCompletionCoreCurrentCardEnhancementsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryContinueEnhancedSlashTargets(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNativeAttackCompletionCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind);
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
    void AdvanceRuntimeProgram(long frameId);
}

internal interface ICoreNativeAttackCompletionCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
    void UpdateLifecycleCardUse(long frameId, global::System.Func<global::CardGame.Core.CardUseFrame, global::CardGame.Core.CardUseFrame> update);
}

internal interface ICoreNativeAttackCompletionCoreNativeCardClassificationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DamageNature GetDamageNature(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreNativeAttackCompletionResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreNativeAttackCompletionCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreNativeAttackCompletionCharacterSimaYiMobileGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ClearMobileSimaRenjieResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNativeAttackCompletionCorePaidVirtualSlashTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryContinuePaidVirtualSlashTargets(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNativeAttackCompletionCoreRoundOnceCardlessProgramRequestsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ResumeRoundOnceCardlessProgramRequest(long id);
}

internal interface ICoreNativeAttackCompletionCoreProgramAttackAttemptOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryAdvanceChainedDamageTarget(global::CardGame.Core.GameEngine.IDamageAttempt attack, out int fromSeat);
}

internal interface ICoreNativeAttackCompletionCoreDragonBoatCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryCompleteDragonBoatAgainstCurrentAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNativeAttackCompletionCoreNativeDamageApplicationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ApplyAttackDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreNativeAttackCompletionCoreCardActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetFangtianEffectiveTarget(global::CardGame.Core.NativeOwners.FangtianHalberdHandle pending, int index);
}

internal interface ICoreNativeAttackCompletionCharacterGuanYuWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryCompleteWumiaoFloodAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNativeAttackCompletionCoreNativeSlashCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool FinishAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack, bool allowYingboGift = true);
}

internal interface ICoreNativeAttackCompletionCorePhysicalDrawFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> DrawCards(global::CardGame.Core.CharacterState player, int count, bool log, global::CardGame.Core.CardMoveReason? reason = default, global::CardGame.Core.CardConversionSource? directProducer = default);
}

internal interface ICoreNativeAttackCompletionCharacterDongChengMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ContinueMouDcSlash(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNativeAttackCompletionCoreOriginalTargetAdditionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int CompletedFangtianOriginalTargetIndex(global::CardGame.Core.NativeOwners.FangtianHalberdHandle multi);
}

internal interface ICoreNativeAttackCompletionKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreNativeAttackCompletionCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.GroupCardHandle? ActiveGroupCard { get; set; }

    global::CardGame.Core.NativeOwners.DuelHandle? ActiveDuel { get; set; }

    global::CardGame.Core.NativeOwners.FangtianHalberdHandle? ActiveFangtianHalberd { get; set; }

    global::CardGame.Core.NativeOwners.BorrowedSwordHandle? ActiveBorrowedSword { get; set; }
}

internal interface ICoreNativeAttackCompletionCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreNativeAttackCompletionCoreNativeNullificationFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetCardUseTargetIndex(long frameId, int targetIndex);
}

internal interface ICoreNativeAttackCompletionCoreNativeJudgmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResumeAfterLightningDamage(int delayedSeat, int? excludedCardId = default);
}

internal interface ICoreNativeAttackCompletionCharacterLuXunWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RetireWuXiongmuCardDamageCompleted(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}
