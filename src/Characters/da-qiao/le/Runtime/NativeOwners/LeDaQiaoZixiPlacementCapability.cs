#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class LeDaQiaoZixiPlacementCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterDaQiaoLeEntries> getCharacterDaQiaoLeRuntime) : ILeDaQiaoZixiPlacementHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterDaQiaoLeRuntime().BeginZixiPlacement(f);
}
