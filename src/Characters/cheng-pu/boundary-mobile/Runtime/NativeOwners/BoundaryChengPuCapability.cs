#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryChengPuCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterChengPuBoundaryMobileEntries> getCharacterChengPuBoundaryMobileRuntime) : IBoundaryChengPuHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterChengPuBoundaryMobileRuntime().BeginBoundaryChengPuLihuo(f);
}
