#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAttributedMarkerEventsEntries
{
    internal int ActiveSubjectMarkerCount(int subject, global::CardGame.Core.PlayerMarkerKind marker) => _resolve().ActiveSubjectMarkerCount(subject, marker);
    internal void ApplyAttributedEvent(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect, int targetSeat) => _resolve().ApplyAttributedEvent(frame, effect, targetSeat);
    internal bool CanOfferAttributedEvent(global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferAttributedEvent(trigger, context);
    internal global::CardGame.Core.ProgramAiPublicContext CreateAttributedPaymentAiContext(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().CreateAttributedPaymentAiContext(owner, trigger);
    internal bool HasAttributedEventOperations() => _resolve().HasAttributedEventOperations();
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> IncludeAttributedDrawObservers(global::CardGame.Core.CharacterState subject, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> existing) => _resolve().IncludeAttributedDrawObservers(subject, existing);
    internal bool IsMarkerSourceEnabled(int source, global::CardGame.Core.PlayerMarkerKind marker) => _resolve().IsMarkerSourceEnabled(source, marker);
    internal bool MarkerEventApplied(long parent, int subject, global::CardGame.Core.PlayerMarkerKind marker, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().MarkerEventApplied(parent, subject, marker, op);
    internal void MutateParticipantMarker(global::CardGame.Core.ProgramSkillFrame frame, int seat, global::CardGame.Core.PlayerMarkerKind marker, int delta, bool activeOnly = false) => _resolve().MutateParticipantMarker(frame, seat, marker, delta, activeOnly);
}
