using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSourceKeshouChecks
{
    private const string Skill = StandardShanheSourceKeshouPackage.SkillId;
    private const string Owner = "fixture:source-keshou-owner", Peer = "fixture:source-keshou-peer", Driver = "fixture:source-keshou-driver",
        Watch = "fixture:source-keshou-cost-child", Deck = "fixture:source-keshou-deck";
    private const string CostReason = "skill-program.shanhe:source-keshou-993.MoveBoundCards";

    public static void SameColourHePaymentRedJudgmentAndCurrentFactionGateUseTheOriginalDamageFrame()
    {
        foreach (var sharedFaction in new[] { false, true })
        {
            var s = Scene.Start(sharedFaction: sharedFaction);
            var hand = s.V(0).Hand.Single(c => c.Kind == CardKind.Dodge).Id;
            var equipment = s.V(0).Equipment.Single(c => c.Kind == CardKind.Crossbow).Id;
            s.Use("self-two"); s.Until(() => s.P?.SkillPrompt?.SkillId == Skill);
            s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
            s.Until(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "source-keshou-cost") == true);
            Need(s.P!.Choices.All(c => c.Cards.Order().SequenceEqual(new[] { hand, equipment }.Order())) &&
                s.G.CardMovements.All(m => m.CardId != hand || m.From != CardLocation.Hand(0)),
                "The original optional damage trigger exposes only the actual same-colour HE pair and does not pay before the real selection.");
            s.Answer(c => c.Cards.Order().SequenceEqual(new[] { hand, equipment }.Order()));
            s.Until(() => s.P?.SkillPrompt?.SkillId == Watch);
            var frame = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Skill);
            var receipt = frame.SourceKeshou!;
            Need(receipt.Materials.Count == 2 && receipt.Materials.All(m => m.IsRed) &&
                receipt.Materials.Select(m => m.From).OrderBy(l => l.Zone).SequenceEqual(new[] { CardLocation.Hand(0), CardLocation.Equipment(0) }.OrderBy(l => l.Zone)) &&
                s.G.ResolutionStack.OfType<BeforeDamageProgramWindowFrame>().Single(w => w.Id == receipt.WindowId).Amount == 2 &&
                s.Events<ProgramDamageReducedEvent>().Length == 0 && s.Events<DamageAppliedEvent>().Length == 0,
                "Both actual materials are paid atomically while their original movement child pauses the same damage frame before reduction; mixed zones do not create another damage or payment.");
            s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); s.Play();
            var paid = s.Events<SourceKeshouPaidBranchEvent>().Single();
            var reduction = s.Events<ProgramDamageReducedEvent>().Single();
            Need(paid.ProgramFrameId == frame.Id && paid.Receipt.WindowId == receipt.WindowId && paid.Receipt.AttackId == receipt.AttackId &&
                paid.Receipt.Source is { OwnerSeat: 0, SkillId: Skill } && !string.IsNullOrWhiteSpace(paid.Receipt.Source.SkillInstanceId) &&
                paid.Receipt.SoleFactionAfterCost == !sharedFaction && reduction.FrameId == frame.Id &&
                reduction.AmountBefore == 2 && reduction.AmountAfter == 1 && s.V(0).Hp == 5 &&
                s.Events<DamageAppliedEvent>().Single().Amount == 1 &&
                s.G.CardMovements.Count(m => m.CardId == hand && m.From == CardLocation.Hand(0) && m.To == CardLocation.DiscardPile && m.Reason.Value == CostReason) == 1 &&
                s.G.CardMovements.Count(m => m.CardId == equipment && m.From == CardLocation.Equipment(0) && m.To == CardLocation.DiscardPile && m.Reason.Value == CostReason) == 1,
                "The paid pair returns once to ordinary native reduction and the original one-point remaining damage, retaining its exact source/window/attack identity.");
            var judgments = s.Events<JudgmentResolvedEvent>().Where(e => e.Reason == "shanhe.source-keshou").ToArray();
            Need(judgments.Length == (sharedFaction ? 0 : 1) && s.V(0).HandCount == (sharedFaction ? 1 : 2) &&
                s.G.CardMovements.Count(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(0)) == (sharedFaction ? 0 : 1) &&
                s.G.CardMovements.Count(m => m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile && m.CardId != hand && m.CardId != equipment) == (sharedFaction ? 0 : 1),
                "A sole effective faction performs its real red judgment, draws exactly one original deck entity and cleans up the judged card; a same-faction enemy suppresses this branch despite being on the opposing team.");
        }
        var noPair = Scene.Start(noPair: true); noPair.Use("self-two"); noPair.Play();
        Need(noPair.V(0).Hp == 4 && noPair.Events<SourceKeshouPaidBranchEvent>().Length == 0 &&
            noPair.Events<ProgramDamageReducedEvent>().Length == 0 && noPair.Events<JudgmentResolvedEvent>().Length == 0,
            "Two differently coloured HE cards do not enable payment, reduction or a free sole-faction judgment.");
    }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public PendingDecision? P => G.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] Events<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public void Submit(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected source993 command."); }
        public void Use(string id) => Submit(new UseProgramSkillCommand(0, Driver, id, [], [], G.Revision, P!.PromptId));
        public void Answer(Func<PromptChoice, bool> condition)
        { var p = P!; var c = p.Choices.Single(condition); Submit(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, c.Id, G.Revision)); }
        public void Play() => Until(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && G.ResolutionStack.Count == 0);
        public void Until(Func<bool> ready)
        {
            for (var i = 0; i < 240; i++)
            {
                if (ready()) return;
                Need(G.State.Winner == Winner.None, "Source993 fixture remains a bounded live rule flow.");
                if (P is { } p && !(p.Kind == DecisionKind.PlayCard && G.ResolutionStack.Count != 0))
                    throw new InvalidOperationException("Unexpected source993 prompt: " + JsonSerializer.Serialize(new { p.Kind, p.PlayerSeat, p.SkillPrompt?.SkillId }));
                Submit(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Source993 did not reach its bounded native continuation.");
        }
        public static Scene Start(bool sharedFaction = false, bool noPair = false)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheSourceKeshouPackage(), new Fixture(sharedFaction));
            var cards = noPair ? new[] { new ShanheStartingCard(CardKind.Dodge, Suit.Spade, 2) } :
                new[] { new ShanheStartingCard(CardKind.Dodge, Suit.Heart, 2), new ShanheStartingCard(CardKind.Peach, Suit.Spade, 4) };
            var setup = new ShanheBattleSetup(47, 1, "source-keshou-native", Owner, 6, 0, 0, 0, 0, [], cards,
                [new(CardKind.Crossbow, Suit.Diamond, 3)], [new(Peer)], [], new string('c', 64));
            var s = new Scene(GameEngine.CreateStandard(new()
            { Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 3 }, registry));
            s.Submit(new StartGameCommand()); s.Play(); return s;
        }
    }
    private sealed class Fixture(bool sharedFaction) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:source-keshou", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            var programs = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = new object[]
            {
                new { id = Driver, revision = 1, activations = new object[]
                {
                    new { id = "self-two", minCards = 0, maxCards = 0, minTargets = 0, maxTargets = 0, targetKind = "anyLiving", usesPerTurn = (int?)null,
                        effects = new[] { new { op = "damage", target = "owner", amount = 2 } } }
                } },
                new { id = Watch, revision = 1, triggers = new[] { new { id = "paid-original-he", window = "cardsMoved", subject = "owner",
                    sourceZones = new[] { "equipment" }, movementOccurrence = "perBatch", movementReasons = new[] { CostReason }, optional = false,
                    effects = new[] { new { op = "chooseOption", target = "owner", resultBind = "return", options = new[] { new { id = "continue" } } } } } } }
            } }), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = new Dictionary<string, object>
            {
                [Driver] = new { name = "真实伤害驱动", description = "保留原始受伤父帧。" },
                [Watch] = new { name = "真实已付装备孩子", description = "两牌成本的实际失装移牌子流程。",
                    optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } }
            } }));
            foreach (var (id, p) in programs.Programs) b.AddSkill(new(id, programs.Presentations[id].Name, programs.Presentations[id].Description)
            { Program = p, ProgramPresentation = programs.Presentations[id] });
            b.AddGeneral(new(Owner, "真实来源技能持有者", "supporter", Skill, "qun", 8, [Driver, Watch]));
            b.AddGeneral(new(Peer, "实际势力同行", "supporter", "standard:none", sharedFaction ? "qun" : "wei", 6));
            b.AddDeck(new(Deck, "红色原生判定牌堆", 0, 0, [])
            { PhysicalCards = Enumerable.Range(0, 32).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, 5)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "来源技能原生流程", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
    private static void Need(bool yes, string message) { if (!yes) throw new InvalidOperationException(message); }
}
