#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSunRuOlEntries
{
    internal void AnswerSunRuOlChoice(PromptChoice choice) => _resolve().AnswerSunRuOlChoice(choice);
    internal SkillProgramStepOutcome BeginSunRuOlChishi(ProgramSkillFrame supplied) => _resolve().BeginSunRuOlChishi(supplied);
    internal SkillProgramStepOutcome BeginSunRuOlWeimian(ProgramSkillFrame supplied) => _resolve().BeginSunRuOlWeimian(supplied);
    internal bool CanActivateSunRuOlWeimian(int ownerSeat) => _resolve().CanActivateSunRuOlWeimian(ownerSeat);
    internal bool CanContinuePaidSunRuOl(ProgramSkillFrame f) => _resolve().CanContinuePaidSunRuOl(f);
    internal bool CanRunSunRuOlChishiCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().CanRunSunRuOlChishiCandidate(c, context);
    internal CardMovementBatchContext CaptureSunRuOlChishiBatch(CardMovementBatchContext batch) => _resolve().CaptureSunRuOlChishiBatch(batch);
    internal PromptChoice ChooseAiSunRuOl(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseAiSunRuOl(p, f);
    internal IEnumerable<ProgramTriggerCandidate> CollectSunRuOlChishiCandidates(CardMovementBatchContext batch) => _resolve().CollectSunRuOlChishiCandidates(batch);
    internal void CollectWindow_CollectSunRuOlChishiCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectSunRuOlChishiCandidates(batch, result);
    internal CardMovementBatchContext CompleteMovementBatchCaptureSunRuOlChishiBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureSunRuOlChishiBatch(batch, completed);
    internal void CompleteSunRuOl(ProgramSkillFrame f, bool completed) => _resolve().CompleteSunRuOl(f, completed);
    internal void ContinueSunRuOl(long id) => _resolve().ContinueSunRuOl(id);
    internal ProgramSkillWindowContext CreateSunRuOlChishiContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate c) => _resolve().CreateSunRuOlChishiContext(frame, c);
    internal bool DrainSunRuOlChildren(long id) => _resolve().DrainSunRuOlChildren(id);
    internal bool ExcludeOrdinaryMovement_IsSunRuOlChishiTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_IsSunRuOlChishiTrigger(candidate, trigger);
    internal bool IsSunRuOlRecoveryParent(ProgramSkillFrame producer, RecoveryReplacementFrame recovery) => _resolve().IsSunRuOlRecoveryParent(producer, recovery);
    internal void IssueSunRuOlRefreshDraw(ProgramSkillFrame f) => _resolve().IssueSunRuOlRefreshDraw(f);
    internal bool? ProveSunRuMovement(SkillProgramTrigger trigger, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().ProveSunRuMovement(trigger, candidate, context);
    internal void PublishSunRuOlChoice(ProgramSkillFrame f) => _resolve().PublishSunRuOlChoice(f);
    internal bool ResumeSunRuOl(long id) => _resolve().ResumeSunRuOl(id);
    internal bool SunRuOlActualTurn(SunRuOlTurnToken t) => _resolve().SunRuOlActualTurn(t);
    internal int SunRuOlAvailableSlotMask(int seat) => _resolve().SunRuOlAvailableSlotMask(seat);
    internal int SunRuOlBenefitAiValue(ProgramSkillFrame f, int mask) => _resolve().SunRuOlBenefitAiValue(f, mask);
    internal bool SunRuOlChishiUsed(int owner, string skill, SunRuOlTurnToken turn) => _resolve().SunRuOlChishiUsed(owner, skill, turn);
    internal IReadOnlyList<PromptChoice> SunRuOlChoices(ProgramSkillFrame f) => _resolve().SunRuOlChoices(f);
    internal bool SunRuOlFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().SunRuOlFirstChild(root, child);
    internal ProgramSkillFrame? SunRuOlObserverRoot() => _resolve().SunRuOlObserverRoot();
    internal CardConversionSource SunRuOlSource(ProgramSkillFrame f) => _resolve().SunRuOlSource(f);
    internal bool TryAdvanceSunRuOlSubtree() => _resolve().TryAdvanceSunRuOlSubtree();
    internal ProgramSkillWindowContext? TryCreateSunRuOlChishiContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _resolve().TryCreateSunRuOlChishiContext(frame, candidate);
}
