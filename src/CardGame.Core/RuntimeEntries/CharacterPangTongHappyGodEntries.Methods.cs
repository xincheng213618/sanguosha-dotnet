#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterPangTongHappyGodEntries
{
    internal void AnswerHappyPt(PromptChoice choice) => _resolve().AnswerHappyPt(choice);
    internal void AssertHappyPtUpperReturn(CardUseFrame use) => _resolve().AssertHappyPtUpperReturn(use);
    internal SkillProgramStepOutcome BeginHappyPangTong(ProgramSkillFrame supplied, SkillProgramEffectOp operation) => _resolve().BeginHappyPangTong(supplied, operation);
    internal void BeginHappyPtReward(ProgramSkillFrame f) => _resolve().BeginHappyPtReward(f);
    internal bool BeginHappyPtSettled(ProgramSkillFrame root, bool success) => _resolve().BeginHappyPtSettled(root, success);
    internal void BindHappyPtUpper(IGameEvent payload) => _resolve().BindHappyPtUpper(payload);
    internal bool CanOfferHappyPangTong(ProgramTriggerLegalityContext g) => _resolve().CanOfferHappyPangTong(g);
    internal bool CanRunHappyPangTongSettled(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().CanRunHappyPangTongSettled(candidate, context);
    internal bool CompleteHappyPtWindow(ProgramLifecycleTriggerWindowFrame window) => _resolve().CompleteHappyPtWindow(window);
    internal void ContinueHappyPt(long id) => _resolve().ContinueHappyPt(id);
    internal bool DrainHappyPt(ProgramSkillFrame f) => _resolve().DrainHappyPt(f);
    internal void FinishHappyPt(ProgramSkillFrame f) => _resolve().FinishHappyPt(f);
    internal bool HappyPangTongStructuralEdge(ResolutionFrame child, ResolutionFrame parent) => _resolve().HappyPangTongStructuralEdge(child, parent);
    internal PromptChoice HappyPtAi(PendingDecision p, ProgramSkillFrame f) => _resolve().HappyPtAi(p, f);
    internal ProgramSkillFrame HappyPtChildren(ProgramSkillFrame f, HappyPangTongStage stage) => _resolve().HappyPtChildren(f, stage);
    internal PromptChoice HappyPtChoice(ProgramSkillFrame f, string mode, string text, IReadOnlyList<int>? targets = default, string? value = default) => _resolve().HappyPtChoice(f, mode, text, targets, value);
    internal IReadOnlyList<PromptChoice> HappyPtChoices(ProgramSkillFrame f) => _resolve().HappyPtChoices(f);
    internal bool HappyPtDrawOutcomeProof(ProgramSkillFrame f) => _resolve().HappyPtDrawOutcomeProof(f);
    internal long[] HappyPtDue(int owner, ProgramSkillWindowContext context) => _resolve().HappyPtDue(owner, context);
    internal bool HappyPtIssuedPlanProof(HappyPangTongPlanIssuedEvent plan) => _resolve().HappyPtIssuedPlanProof(plan);
    internal bool HappyPtOriginalCandidate(ProgramSkillFrame f) => _resolve().HappyPtOriginalCandidate(f);
    internal IEnumerable<HappyPangTongPlanIssuedEvent> HappyPtOutstanding(int owner) => _resolve().HappyPtOutstanding(owner);
    internal HappyPangTongPlanIssuedEvent HappyPtPlan(long id) => _resolve().HappyPtPlan(id);
    internal bool HappyPtSettlementPaid(ProgramLifecycleTriggerWindowFrame window) => _resolve().HappyPtSettlementPaid(window);
    internal CardConversionSource HappyPtSource(ProgramSkillFrame f) => _resolve().HappyPtSource(f);
    internal string HappyPtUpperBind(ProgramSkillFrame f) => _resolve().HappyPtUpperBind(f);
    internal bool HappyPtUpperRidesOn(CardUseFrame use, ProgramSkillFrame root) => _resolve().HappyPtUpperRidesOn(use, root);
    internal bool IsHappyPangTongFactionOrigin(ProgramSkillFrame frame, int actor, int target, string? bind) => _resolve().IsHappyPangTongFactionOrigin(frame, actor, target, bind);
    internal bool IsHappyPtActualRoundStart(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().IsHappyPtActualRoundStart(candidate, context);
    internal bool KeepHappyPtPaid(ProgramSkillFrame f) => _resolve().KeepHappyPtPaid(f);
    internal void ObserveHappyPangTongTurnFacts(IGameEvent payload) => _resolve().ObserveHappyPangTongTurnFacts(payload);
    internal void PublishHappyPt(ProgramSkillFrame f) => _resolve().PublishHappyPt(f);
    internal bool ResumeHappyPt(long id) => _resolve().ResumeHappyPt(id);
    internal void ReturnHappyPtDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome, HappyPangTongStage from, HappyPangTongStage to) => _resolve().ReturnHappyPtDraw(supplied, outcome, from, to);
    internal void ReturnHappyPtLowerDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => _resolve().ReturnHappyPtLowerDraw(f, outcome);
    internal bool ReturnHappyPtMovement(ProgramSkillFrame f) => _resolve().ReturnHappyPtMovement(f);
    internal void ReturnHappyPtRewardDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => _resolve().ReturnHappyPtRewardDraw(f, outcome);
    internal void ReturnHappyPtSettled(long rootId, long planId) => _resolve().ReturnHappyPtSettled(rootId, planId);
    internal void ReturnHappyPtUpper(CardUseFrame completed) => _resolve().ReturnHappyPtUpper(completed);
    internal void ReturnHappyPtWindow(ProgramSkillFrame child, bool completed) => _resolve().ReturnHappyPtWindow(child, completed);
    internal void SettleHappyPt(ProgramSkillFrame supplied, bool success) => _resolve().SettleHappyPt(supplied, success);
    internal bool ValidHappyPt(ProgramSkillFrame f) => _resolve().ValidHappyPt(f);
    internal bool ValidateHappyPangTongFactionParent(NativeOwners.FactionCardRequestHandle request, ProgramSkillFrame frame) => _resolve().ValidateHappyPangTongFactionParent(request, frame);
    internal bool HasHappyPangTong { get => _resolve().HasHappyPangTong; }
}
