#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShiGaoXiangCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterGaoXiangMobileShiRuntime>> getCharacterGaoXiangMobileShiRuntime) : IShiGaoXiangHost
{
    public SkillProgramStepOutcome ResolveChiyuan(ProgramSkillFrame frame) => getCharacterGaoXiangMobileShiRuntime().Value.BeginShiZxChiyuan(frame);
    public SkillProgramStepOutcome ResolveErdi(ProgramSkillFrame frame) => getCharacterGaoXiangMobileShiRuntime().Value.BeginShiZxErdi(frame);
}
