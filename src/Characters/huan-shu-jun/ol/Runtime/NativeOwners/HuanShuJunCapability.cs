#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class HuanShuJunCapability(Func<RuntimeEntries.CharacterHuanShuJunOlEntries> getCharacterHuanShuJunOlRuntime) : IHuanShuJunHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterHuanShuJunOlRuntime().BeginHuanShuJun(f, op);
}
