#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MouLuBuMobileCapability(Func<RuntimeEntries.CharacterLuBuMouMobileEntries> getCharacterLuBuMouMobileRuntime) : IMouLuBuMobileHost
{
    public SkillProgramStepOutcome Bonus(ProgramSkillFrame f) => getCharacterLuBuMouMobileRuntime().ResolveMouLbBonus(f);
    public SkillProgramStepOutcome Liyu(ProgramSkillFrame f) => getCharacterLuBuMouMobileRuntime().BeginMouLbLiyu(f);
}
