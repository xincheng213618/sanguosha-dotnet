#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShiYangHongCapability(Func<RuntimeEntries.CharacterYangHongMobileShiEntries> getCharacterYangHongMobileShiRuntime) : IShiYangHongHost
{
    public SkillProgramStepOutcome ResolveJijian(ProgramSkillFrame frame) => getCharacterYangHongMobileShiRuntime().BeginShiYhJijian(frame);
    public SkillProgramStepOutcome ResolveYuanmo(ProgramSkillFrame frame) => getCharacterYangHongMobileShiRuntime().BeginShiYhYuanmo(frame);
}
