using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShiZangHongChecks
{
    private const string Hero = "mobile:shi-zang-hong", Liezhi = "mobile:shi-liezhi", Juegu = "mobile:shi-juegu",
        Dry = "fixture:shi-zang-hong-dry", Keeper = "fixture:shi-zang-hong-keeper",
        Deck = "fixture:shi-zang-hong-deck", Peer = "fixture:shi-zang-hong-peer";

    public static void LiezhiWoundedPeachRecoversForPaidMaximumHp()
    {
        var s = Scene.Start("liezhi-peach", 4, [Keeper],
            [new Enemy(8, [Dry]) { StartingHand = [new ShanheStartingCard(CardKind.Slash, Suit.Club, 9)] },
                new Enemy(8, [Dry]) { StartingHand = [new ShanheStartingCard(CardKind.Duel, Suit.Spade, 6)] },
                new Enemy(8, [Dry]), new Enemy(8, [Dry])],
            openingOverride: [new ShanheStartingCard(CardKind.Dodge, Suit.Heart, 6),
                new ShanheStartingCard(CardKind.Slash, Suit.Club, 12)]);
        s.JueguInvokeBranch = "skip";
        s.Play();
        s.End();
        s.Reach(() => s.G.State.TurnNumber >= 6 && s.P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        s.Need(s.V(0).Hp == 2 && s.V(0).MaxHp == 4 &&
            s.E<DamageAppliedEvent>().Count(e => e.TargetSeat == 0) == 2,
            "A declined slash and a lost duel left the owner wounded at two of four maximum HP.");
        s.Need(s.Offered(Liezhi), "The wounded owner with a reachable Peach option is offered LieZhi.");
        s.LiezhiBranch = "Peach:0";
        s.ActivateLiezhi();
        s.Reach(() => s.E<RecoveryAppliedEvent>().Any(e => e.TargetSeat == 0));
        s.Need(s.E<ShiZhLiezhiPaidEvent>() is [{ OwnerSeat: 0, MaxHpAfter: 3 }] &&
            s.E<MaximumHpChangedEvent>().Any(e => e.PlayerSeat == 0 && e.Delta == -1 && e.MaximumHp == 3),
            "Activating LieZhi paid one maximum HP down to three.");
        s.Need(s.V(0).Hp == 3 && s.V(0).MaxHp == 3 &&
            s.E<CardUseDeclaredEvent>().Any(e => e.SourceSeat == 0 && e.CardKind == CardKind.Peach) &&
            s.E<RecoveryAppliedEvent>().Any(e => e.TargetSeat == 0 && e.Amount == 1),
            "The virtual Peach recovered one HP for the paid maximum.");
        s.Cold(); s.Replay();
    }

    public static void LiezhiWineAtFullHpClampsAndAppliesAlcohol()
    {
        var s = Scene.Start("liezhi-wine", 4, [Keeper],
            [new Enemy(8, [Dry]), new Enemy(8, [Dry]), new Enemy(8, [Dry]), new Enemy(8, [Dry])],
            openingOverride: [new ShanheStartingCard(CardKind.Dodge, Suit.Heart, 6),
                new ShanheStartingCard(CardKind.Slash, Suit.Club, 12)]);
        s.Play();
        s.Need(s.Offered(Liezhi), "A full-HP owner may still pay for the Wine option.");
        s.LiezhiBranch = "Alcohol:0";
        s.ActivateLiezhi();
        s.Reach(() => s.E<AlcoholAppliedEvent>().Any(e => e.SourceSeat == 0));
        s.Need(s.E<ShiZhLiezhiPaidEvent>() is [{ OwnerSeat: 0, MaxHpAfter: 3 }] && s.V(0).MaxHp == 3 && s.V(0).Hp == 3,
            "The maximum-HP payment clamped the owner's full HP to three.");
        s.Need(s.E<CardUseDeclaredEvent>().Any(e => e.SourceSeat == 0 && e.CardKind == CardKind.Alcohol) &&
            s.E<AlcoholAppliedEvent>().Any(e => e.SourceSeat == 0),
            "The virtual Wine applied its alcohol effect without a physical card.");
        s.Need(!s.Offered(Liezhi),
            "With the alcohol effect active no virtual option remains, so LieZhi is hidden.");
        s.Cold(); s.Replay();
    }

    public static void JueguDrawsAndDiscardsGrowCountingWithinTheTurn()
    {
        var s = Scene.Start("juegu-grow", 4, [Keeper],
            [new Enemy(8, [Dry]) { StartingHand = [new ShanheStartingCard(CardKind.Duel, Suit.Spade, 6),
                    new ShanheStartingCard(CardKind.Slash, Suit.Club, 9), new ShanheStartingCard(CardKind.Slash, Suit.Club, 10)] },
                new Enemy(8, [Dry]), new Enemy(8, [Dry]), new Enemy(8, [Dry])],
            openingOverride: [new ShanheStartingCard(CardKind.Slash, Suit.Club, 5),
                new ShanheStartingCard(CardKind.Slash, Suit.Club, 12),
                new ShanheStartingCard(CardKind.Dodge, Suit.Heart, 6)]);
        s.DuelWinBranch = true;
        s.JueguInvokeBranch = "activate";
        s.Play();
        s.End();
        s.Reach(() => s.E<ShiZhJueguUsedEvent>() is [{ Turn: 2 }, { Turn: 2 }] && s.V(0).Hp == 3);
        s.Need(s.E<ShiZhJueguUsedEvent>() is [{ OwnerSeat: 0, Turn: 2 }, { OwnerSeat: 0, Turn: 2 }],
            "Both same-turn targetings while unwounded resolved JuGu once each.");
        s.Need(s.V(0).Hp == 3 && s.V(0).MaxHp == 4 && s.V(1).Hp == 7,
            "The duel's damage hit the contestant while the Slash wounded the owner once.");
        var discards = s.G.CardMovements.Where(m => m.Reason.Value == "skill-program.mobile:shi-juegu.discard").ToArray();
        s.Need(discards.Length == 3 && discards.All(m => m.To == CardLocation.DiscardPile && m.From.Zone == CardZoneKind.Hand),
            "The first JuGu discarded one card and the second discarded two grown cards.");
        s.Need(s.V(0).HandCount == 3,
            "The draw-then-discard flow left the owner with three hand cards.");
        s.Cold(); s.Replay();
    }

    private sealed record Enemy(int Hp, string[] Extras)
    {
        public ShanheStartingCard[] StartingHand { get; init; } = [];
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G { get; private set; } = game;
        public PendingDecision? P => Enumerable.Range(0, 5).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public bool Offered(string skill) =>
            G.GetHumanLegalActions().Any(a => a.Kind == LegalActionKind.UseProgramSkill && a.ProgramSkillId == skill);

        public string LiezhiBranch { get; set; } = "Peach:0";
        public string JueguInvokeBranch { get; set; } = "activate";
        public bool DuelWinBranch { get; set; }

        public void ActivateLiezhi()
        {
            Accept0(new UseProgramSkillCommand(0, Liezhi, "liezhi-virtual", [], [], G.Revision, P!.PromptId));
        }

        public void End()
        {
            Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
            Accept0(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
        }
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });

        public void Reach(Func<bool> ready)
        {
            for (var i = 0; i < 2600; i++)
            {
                if (ready()) return;
                if (P is { } p)
                {
                    if (p.Kind == DecisionKind.SelectFaction) { Answer(_ => true); continue; }
                    if (p.PlayerSeat == 0 && p.Kind == DecisionKind.PlayCard) { End(); continue; }
                    if (p.PlayerSeat == 0 && p.SkillPrompt?.SkillId == Liezhi &&
                        p.Choices.Any(c => c.Parameters.GetValueOrDefault("basic-option") is not null))
                    { Answer(c => c.Parameters.GetValueOrDefault("basic-option") == LiezhiBranch); continue; }
                    if (p.PlayerSeat == 0 && p.SkillPrompt?.SkillId == Juegu &&
                        p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") is "activate" or "skip"))
                    { Answer(c => c.Parameters.GetValueOrDefault("program-action") == JueguInvokeBranch); continue; }
                    if (p.PlayerSeat == 0 && p.SkillPrompt?.SkillId is Juegu &&
                        p.Choices.Any(c => c.Parameters.GetValueOrDefault("branch")?.StartsWith("discard-", StringComparison.Ordinal) == true))
                    { Answer(c => c.Parameters.GetValueOrDefault("branch")?.StartsWith("discard-", StringComparison.Ordinal) == true); continue; }
                    if (p.PlayerSeat == 0 && p.Kind is DecisionKind.RespondDodge or DecisionKind.RespondSlash or DecisionKind.Nullification or DecisionKind.RescueDying)
                    {
                        if (DuelWinBranch && p.Kind == DecisionKind.RespondSlash && p.Choices.Any(c => c.Cards.Count == 1))
                        { DuelWinBranch = false; Answer(c => c.Cards.Count == 1); continue; }
                        Answer(c => c.Cards.Count == 0);
                        continue;
                    }
                    if (p.PlayerSeat == 0 && p.Kind != DecisionKind.PlayCard &&
                        p.Choices.Any(c => c.Parameters.GetValueOrDefault("action") == "end-play"))
                    { Answer(c => c.Parameters.GetValueOrDefault("action") == "end-play"); continue; }
                    if (p.PlayerSeat == 0) throw new InvalidOperationException("Unexpected Zang Hong boundary. " + Dump());
                }
                Need(G.State.Winner == Winner.None, "The bounded real scene must retain its participants.");
                Accept0(new AdvanceOneStepCommand(G.Revision));
            }
            Need(false, "Zang Hong did not reach the expected bounded native boundary.");
        }

        public void Answer(Func<PromptChoice, bool> pick)
        {
            var p = P!; var choice = p.Choices.FirstOrDefault(pick); Need(choice is not null, "The expected prompt choice must be published.");
            Accept0(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, choice!.Id, G.Revision));
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
        { var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, result.Error?.Message ?? "Rejected Zang Hong command."); }
        private string State() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 5).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),
            Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray(),
            G.CardMovements, Zones = G.CreateCardZoneDiagnostics(), Commands = CommandJson.Serialize(G.AcceptedCommands) });
        private string Dump() => JsonSerializer.Serialize(new { G.State.TurnNumber, G.State.CurrentSeat, G.State.Winner, P,
            Players = Enumerable.Range(0, 5).Select(i => new { i, V(i).Hp, V(i).MaxHp, V(i).HandCount }),
            Paid = E<ShiZhLiezhiPaidEvent>(), Used = E<ShiZhJueguUsedEvent>(),
            Declared = E<CardUseDeclaredEvent>().Select(e => new { e.SourceSeat, e.CardKind }),
            Recovery = E<RecoveryAppliedEvent>().Select(e => new { e.TargetSeat, e.Amount }),
            Alcohol = E<AlcoholAppliedEvent>().Select(e => new { e.SourceSeat }),
            MaxHp = E<MaximumHpChangedEvent>().Select(e => new { e.PlayerSeat, e.Delta, e.MaximumHp }),
            Damage = E<DamageAppliedEvent>().Select(e => new { e.TargetSeat, e.SourceSeat, e.Amount }),
            Moves = G.CardMovements.TakeLast(40).Select(m => new { m.CardId, From = m.From.Zone, FromSeat = m.From.OwnerSeat, To = m.To.Zone, ToSeat = m.To.OwnerSeat, Reason = m.Reason.Value }),
            Last = G.Events.TakeLast(6).Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray() });

        public static Scene Start(string nodeId, int heroHp, string[] heroExtras, Enemy[] enemies,
            ShanheStartingCard[]? openingOverride = null, int maxTurns = 9)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            ShanheStartingCard[] opening = openingOverride ?? Enumerable.Range(0, 4)
                .Select(i => i == 3
                    ? new ShanheStartingCard(CardKind.Duel, Suit.Spade, 6)
                    : new ShanheStartingCard(CardKind.Slash, Suit.Club, 4 + i % 9)).ToArray();
            ShanheCombatantDefinition Combatant(int seat, Enemy enemy) => new(Peer, Hp: enemy.Hp, ExtraSkillIds: enemy.Extras)
            { StartingHand = enemy.StartingHand };
            var setup = new ShanheBattleSetup(96, 1, nodeId, Hero, heroHp, 0, 0, 0, 0, heroExtras, opening, [],
                enemies.Select((enemy, index) => Combatant(index + 1, enemy)).ToArray(), [], new string('e', 64));
            var game = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 5, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = maxTurns }, registry);
            var scene = new Scene(game, registry); scene.Accept0(new StartGameCommand()); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shi-zang-hong", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero);
            var rules = $$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
              {"id":"{{Dry}}","revision":1,"triggers":[{"id":"no-draw","window":"drawPhaseStarting","subject":"owner","optional":false,"isSkillInvocation":false,
                "effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]}]},
              {"id":"{{Keeper}}","revision":1,"modifiers":[{"id":"keep","query":"handLimit","operation":"add","value":10,"priority":0}]}
            ]}
            """;
            var presentations = new Dictionary<string, object>
            {
                [Dry] = new { name = Dry, description = "只跳过普通摸牌，保留真实出牌阶段。" },
                [Keeper] = new { name = Keeper, description = "保持手牌。" },
            };
            var catalog = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = presentations }));
            foreach (var (id, program) in catalog.Programs) b.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
            { Program = program, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new(Peer, "安静真实同桌", "", Dry, "wei", 8));
            b.AddCard(new("fixture:shi-zang-hong-slash", "杀", "基本牌", "同体杀实体", CardKind.Slash));
            b.AddCard(new("fixture:shi-zang-hong-duel", "决斗", "锦囊牌", "同体决斗实体", CardKind.Duel));
            b.AddCard(new("fixture:shi-zang-hong-dodge", "闪", "基本牌", "同体闪实体", CardKind.Dodge));
            b.AddDeck(new(Deck, "固定真实混合牌堆", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 150)
                .Select(i => i % 4 == 0
                    ? new ContentDeckPhysicalCard("fixture:shi-zang-hong-dodge", Suit.Heart, 6)
                    : i % 3 == 0
                        ? new ContentDeckPhysicalCard("fixture:shi-zang-hong-duel", Suit.Spade, 6)
                        : new ContentDeckPhysicalCard("fixture:shi-zang-hong-slash", Suit.Club, 5)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "势臧洪实际检查", 5, 5, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 4 }));
        }
    }
}
