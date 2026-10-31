#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryChengPuCapability(Func<RuntimeEntries.CharacterChengPuBoundaryMobileEntries> getCharacterChengPuBoundaryMobileRuntime) : IBoundaryChengPuHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterChengPuBoundaryMobileRuntime().BeginBoundaryChengPuLihuo(f);
}
