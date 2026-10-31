#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class BoundarySunXiuHost(Func<RuntimeEntries.CharacterSunXiuBoundaryMobileEntries> getCharacterSunXiuBoundaryMobileRuntime) : IBoundarySunXiuHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCharacterSunXiuBoundaryMobileRuntime().BeginBoundaryXingxue(frame);
}
