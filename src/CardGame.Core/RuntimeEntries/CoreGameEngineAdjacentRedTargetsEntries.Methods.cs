#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAdjacentRedTargetsEntries
{
    internal IReadOnlyList<PromptChoice> AdjacentBorrowedSwordChoices(ProgramSkillFrame f, CardUseFrame use) => _resolve().AdjacentBorrowedSwordChoices(f, use);
    internal IEnumerable<int> AdjacentRedOriginalPrimaryTargets(CardUseFrame use) => _resolve().AdjacentRedOriginalPrimaryTargets(use);
    internal bool AdjacentRedTargetEligible(CardUseFrame use, int owner) => _resolve().AdjacentRedTargetEligible(use, owner);
    internal bool AdjacentRedTargetFinalizedAfterCategoryTargetWindow(ProgramTargetCardWindowQuery query) => _resolve().AdjacentRedTargetFinalizedAfterCategoryTargetWindow(query);
    internal void ApplyAdjacentRedTarget(ProgramSkillFrame f, IReadOnlyList<int> added) => _resolve().ApplyAdjacentRedTarget(f, added);
    internal SkillProgramStepOutcome BeginAdjacentRedTarget(ProgramSkillFrame supplied) => _resolve().BeginAdjacentRedTarget(supplied);
    internal bool HasAdjacentRedTargetTail(CardUseFrame use) => _resolve().HasAdjacentRedTargetTail(use);
    internal bool IsAdjacentRedTargetFact(CardUseFrame use, AdjacentRedTargetAddedEvent fact, IReadOnlyList<int> original, int? currentProofActor = default) => _resolve().IsAdjacentRedTargetFact(use, fact, original, currentProofActor);
    internal CardUseFrame RequireAdjacentRedParent(ProgramSkillFrame f, scoped out ProgramCardTriggerWindowFrame window) => _resolve().RequireAdjacentRedParent(f, out window);
    internal void ResolveAdjacentRedTargetChoice(PromptChoice selected) => _resolve().ResolveAdjacentRedTargetChoice(selected);
    internal bool TryGetAdjacentRedTargetUse(int owner, ProgramSkillWindowContext context, scoped out CardUseFrame use, scoped out ProgramCardTriggerWindowFrame window) => _resolve().TryGetAdjacentRedTargetUse(owner, context, out use, out window);
    internal bool HasAdjacentRedTargetCapability { get => _resolve().HasAdjacentRedTargetCapability; }
}
