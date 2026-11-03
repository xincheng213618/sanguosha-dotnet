#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeCardUseFramesRuntime
{
    internal void SetCardUseStep(long frameId, ResolutionFrameStep step)
    {
        var index = _resolutionStackOperations._resolutionStack.FindLastIndex(frame => frame.Id == frameId);
        if (index < 0 || _resolutionStackOperations._resolutionStack[index] is not CardUseFrame cardUse)
        {
            throw new InvalidOperationException($"Resolution frame {(frameId)} is not a CardUse frame.");
        }

        var updated = cardUse with { Step = step };
        _coreRuntimeOperations.ReplaceRuntimeFrame(updated.Id, updated);
        if (step == ResolutionFrameStep.Completed)
            _programUseResults.Capture(updated);
    }

    internal void RedirectCardUseTarget(long frameId, int targetIndex, int originalTargetSeat, int redirectedTargetSeat)
    {
        var index = _resolutionStackOperations._resolutionStack.FindLastIndex(frame => frame.Id == frameId);
        if (index < 0 || _resolutionStackOperations._resolutionStack[index] is not CardUseFrame cardUse)
        {
            throw new InvalidOperationException($"Resolution frame {(frameId)} is not a CardUse frame.");
        }

        var targetSeats = cardUse.TargetSeats.ToArray();
        // The native cursor identifies the occurrence, even when seats repeat.
        if (targetIndex != cardUse.TargetIndex || targetIndex < 0 || targetIndex >= targetSeats.Length ||
            targetSeats[targetIndex] != originalTargetSeat)
            throw new InvalidOperationException("The redirected Slash target is absent from its current native slot.");

        targetSeats[targetIndex] = redirectedTargetSeat;
        _coreRuntimeOperations.ReplaceRuntimeFrame(_resolutionStackOperations._resolutionStack[index].Id, cardUse with { TargetSeats = targetSeats });
    }
}
