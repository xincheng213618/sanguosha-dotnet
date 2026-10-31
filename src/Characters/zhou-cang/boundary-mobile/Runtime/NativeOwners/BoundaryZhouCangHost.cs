#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryZhouCangHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhouCangBoundaryMobileEntries> getCharacterZhouCangBoundaryMobileRuntime) : IBoundaryZhouCangHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterZhouCangBoundaryMobileRuntime().BeginBoundaryZhongyong(f);
}
