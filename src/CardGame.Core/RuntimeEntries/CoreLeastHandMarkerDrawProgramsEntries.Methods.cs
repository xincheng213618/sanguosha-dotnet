#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreLeastHandMarkerDrawProgramsEntries
{
    internal SkillProgramStepOutcome BeginLeastHandMarkerOrDraw(ProgramSkillFrame frame, int targetSeat, PlayerMarkerKind marker) => _resolve().BeginLeastHandMarkerOrDraw(frame, targetSeat, marker);
    internal ProgramMarkerExtraDrawPolicy? ConsumeIssuedExtraDrawMarker(DrawPhaseObligationFrame frame) => _resolve().ConsumeIssuedExtraDrawMarker(frame);
    internal bool HasIssuedExtraDrawMarker(int seat) => _resolve().HasIssuedExtraDrawMarker(seat);
    internal int IndependentIssuedDrawMarkerCount(CharacterState target, PlayerMarkerKind marker, int source) => _resolve().IndependentIssuedDrawMarkerCount(target, marker, source);
    internal bool IsBeforeActualDamageDistancePolicyQualified(CharacterState owner) => _resolve().IsBeforeActualDamageDistancePolicyQualified(owner);
    internal bool ResumeLeastHandMarkerDraw(long frameId) => _resolve().ResumeLeastHandMarkerDraw(frameId);
}
