#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYanPuOlEntries
{
    internal bool AdvanceYanPuSubtree() => _resolve().AdvanceYanPuSubtree();
    internal void AnswerYanPu(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerYanPu(choice);
    internal void AppendYanPuEndingItems(global::System.Collections.Generic.List<global::CardGame.Core.TurnEndingBoundaryItem> items) => _resolve().AppendYanPuEndingItems(items);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginYanPu(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginYanPu(supplied, op);
    internal bool CanRunYanPuEnding(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunYanPuEnding(c, context);
    internal global::CardGame.Core.PromptChoice ChooseAiYanPu(global::CardGame.Core.PendingDecision d, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseAiYanPu(d, f);
    internal bool CollectYanPuCandidate(global::CardGame.Core.SkillProgramTrigger t) => _resolve().CollectYanPuCandidate(t);
    internal void CommitYanPuMovement(long id, global::CardGame.Core.YanPuMovement movement) => _resolve().CommitYanPuMovement(id, movement);
    internal void CompleteYanPu(global::CardGame.Core.ProgramSkillFrame f, bool applied) => _resolve().CompleteYanPu(f, applied);
    internal bool ContinuePaidYanPu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidYanPu(f);
    internal void ContinueYanPu(long id) => _resolve().ContinueYanPu(id);
    internal global::CardGame.Core.YanPuHuantuPromise? CurrentYanPuEnding(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CurrentYanPuEnding(c, context);
    internal bool? DecideIssuedTrigger680(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().DecideIssuedTrigger680(candidate, context);
    internal bool DrainYanPuChildren(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainYanPuChildren(f);
    internal bool HasYanPuDamageObserver(long id) => _resolve().HasYanPuDamageObserver(id);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> IncludeYanPuDrawObservers(global::CardGame.Core.CharacterState subject, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> existing) => _resolve().IncludeYanPuDrawObservers(subject, existing);
    internal bool IsYanPuAwaitedMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsYanPuAwaitedMovement(f, e, p);
    internal bool IsYanPuDrawContext(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsYanPuDrawContext(c, context);
    internal bool IsYanPuDyingExitContext(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsYanPuDyingExitContext(c, context);
    internal bool IsYanPuDyingExitEdge(int index) => _resolve().IsYanPuDyingExitEdge(index);
    internal bool IsYanPuEndingCandidate(global::CardGame.Core.ProgramTriggerCandidate c) => _resolve().IsYanPuEndingCandidate(c);
    internal bool IsYanPuRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsYanPuRecoveryParent(f, recovery);
    internal void IssueYanPuDraw(global::CardGame.Core.ProgramSkillFrame f, int subject, int count, global::CardGame.Core.YanPuStage stage) => _resolve().IssueYanPuDraw(f, subject, count, stage);
    internal void PublishYanPuChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishYanPuChoice(f);
    internal bool QualifyYanPuCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().QualifyYanPuCandidate(c, t, context);
    internal bool ResumeYanPu(long id) => _resolve().ResumeYanPu(id);
    internal void ReturnYanPuDyingExit(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame frame) => _resolve().ReturnYanPuDyingExit(frame);
    internal bool ReturnYanPuMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnYanPuMovement(f);
    internal bool TryBeginYanPuDyingExit(global::CardGame.Core.GameEngine.DyingCompletionReceipt dying, bool survived) => _resolve().TryBeginYanPuDyingExit(dying, survived);
    internal bool ValidYanPuPromise(global::CardGame.Core.YanPuHuantuPromise p) => _resolve().ValidYanPuPromise(p);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> YanPuChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().YanPuChoices(f);
    internal global::CardGame.Core.ProgramTriggerCandidate? YanPuCurrentEndingFrameCandidate(global::CardGame.Core.ProgramSkillFrame f) => _resolve().YanPuCurrentEndingFrameCandidate(f);
    internal bool YanPuFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().YanPuFirstChild(root, child);
    internal global::CardGame.Core.ProgramSkillFrame? YanPuObserverRoot() => _resolve().YanPuObserverRoot();
    internal bool YanPuPromiseConsumed(long id) => _resolve().YanPuPromiseConsumed(id);
    internal bool YanPuRoundUsed(int seat, string skill) => _resolve().YanPuRoundUsed(seat, skill);
}
