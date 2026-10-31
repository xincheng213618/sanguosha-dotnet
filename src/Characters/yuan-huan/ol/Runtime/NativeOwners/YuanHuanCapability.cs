#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class YuanHuanCapability(Func<RuntimeEntries.CharacterYuanHuanOlEntries> getCharacterYuanHuanOlRuntime) : IYuanHuanHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterYuanHuanOlRuntime().BeginYuanHuan(f, op);
}
