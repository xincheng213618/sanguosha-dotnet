#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryCheZhouHost(Func<RuntimeEntries.CharacterCheZhouAnniversaryEntries> getCharacterCheZhouAnniversaryRuntime) : IAnniversaryCheZhouHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterCheZhouAnniversaryRuntime().BeginAnniversaryCheZhouPigua(f);
}
