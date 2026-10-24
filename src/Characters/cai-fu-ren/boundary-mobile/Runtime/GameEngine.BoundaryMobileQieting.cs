using System.Globalization;

namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string BoundaryMobileQietingSkill = "boundary:qieting-mobile", BoundaryMobileQietingHandReason = "skill-program.cai-fu-ren.qieting.hand";
    private static bool IsBoundaryMobileQieting(ProgramSkillFrame frame) => frame.SkillId == BoundaryMobileQietingSkill;
    private static bool IsKnownCaiFuRenQietingBinding(string skill, string? binding) => skill == "ol:qieting" && binding == "ordinary-turn-end" || skill == BoundaryMobileQietingSkill && binding == "mobile-turn-end";
    private static bool IsCaiFuRenQietingOperation(ProgramSkillFrame frame, SkillProgramEffect? effect) => effect?.Op ==
        (IsBoundaryMobileQieting(frame) ? SkillProgramEffectOp.ResolveBoundaryMobileQieting : SkillProgramEffectOp.ResolveOrdinaryCaiFuRenQieting);
    private bool CaiFuRenQietingDisqualified(string skill, OrdinaryQietingTurnContext qualification)
    {
        if (skill != BoundaryMobileQietingSkill) return qualification.UsedOnAnother || OrdinaryQietingUsedOnAnother(qualification);
        var history = CompleteProgramEventHistory().Take(checked((int)qualification.HistoryCutoff)).ToArray();
        var end = Array.FindLastIndex(history, entry => entry is TurnEndedEvent ended && ended.TurnNumber == qualification.ActualTurn && ended.ActorSeat == qualification.TurnOwnerSeat);
        var start = end < 0 ? -1 : Array.FindLastIndex(history, end, entry => entry is TurnStartedEvent begun && begun.TurnNumber == qualification.ActualTurn && begun.ActorSeat == qualification.TurnOwnerSeat);
        return start < 0 || end <= start || history.Skip(start + 1).Take(end - start - 1).OfType<DamageAppliedEvent>().Any(damage =>
            !damage.SourceLess && damage.Amount > 0 && damage.SourceSeat == qualification.TurnOwnerSeat && damage.TargetSeat != qualification.TurnOwnerSeat);
    }
    [ProgramCapability]
    private sealed class BoundaryMobileQietingCapability(GameEngine engine) : IBoundaryMobileQietingHost
    { public SkillProgramStepOutcome Begin(ProgramSkillFrame frame) => engine.BeginOrdinaryQieting(frame); }

    private IReadOnlyList<PromptChoice> BoundaryMobileQietingHandChoices(ProgramSkillFrame frame) => frame.OrdinaryQieting is { Paid: true, Option: "hand", Stage: OrdinaryQietingStage.Viewing, MobileSelection: { } selection } receipt
        ? Array.AsReadOnly(selection.Viewed.Where(id => _cardZones.GetLocation(id) == CardLocation.Hand(receipt.Qualification.TurnOwnerSeat)).Select(id => new PromptChoice(
            new($"ordinary-qieting.{frame.Id}.hand.{id}"), $"获得【{GetAdvancedCard(id).DisplayName}】", [id], [frame.OwnerSeat], new Dictionary<string, string>
            { ["program-action"] = "ordinary-qieting", ["frame-id"] = frame.Id.ToString(CultureInfo.InvariantCulture), ["option"] = "take-hand",
                ["source-zone"] = "Hand", ["card-owner-seat"] = receipt.Qualification.TurnOwnerSeat.ToString(CultureInfo.InvariantCulture) })).ToArray()) : [];

    private void AnswerBoundaryMobileQieting(ProgramSkillFrame frame, PromptChoice choice)
    {
        AssertOrdinaryQieting(frame);
        if (_pendingDecision?.PlayerSeat != frame.OwnerSeat || !OrdinaryQietingChoices(frame).Any(candidate => AssistedChoicesEqual([candidate], [choice])) ||
            !HasRuntimeSkillInstance(_players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId)) throw new InvalidOperationException("Mobile Qieting requires the current original single option or one viewed entity.");
        var receipt = frame.OrdinaryQieting!; var option = choice.Parameters["option"]; var before = _movementSequence; ClearPendingDecision();
        if (receipt.Stage == OrdinaryQietingStage.Viewing)
        {
            if (option != "take-hand" || choice.Cards is not [var id] || receipt.MobileSelection is not { } selection || !selection.Viewed.Contains(id) || _cardZones.GetLocation(id) != CardLocation.Hand(receipt.Qualification.TurnOwnerSeat))
                throw new InvalidOperationException("Mobile Qieting cannot obtain any card outside its original two-card private view.");
            var card = GetAdvancedCard(id);
            ReplaceRuntimeTop(frame = frame with { Step = ResolutionFrameStep.ResolvingEffect, OrdinaryQieting = receipt with { Stage = OrdinaryQietingStage.MovementChildren, CardId = id, PrintedKind = card.Kind, Before = before, After = before }, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
            MoveCard(card, CardLocation.Hand(receipt.Qualification.TurnOwnerSeat), CardLocation.Hand(frame.OwnerSeat), new(BoundaryMobileQietingHandReason));
            frame = GetActiveProgramFrame(frame.Id); ReplaceRuntimeTop(frame = frame with { OrdinaryQieting = frame.OrdinaryQieting! with { After = _movementSequence } });
            ContinueOrdinaryQieting(frame.Id); return;
        }
        if (receipt.Paid || receipt.Stage != OrdinaryQietingStage.Choice) throw new InvalidOperationException("Mobile Qieting cannot issue a second benefit.");
        if (option == "hand")
        {
            var hand = GetHand(_players[receipt.Qualification.TurnOwnerSeat]).Select(card => card.Id).ToArray(); var candidates = hand.ToList(); var viewed = new List<int>(); var randomBefore = _random.State;
            while (viewed.Count < 2 && candidates.Count > 0) { var index = _random.Next(candidates.Count); viewed.Add(candidates[index]); candidates.RemoveAt(index); }
            ReplaceRuntimeTop(frame = frame with { OrdinaryQieting = receipt with { Paid = true, Option = "hand", Stage = OrdinaryQietingStage.Viewing,
                MobileSelection = new(randomBefore, _random.State) { Hand = hand, Viewed = viewed } } });
            AdvanceEventRulesAndQueueFact(new OrdinaryQietingOptionIssuedEvent(frame.Id, "hand", null, null, before));
            AdvanceEventRulesAndQueueFact(new BoundaryMobileQietingViewedEvent(frame.Id, frame.OwnerSeat, receipt.Qualification.TurnOwnerSeat, viewed.Count));
            PublishOrdinaryQieting(frame); return;
        }
        if (option == "draw")
        {
            ReplaceRuntimeTop(frame = frame with { Step = ResolutionFrameStep.ResolvingEffect, OrdinaryQieting = receipt with { Paid = true, Option = "draw", Stage = OrdinaryQietingStage.Draw, Before = before, After = before } });
            AdvanceEventRulesAndQueueFact(new OrdinaryQietingOptionIssuedEvent(frame.Id, "draw", null, null, before)); ContinueOrdinaryQieting(frame.Id); return;
        }
        var equipment = GetAdvancedCard(choice.Cards.Single()); if (!CanMoveOrdinaryQietingEquipment(frame, equipment)) throw new InvalidOperationException("Mobile Qieting lost its real current equipment.");
        var slot = FieldEquipmentSlot(equipment); var current = GetEquipment(_players[frame.OwnerSeat]).Where(card => FieldEquipmentSlot(card) == slot).ToArray();
        var replaced = current.Length >= _players[frame.OwnerSeat].EquipmentSlotCapacity(slot) ? current[0] : null;
        ReplaceRuntimeTop(frame = frame with { Step = ResolutionFrameStep.ResolvingEffect, OrdinaryQieting = receipt with { Paid = true, Option = "equipment", Stage = OrdinaryQietingStage.MovementChildren,
            CardId = equipment.Id, PrintedKind = equipment.Kind, Before = before, After = before,
            MobileSelection = new(0, 0) { ReplacedCardId = replaced?.Id, ReplacedKind = replaced?.Kind },
            GrainIds = equipment.Kind == CardKind.WoodenOx ? GetWoodenOxGrain(_players[receipt.Qualification.TurnOwnerSeat]).Select(card => card.Id).ToArray() : [] }, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        AdvanceEventRulesAndQueueFact(new OrdinaryQietingOptionIssuedEvent(frame.Id, "equipment", equipment.Id, equipment.Kind, before));
        if (replaced is not null) { CopyFirstReplacedWeapon(equipment, replaced); MoveCard(replaced, CardLocation.Equipment(frame.OwnerSeat), CardLocation.DiscardPile, CardMoveReasons.EquipmentReplace); }
        var from = CardLocation.Equipment(receipt.Qualification.TurnOwnerSeat);
        MoveCard(equipment, from, CardLocation.Equipment(frame.OwnerSeat), new(OrdinaryQietingMoveReason), movement =>
        {
            var active = GetActiveProgramFrame(frame.Id); ReplaceRuntimeTop(active with { OrdinaryQieting = active.OrdinaryQieting! with { After = movement.Sequence } });
            AdvanceEventRulesAndQueueFact(new OrdinaryQietingEquipmentMovedEvent(frame.Id, equipment.Id, equipment.Kind, from, movement.To, movement.Sequence));
        });
        frame = GetActiveProgramFrame(frame.Id); ReplaceRuntimeTop(frame = frame with { OrdinaryQieting = frame.OrdinaryQieting! with { After = _movementSequence } });
        AdvanceEventRulesAndQueueFact(new EquipmentChangedEvent(frame.Id, frame.OwnerSeat, slot, equipment.Id, equipment.Kind, replaced?.Id)); ContinueOrdinaryQieting(frame.Id);
    }
    [ProgramViewContributor(ProgramViewContribution.PrivateCards, 56600)]
    private CardSnapshot[] BoundaryMobileQietingPrivateCards(int viewer) => _resolutionStack.OfType<ProgramSkillFrame>().Where(frame => frame.OwnerSeat == viewer && IsBoundaryMobileQieting(frame) && frame.OrdinaryQieting is { Stage: OrdinaryQietingStage.Viewing, MobileSelection: not null })
        .SelectMany(frame => frame.OrdinaryQieting!.MobileSelection!.Viewed.Where(id => _cardZones.GetLocation(id) == CardLocation.Hand(frame.OrdinaryQieting.Qualification.TurnOwnerSeat)).Select(id => ProjectHandCard(_players[frame.OrdinaryQieting.Qualification.TurnOwnerSeat], GetAdvancedCard(id)))).ToArray();
    [ProgramAiChoice(SkillProgramEffectOp.ResolveBoundaryMobileQieting)]
    private PromptChoice ChooseBoundaryMobileQietingAi(PendingDecision decision, ProgramSkillFrame frame) => ChooseOrdinaryQietingAi(decision, frame);
}
