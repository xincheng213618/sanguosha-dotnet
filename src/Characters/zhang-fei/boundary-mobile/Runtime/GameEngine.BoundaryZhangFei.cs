namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string BoundaryZhangFeiPaoxiao = "boundary:paoxiao-mobile", BoundaryZhangFeiLiyong = "boundary:liyong-mobile",
        BoundaryZhangFeiReserve = "reserve-after-real-dodge", BoundaryZhangFeiIssue = "issue-next-real-use", BoundaryZhangFeiTargets = "issued-target-rights", BoundaryZhangFeiPenalty = "issued-surviving-damage";

    [ProgramDistanceRule(ProgramDistanceStage.PhysicalSlash, 58600)]
    [ProgramDistanceRule(ProgramDistanceStage.VirtualSlash, 58600)]
    [ProgramDistanceRule(ProgramDistanceStage.ProvidedSlash, 58600)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedSlash, 58600)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPhysicalSlash, 58600)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPaidWeaponSlash, 58600)]
    [ProgramDistanceRule(ProgramDistanceStage.SpecificSlashExemption, 58600)]
    private bool BoundaryZhangFeiPaoxiaoDistance(ProgramSlashDistanceQuery query) =>
        IsSlashCard(query.Kind) && _phase == TurnPhase.Play && query.Actor.Seat == _currentSeat && _turnProgression.OwnerSeat == query.Actor.Seat &&
        HasRuntimeSkill(query.Actor, BoundaryZhangFeiPaoxiao) && EventsSinceLastBoundary(fact => fact is PhaseChangedEvent or TurnStartedEvent)
            .OfType<CardUseDeclaredEvent>().Any(fact => fact.SourceSeat == query.Actor.Seat && IsSlashCard(fact.CardKind) && fact.ResolutionId != query.ExistingUseFrameId);

    private CardConversionSource BoundaryZhangFeiSource(ProgramSkillFrame frame) => new(frame.SkillId, GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId);
    private bool BoundaryZhangFeiOwnPlay(int owner) => _phase == TurnPhase.Play && _currentSeat == owner && _turnProgression.OwnerSeat == owner && _turnProgression.TurnNumber == _turnNumber;
    private BoundaryLiyongReserve? BoundaryZhangFeiReady(int owner, string instance)
    {
        if (!BoundaryZhangFeiOwnPlay(owner)) return null;
        var reserve = ProgramEventHistory<BoundaryLiyongReservedEvent>().Select(fact => fact.Reserve)
            .LastOrDefault(item => item.Source.OwnerSeat == owner && item.Source.SkillInstanceId == instance && item.TurnNumber == _turnNumber && item.PhaseInstanceId == _cardUseDebitPhaseInstanceId);
        return reserve is not null && !ProgramEventHistory<BoundaryLiyongUseIssuedEvent>().Any(fact => fact.Use.Reserve == reserve) ? reserve : null;
    }
    private CardUseFrame? BoundaryZhangFeiWindowUse(ProgramSkillWindowContext? context) => context?.CardUse is { } card &&
        LifecycleCardUse(card.ParentCardUseFrameId) is { Action: { Type: CardActionType.Use } action } use && action.ActionId == card.CardActionId && IsSlashCard(action.EffectiveKind) ? use : null;
    private bool BoundaryZhangFeiSourceMatches(BoundaryLiyongUse right, ProgramTriggerCandidate candidate) => right.Reserve.Source.OwnerSeat == candidate.OwnerSeat &&
        right.Reserve.Source.SkillId == candidate.SkillId && right.Reserve.Source.SkillInstanceId == candidate.SkillInstanceId && right.Reserve.GameplayHash == candidate.GameplayHash;

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 58600)]
    private bool CanOfferBoundaryZhangFei(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (candidate.SkillId != BoundaryZhangFeiLiyong) return true;
        if (candidate.BindingId == BoundaryZhangFeiReserve) return BoundaryZhangFeiOwnPlay(candidate.OwnerSeat) && BoundaryZhangFeiWindowUse(context) is { CardAttack: { } state } use &&
            use.Action!.ActorSeat == candidate.OwnerSeat && state.SuccessfulDodgeResponses >= state.RequiredDodgeResponses &&
            ActiveCardAttack is { } attack && attack.ResolutionId == use.Id && !IsSlashDodgeCancellationPrevented(attack);
        if (candidate.BindingId == BoundaryZhangFeiIssue) return BoundaryZhangFeiOwnPlay(candidate.OwnerSeat) && BoundaryZhangFeiReady(candidate.OwnerSeat, candidate.SkillInstanceId) is not null &&
            BoundaryZhangFeiWindowUse(context) is { BoundaryLiyong: null, Action: { } action } && action.ActorSeat == candidate.OwnerSeat;
        if (candidate.BindingId == BoundaryZhangFeiTargets) return BoundaryZhangFeiWindowUse(context) is { BoundaryLiyong: { } right, Action: { } action } &&
            BoundaryZhangFeiSourceMatches(right, candidate) && context.TargetSeat is { } target && _players[target].IsAlive && action.EffectiveDesignatedTargetSeats.Contains(target);
        return candidate.BindingId == BoundaryZhangFeiPenalty && BoundaryZhangFeiPenaltyUse(candidate, context) is not null;
    }

    private SkillProgramStepOutcome ResolveBoundaryZhangFei(ProgramSkillFrame supplied, SkillProgramEffectOp operation)
    {
        var frame = GetActiveProgramFrame(supplied.Id); var context = frame.WindowContext ?? throw new InvalidOperationException("Liyong lost its native window.");
        if (operation == SkillProgramEffectOp.PayBoundaryZhangFeiLiyong) return PayBoundaryZhangFeiLiyong(frame);
        var candidate = new ProgramTriggerCandidate(frame.OwnerSeat, frame.SkillId, GetProgramBindingId(frame), frame.SkillInstanceId, frame.GameplayHash, GetProgramTrigger(frame).Priority);
        if (!CanOfferBoundaryZhangFei(candidate, context)) throw new InvalidOperationException("Liyong lost its original actual use or phase qualification.");
        var use = BoundaryZhangFeiWindowUse(context)!;
        if (operation == SkillProgramEffectOp.ReserveBoundaryZhangFeiLiyong)
        {
            var target = use.CardAttack!.TargetSeat; var cancellation = new NamedSlashCancellation(frame.SkillId, frame.OwnerSeat, target);
            if (use.NamedSlashCancellations?.Contains(cancellation) == true) return SkillProgramStepOutcome.Continue;
            ReplaceRuntimeFrame(use.Id, use with { NamedSlashCancellations = Array.AsReadOnly((use.NamedSlashCancellations ?? []).Append(cancellation).ToArray()) });
            AdvanceEventRulesAndQueueFact(new BoundaryLiyongReservedEvent(new(frame.Id, BoundaryZhangFeiSource(frame), frame.GameplayHash, _turnNumber,
                _cardUseDebitPhaseInstanceId, use.Id, use.Action!.ActionId, target)));
        }
        else if (context.Window == SkillProgramTriggerWindow.CardUseCommitted)
        {
            var reserve = BoundaryZhangFeiReady(frame.OwnerSeat, frame.SkillInstanceId)!;
            var right = new BoundaryLiyongUse(reserve, use.Id, use.Action!.ActionId);
            ReplaceRuntimeFrame(use.Id, use with { BoundaryLiyong = right });
            AdvanceEventRulesAndQueueFact(new BoundaryLiyongUseIssuedEvent(right));
        }
        else
        {
            var target = context.TargetSeat!.Value;
            IssueCurrentTurnNonLockedSkillSuppression(frame, target);
            AdvanceEventRulesAndQueueFact(new BoundaryLiyongTargetIssuedEvent(frame.Id, use.Id, use.Action!.ActionId, target));
        }
        return SkillProgramStepOutcome.Continue;
    }

    private bool IsBoundaryZhangFeiLiyongUnrespondable(long frameId) => LifecycleCardUse(frameId) is { Action: { } action, BoundaryLiyong: { } right } &&
        right.CardUseFrameId == frameId && right.CardActionId == action.ActionId && IsSlashCard(action.EffectiveKind);
    private int BoundaryZhangFeiLiyongCardDamage(IDamageAttempt attack) => !attack.IsChainPropagation && attack is CardAttackHandle && IsSlashCard(attack.EffectiveCardKind ?? CardKind.Slash)
        ? FinalTargetSlashReceipts(attack.ResolutionId, attack.TargetSeat).Where(receipt => receipt.SkillId == BoundaryZhangFeiLiyong).Sum(receipt => receipt.DamageBonus) : 0;

    [ProgramPipelineHook(ProgramPipelinePhase.CollectIssuedCardTrigger, 58600)]
    private void IncludeBoundaryZhangFeiTargets(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result)
    {
        if (!IsSlashCard(action.EffectiveKind) || action.Type != CardActionType.Use || window is not (SkillProgramTriggerWindow.CardUseCommitted or SkillProgramTriggerWindow.CardUseTargetsFinalized)) return;
        var use = _resolutionStack.OfType<CardUseFrame>().SingleOrDefault(item => item.Action?.ActionId == action.ActionId);
        var reserve = window == SkillProgramTriggerWindow.CardUseCommitted ? ProgramEventHistory<BoundaryLiyongReservedEvent>().Select(item => item.Reserve)
            .LastOrDefault(item => item.Source.OwnerSeat == action.ActorSeat && BoundaryZhangFeiReady(action.ActorSeat, item.Source.SkillInstanceId) == item) : use?.BoundaryLiyong?.Reserve;
        if (reserve is null) return;
        var binding = window == SkillProgramTriggerWindow.CardUseCommitted ? BoundaryZhangFeiIssue : BoundaryZhangFeiTargets;
        result.RemoveAll(candidate => candidate.SkillId == BoundaryZhangFeiLiyong && candidate.TriggerId == binding);
        var trigger = _contentRegistry.GetSkill(BoundaryZhangFeiLiyong).Program!.Triggers.Single(item => item.Id == binding);
        var targets = window == SkillProgramTriggerWindow.CardUseCommitted ? new[] { action.ActorSeat } : action.EffectiveDesignatedTargetSeats.Where(seat => _players[seat].IsAlive).ToArray();
        foreach (var target in targets)
        {
            var owner = reserve.Source.OwnerSeat;
            var context = CreateCardActionProgramContext(action, window, 0, owner, target, CaptureProgramTriggerFacts(_players[owner], action));
            result.Add(new(owner, target, BoundaryZhangFeiLiyong, trigger.Id, reserve.GameplayHash, reserve.Source.SkillInstanceId, trigger.Priority, context));
        }
    }
    [ProgramPipelineHook(ProgramPipelinePhase.IssuedTriggerCandidate, 58600)]
    private bool IsIssuedBoundaryZhangFei(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => candidate.SkillId == BoundaryZhangFeiLiyong &&
        (candidate.BindingId == BoundaryZhangFeiIssue && context.Window == SkillProgramTriggerWindow.CardUseCommitted && BoundaryZhangFeiReady(candidate.OwnerSeat, candidate.SkillInstanceId) is { } reserve && reserve.GameplayHash == candidate.GameplayHash && BoundaryZhangFeiWindowUse(context)?.Action?.ActorSeat == candidate.OwnerSeat ||
         candidate.BindingId == BoundaryZhangFeiTargets && BoundaryZhangFeiWindowUse(context)?.BoundaryLiyong is { } right && BoundaryZhangFeiSourceMatches(right, candidate) ||
         candidate.BindingId == BoundaryZhangFeiPenalty && BoundaryZhangFeiPenaltyUse(candidate, context) is not null);
    [ProgramPipelineHook(ProgramPipelinePhase.AdmitTurnSuppression, 58600)]
    private bool AdmitBoundaryZhangFeiSuppression(ProgramSkillFrame frame, int target) => frame.SkillId == BoundaryZhangFeiLiyong && frame.TriggerId == BoundaryZhangFeiTargets &&
        frame.WindowContext?.TargetSeat == target && BoundaryZhangFeiWindowUse(frame.WindowContext)?.BoundaryLiyong is { } right && right.Reserve.Source.SkillInstanceId == frame.SkillInstanceId;
    [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish, 58600)]
    private void AssertBoundaryZhangFeiUses()
    {
        foreach (var use in _resolutionStack.OfType<CardUseFrame>().Where(item => item.BoundaryLiyong is not null))
        {
            var right = use.BoundaryLiyong!; var reserve = right.Reserve;
            if (use.Action is not { Type: CardActionType.Use } action || !IsSlashCard(action.EffectiveKind) || right.CardUseFrameId != use.Id || right.CardActionId != action.ActionId ||
                reserve.Source.SkillId != BoundaryZhangFeiLiyong || reserve.Source.OwnerSeat != action.ActorSeat || reserve.Source.BindingId != BoundaryZhangFeiReserve || reserve.ProducerFrameId <= 0 ||
                reserve.TurnNumber != _turnNumber || reserve.PhaseInstanceId != _cardUseDebitPhaseInstanceId || reserve.CancelledUseId == use.Id ||
                ProgramEventHistory<BoundaryLiyongReservedEvent>().Count(fact => fact.Reserve == reserve) != 1 || ProgramEventHistory<BoundaryLiyongUseIssuedEvent>().Count(fact => fact.Use == right) != 1)
                throw new InvalidOperationException("Liyong lost the original phase cancellation or exact next actual Use receipt.");
        }
    }
    [ProgramCapability]
    private sealed class BoundaryZhangFeiCapability(GameEngine engine) : IBoundaryZhangFeiHost
    {
        public SkillProgramStepOutcome ResolveBoundaryZhangFei(ProgramSkillFrame frame, SkillProgramEffectOp operation) => engine.ResolveBoundaryZhangFei(frame, operation);
    }
}
