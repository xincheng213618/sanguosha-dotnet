#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class ShenXuChuHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterXuChuAnniversaryGodEntries> getCharacterXuChuAnniversaryGodRuntime) : IShenXuChuHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterXuChuAnniversaryGodRuntime().BeginShenXuChu(f, op);
}
