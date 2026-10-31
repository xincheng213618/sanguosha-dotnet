#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Text.Json.Serialization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXiZhiCaiOlRuntime
{
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 590)]
    internal bool AllowsRegisteredIsXianfuEchoTrigger(ProgramTriggerLegalityContext gate)
    {
        var candidate = gate.Candidate;
        var trigger = gate.Trigger;
        var context = gate.Context;
        return !(IsXianfuEchoTrigger(trigger) && !CanRunXianfuEcho(candidate, context));
    }

    internal bool HasXianfuRelation
    {
        get
        {
            foreach (var p in _coreMatchParticipantsPlayersAccess._players)
                if (p.XianfuRelations.Count > 0)
                    return true;
            return false;
        }
    }

    internal IEnumerable<XianfuRelationGrant> LiveXianfuRelations() => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.XianfuRelations.Count > 0).SelectMany(p => p.XianfuRelations).Where(g => _coreMatchParticipantsPlayersAccess._players[g.TargetSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[g.Source.OwnerSeat], g.Source.SkillId, g.Source.SkillInstanceId));
    internal SkillProgramStepOutcome ArmXianfuRelation(ProgramSkillFrame supplied, int target)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.WindowContext is not { Window: SkillProgramTriggerWindow.GameStarting } w || f.SelectedTargetSeats is not [var selected] || selected != target || target == f.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].XianfuRelations.Any(g => g.Source.SkillInstanceId == f.SkillInstanceId))
            throw new InvalidOperationException("Xianfu requires its original actual game-start other-character selection.");
        var g = new XianfuRelationGrant(f.Id, XiZhiCaiSource(f), f.GameplayHash, target, w.ParentFrameId);
        _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].XianfuRelations = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].XianfuRelations.Append(g).ToArray();
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XianfuRelationArmedEvent(g));
        return SkillProgramStepOutcome.Continue;
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 130)]
    internal void ObserveXianfuDamageQualification(IGameEvent payload)
    {
        if (payload is not DamageAppliedEvent { Amount: > 0 } e || !HasXianfuRelation || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { } attack || !attack.DamageWasApplied || attack.TargetSeat != e.TargetSeat || attack.DamageAmount != e.Amount)
            return;
        var native = _resolutionStackOperations._resolutionStack.OfType<DamageFrame>().LastOrDefault(d => d.ParentFrameId == attack.ResolutionId && d.TargetSeat == e.TargetSeat && d.Amount == e.Amount) ?? throw new InvalidOperationException("Xianfu damage lacks its native damage owner.");
        foreach (var g in LiveXianfuRelations().Where(g => g.TargetSeat == e.TargetSeat).ToArray())
        {
            var receipt = new XianfuEchoQualification(g, XianfuEchoKind.Damage, native.Id, native.ParentFrameId, e.TargetSeat, e.Amount, attack.IsSourceLess || attack.IsDelayedJudgmentDamage ? null : e.SourceSeat, null, e.RemainingHp);
            if (!_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<XianfuEchoQualifiedEvent>().Any(x => x.Receipt.Grant == g && x.Receipt.NativeChangeId == native.Id))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XianfuEchoQualifiedEvent(receipt));
        }
    }

    internal void CaptureXianfuRecoveryQualification(HpChangeContext change)
    {
        if (!HasXianfuRelation || change.Kind != HpChangeKind.Recovery || change.HpAfter <= change.HpBefore)
            return;
        foreach (var g in LiveXianfuRelations().Where(g => g.TargetSeat == change.TargetSeat).ToArray())
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XianfuEchoQualifiedEvent(new(g, XianfuEchoKind.Recovery, change.Id, change.ParentFrameId ?? 0, change.TargetSeat, change.HpAfter - change.HpBefore, change.SourceSeat, change.HpBefore, change.HpAfter)));
    }

    internal bool IsXianfuEchoTrigger(SkillProgramTrigger trigger) => trigger.Effects is [{ Op: SkillProgramEffectOp.SettleXianfuNativeEcho }];
    internal XianfuEchoQualification? XianfuQualification(ProgramTriggerCandidate c, long nativeId) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<XianfuEchoQualifiedEvent>().Select(e => e.Receipt).SingleOrDefault(r => r.NativeChangeId == nativeId && r.Grant.Source.OwnerSeat == c.OwnerSeat && r.Grant.Source.SkillId == c.SkillId && r.Grant.Source.SkillInstanceId == c.SkillInstanceId);
    internal bool XianfuQualifiedSourceEnabled(XianfuEchoQualification r) => _coreMatchParticipantsPlayersAccess._players[r.Grant.Source.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[r.Grant.Source.OwnerSeat].XianfuRelations.Contains(r.Grant) && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[r.Grant.Source.OwnerSeat], r.Grant.Source.SkillId, r.Grant.Source.SkillInstanceId);
    internal bool XianfuEchoConsumed(XianfuEchoQualification r) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<XianfuEchoIssuedEvent>().Any(e => e.Receipt.Grant == r.Grant && e.Receipt.NativeChangeId == r.NativeChangeId);
    internal IReadOnlyList<ProgramTriggerCandidate> CollectXianfuHpCandidates(HpChangeContext change)
    {
        if (!HasXianfuRelation || change.Kind != HpChangeKind.Recovery || !_coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.XianfuRelations.Any(g => g.TargetSeat == change.TargetSeat)))
            return[];
        return _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.XianfuRelations.Count > 0).SelectMany(p => p.XianfuRelations).Where(g => g.TargetSeat == change.TargetSeat).SelectMany(g => _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(_coreMatchParticipantsPlayersAccess._players[g.Source.OwnerSeat], SkillProgramTriggerWindow.AfterHpRecovered).Where(c => c.SkillId == g.Source.SkillId && c.SkillInstanceId == g.Source.SkillInstanceId && IsXianfuEchoTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(c)) && XianfuQualification(c, change.Id)is { } q && XianfuQualifiedSourceEnabled(q) && !XianfuEchoConsumed(q))).Distinct().ToArray();
    }

    internal ProgramSkillWindowContext CreateXianfuHpContext(HpChangeContext change, ProgramTriggerCandidate c)
    {
        var r = XianfuQualification(c, change.Id) ?? throw new InvalidOperationException("Xianfu lacks its actual recovered-HP qualification.");
        return new(SkillProgramTriggerWindow.AfterHpRecovered, change.Id, c.OwnerSeat, SourceSeat: change.SourceSeat, TargetSeat: change.TargetSeat, Amount: r.ActualAmount, HpChange: change)
        {
            XianfuEcho = r
        };
    }

    internal ProgramSkillWindowContext CreateXianfuDamageContext(DamageTriggerWindowFrame window, ProgramTriggerCandidate c)
    {
        var r = XianfuQualification(c, window.ParentFrameId) ?? throw new InvalidOperationException("Xianfu lacks its actual applied-damage qualification.");
        return new(SkillProgramTriggerWindow.AfterDamageApplied, window.Id, c.OwnerSeat, SourceSeat: r.OriginalSourceSeat, TargetSeat: r.OriginalTargetSeat, DamageFrameId: window.ParentFrameId, Amount: r.ActualAmount)
        {
            XianfuEcho = r
        };
    }

    internal bool CanOfferXianfuDamage(int ownerSeat, SkillProgramTrigger trigger, global::CardGame.Core.GameEngine.IDamageAttempt attack)
    {
        if (!IsXianfuEchoTrigger(trigger))
            return true;
        return _coreMatchParticipantsPlayersAccess._players[ownerSeat].XianfuRelations.Any(g => g.Source.OwnerSeat == ownerSeat && g.TargetSeat == attack.TargetSeat && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<XianfuEchoQualifiedEvent>().Any(e => e.Receipt.Grant == g && e.Receipt.Kind == XianfuEchoKind.Damage && e.Receipt.NativeParentFrameId == attack.ResolutionId && XianfuQualifiedSourceEnabled(e.Receipt) && !XianfuEchoConsumed(e.Receipt)));
    }

    internal bool CanRunXianfuEcho(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (context.XianfuEcho is not { } r || r.Grant.Source.OwnerSeat != c.OwnerSeat || r.Grant.Source.SkillId != c.SkillId || r.Grant.Source.SkillInstanceId != c.SkillInstanceId || r.ActualAmount <= 0 || XianfuEchoConsumed(r) || !XianfuQualifiedSourceEnabled(r))
            return false;
        return r == XianfuQualification(c, r.NativeChangeId) && (r.Kind == XianfuEchoKind.Damage ? context.DamageFrameId == r.NativeChangeId && context.Window == SkillProgramTriggerWindow.AfterDamageApplied : context.HpChange is { } hp && context.Window == SkillProgramTriggerWindow.AfterHpRecovered && hp.Id == r.NativeChangeId && hp.Kind == HpChangeKind.Recovery && hp.HpAfter - hp.HpBefore == r.ActualAmount);
    }

    internal SkillProgramStepOutcome SettleXianfuNativeEcho(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.WindowContext?.XianfuEcho ?? throw new InvalidOperationException("Xianfu lost its issued native cause.");
        if (XianfuEchoConsumed(r))
            throw new InvalidOperationException("Xianfu cannot pay the same actual native mutation twice.");
        var nativeAmount = r.Kind == XianfuEchoKind.Damage ? r.ActualAmount : Math.Max(0, Math.Min(r.ActualAmount, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp - _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { XianfuPending = new(r, f.InstructionIndex, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp, true, nativeAmount), PendingMovementContinuation = r.Kind == XianfuEchoKind.Recovery ? new(f.OwnerSeat, 0, null) : null });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XianfuEchoIssuedEvent(f.Id, r, f.InstructionIndex, nativeAmount));
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            FinishXianfuEcho(f);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (r.Kind == XianfuEchoKind.Damage)
            return _coreSkillProgramsOperations.BeginProgramSkillDamage(f, f.OwnerSeat, r.ActualAmount, sourceLess: true);
        _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, f.OwnerSeat, f.OwnerSeat, nativeAmount, null, null);
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 23101)]
    internal bool ResumeXianfuEcho(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { XianfuPending: { } } f || f.Id != id)
            return false;
        RequireIssuedXiZhiCai(f);
        if (f.AttackAttempt is not null)
            return false;
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return true;
        FinishXianfuEcho(f);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 23101)]
    internal bool ReturnXianfuEchoMovement(ProgramSkillFrame f) => f.XianfuPending is not null && ResumeXianfuEcho(f.Id);
    internal void FinishXianfuEcho(ProgramSkillFrame f)
    {
        var r = f.XianfuPending!.Receipt;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XianfuEchoCompletedEvent(f.Id, r.NativeChangeId, r.Kind));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { XianfuPending = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramTriggerProof(ProgramTriggerProofPhase.HpChangeOwner, 70, ProgramTriggerProofMatch.Decision)]
    internal bool? ProveXianfuHealth(SkillProgramTrigger trigger, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => IsXianfuEchoTrigger(trigger) ? CanRunXianfuEcho(candidate, context) : null;
}
