#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class ShiYangHongCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterYangHongMobileShiEntries> getCharacterYangHongMobileShiRuntime) : IShiYangHongHost
{
    public SkillProgramStepOutcome ResolveJijian(ProgramSkillFrame frame) => getCharacterYangHongMobileShiRuntime().BeginShiYhJijian(frame);
    public SkillProgramStepOutcome ResolveYuanmo(ProgramSkillFrame frame) => getCharacterYangHongMobileShiRuntime().BeginShiYhYuanmo(frame);
}
