#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryZhuRanHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterZhuRanBoundaryMobileRuntime>> getCharacterZhuRanBoundaryMobileRuntime) : IBoundaryZhuRanHost
{
    public SkillProgramStepOutcome Prepare(ProgramSkillFrame f) => getCharacterZhuRanBoundaryMobileRuntime().Value.PrepareBoundaryDanshou(f);
}
