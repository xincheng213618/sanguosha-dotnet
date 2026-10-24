namespace CardGame.Core;

public sealed partial class GameEngine
{
    private bool IsHellNpc(CharacterState player) => HellFeature?.IsNpcTeam(player.TeamId) ?? false;
    private bool HasHellRemenImmunity(CharacterState player, CardKind kind) => player.IsAlive && !HasArmor(player) &&
        HasCardPolicy(player, SkillProgramCardPolicyKind.HellUnarmoredRemen, kind);
    private bool CanPlanHellDiscard(CardLocation destination) => destination == CardLocation.DiscardPile &&
        HasHellShiyouCapability && _players.Any(player => player.IsAlive && !player.IsHuman &&
            HasCardPolicy(player, SkillProgramCardPolicyKind.HellShiyouIntercept)) &&
        CurrentActualDiscardRecoveryPhase() is { Kind: ActualDiscardRecoveryPhaseKind.Discard };
    private bool IsExactHellPreparationTargetContext(int ownerSeat, ProgramSkillWindowContext? context) =>
        context is { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow, TargetSeat: { } target } &&
        target == _currentSeat && context.SourceSeat == target && context.OwnerSeat == ownerSeat && _players[target].IsAlive &&
        _resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(window => window.Id == context.ParentFrameId) is { } parent &&
        parent.OwnerSeat == target && parent.Window == context.Window && parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count &&
        parent.Candidates[parent.CandidateIndex].OwnerSeat == ownerSeat &&
        GetProgramTrigger(parent.Candidates[parent.CandidateIndex]).Effects.Any(effect => effect.Op == SkillProgramEffectOp.UseHellPreparationAlcohol);
    private void RecordHellInterceptedMovement(HellShiyouTransferPlan plan, CardMovementRecord move, Action<CardMovementRecord>? beforeFact)
    {
        if (plan.Redirects.Any(r => r.CardId == move.CardId))
            CompleteHellShiyouDiscardTransfers(plan with { Redirects = Array.AsReadOnly(plan.Redirects.Where(r => r.CardId == move.CardId).ToArray()) }, [move]);
        beforeFact?.Invoke(move);
    }

    private bool IsExactHellAppearanceWindow(ProgramLifecycleTriggerWindowFrame window) =>
        window.Continuation == ProgramLifecycleContinuation.ResumeHellAppearance &&
        window.Window == SkillProgramTriggerWindow.GameStarting && window.HellAppearanceId is { } id &&
        HellFeature?.IsPendingAppearance(id, Array.AsReadOnly(window.Candidates.Select(candidate => candidate.OwnerSeat).ToArray())) == true;

    // Virtual-use validation and the mode's damage query share this native
    // parent/candidate proof. The feature still owns campaign qualification.
    private ModeNpcArrivalSourceFacts? CaptureNativeArrivalProgramSource(ProgramSkillFrame frame)
    {
        if (frame.WindowContext is not { Window: SkillProgramTriggerWindow.GameStarting } context || frame.TriggerId is null ||
            _resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(candidate => candidate.Id == context.ParentFrameId) is not { } window ||
            window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count) return null;
        var initial = window.Continuation == ProgramLifecycleContinuation.CompleteGameStarting && _phase == TurnPhase.NotStarted;
        var appearance = window.Continuation == ProgramLifecycleContinuation.ResumeHellAppearance &&
            window.Window == SkillProgramTriggerWindow.GameStarting ? window.HellAppearanceId : null;
        if ((!initial && appearance is null) || !MountObserverCandidateMatches(frame, window.Candidates[window.CandidateIndex])) return null;
        return new(frame.SkillId, initial, appearance, window.Candidates.Select(candidate => candidate.OwnerSeat).ToArray());
    }

    private bool IsExactHellArrivalProgram(ProgramSkillFrame frame) => HellFeature is { } feature &&
        CaptureNativeArrivalProgramSource(frame) is { } source && (source.IsInitialGameStarting ||
            source.AppearanceId is { } id && feature.IsPendingAppearance(id, source.CandidateOwnerSeats));

}

public sealed partial record ProgramLifecycleTriggerWindowFrame
{
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public long? HellAppearanceId { get; init; }
}
