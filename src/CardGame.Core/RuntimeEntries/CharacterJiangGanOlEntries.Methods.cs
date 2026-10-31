#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterJiangGanOlEntries
{
    internal bool CanStartDaoshuGuess(CharacterState owner) => _resolve().CanStartDaoshuGuess(owner);
    internal SkillProgramStepOutcome DaoshuProgramGuessAndTake(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().DaoshuProgramGuessAndTake(frame, effect);
    internal void PresentDaoshuGiveBackPrompt(ProgramSkillFrame frame, Card[] eligible) => _resolve().PresentDaoshuGiveBackPrompt(frame, eligible);
    internal void PresentDaoshuSuitPrompt(ProgramSkillFrame frame) => _resolve().PresentDaoshuSuitPrompt(frame);
    internal void ResolveDaoshuGiveBackChoice(PromptChoice selected) => _resolve().ResolveDaoshuGiveBackChoice(selected);
    internal void ResolveDaoshuSuitChoice(PromptChoice selected) => _resolve().ResolveDaoshuSuitChoice(selected);
}
