#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSimaHuiOlRuntime
{
    internal SkillProgramEffect? SimaHuiPausedEffect(ProgramSkillFrame f) => ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).GetPausedInstruction(f.InstructionIndex).Effect;
    internal bool ExactSimaHuiParent(ProgramSkillFrame f)
    {
        if (f.TriggerId is null)
            return f.WindowContext is null && f.ParentFrameId is null;
        if (f.WindowContext is not { } c)
            return false;
        if (_resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(w => w.Id == c.ParentFrameId)is { } l)
            return l.CandidateIndex >= 0 && l.CandidateIndex < l.Candidates.Count && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, l.Candidates[l.CandidateIndex]) && l.Window == c.Window && c.BeforeTurnPreludeProof == l.BeforeTurnPreludeProof && c.BeforeTurnPreludeProof is { } proof && _characterBeiMiHuOlOperations.ValidBeiMiHuBeforeTurnPrelude(proof);
        if (_resolutionStackOperations._resolutionStack.OfType<ProgramKillTriggerWindowFrame>().SingleOrDefault(w => w.Id == c.ParentFrameId)is { } k)
            return k.CandidateIndex >= 0 && k.CandidateIndex < k.Candidates.Count && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, k.Candidates[k.CandidateIndex]) && k.VictimSeat == c.TargetSeat;
        if (_resolutionStackOperations._resolutionStack.OfType<DamageTriggerWindowFrame>().SingleOrDefault(w => w.Id == c.ParentFrameId)is { } d)
            return d.CandidateIndex >= 0 && d.CandidateIndex < d.Candidates.Count && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, d.Candidates[d.CandidateIndex].ToProgramCandidate()) && d.ParentFrameId == c.DamageFrameId;
        return false;
    }

    internal bool ValidSimaHui(ProgramSkillFrame f)
    {
        if (f.SimaHui is not { } r || r.Source != SimaHuiSource(f) || r.GameplayHash != f.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != r.GameplayHash || r.ParentFrameId != (f.WindowContext?.ParentFrameId ?? 0) || r.ActualTurn != (f.WindowContext?.BeforeTurnPreludeProof?.UpcomingTurnNumber ?? _coreActualTurnProgressTurnNumberAccess._turnNumber) || !ExactSimaHuiParent(f))
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        if (r.Stage == "yeyan-paid")
            return r.InstructionIndex == 1 && f.InstructionIndex >= 1 && r.OriginalCards.Count is 0 or 4 && r.OriginalCards.SequenceEqual(f.SelectedCardIds) && history.OfType<SimaHuiYeyanSealsPaidEvent>().Where(e => e.FrameId == f.Id).ToArray()is [var paid] && paid.Source == r.Source && paid.GameplayHash == r.GameplayHash && paid.ActualTurn == r.ActualTurn && paid.GrantSourceIds.Count >= 2 && paid.GrantSourceIds.Distinct().Count() == paid.GrantSourceIds.Count && paid.MaterialCardIds.SequenceEqual(r.OriginalCards);
        if (f.InstructionIndex != r.InstructionIndex || f.InstructionIndex != 1 || r.Cursor < 0 || r.Cursor > r.OriginalCards.Count || r.MovementBefore > r.MovementAfter)
            return false;
        if (r.Stage is "initial-dragon" or "initial-phoenix" or "transfer-kind" or "transfer-target" or "seal-children")
            return history.OfType<SimaHuiChoiceStartedEvent>().Where(e => e.FrameId == f.Id).ToArray()is [var choice] && choice.Source == r.Source && choice.GameplayHash == r.GameplayHash && choice.ParentFrameId == r.ParentFrameId && choice.ActualTurn == r.ActualTurn && r.Candidates.Distinct().Count() == r.Candidates.Count && r.Candidates.All(_coreTableSeatQueriesOperations.IsValidPlayerSeat) && (r.Stage != "seal-children" || history.OfType<SimaHuiSealMovedEvent>().Any(e => e.FrameId == f.Id && e.Holder == r.ChosenHolder && e.ActualTurn == r.ActualTurn));
        if (r.ChainOrigin is not { } origin || history.OfType<SimaHuiTopViewedEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var viewed] || viewed.Source != r.Source || viewed.GameplayHash != r.GameplayHash || viewed.Origin != origin || !viewed.CardIds.SequenceEqual(r.OriginalCards) || r.OriginalCards.Count > origin.LivingChainedCount + 1 || r.OriginalCards.Distinct().Count() != r.OriginalCards.Count || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > viewed.Before && m.Sequence <= viewed.After && m.From == CardLocation.DrawPile && m.To == CardLocation.Processing && m.Reason.Value == SimaHuiViewReason).Select(m => m.CardId).SequenceEqual(r.OriginalCards))
            return false;
        if (r.Stage == "gift-children")
            return r.Cursor < r.OriginalCards.Count && r.ChosenHolder is { } recipient && history.OfType<SimaHuiCardDistributedEvent>().Where(e => e.FrameId == f.Id && e.Cursor == r.Cursor).ToArray()is [var gift] && gift.Source == r.Source && gift.GameplayHash == r.GameplayHash && gift.CardId == r.OriginalCards[r.Cursor] && gift.Recipient == recipient && gift.Before == r.MovementBefore && gift.After == r.MovementAfter && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > gift.Before && m.Sequence <= gift.After && m.CardId == gift.CardId && m.From == CardLocation.Processing && m.To == CardLocation.Hand(recipient) && m.Reason.Value == SimaHuiGiftReason) == 1;
        if (r.Stage == "return-children")
            return history.OfType<SimaHuiUnassignedReturnedEvent>().Where(e => e.FrameId == f.Id).ToArray()is [var returned] && returned.Source == r.Source && returned.GameplayHash == r.GameplayHash && returned.Before == r.MovementBefore && returned.After == r.MovementAfter && returned.CardIds.All(r.OriginalCards.Contains) && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > returned.Before && m.Sequence <= returned.After && m.From == CardLocation.Processing && m.To == CardLocation.DrawPile && m.Reason.Value == "program.sima-hui.chenghao.return").Select(m => m.CardId).SequenceEqual(returned.CardIds);
        return r.Stage is "view-children" or "distribute";
    }

    internal void AssertSimaHui(ProgramSkillFrame f)
    {
        if (f.SimaHui is not null && !ValidSimaHui(f))
            throw new InvalidOperationException("Sima Hui lost its exact native source, frozen cards or completed payment invoice.");
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 180)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 180)]
    internal bool CanContinueSimaHui(ProgramSkillFrame f) => f.SimaHui is { } r && ValidSimaHui(f) && (r.Stage == "yeyan-paid" || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<SimaHuiSealMovedEvent>().Any(e => e.FrameId == f.Id) || r.ChainOrigin is not null);
    internal bool IsSimaHuiMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => f.SimaHui is { Stage: "seal-children" or "view-children" or "gift-children" or "return-children" } && effect is not null && IsSimaHuiOp(effect.Op) && pending.SubjectSeat == f.OwnerSeat && pending.CoverageResultBind is null && ValidSimaHui(f);
    [ProgramContinuation(ProgramContinuationKind.Movement, 23500)]
    internal bool ReturnSimaHuiMovement(ProgramSkillFrame f)
    {
        if (f.SimaHui is null)
            return false;
        if (f.SimaHui.Stage == "yeyan-paid")
        {
            if (f.SelectedCardPayment is null || f.SelectedCardPaymentResult is not null)
                return false;
            if (!IsSimaHuiYeyanSelectedPayment(f) || f.SelectedCardPayment.ActiveChildFrameId is not null || f.PendingMovementContinuation is not null)
                throw new InvalidOperationException("Yeyan cannot return a foreign or incomplete native four-card payment.");
            var payment = f.SelectedCardPayment;
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { SelectedCardPaymentResult = new(payment.InstructionIndex, payment.Operation, payment.LastCompletedChildFrameId, true) });
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return true;
        }

        var pending = f.PendingMovementContinuation ?? throw new InvalidOperationException("Missing Sima Hui movement invoice.");
        if (!IsSimaHuiMovement(f, SimaHuiPausedEffect(f), pending))
            throw new InvalidOperationException("Sima Hui received a foreign movement return.");
        return ResumeSimaHui(f.Id);
    }

    internal bool SimaHuiFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.SimaHui is not { } r || !ValidSimaHui(f))
            return false;
        if (r.Stage == "yeyan-paid")
            return SimaHuiYeyanFirstChild(f, child);
        if (child is CardsMovedTriggerWindowFrame moved)
            return moved.Batch.ParentFrameId == f.Id && moved.Batch.AwaitingProgramFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.MovementBefore && m.Sequence <= r.MovementAfter);
        if (child is ProgramLifecycleTriggerWindowFrame skills)
            return r.Stage == "seal-children" && skills.Window == SkillProgramTriggerWindow.SkillsChanged && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        return false;
    }

    internal ProgramSkillFrame? SimaHuiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => this.SimaHuiFirstChild(f, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 250)]
    internal bool TryAdvanceSimaHuiSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && SimaHuiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    internal IEnumerable<Card> SimaHuiPrivatelyViewedCards(int viewer) => _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Where(f => f.OwnerSeat == viewer && f.SimaHui is { OriginalCards.Count: > 0 }).SelectMany(f => f.SimaHui!.OriginalCards).Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Processing).Select(id => _cardZonesOperations._cardZones.CardsAt(CardLocation.Processing).Single(c => c.Id == id));
}
