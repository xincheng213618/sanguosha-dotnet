#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramPhaseScheduleState(EngineStates.ProgramPhaseScheduleStorage phaseScheduleStorage, EngineStates.CoreActualTurnProgressState actualTurnProgressState) : IProgramPhaseScheduleState
{
    public ProgramPhaseSchedule? Current { get => phaseScheduleStorage.Current; set => phaseScheduleStorage.Current = value; }

    public void RestorePhase(TurnPhase phase) => actualTurnProgressState.ReferenceToPhase = phase;
}
