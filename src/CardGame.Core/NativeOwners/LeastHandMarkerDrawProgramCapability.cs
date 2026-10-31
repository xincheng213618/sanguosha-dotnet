#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LeastHandMarkerDrawProgramCapability(Func<RuntimeEntries.CoreLeastHandMarkerDrawProgramsEntries> getCoreLeastHandMarkerDrawProgramsRuntime) : ILeastHandMarkerDrawProgramHost
{
    public SkillProgramStepOutcome GrantLeastHandMarkerOrDraw(ProgramSkillFrame frame, int targetSeat, PlayerMarkerKind marker) => getCoreLeastHandMarkerDrawProgramsRuntime().BeginLeastHandMarkerOrDraw(frame, targetSeat, marker);
}
