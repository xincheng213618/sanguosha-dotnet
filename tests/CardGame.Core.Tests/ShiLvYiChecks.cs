using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShiLvYiChecks
{
    private const string Hero = "mobile:shi-lv-yi", Huilv = "mobile:shi-huilv", Songyan = "mobile:shi-songyan",
        Dry = "fixture:shi-lv-yi-dry", Keeper = "fixture:shi-lv-yi-keeper",
        Deck = "fixture:shi-lv-yi-deck", Peer = "fixture:shi-lv-yi-peer";

    public static void HuilvGrantsRoundColorsAndSettlesUnusedGrant()
    {
        var s = Scene.Start("huilv", 4, [Keeper],
            [new Enemy(8, [Dry]), new Enemy(8, [Dry]), new Enemy(8, [Dry]), new Enemy(8, [Dry])],
            openingCards: 2);
        s.HuilvBranch = "red";
        s.Play();
        s.End();
        s.Reach(() => s.E<ShiLyHuilvGrantedEvent>() is [{ OwnerSeat: 0, ActorSeat: 1, Red: true }]);
        var granted = s.E<ShiLyHuilvGrantedEvent>()[0];
        s.Need(s.V(1).HandCount == 1 &&
            s.G.CardMovements.Any(m => m.CardId == granted.CardId && m.To == CardLocation.Hand(1) &&
                m.From == CardLocation.DrawPile && m.Reason.Value == "skill-program.mobile:shi-huilv.gain"),
            "The red grant moved a draw-pile card into the actor's hand with its gain reason.");
        s.Need(s.E<ShiLyHuilvSettledEvent>().Length == 0,
            "The grant stays open during the actor's play phase.");
        s.HuilvBranch = "black";
        s.Reach(() => s.E<ShiLyHuilvGrantedEvent>().Length == 2);
        s.Need(s.E<ShiLyHuilvGrantedEvent>() is [{ Red: true, ActorSeat: 1 }, { Red: false, ActorSeat: 2 }],
            "The second grant of the round must be the remaining black color for the next actor.");
        s.HuilvBranch = "pass";
        s.Reach(() => s.E<ShiLyShishiRecordedEvent>().Length > 0 || s.G.State.TurnNumber >= 4);
        s.Need(s.E<ShiLyHuilvGrantedEvent>().Length == 2,
            "Both round colors are spent, so later play starts offer only the pass choice.");
        s.Cold(); s.Replay();
    }

    public static void HuilvSettlesUnusedGrantWithVirtualDamageAndGive()
    {
        var s = Scene.Start("huilv-settle", 4, [Keeper],
            [new Enemy(8, [Dry]), new Enemy(8, [Dry]), new Enemy(8, [Dry]), new Enemy(8, [Dry])],
            openingCards: 2);
        s.HuilvBranch = "red";
        s.HuilvGiveBranch = "give-2";
        s.Play();
        s.End();
        s.Reach(() => s.E<ShiLyHuilvGrantedEvent>() is [{ ActorSeat: 1, Red: true }]);
        var grantedCard = s.E<ShiLyHuilvGrantedEvent>()[0].CardId;
        s.Reach(() => s.E<ShiLyHuilvSettledEvent>() is [{ ActorSeat: 1 }]);
        var settled = s.E<ShiLyHuilvSettledEvent>()[0];
        s.Need(settled.Outcome == "given" && settled.GivenToSeat == 2 &&
            s.G.CardMovements.Any(m => m.CardId == grantedCard && m.To == CardLocation.Hand(2) &&
                m.Reason.Value == "skill-program.mobile:shi-huilv.give"),
            "The unused granted peach left the actor's hand through the give branch to seat 2.");
        s.Need(s.E<DamageAppliedEvent>().Any(e => e.TargetSeat == 0 && e.SourceSeat == 1 && e.Amount == 1) &&
            s.V(0).Hp == 3,
            "The unused non-discarded grant cost the owner one virtual damage sourced from the actor.");
        s.Need(s.V(1).Hp == 8 && s.E<ShiLyHuilvSettledEvent>().Length == 1,
            "The actor took no damage because the granted card was never used.");
        s.Need(s.V(2).HandCount == 1,
            "Seat 2 holds the re-given card.");
        s.Cold(); s.Replay();
    }

    public static void SongyanDrawsVirtuallyAndShishiSuppressesDamagers()
    {
        var s = Scene.Start("songyan-shishi", 4, [Keeper],
            [new Enemy(8, [Dry]) { StartingHand = [new ShanheStartingCard(CardKind.Slash, Suit.Club, 9),
                    new ShanheStartingCard(CardKind.Dodge, Suit.Heart, 10)] },
                new Enemy(8, [Dry]), new Enemy(8, [Dry]),
                new Enemy(8, [Dry]) { StartingHand = [new ShanheStartingCard(CardKind.Dodge, Suit.Spade, 11)] }],
            openingCards: 4, maxTurns: 13);
        s.ArmSongyanPrompts = true;
        s.Play();
        var openHand = s.V(0).HandCount;
        s.Need(s.E<ShiLySongyanUsedEvent>() is [{ OwnerSeat: 0, UseId: > 0 }] && s.V(0).HandCount == openHand &&
            openHand >= 4,
            "The prep-phase virtual ExNihilo resolved with the used ledger event.");
        s.Reach(() => s.G.State.CurrentSeat == 1 && s.E<DamageAppliedEvent>().Any(e => e.TargetSeat == 0));
        s.Need(s.E<DamageAppliedEvent>().Any(e => e.TargetSeat == 0 && e.SourceSeat == 1 && e.Amount == 1) &&
            s.E<ShiLyShishiRecordedEvent>() is [{ OwnerSeat: 0, SourceSeat: 1 }],
            "Seat 1's slash recorded it as a permanent Shishi damager.");
        s.ArmSongyanPrompts = false;
        s.Reach(() => s.G.State.CurrentSeat == 0 && s.P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } &&
            s.G.State.TurnNumber >= 3);
        s.PlayHandSlash(1);
        s.Reach(() => s.E<DamageAppliedEvent>().Any(e => e.TargetSeat == 1 && e.SourceSeat == 0 && e.Amount == 1));
        s.Need(s.V(1).Hp == 7 &&
            !s.G.Events.Select(e => e.Payload).OfType<CardRespondedEvent>().Any(r =>
                r.SourceSeat == 0 && r.ResponderSeat == 1),
            "The damager could not dodge the owner's Slash even while holding one.");
        s.Reach(() => s.G.State.CurrentSeat == 0 && s.P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } &&
            s.G.State.TurnNumber >= 11);
        s.PlayHandSlash(4);
        s.Reach(() => s.E<CardRespondedEvent>().Any(r => r.SourceSeat == 0 && r.ResponderSeat == 4) ||
            s.E<DamageAppliedEvent>().Any(e => e.TargetSeat == 4 && e.SourceSeat == 0));
        s.Need(s.E<CardRespondedEvent>().Any(r => r.SourceSeat == 0 && r.ResponderSeat == 4),
            "A player who never damaged the owner still responded to the owner's Slash.");
        s.Need(s.V(4).Hp == 8,
            "The non-damager's dodge kept the second Slash from landing.");
        s.Cold(); s.Replay();
    }

    public static void SongyanGateKeepsConversionOffForeignExNihiloWindows()
    {
        var s = Scene.Start("songyan-gate", 4, [],
            [new Enemy(8, [Dry]) { StartingHand = [new ShanheStartingCard(CardKind.DrawTwo, Suit.Heart, 7)] },
                new Enemy(8, [Dry]), new Enemy(8, [Dry]), new Enemy(8, [Dry])],
            openingOverride: [new ShanheStartingCard(CardKind.Nullification, Suit.Spade, 6),
                new ShanheStartingCard(CardKind.Slash, Suit.Club, 9),
                new ShanheStartingCard(CardKind.Slash, Suit.Club, 10),
                new ShanheStartingCard(CardKind.Slash, Suit.Club, 11)], maxTurns: 13);
        s.ArmSongyanPrompts = true;
        s.Play();
        s.Reach(() => s.E<ShiLyRoundDiscardedEvent>().Count(e => e.Seat == 0 && e.Round == 1) >= 2);
        s.Need(s.E<ShiLyRoundDiscardedEvent>().Where(e => e.Seat == 0).All(e => e.Red == false) &&
            s.V(0).Hand.Any(card => card.Kind == CardKind.Nullification),
            "The round discards are recorded black and a real Nullification survives them.");
        s.Reach(() => s.P is { PlayerSeat: 0, Kind: DecisionKind.Nullification });
        s.Need(s.P!.Choices.Where(c => c.Cards.Count == 1)
                .All(c => !c.Parameters.ContainsKey("conversion-skill-id")),
            "A foreign ExNihilo window must not offer the SongYan same-color conversion.");
        s.Reach(() => s.G.State.TurnNumber >= 3);
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

        public string HuilvBranch { get; set; } = "pass";
        public string HuilvGiveBranch { get; set; } = "keep";
        public bool ArmSongyanPrompts { get; set; }

        public void PlayHandSlash(int targetSeat)
        {
            var slashId = V(0).Hand.Where(card => card.Kind == CardKind.Slash)
                .Select(card => card.Id).First();
            Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } &&
                G.GetHumanLegalActions().Any(a => a.CardId == slashId && a.TargetSeats.Contains(targetSeat)));
            var a = G.GetHumanLegalActions().First(a => a.CardId == slashId && a.TargetSeats.Contains(targetSeat));
            Accept0(new PlayCardCommand(0, a.CardId!.Value, a.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind));
        }

        public void End() => Accept0(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
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
                    if (p.PlayerSeat == 0 && p.SkillPrompt?.SkillId == Huilv)
                    {
                        if (p.Choices.Any(c => c.Parameters.GetValueOrDefault("branch")?.StartsWith("give-") == true))
                            Answer(c => c.Parameters.GetValueOrDefault("branch") == HuilvGiveBranch);
                        else if (p.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == HuilvBranch))
                            Answer(c => c.Parameters.GetValueOrDefault("branch") == HuilvBranch);
                        else
                            Answer(c => c.Parameters.GetValueOrDefault("branch") == "pass");
                        continue;
                    }
                    if (p.PlayerSeat == 0 && p.SkillPrompt?.SkillId == Songyan &&
                        p.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "pass"))
                    { Answer(c => c.Parameters.GetValueOrDefault("branch") == (ArmSongyanPrompts ? "use" : "pass")); continue; }
                    if (p.PlayerSeat == 0 && p.SkillPrompt?.SkillId is Huilv or Songyan)
                        throw new InvalidOperationException("Unexpected Lü Yi boundary. " + Dump());
                    if (p.PlayerSeat == 0 && p.Kind is DecisionKind.RespondDodge or DecisionKind.RespondSlash or DecisionKind.Nullification or DecisionKind.RescueDying)
                    { Answer(c => c.Cards.Count == 0); continue; }
                    if (p.PlayerSeat == 0 && p.Kind != DecisionKind.PlayCard &&
                        p.Choices.Any(c => c.Parameters.GetValueOrDefault("action") == "end-play"))
                    { Answer(c => c.Parameters.GetValueOrDefault("action") == "end-play"); continue; }
                    if (p.PlayerSeat == 0) throw new InvalidOperationException("Unexpected Lü Yi boundary. " + Dump());
                }
                Need(G.State.Winner == Winner.None, "The bounded real scene must retain its participants.");
                Accept0(new AdvanceOneStepCommand(G.Revision));
            }
            Need(false, "Lü Yi did not reach the expected bounded native boundary.");
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
        { var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, result.Error?.Message ?? "Rejected Lü Yi command."); }
        private string State() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 5).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),
            Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray(),
            G.CardMovements, Zones = G.CreateCardZoneDiagnostics(), Commands = CommandJson.Serialize(G.AcceptedCommands) });
        private string Dump() => JsonSerializer.Serialize(new { G.State.TurnNumber, G.State.CurrentSeat, G.State.Winner, P,
            Players = Enumerable.Range(0, 5).Select(i => new { i, V(i).Hp, V(i).HandCount }),
            Grants = E<ShiLyHuilvGrantedEvent>(), Settled = E<ShiLyHuilvSettledEvent>(), Songyan = E<ShiLySongyanUsedEvent>(),
            Discards = E<ShiLyRoundDiscardedEvent>().Length, Recorded = E<ShiLyShishiRecordedEvent>(),
            Damage = E<DamageAppliedEvent>().Select(e => new { e.TargetSeat, e.SourceSeat, e.Amount }),
            Moves = G.CardMovements.TakeLast(40).Select(m => new { m.CardId, From = m.From.Zone, FromSeat = m.From.OwnerSeat, To = m.To.Zone, ToSeat = m.To.OwnerSeat, Reason = m.Reason.Value }),
            Last = G.Events.TakeLast(6).Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray() });

        public static Scene Start(string nodeId, int heroHp, string[] heroExtras, Enemy[] enemies, int openingCards = 4,
            ShanheStartingCard[]? openingOverride = null, int maxTurns = 9)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            ShanheStartingCard[] opening = openingOverride ?? Enumerable.Range(0, openingCards)
                .Select(i => i == openingCards - 1
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
        public PackageManifest Manifest { get; } = new("fixture-shi-lv-yi", new(1, 0, 0));
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
            b.AddCard(new("fixture:shi-lv-yi-slash", "杀", "基本牌", "同体杀实体", CardKind.Slash));
            b.AddCard(new("fixture:shi-lv-yi-duel", "决斗", "锦囊牌", "同体决斗实体", CardKind.Duel));
            b.AddCard(new("fixture:shi-lv-yi-peach", "桃", "基本牌", "同体桃实体", CardKind.Peach));
            b.AddCard(new("fixture:shi-lv-yi-dodge", "闪", "基本牌", "同体闪实体", CardKind.Dodge));
            b.AddDeck(new(Deck, "固定真实混合牌堆", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 150)
                .Select(i => i % 4 == 0
                    ? new ContentDeckPhysicalCard("fixture:shi-lv-yi-peach", Suit.Heart, 6)
                    : i % 3 == 0
                        ? new ContentDeckPhysicalCard("fixture:shi-lv-yi-duel", Suit.Spade, 6)
                        : new ContentDeckPhysicalCard("fixture:shi-lv-yi-slash", Suit.Club, 5)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "势吕壹实际检查", 5, 5, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 4 }));
        }
    }
}
