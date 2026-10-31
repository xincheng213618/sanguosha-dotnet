#nullable enable
using System.Text.Json.Serialization;

namespace CardGame.Core.NativeOwners;
internal sealed class BoundarySunCeMobileHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterSunCeBoundaryMobileEntries> getCharacterSunCeBoundaryMobileRuntime) : IBoundarySunCeMobileHost
{
    public SkillProgramStepOutcome Challenge(ProgramSkillFrame f) => getCharacterSunCeBoundaryMobileRuntime().BeginBoundarySunCeMobileChallenge(f);
}
