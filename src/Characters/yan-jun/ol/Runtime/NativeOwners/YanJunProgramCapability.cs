#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class YanJunProgramCapability(Func<RuntimeEntries.CharacterYanJunOlEntries> getCharacterYanJunOlRuntime) : IYanJunProgramHost
{
    public SkillProgramStepOutcome GuanchaoChoosePattern(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterYanJunOlRuntime().GuanchaoProgramChoosePattern(frame, effect);
    public SkillProgramStepOutcome GuanchaoRankDraw(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterYanJunOlRuntime().GuanchaoProgramRankDraw(frame, effect);
    public SkillProgramStepOutcome XunxianGiftUsedCard(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterYanJunOlRuntime().XunxianProgramGiftUsedCard(frame, effect);
}
