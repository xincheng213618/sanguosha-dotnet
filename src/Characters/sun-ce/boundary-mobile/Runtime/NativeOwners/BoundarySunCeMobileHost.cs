#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class BoundarySunCeMobileHost(Func<RuntimeEntries.CharacterSunCeBoundaryMobileEntries> getCharacterSunCeBoundaryMobileRuntime) : IBoundarySunCeMobileHost
{
    public SkillProgramStepOutcome Challenge(ProgramSkillFrame f) => getCharacterSunCeBoundaryMobileRuntime().BeginBoundarySunCeMobileChallenge(f);
}
