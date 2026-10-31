#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;
using System.Text.Json.Serialization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuangQuanOlRuntime
{
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 600)]
    internal bool AllowsRegisteredIsDianhuDrawTrigger(ProgramTriggerLegalityContext gate)
    {
        var candidate = gate.Candidate;
        var trigger = gate.Trigger;
        var context = gate.Context;
        return !(IsDianhuDrawTrigger(trigger) && !CanRunDianhu(candidate, context));
    }

    internal bool HasDianhuRelation
    {
        get
        {
            foreach (var p in _coreMatchParticipantsPlayersAccess._players)
                if (p.DianhuRelations.Count != 0)
                    return true;
            return false;
        }
    }

    internal bool DianhuSourceEnabled(DianhuRelation g) => _coreMatchParticipantsPlayersAccess._players[g.Source.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[g.Source.OwnerSeat].DianhuRelations.Contains(g) && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[g.Source.OwnerSeat], g.Source.SkillId, g.Source.SkillInstanceId) && _contentRegistryOperations._contentRegistry.GetSkill(g.Source.SkillId).Program?.GameplayHash == g.GameplayHash;
    internal IEnumerable<DianhuRelation> LiveDianhuRelations() => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.DianhuRelations.Count > 0).SelectMany(p => p.DianhuRelations).Where(g => _coreMatchParticipantsPlayersAccess._players[g.TargetSeat].IsAlive && DianhuSourceEnabled(g));
    internal bool IsDianhuDrawTrigger(SkillProgramTrigger t) => t.Effects is [{ Op: SkillProgramEffectOp.DrawForDianhuActualOccurrence }];
    internal DianhuOccurrence? DianhuQualification(ProgramTriggerCandidate c, long nativeId) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DianhuQualifiedEvent>().Select(e => e.Receipt).SingleOrDefault(r => r.NativeChangeId == nativeId && r.Relation.Source.OwnerSeat == c.OwnerSeat && r.Relation.Source.SkillId == c.SkillId && r.Relation.Source.SkillInstanceId == c.SkillInstanceId);
    internal bool DianhuConsumed(DianhuOccurrence r) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<HuangQuanDrawPaidEvent>().Any(e => e.Occurrence == r);
    internal SkillProgramStepOutcome ArmDianhuRelation(ProgramSkillFrame supplied, int target)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.WindowContext is not { Window: SkillProgramTriggerWindow.GameStarting } w || f.SelectedTargetSeats is not [var selected] || target != selected || target == f.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].DianhuRelations.Any(g => g.Source.SkillInstanceId == f.SkillInstanceId))
            throw new InvalidOperationException("Dianhu requires its actual game-start other-living choice exactly once.");
        var grant = new DianhuRelation(f.Id, HuangQuanSource(f), f.GameplayHash, target, w.ParentFrameId);
        _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].DianhuRelations = Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].DianhuRelations.Append(grant).ToArray());
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DianhuArmedEvent(grant));
        return SkillProgramStepOutcome.Continue;
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 140)]
    internal void ObserveDianhuDamageQualification(IGameEvent payload)
    {
        if (payload is not DamageAppliedEvent e || !HasDianhuRelation || e.SourceLess || e.Amount <= 0 || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { } attack || attack.IsSourceLess || attack.IsDelayedJudgmentDamage || !attack.DamageWasApplied || attack.TargetSeat != e.TargetSeat || attack.SourceSeat != e.SourceSeat)
            return;
        var native = _resolutionStackOperations._resolutionStack.OfType<DamageFrame>().LastOrDefault();
        if (native is null || native.ParentFrameId != attack.ResolutionId || native.TargetSeat != e.TargetSeat || native.Amount != e.Amount)
            return;
        foreach (var g in LiveDianhuRelations().Where(g => g.TargetSeat == e.TargetSeat && g.Source.OwnerSeat == e.SourceSeat).ToArray())
        {
            var r = new DianhuOccurrence(g, false, native.Id, attack.ResolutionId, e.TargetSeat, e.Amount, e.SourceSeat);
            if (!_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DianhuQualifiedEvent>().Any(q => q.Receipt == r))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DianhuQualifiedEvent(r));
        }
    }

    internal void CaptureDianhuRecoveryQualification(HpChangeContext change)
    {
        if (change.Kind != HpChangeKind.Recovery || change.HpAfter <= change.HpBefore || !HasDianhuRelation)
            return;
        foreach (var g in LiveDianhuRelations().Where(g => g.TargetSeat == change.TargetSeat).ToArray())
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DianhuQualifiedEvent(new(g, true, change.Id, change.ParentFrameId ?? 0, change.TargetSeat, change.HpAfter - change.HpBefore, change.SourceSeat)));
    }

    internal IReadOnlyList<ProgramTriggerCandidate> CollectDianhuHpCandidates(HpChangeContext change)
    {
        if (change.Kind != HpChangeKind.Recovery || !HasDianhuRelation)
            return[];
        return _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.DianhuRelations.Count > 0).SelectMany(p => p.DianhuRelations).Where(g => g.TargetSeat == change.TargetSeat && DianhuSourceEnabled(g)).SelectMany(g => _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(_coreMatchParticipantsPlayersAccess._players[g.Source.OwnerSeat], SkillProgramTriggerWindow.AfterHpRecovered).Where(c => c.SkillId == g.Source.SkillId && c.SkillInstanceId == g.Source.SkillInstanceId && IsDianhuDrawTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(c)) && DianhuQualification(c, change.Id)is { } r && !DianhuConsumed(r))).Distinct().ToArray();
    }

    internal ProgramSkillWindowContext CreateDianhuHpContext(HpChangeContext change, ProgramTriggerCandidate c)
    {
        var r = DianhuQualification(c, change.Id) ?? throw new InvalidOperationException("Dianhu requires its actual recovered HP occurrence.");
        return new(SkillProgramTriggerWindow.AfterHpRecovered, change.Id, c.OwnerSeat, SourceSeat: change.SourceSeat, TargetSeat: change.TargetSeat, Amount: change.Amount, HpChange: change)
        {
            Dianhu = r
        };
    }

    internal ProgramSkillWindowContext CreateDianhuDamageContext(DamageTriggerWindowFrame w, ProgramTriggerCandidate c)
    {
        var r = DianhuQualification(c, w.ParentFrameId) ?? throw new InvalidOperationException("Dianhu requires its actual owner-caused damage occurrence.");
        return new(SkillProgramTriggerWindow.AfterDamageApplied, w.Id, c.OwnerSeat, SourceSeat: r.SourceSeat, TargetSeat: r.TargetSeat, DamageFrameId: w.ParentFrameId, Amount: r.ActualAmount)
        {
            Dianhu = r
        };
    }

    internal bool CanOfferDianhuDamage(int owner, SkillProgramTrigger t, global::CardGame.Core.GameEngine.IDamageAttempt attack) => !IsDianhuDrawTrigger(t) || !attack.IsSourceLess && !attack.IsDelayedJudgmentDamage && attack.SourceSeat == owner && _coreMatchParticipantsPlayersAccess._players[owner].DianhuRelations.Any(g => g.TargetSeat == attack.TargetSeat && DianhuSourceEnabled(g) && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DianhuQualifiedEvent>().Any(e => e.Receipt.Relation == g && e.Receipt.NativeParentId == attack.ResolutionId && !e.Receipt.Recovery && !DianhuConsumed(e.Receipt)));
    internal bool CanRunDianhu(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (context.Dianhu is not { } r || r.Relation.Source.OwnerSeat != c.OwnerSeat || r.Relation.Source.SkillId != c.SkillId || r.Relation.Source.SkillInstanceId != c.SkillInstanceId || !DianhuSourceEnabled(r.Relation) || DianhuConsumed(r) || r != DianhuQualification(c, r.NativeChangeId))
            return false;
        return r.Recovery ? context.HpChange is { Kind: HpChangeKind.Recovery } hp && hp.Id == r.NativeChangeId && hp.HpAfter - hp.HpBefore == r.ActualAmount : context.DamageFrameId == r.NativeChangeId && context.SourceSeat == c.OwnerSeat && context.Amount == r.ActualAmount;
    }

    internal SkillProgramStepOutcome DrawForDianhuActualOccurrence(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.WindowContext?.Dianhu ?? throw new InvalidOperationException("Dianhu lost its native occurrence.");
        if (DianhuConsumed(r))
            throw new InvalidOperationException("Dianhu cannot draw twice for the same actual event.");
        return IssueHuangQuanDraw(f, f.OwnerSeat, "dianhu", r);
    }

    [ProgramTriggerProof(ProgramTriggerProofPhase.HpChangeOwner, 80, ProgramTriggerProofMatch.Decision)]
    internal bool? ProveDianhuHealth(SkillProgramTrigger trigger, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => IsDianhuDrawTrigger(trigger) ? CanRunDianhu(candidate, context) : null;
}
