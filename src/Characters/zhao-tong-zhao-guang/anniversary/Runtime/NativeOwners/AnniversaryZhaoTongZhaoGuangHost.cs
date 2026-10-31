#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryZhaoTongZhaoGuangHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhaoTongZhaoGuangAnniversaryEntries> getCharacterZhaoTongZhaoGuangAnniversaryRuntime) : IAnniversaryZhaoTongZhaoGuangHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterZhaoTongZhaoGuangAnniversaryRuntime().BeginAnniversaryZhaoTongZhaoGuang(f, op);
}
