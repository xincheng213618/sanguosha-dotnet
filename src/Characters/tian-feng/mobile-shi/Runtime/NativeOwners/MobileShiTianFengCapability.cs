#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MobileShiTianFengCapability(Func<RuntimeEntries.CharacterTianFengMobileShiEntries> getCharacterTianFengMobileShiRuntime) : IMobileShiTianFengHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCharacterTianFengMobileShiRuntime().BeginMobileShiTianFeng(frame);
}
