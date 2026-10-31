#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFinalTargetTurnCountsEntries
{
    internal bool CanOfferNonFinalTargetPayment(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferNonFinalTargetPayment(candidate, context);
    internal global::CardGame.Core.SkillProgramStepOutcome ExecuteFinalTargetTurnCount(global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ExecuteFinalTargetTurnCount(effect, frame);
    internal int GetNamedTurnSkillCount(int seat, string skill) => _resolve().GetNamedTurnSkillCount(seat, skill);
    internal bool NamedTurnActionAlreadyCommitted(int seat, string skill, long actionId) => _resolve().NamedTurnActionAlreadyCommitted(seat, skill, actionId);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> NonFinalTargetChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().NonFinalTargetChoices(frame);
    internal global::CardGame.Core.CardUseFrame NonFinalTargetUse(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().NonFinalTargetUse(frame);
    internal void PublishNamedTurnFlow(global::CardGame.Core.ProgramSkillFrame frame, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> choices, string message, bool privateChoice) => _resolve().PublishNamedTurnFlow(frame, choices, message, privateChoice);
    internal void PublishNamedTurnHandDiscard(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishNamedTurnHandDiscard(frame);
    internal void ResolveNamedTurnFlowChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveNamedTurnFlowChoice(selected);
    internal bool ResumeNamedTurnFlow(long frameId) => _resolve().ResumeNamedTurnFlow(frameId);
}
