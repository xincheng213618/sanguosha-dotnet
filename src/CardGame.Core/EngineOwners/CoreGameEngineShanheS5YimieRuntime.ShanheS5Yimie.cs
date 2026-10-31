#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheS5YimieRuntime
{
    internal const string ShanheS5YimieSkill = "shanhe:s5-yimie-7005";
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 5795, SkillProgramEffectOp.ResolveShanheS5Yimie)]
    internal bool CanOfferShanheS5Yimie(ProgramTriggerLegalityContext c) => c.SkillId == ShanheS5YimieSkill && c.Owner is { IsAlive: true, HasHpValue: true, Hp: > 0 } && c.Context is { Window: SkillProgramTriggerWindow.BeforeDamageApplied, SourceSeat: { } source, TargetSeat: { } target } && source == c.OwnerSeat && source != target && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && _coreHealthQueriesOperations.UnambiguousHp(_coreMatchParticipantsPlayersAccess._players[target])is not null && _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { IsSourceLess: false, DamageAmount: > 0 } attack && attack.SourceSeat == source && attack.TargetSeat == target && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShanheS5YimieFrozenEvent>().Any(e => e.Origin.Source.OwnerSeat == source && e.Origin.TurnNumber == _coreActualTurnProgressTurnNumberAccess._turnNumber);
    internal bool HasShanheS5YimieCostPaid(ProgramSkillFrame f) => f.ShanheS5YimieCost is { } r && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ProgramSkillHpLostEvent>().Count(e => e.FrameId == f.Id && e.SkillId == f.SkillId && e.TargetSeat == f.OwnerSeat && e.Amount == 1 && e.RemainingHp == r.CostHpBefore - 1) == 1;
    internal void RequireShanheS5YimieOrigin(ShanheS5YimieOrigin r)
    {
        if (r.Source.SkillId != ShanheS5YimieSkill || r.ProgramFrameId <= 0 || r.AttackId <= 0 || r.BeforeDamageWindowId <= 0 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.Source.OwnerSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.TargetSeat) || r.Source.OwnerSeat == r.TargetSeat || r.OriginalDamage <= 0 || r.CostHpBefore <= 0 || r.X != Math.Max(0, r.OriginalTargetHp - r.OriginalDamage) || _contentRegistryOperations._contentRegistry.GetSkill(r.Source.SkillId).Program?.GameplayHash != r.GameplayHash || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShanheS5YimieFrozenEvent>().Count(e => e.Origin == r) != 1)
            throw new InvalidOperationException("Yimie lost its exact frozen original source, target HP or damage amount.");
    }

    internal SkillProgramStepOutcome ResolveShanheS5Yimie(ProgramSkillFrame supplied, string stage)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.WindowContext is not { Window: SkillProgramTriggerWindow.BeforeDamageApplied } context || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not BeforeDamageProgramWindowFrame window || window.Id != context.ParentFrameId || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { IsSourceLess: false } attack || attack.SourceSeat != f.OwnerSeat || attack.SourceSeat != window.SourceSeat || attack.TargetSeat != window.TargetSeat || window.TargetSeat == f.OwnerSeat)
            throw new InvalidOperationException("Yimie requires its original other-target actual damage window.");
        if (stage == "prepare")
        {
            if (f.InstructionIndex != 1 || f.ShanheS5YimieCost is not null || window.Prevented || window.Amount <= 0 || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].HasHpValue || _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp <= 0 || _coreHealthQueriesOperations.UnambiguousHp(_coreMatchParticipantsPlayersAccess._players[window.TargetSeat])is not { } hp || _resolutionStackOperations._resolutionStack.Single(frame => frame.Id == attack.ResolutionId).ShanheS5YimieDamage is not null || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShanheS5YimieFrozenEvent>().Any(e => e.Origin.Source.OwnerSeat == f.OwnerSeat && e.Origin.TurnNumber == _coreActualTurnProgressTurnNumberAccess._turnNumber))
                throw new InvalidOperationException("Yimie cannot duplicate its unpaid original damage opportunity.");
            var r = new ShanheS5YimieOrigin(f.Id, new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, window.Id, attack.ResolutionId, attack is NativeOwners.CardAttackHandle, window.TargetSeat, _coreActualTurnProgressTurnNumberAccess._turnNumber, window.Amount, hp, Math.Max(0, hp - window.Amount), _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp, _coreNativeCardClassificationsOperations.GetDamageNature(attack));
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShanheS5YimieCost = r });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheS5YimieFrozenEvent(r));
            return SkillProgramStepOutcome.Continue;
        }

        var origin = f.ShanheS5YimieCost ?? throw new InvalidOperationException("Yimie lost its accepted HP cost.");
        RequireShanheS5YimieOrigin(origin);
        if (f.InstructionIndex != 3 || !HasShanheS5YimieCostPaid(f) || origin.BeforeDamageWindowId != window.Id || origin.AttackId != attack.ResolutionId || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShanheS5YimieIncreasedEvent>().Any(e => e.ProgramFrameId == f.Id))
            throw new InvalidOperationException("Yimie must return from its exact once-paid native HP loss.");
        if (window.Prevented || !_coreMatchParticipantsPlayersAccess._players[origin.TargetSeat].IsAlive)
            return SkillProgramStepOutcome.Continue;
        var before = window.Amount;
        if (origin.X > 0)
        {
            // Like the shared signed-recipient payment, the original before-damage
            // window and paid cost prove this segment. DamageWasApplied belongs to
            // the entire attack and may already be true for an earlier chain victim.
            var after = checked(before + origin.X);
            if (attack is NativeOwners.ProgramAttackHandle)
                _coreProgramAttackAttemptOperations.UpdateProgramAttackState(attack.ResolutionId, state => state with { DamageAmount = after });
            else
                _coreCardAttackStateOperations.UpdateCardAttackState(attack.ResolutionId, state => state!with { DamageAmount = after });
        }

        _coreRuntimeOperations.ReplaceRuntimeFrame(window.Id, window with { Amount = checked(before + origin.X) });
        var owning = _resolutionStackOperations._resolutionStack.Single(frame => frame.Id == attack.ResolutionId);
        _coreRuntimeOperations.ReplaceRuntimeFrame(owning.Id, owning with { ShanheS5YimieDamage = new(origin) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheS5YimieIncreasedEvent(f.Id, attack.ResolutionId, before, origin.X, before + origin.X));
        return SkillProgramStepOutcome.Continue;
    }

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 5795)]
    internal void RecordShanheS5YimieActualDamage(DamageAppliedEvent damage)
    {
        if (damage.Amount <= 0 || damage.SourceLess || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { } attack || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == attack.ResolutionId)is not { ShanheS5YimieDamage: { } r } owner || damage.SourceSeat != r.Origin.Source.OwnerSeat || damage.TargetSeat != r.Origin.TargetSeat)
            return;
        RequireShanheS5YimieOrigin(r.Origin);
        var frame = _resolutionStackOperations._resolutionStack.OfType<DamageFrame>().LastOrDefault();
        if (r.AppliedDamageFrameId is not null || frame is null || frame.ParentFrameId != owner.Id || frame.TargetSeat != r.Origin.TargetSeat)
            throw new InvalidOperationException("Yimie cannot borrow or duplicate another actual damage segment.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(owner.Id, owner with { ShanheS5YimieDamage = r with { AppliedDamageFrameId = frame.Id, AppliedAmount = damage.Amount } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheS5YimieAppliedEvent(owner.Id, frame.Id, damage.TargetSeat, damage.Amount));
    }

    [ProgramDamageHook(ProgramDamagePhase.AttackCompletionPrelude, 5)]
    internal bool CompleteShanheS5YimieCardDamage(NativeOwners.CardAttackHandle attack, out bool paused)
    {
        paused = TryBeginShanheS5YimieRecovery(attack);
        return paused;
    }

    internal bool TryBeginShanheS5YimieRecovery(GameEngine.IDamageAttempt attack)
    {
        if (_resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == attack.ResolutionId)is not { ShanheS5YimieDamage: { } receipt } owner)
            return false;
        RequireShanheS5YimieOrigin(receipt.Origin);
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != owner.Id)
            throw new InvalidOperationException("Yimie recovery must resume its original quiet attack frame.");
        var actual = receipt.AppliedDamageFrameId is not null;
        if (receipt.Origin.X == 0 || !actual || !_coreMatchParticipantsPlayersAccess._players[receipt.Origin.TargetSeat].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(owner with { ShanheS5YimieDamage = null });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheS5YimieCompletedEvent(owner.Id, receipt.Origin.ProgramFrameId, receipt.Origin.TargetSeat, receipt.Origin.X, actual, false));
            return false;
        }

        var child = new ShanheS5YimieRecoveryFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, owner.Id, receipt);
        _coreRuntimeOperations.PushRuntimeFrame(child);
        ContinueShanheS5YimieRecovery(child.Id);
        return true;
    }

    [NativeFrameAutomaticStep(NativeFrameAutomaticPhase.BeforeOpening, 5795)]
    [NativeFrameRoute(typeof(ShanheS5YimieRecoveryFrame), 5795)]
    internal void ContinueShanheS5YimieRecovery(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ShanheS5YimieRecoveryFrame frame || frame.Id != id)
            return;
        var origin = frame.Receipt.Origin;
        RequireShanheS5YimieOrigin(origin);
        if (!frame.Requested)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { Requested = true });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheS5YimieRecoveryRequestedEvent(frame.Id, frame.ParentFrameId, frame.Receipt.AppliedDamageFrameId!.Value, origin.Source.OwnerSeat, origin.TargetSeat, origin.X));
            var target = _coreMatchParticipantsPlayersAccess._players[origin.TargetSeat];
            if (target.IsAlive && target.HasHpValue && _characterDaQiaoXiaoQiaoHappyGodOperations.HappyQiaoAnyWounded(target.Seat) && !_coreRecoveryReplacementOperations.TryQueueRecoveryReplacement(frame.Id, origin.Source.OwnerSeat, target.Seat, origin.X, new(RecoveryAttemptProducer.Program)))
            {
                var amount = Math.Max(0, Math.Min(origin.X, target.MaxHp - target.Hp));
                if (amount > 0)
                {
                    var recovery = _coreNativeRecoveryFramesOperations.BeginRecovery(frame.Id, origin.Source.OwnerSeat, target.Seat, amount);
                    try
                    {
                        target.Hp += amount;
                        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RecoveryAppliedEvent(origin.Source.OwnerSeat, target.Seat, amount, target.Hp));
                    }
                    finally
                    {
                        _coreRuntimeOperations.PopResolutionFrame(recovery, ResolutionFrameKind.Recovery);
                    }
                }
            }
        }

        if (_coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frame.Id, PostEventContinuation.ShanheS5YimieRecovery))
            return;
        var parent = _resolutionStackOperations._resolutionStack.Single(f => f.Id == frame.ParentFrameId);
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { ShanheS5YimieDamage = null });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheS5YimieCompletedEvent(parent.Id, origin.ProgramFrameId, origin.TargetSeat, origin.X, true, true));
        _coreRuntimeOperations.PopResolutionFrame(frame.Id, ResolutionFrameKind.ShanheS5YimieRecovery);
        _coreNativeDamageCompletionOperations.CompleteDamageAttack(origin.CardAttack ? _kernelCoreMainOperations.CreateCardAttackHandle(origin.AttackId) : _kernelCoreGameEngineProgramAttackAttemptOperations.CreateProgramAttackHandle(origin.AttackId));
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 5795)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 5795)]
    internal bool ContinuePaidShanheS5Yimie(ProgramSkillFrame f) => HasShanheS5YimieCostPaid(f);

    SkillProgramStepOutcome IShanheS5YimieHost.ResolveShanheS5Yimie(ProgramSkillFrame f, string stage) => ResolveShanheS5Yimie(f, stage);
}
