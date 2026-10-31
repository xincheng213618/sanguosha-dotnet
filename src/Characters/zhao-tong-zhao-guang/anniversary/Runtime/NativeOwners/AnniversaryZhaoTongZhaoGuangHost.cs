#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryZhaoTongZhaoGuangHost(Func<RuntimeEntries.CharacterZhaoTongZhaoGuangAnniversaryEntries> getCharacterZhaoTongZhaoGuangAnniversaryRuntime) : IAnniversaryZhaoTongZhaoGuangHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterZhaoTongZhaoGuangAnniversaryRuntime().BeginAnniversaryZhaoTongZhaoGuang(f, op);
}
