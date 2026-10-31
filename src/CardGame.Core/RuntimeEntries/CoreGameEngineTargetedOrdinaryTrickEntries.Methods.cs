#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTargetedOrdinaryTrickEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginTargetedOrdinaryTrick(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginTargetedOrdinaryTrick(supplied);
    internal bool CanAddTargetedTrickTarget(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CharacterState target, bool removeTargets) => _resolve().CanAddTargetedTrickTarget(use, target, removeTargets);
    internal bool CanOfferTargetedOrdinaryTrick(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferTargetedOrdinaryTrick(candidate, trigger, context);
    internal bool HasTargetedOrdinaryTrickTail(global::CardGame.Core.CardUseFrame use) => _resolve().HasTargetedOrdinaryTrickTail(use);
    internal bool IsTargetedOrdinaryTrickResolvedFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.TargetedOrdinaryTrickResolvedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> before) => _resolve().IsTargetedOrdinaryTrickResolvedFact(use, fact, before);
    internal global::CardGame.Core.CardUseFrame RequireTargetedOrdinaryTrickParent(global::CardGame.Core.ProgramSkillFrame f, scoped out global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().RequireTargetedOrdinaryTrickParent(f, out window);
    internal void ResolveTargetedOrdinaryTrickChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveTargetedOrdinaryTrickChoice(selected);
    internal global::CardGame.Core.PromptChoice SelectAiTargetedOrdinaryTrick(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiTargetedOrdinaryTrick(p, f);
    internal bool TargetedOrdinaryTrickFinalizedAfterCategoryTargetWindow(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().TargetedOrdinaryTrickFinalizedAfterCategoryTargetWindow(query);
    internal bool TargetedOrdinaryTrickNormalizeImplicitDrawTwoTargetWindow(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().TargetedOrdinaryTrickNormalizeImplicitDrawTwoTargetWindow(query);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> TargetedTrickChoices(long frameId, global::CardGame.Core.CardUseFrame use, bool removeTargets = false) => _resolve().TargetedTrickChoices(frameId, use, removeTargets);
    internal bool TargetedTrickEffectIsEffective(global::CardGame.Core.CardUseFrame use, int seat) => _resolve().TargetedTrickEffectIsEffective(use, seat);
    internal int[] TargetedTrickPrimaryTargets(global::CardGame.Core.CardUseFrame use) => _resolve().TargetedTrickPrimaryTargets(use);
    internal bool TargetedTrickRemovesTargets(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().TargetedTrickRemovesTargets(frame);
    internal bool TryGetTargetedOrdinaryTrickUse(global::CardGame.Core.ProgramSkillWindowContext context, scoped out global::CardGame.Core.CardUseFrame use, scoped out global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().TryGetTargetedOrdinaryTrickUse(context, out use, out window);
    internal bool HasTargetedOrdinaryTrickAdjustment { get => _resolve().HasTargetedOrdinaryTrickAdjustment; }
}
