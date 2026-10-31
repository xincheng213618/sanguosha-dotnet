#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class LeDaQiaoZixiPlacementCapability(Func<RuntimeEntries.CharacterDaQiaoLeEntries> getCharacterDaQiaoLeRuntime) : ILeDaQiaoZixiPlacementHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterDaQiaoLeRuntime().BeginZixiPlacement(f);
}
