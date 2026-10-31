#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShiHuangChongCapability(Func<RuntimeEntries.CharacterHuangChongMobileShiEntries> getCharacterHuangChongMobileShiRuntime) : IShiHuangChongHost
{
    public SkillProgramStepOutcome ResolveCujun(ProgramSkillFrame frame) => getCharacterHuangChongMobileShiRuntime().BeginShiHcCujun(frame);
    public SkillProgramStepOutcome ResolveSili(ProgramSkillFrame frame) => getCharacterHuangChongMobileShiRuntime().BeginShiHcSili(frame);
}
