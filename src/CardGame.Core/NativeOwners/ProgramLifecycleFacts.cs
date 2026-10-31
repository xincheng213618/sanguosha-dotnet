#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramLifecycleFacts(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreDyingCursorRuntimeRuntime>> getCoreDyingCursorRuntimeRuntime, Func<RuntimeEntries.CoreGameEngineAttributedMarkerEventsEntries> getCoreGameEngineAttributedMarkerEventsRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreGameEngineHellActivitiesEntries> getCoreGameEngineHellActivitiesRuntime) : IProgramLifecycleFacts
{
    public int? DyingKillerSeat => getCoreDyingCursorRuntimeRuntime().Value.ActiveDying?.KillerSeat;
    public bool HasAttributedEventOperations => getCoreGameEngineAttributedMarkerEventsRuntime().HasAttributedEventOperations();

    public IReadOnlyDictionary<PlayerMarkerKind, int> MarkerCounts(int seat) => new Dictionary<PlayerMarkerKind, int>(matchParticipantsState.ReferenceToPlayers[seat].Markers);
    public bool IsExactHellAppearanceWindow(ProgramLifecycleTriggerWindowFrame frame) => getCoreGameEngineHellActivitiesRuntime().IsExactHellAppearanceWindow(frame);
}
