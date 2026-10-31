#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineBoundCardMovementContinuationRuntime
{
    SkillProgramStepOutcome IBoundCardMovementContinuationHost.AwaitBoundCardMovements(long frameId, int ownerSeat) => AwaitProgramBoundCardMovements(frameId, ownerSeat);
    internal SkillProgramStepOutcome AwaitProgramBoundCardMovements(long frameId, int ownerSeat)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
        if (frame.OwnerSeat != ownerSeat)
            throw new InvalidOperationException("The bound movement lost its program owner.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { PendingMovementContinuation = new(ownerSeat, 0, null) });
        if (_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            return SkillProgramStepOutcome.AwaitChild;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { PendingMovementContinuation = null });
        return SkillProgramStepOutcome.Continue;
    }
}
