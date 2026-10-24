using System.Text.Json;
using CardGame.Core;
using static BoundaryCaiFuRenScenario;

internal static class BoundaryCaiFuRenMobileChecks
{
    public static void ActualTurnDamageAndTwoCardPrivateTakeCold()
    {
        var (registry, game) = CreateScene(); End(game); Reach(game, prompt => prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 1 });
        var ironChain = EnsureCard(game, CardKind.IronChain);
        PlayCard(game, ironChain, [2]); Reach(game, prompt => prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 1 }); End(game); Reach(game, Optional);
        Require(game.Events.Select(entry => entry.Payload).OfType<CardUseDeclaredEvent>().Any(entry => entry.SourceSeat == 1 && entry.CardKind == CardKind.IronChain),
            "The foreign actual turn really used a harmless card on another person; mobile Qieting must still qualify.");
        Activate(game); Answer(game, choice => choice.Parameters.GetValueOrDefault("option") == "hand");
        var root = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Listen); var selection = root.OrdinaryQieting!.MobileSelection!;
        var original = selection.Viewed.ToArray(); var beforeHand = Player(game, 0).HandCount;
        Require(original.Length == 2 && selection.Hand.Count >= 2 && original.All(id => game.CreateSnapshot(0).PrivateRevealedCards!.Any(card => card.Id == id)) &&
            Enumerable.Range(1, 3).All(viewer => !(game.CreateSnapshot(viewer).PrivateRevealedCards?.Any(card => original.Contains(card.Id)) ?? false)) &&
            !game.CreateSnapshot(0).PrivateRevealedCards!.Any(card => selection.Hand.Except(original).Contains(card.Id)),
            "Only Cai Fu Ren sees exactly the original two sampled real hand cards, rather than the holder's entire hand or any third-party view.");
        Cold(ref game, registry);
        var taken = original[0]; Answer(game, choice => choice.Cards.SequenceEqual([taken])); Reach(game, prompt => prompt?.SkillPrompt?.SkillId == Watch);
        root = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == root.Id);
        Require(root.OrdinaryQieting is { Paid: true, Option: "hand", Stage: OrdinaryQietingStage.MovementChildren, CardId: var paid } && paid == taken &&
            Player(game, 0).HandCount == beforeHand + 1 && Player(game, 0).Hand.Any(card => card.Id == taken) && !Player(game, 1).Hand.Any(card => card.Id == taken),
            "The chosen original entity moves before its real gained-card child; the already-issued hand benefit cannot become a second option.");
        Cold(ref game, registry); Continue(game); Reach(game, prompt => prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 2 });
        Require(game.CardMovements.Count(move => move.CardId == taken && move.From == CardLocation.Hand(1) && move.To == CardLocation.Hand(0) && move.Reason.Value == HandReason) == 1 &&
            game.Events.Select(entry => entry.Payload).OfType<OrdinaryQietingOptionIssuedEvent>().Count(entry => entry.FrameId == root.Id) == 1,
            "Cold recovery returns to the original ended-turn frame without resampling, repaying or offering another mobile Qieting option.");
        var peerHp = Player(game, 2).Hp; Use(game, "self-damage"); Reach(game, prompt => prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 2 });
        Require(Player(game, 2).Hp == peerHp - 1, "The next actual turn really caused one point of self damage.");
        End(game); Reach(game, Optional); Activate(game); Answer(game, choice => choice.Parameters.GetValueOrDefault("option") == "draw");
        Reach(game, prompt => prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 3 });
        var targetHp = Player(game, 2).Hp; Use(game, "other-damage", [2]); Reach(game, prompt => prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 3 });
        Require(Player(game, 2).Hp == targetHp - 1, "The third foreign actual turn really damaged another living role.");
        var offered = game.Events.Select(entry => entry.Payload).OfType<OrdinaryQietingStartedEvent>().Count(); End(game);
        Reach(game, prompt => Optional(prompt) || prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        Require(!Optional(Decision(game)) && game.Events.Select(entry => entry.Payload).OfType<OrdinaryQietingStartedEvent>().Count() == offered,
            "Self damage preserves mobile Qieting eligibility, but actual positive damage to another person blocks it for that entire actual turn.");
    }

    public static void EquipmentReplacementAndOriginalMovementCold()
    {
        var (registry, game) = CreateScene(); var old = EnsureCard(game, CardKind.Crossbow); PlayCard(game, old);
        Reach(game, prompt => prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }); End(game); Reach(game, prompt => prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 1 });
        var incoming = EnsureCard(game, CardKind.Crossbow); PlayCard(game, incoming); Reach(game, prompt => prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 1 });
        End(game); Reach(game, Optional); Activate(game);
        Require(Decision(game)!.Choices.Any(choice => choice.Cards.SequenceEqual([incoming])), "An occupied legal slot still admits mobile Qieting's actual foreign equipment.");
        Answer(game, choice => choice.Cards.SequenceEqual([incoming])); Reach(game, prompt => prompt?.SkillPrompt?.SkillId == Watch);
        var root = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Listen);
        Require(root.OrdinaryQieting is { Paid: true, Option: "equipment", Stage: OrdinaryQietingStage.MovementChildren, MobileSelection.ReplacedCardId: var replaced } && replaced == old &&
            Player(game, 0).Equipment.Any(card => card.Id == incoming) && Player(game, 0).Equipment.All(card => card.Id != old) && Player(game, 1).Equipment.All(card => card.Id != incoming),
            "The original foreign equipment replaces the owner's actual same-slot equipment before the native loss child returns.");
        Cold(ref game, registry); Continue(game); Reach(game, prompt => prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 2 });
        Require(game.CardMovements.Count(move => move.CardId == old && move.From == CardLocation.Equipment(0) && move.To == CardLocation.DiscardPile && move.Reason == CardMoveReasons.EquipmentReplace) == 1 &&
            game.CardMovements.Count(move => move.CardId == incoming && move.From == CardLocation.Equipment(1) && move.To == CardLocation.Equipment(0) && move.Reason.Value == EquipmentReason) == 1 &&
            game.Events.Select(entry => entry.Payload).OfType<OrdinaryQietingCompletedEvent>().Count(entry => entry.FrameId == root.Id && entry.Paid) == 1,
            "The paid slot replacement drains its original movement child exactly once after cold recovery.");
    }

    private static int EnsureCard(GameEngine game, CardKind kind)
    {
        for (var attempt = 0; attempt < 8; attempt++)
        {
            var actor = Decision(game)!.PlayerSeat; if (Player(game, actor).Hand.FirstOrDefault(card => card.Kind == kind) is { } card) return card.Id;
            Use(game, "refill"); Reach(game, prompt => prompt is { Kind: DecisionKind.PlayCard } && prompt.PlayerSeat == actor);
        }
        throw new InvalidOperationException("The fixed native deck did not provide the necessary physical fixture card.");
    }
    private static void Cold(ref GameEngine game, ContentRegistry registry)
    {
        var source = game;
        var original = Enumerable.Range(0, 4).Select(viewer => SnapshotJson.Serialize(source.CreateSnapshot(viewer))).ToArray();
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(source.CreateCheckpoint())), registry);
        Require(original.SequenceEqual(Enumerable.Range(0, 4).Select(viewer => SnapshotJson.Serialize(restored.CreateSnapshot(viewer)))), "Actual private samples, paid entities, exact parent and all viewer projections survive JSON cold restore.");
        game = restored;
    }
    private static void Require(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
}
