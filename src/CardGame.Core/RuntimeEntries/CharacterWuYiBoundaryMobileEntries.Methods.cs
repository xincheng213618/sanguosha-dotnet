#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWuYiBoundaryMobileEntries
{
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.BoundaryWuYiBenxiGrantedEvent> ActiveBoundaryWuYiGrants(int owner) => _resolve().ActiveBoundaryWuYiGrants(owner);
    internal bool AdmitIssuedBoundaryWuYi(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().AdmitIssuedBoundaryWuYi(c, context);
    internal void AssertBoundaryWuYiFrame(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertBoundaryWuYiFrame(f);
    internal void AssertBoundaryWuYiTargets(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertBoundaryWuYiTargets(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginBoundaryWuYiTargets(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BeginBoundaryWuYiTargets(f);
    internal bool BoundaryWuYiCostPaid(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BoundaryWuYiCostPaid(f);
    internal void BoundaryWuYiDistance(global::CardGame.Core.ProgramDistanceQuery query, global::System.Collections.Generic.List<global::CardGame.Core.RuleQueryBaseTerm> terms) => _resolve().BoundaryWuYiDistance(query, terms);
    internal global::CardGame.Core.BoundaryWuYiBenxiGrantedEvent? BoundaryWuYiIssuedGrant(long useId, long actionId, int owner, string instance) => _resolve().BoundaryWuYiIssuedGrant(useId, actionId, owner, instance);
    internal bool BoundaryWuYiNeedsFinalizedTrickWindow(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().BoundaryWuYiNeedsFinalizedTrickWindow(query);
    internal global::System.Collections.Generic.IReadOnlyList<global::System.Collections.Generic.IReadOnlyList<int>> BoundaryWuYiTargetCandidates(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.BoundaryWuYiTargetDraft draft) => _resolve().BoundaryWuYiTargetCandidates(use, draft);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> BoundaryWuYiTargetChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BoundaryWuYiTargetChoices(f);
    internal bool CanOfferBoundaryWuYiReward(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferBoundaryWuYiReward(c, context);
    internal bool CanOfferBoundaryWuYiTargets(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferBoundaryWuYiTargets(c, context);
    internal void CollectIssuedBoundaryWuYi(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.List<global::CardGame.Core.ProgramCardTriggerCandidate> result) => _resolve().CollectIssuedBoundaryWuYi(action, window, result);
    internal void ConsumeBoundaryWuYiNextUse(global::CardGame.Core.IGameEvent payload) => _resolve().ConsumeBoundaryWuYiNextUse(payload);
    internal global::CardGame.Core.SkillProgramStepOutcome ExecuteBoundaryWuYi(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().ExecuteBoundaryWuYi(supplied, op);
    internal bool IsBoundaryWuYiTargetFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.DesignatedExtraTargetResolvedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> preceding) => _resolve().IsBoundaryWuYiTargetFact(use, fact, preceding);
    internal bool KeepBoundaryWuYiIssuedFrame(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepBoundaryWuYiIssuedFrame(f);
    internal void PublishBoundaryWuYiTargets(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishBoundaryWuYiTargets(f);
    internal bool QualifyBoundaryWuYi(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().QualifyBoundaryWuYi(c, context);
    internal global::CardGame.Core.CardUseFrame RequireBoundaryWuYiUse(global::CardGame.Core.ProgramSkillFrame f, scoped out global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().RequireBoundaryWuYiUse(f, out window);
    internal void ResolveBoundaryWuYiTargetChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveBoundaryWuYiTargetChoice(selected);
    internal global::CardGame.Core.PromptChoice SelectAiBoundaryWuYiTargets(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiBoundaryWuYiTargets(decision, f);
    internal bool ValidBoundaryWuYiGrant(global::CardGame.Core.BoundaryWuYiBenxiGrantedEvent grant) => _resolve().ValidBoundaryWuYiGrant(grant);
    internal bool HasBoundaryWuYi { get => _resolve().HasBoundaryWuYi; }
}
