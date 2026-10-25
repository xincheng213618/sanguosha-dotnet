using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheLegendaryTacticBuildChecks
{
    private const string Owner = "fixture:legendary-tactic-owner", Driver = "fixture:legendary-tactic-driver";
    private const string Quiet = "fixture:legendary-tactic-quiet", Deck = "fixture:legendary-tactic-deck";

    public static void UnlimitedSpadesOpeningArmorAndEveryDamageReflectionsUseNativeFlows()
    {
        var s = Scene.Start();
        Need(s.V().Armor == 6 && s.E<ArmorGrantedEvent>() is [{ Amount: 6 }],
            "The acquired legendary opening tactic grants six real Armor once, before the first Play phase.");
        var ordinary = s.V().Hand.Single(c => c.Kind == CardKind.Slash && c.Suit == Suit.Club).Id;
        var unused = s.V().Hand.Single(c => c.Kind == CardKind.Slash && c.Suit == Suit.Heart).Id;
        s.Slash(ordinary);
        Need(!s.G.GetHumanLegalActions().Any(a => a.CardId == unused && a.Kind == LegalActionKind.Slash),
            "An ordinary Slash genuinely consumes the normal phase quota; the non-Spade material stays unusable.");
        foreach (var card in s.V().Hand.Where(c => c.Kind == CardKind.Slash && c.Suit == Suit.Spade).ToArray()) s.Slash(card.Id);
        Need(s.E<CardUsedEvent>().Count(e => e.SourceSeat == 0 && e.CardKind == CardKind.Slash) == 3 &&
            s.V().Hand.Any(c => c.Id == unused),
            "Both real Spade Slashes remain usable after the ordinary quota is spent, without freeing a Heart Slash.");

        s.Use("incoming-one");
        Need(s.V().Hp == 16 && s.V().Armor == 5 && s.E<DamageAppliedEvent>().Count(e => e.SourceSeat == 0 && e.SourceCardUseResolutionId is null) == 2,
            "Real Armor absorbs the incoming point while both received-damage tactics each issue one native reflection.");
        s.Use("incoming-eight");
        var incoming = s.E<DamageAppliedEvent>().Where(e => e.TargetSeat == 0).ToArray();
        var reflections = s.E<DamageAppliedEvent>().Where(e => e.SourceSeat == 0 && e.SourceCardUseResolutionId is null).ToArray();
        Need(s.V().Hp == 13 && s.V().Armor == 0 && incoming.Length == 2 && reflections.Length == 4 &&
            reflections.All(e => e.Amount == 1 && e.TargetSeat is >= 1 and <= 3) &&
            reflections.Count(e => e.TargetSeat == 1) >= 2,
            "A second hit in the same actual turn repeats both reflections once per damage event, never once per damage point or only once per turn.");
        s.Use("lose");
        Need(s.V().Hp == 12 && s.E<DamageAppliedEvent>().Length == incoming.Length + reflections.Length + 3,
            "Losing HP is not received damage and cannot activate either native reflection tactic.");
        var checkpoint = GameCheckpointJson.Serialize(s.G.CreateCheckpoint());
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(checkpoint), s.Registry);
        Need(GameCheckpointJson.Serialize(restored.CreateCheckpoint()) == checkpoint &&
            SnapshotJson.Serialize(restored.CreateSnapshot(0)) == SnapshotJson.Serialize(s.G.CreateSnapshot(0)),
            "The current build reloads its paid Armor, actual Slash debits, random enemy choices and native reflection facts.");
    }

    private static void Need(bool value, string message)
    { if (!value) throw new InvalidOperationException(message); }

    private static string Peer(int seat) => "fixture:legendary-tactic-peer-" + seat;

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G { get; } = game;
        public ContentRegistry Registry { get; } = registry;
        private PendingDecision? P => G.PendingDecision;
        public PlayerSnapshot V() => G.CreateSnapshot(0).Players[0];
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        private void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native legendary-tactic command."); }
        public void Play()
        {
            for (var step = 0; step < 180; step++)
            {
                if (P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }) return;
                if (P is { PlayerSeat: 0, Choices.Count: > 0 } prompt)
                    Accept(new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices[0].Id, G.Revision));
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Missing bounded native tactic Play boundary.");
        }
        public void Slash(int cardId)
        {
            var action = G.GetHumanLegalActions().Single(a => a.CardId == cardId && a.TargetSeats.SequenceEqual([1]) && a.Kind == LegalActionKind.Slash);
            Accept(new PlayCardCommand(0, cardId, action.TargetSeats, G.Revision, P!.PromptId, action.PlayedCardKind, action.TargetCardId)); Play();
        }
        public void Use(string activation)
        { Accept(new UseProgramSkillCommand(0, Driver, activation, [], [1], G.Revision, P!.PromptId)); Play(); }
        public static Scene Start()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheLegendaryTacticsPackage(), new Fixture());
            var setup = new ShanheBattleSetup(83, 1, "fixture:legendary-tactic-build", Owner, 16, 0, 0, 0, 0, [],
                [new(CardKind.Slash, Suit.Spade, 7), new(CardKind.Slash, Suit.Spade, 8), new(CardKind.Slash, Suit.Club, 9), new(CardKind.Slash, Suit.Heart, 10)], [],
                Enumerable.Range(1, 3).Select(i => new ShanheCombatantDefinition(Peer(i)) { StartingHand = [] }).ToArray(), [], new string('b', 64))
            { TacticSkillIds = [StandardShanheCampaignCatalog.CurrentTouxiSkillId, StandardShanheCampaignCatalog.CurrentTiebushan4SkillId,
                StandardShanheCampaignCatalog.CurrentJingjijiaSkillId, StandardShanheCampaignCatalog.CurrentPianzhuanjiaSkillId] };
            var game = GameEngine.CreateStandard(new GameOptions
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 8, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, registry);
            var scene = new Scene(game, registry); scene.Accept(new StartGameCommand()); scene.Play(); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-legendary-tactic-build", new(1, 0, 0), []);
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,"activations":[
                    {"id":"incoming-one","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"owner","sourceRef":{"kind":"selectedTarget"},"amount":1}]},
                    {"id":"incoming-eight","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"owner","sourceRef":{"kind":"selectedTarget"},"amount":8}]},
                    {"id":"lose","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"loseHp","target":"owner","amount":1}]}]},
                  {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}
                ]}
                """, JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new[] { Driver, Quiet }.ToDictionary(id => id, _ => new { name = "原生构筑驱动", description = "真实伤害与真实失去体力" })
            }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Owner, "传说构筑使用者", "supporter", Driver, "qun", 16));
            foreach (var seat in Enumerable.Range(1, 3)) builder.AddGeneral(new(Peer(seat), "固定敌军", "supporter", Quiet, "wei", 16));
            builder.AddDeck(new(Deck, "固定实体牌", 0, 0, [])
            { PhysicalCards = Enumerable.Range(0, 80).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Diamond, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "传说战法构筑实战", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer(1), Peer(2), Peer(3)], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}
