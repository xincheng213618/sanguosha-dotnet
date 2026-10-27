using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheBossChecks
{
    private const string Wushuang = "shanhe:shengui-wushuang", Shenqu = "shanhe:shengui-shenqu";
    private const string Owner = "fixture:shanhe-boss-owner", Driver = "fixture:shanhe-boss-driver";
    private const string Quiet = "fixture:shanhe-boss-quiet", Watch = "fixture:shanhe-boss-draw-watch";
    private const string Mode = "identity:classic-shanhe-boss-rules";

    public static void FixedCompanionOpeningReplacesRandomBaseFreezesFacesAndReplaysBeforeNativeFudao()
    {
        var input = new List<ShanheStartingCard>
        {
            new(CardKind.Slash, Suit.Heart, 5), new(CardKind.Dodge, Suit.Heart, 13),
            new(CardKind.ArrowBarrage, Suit.Heart, 1), new(CardKind.Alcohol, Suit.Spade, 3)
        };
        var expected = input.ToArray();
        var ally = new ShanheCombatantDefinition(StandardShanheBossPackage.FenghuangId, InitialHandBonus: 2)
        { StartingHand = input };
        input.Clear();
        Require(ally.StartingHand!.SequenceEqual(expected), "A combatant immediately freezes its caller-owned starting-hand collection.");
        var rejectedMutation = false;
        try { ((IList<ShanheStartingCard>)ally.StartingHand)[0] = new(CardKind.Slash); }
        catch (NotSupportedException) { rejectedMutation = true; }
        Require(rejectedMutation, "An exposed combatant starting hand rejects mutation.");
        var definition = OpeningDefinition(ally);
        var different = OpeningDefinition(ally with { StartingHand = [new(CardKind.Slash, Suit.Diamond, 9), .. expected.Skip(1)] });
        Require(ShanheCampaignContent.GetContentHash(definition) != ShanheCampaignContent.GetContentHash(different) &&
            !JsonSerializer.Serialize(new ShanheCombatantDefinition("classic:ma-chao")).Contains("StartingHand", StringComparison.Ordinal),
            "Physical recipe faces enter the campaign fingerprint while an absent optional recipe preserves legacy JSON shape.");
        var session = ShanheCampaignSession.Create(definition, new(17, "classic:cao-cao"), StandardShanheCampaignCatalog.CreateBattleRegistry);
        Require(session.Submit(new MoveShanheNodeCommand("gate", 0)).Accepted, "A real campaign command opens the companion encounter.");
        var checkpoint = ShanheCampaignJson.Deserialize(ShanheCampaignJson.Serialize(session.CreateCheckpoint()));
        var restored = ShanheCampaignSession.Restore(checkpoint, definition, StandardShanheCampaignCatalog.CreateBattleRegistry);
        Require(ShanheBattleSetup.ValueEquals(session.PendingBattleSetup, restored.PendingBattleSetup),
            "Campaign JSON replay retains the frozen per-member opening recipe.");
        RejectRecipe(() => ShanheCampaignSession.Restore(checkpoint, different, StandardShanheCampaignCatalog.CreateBattleRegistry));

        var setup = session.PendingBattleSetup!;
        var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup);
        var g = StandardShanheCampaignCatalog.CreateBattle(setup);
        var generated = g.CardMovements.Where(m => m.From == CardLocation.OutsideGame && m.To == CardLocation.Hand(1)).ToArray();
        var hand = V(g, 1).Hand;
        Require(V(g, 1).HandCount == expected.Length + 2 && generated.Length == expected.Length &&
            generated.Select(m => hand.Single(c => c.Id == m.CardId)).Select(c => new ShanheStartingCard(c.Kind, c.Suit, c.Rank)).SequenceEqual(expected) &&
            g.CardMovements.Count(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(1)) == 2,
            "The exact four fixed faces replace the normal random base, while the two-card initial bonus still draws two genuine deck entities.");
        Require(g.CreateSnapshot(0).Players.Single(p => p.Seat == 1).Hand.Count == 0,
            "A fixed companion hand remains private in the player's filtered view.");
        g = Cold(g, registry);
        Accept(g, new StartGameCommand());
        Until(g, () => E<FudaoHandRecordedEvent>(g).Any(e => e.Source.OwnerSeat == 1));
        Require(g.CardMovements.Count(m => m.To == CardLocation.Hand(1) && m.Reason.Value == "skill-program.ding-shang-wan.draw") == 3 &&
            g.CreateCardZoneDiagnostics().Select(c => c.CardId).Distinct().Count() == g.CreateCardZoneDiagnostics().Count,
            "The original Fudao GameStarting program draws three only after actual opening placement, with every generated entity in one real zone.");
        _ = Cold(g, registry);

        var empty = setup with { EnemyRoster = [new("classic:ma-chao") { StartingHand = [] }] };
        var zero = StandardShanheCampaignCatalog.CreateBattle(empty);
        Require(V(zero, 2).HandCount == 0, "An explicitly empty fixed hand means zero base cards rather than falling back to the ordinary random hand.");
        RejectRecipe(() => ShanheCampaignContent.FreezeAndValidate(OpeningDefinition(ally with
        { StartingHand = [new(CardKind.Slash, Suit.None, 1)] })));
    }

    private static ShanheCampaignDefinition OpeningDefinition(ShanheCombatantDefinition ally) => new(
        "fixture:shanhe-fixed-opening", "Actual fixed companion opening", ["classic:cao-cao"], ["gate"],
        [new("gate", "Fixed opening encounter", ShanheNodeKind.Battle, [],
            [new("encounter", "Fixed companion", [new("classic:ma-chao")], [ally])])], []);

    private static void RejectRecipe(Action action)
    {
        try { action(); }
        catch (ArgumentException) { return; }
        throw new InvalidOperationException("An invalid or mismatched fixed opening recipe was accepted.");
    }
    private static PlayerSnapshot V(GameEngine g, int seat) => g.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
    private static PendingDecision? P(GameEngine g) => Enumerable.Range(0, g.CreateSnapshot(0).Players.Count)
        .Select(s => g.CreateSnapshot(s).PendingDecision).FirstOrDefault(p => p is not null);
    private static IEnumerable<T> E<T>(GameEngine g) => g.Events.Select(e => e.Payload).OfType<T>();
    private static void Answer(GameEngine g, Func<PromptChoice, bool> predicate)
    {
        var p = P(g)!;
        Require(p.PlayerSeat == 0, "Only the real human may be answered; NPCs must use their native AI command.");
        Accept(g, new AnswerPromptCommand(0, p.PromptId, p.Choices.First(predicate).Id, g.Revision));
    }
    private static void Until(GameEngine g, Func<bool> ready)
    {
        for (var step = 0; step < 200; step++)
        {
            if (ready()) return;
            Require(g.State.Winner == Winner.None, "Shanhe boss fixture ended before its required boundary.");
            var p = P(g);
            if (p is { PlayerSeat: 0 } && p.Choices.Any(c => c.Parameters.GetValueOrDefault("response") == "pass"))
                Answer(g, c => c.Parameters.GetValueOrDefault("response") == "pass");
            else Accept(g, new AdvanceOneStepCommand(g.Revision));
        }
        throw new InvalidOperationException("Shanhe boss fixture missed its bounded native boundary: " + JsonSerializer.Serialize(P(g)));
    }
    private static GameEngine Cold(GameEngine g, ContentRegistry registry)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(g.CreateCheckpoint())), registry);
        Require(Enumerable.Range(0, g.CreateSnapshot(0).Players.Count).All(s => SnapshotJson.Serialize(g.CreateSnapshot(s)) == SnapshotJson.Serialize(restored.CreateSnapshot(s))) &&
            JsonSerializer.Serialize(g.CardMovements) == JsonSerializer.Serialize(restored.CardMovements) &&
            g.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).SequenceEqual(
                restored.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType()))),
            "Cold command replay preserves the exact responses, native draw request, private cards and physical movements.");
        return restored;
    }
    private static void Accept(GameEngine g, GameCommand command)
    { var result = g.Submit(command); Require(result.Accepted, result.Error?.Message ?? "Shanhe boss command was rejected."); }
    private static void Require(bool condition, string reason) { if (!condition) throw new InvalidOperationException(reason); }

    private static (GameEngine, ContentRegistry) Start(bool shenqu)
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardActiveSkillExpansionPackage(true),
            new StandardRescueSkillExpansionPackage(), new StandardClassicGeneralPackage(), new StandardShanheBossPackage(), new Fixture(shenqu));
        var g = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 7, PlayerCount = 4, HumanSeat = 0, HumanRole = Role.Lord, ModeId = Mode,
            UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8
        }, registry);
        Accept(g, new StartGameCommand()); Until(g, () => P(g)?.Kind == DecisionKind.SelectGeneral);
        Accept(g, new SelectGeneralCommand(0, Owner, g.Revision, P(g)!.PromptId));
        return (g, registry);
    }

    private sealed class Fixture(bool shenqu) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("shanhe-boss-rules-fixture", new Version(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            var rules = $$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
              {"id":"{{Driver}}","revision":1,"modifiers":[{"id":"slash-room","query":"slashLimit","operation":"add","value":10,"priority":0}],"activations":[
                {"id":"slash","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"useVirtualSlash","target":"selectedTarget"}]},
                {"id":"refill","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"draw","target":"selectedTarget","amount":8}]},
                {"id":"duel","minCards":0,"maxCards":0,"minTargets":2,"maxTargets":2,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"startVirtualDuel","target":"owner"}]},
                {"id":"damage","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"owner","amount":1}]}]},
              {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]},
              {"id":"{{Watch}}","revision":1,"triggers":[{"id":"pause-original-draw","window":"nativeDrawStarting","subject":"owner","optional":false,"usageScope":"game","usageLimit":1,"effects":[{"op":"changeMaximumHp","target":"owner","amount":2},{"op":"chooseOption","target":"owner","resultBind":"resume","options":[{"id":"continue"}]}]}]}
            ]}
            """;
            var catalog = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new[] { Driver, Quiet, Watch }.ToDictionary(id => id, id => new
                {
                    name = id, description = "Small actual Shanhe boss behavior fixture.",
                    optionLabels = id == Watch ? new Dictionary<string, string> { ["continue"] = "继续" } : []
                })
            }));
            foreach (var (id, program) in catalog.Programs)
                b.AddSkill(new(id, id, "Actual Shanhe native rule fixture.") { Program = program, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new(Owner, "Shanhe rule actor", "", Driver, "qun", shenqu ? 3 : 20,
                shenqu ? [Shenqu, Watch] : [Wushuang, "classic:wusheng"]));
            var peers = Enumerable.Range(1, 3).Select(i => $"fixture:shanhe-boss-peer-{i}").ToArray();
            // Native Duel AI conserves Slash at full HP; two missing HP makes it
            // genuinely answer, without bypassing its private response command.
            foreach (var peer in peers) b.AddGeneral(new(peer, "Shanhe quiet opponent", "", Quiet, "wei", 20,
                shenqu ? [] : ["classic:wusheng"]) { InitialHp = shenqu ? null : 18 });
            b.AddDeck(new("fixture:shanhe-boss-deck", "Uniform actual cards", 4, 2, [])
            {
                PhysicalCards = Enumerable.Range(0, 160).Select(i => new ContentDeckPhysicalCard(
                    shenqu ? "standard:peach" : "standard:dodge", Suit.Heart, i % 13 + 1)).ToArray()
            });
            b.AddMode(new(Mode, "Native Shanhe boss rules", 4, 4,
                new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 3 },
                "fixture:shanhe-boss-deck", GeneralCandidateCount: 4, GeneralPoolIds: [Owner, .. peers]));
        }
    }
}
