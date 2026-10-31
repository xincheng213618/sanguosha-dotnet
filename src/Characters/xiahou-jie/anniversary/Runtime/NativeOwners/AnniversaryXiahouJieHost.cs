#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryXiahouJieHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterXiahouJieAnniversaryEntries> getCharacterXiahouJieAnniversaryRuntime) : IAnniversaryXiahouJieHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterXiahouJieAnniversaryRuntime().BeginAnniversaryXiahouJie(f, op);
}
