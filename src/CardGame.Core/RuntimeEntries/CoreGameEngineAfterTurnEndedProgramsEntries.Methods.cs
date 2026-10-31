#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAfterTurnEndedProgramsEntries
{
    internal void AdvanceAfterTurnEndedCandidate(global::CardGame.Core.DeferredTurnEndFrame parent, global::CardGame.Core.ProgramTriggerCandidate candidate, bool activated, bool completed) => _resolve().AdvanceAfterTurnEndedCandidate(parent, candidate, activated, completed);
    internal global::CardGame.Core.ProgramTriggerCandidate AfterTurnEndedCandidate(global::CardGame.Core.DeferredTurnEndFrame parent) => _resolve().AfterTurnEndedCandidate(parent);
    internal void AttachAfterTurnEndedChild(global::CardGame.Core.ProgramSkillFrame child, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().AttachAfterTurnEndedChild(child, candidate, context);
    internal bool CanRunAfterTurnEndedCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().CanRunAfterTurnEndedCandidate(candidate, context, trigger);
    internal bool CompleteAfterTurnEndedBinding(global::CardGame.Core.DeferredTurnEndFrame parent, global::CardGame.Core.ProgramSkillFrame child, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CompleteAfterTurnEndedBinding(parent, child, context);
    internal bool ContinueAfterTurnEndedPrograms(global::CardGame.Core.DeferredTurnEndFrame parent) => _resolve().ContinueAfterTurnEndedPrograms(parent);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateAfterTurnEndedContext(global::CardGame.Core.DeferredTurnEndFrame parent, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().CreateAfterTurnEndedContext(parent, candidate);
    internal bool IsActualAfterTurnEndedParent(global::CardGame.Core.DeferredTurnEndFrame parent) => _resolve().IsActualAfterTurnEndedParent(parent);
    internal bool MatchesAfterTurnEndedChild(global::CardGame.Core.DeferredTurnEndFrame parent, global::CardGame.Core.ProgramSkillFrame child, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().MatchesAfterTurnEndedChild(parent, child, context);
    internal bool HasAfterTurnEndedPrograms { get => _resolve().HasAfterTurnEndedPrograms; }
}
