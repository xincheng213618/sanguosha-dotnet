#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoTengOlRuntime
{
    internal bool ValidCaoTeng(ProgramSkillFrame f)
    {
        if (f.CaoTeng is not { } r || !HasCaoTengCapability || f.InstructionIndex != 1 || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != r.Op || r.Source != CaoTengSource(f) || r.GameplayHash != f.GameplayHash || r.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || !Enum.IsDefined(r.Stage) || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.PendingMovementContinuation is not null || f.WindowContext?.ParentFrameId != r.ParentId || !CaoTengParentMatches(f) || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CaoTengStartedEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var start] || start != new CaoTengStartedEvent(f.Id, r.Source, r.GameplayHash, r.Op, r.ActualTurn, r.ParentId))
            return false;
        if (r.Op == SkillProgramEffectOp.ResolveQingliuFactionChoice)
            return f.SkillId == "ol:qingliu" && r.Materials.Count == 0 && r.TargetSeat is null && r.Cursor == 0 && r.Stage is (CaoTengStage.FactionChoice or CaoTengStage.FactionApplying) && (r.Stage == CaoTengStage.FactionChoice ? r.Faction is null : _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CaoTengFactionChosenEvent>().Count(e => e.FrameId == f.Id && e.Faction == r.Faction) == 1);
        if (r.Op == SkillProgramEffectOp.ResolveYongzuDistinctBenefits)
        {
            if (f.SkillId != "ol:yongzu" || r.Materials.Count != 0 || r.Cursor is < 0 or > 1 || r.Stage is not (CaoTengStage.YongzuTarget or CaoTengStage.YongzuChoice or CaoTengStage.YongzuChildren))
                return false;
            var targets = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CaoTengRecipientChosenEvent>().Where(e => e.FrameId == f.Id).ToArray();
            if (r.Stage == CaoTengStage.YongzuTarget)
                return r.TargetSeat is null && targets.Length == 0 && r.FirstOption is null && r.Option is null;
            if (r.TargetSeat is not { } target || target == f.OwnerSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || targets is not [var chosen] || chosen.TargetSeat != target || chosen.SameFaction != r.SameFaction)
                return false;
            var issued = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CaoTengBenefitIssuedEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Index).ToArray();
            if (issued.Length != r.Cursor + (r.Stage == CaoTengStage.YongzuChildren ? 1 : 0) || issued.Where((e, i) => e.Index != i || e.RecipientSeat != (i == 0 ? f.OwnerSeat : target) || e.Option is not ("draw" or "recover" or "restore" or "limit" or "skill") || !r.SameFaction && e.Option is ("limit" or "skill") || e.RecoveryRequested is < 0 or > 1 || e.Before < 0).Any() || issued.Select(e => e.Option).Distinct().Count() != issued.Length)
                return false;
            if (r.Cursor == 1 && (issued[0].Option != r.FirstOption || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CaoTengBenefitAppliedEvent>().Count(e => e.FrameId == f.Id && e.Index == 0) != 1))
                return false;
            return r.Stage == CaoTengStage.YongzuChoice ? r.Option is null : r.Option == issued[^1].Option && r.Before == issued[^1].Before && r.RecoveryRequested == issued[^1].RecoveryRequested && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CaoTengBenefitAppliedEvent>().Count(e => e.FrameId == f.Id && e.Index == r.Cursor && e.After == r.After) == 1;
        }

        if (r.Op != SkillProgramEffectOp.ResolveCaoTengJianxiongDrawAndClaim || f.SkillId != "ol:caoteng-jianxiong" || r.Stage is not (CaoTengStage.JianxiongDrawChildren or CaoTengStage.JianxiongClaimChildren) || r.TargetSeat is not null || r.Cursor != 0 || r.Materials.Select(m => m.CardId).Distinct().Count() != r.Materials.Count)
            return false;
        var materials = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CaoTengDamageMaterialFrozenEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (materials.Length != r.Materials.Count || materials.Where((e, i) => e.Index != i || e.Material != r.Materials[i]).Any() || r.Materials.Any(m => !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(v => v.CardId == m.CardId && v.CardKind == m.Kind && v.Sequence == m.EntrySequence && v.To == CardLocation.Processing)) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CaoTengJianxiongDrawIssuedEvent>().Count(e => e.FrameId == f.Id) != 1)
            return false;
        return r.Stage == CaoTengStage.JianxiongDrawChildren ? _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CaoTengJianxiongClaimIssuedEvent>().All(e => e.FrameId != f.Id) : _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CaoTengJianxiongClaimIssuedEvent>().Count(e => e.FrameId == f.Id && e.Before == r.Before) == 1 && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CaoTengJianxiongClaimedEvent>().Where(e => e.FrameId == f.Id).All(e => r.Materials.Any(m => m.CardId == e.CardId) && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == e.Sequence && m.Sequence > r.Before && m.Sequence <= r.After && m.CardId == e.CardId && m.From == CardLocation.Processing && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == CaoTengJianxiongClaimReason) == 1);
    }

    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 100)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 90)]
    internal bool IsCaoTengRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => f.CaoTeng is { Stage: CaoTengStage.YongzuChildren, Option: "recover", RecoveryRequested: > 0 } r && ValidCaoTeng(f) && recovery.ParentFrameId == f.Id && recovery.Return == new RecoveryReplacementReturn(PostEventContinuation.Program, f.Id) && recovery.Attempt.SourceSeat == f.OwnerSeat && recovery.Attempt.TargetSeat == CaoTengChooser(f) && recovery.Attempt.Amount == r.RecoveryRequested && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.Program && recovery.Attempt.Completion.InstructionIndex == 1;
    internal bool CaoTengFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.CaoTeng is not { } r || !ValidCaoTeng(f))
            return false;
        if (child is RecoveryReplacementFrame recovery)
            return IsCaoTengRecoveryParent(f, recovery);
        if (child is CardsMovedTriggerWindowFrame moved)
        {
            if (!CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) || moved.ResumeProgramFrameId != f.Id || moved.Batch.Movements.Any(m => !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) || m.Sequence <= r.Before || m.Sequence > r.After))
                return false;
            var reason = r.Stage == CaoTengStage.JianxiongDrawChildren ? CaoTengJianxiongDrawReason : r.Stage == CaoTengStage.JianxiongClaimChildren ? CaoTengJianxiongClaimReason : r.Stage == CaoTengStage.YongzuChildren && r.Option == "draw" ? CaoTengYongzuDrawReason : null;
            var actor = r.Stage == CaoTengStage.YongzuChildren ? CaoTengChooser(f) : f.OwnerSeat;
            return reason is not null && moved.Batch.Movements.All(m => m.To == CardLocation.Hand(actor) && m.Reason.Value == reason && m.From == (r.Stage == CaoTengStage.JianxiongClaimChildren ? CardLocation.Processing : CardLocation.DrawPile) || r.Stage != CaoTengStage.JianxiongClaimChildren && m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle);
        }

        if (child is HpChangedTriggerWindowFrame hp)
            return r.Stage == CaoTengStage.YongzuChildren && r.Option == "recover" && hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.Program && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == f.OwnerSeat && hp.Change.TargetSeat == CaoTengChooser(f) && hp.Change.Amount > 0;
        if (child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange)
            return r.Stage == CaoTengStage.YongzuChildren && r.Option == "restore" && state.ResumeProgramFrameId == f.Id && state.CharacterStateContinuation == CharacterStateContinuation.Program && state.OwnerSeat == CaoTengChooser(f) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == state.Window);
        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal ProgramSkillFrame? CaoTengObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => this.CaoTengFirstChild(f, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27300)]
    internal bool AdvanceCaoTengChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && CaoTengObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
