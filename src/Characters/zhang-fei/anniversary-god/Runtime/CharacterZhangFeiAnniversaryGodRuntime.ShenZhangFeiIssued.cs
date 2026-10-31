#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangFeiAnniversaryGodRuntime
{
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 38600, SkillProgramEffectOp.ResolveShenZhangFeiPunishment, SkillProgramEffectOp.ResolveShenZhangFeiXunshi)]
    internal bool ShenZfActualTrigger(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => t.Effects[0].Op == SkillProgramEffectOp.ResolveShenZhangFeiPunishment ? CanRunIssuedShenZf(c, w) : ShenZfUseAction(w)is { } action && action.ActorSeat == c.OwnerSeat && ShenZfColorless(action);
    internal ProgramTriggerCandidate ShenZfFrameCandidate(ProgramSkillFrame f) => new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!.Triggers.Single(t => t.Id == f.TriggerId).Priority);
    internal ProgramTriggerCandidate ShenZfPenaltyCandidate(int seat, string binding, string? capturedHash = null)
    {
        var skill = capturedHash is null ? ShenZfIssuedSkill(seat) : new[]
        {
            ShenZhangFeiComposition.Shencai,
            ShenZhangFeiComposition.HappyShencai
        }.SingleOrDefault(id => _contentRegistryOperations._contentRegistry.Skills.TryGetValue(id, out var s) && s.Program?.GameplayHash == capturedHash);
        var p = _contentRegistryOperations._contentRegistry.GetSkill(skill ?? throw new InvalidOperationException("An issued Shencai punishment requires its exact registered family source.")).Program!;
        var t = p.Triggers.Single(t => t.Id == binding);
        return new(seat, p.Id, binding, ShenZfIssuedInstance(seat), p.GameplayHash, t.Priority);
    }

    internal static PlayerMarkerKind? ShenZfPenaltyMarker(string binding) => binding switch
    {
        "chi-damage" => PlayerMarkerKind.ShencaiChi,
        "tu-hand-loss" => PlayerMarkerKind.ShencaiTu,
        "liu-turn-ending" => PlayerMarkerKind.ShencaiLiu,
        "si-turn-ended" => PlayerMarkerKind.ShencaiSi,
        _ => null
    };
    internal bool ShenZfPenaltyExists(int seat, string binding) => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive && ShenZfPenaltyMarker(binding)is { } marker && ShenZfMark(seat, marker) > 0 && ShenZfIssued(seat);
    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 4600)]
    internal void AppendShenZfMovementCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> candidates)
    {
        foreach (var loss in _coreProgramEventHistoryOperations.ProgramEventHistory<ShenZhangFeiTuLossEvent>().Where(e => e.BatchId == batch.Id).DistinctBy(e => e.TargetSeat))
        {
            var seat = loss.TargetSeat;
            if (!_coreMatchParticipantsPlayersAccess._players[seat].IsAlive)
                continue;
            var c = ShenZfPenaltyCandidate(seat, "tu-hand-loss", loss.GameplayHash);
            if (!candidates.Any(p => p.OwnerSeat == seat && p.SkillId == c.SkillId && p.BindingId == c.BindingId && p.SkillInstanceId == c.SkillInstanceId))
                candidates.Add(c);
        }
    }

    [ProgramCardUsePayment(ProgramCardUsePaymentStage.CaptureMovement, 38600)]
    internal void CaptureShenZfTuLoss(CardMovementBatchContext batch)
    {
        foreach (var seat in batch.Movements.Where(m => m.From.Zone == CardZoneKind.Hand && m.From.OwnerSeat is not null && m.To != m.From && !m.Reason.Value.StartsWith(ShenZfReason, StringComparison.Ordinal)).Select(m => m.From.OwnerSeat!.Value).Distinct())
            if (ShenZfPenaltyExists(seat, "tu-hand-loss"))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenZhangFeiTuLossEvent(batch.Id, seat, _contentRegistryOperations._contentRegistry.GetSkill(ShenZfIssuedSkill(seat)!).Program!.GameplayHash));
    }

    [ProgramPipelineHook(ProgramPipelinePhase.IncludeTriggerCandidate, 38600)]
    internal IReadOnlyList<ProgramTriggerCandidate> IncludeShenZfPenalties(CharacterState owner, SkillProgramTriggerWindow window, IReadOnlyList<ProgramTriggerCandidate> candidates)
    {
        var retained = candidates.Where(c => !ShenZhangFeiComposition.IsShencai(c.SkillId) || ShenZfPenaltyMarker(c.BindingId)is null).ToArray();
        var binding = window switch
        {
            SkillProgramTriggerWindow.AfterDamageApplied => "chi-damage",
            SkillProgramTriggerWindow.CardsMoved => "tu-hand-loss",
            SkillProgramTriggerWindow.TurnEnding => "liu-turn-ending",
            SkillProgramTriggerWindow.AfterTurnEnded => "si-turn-ended",
            _ => null
        };
        if (binding is null || !ShenZfPenaltyExists(owner.Seat, binding))
            return retained;
        return[..retained, ShenZfPenaltyCandidate(owner.Seat, binding)];
    }

    [ProgramPipelineHook(ProgramPipelinePhase.IssuedTriggerCandidate, 38600)]
    internal bool CanRunIssuedShenZf(ProgramTriggerCandidate c, ProgramSkillWindowContext w)
    {
        if (!ShenZhangFeiComposition.IsShencai(c.SkillId))
            return false;
        if (c.BindingId == "judgment-first-gain")
            return IsShenZfGainCandidate(c, w);
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(c.OwnerSeat) || !_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive || c.BindingId != "tu-hand-loss" && !ShenZfPenaltyExists(c.OwnerSeat, c.BindingId) || c.SkillInstanceId != ShenZfIssuedInstance(c.OwnerSeat) || c.GameplayHash != _contentRegistryOperations._contentRegistry.GetSkill(c.SkillId).Program!.GameplayHash || w.OwnerSeat != c.OwnerSeat)
            return false;
        if (c.BindingId == "chi-damage")
            return w.Window == SkillProgramTriggerWindow.AfterDamageApplied && w.TargetSeat == c.OwnerSeat && w.Amount is> 0 && _resolutionStackOperations._resolutionStack.OfType<DamageTriggerWindowFrame>().SingleOrDefault(p => p.Id == w.ParentFrameId)is { } d && d.TargetSeat == c.OwnerSeat && d.CandidateIndex < d.Candidates.Count && d.Candidates[d.CandidateIndex].ProgramId == c.SkillId && d.Candidates[d.CandidateIndex].ProgramTriggerId == c.BindingId && d.Candidates[d.CandidateIndex].SkillInstanceId == c.SkillInstanceId;
        if (c.BindingId == "tu-hand-loss")
            return w.Window == SkillProgramTriggerWindow.CardsMoved && w.MovementBatch is { } b && _coreProgramEventHistoryOperations.ProgramEventHistory<ShenZhangFeiTuLossEvent>().Count(e => e.BatchId == b.Id && e.TargetSeat == c.OwnerSeat && e.GameplayHash == c.GameplayHash) == 1 && _resolutionStackOperations._resolutionStack.OfType<CardsMovedTriggerWindowFrame>().SingleOrDefault(p => p.Id == w.ParentFrameId)is { } moved && moved.Batch.Id == b.Id && moved.CandidateIndex < moved.Candidates.Count && moved.Candidates[moved.CandidateIndex] == c && b.Movements.Any(m => m.From == CardLocation.Hand(c.OwnerSeat) && m.To != m.From && !m.Reason.Value.StartsWith(ShenZfReason, StringComparison.Ordinal));
        if (c.BindingId == "liu-turn-ending")
            return w.Window == SkillProgramTriggerWindow.TurnEnding && c.OwnerSeat == _coreActualTurnProgressCurrentSeatAccess._currentSeat && _resolutionStackOperations._resolutionStack.OfType<TurnEndingBoundaryFrame>().SingleOrDefault(p => p.Id == w.ParentFrameId)is { } ending && ending.OwnerSeat == c.OwnerSeat && ending.ItemIndex < ending.Items.Count && ending.Items[ending.ItemIndex].Candidate == c;
        if (c.BindingId == "si-turn-ended")
            return w.Window == SkillProgramTriggerWindow.AfterTurnEnded && c.OwnerSeat == _coreActualTurnProgressCurrentSeatAccess._currentSeat && ShenZfMark(c.OwnerSeat, PlayerMarkerKind.ShencaiSi) > _coreMatchParticipantsPlayersAccess._players.Count(p => p.IsAlive) && _resolutionStackOperations._resolutionStack.OfType<DeferredTurnEndFrame>().SingleOrDefault(p => p.Id == w.ParentFrameId)is { } ended && _coreAfterTurnEndedProgramsOperations.IsActualAfterTurnEndedParent(ended) && _coreAfterTurnEndedProgramsOperations.AfterTurnEndedCandidate(ended) == c;
        return false;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.AppendTurnEndingItems, 38600)]
    internal void AppendShenZfEnding(List<TurnEndingBoundaryItem> items)
    {
        if (!ShenZfPenaltyExists(_coreActualTurnProgressCurrentSeatAccess._currentSeat, "liu-turn-ending"))
            return;
        var c = ShenZfPenaltyCandidate(_coreActualTurnProgressCurrentSeatAccess._currentSeat, "liu-turn-ending");
        if (!items.Any(i => i.Candidate == c))
            items.Add(new(TurnEndingBoundaryItemKind.Program, c.Priority, $"shencai-liu:{(c.OwnerSeat)}", c, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat])));
    }

    internal IReadOnlyList<TurnEndingBoundaryItem> IncludeShenZfAfterTurnEndedItems(DeferredTurnEndFrame p, IReadOnlyList<TurnEndingBoundaryItem> items)
    {
        if (!ShenZfPenaltyExists(p.OwnerSeat, "si-turn-ended"))
            return items;
        var c = ShenZfPenaltyCandidate(p.OwnerSeat, "si-turn-ended");
        if (items.Any(i => i.Candidate == c))
            return items;
        return Array.AsReadOnly(items.Append(new(TurnEndingBoundaryItemKind.Program, c.Priority, $"shencai-si:{(c.OwnerSeat)}", c, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat]))).OrderBy(i => _coreTableOrderOperations.RelativeTableOrder(p.OwnerSeat, i.Candidate!.OwnerSeat)).ThenByDescending(i => i.Priority).ThenBy(i => i.StableIdentity, StringComparer.Ordinal).ToArray());
    }
}
