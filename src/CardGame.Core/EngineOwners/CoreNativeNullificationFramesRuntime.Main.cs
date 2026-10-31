#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeNullificationFramesRuntime
{
    internal NullificationWindowFrame ReplaceNullificationWindowFrame(NullificationWindowFrame next)
    {
        var index = _resolutionStackOperations._resolutionStack.FindLastIndex(frame => frame.Id == next.Id);
        if (index < 0 || _resolutionStackOperations._resolutionStack[index] is not NullificationWindowFrame)
        {
            throw new InvalidOperationException($"Resolution frame {(next.Id)} is not a Nullification window.");
        }

        _coreRuntimeOperations.ReplaceRuntimeFrame(_resolutionStackOperations._resolutionStack[index].Id, next);
        return next;
    }

    internal static int? GetNullificationTargetSeat(NullificationWindowFrame frame) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_CommonRules.GetNullificationTargetSeat(frame);
    internal Card GetNullificationEffectCard(NullificationWindowFrame frame) => frame.DelayedReturn is { OriginalUseLightning: not null } delayed && _coreOriginalUseLightningOperations.ValidOriginalUseLightningDelayed(delayed) ? new(0, CardKind.Lightning, Suit.None, 0) : _coreNativeSlashTargetDuelOperations.TryGetNativeSlashTargetDuel(frame, out var nativeUse, out _, out _) ? _coreNativeSlashTargetDuelOperations.NativeSlashTargetDuelRepresentation(nativeUse) : _coreDamageColorDuelProgramsOperations.GetTrickRepresentation(frame.ParentFrameId, frame.EffectCardId);
    internal void SetCardUseTargetIndex(long frameId, int targetIndex)
    {
        var index = _resolutionStackOperations._resolutionStack.FindLastIndex(frame => frame.Id == frameId);
        if (index < 0 || _resolutionStackOperations._resolutionStack[index] is not CardUseFrame cardUse)
        {
            throw new InvalidOperationException($"Resolution frame {(frameId)} is not a CardUse frame.");
        }

        if (targetIndex < 0 || targetIndex > cardUse.TargetSeats.Count)
        {
            throw new ArgumentOutOfRangeException("targetIndex");
        }

        _coreRuntimeOperations.ReplaceRuntimeFrame(_resolutionStackOperations._resolutionStack[index].Id, cardUse with { TargetIndex = targetIndex });
    }
}
