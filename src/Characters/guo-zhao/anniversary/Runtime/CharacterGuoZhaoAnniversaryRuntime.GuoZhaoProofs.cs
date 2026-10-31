#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuoZhaoAnniversaryRuntime
{
    internal bool GuoZhaoOriginal(ProgramSkillFrame f)
    {
        if (_contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash)
            return false;
        if (f.SkillId == AnniversaryGuoZhaoComposition.Zunwei)
            return f.TriggerId is null && f.WindowContext is null && f.ActivationId == "three-permanent-options" && _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSkillStartedEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.SkillId == f.SkillId && e.ActivationId == f.ActivationId) == 1;
        if (f.SkillId != AnniversaryGuoZhaoComposition.Pianchong || f.ActivationId != f.TriggerId || f.WindowContext is not { } context || context.OwnerSeat != f.OwnerSeat || _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramBindingStartedEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.SkillId == f.SkillId && e.SkillInstanceId == f.SkillInstanceId && e.BindingId == f.TriggerId && e.Window == context.Window) != 1)
            return false;
        if (f.TriggerId == "replace-draw-colors")
            return context.Window == SkillProgramTriggerWindow.DrawPhaseStarting && context.SourceSeat == f.OwnerSeat && context.TargetSeat == f.OwnerSeat && _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == context.ParentFrameId)is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.DrawPhaseStarting } w && w.OwnerSeat == f.OwnerSeat && w.CandidateIndex >= 0 && w.CandidateIndex < w.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, w.Candidates[w.CandidateIndex]);
        return f.TriggerId == "lost-color-benefit" && CanRunIssuedAnniversaryGuoZhaoCandidate(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex), context);
    }

    internal bool GuoZhaoDrawProof(ProgramSkillFrame f)
    {
        var r = f.AnniversaryGuoZhao!;
        if (r.Draw is not { } draw)
            return r.Stage != AnniversaryGuoZhaoStage.DrawChildren;
        if (draw.RecipientSeat != f.OwnerSeat || draw.RequestedCount != r.Amount || draw.SequenceBefore != r.Before || draw.SequenceAfter != r.After || draw.ActualDrawIds.Distinct().Count() != draw.ActualDrawIds.Count || draw.ActualDrawIds is not System.Collections.IList { IsReadOnly: true } || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryGuoZhaoDrawReturnedEvent>().Count(e => e == new AnniversaryGuoZhaoDrawReturnedEvent(f.Id, r.Cursor, r.Amount, draw.ActualDrawIds.Count, r.Before, r.After)) != 1)
            return false;
        var reason = r.Op == SkillProgramEffectOp.DrawAnniversaryGuoZhaoLostColor ? GuoZhaoColorDrawReason : GuoZhaoZunweiDrawReason;
        return draw.ActualDrawIds.All(id => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > draw.SequenceBefore && m.Sequence <= draw.SequenceAfter && m.CardId == id && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == reason) == 1);
    }

    internal bool GuoZhaoEquipmentProof(ProgramSkillFrame f)
    {
        var r = f.AnniversaryGuoZhao!;
        var issued = _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryGuoZhaoEquipmentIssuedEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Ordinal).ToArray();
        var returned = _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryGuoZhaoEquipmentReturnedEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Ordinal).ToArray();
        if (issued.Length > r.Cursor + 1 || returned.Length != r.Cursor || issued.Where((e, i) => e.Ordinal != i || e.UseId <= f.Id || e.ActionId <= 0 || !_coreEquipmentContentOperations.EquipmentRules.IsEquipment(e.Kind)).Any() || returned.Where((e, i) => e.Ordinal != i || issued.Length <= i || e.UseId != issued[i].UseId || e.ActionId != issued[i].ActionId || _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseFinishedEvent>().Count(done => done.ResolutionId == e.UseId && done.CardId == issued[i].CardId) != 1).Any())
            return false;
        if (r.Equipment is not { } paid)
            return r.Cursor == 0 && r.Stage is AnniversaryGuoZhaoStage.EquipmentNext or AnniversaryGuoZhaoStage.Completed;
        if (paid.Candidates.Count < 1 || paid.Candidates is not System.Collections.IList { IsReadOnly: true } || paid.Candidates.Distinct().Count() != paid.Candidates.Count || paid.Ordinal < 0 || paid.Ordinal != (r.Stage == AnniversaryGuoZhaoStage.NativeEquipment ? r.Cursor : r.Cursor - 1))
            return false;
        var random = new DeterministicRandom(unchecked((int)paid.RandomBefore));
        if (paid.Candidates[random.Next(paid.Candidates.Count)] != paid.CardId || random.State != paid.RandomAfter || issued.Count(e => e == new AnniversaryGuoZhaoEquipmentIssuedEvent(f.Id, paid.Ordinal, paid.CardId, paid.Kind, paid.UseId, paid.ActionId)) != 1)
            return false;
        return r.Stage == AnniversaryGuoZhaoStage.NativeEquipment ? issued.Length == r.Cursor + 1 : issued.Length == r.Cursor;
    }

    internal bool ValidGuoZhao(ProgramSkillFrame f)
    {
        if (f.AnniversaryGuoZhao is not { } r || !GuoZhaoOriginal(f) || r.Source != GuoZhaoSource(f) || r.Hash != f.GameplayHash || r.Turn != _coreActualTurnProgressTurnNumberAccess._turnNumber || r.ParentId != (f.WindowContext?.ParentFrameId ?? 0) || f.InstructionIndex is < 1 or > 2 || f.InstructionIndex == 2 && r.Stage != AnniversaryGuoZhaoStage.Completed || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || !Enum.IsDefined(r.Stage) || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.Cursor < 0 || r.Amount < 0 || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryGuoZhaoStartedEvent>().Count(e => e == new AnniversaryGuoZhaoStartedEvent(f.Id, r.Source, r.Hash, r.Op, r.ParentId, r.Turn)) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryGuoZhaoCompletedEvent>().Count(e => e.FrameId == f.Id) != (r.Stage == AnniversaryGuoZhaoStage.Completed ? 1 : 0) || r.Gains is not System.Collections.IList { IsReadOnly: true } || r.Benefits is not System.Collections.IList { IsReadOnly: true })
            return false;
        if (r.Op == SkillProgramEffectOp.ResolveAnniversaryGuoZhaoPianchong)
        {
            if (f.SkillId != AnniversaryGuoZhaoComposition.Pianchong || f.TriggerId != "replace-draw-colors" || r.Target != -1 || r.Benefits.Count != 0 || r.Draw is not null || r.Equipment is not null || r.Gains.Count > 2 || r.Gains.Select(g => g.IsRed).Distinct().Count() != r.Gains.Count || r.Gains.Any(g => !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == g.Sequence && m.CardId == g.CardId && m.Sequence > r.Before && m.Sequence <= r.After && m.From == CardLocation.DrawPile && m.Reason.Value == GuoZhaoObtainReason && (m.To == CardLocation.Hand(f.OwnerSeat) || _coreHappyWeiYanGainDestinationsOperations.IsHappyWeiYanRedirectedGain(m, CardLocation.Hand(f.OwnerSeat))))))
                return false;
            return r.Stage is AnniversaryGuoZhaoStage.GainChildren or AnniversaryGuoZhaoStage.ColorChoice && !r.Paid && r.Option == "" || r.Stage == AnniversaryGuoZhaoStage.Completed && r.Paid && r.Option is "lose-red" or "lose-black" && _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryGuoZhaoLeaseEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.Hash == r.Hash && e.Turn == r.Turn && e.LoseRed == (r.Option == "lose-red")) == 1;
        }

        if (r.Op == SkillProgramEffectOp.DrawAnniversaryGuoZhaoLostColor)
        {
            if (r.Benefits.Count == 0 || !r.Benefits.SequenceEqual(GuoZhaoLostBenefits(f.OwnerSeat, f.SkillInstanceId, f.GameplayHash, f.WindowContext!.MovementBatch)) || r.Cursor > r.Benefits.Count || r.Gains.Count != 0 || r.Equipment is not null || r.Paid || r.Option != "" || r.Target != -1)
                return false;
            var count = _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryGuoZhaoBenefitIssuedEvent>().Count(e => e.FrameId == f.Id);
            if (r.Stage == AnniversaryGuoZhaoStage.Completed)
                return count == r.Cursor;
            if (r.Cursor >= r.Benefits.Count || r.Stage is not (AnniversaryGuoZhaoStage.Drawing or AnniversaryGuoZhaoStage.DrawChildren))
                return false;
            var benefit = r.Benefits[r.Cursor];
            return r.Amount == benefit.Count && r.DrawRed == benefit.DrawRed && count == r.Cursor + 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryGuoZhaoBenefitIssuedEvent>().Count(e => e == new AnniversaryGuoZhaoBenefitIssuedEvent(f.Id, r.Cursor, benefit.LeaseId, benefit.Count, benefit.DrawRed)) == 1 && GuoZhaoDrawProof(f);
        }

        if (r.Op != SkillProgramEffectOp.ResolveAnniversaryGuoZhaoZunwei || f.SkillId != AnniversaryGuoZhaoComposition.Zunwei || r.Gains.Count != 0 || r.Benefits.Count != 0 || r.DrawRed is not null)
            return false;
        var payments = _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryGuoZhaoOptionPaidEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (!r.Paid)
            return r.Stage == AnniversaryGuoZhaoStage.OptionChoice && r.Target == -1 && r.Option == "" && payments.Length == 0 && r.Draw is null && r.Equipment is null;
        if (payments is not [var paid] || paid.Source != r.Source || paid.Hash != r.Hash || paid.Target != r.Target || paid.Option != r.Option || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryGuoZhaoOptionPaidEvent>().Count(e => e.Source.OwnerSeat == f.OwnerSeat && e.Option == r.Option) != 1 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.Target) || r.Target == f.OwnerSeat)
            return false;
        if (r.Option == "hand")
            return r.Stage is AnniversaryGuoZhaoStage.Drawing or AnniversaryGuoZhaoStage.DrawChildren or AnniversaryGuoZhaoStage.Completed && r.Amount == paid.Amount && r.Amount <= 5 && r.Equipment is null && GuoZhaoDrawProof(f);
        if (r.Option == "equipment")
            return r.Stage is AnniversaryGuoZhaoStage.EquipmentNext or AnniversaryGuoZhaoStage.NativeEquipment or AnniversaryGuoZhaoStage.EquipmentChildren or AnniversaryGuoZhaoStage.Completed && r.Draw is null && GuoZhaoEquipmentProof(f);
        return r.Option == "hp" && r.Stage is AnniversaryGuoZhaoStage.RecoveryChildren or AnniversaryGuoZhaoStage.Completed && r.Draw is null && r.Equipment is null && (r.Cursor == 0 ? r.Amount == 0 && !_coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryGuoZhaoRecoveryIssuedEvent>().Any(e => e.FrameId == f.Id) : r.Cursor == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryGuoZhaoRecoveryIssuedEvent>().Count(e => e.FrameId == f.Id && e.Amount == r.Amount && e.Amount == Math.Max(0, e.TargetHp - e.OwnerHp)) == 1);
    }

    internal bool HasAnniversaryGuoZhaoPrompt() => _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { AnniversaryGuoZhao: { Stage: AnniversaryGuoZhaoStage.ColorChoice or AnniversaryGuoZhaoStage.OptionChoice }, Step: ResolutionFrameStep.AwaitingResponse } f && _corePendingDecisionPolicyPipelineOperations._pendingDecision is { Kind: DecisionKind.ProgramTrigger } p && p.PlayerSeat == f.OwnerSeat && ValidGuoZhao(f) && global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(p.Choices, GuoZhaoChoices(f));
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 44300)]
    internal void AssertGuoZhao(ProgramSkillFrame f)
    {
        if (f.AnniversaryGuoZhao is not null && !ValidGuoZhao(f))
            throw new InvalidOperationException($"Guo Zhao lost original frame, color lease, permanent option or native receipt ({(f.Id)}/{(f.SkillId)}/{(f.AnniversaryGuoZhao.Stage)}/{(f.InstructionIndex)}).");
    }

    [ProgramMovementProof(ProgramMovementProofPhase.NativeCost, 44300)]
    internal bool GuoZhaoMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => f.AnniversaryGuoZhao is { Stage: AnniversaryGuoZhaoStage.GainChildren or AnniversaryGuoZhaoStage.DrawChildren or AnniversaryGuoZhaoStage.RecoveryChildren or AnniversaryGuoZhaoStage.NativeEquipment or AnniversaryGuoZhaoStage.EquipmentChildren } r && ValidGuoZhao(f) && e?.Op == r.Op && f.PendingMovementContinuation == p && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 44300)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 44300)]
    internal bool GuoZhaoRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => f.AnniversaryGuoZhao is { Paid: true, Option: "hp", Cursor: 1, Stage: AnniversaryGuoZhaoStage.RecoveryChildren } r && ValidGuoZhao(f) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return == new RecoveryReplacementReturn(PostEventContinuation.Program, f.Id) && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.Program && recovery.Attempt.Completion.InstructionIndex == f.InstructionIndex && recovery.Attempt.SourceSeat == f.OwnerSeat && recovery.Attempt.TargetSeat == f.OwnerSeat && recovery.Attempt.Amount == r.Amount;
    internal bool GuoZhaoFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.AnniversaryGuoZhao is not { } r || !ValidGuoZhao(f))
            return false;
        if (child is NativeDrawAttemptFrame draw)
            return r.Stage == AnniversaryGuoZhaoStage.Drawing && draw.Context.ParentFrameId == f.Id && draw.Context.RecipientSeat == f.OwnerSeat && draw.Context.RequestedCount == r.Amount && global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(draw, f);
        if (child is CardUseFrame use)
            return IsAnniversaryGuoZhaoNativeEquipment(use, f);
        if (child is HappyQiaoHealthChoiceFrame)
            return r.Option == "hp" && r.Stage == AnniversaryGuoZhaoStage.RecoveryChildren && _characterDaQiaoXiaoQiaoHappyGodOperations.HappyQiaoHealthChoiceRidesOn(child, f);
        if (child is CardsMovedTriggerWindowFrame moved)
            return f.PendingMovementContinuation is not null && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
        if (child is RecoveryReplacementFrame recovery)
            return GuoZhaoRecoveryParent(f, recovery);
        if (child is HpChangedTriggerWindowFrame hp)
            return r.Stage == AnniversaryGuoZhaoStage.RecoveryChildren && r.Cursor == 1 && hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && hp.Continuation == PostEventContinuation.Program && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == f.OwnerSeat && hp.Change.TargetSeat == f.OwnerSeat && hp.Change.Amount <= r.Amount;
        return child is ProgramLifecycleTriggerWindowFrame skills && skills.Window == SkillProgramTriggerWindow.SkillsChanged && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal bool GuoZhaoObserverSuffix(int root) => _coreObserverSubtreesOperations.HasObserverSuffix(root, (i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreDeckEquipmentProgramsOperations.RandomEquipmentFrameRidesOn(_resolutionStackOperations2._resolutionStack()[i], _resolutionStackOperations2._resolutionStack()[i - 1]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[i], _resolutionStackOperations2._resolutionStack()[i - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[i], _resolutionStackOperations2._resolutionStack()[i - 1]), (i, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, dying));
    internal ProgramSkillFrame? GuoZhaoObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterGuoZhaoAnniversaryOperations.GuoZhaoFirstChild(f, child), (i) => _characterGuoZhaoAnniversaryOperations.GuoZhaoObserverSuffix(i));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 44300)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 44300)]
    internal bool GuoZhaoDamageObserver(long id) => GuoZhaoObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 44300)]
    internal bool GuoZhaoBeforeDamageObserver(long id) => GuoZhaoObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 44300)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 44300)]
    internal bool GuoZhaoDyingObserver() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && GuoZhaoObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 44300)]
    internal bool AdvanceGuoZhaoSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && GuoZhaoObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 44300)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 44300)]
    internal bool KeepGuoZhaoPaid(ProgramSkillFrame f) => f.AnniversaryGuoZhao is { } r ? (r.Paid || r.Op != SkillProgramEffectOp.ResolveAnniversaryGuoZhaoZunwei) && ValidGuoZhao(f) : f.TriggerId == "lost-color-benefit" && f.WindowContext is { } c && CanRunIssuedAnniversaryGuoZhaoCandidate(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, c.OccurrenceIndex), c);
}
