#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreDamagePreventionProgramsCoreProgramAttackAttemptOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameEngine.IDamageAttempt? CurrentDamageAttempt { get; }

    global::CardGame.Core.AttackAttemptState GetProgramAttackState(long ownerFrameId);
    void UpdateProgramAttackState(long ownerFrameId, global::System.Func<global::CardGame.Core.AttackAttemptState, global::CardGame.Core.AttackAttemptState> update);
}

internal interface ICoreDamagePreventionProgramsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreDamagePreventionProgramsCoreProgramRuntimeHooksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramRuntimeHooks RuntimeHooks { get; }
}

internal interface ICoreDamagePreventionProgramsCoreProgramJudgmentOutcomesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRecipientJudgmentOutcomePrevention(int targetSeat);
    bool IsJudgmentOutcomePrevention(global::CardGame.Core.SkillProgramTrigger trigger);
}

internal interface ICoreDamagePreventionProgramsCharacterLvQianOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.BeforeDamageProgramCandidate> CollectZengdaoDamageCandidates(int source, int target, bool rangeChainOnly);
    global::CardGame.Core.ProgramSkillWindowContext CreateLvQianBeforeDamageContext(global::CardGame.Core.BeforeDamageProgramCandidate item, global::CardGame.Core.ProgramSkillWindowContext context);
}

internal interface ICoreDamagePreventionProgramsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreDamagePreventionProgramsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreDamagePreventionProgramsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame);
    void AdvanceRuntimeTop<TFrame>()
        where TFrame : global::CardGame.Core.ResolutionFrame;
    void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind);
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
}

internal interface ICoreDamagePreventionProgramsCharacterDongChengMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMouDongCheng { get; }

    global::CardGame.Core.MouDcOccurrence? ObserveMouDcCardDamage();
}

internal interface ICoreDamagePreventionProgramsCharacterFuRongFuQianAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.BeforeDamageProgramCandidate> CollectAnniversaryFuRongFuQianXueweiCandidates(int sourceSeat, int targetSeat, bool rangeChainOnly);
}

internal interface ICoreDamagePreventionProgramsCharacterCaoSongOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasDamageAmountSupport(int sourceSeat, int targetSeat);
}

internal interface ICoreDamagePreventionProgramsCoreNativeDamageApplicationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ApplyAttackDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreDamagePreventionProgramsCoreCardAttackStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardAttackState GetCardAttackState(long id);
    void UpdateCardAttackState(long id, global::System.Func<global::CardGame.Core.CardAttackState?, global::CardGame.Core.CardAttackState> update);
}

internal interface ICoreDamagePreventionProgramsCoreAttributedMarkerEventsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAttributedEventOperations();
}

internal interface ICoreDamagePreventionProgramsCharacterCaiYangAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ResumeAnniversaryCaiYangJiaofengSourceWindow(global::CardGame.Core.BeforeDamageProgramWindowFrame p, global::CardGame.Core.GameEngine.IDamageAttempt a);
}

internal interface ICoreDamagePreventionProgramsCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreDamagePreventionProgramsCoreNativeDamageCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteDamageAttack(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreDamagePreventionProgramsCharacterZhangLiaoHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasHappyZlSourceRestoration(int source, int target);
}

internal interface ICoreDamagePreventionProgramsProgramWindowCursorOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramWindowCursorRuntime Get();
}

internal interface ICoreDamagePreventionProgramsCoreHealthQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int? UnambiguousHp(global::CardGame.Core.CharacterState player);
}

internal interface ICoreDamagePreventionProgramsCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player);
}

internal interface ICoreDamagePreventionProgramsCoreTableOrderOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int RelativeTableOrder(int from, int target);
}

internal interface ICoreDamagePreventionProgramsCoreProgramPrivateDamageOfferOperationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDamageOfferPairBlocked(int owner, string skill, int source);
}

internal interface ICoreDamagePreventionProgramsCharacterChenGongMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouChenGongZhichiBeforeCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate);
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.BeforeDamageProgramCandidate> CollectIssuedMouChenGongZhichiCandidates(int source, int target, bool rangeChainOnly);
    global::CardGame.Core.ProgramSkillWindowContext AttachMouChenGongProtection(global::CardGame.Core.BeforeDamageProgramCandidate item, global::CardGame.Core.ProgramSkillWindowContext context);
}

internal interface ICoreDamagePreventionProgramsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreDamagePreventionProgramsCoreResolutionIdentityBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long _resolutionSequence { get; set; }
}

internal interface ICoreDamagePreventionProgramsCoreTreatedWoundedOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool UsesTreatedWounded { get; }

    bool IsRuleWounded(global::CardGame.Core.CharacterState player);
}

internal interface ICoreDamagePreventionProgramsCharacterXinPiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.BeforeDamageProgramCandidate> CollectIssuedYinjuCandidates(int sourceSeat, int targetSeat, bool rangeChainOnly);
}

internal interface ICoreDamagePreventionProgramsCoreDamageCardColorHealthOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRecipientDamageCardColorHealth(int seat);
}

internal interface ICoreDamagePreventionProgramsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramTriggerFacts CaptureProgramTriggerFacts(global::CardGame.Core.CharacterState owner);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectProgramTriggerCandidates(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTriggerWindow window, int occurrenceIndex = 0);
    global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate);
    bool CanRunProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context);
    void ExposeProgramTriggerDecision(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context);
    void BeginProgramBinding(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context);
}

internal interface ICoreDamagePreventionProgramsCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICoreDamagePreventionProgramsCharacterMangYaChangAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ResumeAnniversaryJiedaoSourceWindow(global::CardGame.Core.BeforeDamageProgramWindowFrame window, global::CardGame.Core.GameEngine.IDamageAttempt attack);
}
