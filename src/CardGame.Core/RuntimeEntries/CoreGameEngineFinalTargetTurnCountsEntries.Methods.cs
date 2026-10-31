#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFinalTargetTurnCountsEntries
{
    internal bool CanOfferNonFinalTargetPayment(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().CanOfferNonFinalTargetPayment(candidate, context);
    internal SkillProgramStepOutcome ExecuteFinalTargetTurnCount(SkillProgramEffect effect, ProgramSkillFrame frame) => _resolve().ExecuteFinalTargetTurnCount(effect, frame);
    internal int GetNamedTurnSkillCount(int seat, string skill) => _resolve().GetNamedTurnSkillCount(seat, skill);
    internal bool NamedTurnActionAlreadyCommitted(int seat, string skill, long actionId) => _resolve().NamedTurnActionAlreadyCommitted(seat, skill, actionId);
    internal IReadOnlyList<PromptChoice> NonFinalTargetChoices(ProgramSkillFrame frame) => _resolve().NonFinalTargetChoices(frame);
    internal CardUseFrame NonFinalTargetUse(ProgramSkillFrame frame) => _resolve().NonFinalTargetUse(frame);
    internal void PublishNamedTurnFlow(ProgramSkillFrame frame, IReadOnlyList<PromptChoice> choices, string message, bool privateChoice) => _resolve().PublishNamedTurnFlow(frame, choices, message, privateChoice);
    internal void PublishNamedTurnHandDiscard(ProgramSkillFrame frame) => _resolve().PublishNamedTurnHandDiscard(frame);
    internal void ResolveNamedTurnFlowChoice(PromptChoice selected) => _resolve().ResolveNamedTurnFlowChoice(selected);
    internal bool ResumeNamedTurnFlow(long frameId) => _resolve().ResumeNamedTurnFlow(frameId);
}
