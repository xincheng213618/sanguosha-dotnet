#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ZhangChangPuProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhangChangPuOlEntries> getCharacterZhangChangPuOlRuntime) : IZhangChangPuProgramHost
{
    public void YanjiaoRevealTopCards(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterZhangChangPuOlRuntime().YanjiaoRevealProgramTopCards(frame, effect);
    public SkillProgramStepOutcome YanjiaoSplitRevealedCards(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterZhangChangPuOlRuntime().YanjiaoSplitRevealedCards(frame, effect);
    public void ShenShenDrawAndArmBonus(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterZhangChangPuOlRuntime().ShenShenDrawAndArmBonus(frame, effect);
}
