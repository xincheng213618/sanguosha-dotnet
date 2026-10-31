#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreLeastHandMarkerDrawProgramsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginLeastHandMarkerOrDraw(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat, global::CardGame.Core.PlayerMarkerKind marker) => _resolve().BeginLeastHandMarkerOrDraw(frame, targetSeat, marker);
    internal global::CardGame.Core.ProgramMarkerExtraDrawPolicy? ConsumeIssuedExtraDrawMarker(global::CardGame.Core.DrawPhaseObligationFrame frame) => _resolve().ConsumeIssuedExtraDrawMarker(frame);
    internal bool HasIssuedExtraDrawMarker(int seat) => _resolve().HasIssuedExtraDrawMarker(seat);
    internal int IndependentIssuedDrawMarkerCount(global::CardGame.Core.CharacterState target, global::CardGame.Core.PlayerMarkerKind marker, int source) => _resolve().IndependentIssuedDrawMarkerCount(target, marker, source);
    internal bool IsBeforeActualDamageDistancePolicyQualified(global::CardGame.Core.CharacterState owner) => _resolve().IsBeforeActualDamageDistancePolicyQualified(owner);
    internal bool ResumeLeastHandMarkerDraw(long frameId) => _resolve().ResumeLeastHandMarkerDraw(frameId);
}
