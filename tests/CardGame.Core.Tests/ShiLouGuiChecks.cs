using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShiLouGuiChecks
{
    private const string Hero = "mobile:shi-lou-gui", GuanSha = "mobile:shi-guansha", JiYu = "mobile:shi-jiyu",
        Quiet = "fixture:shi-lou-gui-quiet", HandKeeper = "fixture:shi-lou-gui-keeper",
        Deck = "fixture:shi-lou-gui-deck", Peer = "fixture:shi-lou-gui-peer";

    public static void GuanShaReplacesAllCardsWithBasicsAndRaisesTurnHandLimit()
    {
        var s = Scene.Start("guansha", 3, [HandKeeper],
            [new Enemy(8, [Quiet]), new Enemy(8, [Quiet]), new Enemy(8, [Quiet]), new Enemy(8, [Quiet])],
            openingCards: 4); s.Play();
        s.End();
        s.ArmGuanSha();
        s.Need(s.E<ShiLgGuanshaAppliedEvent>() is [{ OwnerSeat: 0, ReplacedCount: 4, GainedCount: 4, DistinctNames: 1 }] &&
            s.V(0).HandCount == 4,
            "Arming replaces all four cards with four random basics whose only name is Slash.");
        s.Need(s.G.CardMovements.Count(m => m.Reason.Value == Scene.GuanShaReplaceReason && m.From.OwnerSeat == 0) == 4 &&
            s.G.CardMovements.Count(m => m.Reason.Value == Scene.GuanShaGainReason && m.To.OwnerSeat == 0) == 4,
            "The four owned cards moved to the discard pile and four draw-pile basics moved into the hand.");
        s.Reach(() => s.G.State.TurnNumber >= 2);
        s.Need(!s.G.CardMovements.Any(m => m.Reason.Value == "rule.hand-limit-discard" && m.From.OwnerSeat == 0),
            "The +1 distinct-name hand limit keeps all four gained cards through the discard phase.");
        s.Reach(() => s.G.State.TurnNumber >= 7 || (s.G.State.TurnNumber == 6 && s.G.State.CurrentSeat != 0));
        s.Need(s.E<ShiLgGuanshaAppliedEvent>().Length == 1,
            "The limited skill never re-arms in a later play ending.");
        s.Cold(); s.Replay();
    }

    public static void JiYuGainsOtherCategoriesAndReArmsAfterUsingEveryGain()
    {
        var s = Scene.Start("jiyu", 3, [HandKeeper],
            [new Enemy(8, [Quiet]), new Enemy(8, [Quiet]), new Enemy(8, [Quiet]), new Enemy(8, [Quiet])],
            openingCards: 4); s.Play();
        s.ActivateJiYu();
        s.PayJiYuCost(Scene.GainedKind.Basic);
        s.Need(s.E<ShiLgJiYuCostPaidEvent>() is [{ OwnerSeat: 0 }] &&
            s.E<ShiLgJiYuGainedEvent>().Select(e => e.GainOrdinal).ToArray() is [1] &&
            s.V(0).HandCount == 4,
            "Discarding the basic gains the only other category present (a trick), for a net hand of four.");
        s.Reach(() => s.P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        s.TryActivateJiYu();
        s.Need(s.E<ShiLgJiYuCostPaidEvent>().Length == 1,
            "The re-arm gate keeps the second activation closed while the gained trick remains unused.");
        s.PlayGained(Scene.GainedKind.Trick, 1);
        s.Reach(() => s.P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        s.TryActivateJiYu();
        s.PayJiYuCost(Scene.GainedKind.Basic);
        s.Need(s.E<ShiLgJiYuCostPaidEvent>().Length == 2,
            "Using the gained trick re-arms the second per-phase activation.");
        s.Reach(() => s.P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        s.TryActivateJiYu();
        s.Need(s.E<ShiLgJiYuCostPaidEvent>().Length == 2,
            "The twice-per-phase cap keeps the third activation closed.");
        s.Cold(); s.Replay();
    }

    private sealed record Enemy(int Hp, string[] Extras);

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public const string GuanShaReplaceReason = "skill-program.mobile:shi-guansha.replace";
        public const string GuanShaGainReason = "skill-program.mobile:shi-guansha.gain";

        public enum GainedKind { Trick, Basic }

        public GameEngine G { get; private set; } = game;
        public PendingDecision? P => Enumerable.Range(0, 5).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public bool Offered(string skill) =>
            G.GetHumanLegalActions().Any(a => a.Kind == LegalActionKind.UseProgramSkill && a.ProgramSkillId == skill);

        private int _gainedOrdinal;

        public void ArmGuanSha()
        {
            Reach(() => P is { PlayerSeat: 0 } p && p.Kind == DecisionKind.ProgramTrigger &&
                p.SkillPrompt?.SkillId == GuanSha && p.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "arm"));
            Answer(c => c.Parameters.GetValueOrDefault("branch") == "arm");
        }

        public void ActivateJiYu()
        {
            Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } && Offered(JiYu));
            Accept0(new UseProgramSkillCommand(0, JiYu, "jiyu-use", [], [], G.Revision, P!.PromptId));
        }

        // The legality gate declines the start gracefully; the command may be
        // rejected or accepted as a no-op, so only the cost ledger asserts.
        public void TryActivateJiYu()
        {
            var p = P!;
            var command = new UseProgramSkillCommand(0, JiYu, "jiyu-use", [], [], G.Revision, p.PromptId);
            G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single());
        }

        public void PayJiYuCost(GainedKind costKind)
        {
            Reach(() => P is { PlayerSeat: 0 } p && p.Kind == DecisionKind.ProgramTrigger &&
                p.SkillPrompt?.SkillId == JiYu && p.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "cost"));
            var expectedKind = costKind == GainedKind.Trick ? CardKind.Duel : CardKind.Slash;
            var costId = V(0).Hand.Where(card => card.Kind == expectedKind).Select(card => card.Id).First();
            Answer(c => c.Parameters.GetValueOrDefault("branch") == "cost" && c.Cards.Contains(costId));
        }

        public void PlayGained(GainedKind kind, int ordinal)
        {
            var expectedKind = kind == GainedKind.Trick ? CardKind.Duel : CardKind.Slash;
            var gainedIds = G.CardMovements.Where(m => m.Reason.Value == "skill-program.mobile:shi-jiyu.gain" && m.To.OwnerSeat == 0)
                .Select(m => m.CardId).ToHashSet();
            var targetId = V(0).Hand.Where(card => card.Kind == expectedKind && gainedIds.Contains(card.Id))
                .Select(card => card.Id).Skip(ordinal - 1).First();
            Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } &&
                G.GetHumanLegalActions().Any(a => a.CardId == targetId && a.TargetSeats.Contains(1)));
            var a = G.GetHumanLegalActions().First(a => a.CardId == targetId && a.TargetSeats.Contains(1));
            Accept0(new PlayCardCommand(0, a.CardId!.Value, a.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind));
        }

        public void Answer(Func<PromptChoice, bool> pick)
        {
            var p = P!; var choice = p.Choices.FirstOrDefault(pick); Need(choice is not null, "The expected prompt choice must be published.");
            Accept0(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, choice!.Id, G.Revision));
        }

        public void Step() => Accept0(new AdvanceOneStepCommand(G.Revision));
        public void End() => Accept0(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });

        public void Reach(Func<bool> ready)
        {
            for (var i = 0; i < 2200; i++)
            {
                if (ready()) return;
                if (P is { } p)
                {
                    if (p.Kind == DecisionKind.SelectFaction) { Answer(_ => true); continue; }
                    if (p.PlayerSeat == 0 && p.Kind == DecisionKind.PlayCard) { End(); continue; }
                    if (p.PlayerSeat == 0 && p.SkillPrompt?.SkillId == GuanSha &&
                        p.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "pass"))
                    { Answer(c => c.Parameters.GetValueOrDefault("branch") == "pass"); continue; }
                    if (p.PlayerSeat == 0 && p.SkillPrompt?.SkillId is GuanSha or JiYu)
                        throw new InvalidOperationException("Unexpected Lou Gui boundary. " + Dump());
                    if (p.PlayerSeat == 0 && p.Kind is DecisionKind.RespondDodge or DecisionKind.RespondSlash or DecisionKind.Nullification or DecisionKind.RescueDying)
                    { Answer(c => c.Cards.Count == 0); continue; }
                    if (p.PlayerSeat == 0 && p.Kind != DecisionKind.PlayCard && p.SkillPrompt?.SkillId is not (GuanSha or JiYu) &&
                        p.Choices.Any(c => c.Parameters.GetValueOrDefault("action") == "end-play"))
                    { Answer(c => c.Parameters.GetValueOrDefault("action") == "end-play"); continue; }
                    if (p.PlayerSeat == 0) throw new InvalidOperationException("Unexpected Lou Gui boundary. " + Dump());
                }
                Need(G.State.Winner == Winner.None, "The bounded real scene must retain its participants.");
                Accept0(new AdvanceOneStepCommand(G.Revision));
            }
            Need(false, "Lou Gui did not reach the expected bounded native boundary.");
        }

        public void Cold()
        { var before = State(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry); Need(State() == before, "Cold restore preserves exact phase identity and ledger facts."); }
        public void Replay()
        {
            var before = State(); G = GameReplay.Replay(G.CreateCheckpoint().Options, G.AcceptedCommands, registry);
            Need(State() == before, "The short actual command journal reproduces the activations exactly.");
        }
        public void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message + " " + Dump()); }
        private void Accept0(GameCommand command)
        { var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, result.Error?.Message ?? "Rejected Lou Gui command."); }
        private string State() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 5).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),
            Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray(),
            G.CardMovements, Zones = G.CreateCardZoneDiagnostics(), Commands = CommandJson.Serialize(G.AcceptedCommands) });
        private string Dump() => JsonSerializer.Serialize(new { G.State.TurnNumber, G.State.CurrentSeat, G.State.Winner, P,
            Players = Enumerable.Range(0, 5).Select(i => new { i, V(i).Hp, V(i).HandCount }),
            Applied = E<ShiLgGuanshaAppliedEvent>(), Cost = E<ShiLgJiYuCostPaidEvent>(), Gained = E<ShiLgJiYuGainedEvent>(),
            Damage = E<DamageAppliedEvent>().Select(e => new { e.TargetSeat, e.Amount }),
            Moves = G.CardMovements.TakeLast(40).Select(m => new { m.CardId, From = m.From.Zone, FromSeat = m.From.OwnerSeat, To = m.To.Zone, ToSeat = m.To.OwnerSeat, Reason = m.Reason.Value }),
            Last = G.Events.TakeLast(6).Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray() });

        public static Scene Start(string nodeId, int heroHp, string[] heroExtras, Enemy[] enemies, int openingCards = 4)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            ShanheStartingCard[] opening = Enumerable.Range(0, openingCards)
                .Select(i => i == openingCards - 1
                    ? new ShanheStartingCard(CardKind.Duel, Suit.Spade, 6)
                    : new ShanheStartingCard(CardKind.Slash, Suit.Club, 4 + i % 9)).ToArray();
            ShanheCombatantDefinition Combatant(int seat, Enemy enemy) => new(Peer, Hp: enemy.Hp, ExtraSkillIds: enemy.Extras)
            { StartingHand = [] };
            var setup = new ShanheBattleSetup(96, 1, nodeId, Hero, heroHp, heroHp - 3, 0, 0, 0, heroExtras, opening, [],
                enemies.Select((enemy, index) => Combatant(index + 1, enemy)).ToArray(), [], new string('e', 64));
            var game = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 5, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 9 }, registry);
            var scene = new Scene(game, registry); scene.Accept0(new StartGameCommand()); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shi-lou-gui", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero);
            var rules = $$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
              {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"no-draw","window":"drawPhaseStarting","subject":"owner","optional":false,"isSkillInvocation":false,
                "effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]},
                {"id":"no-play","window":"afterNormalDraw","subject":"owner","optional":false,"isSkillInvocation":false,
                "effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]},
              {"id":"{{HandKeeper}}","revision":1,"modifiers":[{"id":"keep","query":"handLimit","operation":"add","value":10,"priority":0},{"id":"many","query":"slashLimit","operation":"add","value":10,"priority":0}]}
            ]}
            """;
            var presentations = new Dictionary<string, object>
            {
                [Quiet] = new { name = Quiet, description = "安静真实回合并跳过普通摸牌出牌。" },
                [HandKeeper] = new { name = HandKeeper, description = "保持手牌并无限制出杀。" },
            };
            var catalog = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = presentations }));
            foreach (var (id, program) in catalog.Programs) b.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
            { Program = program, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new(Peer, "安静真实同桌", "", Quiet, "wei", 8));
            b.AddCard(new("fixture:shi-lou-gui-slash", "杀", "基本牌", "同体杀实体", CardKind.Slash));
            b.AddCard(new("fixture:shi-lou-gui-duel", "决斗", "锦囊牌", "同体决斗实体", CardKind.Duel));
            b.AddDeck(new(Deck, "固定真实混合牌堆", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 150)
                .Select(i => i % 3 == 0
                    ? new ContentDeckPhysicalCard("fixture:shi-lou-gui-duel", Suit.Spade, 6)
                    : new ContentDeckPhysicalCard("fixture:shi-lou-gui-slash", Suit.Club, 5)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "势娄圭实际检查", 5, 5, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 4 }));
        }
    }
}
