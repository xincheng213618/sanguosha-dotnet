#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MouChenGongCapability(Func<RuntimeEntries.CharacterChenGongMouMobileEntries> getCharacterChenGongMouMobileRuntime) : IMouChenGongHost
{
    public SkillProgramStepOutcome Gift(ProgramSkillFrame frame) => getCharacterChenGongMouMobileRuntime().BeginMouChenGongGift(frame);
    public SkillProgramStepOutcome Release(ProgramSkillFrame frame) => getCharacterChenGongMouMobileRuntime().BeginMouChenGongRelease(frame);
}
