#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreLeastHandMarkerDrawProgramsRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.ILeastHandMarkerDrawProgramHost.GrantLeastHandMarkerOrDraw(ProgramSkillFrame frame, int targetSeat, PlayerMarkerKind marker) => BeginLeastHandMarkerOrDraw(frame, targetSeat, marker);
    internal SkillProgramStepOutcome BeginLeastHandMarkerOrDraw(ProgramSkillFrame frame, int targetSeat, PlayerMarkerKind marker)
    {
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (frame.LeastHandMarkerDraw is not null || frame.SelectedTargetSeats is not [var selected] || selected != targetSeat || frame.WindowContext is not { Window: SkillProgramTriggerWindow.DrawPhaseSkipped } context || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not ProgramLifecycleTriggerWindowFrame parent || parent.Id != context.ParentFrameId || parent.Window != context.Window || parent.ResumeDrawPhaseObligationFrameId is null)
            throw new InvalidOperationException("A least-hand marker benefit lost its actual skipped-draw parent.");
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[targetSeat];
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !owner.IsAlive || !target.IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, frame.SkillId, frame.SkillInstanceId))
            return SkillProgramStepOutcome.Continue;
        var minimum = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Min(p => _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count);
        var markerBranch = _corePhysicalOwnedCardQueriesOperations.GetHand(target).Count == minimum && target.Markers.GetValueOrDefault(marker) == 0;
        if (markerBranch)
        {
            _coreAttributedMarkerEventsOperations.MutateParticipantMarker(frame, targetSeat, marker, 1);
            _coreLeastHandMarkerDrawProgramsMarkerExtraDrawPoliciesAccess._markerExtraDrawPolicies[(targetSeat, marker)] = new(targetSeat, marker, frame.OwnerSeat, frame.SkillId, frame.SkillInstanceId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame));
        }
        else
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { LeastHandMarkerDraw = new(frame.InstructionIndex, targetSeat) });
            _corePhysicalDrawFacadesOperations.DrawCards(target, 1, true, new("program.least-hand-marker.draw"));
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramLeastHandMarkerOrDrawEvent(frame.Id, frame.OwnerSeat, targetSeat, frame.SkillId, frame.SkillInstanceId, marker, markerBranch));
        if (markerBranch)
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 710)]
    internal bool ResumeLeastHandMarkerDraw(long frameId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame frame || frame.Id != frameId || frame.LeastHandMarkerDraw is not { } receipt)
            return false;
        var plan = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!);
        if (frame.InstructionIndex != receipt.InstructionIndex || receipt.InstructionIndex < 1 || receipt.InstructionIndex > plan.Instructions.Count || plan.Instructions[receipt.InstructionIndex - 1].Op != SkillProgramEffectOp.GrantLeastHandMarkerOrDraw || frame.SelectedTargetSeats is not [var seat] || seat != receipt.TargetSeat)
            throw new InvalidOperationException("A least-hand draw lost its exact completed instruction and selected target.");
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(frame.Id, PostEventContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frame.Id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(frame.Id))
            return true;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { LeastHandMarkerDraw = null });
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), "截辎得牌孩子结清后，来源失效，剩余程序取消。");
            return true;
        }

        return false;
    }

    internal bool HasIssuedExtraDrawMarker(int seat) => _coreLeastHandMarkerDrawProgramsMarkerExtraDrawPoliciesAccess._markerExtraDrawPolicies.Values.Any(policy => policy.TargetSeat == seat && _coreMatchParticipantsPlayersAccess._players[seat].Markers.GetValueOrDefault(policy.Marker) > 0);
    internal int IndependentIssuedDrawMarkerCount(CharacterState target, PlayerMarkerKind marker, int source) => target.IsAlive && target.Markers.GetValueOrDefault(marker) > 0 && _coreLeastHandMarkerDrawProgramsMarkerExtraDrawPoliciesAccess._markerExtraDrawPolicies.TryGetValue((target.Seat, marker), out var policy) && policy.SourceSeat == source ? 1 : 0;
    internal ProgramMarkerExtraDrawPolicy? ConsumeIssuedExtraDrawMarker(DrawPhaseObligationFrame frame)
    {
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        var policy = _coreLeastHandMarkerDrawProgramsMarkerExtraDrawPoliciesAccess._markerExtraDrawPolicies.Values.Where(p => p.TargetSeat == owner.Seat && owner.Markers.GetValueOrDefault(p.Marker) > 0).OrderBy(p => p.Marker).ThenBy(p => p.SourceSeat).FirstOrDefault();
        if (policy is null)
            return null;
        var key = (policy.Marker, policy.SourceSeat);
        if (owner.MarkerSourceCounts.GetValueOrDefault(key) < 1)
            throw new InvalidOperationException("An issued extra-draw marker lost its exact source attribution.");
        _coreLeastHandMarkerDrawProgramsMarkerExtraDrawPoliciesAccess._markerExtraDrawPolicies.Remove((owner.Seat, policy.Marker));
        var sourceLeft = owner.MarkerSourceCounts[key] - 1;
        if (sourceLeft == 0)
            owner.MarkerSourceCounts.Remove(key);
        else
            owner.MarkerSourceCounts[key] = sourceLeft;
        var total = owner.Markers[policy.Marker] - 1;
        if (total == 0)
            owner.Markers.Remove(policy.Marker);
        else
            owner.Markers[policy.Marker] = total;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlayerMarkerChangedEvent(frame.Id, owner.Seat, policy.Marker, -1, total, policy.SourceSeat, "program.marker-extra-draw.consume"));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramMarkerExtraDrawConsumedEvent(frame.Id, owner.Seat, policy.Marker, policy.SourceSeat, policy.SkillId, policy.SkillInstanceId));
        return policy;
    }

    internal bool IsBeforeActualDamageDistancePolicyQualified(CharacterState owner) => !_corePlayCardHistoryOperations.EventsSinceLastBoundary(e => e is TurnStartedEvent).OfType<DamageAppliedEvent>().Any(e => !e.SourceLess && e.SourceSeat == owner.Seat && e.Amount > 0);
}
