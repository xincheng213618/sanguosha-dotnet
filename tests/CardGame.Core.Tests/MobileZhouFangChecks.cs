using System.Globalization;
using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileZhouFangChecks
{
    private const string Duanfa = "mobile:duanfa", Youdi = "mobile:youdi", DriverId = "fixture:mobile-zhou-fang-driver", WatchId = "fixture:mobile-zhou-fang-watch";
    private const string Mode = "identity:classic-mobile-zhou-fang", DiscardReason = "skill-program.mobile-duanfa.discard";
    public static void BlackHePaidCostColdAndSeparateActualPlayBudgetsInSameTurn()
    {
        var s = Scene.Start(extra: true); var equip = s.Find(c => c.Kind == CardKind.Crossbow);
        s.Native(equip, []); s.Play(0);
        var limit = s.V(0).MaxHp; var blackHand = s.V(0).Hand.Where(c => c.Suit is Suit.Spade or Suit.Club).Take(limit - 1).Select(c => c.Id).ToArray();
        s.Need(blackHand.Length == limit - 1 && s.V(0).Equipment.Any(c => c.Id == equip), "The actual fixture supplies one equipped black card and the remaining black hand costs.");
        var hand = s.V(0).HandCount; var drawBefore = s.Draws(0); var cost = new[] { equip }.Concat(blackHand).ToArray();
        s.UseDuanfa(); foreach (var card in cost) s.PickDuanfa(card);
        s.Need(s.P!.Choices.All(c => c.Parameters.GetValueOrDefault("branch") != "card") &&
            s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "pay"), "Selecting the full current phase allowance leaves no over-budget extra card choice.");
        s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "pay");
        s.Reach(() => s.IsWatch("duanfa-cost") || s.P is { Kind: DecisionKind.PlayCard });
        var paid = s.E<MobileDuanfaPaidEvent>().Single();
        s.Need(s.IsWatch("duanfa-cost") && paid.ActualCount == limit && paid.Limit == limit && s.Draws(0) == drawBefore && s.V(0).Equipment.Count == 0 &&
            s.V(0).HandCount == hand - blackHand.Length && s.G.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.MobileDuanfa is { Stage: MobileDuanfaStage.CostChildren, ActualCount: var actual } && actual == limit),
            "All actual black HE costs and the phase ledger commit before any draw, and the native cost child retains the original paid frame.");
        s.Cold(); s.ContinueWatch(); s.Play(0);
        s.Need(s.Draws(0) == drawBefore + limit && s.E<MobileDuanfaDrawReturnedEvent>() is [{ Requested: var requested, ActualCount: var drawn }] && requested == limit && drawn == limit &&
            !s.P!.Choices.Any(c => c.Parameters.GetValueOrDefault("skill-id") == Duanfa) &&
            s.G.CardMovements.Count(m => cost.Contains(m.CardId) && m.Reason.Value == DiscardReason && m.To == CardLocation.DiscardPile) == limit,
            "Cold return draws exactly the actual discard count once and the exhausted actual Play cannot start another Duanfa.");
        var turn = s.G.State.TurnNumber; s.End(); s.Play(0);
        s.Need(s.G.State.TurnNumber == turn && s.P!.Choices.Any(c => c.Parameters.GetValueOrDefault("skill-id") == Duanfa),
            "The normal Play after the inserted real Play has a fresh allowance in the same actual turn.");
        var second = s.Find(c => c.Suit is Suit.Spade or Suit.Club); s.UseDuanfa(); s.PickDuanfa(second);
        s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "pay"); s.Play(0);
        var ledger = s.E<MobileDuanfaPaidEvent>();
        s.Need(ledger.Length == 2 && ledger[1].ActualCount == 1 && ledger[1].SpentBefore == 0 && ledger[0].Phase.TurnNumber == ledger[1].Phase.TurnNumber &&
            ledger[0].Phase.PhaseInstanceId != ledger[1].Phase.PhaseInstanceId && s.E<MobileDuanfaDrawReturnedEvent>().Sum(e => e.ActualCount) == limit + 1,
            "Only distinct actual phase instances reset the owner budget; the later paid draw does not inherit the first phase's exhausted turn-wide ledger.");
    }
    public static void YoudiNonblackDrawPrecedesChosenHeTakeAndBlackElementalBranchesCold()
    {
        foreach (var variant in new[] { "red-nonslash", "black-nonslash", "black-elemental-slash" })
        {
            var s = Scene.Start(); s.Case = variant; s.EquipOther(1); var equipment = s.V(1).Equipment.Single().Id;
            var bait = variant switch
            {
                "red-nonslash" => s.Find(c => c.Kind == CardKind.Dodge && c.Suit == Suit.Heart),
                "black-nonslash" => s.Find(c => c.Kind == CardKind.Dodge && c.Suit == Suit.Club),
                _ => s.Find(c => c.Kind == CardKind.FireSlash && c.Suit == Suit.Spade)
            };
            var hand = s.V(0).HandCount; var draws = s.Draws(0); s.BeginYoudi(1);
            var slot = s.V(0).Hand.Select((c, i) => (c, i)).Single(p => p.c.Id == bait).i;
            s.Need(s.P!.PlayerSeat == 1 && s.P.Choices.All(c => c.Cards.Count == 0), "The selected counterpart chooses a real concealed hand slot, without learning physical bait identities.");
            s.Answer(c => c.Parameters.GetValueOrDefault("source-zone") == nameof(CardZoneKind.Hand) && c.Parameters.GetValueOrDefault("slot-index") == slot.ToString(CultureInfo.InvariantCulture));
            if (variant == "red-nonslash")
            {
                s.Reach(() => s.IsWatch("youdi-draw") || s.IsTakePrompt || s.P is { Kind: DecisionKind.PlayCard });
                s.Need(s.IsWatch("youdi-draw") && s.Draws(0) == draws + 1 && s.V(0).HandCount == hand && s.V(1).Equipment.Any(c => c.Id == equipment),
                    "The nonblack bait first draws one actual card and suspends in that native draw child before obtaining any counterpart card.");
                s.Cold(); s.ContinueWatch();
            }
            if (variant != "black-elemental-slash")
            {
                s.Reach(() => s.IsTakePrompt || s.P is { Kind: DecisionKind.PlayCard });
                s.Need(s.IsTakePrompt && s.P!.Choices.Any(c => c.Cards.SequenceEqual([equipment])) && s.Draws(0) == draws + (variant == "red-nonslash" ? 1 : 0),
                    "A non-Slash bait lets Zhou Fang choose a real HE entity, including the visible equipment; black alone never grants a draw.");
                s.Answer(c => c.Cards.SequenceEqual([equipment]));
            }
            s.Play(1);
            s.Need(s.G.CardMovements.Count(m => m.CardId == bait && m.From == CardLocation.Hand(0) && m.To == CardLocation.DiscardPile) == 1 &&
                s.Draws(0) == draws + (variant == "red-nonslash" ? 1 : 0) &&
                (variant == "black-elemental-slash" ? s.V(1).Equipment.Any(c => c.Id == equipment) && s.V(0).HandCount == hand - 1 :
                    !s.V(1).Equipment.Any(c => c.Id == equipment) && s.V(0).Hand.Any(c => c.Id == equipment)),
                "Both Youdi branches follow the actual discarded physical card once: red non-Slash draws then takes, black non-Slash only takes, and black Fire Slash grants neither reward.");
        }
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public string Case = "duanfa-phase";
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public int Draws(int seat) => E<CardsDrawnEvent>().Where(e => e.PlayerSeat == seat).Sum(e => e.Count);
        public bool IsWatch(string bind) => P?.SkillPrompt?.SkillId == WatchId && P.Choices.Any(c => c.Parameters.GetValueOrDefault("result-bind") == bind);
        public bool IsTakePrompt => P?.SkillPrompt?.SkillId == Youdi && P.PlayerSeat == 0 && P.Choices.Any(c => c.Parameters.GetValueOrDefault("card-owner-seat") == "1");
        public void ContinueWatch() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void Driver(string id, int[] targets) => Accept(new UseProgramSkillCommand(0, DriverId, id, [], targets, G.Revision, P!.PromptId));
        public int Find(Func<CardSnapshot, bool> predicate)
        {
            for (var i = 0; i < 5; i++) { var card = V(0).Hand.FirstOrDefault(predicate); if (card is not null) return card.Id; Driver("draw-three", []); Play(0); }
            throw new InvalidOperationException("The bounded mixed native deck did not provide the requested material.");
        }
        public void EquipOther(int seat)
        { for (var i = 0; i < 4 && V(seat).Equipment.Count == 0; i++) { Driver("equip", [seat]); Play(0); } Need(V(seat).Equipment.Count == 1, "The real equipment fixture must equip the selected counterpart."); }
        public void UseDuanfa()
        { Accept(new UseProgramSkillCommand(0, Duanfa, "phase-black-recycle", [], [], G.Revision, P!.PromptId)); Reach(() => P?.SkillPrompt?.SkillId == Duanfa); }
        public void PickDuanfa(int card) => Answer(c => c.Cards.SequenceEqual([card]) && c.Parameters.GetValueOrDefault("branch") == "card");
        public void BeginYoudi(int target)
        {
            End(); Reach(() => P?.SkillPrompt?.SkillId == Youdi && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
            Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
            Reach(() => P?.SkillPrompt?.SkillId == Youdi && P.Choices.Any(c => c.Targets.SequenceEqual([target]))); Answer(c => c.Targets.SequenceEqual([target]));
            Reach(() => P?.SkillPrompt?.SkillId == Youdi && P.PlayerSeat == target && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card"));
        }
        public void Native(int card, int[] targets)
        {
            var a = G.GetHumanLegalActions().First(a => a.CardId == card && a.ConversionSource is null && a.TargetSeats.SequenceEqual(targets));
            Accept(new PlayCardCommand(0, card, a.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind, a.TargetCardId));
        }
        public void Play(int seat) => Reach(() => P is { Kind: DecisionKind.PlayCard } p && p.PlayerSeat == seat);
        public void End() => Accept(new EndPlayPhaseCommand(P!.PlayerSeat, G.Revision, P.PromptId));
        public void Answer(Func<PromptChoice, bool> predicate)
        { var p = P!; var c = p.Choices.FirstOrDefault(predicate); Need(c is not null, "Required native Zhou Fang choice is missing."); Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, c!.Id, G.Revision)); }
        public void Accept(GameCommand command) { var r = G.Submit(command); Need(r.Accepted, r.Error?.Message ?? "Actual mobile Zhou Fang command rejected."); }
        public void Reach(Func<bool> ready)
        {
            for (var i = 0; i < 220 && !ready(); i++)
            {
                if (P is { Kind: DecisionKind.SelectGeneral } g) { Accept(new SelectGeneralCommand(g.PlayerSeat, General(g.PlayerSeat), G.Revision, g.PromptId)); continue; }
                if (P is { Kind: DecisionKind.PlayCard }) { End(); continue; }
                if (P is { Kind: DecisionKind.DiscardCards } d) { Accept(new DiscardCardsCommand(d.PlayerSeat, d.ValidCardIds.Take(d.RequiredCardCount).ToArray(), d.PromptId, G.Revision)); continue; }
                if (P is { } p)
                {
                    var c = p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("program-action") == "skip") ??
                        p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage" or "let-die") ??
                        p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("option-id") == "continue") ?? p.Choices.First();
                    Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, c.Id, G.Revision));
                }
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(ready(), "Missing bounded actual mobile Zhou Fang boundary.");
        }
        public void Cold()
        {
            var copy = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(Enumerable.Range(0, 4).All(i => SnapshotJson.Serialize(G.CreateSnapshot(i)) == SnapshotJson.Serialize(copy.CreateSnapshot(i))) &&
                G.CardMovements.SequenceEqual(copy.CardMovements) && G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).SequenceEqual(copy.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType()))),
                "Cold recovery preserves the original paid HE discard, actual phase allowance and native reward child."); G = copy;
        }
        public void Need(bool ok, string text)
        {
            if (!ok) throw new InvalidOperationException(text + " " + JsonSerializer.Serialize(new { Case, OwnerDraws = Draws(0), OwnerHand = V(0).HandCount, TargetEquipment = V(1).Equipment.Select(c => c.Id).ToArray(), Prompt = P is { } p ? new { p.Kind, p.PlayerSeat, Skill = p.SkillPrompt?.SkillId,
                    Choices = p.Choices.Take(8).Select(c => new { c.Cards, c.Targets, c.Parameters }).ToArray(), ChoiceCount = p.Choices.Count } : null, G.State.TurnNumber, G.State.CurrentSeat,
                Facts = G.Events.Select(e => e.Payload).Where(e => e is MobileDuanfaIssuedEvent or MobileDuanfaPaidEvent or MobileDuanfaDrawReturnedEvent or MobileYoudiBaitPublishedEvent or ProgramBindingResolvedEvent)
                    .TakeLast(9).Select(e => JsonSerializer.Serialize(e, e.GetType())).ToArray() }));
        }
        public static Scene Start(bool extra = false)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(extra));
            var s = new Scene(GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
                ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 9 }, registry), registry);
            s.Accept(new StartGameCommand()); s.Play(0); return s;
        }
    }
    private static string General(int seat) => "fixture:mobile-zhou-fang-general-" + seat;
    private sealed class Fixture(bool extra) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:mobile-zhou-fang", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, "mobile:zhou-fang");
            var insert = extra ? """{"id":"first-extra-play","window":"turnStartBeforeNormalFlow","subject":"owner","optional":false,"usageScope":"game","usageLimit":1,"effects":[{"op":"insertPhase","target":"owner","phase":"play","phaseContinuation":"beforeNormalPreparation"}]}""" : "";
            var rules = $$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
              {"id":"{{DriverId}}","revision":1,"modifiers":[{"id":"retain-fixture-materials","query":"handLimit","operation":"add","value":20,"priority":0}],"triggers":[{{insert}}],"activations":[
                {"id":"equip","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"useRandomDeckEquipment","target":"owner","resultBind":"equipment"}]},
                {"id":"draw-three","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"draw","target":"owner","amount":3}]}
              ]},
              {"id":"{{WatchId}}","revision":1,"triggers":[
                {"id":"paid-duanfa-child","window":"cardsMoved","subject":"owner","sourceZones":["hand","equipment"],"movementReasons":["{{DiscardReason}}"],"movementOccurrence":"perOwnerBatch","usageScope":"game","usageLimit":1,"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"duanfa-cost","options":[{"id":"continue"}]}]},
                {"id":"youdi-native-draw-child","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementReasons":["skill-program.mobile:youdi.Draw"],"movementOccurrence":"perBatch","usageScope":"game","usageLimit":1,"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"youdi-draw","options":[{"id":"continue"}]}]}
              ]}
            ]}
            """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [DriverId] = new { name = "真实移动周鲂准备", description = "实际插入出牌阶段、装备和手牌材料。" },
                    [WatchId] = new { name = "实际周鲂子窗", description = "原生弃牌或摸牌暂停。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } } });
            var catalog = SkillProgramCatalog.Load(rules, presentation);
            foreach (var (id, p) in catalog.Programs) b.AddSkill(new(id, id, id) { Program = p, ProgramPresentation = catalog.Presentations[id] });
            for (var seat = 0; seat < 4; seat++) b.AddGeneral(new(General(seat), "固定真实参与者", "supporter", seat == 0 ? Duanfa : "standard:none", "wu", seat == 0 ? 3 : 4,
                seat == 0 ? [Youdi, DriverId, WatchId] : []));
            const string deck = "fixture:mobile-zhou-fang-deck";
            var palette = new[] { new ContentDeckPhysicalCard("standard:crossbow", Suit.Club, 1), new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 2),
                new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, 2), new ContentDeckPhysicalCard("standard:fire_slash", Suit.Spade, 7) };
            b.AddDeck(new(deck, "固定真实黑红材料", 12, 0, []) { PhysicalCards = Enumerable.Range(0, 40).SelectMany(_ => palette).ToArray() });
            b.AddMode(new(Mode, "移动周鲂原生流程", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 1, [nameof(Role.Renegade)] = 2 }, deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Enumerable.Range(0, 4).Select(General).ToArray()));
        }
    }
}
