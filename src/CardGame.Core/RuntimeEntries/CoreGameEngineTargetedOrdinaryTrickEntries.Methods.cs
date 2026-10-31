#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTargetedOrdinaryTrickEntries
{
    internal SkillProgramStepOutcome BeginTargetedOrdinaryTrick(ProgramSkillFrame supplied) => _resolve().BeginTargetedOrdinaryTrick(supplied);
    internal bool CanAddTargetedTrickTarget(CardUseFrame use, CharacterState target, bool removeTargets) => _resolve().CanAddTargetedTrickTarget(use, target, removeTargets);
    internal bool CanOfferTargetedOrdinaryTrick(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferTargetedOrdinaryTrick(candidate, trigger, context);
    internal bool HasTargetedOrdinaryTrickTail(CardUseFrame use) => _resolve().HasTargetedOrdinaryTrickTail(use);
    internal bool IsTargetedOrdinaryTrickResolvedFact(CardUseFrame use, TargetedOrdinaryTrickResolvedEvent fact, IReadOnlyList<int> before) => _resolve().IsTargetedOrdinaryTrickResolvedFact(use, fact, before);
    internal CardUseFrame RequireTargetedOrdinaryTrickParent(ProgramSkillFrame f, scoped out ProgramCardTriggerWindowFrame window) => _resolve().RequireTargetedOrdinaryTrickParent(f, out window);
    internal void ResolveTargetedOrdinaryTrickChoice(PromptChoice selected) => _resolve().ResolveTargetedOrdinaryTrickChoice(selected);
    internal PromptChoice SelectAiTargetedOrdinaryTrick(PendingDecision p, ProgramSkillFrame f) => _resolve().SelectAiTargetedOrdinaryTrick(p, f);
    internal bool TargetedOrdinaryTrickFinalizedAfterCategoryTargetWindow(ProgramTargetCardWindowQuery query) => _resolve().TargetedOrdinaryTrickFinalizedAfterCategoryTargetWindow(query);
    internal bool TargetedOrdinaryTrickNormalizeImplicitDrawTwoTargetWindow(ProgramTargetCardWindowQuery query) => _resolve().TargetedOrdinaryTrickNormalizeImplicitDrawTwoTargetWindow(query);
    internal IReadOnlyList<PromptChoice> TargetedTrickChoices(long frameId, CardUseFrame use, bool removeTargets = false) => _resolve().TargetedTrickChoices(frameId, use, removeTargets);
    internal bool TargetedTrickEffectIsEffective(CardUseFrame use, int seat) => _resolve().TargetedTrickEffectIsEffective(use, seat);
    internal int[] TargetedTrickPrimaryTargets(CardUseFrame use) => _resolve().TargetedTrickPrimaryTargets(use);
    internal bool TargetedTrickRemovesTargets(ProgramSkillFrame frame) => _resolve().TargetedTrickRemovesTargets(frame);
    internal bool TryGetTargetedOrdinaryTrickUse(ProgramSkillWindowContext context, scoped out CardUseFrame use, scoped out ProgramCardTriggerWindowFrame window) => _resolve().TryGetTargetedOrdinaryTrickUse(context, out use, out window);
    internal bool HasTargetedOrdinaryTrickAdjustment { get => _resolve().HasTargetedOrdinaryTrickAdjustment; }
}
