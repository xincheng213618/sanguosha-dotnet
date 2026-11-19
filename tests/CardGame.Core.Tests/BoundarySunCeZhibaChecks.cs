using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class BoundarySunCeZhibaChecks
{
    private const string Hero = "boundary:sun-ce", Zhiba = "boundary:zhiba-current", Zongshi = "boundary:zongshih-mobile";
    private const string TaishiCi = "boundary:taishi-ci", Hanzhan = "boundary:hanzhan";
    private const string Mode = "identity:boundary-zhiba-test", Deck = "fixture:boundary-zhiba-deck";
    private const string Claim = "program.pindian.claim", ZongshiObtain = "skill-program.boundary:zongshih-mobile.obtain";
    private static readonly string[] Generals = [Hero, "fixture:boundary-zhiba-wu", "fixture:boundary-zhiba-wei", "fixture:boundary-zhiba-shu"];

    internal static void SourceClaimsOriginalTieEntriesOrDeclinesAfterColdRestore()
    {
        var (game, content, result) = Contest(new Fixture());
        var expected = new[] { result.SourceCardId, result.OpponentCardId }.Order().ToArray();
        var claim = ReachClaim(game, expected);
        var revision = game.Revision;
        Require(!game.Submit(new AnswerPromptCommand(1, claim.PromptId, claim.Choices.Single(Take).Id, revision)).Accepted &&
            game.Revision == revision, "Only the source lord can answer the original OL claim.");
        var take = Restore(game, content);
        var decline = Restore(game, content);

        foreach (var run in new[] { game, take })
        {
            ReachClaim(run, expected);
            Answer(run, Take);
            Settle(run);
            var moves = run.CardMovements.Where(move => move.Reason.Value == Claim).ToArray();
            Require(moves.Select(move => move.CardId).SequenceEqual(expected) &&
                moves.All(move => move.From == CardLocation.Processing && move.To == CardLocation.Hand(0)) &&
                run.CardMovements.All(move => move.Reason != CardMoveReasons.PindianFinish) &&
                run.ResolutionStack.Count == 0,
                "Accepting Zhiba transfers the two original tie entries once to the source lord and completes the contest.");
        }

        ReachClaim(decline, expected);
        Answer(decline, choice => choice.Parameters.GetValueOrDefault("take") == "false");
        Settle(decline);
        var finished = decline.CardMovements.Where(move => move.Reason == CardMoveReasons.PindianFinish).ToArray();
        Require(finished.Select(move => move.CardId).Order().SequenceEqual(expected) &&
            finished.All(move => move.From == CardLocation.Processing && move.To == CardLocation.DiscardPile) &&
            decline.CardMovements.All(move => move.Reason.Value != Claim),
            "Declining after cold restore discards both original entries once through native Pindian cleanup.");
    }

    internal static void EarlierClaimLeavesOnlyOriginalSurvivorAndOpponentWinDoesNotOffer()
    {
        var (game, content, result) = Contest(new Fixture(earlierClaim: true));
        Reach(game, prompt => prompt is { PlayerSeat: 1, SkillPrompt.SkillId: Zongshi });
        Answer(game, choice => choice.Parameters.GetValueOrDefault("branch") == "contest" && choice.Cards.SequenceEqual([result.OpponentCardId]));

        ReachClaim(game, [result.SourceCardId]);
        game = Restore(game, content);
        ReachClaim(game, [result.SourceCardId]);
        Answer(game, Take);
        Settle(game);
        var earlier = game.CardMovements.Single(move => move.Reason.Value == ZongshiObtain);
        var zhiba = game.CardMovements.Single(move => move.Reason.Value == Claim);
        Require(earlier.CardId == result.OpponentCardId && earlier.From == CardLocation.Processing && earlier.To == CardLocation.Hand(1) &&
            zhiba.CardId == result.SourceCardId && zhiba.From == CardLocation.Processing && zhiba.To == CardLocation.Hand(0) &&
            earlier.Sequence < zhiba.Sequence && game.CardMovements.All(move => move.Reason != CardMoveReasons.PindianFinish),
            "Zhiba takes only the surviving original entry after Zongshi has already taken the other card.");

        var (loss, _, lost) = Contest(new Fixture(opponentWins: true));
        Settle(loss);
        Require(lost is { SourceRank: 7, OpponentRank: 13, SourceWon: false } && lost.WonBy(1) &&
            P(loss) is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && loss.ResolutionStack.Count == 0 &&
            !loss.CardMovements.Any(move => move.Reason.Value == Claim) &&
            loss.CardMovements.Where(move => move.Reason == CardMoveReasons.PindianFinish).Select(move => move.CardId)
                .SequenceEqual([lost.SourceCardId, lost.OpponentCardId]),
            "An actual opponent victory closes the source lord's claim and finishes both original cards.");
    }

    internal static void HanzhanWithoutSlashKeepsTheSourceClaim()
    {
        var (game, content, result) = Contest(new Fixture(hanzhan: true));
        Require(result is { SourceRank: 7, OpponentRank: 7 } && game.CardMovements.Count(move =>
                move.Reason == CardMoveReasons.PindianReveal && move.CardKind == CardKind.Crossbow) == 2,
            "The real Zhiba and Hanzhan participants tie using two revealed Crossbows.");
        Reach(game, _ => true);
        Require(P(game) is { PlayerSeat: 0, SkillPrompt.SkillId: Zhiba },
            "OL Zhiba must keep the source lord's tie claim when Hanzhan has no Slash to take.");

        var expected = new[] { result.SourceCardId, result.OpponentCardId }.Order().ToArray();
        game = Restore(game, content);
        ReachClaim(game, expected);
        Answer(game, Take);
        Settle(game);
        var moves = game.CardMovements.Where(move => move.Reason.Value == Claim).ToArray();
        Require(moves.Select(move => move.CardId).SequenceEqual(expected) &&
            moves.All(move => move.From == CardLocation.Processing && move.To == CardLocation.Hand(0)) &&
            game.CardMovements.All(move => move.Reason != CardMoveReasons.PindianFinish),
            "The cold-restored source claim obtains both original Crossbows once.");
    }

    internal static void HanzhanTakesOriginalTiedSlashesAfterSourceDeclines()
    {
        var (game, content, result) = Contest(new Fixture(hanzhan: true, slash: true));
        var cards = new[] { result.SourceCardId, result.OpponentCardId };
        Reach(game, _ => true);
        Require(P(game) is { PlayerSeat: 0, SkillPrompt.SkillId: Zhiba },
            "The source's Zhiba result claim precedes the opponent's Hanzhan result claim.");
        ReachClaim(game, cards.Order().ToArray());
        Answer(game, choice => choice.Parameters.GetValueOrDefault("take") == "false");
        Reach(game, prompt => prompt is { PlayerSeat: 1, SkillPrompt.SkillId: Hanzhan } && prompt.Choices.Any(Take));
        game = Restore(game, content);
        Require(P(game)!.Choices.Single(Take).Cards.SequenceEqual(cards) && game.CardMovements.All(move => move.Reason.Value != Claim),
            "The opponent's restored Hanzhan choice contains both original tied maximum Slashes.");
        Answer(game, Take);
        Settle(game);
        var moves = game.CardMovements.Where(move => move.Reason.Value == Claim).ToArray();
        Require(moves.Select(move => move.CardId).SequenceEqual(cards) &&
            moves.All(move => move.From == CardLocation.Processing && move.To == CardLocation.Hand(1)) &&
            game.CardMovements.All(move => move.Reason != CardMoveReasons.PindianFinish),
            "Hanzhan obtains the actual original Slashes once after the source declines.");
    }

    private static (GameEngine Game, ContentRegistry Content, PindianResult Result) Contest(Fixture fixture)
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), fixture);
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 47, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
            ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false
        }, content);
        Accept(game, new StartGameCommand());
        ReachSetup();
        while (P(game) is { Kind: DecisionKind.SelectGeneral } selection)
        {
            Accept(game, new SelectGeneralCommand(selection.PlayerSeat, fixture.GeneralIds[selection.PlayerSeat], game.Revision, selection.PromptId));
            ReachSetup();
        }

        var source = game.CreateSnapshot(0).Players[0].Hand[0].Id;
        var opponent = game.CreateSnapshot(1).Players[1].Hand[0].Id;
        var play = P(game)!;
        var revision = game.Revision;
        Require(!game.Submit(new UseProgramSkillCommand(0, Zhiba, "challenge-wu", [source], [2], revision, play.PromptId)).Accepted &&
            game.Revision == revision, "The OL Zhiba activation admits only other Wu targets.");
        Accept(game, new UseProgramSkillCommand(0, Zhiba, "challenge-wu", [source], [1], game.Revision, play.PromptId));
        if (fixture.GeneralIds[1] == TaishiCi)
        {
            Reach(game, prompt => prompt is { PlayerSeat: 1, SkillPrompt.SkillId: Hanzhan } &&
                prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("action") == "pindian-random"));
            Answer(game, choice => choice.Parameters.GetValueOrDefault("take") == "false");
        }
        Reach(game, prompt => prompt.PlayerSeat == 1 && prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("action") == "pindian-card"));
        Answer(game, choice => choice.Cards.SequenceEqual([opponent]));

        var result = game.Events.Select(e => e.Payload).OfType<PindianResultDeterminedEvent>().Single().Result;
        Require(result is { SourceSeat: 0, OpponentSeat: 1, SourceRank: 7 } && result.SourceCardId == source && result.OpponentCardId == opponent,
            "The OL lord is the native source and the selected Wu character supplies the opposing hand card.");
        return (game, content, result);

        void ReachSetup() => Reach(game, prompt => prompt.Kind == DecisionKind.SelectGeneral || prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
    }

    private static PendingDecision ReachClaim(GameEngine game, int[] expected)
    {
        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Zhiba } && prompt.Choices.Any(Take));
        var claim = P(game)!;
        var frame = game.ResolutionStack.OfType<PindianFrame>().Single();
        var revealed = game.CardMovements.Where(move => move.Reason == CardMoveReasons.PindianReveal).ToArray();
        Require(frame is { PindianStep: PindianStep.ClaimResult, Result: { SourceRank: 7, OpponentRank: 7 } } &&
            revealed.Length == 2 && revealed.All(move => move.To == CardLocation.Processing &&
                frame.ProcessingEntries!.Any(entry => entry.CardId == move.CardId && entry.EntrySequence == move.Sequence)) &&
            claim.Choices.Single(Take).Cards.SequenceEqual(expected) && game.CreateSnapshot(1).PendingDecision is null,
            "The source lord's tie claim publishes exactly the surviving original Processing entries.");
        return claim;
    }

    private static bool Take(PromptChoice choice) => choice.Parameters.GetValueOrDefault("take") == "true";

    private static GameEngine Restore(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(Enumerable.Range(0, game.PlayerCount).All(seat =>
                SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))) &&
            restored.CardMovements.SequenceEqual(game.CardMovements), "Cold replay preserves every player's view and the native Processing entries.");
        return restored;
    }

    private sealed class Fixture(bool earlierClaim = false, bool opponentWins = false, bool hanzhan = false, bool slash = false) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-zhiba", new(1, 0));
        public IReadOnlyList<string> GeneralIds { get; } = hanzhan ? [Hero, TaishiCi, Generals[2], Generals[3]] : Generals;

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero, "classic:zhou-yu", "classic:sun-jian");
            if (opponentWins)
                CharacterFixtureContent.Register(builder, "classic:qin-mi");
            if (earlierClaim)
            {
                var bundle = CharacterFixtureContent.EmbeddedProgramBundle("boundary-jian-yong-mobile");
                var presentation = bundle.Presentations[Zongshi];
                builder.AddSkill(new(Zongshi, presentation.Name, presentation.Description)
                    { Program = bundle.Programs[Zongshi], ProgramPresentation = presentation });
            }
            if (GeneralIds[1] == TaishiCi)
                CharacterFixtureContent.Register(builder, TaishiCi);
            else
            {
                var opponentSkill = earlierClaim ? Zongshi : opponentWins ? "classic:tianbian" : "standard:none";
                builder.AddGeneral(new(Generals[1], "吴势力拼点者", "supporter", opponentSkill, "wu", 4));
            }
            builder.AddGeneral(new(Generals[2], "魏势力角色", "supporter", "standard:none", "wei", 4));
            builder.AddGeneral(new(Generals[3], "蜀势力角色", "supporter", "standard:none", "shu", 4));
            builder.AddDeck(new(Deck, "固定红桃七", 4, 2, []) { PhysicalCards = Enumerable.Range(0, 48)
                .Select(_ => new ContentDeckPhysicalCard(slash ? "standard:slash" : "standard:crossbow", Suit.Heart, 7)).ToArray() });
            builder.AddMode(new(Mode, "OL 界制霸", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: GeneralIds));
        }
    }
}
