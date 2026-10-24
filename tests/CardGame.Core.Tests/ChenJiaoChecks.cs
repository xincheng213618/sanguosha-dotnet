using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ChenJiaoChecks
{
    private const string Hero = "mobile:shi-chen-jiao", QingYan = "mobile:shi-qingyan", CeDuan = "mobile:shi-ceduan",
        Quiet = "fixture:shi-chen-jiao-quiet", Peer = "fixture:shi-chen-jiao-peer",
        HandKeeper = "fixture:shi-chen-jiao-keeper", RangeBoost = "fixture:shi-chen-jiao-range",
        Deck = "fixture:shi-chen-jiao-deck";

    public static void QingYanEscalatesLocksAndRearms()
    {
        var s = Scene.Start("qingyan", 3, [HandKeeper, RangeBoost],
            [new(8), new(8), new(8), new(8)], openingSlashes: 4); s.Play();
        s.Need(s.Offered(QingYan), "The first round offers QingYan while no shown card stays in hand.");
        s.ActivateQingYan();
        var first = s.ShowNext();
        s.Need(s.E<ChenJiaoQingYanShownEvent>() is [{ Ordinal: 1, ShowIndex: 0 }] &&
            s.E<ChenJiaoQingYanShownEvent>()[0].CardId == first &&
            s.V(0).HandCount == 4,
            "X=1 shows exactly one hand card and the shown card stays in the hand.");
        s.AnswerKind("dodge");
        s.Need(s.E<ChenJiaoQingYanUsedEvent>() is [{ Ordinal: 1, Kind: CardKind.Dodge }] &&
            s.E<CardUsedEvent>().Any(e => e.CardId == 0 && e.CardKind == CardKind.Dodge && e.SourceSeat == 0) &&
            s.E<PlayerMarkerChangedEvent>().Last(e => e.Marker == PlayerMarkerKind.ChenJiaoQingYanShown).Count == 1,
            "The first activation records its ordinal, uses a cardless virtual Dodge and mirrors the shown card on the marker.");
        s.Play();
        s.Need(!s.Offered(QingYan),
            "The skill is失效 while the shown card remains in hand.");
        s.PlaySlash(first, 1);
        s.Reach(() => s.E<DamageAppliedEvent>().Any(e => e.TargetSeat == 1));
        s.Play();
        s.Need(s.Offered(QingYan) &&
            s.E<PlayerMarkerChangedEvent>().Last(e => e.Marker == PlayerMarkerKind.ChenJiaoQingYanShown).Count == 0,
            "Using the shown card empties the shown set and re-arms the skill.");
        s.ActivateQingYan();
        var second = s.ShowNext();
        var third = s.ShowNext();
        s.Need(s.E<ChenJiaoQingYanShownEvent>().Select(e => e.Ordinal).ToArray() is [1, 2, 2] &&
            s.E<ChenJiaoQingYanShownEvent>().Last(e => e.ShowIndex == 0).CardId == second &&
            s.E<ChenJiaoQingYanShownEvent>().Last(e => e.ShowIndex == 1).CardId == third,
            "The second activation escalates to X=2 and shows two distinct hand cards.");
        s.AnswerKind("nullification");
        s.Need(s.E<ChenJiaoQingYanUsedEvent>() is [{ Ordinal: 1, Kind: CardKind.Dodge }, { Ordinal: 2, Kind: CardKind.Nullification }] &&
            s.V(0).HandCount == 3,
            "The second activation may pick the virtual Nullification branch without consuming hand cards.");
        s.Play();
        s.PlaySlash(second, 1);
        s.Reach(() => s.E<DamageAppliedEvent>().Count(e => e.TargetSeat == 1) >= 2);
        s.Play();
        s.PlaySlash(third, 2);
        s.Reach(() => s.E<DamageAppliedEvent>().Any(e => e.TargetSeat == 2));
        s.Play();
        s.Need(s.E<PlayerMarkerChangedEvent>().Last(e => e.Marker == PlayerMarkerKind.ChenJiaoQingYanShown).Count == 0 &&
            !s.Offered(QingYan),
            "Clearing every shown card empties the shown set; the X=3 escalation now waits for three hand cards again.");
        s.Cold(); s.Replay();
    }

    public static void CeDuanShowsInRangeHandsAndReusesMajorityColor()
    {
        var s = Scene.Start("ceduan", 3, [HandKeeper],
            [new(8, new(CardKind.Slash, Suit.Heart, 5)), new(8, new(CardKind.Slash, Suit.Club, 5)), new(8), new(8)],
            openingSlashes: 1); s.Play();
        s.Need(s.CeDuanTargets().SequenceEqual([1, 4]),
            "Only seats whose attack range contains the owner (both circular neighbours) are selectable targets, got [" +
            string.Join(",", s.CeDuanTargets()) + "].");
        s.ActivateCeDuan(1);
        s.AnswerShow();
        s.Reach(() => s.E<ChenJiaoCeDuanShownEvent>().Length >= 3);
        s.Need(s.E<ChenJiaoCeDuanStartedEvent>() is [{ TargetSeat: 1, HolderCount: 3 }] &&
            s.E<ChenJiaoCeDuanShownEvent>().Select(e => e.HolderSeat).ToArray() is [0, 1, 2] &&
            s.E<ChenJiaoCeDuanShownEvent>().Length == 3,
            "The target's attack range shows one hand card from the owner and both in-range neighbours.");
        s.Reach(() => s.E<CardUsedEvent>().Any(e => e.CardKind == CardKind.Slash && e.SourceSeat == 0 && e.TargetSeat == 1));
        s.Need(s.E<ChenJiaoCeDuanUseIssuedEvent>() is [{ MaterialCount: 2 }] &&
            s.E<DamageAppliedEvent>().Any(e => e.TargetSeat == 1 && e.Amount == 1) &&
            s.E<ChenJiaoCeDuanCompletedEvent>() is [{ Damaged: true, Drew: true }] &&
            s.V(0).HandCount == 3 && s.V(2).HandCount == 0,
            "The two black shown cards become one off-count Slash, the wound draws one card and the materials leave their hands.");
        s.Play();
        s.Need(!s.Offered(CeDuan) && s.Offered(QingYan),
            "CeDuan is spent for this play phase while QingYan stays independently offered.");
        s.Cold(); s.Replay();
    }

    public static void CeDuanTieColorChoiceAndEmptyShows()
    {
        var s = Scene.Start("ceduan-tie", 3, [HandKeeper],
            [new(8, new(CardKind.Slash, Suit.Heart, 5)), new(8), new(8), new(8)],
            openingSlashes: 1); s.Play();
        s.ActivateCeDuan(1);
        s.AnswerShow();
        s.Reach(() => s.E<ChenJiaoCeDuanShownEvent>().Length >= 2);
        s.Need(s.E<ChenJiaoCeDuanShownEvent>().Select(e => e.HolderSeat).ToArray() is [0, 1],
            "Only the two holders with hand cards show, one red and one black.");
        var tie = s.P is { PlayerSeat: 0 } p && p.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "color");
        s.Need(tie, "A 1:1 tie hands the color decision to the owner.");
        s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "color" && c.Parameters.GetValueOrDefault("value") == "black");
        s.Reach(() => s.E<ChenJiaoCeDuanCompletedEvent>().Length >= 1);
        s.Need(s.E<ChenJiaoCeDuanUseIssuedEvent>() is [{ MaterialCount: 1 }] &&
            s.E<DamageAppliedEvent>().Any(e => e.TargetSeat == 1) &&
            s.E<ChenJiaoCeDuanCompletedEvent>() is [{ Damaged: true, Drew: true }],
            "The chosen black shown card becomes the Slash and its wound still draws.");
        s.Cold(); s.Replay();
    }

    private sealed record Enemy(int Hp, ShanheStartingCard? Hand = null);

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G { get; private set; } = game;
        public PendingDecision? P => Enumerable.Range(0, 5).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public bool Offered(string skill) =>
            G.GetHumanLegalActions().Any(a => a.Kind == LegalActionKind.UseProgramSkill && a.ProgramSkillId == skill);
        public int[] CeDuanTargets() =>
            G.GetHumanLegalActions().Where(a => a.Kind == LegalActionKind.UseProgramSkill && a.ProgramSkillId == CeDuan)
                .SelectMany(a => a.SelectableTargetSeats).Distinct().Order().ToArray();
        public void ActivateQingYan()
        {
            Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } && Offered(QingYan));
            Accept0(new UseProgramSkillCommand(0, QingYan, "qingyan-show", [], [], G.Revision, P!.PromptId));
        }
        public void ActivateCeDuan(int target)
        {
            Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } &&
                G.GetHumanLegalActions().Any(a => a.Kind == LegalActionKind.UseProgramSkill &&
                    a.ProgramSkillId == CeDuan));
            Accept0(new UseProgramSkillCommand(0, CeDuan, "ceduan-use", [], [target], G.Revision, P!.PromptId));
        }
        public int ShowNext()
        {
            Reach(() => P is { PlayerSeat: 0 } p && p.Kind == DecisionKind.ProgramTrigger &&
                p.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "show"));
            var choice = P!.Choices.First(c => c.Parameters.GetValueOrDefault("branch") == "show");
            var cardId = choice.Cards[0];
            Answer(c => c.Parameters.GetValueOrDefault("branch") == "show" && c.Cards.Contains(cardId));
            return cardId;
        }
        public void AnswerShow()
        {
            Reach(() => P is { PlayerSeat: 0 } p && p.Kind == DecisionKind.ProgramTrigger &&
                p.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "show"));
            Answer(c => c.Parameters.GetValueOrDefault("branch") == "show");
        }
        public void AnswerKind(string value) =>
            Answer(c => c.Parameters.GetValueOrDefault("branch") == "kind" && c.Parameters.GetValueOrDefault("value") == value);
        public void Answer(Func<PromptChoice, bool> pick)
        {
            var p = P!; var choice = p.Choices.FirstOrDefault(pick); Need(choice is not null, "The expected prompt choice must be published.");
            Accept0(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, choice!.Id, G.Revision));
        }
        public void End() => Accept0(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Reach(Func<bool> ready)
        {
            for (var i = 0; i < 1600; i++)
            {
                if (ready()) return;
                if (P is { } p)
                {
                    if (p.Kind == DecisionKind.SelectFaction) { Answer(_ => true); continue; }
                    if (p.PlayerSeat == 0 && p.Kind == DecisionKind.PlayCard) { End(); continue; }
                    if (p.PlayerSeat == 0 && p.SkillPrompt?.SkillId is QingYan or CeDuan)
                        throw new InvalidOperationException("Unexpected Chen Jiao boundary. " + Dump());
                    if (p.PlayerSeat == 0 && p.Kind is DecisionKind.RespondDodge or DecisionKind.RespondSlash or DecisionKind.Nullification or DecisionKind.RescueDying)
                    { Answer(c => c.Cards.Count == 0); continue; }
                    if (p.PlayerSeat == 0 && p.Kind != DecisionKind.PlayCard && p.SkillPrompt?.SkillId is not (QingYan or CeDuan) &&
                        p.Choices.Any(c => c.Parameters.GetValueOrDefault("action") == "end-play"))
                    { Answer(c => c.Parameters.GetValueOrDefault("action") == "end-play"); continue; }
                    if (p.PlayerSeat == 0) throw new InvalidOperationException("Unexpected Chen Jiao boundary. " + Dump());
                }
                Need(G.State.Winner == Winner.None, "The bounded real scene must retain its participants.");
                Accept0(new AdvanceOneStepCommand(G.Revision));
            }
            Need(false, "Chen Jiao did not reach the expected bounded native boundary.");
        }
        public void PlaySlash(int cardId, int target)
        {
            Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } &&
                G.GetHumanLegalActions().Any(a => a.Kind == LegalActionKind.Slash && a.CardId == cardId &&
                    a.TargetSeats.Contains(target)));
            var a = G.GetHumanLegalActions().First(a => a.Kind == LegalActionKind.Slash && a.CardId == cardId &&
                a.TargetSeats.Contains(target));
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
        { var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, result.Error?.Message ?? "Rejected Chen Jiao command."); }
        private string State() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 5).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),
            Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray(),
            G.CardMovements, Zones = G.CreateCardZoneDiagnostics(), Commands = CommandJson.Serialize(G.AcceptedCommands) });
        private string Dump() => JsonSerializer.Serialize(new { G.State.TurnNumber, G.State.CurrentSeat, P,
            Players = Enumerable.Range(0, 5).Select(i => new { i, V(i).Hp, V(i).HandCount }),
            Markers = E<PlayerMarkerChangedEvent>().Select(e => new { e.Marker, e.Delta, e.Count }),
            Shown = E<ChenJiaoQingYanShownEvent>(), Used = E<ChenJiaoQingYanUsedEvent>(),
            Started = E<ChenJiaoCeDuanStartedEvent>(), CeShows = E<ChenJiaoCeDuanShownEvent>(),
            Issued = E<ChenJiaoCeDuanUseIssuedEvent>(), Completed = E<ChenJiaoCeDuanCompletedEvent>(),
            Damage = E<DamageAppliedEvent>().Select(e => new { e.TargetSeat, e.Amount }),
            Moves = G.CardMovements.TakeLast(40).Select(m => new { m.CardId, From = m.From.Zone, FromSeat = m.From.OwnerSeat, To = m.To.Zone, ToSeat = m.To.OwnerSeat, Reason = m.Reason.Value }),
            Last = G.Events.TakeLast(6).Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray() });
        public static Scene Start(string nodeId, int heroHp, string[] heroExtras, Enemy[] enemies, int openingSlashes = 3)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            ShanheStartingCard[] opening = Enumerable.Range(0, openingSlashes)
                .Select(i => new ShanheStartingCard(CardKind.Slash, i % 2 == 0 ? Suit.Club : Suit.Heart, 4 + i % 9)).ToArray();
            ShanheCombatantDefinition Combatant(int seat, Enemy enemy) => new(Peer, Hp: enemy.Hp, ExtraSkillIds: [Quiet])
            { StartingHand = enemy.Hand is null ? [] : [enemy.Hand] };
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
        public PackageManifest Manifest { get; } = new("fixture-shi-chen-jiao", new(1, 0, 0));
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
            b.AddCard(new("fixture:shi-chen-jiao-slash", "杀", "基本牌", "同体杀实体", CardKind.Slash));
            b.AddCard(new("fixture:shi-chen-jiao-dodge", "闪", "基本牌", "同体闪实体", CardKind.Dodge));
            b.AddDeck(new(Deck, "固定真实杀牌堆", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 120)
                .Select(i => i % 7 == 3
                    ? new ContentDeckPhysicalCard("fixture:shi-chen-jiao-dodge", Suit.Heart, 7)
                    : new ContentDeckPhysicalCard("fixture:shi-chen-jiao-slash", i % 2 == 0 ? Suit.Heart : Suit.Club, 5)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "势陈矫实际检查", 5, 5, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 4 }));
        }
    }
}
