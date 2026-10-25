using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShiYangHongChecks
{
    private const string Hero = "mobile:shi-yang-hong", Jijian = "mobile:shi-jijian", Yuanmo = "mobile:shi-yuanmo",
        Dry = "fixture:shi-yang-hong-dry", Keeper = "fixture:shi-yang-hong-keeper",
        Deck = "fixture:shi-yang-hong-deck", Peer = "fixture:shi-yang-hong-peer";

    public static void JijianContestWithoutSwapSlashesLoser()
    {
        var s = Scene.Start("jijian-basic", 3, [Keeper],
            [new Enemy(8, [Dry]) { StartingHand = [new ShanheStartingCard(CardKind.Slash, Suit.Spade, 9)] },
                new Enemy(8, [Dry]) { StartingHand = [new ShanheStartingCard(CardKind.Dodge, Suit.Heart, 5)] },
                new Enemy(8, [Dry]), new Enemy(8, [Dry])],
            openingOverride: [new ShanheStartingCard(CardKind.Slash, Suit.Club, 5),
                new ShanheStartingCard(CardKind.Slash, Suit.Club, 12)]);
        s.Play();
        var handBefore = s.V(0).HandCount;
        var hidden = s.V(0).Hand.First(c => c.Rank == 5).Id;
        s.ActivateJijian(hidden, 1, 2);
        s.Reach(() => s.E<ShiYhJijianResolvedEvent>() is [{ Outcome: "slash", WinnerSeat: 1, LoserSeat: 2 }]);
        s.Reach(() => s.E<DamageAppliedEvent>().Any(e => e.TargetSeat == 2 && e.SourceSeat == 1));
        s.Need(s.E<ShiYhJijianStartedEvent>() is [{ OwnerSeat: 0, CardId: var started, FirstSeat: 1, SecondSeat: 2,
                FirstSwap: false, SecondSwap: false }] && started == hidden,
            "Both contestants kept their own cards, so the started event records no swap.");
        s.Need(s.V(2).Hp == 7 && s.E<DamageAppliedEvent>().Any(e => e.TargetSeat == 2 && e.SourceSeat == 1 && e.Amount == 1),
            "The rank-9 contestant won, slashed the rank-5 loser without distance limit and dealt one damage.");
        s.Need(s.V(0).HandCount == handBefore && s.V(0).Hand.Any(c => c.Rank == 5) &&
            !s.G.CardMovements.Any(m => m.Reason.Value == "skill-program.mobile:shi-jijian.lend"),
            "The untouched secret card stayed in the owner's hand and nothing was lent.");
        s.Need(s.E<ShiYhJijianResolvedEvent>().Length == 1 && s.V(0).Hp == 3 && s.V(1).Hp == 8,
            "The once-per-phase activation resolved once and the retired Slash hurt nobody else.");
        s.Cold(); s.Replay();
    }

    public static void JijianOwnerSwapSlashesAndRetaliates()
    {
        var s = Scene.Start("jijian-swap", 3, [Keeper],
            [new Enemy(8, [Dry]) { StartingHand = [new ShanheStartingCard(CardKind.Dodge, Suit.Heart, 5)] },
                new Enemy(8, [Dry]), new Enemy(8, [Dry]), new Enemy(8, [Dry])],
            openingOverride: [new ShanheStartingCard(CardKind.Slash, Suit.Spade, 13),
                new ShanheStartingCard(CardKind.Slash, Suit.Club, 12)]);
        s.Play();
        s.JijianBranch = "swap";
        var hidden = s.V(0).Hand.First(c => c.Rank == 13).Id;
        s.ActivateJijian(hidden, 0, 1);
        s.Reach(() => s.E<ShiYhJijianResolvedEvent>() is [{ Outcome: "slash", WinnerSeat: 0, LoserSeat: 1 }]);
        s.Reach(() => s.E<DamageAppliedEvent>().Any(e => e.TargetSeat == 0 && e.SourceSeat == 0));
        s.Need(s.E<ShiYhJijianStartedEvent>() is [{ OwnerSeat: 0, CardId: var started, FirstSeat: 0, SecondSeat: 1,
            FirstSwap: true, SecondSwap: false }] && started == hidden,
            "The owner secretly swapped the hidden card in while the other contestant kept theirs.");
        s.Need(s.V(1).Hp == 7 && s.E<DamageAppliedEvent>().Any(e => e.TargetSeat == 1 && e.SourceSeat == 0 && e.Amount == 1),
            "The hidden rank-13 Slash won the contest and struck the loser without distance limit.");
        s.Need(s.V(0).Hp == 2 && s.E<DamageAppliedEvent>().Any(e => e.TargetSeat == 0 && e.SourceSeat == 0 && e.Amount == 1),
            "Using the swapped-in Slash costs the swapping owner one retaliation damage.");
        s.Need(!s.V(0).Hand.Any(c => c.Rank == 13) &&
            s.G.CardMovements.Any(m => m.CardId == hidden && m.From == CardLocation.Processing &&
                m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.PindianFinish),
            "The swapped card left the owner's hand through the Pindian settlement discard.");
        s.Need(!s.G.CardMovements.Any(m => m.Reason.Value == "skill-program.mobile:shi-jijian.lend"),
            "An owner-swapped secret card never lends a physical copy to the other contestant.");
        s.Cold(); s.Replay();
    }

    public static void YuanmoMovesFieldCardAndDrawsForLostRange()
    {
        var s = Scene.Start("yuanmo-move", 4, [Keeper],
            [new Enemy(8, [Dry]) { StartingHand = [new ShanheStartingCard(CardKind.Duel, Suit.Spade, 6),
                    new ShanheStartingCard(CardKind.Slash, Suit.Club, 9)] },
                new Enemy(8, [Dry]), new Enemy(8, [Dry]), new Enemy(8, [Dry])],
            openingOverride: [new ShanheStartingCard(CardKind.OffensiveHorse, Suit.Heart, 13),
                new ShanheStartingCard(CardKind.Slash, Suit.Club, 12)]);
        s.Play();
        s.PlayHandCard(CardKind.OffensiveHorse);
        s.Need(s.V(0).HandCount == 1 && s.G.CreateCardZoneDiagnostics().Any(c =>
                c.CardKind == CardKind.OffensiveHorse && c.Location.Zone == CardZoneKind.Equipment &&
                c.Location.OwnerSeat == 0),
            "The horse was equipped from the fixed opening hand for the range test.");
        s.End();
        s.ArmYuanmoPrompts = true;
        s.Reach(() => s.E<DamageAppliedEvent>().Count(e => e.TargetSeat == 0) == 1);
        s.Reach(() => s.P is { PlayerSeat: 0 } p && p.SkillPrompt?.SkillId == Yuanmo &&
            p.Choices.Any(c => c.Parameters.GetValueOrDefault("branch")?.StartsWith("move-", StringComparison.Ordinal) == true));
        var horseId = s.HorseCardId();
        s.AnswerShiYh($"move-{horseId}-3");
        s.Reach(() => s.P is { PlayerSeat: 0 } p && p.SkillPrompt?.SkillId == Yuanmo &&
            p.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "draw"));
        s.AnswerShiYh("draw");
        s.Need(s.E<ShiYhYuanmoUsedEvent>() is [{ OwnerSeat: 0, Turn: 2 }] && s.V(0).HandCount == 3,
            "Moving the horse from seat 0 to seat 3 dropped seats 2 and 3 from the owner's attack range and paid a two-card draw.");
        s.Reach(() => s.E<DamageAppliedEvent>().Count(e => e.TargetSeat == 0) == 2);
        s.Reach(() => s.P is { PlayerSeat: 0 } p && p.SkillPrompt?.SkillId == Yuanmo &&
            p.Choices.Any(c => c.Parameters.GetValueOrDefault("branch")?.StartsWith("move-", StringComparison.Ordinal) == true));
        s.AnswerShiYh($"move-{horseId}-1");
        s.Reach(() => s.P is { PlayerSeat: 0 } p && p.SkillPrompt?.SkillId == Yuanmo &&
            p.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "draw"));
        s.AnswerShiYh("draw");
        s.Need(s.E<ShiYhYuanmoUsedEvent>() is [{ Turn: 2 }, { Turn: 2 }] && s.V(3).HandCount == 2 &&
            s.V(0).Hp == 2,
            "The second damage of the same turn reopened YuanMo, seat 3 lost seats 0 and 1 from range and drew twice.");
        s.Need(s.G.CardMovements.Any(m => m.CardId == horseId && m.From == new CardLocation(CardZoneKind.Equipment, 0) &&
                m.To == new CardLocation(CardZoneKind.Equipment, 3) &&
                m.Reason.Value == "skill-program.mobile:shi-yuanmo.move") &&
            s.G.CardMovements.Any(m => m.CardId == horseId && m.From == new CardLocation(CardZoneKind.Equipment, 3) &&
                m.To == new CardLocation(CardZoneKind.Equipment, 1)),
            "Both YuanMo moves travelled between equipment zones with the skill reason on the first hop.");
        s.Reach(() => s.G.State.TurnNumber >= 6 && s.P is { PlayerSeat: 0 } p &&
            p.SkillPrompt?.SkillId == Yuanmo && p.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "pass"));
        s.AnswerShiYh("pass");
        s.Need(s.E<ShiYhYuanmoUsedEvent>().Length == 2,
            "The owner's next prep phase offered YuanMo again, but passing spends nothing from the ledger.");
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

        public string JijianBranch { get; set; } = "keep";
        public bool ArmYuanmoPrompts { get; set; }

        public void ActivateJijian(int cardId, int first, int second)
        {
            Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } && Offered(Jijian));
            Accept0(new UseProgramSkillCommand(0, Jijian, "jijian-contest", [cardId], [first, second], G.Revision, P!.PromptId));
        }

        public int HorseCardId() => G.CreateCardZoneDiagnostics()
            .Single(c => c.CardKind == CardKind.OffensiveHorse && c.Location.Zone == CardZoneKind.Equipment).CardId;

        public void AnswerShiYh(string branch)
        {
            if (P is not { } p || p.SkillPrompt?.SkillId is not (Jijian or Yuanmo))
                throw new InvalidOperationException($"No Yang Hong prompt is pending for branch '{branch}'.");
            var choice = p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == branch) ??
                throw new InvalidOperationException($"The Yang Hong prompt lacks branch '{branch}'.");
            Accept0(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, choice.Id, G.Revision));
        }

        public void PlayHandCard(CardKind kind)
        {
            var cardId = V(0).Hand.Where(card => card.Kind == kind).Select(card => card.Id).First();
            Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } &&
                G.GetHumanLegalActions().Any(a => a.CardId == cardId));
            var a = G.GetHumanLegalActions().First(a => a.CardId == cardId);
            Accept0(new PlayCardCommand(0, a.CardId!.Value, a.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind));
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
                    if (p.PlayerSeat == 0 && p.SkillPrompt?.SkillId == Jijian)
                    { Answer(c => c.Parameters.GetValueOrDefault("branch") == JijianBranch); continue; }
                    if (p.PlayerSeat == 0 && p.SkillPrompt?.SkillId == Yuanmo)
                    {
                        if (ArmYuanmoPrompts)
                            throw new InvalidOperationException("Armed YuanMo boundary. " + Dump());
                        Answer(c => c.Parameters.GetValueOrDefault("branch") == "pass");
                        continue;
                    }
                    if (p.PlayerSeat == 0 && p.Kind is DecisionKind.RespondDodge or DecisionKind.RespondSlash or DecisionKind.Nullification or DecisionKind.RescueDying)
                    { Answer(c => c.Cards.Count == 0); continue; }
                    if (p.PlayerSeat == 0 && p.Kind != DecisionKind.PlayCard &&
                        p.Choices.Any(c => c.Parameters.GetValueOrDefault("action") == "end-play"))
                    { Answer(c => c.Parameters.GetValueOrDefault("action") == "end-play"); continue; }
                    if (p.PlayerSeat == 0) throw new InvalidOperationException("Unexpected Yang Hong boundary. " + Dump());
                }
                Need(G.State.Winner == Winner.None, "The bounded real scene must retain its participants.");
                Accept0(new AdvanceOneStepCommand(G.Revision));
            }
            Need(false, "Yang Hong did not reach the expected bounded native boundary.");
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
        { var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, result.Error?.Message ?? "Rejected Yang Hong command."); }
        private string State() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 5).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),
            Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray(),
            G.CardMovements, Zones = G.CreateCardZoneDiagnostics(), Commands = CommandJson.Serialize(G.AcceptedCommands) });
        private string Dump() => JsonSerializer.Serialize(new { G.State.TurnNumber, G.State.CurrentSeat, G.State.Winner, P,
            Players = Enumerable.Range(0, 5).Select(i => new { i, V(i).Hp, V(i).HandCount }),
            Started = E<ShiYhJijianStartedEvent>(), Resolved = E<ShiYhJijianResolvedEvent>(),
            Moved = E<ShiYhYuanmoMovedEvent>(), Used = E<ShiYhYuanmoUsedEvent>(),
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
            var setup = new ShanheBattleSetup(96, 1, nodeId, Hero, heroHp, heroHp - 3, 0, 0, 0, heroExtras, opening, [],
                enemies.Select((enemy, index) => Combatant(index + 1, enemy)).ToArray(), [], new string('e', 64));
            var game = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 5, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = maxTurns }, registry);
            var scene = new Scene(game, registry); scene.Accept0(new StartGameCommand()); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shi-yang-hong", new(1, 0, 0));
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
            b.AddCard(new("fixture:shi-yang-hong-slash", "杀", "基本牌", "同体杀实体", CardKind.Slash));
            b.AddCard(new("fixture:shi-yang-hong-duel", "决斗", "锦囊牌", "同体决斗实体", CardKind.Duel));
            b.AddCard(new("fixture:shi-yang-hong-dodge", "闪", "基本牌", "同体闪实体", CardKind.Dodge));
            b.AddCard(new("fixture:shi-yang-hong-horse", "进攻马", "装备牌", "+1马", CardKind.OffensiveHorse));
            b.AddDeck(new(Deck, "固定真实混合牌堆", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 150)
                .Select(i => i % 4 == 0
                    ? new ContentDeckPhysicalCard("fixture:shi-yang-hong-dodge", Suit.Heart, 6)
                    : i % 3 == 0
                        ? new ContentDeckPhysicalCard("fixture:shi-yang-hong-duel", Suit.Spade, 6)
                        : new ContentDeckPhysicalCard("fixture:shi-yang-hong-slash", Suit.Club, 5)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "势杨弘实际检查", 5, 5, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 4 }));
        }
    }
}
