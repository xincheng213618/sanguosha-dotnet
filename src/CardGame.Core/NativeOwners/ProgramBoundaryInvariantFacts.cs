#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramBoundaryInvariantFacts(global::CardGame.Core.EngineStates.CoreActualRoundProgressState actualRoundProgressState, global::CardGame.Core.EngineStates.CoreActualTurnProgressState actualTurnProgressState, global::System.Func<global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries> getCorePendingDecisionPolicyPipelineRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePhaseHandSeizureEntries> getCoreGameEnginePhaseHandSeizureRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineHellActivitiesEntries> getCoreGameEngineHellActivitiesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheMarketOpeningEntries> getCoreGameEngineShanheMarketOpeningRuntime) : IProgramBoundaryInvariantFacts
{
    public int RoundNumber => actualRoundProgressState.ReferenceToRoundNumber;
    public ActualTurnKind TurnKind => actualTurnProgressState.ReferenceToTurnProgression.Kind;
    public DecisionKind? PendingDecisionKind => getCorePendingDecisionPolicyPipelineRuntime()._pendingDecision?.Kind;
    public int SkipDrawPhaseMask => (int)global::CardGame.Core.GameEngine.DelayedTurnEffects.SkipDrawPhase;

    public bool IsTerminalPhaseHandDebtBoundary(ProgramLifecycleTriggerWindowFrame frame) => getCoreGameEnginePhaseHandSeizureRuntime().IsTerminalPhaseHandDebtBoundary(frame);
    public bool IsExactHellAppearanceWindow(ProgramLifecycleTriggerWindowFrame frame) => getCoreGameEngineHellActivitiesRuntime().IsExactHellAppearanceWindow(frame);
    public bool IsExactShanheMarketOpeningWindow(ProgramLifecycleTriggerWindowFrame frame) => getCoreGameEngineShanheMarketOpeningRuntime().IsExactShanheMarketOpeningWindow(frame);
}
