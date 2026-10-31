#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYanPuOlEntries
{
    internal bool AdvanceYanPuSubtree() => _resolve().AdvanceYanPuSubtree();
    internal void AnswerYanPu(PromptChoice choice) => _resolve().AnswerYanPu(choice);
    internal void AppendYanPuEndingItems(List<TurnEndingBoundaryItem> items) => _resolve().AppendYanPuEndingItems(items);
    internal SkillProgramStepOutcome BeginYanPu(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginYanPu(supplied, op);
    internal bool CanRunYanPuEnding(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().CanRunYanPuEnding(c, context);
    internal PromptChoice ChooseAiYanPu(PendingDecision d, ProgramSkillFrame f) => _resolve().ChooseAiYanPu(d, f);
    internal bool CollectYanPuCandidate(SkillProgramTrigger t) => _resolve().CollectYanPuCandidate(t);
    internal void CommitYanPuMovement(long id, YanPuMovement movement) => _resolve().CommitYanPuMovement(id, movement);
    internal void CompleteYanPu(ProgramSkillFrame f, bool applied) => _resolve().CompleteYanPu(f, applied);
    internal bool ContinuePaidYanPu(ProgramSkillFrame f) => _resolve().ContinuePaidYanPu(f);
    internal void ContinueYanPu(long id) => _resolve().ContinueYanPu(id);
    internal YanPuHuantuPromise? CurrentYanPuEnding(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().CurrentYanPuEnding(c, context);
    internal bool? DecideIssuedTrigger680(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().DecideIssuedTrigger680(candidate, context);
    internal bool DrainYanPuChildren(ProgramSkillFrame f) => _resolve().DrainYanPuChildren(f);
    internal IReadOnlyList<ProgramTriggerCandidate> IncludeYanPuDrawObservers(CharacterState subject, IReadOnlyList<ProgramTriggerCandidate> existing) => _resolve().IncludeYanPuDrawObservers(subject, existing);
    internal bool IsYanPuDrawContext(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().IsYanPuDrawContext(c, context);
    internal bool IsYanPuDyingExitContext(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().IsYanPuDyingExitContext(c, context);
    internal bool IsYanPuDyingExitEdge(int index) => _resolve().IsYanPuDyingExitEdge(index);
    internal bool IsYanPuEndingCandidate(ProgramTriggerCandidate c) => _resolve().IsYanPuEndingCandidate(c);
    internal bool IsYanPuRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsYanPuRecoveryParent(f, recovery);
    internal void IssueYanPuDraw(ProgramSkillFrame f, int subject, int count, YanPuStage stage) => _resolve().IssueYanPuDraw(f, subject, count, stage);
    internal void PublishYanPuChoice(ProgramSkillFrame f) => _resolve().PublishYanPuChoice(f);
    internal bool QualifyYanPuCandidate(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => _resolve().QualifyYanPuCandidate(c, t, context);
    internal bool ResumeYanPu(long id) => _resolve().ResumeYanPu(id);
    internal void ReturnYanPuDyingExit(ProgramLifecycleTriggerWindowFrame frame) => _resolve().ReturnYanPuDyingExit(frame);
    internal bool ReturnYanPuMovement(ProgramSkillFrame f) => _resolve().ReturnYanPuMovement(f);
    internal bool TryBeginYanPuDyingExit(GameEngine.DyingCompletionReceipt dying, bool survived) => _resolve().TryBeginYanPuDyingExit(dying, survived);
    internal bool ValidYanPuPromise(YanPuHuantuPromise p) => _resolve().ValidYanPuPromise(p);
    internal IReadOnlyList<PromptChoice> YanPuChoices(ProgramSkillFrame f) => _resolve().YanPuChoices(f);
    internal ProgramTriggerCandidate? YanPuCurrentEndingFrameCandidate(ProgramSkillFrame f) => _resolve().YanPuCurrentEndingFrameCandidate(f);
    internal bool YanPuFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().YanPuFirstChild(root, child);
    internal ProgramSkillFrame? YanPuObserverRoot() => _resolve().YanPuObserverRoot();
    internal bool YanPuPromiseConsumed(long id) => _resolve().YanPuPromiseConsumed(id);
    internal bool YanPuRoundUsed(int seat, string skill) => _resolve().YanPuRoundUsed(seat, skill);
}
