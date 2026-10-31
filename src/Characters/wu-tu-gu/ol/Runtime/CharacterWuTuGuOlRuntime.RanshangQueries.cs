#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWuTuGuOlRuntime
{
    internal bool WuTuGuEndingParentMatches(ProgramSkillFrame f) => f.WindowContext is { Window: SkillProgramTriggerWindow.TurnEnding, TargetSeat: { } target, SourceSeat: { } source } c && target == f.OwnerSeat && source == f.OwnerSeat && f.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && _resolutionStackOperations._resolutionStack.OfType<TurnEndingBoundaryFrame>().SingleOrDefault(w => w.Id == c.ParentFrameId)is { } end && end.OwnerSeat == f.OwnerSeat && end.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && end.ItemIndex >= 0 && end.ItemIndex < end.Items.Count && end.Items[end.ItemIndex].Candidate is { } candidate && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, candidate);
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 27320)]
    internal void CaptureWuTuGuRanshangPayment(ProgramSkillHpLostEvent paid)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { WuTuGuRanshang: { Stage: WuTuGuRanshangStage.Issuing } r } f || paid.FrameId != f.Id || paid.SkillId != f.SkillId || paid.TargetSeat != f.OwnerSeat || paid.Amount != r.Amount)
            return;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { WuTuGuRanshang = r with { Stage = WuTuGuRanshangStage.HpChildren } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuTuGuRanshangPaidEvent(f.Id, paid.Amount, paid.RemainingHp));
    }
}
