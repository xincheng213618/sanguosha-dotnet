#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryXunXuHost(Func<RuntimeEntries.CharacterXunXuAnniversaryEntries> getCharacterXunXuAnniversaryRuntime) : IAnniversaryXunXuHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterXunXuAnniversaryRuntime().BeginXunXu(f, op);
}
