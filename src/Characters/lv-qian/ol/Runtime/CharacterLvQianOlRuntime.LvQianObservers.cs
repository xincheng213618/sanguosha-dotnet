#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLvQianOlRuntime
{
    internal bool HasLvQianCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.RecordWeiluActualDamageSource);

    internal static CardConversionSource LvQianSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_lv_qian_ol_LvQianObservers.LvQianSource(f);
    internal bool LvQianSourceCurrent(CardConversionSource source, string hash) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(source.OwnerSeat) && _coreMatchParticipantsPlayersAccess._players[source.OwnerSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[source.OwnerSeat], source.SkillId, source.SkillInstanceId) && _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program?.GameplayHash == hash;
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 520)]
    internal void ObserveLvQianTurnBoundary(IGameEvent payload)
    {
        if (!HasLvQianCapability || payload is not (TurnStartedEvent or TurnEndedEvent or PlayerDiedEvent))
            return;
        if (payload is TurnStartedEvent started)
        {
            var owner = _coreMatchParticipantsPlayersAccess._players[started.ActorSeat];
            if (owner.WeiluFuture is { } future)
                owner.WeiluFuture = future.Select(g => g.DueOwnTurn is null && g.ArmedTurn < started.TurnNumber ? g with { DueOwnTurn = started.TurnNumber } : g).ToArray();
        }

        if (payload is TurnEndedEvent ended)
        {
            var owner = _coreMatchParticipantsPlayersAccess._players[ended.ActorSeat];
            if (owner.WeiluFuture is { } future)
                owner.WeiluFuture = future.Where(g => g.DueOwnTurn != ended.TurnNumber).ToArray();
            if (owner.WeiluDebts is { } debts)
                foreach (var debt in debts.Where(g => g.Phase.ActualTurn == ended.TurnNumber).ToArray())
                    ConsumeUnreturnedWeiluDebt(owner, debt);
        }

        if (payload is PlayerDiedEvent died)
            foreach (var owner in _coreMatchParticipantsPlayersAccess._players)
            {
                if (owner.WeiluFuture is { } future)
                    owner.WeiluFuture = future.Where(g => owner.Seat != died.VictimSeat && g.DamageSourceSeat != died.VictimSeat).ToArray();
                if (owner.WeiluDebts is { } debts)
                    foreach (var debt in debts.Where(g => g.TargetSeat == died.VictimSeat).ToArray())
                        ConsumeUnreturnedWeiluDebt(owner, debt);
            }
    }

    internal void ObserveLvQianActualPlayEntry(int ownerSeat, int phaseInstance)
    {
        if (!HasLvQianCapability)
            return;
        var owner = _coreMatchParticipantsPlayersAccess._players[ownerSeat];
        if (owner.WeiluFuture is not { } future)
            return;
        var phase = new WeiluPhaseKey(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, ownerSeat, phaseInstance);
        owner.WeiluFuture = future.Select(g => g.DueOwnTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && g.FirstPhase is null ? g with { FirstPhase = phase } : g).ToArray();
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 530)]
    internal bool CanOfferLvQian(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w)
    {
        if (!t.Effects.Any(e => LvQianComposition.Own(e.Op)))
            return true;
        return t.Effects[0].Op switch
        {
            SkillProgramEffectOp.RecordWeiluActualDamageSource => w.SourceSeat is { } source && source != c.OwnerSeat && w.TargetSeat == c.OwnerSeat && w.Amount > 0 && w.DamageFrameId is not null,
            SkillProgramEffectOp.PayWeiluNextActualPlayHpLoss => w.WeiluPhase is { } p && p.OwnerSeat == c.OwnerSeat && (_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].WeiluFuture ?? []).Any(g => g.FirstPhase == p && g.Source.SkillId == c.SkillId && g.Source.SkillInstanceId == c.SkillInstanceId),
            SkillProgramEffectOp.RestoreWeiluExactPaidPlayHp => HasIssuedWeiluDebtCandidate(c, w),
            SkillProgramEffectOp.ConsumeZengdaoForActualSourceDamage => IsIssuedZengdaoCandidate(c, w),
            _ => false
        };
    }

    internal ProgramSkillWindowContext CreateLvQianBeforeDamageContext(BeforeDamageProgramCandidate item, ProgramSkillWindowContext context) => context with
    {
        ZengdaoGrant = MatchingZengdaoGrant(item.Candidate)
    };

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 310)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 310)]
    internal bool CanContinueLvQian(ProgramSkillFrame f)
    {
        if (f.LvQian is not null)
        {
            return true;
        }

        if (f.InstructionIndex != 0 || f.TriggerId is null || f.WindowContext is not { } window)
            return false;
        var candidate = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, window.OccurrenceIndex);
        return HasIssuedWeiluDebtCandidate(candidate, window) || IsIssuedZengdaoCandidate(candidate, window);
    }

    [ProgramMovementProof(ProgramMovementProofPhase.NativeCost, 310)]
    internal bool IsLvQianMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => f.LvQian is not null && e is not null && LvQianComposition.Own(e.Op) && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null && CanContinueLvQian(f);
    internal bool LvQianFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (root.LvQian is not { } r || !CanContinueLvQian(root))
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return moved.Batch.ParentFrameId == root.Id && moved.Batch.AwaitingProgramFrameId == root.Id && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == root.Id && hp.ResumeFrameId == root.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && r.HpProof is { } proof && hp.Change.TargetSeat == proof.TargetSeat && hp.Change.Kind == proof.Kind && hp.Change.HpBefore == proof.HpBefore && hp.Change.Amount <= proof.Amount;
        return child is RecoveryReplacementFrame recovery && recovery.ParentFrameId == root.Id && recovery.Return.ResumeFrameId == root.Id && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && IsLvQianRecoveryCompletion(root, recovery.Attempt);
    }

    internal ProgramSkillFrame? LvQianObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((r, c) => _characterLvQianOlOperations.LvQianFirstChild(r, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d) || _coreTieredRoundZeroReturnsOperations.TieredRoundZeroDyingRescueRide(n, d) || (_coreDrawFundedDistinctBasicReturnsOperations.DrawFundedDistinctBasicDyingRescueRide(n, d) || _characterHuanShuJunOlOperations.HuanpeiDyingRescueRide(n, d))));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 270)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 380)]
    internal bool HasLvQianDamageObserver(long id) => LvQianObserverRoot()is { } r && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == r.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 470)]
    internal bool HasLvQianBeforeDamageObserver(long id) => LvQianObserverRoot()is { } r && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is BeforeDamageProgramWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == r.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 380)]
    internal bool TryAdvanceLvQianSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && LvQianObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
    internal bool IsLvQianRecoveryCompletion(ProgramSkillFrame f, RecoveryAttempt attempt) => f.LvQian is { Stage: LvQianStage.RecoveryChildren, HpProof: { Kind: HpChangeKind.Recovery } proof, Phase: { } phase } r && phase == f.WindowContext?.WeiluPhase && attempt.SourceSeat == f.OwnerSeat && attempt.TargetSeat == proof.TargetSeat && attempt.Amount == proof.Amount && attempt.HpBefore == proof.HpBefore && attempt.Completion.Producer == RecoveryAttemptProducer.Program && attempt.Completion.InstructionIndex == f.InstructionIndex && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<WeiluDebtConsumedEvent>().Count(e => e.FrameId == f.Id && e.Phase == phase && e.TargetSeat == proof.TargetSeat && e.TargetAlive && e.Amount >= proof.Amount) == 1;
}
