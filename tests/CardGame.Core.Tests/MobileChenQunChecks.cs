using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileChenQunChecks
{
    private const string Hero = "mobile:chen-qun", Dingpin = "mobile:dingpin", Faen = "boundary:faen-mobile",
        Driver = "fixture:mobile-chen-qun-driver", Hold = "fixture:mobile-chen-qun-cost",
        Owner = "fixture:mobile-chen-qun-owner", Mode = "identity:classic-mobile-chen-qun-fixture",
        Deck = "fixture:mobile-chen-qun-deck";

    public static void BlackDrawsAllLostHpAndBansTargetAfterPaidCostCold()
    {
        var s = Scene.Start(Suit.Spade); s.Play();
        s.Need(s.Action()?.SelectableTargetSeats?.Contains(1) == false && s.Action()?.SelectableTargetSeats?.Contains(3) == true,
            "Dingpin targets actual wounded players and excludes an otherwise living full-HP peer.");
        var equipment = s.V().Hand.First(c => EquipmentCatalog.IsEquipment(c.Kind)).Id;
        s.NativeEquipment(equipment); s.Play();
        s.Need(s.V().Hand.Any(c => EquipmentCatalog.IsEquipment(c.Kind)) &&
            s.Action()!.SelectableCardIds!.All(id => !EquipmentCatalog.IsEquipment(s.Card(id).Kind)),
            "A real native equipment use spends Equipment for this actual turn even while other equipment remains in hand.");
        var cost = s.V().Hand.First(c => c.Kind == CardKind.Dodge).Id;
        var before = s.V(3).Hand.Count; var lost = s.V(3).MaxHp - s.V(3).Hp;
        s.Need(lost == 6, "The small fixture has an actually wounded 8-maximum-HP, 2-current-HP target.");
        s.Dingpin(cost, 3); s.Cost();
        var prepared = s.Facts<MobileDingpinPreparedEvent>().Single();
        s.Need(prepared.CardId == cost && prepared.Target == 3 && s.Zone(cost) == CardLocation.DiscardPile &&
            s.Facts<JudgmentRequestedEvent>().Length == 0 &&
            s.Facts<NativeLogicalDiscardMaterialCompletedEvent>().Count(e => e.CardId == cost) == 1,
            "The original real hand cost is logically discarded once before its native child and before judgment.");
        s.Cold(); s.Release(); s.Play();
        s.Need(s.V(3).Hand.Count == before + lost && s.Facts<MobileDingpinCompletedEvent>() is [{ Target: 3, Suit: Suit.Spade }] &&
            s.G.CardMovements.Count(m => m.CardId == cost && m.To == CardLocation.DiscardPile) == 1 &&
            s.Action()!.SelectableTargetSeats!.Contains(2) && !s.Action()!.SelectableTargetSeats!.Contains(3),
            "The finalized black result draws all six lost-HP cards without a three-card cap and bans only its original wounded target for the current turn.");
        s.Need(s.V().Hand.Any(c => c.Kind == CardKind.Dodge) &&
            s.Action()!.SelectableCardIds!.All(id => s.Card(id).Kind is CardKind.IronChain or CardKind.Dismantlement),
            "The actual Basic discard and earlier actual Equipment use are both excluded, while unused Trick costs remain legal.");
        s.Cold();
    }

    public static void BothRedSuitsTurnOverWithoutRefundAndFaenUsesOriginalWindows()
    {
        foreach (var suit in new[] { Suit.Heart, Suit.Diamond })
        {
            var s = Scene.Start(suit); s.Play();
            var cost = s.V().Hand.First(c => c.Kind == CardKind.Dodge).Id;
            s.Dingpin(cost, 3); s.Cost(); s.Release(); s.Faen();
            s.Need(s.V().IsFaceDown && s.Facts<MobileDingpinCompletedEvent>() is [var completed] && completed.Suit == suit && s.FaenDraws() == 0,
                "Both finalized red suits really turn Chen Qun face down and offer mobile Faen before its optional draw.");
            s.AcceptFaen(); s.Play();
            s.Need(s.Action()!.SelectableTargetSeats!.Contains(3) && s.FaenDraws() == 1 &&
                s.V().Hand.Any(c => c.Kind == CardKind.Dodge) &&
                s.Action()!.SelectableCardIds!.All(id => s.Card(id).Kind != CardKind.Dodge),
                "Red Dingpin retains the paid Basic-category exclusion and legal wounded target; accepting mobile Faen draws once on the actual face-down transition.");
            s.Drive("flip"); s.Faen(); s.AcceptFaen(); s.Play();
            s.Need(!s.V().IsFaceDown && s.FaenDraws() == 2,
                "The next real face-up transition independently invokes mobile Faen and draws exactly once.");
            s.Drive("chain"); s.Faen(); s.AcceptFaen(); s.Play();
            s.Need(s.V().IsChained && s.FaenDraws() == 3,
                "Entering chain supplies the third actual mobile Faen opportunity after both turnover directions.");
        }
    }

    private static string Peer(int seat) => "fixture:mobile-chen-qun-peer-" + seat;
    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat = 0) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public CardSnapshot Card(int id) => V().Hand.Single(c => c.Id == id);
        public T[] Facts<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public CardLocation Zone(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        public LegalAction? Action() => G.GetHumanLegalActions().SingleOrDefault(a => a.ProgramSkillId == MobileChenQunChecks.Dingpin);
        public int FaenDraws() => G.CardMovements.Count(m => m.Reason.Value == "skill-program.boundary:faen-mobile.Draw" && m.To == CardLocation.Hand(0));
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Cost() => Reach(() => P?.SkillPrompt?.SkillId == Hold);
        public void Faen() => Reach(() => P?.SkillPrompt?.SkillId == MobileChenQunChecks.Faen);
        public void Release() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void Dingpin(int card, int target) => Accept(new UseProgramSkillCommand(0, MobileChenQunChecks.Dingpin,
            "judge-unused-category", [card], [target], G.Revision, P!.PromptId));
        public void Drive(string action) => Accept(new UseProgramSkillCommand(0, Driver, action, [], [], G.Revision, P!.PromptId));
        public void NativeEquipment(int card)
        {
            var action = G.GetHumanLegalActions().First(a => a.CardId == card && a.Kind != LegalActionKind.Recast);
            Accept(new PlayCardCommand(0, card, action.TargetSeats, G.Revision, P!.PromptId, action.PlayedCardKind, action.TargetCardId)
                { ConversionSource = action.ConversionSource, AdditionalConversionSources = action.AdditionalConversionSources });
        }
        public void Answer(Func<PromptChoice, bool> predicate)
        { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.Single(predicate).Id, G.Revision)); }
        public void AcceptFaen()
        {
            Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
            Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }, settleFaen: true);
        }
        public void Reach(Func<bool> done, bool settleFaen = false)
        {
            for (var i = 0; i < 160 && !done(); i++)
            {
                if (P is { Kind: DecisionKind.SelectGeneral } select)
                { Accept(new SelectGeneralCommand(select.PlayerSeat, select.PlayerSeat == 0 ? Owner : Peer(select.PlayerSeat), G.Revision, select.PromptId)); continue; }
                if (settleFaen && P?.SkillPrompt?.SkillId == MobileChenQunChecks.Faen)
                { Answer(c => c.Targets.Count == 1); continue; }
                Need(P is null, "Unexpected native mobile Chen Qun boundary.");
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(done(), "The fixed native Dingpin fixture missed its bounded continuation.");
        }
        public void Cold()
        {
            string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),
                Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements,
                Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray() });
            var original = Dump();
            G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(original == Dump(), "Cold restores the original paid hand cost, native judgment/draw history and actual turn restrictions without repeating payment.");
        }
        public void Need(bool value, string message)
        { if (!value) throw new InvalidOperationException(message + " " + JsonSerializer.Serialize(new { Pending = P, State = G.State, Hand = V().Hand })); }
        private void Accept(GameCommand command)
        { var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, result.Error?.Message ?? "Rejected native mobile Chen Qun command."); }
        public static Scene Start(Suit suit)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(suit));
            var game = GameEngine.CreateStandard(new() { Seed = 29, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3],
                HumanRole = Role.Lord, ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 5 }, registry);
            var scene = new Scene(game, registry); scene.Accept(new StartGameCommand()); return scene;
        }
    }
    private sealed class Fixture(Suit suit) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-chen-qun", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero, "boundary:chen-qun-mobile");
            var catalog = SkillProgramCatalog.Load($$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
              {"id":"{{Hold}}","revision":1,"triggers":[{"id":"original-paid-cost","window":"cardsMoved","subject":"owner",
                "sourceZones":["hand"],"movementOccurrence":"perOwnerBatch","movementReasons":["skill-program.mobile:dingpin.MoveBoundCards"],
                "optional":false,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]}]},
              {"id":"{{Driver}}","revision":1,"activations":[
                {"id":"flip","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"turnOver","target":"owner"}]},
                {"id":"chain","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"setChainedState","target":"owner","chained":true}]}
              ]}
            ]}
            """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> {
                    [Driver] = new { name = "真实状态驱动", description = "原生翻面和连环。" },
                    [Hold] = new { name = "原付款子窗", description = "已实际弃牌，尚未开始判定。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } }
                } }));
            foreach (var (id, program) in catalog.Programs) b.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                { Program = program, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new(Owner, "移动定品完整规则持有者", "supporter", Dingpin, "wei", 3, [Faen, Driver, Hold]));
            for (var seat = 1; seat < 4; seat++) b.AddGeneral(new(Peer(seat), "实际已受伤同桌", "supporter", "standard:none", "wei", 8)
                { InitialHp = seat == 3 ? 2 : seat == 2 ? 4 : null });
            string[] cards = ["standard:dodge", "standard:dodge", "standard:crossbow", "standard:qinggang_sword", "standard:iron_chain", "standard:dismantlement"];
            b.AddDeck(new(Deck, "同判定花色的固定实体库存", 12, 0, []) { PhysicalCards = Enumerable.Range(0, 120)
                .Select(i => new ContentDeckPhysicalCard(cards[i % cards.Length], suit, 7)).ToArray() });
            b.AddMode(new(Mode, "移动陈群实际伤势与类型", 4, 4, new Dictionary<string, int> {
                [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: [Owner, Peer(1), Peer(2), Peer(3)]));
        }
    }
}
