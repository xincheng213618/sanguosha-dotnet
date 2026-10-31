#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDistinctFactionDiscardsEntries
{
    internal SkillProgramStepOutcome BeginDistinctFactionDiscards(ProgramSkillFrame f) => _resolve().BeginDistinctFactionDiscards(f);
    internal bool CanStartDistinctFactionDiscard(ProgramActivationLegalityContext gate) => _resolve().CanStartDistinctFactionDiscard(gate);
    internal void CancelDistinctFactionDiscards(ProgramSkillFrame f, string reason) => _resolve().CancelDistinctFactionDiscards(f, reason);
    internal void CancelOrSkipDistinctFactionPayment(ProgramSkillFrame f, ProgramDistinctFactionDiscardDraft d) => _resolve().CancelOrSkipDistinctFactionPayment(f, d);
    internal bool DistinctFactionOwnerValid(ProgramSkillFrame f) => _resolve().DistinctFactionOwnerValid(f);
    internal bool DistinctFactionPlanValid(int owner, IReadOnlyList<int> seats) => _resolve().DistinctFactionPlanValid(owner, seats);
    internal IReadOnlyList<int> FreezeDiscardParticipantOrder(int owner, IReadOnlyList<int> chosen) => _resolve().FreezeDiscardParticipantOrder(owner, chosen);
    internal bool HasPayableDistinctFactionOwnerHe(int seat, string skillId, string instanceId) => _resolve().HasPayableDistinctFactionOwnerHe(seat, skillId, instanceId);
    internal void PublishDistinctFactionPrompt(ProgramSkillFrame f, string text, IReadOnlyList<PromptChoice> choices) => _resolve().PublishDistinctFactionPrompt(f, text, choices);
    internal void PublishDistinctFactionSelection(ProgramSkillFrame f) => _resolve().PublishDistinctFactionSelection(f);
    internal void ResolveDistinctFactionDiscardChoice(PromptChoice choice) => _resolve().ResolveDistinctFactionDiscardChoice(choice);
    internal bool ResumeDistinctFactionDiscards(long id) => _resolve().ResumeDistinctFactionDiscards(id);
    internal PromptChoice SelectAiDistinctFactionDiscard(PendingDecision decision, ProgramSkillFrame f) => _resolve().SelectAiDistinctFactionDiscard(decision, f);
}
