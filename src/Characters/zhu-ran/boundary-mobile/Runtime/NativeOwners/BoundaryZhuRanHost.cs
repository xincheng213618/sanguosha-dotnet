#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryZhuRanHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhuRanBoundaryMobileEntries> getCharacterZhuRanBoundaryMobileRuntime) : IBoundaryZhuRanHost
{
    public SkillProgramStepOutcome Prepare(ProgramSkillFrame f) => getCharacterZhuRanBoundaryMobileRuntime().PrepareBoundaryDanshou(f);
}
