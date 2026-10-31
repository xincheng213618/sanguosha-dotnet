#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineHellActivitiesRuntime
{
    internal bool IsHellNpc(CharacterState player) => _coreHellEncounterHostOperations.HellFeature?.IsNpcTeam(player.TeamId) ?? false;
    internal bool HasHellRemenImmunity(CharacterState player, CardKind kind) => player.IsAlive && !_coreNativeEquipmentAndFactionQueriesOperations.HasArmor(player) && _coreProgramCardPoliciesOperations.HasCardPolicy(player, SkillProgramCardPolicyKind.HellUnarmoredRemen, kind);
    internal bool CanPlanHellDiscard(CardLocation destination) => destination == CardLocation.DiscardPile && _coreHellMengpoProgramsOperations.HasHellShiyouCapability && _coreMatchParticipantsPlayersAccess._players.Any(player => player.IsAlive && !player.IsHuman && _coreProgramCardPoliciesOperations.HasCardPolicy(player, SkillProgramCardPolicyKind.HellShiyouIntercept)) && _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Discard };
    internal bool IsExactHellPreparationTargetContext(int ownerSeat, ProgramSkillWindowContext? context) => context is { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow, TargetSeat: { } target } && target == _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat && context.SourceSeat == target && context.OwnerSeat == ownerSeat && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(window => window.Id == context.ParentFrameId)is { } parent && parent.OwnerSeat == target && parent.Window == context.Window && parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count && parent.Candidates[parent.CandidateIndex].OwnerSeat == ownerSeat && _coreProgramLifecycleOperations.GetProgramTrigger2(parent.Candidates[parent.CandidateIndex]).Effects.Any(effect => effect.Op == SkillProgramEffectOp.UseHellPreparationAlcohol);
    internal void RecordHellInterceptedMovement(HellShiyouTransferPlan plan, CardMovementRecord move, Action<CardMovementRecord>? beforeFact)
    {
        if (plan.Redirects.Any(r => r.CardId == move.CardId))
            _coreHellMengpoProgramsOperations.CompleteHellShiyouDiscardTransfers(plan with { Redirects = Array.AsReadOnly(plan.Redirects.Where(r => r.CardId == move.CardId).ToArray()) }, [move]);
        beforeFact?.Invoke(move);
    }

    [ProgramTriggerProof(ProgramTriggerProofPhase.GameStart, 10)]
    internal bool IsExactHellAppearanceWindow(ProgramLifecycleTriggerWindowFrame window) => window.Continuation == ProgramLifecycleContinuation.ResumeHellAppearance && window.Window == SkillProgramTriggerWindow.GameStarting && window.HellAppearanceId is { } id && _coreHellEncounterHostOperations.HellFeature?.IsPendingAppearance(id, Array.AsReadOnly(window.Candidates.Select(candidate => candidate.OwnerSeat).ToArray())) == true;
    internal ModeNpcArrivalSourceFacts? CaptureNativeArrivalProgramSource(ProgramSkillFrame frame)
    {
        if (frame.WindowContext is not { Window: SkillProgramTriggerWindow.GameStarting } context || frame.TriggerId is null || _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(candidate => candidate.Id == context.ParentFrameId)is not { } window || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count)
            return null;
        var initial = window.Continuation == ProgramLifecycleContinuation.CompleteGameStarting && _coreActualTurnProgressCurrentSeatPhaseAccess._phase == TurnPhase.NotStarted;
        var appearance = window.Continuation == ProgramLifecycleContinuation.ResumeHellAppearance && window.Window == SkillProgramTriggerWindow.GameStarting ? window.HellAppearanceId : null;
        if ((!initial && appearance is null) || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(frame, window.Candidates[window.CandidateIndex]))
            return null;
        return new(frame.SkillId, initial, appearance, window.Candidates.Select(candidate => candidate.OwnerSeat).ToArray());
    }

    internal bool IsExactHellArrivalProgram(ProgramSkillFrame frame) => _coreHellEncounterHostOperations.HellFeature is { } feature && CaptureNativeArrivalProgramSource(frame)is { } source && (source.IsInitialGameStarting || source.AppearanceId is { } id && feature.IsPendingAppearance(id, source.CandidateOwnerSeats));
}
