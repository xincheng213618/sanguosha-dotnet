using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class MobileYuanShaoChecks
{
    internal const string Hero = "mobile:yuan-shao", Skill = "mobile:luanji", Binding = "same-suit-pair-as-arrow-barrage";

    public static void EffectiveSuitPairUsesOriginalArrowAndReturnsCold()
    {
        var s = Scene.Start(); s.Play();
        var pair = s.V(0).Hand.Where(c => c.Rank is 2 or 3).Select(c => c.Id).ToArray();
        var weapon = s.V(0).Equipment.Single().Id;
        var action = s.G.GetHumanLegalActions().Single(a => a.ProgramSkillId == Skill);
        s.Need(s.V(0).GeneralId == Hero && s.V(0).Skills!.Any(k => k.Id == "classic:hongyan") &&
            pair.Length == 2 && pair.Select(id => s.V(0).Hand.Single(c => c.Id == id).Suit).Distinct().Count() == 2 &&
            action.SelectableCardIds.Order().SequenceEqual(pair.Order()) && !action.SelectableCardIds.Contains(weapon) &&
            action.SelectableCardSuitOverrides is { Count: 2 } suits && pair.All(id => suits[id] == Suit.Heart) &&
            !s.G.GetHumanLegalActions().Any(a => a.ProgramSkillId == "classic:luanji") &&
            new SimpleAiBrain(0, 17).ChooseActiveSkillCards(s.G.CreateSnapshot(0), action).Order().SequenceEqual(pair.Order()),
            "The real mobile Yuan Shao receives native Hongyan: only his mixed-printed Spade/Heart Hand pair shares effective Heart, the equipped Spade stays excluded, classic's default pair remains unavailable, and AI selects the actual effective-suit cost.");
        var map = action.SelectableCardSuitOverrides!;
        var writeRejected = false;
        try { ((IDictionary<int, Suit>)map)[pair[0]] = Suit.Club; }
        catch (NotSupportedException) { writeRejected = true; }
        var classicShape = new LegalAction(LegalActionKind.UseProgramSkill, null, null, "classic same-suit default",
            MinCardCount: 2, MaxCardCount: 2)
        { ProgramSkillId = "classic:luanji", SelectedCardsSameSuit = true };
        s.Need(writeRejected && map[pair[0]] == Suit.Heart && classicShape.SelectableCardSuitOverrides is null &&
            !JsonSerializer.Serialize(classicShape).Contains(nameof(LegalAction.SelectableCardSuitOverrides), StringComparison.Ordinal),
            "The issued effective-suit map refuses observer writes; the default classic action keeps a null omitted field and its previous serialized shape.");
        var revision = s.G.Revision;
        var rejected = s.G.Submit(new UseProgramSkillCommand(0, Skill, Binding, [pair[0], weapon], [], revision, s.P!.PromptId));
        s.Need(!rejected.Accepted && s.G.Revision == revision && s.Zone(weapon) == CardLocation.Equipment(0),
            "An effective-suit match never broadens the ordinary Hand-only payment to the equipment area.");
        s.Accept(new UseProgramSkillCommand(0, Skill, Binding, pair, [], s.G.Revision, s.P!.PromptId));
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch);
        var use = s.G.ResolutionStack.OfType<CardUseFrame>().Single(f => f.CardKind == CardKind.ArrowBarrage);
        var parent = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Skill && f.TriggerId is null);
        var useId = use.Id; var parentId = parent.Id;
        s.Need(use is { SourceSeat: 0, PhysicalCardIds.Count: 2, Action.Type: CardActionType.Use } &&
            use.Action.PhysicalCards.Select(c => c.CardId).Order().SequenceEqual(pair.Order()) &&
            use.Action.ConversionChain.Single().SkillId == Skill &&
            parent.SelectedCardIds.Order().SequenceEqual(pair.Order()) && parent.InstructionIndex == 1 &&
            s.G.ResolutionStack.SkipWhile(f => f.Id != parentId).Skip(1).First().Id == useId &&
            pair.All(id => s.Zone(id) == CardLocation.Processing && s.G.CardMovements.Count(m => m.CardId == id &&
                m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1),
            "One genuine native Arrow owns both original materials and remains the paid child of the original activation at its actual finalized-target child window.");
        s.Cold();
        var pause = s.P!;
        s.Accept(new AnswerPromptCommand(pause.PlayerSeat, pause.PromptId,
            pause.Choices.Single(c => c.Parameters.GetValueOrDefault("option-id") == "continue").Id, s.G.Revision));
        s.Play();
        s.Need(s.E<CardRespondedEvent>().Count(e => e.EffectiveCardKind == CardKind.Dodge) == 3 &&
            !s.E<DamageAppliedEvent>().Any() &&
            s.E<ProgramViewAsConvertedEvent>().Count(e => e.SkillId == Skill && e.IsUse) == 1 &&
            s.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == useId && e.CardKind == CardKind.ArrowBarrage) == 1 &&
            s.E<ProgramSkillResolvedEvent>().Count(e => e.FrameId == parentId && e.Completed) == 1 &&
            s.G.ResolutionStack.All(f => f.Id != parentId && f.Id != useId) &&
            pair.All(id => s.Zone(id) == CardLocation.DiscardPile && s.G.CardMovements.Count(m => m.CardId == id &&
                m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile) == 1) &&
            s.Zone(weapon) == CardLocation.Equipment(0),
            "Cold resumes three real Dodge responses and returns once to the original paid activation; both original materials settle once and equipment is untouched.");
    }

    internal sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G { get; private set; } = game;
        public ContentRegistry Registry { get; } = registry;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public CardLocation Zone(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        public void Accept(GameCommand command)
        { var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, result.Error?.Message ?? "Rejected real ordinary Yuan Shao command."); }
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Reach(Func<bool> ready)
        {
            for (var i = 0; i < 160; i++)
            {
                if (ready()) return;
                if (P is { PlayerSeat: 0, Kind: DecisionKind.Nullification or DecisionKind.RespondDodge } prompt)
                {
                    var choice = prompt.Choices.First(c => c.Cards.Count == 0);
                    Accept(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, choice.Id, G.Revision)); continue;
                }
                Need(P is not { PlayerSeat: 0 }, "Unexpected ordinary Yuan Shao boundary."); Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Fixed ordinary Yuan Shao flow did not reach its next native boundary.");
        }
        public void Cold()
        {
            string Dump() => string.Join("|", Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i)))) +
                JsonSerializer.Serialize(G.ResolutionStack) + GameCheckpointJson.Serialize(G.CreateCheckpoint());
            var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), Registry);
            Need(before == Dump(), "Cold preserves the original paid Arrow materials, actual finalized-target child and original activation.");
        }
        public void Need(bool yes, string why)
        { if (!yes) throw new InvalidOperationException(why + " " + JsonSerializer.Serialize(new { G.State.Phase, Pending = P is { } p ? new { p.Kind, p.PlayerSeat, Skill = p.SkillPrompt?.SkillId } : null, Frames = G.ResolutionStack.Select(f => new { f.Id, f.Kind }) })); }
        public static Scene Start()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var peer = new ShanheCombatantDefinition(Peer, Hp: 4) { StartingHand = [new(CardKind.Dodge, Suit.Diamond, 9)] };
            var setup = new ShanheBattleSetup(17, 1, "ordinary-mobile-yuan-shao-effective-pair", Hero, 4, 0, 0, 0, 0,
                ["classic:hongyan", "classic:luanji", Watch],
                [new(CardKind.Slash, Suit.Spade, 2), new(CardKind.Slash, Suit.Heart, 3), new(CardKind.Slash, Suit.Club, 4)],
                [new(CardKind.QinggangSword, Suit.Spade, 8)], [peer, peer], [peer], new string('f', 64));
            var scene = new Scene(GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 3 }, registry), registry);
            scene.Accept(new StartGameCommand()); return scene;
        }
    }

    private const string Peer = "fixture:ordinary-mobile-yuan-shao-peer", Watch = "fixture:ordinary-mobile-yuan-shao-arrow-child";
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ordinary-mobile-yuan-shao", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero, "classic:yuan-shao", "classic:xiao-qiao");
            var skills = new[] { new { id = Watch, revision = 1, triggers = new[] {
                new { id = "actual-arrow-finalized", window = "cardUseTargetsFinalized", ownerRelation = "actor", cardKinds = new[] { "arrowBarrage" }, optional = false,
                    usageScope = "game", usageLimit = 1, effects = new[] { new { op = "chooseOption", target = "owner", resultBind = "pause", options = new[] { new { id = "continue" } } } } }
            } } };
            var catalog = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Watch] = new { name = "真实万箭子窗", description = "原万箭目标已确认后继续。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续万箭" } } } }));
            foreach (var (id, program) in catalog.Programs) b.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                { Program = program, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new(Peer, "真实万箭响应者", "supporter", "standard:none", "wei", 4));
            const string deck = "fixture:ordinary-mobile-yuan-shao-deck";
            b.AddDeck(new(deck, "固定普通万箭费用", 0, 0, [])
                { PhysicalCards = Enumerable.Range(0, 40).Select(_ => new ContentDeckPhysicalCard("standard:slash", Suit.Club, 7)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "普通移动乱击有效花色", 4, 4, new Dictionary<string, int>(), deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
