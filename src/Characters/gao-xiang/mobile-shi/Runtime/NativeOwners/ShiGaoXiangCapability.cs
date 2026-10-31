#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class ShiGaoXiangCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterGaoXiangMobileShiEntries> getCharacterGaoXiangMobileShiRuntime) : IShiGaoXiangHost
{
    public SkillProgramStepOutcome ResolveChiyuan(ProgramSkillFrame frame) => getCharacterGaoXiangMobileShiRuntime().BeginShiZxChiyuan(frame);
    public SkillProgramStepOutcome ResolveErdi(ProgramSkillFrame frame) => getCharacterGaoXiangMobileShiRuntime().BeginShiZxErdi(frame);
}
