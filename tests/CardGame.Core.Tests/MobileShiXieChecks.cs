using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileShiXieChecks
{
    private const string Owner = "fixture:mobile-shi-xie-owner", Driver = "fixture:mobile-shi-xie-driver", Watch = "fixture:mobile-shi-xie-watch", Mode = "identity:mobile-shi-xie";
    private const string Biluan = "mobile:biluan", Lixia = "mobile:lixia";
    public static void DrawReplacementUsesIncomingDirectionAndAllNormalCardsCold()
    {
        var s = Scene.Start(); s.Reach(() => s.OptionalBiluan || s.P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        s.Need(s.OptionalBiluan && s.G.GetCombatDistance(1, 0) == 1 && s.G.GetCombatDistance(0, 1) == 6 && s.V(0).HandCount == 4,
            "Biluan qualifies from another character's incoming distance1 even though the owner's opposite outgoing distance is6, before its five-card normal Draw.");
        s.Cold(); s.ActivateBiluan(); s.Play(0);
        var change = s.E<MobileShiXieIncomingDistanceChangedEvent>().Single();
        s.Need(change.Amount == 4 && change.Source.OwnerSeat == 0 && s.V(0).HandCount == 4 && s.CurrentTurnDraws(0).Length == 0 &&
            s.G.GetCombatDistance(1, 0) == 5 && s.G.GetCombatDistance(2, 0) == 6 && s.G.GetCombatDistance(0, 1) == 6,
            "The accepted native replacement abandons all five normal draw cards and adds the four living factions only to other characters' incoming distance."); s.Cold();

        var blocked = Scene.Start(incomingBlocked: true); blocked.Reach(() => blocked.OptionalBiluan || blocked.P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        blocked.Need(!blocked.OptionalBiluan && blocked.P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && blocked.G.GetCombatDistance(0, 1) == 1 &&
            blocked.G.GetCombatDistance(1, 0) == 3 && blocked.V(0).HandCount == 9 && blocked.CurrentTurnDraws(0).SequenceEqual([5]) && blocked.E<MobileShiXieIncomingDistanceChangedEvent>().Length == 0,
            "Outgoing distance1 cannot qualify when every other incoming distance exceeds1; the unchanged native Draw gives its full base2 plus modifier3 cards.");
    }
    public static void CourtesyHasTwoExclusiveOneCardBenefitsAndCumulativeDistanceCold()
    {
        var s = Scene.Start(); s.ActivateBiluan(); s.Play(0); s.End(); s.Play(1); s.End(); s.Courtesy(); s.TwoOptions();
        var first = s.Root!.Id; var hand0 = s.V(0).HandCount; var hand1 = s.V(1).HandCount;
        s.Cold(); s.Option("self"); s.GainPause();
        s.Need(s.Root is { InstructionIndex: 2 } root && root.Id == first && s.V(0).HandCount == hand0 + 1 && s.V(1).HandCount == hand1 &&
            s.G.GetCombatDistance(1, 0) == 5 && s.E<MobileShiXieIncomingDistanceChangedEvent>().All(e => e.Amount > 0),
            "The self branch draws exactly one physical card on the original Ending frame; the actual gain observer suspends it before any distance decrease.");
        s.Cold(); s.Continue(); s.Play(2);
        s.Need(s.E<MobileShiXieIncomingDistanceChangedEvent>().Select(e => e.Amount).SequenceEqual([4, -1]) && s.G.GetCombatDistance(1, 0) == 4,
            "Returning the real self-draw child settles one permanent negative distance step exactly once.");
        s.End(); s.Courtesy(); s.TwoOptions(); var second = s.Root!.Id; hand0 = s.V(0).HandCount; var hand2 = s.V(2).HandCount;
        s.Option("current"); s.GainPause();
        s.Need(s.Root is { InstructionIndex: 2 } current && current.Id == second && s.V(2).HandCount == hand2 + 1 && s.V(0).HandCount == hand0 &&
            s.G.GetCombatDistance(1, 0) == 4 && s.E<MobileShiXieIncomingDistanceChangedEvent>().Count(e => e.Amount == -1) == 1,
            "The other branch gives exactly one card to the actual current Ending actor2, without also paying the self branch or decreasing distance before its native gain child returns.");
        s.Cold(); s.Continue(); s.Play(3);
        var changes = s.E<MobileShiXieIncomingDistanceChangedEvent>();
        s.Need(changes.Select(e => e.Amount).SequenceEqual([4, -1, -1]) && changes.Where(e => e.Amount < 0).Select(e => e.ActualTurnOwnerSeat).SequenceEqual([1, 2]) &&
            changes.All(e => e.Source.OwnerSeat == 0) && s.G.GetCombatDistance(1, 0) == 3 && s.G.GetCombatDistance(2, 0) == 4 && s.G.GetCombatDistance(0, 1) == 6 &&
            s.G.CardMovements.Count(m => m.Reason.Value == "skill-program.mobile:lixia.Draw" && m.To == CardLocation.Hand(0)) == 1 &&
            s.G.CardMovements.Count(m => m.Reason.Value == "skill-program.mobile:lixia.Draw" && m.To == CardLocation.Hand(2)) == 1,
            "Separate actual Ending windows accumulate signed incoming distance and preserve each branch's sole one-card recipient across cold replay."); s.Cold();
    }
    public static void IssuedCourtesyDrawReturnsAfterSkillLossCold()
    {
        var s = Scene.Start(loseAtGain: true); s.ActivateBiluan(); s.Play(0); s.End(); s.Play(1); s.End(); s.Courtesy();
        var root = s.Root!.Id; s.Option("self"); s.RemoveCourtesyInGainChild(); s.GainPause();
        s.Need(!s.V(0).Skills!.Any(skill => skill.Id == Lixia) && s.Root is { InstructionIndex: 2 } f && f.Id == root &&
            s.E<MobileShiXieCourtesyDrawIssuedEvent>().Any(e => e.ProgramFrameId == f.Id && e.InstructionIndex == 2 && e.RecipientSeat == 0 && e.ActualCount == 1) && s.E<MobileShiXieIncomingDistanceChangedEvent>().All(e => e.Amount > 0),
            "The selected self benefit has genuinely drawn and its gain child removes the exact Lixia instance while the original issued frame still waits before its distance step.");
        s.Cold(); s.Continue(); s.Play(2);
        s.Need(s.E<MobileShiXieIncomingDistanceChangedEvent>().Select(e => e.Amount).SequenceEqual([4, -1]) &&
            s.E<MobileShiXieIncomingDistanceChangedEvent>().Single(e => e.Amount == -1).ProgramFrameId == root && s.G.GetCombatDistance(1, 0) == 4 && !s.V(0).Skills!.Any(skill => skill.Id == Lixia),
            "Only the already-issued native benefit returns through its original frame after skill loss and issues one distance decrease; previous permanent increments remain.");
        s.End(); s.Play(3);
        s.Need(s.E<MobileShiXieIncomingDistanceChangedEvent>().Select(e => e.Amount).SequenceEqual([4, -1]),
            "Losing Lixia allows its paid return but grants no fresh later Ending trigger."); s.Cold();
    }
    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public ProgramSkillFrame? Root => G.ResolutionStack.OfType<ProgramSkillFrame>().LastOrDefault(f => f.SkillId == Lixia);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public bool OptionalBiluan => P?.SkillPrompt?.SkillId == Biluan && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        public int[] CurrentTurnDraws(int seat)
        {
            var all = G.Events.Select(e => e.Payload).ToArray(); var start = Array.FindLastIndex(all, e => e is TurnStartedEvent t && t.TurnNumber == G.State.TurnNumber && t.ActorSeat == G.State.CurrentSeat);
            return all.Skip(start + 1).OfType<CardsDrawnEvent>().Where(e => e.PlayerSeat == seat).Select(e => e.Count).ToArray();
        }
        public void ActivateBiluan() { Reach(() => OptionalBiluan); Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate"); }
        public void Courtesy() => Reach(() => P?.SkillPrompt?.SkillId == Lixia && P.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "self"));
        public void TwoOptions() => Need(P!.Choices.Count == 2 && P.Choices.Select(c => c.Parameters.GetValueOrDefault("option-id")).Order().SequenceEqual(["current", "self"]) && P.Choices.All(c => c.Parameters.GetValueOrDefault("program-action") != "skip"),
            "Locked Lixia offers exactly one of two one-card benefits, without decline, recovery, two-card or combined choices.");
        public void Option(string id) => Answer(c => c.Parameters.GetValueOrDefault("option-id") == id);
        public void GainPause() => Reach(() => P?.SkillPrompt?.SkillId == Watch && P.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "continue"));
        public void RemoveCourtesyInGainChild()
        {
            Reach(() => P?.SkillPrompt?.SkillId == Watch && P.Choices.Any(c => c.Parameters.GetValueOrDefault("advanced-value") == Lixia));
            Need(Root is { InstructionIndex: 2 } && V(0).Skills!.Any(skill => skill.Id == Lixia),
                "The real courtesy Draw gain child offers its original live Lixia instance for the native permanent replacement.");
            Answer(c => c.Parameters.GetValueOrDefault("advanced-value") == Lixia);
            Reach(() => P?.SkillPrompt?.SkillId == Watch && P.Choices.Any(c => c.Parameters.GetValueOrDefault("advanced-value") == "finish"));
            Answer(c => c.Parameters.GetValueOrDefault("advanced-value") == "finish");
        }
        public void Continue() => Option("continue");
        public void Play(int seat) => Reach(() => P is { Kind: DecisionKind.PlayCard } p && p.PlayerSeat == seat);
        public void End() { var p = P!; Accept(new EndPlayPhaseCommand(p.PlayerSeat, G.Revision, p.PromptId)); }
        public void Answer(Func<PromptChoice, bool> predicate) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(predicate).Id, G.Revision)); }
        public void Accept(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected actual mobile Shi Xie command."); }
        public void Reach(Func<bool> ready)
        {
            for (var i = 0; i < 220 && !ready(); i++)
            {
                if (P is { Kind: DecisionKind.SelectGeneral } select) { Accept(new SelectGeneralCommand(select.PlayerSeat, select.PlayerSeat == 0 ? Owner : Peer(select.PlayerSeat), G.Revision, select.PromptId)); continue; }
                if (P is { Kind: DecisionKind.PlayCard }) { End(); continue; }
                if (P is { Kind: DecisionKind.DiscardCards } discard) { Accept(new DiscardCardsCommand(discard.PlayerSeat, discard.ValidCardIds.Take(discard.RequiredCardCount).ToArray(), discard.PromptId, G.Revision)); continue; }
                if (P is { } p)
                {
                    var choice = p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("program-action") == "skip") ?? p.Choices.FirstOrDefault(c => c.Cards.Count == 0 && c.Targets.Count == 0) ?? p.Choices.First();
                    Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, choice.Id, G.Revision));
                }
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(ready(), "Missing bounded mobile Shi Xie actual phase boundary.");
        }
        public void Cold()
        {
            var copy = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(Enumerable.Range(0, 4).All(i => SnapshotJson.Serialize(G.CreateSnapshot(i)) == SnapshotJson.Serialize(copy.CreateSnapshot(i))) &&
                G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).SequenceEqual(copy.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType()))) && G.CardMovements.SequenceEqual(copy.CardMovements),
                "Cold replay preserves original native choices, real one-card Draw child frames and scalar permanent directed distance history."); G = copy;
        }
        public void Need(bool ok, string text) { if (!ok) throw new InvalidOperationException(text + " " + JsonSerializer.Serialize(new { P, Turn = G.State.TurnNumber, Current = G.State.CurrentSeat })); }
        public static Scene Start(bool incomingBlocked = false, bool loseAtGain = false)
        {
            var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture(incomingBlocked, loseAtGain));
            var s = new Scene(GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
                ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8 }, r), r);
            s.Accept(new StartGameCommand()); return s;
        }
    }
    private static string Peer(int seat) => "fixture:mobile-shi-xie-peer-" + seat;
    private sealed class Fixture(bool incomingBlocked, bool loseAtGain) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:mobile-shi-xie", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, "mobile:shi-xie");
            var loss = loseAtGain ? """{"op":"replaceSkillsOnAwakening","target":"owner","skillIds":["standard:none"]},""" : "";
            var direction = incomingBlocked ? """{"id":"only-incoming","query":"incomingDistance","operation":"add","value":2,"priority":0}""" : """{"id":"only-outgoing","query":"outgoingDistance","operation":"add","value":5,"priority":0}""";
            var rules = $$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
              {"id":"{{Driver}}","revision":1,"modifiers":[{"id":"full-five-card-draw","query":"drawCount","operation":"add","value":3,"priority":0},{"id":"retain-real-materials","query":"handLimit","operation":"add","value":20,"priority":0},{{direction}}]},
              {"id":"{{Watch}}","revision":1,"triggers":[{"id":"paid-real-draw-child","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementReasons":["skill-program.mobile:lixia.Draw"],"movementOccurrence":"perBatch","usageScope":"game","usageLimit":1,"optional":false,
                "effects":[{{loss}}{"op":"chooseOption","target":"owner","resultBind":"draw-child-pause","options":[{"id":"continue"}]}]}]}
            ]}
            """;
            var names = new Dictionary<string, object> { [Driver] = new { name = "真实方向和完整摸牌数", description = "原生方向修正与摸牌数量" }, [Watch] = new { name = "礼下实际摸牌子返回", description = "实际移动子链暂停", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } };
            var c = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = names }));
            foreach (var (id, p) in c.Programs) b.AddSkill(new(id, id, id) { Program = p });
            var replacementQualifications = loseAtGain ? Enumerable.Range(0, 6).Select(i => "fixture:mobile-shi-xie-qualification-" + i).ToArray() : [];
            foreach (var id in replacementQualifications) b.AddSkill(new(id, "原生替换资格", "使真实获得子窗满足原生技能替换数量条件。"));
            b.AddGeneral(new(Owner, "移动士燮真实阶段", "supporter", Biluan, "qun", 8, [Lixia, Driver, Watch, .. replacementQualifications]));
            for (var seat = 1; seat < 4; seat++) b.AddGeneral(new(Peer(seat), "固定其他势力", "supporter", Watch, new[] { "qun", "wei", "shu", "wu" }[seat], 8));
            const string deck = "fixture:mobile-shi-xie-deck";
            b.AddDeck(new(deck, "固定原生闪实体", 4, 2, []) { PhysicalCards = Enumerable.Range(0, 100).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 2)).ToArray() });
            b.AddMode(new(Mode, "移动士燮有向永久距离", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 1, [nameof(Role.Renegade)] = 2 }, deck,
                GeneralCandidateCount: 4, GeneralPoolIds: [Owner, Peer(1), Peer(2), Peer(3)]));
        }
    }
}
