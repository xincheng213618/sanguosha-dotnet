using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class BoundaryCaoRenMobileChecks
{
    private const string Hero = "boundary:cao-ren-mobile", Skill = "boundary:jiewei-mobile", Jushou = "boundary:jushou-mobile",
        Driver = "fixture:mobile-cao-ren-driver", Paid = "fixture:mobile-cao-ren-paid", Plain = "fixture:mobile-cao-ren-peer";
    private const string MoveReason = "skill-program.boundary:jiewei-mobile.SelectAndMoveOwnedCard";

    public static void HandOnlyCostMovesNativeJudgmentAndResumesPaidChildCold()
    {
        var s = Scene.Start(); s.Play();
        var judgment = s.V().Hand.Single(c => c.Kind == CardKind.Indulgence).Id;
        var armor = s.V().Hand.Single(c => c.Kind == CardKind.BaguaFormation).Id;
        var cost = s.V().Hand.Single(c => c.Kind == CardKind.Dodge).Id;
        s.Native(judgment, 2); s.Play(); s.Native(armor); s.Play();
        s.Need(s.Zone(judgment) == CardLocation.Judgment(2) && s.Zone(armor) == CardLocation.Equipment(0),
            "The original delayed trick and armor enter real public areas through native card uses.");
        s.Use("down"); s.Play();
        s.Need(s.V().IsFaceDown && !s.E<ProgramBindingStartedEvent>().Any(e => e.SkillId == Skill),
            "Turning face down does not offer the mobile face-up relocation.");
        s.End(); s.Reach(() => s.P?.SkillPrompt?.SkillId == Jushou && s.P.Choices.Any(c => Action(c) == "activate"));
        s.Answer(c => Action(c) == "activate");
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Skill && s.P.Choices.Any(c => Action(c) == "activate"));
        var jushouId = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Jushou).Id;
        s.Need(!s.V().IsFaceDown && !s.G.CardMovements.Any(m => m.Reason.Value == $"skill-program.{Jushou}.Draw"),
            "Actual Jushou flips a previously face-down general up and waits for Jiewei before its four-card draw.");
        s.Answer(c => Action(c) == "activate");
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Skill && s.P.Choices.Any(c => Action(c) == "select-and-move-owned-card"));
        var frameId = s.Root!.Id;
        s.Need(!s.V().IsFaceDown && s.P!.Choices.All(c => Action(c) != "select-and-move-owned-card" ||
                c.Parameters.GetValueOrDefault("source-zone") == nameof(CardZoneKind.Hand)) &&
            s.P.Choices.Any(c => c.Cards.SequenceEqual([cost])) && s.P.Choices.All(c => !c.Cards.Contains(armor)),
            "The mobile cost is exactly a discardable hand card; equipped armor cannot pay it.");
        s.Cold(); s.Answer(c => c.Cards.SequenceEqual([cost])); s.Reach(() => s.P?.SkillPrompt?.SkillId == Paid);
        s.Need(s.Root is { PendingMovementContinuation: not null } && s.Root.Id == frameId &&
            s.Zone(cost) == CardLocation.DiscardPile && s.Zone(armor) == CardLocation.Equipment(0) && s.Costs(cost) == 1,
            "The original Jiewei frame owns its already-paid hand discard while the actual movement child is suspended.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Skill && s.P.Choices.Any(c => Action(c) == "select-targets"));
        s.Need(s.Root?.Id == frameId && s.Costs(cost) == 1 && s.P!.Choices.Any(c => c.Targets.SequenceEqual([2, 0])),
            "Returning from the paid child preserves the root and permits Cao Ren himself as the destination.");
        s.Cold(); s.Answer(c => c.Targets.SequenceEqual([2, 0]));
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Skill && s.P.Choices.Any(c => c.Cards.SequenceEqual([judgment])));
        s.Need(s.P!.Choices.Where(c => c.Cards.Contains(judgment)).All(c =>
                c.Parameters.GetValueOrDefault("source-zone") == nameof(CardZoneKind.Judgment)),
            "The genuine delayed trick is selectable from Judgment, beyond the old equipment-only edition.");
        s.Cold(); s.Answer(c => c.Cards.SequenceEqual([judgment]));
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Jushou && s.P.Choices.Any(c => Action(c) == "strategic-choice"));
        s.Need(s.Zone(judgment) == CardLocation.Judgment(0) && s.Zone(armor) == CardLocation.Equipment(0) && s.Costs(cost) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == judgment && m.From == CardLocation.Judgment(2) &&
                m.To == CardLocation.Judgment(0) && m.Reason.Value == MoveReason) == 1 &&
            s.E<ProgramBindingResolvedEvent>().Count(e => e.FrameId == frameId) == 1 &&
            s.G.ResolutionStack.All(f => f.Id != frameId),
            "The original Judgment entity moves once to Cao Ren; cold returns finish the paid skill without repeating its cost or field move.");
        var equipment = s.V().Hand.First(c => c.Kind == CardKind.BaguaFormation).Id;
        s.Need(s.G.CardMovements.Count(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(0) &&
                m.Reason.Value == $"skill-program.{Jushou}.Draw") == 4 && s.P!.Choices.Any(c => c.Cards.SequenceEqual([equipment])),
            "The same original Jushou resumes after Jiewei, draws exactly four and selects a real equipment hand card.");
        s.Cold(); s.Answer(c => c.Cards.SequenceEqual([equipment]));
        s.Reach(() => s.E<ProgramBindingResolvedEvent>().Any(e => e.FrameId == jushouId));
        s.Need(s.Zone(equipment) == CardLocation.Equipment(0) && s.Zone(armor) == CardLocation.DiscardPile &&
            s.G.CardMovements.Count(m => m.CardId == equipment && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing &&
                m.Reason == CardMoveReasons.EquipmentUse) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == equipment && m.From == CardLocation.Processing && m.To == CardLocation.Equipment(0)) == 1 &&
            s.E<EquipmentChangedEvent>().Count(e => e.CardId == equipment && e.PlayerSeat == 0) == 1 && s.Costs(cost) == 1 &&
            s.E<ProgramBindingResolvedEvent>().Count(e => e.FrameId == jushouId) == 1,
            "Jushou uses the selected equipment through native Processing and replacement exactly once after cold, then completes its original frame.");
        s.Cold();
    }

    private static string? Action(PromptChoice choice) => choice.Parameters.GetValueOrDefault("program-action");

    private sealed class Scene(ContentRegistry registry, GameEngine game)
    {
        public GameEngine G = game;
        public PlayerSnapshot V() => G.CreateSnapshot(0).Players[0];
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public ProgramSkillFrame? Root => G.ResolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.SkillId == Skill);
        public IEnumerable<T> E<T>() => G.Events.Select(e => e.Payload).OfType<T>();
        public CardLocation Zone(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        public int Costs(int id) => G.CardMovements.Count(m => m.CardId == id && m.From == CardLocation.Hand(0) &&
            m.To == CardLocation.DiscardPile && m.Reason.Value == MoveReason);
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void End() => Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
        public void Use(string id) => Accept(new UseProgramSkillCommand(0, Driver, id, [], [], G.Revision, P!.PromptId));
        public void Native(int id, int? target = null)
        {
            var action = G.GetHumanLegalActions().FirstOrDefault(a => a.CardId == id &&
                (target is null || a.TargetSeats.SequenceEqual([target.Value])));
            Need(action is not null, "The fixed original card has a native legal use.");
            Accept(new PlayCardCommand(0, id, action!.TargetSeats, G.Revision, P!.PromptId, action.PlayedCardKind, action.TargetCardId)
                { ConversionSource = action.ConversionSource, AdditionalConversionSources = action.AdditionalConversionSources });
        }
        public void Answer(Func<PromptChoice, bool> match)
        { var p = P!; var c = p.Choices.FirstOrDefault(match); Need(p.PlayerSeat == 0 && c is not null, "The exact native choice belongs to Cao Ren."); Accept(new AnswerPromptCommand(0, p.PromptId, c!.Id, G.Revision)); }
        public void Reach(Func<bool> ready)
        {
            for (var i = 0; i < 160; i++)
            {
                if (ready()) return;
                Need(P is not { PlayerSeat: 0 }, "Unexpected native mobile Cao Ren boundary.");
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("The bounded native Cao Ren scene did not reach its boundary.");
        }
        public void Cold()
        {
            var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(before == Dump(), "Cold preserves the original face-up, paid Hand cost, chosen field pair and Judgment entity.");
        }
        private string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),
            Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements });
        public void Need(bool ok, string why)
        { if (!ok) throw new InvalidOperationException(why + " " + JsonSerializer.Serialize(new { P, Root = Root?.Id, G.State.Phase, G.State.CurrentSeat })); }
        public void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native mobile Cao Ren command."); }
        public static Scene Start()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            ShanheStartingCard[] cards = [new(CardKind.Indulgence, Suit.Club, 5), new(CardKind.BaguaFormation, Suit.Club, 2),
                new(CardKind.Dodge, Suit.Heart, 7), new(CardKind.Slash, Suit.Spade, 8)];
            var setup = new ShanheBattleSetup(41, 1, "mobile-cao-ren-native-fields", Hero, 4, 0, 0, 0, 0, [Driver, Paid], cards, [],
                [new(Plain, Hp: 8), new(Plain, Hp: 8)], [new(Plain, Hp: 8)], new string('c', 64));
            var engine = GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0,
                HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
                ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 6 }, registry);
            var s = new Scene(registry, engine); s.Accept(new StartGameCommand()); return s;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-cao-ren-mobile", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero);
            object Activation(string id, bool faceDown) => new { id, minCards = 0, maxCards = 0, minTargets = 0, maxTargets = 0,
                targetKind = "anyLiving", usesPerTurn = (int?)null, effects = new[] { new { op = "setFaceState", target = "owner", faceDown } } };
            var skills = new object[] {
                new { id = Driver, revision = 1, activations = new[] { Activation("down", true), Activation("up", false) } },
                new { id = Paid, revision = 1, triggers = new[] { new { id = "native-hand-discard", window = "cardsMoved", subject = "owner",
                    sourceZones = new[] { "hand" }, movementOccurrence = "perBatch", movementReasons = new[] { MoveReason }, optional = false,
                    effects = new[] { new { op = "chooseOption", target = "owner", resultBind = "paid", options = new[] { new { id = "continue" } } } } } } }
            };
            var labels = new Dictionary<string, object> {
                [Driver] = new { name = "原生翻面驱动", description = "真实翻背及翻正" },
                [Paid] = new { name = "解围真实已付子窗", description = "真实手牌成本移动后暂停", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } }
            };
            var catalog = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in catalog.Programs) b.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                { Program = program, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new(Plain, "固定同桌", "supporter", "standard:none", "wei", 8));
            const string deck = "fixture:mobile-cao-ren-deck";
            b.AddDeck(new(deck, "固定据守装备", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 40).Select(_ => new ContentDeckPhysicalCard("standard:bagua", Suit.Club, 2)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "移动界曹仁原生判定移动", 4, 4, new Dictionary<string, int>(), deck,
                GeneralPoolIds: [Hero, Plain], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
