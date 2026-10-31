#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryXunXuHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterXunXuAnniversaryEntries> getCharacterXunXuAnniversaryRuntime) : IAnniversaryXunXuHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterXunXuAnniversaryRuntime().BeginXunXu(f, op);
}
