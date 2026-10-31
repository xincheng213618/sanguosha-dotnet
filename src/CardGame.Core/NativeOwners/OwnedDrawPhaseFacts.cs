#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class OwnedDrawPhaseFacts(EngineStates.CoreMatchOutcomeState matchOutcomeState, Func<RuntimeEntries.CoreGameEngineNativeDrawAttemptsEntries> getCoreGameEngineNativeDrawAttemptsRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineNativeDrawThresholdRuntime>> getCoreGameEngineNativeDrawThresholdRuntime, Func<RuntimeEntries.CoreGameEngineShanheS5ShensuEntries> getCoreGameEngineShanheS5ShensuRuntime, Func<RuntimeEntries.CharacterZhuHuanBoundaryMobileEntries> getCharacterZhuHuanBoundaryMobileRuntime, Func<RuntimeEntries.CoreLeastHandMarkerDrawProgramsEntries> getCoreLeastHandMarkerDrawProgramsRuntime, Func<RuntimeEntries.CoreGameEngineTableOrderEntries> getCoreGameEngineTableOrderRuntime) : IOwnedDrawPhaseFacts
{
    public Winner Winner => matchOutcomeState.ReferenceToWinner;
    public int SkipDrawPhaseMask => (int)GameEngine.DelayedTurnEffects.SkipDrawPhase;
    public bool HasAttemptCapability => getCoreGameEngineNativeDrawAttemptsRuntime().HasNativeDrawAttemptCapability;
    public bool TracksNativeDrawThresholds => getCoreGameEngineNativeDrawThresholdRuntime().Value.TracksNativeDrawThresholds;
    public bool TracksShanheS5PhaseSkips => getCoreGameEngineShanheS5ShensuRuntime().TracksShanheS5PhaseSkips;
    public bool TracksBoundaryZhuHuanPhaseSkips => getCharacterZhuHuanBoundaryMobileRuntime().TracksBoundaryZhuHuanPhaseSkips;

    public bool HasIssuedExtraDrawMarker(int seat) => getCoreLeastHandMarkerDrawProgramsRuntime().HasIssuedExtraDrawMarker(seat);
    public int RelativeTableOrder(int from, int target) => getCoreGameEngineTableOrderRuntime().RelativeTableOrder(from, target);
}
