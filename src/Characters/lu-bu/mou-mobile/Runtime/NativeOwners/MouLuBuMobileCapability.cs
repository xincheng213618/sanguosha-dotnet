#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MouLuBuMobileCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLuBuMouMobileEntries> getCharacterLuBuMouMobileRuntime) : IMouLuBuMobileHost
{
    public SkillProgramStepOutcome Bonus(ProgramSkillFrame f) => getCharacterLuBuMouMobileRuntime().ResolveMouLbBonus(f);
    public SkillProgramStepOutcome Liyu(ProgramSkillFrame f) => getCharacterLuBuMouMobileRuntime().BeginMouLbLiyu(f);
}
