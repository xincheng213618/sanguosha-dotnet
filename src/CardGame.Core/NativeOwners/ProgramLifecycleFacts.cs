#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramLifecycleFacts(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries> getCoreDyingCursorRuntimeRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAttributedMarkerEventsEntries> getCoreGameEngineAttributedMarkerEventsRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineHellActivitiesEntries> getCoreGameEngineHellActivitiesRuntime) : IProgramLifecycleFacts
{
    public int? DyingKillerSeat => getCoreDyingCursorRuntimeRuntime().ActiveDying?.KillerSeat;
    public bool HasAttributedEventOperations => getCoreGameEngineAttributedMarkerEventsRuntime().HasAttributedEventOperations();

    public IReadOnlyDictionary<PlayerMarkerKind, int> MarkerCounts(int seat) => new Dictionary<PlayerMarkerKind, int>(matchParticipantsState.ReferenceToPlayers[seat].Markers);
    public bool IsExactHellAppearanceWindow(ProgramLifecycleTriggerWindowFrame frame) => getCoreGameEngineHellActivitiesRuntime().IsExactHellAppearanceWindow(frame);
}
