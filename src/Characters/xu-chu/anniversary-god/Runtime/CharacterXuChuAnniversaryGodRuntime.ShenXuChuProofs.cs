#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXuChuAnniversaryGodRuntime
{
    [ProgramMovementProof(ProgramMovementProofPhase.NativeCost, 38800)]
    internal bool IsZhuangpoPaidMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p)
    {
        if (f.ShenXuChu is not { Operation: SkillProgramEffectOp.ResolveZhuangpoTarget, Stage: ShenXuChuStage.DiscardChildren } r || e?.Op != r.Operation || p.SubjectSeat != r.TargetSeat || p.BeforeCount != 0 || p.CoverageResultBind is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<ZhuangpoDiscardPaidEvent>().Where(x => x.FrameId == f.Id && x.TargetSeat == r.TargetSeat && x.Before == r.Before).ToArray()is not [var paid] || paid.After != r.After || paid.Count != r.RequiredDiscards || r.Selected.Count != paid.Count)
            return false;
        return r.Selected.All(id => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > paid.Before && m.Sequence <= paid.After && m.CardId == id && m.From.OwnerSeat == r.TargetSeat && m.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && m.To == CardLocation.DiscardPile && m.Reason.Value == ZhuangpoDiscardReason(f)) == 1);
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 38800)]
    internal void RequireShenXuChu(ProgramSkillFrame f)
    {
        if (f.ShenXuChu is not { } r)
            return;
        if (r.Source != ShenXuChuSource(f) || r.GameplayHash != f.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash || r.InstructionIndex != f.InstructionIndex || r.ParentId != f.WindowContext?.ParentFrameId || !Enum.IsDefined(r.Stage) || r.Draws is not System.Collections.IList { IsReadOnly: true } || r.Selected is not System.Collections.IList { IsReadOnly: true } || r.Targets is not System.Collections.IList { IsReadOnly: true } || r.DrawIndex < 0 || r.DrawIndex > r.Draws.Count || r.Draws.Any(d => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(d.Seat) || d.Count is < 1 or > 5) || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.PaidMarks < 0 || r.RequiredDiscards < 0 || r.RequiredDiscards > r.PaidMarks || r.Selected.Distinct().Count() != r.Selected.Count || r.Selected.Count > r.RequiredDiscards)
            throw new InvalidOperationException("Shen Xu Chu lost its immutable owning frame, original content or native payment cursor.");
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1].Id != r.ParentId)
            throw new InvalidOperationException("Shen Xu Chu lost its suspended original parent.");
        if (r.Operation == SkillProgramEffectOp.SettleZhengqingRound)
        {
            // RNG proof stays on the trusted owning frame. Public history
            // exposes the settled result without revealing later secret rolls.
            var random = new DeterministicRandom(unchecked((int)r.RandomBefore));
            var winner = ZhengqingWinner(r.MaximumSeats, f.OwnerSeat, random);
            if (!ShenXuChuComposition.IsZhengqing(f.SkillId) || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.RoundEnding } parent || parent.CandidateIndex >= parent.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, parent.Candidates[parent.CandidateIndex]) || _resolutionStackOperations._resolutionStack.FirstOrDefault()is not RoundEndingBoundaryFrame boundary || _coreProgramEventHistoryOperations.ProgramEventHistory<ZhengqingSettledEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var settled] || settled.Source != r.Source || settled.Round != boundary.RoundNumber || settled.Round != _coreActualRoundProgressRoundNumberAccess._roundNumber || settled.WinnerSeat != r.TargetSeat || settled.MaximumDamage != r.PaidMarks || r.RandomBefore == 0 || random.State != r.RandomAfter || winner != r.TargetSeat || r.MaximumSeats is not System.Collections.IList { IsReadOnly: true } || r.MaximumSeats.Distinct().Count() != r.MaximumSeats.Count || r.MaximumSeats.Any(s => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(s)) || (r.PaidMarks == 0) != (r.MaximumSeats.Count == 0) || !r.Draws.SequenceEqual(_characterXuChuHappyGodOperations.ShenXuChuRoundDraws(f, r.TargetSeat, r.PaidMarks)))
                throw new InvalidOperationException("Zhengqing lost its settled actual round and fixed draw entitlement.");
        }
        else if (r.Operation == SkillProgramEffectOp.ResolveZhuangpoTarget)
        {
            if (!ShenXuChuComposition.IsZhuangpo(f.SkillId) || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramCardTriggerWindowFrame parent || f.WindowContext?.Window != SkillProgramTriggerWindow.CardUseTargetsFinalized || parent.ParentFrameId != r.UseId || parent.Action.ActionId != r.ActionId || parent.CandidateIndex >= parent.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(parent.Candidates[parent.CandidateIndex])) || r.Targets.Count == 0 || r.Targets.Distinct().Count() != r.Targets.Count || r.Targets.Any(s => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(s) || s == f.OwnerSeat) || r.TargetIndex < 0 || r.TargetIndex > r.Targets.Count || r.TargetIndex < r.Targets.Count && r.TargetSeat != r.Targets[r.TargetIndex] || !r.Targets.SequenceEqual(parent.Action.TargetSeats) || _coreProgramEventHistoryOperations.ProgramEventHistory<ZhuangpoTargetIssuedEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var issued] || issued.Source != r.Source || issued.GameplayHash != r.GameplayHash || issued.UseId != r.UseId || issued.ActionId != r.ActionId || issued.TargetMarked != r.TargetMarked || !r.Draws.SequenceEqual(global::CardGame.Core.EngineOwners.CharacterXuChuHappyGodRuntime.ShenXuChuTargetDraws(f, r.TargetMarked)))
                throw new InvalidOperationException("Zhuangpo lost its original converted Duel designation and fixed marked-target benefit.");
            if (r.Stage is ShenXuChuStage.DiscardChoice or ShenXuChuStage.DiscardChildren && _coreProgramEventHistoryOperations.ProgramEventHistory<ZhuangpoMarksPaidEvent>().Count(e => e.FrameId == f.Id && e.TargetSeat == r.TargetSeat && e.OwnerSeat == f.OwnerSeat && e.Removed == r.PaidMarks && e.Removed > 0 && e.Before - e.After == e.Removed && e.After >= 0) != 1)
                throw new InvalidOperationException("Zhuangpo's forced discard lost its actual paid Qing marker receipt.");
        }
        else
            throw new InvalidOperationException("Unknown Shen Xu Chu operation.");
        var returns = _coreProgramEventHistoryOperations.ProgramEventHistory<ShenXuChuDrawReturnedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        var count = r.Stage == ShenXuChuStage.DrawChildren ? r.DrawIndex + 1 : r.DrawIndex;
        if (returns.Length != count || returns.Select(e => e.Index).Distinct().Count() != returns.Length || returns.Any(e => e.Index < 0 || e.Index >= r.Draws.Count || r.Draws[e.Index] != new ShenXuChuDraw(e.Seat, e.Requested) || e.Actual < 0 || e.Actual > e.Requested || e.Before < 0 || e.After < e.Before || e.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence))
            throw new InvalidOperationException("Shen Xu Chu changed its once-only native draw returns.");
    }
}
