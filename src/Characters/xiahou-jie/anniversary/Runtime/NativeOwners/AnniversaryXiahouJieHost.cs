#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryXiahouJieHost(Func<RuntimeEntries.CharacterXiahouJieAnniversaryEntries> getCharacterXiahouJieAnniversaryRuntime) : IAnniversaryXiahouJieHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterXiahouJieAnniversaryRuntime().BeginAnniversaryXiahouJie(f, op);
}
