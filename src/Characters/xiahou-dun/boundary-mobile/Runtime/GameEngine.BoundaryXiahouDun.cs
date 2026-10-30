using System.Globalization;
namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string BoundaryXiahouDunQingjian = "boundary:qingjian-mobile", BoundaryXiahouDunAction = "boundary-xiahou-dun-qingjian";
    private const string BoundaryXiahouDunStoreReason = "skill.boundary-xiahou-dun.qingjian.store", BoundaryXiahouDunGiveReason = "skill.boundary-xiahou-dun.qingjian.give";
    private static CardConversionSource BoundaryXiahouDunSource(ProgramSkillFrame f) => new(f.SkillId, GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    [ProgramCapability] private sealed class BoundaryXiahouDunCapability(GameEngine engine) : IBoundaryXiahouDunHost
    { public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => engine.BeginBoundaryXiahouDun(f, op); }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 60010)]
    private bool OfferBoundaryXiahouDun(ProgramTriggerLegalityContext gate) => gate.SkillId != BoundaryXiahouDunQingjian ||
        (gate.Trigger.Id == "store-after-gain" ? GetHand(_players[gate.OwnerSeat]).Count > 0 : BoundaryXiahouDunDueDeposits(gate.OwnerSeat, gate.Context.ParentFrameId).Any());

    private SkillProgramStepOutcome BeginBoundaryXiahouDun(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = GetActiveProgramFrame(supplied.Id);
        if (f.BoundaryXiahouDun is not null || f.InstructionIndex != 1 || f.SkillId != BoundaryXiahouDunQingjian || f.WindowContext is not { } context ||
            (op == SkillProgramEffectOp.StoreBoundaryXiahouDunQingjian ? context.Window != SkillProgramTriggerWindow.CardsGained || f.TriggerId != "store-after-gain" :
                op != SkillProgramEffectOp.DistributeBoundaryXiahouDunQingjian || context.Window != SkillProgramTriggerWindow.TurnEnding || f.TriggerId != "distribute-at-ending"))
            throw new InvalidOperationException("Mobile Qingjian requires its original gain or issued ending frame.");
        var storing = op == SkillProgramEffectOp.StoreBoundaryXiahouDunQingjian;
        var r = new BoundaryXiahouDunReceipt(BoundaryXiahouDunSource(f), f.GameplayHash, op, context.ParentFrameId, _turnNumber)
        {
            Stage = storing ? BoundaryXiahouDunStage.Selecting : BoundaryXiahouDunStage.Distributing,
            Location = storing ? new(CardZoneKind.PrivatePersistentPile, f.OwnerSeat, $"boundary-xiahou-dun:{f.Id}") : null,
            Deposits = storing ? [] : BoundaryXiahouDunDueDeposits(f.OwnerSeat, context.ParentFrameId).Where(d => d.Source.SkillInstanceId == f.SkillInstanceId && d.Hash == f.GameplayHash).Select(d => d.FrameId).ToArray(),
            Before = _movementSequence, After = _movementSequence
        };
        ReplaceRuntimeTop(f = f with { BoundaryXiahouDun = r }); ContinueBoundaryXiahouDun(f.Id); return SkillProgramStepOutcome.AwaitChoice;
    }

    private IReadOnlyList<PromptChoice> BoundaryXiahouDunHandChoices(ProgramSkillFrame f) =>
        BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, f.OwnerSeat, [CardZoneKind.Hand], OwnedCardMoveIntent.Transfer).Select(c => c with
        { Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = BoundaryXiahouDunAction, ["branch"] = "toggle" } }).ToArray();
    private Card BoundaryXiahouDunHandCard(ProgramSkillFrame f, PromptChoice c) => _cardZones.CardsAt(CardLocation.Hand(f.OwnerSeat))[int.Parse(c.Parameters["slot-index"], CultureInfo.InvariantCulture)];
    private IEnumerable<Card> BoundaryXiahouDunHeld(ProgramSkillFrame f) => f.BoundaryXiahouDun!.Deposits.SelectMany(id => _cardZones.CardsAt(BoundaryXiahouDunDeposit(id).Location));
    private PromptChoice BoundaryXiahouDunChoice(ProgramSkillFrame f, string branch, int value, string text, IReadOnlyList<int>? cards = null, IReadOnlyList<int>? targets = null) =>
        new(new($"boundary-xiahou-dun.{f.Id}.{branch}.{value}.{targets?.FirstOrDefault() ?? -1}"), text, cards ?? [], targets ?? [],
            new Dictionary<string, string> { ["program-action"] = BoundaryXiahouDunAction, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch });
    private IReadOnlyList<PromptChoice> BoundaryXiahouDunChoices(ProgramSkillFrame f)
    {
        var r = f.BoundaryXiahouDun!;
        if (r.Stage == BoundaryXiahouDunStage.Selecting)
            return BoundaryXiahouDunHandChoices(f).Select(c => c with { Description = (r.Selected.Contains(BoundaryXiahouDunHandCard(f, c).Id) ? "已选：" : "") + c.Description })
                .Append(BoundaryXiahouDunChoice(f, "store", 0, $"扣置已选的{r.Selected.Count}张手牌")).ToArray();
        if (r.Stage == BoundaryXiahouDunStage.Distributing)
            return BoundaryXiahouDunHeld(f).SelectMany(card => _players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat).Select(p =>
                BoundaryXiahouDunChoice(f, "give", card.Id, $"将【{card.DisplayName}】交给 {p.Name}", [card.Id], [p.Seat]))).ToArray();
        return [];
    }
    private void PublishBoundaryXiahouDun(ProgramSkillFrame f)
    {
        var choices = BoundaryXiahouDunChoices(f); var skill = _contentRegistry.GetSkill(f.SkillId);
        if (choices.Count == 0) throw new InvalidOperationException("Qingjian has no real available distribution recipient.");
        ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, f.BoundaryXiahouDun!.Stage == BoundaryXiahouDunStage.Selecting ? "清俭：选择任意手牌扣置，最后确认。" : "清俭：将剩余扣置牌全部分配给其他角色。",
            choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        { PromptId = CreatePromptId(), IsPrivate = true, Choices = choices, SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description) };
        _status = _players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running; AdvanceRulesAndPublishState();
    }
    [ProgramAnswer(BoundaryXiahouDunAction)]
    private void AnswerBoundaryXiahouDun(PromptChoice selected)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { BoundaryXiahouDun: { } r } f || !ValidBoundaryXiahouDun(f) ||
            !BoundaryXiahouDunChoices(f).Any(c => AssistedChoicesEqual([c], [selected])) ||
            r.Stage == BoundaryXiahouDunStage.Selecting && !HasRuntimeSkillInstance(_players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            throw new InvalidOperationException("Qingjian's exact private selection or issued distribution changed.");
        ClearPendingDecision(); ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        if (selected.Parameters["branch"] == "toggle")
        {
            var card = BoundaryXiahouDunHandCard(f, selected);
            ReplaceRuntimeTop(f = f with { BoundaryXiahouDun = r with { Selected = r.Selected.Contains(card.Id) ? r.Selected.Where(id => id != card.Id).ToArray() : r.Selected.Append(card.Id).ToArray() } });
            PublishBoundaryXiahouDun(f); return;
        }
        if (selected.Parameters["branch"] == "store") { StoreBoundaryXiahouDun(f); return; }
        if (selected.Parameters["branch"] != "give") throw new InvalidOperationException("Unknown Qingjian answer.");
        var entity = GetAdvancedCard(selected.Cards.Single()); var from = _cardZones.GetLocation(entity.Id); var target = selected.Targets.Single(); var before = _movementSequence;
        ReplaceRuntimeTop(f = f with { BoundaryXiahouDun = r with { Stage = BoundaryXiahouDunStage.GiveChildren, Target = target,
            Payment = [new(entity.Id, entity.Kind, from)], Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        MoveProgramCardsFromMultipleSources([entity.Id], CardLocation.Hand(target), new(BoundaryXiahouDunGiveReason), disclosure: CardMovementDisclosure.FaceDown);
        var actual = _cardMovements.Count(m => m.Sequence > before && m.CardId == entity.Id && m.From == from && m.To == CardLocation.Hand(target) && m.Reason.Value == BoundaryXiahouDunGiveReason);
        f = GetActiveProgramFrame(f.Id); ReplaceRuntimeTop(f = f with { BoundaryXiahouDun = f.BoundaryXiahouDun! with { After = _movementSequence, Given = r.Given + actual } });
        AdvanceEventRulesAndQueueFact(new BoundaryXiahouDunGivenEvent(f.Id, f.OwnerSeat, target, actual, before, _movementSequence)); ContinueBoundaryXiahouDun(f.Id);
    }
    private void StoreBoundaryXiahouDun(ProgramSkillFrame f)
    {
        var r = f.BoundaryXiahouDun!;
        if (r.Selected.Count == 0) { FinishBoundaryXiahouDun(f); return; }
        var available = BoundaryXiahouDunHandChoices(f).Select(c => BoundaryXiahouDunHandCard(f, c).Id).ToHashSet();
        if (r.Selected.Any(id => !available.Contains(id))) throw new InvalidOperationException("Qingjian cannot debit foreign or stale selected hand cards.");
        var before = _movementSequence; var location = r.Location!.Value; _cardZones.EnsurePrivatePersistentPile(location);
        ReplaceRuntimeTop(f = f with { BoundaryXiahouDun = r with { Stage = BoundaryXiahouDunStage.StoreChildren,
            Payment = r.Selected.Select(id => new BoundaryXiahouDunMaterial(id, GetAdvancedCard(id).Kind, CardLocation.Hand(f.OwnerSeat))).ToArray(), Before = before, After = before },
            PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        MoveProgramCardsFromMultipleSources(r.Selected, location, new(BoundaryXiahouDunStoreReason), disclosure: CardMovementDisclosure.FaceDown);
        f = GetActiveProgramFrame(f.Id); ReplaceRuntimeTop(f = f with { BoundaryXiahouDun = f.BoundaryXiahouDun! with { After = _movementSequence } });
        var count = _cardZones.CardsAt(location).Count;
        if (count > 0) AdvanceEventRulesAndQueueFact(new BoundaryXiahouDunStoredEvent(f.Id, r.Source, r.Hash, location, count, before, _movementSequence));
        ContinueBoundaryXiahouDun(f.Id);
    }
    private bool DrainBoundaryXiahouDun(ProgramSkillFrame f) => TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    private void ContinueBoundaryXiahouDun(long id)
    {
        if (_pendingDecision is not null || _resolutionStack.LastOrDefault() is not ProgramSkillFrame { BoundaryXiahouDun: { } r } f || f.Id != id) return;
        if (!ValidBoundaryXiahouDun(f)) throw new InvalidOperationException("Qingjian lost its original receipt or paid movement child.");
        if (r.Stage == BoundaryXiahouDunStage.Drawing) return;
        if (r.Stage is BoundaryXiahouDunStage.StoreChildren or BoundaryXiahouDunStage.GiveChildren or BoundaryXiahouDunStage.DrawChildren)
        {
            if (DrainBoundaryXiahouDun(f)) return; f = GetActiveProgramFrame(id); r = f.BoundaryXiahouDun!;
            ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
            if (r.Stage != BoundaryXiahouDunStage.GiveChildren) { FinishBoundaryXiahouDun(f); return; }
            ReplaceRuntimeTop(f = f with { BoundaryXiahouDun = r = r with { Stage = BoundaryXiahouDunStage.Distributing, Payment = [], Target = -1 } });
        }
        if (!_players[f.OwnerSeat].IsAlive || _winner != Winner.None) { FinishBoundaryXiahouDun(f); return; }
        if (r.Stage == BoundaryXiahouDunStage.Distributing && !BoundaryXiahouDunHeld(f).Any())
        {
            if (r.Given <= 1) { FinishBoundaryXiahouDun(f); return; }
            ReplaceRuntimeTop(f = f with { BoundaryXiahouDun = r with { Stage = BoundaryXiahouDunStage.Drawing } });
            BeginOperationProgramDraw(f, f.OwnerSeat, 1, new("skill.boundary-xiahou-dun.qingjian.reward")); return;
        }
        PublishBoundaryXiahouDun(f);
    }
    [ProgramNativeDrawReturn(SkillProgramEffectOp.DistributeBoundaryXiahouDunQingjian)]
    private void ReturnBoundaryXiahouDunDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        if (f.BoundaryXiahouDun is not { Stage: BoundaryXiahouDunStage.Drawing, Given: > 1, Draw: null } r || !ValidBoundaryXiahouDun(f) || outcome.RecipientSeat != f.OwnerSeat || outcome.RequestedCount != 1)
            throw new InvalidOperationException("Qingjian reward must return to its original fully distributed frame.");
        ReplaceRuntimeTop(f = f with { BoundaryXiahouDun = r with { Stage = BoundaryXiahouDunStage.DrawChildren, Draw = outcome, Before = outcome.SequenceBefore, After = outcome.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        AdvanceEventRulesAndQueueFact(new BoundaryXiahouDunRewardEvent(f.Id, f.OwnerSeat, r.Given, outcome.AttemptFrameId, outcome.ActualDrawIds.Count, outcome.SequenceBefore, outcome.SequenceAfter));
        ContinueBoundaryXiahouDun(f.Id);
    }
    private void FinishBoundaryXiahouDun(ProgramSkillFrame f)
    { ReplaceRuntimeTop(f = f with { BoundaryXiahouDun = null, PendingMovementContinuation = null }); FinishProgramSkill(f, true); }
    [ProgramAiChoice(SkillProgramEffectOp.StoreBoundaryXiahouDunQingjian, SkillProgramEffectOp.DistributeBoundaryXiahouDunQingjian)]
    private PromptChoice AiBoundaryXiahouDun(PendingDecision prompt, ProgramSkillFrame f) => f.BoundaryXiahouDun is { Stage: BoundaryXiahouDunStage.Selecting, Selected.Count: > 0 }
        ? prompt.Choices.Last() : prompt.Choices.First();
}
