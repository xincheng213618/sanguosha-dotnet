using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class MobileGuzhengReturnReplacementChecks
{
    private const string Guzheng = "boundary:guzheng-mobile", Gujing = "happy:gujing";
    private const string Mode = "identity:classic-mobile-guzheng-return", Deck = "fixture:mobile-guzheng-return-deck";
    private const string Return = "skill-program.boundary:guzheng-mobile.phase-return";
    private const string Claim = "skill-program.boundary:guzheng-mobile.phase-claim";
    private static readonly string[] Generals = ["happy:shen-wei-yan", "fixture:guzheng-return-discarder",
        "boundary:zhang-zhao-zhang-hong-mobile", "fixture:guzheng-return-peer"];

    internal static void OnlyTheActualReturnToThePhaseOwnerUnlocksTheRestCold()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        CheckReturn(content, redirect: false);
        CheckReturn(content, redirect: true);
    }

    private static void CheckReturn(ContentRegistry content, bool redirect)
    {
        var game = Start(content);
        if (redirect)
        {
            var bone = game.CreateSnapshot(0).Players[0].Hand.First().Id;
            Accept(game, new UseProgramSkillCommand(0, Gujing, "bone-path", [], [1], game.Revision, Next(game).PromptId));
            GujingChoice(game, "card:" + bone);
            GujingChoice(game, "slot:Weapon");
            GujingChoice(game, "abolish:" + bone);
            GujingChoice(game, "finish");
            var issue = game.Events.Select(entry => entry.Payload).OfType<HappyWeiYanGujingIssuedEvent>().Single();
            var removed = game.Events.Select(entry => entry.Payload).OfType<HappyWeiYanGujingAbolishedEvent>().Single();
            Require(issue.Source.OwnerSeat == 0 && removed is { Holder: 1, Slot: EquipmentSlot.Weapon } &&
                Zone(game, bone) == CardLocation.DiscardPile,
                "Actual Gujing places and discards its bone to abolish the future discarding player's weapon slot.");
        }

        EndPlay(game, 0);
        if (redirect)
        {
            Require(Next(game) is { PlayerSeat: 0, SkillPrompt.SkillId: "happy:ezhi" },
                "The original bone reaches its own actual turn-end window before seat one plays.");
            Answer(game, choice => choice.Parameters.GetValueOrDefault("option") == "skip");
        }
        Require(Next(game) is { PlayerSeat: 1, Kind: DecisionKind.PlayCard }, "The future discard owner reaches its actual play phase.");
        var discarder = game.CreateSnapshot(1).Players[1];
        Require(discarder.HandLimit is { } limit && discarder.HandCount - limit >= 2,
            $"Seat one's actual live hand must exceed its limit by two or more: hand {discarder.HandCount}, HP {discarder.Hp}/{discarder.MaxHp}, limit {discarder.HandLimit}.");
        EndPlay(game, 1);
        var activation = Next(game);
        Require(activation is { PlayerSeat: 2, SkillPrompt.SkillId: Guzheng } &&
            activation.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate"),
            "Actual mobile Guzheng is offered after seat one's real discard phase.");
        Activate(game);
        var prompt = Next(game);
        var handToDiscard = game.CardMovements.Where(move => move.From == CardLocation.Hand(1) &&
            move.To == CardLocation.DiscardPile).ToArray();
        var originalDiscards = handToDiscard.Where(move => move.Reason == CardMoveReasons.HandLimitDiscard).ToArray();
        var view = game.CreateSnapshot(1);
        Require(prompt is { PlayerSeat: 2, SkillPrompt.SkillId: Guzheng } && originalDiscards.Length >= 2 &&
            originalDiscards.All(move => move.CardKind == CardKind.Crossbow) &&
            prompt.Choices.SelectMany(choice => choice.Cards).ToHashSet().SetEquals(originalDiscards.Select(move => move.CardId)),
            $"The phase owner's actual hand-discarded weapons must form the return choices. Redirect={redirect}, " +
            $"turn {view.TurnNumber} {view.Phase}, seat {prompt.PlayerSeat} {prompt.Kind} {prompt.SkillPrompt?.SkillId}, " +
            $"choices [{string.Join(", ", prompt.Choices.Select(choice => $"{choice.Id}:{string.Join('/', choice.Cards)}"))}], " +
            $"seat 1 hand-to-discard [{string.Join(", ", handToDiscard.Select(move => $"{move.Sequence}/{move.CardId}/{move.CardKind}/{move.Reason.Value}/turn {move.TurnNumber}/{move.MovementTiming?.Phase}"))}].");

        game = Cold(game, content);
        var returned = originalDiscards[0].CardId;
        var remaining = originalDiscards.Skip(1).Select(move => move.CardId).ToArray();
        Answer(game, choice => choice.Cards.SequenceEqual([returned]));
        var next = Next(game);
        var delivered = game.CardMovements.Single(record => record.CardId == returned && record.Reason.Value == Return);
        Require(delivered.From == CardLocation.DiscardPile && delivered.Sequence > originalDiscards[0].Sequence &&
            delivered.To == CardLocation.Hand(redirect ? 0 : 1) && Zone(game, returned) == delivered.To &&
            remaining.All(card => Zone(game, card) == CardLocation.DiscardPile),
            "The selected original discard really moves to the actual receiving hand while the rest are still available.");
        if (redirect)
        {
            Require(delivered.HappyWeiYanGainDestination is { } replacement && replacement.RequestedTo == CardLocation.Hand(1) &&
                replacement.ActualTo == CardLocation.Hand(0),
                "The physical return receipt proves Gujing replaced the intended recipient before movement.");
            Require(next is { PlayerSeat: 2, Kind: DecisionKind.PlayCard } &&
                game.CardMovements.All(record => record.Reason.Value != Claim),
                $"A redirected Guzheng return must end without a claim offer. Move {delivered.Sequence} reached seat {delivered.To.OwnerSeat}; next prompt: {next.Prompt}");
            return;
        }

        Require(next is { PlayerSeat: 2, SkillPrompt.SkillId: Guzheng } &&
            next.Choices.Any(choice => choice.Parameters.GetValueOrDefault("branch") == "claim"),
            "An actual return to the phase owner unlocks the separate optional claim.");
        Answer(game, choice => choice.Parameters.GetValueOrDefault("branch") == "claim");
        Require(Next(game) is { PlayerSeat: 2, Kind: DecisionKind.PlayCard } &&
            remaining.All(card => Zone(game, card) == CardLocation.Hand(2)) &&
            game.CardMovements.Where(record => record.Reason.Value == Claim).Select(record => record.CardId).ToHashSet().SetEquals(remaining),
            "The original skill holder really receives the rest after successful return.");
    }

    private static void GujingChoice(GameEngine game, string option)
    {
        Require(Next(game) is { PlayerSeat: 0, SkillPrompt.SkillId: Gujing }, $"Actual Gujing must offer {option}.");
        Answer(game, choice => choice.Parameters.GetValueOrDefault("option") == option);
    }

    private static void EndPlay(GameEngine game, int seat)
    {
        var play = Next(game);
        Require(play.Kind == DecisionKind.PlayCard && play.PlayerSeat == seat, $"Seat {seat} must reach its actual play phase, got {play.PlayerSeat} {play.Prompt}.");
        Accept(game, new EndPlayPhaseCommand(seat, game.Revision, play.PromptId));
    }

    private static PendingDecision Next(GameEngine game) { Reach(game, _ => true); return P(game)!; }
    private static CardLocation Zone(GameEngine game, int id) => game.CreateCardZoneDiagnostics().Single(card => card.CardId == id).Location;

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) && Enumerable.Range(0, game.PlayerCount).All(seat =>
            SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold replay preserves the original phase discards and the return selection.");
        return restored;
    }

    private static GameEngine Start(ContentRegistry content)
    {
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 17, PlayerCount = 4, ModeId = Mode,
            HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, UseInteractiveSetup = true,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        Accept(game, new StartGameCommand());
        while (Next(game) is { Kind: DecisionKind.SelectGeneral or DecisionKind.SelectFaction } selection)
        {
            if (selection.Kind == DecisionKind.SelectFaction)
                Answer(game, choice => choice.Parameters.GetValueOrDefault("faction-id") == "shu");
            else
                Accept(game, new SelectGeneralCommand(selection.PlayerSeat, Generals[selection.PlayerSeat], game.Revision, selection.PromptId));
        }
        return game;
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-guzheng-return", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Generals[0]);
            CharacterFixtureContent.Register(builder, Generals[2]);
            builder.AddGeneral(new(Generals[1], "弃牌角色", "supporter", "standard:none", "wei", 2));
            builder.AddGeneral(new(Generals[3], "同桌", "supporter", "standard:none", "wu", 6));
            builder.AddDeck(new(Deck, "骨径与固政的实体武器", InitialHandSize: 5, DrawPerTurn: 0, [])
            {
                PhysicalCards = Enumerable.Range(0, 36).Select(index =>
                    new ContentDeckPhysicalCard("standard:crossbow", Suit.Club, index % 13 + 1)).ToArray()
            });
            builder.AddMode(new(Mode, "移动固政归还替代", 4, 4, new Dictionary<string, int>
            {
                [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2
            }, Deck, GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}
