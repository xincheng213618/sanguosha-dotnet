#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineSourceBeigongChildrenRuntime
{
    internal bool SourceBeigongFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!CoreGameEngineSourceBeigongRuntime.HasSourceBeigongState(f))
            return false;
        if (child is CardUseFrame use)
            return _coreSourceDingxiNativeOperations.SourceDingxiUseRidesOn(f, use);
        if (child is JudgmentFrame judge)
            return _coreSourceHuojieOperations.Value.IsSourceHuojieJudgment(judge, f);
        if (child is CardsMovedTriggerWindowFrame moved && f.PendingMovementContinuation is { } pending && _coreSourceBeigongOperations.SourceBeigongMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), pending))
        {
            var before = f.SourceDingxi?.Before ?? f.SourceHuojie?.Before ?? f.SourceXingxia?.Before ?? -1;
            var after = f.SourceDingxi?.After ?? f.SourceHuojie?.After ?? f.SourceXingxia?.After ?? -1;
            return CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) && (moved.Batch.ParentFrameId == f.Id && m.Sequence > before && m.Sequence <= after || f.SourceHuojie is { Stage: SourceHuojieStage.JudgmentChildren } h && moved.Batch.ParentFrameId == h.JudgmentId && m.Sequence == h.JudgmentHandoffSequence && m.CardId == h.JudgmentCardId && m.From == CardLocation.Judgment(f.OwnerSeat) && m.To == CardLocation.Processing && m.Reason.Value == "skill-program.judgment.result"));
        }

        if (f.AttackAttempt is { } attack && f.AttackReturn is not null && SourceBeigongOwnAttack(f, attack))
        {
            if (child is BeforeDamageProgramWindowFrame before)
                return before.ParentFrameId == f.Id && before.SourceSeat == attack.SourceSeat && before.TargetSeat == attack.TargetSeat && before.Nature == attack.Nature;
            if (_coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(f, child) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(f, child))
                return true;
        }

        if (f.SourceXingxia is { Stage: SourceXingxiaStage.PaymentChildren, PaymentCardId: > 0 } x && _coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.CardId == x.PaymentCardId && m.Sequence > x.Before && m.Sequence <= x.After && m.CardKind == CardKind.SilverLion && m.From == CardLocation.Equipment(x.Enemies[x.Index]) && m.To == CardLocation.DiscardPile))
        {
            var payer = x.Enemies[x.Index];
            if (child is RecoveryReplacementFrame rr)
                return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(rr, f) && rr.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && rr.Attempt.SourceSeat == payer && rr.Attempt.TargetSeat == payer;
            if (child is HpChangedTriggerWindowFrame hp)
                return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.TargetSeat == payer;
        }

        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool SourceBeigongOwnAttack(ProgramSkillFrame f, AttackAttemptState attack)
    {
        if (attack.SourceSeat != f.OwnerSeat)
            return false;
        if (f.SourceHuojie is { Stage: SourceHuojieStage.DamageChildren } h)
            return attack.SourceLess && attack.TargetSeat == f.OwnerSeat && attack.Nature == DamageNature.Thunder && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<SourceHuojieJudgmentIssuedEvent>().Count(e => e.ProgramFrameId == f.Id && e.JudgmentId == h.JudgmentId && e.Ordinal == h.Done) == 1;
        if (f.SourceXingxia is not { } x || attack.SourceLess || attack.Nature != DamageNature.Fire || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<SourceXingxiaIssuedEvent>().Count(e => e.ProgramFrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.Round == x.Round && e.FriendSeat == x.FriendSeat) != 1)
            return false;
        return x.Stage == SourceXingxiaStage.FriendlyDamage && attack.TargetSeat == x.FriendSeat || x.Stage == SourceXingxiaStage.EnemyDamage && x.Index >= 0 && x.Index < x.Enemies.Count && attack.TargetSeat == x.Enemies[x.Index];
    }

    internal bool SourceBeigongJudgmentEdge(int i) => i > 0 && _resolutionStackOperations._resolutionStack[i - 1] is JudgmentFrame j && _coreSourceHuojieOperations.Value.IsSourceHuojieLightningJudgment(j) && _resolutionStackOperations._resolutionStack[i] is ProgramJudgmentTriggerWindowFrame window && window.ParentFrameId == j.Id && window.Judgment.JudgmentFrameId == j.Id;
    internal bool SourceBeigongEdge(int i) => SourceBeigongJudgmentEdge(i) || _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]) || _resolutionStackOperations._resolutionStack[i - 1] is ProgramSkillFrame f && SourceBeigongFirstChild(f, _resolutionStackOperations._resolutionStack[i]) || CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]) || _coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]);
    internal bool SourceBeigongSuffix(int n) => _resolutionStackOperations._resolutionStack[n + 1] is DyingFrame d && n + 2 < _resolutionStackOperations._resolutionStack.Count && _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n + 1, d) || _coreObserverSubtreesOperations.HasObserverSuffix(n, (i) => this.SourceBeigongEdge(i), (i, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, dying));
    internal ProgramSkillFrame? SourceBeigongRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => this.SourceBeigongFirstChild(f, c), (n) => this.SourceBeigongSuffix(n));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 24510)]
    internal bool SourceBeigongDying() => _coreDyingCursorRuntimeOperations.Value.ActiveDying is { } d && SourceBeigongRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 24510)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 24510)]
    internal bool SourceBeigongRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (SourceBeigongFirstChild(f, recovery))
            return true;
        var n = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var c = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return n >= 0 && c > n + 1 && SourceBeigongFirstChild(f, _resolutionStackOperations._resolutionStack[n + 1]) && SourceBeigongSuffix(n) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[c - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 24510)]
    internal bool AdvanceSourceBeigongChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && SourceBeigongRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
