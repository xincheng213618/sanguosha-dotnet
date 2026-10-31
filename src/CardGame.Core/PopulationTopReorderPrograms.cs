namespace CardGame.Core;

public sealed record ProgramPopulationThresholdCount(int Threshold, int BelowAmount);
/// <summary>Exact owning instruction and actual turn for a population-count viewing; no card sidecar.</summary>
public sealed record ProgramPopulationTopReorder(int InstructionIndex, int TurnNumber, int TurnSeat,
    int RequestedCount, string? AllBottomStateId);

public interface IPopulationTopReorderProgramEffectHost
{
    SkillProgramStepOutcome ReorderTopCardsByPopulation(ProgramSkillFrame frame, int maximumCards,
        ProgramPopulationThresholdCount population, string? allBottomStateId);
}

public sealed partial class GameEngine
{
}
