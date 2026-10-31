#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOverflowTargetCancellationEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginOverflowTargetCancellation(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginOverflowTargetCancellation(input);
    internal bool CanContinueOverflowTargetCancellation(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CanContinueOverflowTargetCancellation(frame);
    internal bool CanOfferOverflowTargetCancellation(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferOverflowTargetCancellation(candidate, trigger, context);
    internal bool DrainOverflowTargetChildren(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().DrainOverflowTargetChildren(frame);
    internal bool ExactOverflowTargetParent(global::CardGame.Core.ProgramSkillFrame frame, scoped out global::CardGame.Core.CardUseFrame use, scoped out global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().ExactOverflowTargetParent(frame, out use, out window);
    internal bool HasOverflowTargetCancellation(global::CardGame.Core.CardUseFrame use) => _resolve().HasOverflowTargetCancellation(use);
    internal bool IsOverflowTargetCancellationAwaitedMovement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsOverflowTargetCancellationAwaitedMovement(frame, effect, pending);
    internal bool IsOverflowTargetCancellationPhysicalClaim(long useId, int cardId) => _resolve().IsOverflowTargetCancellationPhysicalClaim(useId, cardId);
    internal bool ResumeOverflowTargetCancellation(long id) => _resolve().ResumeOverflowTargetCancellation(id);
    internal bool ReturnOverflowTargetCancellation(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnOverflowTargetCancellation(frame);
    internal bool TryCompleteOverflowCanceledUse(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().TryCompleteOverflowCanceledUse(window);
    internal bool TryGetOverflowTargetUse(int owner, global::CardGame.Core.ProgramSkillWindowContext context, scoped out global::CardGame.Core.CardUseFrame use, scoped out global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().TryGetOverflowTargetUse(owner, context, out use, out window);
    internal long OverflowMovementSequence { get => _resolve().OverflowMovementSequence; }
}
