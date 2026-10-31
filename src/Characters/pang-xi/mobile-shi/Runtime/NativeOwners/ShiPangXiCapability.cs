#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class ShiPangXiCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterPangXiMobileShiEntries> getCharacterPangXiMobileShiRuntime) : IShiPangXiHost
{
    public SkillProgramStepOutcome ResolveXuye(ProgramSkillFrame frame) => getCharacterPangXiMobileShiRuntime().BeginShiPxXuye(frame);
    public SkillProgramStepOutcome ResolveKuangxiang(ProgramSkillFrame frame) => getCharacterPangXiMobileShiRuntime().BeginShiPxKuangxiang(frame);
    public SkillProgramStepOutcome ResolveRider(ProgramSkillFrame frame) => getCharacterPangXiMobileShiRuntime().BeginShiPxRider(frame);
    public SkillProgramStepOutcome ResolveSweep(ProgramSkillFrame frame) => getCharacterPangXiMobileShiRuntime().BeginShiPxSweep(frame);
}
