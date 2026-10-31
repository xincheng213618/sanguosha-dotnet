#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MouChenGongCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterChenGongMouMobileEntries> getCharacterChenGongMouMobileRuntime) : IMouChenGongHost
{
    public SkillProgramStepOutcome Gift(ProgramSkillFrame frame) => getCharacterChenGongMouMobileRuntime().BeginMouChenGongGift(frame);
    public SkillProgramStepOutcome Release(ProgramSkillFrame frame) => getCharacterChenGongMouMobileRuntime().BeginMouChenGongRelease(frame);
}
