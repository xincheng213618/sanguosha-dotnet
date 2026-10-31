namespace CardGame.Core;

internal interface IBoundCardMovementContinuationHost : IProgramRuntimeOperations
{
    SkillProgramStepOutcome AwaitBoundCardMovements(long frameId, int ownerSeat);
}
