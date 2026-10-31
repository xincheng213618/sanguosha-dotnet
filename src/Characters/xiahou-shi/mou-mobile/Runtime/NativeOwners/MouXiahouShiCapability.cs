#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MouXiahouShiCapability(Func<RuntimeEntries.CharacterXiahouShiMouMobileEntries> getCharacterXiahouShiMouMobileRuntime) : IMouXiahouShiHost
{
    public SkillProgramStepOutcome Discard(ProgramSkillFrame f) => getCharacterXiahouShiMouMobileRuntime().BeginMouXiahouShiDiscard(f);
    public SkillProgramStepOutcome Support(ProgramSkillFrame f) => getCharacterXiahouShiMouMobileRuntime().BeginMouXiahouShiSupport(f);
    public SkillProgramStepOutcome Qiaoshi(ProgramSkillFrame f) => getCharacterXiahouShiMouMobileRuntime().BeginMouXiahouShiQiaoshi(f);
}
