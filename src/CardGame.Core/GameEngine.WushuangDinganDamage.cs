namespace CardGame.Core;

public sealed partial class GameEngine
{
    // The committed Slash still owns its unstarted attack while Dingan settles.
    // This permission belongs to that one observer and its current native use.
    [ProgramNestedDamageProof(ProgramNestedDamageProofPhase.AfterFactionDebts, 61200)]
    private bool AllowsWushuangDinganCommittedDamage(ProgramSkillFrame f, int target, int amount,
        ProgramParticipantReference? source, DamageNature? nature, bool sourceLess)
    {
        if (sourceLess || source is not null || nature is not null || amount != 1 || f.AttackAttempt is not null ||
            f.SkillId != BDingan || !BSourceCurrent(f) || _resolutionStack.LastOrDefault()?.Id != f.Id ||
            f.WushuangB is not { Operation: SkillProgramEffectOp.ResolveWushuangDingan, Stage: "complete" } r ||
            r.InstructionIndex != f.InstructionIndex || r.TargetSeat != target || target == f.OwnerSeat ||
            !IsValidPlayerSeat(target) || !_players[target].IsAlive ||
            f.WindowContext is not { Window: SkillProgramTriggerWindow.CardUseCommitted, CardUse: { } context } windowContext ||
            _resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == windowContext.ParentFrameId) is not { } window ||
            f.ParentFrameId != window.Id || GetCardActionWindow(window) != SkillProgramTriggerWindow.CardUseCommitted ||
            window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count ||
            LifecycleCardUse(context.ParentCardUseFrameId) is not { ProgramUseCommitted: true, Action: { } action } use ||
            window.ParentFrameId != use.Id || CurrentDamageAttempt?.ResolutionId != use.Id ||
            action.ActionId != context.CardActionId || window.Action.ActionId != action.ActionId ||
            action.EffectiveKind != context.EffectiveKind || action.ActorSeat != context.ActorSeat ||
            !BSecondTurnUse(context.CardActionId, context.EffectiveKind) ||
            ProgramEventHistory<WushuangDinganDrawnEvent>().Count(e => e.ProgramFrameId == f.Id &&
                e.OwnerSeat == f.OwnerSeat && e.TurnNumber == _turnNumber) != 1 ||
            GetHand(_players[target]).Count != _players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat)
                .Select(p => GetHand(p).Count).DefaultIfEmpty().Max()) return false;
        var candidate = window.Candidates[window.CandidateIndex];
        return candidate.OwnerSeat == f.OwnerSeat && candidate.SkillId == f.SkillId &&
            candidate.TriggerId == f.TriggerId && candidate.SkillInstanceId == f.SkillInstanceId &&
            candidate.GameplayHash == f.GameplayHash &&
            ProgramEventHistory<ProgramBindingStartedEvent>().Count(e => e.FrameId == f.Id &&
                e.SkillId == f.SkillId && e.OwnerSeat == f.OwnerSeat && e.SkillInstanceId == f.SkillInstanceId &&
                e.BindingId == f.TriggerId && e.Window == SkillProgramTriggerWindow.CardUseCommitted) == 1;
    }
}
