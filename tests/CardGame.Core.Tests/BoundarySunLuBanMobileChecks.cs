using System.Text.Json;
using CardGame.Core;
using static BoundarySunLuBanScenario;

internal static class BoundarySunLuBanMobileChecks
{
    public static void OwnerChoosesHiddenObtainBeforeOriginalActorReplacementCold()
    {
        foreach (var pattern in new[] { "gain", "black-draw" })
        {
            var scene = CreateScene(pattern, withWatch: true); var game = scene.Game;
            var invited = InvitationSeat(game); var originalTarget = GainPrimaryTarget(game);
            var foreign = game.CreateSnapshot(invited).Players[invited].Hand[0].Id;
            var foreignCount = game.CreateSnapshot(invited).Players[invited].HandCount;
            var originalHand = game.CreateSnapshot(0).Players[0].HandCount;
            var targetHp = game.CreateSnapshot(0).Players[originalTarget].Hp;
            var material = UseCard(game, pattern == "gain" ? CardKind.Slash : CardKind.DrawTwo, pattern == "gain" ? [originalTarget] : []);
            BeginZenhui(game); Reach(game, OwnBranch);
            Need(Decision(game) is { PlayerSeat: 0, IsPrivate: true } && Decision(game)!.Choices.Count == 2,
                "Sun Lu Ban herself privately chooses obtain/replace or add-target; the selected player never chooses this branch.");
            Cold(ref game, scene.Registry);
            Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "become-user");
            Reach(game, prompt => prompt?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card") == true);
            Need(Decision(game)!.PlayerSeat == 0 && Decision(game)!.Choices.All(choice => choice.Cards.Count == 0) &&
                game.CreateSnapshot(0).Players[invited].Hand.Count == 0 && game.CreateSnapshot(0).Players[invited].TeamId != game.CreateSnapshot(0).Players[0].TeamId,
                "The owner selects a foreign hidden hand slot without being shown its identity or asking the foreign owner to donate.");
            Answer(game, choice => choice.Parameters.GetValueOrDefault("source-zone") == "Hand" && choice.Parameters.GetValueOrDefault("slot-index") == "0");
            Reach(game, WatchPending);
            var use = game.ResolutionStack.OfType<CardUseFrame>().Single(frame => frame.Action?.PhysicalCards.Any(cost => cost.CardId == material) == true);
            Need(use.SourceSeat == 0 && use.Action!.ActorSeat == 0 && use.Action.ProviderSeat == 0 &&
                game.CreateSnapshot(0).Players[0].Hand.Any(card => card.Id == foreign) &&
                !game.Events.Select(record => record.Payload).OfType<ProgramCardUseActorReplacedEvent>().Any(),
                "The original card still belongs to the original actor while the actually obtained hand card finishes its native gain child.");
            Cold(ref game, scene.Registry); DrainWatchesAndPlay(game);
            var replaced = game.Events.Select(record => record.Payload).OfType<ProgramCardUseActorReplacedEvent>().Single(fact => fact.SkillId == Zenhui);
            Need(replaced.CardUseFrameId == use.Id && replaced.PreviousActorSeat == 0 && replaced.ActorSeat == invited && replaced.ProviderSeat == 0 &&
                game.CardMovements.Count(move => move.CardId == foreign && move.From == CardLocation.Hand(invited) && move.To == CardLocation.Hand(0)) == 1 &&
                game.CardMovements.Count(move => move.CardId == material && move.To == CardLocation.DiscardPile) == 1,
                "The exact paid gain returns once to native actor replacement without repaying or changing the original card provider/material.");
            if (pattern == "gain")
                Need(game.CreateSnapshot(0).Players[originalTarget].Hp == targetHp - 1 && game.Events.Select(record => record.Payload).OfType<DamageAppliedEvent>()
                    .Any(damage => damage.SourceSeat == invited && damage.TargetSeat == originalTarget && damage.Amount == 1),
                    "The retained original Slash damages its old target from its replacement actor.");
            else
                Need(game.CreateSnapshot(0).Players[0].HandCount == originalHand - 1 + 1 + 2 && game.CreateSnapshot(invited).Players[invited].HandCount == foreignCount - 1 &&
                    game.Events.Select(record => record.Payload).OfType<CardsDrawnEvent>().Count(draw => draw.PlayerSeat == 0 && draw.Count == 2) == 1 &&
                    !game.Events.Select(record => record.Payload).OfType<CardsDrawnEvent>().Any(draw => draw.PlayerSeat == invited && draw.Count > 0),
                    "Replacing the actor of a black implicit-self DrawTwo retains the original self recipient; the replacement user does not take that draw.");
        }
    }

    public static void ExtraTargetsKeepBorrowedSwordPairsAndSelfTrickRecipientsCold()
    {
        var scene = CreateScene("borrowed", withWatch: true); var game = scene.Game;
        var material = UseCard(game, CardKind.BorrowedSword, [1, 3]);
        BeginZenhui(game, 2); Reach(game, OwnBranch);
        Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "extra-target");
        Reach(game, prompt => prompt?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "add-compound-card-target") == true);
        Cold(ref game, scene.Registry);
        Answer(game, choice => choice.Targets.SequenceEqual([3]));
        Reach(game, WatchPending);
        Need(game.ResolutionStack.OfType<CardUseFrame>().Any(use => use.CardKind == CardKind.BorrowedSword && use.TargetSeats.SequenceEqual([1, 3, 2, 3]) &&
                use.Action!.EffectiveDesignatedTargetSeats.SequenceEqual([1, 2])),
            "A single designated weapon owner remains one BorrowedSword main target; adding another owner appends its complete native owner/victim pair.");
        DrainWatchesAndPlay(game);
        Need(game.CardMovements.Count(move => move.CardId == material && move.To == CardLocation.DiscardPile) == 1 &&
            game.CreateSnapshot(0).Players[1].Equipment.Count == 0 && game.CreateSnapshot(0).Players[2].Equipment.Count == 0,
            "Both native weapon-owner pairs resolve and the same original borrowed-sword card finishes once.");

        scene = CreateScene("self-chain"); game = scene.Game;
        UseCard(game, CardKind.IronChain, [0]); BeginZenhui(game, 1); Reach(game, OwnBranch);
        Need(Decision(game)!.Choices.All(choice => choice.Parameters.GetValueOrDefault("option-id") != "become-user"),
            "A legal additional target with no obtainable hand/equipment offers only the executable extra-target branch.");
        Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "extra-target"); Play(game);
        Need(game.CreateSnapshot(0).Players[0].IsChained && game.CreateSnapshot(0).Players[1].IsChained,
            "The mobile wording permits a self-targeted black IronChain's sole original target and retains self when adding another legal target.");

        scene = CreateScene("black-draw", withWatch: true); game = scene.Game;
        var invited = InvitationSeat(game);
        var before = game.CreateSnapshot(0).Players.Select(player => player.HandCount).ToArray();
        material = UseCard(game, CardKind.DrawTwo, []); BeginZenhui(game); Reach(game, OwnBranch);
        Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "extra-target");
        Reach(game, WatchPending);
        var original = game.ResolutionStack.OfType<CardUseFrame>().Single(use => use.CardKind == CardKind.DrawTwo);
        Need(original.TargetSeats.SequenceEqual([0, invited]) && original.NativeCardEffectDraw is { RecipientSeat: 0 } &&
            game.CreateSnapshot(0).Players[0].HandCount == before[0] - 1 + 2 && game.CreateSnapshot(0).Players[invited].HandCount == before[invited],
            "The original black DrawTwo keeps its implicit self recipient and its new legal recipient; self's real Draw2 child finishes before the next recipient.");
        Cold(ref game, scene.Registry); DrainWatchesAndPlay(game);
        Need(game.CreateSnapshot(0).Players.Select(player => player.HandCount).SequenceEqual(before.Select((count, seat) => count + (seat == 0 ? 1 : seat == invited ? 2 : 0))) &&
            game.Events.Select(record => record.Payload).OfType<CardsDrawnEvent>().Count(draw => (draw.PlayerSeat == 0 || draw.PlayerSeat == invited) && draw.Count == 2) == 2 &&
            game.CardMovements.Count(move => move.CardId == material && move.To == CardLocation.DiscardPile) == 1,
            "Cold recovery completes exactly one native Draw2 for each committed recipient and settles the original physical trick only once.");
    }

    public static void JiaojinPaysEquipmentThenPreventsWholeMaleDamageCold()
    {
        var scene = CreateScene("defense", withWatch: true); var game = scene.Game;
        var hp = game.CreateSnapshot(0).Players[0].Hp;
        var male = MaleSourceSeat(game); var female = FemaleSourceSeat(game);
        var cost = game.CreateSnapshot(0).Players[0].Hand.Single(card => card.Kind == CardKind.Crossbow).Id;
        Use(game, "damage", [male]);
        Reach(game, prompt => prompt?.SkillPrompt?.SkillId == Jiaojin && prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate"));
        Answer(game, choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        Reach(game, prompt => prompt?.Choices.Any(choice => choice.Cards.SequenceEqual([cost])) == true);
        Answer(game, choice => choice.Cards.SequenceEqual([cost])); Reach(game, WatchPending);
        Need(game.CreateSnapshot(0).Players[0].Hp == hp &&
            game.CardMovements.Count(move => move.CardId == cost && move.From == CardLocation.Hand(0) && move.To == CardLocation.DiscardPile) == 1 &&
            !game.Events.Select(record => record.Payload).OfType<ProgramDamagePreventedEvent>().Any(fact => fact.SkillId == Jiaojin),
            "The original equipment entity is discarded once and its child finishes before the original whole-damage prevention commits.");
        Cold(ref game, scene.Registry); DrainWatchesAndPlay(game);
        Need(game.CreateSnapshot(0).Players[0].Hp == hp && game.Events.Select(record => record.Payload).OfType<ProgramDamagePreventedEvent>()
            .Any(fact => fact.SkillId == Jiaojin && fact.SourceSeat == male && fact.TargetSeat == 0 && fact.Amount == 2),
            "One real equipment payment prevents all two points from the male source, rather than reducing that damage by one.");
        Use(game, "damage", [female]); Play(game);
        Need(game.CreateSnapshot(0).Players[0].Hp == hp - 2 && game.CreateSnapshot(0).Players[0].Hand.Any(card => card.Kind == CardKind.SilverLion),
            "A female source's real damage is applied while another equipment card remains available; the male-source requirement is preserved.");
    }

    private static void DrainWatchesAndPlay(GameEngine game)
    {
        for (var step = 0; step < 12; step++)
        {
            Reach(game, prompt => prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } || WatchPending(prompt));
            if (Decision(game) is { Kind: DecisionKind.PlayCard }) return;
            Need(game.ResolutionStack.OfType<ProgramSkillFrame>().Last().SkillId == Watch,
                "Only the original public-API fixture's real native return child may be answered while reaching the next Play.");
            ContinueWatch(game);
        }
        throw new InvalidOperationException("Sun Lu Ban did not finish its bounded actual movement/draw children.");
    }
    private static void Cold(ref GameEngine game, ContentRegistry registry)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Need(JsonSerializer.Serialize(game.CreateSnapshot(0)) == JsonSerializer.Serialize(restored.CreateSnapshot(0)),
            "The actual accepted commands restore this owner's original private choice, paid obtain or native compound/draw child exactly.");
        game = restored;
    }
    private static void Need(bool valid, string message) { if (!valid) throw new InvalidOperationException(message); }
}
