#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MouHuangGaiKurouCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHuangGaiMouMobileEntries> getCharacterHuangGaiMouMobileRuntime) : IMouHuangGaiKurouHost
{
    public SkillProgramStepOutcome GiveBeforeLoss(ProgramSkillFrame frame) => getCharacterHuangGaiMouMobileRuntime().BeginMouKurouGift(frame);
    public void GrantLossArmor(ProgramSkillFrame frame) => getCharacterHuangGaiMouMobileRuntime().GrantMouKurouLossArmor(frame);
}
