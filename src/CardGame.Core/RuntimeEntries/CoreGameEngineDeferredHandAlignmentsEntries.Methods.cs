#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDeferredHandAlignmentsEntries
{
    internal void BeginDeferredAlignmentMovement(long frameId) => _resolve().BeginDeferredAlignmentMovement(frameId);
    internal void CleanupDeferredHandAlignments() => _resolve().CleanupDeferredHandAlignments();
    internal void CompleteDeferredAlignmentDiscard(long frameId, IReadOnlyList<int> ids, IReadOnlyList<CardLocation> locations) => _resolve().CompleteDeferredAlignmentDiscard(frameId, ids, locations);
    internal bool CompleteDeferredTurnEndBinding(ProgramSkillFrame frame, ProgramSkillWindowContext context) => _resolve().CompleteDeferredTurnEndBinding(frame, context);
    internal void ContinueDeferredTurnEnd() => _resolve().ContinueDeferredTurnEnd();
    internal void ContinueDeferredTurnEndCore() => _resolve().ContinueDeferredTurnEndCore();
    internal SkillProgramEffect DeferredOwnedSelectionEffect(ProgramSkillFrame frame, SkillProgramEffect original) => _resolve().DeferredOwnedSelectionEffect(frame, original);
    internal DeferredHandAlignment RequireDeferredAlignmentParent(ProgramSkillFrame frame) => _resolve().RequireDeferredAlignmentParent(frame);
    internal SkillProgramStepOutcome ResolveDeferredHandAlignment(ProgramSkillFrame frame) => _resolve().ResolveDeferredHandAlignment(frame);
    internal void ScheduleDeferredHandAlignment(ProgramSkillFrame frame, string continuationId) => _resolve().ScheduleDeferredHandAlignment(frame, continuationId);
    internal bool TryBeginDeferredTurnEnd(CharacterState previous, bool skipped) => _resolve().TryBeginDeferredTurnEnd(previous, skipped);
}
