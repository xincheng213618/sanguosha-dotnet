using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShiHuanJieChecks
{
    private const string Hero = "mobile:shi-huan-jie", Gongmou = "mobile:shi-gongmou", Zhengshuo = "mobile:shi-zhengshuo",
        Driver = "fixture:shi-huan-jie-driver", Watch = "fixture:shi-huan-jie-watch", Quiet = "fixture:shi-huan-jie-quiet", Spent = "fixture:shi-huan-jie-spent", Loss = "fixture:shi-huan-jie-loss",
        Peer = "fixture:shi-huan-jie-peer", Deck = "fixture:shi-huan-jie-deck";

    public static void AtomicHandExchangePaidSourceLossDerivedSkillsAndActualTurnExpiryCold()
    {
        var s = Scene.Start(permanentKanpo: true); s.Offer(Gongmou);
        var owner = s.V(0).Hand.Select(c => c.Id).Order().ToArray(); var other = s.V(1).Hand.Select(c => c.Id).Order().ToArray();
        s.Activate(); s.Reach(() => s.P?.Choices.Any(c => Key(c) == "mobile-shi-huan-jie") == true); s.Cold(); s.Answer(c => c.Targets.SequenceEqual([1]));
        s.AtWatch(); var root = s.Root!; var paid = s.E<MobileShiHuanJieMovementPaidEvent>().Single(e => e.Stage == MobileShiHuanJieStage.ExchangeChildren);
        s.Need(root.MobileShiHuanJie is { Stage: MobileShiHuanJieStage.ExchangeChildren, Target: 1 } && root.PendingMovementContinuation is not null &&
            s.V(0).Hand.Select(c => c.Id).Order().SequenceEqual(other) && s.V(1).Hand.Select(c => c.Id).Order().SequenceEqual(owner) &&
            paid.Count == owner.Length + other.Length && paid.BatchId is not null &&
            s.G.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Single(f => f.Batch.Id == paid.BatchId).Batch.Movements.Count == paid.Count &&
            !s.HasSkill(0, Gongmou) && !s.HasSkill(0, "classic:qice") && s.E<ProgramTurnSkillsGrantedEvent>().All(e => e.FrameId != root.Id),
            "Both original real hands exchange in one physical batch before its paid child; losing Gongmou during that child cannot cause early or duplicate derived grants.");
        s.Need(s.G.CreateSnapshot(2).Players.Where(p => p.Seat is 0 or 1).All(p => p.Hand.Count == 0), "The public observer sees the exchange counts without either original private hand.");
        s.Cold(); s.ContinueWatch(); s.Play();
        var grants = s.E<ProgramTurnSkillsGrantedEvent>().Where(e => e.FrameId == root.Id).ToArray();
        s.Need(grants.Length == 2 && grants.Any(e => e.RecipientSeat == 0 && e.GrantedSkillIds.SequenceEqual(["classic:qice"])) &&
            grants.Any(e => e.RecipientSeat == 1 && e.GrantedSkillIds.SequenceEqual(["classic:kanpo"])) &&
            grants.All(e => e.TurnExpiry == new SkillGrantTurnExpiry(1, 0)) && s.HasSkill(0, "classic:qice") && s.HasSkill(1, "classic:kanpo"),
            "The settled original exchange still grants both existing skills after source loss, each bound to this exact actual turn.");
        var qiceHand = s.V(0).Hand.Select(c => c.Id).Order().ToArray();
        s.Accept(new UseProgramSkillCommand(0, "classic:qice", "all-hand-as-ordinary-trick", qiceHand, [], s.G.Revision, s.P!.PromptId));
        s.Reach(() => s.P?.Choices.Any(c => Key(c) == "use-all-hand-as-ordinary-trick") == true); s.Cold();
        s.Answer(c => c.Parameters.GetValueOrDefault("card-kind") == nameof(CardKind.DrawTwo)); s.Play();
        s.Need(s.E<ProgramViewAsConvertedEvent>().Single(e => e.SkillId == "classic:qice").PhysicalCardIds.Order().SequenceEqual(qiceHand) &&
            s.G.GetHumanLegalActions().All(a => a.ProgramSkillId != "classic:qice"), "The actual granted Qice consumes its complete current hand through the existing once-per-Play native view-as flow.");
        s.End(); s.Reach(() => s.E<TurnStartedEvent>().Any(e => e.TurnNumber == 2 && e.ActorSeat == 1));
        s.Need(!s.HasSkill(0, "classic:qice") && s.HasSkill(1, "classic:kanpo") &&
            s.E<ProgramTurnSkillGrantExpiredEvent>().Count(e => e.TurnExpiry == new SkillGrantTurnExpiry(1, 0) && e.SkillId is "classic:qice" or "classic:kanpo") == 2 &&
            s.E<MobileShiHuanJieMovementPaidEvent>().Count(e => e.Stage == MobileShiHuanJieStage.ExchangeChildren) == 1,
            "Both temporary sources expire at the real turn end while the original printed Kanpo survives; cold recovery never pays the exchange twice."); s.Cold(); s.Replay();
    }

    private static string? Key(PromptChoice c) => c.Parameters.GetValueOrDefault("program-action");
    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G { get; private set; } = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public ProgramSkillFrame? Root => G.ResolutionStack.OfType<ProgramSkillFrame>().LastOrDefault(f => f.MobileShiHuanJie is not null);
        public bool HasSkill(int seat, string id) => V(seat).Skills?.Any(skill => skill.ContentId == id) == true;
        public void Offer(string id) => Reach(() => P?.SkillPrompt?.SkillId == id && P.Choices.Any(c => Key(c) == "activate"));
        public void Activate() => Answer(c => Key(c) == "activate");
        public void AtWatch() => Reach(() => P?.SkillPrompt?.SkillId == Watch);
        public void ContinueWatch() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void End() => Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
        public void Native(CardKind kind, int[] targets)
        {
            var action = G.GetHumanLegalActions().FirstOrDefault(a => a.CardId is > 0 && a.ConversionSource is null && a.Kind != LegalActionKind.UseProgramSkill &&
                V(0).Hand.Any(c => c.Id == a.CardId && c.Kind == kind) && a.TargetSeats.SequenceEqual(targets));
            Need(action is not null, "The prepared physical card must have its native action.");
            Accept(new PlayCardCommand(0, action!.CardId!.Value, action.TargetSeats, G.Revision, P!.PromptId, action.PlayedCardKind, action.TargetCardId));
        }
        public void Answer(Func<PromptChoice, bool> pick)
        { var p = P!; var c = p.Choices.FirstOrDefault(pick); Need(c is not null, "The exact current native choice exists."); Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, c!.Id, G.Revision)); }
        public void Reach(Func<bool> ready)
        {
            for (var n = 0; n < 160; n++)
            {
                if (ready()) return;
                if (P is { PlayerSeat: 0 } p)
                {
                    if (p.Choices.Any(c => Key(c) == "skip")) { Answer(c => Key(c) == "skip"); continue; }
                    if (p.Kind is DecisionKind.RespondSlash or DecisionKind.RespondDodge or DecisionKind.Nullification or DecisionKind.RescueDying)
                    { Answer(c => c.Cards.Count == 0 && c.Parameters.GetValueOrDefault("conversion-skill-id") is null); continue; }
                    throw new InvalidOperationException("Unexpected native Shi Huan Jie boundary. " + Dump());
                }
                Need(G.State.Winner == Winner.None, "The bounded native scene retains living players."); Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(false, "The bounded native Shi Huan Jie scene reaches its next exact boundary.");
        }
        public void Cold()
        { var before = State(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry); Need(State() == before, "Cold recovery retains the exact original paid hand batch, derived grants, shuffle and draw recipient cursor."); }
        public void Replay()
        { var before = State(); G = GameReplay.Replay(G.CreateCheckpoint().Options, G.AcceptedCommands, registry); Need(State() == before, "The original native command journal replays the exact exchange, skill lifetimes and real global hand reset."); }
        public void Accept(GameCommand command)
        { var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, result.Error?.Message ?? "Rejected native Shi Huan Jie command."); }
        public void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message + " " + Dump()); }
        private string State() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),
            Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements,
            Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray(), Commands = CommandJson.Serialize(G.AcceptedCommands) });
        private string Dump() => JsonSerializer.Serialize(new { G.State.TurnNumber, P, Root?.MobileShiHuanJie,
            Paid = E<MobileShiHuanJieMovementPaidEvent>(), Draws = E<MobileShiHuanJieDrawCompletedEvent>(),
            Players = Enumerable.Range(0, 4).Select(i => new { i, V(i).Hp, V(i).HandCount }) });
        public static Scene Start(bool permanentKanpo = false)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(permanentKanpo));
            var setup = new ShanheBattleSetup(92, 1, "shi-huan-jie", Hero, 4, 0, 0, 0, 0, [Driver, Watch, Loss],
                [new(CardKind.BaguaFormation, Suit.Club, 2), new(CardKind.Indulgence, Suit.Club, 6), new(CardKind.Dodge, Suit.Heart, 3), new(CardKind.Peach, Suit.Heart, 4)], [],
                [new(Peer, Hp: 8) { StartingHand = [new(CardKind.Dodge, Suit.Spade, 4), new(CardKind.Dodge, Suit.Club, 5), new(CardKind.Peach, Suit.Heart, 6), new(CardKind.Peach, Suit.Diamond, 7)] },
                 new(Peer, Hp: 8) { StartingHand = [new(CardKind.Dodge, Suit.Club, 8)] }, new(Peer, Hp: 8) { StartingHand = [new(CardKind.Peach, Suit.Heart, 9)] }], [], new string('a', 64));
            var g = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 6 }, registry);
            var s = new Scene(g, registry); s.Accept(new StartGameCommand()); return s;
        }
    }
    private sealed class Fixture(bool permanentKanpo) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shi-huan-jie", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero, "classic:xun-you", "classic:wolong-zhuge-liang");
            var rules = $$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
              {"id":"{{Driver}}","revision":1,"modifiers":[{"id":"keep","query":"handLimit","operation":"add","value":20,"priority":0}],
               "triggers":[{"id":"no-draw","window":"drawPhaseStarting","subject":"owner","optional":false,"effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]}]},
              {"id":"{{Watch}}","revision":1,"triggers":[
                {"id":"exchange-source-loss","priority":1,"window":"cardsMoved","subject":"owner","sourceZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["skill-program.mobile-shi-huan-jie.exchange"],"optional":false,"isSkillInvocation":false,"effects":[{"op":"loseHp","target":"owner","amount":4}]},
                {"id":"exchange-child","window":"cardsMoved","subject":"owner","sourceZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["skill-program.mobile-shi-huan-jie.exchange"],"optional":false,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]},
                {"id":"discard-source-loss","priority":1,"window":"cardsMoved","subject":"owner","sourceZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["skill.mobile-shi-huan-jie.discard"],"optional":false,"isSkillInvocation":false,"effects":[{"op":"loseHp","target":"owner","amount":4}]},
                {"id":"discard-child","window":"cardsMoved","subject":"owner","sourceZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["skill.mobile-shi-huan-jie.discard"],"optional":false,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]},
                {"id":"draw-child","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["skill-program.mobile-shi-huan-jie.draw"],"optional":false,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]}]},
              {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"no-draw","window":"drawPhaseStarting","subject":"owner","optional":false,"effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]},{"id":"no-play","window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]},
              {"id":"{{Loss}}","revision":1,"triggers":[{"id":"actual-dying-loss","window":"dyingEntering","subject":"owner","optional":false,"usageScope":"game","usageLimit":1,"effects":[{"op":"recoverTo","target":"owner","numberExpression":"integerConstant","minimumValue":4,"clampToMaxHp":true},{"op":"loseOwnerSkillsAndGrant","target":"owner","skillIds":["{{(permanentKanpo ? Gongmou : Zhengshuo)}}"],"sourceBind":"{{Spent}}"}]}]}
            ]}
            """;
            var catalog = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Driver] = new { name = "保留原生手牌", description = "固定实际手牌与摸牌阶段。" },
                    [Watch] = new { name = "已支付交换或弃牌子结算", description = "在真实移动支付后丢失原技能并等待。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } },
                    [Quiet] = new { name = "安静同桌", description = "真实回合跳过出牌。" }, [Loss] = new { name = "真实濒死失源", description = "原生濒死入口回复并终结替换原技能。" } } }));
            foreach (var (id, program) in catalog.Programs) b.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                { Program = program, ProgramPresentation = catalog.Presentations[id] });
            b.AddSkill(new(Spent, "原技能已失去", "保留原生支付后的子结算，无新增触发。"));
            b.AddGeneral(new(Peer, "实际同桌角色", "", Quiet, "wei", 10, AdditionalSkillIds: permanentKanpo ? ["classic:kanpo"] : []));
            b.AddDeck(new(Deck, "足量实际桃牌堆", 0, 2, []) { PhysicalCards = Enumerable.Range(0, 100)
                .Select(_ => new ContentDeckPhysicalCard("standard:peach", Suit.Heart, 7)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "势桓阶真实换牌与全局正朔", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}
