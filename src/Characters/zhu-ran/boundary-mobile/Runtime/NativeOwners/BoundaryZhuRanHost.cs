#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryZhuRanHost(Func<RuntimeEntries.CharacterZhuRanBoundaryMobileEntries> getCharacterZhuRanBoundaryMobileRuntime) : IBoundaryZhuRanHost
{
    public SkillProgramStepOutcome Prepare(ProgramSkillFrame f) => getCharacterZhuRanBoundaryMobileRuntime().PrepareBoundaryDanshou(f);
}
