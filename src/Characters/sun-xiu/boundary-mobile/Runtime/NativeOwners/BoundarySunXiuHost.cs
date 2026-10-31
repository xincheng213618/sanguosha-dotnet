#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class BoundarySunXiuHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterSunXiuBoundaryMobileRuntime>> getCharacterSunXiuBoundaryMobileRuntime) : IBoundarySunXiuHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCharacterSunXiuBoundaryMobileRuntime().Value.BeginBoundaryXingxue(frame);
}
