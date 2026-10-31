#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterPangTongHappyGodEntries
{
    internal void AnswerHappyPt(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerHappyPt(choice);
    internal void AssertHappyPtUpperReturn(global::CardGame.Core.CardUseFrame use) => _resolve().AssertHappyPtUpperReturn(use);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginHappyPangTong(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp operation) => _resolve().BeginHappyPangTong(supplied, operation);
    internal void BeginHappyPtReward(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BeginHappyPtReward(f);
    internal bool BeginHappyPtSettled(global::CardGame.Core.ProgramSkillFrame root, bool success) => _resolve().BeginHappyPtSettled(root, success);
    internal void BindHappyPtUpper(global::CardGame.Core.IGameEvent payload) => _resolve().BindHappyPtUpper(payload);
    internal bool CanOfferHappyPangTong(global::CardGame.Core.ProgramTriggerLegalityContext g) => _resolve().CanOfferHappyPangTong(g);
    internal bool CanRunHappyPangTongSettled(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunHappyPangTongSettled(candidate, context);
    internal bool CompleteHappyPtWindow(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window) => _resolve().CompleteHappyPtWindow(window);
    internal void ContinueHappyPt(long id) => _resolve().ContinueHappyPt(id);
    internal bool DrainHappyPt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainHappyPt(f);
    internal void FinishHappyPt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishHappyPt(f);
    internal bool HappyPangTongStructuralEdge(global::CardGame.Core.ResolutionFrame child, global::CardGame.Core.ResolutionFrame parent) => _resolve().HappyPangTongStructuralEdge(child, parent);
    internal global::CardGame.Core.PromptChoice HappyPtAi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().HappyPtAi(p, f);
    internal global::CardGame.Core.ProgramSkillFrame HappyPtChildren(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.HappyPangTongStage stage) => _resolve().HappyPtChildren(f, stage);
    internal global::CardGame.Core.PromptChoice HappyPtChoice(global::CardGame.Core.ProgramSkillFrame f, string mode, string text, global::System.Collections.Generic.IReadOnlyList<int>? targets = default, string? value = default) => _resolve().HappyPtChoice(f, mode, text, targets, value);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> HappyPtChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HappyPtChoices(f);
    internal bool HappyPtDrawOutcomeProof(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HappyPtDrawOutcomeProof(f);
    internal long[] HappyPtDue(int owner, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().HappyPtDue(owner, context);
    internal bool HappyPtIssuedPlanProof(global::CardGame.Core.HappyPangTongPlanIssuedEvent plan) => _resolve().HappyPtIssuedPlanProof(plan);
    internal bool HappyPtOriginalCandidate(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HappyPtOriginalCandidate(f);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.HappyPangTongPlanIssuedEvent> HappyPtOutstanding(int owner) => _resolve().HappyPtOutstanding(owner);
    internal global::CardGame.Core.HappyPangTongPlanIssuedEvent HappyPtPlan(long id) => _resolve().HappyPtPlan(id);
    internal bool HappyPtSettlementPaid(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window) => _resolve().HappyPtSettlementPaid(window);
    internal global::CardGame.Core.CardConversionSource HappyPtSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HappyPtSource(f);
    internal string HappyPtUpperBind(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HappyPtUpperBind(f);
    internal bool HappyPtUpperRidesOn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSkillFrame root) => _resolve().HappyPtUpperRidesOn(use, root);
    internal bool IsHappyPangTongFactionOrigin(global::CardGame.Core.ProgramSkillFrame frame, int actor, int target, string? bind) => _resolve().IsHappyPangTongFactionOrigin(frame, actor, target, bind);
    internal bool IsHappyPtActualRoundStart(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsHappyPtActualRoundStart(candidate, context);
    internal bool KeepHappyPtPaid(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepHappyPtPaid(f);
    internal void ObserveHappyPangTongTurnFacts(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveHappyPangTongTurnFacts(payload);
    internal void PublishHappyPt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishHappyPt(f);
    internal bool ResumeHappyPt(long id) => _resolve().ResumeHappyPt(id);
    internal void ReturnHappyPtDraw(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.NativeDrawOutcome outcome, global::CardGame.Core.HappyPangTongStage from, global::CardGame.Core.HappyPangTongStage to) => _resolve().ReturnHappyPtDraw(supplied, outcome, from, to);
    internal void ReturnHappyPtLowerDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnHappyPtLowerDraw(f, outcome);
    internal bool ReturnHappyPtMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnHappyPtMovement(f);
    internal void ReturnHappyPtRewardDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnHappyPtRewardDraw(f, outcome);
    internal void ReturnHappyPtSettled(long rootId, long planId) => _resolve().ReturnHappyPtSettled(rootId, planId);
    internal void ReturnHappyPtUpper(global::CardGame.Core.CardUseFrame completed) => _resolve().ReturnHappyPtUpper(completed);
    internal void ReturnHappyPtWindow(global::CardGame.Core.ProgramSkillFrame child, bool completed) => _resolve().ReturnHappyPtWindow(child, completed);
    internal void SettleHappyPt(global::CardGame.Core.ProgramSkillFrame supplied, bool success) => _resolve().SettleHappyPt(supplied, success);
    internal bool ValidHappyPt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidHappyPt(f);
    internal bool ValidateHappyPangTongFactionParent(global::CardGame.Core.NativeOwners.FactionCardRequestHandle request, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidateHappyPangTongFactionParent(request, frame);
    internal bool HasHappyPangTong { get => _resolve().HasHappyPangTong; }
}
