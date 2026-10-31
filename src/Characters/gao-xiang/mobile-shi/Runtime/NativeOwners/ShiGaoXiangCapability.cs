#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShiGaoXiangCapability(Func<RuntimeEntries.CharacterGaoXiangMobileShiEntries> getCharacterGaoXiangMobileShiRuntime) : IShiGaoXiangHost
{
    public SkillProgramStepOutcome ResolveChiyuan(ProgramSkillFrame frame) => getCharacterGaoXiangMobileShiRuntime().BeginShiZxChiyuan(frame);
    public SkillProgramStepOutcome ResolveErdi(ProgramSkillFrame frame) => getCharacterGaoXiangMobileShiRuntime().BeginShiZxErdi(frame);
}
