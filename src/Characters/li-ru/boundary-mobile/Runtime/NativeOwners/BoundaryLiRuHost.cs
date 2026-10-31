#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryLiRuHost(Func<RuntimeEntries.CharacterLiRuBoundaryMobileEntries> getCharacterLiRuBoundaryMobileRuntime) : IBoundaryLiRuHost
{
    public SkillProgramStepOutcome ResolveMieji(ProgramSkillFrame frame) => getCharacterLiRuBoundaryMobileRuntime().ResolveBoundaryLiRuMieji(frame);
}
