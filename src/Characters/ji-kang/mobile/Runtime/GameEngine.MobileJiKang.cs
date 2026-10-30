using System.Globalization;
namespace CardGame.Core;

public sealed partial class GameEngine
{
    private static CardConversionSource MobileJiKangSource(ProgramSkillFrame f) => new(f.SkillId, GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    private const string MobileJiKangDiscard = "skill-program.mobile-ji-kang.discard";
    [ProgramCapability]
    private sealed class MobileJiKangHost(GameEngine e) : IMobileJiKangHost
    { public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => e.BeginMobileJiKang(f, op); }

    private IReadOnlyList<PromptChoice> MobileJiKangCosts(long frame, int owner) => BuildOwnedCardPaymentChoices(frame, owner, owner,
        [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard).Where(c => c.Cards.Count == 1).ToArray();
    private bool MobileJiKangHasCost(CharacterState owner) => GetHand(owner).Concat(GetEquipment(owner)).Any(card =>
        !IsSelfHandCategoryDiscardForbidden(owner.Seat, card, _cardZones.GetLocation(card.Id), OwnedCardMoveIntent.Discard) &&
        !IsForeignEquipmentDiscardPrevented(owner.Seat, card, _cardZones.GetLocation(card.Id), OwnedCardMoveIntent.Discard));
    private bool MobileJiKangTargetAllowed(CardConversionSource source, int seat) => IsValidPlayerSeat(seat) && seat != source.OwnerSeat && _players[seat].IsAlive &&
        (source.SkillId != "mobile:canyun" || !ProgramEventHistory<MobileJiKangTargetIssuedEvent>().Any(e => e.Source == source && e.TargetSeat == seat));
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 80900, SkillProgramEffectOp.ResolveMobileJiKangQingxian)]
    private bool CanActivateMobileJiKang(ProgramActivationLegalityContext gate) => gate.Owner is { IsAlive: true, HasHpValue: true, Hp: > 0 } &&
        MobileJiKangHasCost(gate.Owner) && GetSkillBindingShard(gate.Owner).ActiveGrants.Where(g => g.SkillId == gate.SkillId).Any(g =>
            _players.Any(p => MobileJiKangTargetAllowed(new(g.SkillId, gate.Activation.Id, gate.OwnerSeat, g.SkillInstanceId), p.Seat)));

    private SkillProgramStepOutcome BeginMobileJiKang(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = GetActiveProgramFrame(supplied.Id);
        if (f.MobileJiKang is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0)
            throw new InvalidOperationException("Mobile Ji Kang requires his original single issuing instruction.");
        int? killer = null;
        if (op == SkillProgramEffectOp.ResolveMobileJiKangJuexiang)
        {
            if (f.SkillId != "mobile:juexiang" || f.WindowContext is not { Window: SkillProgramTriggerWindow.OwnerDied, SourceSeat: var source } || _players[f.OwnerSeat].IsAlive)
                throw new InvalidOperationException("Juexiang requires the actual original owner-death window.");
            killer = source is { } seat && IsValidPlayerSeat(seat) && _players[seat].IsAlive ? seat : null;
        }
        else if (f.SkillId is not ("mobile:qingxian" or "mobile:canyun") || f.ActivationId != "equipment-comparison" || f.WindowContext is not null ||
            !_players[f.OwnerSeat].IsAlive || !_players[f.OwnerSeat].HasHpValue || _players[f.OwnerSeat].Hp <= 0 || _phase != TurnPhase.Play || _currentSeat != f.OwnerSeat ||
            MobileJiKangCosts(f.Id, f.OwnerSeat).Count == 0 || !_players.Any(p => MobileJiKangTargetAllowed(MobileJiKangSource(f), p.Seat)))
            throw new InvalidOperationException("Qingxian requires its exact current Play activation.");
        var r = new MobileJiKangReceipt { Source = MobileJiKangSource(f), Hash = f.GameplayHash, InitialHp = _players[f.OwnerSeat].Hp, Killer = killer,
            Stage = op == SkillProgramEffectOp.ResolveMobileJiKangJuexiang ? MobileJiKangStage.KillerDiscardChildren : MobileJiKangStage.Targets };
        ReplaceRuntimeTop(f = f with { MobileJiKang = r });
        AdvanceEventRulesAndQueueFact(new MobileJiKangIssuedEvent(f.Id, r.Source, r.Hash, r.InitialHp, killer));
        if (op == SkillProgramEffectOp.ResolveMobileJiKangJuexiang && killer is { } actual)
        {
            var cards = GetEquipment(_players[actual]).Where(c => !IsForeignEquipmentDiscardPrevented(actual, c, _cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Discard)).Select(c => c.Id).ToArray();
            PayMobileJiKang(f, cards, actual, actual, MobileJiKangStage.KillerDiscardChildren);
        }
        ContinueMobileJiKang(f.Id); return SkillProgramStepOutcome.AwaitChild;
    }

    private void PayMobileJiKang(ProgramSkillFrame f, IReadOnlyList<int> cards, int actor, int subject, MobileJiKangStage after)
    {
        var before = _movementSequence;
        ReplaceRuntimeTop(f = f with { MobileJiKang = f.MobileJiKang! with { Stage = after, Cards = cards, Before = before, After = before, Paid = cards.Count == 0 } });
        if (cards.Count == 0) return;
        var logical = CaptureNativeLogicalDiscardInvocation(actor, subject, cards, new(MobileJiKangDiscard), f.Id);
        MoveProgramCardsFromMultipleSources(cards, CardLocation.DiscardPile, new(MobileJiKangDiscard), (_, moves) =>
        {
            var live = GetActiveProgramFrame(f.Id);
            var actual = moves.Count(m => m.To == CardLocation.DiscardPile || m.To == CardLocation.OutsideGame);
            ReplaceRuntimeTop(live with { MobileJiKang = live.MobileJiKang! with { After = _movementSequence, Paid = actual == cards.Count } });
            AdvanceEventRulesAndQueueFact(new MobileJiKangProgressEvent(f.Id, "discard", live.MobileJiKang!.Cursor, subject, actual, actor, before, _movementSequence));
        });
        CompleteNativeLogicalDiscardInvocation(logical);
    }
    private bool DrainMobileJiKang(long id) => TryBeginProgramChildWindows(id, PostEventContinuation.Program);
    private void StepMobileJiKang(ProgramSkillFrame f, MobileJiKangReceipt r)
    { ReplaceRuntimeTop(f with { MobileJiKang = r }); }
    private void FinishMobileJiKang(ProgramSkillFrame f)
    {
        AdvanceEventRulesAndQueueFact(new MobileJiKangProgressEvent(f.Id, "complete", f.MobileJiKang!.Cursor, f.MobileJiKang.Subject, 0, 0));
        ReplaceRuntimeTop(f with { MobileJiKang = null }); AdvanceRuntimeProgram(f.Id);
    }
    private void ContinueMobileJiKang(long id)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { MobileJiKang: { } r } f || f.Id != id) return;
        if (f.NativeDraw is not null) return;
        if (r.Stage is MobileJiKangStage.Targets or MobileJiKangStage.Cards or MobileJiKangStage.Recipient or MobileJiKangStage.Club)
        { if (_pendingDecision is null) PublishMobileJiKang(f); return; }
        if (DrainMobileJiKang(id)) return;
        f = GetActiveProgramFrame(id); r = f.MobileJiKang!;
        if (_winner != Winner.None) { FinishMobileJiKang(f); return; }
        switch (r.Stage)
        {
            case MobileJiKangStage.CostChildren:
                if (!r.Paid) throw new InvalidOperationException("Qingxian did not discard its exact number of selected targets.");
                foreach (var (seat, index) in r.Targets.Select((seat, index) => (seat, index)))
                    AdvanceEventRulesAndQueueFact(new MobileJiKangTargetIssuedEvent(id, r.Source, index, seat));
                StepMobileJiKang(f, r with { Stage = MobileJiKangStage.Compare }); ContinueMobileJiKang(id); return;
            case MobileJiKangStage.Compare:
                if (r.Cursor >= r.Targets.Count) { StepMobileJiKang(f, r with { Stage = MobileJiKangStage.Reward }); ContinueMobileJiKang(id); return; }
                var target = r.Targets[r.Cursor];
                if (!_players[target].IsAlive) { StepMobileJiKang(f, r with { Cursor = r.Cursor + 1 }); ContinueMobileJiKang(id); return; }
                var own = GetEquipment(_players[f.OwnerSeat]).Count; var other = GetEquipment(_players[target]).Count;
                var comparison = other.CompareTo(own);
                AdvanceEventRulesAndQueueFact(new MobileJiKangProgressEvent(id, "compare", r.Cursor, target, own, other));
                if (comparison == 0)
                {
                    StepMobileJiKang(f, r with { Stage = MobileJiKangStage.Drawing, Subject = target, Comparison = comparison });
                    BeginOperationProgramDraw(GetActiveProgramFrame(id), target, 1, new("skill-program.mobile-ji-kang.equal-draw")); return;
                }
                StepMobileJiKang(f, r with { Stage = MobileJiKangStage.Health, Subject = target, Comparison = comparison, Paid = false });
                ContinueMobileJiKang(id); return;
            case MobileJiKangStage.Health:
                if (!_players[r.Subject].IsAlive) { StepMobileJiKang(f, r with { Stage = MobileJiKangStage.Compare, Cursor = r.Cursor + 1 }); ContinueMobileJiKang(id); return; }
                if (r.Comparison < 0)
                {
                    StepMobileJiKang(f, r with { Stage = MobileJiKangStage.HealthChildren, Paid = true });
                    AdvanceEventRulesAndQueueFact(new MobileJiKangProgressEvent(id, "recover", r.Cursor, r.Subject, 1, 0));
                    new ProgramSkillHost(this).Recover(id, f.OwnerSeat, r.Subject, 1, null, null);
                    ContinueMobileJiKang(id); return;
                }
                LoseMobileJiKangHp(f, r.Subject, MobileJiKangStage.HealthChildren); return;
            case MobileJiKangStage.HealthChildren:
                StepMobileJiKang(f, r with { Stage = MobileJiKangStage.Compare, Cursor = r.Cursor + 1 }); ContinueMobileJiKang(id); return;
            case MobileJiKangStage.Reward:
                if (r.Targets.Count == r.InitialHp && _players[f.OwnerSeat].IsAlive)
                {
                    StepMobileJiKang(f, r with { Stage = MobileJiKangStage.RewardDrawing, Subject = f.OwnerSeat });
                    BeginOperationProgramDraw(GetActiveProgramFrame(id), f.OwnerSeat, 1, new("skill-program.mobile-ji-kang.full-count-draw")); return;
                }
                FinishMobileJiKang(f); return;
            case MobileJiKangStage.KillerDiscardChildren:
                if (r.Killer is { } killer && _players[killer].IsAlive)
                { StepMobileJiKang(f, r with { Stage = MobileJiKangStage.KillerHealth, Subject = killer, Paid = false }); ContinueMobileJiKang(id); return; }
                StepMobileJiKang(f, r with { Stage = MobileJiKangStage.Recipient }); ContinueMobileJiKang(id); return;
            case MobileJiKangStage.KillerHealth:
                if (_players[r.Subject].IsAlive) { LoseMobileJiKangHp(f, r.Subject, MobileJiKangStage.KillerHealthChildren); return; }
                StepMobileJiKang(f, r with { Stage = MobileJiKangStage.Recipient }); ContinueMobileJiKang(id); return;
            case MobileJiKangStage.KillerHealthChildren:
                StepMobileJiKang(f, r with { Stage = MobileJiKangStage.Recipient }); ContinueMobileJiKang(id); return;
            case MobileJiKangStage.GrantChildren:
                if (!_players[r.Subject].IsAlive) { FinishMobileJiKang(f); return; }
                StepMobileJiKang(f, r with { Stage = MobileJiKangStage.Club }); ContinueMobileJiKang(id); return;
            case MobileJiKangStage.ClubChildren:
                if (r.Paid && _players[r.Subject].IsAlive)
                {
                    StepMobileJiKang(f, r with { Stage = MobileJiKangStage.FinalGrantChildren });
                    GrantProgramSkills(GetActiveProgramFrame(id), r.Subject, ["mobile:juexiang"]);
                    AdvanceEventRulesAndQueueFact(new MobileJiKangProgressEvent(id, "inherit-juexiang", r.Cursor, r.Subject, 1, 0));
                    ContinueMobileJiKang(id); return;
                }
                FinishMobileJiKang(f); return;
            case MobileJiKangStage.FinalGrantChildren: FinishMobileJiKang(f); return;
            default: throw new InvalidOperationException("Mobile Ji Kang lost its exact typed native return stage.");
        }
    }
    private void LoseMobileJiKangHp(ProgramSkillFrame f, int target, MobileJiKangStage after)
    {
        if (HasHappyQiaoHealth(target) && TryBeginHappyQiaoProgramHealthChoice(f, target, HpChangeKind.Loss, 1)) return;
        var outcome = ApplyProgramHpLoss(f.Id, f.SkillId, target, 1, (before, next, actual) =>
        {
            var live = GetActiveProgramFrame(f.Id);
            StepMobileJiKang(live, live.MobileJiKang! with { Stage = after, Paid = true });
            AdvanceEventRulesAndQueueFact(new MobileJiKangProgressEvent(f.Id, "lose-hp", live.MobileJiKang!.Cursor, target, actual, before - next));
            if (HasHappyQiaoHealth(target)) CompleteHappyQiaoHealthPayment(f.Id, target, HpChangeKind.Loss, before, next);
        });
        if (outcome == SkillProgramStepOutcome.Continue) ContinueMobileJiKang(f.Id);
    }
    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveMobileJiKangQingxian)]
    private void MobileJiKangDrawReturned(ProgramSkillFrame supplied, NativeDrawOutcome outcome)
    {
        var f = GetActiveProgramFrame(supplied.Id); var r = f.MobileJiKang ?? throw new InvalidOperationException("Ji Kang draw lost its original receipt.");
        if (r.Stage is not (MobileJiKangStage.Drawing or MobileJiKangStage.RewardDrawing) || f.NativeDraw is not null ||
            outcome.RecipientSeat != r.Subject || outcome.RequestedCount != 1 || outcome.ActualDrawIds.Count > 1)
            throw new InvalidOperationException("Ji Kang draw lost its exact original recipient and one-card typed return.");
        AdvanceEventRulesAndQueueFact(new MobileJiKangProgressEvent(f.Id, "draw-return", r.Cursor, r.Subject, outcome.ActualDrawIds.Count, 1, outcome.SequenceBefore, outcome.SequenceAfter));
        if (r.Stage == MobileJiKangStage.RewardDrawing) { FinishMobileJiKang(f); return; }
        StepMobileJiKang(f, r with { Stage = MobileJiKangStage.Compare, Cursor = r.Cursor + 1 }); ContinueMobileJiKang(f.Id);
    }
    [ProgramContinuation(ProgramContinuationKind.Resume, 80900)]
    private bool ResumeMobileJiKang(long id)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { MobileJiKang: not null } f || f.Id != id) return false;
        ContinueMobileJiKang(id); return true;
    }
}
