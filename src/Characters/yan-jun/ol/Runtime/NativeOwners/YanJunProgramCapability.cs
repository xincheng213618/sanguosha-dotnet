#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class YanJunProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterYanJunOlEntries> getCharacterYanJunOlRuntime) : IYanJunProgramHost
{
    public SkillProgramStepOutcome GuanchaoChoosePattern(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterYanJunOlRuntime().GuanchaoProgramChoosePattern(frame, effect);
    public SkillProgramStepOutcome GuanchaoRankDraw(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterYanJunOlRuntime().GuanchaoProgramRankDraw(frame, effect);
    public SkillProgramStepOutcome XunxianGiftUsedCard(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterYanJunOlRuntime().XunxianProgramGiftUsedCard(frame, effect);
}
