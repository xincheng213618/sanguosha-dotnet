using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileDengAiChecks
{
    public static void FrozenJudgmentBranchesAwakenAndNativeFieldUseReturnCold()
    {
        var s = MobileDengAiJiangWeiScene.Start(dengAi: true); s.Play(); var maximum = s.V(0).MaxHp;
        s.Use(0, "discard-one", [s.V(0).Hand[0].Id]); s.Play();
        s.Need(!s.E<ProgramBindingStartedEvent>().Any(e => e.SkillId == "boundary:tuntian-mobile"), "Own-turn actual hand discard does not activate mobile Tuntian.");
        s.End(0); s.Play(1); s.Use(1, "draw-twelve", [], [1]); s.Play(1);
        var heart = s.V(1).Hand.First(c => c.Suit == Suit.Heart).Id;
        s.Top(heart); s.DiscardOther(); s.Tuntian(); s.Activate();
        s.Reach(() => s.P?.SkillPrompt?.SkillId == MobileDengAiJiangWeiScene.Watch);
        var root = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == "boundary:tuntian-mobile");
        s.Need(root.JudgmentOutcomeBindings is [{ Suit: Suit.Heart, CardId: var result }] && result == heart &&
            s.V(0).Hand.Any(c => c.Id == heart) && s.V(0).AuthorityCount == 0 && root.PendingMovementContinuation is not null,
            "The exact frozen Heart outcome issues a native gain of its original entity before the real gain child.");
        s.Cold(); s.Continue(); s.Play(1);
        s.Need(s.G.CardMovements.Count(m => m.CardId == heart && m.From == CardLocation.Processing && m.To == CardLocation.Hand(0) &&
            m.Reason.Value == "skill-program.boundary:tuntian-mobile.MoveBoundCards") == 1, "Cold native gain return claims the original Heart judgment once.");
        var fields = new List<int>();
        for (var i = 0; i < 3; i++)
        {
            var card = s.V(1).Hand.First(c => c.Suit == Suit.Spade).Id; fields.Add(card);
            s.Top(card); s.DiscardOther(); s.Tuntian(); s.Activate(); s.Play(1);
        }
        s.Need(s.V(0).AuthorityCount == 3 && fields.All(id => s.Zone(id) == CardLocation.Authority(0)) && s.G.GetCombatDistance(0, 2) == 1,
            "Three separate real off-turn losses each retain one non-Heart result in the native field zone and reduce outgoing distance.");
        s.End(1); s.Play();
        s.Need(s.V(0).MaxHp == maximum - 1 && s.V(0).Skills!.Any(k => k.ContentId == "boundary:deng-ai-jixi-mobile") &&
            s.E<ProgramBindingResolvedEvent>().Count(e => e.SkillId == "boundary:zaoxian-mobile" && e.Completed) == 1,
            "The next actual preparation awakens once from three real fields and grants mobile Jixi through the shared skill-change layer.");
        var action = s.G.GetLegalActions(0).First(a => a.PlayedCardKind == CardKind.Snatch && a.ConversionSource?.SkillId == "boundary:deng-ai-jixi-mobile" && a.TargetSeats.SequenceEqual([2]));
        var material = action.CardId!.Value; s.PlayAction(action); s.Play(settleNativeCard: true);
        s.Need(s.V(0).AuthorityCount == 2 && s.G.CardMovements.Count(m => m.CardId == material && m.From == CardLocation.Authority(0) && m.To == CardLocation.Processing) == 1 &&
            s.E<CardUseFinishedEvent>().Count(e => e.CardId == material && e.CardKind == CardKind.Snatch) == 1,
            "Jixi pays one original field and completes one genuine native Snatch with the remaining field distance.");
    }
}

internal static class MobileJiangWeiChecks
{
    public static void UnreachableTauntAndDodgedNativeSlashKeepPhaseQuotaCold()
    {
        var s = MobileDengAiJiangWeiScene.Start(dengAi: false); s.Play();
        var hand = s.V(2).HandCount;
        s.Need(s.G.GetCombatDistance(2, 0) == 2 && s.G.GetLegalActions(0).Any(a => a.ProgramSkillId == "boundary:tiaoxin-mobile" && a.SelectableTargetSeats.Contains(2)),
            "Mobile Tiaoxin can choose a living distant role whose ordinary Slash cannot reach its owner.");
        s.Taunt(2); s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card") == true);
        s.Answer(_ => true); s.Reach(() => s.P?.SkillPrompt?.SkillId == MobileDengAiJiangWeiScene.Watch);
        var root = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == "boundary:tiaoxin-mobile");
        s.Need(s.V(2).HandCount == hand - 1 && root.PendingMovementContinuation is not null && root.SelectedTargetSeats.SequenceEqual([2]) &&
            !s.E<CardUseDeclaredEvent>().Any(e => e.SourceSeat == 2 && e.CardKind == CardKind.Slash),
            "The unreachable native request pays the real one-card discard and pauses its original movement child without issuing an illegal Slash.");
        s.Cold(); s.Continue(); s.Play();
        s.Need(s.G.GetLegalActions(0).All(a => a.ProgramSkillId != "boundary:tiaoxin-mobile") && s.V(2).HandCount == hand - 1,
            "Cold discard return spends exactly one actual phase quota and never grants the OL second use.");
        s.End(0); s.Reach(() => s.G.State.CurrentSeat == 1 && s.P is { Kind: DecisionKind.PlayCard, PlayerSeat: 1 });
        s.End(1); s.Play();
        var hp = s.V(0).Hp; var sourceHand = s.V(1).HandCount;
        s.Taunt(1); s.Reach(() => s.P is { PlayerSeat: 1 } && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("request-option") == "use"));
        s.Answer(c => c.Parameters.GetValueOrDefault("request-option") == "use" && c.Cards.Count == 1);
        s.Reach(() => s.P is { Kind: DecisionKind.RespondDodge, PlayerSeat: 0 });
        s.Answer(c => c.Parameters.GetValueOrDefault("response") == "dodge" && c.Cards.Count == 1); s.Play();
        s.Need(s.V(0).Hp == hp && s.V(1).HandCount == sourceHand - 1 &&
            !s.G.CardMovements.Any(m => m.From == CardLocation.Hand(1) && m.Reason.Value == "skill-program.boundary:tiaoxin-mobile.SelectAndMoveOwnedCard") &&
            s.G.GetLegalActions(0).All(a => a.ProgramSkillId != "boundary:tiaoxin-mobile"),
            "A genuine requested Slash that is actually Dodged still prevents the mobile discard branch and consumes only one taunt in its new real phase.");
    }
}

internal sealed class MobileDengAiJiangWeiScene(GameEngine game, ContentRegistry registry, bool dengAi)
{
    internal const string Watch = "fixture:mobile-deng-ai-jiang-wei-watch";
    private const string Driver = "fixture:mobile-deng-ai-jiang-wei-driver", Owner = "fixture:mobile-deng-ai-jiang-wei-owner", Mode = "identity:classic-mobile-deng-ai-jiang-wei";
    public GameEngine G = game;
    public PendingDecision? P => Enumerable.Range(0, 5).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
    public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
    public IEnumerable<T> E<T>() => G.Events.Select(e => e.Payload).OfType<T>();
    public CardLocation Zone(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
    public void Need(bool condition, string text) { if (!condition) throw new InvalidOperationException(text + " " + JsonSerializer.Serialize(new { DengAi = dengAi, Pending = P is { } p ? new { p.Kind, p.PlayerSeat, Skill = p.SkillPrompt?.SkillId } : null })); }
    public void Accept(GameCommand command) { var r = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(r.Accepted, r.Error?.Message ?? "Rejected fixed native command."); }
    public void Answer(Func<PromptChoice, bool> test) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(test).Id, G.Revision)); }
    public void Continue() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
    public void Activate() => Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
    public void Tuntian() => Reach(() => P?.SkillPrompt?.SkillId == "boundary:tuntian-mobile" && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
    public void Use(int seat, string activation, int[] cards, int[]? targets = null) => Accept(new UseProgramSkillCommand(seat, Driver, activation, cards, targets ?? [], G.Revision, P!.PromptId));
    public void Taunt(int target) => Accept(new UseProgramSkillCommand(0, "boundary:tiaoxin-mobile", "taunt", [], [target], G.Revision, P!.PromptId));
    public void Top(int card) { Use(1, "top", [card]); Play(1); }
    public void DiscardOther() { Use(1, "discard-other", [], [0]); Reach(() => P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card") == true); Answer(_ => true); }
    public void End(int seat) => Accept(new EndPlayPhaseCommand(seat, G.Revision, P!.PromptId));
    public void Play(int seat = 0, bool settleNativeCard = false) => Reach(() => P is { Kind: DecisionKind.PlayCard } p && p.PlayerSeat == seat, settleNativeCard);
    public void PlayAction(LegalAction a) => Accept(new PlayCardCommand(0, a.CardId!.Value, a.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind, a.TargetCardId)
        { ConversionSource = a.ConversionSource, AdditionalConversionSources = a.AdditionalConversionSources });
    public void Reach(Func<bool> goal, bool settleNativeCard = false)
    {
        for (var step = 0; step < 320 && !goal(); step++)
        {
            if (P is { Kind: DecisionKind.SelectGeneral } p) { Accept(new SelectGeneralCommand(p.PlayerSeat, p.PlayerSeat == 0 ? Owner : "fixture:mobile-deng-ai-jiang-wei-peer-" + p.PlayerSeat, G.Revision, p.PromptId)); continue; }
            if (P is { Kind: DecisionKind.PlayCard, PlayerSeat: not 0 } other) { End(other.PlayerSeat); continue; }
            if (P is { } choice)
            {
                if (settleNativeCard && choice.SkillPrompt is null && choice.Choices.Count > 0) { Answer(_ => true); continue; }
                throw new InvalidOperationException("Unexpected fixed mobile native prompt: " + JsonSerializer.Serialize(new { choice.Kind, choice.PlayerSeat, choice.SkillPrompt, choice.Choices }));
            }
            Accept(new AdvanceOneStepCommand(G.Revision));
        }
        Need(goal(), "Missing bounded mobile original-frame boundary.");
    }
    public void Cold()
    {
        string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 5).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),
            Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements, Facts = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray() });
        var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
        Need(before == Dump(), "Actual costs, private choices, original frames, frozen judgment and native movement child survive checkpoint JSON.");
    }
    public static MobileDengAiJiangWeiScene Start(bool dengAi)
    {
        var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture(dengAi));
        var g = GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = 5, HumanSeat = 0, HumanSeats = [0, 1, 2, 3, 4], HumanRole = Role.Lord,
            ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 12 }, r);
        var s = new MobileDengAiJiangWeiScene(g, r, dengAi); s.Accept(new StartGameCommand()); return s;
    }
    private sealed class Fixture(bool dengAi) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-deng-ai-jiang-wei", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            if (dengAi) CharacterFixtureContent.Register(b, "boundary:deng-ai-mobile");
            else CharacterFixtureContent.Register(b, "boundary:jiang-wei-mobile", "boundary:zhuge-liang", "classic:zhuge-liang");
            object Activation(string id, int cards, int targets, params object[] effects) => new { id, minCards = cards, maxCards = cards, sourceZones = new[] { "hand" }, minTargets = targets, maxTargets = targets, targetKind = "anyLiving", usesPerTurn = (int?)null, effects };
            var watch = new Dictionary<string, object> { ["id"] = "actual-child", ["window"] = dengAi ? "cardsGained" : "cardsMoved", ["subject"] = "owner", ["optional"] = false,
                [dengAi ? "destinationZones" : "sourceZones"] = dengAi ? new[] { "hand" } : new[] { "hand", "equipment" },
                ["movementReasons"] = new[] { dengAi ? "skill-program.boundary:tuntian-mobile.MoveBoundCards" : "skill-program.boundary:tiaoxin-mobile.SelectAndMoveOwnedCard" },
                ["movementOccurrence"] = dengAi ? "perBatch" : "perOwnerBatch", ["effects"] = new[] { new { op = "chooseOption", target = "owner", resultBind = "seen", options = new[] { new { id = "continue" } } } } };
            object[] skills = [new { id = Driver, revision = 1,
                viewAs = new[] { new { id = "physical-slash-as-dodge", inputKinds = new[] { "slash" }, inputSuits = Array.Empty<string>(), outputKind = "dodge", forPlay = false, forResponse = true } },
                activations = new[] {
                    Activation("draw-twelve", 0, 1, new { op = "draw", target = "selectedTarget", amount = 12 }),
                    Activation("discard-one", 1, 0, new { op = "discardSelected", target = "owner", amount = 1 }),
                    Activation("top", 1, 0, new { op = "captureSelectedCards", target = "owner", resultBind = "top" }, new { op = "moveBoundCards", target = "owner", sourceBind = "top", destination = "drawPileTop", awaitMovementTriggers = true }),
                    Activation("discard-other", 0, 1, new { op = "selectAndMoveOwnedCard", target = "owner", chooserRef = new { kind = "owner" }, cardOwnerRef = new { kind = "selectedTarget" }, zones = new[] { "hand", "equipment" }, count = 1, destination = "discardPile", awaitMovementTriggers = true }) } },
                new { id = Watch, revision = 1, triggers = new[] { watch } }];
            var labels = new Dictionary<string, object> { [Driver] = new { name = "真实固定命令", description = "只用原生摸牌、置顶、弃牌及响应。" },
                [Watch] = new { name = "原移动付款孩子", description = "在原实体已移动后暂停。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } };
            var c = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description) { Program = program, ProgramPresentation = c.Presentations[id] });
            b.AddGeneral(new(Owner, "移动界原生固定角色", "supporter", dengAi ? "boundary:tuntian-mobile" : "boundary:tiaoxin-mobile", dengAi ? "wei" : "shu", 4,
                [dengAi ? "boundary:zaoxian-mobile" : "boundary:zhiji-mobile", Driver, Watch]));
            var peers = Enumerable.Range(1, 4).Select(i => "fixture:mobile-deng-ai-jiang-wei-peer-" + i).ToArray();
            foreach (var peer in peers) b.AddGeneral(new(peer, "真实固定角色", "supporter", "standard:none", "wu", 6, [Driver, Watch]));
            const string deck = "fixture:mobile-deng-ai-jiang-wei-deck";
            b.AddDeck(new(deck, "固定普通实体", 4, 2, []) { PhysicalCards = Enumerable.Range(0, 160).Select(i => new ContentDeckPhysicalCard("standard:slash", dengAi && i % 3 == 0 ? Suit.Heart : Suit.Spade, 7)).ToArray() });
            b.AddMode(new(Mode, "移动界原生固定场景", 5, 5, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2, [nameof(Role.Renegade)] = 1 },
                deck, GeneralCandidateCount: 5, GeneralPoolIds: [Owner, .. peers]));
        }
    }
}
