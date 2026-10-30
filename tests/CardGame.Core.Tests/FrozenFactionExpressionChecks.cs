using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class FrozenFactionExpressionChecks
{
    private const string Hero = "fixture:frozen-faction-owner", Template = "boundary:liao-hua", Skill = "boundary:fuli";
    private const string Quiet = "fixture:frozen-faction-quiet", Driver = "fixture:frozen-faction-driver";
    private const string Recovery = "fixture:frozen-faction-recovery", Gain = "fixture:frozen-faction-gain";
    private const string DrawReason = "program.frozen-faction-recovery.draw";

    public static void FrozenRecoveryFactsSurviveRealChildrenAndColdReturn()
    {
        foreach (var (priorDamage, killPeer, flips) in new[] { (3, true, true), (4, false, false) })
        {
            var (game, registry) = Create(killPeer);
            Use(game, "history", [3]);
            Play(game);
            if (priorDamage == 4) { Use(game, "one-more", [3]); Play(game); }
            Need(OwnerDamage(game) == priorDamage, "The comparison starts from actual attributed damage, not an injected counter.");
            Use(game, "fatal");
            Reach(game, () => Decision(game) is { Kind: DecisionKind.RescueDying, PlayerSeat: 0 });
            Answer(game, choice => choice.Parameters.GetValueOrDefault("response") == "program-trigger" &&
                choice.Parameters.GetValueOrDefault("skill-id") == Skill);
            Reach(game, () => Decision(game)?.SkillPrompt?.SkillId == Recovery);
            var root = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Skill);
            var rootId = root.Id;
            Need(View(game, 0).Hp == 4 && !View(game, 0).IsFaceDown && OwnerDamage(game) == priorDamage,
                "The real recovery finishes before its child, while the final face instruction has not executed.");
            game = Cold(game, registry);
            Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
            if (killPeer)
            {
                Reach(game, () => Decision(game)?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "select-target") == true);
                Answer(game, choice => choice.Targets.SequenceEqual([2]));
            }
            Reach(game, () => Decision(game)?.SkillPrompt?.SkillId == Gain);
            Need(View(game, 0).Hand.Count == 4 && !View(game, 0).IsFaceDown &&
                game.CardMovements.Count(move => move.To == CardLocation.Hand(0) && move.Reason.Value == DrawReason) == 4 &&
                (!killPeer || !View(game, 2).IsAlive && OwnerDamage(game) == priorDamage + 2),
                "The draw still fills the frozen four-faction hand target after a real child changes living factions and game damage.");
            var restored = Cold(game, registry);
            foreach (var branch in new[] { game, restored })
            {
                FinishGainChildren(branch);
                Need(View(branch, 0).IsAlive && View(branch, 0).Hp == 4 && View(branch, 0).IsFaceDown == flips &&
                    branch.CardMovements.Count(move => move.To == CardLocation.Hand(0) && move.Reason.Value == DrawReason) == 4 &&
                    Events<ProgramBindingResolvedEvent>(branch).Count(done => done.FrameId == rootId && done.Activated && done.Completed) == 1,
                    "The original frame completes once, preserving its paid draw and frozen comparison across cold return.");
            }
            Need(State(game) == State(restored), "Frozen facts, physical card movements, all views and committed facts replay identically.");
        }
    }

    private static PlayerSnapshot View(GameEngine game, int seat) => game.CreateSnapshot(seat).Players[seat];
    private static PendingDecision? Decision(GameEngine game) => game.PendingDecision ?? game.CreateSnapshot(0).PendingDecision;
    private static T[] Events<T>(GameEngine game) where T : IGameEvent => game.Events.Select(item => item.Payload).OfType<T>().ToArray();
    private static int OwnerDamage(GameEngine game) => Events<DamageAppliedEvent>(game).Where(damage => !damage.SourceLess && damage.SourceSeat == 0).Sum(damage => damage.Amount);
    private static void Need(bool ok, string reason) { if (!ok) throw new InvalidOperationException(reason); }
    private static void Accept(GameEngine game, GameCommand command)
    { var result = game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Frozen-faction command rejected."); }
    private static void Use(GameEngine game, string activation, int[]? targets = null) =>
        Accept(game, new UseProgramSkillCommand(0, Driver, activation, [], targets ?? [], game.Revision, Decision(game)!.PromptId));
    private static void Answer(GameEngine game, Func<PromptChoice, bool> predicate)
    { var prompt = Decision(game)!; Accept(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.First(predicate).Id, game.Revision)); }
    private static void Play(GameEngine game) => Reach(game, () => Decision(game) is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
    private static void FinishGainChildren(GameEngine game)
    {
        var observed = new HashSet<int>();
        for (var step = 0; step < 160; step++)
        {
            if (Decision(game) is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 })
            {
                var originals = game.CardMovements.Where(move => move.To == CardLocation.Hand(0) && move.Reason.Value == DrawReason)
                    .Select(move => move.CardId).ToHashSet();
                Need(originals.Count == 4 && observed.SetEquals(originals),
                    "Every actual drawn entity must finish its genuine gained-card observer before the original frame returns.");
                return;
            }
            if (Decision(game) is { } prompt)
            {
                Need(prompt.SkillPrompt?.SkillId == Gain && !View(game, 0).IsFaceDown,
                    "Only original gained-card children may pause before the frozen face comparison.");
                var observer = game.ResolutionStack.OfType<ProgramSkillFrame>().Last(frame => frame.SkillId == Gain);
                var batch = observer.WindowContext?.MovementBatch ?? throw new InvalidOperationException("A gained-card observer lost its real movement batch.");
                foreach (var move in batch.Movements.Where(move => move.To == CardLocation.Hand(0) && move.Reason.Value == DrawReason))
                    Need(observed.Add(move.CardId), "A gained-card child must not consume another child's original entity twice.");
                Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
            }
            else Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The original gained-card children did not return within the bounded fixture.");
    }
    private static void Reach(GameEngine game, Func<bool> done)
    {
        for (var step = 0; step < 220; step++)
        {
            if (done()) return;
            Need(Decision(game) is null, "Unexpected frozen-faction decision: " + Decision(game)?.Kind + ", skill " + Decision(game)?.SkillPrompt?.SkillId);
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The bounded frozen-faction fixture missed its real boundary.");
    }
    private static string State(GameEngine game) => JsonSerializer.Serialize(new
    {
        Views = Enumerable.Range(0, 4).Select(seat => SnapshotJson.Serialize(game.CreateSnapshot(seat))).ToArray(),
        Events = game.Events.Select(item => new { Type = item.Payload.GetType().Name,
            Payload = JsonSerializer.Serialize(item.Payload, item.Payload.GetType()) }).ToArray(),
        game.CardMovements
    });
    private static GameEngine Cold(GameEngine game, ContentRegistry registry)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Need(State(game) == State(restored), "A real frozen-faction child checkpoint must replay before continuing.");
        return restored;
    }
    private static (GameEngine Game, ContentRegistry Registry) Create(bool killPeer)
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(killPeer));
        var setup = new ShanheBattleSetup(52, 1, "frozen-faction-expression", Hero, 3, 0, 0, 0, 0,
            [Quiet, Driver, Recovery, Gain], [], [],
            [new("fixture:frozen-faction-wei", Hp: 2) { StartingHand = [] },
             new("fixture:frozen-faction-wu", Hp: 20) { StartingHand = [] }],
            [new("fixture:frozen-faction-qun", Hp: 20) { StartingHand = [] }], new string('f', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false, MaxTurns = 8
        }, registry);
        Accept(game, new StartGameCommand()); Play(game); return (game, registry);
    }

    private sealed class Fixture(bool killPeer) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-frozen-faction-expression", new(1, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Template);
            var extraRecovery = killPeer ? """,{"op":"selectTarget","target":"owner","targetKind":"otherLiving"},{"op":"damage","target":"selectedTarget","amount":2}""" : "";
            var rules = $$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"no-draw","isSkillInvocation":false,"window":"drawPhaseStarting","subject":"owner","optional":false,"effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]}]},
                  {"id":"{{Driver}}","revision":1,"activations":[
                    {"id":"history","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":3}]},
                    {"id":"one-more","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":1}]},
                    {"id":"fatal","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"loseHp","target":"owner","amount":6}]}]},
                  {"id":"{{Recovery}}","revision":1,"triggers":[{"id":"paid-recovery-child","window":"afterHpRecovered","subject":"owner","optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"seen","options":[{"id":"continue"}]}{{extraRecovery}}]}]},
                  {"id":"{{Gain}}","revision":1,"triggers":[{"id":"paid-draw-child","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["{{DrawReason}}"],"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"seen","options":[{"id":"continue"}]}]}]}
                ]}
                """;
            var presentation = JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new[] { Quiet, Driver, Recovery, Gain }.ToDictionary(id => id, id => id is Recovery or Gain
                    ? (object)new { name = id, description = "真实冻结事实及已付子结算", optionLabels = new { @continue = "继续" } }
                    : new { name = id, description = "真实冻结事实及已付子结算" })
            });
            var catalog = SkillProgramCatalog.Load(rules, presentation);
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                { Program = program, ProgramPresentation = catalog.Presentations[id] });
            // Keep the genuine Fuli definition while excluding unrelated Dangxian phase setup.
            builder.AddGeneral(new(Hero, "冻结事实拥有者", "supporter", Skill, "shu", 4));
            foreach (var faction in new[] { "wei", "wu", "qun" })
                builder.AddGeneral(new("fixture:frozen-faction-" + faction, faction, "supporter", Quiet, faction, 20));
            builder.AddDeck(new("fixture:frozen-faction-deck", "无杀无桃的原生牌堆", 0, 0, [])
            { PhysicalCards = Enumerable.Range(0, 80).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 2)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "冻结事实原生场景", 4, 4, new Dictionary<string, int>(),
                "fixture:frozen-faction-deck", GeneralPoolIds: [Hero, "fixture:frozen-faction-wei", "fixture:frozen-faction-wu", "fixture:frozen-faction-qun"],
                ModeKind: ContentModeKind.Team, TeamCounts: new Dictionary<string, int>
                { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
