using CardGame.Core;
using static BoundaryYuFanScenario;

namespace CardGame.Core.Tests;

internal static class BoundaryYuFanMobileChecks
{
    public static void ActiveZongxuanDrawsBeforeChoosingAnOriginalHandOrEquipmentCardCold()
    {
        foreach (var equipment in new[] { false, true })
        {
            var (registry, game) = CreateScene();
            var initial = game.CreateSnapshot(0).Players[0];
            var selected = equipment ? initial.Equipment.Single().Id : initial.Hand.Single().Id;
            var from = equipment ? CardLocation.Equipment(0) : CardLocation.Hand(0);
            var beforeDraw = game.CardMovements.Select(move => move.Sequence).DefaultIfEmpty().Max();
            BeginZongxuan(game); Reach(game, TopChoice);
            var frame = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(item => item.SkillId == Zongxuan);
            Need(game.CreateSnapshot(0).Players[0].HandCount == initial.HandCount + 1 &&
                game.CreateSnapshot(0).Players[0].Hand.Any(card => card.Kind == CardKind.Peach) &&
                Decision(game)!.Choices.Any(choice => choice.Cards.SequenceEqual([selected])) &&
                game.CardMovements.All(move => move.CardId != selected || move.From != from || move.To != CardLocation.DrawPile) &&
                game.CardMovements.Count(move => move.Sequence > beforeDraw && move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0) &&
                    move.Reason.Value == $"skill-program.{Zongxuan}.{SkillProgramEffectOp.Draw}") == 1,
                "The real native Draw1 completes before choosing a still-unmoved original hand or equipment card; the choice is not restricted to the newly drawn entity.");
            if (equipment)
                game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
            Need(game.ResolutionStack.OfType<ProgramSkillFrame>().Single(item => item.SkillId == Zongxuan).Id == frame.Id,
                "The owned-card choice continues on the original paying program frame, including after cold recovery.");
            Answer(game, choice => choice.Cards.SequenceEqual([selected])); Play(game);
            Need(game.CardMovements.Count(move => move.CardId == selected && move.From == from && move.To == CardLocation.DrawPile) == 1 &&
                game.CardMovements.Count(move => move.Sequence > beforeDraw && move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0) &&
                    move.Reason.Value == $"skill-program.{Zongxuan}.{SkillProgramEffectOp.Draw}") == 1,
                "Confirmation after representative cold recovery places the exact original selected entity once and does not repeat the paid native draw.");
            var twice = game.Submit(new UseProgramSkillCommand(0, Zongxuan, "draw-then-top", [], [], game.Revision, Decision(game)!.PromptId));
            Need(!twice.Accepted, "The mobile active branch is once per actual play phase.");
            var beforeNextDraw = game.CardMovements.Select(move => move.Sequence).DefaultIfEmpty().Max();
            DrawOne(game); Play(game);
            Need(game.CreateSnapshot(0).Players[0].Hand.Any(card => card.Id == selected) &&
                game.CardMovements.Count(move => move.Sequence > beforeNextDraw && move.CardId == selected && move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0)) == 1,
                "The next native draw receives the exact card placed at the real draw-pile top, including a card originally taken from equipment.");
        }
    }

    public static void MixedDiscardBatchOffersOnlyTheConfiguredOwnedSourceZonesCold()
    {
        var (registry, game) = CreateScene(withJudgmentCard: true);
        var hand = game.CreateSnapshot(0).Players[0].Hand.Single(card => card.Kind == CardKind.Dodge).Id;
        var lightning = game.CreateSnapshot(0).Players[0].Hand.Single(card => card.Kind == CardKind.Lightning).Id;
        var lightningUse = game.GetHumanLegalActions().Single(action => action.CardId == lightning);
        Accept(game, new PlayCardCommand(0, lightning, lightningUse.TargetSeats, game.Revision, Decision(game)!.PromptId, lightningUse.PlayedCardKind, lightningUse.TargetCardId)); Play(game);
        Need(game.CreateSnapshot(0).Players[0].Judgment.Single().Id == lightning,
            "A real native Lightning use places its original entity in the actual judgment zone before the regression's mixed discard.");
        DiscardMixed(game);
        Reach(game, prompt => prompt?.SkillPrompt?.SkillId == Driver && prompt.Choices.Any(choice => choice.Cards.SequenceEqual([hand])));
        Answer(game, choice => choice.Cards.SequenceEqual([hand]));
        Answer(game, choice => choice.Cards.SequenceEqual([lightning]));
        Reach(game, OptionalZongxuan);
        var window = game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Single(frame => frame.Candidates.Any(candidate => candidate.SkillId == Zongxuan));
        Need(window.Batch.Movements.Select(move => move.From.Zone).Order().SequenceEqual(new[] { CardZoneKind.Hand, CardZoneKind.Judgment }.Order()),
            "The original native movement batch retains both actual HE and judgment origins; the ability must filter candidates without rewriting that batch.");
        Answer(game, choice => choice.Parameters.GetValueOrDefault("program-action") == "activate"); Reach(game, DiscardTopChoice);
        Need(Decision(game)!.Choices.Where(choice => choice.Cards.Count > 0).SelectMany(choice => choice.Cards).SequenceEqual([hand]),
            "The mobile HE-only trigger offers the original hand discard while excluding the judgment entity from the same frozen native batch.");
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Answer(game, choice => choice.Cards.SequenceEqual([hand]));
        Answer(game, choice => choice.Parameters.GetValueOrDefault("program-action") == "discard-top-finish"); Play(game);
        Need(game.CardMovements.Count(move => move.CardId == hand && move.From == CardLocation.DiscardPile && move.To == CardLocation.DrawPile) == 1 &&
            game.CardMovements.All(move => move.CardId != lightning || move.From != CardLocation.DiscardPile || move.To != CardLocation.DrawPile) &&
            game.CardMovements.Count(move => move.CardId == lightning && move.From == CardLocation.Judgment(0) && move.To == CardLocation.DiscardPile) == 1,
            "Cold confirmation reclaims only the eligible original hand entity once; the original judgment discard remains discarded and its paid movement is not repeated.");
    }

    private static void Need(bool value, string reason)
    { if (!value) throw new InvalidOperationException(reason); }
}
