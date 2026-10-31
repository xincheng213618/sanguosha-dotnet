#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterJianYongBoundaryMobileEntries
{
    internal bool AdvanceBoundaryZongshiChild() => _resolve().AdvanceBoundaryZongshiChild();
    internal void AnswerBoundaryZongshi(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerBoundaryZongshi(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginBoundaryJianYongZongshiChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BeginBoundaryJianYongZongshiChoice(f);
    internal bool BoundaryJianYongAdjustmentConsumes(global::CardGame.Core.NextActualUseTargetAdjustmentGrantedEvent grant, global::CardGame.Core.CardKind kind) => _resolve().BoundaryJianYongAdjustmentConsumes(grant, kind);
    internal bool BoundaryJianYongPhaseCurrent(global::CardGame.Core.BoundaryJianYongPhaseEvent e) => _resolve().BoundaryJianYongPhaseCurrent(e);
    internal bool BoundaryJianYongTrickForbidden(global::CardGame.Core.ProgramCardUseQuery q) => _resolve().BoundaryJianYongTrickForbidden(q);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> BoundaryZongshiChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BoundaryZongshiChoices(f);
    internal global::CardGame.Core.ProgramSkillWindowContext BoundaryZongshiContext(global::CardGame.Core.ResolutionFrame p, global::CardGame.Core.ProgramTriggerCandidate c) => _resolve().BoundaryZongshiContext(p, c);
    internal bool BoundaryZongshiDyingChild() => _resolve().BoundaryZongshiDyingChild();
    internal bool BoundaryZongshiFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().BoundaryZongshiFirstChild(f, child);
    internal int[] BoundaryZongshiLower(global::CardGame.Core.ResolutionFrame p) => _resolve().BoundaryZongshiLower(p);
    internal bool BoundaryZongshiNativeResult(global::CardGame.Core.ResolutionFrame p, scoped out global::CardGame.Core.PindianResult result, scoped out int comparison) => _resolve().BoundaryZongshiNativeResult(p, out result, out comparison);
    internal global::CardGame.Core.ProgramSkillFrame? BoundaryZongshiObserverRoot() => _resolve().BoundaryZongshiObserverRoot();
    internal bool BoundaryZongshiParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame p) => _resolve().BoundaryZongshiParent(f, p);
    internal bool BoundaryZongshiQualifies(global::CardGame.Core.ProgramTriggerLegalityContext g) => _resolve().BoundaryZongshiQualifies(g);
    internal bool BoundaryZongshiSuffix(int n) => _resolve().BoundaryZongshiSuffix(n);
    internal void FinishBoundaryZongshi(long id) => _resolve().FinishBoundaryZongshi(id);
    internal bool IsBoundaryJianYongAdjustmentCurrent(global::CardGame.Core.NextActualUseTargetAdjustmentGrantedEvent grant) => _resolve().IsBoundaryJianYongAdjustmentCurrent(grant);
    internal bool IsBoundaryJianYongZongshiCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsBoundaryJianYongZongshiCandidate(c, context);
    internal bool IsBoundaryJianYongZongshiSubtree(global::CardGame.Core.PindianFrame p) => _resolve().IsBoundaryJianYongZongshiSubtree(p);
    internal bool KeepBoundaryZongshiPaid(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepBoundaryZongshiPaid(f);
    internal void PublishBoundaryZongshiChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishBoundaryZongshiChoice(f);
    internal global::CardGame.Core.SkillProgramStepOutcome RecordBoundaryJianYongFailure(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RecordBoundaryJianYongFailure(f);
    internal void RequireBoundaryZongshi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireBoundaryZongshi(f);
    internal bool ResumeBoundaryZongshi(long id) => _resolve().ResumeBoundaryZongshi(id);
    internal bool ReturnBoundaryZongshiBinding(global::CardGame.Core.ProgramSkillFrame f, bool completed) => _resolve().ReturnBoundaryZongshiBinding(f, completed);
    internal bool ReturnBoundaryZongshiMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnBoundaryZongshiMovement(f);
    internal global::CardGame.Core.PromptChoice SelectAiBoundaryZongshi(global::CardGame.Core.PendingDecision p) => _resolve().SelectAiBoundaryZongshi(p);
    internal void TrackBoundaryJianYongGrant(global::CardGame.Core.NextActualUseTargetAdjustmentGrantedEvent e) => _resolve().TrackBoundaryJianYongGrant(e);
    internal bool TryBeginBoundaryJianYongSimultaneousZongshi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().TryBeginBoundaryJianYongSimultaneousZongshi(f);
    internal bool TryBeginBoundaryJianYongZongshi(global::CardGame.Core.PindianFrame p) => _resolve().TryBeginBoundaryJianYongZongshi(p);
    internal bool TryBeginBoundaryZongshiContest(global::CardGame.Core.ResolutionFrame supplied, global::CardGame.Core.PindianResult result, int comparison) => _resolve().TryBeginBoundaryZongshiContest(supplied, result, comparison);
}
