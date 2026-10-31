#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterJiangGanOlEntries
{
    internal bool CanStartDaoshuGuess(global::CardGame.Core.CharacterState owner) => _resolve().CanStartDaoshuGuess(owner);
    internal global::CardGame.Core.SkillProgramStepOutcome DaoshuProgramGuessAndTake(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().DaoshuProgramGuessAndTake(frame, effect);
    internal void PresentDaoshuGiveBackPrompt(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.Card[] eligible) => _resolve().PresentDaoshuGiveBackPrompt(frame, eligible);
    internal void PresentDaoshuSuitPrompt(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PresentDaoshuSuitPrompt(frame);
    internal void ResolveDaoshuGiveBackChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveDaoshuGiveBackChoice(selected);
    internal void ResolveDaoshuSuitChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveDaoshuSuitChoice(selected);
}
