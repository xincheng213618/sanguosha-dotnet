#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ZhangQiYingProgramCapability(Func<RuntimeEntries.CharacterZhangQiYingOlEntries> getCharacterZhangQiYingOlRuntime) : IZhangQiYingProgramHost
{
    public SkillProgramStepOutcome ExecuteZhangQiYing(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterZhangQiYingOlRuntime().ExecuteZhangQiYing(f, op);
}
