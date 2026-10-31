#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MouHuangGaiKurouCapability(Func<RuntimeEntries.CharacterHuangGaiMouMobileEntries> getCharacterHuangGaiMouMobileRuntime) : IMouHuangGaiKurouHost
{
    public SkillProgramStepOutcome GiveBeforeLoss(ProgramSkillFrame frame) => getCharacterHuangGaiMouMobileRuntime().BeginMouKurouGift(frame);
    public void GrantLossArmor(ProgramSkillFrame frame) => getCharacterHuangGaiMouMobileRuntime().GrantMouKurouLossArmor(frame);
}
