using System.Globalization;

namespace CardGame.Core;

public sealed partial class GameEngine
{
    private SkillProgramStepOutcome StartSourceXingxia(ProgramSkillFrame supplied)
    {
        var f = GetActiveProgramFrame(supplied.Id);
        if (f.SelectedTargetSeats is not [var friend] || !_players[friend].IsAlive || !OtherRevealedPublicTeamAlly(_players[f.OwnerSeat], _players[friend]))
            throw new InvalidOperationException("Source1117 requires its selected living other public-team ally.");
        ReplaceRuntimeTop(f = f with { SourceXingxia = new(_roundNumber, friend, [], 0, SourceXingxiaStage.FriendlyDamage) });
        AdvanceEventRulesAndQueueFact(new SourceXingxiaIssuedEvent(f.Id, f.OwnerSeat, f.SkillInstanceId, _roundNumber, friend));
        BeginProgramSkillDamage(f, friend, 2, nature: DamageNature.Fire); ContinueSourceBeigong(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }
    private IReadOnlyList<(Card Card, CardLocation From)> SourceXingxiaRedCards(int payer) =>
        new[] { CardZoneKind.Hand, CardZoneKind.Equipment }.SelectMany(zone => _cardZones.CardsAt(new(zone, payer))
            .Where(c => GetProgramEffectiveSuit(_players[payer], c) is Suit.Heart or Suit.Diamond &&
                !c.IsGeneralWeapon && !IsForeignEquipmentDiscardPrevented(payer, c, new(zone, payer), OwnedCardMoveIntent.Discard) &&
                !IsSelfHandCategoryDiscardForbidden(payer, c, new(zone, payer), OwnedCardMoveIntent.Discard))
            .Select(c => (c, new CardLocation(zone, payer)))).ToArray();
    private void ContinueSourceXingxia(ProgramSkillFrame f)
    {
        var r = f.SourceXingxia!;
        if (r.Stage == SourceXingxiaStage.FriendlyDamage)
        {
            var enemies = Array.AsReadOnly(_players.Where(p => p.IsAlive && PublicTeamEnemy(f.OwnerSeat, p)).Select(p => p.Seat).Order().ToArray());
            ReplaceRuntimeTop(f = f with { SourceXingxia = r = r with { Enemies = enemies, Index = 0, Stage = SourceXingxiaStage.EnemyChoice } });
        }
        else if (r.Stage is SourceXingxiaStage.PaymentChildren or SourceXingxiaStage.EnemyDamage)
            ReplaceRuntimeTop(f = f with { SourceXingxia = r = r with { Index = r.Index + 1, Stage = SourceXingxiaStage.EnemyChoice } });
        while (r.Index < r.Enemies.Count && !_players[r.Enemies[r.Index]].IsAlive)
            ReplaceRuntimeTop(f = f with { SourceXingxia = r = r with { Index = r.Index + 1 } });
        if (r.Index >= r.Enemies.Count || _winner != Winner.None) { FinishSourceBeigong(f); return; }
        var payer = r.Enemies[r.Index];
        var choices = SourceXingxiaRedCards(payer).Select(p => new PromptChoice(new ChoiceId($"source-xingxia.{f.Id}.{r.Index}.{p.Card.Id}"),
            $"弃置【{p.Card.DisplayName}】", [p.Card.Id], [], new Dictionary<string, string> { ["program-action"] = "source-xingxia-red", ["branch"] = "discard" })).ToList();
        choices.Add(new(new ChoiceId($"source-xingxia.{f.Id}.{r.Index}.fire"), "受到1点火焰伤害", [], [],
            new Dictionary<string, string> { ["program-action"] = "source-xingxia-red", ["branch"] = "fire" }));
        var skill = _contentRegistry.GetSkill(f.SkillId);
        ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _pendingDecision = new(DecisionKind.ProgramTrigger, payer, "行夏：弃置一张红色手牌或装备，或受到1点火焰伤害。",
            choices.SelectMany(c => c.Cards).ToArray(), [], f.OwnerSeat)
        { PromptId = CreatePromptId(), IsPrivate = true, Choices = Array.AsReadOnly(choices.ToArray()), SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description) };
        _status = _players[payer].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }
    [ProgramAnswer("source-xingxia-red")]
    private void AnswerSourceXingxiaRed(PromptChoice selected)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { SourceXingxia: { Stage: SourceXingxiaStage.EnemyChoice } r } f ||
            r.Index < 0 || r.Index >= r.Enemies.Count || _pendingDecision?.PlayerSeat != r.Enemies[r.Index])
            throw new InvalidOperationException("Source1117 lost its current original enemy choice.");
        var payer = r.Enemies[r.Index]; ClearPendingDecision(); ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        if (selected.Parameters.GetValueOrDefault("branch") == "fire")
        {
            ReplaceRuntimeTop(f = f with { SourceXingxia = r with { Stage = SourceXingxiaStage.EnemyDamage } });
            BeginProgramSkillDamage(f, payer, 1, nature: DamageNature.Fire); ContinueSourceBeigong(f.Id); return;
        }
        var id = selected.Cards.Single();
        if (!SourceXingxiaRedCards(payer).Any(p => p.Card.Id == id)) throw new InvalidOperationException("Source1117 cannot discard a missing or non-red original entity.");
        var reason = new CardMoveReason("skill-program.source-xingxia.discard"); var before = _movementSequence;
        ReplaceRuntimeTop(f = f with { SourceXingxia = r with { Stage = SourceXingxiaStage.PaymentChildren, PaymentCardId = id, Before = before, After = before },
            PendingMovementContinuation = new(payer, 0, null) });
        var logical = CaptureNativeLogicalDiscardInvocation(payer, payer, [id], reason, f.Id);
        MoveProgramCardsFromMultipleSources([id], CardLocation.DiscardPile, reason, (_, moves) =>
        {
            var current = GetActiveProgramFrame(f.Id);
            ReplaceRuntimeTop(current with { SourceXingxia = current.SourceXingxia! with { After = _movementSequence } });
            AdvanceEventRulesAndQueueFact(new SourceXingxiaPaymentEvent(f.Id, f.OwnerSeat, payer, id, _movementSequence));
        });
        CompleteNativeLogicalDiscardInvocation(logical); ContinueSourceBeigong(f.Id);
    }
    [ProgramAiChoice(SkillProgramEffectOp.ResolveSourceXingxia)]
    private PromptChoice SourceXingxiaAiChoice(PendingDecision p, ProgramSkillFrame f) =>
        p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "discard") ?? p.Choices.Single(c => c.Parameters.GetValueOrDefault("branch") == "fire");
}
