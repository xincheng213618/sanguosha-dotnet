#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryLiuYongHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLiuYongAnniversaryEntries> getCharacterLiuYongAnniversaryRuntime) : IAnniversaryLiuYongHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterLiuYongAnniversaryRuntime().BeginAnniversaryLiuYong(f, op);
}
