using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheHandStockTacticChecks
{
    private const string Owner = "fixture:hand-stock-owner", Enemy = "fixture:hand-stock-enemy";
    private const string Driver = "fixture:hand-stock-driver", Quiet = "fixture:hand-stock-quiet";
    private const string Deck = "fixture:hand-stock-deck";
    private const string PriorJingyuMoulue = "shanhe:jingyu-moulue-2-2026";

    public static void NativeDiscardCapacityAndPaidColdSlashThresholdUseActualCards()
    {
        foreach (var held in new[] { false, true })
        {
            string[] tactics = held
                ? [StandardShanheHandStockTacticsPackage.PinangSkillId, StandardShanheHandStockTacticsPackage.Pinang2SkillId,
                    StandardShanheHandStockTacticsPackage.Pinang3SkillId] : [];
            var cards = Enumerable.Range(0, 14).Select(_ => new ShanheStartingCard(CardKind.Dodge)).ToArray();
            var scene = Scene.Start(cards, tactics);
            var capacity = held ? 11 : 3;
            Need(scene.V.HandCount == 14 && scene.V.HandLimit == capacity && scene.V.Hp == 3,
                "The real player hand limit adds the three held Pinang tiers' 1+2+5 to current HP, while the unowned baseline remains HP.");
            scene.Accept(new EndPlayPhaseCommand(0, scene.G.Revision, scene.P!.PromptId));
            scene.Until(() => scene.P is { PlayerSeat: 0, Kind: DecisionKind.DiscardCards });
            var discard = scene.P!;
            Need(discard.RequiredCardCount == 14 - capacity,
                "The actual native discard request uses the held hand-limit contributions instead of granting cards or changing HP.");
            var paid = discard.ValidCardIds.Take(discard.RequiredCardCount).ToArray();
            scene.Accept(new DiscardCardsCommand(0, paid, discard.PromptId, scene.G.Revision));
            scene.Until(() => scene.V.HandCount == capacity && paid.All(id => scene.G.CardMovements.Any(move =>
                move.CardId == id && move.From == CardLocation.Hand(0) && move.To == CardLocation.DiscardPile)));
            Need(scene.V.Hp == 3 && scene.V.Hand.All(card => !paid.Contains(card.Id)),
                "The native discard pays only the requested actual entities, retains the expanded hand and leaves health unchanged.");
        }

        var damage = Scene.Start([new(CardKind.Slash), new(CardKind.ColdSlash), new(CardKind.Duel),
            new(CardKind.Dodge), new(CardKind.Dodge)], [StandardShanheHandStockTacticsPackage.JingyuMoulueSkillId]);
        var first = damage.Card(CardKind.Slash);
        Need(damage.V.HandCount == 4 && damage.Damage() is [{ SourceSeat: 0, TargetSeat: 1, Amount: 1, Nature: DamageNature.Normal }],
            "Paying the first actual Slash leaves exactly four cards, so the strict less-than-four tactic grants no bonus.");
        var cold = damage.Card(CardKind.ColdSlash);
        Need(damage.V.HandCount == 3 && damage.Damage().Last() is { SourceSeat: 0, TargetSeat: 1, Amount: 2, Nature: DamageNature.Cold },
            "Paying the real Cold Slash crosses to three cards and receives exactly one bonus while retaining native Cold damage.");
        var count = damage.Damage().Length;
        damage.Accept(new UseProgramSkillCommand(0, Driver, "hit", [], [1], damage.G.Revision, damage.P!.PromptId));
        damage.Play();
        damage.Card(CardKind.Duel);
        Need(damage.Damage().Skip(count).Select(fact => fact.Amount).SequenceEqual([1, 1]),
            "The same low hand cannot add damage to either a cardless program instruction or a real paid Duel.");
        foreach (var id in new[] { first, cold })
            Need(damage.G.CardMovements.Count(move => move.CardId == id && move.From == CardLocation.Hand(0) &&
                    move.To == CardLocation.Processing) == 1 &&
                damage.G.CardMovements.Count(move => move.CardId == id && move.From == CardLocation.Processing &&
                    move.To == CardLocation.DiscardPile) == 1,
                "Both threshold Slashes pay and retire their original physical material exactly once.");

        var prior = Scene.Start(new[] { new ShanheStartingCard(CardKind.ColdSlash) }
            .Concat(Enumerable.Range(0, 5).Select(_ => new ShanheStartingCard(CardKind.Dodge))).ToArray(), [PriorJingyuMoulue]);
        prior.Card(CardKind.ColdSlash);
        Need(prior.V.HandCount == 5 && prior.Damage() is [{ SourceSeat: 0, TargetSeat: 1, Amount: 2, Nature: DamageNature.Cold }],
            "The actual prior Shanhe 2195 program also includes paid Cold Slash under its distinct less-than-six threshold.");
    }

    private static void Need(bool valid, string message) { if (!valid) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public PendingDecision? P => G.PendingDecision;
        public PlayerSnapshot V => G.CreateSnapshot(0).Players[0];
        public DamageAppliedEvent[] Damage() => G.Events.Select(item => item.Payload).OfType<DamageAppliedEvent>().ToArray();
        public void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected actual hand-stock tactic command."); }
        public void Play() => Until(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public int Card(CardKind kind)
        {
            var id = V.Hand.Single(card => card.Kind == kind).Id;
            var action = G.GetHumanLegalActions().First(action => action.CardId == id && action.TargetSeats.SequenceEqual([1]));
            Accept(new PlayCardCommand(0, id, action.TargetSeats, G.Revision, P!.PromptId,
                action.PlayedCardKind, action.TargetCardId) { ConversionSource = action.ConversionSource });
            Play(); return id;
        }
        public void Until(Func<bool> done)
        {
            for (var step = 0; step < 120; step++)
            {
                if (done()) return;
                Need(G.State.Status != EngineStatus.Completed && P?.PlayerSeat != 0,
                    "Unexpected native hand-stock boundary: " + JsonSerializer.Serialize(P));
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Missing bounded actual hand-stock boundary: " + JsonSerializer.Serialize(P));
        }
        public static Scene Start(IReadOnlyList<ShanheStartingCard> cards, string[] tactics)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheHandStockTacticsPackage(),
                new Fixture(tactics.Contains(PriorJingyuMoulue, StringComparer.Ordinal)));
            var setup = new ShanheBattleSetup(83, 1, "fixture:hand-stock", Owner, 3, 0, 0, 0, 0,
                [], cards, [], [new(Enemy) { StartingHand = [] }], [], new string('c', 64)) { TacticSkillIds = tactics };
            var game = GameEngine.CreateStandard(new GameOptions
            {
                Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
                ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = true,
                AdvanceAfterHumanCommands = false, MaxTurns = 4, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, registry);
            var scene = new Scene(game); scene.Accept(new StartGameCommand()); scene.Play(); return scene;
        }
    }

    private sealed class Fixture(bool includePriorTactic) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shanhe-hand-stock", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var cold = CardCatalog.Get(CardKind.ColdSlash);
            builder.AddCard(new("fixture:hand-stock-cold-slash", cold.DisplayName, cold.CategoryName,
                cold.Description, CardKind.ColdSlash));
            if (includePriorTactic)
            {
                static string Resource(string suffix)
                {
                    var assembly = typeof(StandardContentPackage).Assembly;
                    var name = assembly.GetManifestResourceNames().Single(name => name.EndsWith("." + suffix, StringComparison.Ordinal));
                    using var stream = assembly.GetManifestResourceStream(name)!;
                    using var reader = new StreamReader(stream);
                    return reader.ReadToEnd();
                }
                var production = SkillProgramCatalog.Load(Resource("shanhe-tactics-2026.rules.json"),
                    Resource("shanhe-tactics-2026.presentation.json"));
                var presentation = production.Presentations[PriorJingyuMoulue];
                builder.AddSkill(new(PriorJingyuMoulue, presentation.Name, presentation.Description)
                {
                    Program = production.Programs[PriorJingyuMoulue], ProgramPresentation = presentation,
                    Tags = SkillTag.Locked, CampaignTacticAcquisition = new(PriorJingyuMoulue, Repeatable: true)
                });
            }
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Driver}}","revision":1,
                  "modifiers":[{"id":"slash-quota","query":"slashLimit","operation":"add","value":4,"priority":0}],
                  "activations":[{"id":"hit","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,
                    "targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":1}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Driver] = new { name = "储牌实战驱动", description = "原生杀与独立技能伤害。" } } }));
            builder.AddSkill(new(Driver, "储牌实战驱动", "原生杀与独立技能伤害")
                { Program = catalog.Programs[Driver], ProgramPresentation = catalog.Presentations[Driver] });
            builder.AddSkill(new(Quiet, "普通守军", "没有额外能力"));
            builder.AddGeneral(new(Owner, "储牌者", "supporter", Driver, "qun", 3));
            builder.AddGeneral(new(Enemy, "守军", "supporter", Quiet, "wei", 20));
            builder.AddDeck(new(Deck, "零常规摸牌固定牌堆", 0, 0, [])
                { PhysicalCards = Enumerable.Range(0, 40).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "手牌容量实战", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
