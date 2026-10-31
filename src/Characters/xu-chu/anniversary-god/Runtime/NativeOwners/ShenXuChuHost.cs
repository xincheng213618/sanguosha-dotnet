#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShenXuChuHost(Func<RuntimeEntries.CharacterXuChuAnniversaryGodEntries> getCharacterXuChuAnniversaryGodRuntime) : IShenXuChuHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterXuChuAnniversaryGodRuntime().BeginShenXuChu(f, op);
}
