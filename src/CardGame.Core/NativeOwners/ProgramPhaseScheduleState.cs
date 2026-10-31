#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramPhaseScheduleState(global::CardGame.Core.EngineStates.ProgramPhaseScheduleStorage phaseScheduleStorage, global::CardGame.Core.EngineStates.CoreActualTurnProgressState actualTurnProgressState) : IProgramPhaseScheduleState
{
    public ProgramPhaseSchedule? Current { get => phaseScheduleStorage.Current; set => phaseScheduleStorage.Current = value; }

    public void RestorePhase(TurnPhase phase) => actualTurnProgressState.ReferenceToPhase = phase;
}
