#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeDamageFramesRuntime
{
    internal long BeginDamage(long parentFrameId, int sourceSeat, int targetSeat, int amount, DamageNature nature = DamageNature.Normal)
    {
        var frameId = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        _coreRuntimeOperations.PushRuntimeFrame(new DamageFrame(frameId, parentFrameId, sourceSeat, targetSeat, amount, Nature: nature));
        _coreDynamicDiscardDamagePaymentsOperations.CaptureDynamicDiscardDamageFrame(frameId, parentFrameId, targetSeat);
        return frameId;
    }

    internal void SetDamageTriggerWindowStep(long frameId, ResolutionFrameStep step)
    {
        var index = _resolutionStackOperations._resolutionStack.FindLastIndex(frame => frame.Id == frameId);
        if (index < 0 || _resolutionStackOperations._resolutionStack[index] is not DamageTriggerWindowFrame triggerWindow)
        {
            throw new InvalidOperationException($"Resolution frame {(frameId)} is not a damage trigger window.");
        }

        _coreRuntimeOperations.ReplaceRuntimeFrame(_resolutionStackOperations._resolutionStack[index].Id, triggerWindow with { Step = step });
    }

    internal DamageTriggerWindowFrame SetDamageTriggerWindowCursor(long frameId, int candidateIndex)
    {
        var index = _resolutionStackOperations._resolutionStack.FindLastIndex(frame => frame.Id == frameId);
        if (index < 0 || _resolutionStackOperations._resolutionStack[index] is not DamageTriggerWindowFrame triggerWindow)
        {
            throw new InvalidOperationException($"Resolution frame {(frameId)} is not a damage trigger window.");
        }

        if (candidateIndex < 0 || candidateIndex > triggerWindow.Candidates.Count)
        {
            throw new ArgumentOutOfRangeException("candidateIndex");
        }

        var next = triggerWindow with
        {
            CandidateIndex = candidateIndex
        };
        _coreRuntimeOperations.ReplaceRuntimeFrame(_resolutionStackOperations._resolutionStack[index].Id, next);
        return next;
    }
}
