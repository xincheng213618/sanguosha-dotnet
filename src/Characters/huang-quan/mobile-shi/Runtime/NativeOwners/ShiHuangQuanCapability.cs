#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class ShiHuangQuanCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHuangQuanMobileShiEntries> getCharacterHuangQuanMobileShiRuntime) : IShiHuangQuanHost
{
    public SkillProgramStepOutcome ResolveCuixian(ProgramSkillFrame frame) => getCharacterHuangQuanMobileShiRuntime().BeginShiQuanCuixian(frame);
    public SkillProgramStepOutcome ResolveGushi(ProgramSkillFrame frame) => getCharacterHuangQuanMobileShiRuntime().BeginShiQuanGushi(frame);
}
