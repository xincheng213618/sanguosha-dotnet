#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ZhangChangPuProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterZhangChangPuOlRuntime>> getCharacterZhangChangPuOlRuntime) : IZhangChangPuProgramHost
{
    public void YanjiaoRevealTopCards(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterZhangChangPuOlRuntime().Value.YanjiaoRevealProgramTopCards(frame, effect);
    public SkillProgramStepOutcome YanjiaoSplitRevealedCards(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterZhangChangPuOlRuntime().Value.YanjiaoSplitRevealedCards(frame, effect);
    public void ShenShenDrawAndArmBonus(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterZhangChangPuOlRuntime().Value.ShenShenDrawAndArmBonus(frame, effect);
}
