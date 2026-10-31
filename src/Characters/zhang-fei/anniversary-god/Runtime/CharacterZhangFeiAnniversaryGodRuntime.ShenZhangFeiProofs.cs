#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangFeiAnniversaryGodRuntime
{
    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 38600)]
    internal bool ShenZfMovementProof(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending)
    {
        if (f.ShenZhangFei is not { Paid: true, CardId: > 0, From: { } from, To: { } to } r || effect?.Op != r.Operation || pending != f.PendingMovementContinuation || pending.SubjectSeat != f.OwnerSeat || pending.BeforeCount != 0 || pending.CoverageResultBind is not null || r.After <= r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.Stage is not (ShenZhangFeiStage.GainChildren or ShenZhangFeiStage.TakeChildren or ShenZhangFeiStage.PenaltyChildren or ShenZhangFeiStage.JudgmentCleanupChildren))
            return false;
        var reason = ShenZfReason + (r.Stage == ShenZhangFeiStage.GainChildren ? "judgment-gain" : r.Stage == ShenZhangFeiStage.TakeChildren ? "take" : r.Stage == ShenZhangFeiStage.JudgmentCleanupChildren ? "judgment-unclaimed" : "tu-discard");
        return _coreProgramEventHistoryOperations.ProgramEventHistory<ShenZhangFeiMovementPaidEvent>().Count(e => e == new ShenZhangFeiMovementPaidEvent(f.Id, r.Stage, r.CardId, from, to, r.Before, r.After)) == 1 && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.Before && m.Sequence == r.After && m.CardId == r.CardId && m.From == from && m.To == to && m.Reason.Value == reason) == 1 && (r.Stage == ShenZhangFeiStage.GainChildren ? f.TriggerId == "judgment-first-gain" && from == CardLocation.Judgment(r.TargetSeat) && to == CardLocation.Hand(f.OwnerSeat) : r.Stage == ShenZhangFeiStage.TakeChildren ? r.Operation == SkillProgramEffectOp.ResolveShenZhangFeiShencai && from.OwnerSeat == r.TargetSeat && from.Zone is CardZoneKind.Hand or CardZoneKind.Equipment or CardZoneKind.Judgment && to == (_coreAdvancedLifecycleOperations.GetAdvancedCard(r.CardId).IsGeneralWeapon && from.Zone == CardZoneKind.Equipment ? CardLocation.OutsideGame : CardLocation.Hand(f.OwnerSeat)) : r.Stage == ShenZhangFeiStage.JudgmentCleanupChildren ? from == CardLocation.Processing && to == CardLocation.DiscardPile && HasUnclaimedShenZfJudgmentResult(f, r.CardId, r.Before) : f.TriggerId == "tu-hand-loss" && from == CardLocation.Hand(f.OwnerSeat) && to == CardLocation.DiscardPile);
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 38600)]
    internal void RequireShenZf(ProgramSkillFrame f)
    {
        if (f.ShenZhangFei is not { } r)
            return;
        if (r.Source != ShenZfSource(f) || r.GameplayHash != f.GameplayHash || r.ParentId != (f.WindowContext?.ParentFrameId ?? 0) || f.InstructionIndex != 1 || !Enum.IsDefined(r.Stage) || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.TakeBeforeDeath != (f.SkillId == ShenZhangFeiComposition.HappyShencai && f.TriggerId is null ? (bool? )true : null) || _coreProgramEventHistoryOperations.ProgramEventHistory<ShenZhangFeiStartedEvent>().Count(e => e == new ShenZhangFeiStartedEvent(f.Id, r.Source, r.GameplayHash, r.Operation, r.ParentId, r.TargetSeat, r.Amount)) != 1)
            throw new InvalidOperationException("Shencai lost its original source, native occurrence, owning frame or once-paid instruction.");
        if (r.ParentId != 0)
        {
            var index = _resolutionStackOperations._resolutionStack.FindIndex(p => p.Id == f.Id);
            if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1].Id != r.ParentId || _resolutionStackOperations._resolutionStack[index - 1] switch
            {
                ProgramJudgmentTriggerWindowFrame w => w.CandidateIndex < 0 || w.CandidateIndex >= w.Candidates.Count || w.Candidates[w.CandidateIndex] != new ProgramJudgmentTriggerCandidate(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash),
                DamageTriggerWindowFrame w => w.CandidateIndex < 0 || w.CandidateIndex >= w.Candidates.Count || w.TargetSeat != f.OwnerSeat || w.Candidates[w.CandidateIndex].ProgramId != f.SkillId || w.Candidates[w.CandidateIndex].ProgramTriggerId != f.TriggerId || w.Candidates[w.CandidateIndex].SkillInstanceId != f.SkillInstanceId || f.WindowContext?.Amount != r.Amount,
                CardsMovedTriggerWindowFrame w => w.CandidateIndex < 0 || w.CandidateIndex >= w.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, w.Candidates[w.CandidateIndex]) || w.Batch.Id != r.BatchId || f.WindowContext?.MovementBatch?.Id != r.BatchId,
                TurnEndingBoundaryFrame w => w.ItemIndex < 0 || w.ItemIndex >= w.Items.Count || w.OwnerSeat != f.OwnerSeat || w.Items[w.ItemIndex].Candidate is not { } candidate || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, candidate),
                DeferredTurnEndFrame w => !_coreAfterTurnEndedProgramsOperations.IsActualAfterTurnEndedParent(w) || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, _coreAfterTurnEndedProgramsOperations.AfterTurnEndedCandidate(w)),
                _ => true
            })
                throw new InvalidOperationException("Shencai lost the exact suspended native parent candidate.");
        }

        if (r.Operation == SkillProgramEffectOp.ResolveShenZhangFeiShencai && f.TriggerId is null)
        {
            if (r.TakeBeforeDeath == true && r.Stage is ShenZhangFeiStage.TakeChoice or ShenZhangFeiStage.TakeChildren && _coreProgramEventHistoryOperations.ProgramEventHistory<ShenZhangFeiSentenceEvent>().Any(e => e.FrameId == f.Id))
                throw new InvalidOperationException("Happy Shencai cannot add a death mark before the real take and its original native children have returned.");
            if (f.SelectedTargetSeats is not [var seat] || seat != r.TargetSeat || seat == f.OwnerSeat || r.JudgmentId <= 0)
                throw new InvalidOperationException("Shencai lost its original other participant and native judgment.");
            if (r.Stage == ShenZhangFeiStage.Judging && !_resolutionStackOperations._resolutionStack.OfType<JudgmentFrame>().Any(j => j.Id == r.JudgmentId && IsShenZfJudgment(j, f)))
                throw new InvalidOperationException("Shencai lost its active native judgment child.");
            if (r.Stage != ShenZhangFeiStage.Judging && _coreProgramEventHistoryOperations.ProgramEventHistory<ShenZhangFeiJudgmentReturnedEvent>().Count(e => e == new ShenZhangFeiJudgmentReturnedEvent(f.Id, r.JudgmentId, r.ResultCardId, r.ResultKind)) != 1)
                throw new InvalidOperationException("Shencai lost its final returned judgment result.");
        }

        if (r.Operation == SkillProgramEffectOp.ResolveShenZhangFeiPunishment && (r.Amount < 1 || !r.Paid || _coreProgramEventHistoryOperations.ProgramEventHistory<ShenZhangFeiPunishmentPaidEvent>().Count(e => e == new ShenZhangFeiPunishmentPaidEvent(f.Id, f.TriggerId!, f.OwnerSeat, r.Amount, r.BatchId)) != 1 || r.Source.SkillInstanceId != ShenZfIssuedInstance(f.OwnerSeat) || !ShenZfIssued(f.OwnerSeat)))
            throw new InvalidOperationException("The issued punishment lost its actual occurrence and immutable once-paid receipt.");
        if (f.PendingMovementContinuation is { } p && !ShenZfMovementProof(f, ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).Instructions[0], p))
            throw new InvalidOperationException("Shencai lost its exact physical paid movement invoice.");
    }
}
