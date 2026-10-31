#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAdjacentRedTargetsEntries
{
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> AdjacentBorrowedSwordChoices(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardUseFrame use) => _resolve().AdjacentBorrowedSwordChoices(f, use);
    internal global::System.Collections.Generic.IEnumerable<int> AdjacentRedOriginalPrimaryTargets(global::CardGame.Core.CardUseFrame use) => _resolve().AdjacentRedOriginalPrimaryTargets(use);
    internal bool AdjacentRedTargetEligible(global::CardGame.Core.CardUseFrame use, int owner) => _resolve().AdjacentRedTargetEligible(use, owner);
    internal bool AdjacentRedTargetFinalizedAfterCategoryTargetWindow(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().AdjacentRedTargetFinalizedAfterCategoryTargetWindow(query);
    internal void ApplyAdjacentRedTarget(global::CardGame.Core.ProgramSkillFrame f, global::System.Collections.Generic.IReadOnlyList<int> added) => _resolve().ApplyAdjacentRedTarget(f, added);
    internal void AssertAdjacentRedTargetDraft(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertAdjacentRedTargetDraft(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginAdjacentRedTarget(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginAdjacentRedTarget(supplied);
    internal bool HasAdjacentRedTargetTail(global::CardGame.Core.CardUseFrame use) => _resolve().HasAdjacentRedTargetTail(use);
    internal bool IsAdjacentRedTargetFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.AdjacentRedTargetAddedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> original, int? currentProofActor = default) => _resolve().IsAdjacentRedTargetFact(use, fact, original, currentProofActor);
    internal global::CardGame.Core.CardUseFrame RequireAdjacentRedParent(global::CardGame.Core.ProgramSkillFrame f, scoped out global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().RequireAdjacentRedParent(f, out window);
    internal void ResolveAdjacentRedTargetChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveAdjacentRedTargetChoice(selected);
    internal bool TryGetAdjacentRedTargetUse(int owner, global::CardGame.Core.ProgramSkillWindowContext context, scoped out global::CardGame.Core.CardUseFrame use, scoped out global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().TryGetAdjacentRedTargetUse(owner, context, out use, out window);
    internal bool HasAdjacentRedTargetCapability { get => _resolve().HasAdjacentRedTargetCapability; }
}
