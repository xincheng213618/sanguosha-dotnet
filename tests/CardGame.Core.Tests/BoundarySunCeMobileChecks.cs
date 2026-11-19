using CardGame.Content.Standard;
using CardGame.Core;

internal static class BoundarySunCeMobileChecks
{
    private const string Mode = "identity:mobile-zhiba", Deck = "fixture:mobile-zhiba-deck";
    private const string ClaimReason = "program.pindian.claim";
    private static readonly string[] Peers = ["fixture:mobile-zhiba-peer-1", "fixture:mobile-zhiba-peer-2", "fixture:mobile-zhiba-peer-3"];

    public static void ZhibaClaimsOriginalPairAfterColdRestore()
    {
        VerifyClaim("boundary:sun-ce-mobile", "boundary:zhiba-mobile");
        VerifyClaim("mobile:sun-ce", "mobile:zhiba");
    }

    private static void VerifyClaim(string general, string skill)
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture(general));
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 47, PlayerCount = 4, HumanSeat = 0,
            HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, ModeId = Mode, UseInteractiveSetup = true,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        Submit(game, new StartGameCommand());
        var prompt = Next(game);
        while (prompt.Kind == DecisionKind.SelectGeneral)
        {
            var selectedGeneral = prompt.PlayerSeat == 0 ? general : Peers[prompt.PlayerSeat - 1];
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

        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(Enumerable.Range(0, 4).All(seat => SnapshotJson.Serialize(game.CreateSnapshot(seat)) == SnapshotJson.Serialize(restored.CreateSnapshot(seat))) &&
            restored.ResolutionStack.OfType<PindianFrame>().Single().ProcessingEntries!.SequenceEqual(entries),
            "Cold restore preserves the original reveal entries and all views of the pending claim.");
        ChooseClaim(restored, pair.Id, true);
        var claims = restored.CardMovements.Where(move => move.Reason.Value == ClaimReason).ToArray();
        Require(claims.Select(move => move.CardId).SequenceEqual(cardIds) && claims.All(move =>
                move.From == CardLocation.Processing && move.To == CardLocation.Hand(0)) &&
            cardIds.All(id => restored.CreateSnapshot(0).Players[0].Hand.Any(card => card.Id == id)),
            "Accepting after cold restore obtains each of the two actual cards once in result order.");

        ChooseClaim(game, pair.Id, false);
        var discards = game.CardMovements.Where(move => move.Reason == CardMoveReasons.PindianFinish).ToArray();
        Require(!game.CardMovements.Any(move => move.Reason.Value == ClaimReason) &&
            discards.Select(move => move.CardId).SequenceEqual(cardIds) && discards.All(move =>
                move.From == CardLocation.Processing && move.To == CardLocation.DiscardPile),
            "Declining lets native cleanup discard both exact cards without issuing a claim movement.");
    }

    private static PromptChoice ClaimOption(PendingDecision prompt, bool take) => prompt.Choices.Single(choice =>
        choice.Parameters.GetValueOrDefault("action") == "pindian-claim" && choice.Parameters.GetValueOrDefault("take") == (take ? "true" : "false"));

    private static void ChooseClaim(GameEngine game, long pairId, bool take)
    {
        var prompt = Next(game);
        Submit(game, new AnswerPromptCommand(0, prompt.PromptId, ClaimOption(prompt, take).Id, game.Revision));
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

    private sealed class Fixture(string general) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-zhiba", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, general, "classic:sun-ce", "boundary:zhou-yu", "classic:zhou-yu", "classic:sun-jian");
            foreach (var peer in Peers)
                builder.AddGeneral(new(peer, "吴方挑战者", "supporter", "standard:none", "wu", 4));
            builder.AddDeck(new(Deck, "固定同点数", 4, 0, []) { PhysicalCards = Enumerable.Range(0, 64)
                .Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 7)).ToArray() });
            builder.AddMode(new(Mode, "移动制霸原生身份", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: [general, .. Peers]));
        }
    }
}
