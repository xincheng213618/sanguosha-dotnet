#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAfterTurnEndedProgramsEntries
{
    internal void AdvanceAfterTurnEndedCandidate(DeferredTurnEndFrame parent, ProgramTriggerCandidate candidate, bool activated, bool completed) => _resolve().AdvanceAfterTurnEndedCandidate(parent, candidate, activated, completed);
    internal ProgramTriggerCandidate AfterTurnEndedCandidate(DeferredTurnEndFrame parent) => _resolve().AfterTurnEndedCandidate(parent);
    internal void AttachAfterTurnEndedChild(ProgramSkillFrame child, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().AttachAfterTurnEndedChild(child, candidate, context);
    internal bool CanRunAfterTurnEndedCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context, SkillProgramTrigger trigger) => _resolve().CanRunAfterTurnEndedCandidate(candidate, context, trigger);
    internal bool CompleteAfterTurnEndedBinding(DeferredTurnEndFrame parent, ProgramSkillFrame child, ProgramSkillWindowContext context) => _resolve().CompleteAfterTurnEndedBinding(parent, child, context);
    internal bool ContinueAfterTurnEndedPrograms(DeferredTurnEndFrame parent) => _resolve().ContinueAfterTurnEndedPrograms(parent);
    internal ProgramSkillWindowContext CreateAfterTurnEndedContext(DeferredTurnEndFrame parent, ProgramTriggerCandidate candidate) => _resolve().CreateAfterTurnEndedContext(parent, candidate);
    internal bool IsActualAfterTurnEndedParent(DeferredTurnEndFrame parent) => _resolve().IsActualAfterTurnEndedParent(parent);
    internal bool MatchesAfterTurnEndedChild(DeferredTurnEndFrame parent, ProgramSkillFrame child, ProgramSkillWindowContext context) => _resolve().MatchesAfterTurnEndedChild(parent, child, context);
    internal bool HasAfterTurnEndedPrograms { get => _resolve().HasAfterTurnEndedPrograms; }
}
