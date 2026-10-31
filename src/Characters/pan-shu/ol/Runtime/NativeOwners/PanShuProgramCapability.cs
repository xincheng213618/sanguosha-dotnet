#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class PanShuProgramCapability(Func<RuntimeEntries.CharacterPanShuOlEntries> getCharacterPanShuOlRuntime) : IPanShuProgramHost
{
    public SkillProgramStepOutcome ZhirenReorderTopByLength(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterPanShuOlRuntime().ZhirenProgramReorderTopByLength(frame, effect);
    public SkillProgramStepOutcome ZhirenResolveFieldTiers(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterPanShuOlRuntime().ZhirenProgramResolveFieldTiers(frame, effect);
    public SkillProgramStepOutcome YanerResolvePairBenefit(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterPanShuOlRuntime().YanerProgramResolvePairBenefit(frame, effect);
}
