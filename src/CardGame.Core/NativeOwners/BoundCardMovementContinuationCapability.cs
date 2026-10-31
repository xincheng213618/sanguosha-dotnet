#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundCardMovementContinuationCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineBoundCardMovementContinuationEntries> getCoreGameEngineBoundCardMovementContinuationRuntime) : IBoundCardMovementContinuationHost
{
    public SkillProgramStepOutcome AwaitBoundCardMovements(long frameId, int ownerSeat) => getCoreGameEngineBoundCardMovementContinuationRuntime().AwaitProgramBoundCardMovements(frameId, ownerSeat);
}
