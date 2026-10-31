#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramDiscardChallengesEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome ExecuteDiscardChallenge(global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.ProgramSkillFrame frame, int targetSeat) => _resolve().ExecuteDiscardChallenge(effect, frame, targetSeat);
    internal void PublishDiscardChallenge(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().PublishDiscardChallenge(frame, effect);
    internal void ResolveProgramDiscardChallengeChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveProgramDiscardChallengeChoice(choice);
    internal global::CardGame.Core.PromptChoice SelectAiProgramDiscardChallenge(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SelectAiProgramDiscardChallenge(decision, frame);
    internal void ValidateProgramDiscardChallengeState(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidateProgramDiscardChallengeState(frame);
}
