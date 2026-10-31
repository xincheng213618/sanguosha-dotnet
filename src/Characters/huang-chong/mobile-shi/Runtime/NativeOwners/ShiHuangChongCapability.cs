#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class ShiHuangChongCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHuangChongMobileShiEntries> getCharacterHuangChongMobileShiRuntime) : IShiHuangChongHost
{
    public SkillProgramStepOutcome ResolveCujun(ProgramSkillFrame frame) => getCharacterHuangChongMobileShiRuntime().BeginShiHcCujun(frame);
    public SkillProgramStepOutcome ResolveSili(ProgramSkillFrame frame) => getCharacterHuangChongMobileShiRuntime().BeginShiHcSili(frame);
}
