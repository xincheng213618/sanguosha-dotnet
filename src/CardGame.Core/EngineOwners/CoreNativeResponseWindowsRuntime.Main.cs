#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeResponseWindowsRuntime
{
    internal void PushResponseWindow(long parentFrameId, int sourceSeat, int responderSeat, CardKind incomingCard, CardKind requiredCardKind)
    {
        SetResponseParentStep(parentFrameId, ResolutionFrameStep.AwaitingResponse);
        _coreRuntimeOperations.PushRuntimeFrame(new ResponseWindowFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, parentFrameId, sourceSeat, responderSeat, incomingCard, RequiredCardKind: requiredCardKind));
    }

    internal void SetResponseParentStep(long frameId, ResolutionFrameStep step)
    {
        var index = _resolutionStackOperations._resolutionStack.FindLastIndex(frame => frame.Id == frameId);
        if (index < 0 || _resolutionStackOperations._resolutionStack[index] is not (CardUseFrame or ProgramSkillFrame or JudgmentFrame or ProgramJudgmentTriggerWindowFrame))
        {
            throw new InvalidOperationException($"Resolution frame {(frameId)} cannot own a response window.");
        }

        _coreRuntimeOperations.ReplaceRuntimeFrame(_resolutionStackOperations._resolutionStack[index].Id, _resolutionStackOperations._resolutionStack[index] with { Step = step });
    }

    internal void PopResponseWindow(long parentFrameId)
    {
        if (_resolutionStackOperations._resolutionStack.Count == 0 || _resolutionStackOperations._resolutionStack[^1] is not ResponseWindowFrame response || response.ParentFrameId != parentFrameId)
        {
            throw new InvalidOperationException($"Resolution stack does not have a response window for frame {(parentFrameId)}.");
        }

        _coreRuntimeOperations.PopResolutionFrame(response.Id, ResolutionFrameKind.ResponseWindow);
    }
}
