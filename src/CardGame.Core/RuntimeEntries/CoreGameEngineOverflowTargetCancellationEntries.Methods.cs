#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOverflowTargetCancellationEntries
{
    internal SkillProgramStepOutcome BeginOverflowTargetCancellation(ProgramSkillFrame input) => _resolve().BeginOverflowTargetCancellation(input);
    internal bool CanContinueOverflowTargetCancellation(ProgramSkillFrame frame) => _resolve().CanContinueOverflowTargetCancellation(frame);
    internal bool CanOfferOverflowTargetCancellation(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferOverflowTargetCancellation(candidate, trigger, context);
    internal bool DrainOverflowTargetChildren(ProgramSkillFrame frame) => _resolve().DrainOverflowTargetChildren(frame);
    internal bool ExactOverflowTargetParent(ProgramSkillFrame frame, scoped out CardUseFrame use, scoped out ProgramCardTriggerWindowFrame window) => _resolve().ExactOverflowTargetParent(frame, out use, out window);
    internal bool HasOverflowTargetCancellation(CardUseFrame use) => _resolve().HasOverflowTargetCancellation(use);
    internal bool IsOverflowTargetCancellationPhysicalClaim(long useId, int cardId) => _resolve().IsOverflowTargetCancellationPhysicalClaim(useId, cardId);
    internal bool ResumeOverflowTargetCancellation(long id) => _resolve().ResumeOverflowTargetCancellation(id);
    internal bool ReturnOverflowTargetCancellation(ProgramSkillFrame frame) => _resolve().ReturnOverflowTargetCancellation(frame);
    internal bool TryCompleteOverflowCanceledUse(ProgramCardTriggerWindowFrame window) => _resolve().TryCompleteOverflowCanceledUse(window);
    internal bool TryGetOverflowTargetUse(int owner, ProgramSkillWindowContext context, scoped out CardUseFrame use, scoped out ProgramCardTriggerWindowFrame window) => _resolve().TryGetOverflowTargetUse(owner, context, out use, out window);
    internal long OverflowMovementSequence { get => _resolve().OverflowMovementSequence; }
}
