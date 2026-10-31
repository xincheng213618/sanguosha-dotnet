#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterTaoQianOlRuntime
{

    internal bool ValidZhaohuoPayment(long frameId, long damageId, CardConversionSource source, string hash, int donor, int cardId, CardKind kind, int turn, int turnOwner)
    {
        if (ZhaohuoFirst(damageId)is not { } first || first.VictimSeat != donor || first.ActualTurn != turn || first.TurnOwner != turnOwner || source.OwnerSeat != turnOwner || !ZhaohuoQualified(new(source.OwnerSeat, source.SkillId, source.BindingId, source.SkillInstanceId, hash, 0), first))
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<ZhaohuoStartedEvent>().Count(e => e.FrameId == frameId && e.DamageFrameId == damageId && e.Source == source && e.GameplayHash == hash && e.VictimSeat == donor && e.ActualTurn == turn && e.TurnOwner == turnOwner) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<ZhaohuoGiftCommittedEvent>().Count(e => e.FrameId == frameId && e.DamageFrameId == damageId && e.Source == source && e.GameplayHash == hash && e.DonorSeat == donor && e.CardId == cardId && e.PrintedKind == kind && e.ActualTurn == turn && e.TurnOwner == turnOwner) == 1;
    }

    internal bool ValidZhaohuoPhysicalGift(ZhaohuoGiftPaidEvent e) => e.DonorSeat != e.Source.OwnerSeat && e.From == CardLocation.Hand(e.DonorSeat) && e.To == CardLocation.Hand(e.Source.OwnerSeat) && e.MovementSequence > 0 && ValidZhaohuoPayment(e.FrameId, e.DamageFrameId, e.Source, e.GameplayHash, e.DonorSeat, e.CardId, e.PrintedKind, e.ActualTurn, e.TurnOwner) && _coreProgramEventHistoryOperations.ProgramEventHistory<ZhaohuoGiftPaidEvent>().Count(g => g.FrameId == e.FrameId) == 1 && !_coreProgramEventHistoryOperations.ProgramEventHistory<ZhaohuoSelfGiftCommittedEvent>().Any(g => g.FrameId == e.FrameId) && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == e.MovementSequence && m.TurnNumber == e.ActualTurn && m.CardId == e.CardId && m.CardKind == e.PrintedKind && m.From == e.From && m.To == e.To && m.Reason.Value == ZhaohuoGiftReason) == 1;
    internal bool ValidZhaohuoSelfGift(ZhaohuoSelfGiftCommittedEvent e) => e.SequenceBefore >= 0 && e.SequenceBefore == e.SequenceAfter && e.SequenceAfter <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence && ValidZhaohuoPayment(e.FrameId, e.DamageFrameId, e.Source, e.GameplayHash, e.Source.OwnerSeat, e.CardId, e.PrintedKind, e.ActualTurn, e.TurnOwner) && _coreProgramEventHistoryOperations.ProgramEventHistory<ZhaohuoSelfGiftCommittedEvent>().Count(g => g.FrameId == e.FrameId) == 1 && !_coreProgramEventHistoryOperations.ProgramEventHistory<ZhaohuoGiftPaidEvent>().Any(g => g.FrameId == e.FrameId);

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 28121)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 28121)]
    internal bool ContinueIssuedZhaohuo(ProgramSkillFrame f)
    {
        if (f.Zhaohuo is { } r)
        {
            return r.Paid || _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[r.VictimSeat].IsAlive;
        }

        return f.TriggerId is { } id && f.WindowContext is { } c && IsIssuedZhaohuoCandidate(new(f.OwnerSeat, f.SkillId, id, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, c.OccurrenceIndex), c);
    }
    internal bool ZhaohuoFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.Zhaohuo is not { Paid: true, Stage: ZhaohuoStage.GiftChildren } r)
            return false;
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
