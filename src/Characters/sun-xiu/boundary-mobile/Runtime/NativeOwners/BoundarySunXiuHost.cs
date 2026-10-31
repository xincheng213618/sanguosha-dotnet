#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class BoundarySunXiuHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterSunXiuBoundaryMobileEntries> getCharacterSunXiuBoundaryMobileRuntime) : IBoundarySunXiuHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCharacterSunXiuBoundaryMobileRuntime().BeginBoundaryXingxue(frame);
}
