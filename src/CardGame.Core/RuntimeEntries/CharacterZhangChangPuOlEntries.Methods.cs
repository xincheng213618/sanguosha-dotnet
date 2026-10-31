#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangChangPuOlEntries
{
    internal void BindYanjiaoSubset(ProgramSkillFrame frame, SkillProgramEffect effect, ProgramSkillCardSetBinding source, string bind, IReadOnlyList<int> cardIds, int chooserSeat) => _resolve().BindYanjiaoSubset(frame, effect, source, bind, cardIds, chooserSeat);
    internal void PresentYanjiaoGroupPrompt(ProgramSkillFrame frame, SkillProgramEffect effect, ProgramSkillCardSetBinding source, List<GameEngine.YanjiaoCandidate> cards, int chooserSeat, string stage, int[] masks) => _resolve().PresentYanjiaoGroupPrompt(frame, effect, source, cards, chooserSeat, stage, masks);
    internal List<GameEngine.YanjiaoCandidate> ReadYanjiaoBoundCards(ProgramSkillCardSetBinding source) => _resolve().ReadYanjiaoBoundCards(source);
    internal void ResolveYanjiaoSplitChoice(PromptChoice selected) => _resolve().ResolveYanjiaoSplitChoice(selected);
    internal void ShenShenDrawAndArmBonus(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().ShenShenDrawAndArmBonus(frame, effect);
    internal void YanjiaoApplyLeftoverPenalty(ProgramSkillFrame frame, int leftoverCount) => _resolve().YanjiaoApplyLeftoverPenalty(frame, leftoverCount);
    internal void YanjiaoRevealProgramTopCards(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().YanjiaoRevealProgramTopCards(frame, effect);
    internal SkillProgramStepOutcome YanjiaoSplitRevealedCards(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().YanjiaoSplitRevealedCards(frame, effect);
}
