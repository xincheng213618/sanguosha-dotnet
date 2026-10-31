#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ZixiEffectsCapability(Func<RuntimeEntries.CharacterDaQiaoLeEntries> getCharacterDaQiaoLeRuntime) : IZixiEffectsHost
{
    public SkillProgramStepOutcome ResolveZixiUniqueTargetJudgmentEffects(ProgramSkillFrame f) => getCharacterDaQiaoLeRuntime().BeginZixiEffects(f);
}
