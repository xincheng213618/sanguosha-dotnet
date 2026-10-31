using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class QuHuangRuntimeChecks
{
    private const string Owner = "fixture:qu-huang-owner", Driver = "fixture:qu-huang-driver",
        Watch = "fixture:qu-huang-paid-child", Mode = "identity:qu-huang-runtime",
        Qiejian = "ol:qiejian", Nishou = "ol:nishou",
        DrawReason = "skill-program.qu-huang.qiejian.draw", HoldReason = "skill-program.qu-huang.nishou.hold";

    public static void QiejianPaidDrawFieldDiscardAndRoundBlockCold()
    {
        var s = Scene.Start(qiejian: true); s.Play(); var gear = s.Equip(); s.Use("empty-hand");
        s.Reach(() => s.Offer(Qiejian)); s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch);
        var paid = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.QuHuang is not null);
        var frameId = paid.Id;
        Need(paid.QuHuang is { Stage: QuHuangStage.DrawOwner, Paid: true, ActualCount: 1 } &&
            paid.PendingMovementContinuation is not null && s.E<QuHuangMovementPaidEvent>().Count(e => e.FrameId == frameId) == 1,
            "The original last-Hand loss has paid its first real draw before the draw's private child pauses it.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        s.Reach(() => s.Branch("discard")); s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "discard" && c.Cards.SequenceEqual([gear])); s.Play();
        Need(s.E<QuHuangCompletedEvent>().Count(e => e.FrameId == frameId) == 1 &&
            s.G.CardMovements.Count(m => m.Reason.Value == DrawReason && m.To == CardLocation.Hand(0)) == 2 &&
            s.G.CardMovements.Count(m => m.CardId == gear && m.From == CardLocation.Equipment(0) && m.To == CardLocation.DiscardPile &&
                m.Reason.Value == "skill-program.qu-huang.qiejian.discard") == 1,
            "Cold return pays the second draw and actual field discard once, without drawing again for the resumed paid child.");

        s.Use("empty-hand"); s.Reach(() => s.Offer(Qiejian));
        s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        s.Reach(() => s.Branch("block")); s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "block"); s.Play();
        var draws = s.G.CardMovements.Count(m => m.Reason.Value == DrawReason);
        Need(s.E<QiejianRoundBlockedEvent>().Single() is { OwnerSeat: 0, TargetSeat: 0 }, "The private block branch records the actual original loser for this round.");
        s.Use("empty-hand"); s.Play();
        Need(s.G.CardMovements.Count(m => m.Reason.Value == DrawReason) == draws && s.E<QuHuangStartedEvent>().Count(e => e.Op == SkillProgramEffectOp.ResolveQiejianLastHandLoss) == 2,
            "The same-round blocked original loser cannot reopen Qiejian after another genuine last-Hand loss.");
    }

    public static void NishouIssuedPhaseSwapAndNativeLightningReturnCold()
    {
        var s = Scene.Start(qiejian: false); s.Play(); s.Equip(); s.Use("lose-gear");
        s.Reach(() => s.Branch("defer")); s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "defer"); s.Play();
        var debt = s.E<NishouSwapDebtIssuedEvent>().Single();
        var own = s.V(0).Hand.Select(c => c.Id).ToArray(); var other = s.V(1).Hand.Select(c => c.Id).ToArray();
        s.Accept(new EndPlayPhaseCommand(0, s.G.Revision, s.P!.PromptId)); s.Reach(() => s.Branch("swap"));
        Need(s.G.ResolutionStack.OfType<ActualPhaseEndFrame>().Single().Phase == debt.Phase &&
            s.P!.Choices.All(c => c.Targets is [var seat] && seat != 0 && s.V(seat).HandCount == other.Length),
            "The equipment loss issues its exact current Play-end debt and offers only the genuine minimum-Hand peers.");
        s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "swap" && c.Targets.SequenceEqual([1]));
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch);
        var held = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.QuHuang?.Op == SkillProgramEffectOp.ResolveNishouIssuedCurrentPhaseSwap);
        Need(held.QuHuang is { Stage: QuHuangStage.HoldOwner, Paid: true } receipt &&
            receipt.OwnerCards.Select(c => c.CardId).SequenceEqual(own) && receipt.OtherCards.Select(c => c.CardId).SequenceEqual(other) &&
            own.All(id => s.G.CreateCardZoneDiagnostics().Any(c => c.CardId == id && c.Location == CardLocation.Processing)),
            "The paid exchange freezes both exact original Hands on its owning frame before the first hold child.");
        var swapId = held.Id; s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        s.Reach(() => s.E<QuHuangCompletedEvent>().Any(e => e.FrameId == swapId));
        Need(s.V(1).Hand.Select(c => c.Id).Order().SequenceEqual(own.Order()) &&
            other.All(id => s.V(0).Hand.Any(c => c.Id == id)) && s.V(0).HandCount == other.Length + 1 &&
            s.E<NishouSwapDebtConsumedEvent>().Count(e => e.DebtId == debt.DebtId) == 1 &&
            s.E<QuHuangCompletedEvent>().Count(e => e.FrameId == swapId) == 1,
            "Cold return exchanges each frozen original once; the draw in the paid hold child remains with its actual recipient and never joins the old Hand set.");

        s = Scene.Start(qiejian: false); s.Play(); var gear = s.Equip(); s.Use("lose-gear");
        s.Reach(() => s.Branch("lightning")); s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "lightning"); s.Play();
        var payment = s.E<NishouLightningPaidEvent>().Single();
        Need(payment.Origin.CardId == gear && payment.Origin.PrintedKind == CardKind.Crossbow &&
            s.E<NishouLightningReturnedEvent>().Count(e => e.Origin == payment.Origin) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == gear && m.From == CardLocation.DiscardPile && m.To == CardLocation.Processing && m.Reason == CardMoveReasons.Use) == 1 &&
            s.G.CreateCardZoneDiagnostics().Any(c => c.CardId == gear && c.Location == CardLocation.Judgment(0)) &&
            !s.E<NishouSwapDebtIssuedEvent>().Any(),
            "The alternate branch pays the original discarded physical equipment once, resolves native Lightning and returns to its exact program without issuing a swap debt.");
    }

    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }

    private sealed class Scene(ContentRegistry registry, GameEngine game)
    {
        public GameEngine G = game;
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players[seat];
        public PendingDecision? P => Enumerable.Range(0, 4).Select(seat => G.CreateSnapshot(seat).PendingDecision).FirstOrDefault(p => p is not null);
        public IEnumerable<T> E<T>() => G.Events.Select(e => e.Payload).OfType<T>();
        public bool Offer(string skill) => P?.SkillPrompt?.SkillId == skill && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        public bool Branch(string branch) => P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "qu-huang" && c.Parameters.GetValueOrDefault("branch") == branch) == true;
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Use(string id) => Accept(new UseProgramSkillCommand(0, Driver, id, [], [], G.Revision, P!.PromptId));
        public int Equip()
        {
            var action = G.GetLegalActions(0).First(a => a.Kind == LegalActionKind.Equip &&
                V(0).Hand.Any(card => card.Id == a.CardId && card.Kind == CardKind.Crossbow));
            Accept(new PlayCardCommand(0, action.CardId!.Value, action.TargetSeats, G.Revision, P!.PromptId, action.PlayedCardKind, action.TargetCardId));
            Play(); return action.CardId.Value;
        }
        public void Answer(Func<PromptChoice, bool> predicate)
        { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(predicate).Id, G.Revision)); }
        public void Accept(GameCommand command)
        { var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, result.Error?.Message ?? "Rejected Qu Huang command."); }
        public void Reach(Func<bool> done)
        {
            for (var step = 0; step < 160 && !done(); step++)
            {
                if (P is { Kind: DecisionKind.SelectGeneral } select)
                { Accept(new SelectGeneralCommand(select.PlayerSeat, select.PlayerSeat == 0 ? Owner : "fixture:qu-huang-peer-" + select.PlayerSeat, G.Revision, select.PromptId)); continue; }
                if (P is { Kind: DecisionKind.PlayCard } play)
                { Accept(new EndPlayPhaseCommand(play.PlayerSeat, G.Revision, play.PromptId)); continue; }
                if (P is { } unexpected) throw new InvalidOperationException($"Unexpected Qu Huang prompt {unexpected.Kind}/{unexpected.SkillPrompt?.SkillId}.");
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(done(), "The bounded fixed Qu Huang scene missed its expected boundary.");
        }
        public void Cold()
        {
            var before = State(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(before == State(), "Cold command replay preserves original paid Qu Huang receipts, frozen Hands, private child and native physical zones.");
        }
        private string State() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))),
            Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())), G.CardMovements, Zones = G.CreateCardZoneDiagnostics() });
        public static Scene Start(bool qiejian)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(qiejian));
            var scene = new Scene(registry, GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = 4, HumanSeat = 0,
                HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, ModeId = Mode, UseInteractiveSetup = true,
                UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 3 }, registry));
            scene.Accept(new StartGameCommand()); return scene;
        }
    }

    private sealed class Fixture(bool qiejian) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-qu-huang-runtime", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "ol:qu-huang");
            var skills = new object[] {
                new { id = Driver, revision = 1, modifiers = new[] { new { id = "keep", query = "handLimit", operation = "add", value = 50, priority = 0 } },
                    activations = new[] { new { id = "empty-hand", minCards = 0, maxCards = 0, minTargets = 0, maxTargets = 0,
                        targetKind = "anyLiving", usesPerTurn = (int?)null, effects = new[] { new { op = "discardOwnedZoneCards", target = "owner", zones = new[] { "hand" } } } },
                        new { id = "lose-gear", minCards = 0, maxCards = 0, minTargets = 0, maxTargets = 0,
                        targetKind = "anyLiving", usesPerTurn = (int?)null, effects = new[] { new { op = "discardOwnedZoneCards", target = "owner", zones = new[] { "equipment" } } } } } },
                new { id = Watch, revision = 1, triggers = new[] { new { id = "original-paid-child", window = qiejian ? "cardsGained" : "cardsMoved", subject = "owner",
                    sourceZones = qiejian ? null : new[] { "hand" }, destinationZones = qiejian ? new[] { "hand" } : null, movementOccurrence = "perBatch", movementReasons = new[] { qiejian ? DrawReason : HoldReason },
                    optional = false, usageScope = "game", usageLimit = 1, effects = qiejian ? new object[] {
                        new { op = "chooseOption", target = "owner", resultBind = "continue", options = new[] { new { id = "continue" } } } } : new object[] {
                        new { op = "chooseOption", target = "owner", resultBind = "continue", options = new[] { new { id = "continue" } } },
                        new { op = "draw", target = "owner", amount = 1 } } } } }
            };
            var labels = new Dictionary<string, object> { [Driver] = new { name = "屈晃原生驱动", description = "真实原区弃置" },
                [Watch] = new { name = "屈晃已支付子窗", description = "原生移动子窗", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } };
            var rules = System.Text.Json.Nodes.JsonNode.Parse(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }))!;
            rules["skills"]![1]!["triggers"]![0]!.AsObject().Remove(qiejian ? "sourceZones" : "destinationZones");
            var catalog = SkillProgramCatalog.Load(rules.ToJsonString(),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in catalog.Programs) builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Owner, "屈晃真实结算夹具", "supporter", qiejian ? Qiejian : Nishou, "wu", 6, AdditionalSkillIds: [Driver, Watch]));
            var peers = Enumerable.Range(1, 3).Select(seat => "fixture:qu-huang-peer-" + seat).ToArray();
            foreach (var peer in peers) builder.AddGeneral(new(peer, "实际其他角色", "supporter", "standard:none", "wei", 6));
            const string deck = "fixture:qu-huang-deck";
            builder.AddDeck(new(deck, "固定原生装备实体", 4, 2, []) { PhysicalCards = Enumerable.Range(0, 80)
                .Select(_ => new ContentDeckPhysicalCard("standard:crossbow", Suit.Spade, 7)).ToArray() });
            builder.AddMode(new(Mode, "屈晃四人原生结算", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 3 },
                deck, GeneralCandidateCount: 4, GeneralPoolIds: [Owner, .. peers]));
        }
    }
}
