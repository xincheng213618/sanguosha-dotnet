#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundCardMovementContinuationCapability(Func<RuntimeEntries.CoreGameEngineBoundCardMovementContinuationEntries> getCoreGameEngineBoundCardMovementContinuationRuntime) : IBoundCardMovementContinuationHost
{
    public SkillProgramStepOutcome AwaitBoundCardMovements(long frameId, int ownerSeat) => getCoreGameEngineBoundCardMovementContinuationRuntime().AwaitProgramBoundCardMovements(frameId, ownerSeat);
}
