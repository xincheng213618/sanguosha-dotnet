#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class OwnedDrawPhaseFacts(global::CardGame.Core.EngineStates.CoreMatchOutcomeState matchOutcomeState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNativeDrawAttemptsEntries> getCoreGameEngineNativeDrawAttemptsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNativeDrawThresholdEntries> getCoreGameEngineNativeDrawThresholdRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheS5ShensuEntries> getCoreGameEngineShanheS5ShensuRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhuHuanBoundaryMobileEntries> getCharacterZhuHuanBoundaryMobileRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreLeastHandMarkerDrawProgramsEntries> getCoreLeastHandMarkerDrawProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineTableOrderEntries> getCoreGameEngineTableOrderRuntime) : IOwnedDrawPhaseFacts
{
    public Winner Winner => matchOutcomeState.ReferenceToWinner;
    public int SkipDrawPhaseMask => (int)global::CardGame.Core.GameEngine.DelayedTurnEffects.SkipDrawPhase;
    public bool HasAttemptCapability => getCoreGameEngineNativeDrawAttemptsRuntime().HasNativeDrawAttemptCapability;
    public bool TracksNativeDrawThresholds => getCoreGameEngineNativeDrawThresholdRuntime().TracksNativeDrawThresholds;
    public bool TracksShanheS5PhaseSkips => getCoreGameEngineShanheS5ShensuRuntime().TracksShanheS5PhaseSkips;
    public bool TracksBoundaryZhuHuanPhaseSkips => getCharacterZhuHuanBoundaryMobileRuntime().TracksBoundaryZhuHuanPhaseSkips;

    public bool HasIssuedExtraDrawMarker(int seat) => getCoreLeastHandMarkerDrawProgramsRuntime().HasIssuedExtraDrawMarker(seat);
    public int RelativeTableOrder(int from, int target) => getCoreGameEngineTableOrderRuntime().RelativeTableOrder(from, target);
}
