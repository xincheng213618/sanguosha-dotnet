#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhaoZhongOlRuntime
{
    internal bool HasYintianCapability => _contentRegistryOperations._contentRegistry?.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveYintianFirstRecovery) == true;
    internal bool YintianActualTurnOpen => _coreMatchSetupSetupCompleteAccess._setupComplete && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber > 0 && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.TurnNumber && !_coreProgramEventHistoryOperations.ProgramEventHistory<ActualTurnCursorReturnedEvent>().Any(e => e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && e.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat);

    internal static bool IsYintianRecoveryTrigger(SkillProgramTrigger trigger) => global::CardGame.Core.RuntimeSkills.RuntimeRules_zhao_zhong_ol_Yintian.IsYintianRecoveryTrigger(trigger);
    internal void CaptureYintianRecoveryQualification(HpChangeContext change)
    {
        if (!HasYintianCapability || !YintianActualTurnOpen || change.Kind != HpChangeKind.Recovery || change.HpAfter <= change.HpBefore || change.Amount != change.HpAfter - change.HpBefore || !_coreGameEngineHpChangeProgramsPendingHpChangesAccess._pendingHpChanges.Contains(change))
            return;
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<YintianFirstRecoveryEvent>().Any(e => e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && e.TurnOwner == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat))
            return;
        var first = new YintianFirstRecoveryEvent(change.Id, change.ParentFrameId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, change.SourceSeat, change.TargetSeat, change.Amount, change.HpBefore, change.HpAfter);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(first);
        foreach (var owner in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).OrderBy(p => _coreTableOrderOperations.RelativeTableOrder(first.TurnOwner, p.Seat)))
        {
            var sources = _coreNationalSkillsOperations.GetSkillBindingShard(owner).ProgramInstances.SelectMany(instance => instance.Program.Triggers.Where(IsYintianRecoveryTrigger).Select(trigger => new ProgramTriggerCandidate(owner.Seat, instance.SkillId, trigger.Id, instance.SkillInstanceId, instance.Program.GameplayHash, trigger.Priority))).Where(c => _coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, c.SkillId, c.SkillInstanceId)).OrderByDescending(c => c.Priority).ThenBy(c => c.SkillInstanceId, StringComparer.Ordinal).ToArray();
            if (sources is not [var candidate, ..])
                continue;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YintianRecoveryQualifiedEvent(change.Id, new(candidate.SkillId, candidate.BindingId, candidate.OwnerSeat, candidate.SkillInstanceId), candidate.GameplayHash, first.ActualTurn, first.TurnOwner));
        }
    }

    internal YintianFirstRecoveryEvent? YintianFirstRecovery(HpChangeContext change) => _coreProgramEventHistoryOperations.ProgramEventHistory<YintianFirstRecoveryEvent>().SingleOrDefault(e => e.ChangeId == change.Id && e.NativeParentId == change.ParentFrameId && e.SourceSeat == change.SourceSeat && e.TargetSeat == change.TargetSeat && e.Amount == change.Amount && e.HpBefore == change.HpBefore && e.HpAfter == change.HpAfter && change.Kind == HpChangeKind.Recovery && e.Amount > 0 && e.HpAfter - e.HpBefore == e.Amount);
    internal YintianRecoveryQualifiedEvent? YintianQualification(ProgramTriggerCandidate candidate, long changeId) => _coreProgramEventHistoryOperations.ProgramEventHistory<YintianRecoveryQualifiedEvent>().SingleOrDefault(e => e.ChangeId == changeId && e.Source == new CardConversionSource(candidate.SkillId, candidate.BindingId, candidate.OwnerSeat, candidate.SkillInstanceId) && e.GameplayHash == candidate.GameplayHash);
    internal IReadOnlyList<ProgramTriggerCandidate> CollectYintianRecoveryCandidates(HpChangeContext change)
    {
        if (!HasYintianCapability || YintianFirstRecovery(change)is not { } first || first.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || first.TurnOwner != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat)
            return[];
        return Array.AsReadOnly(_coreProgramEventHistoryOperations.ProgramEventHistory<YintianRecoveryQualifiedEvent>().Where(e => e.ChangeId == change.Id && e.ActualTurn == first.ActualTurn && e.TurnOwner == first.TurnOwner && _coreTableSeatQueriesOperations.IsValidPlayerSeat(e.Source.OwnerSeat) && _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(e.Source.SkillId)?.Program is { } program && program.GameplayHash == e.GameplayHash).Select(e => new ProgramTriggerCandidate(e.Source.OwnerSeat, e.Source.SkillId, e.Source.BindingId, e.Source.SkillInstanceId, e.GameplayHash, _contentRegistryOperations._contentRegistry.GetSkill(e.Source.SkillId).Program!.Triggers.Single(t => t.Id == e.Source.BindingId).Priority)).Where(c => IsYintianRecoveryTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(c))).OrderBy(c => _coreTableOrderOperations.RelativeTableOrder(first.TurnOwner, c.OwnerSeat)).ToArray());
    }

    internal ProgramSkillWindowContext CreateYintianRecoveryContext(HpChangeContext change, ProgramTriggerCandidate candidate)
    {
        if (YintianFirstRecovery(change)is not { } first || YintianQualification(candidate, change.Id)is not { } q || q.ActualTurn != first.ActualTurn || q.TurnOwner != first.TurnOwner)
            throw new InvalidOperationException("Yintian requires its exact producer-frozen first positive recovery.");
        return new(SkillProgramTriggerWindow.AfterHpRecovered, change.Id, candidate.OwnerSeat, SourceSeat: change.SourceSeat, TargetSeat: change.TargetSeat, Amount: change.Amount, OccurrenceIndex: candidate.OccurrenceIndex, HpChange: change, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat])with { HpChangeAmount = change.Amount, HpBeforeChange = change.HpBefore, HpAfterChange = change.HpAfter, HealthChangeKind = change.Kind });
    }

    internal bool CanRunYintianRecovery(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (!IsYintianRecoveryTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)) || candidate.OccurrenceIndex != 0 || context is not { Window: SkillProgramTriggerWindow.AfterHpRecovered, HpChange: { Kind: HpChangeKind.Recovery } change } || context.ParentFrameId != change.Id || context.OwnerSeat != candidate.OwnerSeat || context.SourceSeat != change.SourceSeat || context.TargetSeat != change.TargetSeat || context.Amount != change.Amount || context.OccurrenceIndex != 0 || YintianFirstRecovery(change)is not { } first || YintianQualification(candidate, change.Id)is not { } q || first.ActualTurn != q.ActualTurn || first.TurnOwner != q.TurnOwner || first.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || first.TurnOwner != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == change.Id)is not HpChangedTriggerWindowFrame window || window.Change != change || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || window.Candidates[window.CandidateIndex] != candidate || window.Contexts[window.CandidateIndex].HpChange != change)
            return false;
        return !_coreProgramEventHistoryOperations.ProgramEventHistory<YintianChargeIssuedEvent>().Any(e => e.RecoveryChangeId == change.Id && e.Source.OwnerSeat == candidate.OwnerSeat) && !_coreProgramEventHistoryOperations.ProgramEventHistory<YintianChargeRetainedEvent>().Any(e => e.RecoveryChangeId == change.Id && e.Source.OwnerSeat == candidate.OwnerSeat);
    }

    [ProgramIssuedTrigger(150)]
    internal bool IsIssuedYintianRecoveryCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => IsYintianRecoveryTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)) && CanRunYintianRecovery(candidate, context);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 28113)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 28113)]
    internal bool ContinueIssuedYintianRecovery(ProgramSkillFrame frame)
    {
        if (frame.Yintian is not null)
        {
            return true;
        }

        return frame.TriggerId is { } id && frame.WindowContext is { } context && IsIssuedYintianRecoveryCandidate(new(frame.OwnerSeat, frame.SkillId, id, frame.SkillInstanceId, frame.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(frame).Priority, context.OccurrenceIndex), context);
    }

    SkillProgramStepOutcome global::CardGame.Core.IYintianHost.Execute(ProgramSkillFrame frame) => ResolveYintianFirstRecovery(frame);
    internal SkillProgramStepOutcome ResolveYintianFirstRecovery(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.Yintian is not null || frame.TriggerId is null || frame.InstructionIndex != 1 || frame.WindowContext is not { } context)
            throw new InvalidOperationException("Yintian requires its new exact first-recovery instruction.");
        var candidate = new ProgramTriggerCandidate(frame.OwnerSeat, frame.SkillId, frame.TriggerId, frame.SkillInstanceId, frame.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(frame).Priority, context.OccurrenceIndex);
        if (!CanRunYintianRecovery(candidate, context))
            throw new InvalidOperationException("Yintian lost its original global first-recovery qualification.");
        var first = YintianFirstRecovery(context.HpChange!)!;
        var source = new CardConversionSource(frame.SkillId, frame.TriggerId, frame.OwnerSeat, frame.SkillInstanceId);
        var existing = AvailableYintianCharge(frame.OwnerSeat);
        var receipt = new YintianRecoveryReceipt(source, frame.GameplayHash, first.ChangeId, first.ActualTurn, first.TurnOwner, existing?.GrantId ?? frame.Id, existing is null);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { Yintian = receipt });
        if (existing is null)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YintianChargeIssuedEvent(frame.Id, source, frame.GameplayHash, first.ChangeId, first.ActualTurn, first.TurnOwner));
        else
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YintianChargeRetainedEvent(frame.Id, source, frame.GameplayHash, first.ChangeId, first.ActualTurn, first.TurnOwner, existing.GrantId));
        return SkillProgramStepOutcome.Continue;
    }

    internal bool ValidYintianCharge(YintianChargeIssuedEvent grant) => grant.GrantId > 0 && _coreTableSeatQueriesOperations.IsValidPlayerSeat(grant.Source.OwnerSeat) && _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(grant.Source.SkillId)?.Program is { } program && program.GameplayHash == grant.GameplayHash && program.Triggers.Any(t => t.Id == grant.Source.BindingId && IsYintianRecoveryTrigger(t)) && _coreProgramEventHistoryOperations.ProgramEventHistory<YintianRecoveryQualifiedEvent>().Count(q => q.ChangeId == grant.RecoveryChangeId && q.Source == grant.Source && q.GameplayHash == grant.GameplayHash && q.ActualTurn == grant.ActualTurn && q.TurnOwner == grant.TurnOwner) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<YintianFirstRecoveryEvent>().Count(f => f.ChangeId == grant.RecoveryChangeId && f.ActualTurn == grant.ActualTurn && f.TurnOwner == grant.TurnOwner && f.Amount > 0 && f.HpAfter - f.HpBefore == f.Amount) == 1;
    internal YintianChargeIssuedEvent? AvailableYintianCharge(int owner)
    {
        var grants = _coreProgramEventHistoryOperations.ProgramEventHistory<YintianChargeIssuedEvent>().Where(g => g.Source.OwnerSeat == owner && !_coreProgramEventHistoryOperations.ProgramEventHistory<YintianChargeConsumedEvent>().Any(c => c.GrantId == g.GrantId)).ToArray();
        if (grants.Length > 1 || grants.Any(g => !ValidYintianCharge(g)))
            throw new InvalidOperationException("Yintian must retain at most one exact unconsumed charge per owner.");
        return grants.SingleOrDefault();
    }

    [ProgramTriggerProof(ProgramTriggerProofPhase.HpChangeOwner, 40, ProgramTriggerProofMatch.Decision)]
    internal bool? ProveYintianRecovery(SkillProgramTrigger trigger, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => IsYintianRecoveryTrigger(trigger) ? CanRunYintianRecovery(candidate, context) : null;
}
