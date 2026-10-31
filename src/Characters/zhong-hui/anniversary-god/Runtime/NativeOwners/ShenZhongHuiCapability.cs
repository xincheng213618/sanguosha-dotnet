#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class ShenZhongHuiCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhongHuiAnniversaryGodEntries> getCharacterZhongHuiAnniversaryGodRuntime) : IShenZhongHuiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterZhongHuiAnniversaryGodRuntime().BeginShenZh(f, op);
}
