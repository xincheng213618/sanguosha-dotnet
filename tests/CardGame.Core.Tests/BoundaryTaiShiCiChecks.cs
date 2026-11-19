using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class BoundaryTaiShiCiChecks
{
    private const string Hero = "boundary:taishi-ci", Tianyi = "boundary:tianyi", Hanzhan = "boundary:hanzhan";
    private const string QinMi = "classic:qin-mi", Gushe = "ol:gushe";
    private const string Peer = "fixture:hanzhan-peer", Deck = "fixture:hanzhan-deck", Claim = "program.pindian.claim";

    public static void TianyiClaimsOnlyMaximumEffectiveSlashEntriesCold()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        VerifyPair(content, new(Peer) { StartingHand = [new(CardKind.Crossbow, Suit.Club, 13)] }, 13, [0]);
        VerifyPair(content, new(QinMi, ExtraSkillIds: [Hanzhan]) { StartingHand = [new(CardKind.ThunderSlash, Suit.Heart, 2)] }, 13, [1]);
        VerifyPair(content, new(Peer) { StartingHand = [new(CardKind.ThunderSlash, Suit.Club, 7)] }, 7, [0, 1]);
    }

    private static void VerifyPair(ContentRegistry content, ShanheCombatantDefinition opponent, int effectiveRank, int[] claimSides)
    {
        var game = Start(content, [opponent, EmptyPeer(), EmptyPeer()]);
        var source = game.CreateSnapshot(0).Players[0].Hand.Single(card => card.Kind == CardKind.FireSlash);
        var target = game.CreateSnapshot(1).Players[1].Hand.Single();
        var play = Next(game);
        Submit(game, new UseProgramSkillCommand(0, Tianyi, "contest", [source.Id], [1], game.Revision, play.PromptId));
        var random = Next(game);
        Require(random is { PlayerSeat: 0, SkillPrompt.SkillId: Hanzhan, IsPrivate: true },
            "Tianyi opens the source's actual Hanzhan random-card offer.");
        Answer(game, random, PairChoice(random, "pindian-random", true));
        var secondOwner = opponent.ExtraSkillIds?.Contains(Hanzhan) == true;
        if (secondOwner)
        {
            random = Next(game);
            Require(random is { PlayerSeat: 1, SkillPrompt.SkillId: Hanzhan }, "The second owner receives its separate random-card offer.");
            Answer(game, random, PairChoice(random, "pindian-random", false));
        }

        var offer = Next(game);
        var pair = game.ResolutionStack.OfType<PindianFrame>().Single();
        var cards = new[] { source.Id, target.Id };
        var expected = claimSides.Select(side => cards[side]).ToArray();
        Require(pair.Result is { SourceRank: 7 } result && result.SourceCardId == source.Id && result.OpponentCardId == target.Id &&
            result.OpponentRank == effectiveRank && pair.RandomSelection?.Receipts.Single().CardId == target.Id,
            "The original paid pair carries the actual selected entities and their final effective ranks.");
        AssertEntries(game, pair.ProcessingEntries, cards);
        var claims = pair.PolicyClaims!.Claims;
        Require(claims.Select(claim => claim.OwnerSeat).SequenceEqual(secondOwner ? [0, 1] : [0]) &&
            claims.All(claim => claim.Source.SkillId == Hanzhan && claim.CardIds.SequenceEqual(expected)) &&
            offer is { PlayerSeat: 0, SkillPrompt.SkillId: Hanzhan } && PairChoice(offer, "pindian-claim", true).Cards.SequenceEqual(expected),
            "Hanzhan freezes only physical Slashes at the highest effective rank and offers all tied eligible entities.");

        game = Cold(game, content);
        offer = Next(game);
        var take = PairChoice(offer, "pindian-claim", true);
        Answer(game, offer, take);
        Require(Next(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } && !game.ResolutionStack.Any(frame => frame.Id == pair.Id),
            "Tianyi resumes after one acquisition. A pre-issued second claimant cannot fill a missing maximum with the lower Slash.");
        var moves = game.CardMovements.Where(move => move.Reason.Value == Claim).ToArray();
        Require(moves.Select(move => move.CardId).SequenceEqual(expected) && moves.All(move =>
            move.From == CardLocation.Processing && move.To == CardLocation.Hand(0)) &&
            game.CardMovements.Where(move => move.Reason == CardMoveReasons.PindianFinish).Select(move => move.CardId).SequenceEqual(cards.Except(expected)),
            "The cold claimant receives the original entities once, and ordinary cleanup discards any unclaimed card.");
        Require(!game.Submit(new AnswerPromptCommand(0, offer.PromptId, take.Id, game.Revision)).Accepted &&
            game.CardMovements.Count(move => move.Reason.Value == Claim) == expected.Length,
            "Resubmitting the original claim cannot acquire a second card or move the same one twice.");
    }

    public static void GusheClaimsSharedMaximumOnlyOnce()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = Start(content, [new(Peer) { StartingHand = [new(CardKind.ThunderSlash, Suit.Club, 3)] },
            new(Peer) { StartingHand = [new(CardKind.Slash, Suit.Spade, 4)] }, EmptyPeer()], [Gushe]);
        var common = game.CreateSnapshot(0).Players[0].Hand.Single(card => card.Kind == CardKind.FireSlash).Id;
        var firstPeer = game.CreateSnapshot(1).Players[1].Hand.Single().Id;
        var secondPeer = game.CreateSnapshot(2).Players[2].Hand.Single().Id;
        var play = Next(game);
        Submit(game, new UseProgramSkillCommand(0, Gushe, "simultaneous-pindian", [common], [1, 2], game.Revision, play.PromptId));
        for (var index = 0; index < 2; index++)
        {
            var random = Next(game);
            Require(random is { PlayerSeat: 0, SkillPrompt.SkillId: Hanzhan }, "Gushe opens a separate Hanzhan choice for each counterpart.");
            Answer(game, random, GroupChoice(random, "random"));
        }

        var offer = Next(game);
        var root = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SimultaneousPindian is not null);
        AssertEntries(game, root.SimultaneousPindian!.ProcessingEntries, [common, firstPeer, secondPeer]);
        Require(offer.SkillPrompt?.SkillId == Hanzhan && GroupChoice(offer, "claim").Cards.SequenceEqual([common]),
            "The first comparison freezes its still-held common high Slash as the only maximum.");
        game = Cold(game, content);
        offer = Next(game);
        Answer(game, offer, GroupChoice(offer, "claim"));

        var consequence = Next(game);
        Require(consequence.PlayerSeat == 1, "The first comparison finishes its actual loser consequence before the next one.");
        Answer(game, consequence, GroupChoice(consequence, "draw"));
        offer = Next(game);
        Require(offer.SkillPrompt?.SkillId == Hanzhan && GroupChoice(offer, "claim").Cards.SequenceEqual([secondPeer]),
            "A later comparison freezes from its own available pair. The previously obtained common entity is already gone.");
        Answer(game, offer, GroupChoice(offer, "claim"));
        consequence = Next(game);
        Require(consequence.PlayerSeat == 2, "The second comparison reaches its independent loser consequence.");
        Answer(game, consequence, GroupChoice(consequence, "draw"));
        Require(Next(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } &&
            game.Events.Select(record => record.Payload).OfType<SimultaneousPindianCompletedEvent>()
                .Single(result => result.ProgramFrameId == root.Id).CompletedComparisons == 2 &&
            game.CardMovements.Where(move => move.Reason.Value == Claim).Select(move => move.CardId).SequenceEqual([common, secondPeer]) &&
            game.CardMovements.Where(move => move.Reason == CardMoveReasons.PindianFinish).Select(move => move.CardId).SequenceEqual([firstPeer]),
            "Both comparisons settle once and their exact paid entities are claimed or cleaned up once.");
    }

    private static ShanheCombatantDefinition EmptyPeer() => new(Peer) { StartingHand = [] };

    private static GameEngine Start(ContentRegistry content, ShanheCombatantDefinition[] opponents, string[]? skills = null)
    {
        var setup = new ShanheBattleSetup(417, 1, "hanzhan-original", Hero, 4, 0, 0, 0, 0,
            skills ?? [], [new(CardKind.FireSlash, Suit.Club, 7)], [], opponents, [], new string('d', 64));
        var game = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4,
            HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        Submit(game, new StartGameCommand());
        Require(Next(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }, "The fixed game starts in the real Taishi Ci's Play phase.");
        return game;
    }

    private static void AssertEntries(GameEngine game, IReadOnlyList<PindianProcessingEntry>? entries, int[] cards) =>
        Require(entries is not null && entries.Select(entry => entry.CardId).SequenceEqual(cards) && entries.All(entry =>
            game.CardMovements.LastOrDefault(move => move.CardId == entry.CardId) is { } movement &&
            movement.Sequence == entry.EntrySequence && movement.To == CardLocation.Processing && movement.Reason == CardMoveReasons.PindianReveal),
            "The owning contest retains each exact physical reveal entry before offering its claim.");

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var copy = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(Enumerable.Range(0, 4).All(seat => SnapshotJson.Serialize(game.CreateSnapshot(seat)) == SnapshotJson.Serialize(copy.CreateSnapshot(seat))) &&
            game.CardMovements.SequenceEqual(copy.CardMovements), "Cold restore reproduces every view of the original claim and its reveal movements.");
        return copy;
    }

    private static PendingDecision Next(GameEngine game)
    {
        for (var step = 0; step < 100; step++)
        {
            foreach (var seat in Enumerable.Range(0, 4))
                if (game.CreateSnapshot(seat).PendingDecision is { } prompt)
                    return prompt;
            Submit(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The Hanzhan fixture did not reach a command boundary.");
    }

    private static PromptChoice PairChoice(PendingDecision prompt, string action, bool take) => prompt.Choices.Single(choice =>
        choice.Parameters.GetValueOrDefault("action") == action && choice.Parameters.GetValueOrDefault("take") == (take ? "true" : "false"));

    private static PromptChoice GroupChoice(PendingDecision prompt, string branch) => prompt.Choices.Single(choice =>
        choice.Parameters.GetValueOrDefault("program-action") == "simultaneous-pindian" && choice.Parameters.GetValueOrDefault("branch") == branch);

    private static void Answer(GameEngine game, PendingDecision prompt, PromptChoice choice) =>
        Submit(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, choice.Id, game.Revision));

    private static void Submit(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        Require(result.Accepted, result.Error?.Message ?? "The Hanzhan fixture command was rejected.");
    }

    private static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-hanzhan", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero, QinMi, "ol:wang-lang");
            builder.AddGeneral(new(Peer, "拼点对手", "", "standard:none", "qun", 4));
            builder.AddDeck(new(Deck, "固定牌堆", 4, 2, []) { PhysicalCards = Enumerable.Range(0, 80)
                .Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, 4)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "酣战原生拼点", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer, QinMi], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}
