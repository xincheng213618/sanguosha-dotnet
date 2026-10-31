#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ZixiEffectsCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterDaQiaoLeEntries> getCharacterDaQiaoLeRuntime) : IZixiEffectsHost
{
    public SkillProgramStepOutcome ResolveZixiUniqueTargetJudgmentEffects(ProgramSkillFrame f) => getCharacterDaQiaoLeRuntime().BeginZixiEffects(f);
}
