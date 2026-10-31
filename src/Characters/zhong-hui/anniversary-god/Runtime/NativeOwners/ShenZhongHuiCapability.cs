#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShenZhongHuiCapability(Func<RuntimeEntries.CharacterZhongHuiAnniversaryGodEntries> getCharacterZhongHuiAnniversaryGodRuntime) : IShenZhongHuiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterZhongHuiAnniversaryGodRuntime().BeginShenZh(f, op);
}
