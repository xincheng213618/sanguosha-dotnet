#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineBoundCardMovementContinuationEntries
{
    internal SkillProgramStepOutcome AwaitProgramBoundCardMovements(long frameId, int ownerSeat) => _resolve().AwaitProgramBoundCardMovements(frameId, ownerSeat);
}
