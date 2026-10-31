#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangChangPuOlEntries
{
    internal void BindYanjiaoSubset(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.ProgramSkillCardSetBinding source, string bind, global::System.Collections.Generic.IReadOnlyList<int> cardIds, int chooserSeat) => _resolve().BindYanjiaoSubset(frame, effect, source, bind, cardIds, chooserSeat);
    internal void PresentYanjiaoGroupPrompt(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.ProgramSkillCardSetBinding source, global::System.Collections.Generic.List<global::CardGame.Core.GameEngine.YanjiaoCandidate> cards, int chooserSeat, string stage, int[] masks) => _resolve().PresentYanjiaoGroupPrompt(frame, effect, source, cards, chooserSeat, stage, masks);
    internal global::System.Collections.Generic.List<global::CardGame.Core.GameEngine.YanjiaoCandidate> ReadYanjiaoBoundCards(global::CardGame.Core.ProgramSkillCardSetBinding source) => _resolve().ReadYanjiaoBoundCards(source);
    internal void ResolveYanjiaoSplitChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveYanjiaoSplitChoice(selected);
    internal void ShenShenDrawAndArmBonus(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().ShenShenDrawAndArmBonus(frame, effect);
    internal void YanjiaoApplyLeftoverPenalty(global::CardGame.Core.ProgramSkillFrame frame, int leftoverCount) => _resolve().YanjiaoApplyLeftoverPenalty(frame, leftoverCount);
    internal void YanjiaoRevealProgramTopCards(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().YanjiaoRevealProgramTopCards(frame, effect);
    internal global::CardGame.Core.SkillProgramStepOutcome YanjiaoSplitRevealedCards(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().YanjiaoSplitRevealedCards(frame, effect);
}
