#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryFengXiHost(Func<RuntimeEntries.CharacterFengXiAnniversaryEntries> getCharacterFengXiAnniversaryRuntime) : IAnniversaryFengXiHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterFengXiAnniversaryRuntime().BeginFengXi(f, op);
}
