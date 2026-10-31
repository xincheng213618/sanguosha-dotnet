#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWuYiBoundaryMobileEntries
{
    internal IEnumerable<BoundaryWuYiBenxiGrantedEvent> ActiveBoundaryWuYiGrants(int owner) => _resolve().ActiveBoundaryWuYiGrants(owner);
    internal bool AdmitIssuedBoundaryWuYi(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().AdmitIssuedBoundaryWuYi(c, context);
    internal void AssertBoundaryWuYiTargets(ProgramSkillFrame f) => _resolve().AssertBoundaryWuYiTargets(f);
    internal SkillProgramStepOutcome BeginBoundaryWuYiTargets(ProgramSkillFrame f) => _resolve().BeginBoundaryWuYiTargets(f);
    internal bool BoundaryWuYiCostPaid(ProgramSkillFrame f) => _resolve().BoundaryWuYiCostPaid(f);
    internal void BoundaryWuYiDistance(ProgramDistanceQuery query, List<RuleQueryBaseTerm> terms) => _resolve().BoundaryWuYiDistance(query, terms);
    internal BoundaryWuYiBenxiGrantedEvent? BoundaryWuYiIssuedGrant(long useId, long actionId, int owner, string instance) => _resolve().BoundaryWuYiIssuedGrant(useId, actionId, owner, instance);
    internal bool BoundaryWuYiNeedsFinalizedTrickWindow(ProgramTargetCardWindowQuery query) => _resolve().BoundaryWuYiNeedsFinalizedTrickWindow(query);
    internal IReadOnlyList<IReadOnlyList<int>> BoundaryWuYiTargetCandidates(CardUseFrame use, BoundaryWuYiTargetDraft draft) => _resolve().BoundaryWuYiTargetCandidates(use, draft);
    internal IReadOnlyList<PromptChoice> BoundaryWuYiTargetChoices(ProgramSkillFrame f) => _resolve().BoundaryWuYiTargetChoices(f);
    internal bool CanOfferBoundaryWuYiReward(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().CanOfferBoundaryWuYiReward(c, context);
    internal bool CanOfferBoundaryWuYiTargets(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().CanOfferBoundaryWuYiTargets(c, context);
    internal void CollectIssuedBoundaryWuYi(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result) => _resolve().CollectIssuedBoundaryWuYi(action, window, result);
    internal void ConsumeBoundaryWuYiNextUse(IGameEvent payload) => _resolve().ConsumeBoundaryWuYiNextUse(payload);
    internal SkillProgramStepOutcome ExecuteBoundaryWuYi(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().ExecuteBoundaryWuYi(supplied, op);
    internal bool IsBoundaryWuYiTargetFact(CardUseFrame use, DesignatedExtraTargetResolvedEvent fact, IReadOnlyList<int> preceding) => _resolve().IsBoundaryWuYiTargetFact(use, fact, preceding);
    internal bool KeepBoundaryWuYiIssuedFrame(ProgramSkillFrame f) => _resolve().KeepBoundaryWuYiIssuedFrame(f);
    internal void PublishBoundaryWuYiTargets(ProgramSkillFrame f) => _resolve().PublishBoundaryWuYiTargets(f);
    internal bool QualifyBoundaryWuYi(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().QualifyBoundaryWuYi(c, context);
    internal CardUseFrame RequireBoundaryWuYiUse(ProgramSkillFrame f, scoped out ProgramCardTriggerWindowFrame window) => _resolve().RequireBoundaryWuYiUse(f, out window);
    internal void ResolveBoundaryWuYiTargetChoice(PromptChoice selected) => _resolve().ResolveBoundaryWuYiTargetChoice(selected);
    internal PromptChoice SelectAiBoundaryWuYiTargets(PendingDecision decision, ProgramSkillFrame f) => _resolve().SelectAiBoundaryWuYiTargets(decision, f);
    internal bool ValidBoundaryWuYiGrant(BoundaryWuYiBenxiGrantedEvent grant) => _resolve().ValidBoundaryWuYiGrant(grant);
    internal bool HasBoundaryWuYi { get => _resolve().HasBoundaryWuYi; }
}
