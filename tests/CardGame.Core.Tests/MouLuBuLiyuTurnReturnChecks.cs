using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

using static BoundaryLiDianChecks;

internal static class MouLuBuLiyuTurnReturnChecks
{
    private const string Hero = "mou:lu-bu-mobile", Liyu = "mou-mobile:liyu", Wushuang = "mou-mobile:wushuang";
    private const string Ally = "fixture:liyu-ally", Enemy = "fixture:liyu-enemy", Victim = "fixture:liyu-victim";
    private const string Deck = "fixture:liyu-turn-deck";

    internal static void PaidWushuangSurvivesForeignTurnAndExpiresOnRecipientReturnCold()
    {
        const int source = 0, recipient = 3;
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(7, 1, "liyu-turn-return", Hero, 5, 0, 0, 0, 0, [],
            [Card(CardKind.Alcohol), Card(CardKind.Slash)], [],
            [new(Enemy, Hp: 12) { StartingHand = [] }, new(Victim, Hp: 12)
            {
                StartingHand = [Card(CardKind.Dodge), Card(CardKind.Crossbow), Card(CardKind.Duel)]
            }],
            [new(Ally, Hp: 12) { StartingHand = [] }], new string('d', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = source, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8
        }, registry);
        T[] Facts<T>() where T : IGameEvent => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();
        bool HasWushuang(int seat) => game.CreateSnapshot(seat).Players[seat].Skills?.Any(skill => skill.ContentId == Wushuang) == true;
        static bool FirstHandSlot(PromptChoice choice) => choice.Parameters.GetValueOrDefault("branch") == "take" &&
            choice.Parameters.GetValueOrDefault("source-zone") == "Hand" && choice.Parameters.GetValueOrDefault("slot-index") == "0";
        void ReachFact(Func<bool> ready)
        {
            for (var step = 0; step < 120 && !ready(); step++)
                Accept(game, new AdvanceOneStepCommand(game.Revision));
            Require(ready(), $"The real Liyu fixture missed a boundary in {game.State.Phase} on turn {game.State.TurnNumber}.");
        }
        int Use(CardKind kind, int? target = null)
        {
            var card = game.CreateSnapshot(source).Players[source].Hand.Single(candidate => candidate.Kind == kind);
            var action = game.GetHumanLegalActions().First(candidate => candidate.CardId == card.Id &&
                candidate.ConversionSource is null && (target is null || candidate.TargetSeats.SequenceEqual([target.Value])));
            Accept(game, new PlayCardCommand(source, card.Id, action.TargetSeats, game.Revision, P(game)!.PromptId,
                action.PlayedCardKind, action.TargetCardId));
            return card.Id;
        }

        Accept(game, new StartGameCommand());
        Settle(game);
        var originalPayment = game.CreateSnapshot(recipient).Players[recipient].Hand
            .Where(card => card.Kind is CardKind.Crossbow or CardKind.Duel).Select(card => card.Id).Order().ToArray();
        Require(originalPayment.Length == 2 && !HasWushuang(recipient), "The real victim has two original gain materials.");
        Use(CardKind.Alcohol);
        Settle(game);
        var slash = Use(CardKind.Slash, recipient);
        Reach(game, prompt => prompt.PlayerSeat == source && prompt.SkillPrompt?.SkillId == Liyu &&
            prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate"));
        Answer(game, choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        for (var index = 0; index < 2; index++)
        {
            Reach(game, prompt => prompt.PlayerSeat == source && prompt.Choices.Any(FirstHandSlot));
            Answer(game, FirstHandSlot);
        }
        Settle(game);

        var started = Facts<MouLuBuLiyuStartedEvent>().Single();
        var obtained = Facts<MouLuBuLiyuObtainedEvent>();
        var draw = Facts<MouLuBuLiyuDrawReturnedEvent>().Single();
        var branch = Facts<MouLuBuLiyuBranchEvent>().Single();
        var grant = Facts<MouLuBuLiyuGrantEvent>().Single();
        var originalTurn = Facts<TurnStartedEvent>().Single();
        Require(started is { VictimSeat: recipient, DamageAmount: 2 } &&
            Facts<CardUseDeclaredEvent>().Any(use => use.ResolutionId == started.OuterUseId && use.CardId == slash) &&
            obtained.Select(entry => entry.CardId).Order().SequenceEqual(originalPayment) && obtained.All(entry =>
                entry.FrameId == started.FrameId && entry.ActualGain && entry.From == CardLocation.Hand(recipient) &&
                entry.To == CardLocation.Hand(source) && game.CardMovements.Count(move => move.Sequence == entry.Sequence &&
                    move.CardId == entry.CardId && move.From == entry.From && move.To == entry.To) == 1) &&
            draw is { RecipientSeat: recipient, Requested: 2, Actual: 2, Replacement: 0, CategoryMask: 7 } && draw.FrameId == started.FrameId &&
            game.CardMovements.Count(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(recipient) &&
                move.Reason.Value == "skill-program.mou-mobile:liyu.draw" && move.Sequence > draw.Before && move.Sequence <= draw.After) == 2,
            "The original Slash opens Liyu, moves both real materials, and draws two for the victim to complete all three categories.");
        Require(branch is { ChooserSeat: recipient, Branch: "wushuang", CategoryMask: 7 } && branch.FrameId == started.FrameId &&
            grant.RecipientSeat == recipient && grant.FrameId == started.FrameId && grant.Source == started.Source &&
            grant.AfterTurnStartOrdinal == 1 && originalTurn.ActorSeat == source && originalTurn.TurnNumber == started.ActualTurn &&
            HasWushuang(recipient), "The real NPC victim takes Wushuang with the original issue-time table ordinal.");

        Accept(game, new EndPlayPhaseCommand(source, game.Revision, P(game)!.PromptId));
        ReachFact(() => Facts<ActualTurnCursorReturnedEvent>().Any(turn => turn.OwnerSeat != source && turn.OwnerSeat != recipient));
        var foreignReturn = Facts<ActualTurnCursorReturnedEvent>().Single(turn => turn.OwnerSeat != source && turn.OwnerSeat != recipient);
        Require(Facts<TurnStartedEvent>().Any(turn => turn.ActorSeat == foreignReturn.OwnerSeat &&
            turn.TurnNumber == foreignReturn.ActualTurn && turn.TurnNumber > originalTurn.TurnNumber) &&
            HasWushuang(recipient) && Facts<MouLuBuLiyuGrantExpiredEvent>().Length == 0,
            "Another seat's actual start and cursor return leave the original Wushuang grant active.");
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Require(HasWushuang(recipient) && Facts<MouLuBuLiyuGrantEvent>().Single() == grant &&
            Facts<ActualTurnCursorReturnedEvent>().Contains(foreignReturn), "Cold restore retains the original grant after the foreign return.");

        ReachFact(() => Facts<TurnStartedEvent>().Skip(grant.AfterTurnStartOrdinal).Any(turn => turn.ActorSeat == recipient));
        var due = Facts<TurnStartedEvent>().Skip(grant.AfterTurnStartOrdinal).First(turn => turn.ActorSeat == recipient);
        var expectedReturn = new ActualTurnCursorReturnedEvent(due.TurnNumber, recipient);
        Require(HasWushuang(recipient) && !Facts<ActualTurnCursorReturnedEvent>().Contains(expectedReturn),
            "The recipient's first subsequent actual start still has the original Wushuang grant.");
        ReachFact(() => Facts<ActualTurnCursorReturnedEvent>().Contains(expectedReturn));
        var ended = game.Events.Single(entry => entry.Payload is TurnEndedEvent turn &&
            turn.ActorSeat == recipient && turn.TurnNumber == due.TurnNumber);
        var returned = game.Events.Single(entry => Equals(entry.Payload, expectedReturn));
        var expired = game.Events.Single(entry => entry.Payload is MouLuBuLiyuGrantExpiredEvent);
        Require(Equals(expired.Payload, new MouLuBuLiyuGrantExpiredEvent(grant.GrantId, recipient, due.TurnNumber)) &&
            ended.Sequence < returned.Sequence && returned.Sequence < expired.Sequence && !HasWushuang(recipient) && HasWushuang(source),
            "Only the original paid grant expires when that recipient's actual-turn cursor returns.");
    }

    private static ShanheStartingCard Card(CardKind kind) => new(kind, Suit.Heart, 7);

    private sealed class Fixture : IGameContentPackage
    {
        private const string Quiet = "fixture:liyu-skip-play", NoDuel = "fixture:liyu-no-duel";
        public PackageManifest Manifest { get; } = new("fixture-mou-lu-bu-liyu-turn", new(1, 0, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var rules = $$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,
                    "effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]},
                  {"id":"{{NoDuel}}","revision":1,"cardPolicies":[{"id":"forbid-duel","kind":"prohibitTarget","cardKinds":["duel"]}]}
                ]}
                """;
            var labels = new[] { Quiet, NoDuel }.ToDictionary(id => id, id => new { name = id, description = "利驭实际回合夹具" });
            var catalog = SkillProgramCatalog.Load(rules,
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, id, labels[id].description) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Ally, "固定同桌", "", "standard:none", "shu", 12, [Quiet, NoDuel]));
            builder.AddGeneral(new(Enemy, "固定同桌", "", "standard:none", "wei", 12, [Quiet, NoDuel]));
            builder.AddGeneral(new(Victim, "原受领者", "", "standard:none", "wei", 12));
            builder.AddDeck(new(Deck, "利驭固定补牌", 0, 0, [])
            {
                PhysicalCards = Enumerable.Repeat(new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 7), 48).ToArray()
            });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "利驭原回合归还", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Ally, Enemy, Victim], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
