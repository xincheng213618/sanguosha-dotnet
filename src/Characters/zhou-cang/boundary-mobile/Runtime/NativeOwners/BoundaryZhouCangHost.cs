#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryZhouCangHost(Func<RuntimeEntries.CharacterZhouCangBoundaryMobileEntries> getCharacterZhouCangBoundaryMobileRuntime) : IBoundaryZhouCangHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterZhouCangBoundaryMobileRuntime().BeginBoundaryZhongyong(f);
}
