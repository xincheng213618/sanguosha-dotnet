using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class SourceLessAttributedMarkerChecks
{
    private const string Mode = "fixture:attributed-source", Deck = "fixture:attributed-source-deck",
        Owner = "fixture:attributed-source-owner", Other = "fixture:attributed-source-other",
        Driver = "fixture:attributed-source-driver", Late = "fixture:late-attributed-source",
        OwnMarker = "fixture:own-after-damage-marker", Inactive = "fixture:inactive-source-marker",
        OtherPick = "fixture:attributed-source-other-pick";

    public static void AcquiredQiangxiSourceLessPaymentSkipsSourceMarkersButPreservesOwnerRewardsAndHitCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 7, PlayerCount = 2, HumanSeat = 0,
            HumanRole = null, HumanTeamId = "player", ModeId = Mode, UseInteractiveSetup = true, AdvanceAfterHumanCommands = false,
            ConfirmOpeningHand = false, UseInteractiveDiscard = false, MaxTurns = 4 }, registry);
        Accept(game, new StartGameCommand());
        Reach(game, () => Prompt(game) is { PlayerSeat: 0, Kind: DecisionKind.SelectGeneral });
        Accept(game, new SelectGeneralCommand(0, Owner, game.Revision, Prompt(game)!.PromptId));
        Play(game);
        var originalIds = game.CreateCardZoneDiagnostics().Select(card => card.CardId).Order().ToArray();
        Use(game, Driver, "acquire", []); Play(game);
        Need(Events<SkillsAcquiredEvent>(game).Any(e => e.PlayerSeat == 0 && e.SourceSkillId == Driver &&
            e.SkillIds.SequenceEqual(["boundary:qiangxi"])), "The actual native Qiangxi source is acquired before its paid use.");
        var ownHp = View(game, 0).Hp; var otherHp = View(game, 1).Hp; var hand = View(game, 0).HandCount;
        Use(game, "boundary:qiangxi", "receive-damage", [1]); Play(game);
        var damage = Events<DamageAppliedEvent>(game);
        Need(damage is [{ TargetSeat: 0, Amount: 1, SourceLess: true },
            { SourceSeat: 0, TargetSeat: 1, Amount: 1, SourceLess: false }] &&
            View(game, 0).Hp == ownHp - 1 && View(game, 1).Hp == otherHp - 1,
            "The real acquired Qiangxi pays its one source-less owner damage, then resumes the original activation to damage the selected opponent exactly once.");
        Need(!Events<ProgramBindingStartedEvent>(game).Any(e => e.OwnerSeat == 0 && e.SkillId is "classic:wuhun" or Late) &&
            Events<PlayerMarkerChangedEvent>(game).Where(e => e.Marker == PlayerMarkerKind.Nightmare).ToArray() is
                [{ PlayerSeat: 0, SkillOwnerSeat: 1, Delta: 1, Count: 1 }, { PlayerSeat: 0, SkillOwnerSeat: 1, Delta: 1, Count: 2 }],
            "Source-less payment admits neither the real pre-dying Wuhun nor the late after-damage source marker; the genuinely sourced hit still invokes both opponent-owned marker branches with the exact source attribution.");
        Need(Events<PlayerMarkerChangedEvent>(game).Count(e => e.PlayerSeat == 0 && e.SkillOwnerSeat == 0 &&
                e.Marker == PlayerMarkerKind.Rage && e.Delta == 1) == 1 &&
            Events<ProgramBindingStartedEvent>(game).Count(e => e.SkillId == Inactive) == 1 && View(game, 0).HandCount == hand + 1,
            "An Owner-addressed after-damage marker remains legal without a source; an inactive EventSource marker condition does not suppress that trigger's separate real Draw1 reward.");
        Need(originalIds.SequenceEqual(game.CreateCardZoneDiagnostics().Select(card => card.CardId).Order()) &&
            game.ResolutionStack.Count == 0, "Both damage windows settle without manufacturing physical cards or leaving a paid parent suspended.");
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Need(Enumerable.Range(0, 2).All(seat => SnapshotJson.Serialize(restored.CreateSnapshot(seat)) ==
                SnapshotJson.Serialize(game.CreateSnapshot(seat))) &&
            restored.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).SequenceEqual(
                game.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType()))),
            "Cold replay preserves the accepted acquisition, actual paid Qiangxi, skipped absent-source candidates and both valid marker attributions.");
    }

    private static PlayerSnapshot View(GameEngine game, int seat) => game.CreateSnapshot(seat).Players[seat];
    private static PendingDecision? Prompt(GameEngine game) => Enumerable.Range(0, 2).Select(seat => game.CreateSnapshot(seat).PendingDecision)
        .FirstOrDefault(prompt => prompt is not null);
    private static T[] Events<T>(GameEngine game) where T : IGameEvent => game.Events.Select(e => e.Payload).OfType<T>().ToArray();
    private static void Need(bool value, string reason) { if (!value) throw new InvalidOperationException(reason); }
    private static void Accept(GameEngine game, GameCommand command)
    { var result = game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "The attributed-source fixture command was rejected."); }
    private static void Play(GameEngine game) => Reach(game, () => Prompt(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
    private static void Use(GameEngine game, string skill, string activation, IReadOnlyList<int> targets) =>
        Accept(game, new UseProgramSkillCommand(0, skill, activation, [], targets, game.Revision, Prompt(game)!.PromptId));
    private static void Reach(GameEngine game, Func<bool> done)
    {
        for (var step = 0; step < 160; step++)
        {
            if (done()) return;
            Need(game.State.Winner == Winner.None, "The attributed-source fixture ended before its expected boundary.");
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The attributed-source fixture exceeded its fixed native step budget.");
    }

    private sealed class SetupFactory : IGameModeSetupFeatureFactory
    {
        public string Id => "fixture:attributed-source-setup";
        public string ModeId => Mode;
        public void ValidateInitialOptions(GameOptions options, ModeConfigurationValidationPhase phase)
        {
            if (options.ModeId != Mode || options.PlayerCount != 2)
                throw new ArgumentException("The attributed-source fixture requires its exact two-seat team mode.");
        }
        public IGameModeFeature Create(ModeFeatureCreationContext context) =>
            new CardGame.Modes.Standard.StandardModeSetupFactory().Create(context);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-attributed-source", new Version(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var realSkills = StandardContentRegistry.CreateWithClassicGenerals().Skills;
            builder.AddSkill(realSkills["classic:wuhun"]);
            builder.AddSkill(realSkills["boundary:qiangxi"]);
            var rules = $$$"""
            {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
              {"id":"{{{Driver}}}","revision":1,"activations":[{"id":"acquire","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"grantSkills","target":"owner","skillIds":["boundary:qiangxi"]}]}]},
              {"id":"{{{Late}}}","revision":1,"triggers":[{"id":"late-source","window":"afterDamageApplied","subject":"owner","damageOccurrence":"perDamagePoint","optional":false,"effects":[{"op":"changeAttributedMarker","target":"owner","targetRef":{"kind":"eventSource"},"marker":"nightmare","amount":1}]}]},
              {"id":"{{{OwnMarker}}}","revision":1,"triggers":[{"id":"owner-marker","window":"afterDamageApplied","subject":"owner","damageOccurrence":"perDamagePoint","optional":false,"effects":[{"op":"changeAttributedMarker","target":"owner","targetRef":{"kind":"owner"},"marker":"rage","amount":1}]}]},
              {"id":"{{{Inactive}}}","revision":1,"triggers":[{"id":"inactive-source","window":"afterDamageApplied","subject":"owner","damageOccurrence":"perDamage","optional":false,"effects":[{"op":"changeAttributedMarker","target":"owner","targetRef":{"kind":"eventSource"},"marker":"nightmare","amount":1,"condition":{"kind":"hpAtLeast","value":100}},{"op":"draw","target":"owner","amount":1}]}]}
            ]}
            """;
            var labels = new[] { Driver, Late, OwnMarker, Inactive }.ToDictionary(id => id,
                _ => new { name = "归属标记来源", description = "真实伤害来源资格与已支付父帧继续。" });
            var catalog = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new
                { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, "归属标记来源", "真实伤害来源资格与已支付父帧继续。")
                    { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Owner, "实际武魂与已购强袭", "supporter", Driver, "god", 6,
                ["classic:wuhun", Late, OwnMarker, Inactive], GeneralGender.Male));
            builder.AddSkill(new(OtherPick, "固定对手", "公开选将顺序。")
                { SelectionWeights = Enum.GetValues<Role>().ToDictionary(role => role, _ => 1000000d) });
            builder.AddGeneral(new(Other, "实际有来源武魂目标", "supporter", OtherPick, "wei", 6,
                ["classic:wuhun", Late], GeneralGender.Male));
            builder.AddDeck(new(Deck, "固定实体", 4, 0, [new("standard:dodge", 48)]));
            builder.AddModeFeatureFactory(new SetupFactory());
            builder.AddMode(new(Mode, "来源资格", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralCandidateCount: 2, GeneralPoolIds: [Owner, Other], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { ["player"] = 1, ["other"] = 1 }));
        }
    }
}
