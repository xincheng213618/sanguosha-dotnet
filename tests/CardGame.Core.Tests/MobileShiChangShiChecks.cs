using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class MobileShiChangShiChecks
{
    private const string Attendants = "mobile:shi-chang-shi";
    private const string Spectator = "fixture:attendant-spectator";
    private const string Mode = "identity:classic-attendant-reserve";

    internal static void OpeningAndRestReturnPreserveAttendantReserve()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new AttendantFixture());
        var game = GameEngine.CreateStandard(new GameOptions
        {
            PlayerCount = 4, ModeId = Mode, Seed = 93, HumanSeat = 0,
            HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
            UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false
        }, content);
        Accept(game, new StartGameCommand());
        DriveTo(game, () => P(game)?.SkillPrompt?.SkillId == "mobile:danggu");

        var opening = P(game)!;
        Require(opening.PlayerSeat == 1 && opening.Choices.Count is >= 1 and <= 4 &&
            game.CreateSnapshot(0).PendingDecision?.Choices.Count is null or 0,
            "Only the attendant owner may see the opening pair offer.");
        game = Cold(game, content);
        Require(P(game)?.Prompt == opening.Prompt && P(game)!.Choices.Select(choice => choice.Id).SequenceEqual(opening.Choices.Select(choice => choice.Id)),
            "Restoring the hidden opening prompt preserves its original random pair offer.");
        Answer(game, _ => true);

        for (var pairIndex = 0; pairIndex < 5; pairIndex++)
        {
            DriveTo(game, () => P(game) is { Kind: DecisionKind.PlayCard, PlayerSeat: 1 });
            var revealed = Facts<AttendantPairRevealedEvent>(game);
            Require(revealed.Length == pairIndex + 1 && revealed[^1].Remaining == 8 - 2 * pairIndex &&
                revealed.SelectMany(pair => new[] { pair.Primary, pair.Partner }).Distinct().Count() == 2 * (pairIndex + 1),
                "Each new appearance spends two distinct previously unseen attendants.");
            Require(game.CreateSnapshot(0).Players[1] is { Hp: 1, MaxHp: 1 },
                "The classic identity fixture uses the printed one HP for each return.");
            if (revealed[^1].Primary == "毕岚" || revealed[^1].Partner == "毕岚")
                game = CollectPicaiJudgments(game, content);
            Accept(game, new EndPlayPhaseCommand(1, game.Revision, P(game)!.PromptId));
            DriveTo(game, () => Facts<PlayerDiedEvent>(game).Count(death => death.VictimSeat == 1) == pairIndex + 1);
            var death = Facts<PlayerDiedEvent>(game).Last();
            Require(death.VictimSeat == 1 && death.IsResting == (pairIndex < 4),
                "With attendants remaining, the turn-ending direct death enters rest. The final pair dies normally.");
            if (pairIndex == 4) break;

            var resting = game.CreateSnapshot(0).Players[1];
            Require(!resting.IsAlive && resting.IsResting, "Other seats see a resting participant.");
            DriveTo(game, () => P(game)?.SkillPrompt?.SkillId == "mobile:danggu");
            Require(Facts<PlayerReturnedFromRestEvent>(game).Count(fact => fact.PlayerSeat == 1) == pairIndex + 1,
                "The next actual normal turn reaches exactly one return and pairing.");
            if (pairIndex == 0)
                game = Cold(game, content);
            Answer(game, _ => true);
        }
        Require(Facts<PlayerDiedEvent>(game).Count(death => death.VictimSeat == 1 && death.IsResting) == 4,
            "Ten attendants allow exactly four rest transitions.");
        _ = Cold(game, content);
    }

    private static GameEngine CollectPicaiJudgments(GameEngine game, ContentRegistry content)
    {
        const string skill = "mobile:picai";
        Accept(game, new UseProgramSkillCommand(1, skill, "collect-judgments", [], [], game.Revision, P(game)!.PromptId));
        DriveTo(game, () => P(game) is { Kind: DecisionKind.ProgramRepeatJudgment, PlayerSeat: 1 });
        game = Cold(game, content);
        Answer(game, choice => choice.Parameters.GetValueOrDefault("action") == "continue");
        DriveTo(game, () => P(game)?.SkillPrompt?.SkillId == skill &&
            P(game)!.Choices.Any(choice => choice.Targets.SequenceEqual([2])));
        var judgments = Facts<JudgmentResolvedEvent>(game).Where(fact => fact.Reason == "skill.mobile.picai").ToArray();
        Require(judgments.Length == 2 && judgments.All(fact => fact is { Suit: Suit.Heart, CardId: not null }),
            "The second judgment repeats the first suit and ends the judgment chain.");
        Answer(game, choice => choice.Targets.SequenceEqual([2]));
        DriveTo(game, () => P(game) is { Kind: DecisionKind.PlayCard, PlayerSeat: 1 });
        Require(judgments.All(fact => game.CardMovements.Last(movement => movement.CardId == fact.CardId).To == CardLocation.Hand(2)),
            "Both judgment cards, including the last repeated suit, go to the single chosen recipient.");
        return Cold(game, content);
    }

    private static T[] Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();

    private static void DriveTo(GameEngine game, Func<bool> ready)
    {
        for (var step = 0; step < 250; step++)
        {
            if (ready()) return;
            var prompt = P(game);
            if (prompt is null)
                Accept(game, new AdvanceOneStepCommand(game.Revision));
            else if (prompt.Kind == DecisionKind.SelectGeneral)
                Accept(game, new SelectGeneralCommand(prompt.PlayerSeat,
                    prompt.PlayerSeat == 1 ? Attendants : Spectator + "-" + prompt.PlayerSeat, game.Revision, prompt.PromptId));
            else if (prompt.Kind == DecisionKind.PlayCard)
                Accept(game, new EndPlayPhaseCommand(prompt.PlayerSeat, game.Revision, prompt.PromptId));
            else
                throw new InvalidOperationException($"Unexpected attendant fixture prompt {prompt.Kind}: {prompt.Prompt}");
        }
        throw new InvalidOperationException("The attendant fixture stalled before its expected boundary.");
    }

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(Enumerable.Range(0, restored.PlayerCount).All(seat =>
                SnapshotJson.Serialize(game.CreateSnapshot(seat)) == SnapshotJson.Serialize(restored.CreateSnapshot(seat))) &&
            Facts<AttendantPairRevealedEvent>(game).SequenceEqual(Facts<AttendantPairRevealedEvent>(restored)) &&
            Facts<PlayerDiedEvent>(game).SequenceEqual(Facts<PlayerDiedEvent>(restored)),
            "Public-command replay preserves the pairing, deaths and each private view.");
        return restored;
    }

    private sealed class AttendantFixture : IGameContentPackage
    {
        private const string Deck = "fixture:attendant-deck";
        public PackageManifest Manifest { get; } = new("fixture-attendant-reserve", new(1, 0, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Attendants);
            foreach (var seat in new[] { 0, 2, 3 })
                builder.AddGeneral(new(Spectator + "-" + seat, "旁观角色", "supporter", "standard:none", "wei", 4));
            builder.AddDeck(new(Deck, "固定红桃闪", 2, 0, [])
            {
                PhysicalCards = Enumerable.Range(0, 64)
                    .Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, 7)).ToArray()
            });
            builder.AddMode(new(Mode, "常侍休整", 4, 4, new Dictionary<string, int>
                {
                    [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 2, [nameof(Role.Loyalist)] = 1
                }, Deck, GeneralCandidateCount: 4,
                GeneralPoolIds: [Spectator + "-0", Attendants, Spectator + "-2", Spectator + "-3"]));
        }
    }
}
