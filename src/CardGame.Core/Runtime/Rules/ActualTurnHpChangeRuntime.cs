using CardGame.Core.EngineStates.Contracts;

namespace CardGame.Core;

internal interface IActualTurnHpChangeCapture : IProgramRuntimeOperations
{
    bool Tracks { get; }
    bool Record(int seat, int before, int after);
}

[ProgramRuntimeModule, ProgramCapability]
internal sealed class ActualTurnHpChangeRuntime(INativeFactDemand demand,
    ICoreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess turns) : IActualTurnHpChangeCapture
{
    public bool Tracks => demand.RequiresFact(ProgramTrackedFact.FirstActualTurnHpChange);

    public bool Record(int seat, int before, int after)
    {
        if (before == after || !Tracks || turns._phase is TurnPhase.NotStarted or TurnPhase.Finished)
            return false;

        var turn = turns._turnProgression;
        if (turn.TurnNumber <= 0 || turn.HpChangedSeats?.Contains(seat) == true)
            return false;

        turns._turnProgression = turn with { HpChangedSeats = Array.AsReadOnly<int>([.. turn.HpChangedSeats ?? [], seat]) };
        return true;
    }
}
