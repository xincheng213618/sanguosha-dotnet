#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDeferredHandAlignmentsEntries
{
    internal void BeginDeferredAlignmentMovement(long frameId) => _resolve().BeginDeferredAlignmentMovement(frameId);
    internal void CleanupDeferredHandAlignments() => _resolve().CleanupDeferredHandAlignments();
    internal void CompleteDeferredAlignmentDiscard(long frameId, global::System.Collections.Generic.IReadOnlyList<int> ids, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardLocation> locations) => _resolve().CompleteDeferredAlignmentDiscard(frameId, ids, locations);
    internal bool CompleteDeferredTurnEndBinding(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CompleteDeferredTurnEndBinding(frame, context);
    internal void ContinueDeferredTurnEnd() => _resolve().ContinueDeferredTurnEnd();
    internal void ContinueDeferredTurnEndCore() => _resolve().ContinueDeferredTurnEndCore();
    internal global::CardGame.Core.SkillProgramEffect DeferredOwnedSelectionEffect(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect original) => _resolve().DeferredOwnedSelectionEffect(frame, original);
    internal bool HasDeferredTurnEndBoundaryFrame() => _resolve().HasDeferredTurnEndBoundaryFrame();
    internal bool IsExactDeferredChild(global::CardGame.Core.ProgramSkillFrame child) => _resolve().IsExactDeferredChild(child);
    internal global::CardGame.Core.DeferredHandAlignment RequireDeferredAlignmentParent(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().RequireDeferredAlignmentParent(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveDeferredHandAlignment(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ResolveDeferredHandAlignment(frame);
    internal void ScheduleDeferredHandAlignment(global::CardGame.Core.ProgramSkillFrame frame, string continuationId) => _resolve().ScheduleDeferredHandAlignment(frame, continuationId);
    internal bool TryBeginDeferredTurnEnd(global::CardGame.Core.CharacterState previous, bool skipped) => _resolve().TryBeginDeferredTurnEnd(previous, skipped);
}
