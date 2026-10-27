namespace CardGame.Core;

public sealed partial class GameEngine
{
    private SourceExecutionerHuanfuBet? SourceHuanfuSettlement(int owner, string instance, string hash, CardUseFrame use) =>
        use.SourceExecutionerHuanfuBets?.SingleOrDefault(b => b.Source.OwnerSeat == owner && b.Source.SkillId == SourceExecutionerContract.Huanfu &&
            b.Source.SkillInstanceId == instance && b.GameplayHash == hash);

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 62060)]
    private void RecordSourceHuanfuDamage(DamageAppliedEvent applied)
    {
        if (!SourceExecutionerMode || applied.SourceLess || applied.Amount <= 0 || applied.SourceCardUseResolutionId is not { } id ||
            CurrentDamageAttempt is not { IsSourceLess: false, IsChainPropagation: false, EffectiveCardKind: { } kind } attack ||
            !IsSlashCard(kind) || attack.ResolutionId != id || attack.SourceSeat != applied.SourceSeat || attack.TargetSeat != applied.TargetSeat ||
            _resolutionStack.OfType<CardUseFrame>().SingleOrDefault(u => u.Id == id) is not { SourceExecutionerHuanfuBets: { } bets } use ||
            use.SourceSeat != applied.SourceSeat) return;
        AssertSourceHuanfuBets(use);
        ReplaceRuntimeFrame(use.Id, use with { SourceExecutionerHuanfuBets = bets.Select(b => !b.Settled && use.TargetSeats.Contains(applied.TargetSeat) &&
            (b.ActorWager || b.Source.OwnerSeat == applied.TargetSeat) ? b with { DamageTotal = checked(b.DamageTotal + applied.Amount) } : b).ToArray() });
    }

    private void AssertSourceHuanfuBets(CardUseFrame use)
    {
        if (use.SourceExecutionerHuanfuBets is not { } bets) return;
        if (!SourceExecutionerMode || use.Action is not { Type: CardActionType.Use } action || !IsSlashCard(use.CardKind) ||
            use.SourceSeat != action.ActorSeat || action.EffectiveKind != use.CardKind || bets.Count == 0 ||
            bets.Select(b => b.Source.OwnerSeat).Distinct().Count() != bets.Count)
            throw new InvalidOperationException("Huanfu wagers require independent owners on the same actual Slash Use.");
        foreach (var bet in bets)
        {
            var history = ProgramEventHistory<SourceExecutionerHuanfuPaidEvent>().Where(e => e.ProgramFrameId == bet.ProgramFrameId).ToArray();
            var moves = _cardMovements.Where(m => m.Sequence > bet.SequenceBefore && m.Sequence <= bet.SequenceAfter && m.Reason.Value == SourceHuanfuDiscard).ToArray();
            if (bet.UseId != use.Id || bet.ActionId != action.ActionId || bet.ActorSeat != action.ActorSeat || bet.Source.SkillId != SourceExecutionerContract.Huanfu ||
                bet.GameplayHash != _contentRegistry.GetSkill(bet.Source.SkillId).Program!.GameplayHash || bet.ProgramFrameId <= 0 ||
                !IsValidPlayerSeat(bet.Source.OwnerSeat) || bet.ActorWager != (bet.Source.OwnerSeat == action.ActorSeat) ||
                bet.Source.BindingId != (bet.ActorWager ? "actor-wager" : "target-wager") ||
                !bet.ActorWager && !bet.Targets.Contains(bet.Source.OwnerSeat) || !bet.Targets.SequenceEqual(action.TargetSeats) ||
                bet.Amount < 1 || bet.Amount > bet.MaxHpAtPayment || bet.DamageTotal < 0 || bet.Cards.Count != bet.Amount || bet.Cards.Distinct().Count() != bet.Amount ||
                bet.SequenceBefore < 0 || bet.SequenceAfter < bet.SequenceBefore || bet.SequenceAfter > _movementSequence ||
                history is not [var paid] || paid != new SourceExecutionerHuanfuPaidEvent(bet.ProgramFrameId, use.Id, action.ActionId,
                    bet.Source, bet.GameplayHash, bet.Amount, bet.MaxHpAtPayment, bet.SequenceBefore, bet.SequenceAfter) ||
                moves.Length != bet.Amount || !moves.Select(m => m.CardId).Order().SequenceEqual(bet.Cards.Order()) ||
                moves.Any(m => m.From.OwnerSeat != bet.Source.OwnerSeat || m.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) ||
                    m.To != CardLocation.DiscardPile && m.To != CardLocation.OutsideGame))
                throw new InvalidOperationException("Huanfu lost its original skill instance, HE payment invoice or same-use target scope.");
        }
    }

    private SourceExecutionerHuanfuBet? ExactIssuedSourceHuanfu(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (candidate.SkillId != SourceExecutionerContract.Huanfu || candidate.BindingId != "settle-wager" || !SourceExecutionerMode ||
            context is not { Window: SkillProgramTriggerWindow.CardUseCompleted } || context.OwnerSeat != candidate.OwnerSeat ||
            SourceHuanfuUse(context) is not { Step: ResolutionFrameStep.Completed, Action: { } action } use ||
            _resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId) is not { } window ||
            window.ParentFrameId != use.Id || window.Continuation != ProgramCardContinuation.CompletedSlash || window.CompletedResponseReturn is not null ||
            window.Action.ActionId != action.ActionId || window.Action.Type != CardActionType.Use ||
            !window.Action.TargetSeats.SequenceEqual(action.TargetSeats) || !window.Action.PhysicalCards.SequenceEqual(action.PhysicalCards) ||
            window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count) return null;
        var wi = _resolutionStack.FindIndex(f => f.Id == window.Id); var current = window.Candidates[window.CandidateIndex];
        if (wi < 1 || _resolutionStack[wi - 1].Id != use.Id || current.OwnerSeat != candidate.OwnerSeat || current.SkillId != candidate.SkillId ||
            current.SkillInstanceId != candidate.SkillInstanceId || current.TriggerId != candidate.BindingId || current.GameplayHash != candidate.GameplayHash ||
            CreateCardActionProgramContext(window, current) != context || !_players[candidate.OwnerSeat].IsAlive) return null;
        AssertSourceHuanfuBets(use);
        return SourceHuanfuSettlement(candidate.OwnerSeat, candidate.SkillInstanceId, candidate.GameplayHash, use);
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CollectIssuedCardTrigger, 62060)]
    private void CollectIssuedSourceHuanfu(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result)
    {
        if (window != SkillProgramTriggerWindow.CardUseCompleted || !SourceExecutionerMode || _winner != Winner.None ||
            _resolutionStack.LastOrDefault() is not CardUseFrame { SourceExecutionerHuanfuBets: { } bets } use || use.Action?.ActionId != action.ActionId) return;
        AssertSourceHuanfuBets(use);
        foreach (var bet in bets.Where(b => !b.Settled && _players[b.Source.OwnerSeat].IsAlive))
        {
            var source = bet.Source; var program = _contentRegistry.GetSkill(source.SkillId).Program!;
            var trigger = program.Triggers.Single(t => t.Id == "settle-wager");
            result.RemoveAll(c => c.OwnerSeat == source.OwnerSeat && c.SkillId == source.SkillId && c.TriggerId == trigger.Id);
            var target = action.TargetSeats.Count == 1 ? action.TargetSeats.Single() : -1;
            var context = CreateCardActionProgramContext(action, window, 0, source.OwnerSeat, target,
                CaptureProgramTriggerFacts(_players[source.OwnerSeat], action) with { CardUseCausedDamage = use.CausedDamage });
            result.Add(new(source.OwnerSeat, target, source.SkillId, trigger.Id, bet.GameplayHash, source.SkillInstanceId, trigger.Priority, context));
        }
    }

    [ProgramPipelineHook(ProgramPipelinePhase.IssuedTriggerCandidate, 62060)]
    private bool IsIssuedSourceHuanfu(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) =>
        ExactIssuedSourceHuanfu(candidate, context) is { Settled: false };

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 62060, SkillProgramEffectOp.ResolveSourceExecutionerHuanfu)]
    private bool CanOfferSourceHuanfu(ProgramTriggerLegalityContext gate) => gate.SkillId == SourceExecutionerContract.Huanfu &&
        (gate.Candidate.BindingId == "settle-wager" ? ExactIssuedSourceHuanfu(gate.Candidate, gate.Context) is { Settled: false } :
            SourceHuanfuArmQualifies(gate.OwnerSeat, gate.Candidate.BindingId, gate.Context));

    private bool ValidPaidSourceHuanfuFrame(ProgramSkillFrame f)
    {
        if (f.SourceExecutionerHuanfu is not { Stage: not SourceExecutionerHuanfuStage.Cards } r || f.WindowContext is not { } context ||
            SourceHuanfuUse(context) is not { } use || r.UseId != use.Id || r.ActionId != use.Action!.ActionId) return false;
        AssertSourceHuanfuBets(use);
        var bet = r.Stage == SourceExecutionerHuanfuStage.CostChildren
            ? SourceHuanfuSettlement(f.OwnerSeat, f.SkillInstanceId, f.GameplayHash, use)
            : ExactIssuedSourceHuanfu(new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, 0), context);
        return bet is not null && (r.Stage == SourceExecutionerHuanfuStage.CostChildren ? bet.ProgramFrameId == f.Id && !bet.Settled :
            bet.ProgramFrameId == r.PaidProgramFrameId && bet.Settled && bet.DamageTotal == bet.Amount);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 62060)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 62060)]
    private bool KeepPaidSourceHuanfu(ProgramSkillFrame f) => ValidPaidSourceHuanfuFrame(f) ||
        f.TriggerId == "settle-wager" && f.WindowContext is { } context &&
        ExactIssuedSourceHuanfu(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, 0), context) is not null;

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.BeforeSelectionState, 62060)]
    private void AssertSourceHuanfuFrame(ProgramSkillFrame f)
    {
        if (f.SourceExecutionerHuanfu is not { } r) return;
        var use = ExactSourceHuanfuParent(f);
        if (r.UseId != use.Id || r.ActionId != use.Action!.ActionId || !Enum.IsDefined(r.Stage) || r.Cards.Distinct().Count() != r.Cards.Count ||
            r.Stage == SourceExecutionerHuanfuStage.Cards && (f.TriggerId == "settle-wager" || r.Cards.Count > r.Limit) ||
            r.Stage != SourceExecutionerHuanfuStage.Cards && !ValidPaidSourceHuanfuFrame(f))
            throw new InvalidOperationException("Huanfu lost its owning payment/draw stage or original Slash parent.");
    }
}
