namespace CardGame.Core;

internal interface IProgramBoundaryInvariantFacts : IProgramSelectionQuery
{
    int RoundNumber { get; }
    ActualTurnKind TurnKind { get; }
    DecisionKind? PendingDecisionKind { get; }
    int SkipDrawPhaseMask { get; }
    bool IsTerminalPhaseHandDebtBoundary(ProgramLifecycleTriggerWindowFrame frame);
    bool IsExactHellAppearanceWindow(ProgramLifecycleTriggerWindowFrame frame);
    bool IsExactShanheMarketOpeningWindow(ProgramLifecycleTriggerWindowFrame frame);
}

public sealed partial class GameEngine
{
}
