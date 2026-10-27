using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static partial class ZhouYuChecks
{
    private const string Hero = "mobile:shi-zhou-yu",
        Quiet = "fixture:shi-zhou-yu-quiet", Keeper = "fixture:shi-zhou-yu-keeper",
        Drawer = "fixture:shi-zhou-yu-drawer", Deck = "fixture:shi-zhou-yu-deck",
        Peer = "fixture:shi-zhou-yu-peer";

    private static void AnswerXiongzi(Scene s, string branch)
    {
        s.Reach(() => s.P is { PlayerSeat: 0, Kind: DecisionKind.ProgramTrigger } &&
            s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "keep1"));
        s.Answer(c => c.Parameters.GetValueOrDefault("branch") == branch);
    }

    private static void GiveViaChiyun(Scene s, Func<PromptChoice, bool>? pick = null)
    {
        var before = s.E<ShiZhouYuChiyunOptionEvent>().Length;
        s.Reach(() => s.P is { PlayerSeat: 0, Kind: DecisionKind.ProgramTrigger } &&
            s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "target"));
        s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "target" &&
            c.Parameters.GetValueOrDefault("target") == "1");
        for (var i = 0; i < 2; i++)
        {
            s.Reach(() => s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "pick"));
            s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "pick" && (pick?.Invoke(c) ?? true));
        }
        s.Reach(() => s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "confirm"));
        s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "confirm");
        s.Reach(() => s.E<ShiZhouYuChiyunOptionEvent>().Length >= before + 1);
    }

    private sealed record Enemy(int Hp, int Dodges = 0);

    private static Enemy[] NoEnemies() => [new Enemy(8), new Enemy(8), new Enemy(8), new Enemy(8)];

    private sealed class Scene(GameEngine game, ContentRegistry registry, ShanheStartingCard[] opening)
    {
        public GameEngine G { get; private set; } = game;
        public ShanheStartingCard[] Opening { get; } = opening;
        public PendingDecision? P => Enumerable.Range(0, 5).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();

        public int IdOf(ShanheStartingCard card) => V(0).Hand
            .Single(c => (int)c.Kind == (int)card.Kind && c.Rank == card.Rank).Id;
        public void Use(int cardId)
        {
            var actions = G.GetHumanLegalActions();
            var a = actions.FirstOrDefault(x => x.CardId == cardId);
            Need(a is not null, $"Card {cardId} has no legal action. Available: " +
                JsonSerializer.Serialize(actions.Select(x => new { x.Kind, x.CardId, x.PlayedCardKind }).Take(30)));
            Accept0(new PlayCardCommand(0, a!.CardId!.Value, a.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind));
        }
        public void Answer(Func<PromptChoice, bool> pick)
        {
            var p = P!; var choice = p.Choices.FirstOrDefault(pick); Need(choice is not null, "The expected prompt choice must be published.");
            Accept0(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, choice!.Id, G.Revision));
        }
        public void End()
        {
            // The play phase can auto-end when no legal action remains; tolerate that.
            if (P is { } p) Accept0(new EndPlayPhaseCommand(0, G.Revision, p.PromptId));
        }
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Slash(int target)
        {
            Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
            var a = G.GetHumanLegalActions().First(a => a.Kind == LegalActionKind.Slash && a.TargetSeats.SequenceEqual([target]));
            Accept0(new PlayCardCommand(0, a.CardId!.Value, a.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind));
        }
        public void Reach(Func<bool> ready)
        {
            for (var i = 0; i < 2400; i++)
            {
                if (ready()) return;
                if (P is { } p)
                {
                    if (p.Kind == DecisionKind.SelectFaction) { Answer(_ => true); continue; }
                    if (p.PlayerSeat == 0 && p.Kind == DecisionKind.PlayCard) { End(); continue; }
                    if (p.PlayerSeat == 0 && p.Kind == DecisionKind.DiscardCards) throw new InvalidOperationException("An unhandled discard boundary requires an explicit choice. " + Dump());
                    if (p.PlayerSeat == 0 && p.Kind is DecisionKind.RespondDodge or DecisionKind.RespondSlash or
                        DecisionKind.Nullification or DecisionKind.RescueDying)
                    { Answer(c => c.Cards.Count == 0); continue; }
                    if (p.PlayerSeat == 0 && p.Kind != DecisionKind.PlayCard &&
                        p.Choices.Any(c => c.Parameters.GetValueOrDefault("action") == "end-play"))
                    { Answer(c => c.Parameters.GetValueOrDefault("action") == "end-play"); continue; }
                    if (p.PlayerSeat == 0) throw new InvalidOperationException("Unexpected Shi Zhou Yu boundary. " + Dump());
                }
                Need(G.State.Winner == Winner.None, "The bounded real scene must retain its participants.");
                Accept0(new AdvanceOneStepCommand(G.Revision));
            }
            Need(false, "Shi Zhou Yu did not reach the expected bounded native boundary.");
        }
        public void Cold()
        { var before = State(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry); Need(State() == before, "Cold restore preserves exact phase identity and ledger facts."); }
        public void Replay()
        {
            var before = State(); G = GameReplay.Replay(G.CreateCheckpoint().Options, G.AcceptedCommands, registry);
            Need(State() == before, "The short actual command journal reproduces the observed flow exactly.");
        }
        public void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message + " " + Dump()); }
        private void Accept0(GameCommand command)
        { var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, result.Error?.Message ?? "Rejected Shi Zhou Yu command."); }
        private string State() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 5).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),
            Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray(),
            G.CardMovements, Zones = G.CreateCardZoneDiagnostics(), Commands = CommandJson.Serialize(G.AcceptedCommands) });
        private string Dump() => JsonSerializer.Serialize(new { G.State.TurnNumber, G.State.CurrentSeat, P,
            Players = Enumerable.Range(0, 5).Select(i => new { i, V(i).Hp, V(i).HandCount, V(i).IsChained }),
            Given = E<ShiZhouYuChiyunGivenEvent>().Select(e => new { e.TargetSeat, Count = e.CardIds.Count }),
            Options = E<ShiZhouYuChiyunOptionEvent>().Select(e => new { e.TargetSeat, e.Option }),
            Seen = E<ShiZhouYuChiyunPhaseSeenEvent>().Select(e => new { e.TurnNumber, e.Phase }),
            Shown = E<ShiZhouYuYanhuiShownEvent>().Select(e => new { e.HolderSeat, e.CardId, e.Discarded }),
            Ending = E<ShiZhouYuYanhuiEndingChosenEvent>().Select(e => new { e.Option, e.TargetSeat, e.DrawCount }),
            Fentao = E<ShiZhouYuFentaoChosenEvent>().Select(e => new { e.TargetSeat, e.Option, Count = e.DiscardedCardIds.Count }),
            Rechained = E<ShiZhouYuFentaoRechainedEvent>().Select(e => e.TargetSeat),
            Xiongzi = E<ShiZhouYuXiongziActivatedEvent>().Select(e => new { e.OwnerSeat, e.KeptOption }),
            Damage = E<DamageAppliedEvent>().Select(e => new { e.TargetSeat, e.Amount }),
            Moves = G.CardMovements.TakeLast(40).Select(m => new { m.CardId, From = m.From.Zone, FromSeat = m.From.OwnerSeat, To = m.To.Zone, ToSeat = m.To.OwnerSeat, Reason = m.Reason.Value }),
            Last = G.Events.TakeLast(6).Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray() });
        public static Scene Start(string nodeId, int heroHp, string[] heroExtras, Enemy[] enemies, ShanheStartingCard[] opening)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            ShanheCombatantDefinition Combatant(int seat, Enemy enemy) => new(Peer, Hp: enemy.Hp, ExtraSkillIds: [Quiet])
            { StartingHand = Enumerable.Range(0, enemy.Dodges).Select(i => new ShanheStartingCard(CardKind.Dodge, Suit.Heart, 7 + i)).ToArray() };
            var setup = new ShanheBattleSetup(96, 1, nodeId, Hero, heroHp, heroHp - 4, 0, 0, 0, heroExtras, opening, [],
                enemies.Select((enemy, index) => Combatant(index + 1, enemy)).ToArray(), [], new string('e', 64));
            var game = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 5, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = true, AdvanceAfterHumanCommands = false, MaxTurns = 20 }, registry);
            var scene = new Scene(game, registry, opening); scene.Accept0(new StartGameCommand()); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shi-zhou-yu", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero);
            var rules = $$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
              {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"no-draw","window":"drawPhaseStarting","subject":"owner","optional":false,"isSkillInvocation":false,
                "effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]},
                {"id":"no-play","window":"afterNormalDraw","subject":"owner","optional":false,"isSkillInvocation":false,
                "effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]},
              {"id":"{{Keeper}}","revision":1,"modifiers":[{"id":"keep","query":"handLimit","operation":"add","value":10,"priority":0},
                {"id":"many","query":"slashLimit","operation":"add","value":10,"priority":0}]},
              {"id":"{{Drawer}}","revision":1,"triggers":[{"id":"draw-two","window":"drawPhaseStarting","subject":"owner","optional":false,"isSkillInvocation":false,
                "effects":[{"op":"draw","target":"owner","amount":2}]}]}
            ]}
            """;
            var presentations = new Dictionary<string, object>
            {
                [Quiet] = new { name = Quiet, description = "安静真实回合并跳过普通摸牌出牌。" },
                [Keeper] = new { name = Keeper, description = "保持手牌并无限制出杀。" },
                [Drawer] = new { name = Drawer, description = "摸牌阶段开始时摸两张牌。" },
            };
            var catalog = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = presentations }));
            foreach (var (id, program) in catalog.Programs) b.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
            { Program = program, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new(Peer, "安静真实同桌", "", Quiet, "wei", 8));
            b.AddCard(new("fixture:shi-zhou-yu-slash", "杀", "基本牌", "同体杀实体", CardKind.Slash));
            b.AddCard(new("fixture:shi-zhou-yu-fire-slash", "火杀", "基本牌", "同体火杀实体", CardKind.FireSlash));
            b.AddCard(new("fixture:shi-zhou-yu-dodge", "闪", "基本牌", "同体闪实体", CardKind.Dodge));
            b.AddDeck(new(Deck, "固定真实牌堆", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 160)
                .Select(i => i % 7 == 3
                    ? new ContentDeckPhysicalCard("fixture:shi-zhou-yu-dodge", Suit.Heart, 7)
                    : i % 11 == 5
                        ? new ContentDeckPhysicalCard("fixture:shi-zhou-yu-fire-slash", Suit.Heart, 9)
                        : new ContentDeckPhysicalCard("fixture:shi-zhou-yu-slash", i % 2 == 0 ? Suit.Heart : Suit.Club, 5)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "势周瑜实际检查", 5, 5, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 4 }));
        }
    }
}
