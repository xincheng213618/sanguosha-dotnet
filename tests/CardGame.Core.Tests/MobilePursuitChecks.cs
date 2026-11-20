using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using static BoundaryLiDianChecks;

internal static class MobilePursuitChecks
{
    private const string Yang = "mobile:yang-hui-yu", Cao = "mobile:cao-song";
    private const string Hongyi = "mobile:hongyi", Quanfeng = "mobile:quanfeng", Yijin = "mobile:yijin", Guanzong = "mobile:guanzong";
    private const string Witness = "fixture:pursuit-witness", Deck = "fixture:pursuit-deck";
    private const string RecordDamage = "fixture:pursuit-after-damage";

    internal static void Run()
    {
        PursueCaoSongWithNoOpeningGold();
        DistributeActualOpeningGold();
    }

    private static void PursueCaoSongWithNoOpeningGold()
    {
        var (game, content) = Start(Yang, new(Cao, Hp: 1) { StartingHand = [] });
        var play = Next(game);
        var slash = game.CreateSnapshot(0).Players[0].Hand.Single(card => card.Kind == CardKind.Slash);
        Require(Facts<PlayerMarkerChangedEvent>(game).Count(fact => fact.PlayerSeat == 1 && fact.Delta == 1) == 6,
            "The real opening 亿金 admits the original owner’s stock before anyone plays.");
        Accept(game, new UseProgramSkillCommand(0, Hongyi, "designate", [], [2], game.Revision, play.PromptId));
        play = Next(game);
        Require(Facts<PlayerMarkerChangedEvent>(game).Single(fact => fact.Marker == PlayerMarkerKind.Hongyi).PlayerSeat == 2,
            "弘仪 designates exactly the chosen other character.");
        Accept(game, new PlayCardCommand(0, slash.Id, [1], game.Revision, play.PromptId));

        var pursue = Next(game);
        Require(pursue.SkillPrompt?.SkillId == Quanfeng &&
            game.ResolutionStack.OfType<ProgramKillTriggerWindowFrame>().Single().VictimSeat == 1,
            "劝封 opens on the exact completed death of Cao Song.");
        game = Restore(game, content);
        Answer(game, Next(game), choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        play = Next(game);
        Require(play.Kind == DecisionKind.PlayCard && game.CreateSnapshot(0).Players[0] is { Hp: 4, MaxHp: 4 } &&
            Facts<SkillsAcquiredEvent>(game).Any(fact => fact.PlayerSeat == 0 && fact.SkillIds.SequenceEqual([Yijin, Guanzong])) &&
            Facts<CharacterSkillsLostEvent>(game).Any(fact => fact.Seat == 0 && fact.SkillIds.Contains(Hongyi)) &&
            Facts<PlayerMarkerChangedEvent>(game).Single(fact => fact.Marker == PlayerMarkerKind.Pursued).PlayerSeat == 1 &&
            !Facts<PlayerMarkerChangedEvent>(game).Any(fact => fact.PlayerSeat == 0 && fact.Marker.ToString().StartsWith("Yijin", StringComparison.Ordinal)),
            "Pursuit grants only the printed skills, loses 弘仪, grows health once, and never manufactures opening gold.");

        var before = game.CreateSnapshot(0).Players[3].Hp;
        var damageCount = Facts<DamageAppliedEvent>(game).Count();
        var completionCount = Facts<AfterDamageEvent>(game).Count();
        Accept(game, new UseProgramSkillCommand(0, Guanzong, "virtual-damage", [], [2, 3], game.Revision, play.PromptId));
        play = Next(game);
        Require(Facts<VirtualDamageIssuedEvent>(game).Single() is { SourceSeat: 2, TargetSeat: 3, Amount: 1 } &&
            Facts<DamageAppliedEvent>(game).Count() == damageCount && Facts<AfterDamageEvent>(game).Count() == completionCount &&
            game.CreateSnapshot(0).Players[3].Hp == before &&
            Facts<ProgramBindingResolvedEvent>(game).Count(fact => fact.SkillId == RecordDamage && fact.Completed) == 2,
            "惯纵 offers the selected source and recipient real after-damage triggers without applying actual damage.");
        Accept(game, new EndPlayPhaseCommand(0, game.Revision, play.PromptId));
        for (var step = 0; step < 200 && !Facts<PlayerDiedEvent>(game).Any(fact => fact.VictimSeat == 0); step++)
            Advance(game);
        Require(Facts<ProgramSkillCauseDeathDeclaredEvent>(game).Any(fact => fact.SkillId == Yijin && fact.TargetSeat == 0) &&
            Facts<PlayerDiedEvent>(game).Count(fact => fact.VictimSeat == 0) == 1 &&
            !Facts<PlayerDyingEvent>(game).Any(fact => fact.VictimSeat == 0),
            "The inherited 亿金 directly kills the new holder at the next actual turn start without rescue.");
    }

    private static void DistributeActualOpeningGold()
    {
        var (game, content) = Start(Cao, new(Witness) { StartingHand = [] });
        var gift = Next(game);
        Require(gift.SkillPrompt?.SkillId == Yijin && gift.Choices.Count == 18,
            "The compulsory play-start 亿金 choice exposes six golds and three eligible recipients.");
        game = Restore(game, content);
        Answer(game, Next(game), choice => choice.Targets.SequenceEqual([1]) &&
            choice.Parameters.GetValueOrDefault("gold") == "mobile:yijin-wushi");
        var play = Next(game);
        Require(play.Kind == DecisionKind.PlayCard &&
            Facts<PlayerMarkerChangedEvent>(game).Any(fact => fact.PlayerSeat == 0 && fact.Marker == PlayerMarkerKind.YijinWushi && fact.Count == 0) &&
            Facts<PlayerMarkerChangedEvent>(game).Any(fact => fact.PlayerSeat == 1 && fact.Marker == PlayerMarkerKind.YijinWushi && fact.Count == 1),
            "The mixed gold-and-recipient prompt restores and transfers exactly its accepted gold before Play proceeds.");
        Accept(game, new EndPlayPhaseCommand(0, game.Revision, play.PromptId));
        for (var step = 0; step < 120 && !Facts<PlayerMarkerChangedEvent>(game).Any(fact =>
            fact.PlayerSeat == 1 && fact.Marker == PlayerMarkerKind.YijinWushi && fact.Count == 0); step++)
            Advance(game);
        var draws = Facts<CardsDrawnEvent>(game).Where(fact => fact.PlayerSeat == 1).Select(fact => fact.Count).ToArray();
        var gold = Facts<PlayerMarkerChangedEvent>(game).Where(fact => fact.PlayerSeat == 1 &&
            fact.Marker == PlayerMarkerKind.YijinWushi).Select(fact => fact.Count).ToArray();
        var drawBinding = Facts<ProgramBindingResolvedEvent>(game).Where(fact => fact.OwnerSeat == 1 &&
            fact.SkillId == "mobile:yijin-wushi").Select(fact => $"{fact.BindingId}={fact.Completed}").ToArray();
        Require(draws.Count(count => count == 6) == 1 && gold.SequenceEqual([1, 0]),
            $"膴仕 supplies four extra normal draws and expires after the recipient’s next turn. " +
            $"Draws {string.Join(',', draws)}. Gold {string.Join(',', gold)}. Bindings {string.Join(',', drawBinding)}.");
    }

    private static (GameEngine Game, ContentRegistry Content) Start(string hero, ShanheCombatantDefinition firstOpponent)
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(89, 1, "mobile-dead-general-pursuit", hero, 3, 0, 0, 0, 0, [],
            [new(CardKind.Slash, Suit.Spade, 7)], [],
            [firstOpponent, new(Witness) { StartingHand = [] }, new(Witness) { StartingHand = [] }], [], new string('a', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, ConfirmOpeningHand = false, UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false
        }, content);
        Accept(game, new StartGameCommand());
        return (game, content);
    }

    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();

    private static PendingDecision Next(GameEngine game)
    {
        for (var step = 0; step < 120; step++)
        {
            if (game.CreateSnapshot(0).PendingDecision is { } prompt &&
                !prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("response") == "let-die"))
                return prompt;
            Advance(game);
        }
        throw new InvalidOperationException("The mobile pursuit fixture did not return to its next public prompt.");
    }

    private static void Advance(GameEngine game)
    {
        var snapshot = game.CreateSnapshot(0);
        if (snapshot.PendingDecision is not { } prompt)
        {
            Accept(game, new AdvanceOneStepCommand(game.Revision));
            return;
        }
        if (prompt.Kind == DecisionKind.DiscardCards)
        {
            Accept(game, new DiscardCardsCommand(0, prompt.ValidCardIds.Take(prompt.RequiredCardCount).ToArray(),
                prompt.PromptId, game.Revision));
            return;
        }
        if (prompt.Kind is DecisionKind.RespondDodge or DecisionKind.RespondSlash or DecisionKind.Nullification)
        {
            var choice = prompt.Choices.FirstOrDefault(choice => choice.Cards.Count > 0) ??
                prompt.Choices.Single(choice => choice.Parameters.GetValueOrDefault("response") is "pass" or "take-damage");
            Accept(game, new AnswerPromptCommand(0, prompt.PromptId, choice.Id, game.Revision));
            return;
        }
        if (prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("response") == "let-die"))
        {
            Answer(game, prompt, choice => choice.Parameters.GetValueOrDefault("response") == "let-die");
            return;
        }
        throw new InvalidOperationException($"Unexpected mobile pursuit prompt {prompt.Kind}, {prompt.SkillPrompt?.SkillId}, " +
            $"turn {snapshot.TurnNumber}, seat {snapshot.CurrentSeat}, phase {snapshot.Phase}.");
    }

    private static void Answer(GameEngine game, PendingDecision prompt, Func<PromptChoice, bool> select) =>
        Accept(game, new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices.Single(select).Id, game.Revision));

    private static GameEngine Restore(GameEngine game, ContentRegistry content) =>
        GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-pursuit", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Yang, Cao);
            const string rules = """
                { "skills": [{ "id": "fixture:pursuit-after-damage", "revision": 1, "triggers": [
                  { "id": "source-draw", "window": "afterDamageApplied", "subject": "damageSource", "optional": false, "damageOccurrence": "perDamage",
                    "effects": [{ "op": "draw", "target": "owner", "amount": 1 }] },
                  { "id": "target-draw", "window": "afterDamageApplied", "subject": "owner", "optional": false, "damageOccurrence": "perDamage",
                    "effects": [{ "op": "draw", "target": "owner", "amount": 1 }] }
                ] }] }
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [RecordDamage] = new { name = "见证伤害", description = "伤害后的测试摸牌。" } } });
            var bundle = SkillProgramCatalog.LoadCurrent(rules, presentation);
            builder.AddSkill(new(RecordDamage, "见证伤害", "伤害后的测试摸牌。") { Program = bundle.Programs[RecordDamage] });
            builder.AddGeneral(new(Witness, "见证角色", "supporter", RecordDamage, "qun", 4));
            builder.AddDeck(new(Deck, "固定闪牌堆", 0, 2, [])
            {
                PhysicalCards = Enumerable.Range(0, 16).Select(index =>
                    new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, index % 13 + 1)).ToArray()
            });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "移动版追思测试", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Yang, Cao, Witness], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}
