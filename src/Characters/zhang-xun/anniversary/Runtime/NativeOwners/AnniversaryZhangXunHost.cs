#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryZhangXunHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterZhangXunAnniversaryRuntime>> getCharacterZhangXunAnniversaryRuntime) : IAnniversaryZhangXunHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp operation) => operation == SkillProgramEffectOp.ArmAnniversaryZhangXunSuizheng ? getCharacterZhangXunAnniversaryRuntime().Value.ArmAnniversaryZhangXun(f) : getCharacterZhangXunAnniversaryRuntime().Value.ChooseAnniversaryZhangXunSlash(f);
}
