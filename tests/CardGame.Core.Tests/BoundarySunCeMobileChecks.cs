using CardGame.Content.Standard;
using CardGame.Core;

internal static class BoundarySunCeMobileChecks
{
    private const string Mode = "identity:mobile-zhiba", Deck = "fixture:mobile-zhiba-deck";
    private const string ClaimReason = "program.pindian.claim";
    private const string TaishiCi = "boundary:taishi-ci", Hanzhan = "boundary:hanzhan";
    private static readonly string[] Peers = ["fixture:mobile-zhiba-peer-1", "fixture:mobile-zhiba-peer-2", "fixture:mobile-zhiba-peer-3"];

    public static void ZhibaClaimsOriginalPairAfterColdRestore()
    {
        VerifyClaim("boundary:sun-ce-mobile", "boundary:zhiba-mobile");
        VerifyClaim("mobile:sun-ce", "mobile:zhiba");
    }

    public static void ZhibaHasFirstClaimBeforeChallengersHanzhan() =>
        VerifyClaim("boundary:sun-ce-mobile", "boundary:zhiba-mobile", withHanzhan: true);

    private static void VerifyClaim(string general, string skill, bool withHanzhan = false)
    {
        var fixture = new Fixture(general, withHanzhan);
        var content = ContentRegistry.Build(new StandardContentPackage(), fixture);
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 47, PlayerCount = 4, HumanSeat = 0,
            HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, ModeId = Mode, UseInteractiveSetup = true,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        Submit(game, new StartGameCommand());
        var prompt = Next(game);
        while (prompt.Kind == DecisionKind.SelectGeneral)
        {
            var selectedGeneral = prompt.PlayerSeat == 0 ? general : fixture.Challengers[prompt.PlayerSeat - 1];
            Submit(game, new SelectGeneralCommand(prompt.PlayerSeat, selectedGeneral, game.Revision, prompt.PromptId));
            prompt = Next(game);
        }
        Require(prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }, "The unawakened Lord starts in Play.");
        Submit(game, new EndPlayPhaseCommand(0, game.Revision, prompt.PromptId));
        prompt = Next(game);
        Require(prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 1 }, "The Wu challenger's actual Play phase must begin.");
        var sourceCard = game.CreateSnapshot(1).Players[1].Hand[0].Id;
        Submit(game, new UseProgramSkillCommand(1, skill, "accept-wu-challenge", [sourceCard], [0], game.Revision, prompt.PromptId)
            { SkillOwnerSeat = 0 });
        prompt = Next(game);
        if (withHanzhan)
        {
            Require(prompt is { PlayerSeat: 1, SkillPrompt.SkillId: Hanzhan },
                "The Wu challenger receives its own Hanzhan random-card offer before reveal.");
            var declineRandom = prompt.Choices.Single(choice => choice.Parameters.GetValueOrDefault("action") == "pindian-random" &&
                choice.Parameters.GetValueOrDefault("take") == "false");
            Submit(game, new AnswerPromptCommand(1, prompt.PromptId, declineRandom.Id, game.Revision));
            prompt = Next(game);
        }
        var lordChoice = prompt.Choices.First(choice => choice.Parameters.GetValueOrDefault("action") == "pindian-card");
        Require(prompt is { Kind: DecisionKind.SkillModule, PlayerSeat: 0, IsPrivate: true },
            "The unawakened Lord chooses a private contest card immediately.");
        var lordCard = lordChoice.Cards.Single();
        Submit(game, new AnswerPromptCommand(0, prompt.PromptId, lordChoice.Id, game.Revision));
        prompt = Next(game);
        var pair = game.ResolutionStack.OfType<PindianFrame>().Single();
        var entries = pair.ProcessingEntries ?? throw new InvalidOperationException("Zhiba lost the original pair's reveal receipts.");
        var cardIds = new[] { sourceCard, lordCard };
        Require(pair.SkillId == skill && pair is { PindianStep: PindianStep.ClaimResult, SourceSeat: 1, OpponentSeat: 0,
                Result: { SourceRank: 7, OpponentRank: 7 } } &&
            entries.Select(entry => entry.CardId).SequenceEqual(cardIds) && entries.All(entry =>
                game.CardMovements.LastOrDefault(move => move.CardId == entry.CardId) is { } move && move.Sequence == entry.EntrySequence &&
                move.To == CardLocation.Processing && move.Reason == CardMoveReasons.PindianReveal),
            "The actual tied contest keeps both original Processing entries on its pair frame.");
        Require(prompt.PlayerSeat == 0 && prompt.SkillPrompt?.SkillId == skill &&
            ClaimOption(prompt, true).Cards.SequenceEqual(cardIds) && ClaimOption(prompt, false).Cards.Count == 0,
            "The Lord's optional claim identifies both original cards in result order.");

        var restored = Cold(game, content, entries);
        ChooseClaim(restored, pair.Id, true);
        RequireClaims(restored, cardIds, 0);

        if (withHanzhan)
        {
            Submit(game, new AnswerPromptCommand(0, prompt.PromptId, ClaimOption(prompt, false).Id, game.Revision));
            var followup = Next(game);
            Require(followup is { PlayerSeat: 1, SkillPrompt.SkillId: Hanzhan } &&
                ClaimOption(followup, true).Cards.SequenceEqual(cardIds) &&
                game.ResolutionStack.OfType<PindianFrame>().Single().Id == pair.Id &&
                !game.CardMovements.Any(move => move.Reason.Value == ClaimReason || move.Reason == CardMoveReasons.PindianFinish),
                "Declining Zhiba passes both original tied Slashes to the challenger's Hanzhan offer in the same pair.");
            game = Cold(game, content, entries);
            ChooseClaim(game, pair.Id, true);
            RequireClaims(game, cardIds, 1);
            return;
        }

        ChooseClaim(game, pair.Id, false);
        var discards = game.CardMovements.Where(move => move.Reason == CardMoveReasons.PindianFinish).ToArray();
        Require(!game.CardMovements.Any(move => move.Reason.Value == ClaimReason) &&
            discards.Select(move => move.CardId).SequenceEqual(cardIds) && discards.All(move =>
                move.From == CardLocation.Processing && move.To == CardLocation.DiscardPile),
            "Declining lets native cleanup discard both exact cards without issuing a claim movement.");
    }

    private static GameEngine Cold(GameEngine game, ContentRegistry content, IReadOnlyList<PindianProcessingEntry> entries)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(Enumerable.Range(0, 4).All(seat => SnapshotJson.Serialize(game.CreateSnapshot(seat)) == SnapshotJson.Serialize(restored.CreateSnapshot(seat))) &&
            restored.ResolutionStack.OfType<PindianFrame>().Single().ProcessingEntries!.SequenceEqual(entries),
            "Cold restore preserves the original reveal entries and all views of the pending claim.");
        return restored;
    }

    private static void RequireClaims(GameEngine game, IReadOnlyList<int> cardIds, int recipient)
    {
        var claims = game.CardMovements.Where(move => move.Reason.Value == ClaimReason).ToArray();
        var hand = game.CreateSnapshot(recipient).Players[recipient].Hand;
        Require(claims.Select(move => move.CardId).SequenceEqual(cardIds) && claims.All(move =>
                move.From == CardLocation.Processing && move.To == CardLocation.Hand(recipient)) &&
            cardIds.All(id => hand.Any(card => card.Id == id)),
            "Accepting after cold restore obtains each of the two actual cards once in result order.");
    }

    private static PromptChoice ClaimOption(PendingDecision prompt, bool take) => prompt.Choices.Single(choice =>
        choice.Parameters.GetValueOrDefault("action") == "pindian-claim" && choice.Parameters.GetValueOrDefault("take") == (take ? "true" : "false"));

    private static void ChooseClaim(GameEngine game, long pairId, bool take)
    {
        var prompt = Next(game);
        Submit(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, ClaimOption(prompt, take).Id, game.Revision));
        Require(Next(game) is { Kind: DecisionKind.PlayCard, PlayerSeat: 1 } && game.ResolutionStack.All(frame => frame.Id != pairId) &&
            game.Events.Select(eventRecord => eventRecord.Payload).OfType<PindianResultDeterminedEvent>().Count(e => e.FrameId == pairId) == 1,
            "The native pair settles once and resumes the original Wu challenger.");
    }

    private static PendingDecision Next(GameEngine game)
    {
        for (var step = 0; step < 100; step++)
        {
            foreach (var seat in Enumerable.Range(0, game.PlayerCount))
                if (game.CreateSnapshot(seat).PendingDecision is { } prompt)
                    return prompt;
            Submit(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The Zhiba fixture did not reach its next command boundary.");
    }

    private static void Submit(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        Require(result.Accepted, result.Error?.Message ?? "The Zhiba fixture command was rejected.");
    }

    private static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }

    private sealed class Fixture(string general, bool withHanzhan) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-zhiba", new(1, 0));
        public IReadOnlyList<string> Challengers { get; } = withHanzhan ? [TaishiCi, Peers[1], Peers[2]] : Peers;

        public void Register(IContentRegistryBuilder builder)
        {
            string[] registered = [general, "classic:sun-ce", "boundary:zhou-yu", "classic:zhou-yu", "classic:sun-jian"];
            if (withHanzhan)
                registered = [.. registered, TaishiCi];
            CharacterFixtureContent.Register(builder, registered);
            foreach (var peer in Challengers.Where(peer => peer != TaishiCi))
                builder.AddGeneral(new(peer, "吴方挑战者", "supporter", "standard:none", "wu", 4));
            builder.AddDeck(new(Deck, "固定同点数", 4, 0, []) { PhysicalCards = Enumerable.Range(0, 64)
                .Select(_ => new ContentDeckPhysicalCard(withHanzhan ? "standard:slash" : "standard:dodge", Suit.Club, 7)).ToArray() });
            builder.AddMode(new(Mode, "移动制霸原生身份", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: [general, .. Challengers]));
        }
    }
}
