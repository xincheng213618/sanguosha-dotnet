#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MobileShiTianFengCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterTianFengMobileShiEntries> getCharacterTianFengMobileShiRuntime) : IMobileShiTianFengHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCharacterTianFengMobileShiRuntime().BeginMobileShiTianFeng(frame);
}
