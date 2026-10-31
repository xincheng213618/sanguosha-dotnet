#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterTaoQianOlRuntime
{
    internal bool ValidZhaohuoStarted(ProgramSkillFrame f)
    {
        if (f.Zhaohuo is not { } r || f.TriggerId is null || r.Source != ZhaohuoSource(f) || f.GameplayHash != r.GameplayHash || f.InstructionIndex != 1 || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != SkillProgramEffectOp.ResolveZhaohuoFirstVictimGift || f.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied } c || c.OwnerSeat != f.OwnerSeat || c.ParentFrameId != r.DamageWindowId || c.DamageFrameId != r.DamageFrameId || c.TargetSeat != r.VictimSeat || ZhaohuoFirst(r.DamageFrameId)is not { } first || first.ActualTurn != r.ActualTurn || first.TurnOwner != r.TurnOwner || first.AttackId != r.AttackId || first.VictimSeat != r.VictimSeat || c.SourceSeat != first.DamageSourceSeat || c.Amount != first.Amount || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == r.DamageWindowId)is not DamageTriggerWindowFrame w || w.TriggerWindow != c.Window || w.ParentFrameId != r.DamageFrameId || w.TargetSeat != r.VictimSeat || w.CandidateIndex < 0 || w.CandidateIndex >= w.Candidates.Count || w.Candidates[w.CandidateIndex].ToProgramCandidate()is not { } candidate || candidate.OwnerSeat != f.OwnerSeat || candidate.SkillId != f.SkillId || candidate.BindingId != f.TriggerId || candidate.SkillInstanceId != f.SkillInstanceId || candidate.GameplayHash != f.GameplayHash || candidate.OccurrenceIndex != c.OccurrenceIndex || !ZhaohuoQualified(candidate, first))
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<ZhaohuoStartedEvent>().Count(e => e.FrameId == f.Id && e.DamageFrameId == r.DamageFrameId && e.DamageWindowId == r.DamageWindowId && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.VictimSeat == r.VictimSeat && e.ActualTurn == r.ActualTurn && e.TurnOwner == r.TurnOwner) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<ZhaohuoStartedEvent>().Count(e => e.DamageFrameId == r.DamageFrameId && e.Source.OwnerSeat == f.OwnerSeat) == 1;
    }

    internal bool ValidZhaohuoPayment(long frameId, long damageId, CardConversionSource source, string hash, int donor, int cardId, CardKind kind, int turn, int turnOwner)
    {
        if (ZhaohuoFirst(damageId)is not { } first || first.VictimSeat != donor || first.ActualTurn != turn || first.TurnOwner != turnOwner || source.OwnerSeat != turnOwner || !ZhaohuoQualified(new(source.OwnerSeat, source.SkillId, source.BindingId, source.SkillInstanceId, hash, 0), first))
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<ZhaohuoStartedEvent>().Count(e => e.FrameId == frameId && e.DamageFrameId == damageId && e.Source == source && e.GameplayHash == hash && e.VictimSeat == donor && e.ActualTurn == turn && e.TurnOwner == turnOwner) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<ZhaohuoGiftCommittedEvent>().Count(e => e.FrameId == frameId && e.DamageFrameId == damageId && e.Source == source && e.GameplayHash == hash && e.DonorSeat == donor && e.CardId == cardId && e.PrintedKind == kind && e.ActualTurn == turn && e.TurnOwner == turnOwner) == 1;
    }

    internal bool ValidZhaohuoPhysicalGift(ZhaohuoGiftPaidEvent e) => e.DonorSeat != e.Source.OwnerSeat && e.From == CardLocation.Hand(e.DonorSeat) && e.To == CardLocation.Hand(e.Source.OwnerSeat) && e.MovementSequence > 0 && ValidZhaohuoPayment(e.FrameId, e.DamageFrameId, e.Source, e.GameplayHash, e.DonorSeat, e.CardId, e.PrintedKind, e.ActualTurn, e.TurnOwner) && _coreProgramEventHistoryOperations.ProgramEventHistory<ZhaohuoGiftPaidEvent>().Count(g => g.FrameId == e.FrameId) == 1 && !_coreProgramEventHistoryOperations.ProgramEventHistory<ZhaohuoSelfGiftCommittedEvent>().Any(g => g.FrameId == e.FrameId) && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == e.MovementSequence && m.TurnNumber == e.ActualTurn && m.CardId == e.CardId && m.CardKind == e.PrintedKind && m.From == e.From && m.To == e.To && m.Reason.Value == ZhaohuoGiftReason) == 1;
    internal bool ValidZhaohuoSelfGift(ZhaohuoSelfGiftCommittedEvent e) => e.SequenceBefore >= 0 && e.SequenceBefore == e.SequenceAfter && e.SequenceAfter <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence && ValidZhaohuoPayment(e.FrameId, e.DamageFrameId, e.Source, e.GameplayHash, e.Source.OwnerSeat, e.CardId, e.PrintedKind, e.ActualTurn, e.TurnOwner) && _coreProgramEventHistoryOperations.ProgramEventHistory<ZhaohuoSelfGiftCommittedEvent>().Count(g => g.FrameId == e.FrameId) == 1 && !_coreProgramEventHistoryOperations.ProgramEventHistory<ZhaohuoGiftPaidEvent>().Any(g => g.FrameId == e.FrameId);
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 28121)]
    internal void AssertZhaohuo(ProgramSkillFrame f)
    {
        if (f.Zhaohuo is not { } r)
            return;
        if (!ValidZhaohuoStarted(f) || !Enum.IsDefined(r.Stage) || r.SequenceBefore < 0 || r.SequenceAfter < r.SequenceBefore || r.SequenceAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
            throw new InvalidOperationException("Zhaohuo lost its exact original source, first damage or native window.");
        var committed = _coreProgramEventHistoryOperations.ProgramEventHistory<ZhaohuoGiftCommittedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (!r.Paid)
        {
            if (r.Stage != ZhaohuoStage.Choosing || r.CardId is not null || r.PrintedKind is not null || r.PaymentSequence is not null || r.SelfGift || committed.Length != 0 || f.PendingMovementContinuation is not null)
                throw new InvalidOperationException("Zhaohuo's unpaid donor choice cannot invent a payment.");
            return;
        }

        if (r.Stage != ZhaohuoStage.GiftChildren || r.CardId is not { } id || r.PrintedKind is not { } kind || committed is not [var commit] || !ValidZhaohuoPayment(f.Id, r.DamageFrameId, r.Source, r.GameplayHash, r.VictimSeat, id, kind, r.ActualTurn, r.TurnOwner) || commit.CardId != id || r.SelfGift != (r.VictimSeat == f.OwnerSeat))
            throw new InvalidOperationException("Zhaohuo repeated or replaced its original donor/entity payment.");
        if (r.SelfGift)
        {
            if (r.PaymentSequence is not null || r.SequenceBefore != r.SequenceAfter || f.PendingMovementContinuation is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<ZhaohuoSelfGiftCommittedEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var self] || self.CardId != id || self.PrintedKind != kind || self.SequenceBefore != r.SequenceBefore || !ValidZhaohuoSelfGift(self))
                throw new InvalidOperationException("Zhaohuo self-gift must choose one real Hand entity without inventing zone movement.");
        }
        else if (r.PaymentSequence is not { } sequence || sequence <= r.SequenceBefore || sequence != r.SequenceAfter || _coreProgramEventHistoryOperations.ProgramEventHistory<ZhaohuoGiftPaidEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var gift] || gift.MovementSequence != sequence || gift.CardId != id || gift.PrintedKind != kind || !ValidZhaohuoPhysicalGift(gift))
            throw new InvalidOperationException("Zhaohuo lost its exact once-paid native Hand gift movement.");
        if (f.PendingMovementContinuation is { } pending && !IsZhaohuoMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), pending))
            throw new InvalidOperationException("Zhaohuo lost its exact pending movement return.");
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 28121)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 28121)]
    internal bool ContinueIssuedZhaohuo(ProgramSkillFrame f)
    {
        if (f.Zhaohuo is { } r)
        {
            AssertZhaohuo(f);
            return r.Paid || _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[r.VictimSeat].IsAlive;
        }

        return f.TriggerId is { } id && f.WindowContext is { } c && IsIssuedZhaohuoCandidate(new(f.OwnerSeat, f.SkillId, id, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, c.OccurrenceIndex), c);
    }

    internal bool IsZhaohuoMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => f.Zhaohuo is { Paid: true, SelfGift: false, Stage: ZhaohuoStage.GiftChildren, PaymentSequence: not null } && effect?.Op == SkillProgramEffectOp.ResolveZhaohuoFirstVictimGift && pending == f.PendingMovementContinuation && pending.SubjectSeat == f.OwnerSeat && pending.BeforeCount == 0 && pending.CoverageResultBind is null;
    internal bool ZhaohuoFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.Zhaohuo is not { Paid: true, Stage: ZhaohuoStage.GiftChildren } r)
            return false;
        AssertZhaohuo(f);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state)
            return state.ResumeProgramFrameId == f.Id && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == state.Window);
        return !r.SelfGift && child is CardsMovedTriggerWindowFrame moved && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements is [var material] && material.Sequence == r.PaymentSequence && _coreProgramEventHistoryOperations.ProgramEventHistory<ZhaohuoGiftPaidEvent>().Any(e => e.FrameId == f.Id && e.CardId == material.CardId && e.MovementSequence == material.Sequence && ValidZhaohuoPhysicalGift(e));
    }

    internal bool ZhaohuoLifecycleEdge(int index) => _resolutionStackOperations._resolutionStack[index - 1] is ProgramLifecycleTriggerWindowFrame w && _resolutionStackOperations._resolutionStack[index] is ProgramSkillFrame f && w.CandidateIndex >= 0 && w.CandidateIndex < w.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, w.Candidates[w.CandidateIndex]) && f.WindowContext is { } c && c.ParentFrameId == w.Id && c.Window == w.Window;
    internal bool ZhaohuoObserverSuffix(int index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[n - 1], _resolutionStackOperations2._resolutionStack()[n]) || _characterTaoQianOlOperations.ZhaohuoLifecycleEdge(n) || _resolutionStackOperations2._resolutionStack()[n - 1] is ProgramSkillFrame { Zenrun: not null } z && _characterSunHongOlOperations.ZenrunFirstChild(z, _resolutionStackOperations2._resolutionStack()[n]) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d));
    internal ProgramSkillFrame? ZhaohuoObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterTaoQianOlOperations.ZhaohuoFirstChild(f, child), (i) => _characterTaoQianOlOperations.ZhaohuoObserverSuffix(i));
    internal bool HasZhaohuoDamageObserver(long id) => ZhaohuoObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    internal bool HasZhaohuoBeforeDamageObserver(long id) => ZhaohuoObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    internal bool IsZhaohuoDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && ZhaohuoObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    internal bool AllowsZhaohuoNestedDamage(ProgramSkillFrame f, int target, int amount, ProgramParticipantReference? source, DamageNature? nature, bool sourceLess)
    {
        if (f.AttackAttempt is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != f.Id || ZhaohuoObserverRoot()is not { } root || root.Id == f.Id)
            return false;
        var e = _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f);
        return e is { Op: SkillProgramEffectOp.Damage } && e.Amount == amount && e.ActorReference == source && e.DamageNature == nature && target == (e.TargetReference is { } p ? _coreProgramCardPaymentsOperations.ResolveProgramParticipant(f, p) : global::CardGame.Core.EngineOwners.CoreGameEngineProgramChoicesRuntime.ResolveProgramEffectTarget(f, e.Target));
    }

    internal bool IsZhaohuoRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame child) => ZhaohuoObserverRoot()is { } root && root.Id != f.Id && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(child, f) && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 28121)]
    internal bool AdvanceZhaohuoChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && ZhaohuoObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramCardLegality(ProgramCardLegalityStage.HandPhysicalBan, 20)]
    [ProgramCardLegality(ProgramCardLegalityStage.EntityPhysicalBan, 10)]
    internal bool IsZhaohuoPhysicalRestricted(int actorSeat, int cardId)
    {
        if (!HasZhaohuoCapability || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(actorSeat) || !ZhaohuoActualTurnOpen)
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<ZhaohuoGiftPaidEvent>().Any(e => e.Source.OwnerSeat == actorSeat && e.CardId == cardId && e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && e.TurnOwner == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && ValidZhaohuoPhysicalGift(e)) || _coreProgramEventHistoryOperations.ProgramEventHistory<ZhaohuoSelfGiftCommittedEvent>().Any(e => e.Source.OwnerSeat == actorSeat && e.CardId == cardId && e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && e.TurnOwner == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && ValidZhaohuoSelfGift(e));
    }

    internal bool IsZhaohuoHandLimitExempt(int ownerSeat, int cardId) => IsZhaohuoPhysicalRestricted(ownerSeat, cardId) && _cardZonesOperations._cardZones.Contains(CardLocation.Hand(ownerSeat), cardId);
}
