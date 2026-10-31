#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryZhangFeiCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhangFeiBoundaryMobileEntries> getCharacterZhangFeiBoundaryMobileRuntime) : IBoundaryZhangFeiHost
{
    public SkillProgramStepOutcome ResolveBoundaryZhangFei(ProgramSkillFrame frame, SkillProgramEffectOp operation) => getCharacterZhangFeiBoundaryMobileRuntime().ResolveBoundaryZhangFei(frame, operation);
}
