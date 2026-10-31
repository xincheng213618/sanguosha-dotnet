#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineAppliedDamageBenefitsRuntime
{
    internal string? DistinctTurnTargetUsage(SkillProgramActivation activation) => activation.Effects.SingleOrDefault(effect => effect.Op == SkillProgramEffectOp.ConsumeDistinctTurnTarget)?.StateId;
    internal bool CanActivateDistinctTurnTarget(int ownerSeat, string skillId, string usageId, int targetSeat) => _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber > 0 && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat == ownerSeat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(targetSeat) && targetSeat != ownerSeat && _coreMatchParticipantsPlayersAccess._players[targetSeat].IsAlive && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramDistinctTurnTargetCommittedEvent>().Any(e => e.OwnerSeat == ownerSeat && e.SkillId == skillId && e.UsageId == usageId && e.TargetSeat == targetSeat && e.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.TurnSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat);
    internal void ConsumeDistinctProgramTurnTarget(ProgramSkillFrame frame, string usageId)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var activation = ProgramInstructionResolver.Default.Resolve(active, AppliedDamageProgramDefinition(active.SkillId)).Activation;
        if (active.TriggerId is not null || active.SelectedTargetSeats is not [var target] || activation is null || DistinctTurnTargetUsage(activation) != usageId || active.InstructionIndex != 1 || activation.Effects[0].Op != SkillProgramEffectOp.ConsumeDistinctTurnTarget || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat], active.SkillId, active.SkillInstanceId) || !CanActivateDistinctTurnTarget(active.OwnerSeat, active.SkillId, usageId, target))
            throw new InvalidOperationException("An actual-turn target commitment lost its exact active source, first instruction or unvisited target.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramDistinctTurnTargetCommittedEvent(active.Id, active.OwnerSeat, active.SkillId, active.ActivationId, active.SkillInstanceId, active.GameplayHash, usageId, target, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId));
    }

    internal SkillProgramStepOutcome ReceiveProgramOwnerDamage(ProgramSkillFrame frame, int amount)
    {
        if (frame.TriggerId is not null || frame.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play)
            throw new InvalidOperationException("Receiving damage as an active cost requires the actual owner's Play phase.");
        // The official text says receive damage; it does not designate a source.
        // This goes through the real damage attempt, prevention, HP and dying flow.
        return _coreSkillProgramsOperations.BeginProgramSkillDamage(frame, frame.OwnerSeat, amount, sourceLess: true);
    }

    internal SkillProgramStepOutcome BeginAppliedDamageBenefitDraw(ProgramSkillFrame frame, int amount)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == active.Id);
        if (active.AppliedDamageBenefit is not null || index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not DamageTriggerWindowFrame damage || active.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied } context || context.ParentFrameId != damage.Id || context.DamageFrameId != damage.ParentFrameId || context.TargetSeat != damage.TargetSeat || damage.TriggerWindow != context.Window || damage.CandidateIndex < 0 || damage.CandidateIndex >= damage.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(active, damage.Candidates[damage.CandidateIndex].ToProgramCandidate()))
            throw new InvalidOperationException("An applied-damage Draw requires its exact actual producer and frozen current candidate.");
        var attack = _coreNativeDamageHandlesOperations.GetDamageTriggerAttack(damage);
        if (!attack.DamageWasApplied || attack.DamageAmount <= 0 || context.Amount != attack.DamageAmount)
            throw new InvalidOperationException("An applied-damage benefit requires positive actual applied damage.");
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat], active.SkillId, active.SkillInstanceId))
            return SkillProgramStepOutcome.Continue;
        var first = _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault()?.Sequence ?? 0;
        _coreRuntimeOperations.ReplaceRuntimeTop(active with { AppliedDamageBenefit = new(active.InstructionIndex, damage.Id, damage.ParentFrameId, active.OwnerSeat, damage.TargetSeat, context.SourceSeat, amount, first, first, 0), PendingMovementContinuation = new(active.OwnerSeat, 0, null) });
        var drawn = _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat], amount, true, new("program.applied-damage-benefit.draw"));
        active = _coreProgramLifecycleOperations.GetActiveProgramFrame(active.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(active with { AppliedDamageBenefit = active.AppliedDamageBenefit!with { LastMovementSequence = _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault()?.Sequence ?? first, DrawCount = drawn.Count } });
        if (!_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(active.Id, PostEventContinuation.AwaitedProgramMovement) && !_coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(active.Id, PostEventContinuation.AwaitedProgramMovement) && !_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.ReturnRuntimeProgramMovement(active.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal SkillProgram AppliedDamageProgramDefinition(string skillId) => _contentRegistryOperations._contentRegistry.GetSkill(skillId).Program!;
    SkillProgramStepOutcome global::CardGame.Core.IAppliedDamageBenefitProgramHost.ReceiveOwnerDamage(ProgramSkillFrame frame, int amount) => ReceiveProgramOwnerDamage(frame, amount);
    void global::CardGame.Core.IAppliedDamageBenefitProgramHost.ConsumeDistinctTurnTarget(ProgramSkillFrame frame, string usageId) => ConsumeDistinctProgramTurnTarget(frame, usageId);
    SkillProgramStepOutcome global::CardGame.Core.IAppliedDamageBenefitProgramHost.DrawOwnerAtAppliedDamage(ProgramSkillFrame frame, int amount) => BeginAppliedDamageBenefitDraw(frame, amount);
}
