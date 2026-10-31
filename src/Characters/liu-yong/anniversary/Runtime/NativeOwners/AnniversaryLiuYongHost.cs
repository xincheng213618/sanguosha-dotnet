#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryLiuYongHost(Func<RuntimeEntries.CharacterLiuYongAnniversaryEntries> getCharacterLiuYongAnniversaryRuntime) : IAnniversaryLiuYongHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterLiuYongAnniversaryRuntime().BeginAnniversaryLiuYong(f, op);
}
