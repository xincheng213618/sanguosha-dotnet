#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryZhouFeiCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhouFeiBoundaryMobileEntries> getCharacterZhouFeiBoundaryMobileRuntime) : IBoundaryZhouFeiHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame frame, SkillProgramEffectOp operation) => getCharacterZhouFeiBoundaryMobileRuntime().BeginBoundaryZf(frame, operation);
}
