#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeDyingFramesRuntime
{
    internal void SetDyingFrameStep(long frameId, ResolutionFrameStep step)
    {
        var index = _resolutionStackOperations._resolutionStack.FindLastIndex(frame => frame.Id == frameId);
        if (index < 0 || _resolutionStackOperations._resolutionStack[index] is not DyingFrame dying)
        {
            throw new InvalidOperationException($"Resolution frame {(frameId)} is not a Dying frame.");
        }

        _coreRuntimeOperations.ReplaceRuntimeFrame(_resolutionStackOperations._resolutionStack[index].Id, dying with { Step = step });
    }

    internal DyingFrame UpdateDyingFrame(long frameId, Func<DyingFrame, DyingFrame> update)
    {
        var index = _resolutionStackOperations._resolutionStack.FindLastIndex(frame => frame.Id == frameId);
        if (index < 0 || _resolutionStackOperations._resolutionStack[index] is not DyingFrame dying)
            throw new InvalidOperationException($"Resolution frame {(frameId)} is not a Dying frame.");
        var next = update(dying);
        _coreRuntimeOperations.ReplaceRuntimeFrame(_resolutionStackOperations._resolutionStack[index].Id, next);
        return next;
    }
}
