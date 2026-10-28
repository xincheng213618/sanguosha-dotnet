namespace CardGame.Core;

public sealed partial class GameEngine
{
    private SkillProgramStepOutcome StartSourceHuojie(ProgramSkillFrame supplied)
    {
        var f = GetActiveProgramFrame(supplied.Id); var count = SourceDingxiCount(f.OwnerSeat);
        if (count <= _players.Count) return SkillProgramStepOutcome.Continue;
        ReplaceRuntimeTop(f = f with { SourceHuojie = new(count, 0, SourceHuojieStage.JudgmentChildren) });
        ContinueSourceHuojie(f); return SkillProgramStepOutcome.AwaitChild;
    }
    private bool CanBeginSourceHuojieJudgment(ProgramSkillFrame f, int target, string reason, string bind, SkillProgramCardSetVisibility visibility) =>
        f.SkillId == SourceBeigongIds.Huojie && f.SourceHuojie is { Stage: SourceHuojieStage.Judging, JudgmentId: > 0 } &&
        f.WindowContext?.Window == SkillProgramTriggerWindow.PlayPhaseStarting && target == f.OwnerSeat &&
        reason == JudgmentReasons.Lightning && bind == "source-huojie-lightning" && visibility == SkillProgramCardSetVisibility.Public;
    private bool IsSourceHuojieJudgment(JudgmentFrame j, ProgramSkillFrame f) =>
        CanBeginSourceHuojieJudgment(f, j.TargetSeat, j.Reason, j.ProgramResultBind ?? "", j.ProgramResultVisibility ?? SkillProgramCardSetVisibility.Private) &&
        f.SourceHuojie!.JudgmentId == j.Id && j.ParentFrameId == f.Id && j.SourceSeat == f.OwnerSeat && j.Continuation == JudgmentContinuationKind.ProgramSkill;
    private bool IsSourceHuojieLightningJudgment(JudgmentFrame j) => _resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == j.ParentFrameId) is { } f && IsSourceHuojieJudgment(j, f);
    private bool ReturnSourceHuojieJudgment(JudgmentFrame j)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { SourceHuojie: { Stage: SourceHuojieStage.Judging } r } f || j.ParentFrameId != f.Id) return false;
        if (!IsSourceHuojieJudgment(j, f)) throw new InvalidOperationException("Source3603 lost its exact current Lightning judgment return.");
        var result = ProgramEventHistory<JudgmentResolvedEvent>().Single(e => e.ResolutionId == j.Id && e.ParentResolutionId == f.Id && e.Reason == JudgmentReasons.Lightning);
        var handoff = result.CardId is { } resultId && _cardZones.GetLocation(resultId) == CardLocation.Processing ? _cardMovements.Last(m => m.CardId == resultId) : null;
        if (handoff is not null && (handoff.From != CardLocation.Judgment(f.OwnerSeat) || handoff.To != CardLocation.Processing || handoff.Reason.Value != "skill-program.judgment.result"))
            throw new InvalidOperationException("Source3603 cannot retire an unrelated judgment entity.");
        ReplaceRuntimeTop(f = f with { SourceHuojie = r with { Stage = SourceHuojieStage.JudgmentChildren,
            Hit = result.Succeeded, Before = _movementSequence, After = _movementSequence, JudgmentCardId = handoff?.CardId ?? 0,
            JudgmentHandoffSequence = handoff?.Sequence ?? 0 }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        if (result.CardId is { } id && _cardZones.GetLocation(id) == CardLocation.Processing)
            MoveCard(GetAdvancedCard(id), CardLocation.Processing, CardLocation.DiscardPile, CardMoveReasons.JudgmentFinish);
        f = GetActiveProgramFrame(f.Id); ReplaceRuntimeTop(f with { SourceHuojie = f.SourceHuojie! with { After = _movementSequence } });
        ContinueSourceBeigong(f.Id); return true;
    }
    private void ContinueSourceHuojie(ProgramSkillFrame f)
    {
        var r = f.SourceHuojie!;
        if (r.Stage == SourceHuojieStage.Judging) return;
        if (!_players[f.OwnerSeat].IsAlive) { FinishSourceBeigong(f); return; }
        if (r.Stage == SourceHuojieStage.GainChildren) { FinishSourceBeigong(f); return; }
        if (r.Stage == SourceHuojieStage.DamageChildren)
        {
            var received = ProgramEventHistory<SourceHuojieReceivedDamageEvent>().Any(e => e.ProgramFrameId == f.Id && e.JudgmentId == r.JudgmentId);
            if (received)
            {
                ReplaceRuntimeTop(f = f with { SourceHuojie = r with { Stage = SourceHuojieStage.GainChildren,
                    Before = _movementSequence, After = _movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
                foreach (var pile in SourceDingxiPiles(f.OwnerSeat))
                    MoveProgramCardsFromMultipleSources(PublicPileCards(pile).Select(c => c.Id).ToArray(), CardLocation.Hand(f.OwnerSeat), new("skill-program.source-huojie.obtain"));
                f = GetActiveProgramFrame(f.Id); ReplaceRuntimeTop(f with { SourceHuojie = f.SourceHuojie! with { After = _movementSequence } });
                AdvanceEventRulesAndQueueFact(new SourceHuojieCompletedEvent(f.Id, f.OwnerSeat, r.Done, true));
                ContinueSourceBeigong(f.Id); return;
            }
        }
        else if (r.Hit)
        {
            ReplaceRuntimeTop(f = f with { SourceHuojie = r with { Stage = SourceHuojieStage.DamageChildren } });
            BeginProgramSkillDamage(f, f.OwnerSeat, 3, nature: DamageNature.Thunder, sourceLess: true);
            ContinueSourceBeigong(f.Id); return;
        }
        if (r.Done >= r.MaximumJudgments)
        {
            AdvanceEventRulesAndQueueFact(new SourceHuojieCompletedEvent(f.Id, f.OwnerSeat, r.Done, false)); FinishSourceBeigong(f); return;
        }
        var judgeId = _resolutionSequence + 1;
        ReplaceRuntimeTop(f = f with { SourceHuojie = r with { Stage = SourceHuojieStage.Judging, Done = r.Done + 1, JudgmentId = judgeId, Hit = false } });
        AdvanceEventRulesAndQueueFact(new SourceHuojieJudgmentIssuedEvent(f.Id, judgeId, f.OwnerSeat, r.Done + 1, r.MaximumJudgments));
        _ = BeginJudgment(null, f.OwnerSeat, JudgmentReasons.Lightning, f.Id, CardKind.Lightning, JudgmentContinuationKind.ProgramSkill,
            sourceSeat: f.OwnerSeat, programResultBind: "source-huojie-lightning", programResultVisibility: SkillProgramCardSetVisibility.Public);
    }
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 24512)]
    private void SourceHuojieActualReceivedDamage(DamageAppliedEvent fact)
    {
        if (!fact.SourceLess || fact.Nature != DamageNature.Thunder || fact.Amount <= 0 || CurrentDamageAttempt is not { } attempt) return;
        var f = _resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(p => p.Id == attempt.ResolutionId &&
            p.SkillId == SourceBeigongIds.Huojie && p.SourceHuojie is { Stage: SourceHuojieStage.DamageChildren });
        if (f is null || fact.TargetSeat != f.OwnerSeat || attempt.TargetSeat != f.OwnerSeat || attempt.SourceSeat != f.OwnerSeat ||
            f.AttackAttempt is not { SourceLess: true, Nature: DamageNature.Thunder }) return;
        if (!ProgramEventHistory<SourceHuojieReceivedDamageEvent>().Any(e => e.ProgramFrameId == f.Id && e.JudgmentId == f.SourceHuojie!.JudgmentId))
            AdvanceEventRulesAndQueueFact(new SourceHuojieReceivedDamageEvent(f.Id, f.SourceHuojie!.JudgmentId, f.OwnerSeat, fact.Amount));
    }
}
