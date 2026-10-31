#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryLiRuHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLiRuBoundaryMobileEntries> getCharacterLiRuBoundaryMobileRuntime) : IBoundaryLiRuHost
{
    public SkillProgramStepOutcome ResolveMieji(ProgramSkillFrame frame) => getCharacterLiRuBoundaryMobileRuntime().ResolveBoundaryLiRuMieji(frame);
}
