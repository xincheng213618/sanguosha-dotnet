#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDamageJudgmentSuitPaymentObserversRuntime
{
    internal bool IsValidDamageJudgmentSuitPayment(ProgramSkillFrame frame)
    {
        if (frame.DamageJudgmentSuitPayment is not { } draft || frame.TriggerId is null || draft.InstructionIndex != frame.InstructionIndex || draft.InstructionIndex != 1 || draft.OwnerSeat != frame.OwnerSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(draft.SourceSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(draft.TargetSeat) || frame.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 0 } context || context.ParentFrameId != draft.DamageWindowId || context.DamageFrameId != draft.DamageFrameId || context.SourceSeat != draft.SourceSeat || context.TargetSeat != draft.TargetSeat || context.OccurrenceIndex != draft.OccurrenceIndex || _resolutionStackOperations._resolutionStack.OfType<DamageTriggerWindowFrame>().SingleOrDefault(w => w.Id == draft.DamageWindowId)is not { } window || window.ParentFrameId != draft.DamageFrameId || window.SourceSeat != draft.SourceSeat || window.TargetSeat != draft.TargetSeat || window.TriggerWindow != context.Window || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(frame, window.Candidates[window.CandidateIndex].ToProgramCandidate()) || _resolutionStackOperations._resolutionStack.OfType<DamageFrame>().SingleOrDefault(d => d.Id == draft.DamageFrameId)is not { Amount: > 0 } damage || damage.ParentFrameId != draft.CardUseFrameId || damage.SourceSeat != draft.SourceSeat || damage.TargetSeat != draft.TargetSeat || !_coreDamageJudgmentSuitPaymentsOperations.DamageJudgmentUseMatches(draft, window.SourceCard) || ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).Instructions is not [{ Op: SkillProgramEffectOp.JudgeDamageTargetThenOfferSuitDiscard } instruction] || instruction.JudgmentReason != draft.Reason || instruction.ResultBind != draft.ResultBind || !Enum.IsDefined(draft.Stage) || draft.SourceDiscardsRemaining is < 0 or > 2)
            return false;
        if (draft.JudgmentCardId is { } judged)
        {
            if (draft.JudgmentCardKind is null || draft.JudgmentSuit is not { } suit || !Enum.IsDefined(suit) || draft.JudgmentRank is null or < 1 or > 13 || !_cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(judged)).Any(c => c.Id == judged && c.Kind == draft.JudgmentCardKind && c.Rank == draft.JudgmentRank) || !_coreCommandJournalStorageEventsPendingEventsAccess._events.Select(e => e.Payload).Concat(_coreCommandJournalStorageEventsPendingEventsAccess._pendingEvents).OfType<JudgmentResolvedEvent>().Any(e => e.ResolutionId == draft.JudgmentFrameId && e.ParentResolutionId == frame.Id && e.TargetSeat == draft.TargetSeat && e.Reason == draft.Reason && e.CardId == judged && e.CardKind == draft.JudgmentCardKind && e.Suit == draft.JudgmentSuit && e.Rank == draft.JudgmentRank))
                return false;
        }
        else if (draft.Stage is not (ProgramDamageJudgmentPaymentStage.Judging or ProgramDamageJudgmentPaymentStage.CleanupChildren or ProgramDamageJudgmentPaymentStage.Complete) || draft.PaidCardId is not null)
            return false;
        if (draft.PaidCardId is not { } paid)
            return draft.PaidMovementSequence is null && !draft.JudgmentClaimIssued && !draft.PaymentClaimIssued && draft.JudgmentClaimMovementSequence is null && draft.PaymentClaimMovementSequence is null && draft.SourceDiscardMovementSequence1 is null && draft.SourceDiscardMovementSequence2 is null && draft.Stage is ProgramDamageJudgmentPaymentStage.Judging or ProgramDamageJudgmentPaymentStage.ChoosingPayment or ProgramDamageJudgmentPaymentStage.CleanupChildren or ProgramDamageJudgmentPaymentStage.Complete;
        if (draft.PaidMovementSequence is not { } sequence || draft.PaidCardKind is null || draft.PaidFrom is not { } from || draft.PaidTo is not { } to || from.OwnerSeat != frame.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageJudgmentSuitPaymentsDefinitions.DamageJudgmentPaymentZones.Contains(from.Zone) || to != CardLocation.DiscardPile && to != CardLocation.OutsideGame || draft.PaidSuit is not { } paidSuit || !Enum.IsDefined(paidSuit) || draft.PaidRank is null or < 1 or > 13 || !_cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(paid)).Any(c => c.Id == paid && c.Kind == draft.PaidCardKind && c.Rank == draft.PaidRank) || !_coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.Sequence == sequence && m.CardId == paid && m.CardKind == draft.PaidCardKind && m.From == from && m.To == to && m.Reason.Value == "skill-program.damage-judgment-suit.payment"))
            return false;
        if (!HasDamageJudgmentMatchingClaim(frame, draft, true) || !HasDamageJudgmentMatchingClaim(frame, draft, false))
            return false;
        var sourceSequences = new[]
        {
            draft.SourceDiscardMovementSequence1,
            draft.SourceDiscardMovementSequence2
        }.OfType<long>().ToArray();
        if (sourceSequences.Distinct().Count() != sourceSequences.Length || draft.SourceDiscardMovementSequence2 is not null && draft.SourceDiscardMovementSequence1 is null || sourceSequences.Any(seq => !_coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.Sequence == seq && m.From.OwnerSeat == draft.SourceSeat && global::CardGame.Core.EngineOwners.CoreGameEngineDamageJudgmentSuitPaymentsDefinitions.DamageJudgmentPaymentZones.Contains(m.From.Zone) && (m.To == CardLocation.DiscardPile || m.To == CardLocation.OutsideGame) && m.Reason.Value == "skill-program.damage-judgment-suit.source-discard")) || draft.JudgmentSuit != Suit.Club && (sourceSequences.Length != 0 || draft.SourceDiscardsRemaining != 0) || draft.JudgmentSuit == Suit.Club && draft.Stage != ProgramDamageJudgmentPaymentStage.PaymentChildren && sourceSequences.Length != 2 - draft.SourceDiscardsRemaining)
            return false;
        return _coreCommandJournalStorageEventsPendingEventsAccess._events.Select(e => e.Payload).Concat(_coreCommandJournalStorageEventsPendingEventsAccess._pendingEvents).OfType<ProgramJudgmentSuitPaymentCommittedEvent>().Any(e => e.FrameId == frame.Id && e.SkillId == frame.SkillId && e.BindingId == global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame) && e.SkillInstanceId == frame.SkillInstanceId && e.OwnerSeat == frame.OwnerSeat && e.JudgmentFrameId == draft.JudgmentFrameId && e.JudgmentCardId == draft.JudgmentCardId && e.PaidCardId == paid && e.PaidMovementSequence == sequence && e.SuitMatched == (draft.PaidSuit == draft.JudgmentSuit) && e.RankMatched == (draft.PaidRank == draft.JudgmentRank) && e.JudgmentSuit == draft.JudgmentSuit && e.JudgmentRank == draft.JudgmentRank && e.PaidSuit == draft.PaidSuit && e.PaidRank == draft.PaidRank);
    }

    internal bool HasDamageJudgmentMatchingClaim(ProgramSkillFrame frame, ProgramDamageJudgmentSuitPayment draft, bool judged)
    {
        var issued = judged ? draft.JudgmentClaimIssued : draft.PaymentClaimIssued;
        var sequence = judged ? draft.JudgmentClaimMovementSequence : draft.PaymentClaimMovementSequence;
        if (!issued)
            return sequence is null;
        var id = judged ? draft.JudgmentCardId : draft.PaidCardId;
        return sequence is { } seq && id is { } card && (judged ? draft.JudgmentSuit == draft.PaidSuit : draft.JudgmentRank == draft.PaidRank) && _coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.Sequence == seq && m.CardId == card && m.To == CardLocation.Hand(frame.OwnerSeat) && m.From == (judged ? CardLocation.Processing : CardLocation.DiscardPile) && m.Reason.Value == (judged ? "skill-program.damage-judgment-suit.claim-judgment" : "skill-program.damage-judgment-suit.reclaim-payment")) && _coreCommandJournalStorageEventsPendingEventsAccess._events.Select(e => e.Payload).Concat(_coreCommandJournalStorageEventsPendingEventsAccess._pendingEvents).OfType<ProgramJudgmentSuitPaymentCardClaimedEvent>().Any(e => e.FrameId == frame.Id && e.SkillId == frame.SkillId && e.SkillInstanceId == frame.SkillInstanceId && e.OwnerSeat == frame.OwnerSeat && e.JudgmentFrameId == draft.JudgmentFrameId && e.CardId == card && e.MovementSequence == seq && e.IsJudgmentCard == judged);
    }

    internal ProgramSkillFrame? DamageJudgmentSuitPaymentRoot(long damageWindow)
    {
        var parentIndex = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == damageWindow);
        if (parentIndex < 0 || parentIndex + 1 >= _resolutionStackOperations._resolutionStack.Count || _resolutionStackOperations._resolutionStack[parentIndex + 1] is not { } owned || owned is not ProgramSkillFrame frame || !IsValidDamageJudgmentSuitPayment(frame))
            return null;
        var draft = frame.DamageJudgmentSuitPayment!;
        for (var index = parentIndex + 2; index < _resolutionStackOperations._resolutionStack.Count; index++)
        {
            if (!DamageJudgmentSuitPaymentEdge(index, frame, draft))
                return null;
            if (_resolutionStackOperations._resolutionStack[index] is DyingFrame dying && _coreParticipantHandProgramRescueRidesOperations.IsPaidHandRepaymentProgramAlcoholRide(index, dying))
                return frame;
        }

        return frame;
    }

    internal bool DamageJudgmentSuitPaymentEdge(int index, ProgramSkillFrame root, ProgramDamageJudgmentSuitPayment draft)
    {
        var child = _resolutionStackOperations._resolutionStack[index];
        var parent = _resolutionStackOperations._resolutionStack[index - 1];
        if (child is JudgmentFrame judgment && parent.Id == root.Id)
            return draft.Stage == ProgramDamageJudgmentPaymentStage.Judging && judgment.Id == draft.JudgmentFrameId && judgment.ParentFrameId == root.Id && judgment.TargetSeat == draft.TargetSeat && judgment.SourceSeat == draft.SourceSeat && judgment.Reason == draft.Reason && judgment.ProgramResultBind == draft.ResultBind && _coreProgramLifecycleOperations.IsValidProgramJudgmentContinuation(judgment, root);
        if (child is ProgramJudgmentTriggerWindowFrame window && parent is JudgmentFrame judged)
            return judged.Id == draft.JudgmentFrameId && window.ParentFrameId == judged.Id && window.Judgment.JudgmentFrameId == judged.Id && window.Judgment.SubjectSeat == draft.TargetSeat && window.Judgment.SourceSeat == draft.SourceSeat && window.Judgment.Reason == draft.Reason;
        if (child is ProgramSkillFrame final && parent is ProgramJudgmentTriggerWindowFrame finalized)
        {
            if (finalized.ZhenyiBeforeResult == true)
                return finalized.Judgment.JudgmentFrameId == draft.JudgmentFrameId && _coreZhenyiJudgmentWindowsOperations.IsExactZhenyiBeforeResultProgramChild(finalized, final);
            if (finalized.Judgment.JudgmentFrameId != draft.JudgmentFrameId || finalized.CandidateIndex < 0 || finalized.CandidateIndex >= finalized.Candidates.Count || final.WindowContext is not { Window: SkillProgramTriggerWindow.JudgmentFinalized } context || context.ParentFrameId != finalized.Id || context.Judgment != finalized.Judgment)
                return false;
            var candidate = finalized.Candidates[finalized.CandidateIndex];
            return candidate.OwnerSeat == final.OwnerSeat && candidate.SkillId == final.SkillId && candidate.TriggerId == final.TriggerId && candidate.SkillInstanceId == final.SkillInstanceId && candidate.GameplayHash == final.GameplayHash;
        }

        if (child is ProgramSkillFrame replacementProgram && parent is JudgmentFrame replace)
            return replace.Id == draft.JudgmentFrameId && replacementProgram.WindowContext is { Window: SkillProgramTriggerWindow.JudgmentReplacing, JudgmentReplacement: { } replacement } c && c.ParentFrameId == replace.Id && replacement.JudgmentFrameId == replace.Id && _coreNativeJudgmentOperations.CurrentJudgmentCandidate(replace)is { } candidate && candidate.OwnerSeat == replacementProgram.OwnerSeat && candidate.ProgramId == replacementProgram.SkillId && candidate.ProgramTriggerId == replacementProgram.TriggerId && candidate.SkillInstanceId == replacementProgram.SkillInstanceId && candidate.GameplayHash == replacementProgram.GameplayHash;
        if (child is DyingFrame hpLossDying && parent is ProgramSkillFrame hpLossProgram)
            return IsDamageJudgmentHpLossDyingRide(hpLossDying, hpLossProgram);
        if (child is ProgramSkillFrame entryProgram && parent is ProgramLifecycleTriggerWindowFrame { Continuation: ProgramLifecycleContinuation.ResumeDyingEntry } entry && _coreDyingCursorRuntimeOperations.ActiveDying is { ResumesProgramSkill: true } entered && _resolutionStackOperations._resolutionStack.OfType<DyingFrame>().SingleOrDefault(f => f.Id == entered.Id)is { } entryDying)
            return global::CardGame.Core.EngineOwners.CoreParticipantHandDamageRidesRuntime.ParticipantHandDyingEntryRide(entryProgram, entry, entryDying) && entry.ResumeDyingFrameId == entered.Id && entryDying.ParentFrameId == entered.ParentFrameId && entryDying.VictimSeat == entered.VictimSeat && entry.OwnerSeat == entered.VictimSeat && entry.CandidateIndex >= 0 && entry.CandidateIndex < entry.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(entryProgram, entry.Candidates[entry.CandidateIndex]) && entryProgram.WindowContext is { Window: SkillProgramTriggerWindow.DyingEntering } entryContext && entryContext.ParentFrameId == entry.Id && entryContext.OwnerSeat == entryProgram.OwnerSeat && entryContext.TargetSeat == entered.VictimSeat && entryContext.SourceSeat == entered.KillerSeat;
        if (child is ProgramLifecycleTriggerWindowFrame stateWindow && parent is ProgramSkillFrame stateParent)
            return stateWindow.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && stateWindow.ResumeProgramFrameId == stateParent.Id && stateWindow.CharacterStateContinuation == CharacterStateContinuation.Program && stateWindow.Window is SkillProgramTriggerWindow.CharacterTurnedOver or SkillProgramTriggerWindow.CharacterTurnedFaceUp or SkillProgramTriggerWindow.CharacterEnteredChain && _coreCommandJournalStorageEventsPendingEventsAccess._events.Select(e => e.Payload).Concat(_coreCommandJournalStorageEventsPendingEventsAccess._pendingEvents).OfType<CharacterStateChangedEvent>().Any(e => e.Change.Id == stateWindow.Id && e.Change.ParentFrameId == stateParent.Id && e.Change.TargetSeat == stateWindow.OwnerSeat && e.Change.Window == stateWindow.Window);
        if (child is ProgramSkillFrame stateProgram && parent is ProgramLifecycleTriggerWindowFrame state)
            return state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && state.CandidateIndex >= 0 && state.CandidateIndex < state.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(stateProgram, state.Candidates[state.CandidateIndex]) && stateProgram.WindowContext?.ParentFrameId == state.Id && stateProgram.WindowContext.Window == state.Window;
        // This helper proves only generic typed descendants. Our root above
        // independently locks the 4200 actual Slash/damage/judgment/payment.
        return _coreProgramChildResolutionOperations.ProgramChildResolutionEdge(index);
    }

    internal bool IsDamageJudgmentHpLossDyingRide(DyingFrame dying, ProgramSkillFrame program) => dying.ResumesProgramSkill && dying.ParentFrameId == program.Id && dying.KillerSeat is null && _coreDyingCursorRuntimeOperations.ActiveDying is { ResumesProgramSkill: true } active && active.Id == dying.Id && active.ParentFrameId == program.Id && active.VictimSeat == dying.VictimSeat && _coreCommandJournalStorageEventsPendingEventsAccess._events.Select(e => e.Payload).Concat(_coreCommandJournalStorageEventsPendingEventsAccess._pendingEvents).OfType<ProgramSkillHpLostEvent>().LastOrDefault(e => e.FrameId == program.Id && e.SkillId == program.SkillId && e.TargetSeat == dying.VictimSeat)is { Amount: > 0, RemainingHp: <= 0 } && _coreCommandJournalStorageEventsPendingEventsAccess._events.Select(e => e.Payload).Concat(_coreCommandJournalStorageEventsPendingEventsAccess._pendingEvents).OfType<PlayerDyingEvent>().Any(e => e.ResolutionId == dying.Id && e.VictimSeat == dying.VictimSeat && e.KillerSeat is null);
    internal bool IsDamageJudgmentSuitPaymentMovement(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation movement) => effect?.Op == SkillProgramEffectOp.JudgeDamageTargetThenOfferSuitDiscard && movement.SubjectSeat == frame.OwnerSeat && movement.BeforeCount == 0 && movement.CoverageResultBind is null && frame.DamageJudgmentSuitPayment is { Stage: ProgramDamageJudgmentPaymentStage.PaymentChildren or ProgramDamageJudgmentPaymentStage.SourceDiscardChildren or ProgramDamageJudgmentPaymentStage.ClaimChildren or ProgramDamageJudgmentPaymentStage.CleanupChildren } && IsValidDamageJudgmentSuitPayment(frame);
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 410)]
    internal bool IsDamageJudgmentSuitPaymentDying() => _coreDamageCursorRuntimeOperations.ActiveDamageTrigger is { } damage && _coreDyingCursorRuntimeOperations.ActiveDying is { ResumesProgramSkill: true } && DamageJudgmentSuitPaymentRoot(damage.Id)is not null;

    [ProgramAiChoice(SkillProgramEffectOp.JudgeDamageTargetThenOfferSuitDiscard)]
    internal PromptChoice SelectAiDamageJudgmentSuitPayment(PendingDecision decision)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("AI damage judgment owner missing.");
        var draft = frame.DamageJudgmentSuitPayment!;
        if (draft.Stage == ProgramDamageJudgmentPaymentStage.ChoosingSourceDiscard)
            return decision.Choices.OrderBy(c => c.Id.Value, StringComparer.Ordinal).First();
        var view = _corePlayerViewProjectionOperations.CreateSnapshot(frame.OwnerSeat);
        var benefit = draft.JudgmentSuit switch
        {
            Suit.Heart => _coreAiBrainsAiBrainsAccess._aiBrains[frame.OwnerSeat].ScoreProgramTarget(view, draft.TargetSeat, new(0, 1, 0, 0, 0, 0, false, false)),
            Suit.Diamond => _coreAiBrainsAiBrainsAccess._aiBrains[frame.OwnerSeat].ScoreProgramTarget(view, draft.TargetSeat, new(2, 0, 0, 0, 0, 0, false, false)),
            _ => _coreAiBrainsAiBrainsAccess._aiBrains[frame.OwnerSeat].ScoreProgramTarget(view, draft.SourceSeat, new(0, 0, 0, 0, 0, 1, false, false))};
        return decision.Choices.Select(c => (Choice: c, Value: c.Parameters["payment-kind"] == "decline" ? 0d : benefit - 8d + (_cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(c.Cards.Single())).Single(candidateCard => candidateCard.Id == c.Cards.Single())is { } card ? (_coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], card) == draft.JudgmentSuit ? 10d : 0d) + (card.Rank == draft.JudgmentRank ? 10d : 0d) : 0d))).OrderByDescending(x => x.Value).ThenBy(x => x.Choice.Id.Value, StringComparer.Ordinal).First().Choice;
    }
}
