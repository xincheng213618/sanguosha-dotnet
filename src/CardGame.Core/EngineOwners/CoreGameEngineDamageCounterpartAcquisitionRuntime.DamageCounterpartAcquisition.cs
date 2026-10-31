#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDamageCounterpartAcquisitionRuntime
{
    internal bool CanOfferDamageCounterpartAcquisition(int owner, SkillProgramTrigger trigger, global::CardGame.Core.GameEngine.IDamageAttempt attack) => !trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.RecordDamageCounterpartAcquisition) || !attack.IsSourceLess && attack.DamageWasApplied && attack.DamageAmount > 0 && _coreTableSeatQueriesOperations.IsValidPlayerSeat(attack.SourceSeat) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(attack.TargetSeat) && attack.SourceSeat != attack.TargetSeat && (owner == attack.SourceSeat || owner == attack.TargetSeat);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 1010)]
    internal bool CanOfferDamageCounterpartAcquisition(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (!trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.RecordDamageCounterpartAcquisition))
            return true;
        return context is { Window: SkillProgramTriggerWindow.AfterDamageApplied, SourceSeat: { } source, TargetSeat: { } target, Amount: > 0, DamageFrameId: { } damageId } && source != target && (candidate.OwnerSeat == source || candidate.OwnerSeat == target) && _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == context.ParentFrameId)is DamageTriggerWindowFrame window && window.ParentFrameId == damageId && window.TriggerWindow == context.Window && CanOfferDamageCounterpartAcquisition(candidate.OwnerSeat, trigger, _coreNativeDamageHandlesOperations.GetDamageTriggerAttack(window)) && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DamageCounterpartAcquisitionGrantedEvent>().Any(e => e.Source.OwnerSeat == candidate.OwnerSeat && e.Source.SkillId == candidate.SkillId && e.StateId == trigger.Effects[0].StateId && e.TargetSkillId == trigger.Effects[0].SkillIds.Single() && e.DamageFrameId == damageId);
    }

    internal SkillProgramStepOutcome RecordDamageCounterpartAcquisition(ProgramSkillFrame supplied, string stateId, string skillId)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.TriggerId is null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, SourceSeat: { } source, TargetSeat: { } target, DamageFrameId: { } damageId, Amount: > 0 } context || _resolutionStackOperations._resolutionStack.Count < 3 || _resolutionStackOperations._resolutionStack[^2] is not DamageTriggerWindowFrame window || window.Id != context.ParentFrameId || window.ParentFrameId != damageId || window.TriggerWindow != context.Window || _resolutionStackOperations._resolutionStack[^3] is not DamageFrame damage || damage.Id != damageId || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, window.Candidates[window.CandidateIndex].ToProgramCandidate()) || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program is not { } program || program.GameplayHash != f.GameplayHash)
            throw new InvalidOperationException("Counterpart acquisition lost its exact positive applied-damage candidate.");
        var trigger = ProgramInstructionResolver.Default.Resolve(f, program).Trigger!;
        DamageCounterpartAcquisitionContract.ValidateTrigger(f.SkillId, trigger);
        if (trigger.Effects[0].StateId != stateId || trigger.Effects[0].SkillIds.Single() != skillId || context.OccurrenceIndex != 0)
            throw new InvalidOperationException("Counterpart acquisition changed its declared state or target skill.");
        var candidate = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, 0);
        if (!CanOfferDamageCounterpartAcquisition(candidate, trigger, context))
            return SkillProgramStepOutcome.Continue;
        var attack = _coreNativeDamageHandlesOperations.GetDamageTriggerAttack(window);
        if (window.SourceSeat != source || window.TargetSeat != target || damage.SourceSeat != source || damage.TargetSeat != target || damage.Amount != context.Amount || damage.Nature != _coreNativeCardClassificationsOperations.GetDamageNature(attack) || damage.ParentFrameId != attack.ResolutionId || attack.SourceSeat != source || attack.TargetSeat != target || attack.DamageAmount != context.Amount || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DamageRequestedEvent>().Any(e => e.ResolutionId == damageId && e.SourceSeat == source && e.TargetSeat == target && e.Amount == context.Amount && e.Nature == damage.Nature && !e.SourceLess))
            throw new InvalidOperationException("Counterpart acquisition cannot be issued from HP loss, sourceless or unrelated damage.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DamageCounterpartAcquisitionGrantedEvent(f.Id, new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, stateId, skillId, source == f.OwnerSeat ? target : source, damageId, attack.ResolutionId, source, target, context.Amount, damage.Nature, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat));
        return SkillProgramStepOutcome.Continue;
    }

    internal bool HasDamageCounterpartAcquisition(int owner, string skillId, string stateId, int counterpart)
    {
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var cutoff = Array.FindLastIndex(history, e => e is TurnStartedEvent t && t.ActorSeat == owner);
        return history.Skip(cutoff + 1).OfType<DamageCounterpartAcquisitionGrantedEvent>().Any(e => e.Source.OwnerSeat == owner && e.TargetSkillId == skillId && e.StateId == stateId && e.CounterpartSeat == counterpart && e.Amount > 0 && e.DamageSourceSeat != e.DamageTargetSeat && (e.DamageSourceSeat == owner && e.DamageTargetSeat == counterpart || e.DamageTargetSeat == owner && e.DamageSourceSeat == counterpart) && _contentRegistryOperations._contentRegistry.GetSkill(e.Source.SkillId).Program is { } program && program.GameplayHash == e.GameplayHash && ProgramInstructionResolver.Default.FindTrigger(program, e.Source.BindingId)is { } trigger && trigger.Effects is [{ Op: SkillProgramEffectOp.RecordDamageCounterpartAcquisition, SkillIds: [var targetSkill] } effect] && targetSkill == skillId && effect.StateId == stateId);
    }

    SkillProgramStepOutcome global::CardGame.Core.IDamageCounterpartAcquisitionHost.RecordDamageCounterpartAcquisition(ProgramSkillFrame f, string stateId, string skillId) => RecordDamageCounterpartAcquisition(f, stateId, skillId);
}
