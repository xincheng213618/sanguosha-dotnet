using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShiSunShaoChecks
{
    private const string Hero = "mobile:shi-sun-shao", GanJue = "mobile:shi-ganjue", ZhuJi = "mobile:shi-zhuji",
        Quiet = "fixture:shi-sun-shao-quiet", Keeper = "fixture:shi-sun-shao-keeper",
        Deck = "fixture:shi-sun-shao-deck", Peer = "fixture:shi-sun-shao-peer";

    public static void GanJueStrikesWithEquipmentAndBlocksMismatchedSuitResponse()
    {
        var s = Scene.Start("ganjue", 4, [Keeper],
            [new Enemy(8, [Quiet]) { StartingHand = [new ShanheStartingCard(CardKind.Dodge, Suit.Spade, 3)] },
                new Enemy(8, [Quiet]), new Enemy(8, [Quiet]), new Enemy(8, [Quiet])],
            openingCards: 4,
            startingEquipment: [new ShanheStartingCard(CardKind.QinggangSword, Suit.Spade, 6),
                new ShanheStartingCard(CardKind.BaguaFormation, Suit.Club, 1)]); s.Play();
        s.Need(s.Offered(GanJue), "The once-per-phase activation is offered while the owner owns equipment.");
        s.ActivateGanJue(1);
        s.AnswerGanJueMaterial("sword");
        s.Need(s.E<ShiSsGanjueIssuedEvent>() is [{ OwnerSeat: 0, StrikeSuit: Suit.Spade, TargetSeat: 1 }],
            "The spade sword issued its strike against the seated target.");
        s.Need(!s.E<DamageAppliedEvent>().Any(e => e.TargetSeat == 1),
            "The target holding a spade dodge keeps its response against the matching suit.");
        s.Reach(() => s.V(1).HandCount == 0);
        s.Need(s.E<ShiSsGanjueSettledEvent>() is [{ Damaged: false }],
            "The answered strike settled without damage.");
        s.Need(!s.Offered(GanJue),
            "The once-per-play-phase limit offers no second activation in the same phase.");
        s.PlayHandSlash(1);
        s.Need(s.E<DamageAppliedEvent>().Any(e => e.TargetSeat == 1 && e.Amount == 1),
            "A normal Slash still resolves in the same phase, so the equipment strike never counted against the limit.");
        s.Reach(() => s.G.State.TurnNumber >= 6 && s.G.State.CurrentSeat == 0 &&
            s.P is { Kind: DecisionKind.PlayCard });
        s.Need(s.Offered(GanJue), "The activation re-arms on the owner's next play phase.");
        s.ActivateGanJue(1);
        s.AnswerGanJueMaterial("armor");
        s.Need(s.E<ShiSsGanjueIssuedEvent>() is [_, { StrikeSuit: Suit.Club, TargetSeat: 1 }],
            "The club formation re-arms the strike on the next play phase.");
        s.Need(s.E<DamageAppliedEvent>().Count(e => e.TargetSeat == 1) == 2 &&
            s.E<ShiSsGanjueSettledEvent>() is [{ Damaged: false }, { Damaged: true }],
            "The empty-handed target cannot answer the mismatched suit and takes the strike.");
        s.Cold(); s.Replay();
    }

    public static void ZhuJiDiscardsOneSuitEquipsSameSuitAndScoresBonus()
    {
        var s = Scene.Start("zhuji", 4, [Keeper],
            [new Enemy(8, [Quiet]), new Enemy(8, [Quiet]), new Enemy(8, [Quiet]), new Enemy(8, [Quiet])],
            openingCards: 4,
            openingOverride: [new ShanheStartingCard(CardKind.Slash, Suit.Club, 4),
                new ShanheStartingCard(CardKind.Slash, Suit.Club, 5),
                new ShanheStartingCard(CardKind.Duel, Suit.Spade, 6),
                new ShanheStartingCard(CardKind.Slash, Suit.Heart, 7)]); s.Play();
        var clubsAtArm = s.V(0).Hand.Count(card => card.Suit == Suit.Club);
        var handAtArm = s.V(0).HandCount;
        s.Need(clubsAtArm >= 2, "The scene arms ZhuJi with at least the two opening clubs in hand.");
        s.End();
        s.Need(s.P is { PlayerSeat: 0 } p && p.Kind == DecisionKind.ProgramTrigger &&
            p.SkillPrompt?.SkillId == ZhuJi && p.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "arm"),
            "The play-ending trigger publishes the ZhuJi arm choice.");
        s.AnswerZhuji("arm");
        s.AnswerZhuji("hand", club: true);
        s.Need(s.E<ShiSsZhujiDiscardedEvent>() is [{ OwnerSeat: 0, Suit: Suit.Club }] &&
            s.E<ShiSsZhujiDiscardedEvent>()[0].DiscardedCount == clubsAtArm &&
            s.G.CardMovements.Count(m => m.Reason.Value == Scene.ZhujiDiscardReason && m.From.OwnerSeat == 0) == clubsAtArm,
            "Choosing a club discards exactly every club hand card.");
        s.Need(s.E<ShiSsZhujiEquippedEvent>() is [{ OwnerSeat: 0, GainedCardId: var gained, ReplacedCardId: null }] &&
            s.G.CardMovements.Any(m => m.CardId == gained && m.Reason.Value == Scene.ZhujiGainReason &&
                m.From.Zone == CardZoneKind.DrawPile && m.To.Zone == CardZoneKind.Hand && m.To.OwnerSeat == 0) &&
            s.G.CardMovements.Any(m => m.CardId == gained && m.Reason.Value == CardMoveReasons.EquipmentEnter.Value &&
                m.From.Zone == CardZoneKind.Hand && m.To.Zone == CardZoneKind.Equipment && m.To.OwnerSeat == 0),
            "The same-suit equipment left the draw pile and entered the empty weapon slot.");
        s.Need(s.P is { PlayerSeat: 0 } bonus && bonus.Kind == DecisionKind.ProgramTrigger &&
            bonus.Choices.Count == 3 && bonus.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "draw"),
            "Discarding at least two cards against a one-card zone qualifies the three-way bonus.");
        s.AnswerZhuji("draw");
        s.Need(s.E<ShiSsZhujiBonusEvent>() is [{ OwnerSeat: 0, Choice: "draw", Amount: 2 }] &&
            s.V(0).HandCount == handAtArm - clubsAtArm + 2,
            "The draw bonus nets two replacement cards after the full-suit discard.");
        s.Cold(); s.Replay();
    }

    private sealed record Enemy(int Hp, string[] Extras)
    {
        public ShanheStartingCard[] StartingHand { get; init; } = [];
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public const string ZhujiDiscardReason = "skill-program.mobile:shi-zhuji.discard";
        public const string ZhujiGainReason = "skill-program.mobile:shi-zhuji.gain";

        public GameEngine G { get; private set; } = game;
        public PendingDecision? P => Enumerable.Range(0, 5).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public bool Offered(string skill) =>
            G.GetHumanLegalActions().Any(a => a.Kind == LegalActionKind.UseProgramSkill && a.ProgramSkillId == skill);

        public void ActivateGanJue(int targetSeat)
        {
            Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } && Offered(GanJue));
            Accept0(new UseProgramSkillCommand(0, GanJue, "ganjue-use", [], [targetSeat], G.Revision, P!.PromptId));
        }

        public void AnswerGanJueMaterial(string slot)
        {
            Reach(() => P is { PlayerSeat: 0 } p && p.Kind == DecisionKind.ProgramTrigger &&
                p.SkillPrompt?.SkillId == GanJue);
            var marker = slot == "sword" ? "青釭" : "八卦";
            var choice = P!.Choices.First(c => c.Cards.Count > 0 && c.Label.Contains(marker));
            Accept0(new AnswerPromptCommand(P!.PlayerSeat, P!.PromptId, choice.Id, G.Revision));
        }

        public void PlayHandSlash(int targetSeat)
        {
            var slashId = V(0).Hand.Where(card => card.Kind == CardKind.Slash).Select(card => card.Id).First();
            Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } &&
                G.GetHumanLegalActions().Any(a => a.CardId == slashId && a.TargetSeats.Contains(targetSeat)));
            var a = G.GetHumanLegalActions().First(a => a.CardId == slashId && a.TargetSeats.Contains(targetSeat));
            Accept0(new PlayCardCommand(0, a.CardId!.Value, a.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind));
        }

        public void AnswerZhuji(string branch, bool club = false)
        {
            Reach(() => P is { PlayerSeat: 0 } p && p.Kind == DecisionKind.ProgramTrigger &&
                p.SkillPrompt?.SkillId == ZhuJi);
            var choice = branch == "hand"
                ? P!.Choices.First(c => c.Parameters.GetValueOrDefault("branch") == "hand" && club &&
                    V(0).Hand.Where(card => card.Suit == Suit.Club).Any(card => card.Id == c.Cards[0]))
                : P!.Choices.First(c => c.Parameters.GetValueOrDefault("branch") == branch);
            Accept0(new AnswerPromptCommand(P!.PlayerSeat, P!.PromptId, choice.Id, G.Revision));
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
                    if (p.PlayerSeat == 0 && p.SkillPrompt?.SkillId == ZhuJi &&
                        p.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "pass"))
                    { Answer(c => c.Parameters.GetValueOrDefault("branch") == "pass"); continue; }
                    if (p.PlayerSeat == 0 && p.SkillPrompt?.SkillId is ZhuJi or GanJue)
                        throw new InvalidOperationException("Unexpected Sun Shao boundary. " + Dump());
                    if (p.PlayerSeat == 0 && p.Kind is DecisionKind.RespondDodge or DecisionKind.RespondSlash or DecisionKind.Nullification or DecisionKind.RescueDying)
                    { Answer(c => c.Cards.Count == 0); continue; }
                    if (p.PlayerSeat == 0 && p.Kind != DecisionKind.PlayCard && p.SkillPrompt?.SkillId is not (ZhuJi or GanJue) &&
                        p.Choices.Any(c => c.Parameters.GetValueOrDefault("action") == "end-play"))
                    { Answer(c => c.Parameters.GetValueOrDefault("action") == "end-play"); continue; }
                    if (p.PlayerSeat == 0) throw new InvalidOperationException("Unexpected Sun Shao boundary. " + Dump());
                }
                Need(G.State.Winner == Winner.None, "The bounded real scene must retain its participants.");
                Accept0(new AdvanceOneStepCommand(G.Revision));
            }
            Need(false, "Sun Shao did not reach the expected bounded native boundary.");
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
        { var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, result.Error?.Message ?? "Rejected Sun Shao command."); }
        private string State() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 5).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),
            Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray(),
            G.CardMovements, Zones = G.CreateCardZoneDiagnostics(), Commands = CommandJson.Serialize(G.AcceptedCommands) });
        private string Dump() => JsonSerializer.Serialize(new { G.State.TurnNumber, G.State.CurrentSeat, G.State.Winner, P,
            Players = Enumerable.Range(0, 5).Select(i => new { i, V(i).Hp, V(i).HandCount }),
            Issued = E<ShiSsGanjueIssuedEvent>(), Settled = E<ShiSsGanjueSettledEvent>(),
            Discarded = E<ShiSsZhujiDiscardedEvent>(), Equipped = E<ShiSsZhujiEquippedEvent>(), Bonus = E<ShiSsZhujiBonusEvent>(),
            Damage = E<DamageAppliedEvent>().Select(e => new { e.TargetSeat, e.Amount }),
            Moves = G.CardMovements.TakeLast(40).Select(m => new { m.CardId, From = m.From.Zone, FromSeat = m.From.OwnerSeat, To = m.To.Zone, ToSeat = m.To.OwnerSeat, Reason = m.Reason.Value }),
            Last = G.Events.TakeLast(6).Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray() });

        public static Scene Start(string nodeId, int heroHp, string[] heroExtras, Enemy[] enemies, int openingCards = 4,
            ShanheStartingCard[]? startingEquipment = null, ShanheStartingCard[]? openingOverride = null)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            ShanheStartingCard[] opening = openingOverride ?? Enumerable.Range(0, openingCards)
                .Select(i => i == openingCards - 1
                    ? new ShanheStartingCard(CardKind.Duel, Suit.Spade, 6)
                    : new ShanheStartingCard(CardKind.Slash, Suit.Club, 4 + i % 9)).ToArray();
            ShanheCombatantDefinition Combatant(int seat, Enemy enemy) => new(Peer, Hp: enemy.Hp, ExtraSkillIds: enemy.Extras)
            { StartingHand = enemy.StartingHand };
            var setup = new ShanheBattleSetup(96, 1, nodeId, Hero, heroHp, heroHp - 3, 0, 0, 0, heroExtras, opening,
                startingEquipment ?? [], enemies.Select((enemy, index) => Combatant(index + 1, enemy)).ToArray(), [],
                new string('e', 64));
            var game = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 5, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 9 }, registry);
            var scene = new Scene(game, registry); scene.Accept0(new StartGameCommand()); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shi-sun-shao", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero);
            var rules = $$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
              {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"no-draw","window":"drawPhaseStarting","subject":"owner","optional":false,"isSkillInvocation":false,
                "effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]},
                {"id":"no-play","window":"afterNormalDraw","subject":"owner","optional":false,"isSkillInvocation":false,
                "effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]},
              {"id":"{{Keeper}}","revision":1,"modifiers":[{"id":"keep","query":"handLimit","operation":"add","value":10,"priority":0}]}
            ]}
            """;
            var presentations = new Dictionary<string, object>
            {
                [Quiet] = new { name = Quiet, description = "安静真实回合并跳过普通摸牌出牌。" },
                [Keeper] = new { name = Keeper, description = "保持手牌。" },
            };
            var catalog = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = presentations }));
            foreach (var (id, program) in catalog.Programs) b.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
            { Program = program, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new(Peer, "安静真实同桌", "", Quiet, "wei", 8));
            b.AddCard(new("fixture:shi-sun-shao-slash", "杀", "基本牌", "同体杀实体", CardKind.Slash));
            b.AddCard(new("fixture:shi-sun-shao-duel", "决斗", "锦囊牌", "同体决斗实体", CardKind.Duel));
            b.AddCard(new("fixture:shi-sun-shao-blade", "青釭剑", "装备牌", "同体青釭剑实体", CardKind.QinggangSword));
            b.AddDeck(new(Deck, "固定真实混合牌堆", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 150)
                .Select(i => i % 5 == 0
                    ? new ContentDeckPhysicalCard("fixture:shi-sun-shao-blade", Suit.Club, 3)
                    : i % 3 == 0
                        ? new ContentDeckPhysicalCard("fixture:shi-sun-shao-duel", Suit.Spade, 6)
                        : new ContentDeckPhysicalCard("fixture:shi-sun-shao-slash", Suit.Club, 5)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "势孙韶实际检查", 5, 5, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 4 }));
        }
    }
}
