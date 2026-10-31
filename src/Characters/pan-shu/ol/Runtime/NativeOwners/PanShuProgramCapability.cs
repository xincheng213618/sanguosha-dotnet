#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class PanShuProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterPanShuOlRuntime>> getCharacterPanShuOlRuntime) : IPanShuProgramHost
{
    public SkillProgramStepOutcome ZhirenReorderTopByLength(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterPanShuOlRuntime().Value.ZhirenProgramReorderTopByLength(frame, effect);
    public SkillProgramStepOutcome ZhirenResolveFieldTiers(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterPanShuOlRuntime().Value.ZhirenProgramResolveFieldTiers(frame, effect);
    public SkillProgramStepOutcome YanerResolvePairBenefit(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterPanShuOlRuntime().Value.YanerProgramResolvePairBenefit(frame, effect);
}
