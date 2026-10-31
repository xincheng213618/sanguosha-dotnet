#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDistinctFactionDiscardsEntries
{
    internal void AssertDistinctFactionDiscardDraft(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect paused) => _resolve().AssertDistinctFactionDiscardDraft(f, paused);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginDistinctFactionDiscards(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BeginDistinctFactionDiscards(f);
    internal bool CanStartDistinctFactionDiscard(global::CardGame.Core.ProgramActivationLegalityContext gate) => _resolve().CanStartDistinctFactionDiscard(gate);
    internal void CancelDistinctFactionDiscards(global::CardGame.Core.ProgramSkillFrame f, string reason) => _resolve().CancelDistinctFactionDiscards(f, reason);
    internal void CancelOrSkipDistinctFactionPayment(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramDistinctFactionDiscardDraft d) => _resolve().CancelOrSkipDistinctFactionPayment(f, d);
    internal bool DistinctFactionOwnerValid(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DistinctFactionOwnerValid(f);
    internal bool DistinctFactionPlanValid(int owner, global::System.Collections.Generic.IReadOnlyList<int> seats) => _resolve().DistinctFactionPlanValid(owner, seats);
    internal global::System.Collections.Generic.IReadOnlyList<int> FreezeDiscardParticipantOrder(int owner, global::System.Collections.Generic.IReadOnlyList<int> chosen) => _resolve().FreezeDiscardParticipantOrder(owner, chosen);
    internal bool HasPayableDistinctFactionOwnerHe(int seat, string skillId, string instanceId) => _resolve().HasPayableDistinctFactionOwnerHe(seat, skillId, instanceId);
    internal void PublishDistinctFactionPrompt(global::CardGame.Core.ProgramSkillFrame f, string text, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> choices) => _resolve().PublishDistinctFactionPrompt(f, text, choices);
    internal void PublishDistinctFactionSelection(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishDistinctFactionSelection(f);
    internal void ResolveDistinctFactionDiscardChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveDistinctFactionDiscardChoice(choice);
    internal bool ResumeDistinctFactionDiscards(long id) => _resolve().ResumeDistinctFactionDiscards(id);
    internal global::CardGame.Core.PromptChoice SelectAiDistinctFactionDiscard(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiDistinctFactionDiscard(decision, f);
}
