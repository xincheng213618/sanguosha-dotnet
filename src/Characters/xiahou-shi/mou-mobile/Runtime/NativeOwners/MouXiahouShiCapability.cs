#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MouXiahouShiCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterXiahouShiMouMobileEntries> getCharacterXiahouShiMouMobileRuntime) : IMouXiahouShiHost
{
    public SkillProgramStepOutcome Discard(ProgramSkillFrame f) => getCharacterXiahouShiMouMobileRuntime().BeginMouXiahouShiDiscard(f);
    public SkillProgramStepOutcome Support(ProgramSkillFrame f) => getCharacterXiahouShiMouMobileRuntime().BeginMouXiahouShiSupport(f);
    public SkillProgramStepOutcome Qiaoshi(ProgramSkillFrame f) => getCharacterXiahouShiMouMobileRuntime().BeginMouXiahouShiQiaoshi(f);
}
