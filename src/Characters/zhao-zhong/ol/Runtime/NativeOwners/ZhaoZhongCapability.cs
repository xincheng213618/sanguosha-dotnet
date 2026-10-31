#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ZhaoZhongCapability(Func<RuntimeEntries.CharacterZhaoZhongOlEntries> getCharacterZhaoZhongOlRuntime) : IZhaoZhongHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterZhaoZhongOlRuntime().BeginZhaoZhong(f, op);
}
