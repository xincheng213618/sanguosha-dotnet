using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileLuJiChecks
{
    private const string Huaiju = "classic:huaiju", Yili = "mobile:yili", Zhenglun = "mobile:zhenglun";
    private const string Owner = "fixture:mobile-lu-ji-owner", Driver = "fixture:mobile-lu-ji-driver",
        Watch = "fixture:mobile-lu-ji-paid-child", Mode = "identity:classic-mobile-lu-ji";

    public static void HuaijuQualificationAndAlreadyPaidGiftSurviveRealLossCold()
    {
        var s = Scene.Start(); s.Reach(() => s.Offered(Zhenglun));
        s.Need(s.Has(Huaiju) && s.Oranges(0) == 0 && s.V(0).HandCount == 4,
            "The fixture removes the three genuine opening oranges; owning Huaiju qualifies the real zero-orange Draw replacement.");
        s.Activate(); s.Reach(() => s.Offered(Yili));
        s.Need(s.Oranges(0) == 1 && s.V(0).HandCount == 4 && s.CurrentTurnDraws(0).Length == 0,
            "With Huaiju still owned, Zhenglun replaces the whole native Draw with exactly one orange and Yili is offered at the actual Play start.");
        s.Activate(); s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("pay") == "hp") == true);
        var paidRoot = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Yili).Id;
        var beforeHp = s.V(0).Hp;
        s.Answer(c => c.Parameters.GetValueOrDefault("pay") == "hp");
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("advanced-value") == Huaiju));
        s.Need(s.V(0).Hp == beforeHp - 1 && s.E<ProgramSkillHpLostEvent>().Count(e => e.FrameId == paidRoot && e.SkillId == Yili && e.TargetSeat == 0 && e.Amount == 1) == 1,
            "The original Yili frame pays one real HP loss before its genuine after-HP-loss child offers a permanent source replacement.");
        s.Answer(c => c.Parameters.GetValueOrDefault("advanced-value") == Huaiju);
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("advanced-value") == "finish"));
        s.Answer(c => c.Parameters.GetValueOrDefault("advanced-value") == "finish");
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "continue"));
        s.Need(!s.Has(Huaiju) && s.Has(Yili) && s.Has(Zhenglun) && s.Has("standard:none") && s.Oranges(0) == 1 && s.Oranges(1) == 0 &&
            s.G.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.Id == paidRoot && f.SkillId == Yili),
            "Only Huaiju is genuinely replaced; the already-paid original Yili and both dependent skills remain while its HP child pauses before granting any orange.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Yili && s.P.Choices.Any(c => c.Targets.SequenceEqual([1])));
        s.Answer(c => c.Targets.SequenceEqual([1])); s.Play(0);
        s.Need(!s.Has(Huaiju) && s.Oranges(0) == 1 && s.Oranges(1) == 1 && s.V(0).Hp == beforeHp - 1 &&
            s.E<ProgramSkillHpLostEvent>().Count(e => e.FrameId == paidRoot && e.SkillId == Yili && e.Amount == 1) == 1 &&
            s.E<ProgramBindingStartedEvent>().Count(e => e.SkillId == Yili && e.OwnerSeat == 0) == 1,
            "Cold return from the real loss child finishes the same paid Yili gift exactly once after Huaiju disappears, without charging HP or orange again.");
        var hand = s.V(0).HandCount; s.End(); s.Play(1);
        s.Need(s.Oranges(0) == 0 && s.Oranges(1) == 1 && !s.Has(Huaiju),
            "An ordinary Ending marker instruction removes the remaining owner orange, leaving a real zero-orange next-Draw qualification boundary.");
        s.Cold(); s.Reach(() => s.Offered(Zhenglun) || s.Offered(Yili) || s.P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        s.Need(s.P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && !s.Has(Huaiju) && s.Has(Yili) && s.Has(Zhenglun) && s.Oranges(0) == 0 && s.Oranges(1) == 1 &&
            s.V(0).HandCount == hand + 2 && s.CurrentTurnDraws(0).SequenceEqual([2]) &&
            s.E<ProgramBindingStartedEvent>().Count(e => e.SkillId == Zhenglun && e.OwnerSeat == 0) == 1 &&
            s.E<ProgramBindingStartedEvent>().Count(e => e.SkillId == Yili && e.OwnerSeat == 0) == 1,
            "After source loss and cold replay, zero oranges do not permit fresh Zhenglun and the following actual Play start does not permit fresh Yili; the full normal two-card Draw remains intact.");
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public bool Has(string id) => V(0).Skills!.Any(skill => skill.ContentId == id);
        public int Oranges(int seat) => V(seat).Markers?.Where(m => m.Kind == PlayerMarkerKind.Orange).Sum(m => m.Count) ?? 0;
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public bool Offered(string id) => P?.SkillPrompt?.SkillId == id && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        public int[] CurrentTurnDraws(int seat)
        {
            var events = G.Events.Select(e => e.Payload).ToArray();
            var start = Array.FindLastIndex(events, e => e is TurnStartedEvent t && t.TurnNumber == G.State.TurnNumber && t.ActorSeat == G.State.CurrentSeat);
            return events.Skip(start + 1).OfType<CardsDrawnEvent>().Where(e => e.PlayerSeat == seat).Select(e => e.Count).ToArray();
        }
        public void Activate() => Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        public void Play(int seat) => Reach(() => P is { Kind: DecisionKind.PlayCard } p && p.PlayerSeat == seat);
        public void End() { var p = P!; Accept(new EndPlayPhaseCommand(p.PlayerSeat, G.Revision, p.PromptId)); }
        public void Answer(Func<PromptChoice, bool> predicate) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(predicate).Id, G.Revision)); }
        public void Accept(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected actual mobile Lu Ji command."); }
        public void Reach(Func<bool> ready)
        {
            for (var step = 0; step < 240 && !ready(); step++)
            {
                if (P is { Kind: DecisionKind.SelectGeneral } select) { Accept(new SelectGeneralCommand(select.PlayerSeat, select.PlayerSeat == 0 ? Owner : Peer(select.PlayerSeat), G.Revision, select.PromptId)); continue; }
                if (P is { Kind: DecisionKind.PlayCard }) { End(); continue; }
                if (P is { Kind: DecisionKind.DiscardCards } discard) { Accept(new DiscardCardsCommand(discard.PlayerSeat, discard.ValidCardIds.Take(discard.RequiredCardCount).ToArray(), discard.PromptId, G.Revision)); continue; }
                if (P is { } prompt)
                {
                    var choice = prompt.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("program-action") == "skip") ??
                        prompt.Choices.FirstOrDefault(c => c.Cards.Count == 0 && c.Targets.Count == 0) ?? prompt.Choices.First();
                    Accept(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, choice.Id, G.Revision));
                }
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(ready(), "Missing bounded actual mobile Lu Ji phase boundary.");
        }
        public void Cold()
        {
            var copy = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(Enumerable.Range(0, 4).All(i => SnapshotJson.Serialize(G.CreateSnapshot(i)) == SnapshotJson.Serialize(copy.CreateSnapshot(i))) &&
                G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).SequenceEqual(copy.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType()))) &&
                G.CardMovements.SequenceEqual(copy.CardMovements), "Cold recovery preserves the exact paid child, original Yili frame, actual loss and attributed orange state.");
            G = copy;
        }
        public void Need(bool ok, string text) { if (!ok) throw new InvalidOperationException(text + " " + JsonSerializer.Serialize(new { P, Turn = G.State.TurnNumber, Current = G.State.CurrentSeat })); }
        public static Scene Start()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var scene = new Scene(GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
                ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 9 }, registry), registry);
            scene.Accept(new StartGameCommand()); return scene;
        }
    }

    private static string Peer(int seat) => "fixture:mobile-lu-ji-peer-" + seat;
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:mobile-lu-ji", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, "mobile:lu-ji", "classic:lu-ji");
            var rules = $$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
              {"id":"{{Driver}}","revision":1,"modifiers":[{"id":"retain-native-draw","query":"handLimit","operation":"add","value":20,"priority":0}],"triggers":[
                {"id":"real-zero-orange-opening","window":"gameStarting","subject":"owner","priority":-100,"optional":false,"effects":[{"op":"changeParticipantMarker","target":"owner","marker":"orange","amount":-3}]},
                {"id":"real-zero-orange-next-draw","window":"turnEnding","subject":"owner","optional":false,"usageScope":"game","usageLimit":1,"effects":[{"op":"changeParticipantMarker","target":"owner","marker":"orange","amount":-1,"clearMarker":true}]}
              ]},
              {"id":"{{Watch}}","revision":1,"triggers":[{"id":"real-paid-hp-child","window":"afterHpLost","subject":"owner","optional":false,"usageScope":"game","usageLimit":1,"effects":[
                {"op":"replaceSkillsOnAwakening","target":"owner","skillIds":["standard:none"]},
                {"op":"chooseOption","target":"owner","resultBind":"paid-child-pause","options":[{"id":"continue"}]}
              ]}]}
            ]}
            """;
            var names = new Dictionary<string, object> { [Driver] = new { name = "真实零橘阶段", description = "原生开局和结束窗口移除实际标记。" },
                [Watch] = new { name = "真实遗礼付款子窗", description = "在真实体力流失子窗永久替换怀橘。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } };
            var catalog = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = names }));
            foreach (var (id, program) in catalog.Programs) b.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            var qualifications = Enumerable.Range(0, 6).Select(i => "fixture:mobile-lu-ji-qualification-" + i).ToArray();
            foreach (var id in qualifications) b.AddSkill(new(id, "原生替换资格", "使真实HP子窗满足已有原生替换条件。"));
            b.AddGeneral(new(Owner, "移动陆绩真实资格", "supporter", Huaiju, "wu", 3, [Yili, Zhenglun, Driver, Watch, .. qualifications]));
            for (var seat = 1; seat < 4; seat++) b.AddGeneral(new(Peer(seat), "固定其他角色", "supporter", "standard:none", "wei", 4));
            const string deck = "fixture:mobile-lu-ji-deck";
            b.AddDeck(new(deck, "固定原生闪实体", 4, 2, []) { PhysicalCards = Enumerable.Range(0, 100).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 2)).ToArray() });
            b.AddMode(new(Mode, "移动陆绩主技能资格", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 1, [nameof(Role.Renegade)] = 2 }, deck,
                GeneralCandidateCount: 4, GeneralPoolIds: [Owner, Peer(1), Peer(2), Peer(3)]));
        }
    }
}
