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

        _coreRuntimeOperations.ReplaceRuntimeFrame(_resolutionStackOperations._resolutionStack[index].Id, cardUse with { Step = step });
    }

    internal void RedirectCardUseTarget(long frameId, int originalTargetSeat, int redirectedTargetSeat)
    {
        var index = _resolutionStackOperations._resolutionStack.FindLastIndex(frame => frame.Id == frameId);
        if (index < 0 || _resolutionStackOperations._resolutionStack[index] is not CardUseFrame cardUse)
        {
            throw new InvalidOperationException($"Resolution frame {(frameId)} is not a CardUse frame.");
        }

        var targetSeats = cardUse.TargetSeats.ToArray();
        // This batch's extra target can redirect onto another actual target. Use its
        // native cursor, because an earlier redirected seat may now be duplicated.
        var targetIndex = cardUse.ShortRangeSlashTarget is not null || _coreCurrentEnhancementSlashRedirectsOperations.HasCurrentEnhancementVirtualProducer(cardUse) || ((_characterQianZhaoOlOperations.HasWeifuTargetTail(cardUse) || ((_characterYangFuOlOperations.HasBingzhongTargetTail(cardUse) || _characterMaChengOlOperations.HasChenglieTargetTail(cardUse)) || _characterLiuPanOlOperations.HasPijingTargetTail(cardUse))) || _characterMaWanOlOperations.HasMaWanHunjiangTargetTail(cardUse) || (_characterZhangYanOlOperations.HasZhangYanLangdaoTargetTail(cardUse) || _characterMaXiuMaTieOlOperations.HasKenshangTargetTail(cardUse))) ? cardUse.TargetIndex : Array.IndexOf(targetSeats, originalTargetSeat);
        if (targetIndex < 0 || targetIndex >= targetSeats.Length || targetSeats[targetIndex] != originalTargetSeat)
        {
            throw new InvalidOperationException("The redirected Slash target is absent from its CardUse frame.");
        }

        targetSeats[targetIndex] = redirectedTargetSeat;
        _coreRuntimeOperations.ReplaceRuntimeFrame(_resolutionStackOperations._resolutionStack[index].Id, cardUse with { TargetSeats = targetSeats });
    }
}
