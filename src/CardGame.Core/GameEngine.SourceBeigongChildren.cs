namespace CardGame.Core;

public sealed partial class GameEngine
{
    private bool SourceBeigongFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!HasSourceBeigongState(f)) return false;
        if (child is CardUseFrame use) return SourceDingxiUseRidesOn(f, use);
        if (child is JudgmentFrame judge) return IsSourceHuojieJudgment(judge, f);
        if (child is CardsMovedTriggerWindowFrame moved && f.PendingMovementContinuation is { } pending &&
            SourceBeigongMovement(f, GetPausedPrivateOfferEffect(f), pending))
        {
            var before = f.SourceDingxi?.Before ?? f.SourceHuojie?.Before ?? f.SourceXingxia?.Before ?? -1;
            var after = f.SourceDingxi?.After ?? f.SourceHuojie?.After ?? f.SourceXingxia?.After ?? -1;
            return ProgramMovementReturnMatches(f, moved) && moved.Batch.Movements.Count > 0 &&
                moved.Batch.Movements.All(m => _cardMovements.Contains(m) &&
                    (moved.Batch.ParentFrameId == f.Id && m.Sequence > before && m.Sequence <= after ||
                     f.SourceHuojie is { Stage: SourceHuojieStage.JudgmentChildren } h && moved.Batch.ParentFrameId == h.JudgmentId &&
                     m.Sequence == h.JudgmentHandoffSequence && m.CardId == h.JudgmentCardId &&
                     m.From == CardLocation.Judgment(f.OwnerSeat) && m.To == CardLocation.Processing && m.Reason.Value == "skill-program.judgment.result"));
        }
        if (f.AttackAttempt is { } attack && f.AttackReturn is not null && SourceBeigongOwnAttack(f, attack))
        {
            if (child is BeforeDamageProgramWindowFrame before) return before.ParentFrameId == f.Id &&
                before.SourceSeat == attack.SourceSeat && before.TargetSeat == attack.TargetSeat && before.Nature == attack.Nature;
            if (DyingSuitsStructuralEdge(f, child) || TypedDamagePostEventEdge(f, child)) return true;
        }
        if (f.SourceXingxia is { Stage: SourceXingxiaStage.PaymentChildren, PaymentCardId: > 0 } x &&
            _cardMovements.Any(m => m.CardId == x.PaymentCardId && m.Sequence > x.Before && m.Sequence <= x.After &&
                m.CardKind == CardKind.SilverLion && m.From == CardLocation.Equipment(x.Enemies[x.Index]) && m.To == CardLocation.DiscardPile))
        {
            var payer = x.Enemies[x.Index];
            if (child is RecoveryReplacementFrame rr) return RecoveryReplacementFrameRidesOn(rr, f) && rr.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && rr.Attempt.SourceSeat == payer && rr.Attempt.TargetSeat == payer;
            if (child is HpChangedTriggerWindowFrame hp) return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.TargetSeat == payer;
        }
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id &&
            (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram ||
             state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }
    private bool SourceBeigongOwnAttack(ProgramSkillFrame f, AttackAttemptState attack)
    {
        if (attack.SourceSeat != f.OwnerSeat) return false;
        if (f.SourceHuojie is { Stage: SourceHuojieStage.DamageChildren } h)
            return attack.SourceLess && attack.TargetSeat == f.OwnerSeat && attack.Nature == DamageNature.Thunder &&
                ProgramEventHistory<SourceHuojieJudgmentIssuedEvent>().Count(e => e.ProgramFrameId == f.Id && e.JudgmentId == h.JudgmentId && e.Ordinal == h.Done) == 1;
        if (f.SourceXingxia is not { } x || attack.SourceLess || attack.Nature != DamageNature.Fire ||
            ProgramEventHistory<SourceXingxiaIssuedEvent>().Count(e => e.ProgramFrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.Round == x.Round && e.FriendSeat == x.FriendSeat) != 1) return false;
        return x.Stage == SourceXingxiaStage.FriendlyDamage && attack.TargetSeat == x.FriendSeat ||
            x.Stage == SourceXingxiaStage.EnemyDamage && x.Index >= 0 && x.Index < x.Enemies.Count && attack.TargetSeat == x.Enemies[x.Index];
    }
    private bool SourceBeigongJudgmentEdge(int i) => i > 0 && _resolutionStack[i - 1] is JudgmentFrame j && IsSourceHuojieLightningJudgment(j) &&
        _resolutionStack[i] is ProgramJudgmentTriggerWindowFrame window && window.ParentFrameId == j.Id && window.Judgment.JudgmentFrameId == j.Id;
    private bool SourceBeigongEdge(int i) => SourceBeigongJudgmentEdge(i) || IsMovementObserverEdge(i) || IsSameNameHandObserverEdge(i) ||
        IsYanPuDyingExitEdge(i) || TypedDamagePostEventEdge(_resolutionStack[i - 1], _resolutionStack[i]) ||
        _resolutionStack[i - 1] is ProgramSkillFrame f && SourceBeigongFirstChild(f, _resolutionStack[i]) ||
        NativeDrawAttemptRidesOn(_resolutionStack[i], _resolutionStack[i - 1]) || RecoveryReplacementFrameRidesOn(_resolutionStack[i], _resolutionStack[i - 1]) ||
        DyingSuitsStructuralEdge(_resolutionStack[i - 1], _resolutionStack[i]);
    private bool SourceBeigongSuffix(int n) => _resolutionStack[n + 1] is DyingFrame d && n + 2 < _resolutionStack.Count && IsSuspendedOrPaidRescueObserverSuffix(n + 1, d) ||
        HasObserverSuffix(n, static (g, i) => g.SourceBeigongEdge(i), static (g, i, dying) => g.IsSuspendedOrPaidRescueObserverSuffix(i, dying));
    private ProgramSkillFrame? SourceBeigongRoot() => FindProgramObserverRoot(static (g, f, c) => g.SourceBeigongFirstChild(f, c), static (g, n) => g.SourceBeigongSuffix(n));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 24510)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 24510)]
    private bool SourceBeigongDamageWindow(long id) => SourceBeigongRoot() is { } f && _resolutionStack.FindIndex(x => x.Id == id && x is DamageTriggerWindowFrame) > _resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 24510)]
    private bool SourceBeigongBeforeDamage(long id) => SourceBeigongRoot() is { } f && _resolutionStack.FindIndex(x => x.Id == id && x is BeforeDamageProgramWindowFrame) > _resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 24510)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 24510)]
    private bool SourceBeigongDying() => ActiveDying is { } d && SourceBeigongRoot() is { } f && _resolutionStack.FindIndex(x => x.Id == d.Id) > _resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramNestedDamageProof(ProgramNestedDamageProofPhase.BeforeFactionDebts, 24510)]
    [ProgramNestedDamageProof(ProgramNestedDamageProofPhase.AfterFactionDebts, 24510)]
    private bool SourceBeigongNestedDamage(ProgramSkillFrame f, int target, int amount, ProgramParticipantReference? source, DamageNature? nature, bool sourceLess)
    {
        if (f.AttackAttempt is not null || _resolutionStack.LastOrDefault()?.Id != f.Id || sourceLess || SourceBeigongRoot() is not { } root || root.Id == f.Id) return false;
        var e = GetPausedPrivateOfferEffect(f);
        return e is { Op: SkillProgramEffectOp.Damage } && amount == e.Amount && source == e.ActorReference && nature == e.DamageNature &&
            target == (e.TargetReference is { } p ? ResolveProgramParticipant(f, p) : ResolveProgramEffectTarget(f, e.Target));
    }
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 24510)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 24510)]
    private bool SourceBeigongRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (SourceBeigongFirstChild(f, recovery)) return true;
        var n = _resolutionStack.FindIndex(x => x.Id == f.Id); var c = _resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return n >= 0 && c > n + 1 && SourceBeigongFirstChild(f, _resolutionStack[n + 1]) && SourceBeigongSuffix(n) && RecoveryReplacementFrameRidesOn(recovery, _resolutionStack[c - 1]);
    }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 24510)]
    private bool AdvanceSourceBeigongChildren() => _pendingDecision is null && SourceBeigongRoot() is not null && TryAdvanceObserverSubtree(true);
}
