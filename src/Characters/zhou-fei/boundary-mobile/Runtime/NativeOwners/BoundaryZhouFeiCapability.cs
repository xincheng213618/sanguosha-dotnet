#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryZhouFeiCapability(Func<RuntimeEntries.CharacterZhouFeiBoundaryMobileEntries> getCharacterZhouFeiBoundaryMobileRuntime) : IBoundaryZhouFeiHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame frame, SkillProgramEffectOp operation) => getCharacterZhouFeiBoundaryMobileRuntime().BeginBoundaryZf(frame, operation);
}
