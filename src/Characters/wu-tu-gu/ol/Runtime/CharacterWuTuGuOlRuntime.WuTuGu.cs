#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWuTuGuOlRuntime
{
    internal bool HasWuTuGuCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ArmHanyongNativeGroupDamage) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.AccumulateRanshangFirePoint) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveRanshangEndingHpLoss);

    internal static CardConversionSource WuTuGuSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_wu_tu_gu_ol_WuTuGu.WuTuGuSource(f);
    SkillProgramStepOutcome global::CardGame.Core.IWuTuGuHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginWuTuGu(f, op);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 27320)]
    internal bool CanOfferWuTuGu(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => t.Effects is [{ Op: SkillProgramEffectOp.ArmHanyongNativeGroupDamage }] ? WuTuGuHanyongQualification(c, context)is not null : t.Effects is not [{ Op: SkillProgramEffectOp.ResolveRanshangEndingHpLoss }] || _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].Markers.GetValueOrDefault(PlayerMarkerKind.RanshangRan) > 0;
    internal SkillProgramStepOutcome BeginWuTuGu(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.InstructionIndex != 1 || f.TriggerId is null || f.ActivationId != f.TriggerId || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { } context || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != op)
            throw new InvalidOperationException("Wu Tu Gu requires the exact original native binding.");
        if (op == SkillProgramEffectOp.ArmHanyongNativeGroupDamage)
        {
            if (f.SkillId != "ol:hanyong" || context.Window != SkillProgramTriggerWindow.CardUseCommitted || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not ProgramCardTriggerWindowFrame parent || parent.Id != context.ParentFrameId || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(parent.Candidates[parent.CandidateIndex])) || WuTuGuHanyongQualification(global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(parent.Candidates[parent.CandidateIndex]), context)is not { } q)
                throw new InvalidOperationException("Hanyong lost its frozen native-use HP/round qualification.");
            var use = _coreCardUseLifecycleOperations.LifecycleCardUse(q.UseId)!;
            if (use.WuTuGuHanyong?.Grants.Any(g => g.Source == q.Source) == true)
                throw new InvalidOperationException("Hanyong cannot arm one original source twice.");
            var grant = new WuTuGuHanyongGrant(f.Id, q.UseId, q.ActionId, q.Source, q.GameplayHash, q.ActualTurn, q.Round, q.HpAtUse, q.ProviderSeat, q.Kind);
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { WuTuGuHanyong = new() { Grants = (u.WuTuGuHanyong?.Grants ?? []).Append(grant).ToArray() } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuTuGuHanyongArmedEvent(grant));
            return SkillProgramStepOutcome.Continue;
        }

        if (op == SkillProgramEffectOp.AccumulateRanshangFirePoint)
        {
            if (f.SkillId != "ol:ranshang" || context.Window != SkillProgramTriggerWindow.AfterDamageApplied || context.TargetSeat != f.OwnerSeat || context.Amount <= 0 || _resolutionStackOperations._resolutionStack.Count < 3 || _resolutionStackOperations._resolutionStack[^2] is not DamageTriggerWindowFrame damage || damage.Id != context.ParentFrameId || damage.ParentFrameId != context.DamageFrameId || damage.TargetSeat != f.OwnerSeat || damage.TriggerWindow != context.Window || damage.CandidateIndex < 0 || damage.CandidateIndex >= damage.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, damage.Candidates[damage.CandidateIndex].ToProgramCandidate()) || _coreNativeCardClassificationsOperations.GetDamageNature(_coreNativeDamageHandlesOperations.GetDamageTriggerAttack(damage)) != DamageNature.Fire || !_coreNativeDamageHandlesOperations.GetDamageTriggerAttack(damage).DamageWasApplied || context.OccurrenceIndex < 0 || context.OccurrenceIndex >= context.Amount)
                throw new InvalidOperationException("Ranshang requires one exact point of truly applied native fire damage.");
            var source = WuTuGuSource(f);
            if (_coreProgramEventHistoryOperations.ProgramEventHistory<WuTuGuFirePointAddedEvent>().Any(e => e.DamageFrameId == context.DamageFrameId && e.Source == source && e.Point == context.OccurrenceIndex))
                throw new InvalidOperationException("Ranshang cannot count a fire point twice.");
            var count = checked(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Markers.GetValueOrDefault(PlayerMarkerKind.RanshangRan) + 1);
            _coreStrategicProgramsOperations.SetStrategicMarker(f.OwnerSeat, PlayerMarkerKind.RanshangRan, f.OwnerSeat, count, f);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuTuGuFirePointAddedEvent(f.Id, context.DamageFrameId!.Value, context.ParentFrameId, source, f.GameplayHash, context.OccurrenceIndex, context.Amount, count));
            return SkillProgramStepOutcome.Continue;
        }

        if (op != SkillProgramEffectOp.ResolveRanshangEndingHpLoss || f.SkillId != "ol:ranshang" || f.WuTuGuRanshang is not null || !WuTuGuEndingParentMatches(f))
            throw new InvalidOperationException("Ranshang requires its original actual Ending binding.");
        var amount = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Markers.GetValueOrDefault(PlayerMarkerKind.RanshangRan);
        if (amount <= 0)
            return SkillProgramStepOutcome.Continue;
        var r = new WuTuGuRanshangReceipt(WuTuGuSource(f), f.GameplayHash, context.ParentFrameId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, amount, WuTuGuRanshangStage.Issuing);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuTuGuRanshang = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuTuGuRanshangIssuedEvent(f.Id, r.Source, r.GameplayHash, r.ParentId, r.ActualTurn, r.Amount));
        ContinueWuTuGu(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void ContinueWuTuGu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { WuTuGuRanshang: { } r } f || f.Id != id)
            return;
        AssertWuTuGuRanshang(f);
        if (r.Stage == WuTuGuRanshangStage.Issuing)
        {
            // Use the existing native unclamped LoseHp primitive: one actual
            // loss of X, one Dying procedure, never one rescue window per mark.
            _ = _coreParticipantAndReserveProgramsOperations.ExecuteParticipantReserve(new SkillProgramEffect(SkillProgramEffectOp.LoseHpUnclamped, SkillProgramEffectTarget.Owner, r.Amount, new(SkillProgramConditionKind.Always, 0, [])), f);
            var owner = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(p => p.Id == id);
            if (owner is null)
                return;
            if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != id)
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.WuTuGuRanshang!;
            if (r.Stage == WuTuGuRanshangStage.Issuing)
                return;
        }

        if (_coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuTuGuRanshangCompletedEvent(id));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuTuGuRanshang = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27320)]
    internal bool ResumeWuTuGu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { WuTuGuRanshang: not null } f || f.Id != id)
            return false;
        ContinueWuTuGu(id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27320)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27320)]
    internal bool ContinuePaidWuTuGu(ProgramSkillFrame f) => f.WuTuGuRanshang is { Stage: WuTuGuRanshangStage.HpChildren } && ValidWuTuGuRanshang(f);
}
