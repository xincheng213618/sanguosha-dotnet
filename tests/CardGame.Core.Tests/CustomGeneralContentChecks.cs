using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class CustomGeneralContentChecks
{
    private const string General = "custom:general-10000000000000000000000000000001";
    private const string Draw = "custom:skill-20000000000000000000000000000001";
    private const string Hurt = "custom:skill-20000000000000000000000000000002";
    private const string Recover = "custom:skill-20000000000000000000000000000003";
    private const string Reward = "custom:skill-20000000000000000000000000000004";
    private const string Watch = "fixture:custom-author-paid";
    private const string Mode = "fixture:custom-author-identity";

    public static void CompilerFreezesValidatesAndExtendsExistingContent()
    {
        var factory = new ProbeFactory();
        var baseline = ContentRegistry.Build(new StandardContentPackage(), new Fixture(factory));
        var oldHash = baseline.ContentHash;
        var effects = new List<CustomSkillEffect> { new() { Kind = CustomSkillEffectKind.Draw, Amount = 2 } };
        var skills = new List<CustomSkillDraft> { new() { Id = Draw, Name = "自建摸牌", Effects = effects } };
        var printed = new List<string> { "standard:yingzi" };
        var drafts = new List<CustomGeneralDraft> { new() { Id = General, Name = "自建武将", SkillIds = printed, CustomSkills = skills } };
        var snapshot = CustomGeneralContent.Snapshot(new() { Generals = drafts });
        effects.Clear(); skills.Clear(); printed.Clear(); drafts.Clear();
        Need(snapshot.Generals is IList<CustomGeneralDraft> { IsReadOnly: true } &&
            snapshot.Generals[0].CustomSkills is IList<CustomSkillDraft> { IsReadOnly: true } &&
            snapshot.Generals[0].CustomSkills[0].Effects is IList<CustomSkillEffect> { IsReadOnly: true } &&
            snapshot.Generals[0].CustomSkills[0].Effects[0].Amount == 2 && snapshot.Generals[0].SkillIds.Single() == "standard:yingzi",
            "The content snapshot freezes every nested caller-owned collection.");
        var first = CustomGeneralContent.CreateRegistry(baseline, snapshot);
        var second = CustomGeneralContent.CreateRegistry(baseline, snapshot);
        Need(baseline.ContentHash == oldHash && !baseline.Generals.ContainsKey(General) && first.ContentHash == second.ContentHash &&
            first.ContentHash != oldHash && first.Generals[General].SkillIds.SequenceEqual(["standard:yingzi", Draw]) &&
            ReferenceEquals(baseline, CustomGeneralContent.CreateRegistry(baseline, CustomGeneralLibrary.Empty)) &&
            ReferenceEquals(baseline, CustomGeneralContent.CreateRegistry(baseline, snapshot with { Generals = [snapshot.Generals[0] with { Enabled = false }] })),
            "Extensions preserve the base snapshot, attach custom skills automatically and keep deterministic content identity; empty/disabled libraries are exact no-ops.");
        foreach (var mode in baseline.Modes.Values.Where(m => m.ModeKind == ContentModeKind.Identity))
            Need(first.Modes[mode.Id].GeneralPoolIds?.Contains(General) ?? true, "Custom generals join the existing identity mode without replacing it.");
        Need(first.Modes[Mode].GeneralSelectionPoolIds!.Contains(General) &&
            !baseline.Modes[Mode].GeneralSelectionPoolIds!.Contains(General) &&
            JsonSerializer.Serialize(first.EquipmentPackages) == JsonSerializer.Serialize(baseline.EquipmentPackages),
            "Existing selection pools and explicit equipment definitions survive extension unchanged.");
        var probe = first.ModeFeatureFactories.Single(f => f.Id == ProbeFactory.IdValue);
        Need(probe.Create(new ModeFeatureCreationContext(19, first, _ => null)) is ProbeFeature { Seed: 19 } && factory.Calls == 1,
            "An imported executable factory retains its original implementation and frozen mode metadata.");
        var copied = baseline.WithExtension(new EmptyExtension());
        var rebuilt = ContentRegistry.Build(new StandardContentPackage(), new Fixture(new ProbeFactory()), new EmptyExtension());
        Need(copied.ContentHash == rebuilt.ContentHash && copied.ContentHash != oldHash,
            "Extending the immutable snapshot has exactly the same content hash as fresh package registration, including original skill/equipment package ownership.");

        foreach (var trigger in Enum.GetValues<CustomSkillTrigger>())
        {
            var compiled = CustomGeneralContent.CompileSkill(new()
            {
                Id = Draw, Name = "组合", Trigger = trigger, Target = (CustomSkillTarget)((int)trigger % 3),
                Condition = (CustomSkillCondition)((int)trigger % 4), UsesPerTurn = 2,
                Effects = Enum.GetValues<CustomSkillEffectKind>().Select(kind => new CustomSkillEffect { Kind = kind, Amount = 1, DamageNature = DamageNature.Fire }).ToArray()
            });
            Need(compiled.Program is not null && compiled.Description.Contains("每回合限2次", StringComparison.Ordinal),
                "Every exposed visual template compiles through the real bounded skill loader and describes its cap.");
        }
        foreach (var condition in Enum.GetValues<CustomSkillCondition>())
        foreach (var trigger in new[] { CustomSkillTrigger.Active, CustomSkillTrigger.AfterDamage })
            _ = CustomGeneralContent.CompileSkill(new() { Id = Draw, Name = "条件", Trigger = trigger, Condition = condition, Effects = [new()] });
        foreach (var nature in Enum.GetValues<DamageNature>())
            _ = CustomGeneralContent.CompileSkill(new() { Id = Hurt, Name = "属性", Target = CustomSkillTarget.Other,
                Effects = [new() { Kind = CustomSkillEffectKind.Damage, DamageNature = nature }] });
        Reject(() => CustomGeneralContent.Snapshot(snapshot with { Generals = [snapshot.Generals[0] with { BaseHp = 0 }] }), "Invalid HP must fail before registry assembly.");
        Reject(() => CustomGeneralContent.CompileSkill(new() { Id = Draw, Name = "循环", UsesPerTurn = 0, Effects = [new()] }), "Unbounded custom triggering must not be admitted.");
        Reject(() => CustomGeneralContent.CreateRegistry(baseline, snapshot with { Generals = [snapshot.Generals[0] with { SkillIds = ["unknown:skill"] }] }), "Unknown printed skills must not enter a match.");
        Reject(() => CustomGeneralContent.Snapshot(snapshot with { Generals = [snapshot.Generals[0], snapshot.Generals[0]] }), "Duplicate stable general identities must be rejected.");
        Need(CustomGeneralContent.SelectableSkills(baseline).Any(s => s.Id == "standard:yingzi") &&
            CustomGeneralContent.SelectableSkills(baseline).All(s => s.Id != "standard:none"), "The authoring picker exposes real printed skills, including their original identities.");
    }

    public static void ExistingSkillAndVisualProgramsUseNativeChildrenAndColdReplay()
    {
        var baseline = ContentRegistry.Build(new StandardContentPackage(), new Fixture(new ProbeFactory()));
        var library = new CustomGeneralLibrary { Generals = [new()
        {
            Id = General, Name = "自建实战", SkillIds = ["standard:yingzi", Watch],
            CustomSkills = [
                new() { Id = Draw, Name = "摸牌", Effects = [new() { Kind = CustomSkillEffectKind.Draw, Amount = 2 }] },
                new() { Id = Hurt, Name = "自伤", Target = CustomSkillTarget.Any, Effects = [new() { Kind = CustomSkillEffectKind.Damage, DamageNature = DamageNature.Fire }] },
                new() { Id = Recover, Name = "受伤回复", Trigger = CustomSkillTrigger.AfterDamage, Optional = false, Condition = CustomSkillCondition.Wounded, Effects = [new() { Kind = CustomSkillEffectKind.Recover }] },
                new() { Id = Reward, Name = "伤害摸牌", Trigger = CustomSkillTrigger.AfterDealtDamage, Optional = false, Condition = CustomSkillCondition.HpAtMost, ConditionValue = 20, Effects = [new()] }
            ]
        }] };
        var registry = CustomGeneralContent.CreateRegistry(baseline, library);
        var scene = Scene.Start(registry);
        Need(scene.G.CreateSnapshot(0).Players[0].Hand.Count == 5,
            "The original printed Yingzi applies to an unrelated custom general ID during the real draw phase (normal draw count zero plus one).");
        var equip = scene.G.GetHumanLegalActions().First(a => a.Kind == LegalActionKind.Equip);
        scene.Submit(new PlayCardCommand(0, equip.CardId!.Value, equip.TargetSeats, scene.G.Revision, scene.P!.PromptId, equip.PlayedCardKind));
        scene.Reach(scene.AtPlay);
        Need(scene.G.GetAttackRange(0) == 6, "The extended registry actually executes its preserved explicit range-six equipment definition.");
        var hand = scene.G.CreateSnapshot(0).Players[0].Hand.Count;
        scene.Use(Draw, []);
        scene.Reach(() => scene.P?.SkillPrompt?.SkillId == Watch);
        var root = scene.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Draw);
        var rootId = root.Id;
        Need(root.InstructionIndex > 0 && scene.G.CreateSnapshot(0).Players[0].Hand.Count == hand + 2 &&
            scene.G.CardMovements.Count(m => m.Reason.Value == $"skill-program.{Draw}.Draw") == 2,
            "The custom activation pays its native two-card draw once before the existing gain observer child pauses the original frame.");
        scene.Cold();
        Need(scene.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Draw).Id == rootId,
            "Cold recovery retains the owning custom activation and its paid child.");
        scene.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        scene.Reach(scene.AtPlay);
        Need(scene.G.CardMovements.Count(m => m.Reason.Value == $"skill-program.{Draw}.Draw") == 2 &&
            scene.G.CreateSnapshot(0).Players[0].Hand.Count == hand + 2,
            "The child returns to the original paid program without repeating its draw.");
        var revision = scene.G.Revision;
        var rejected = scene.G.Submit(new UseProgramSkillCommand(0, Draw, CustomGeneralContent.ActivationId, [], [], revision, scene.P!.PromptId));
        Need(!rejected.Accepted && scene.G.Revision == revision, "The bounded active quota rejects a second use in the same real turn.");
        var hp = scene.G.CreateSnapshot(0).Players[0].Hp;
        var beforeReward = scene.G.CreateSnapshot(0).Players[0].Hand.Count;
        scene.Use(Hurt, [0]);
        scene.Reach(scene.AtPlay);
        Need(scene.G.CreateSnapshot(0).Players[0].Hp == hp && scene.G.CreateSnapshot(0).Players[0].Hand.Count == beforeReward + 1 &&
            scene.G.CardMovements.Count(m => m.Reason.Value == $"skill-program.{Reward}.Draw") == 1 &&
            scene.G.Events.Select(e => e.Payload).OfType<ProgramBindingResolvedEvent>().Count(e => e.SkillId == Recover) == 1,
            "Real self-inflicted fire damage completes both visual damage-trigger bindings: one wound-qualified recovery and one source-qualified native draw.");
        scene.Cold();
    }

    private sealed class Scene(ContentRegistry registry, GameEngine game)
    {
        public GameEngine G { get; private set; } = game;
        public PendingDecision? P => G.CreateSnapshot(0).PendingDecision;
        public bool AtPlay() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && !G.ResolutionStack.OfType<ProgramSkillFrame>().Any();
        public void Submit(GameCommand command)
        { var r = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(r.Accepted, r.Error?.Message ?? "Custom authoring command failed."); }
        public void Use(string id, int[] targets) => Submit(new UseProgramSkillCommand(0, id, CustomGeneralContent.ActivationId, [], targets, G.Revision, P!.PromptId));
        public void Answer(Func<PromptChoice, bool> predicate)
        { var p = P!; Submit(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.Single(predicate).Id, G.Revision)); }
        public void Reach(Func<bool> done)
        {
            for (var i = 0; i < 70; i++) { if (done()) return; Submit(new AdvanceOneStepCommand(G.Revision)); }
            throw new InvalidOperationException("Custom authoring fixture missed its native boundary: " + JsonSerializer.Serialize(new { G.State, P }));
        }
        public void Cold()
        {
            var views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray();
            var movements = JsonSerializer.Serialize(G.CardMovements);
            G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).SequenceEqual(views) && movements == JsonSerializer.Serialize(G.CardMovements),
                "Strict cold command replay preserves every viewer and original paid physical movements under the custom content hash.");
        }
        public static Scene Start(ContentRegistry registry)
        {
            var game = GameEngine.CreateStandard(new GameOptions { Seed = 17, PlayerCount = 4, HumanSeat = 0, HumanRole = Role.Lord, ModeId = Mode,
                UseInteractiveSetup = true, AdvanceAfterHumanCommands = false, MaxTurns = 3 }, registry);
            var scene = new Scene(registry, game);
            scene.Submit(new StartGameCommand());
            scene.Submit(new SelectGeneralCommand(0, General, game.Revision, scene.P!.PromptId));
            scene.Reach(scene.AtPlay);
            return scene;
        }
    }

    private sealed record ProbeFeature(int Seed) : IGameModeFeature;
    private sealed class EmptyExtension : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:custom-empty-extension", new(1, 0, 0), []);
        public void Register(IContentRegistryBuilder builder) { }
    }
    private sealed class ProbeFactory : IGameModeFeatureFactory
    {
        public const string IdValue = "fixture:custom-author-probe";
        public string Id => IdValue;
        public string ModeId => Mode;
        public int Calls { get; private set; }
        public IGameModeFeature Create(ModeFeatureCreationContext context) { Calls++; return new ProbeFeature(context.Seed); }
    }
    private sealed class Fixture(ProbeFactory factory) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:custom-authoring", new(1, 0, 0), []);
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = new[] { new
            {
                id = Watch, revision = 1, triggers = new[] { new { id = "paid", window = "cardsGained", subject = "owner", destinationZones = new[] { "hand" },
                    movementOccurrence = "perBatch", movementReasons = new[] { $"skill-program.{Draw}.Draw" }, usageScope = "game", usageLimit = 1, optional = false,
                    effects = new[] { new { op = "chooseOption", target = "owner", resultBind = "paid", options = new[] { new { id = "continue" } } } } } }
            } } }), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Watch] = new { name = "已摸牌", description = "支付后观察", optionLabels = new { @continue = "继续" } } } }));
            builder.AddSkill(new(Watch, "已摸牌", "支付后观察") { Program = catalog.Programs[Watch], ProgramPresentation = catalog.Presentations[Watch] });
            var peers = Enumerable.Range(1, 4).Select(i => "fixture:custom-author-peer-" + i).ToArray();
            foreach (var peer in peers) builder.AddGeneral(new(peer, "测试角色", "supporter", Watch, "wei", 5));
            builder.AddDeck(new("fixture:custom-author-deck", "玄剑牌堆", 4, 0, [new("standard:xuanjian", 80)]));
            builder.AddEquipmentContent(new("fixture:custom-author-equipment", [EquipmentCatalog.Get(CardKind.Xuanjian) with { WeaponAttackRange = 6 }]));
            builder.AddMode(new(Mode, "自建武将实战", 4, 4, new Dictionary<string, int> { ["Lord"] = 1, ["Loyalist"] = 1, ["Rebel"] = 2 },
                "fixture:custom-author-deck", GeneralCandidateCount: 8, GeneralPoolIds: peers) { GeneralSelectionPoolIds = peers });
            builder.AddModeFeatureFactory(factory);
        }
    }
    private static void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message); }
    private static void Reject(Action action, string message)
    { try { action(); } catch (ArgumentException) { return; } throw new InvalidOperationException(message); }
}
