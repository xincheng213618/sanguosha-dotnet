using System.IO;
using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ProgramLiangzhuCompositionChecks
{
    private const string Hero = "ol:sp-sun-shangxiang", Liangzhu = "ol:liangzhu", Zenrun = "ol:zenrun";
    private const string Owner = "fixture:liangzhu-recipient", EnemyA = "fixture:liangzhu-enemy-a", EnemyB = "fixture:liangzhu-enemy-b";
    private const string Driver = "fixture:liangzhu-recovery", Before = "fixture:liangzhu-before", Gain = "fixture:liangzhu-gain", Quiet = "fixture:liangzhu-quiet";
    private const string DrawReason = "skill-program.ol:liangzhu.Draw", TakeReason = "skill-program.sun-hong.zenrun.obtain";

    public static void HelpedIsRecordedAtNativeDrawReturnIncludingReplacementZero()
    {
        foreach (var replaced in new[] { false, true })
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(replaced));
            var game = Start(registry, replaced);
            Reach(game, () => P(game) is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
            Accept(game, new UseProgramSkillCommand(0, Driver, "recover", [], [], game.Revision, P(game)!.PromptId));
            Reach(game, () => P(game)?.SkillPrompt?.SkillId == Liangzhu);
            var rootId = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Liangzhu).Id;
            Need(E<SpSunShangxiangRecoveryQualifiedEvent>(game).Single() is { RecoveredSeat: 0, HpBefore: 3, HpAfter: 4 } &&
                !E<SpSunShangxiangHelpedEvent>(game).Any(),
                "Real own-Play recovery qualifies the actual SP Liangzhu owner before any assisted draw return.");
            game = Cold(game, registry);
            Reach(game, () => P(game)?.SkillPrompt?.SkillId == (replaced ? Zenrun : Before));
            Need(!E<SpSunShangxiangHelpedEvent>(game).Any() &&
                E<ProgramOptionChosenEvent>(game).Single(e => e.FrameId == rootId).OptionId == "help",
                "The production ally AI commits help, while its pending native draw has not issued Helped.");
            if (replaced)
            {
                Answer(game, c => c.Parameters.GetValueOrDefault("program-action") == "activate");
                Reach(game, () => ZenrunFrame(game)?.Zenrun?.Stage == ZenrunStage.Target);
                Answer(game, c => c.Parameters.GetValueOrDefault("branch") == "target" && c.Targets.SequenceEqual([2]));
                for (var pick = 0; pick < 2; pick++)
                    Answer(game, c => c.Parameters.GetValueOrDefault("branch") == "pick" && c.Parameters.GetValueOrDefault("source-zone") == nameof(CardZoneKind.Hand));
                Answer(game, c => c.Parameters.GetValueOrDefault("branch") == "take");
                Reach(game, () => P(game)?.SkillPrompt?.SkillId == Gain);
                Need(game.CardMovements.Count(m => m.Reason.Value == TakeReason && m.To == CardLocation.Hand(0)) == 2 &&
                    !E<ProgramDrawCompletedEvent>(game).Any(e => e.FrameId == rootId) && !E<SpSunShangxiangHelpedEvent>(game).Any(),
                    "Replacement obtains and their movement children precede the original draw's return and cannot issue Helped early.");
            }
            else
            {
                Answer(game, c => c.Parameters.GetValueOrDefault("program-action") == "skip");
                Reach(game, () => P(game)?.SkillPrompt?.SkillId == Gain);
                var facts = game.Events.Select(e => e.Payload).ToArray();
                var helped = Array.FindIndex(facts, e => e is SpSunShangxiangHelpedEvent h && h.FrameId == rootId);
                var draw = Array.FindIndex(facts, e => e is ProgramDrawCompletedEvent d && d.FrameId == rootId);
                var child = Array.FindIndex(facts, e => e is ProgramBindingStartedEvent b && b.SkillId == Gain);
                Need(helped >= 0 && helped < draw && draw < child &&
                    E<ProgramDrawCompletedEvent>(game).Single(e => e.FrameId == rootId) is { RequestedCount: 2, ActualDrawCount: 2 },
                    "Helped is signed at native Draw2 return before the original draw's first movement child.");
            }
            game = Cold(game, registry);
            Reach(game, () => P(game) is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
            Need(E<ProgramDrawCompletedEvent>(game).Single(e => e.FrameId == rootId) is { RecipientSeat: 0, RequestedCount: 2 } returned &&
                returned.ActualDrawCount == (replaced ? 0 : 2) &&
                E<SpSunShangxiangHelpedEvent>(game).Count(e => e.FrameId == rootId && e.OwnerSeat == 1 && e.HelpedSeat == 0) == 1 &&
                E<ProgramBindingResolvedEvent>(game).Count(e => e.FrameId == rootId && e.Completed) == 1 &&
                game.CardMovements.Count(m => m.Reason.Value == DrawReason) == (replaced ? 0 : 2),
                "Both cold paths issue Helped once at the actual native return, including a replaced draw with zero original cards.");
            if (replaced)
                Need(E<NativeDrawAttemptDecidedEvent>(game).Any(e => e.ParentFrameId == rootId && e.Replaced && e.RequestedCount == 2) &&
                    E<ZenrunPaidEvent>(game).Count(e => e.Count == 2) == 1,
                    "The zero result comes from one paid production Zenrun replacement, not a fabricated zero amount.");
        }
    }

    private static IEnumerable<T> E<T>(GameEngine game) => game.Events.Select(e => e.Payload).OfType<T>();
    private static PendingDecision? P(GameEngine game) => Enumerable.Range(0, 4).Select(seat => game.CreateSnapshot(seat).PendingDecision).FirstOrDefault(p => p is not null);
    private static ProgramSkillFrame? ZenrunFrame(GameEngine game) => game.ResolutionStack.OfType<ProgramSkillFrame>().LastOrDefault(f => f.Zenrun is not null);
    private static void Answer(GameEngine game, Func<PromptChoice, bool> predicate)
    {
        var prompt = P(game)!;
        Accept(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.First(predicate).Id, game.Revision));
    }
    private static void Reach(GameEngine game, Func<bool> reached)
    {
        for (var step = 0; step < 160; step++)
        {
            if (reached()) return;
            if (P(game) is { PlayerSeat: 0 } prompt)
            {
                Need(prompt.SkillPrompt?.SkillId == Gain, "Unexpected Liangzhu boundary: " + JsonSerializer.Serialize(prompt));
                Answer(game, c => c.Parameters.GetValueOrDefault("option-id") == "continue");
            }
            else Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The bounded real Liangzhu composition did not reach its expected boundary.");
    }
    private static GameEngine Cold(GameEngine game, ContentRegistry registry) => GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
    private static void Accept(GameEngine game, GameCommand command)
    {
        var result = game.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single());
        Need(result.Accepted, result.Error?.Message ?? "The real Liangzhu command was rejected.");
    }
    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }

    private static GameEngine Start(ContentRegistry registry, bool replaced)
    {
        var setup = new ShanheBattleSetup(7, 1, "composed-liangzhu", Owner, 4, 0, 0, 0, 0, [],
            [new(CardKind.Dodge, Suit.Club, 7)], [],
            [new(EnemyA, Hp: 8) { StartingHand = [new(CardKind.Dodge, Suit.Heart, 6), new(CardKind.Dodge, Suit.Club, 8)] }, new(EnemyB, Hp: 8) { StartingHand = [] }],
            [new(Hero, Hp: 3) { StartingHand = [] }], new string('d', 64));
        var game = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0,
            HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false,
            MaxTurns = 8, AiPolicyVersion = AiDecisionPolicy.CurrentVersion }, registry);
        Accept(game, new StartGameCommand());
        return game;
    }

    private sealed class Fixture(bool replaced) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-composed-liangzhu", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            if (replaced)
            {
                string Read(string suffix)
                {
                    using var stream = typeof(StandardContentPackage).Assembly.GetManifestResourceStream("CardGame.Content.Standard.SkillPrograms.ol-sun-hong." + suffix + ".json")!;
                    using var reader = new StreamReader(stream); return reader.ReadToEnd();
                }
                var production = SkillProgramCatalog.Load(Read("rules"), Read("presentation"));
                builder.AddSkill(new(Zenrun, "禅润", "Native draw replacement") { Program = production.Programs[Zenrun], ProgramPresentation = production.Presentations[Zenrun] });
            }
            var rules = $$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,"activations":[{"id":"recover","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                    "effects":[{"op":"loseHp","target":"owner","amount":1},{"op":"recover","target":"owner","amount":1}]}]},
                  {"id":"{{Before}}","revision":1,"triggers":[{"id":"before-draw","window":"nativeDrawStarting","subject":"owner","optional":true,
                    "condition":{"kind":"phaseIsPlay"},"effects":[{"op":"chooseOption","target":"owner","resultBind":"before","options":[{"id":"continue"}]}]}]},
                  {"id":"{{Gain}}","revision":1,"triggers":[{"id":"draw-gain","window":"cardsGained","subject":"owner","optional":false,
                    "destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["{{DrawReason}}","{{TakeReason}}"],
                    "effects":[{"op":"chooseOption","target":"owner","resultBind":"gain-child","options":[{"id":"continue"}]}]}]},
                  {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,
                    "effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}]}
                """;
            var labels = new[] { Driver, Before, Gain, Quiet }.ToDictionary(id => id, id => new { name = id, description = "Real native Liangzhu boundary",
                optionLabels = id is Before or Gain ? new Dictionary<string, string> { ["continue"] = "Continue" } : new() });
            var catalog = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in catalog.Programs) builder.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Owner, "Recovered recipient", "", Driver, "shu", 4, [Gain, replaced ? Zenrun : Before]));
            foreach (var id in new[] { EnemyA, EnemyB }) builder.AddGeneral(new(id, "Other", "", Quiet, "qun", 8));
            const string deck = "fixture:composed-liangzhu-deck";
            builder.AddDeck(new(deck, "Fixed Dodge deck", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 48).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "Liangzhu composition", 4, 4, new Dictionary<string, int>(), deck,
                GeneralPoolIds: [Owner, Hero, EnemyA, EnemyB], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
