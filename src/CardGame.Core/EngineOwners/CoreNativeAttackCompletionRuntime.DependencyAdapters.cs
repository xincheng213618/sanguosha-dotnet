#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessReturnsRuntime))]
internal sealed class CoreNativeAttackCompletionCoreRoundOnceCardlessReturnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessReturnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAttackCompletionCoreRoundOnceCardlessReturnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessReturnsRuntime> _targetFactory = targetFactory;
    public bool RoundOnceCardlessRequestedOwnerWasDamaged(global::CardGame.Core.ProgramSkillFrame parent, global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => this._targetFactory().RoundOnceCardlessRequestedOwnerWasDamaged(parent, completion);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOriginalUseLightningRuntime))]
internal sealed class CoreNativeAttackCompletionCoreOriginalUseLightningAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalUseLightningRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAttackCompletionCoreOriginalUseLightningOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalUseLightningRuntime> _targetFactory = targetFactory;
    public void PreserveNativeDelayedAttackCompletion(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => this._targetFactory().PreserveNativeDelayedAttackCompletion(completion);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeGroupAttackRuntime))]
internal sealed class CoreNativeAttackCompletionCoreNativeGroupAttackAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeGroupAttackRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAttackCompletionCoreNativeGroupAttackOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeGroupAttackRuntime> _targetFactory = targetFactory;
    public void BeginGroupAttackResponse(global::CardGame.Core.NativeOwners.GroupCardHandle group) => this._targetFactory().BeginGroupAttackResponse(group);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramNativeReturnsRuntime))]
internal sealed class CoreNativeAttackCompletionCoreProgramNativeReturnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramNativeReturnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAttackCompletionCoreProgramNativeReturnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramNativeReturnsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramNativeReturns NativeReturns { get => this._targetFactory().NativeReturns; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXunYuMobileGodRuntime))]
internal sealed class CoreNativeAttackCompletionCharacterXunYuMobileGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXunYuMobileGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAttackCompletionCharacterXunYuMobileGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXunYuMobileGodRuntime> _targetFactory = targetFactory;
    public bool TryCompleteQizhengAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().TryCompleteQizhengAttack(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeSlashUseRuntime))]
internal sealed class CoreNativeAttackCompletionCoreNativeSlashUseAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSlashUseRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAttackCompletionCoreNativeSlashUseOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSlashUseRuntime> _targetFactory = targetFactory;
    public void BeginNextFangtianHalberdTarget(global::CardGame.Core.NativeOwners.FangtianHalberdHandle pending) => this._targetFactory().BeginNextFangtianHalberdTarget(pending);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime))]
internal sealed class CoreNativeAttackCompletionCoreNativeBorrowedSwordAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAttackCompletionCoreNativeBorrowedSwordOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime> _targetFactory = targetFactory;
    public void CompleteBorrowedSwordAfterSlash(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending) => this._targetFactory().CompleteBorrowedSwordAfterSlash(pending);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramVirtualDuelRuntime))]
internal sealed class CoreNativeAttackCompletionCoreProgramVirtualDuelAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramVirtualDuelRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAttackCompletionCoreProgramVirtualDuelOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramVirtualDuelRuntime> _targetFactory = targetFactory;
    public int? DamageProgramUseInstructionIndex(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().DamageProgramUseInstructionIndex(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchLogRuntime))]
internal sealed class CoreNativeAttackCompletionCoreMatchLogAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAttackCompletionCoreMatchLogOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> _targetFactory = targetFactory;
    public void AddLog(string type, string message, int? actor = default, int? target = default) => this._targetFactory().AddLog(type, message, actor, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime))]
internal sealed class CoreNativeAttackCompletionCoreCardAttackStateAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAttackCompletionCoreCardAttackStateOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.GameEngine.AttackCompletionReceipt CaptureAttackCompletion(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().CaptureAttackCompletion(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSequentialTrickTargetsRuntime))]
internal sealed class CoreNativeAttackCompletionCoreSequentialTrickTargetsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSequentialTrickTargetsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAttackCompletionCoreSequentialTrickTargetsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSequentialTrickTargetsRuntime> _targetFactory = targetFactory;
    public bool TryContinueSequentialTrick(long frameId, bool afterAttack = false) => this._targetFactory().TryContinueSequentialTrick(frameId, afterAttack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime))]
internal sealed class CoreNativeAttackCompletionCoreActiveNativeAttackQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAttackCompletionCoreActiveNativeAttackQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get => this._targetFactory().ActiveCardAttack; set => this._targetFactory().ActiveCardAttack = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentCardEnhancementsRuntime))]
internal sealed class CoreNativeAttackCompletionCoreCurrentCardEnhancementsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentCardEnhancementsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAttackCompletionCoreCurrentCardEnhancementsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentCardEnhancementsRuntime> _targetFactory = targetFactory;
    public bool TryContinueEnhancedSlashTargets(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().TryContinueEnhancedSlashTargets(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreNativeAttackCompletionCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAttackCompletionCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind) => this._targetFactory().PopResolutionFrame(frameId, expectedKind);
    public void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeFrame(expectedFrameId, next);
    public void AdvanceRuntimeProgram(long frameId) => this._targetFactory().AdvanceRuntimeProgram(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime))]
internal sealed class CoreNativeAttackCompletionCoreCardUseLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAttackCompletionCoreCardUseLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId) => this._targetFactory().LifecycleCardUse(frameId);
    public void UpdateLifecycleCardUse(long frameId, global::System.Func<global::CardGame.Core.CardUseFrame, global::CardGame.Core.CardUseFrame> update) => this._targetFactory().UpdateLifecycleCardUse(frameId, update);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime))]
internal sealed class CoreNativeAttackCompletionCoreNativeCardClassificationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAttackCompletionCoreNativeCardClassificationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DamageNature GetDamageNature(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().GetDamageNature(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime))]
internal sealed class CoreNativeAttackCompletionCoreDyingCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAttackCompletionCoreDyingCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DyingFrame? ActiveDying { get => this._targetFactory().ActiveDying; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSimaYiMobileGodRuntime))]
internal sealed class CoreNativeAttackCompletionCharacterSimaYiMobileGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaYiMobileGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAttackCompletionCharacterSimaYiMobileGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaYiMobileGodRuntime> _targetFactory = targetFactory;
    public void ClearMobileSimaRenjieResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().ClearMobileSimaRenjieResponse(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePaidVirtualSlashTargetsRuntime))]
internal sealed class CoreNativeAttackCompletionCorePaidVirtualSlashTargetsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePaidVirtualSlashTargetsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAttackCompletionCorePaidVirtualSlashTargetsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePaidVirtualSlashTargetsRuntime> _targetFactory = targetFactory;
    public bool TryContinuePaidVirtualSlashTargets(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().TryContinuePaidVirtualSlashTargets(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessProgramRequestsRuntime))]
internal sealed class CoreNativeAttackCompletionCoreRoundOnceCardlessProgramRequestsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessProgramRequestsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAttackCompletionCoreRoundOnceCardlessProgramRequestsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessProgramRequestsRuntime> _targetFactory = targetFactory;
    public bool ResumeRoundOnceCardlessProgramRequest(long id) => this._targetFactory().ResumeRoundOnceCardlessProgramRequest(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramAttackAttemptRuntime))]
internal sealed class CoreNativeAttackCompletionCoreProgramAttackAttemptAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAttackAttemptRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAttackCompletionCoreProgramAttackAttemptOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAttackAttemptRuntime> _targetFactory = targetFactory;
    public bool TryAdvanceChainedDamageTarget(global::CardGame.Core.GameEngine.IDamageAttempt attack, out int fromSeat) => this._targetFactory().TryAdvanceChainedDamageTarget(attack, out fromSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatCardsRuntime))]
internal sealed class CoreNativeAttackCompletionCoreDragonBoatCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAttackCompletionCoreDragonBoatCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatCardsRuntime> _targetFactory = targetFactory;
    public bool TryCompleteDragonBoatAgainstCurrentAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().TryCompleteDragonBoatAgainstCurrentAttack(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageApplicationRuntime))]
internal sealed class CoreNativeAttackCompletionCoreNativeDamageApplicationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageApplicationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAttackCompletionCoreNativeDamageApplicationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageApplicationRuntime> _targetFactory = targetFactory;
    public bool ApplyAttackDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().ApplyAttackDamage(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime))]
internal sealed class CoreNativeAttackCompletionCoreCardActionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAttackCompletionCoreCardActionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> _targetFactory = targetFactory;
    public int GetFangtianEffectiveTarget(global::CardGame.Core.NativeOwners.FangtianHalberdHandle pending, int index) => this._targetFactory().GetFangtianEffectiveTarget(pending, index);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime))]
internal sealed class CoreNativeAttackCompletionCharacterGuanYuWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAttackCompletionCharacterGuanYuWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime> _targetFactory = targetFactory;
    public bool TryCompleteWumiaoFloodAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().TryCompleteWumiaoFloodAttack(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeSlashCompletionRuntime))]
internal sealed class CoreNativeAttackCompletionCoreNativeSlashCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSlashCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAttackCompletionCoreNativeSlashCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSlashCompletionRuntime> _targetFactory = targetFactory;
    public bool FinishAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack, bool allowYingboGift = true) => this._targetFactory().FinishAttack(attack, allowYingboGift);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime))]
internal sealed class CoreNativeAttackCompletionCorePhysicalDrawFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAttackCompletionCorePhysicalDrawFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int> DrawCards(global::CardGame.Core.CharacterState player, int count, bool log, global::CardGame.Core.CardMoveReason? reason = default, global::CardGame.Core.CardConversionSource? directProducer = default) => this._targetFactory().DrawCards(player, count, log, reason, directProducer);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDongChengMouRuntime))]
internal sealed class CoreNativeAttackCompletionCharacterDongChengMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongChengMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAttackCompletionCharacterDongChengMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongChengMouRuntime> _targetFactory = targetFactory;
    public bool ContinueMouDcSlash(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().ContinueMouDcSlash(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOriginalTargetAdditionsRuntime))]
internal sealed class CoreNativeAttackCompletionCoreOriginalTargetAdditionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalTargetAdditionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAttackCompletionCoreOriginalTargetAdditionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalTargetAdditionsRuntime> _targetFactory = targetFactory;
    public int CompletedFangtianOriginalTargetIndex(global::CardGame.Core.NativeOwners.FangtianHalberdHandle multi) => this._targetFactory().CompletedFangtianOriginalTargetIndex(multi);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime))]
internal sealed class CoreNativeAttackCompletionCoreNativeCardContinuationCursorsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAttackCompletionCoreNativeCardContinuationCursorsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.GroupCardHandle? ActiveGroupCard { get => this._targetFactory().ActiveGroupCard; set => this._targetFactory().ActiveGroupCard = value; }
    public global::CardGame.Core.NativeOwners.DuelHandle? ActiveDuel { get => this._targetFactory().ActiveDuel; set => this._targetFactory().ActiveDuel = value; }
    public global::CardGame.Core.NativeOwners.FangtianHalberdHandle? ActiveFangtianHalberd { get => this._targetFactory().ActiveFangtianHalberd; set => this._targetFactory().ActiveFangtianHalberd = value; }
    public global::CardGame.Core.NativeOwners.BorrowedSwordHandle? ActiveBorrowedSword { get => this._targetFactory().ActiveBorrowedSword; set => this._targetFactory().ActiveBorrowedSword = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreNativeAttackCompletionCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAttackCompletionCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeNullificationFramesRuntime))]
internal sealed class CoreNativeAttackCompletionCoreNativeNullificationFramesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationFramesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAttackCompletionCoreNativeNullificationFramesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationFramesRuntime> _targetFactory = targetFactory;
    public void SetCardUseTargetIndex(long frameId, int targetIndex) => this._targetFactory().SetCardUseTargetIndex(frameId, targetIndex);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeJudgmentRuntime))]
internal sealed class CoreNativeAttackCompletionCoreNativeJudgmentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeJudgmentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAttackCompletionCoreNativeJudgmentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeJudgmentRuntime> _targetFactory = targetFactory;
    public void ResumeAfterLightningDamage(int delayedSeat, int? excludedCardId = default) => this._targetFactory().ResumeAfterLightningDamage(delayedSeat, excludedCardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuXunWumiaoRuntime))]
internal sealed class CoreNativeAttackCompletionCharacterLuXunWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuXunWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAttackCompletionCharacterLuXunWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuXunWumiaoRuntime> _targetFactory = targetFactory;
    public void RetireWuXiongmuCardDamageCompleted(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().RetireWuXiongmuCardDamageCompleted(attack);
}
