#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryZhangXunHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhangXunAnniversaryEntries> getCharacterZhangXunAnniversaryRuntime) : IAnniversaryZhangXunHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp operation) => operation == SkillProgramEffectOp.ArmAnniversaryZhangXunSuizheng ? getCharacterZhangXunAnniversaryRuntime().ArmAnniversaryZhangXun(f) : getCharacterZhangXunAnniversaryRuntime().ChooseAnniversaryZhangXunSlash(f);
}
