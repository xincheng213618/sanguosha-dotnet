#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAttributedMarkerEventsEntries
{
    internal int ActiveSubjectMarkerCount(int subject, PlayerMarkerKind marker) => _resolve().ActiveSubjectMarkerCount(subject, marker);
    internal void ApplyAttributedEvent(ProgramSkillFrame frame, SkillProgramEffect effect, int targetSeat) => _resolve().ApplyAttributedEvent(frame, effect, targetSeat);
    internal bool CanOfferAttributedEvent(SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferAttributedEvent(trigger, context);
    internal ProgramAiPublicContext CreateAttributedPaymentAiContext(CharacterState owner, SkillProgramTrigger trigger) => _resolve().CreateAttributedPaymentAiContext(owner, trigger);
    internal bool HasAttributedEventOperations() => _resolve().HasAttributedEventOperations();
    internal IReadOnlyList<ProgramTriggerCandidate> IncludeAttributedDrawObservers(CharacterState subject, IReadOnlyList<ProgramTriggerCandidate> existing) => _resolve().IncludeAttributedDrawObservers(subject, existing);
    internal bool IsMarkerSourceEnabled(int source, PlayerMarkerKind marker) => _resolve().IsMarkerSourceEnabled(source, marker);
    internal bool MarkerEventApplied(long parent, int subject, PlayerMarkerKind marker, SkillProgramEffectOp op) => _resolve().MarkerEventApplied(parent, subject, marker, op);
    internal void MutateParticipantMarker(ProgramSkillFrame frame, int seat, PlayerMarkerKind marker, int delta, bool activeOnly = false) => _resolve().MutateParticipantMarker(frame, seat, marker, delta, activeOnly);
}
