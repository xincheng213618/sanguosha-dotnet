#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class BoundarySunCeMobileHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterSunCeBoundaryMobileRuntime>> getCharacterSunCeBoundaryMobileRuntime) : IBoundarySunCeMobileHost
{
    public SkillProgramStepOutcome Challenge(ProgramSkillFrame f) => getCharacterSunCeBoundaryMobileRuntime().Value.BeginBoundarySunCeMobileChallenge(f);
}
