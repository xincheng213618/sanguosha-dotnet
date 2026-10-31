#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ZhangYanCapability(Func<RuntimeEntries.CharacterZhangYanOlEntries> getCharacterZhangYanOlRuntime) : IZhangYanHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterZhangYanOlRuntime().BeginZhangYan(f, op);
}
