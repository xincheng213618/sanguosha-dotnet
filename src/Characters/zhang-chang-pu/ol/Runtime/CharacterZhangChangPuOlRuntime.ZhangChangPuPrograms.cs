#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangChangPuOlRuntime
{
    void IZhangChangPuProgramHost.YanjiaoRevealTopCards(ProgramSkillFrame frame, SkillProgramEffect effect) => YanjiaoRevealProgramTopCards(frame, effect);
    SkillProgramStepOutcome IZhangChangPuProgramHost.YanjiaoSplitRevealedCards(ProgramSkillFrame frame, SkillProgramEffect effect) => YanjiaoSplitRevealedCards(frame, effect);
    void IZhangChangPuProgramHost.ShenShenDrawAndArmBonus(ProgramSkillFrame frame, SkillProgramEffect effect) => ShenShenDrawAndArmBonus(frame, effect);
}
