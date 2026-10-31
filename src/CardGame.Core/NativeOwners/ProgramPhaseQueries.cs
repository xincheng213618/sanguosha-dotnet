#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramPhaseQueries(global::CardGame.Core.EngineStates.CoreActualTurnProgressState actualTurnProgressState) : IProgramPhaseQueries
{
    public TurnPhase Phase => actualTurnProgressState.ReferenceToPhase;
}
