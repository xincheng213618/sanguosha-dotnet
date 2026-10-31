#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSunRuOlEntries
{
    internal void AnswerSunRuOlChoice(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerSunRuOlChoice(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginSunRuOlChishi(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginSunRuOlChishi(supplied);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginSunRuOlWeimian(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginSunRuOlWeimian(supplied);
    internal bool CanActivateSunRuOlWeimian(int ownerSeat) => _resolve().CanActivateSunRuOlWeimian(ownerSeat);
    internal bool CanContinuePaidSunRuOl(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinuePaidSunRuOl(f);
    internal bool CanRunSunRuOlChishiCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunSunRuOlChishiCandidate(c, context);
    internal global::CardGame.Core.CardMovementBatchContext CaptureSunRuOlChishiBatch(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CaptureSunRuOlChishiBatch(batch);
    internal global::CardGame.Core.PromptChoice ChooseAiSunRuOl(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseAiSunRuOl(p, f);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> CollectSunRuOlChishiCandidates(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CollectSunRuOlChishiCandidates(batch);
    internal void CollectWindow_CollectSunRuOlChishiCandidates(global::CardGame.Core.CardMovementBatchContext batch, global::System.Collections.Generic.List<global::CardGame.Core.ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectSunRuOlChishiCandidates(batch, result);
    internal global::CardGame.Core.CardMovementBatchContext CompleteMovementBatchCaptureSunRuOlChishiBatch(global::CardGame.Core.CardMovementBatchBuilder batch, global::CardGame.Core.CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureSunRuOlChishiBatch(batch, completed);
    internal void CompleteSunRuOl(global::CardGame.Core.ProgramSkillFrame f, bool completed) => _resolve().CompleteSunRuOl(f, completed);
    internal void ContinueSunRuOl(long id) => _resolve().ContinueSunRuOl(id);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateSunRuOlChishiContext(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate c) => _resolve().CreateSunRuOlChishiContext(frame, c);
    internal bool DrainSunRuOlChildren(long id) => _resolve().DrainSunRuOlChildren(id);
    internal bool ExcludeOrdinaryMovement_IsSunRuOlChishiTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_IsSunRuOlChishiTrigger(candidate, trigger);
    internal bool HasSunRuOlDamageObserver(long id) => _resolve().HasSunRuOlDamageObserver(id);
    internal bool IsSunRuOlRecoveryParent(global::CardGame.Core.ProgramSkillFrame producer, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsSunRuOlRecoveryParent(producer, recovery);
    internal void IssueSunRuOlRefreshDraw(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueSunRuOlRefreshDraw(f);
    internal bool? ProveSunRuMovement(global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().ProveSunRuMovement(trigger, candidate, context);
    internal void PublishSunRuOlChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishSunRuOlChoice(f);
    internal bool ResumeSunRuOl(long id) => _resolve().ResumeSunRuOl(id);
    internal bool SunRuOlActualTurn(global::CardGame.Core.SunRuOlTurnToken t) => _resolve().SunRuOlActualTurn(t);
    internal int SunRuOlAvailableSlotMask(int seat) => _resolve().SunRuOlAvailableSlotMask(seat);
    internal int SunRuOlBenefitAiValue(global::CardGame.Core.ProgramSkillFrame f, int mask) => _resolve().SunRuOlBenefitAiValue(f, mask);
    internal bool SunRuOlChishiUsed(int owner, string skill, global::CardGame.Core.SunRuOlTurnToken turn) => _resolve().SunRuOlChishiUsed(owner, skill, turn);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> SunRuOlChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SunRuOlChoices(f);
    internal bool SunRuOlFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().SunRuOlFirstChild(root, child);
    internal global::CardGame.Core.ProgramSkillFrame? SunRuOlObserverRoot() => _resolve().SunRuOlObserverRoot();
    internal global::CardGame.Core.CardConversionSource SunRuOlSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SunRuOlSource(f);
    internal bool TryAdvanceSunRuOlSubtree() => _resolve().TryAdvanceSunRuOlSubtree();
    internal global::CardGame.Core.ProgramSkillWindowContext? TryCreateSunRuOlChishiContext(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().TryCreateSunRuOlChishiContext(frame, candidate);
}
