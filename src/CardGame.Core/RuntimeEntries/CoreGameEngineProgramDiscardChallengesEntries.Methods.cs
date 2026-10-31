#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramDiscardChallengesEntries
{
    internal SkillProgramStepOutcome ExecuteDiscardChallenge(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat) => _resolve().ExecuteDiscardChallenge(effect, frame, targetSeat);
    internal void PublishDiscardChallenge(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().PublishDiscardChallenge(frame, effect);
    internal void ResolveProgramDiscardChallengeChoice(PromptChoice choice) => _resolve().ResolveProgramDiscardChallengeChoice(choice);
    internal PromptChoice SelectAiProgramDiscardChallenge(PendingDecision decision, ProgramSkillFrame frame) => _resolve().SelectAiProgramDiscardChallenge(decision, frame);
}
