#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramPhaseQueries(EngineStates.CoreActualTurnProgressState actualTurnProgressState) : IProgramPhaseQueries
{
    public TurnPhase Phase => actualTurnProgressState.ReferenceToPhase;
}
