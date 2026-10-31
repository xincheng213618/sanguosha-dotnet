namespace CardGame.Core;

public sealed record ProgramBlindHandTakeReceipt(int InstructionIndex,
    IReadOnlyList<int> TargetSeats, IReadOnlyList<int> CardIds, IReadOnlyList<CardLocation> SourceLocations);
public interface IAwaitedBlindHandTakeProgramHost
{
    SkillProgramStepOutcome TakeRandomHandCardsAndAwait(ProgramSkillFrame frame, int amountPerTarget, CardMoveReason reason);
}
