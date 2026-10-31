#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class SunYuanCapability(Func<RuntimeEntries.CharacterSunYuanOlEntries> getCharacterSunYuanOlRuntime) : ISunYuanHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterSunYuanOlRuntime().BeginSunYuan(f, op);
}
