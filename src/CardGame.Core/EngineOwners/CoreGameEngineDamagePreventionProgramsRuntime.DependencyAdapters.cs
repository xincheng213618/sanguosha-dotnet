#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramAttackAttemptRuntime))]
internal sealed class CoreDamagePreventionProgramsCoreProgramAttackAttemptAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAttackAttemptRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamagePreventionProgramsCoreProgramAttackAttemptOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAttackAttemptRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.GameEngine.IDamageAttempt? CurrentDamageAttempt { get => this._targetFactory().CurrentDamageAttempt; }

    public global::CardGame.Core.AttackAttemptState GetProgramAttackState(long ownerFrameId) => this._targetFactory().GetProgramAttackState(ownerFrameId);
    public void UpdateProgramAttackState(long ownerFrameId, global::System.Func<global::CardGame.Core.AttackAttemptState, global::CardGame.Core.AttackAttemptState> update) => this._targetFactory().UpdateProgramAttackState(ownerFrameId, update);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramRuntimeHooksRuntime))]
internal sealed class CoreDamagePreventionProgramsCoreProgramRuntimeHooksAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramRuntimeHooksRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamagePreventionProgramsCoreProgramRuntimeHooksOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramRuntimeHooksRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramRuntimeHooks RuntimeHooks { get => this._targetFactory().RuntimeHooks; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramJudgmentOutcomesRuntime))]
internal sealed class CoreDamagePreventionProgramsCoreProgramJudgmentOutcomesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramJudgmentOutcomesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamagePreventionProgramsCoreProgramJudgmentOutcomesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramJudgmentOutcomesRuntime> _targetFactory = targetFactory;
    public bool HasRecipientJudgmentOutcomePrevention(int targetSeat) => this._targetFactory().HasRecipientJudgmentOutcomePrevention(targetSeat);
    public bool IsJudgmentOutcomePrevention(global::CardGame.Core.SkillProgramTrigger trigger) => this._targetFactory().IsJudgmentOutcomePrevention(trigger);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLvQianOlRuntime))]
internal sealed class CoreDamagePreventionProgramsCharacterLvQianOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLvQianOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamagePreventionProgramsCharacterLvQianOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLvQianOlRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.BeforeDamageProgramCandidate> CollectZengdaoDamageCandidates(int source, int target, bool rangeChainOnly) => this._targetFactory().CollectZengdaoDamageCandidates(source, target, rangeChainOnly);
    public global::CardGame.Core.ProgramSkillWindowContext CreateLvQianBeforeDamageContext(global::CardGame.Core.BeforeDamageProgramCandidate item, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().CreateLvQianBeforeDamageContext(item, context);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreDamagePreventionProgramsCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamagePreventionProgramsCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame) => this._targetFactory().PushRuntimeFrame(frame);
    public void AdvanceRuntimeTop<TFrame>()
        where TFrame : global::CardGame.Core.ResolutionFrame => this._targetFactory().AdvanceRuntimeTop<TFrame>();
    public void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind) => this._targetFactory().PopResolutionFrame(frameId, expectedKind);
    public void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeTop(next);
    public void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeFrame(expectedFrameId, next);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDongChengMouRuntime))]
internal sealed class CoreDamagePreventionProgramsCharacterDongChengMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongChengMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamagePreventionProgramsCharacterDongChengMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongChengMouRuntime> _targetFactory = targetFactory;
    public bool HasMouDongCheng { get => this._targetFactory().HasMouDongCheng; }

    public global::CardGame.Core.MouDcOccurrence? ObserveMouDcCardDamage() => this._targetFactory().ObserveMouDcCardDamage();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterFuRongFuQianAnniversaryRuntime))]
internal sealed class CoreDamagePreventionProgramsCharacterFuRongFuQianAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterFuRongFuQianAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamagePreventionProgramsCharacterFuRongFuQianAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterFuRongFuQianAnniversaryRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.BeforeDamageProgramCandidate> CollectAnniversaryFuRongFuQianXueweiCandidates(int sourceSeat, int targetSeat, bool rangeChainOnly) => this._targetFactory().CollectAnniversaryFuRongFuQianXueweiCandidates(sourceSeat, targetSeat, rangeChainOnly);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoSongOlRuntime))]
internal sealed class CoreDamagePreventionProgramsCharacterCaoSongOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoSongOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamagePreventionProgramsCharacterCaoSongOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoSongOlRuntime> _targetFactory = targetFactory;
    public bool HasDamageAmountSupport(int sourceSeat, int targetSeat) => this._targetFactory().HasDamageAmountSupport(sourceSeat, targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageApplicationRuntime))]
internal sealed class CoreDamagePreventionProgramsCoreNativeDamageApplicationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageApplicationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamagePreventionProgramsCoreNativeDamageApplicationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageApplicationRuntime> _targetFactory = targetFactory;
    public bool ApplyAttackDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().ApplyAttackDamage(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime))]
internal sealed class CoreDamagePreventionProgramsCoreCardAttackStateAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamagePreventionProgramsCoreCardAttackStateOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardAttackState GetCardAttackState(long id) => this._targetFactory().GetCardAttackState(id);
    public void UpdateCardAttackState(long id, global::System.Func<global::CardGame.Core.CardAttackState?, global::CardGame.Core.CardAttackState> update) => this._targetFactory().UpdateCardAttackState(id, update);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAttributedMarkerEventsRuntime))]
internal sealed class CoreDamagePreventionProgramsCoreAttributedMarkerEventsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAttributedMarkerEventsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamagePreventionProgramsCoreAttributedMarkerEventsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAttributedMarkerEventsRuntime> _targetFactory = targetFactory;
    public bool HasAttributedEventOperations() => this._targetFactory().HasAttributedEventOperations();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaiYangAnniversaryRuntime))]
internal sealed class CoreDamagePreventionProgramsCharacterCaiYangAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiYangAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamagePreventionProgramsCharacterCaiYangAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiYangAnniversaryRuntime> _targetFactory = targetFactory;
    public bool ResumeAnniversaryCaiYangJiaofengSourceWindow(global::CardGame.Core.BeforeDamageProgramWindowFrame p, global::CardGame.Core.GameEngine.IDamageAttempt a) => this._targetFactory().ResumeAnniversaryCaiYangJiaofengSourceWindow(p, a);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchLogRuntime))]
internal sealed class CoreDamagePreventionProgramsCoreMatchLogAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamagePreventionProgramsCoreMatchLogOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> _targetFactory = targetFactory;
    public void AddLog(string type, string message, int? actor = default, int? target = default) => this._targetFactory().AddLog(type, message, actor, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageCompletionRuntime))]
internal sealed class CoreDamagePreventionProgramsCoreNativeDamageCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamagePreventionProgramsCoreNativeDamageCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageCompletionRuntime> _targetFactory = targetFactory;
    public void CompleteDamageAttack(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().CompleteDamageAttack(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangLiaoHappyGodRuntime))]
internal sealed class CoreDamagePreventionProgramsCharacterZhangLiaoHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangLiaoHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamagePreventionProgramsCharacterZhangLiaoHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangLiaoHappyGodRuntime> _targetFactory = targetFactory;
    public bool HasHappyZlSourceRestoration(int source, int target) => this._targetFactory().HasHappyZlSourceRestoration(source, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramWindowCursorRuntime))]
internal sealed class CoreDamagePreventionProgramsProgramWindowCursorAdapter(global::System.Func<global::CardGame.Core.ProgramWindowCursorRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamagePreventionProgramsProgramWindowCursorOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramWindowCursorRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramWindowCursorRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHealthQueriesRuntime))]
internal sealed class CoreDamagePreventionProgramsCoreHealthQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHealthQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamagePreventionProgramsCoreHealthQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHealthQueriesRuntime> _targetFactory = targetFactory;
    public int? UnambiguousHp(global::CardGame.Core.CharacterState player) => this._targetFactory().UnambiguousHp(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CoreDamagePreventionProgramsCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamagePreventionProgramsCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player) => this._targetFactory().GetSkillBindingShard(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime))]
internal sealed class CoreDamagePreventionProgramsCoreTableOrderAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamagePreventionProgramsCoreTableOrderOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime> _targetFactory = targetFactory;
    public int RelativeTableOrder(int from, int target) => this._targetFactory().RelativeTableOrder(from, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramPrivateDamageOfferOperationsRuntime))]
internal sealed class CoreDamagePreventionProgramsCoreProgramPrivateDamageOfferOperationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramPrivateDamageOfferOperationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamagePreventionProgramsCoreProgramPrivateDamageOfferOperationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramPrivateDamageOfferOperationsRuntime> _targetFactory = targetFactory;
    public bool IsDamageOfferPairBlocked(int owner, string skill, int source) => this._targetFactory().IsDamageOfferPairBlocked(owner, skill, source);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterChenGongMouMobileRuntime))]
internal sealed class CoreDamagePreventionProgramsCharacterChenGongMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChenGongMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamagePreventionProgramsCharacterChenGongMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChenGongMouMobileRuntime> _targetFactory = targetFactory;
    public bool IsMouChenGongZhichiBeforeCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate) => this._targetFactory().IsMouChenGongZhichiBeforeCandidate(candidate);
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.BeforeDamageProgramCandidate> CollectIssuedMouChenGongZhichiCandidates(int source, int target, bool rangeChainOnly) => this._targetFactory().CollectIssuedMouChenGongZhichiCandidates(source, target, rangeChainOnly);
    public global::CardGame.Core.ProgramSkillWindowContext AttachMouChenGongProtection(global::CardGame.Core.BeforeDamageProgramCandidate item, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().AttachMouChenGongProtection(item, context);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreDamagePreventionProgramsCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamagePreventionProgramsCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime))]
internal sealed class CoreDamagePreventionProgramsCoreResolutionIdentityBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamagePreventionProgramsCoreResolutionIdentityBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime> _targetFactory = targetFactory;
    public long _resolutionSequence { get => this._targetFactory()._resolutionSequence; set => this._targetFactory()._resolutionSequence = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTreatedWoundedRuntime))]
internal sealed class CoreDamagePreventionProgramsCoreTreatedWoundedAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTreatedWoundedRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamagePreventionProgramsCoreTreatedWoundedOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTreatedWoundedRuntime> _targetFactory = targetFactory;
    public bool UsesTreatedWounded { get => this._targetFactory().UsesTreatedWounded; }

    public bool IsRuleWounded(global::CardGame.Core.CharacterState player) => this._targetFactory().IsRuleWounded(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXinPiOlRuntime))]
internal sealed class CoreDamagePreventionProgramsCharacterXinPiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXinPiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamagePreventionProgramsCharacterXinPiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXinPiOlRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.BeforeDamageProgramCandidate> CollectIssuedYinjuCandidates(int sourceSeat, int targetSeat, bool rangeChainOnly) => this._targetFactory().CollectIssuedYinjuCandidates(sourceSeat, targetSeat, rangeChainOnly);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDamageCardColorHealthRuntime))]
internal sealed class CoreDamagePreventionProgramsCoreDamageCardColorHealthAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageCardColorHealthRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamagePreventionProgramsCoreDamageCardColorHealthOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageCardColorHealthRuntime> _targetFactory = targetFactory;
    public bool HasRecipientDamageCardColorHealth(int seat) => this._targetFactory().HasRecipientDamageCardColorHealth(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreDamagePreventionProgramsCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamagePreventionProgramsCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramTriggerFacts CaptureProgramTriggerFacts(global::CardGame.Core.CharacterState owner) => this._targetFactory().CaptureProgramTriggerFacts(owner);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectProgramTriggerCandidates(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTriggerWindow window, int occurrenceIndex = 0) => this._targetFactory().CollectProgramTriggerCandidates(owner, window, occurrenceIndex);
    public global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate) => this._targetFactory().GetProgramTrigger(candidate);
    public bool CanRunProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().CanRunProgramTrigger(candidate, context);
    public void ExposeProgramTriggerDecision(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().ExposeProgramTriggerDecision(candidate, context);
    public void BeginProgramBinding(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().BeginProgramBinding(candidate, context);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime))]
internal sealed class CoreDamagePreventionProgramsCoreCardUseLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamagePreventionProgramsCoreCardUseLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId) => this._targetFactory().LifecycleCardUse(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMangYaChangAnniversaryRuntime))]
internal sealed class CoreDamagePreventionProgramsCharacterMangYaChangAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMangYaChangAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamagePreventionProgramsCharacterMangYaChangAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMangYaChangAnniversaryRuntime> _targetFactory = targetFactory;
    public bool ResumeAnniversaryJiedaoSourceWindow(global::CardGame.Core.BeforeDamageProgramWindowFrame window, global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().ResumeAnniversaryJiedaoSourceWindow(window, attack);
}
