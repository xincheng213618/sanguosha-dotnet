#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterJianYongBoundaryMobileEntries
{
    internal bool AdvanceBoundaryZongshiChild() => _resolve().AdvanceBoundaryZongshiChild();
    internal void AnswerBoundaryZongshi(PromptChoice choice) => _resolve().AnswerBoundaryZongshi(choice);
    internal SkillProgramStepOutcome BeginBoundaryJianYongZongshiChoice(ProgramSkillFrame f) => _resolve().BeginBoundaryJianYongZongshiChoice(f);
    internal bool BoundaryJianYongAdjustmentConsumes(NextActualUseTargetAdjustmentGrantedEvent grant, CardKind kind) => _resolve().BoundaryJianYongAdjustmentConsumes(grant, kind);
    internal bool BoundaryJianYongPhaseCurrent(BoundaryJianYongPhaseEvent e) => _resolve().BoundaryJianYongPhaseCurrent(e);
    internal bool BoundaryJianYongTrickForbidden(ProgramCardUseQuery q) => _resolve().BoundaryJianYongTrickForbidden(q);
    internal IReadOnlyList<PromptChoice> BoundaryZongshiChoices(ProgramSkillFrame f) => _resolve().BoundaryZongshiChoices(f);
    internal ProgramSkillWindowContext BoundaryZongshiContext(ResolutionFrame p, ProgramTriggerCandidate c) => _resolve().BoundaryZongshiContext(p, c);
    internal bool BoundaryZongshiDyingChild() => _resolve().BoundaryZongshiDyingChild();
    internal bool BoundaryZongshiFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().BoundaryZongshiFirstChild(f, child);
    internal int[] BoundaryZongshiLower(ResolutionFrame p) => _resolve().BoundaryZongshiLower(p);
    internal bool BoundaryZongshiNativeResult(ResolutionFrame p, scoped out PindianResult result, scoped out int comparison) => _resolve().BoundaryZongshiNativeResult(p, out result, out comparison);
    internal ProgramSkillFrame? BoundaryZongshiObserverRoot() => _resolve().BoundaryZongshiObserverRoot();
    internal bool BoundaryZongshiParent(ProgramSkillFrame f, ResolutionFrame p) => _resolve().BoundaryZongshiParent(f, p);
    internal bool BoundaryZongshiQualifies(ProgramTriggerLegalityContext g) => _resolve().BoundaryZongshiQualifies(g);
    internal bool BoundaryZongshiSuffix(int n) => _resolve().BoundaryZongshiSuffix(n);
    internal void FinishBoundaryZongshi(long id) => _resolve().FinishBoundaryZongshi(id);
    internal bool IsBoundaryJianYongAdjustmentCurrent(NextActualUseTargetAdjustmentGrantedEvent grant) => _resolve().IsBoundaryJianYongAdjustmentCurrent(grant);
    internal bool IsBoundaryJianYongZongshiCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().IsBoundaryJianYongZongshiCandidate(c, context);
    internal bool IsBoundaryJianYongZongshiSubtree(PindianFrame p) => _resolve().IsBoundaryJianYongZongshiSubtree(p);
    internal bool KeepBoundaryZongshiPaid(ProgramSkillFrame f) => _resolve().KeepBoundaryZongshiPaid(f);
    internal void PublishBoundaryZongshiChoice(ProgramSkillFrame f) => _resolve().PublishBoundaryZongshiChoice(f);
    internal SkillProgramStepOutcome RecordBoundaryJianYongFailure(ProgramSkillFrame f) => _resolve().RecordBoundaryJianYongFailure(f);
    internal void RequireBoundaryZongshi(ProgramSkillFrame f) => _resolve().RequireBoundaryZongshi(f);
    internal bool ResumeBoundaryZongshi(long id) => _resolve().ResumeBoundaryZongshi(id);
    internal bool ReturnBoundaryZongshiBinding(ProgramSkillFrame f, bool completed) => _resolve().ReturnBoundaryZongshiBinding(f, completed);
    internal bool ReturnBoundaryZongshiMovement(ProgramSkillFrame f) => _resolve().ReturnBoundaryZongshiMovement(f);
    internal PromptChoice SelectAiBoundaryZongshi(PendingDecision p) => _resolve().SelectAiBoundaryZongshi(p);
    internal void TrackBoundaryJianYongGrant(NextActualUseTargetAdjustmentGrantedEvent e) => _resolve().TrackBoundaryJianYongGrant(e);
    internal bool TryBeginBoundaryJianYongSimultaneousZongshi(ProgramSkillFrame f) => _resolve().TryBeginBoundaryJianYongSimultaneousZongshi(f);
    internal bool TryBeginBoundaryJianYongZongshi(PindianFrame p) => _resolve().TryBeginBoundaryJianYongZongshi(p);
    internal bool TryBeginBoundaryZongshiContest(ResolutionFrame supplied, PindianResult result, int comparison) => _resolve().TryBeginBoundaryZongshiContest(supplied, result, comparison);
}
