#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryLiRuHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterLiRuBoundaryMobileRuntime>> getCharacterLiRuBoundaryMobileRuntime) : IBoundaryLiRuHost
{
    public SkillProgramStepOutcome ResolveMieji(ProgramSkillFrame frame) => getCharacterLiRuBoundaryMobileRuntime().Value.ResolveBoundaryLiRuMieji(frame);
}
