#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLvQianOlRuntime
{
    internal void ConsumeUnreturnedWeiluDebt(CharacterState owner, WeiluPaidDebt debt)
    {
        owner.WeiluDebts = (owner.WeiluDebts ?? []).Where(g => g != debt).ToArray();
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WeiluDebtConsumedEvent(0, debt.ProducerFrameId, debt.Phase, debt.TargetSeat, 0, _coreMatchParticipantsPlayersAccess._players[debt.TargetSeat].IsAlive));
    }

    internal WeiluPhaseKey? CaptureWeiluStartingPhase(CharacterState owner) => owner.WeiluFuture?.FirstOrDefault(g => g.FirstPhase is { } p && p.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && p.PhaseInstance == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId && LvQianSourceCurrent(g.Source, g.GameplayHash))?.FirstPhase;
    internal WeiluPhaseKey? CaptureWeiluEndingPhase(CharacterState owner) => owner.WeiluDebts?.Any(g => g.Phase.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && g.Phase.PhaseInstance == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId) == true ? new(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, owner.Seat, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId) : null;
    internal bool HasCurrentIssuedWeiluDebt(CharacterState owner) => CaptureWeiluEndingPhase(owner)is not null;
    internal ProgramSkillWindowContext CreateWeiluStartingContext(PlayPhaseStartingBoundaryFrame frame, ProgramSkillWindowContext context) => context with
    {
        WeiluPhase = frame.WeiluStartingPhase
    };
    internal IEnumerable<ProgramTriggerCandidate> IssuedWeiluEndingCandidates(CharacterState owner) => (owner.WeiluDebts ?? []).Where(g => g.Phase.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && g.Phase.PhaseInstance == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId).GroupBy(g => (g.Source.SkillId, g.Source.SkillInstanceId, g.GameplayHash)).Select(g => new ProgramTriggerCandidate(owner.Seat, g.Key.SkillId, "restore-paid-phase", g.Key.SkillInstanceId, g.Key.GameplayHash, 0));
    internal ProgramSkillWindowContext CreateWeiluEndingContext(ProgramLifecycleTriggerWindowFrame frame, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => context with
    {
        WeiluPhase = frame.WeiluEndingPhase
    };
    [ProgramTriggerProof(ProgramTriggerProofPhase.OwnerException, 10)]
    [ProgramTriggerProof(ProgramTriggerProofPhase.InstanceException, 150)]
    internal bool HasIssuedWeiluDebtCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => w.Window == SkillProgramTriggerWindow.PlayEnding && w.WeiluPhase is { } phase && phase.OwnerSeat == c.OwnerSeat && (_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].WeiluDebts ?? []).Any(g => g.Phase == phase && g.Source.SkillId == c.SkillId && g.Source.SkillInstanceId == c.SkillInstanceId && g.GameplayHash == c.GameplayHash) && c.BindingId == "restore-paid-phase" && _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(f => f.Id == w.ParentFrameId)is { } actual && actual.OwnerSeat == c.OwnerSeat && actual.WeiluEndingPhase == phase && actual.CandidateIndex >= 0 && actual.CandidateIndex < actual.Candidates.Count && actual.Candidates[actual.CandidateIndex] == c;
    internal bool TryBeginForcedWeiluPhaseEnd()
    {
        if (!HasLvQianCapability || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.Count != 0)
            return false;
        var owner = _coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat];
        var phase = CaptureWeiluEndingPhase(owner);
        if (phase is null)
            return false;
        var candidates = IssuedWeiluEndingCandidates(owner).ToArray();
        if (candidates.Length == 0)
            return false;
        var facts = _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(owner);
        var frame = new ProgramLifecycleTriggerWindowFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, owner.Seat, SkillProgramTriggerWindow.PlayEnding, Array.AsReadOnly(candidates), ProgramLifecycleContinuation.CompleteWeiluForcedPlayEnd, facts)
        {
            WeiluEndingPhase = phase
        };
        _coreRuntimeOperations.PushRuntimeFrame(frame);
        _coreRuntimeOperations.AdvanceRuntimeTop<ProgramLifecycleTriggerWindowFrame>();
        return true;
    }

    internal bool ExactWeiluDamageParent(ProgramSkillFrame f, long damageId, int attacker, int amount)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (index < 2 || f.WindowContext is not { } context || _resolutionStackOperations._resolutionStack[index - 1] is not DamageTriggerWindowFrame window || window.Id != context.ParentFrameId || window.ParentFrameId != damageId || window.TriggerWindow != SkillProgramTriggerWindow.AfterDamageApplied || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, window.Candidates[window.CandidateIndex].ToProgramCandidate()) || _resolutionStackOperations._resolutionStack[index - 2] is not DamageFrame damage || damage.Id != damageId || damage.SourceSeat != attacker || damage.TargetSeat != f.OwnerSeat || damage.Amount != amount)
            return false;
        var attack = _coreNativeDamageHandlesOperations.GetDamageTriggerAttack(window);
        return attack.ResolutionId == damage.ParentFrameId && attack.DamageWasApplied && !attack.IsSourceLess && !attack.IsDelayedJudgmentDamage && attack.SourceSeat == attacker && attack.TargetSeat == f.OwnerSeat && attack.DamageAmount == amount && global::CardGame.Core.EngineOwners.CoreGameEngineTurnDefaultStatsValidationRuntime.TurnDefaultStatDamageRequestMatches(_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray(), damageId, attacker, f.OwnerSeat, amount, damage.Nature, false);
    }
}
