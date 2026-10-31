#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversarySunHuanHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterSunHuanAnniversaryEntries> getCharacterSunHuanAnniversaryRuntime) : IAnniversarySunHuanHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterSunHuanAnniversaryRuntime().BeginSunHuan(f, op);
}
