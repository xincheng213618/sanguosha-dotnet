#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryZhouCangHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterZhouCangBoundaryMobileRuntime>> getCharacterZhouCangBoundaryMobileRuntime) : IBoundaryZhouCangHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterZhouCangBoundaryMobileRuntime().Value.BeginBoundaryZhongyong(f);
}
