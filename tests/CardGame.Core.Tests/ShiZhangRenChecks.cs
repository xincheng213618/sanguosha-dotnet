using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShiZhangRenChecks
{
    private const string Hero = "mobile:shi-zhang-ren", Fuan = "mobile:shi-fuan", YinXian = "mobile:shi-yinxian",
        Quiet = "fixture:shi-zhang-ren-quiet", HandKeeper = "fixture:shi-zhang-ren-keeper", RangeBoost = "fixture:shi-zhang-ren-range",
        Deck = "fixture:shi-zhang-ren-deck", Peer = "fixture:shi-zhang-ren-peer";

    public static void FuAnArmsZeroRangeDrawAndRecordsPerRound()
    {
        var s = Scene.Start("fuan-arm", 4, [HandKeeper, RangeBoost],
            [new Enemy(8, [Quiet]), new Enemy(8, [Quiet]), new Enemy(8, [Quiet]), new Enemy(8, [Quiet])],
            openingCards: 4); s.Play();
        s.End();
        s.ArmFuan();
        s.AnswerSuit();
        s.Need(s.E<ShiZrFuanArmedEvent>() is [{ OwnerSeat: 0, DrawCount: 3 }] &&
            s.V(0).HandCount == 7,
            "Arming with range 3 draws exactly X=3 cards.");
        s.Need(s.E<ShiZrFuanRecordedEvent>() is [{ Suit: Suit.Club, Round: 1, ViaRelaunch: false }],
            "The only hand suit Club is recorded for round one without a relaunch flag.");
        s.Play();
        s.Need(s.E<ShiZrFuanStrikeIssuedEvent>().Length == 0,
            "Quiet foreign turns discard nothing, so the strike never arms.");
        s.Play();
        s.End();
        s.ArmFuan();
        s.AnswerSuit();
        s.Need(s.E<ShiZrFuanArmedEvent>() is [{ DrawCount: 3 }, { DrawCount: 2 }] &&
            s.E<ShiZrFuanRecordedEvent>() is [_, { Suit: Suit.Club, Round: 2 }],
            "The held zero keeps only the fixture bonus in round two while Club becomes recordable again.");
        s.Cold(); s.Replay();
    }

    public static void FuAnStrikesWithoutDistanceAndSealsUntilNextTurnEnd()
    {
        var s = Scene.Start("fuan-strike", 4, [],
            [new Enemy(4, [], Scene.ClubHand(6)), new Enemy(8, [Quiet]), new Enemy(8, [Quiet]), new Enemy(8, [Quiet])],
            openingCards: 4); s.Play();
        s.End();
        s.ArmFuan();
        s.AnswerSuit();
        s.Need(s.E<ShiZrFuanArmedEvent>() is [{ DrawCount: 1 }] &&
            s.E<ShiZrFuanRecordedEvent>() is [{ Suit: Suit.Club }],
            "Arming with range 1 draws one card and records Club.");
        s.Reach(() => s.E<ShiZrFuanSuitDiscardedEvent>().Any(e => e.OwnerSeat == 0 && e.Suit == Suit.Club));
        s.Need(s.E<ShiZrFuanSuitDiscardedEvent>().Any(e => e.ActualTurn == s.G.State.TurnNumber),
            "Seat one's over-limit Club discards arm the strike during its own turn.");
        s.StrikeFuan(3);
        s.Need(s.E<ShiZrFuanClearedEvent>() is [{ TargetSeat: 3 }] &&
            s.E<ShiZrFuanStrikeIssuedEvent>() is [{ OwnerSeat: 0, TargetSeat: 3, DamageCountBefore: 0 }] &&
            s.E<CardUsedEvent>().Any(e => e.CardId == 0 && e.CardKind == CardKind.Slash && e.SourceSeat == 0 && e.TargetSeat == 3) &&
            s.E<DamageAppliedEvent>().Any(e => e.TargetSeat == 3 && e.Amount == 1),
            "The strike ignores the distance-2 target with a held zero range and wounds for one.");
        s.Need(s.E<ShiZrFuanSealedEvent>() is [{ SourceSeat: 0, TargetSeat: 3 }] &&
            s.E<ShiZrFuanStrikeSettledEvent>().Any(e => e.Choice == "seal" && e.Damaged),
            "The wound seals seat three's skills until the end of its next turn.");
        s.Reach(() => s.E<ShiZrFuanSealExpiredEvent>() is [{ TargetSeat: 3 }]);
        s.Need(s.E<PhaseChangedEvent>().Any(e => e.Phase == TurnPhase.Play && e.ActorSeat == 3),
            "The sealed seat's Quiet play-phase skip is lifted, so its sealed turn runs a real play phase.");
        s.Need(!s.E<PhaseChangedEvent>().Any(e => e.Phase == TurnPhase.Play && e.ActorSeat == 2),
            "The unsealed Quiet neighbour never runs a play phase, isolating the seal as the cause.");
        s.Need(s.E<ShiZrFuanSealExpiredEvent>() is [{ FrameId: var frame }] &&
            s.E<ShiZrFuanSealedEvent>().Single().FrameId == frame,
            "The expiry retires exactly the issued seal.");
        s.Cold(); s.Replay();
    }

    public static void YinXianAddsFarthestDamageAndBonusRange()
    {
        var s = Scene.Start("yinxian", 4, [HandKeeper, RangeBoost],
            [new Enemy(8, [Quiet]), new Enemy(8, [Quiet]), new Enemy(8, [Quiet]), new Enemy(8, [Quiet])],
            openingCards: 4); s.Play();
        s.PlaySlash(s.OpeningClub(), 2);
        s.Reach(() => s.E<DamageAppliedEvent>().Any(e => e.TargetSeat == 2));
        s.PlaySlash(s.OpeningClub(), 1);
        s.Reach(() => s.E<DamageAppliedEvent>().Count(e => e.TargetSeat is 1 or 2) >= 2);
        s.Need(s.E<DamageAppliedEvent>().Where(e => e.SourceSeat == 0).Select(e => (e.TargetSeat, e.Amount))
                .Order().ToArray() is [(1, 1), (2, 2)],
            "The distance-2 farthest target takes 2 damage while the distance-1 neighbour takes 1.");
        s.Play();
        s.End();
        s.ArmFuan();
        s.AnswerSuit();
        s.Need(s.E<ShiZrFuanArmedEvent>() is [{ DrawCount: 3 }],
            "With no out-of-turn use the arm still draws the full reduced range of 3.");
        s.Cold(); s.Replay();
    }

    private sealed record Enemy(int Hp, string[] Extras, ShanheStartingCard[]? Hand = null);

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G { get; private set; } = game;
        public PendingDecision? P => Enumerable.Range(0, 5).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();

        public static ShanheStartingCard[] ClubHand(int count) => Enumerable.Range(0, count)
            .Select(i => new ShanheStartingCard(CardKind.Slash, Suit.Club, 4 + i % 9)).ToArray();
        private readonly List<int> _usedOpening = [];

        public int OpeningClub() => G.CreateSnapshot(0).Players.Single(p => p.Seat == 0).Hand
            .Select(card => card.Id).First(id => !_usedOpening.Contains(id));
        public void NoteUsed(int cardId) => _usedOpening.Add(cardId);

        public bool Offered(string skill) =>
            G.GetHumanLegalActions().Any(a => a.Kind == LegalActionKind.UseProgramSkill && a.ProgramSkillId == skill);

        public void ArmFuan()
        {
            Reach(() => P is { PlayerSeat: 0 } p && p.Kind == DecisionKind.ProgramTrigger &&
                p.SkillPrompt?.SkillId == Fuan && p.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "arm"));
            Answer(c => c.Parameters.GetValueOrDefault("branch") == "arm");
        }

        public void AnswerSuit()
        {
            Reach(() => P is { PlayerSeat: 0 } p && p.Kind == DecisionKind.ProgramTrigger &&
                p.SkillPrompt?.SkillId == Fuan && p.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "suit"));
            Answer(c => c.Parameters.GetValueOrDefault("branch") == "suit");
        }

        public void StrikeFuan(int target)
        {
            ArmFuan();
            Reach(() => P is { PlayerSeat: 0 } p && p.Kind == DecisionKind.ProgramTrigger &&
                p.SkillPrompt?.SkillId == Fuan && p.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "target"));
            Answer(c => c.Parameters.GetValueOrDefault("branch") == "target" && c.Targets.Contains(target));
            Reach(() => P is { PlayerSeat: 0 } p && p.Kind == DecisionKind.ProgramTrigger &&
                p.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "seal"));
            Answer(c => c.Parameters.GetValueOrDefault("branch") == "seal");
        }

        public void Answer(Func<PromptChoice, bool> pick)
        {
            var p = P!; var choice = p.Choices.FirstOrDefault(pick); Need(choice is not null, "The expected prompt choice must be published.");
            Accept0(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, choice!.Id, G.Revision));
        }

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
                    if (p.PlayerSeat == 0 && p.SkillPrompt?.SkillId is Fuan or YinXian)
                        throw new InvalidOperationException("Unexpected Zhang Ren boundary. " + Dump());
                    if (p.PlayerSeat == 0 && p.Kind is DecisionKind.RespondDodge or DecisionKind.RespondSlash or DecisionKind.Nullification or DecisionKind.RescueDying)
                    { Answer(c => c.Cards.Count == 0); continue; }
                    if (p.PlayerSeat == 0 && p.Kind != DecisionKind.PlayCard && p.SkillPrompt?.SkillId is not (Fuan or YinXian) &&
                        p.Choices.Any(c => c.Parameters.GetValueOrDefault("action") == "end-play"))
                    { Answer(c => c.Parameters.GetValueOrDefault("action") == "end-play"); continue; }
                    if (p.PlayerSeat == 0) throw new InvalidOperationException("Unexpected Zhang Ren boundary. " + Dump());
                }
                Need(G.State.Winner == Winner.None, "The bounded real scene must retain its participants.");
                Accept0(new AdvanceOneStepCommand(G.Revision));
            }
            Need(false, "Zhang Ren did not reach the expected bounded native boundary.");
        }

        public void PlaySlash(int cardId, int target)
        {
            Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } &&
                G.GetHumanLegalActions().Any(a => a.Kind == LegalActionKind.Slash && a.CardId == cardId &&
                    a.TargetSeats.Contains(target)));
            var a = G.GetHumanLegalActions().First(a => a.Kind == LegalActionKind.Slash && a.CardId == cardId &&
                a.TargetSeats.Contains(target));
            NoteUsed(cardId);
            Accept0(new PlayCardCommand(0, a.CardId!.Value, a.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind));
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
        { var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, result.Error?.Message ?? "Rejected Zhang Ren command."); }
        private string State() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 5).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),
            Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray(),
            G.CardMovements, Zones = G.CreateCardZoneDiagnostics(), Commands = CommandJson.Serialize(G.AcceptedCommands) });
        private string Dump() => JsonSerializer.Serialize(new { G.State.TurnNumber, G.State.CurrentSeat, P,
            Players = Enumerable.Range(0, 5).Select(i => new { i, V(i).Hp, V(i).HandCount }),
            Armed = E<ShiZrFuanArmedEvent>(), Recorded = E<ShiZrFuanRecordedEvent>(), Discarded = E<ShiZrFuanSuitDiscardedEvent>(),
            Issued = E<ShiZrFuanStrikeIssuedEvent>(), Settled = E<ShiZrFuanStrikeSettledEvent>(), Sealed = E<ShiZrFuanSealedEvent>(),
            Expired = E<ShiZrFuanSealExpiredEvent>(), Bonus = E<ShiZrYinXianBonusGainedEvent>(), Draws = E<CardsDrawnEvent>(),
            Damage = E<DamageAppliedEvent>().Select(e => new { e.TargetSeat, e.Amount }),
            Moves = G.CardMovements.TakeLast(40).Select(m => new { m.CardId, From = m.From.Zone, FromSeat = m.From.OwnerSeat, To = m.To.Zone, ToSeat = m.To.OwnerSeat, Reason = m.Reason.Value }),
            Last = G.Events.TakeLast(6).Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray() });

        public static Scene Start(string nodeId, int heroHp, string[] heroExtras, Enemy[] enemies, int openingCards = 4)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            ShanheStartingCard[] opening = Enumerable.Range(0, openingCards)
                .Select(i => new ShanheStartingCard(CardKind.Slash, Suit.Club, 4 + i % 9)).ToArray();
            ShanheCombatantDefinition Combatant(int seat, Enemy enemy) => new(Peer, Hp: enemy.Hp, ExtraSkillIds: enemy.Extras)
            { StartingHand = enemy.Hand ?? [] };
            var setup = new ShanheBattleSetup(96, 1, nodeId, Hero, heroHp, heroHp - 4, 0, 0, 0, heroExtras, opening, [],
                enemies.Select((enemy, index) => Combatant(index + 1, enemy)).ToArray(), [], new string('e', 64));
            var game = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 5, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 9 }, registry);
            var scene = new Scene(game, registry); scene.Accept0(new StartGameCommand()); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shi-zhang-ren", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero);
            var rules = $$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
              {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"no-draw","window":"drawPhaseStarting","subject":"owner","optional":false,"isSkillInvocation":false,
                "effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]},
                {"id":"no-play","window":"afterNormalDraw","subject":"owner","optional":false,"isSkillInvocation":false,
                "effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]},
              {"id":"{{HandKeeper}}","revision":1,"modifiers":[{"id":"keep","query":"handLimit","operation":"add","value":10,"priority":0},{"id":"many","query":"slashLimit","operation":"add","value":10,"priority":0}]},
              {"id":"{{RangeBoost}}","revision":1,"modifiers":[{"id":"reach","query":"attackRange","operation":"add","value":2,"priority":0}]}

            ]}
            """;
            var presentations = new Dictionary<string, object>
            {
                [Quiet] = new { name = Quiet, description = "安静真实回合并跳过普通摸牌出牌。" },
                [HandKeeper] = new { name = HandKeeper, description = "保持手牌并无限制出杀。" },
                [RangeBoost] = new { name = RangeBoost, description = "攻击范围+2。" },
            };
            var catalog = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = presentations }));
            foreach (var (id, program) in catalog.Programs) b.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
            { Program = program, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new(Peer, "安静真实同桌", "", Quiet, "wei", 8));
            b.AddCard(new("fixture:shi-zhang-ren-slash", "杀", "基本牌", "同体杀实体", CardKind.Slash));
            b.AddCard(new("fixture:shi-zhang-ren-dodge", "闪", "基本牌", "同体闪实体", CardKind.Dodge));
            b.AddDeck(new(Deck, "固定真实杀牌堆", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 120)
                .Select(i => i % 7 == 3
                    ? new ContentDeckPhysicalCard("fixture:shi-zhang-ren-dodge", Suit.Club, 7)
                    : new ContentDeckPhysicalCard("fixture:shi-zhang-ren-slash", Suit.Club, 5)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "势张任实际检查", 5, 5, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 4 }));
        }
    }
}
