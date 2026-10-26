using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class ZhiyanAvailableDrawChecks
{
    private const string Watch = "fixture:zhiyan-draw-moved", Strict = "fixture:zhiyan-strict-filter", Mode = "identity:zhiyan-available-fixture";

    public static void DrawMovedByRealChildDoesNotRevealEquipOrChargeItsFormerRecipientCold()
    {
        foreach (var skill in new[] { "classic:zhiyan", "boundary:zhiyan-current", "boundary:zhiyan-mobile" })
        foreach (var equipment in new[] { false, true })
        {
            var (g, registry) = Start(skill, equipment);
            Reach(g, () => P(g)?.Kind == DecisionKind.PlayCard);
            Accept(g, new EndPlayPhaseCommand(0, g.Revision, P(g)!.PromptId));
            Reach(g, () => P(g)?.SkillPrompt?.SkillId == skill);
            Answer(g, c => c.Parameters.GetValueOrDefault("program-action") == "activate");
            Reach(g, () => P(g)?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-target") == true);
            Answer(g, c => c.Targets.SequenceEqual([0]));
            Reach(g, () => P(g)?.SkillPrompt?.SkillId == Watch && P(g)!.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-target"));
            Answer(g, c => c.Targets.SequenceEqual([1]));
            Reach(g, () => P(g)?.SkillPrompt?.SkillId == Watch && P(g)!.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card"));
            var root = g.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == skill);
            var drawn = root.CardSetBindings.Single(b => b.Name == "drawn");
            var id = drawn.CardIds.Single();
            Need(drawn.Visibility == SkillProgramCardSetVisibility.Private && drawn.SourceLocations.SequenceEqual([CardLocation.Hand(0)]) &&
                Enumerable.Range(0, 4).Where(s => s != 0).All(s => g.CreateSnapshot(s).Players[0].Hand.All(c => c.Id != id)),
                "The original draw receipt remains frozen and private to its actual recipient before the legal child transfers it.");
            g = Cold(g, registry);
            Answer(g, choice => choice.Cards.SequenceEqual([id]));
            Reach(g, () => P(g)?.SkillPrompt?.SkillId == Watch && P(g)!.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "continue"));
            var move = g.CardMovements.Single(m => m.CardId == id && m.From == CardLocation.Processing && m.To == CardLocation.Hand(1));
            Need(g.CardMovements.Count(m => m.CardId == id && m.From == CardLocation.Hand(0) &&
                    m.To == CardLocation.Processing && m.Sequence < move.Sequence && m.Reason == move.Reason) == 1 &&
                !g.Events.Any(e => e.Payload is ProgramCardsRevealedEvent shown && shown.FrameId == root.Id) &&
                g.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.Id == root.Id).CardSetBindings.Single(b => b.Name == "drawn").CardIds.SequenceEqual([id]),
                "A real native gain child has transferred the actual drawn entity once without rewriting or revealing the parent's original private receipt.");
            g = Cold(g, registry);
            var targetHp = g.CreateSnapshot(0).Players[0].Hp;
            Answer(g, c => c.Parameters.GetValueOrDefault("option-id") == "continue");
            Reach(g, () => g.Events.Any(e => e.Payload is ProgramBindingResolvedEvent done && done.FrameId == root.Id));
            Need(g.Events.Select(e => e.Payload).OfType<ProgramCardsRevealedEvent>().Where(e => e.FrameId == root.Id).All(e => e.Cards.Count == 0) &&
                !g.Events.Select(e => e.Payload).OfType<RevealedCardHpComparedEvent>().Any(e => e.ProgramFrameId == root.Id) &&
                g.CreateSnapshot(0).Players[0].Hp == targetHp && g.CreateSnapshot(0).Players.All(p => p.Equipment.All(c => c.Id != id)) &&
                g.CardMovements.Count(m => m.CardId == id && m.Sequence > move.Sequence) == 0 &&
                g.Events.Select(e => e.Payload).OfType<ProgramBindingResolvedEvent>().Count(e => e.FrameId == root.Id && e.Activated && e.Completed) == 1,
                "All three genuine Zhiyan variants finish once after cold return, without revealing another hand, using departed equipment or charging the former recipient HP.");
            _ = Cold(g, registry);
        }
    }

    public static void FilterOptInPreservesStrictDefaultAndRejectsInvalidFlag()
    {
        SkillProgram Load(string flag) => SkillProgramCatalog.Load($$$"""
            {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[{"id":"{{{Strict}}}","revision":1,"activations":[{"id":"filter","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[
              {"op":"draw","target":"owner","amount":1,"resultBind":"drawn"},
              {"op":"filterBoundCards","target":"owner","sourceBind":"drawn","resultBind":"equipment","categories":["equipment"]{{{flag}}}}]}]}]}
            """, "{\"schemaVersion\":" + SkillProgramCatalog.PresentationSchemaVersion + ",\"skills\":{\"" + Strict + "\":{\"name\":\"filter\",\"description\":\"filter\"}}}").Programs[Strict];
        foreach (var invalid in new[] { "false", "null", "0", "\"true\"" })
        {
            var rejected = false;
            try { _ = Load(",\"availableAtSourceOnly\":" + invalid); } catch (InvalidOperationException) { rejected = true; }
            Need(rejected, "The available-source filter opt-in accepts only explicit true.");
        }
        var old = Load("").Activations.Single().Effects[1];
        Need(old.AvailableAtSourceOnly is null && !JsonSerializer.Serialize(old).Contains("AvailableAtSourceOnly") &&
            Load(",\"availableAtSourceOnly\":true").Activations.Single().Effects[1].AvailableAtSourceOnly == true,
            "Absent filter metadata keeps its old null serialization and explicit true reuses the optional compatible flag.");
        var (g, registry) = Start(Strict, false);
        Reach(g, () => P(g)?.Kind == DecisionKind.PlayCard);
        Accept(g, new EndPlayPhaseCommand(0, g.Revision, P(g)!.PromptId));
        Reach(g, () => P(g)?.SkillPrompt?.SkillId == Strict);
        Answer(g, c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        Reach(g, () => P(g)?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-target") == true);
        Answer(g, c => c.Targets.SequenceEqual([0]));
        Reach(g, () => P(g)?.SkillPrompt?.SkillId == Watch && P(g)!.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-target"));
        Answer(g, c => c.Targets.SequenceEqual([1]));
        Reach(g, () => P(g)?.SkillPrompt?.SkillId == Watch && P(g)!.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card"));
        Answer(g, _ => true);
        Reach(g, () => P(g)?.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "continue") == true);
        g = Cold(g, registry);
        var failedStrictly = false;
        try
        {
            Answer(g, c => c.Parameters.GetValueOrDefault("option-id") == "continue");
            Reach(g, () => g.Events.Select(e => e.Payload).OfType<ProgramBindingResolvedEvent>().Any(e => e.SkillId == Strict));
        }
        catch (InvalidOperationException ex) { failedStrictly = ex.Message.Contains("bound card left its frozen source", StringComparison.OrdinalIgnoreCase); }
        Need(failedStrictly, "A missing-flag filter retains its exact original strict failure after a real child has moved the drawn entity, including after cold recovery.");
    }

    private static PendingDecision? P(GameEngine g) => Enumerable.Range(0, 4).Select(s => g.CreateSnapshot(s).PendingDecision).FirstOrDefault(p => p is not null);
    private static void Need(bool ok, string message) { if (!ok) throw new InvalidOperationException(message); }
    private static void Accept(GameEngine g, GameCommand c) { var result = g.Submit(c); Need(result.Accepted, result.Error?.Message ?? "Zhiyan fixture command rejected."); }
    private static void Answer(GameEngine g, Func<PromptChoice, bool> predicate)
    { var p = P(g)!; Accept(g, new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(predicate).Id, g.Revision)); }
    private static void Reach(GameEngine g, Func<bool> done)
    {
        for (var i = 0; i < 120; i++)
        {
            if (done()) return;
            Need(g.State.Winner == Winner.None, "Zhiyan fixture ended before its exact native boundary.");
            Accept(g, new AdvanceOneStepCommand(g.Revision));
        }
        throw new InvalidOperationException("Zhiyan fixture missed its bounded native boundary: " + JsonSerializer.Serialize(P(g)));
    }
    private static string State(GameEngine g) => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(s => SnapshotJson.Serialize(g.CreateSnapshot(s))),
        Frames = g.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())), Events = g.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())), g.CardMovements });
    private static GameEngine Cold(GameEngine g, ContentRegistry registry)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(g.CreateCheckpoint())), registry);
        Need(State(g) == State(restored), "Cold replay retains all four private views, exact original draw receipt, real transfer and pending native child.");
        return restored;
    }
    private static (GameEngine, ContentRegistry) Start(string skill, bool equipment)
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(skill, equipment));
        var g = GameEngine.CreateStandard(new GameOptions { Seed = 31, PlayerCount = 4, ModeId = Mode, HumanSeat = 0, HumanRole = Role.Lord,
            UseInteractiveSetup = true, AdvanceAfterHumanCommands = false, MaxTurns = 5 }, registry);
        Accept(g, new StartGameCommand()); Reach(g, () => P(g)?.Kind == DecisionKind.SelectGeneral);
        Accept(g, new SelectGeneralCommand(0, "fixture:zhiyan-owner", g.Revision, P(g)!.PromptId));
        return (g, registry);
    }
    private sealed class Fixture(string skill, bool equipment) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:zhiyan-available", new Version(1, 0, 0), []);
        public void Register(IContentRegistryBuilder b)
        {
            if (skill != Strict)
                CharacterFixtureContent.Register(b, skill == "classic:zhiyan" ? "classic:yu-fan" : skill == "boundary:zhiyan-current" ? "boundary:yu-fan" : "boundary:yu-fan-mobile");
            else
            {
                var strict = SkillProgramCatalog.Load($$$"""
                    {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[{"id":"{{{Strict}}}","revision":1,"triggers":[{"id":"ending","window":"turnEnding","subject":"owner","optional":true,"effects":[
                      {"op":"selectTarget","target":"owner","targetKind":"anyLiving"},
                      {"op":"draw","target":"selectedTarget","amount":1,"resultBind":"drawn"},
                      {"op":"filterBoundCards","target":"owner","sourceBind":"drawn","resultBind":"equipment","categories":["equipment"]}]}]}]}
                    """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                        skills = new Dictionary<string, object> { [Strict] = new { name = "原严格过滤", description = "无可用位置可选字段的原过滤语义。" } } }));
                b.AddSkill(new(Strict, "原严格过滤", "严格绑定") { Program = strict.Programs[Strict], ProgramPresentation = strict.Presentations[Strict] });
            }
            var c = SkillProgramCatalog.Load($$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[{"id":"{{{Watch}}}","revision":1,"triggers":[{"id":"move","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["skill-program.{{{skill}}}.Draw"],"optional":false,"effects":[
                  {"op":"selectTarget","target":"owner","targetKind":"otherLiving"},
                  {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},"zones":["hand"],"count":1,"destination":"selectedTargetHand","targetRef":{"kind":"selectedTarget"},"awaitMovementTriggers":true},
                  {"op":"chooseOption","target":"owner","resultBind":"moved","options":[{"id":"continue"}]}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = new Dictionary<string, object> {
                    [Watch] = new { name = "摸牌实体的原生转移子窗", description = "实际获取唯一新摸手牌后等待。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } } }));
            b.AddSkill(new(Watch, "原生转移子窗", "真实转移") { Program = c.Programs[Watch], ProgramPresentation = c.Presentations[Watch] });
            b.AddSkill(new("fixture:zhiyan-owner-pick", "本人", "公开选将权重") { SelectionWeights = Enum.GetValues<Role>().ToDictionary(r => r, r => r == Role.Lord ? 10000d : -10000d) });
            b.AddGeneral(new("fixture:zhiyan-owner", "直言来源", "supporter", "fixture:zhiyan-owner-pick", "wu", 4, [skill, Watch]));
            var peers = Enumerable.Range(1, 3).Select(i => "fixture:zhiyan-peer-" + i).ToArray();
            foreach (var id in peers) b.AddGeneral(new(id, "其他角色", "supporter", "standard:none", "shu", 6));
            b.AddDeck(new("fixture:zhiyan-deck", "真实单牌结果", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 32).Select(_ =>
                new ContentDeckPhysicalCard(equipment ? "standard:crossbow" : "standard:dodge", Suit.Heart, 7)).ToArray() });
            b.AddMode(new(Mode, "直言真实获得续接", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 3 },
                "fixture:zhiyan-deck", GeneralCandidateCount: 4, GeneralPoolIds: ["fixture:zhiyan-owner", .. peers]));
        }
    }
}
