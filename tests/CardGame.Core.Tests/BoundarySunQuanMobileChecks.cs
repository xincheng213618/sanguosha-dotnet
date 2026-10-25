using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class BoundarySunQuanMobileChecks
{
    private const string Hero = "boundary:sun-quan-mobile", Skill = "boundary:jiuyuan-mobile", Owner = "fixture:mobile-sun-quan-provider",
        Driver = "fixture:mobile-sun-quan-driver", Gain = "fixture:mobile-sun-quan-gain", Hp = "fixture:mobile-sun-quan-hp",
        Mode = "identity:classic-mobile-sun-quan";

    public static void SelfPeachStrictComparisonPaidRecoveryAndRewardCold()
    {
        var s = Scene.Start(); s.Play(); var lord = s.G.State.Players.Single(p => p.Role == Role.Lord).Seat;
        s.Need(lord != 0 && s.V(0).Hp > s.V(lord).Hp, "The actual other Wu provider and the true native Lord have distinct roles and initial HP.");
        while (s.V(0).Hp > s.V(lord).Hp) { s.Use("lose-one"); s.Play(); }
        var equal = s.V(0).Hp; var lordHp = s.V(lord).Hp;
        var first = s.V(0).Hand.First(c => c.Kind == CardKind.Peach).Id;
        s.Peach(first); s.Play();
        s.Need(s.V(0).Hp == equal + 1 && s.V(lord).Hp == lordHp && !s.E<RecoveryReplacementChosenEvent>().Any(),
            "Equal HP does not qualify mobile Jiuyuan: the actual self-Peach heals its original user, unlike the OL greater-or-equal recovery rule.");
        var providerHp = s.V(0).Hp;
        s.Use("ordinary-recovery"); s.Play();
        s.Need(s.V(0).Hp == providerHp + 1 && s.V(lord).Hp == lordHp && !s.E<RecoveryReplacementChosenEvent>().Any(),
            "A genuine own-turn self recovery without a Peach remains ordinary even while the provider has strictly more HP than the Lord.");

        providerHp = s.V(0).Hp;
        var material = s.V(0).Hand.First(c => c.Kind == CardKind.Peach).Id;
        var hand = s.V(0).HandCount;
        s.Peach(material); s.Reach(() => s.P is { Kind: DecisionKind.RecoveryReplacement, PlayerSeat: 0 });
        var recovery = s.G.ResolutionStack.OfType<RecoveryReplacementFrame>().Single();
        var use = s.G.ResolutionStack.OfType<CardUseFrame>().Single(f => f.Id == recovery.ParentFrameId);
        s.Need(recovery.Attempt is { SourceSeat: 0, TargetSeat: 0 } && recovery.Attempt.Candidates is [var candidate] &&
            candidate.SkillId == Skill && candidate.OwnerSeat == lord && candidate.RecoveryAmount == 1 && candidate.ProviderDrawCount == 1 &&
            use.Action is { Type: CardActionType.Use, EffectiveKind: CardKind.Peach, ActorSeat: 0 } && use.CardId == material &&
            s.V(0).Hp == providerHp && s.V(lord).Hp == lordHp,
            "Before any recovery, the original paid native self-Peach offers exactly its strict-HP mobile Lord candidate on the same recovery attempt.");
        var recoveryId = recovery.Id; var useId = use.Id;
        s.Answer(c => c.Parameters.GetValueOrDefault("recovery-action") == "redirect" && c.Targets.SequenceEqual([lord]));
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Hp);
        s.Need(s.V(lord).Hp == lordHp + 1 && s.V(0).Hp == providerHp && s.V(0).HandCount == hand - 1 &&
            s.E<RecoveryReplacementChosenEvent>().Single() is { SkillId: Skill, RecoveryFrameId: var chosenId, RecoveringSeat: 0, BeneficiarySeat: var beneficiary } &&
            chosenId == recoveryId && beneficiary == lord,
            "The real HP child interrupts after the Lord's one recovery and before the provider's reward, preserving the original self-Peach payment.");
        s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("advanced-value") == Skill) == true);
        s.Answer(c => c.Parameters.GetValueOrDefault("advanced-value") == Skill);
        s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("advanced-value") == "finish") == true);
        s.Answer(c => c.Parameters.GetValueOrDefault("advanced-value") == "finish");
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Gain);
        recovery = s.G.ResolutionStack.OfType<RecoveryReplacementFrame>().Single(f => f.Id == recoveryId);
        s.Need(recovery.Stage == RecoveryReplacementStage.RewardApplied && recovery.CandidateIndex == 0 &&
            !s.V(lord).Skills!.Any(k => k.ContentId == Skill) && s.V(0).HandCount == hand && s.V(0).Hp == providerHp &&
            s.G.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Any(f => f.Batch.ParentFrameId == recoveryId &&
                f.Batch.Movements.All(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(0) && m.Reason.Value == "skill-program.recovery-replacement.reward")),
            "The already-chosen native recovery survives real source skill loss, draws one for the original provider and waits on that exact reward movement child.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); s.Play();
        s.Need(s.V(lord).Hp == lordHp + 1 && s.V(0).Hp == providerHp && s.V(0).HandCount == hand &&
            s.E<RecoveryReplacementChosenEvent>().Count() == 1 && s.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == useId) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == material && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1 &&
            s.G.CardMovements.Count(m => m.Reason.Value == "skill-program.recovery-replacement.reward" && m.To == CardLocation.Hand(0)) == 1 &&
            s.G.ResolutionStack.All(f => f.Id != recoveryId && f.Id != useId),
            "Cold returns through the original recovery and paid Peach once; it neither recovers again nor draws or pays the Peach twice after losing Jiuyuan.");
    }

    private sealed class Scene(ContentRegistry registry, GameEngine game)
    {
        public GameEngine G = game;
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players[seat];
        public PendingDecision? P => Enumerable.Range(0, 5).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public IEnumerable<T> E<T>() => G.Events.Select(e => e.Payload).OfType<T>();
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Use(string id) => Accept(new UseProgramSkillCommand(0, Driver, id, [], [], G.Revision, P!.PromptId));
        public void Peach(int material)
        {
            var action = G.GetLegalActions(0).First(a => a.Kind == LegalActionKind.Peach && a.CardId == material);
            Accept(new PlayCardCommand(0, material, action.TargetSeats, G.Revision, P!.PromptId, action.PlayedCardKind, action.TargetCardId)
                { ConversionSource = action.ConversionSource, AdditionalConversionSources = action.AdditionalConversionSources });
        }
        public void Answer(Func<PromptChoice, bool> predicate) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(predicate).Id, G.Revision)); }
        public void Accept(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native mobile Sun Quan command."); }
        public void Reach(Func<bool> goal)
        {
            for (var step = 0; step < 240; step++)
            {
                if (goal()) return;
                if (P is { Kind: DecisionKind.SelectGeneral } choose) { Accept(new SelectGeneralCommand(choose.PlayerSeat, choose.PlayerSeat == 0 ? Owner : Peer(choose.PlayerSeat), G.Revision, choose.PromptId)); continue; }
                if (P is { Kind: DecisionKind.PlayCard, PlayerSeat: not 0 } foreign) { Accept(new EndPlayPhaseCommand(foreign.PlayerSeat, G.Revision, foreign.PromptId)); continue; }
                Need(P is not { Kind: DecisionKind.ProgramTrigger or DecisionKind.RecoveryReplacement or DecisionKind.PlayCard }, "Unexpected native Sun Quan boundary.");
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("The mobile Sun Quan fixture missed its bounded native boundary.");
        }
        public void Cold()
        {
            var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(before == Dump(), "Cold preserves the exact paid replacement, lost source, native self-Peach and reward movement child.");
        }
        private string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 5).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(), Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements });
        public void Need(bool yes, string why)
        { if (!yes) throw new InvalidOperationException(why + " " + JsonSerializer.Serialize(new { Pending = P?.Kind, Seat = P?.PlayerSeat, Skill = P?.SkillPrompt?.SkillId, G.State.CurrentSeat, G.State.Phase, Players = Enumerable.Range(0, 5).Select(i => new { Seat = i, V(i).Hp, V(i).HandCount }) })); }
        public static Scene Start()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var game = GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = 5, HumanSeat = 0, HumanSeats = [0, 1, 2, 3, 4], HumanRole = Role.Renegade,
                ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8 }, registry);
            var scene = new Scene(registry, game); scene.Accept(new StartGameCommand()); return scene;
        }
    }
    private static string Peer(int seat) => "fixture:mobile-sun-quan-lord-" + seat;
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-sun-quan-mobile", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero, "boundary:sun-quan");
            object Activation(string id, object effect) => new { id, minCards = 0, maxCards = 0, minTargets = 0, maxTargets = 0,
                targetKind = "anyLiving", usesPerTurn = (int?)null, effects = new[] { effect } };
            var skills = new object[] {
                new { id = Driver, revision = 1, activations = new[] {
                    Activation("lose-one", new { op = "loseHp", target = "owner", amount = 1 }),
                    Activation("ordinary-recovery", new { op = "recover", target = "owner", amount = 1 }) } },
                new { id = Hp, revision = 1, triggers = new[] { new { id = "lord-paid-recovery", window = "afterHpRecovered", subject = "owner", optional = false,
                    effects = new object[] { new { op = "chooseOption", target = "owner", resultBind = "pause", options = new[] { new { id = "continue" } } },
                        new { op = "replaceSkillsOnAwakening", target = "owner", skillIds = new[] { "standard:none" } } } } } },
                new { id = Gain, revision = 1, triggers = new[] { new { id = "provider-reward", window = "cardsGained", subject = "owner", optional = false,
                    destinationZones = new[] { "hand" }, movementOccurrence = "perBatch", movementReasons = new[] { "skill-program.recovery-replacement.reward" },
                    effects = new[] { new { op = "chooseOption", target = "owner", resultBind = "pause", options = new[] { new { id = "continue" } } } } } } }
            };
            var labels = new Dictionary<string, object> {
                [Driver] = new { name = "原生体力驱动", description = "真实失体力及非桃回复" },
                [Hp] = new { name = "真实主公回复子窗", description = "回复后暂停并真实失去来源", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } },
                [Gain] = new { name = "真实救援摸牌子窗", description = "原生奖赏移动后暂停", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } }
            };
            var catalog = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in catalog.Programs) builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Owner, "真实其他吴角色", "supporter", Driver, "wu", 6, AdditionalSkillIds: [Gain]) { InitialHp = 4 });
            var peers = Enumerable.Range(1, 4).Select(Peer).ToArray();
            var replacements = Enumerable.Range(0, 6).Select(i => "fixture:mobile-sun-quan-dummy-" + i).ToArray();
            foreach (var id in replacements) builder.AddSkill(new(id, "真实原生技能替换资格", "令原生替换能力达到技能数资格。"));
            foreach (var peer in peers) builder.AddGeneral(new(peer, "真实移动版主公候选", "supporter", Skill, "wu", 4, AdditionalSkillIds: [Hp, .. replacements]) { InitialHp = 2 });
            const string deck = "fixture:mobile-sun-quan-deck";
            builder.AddDeck(new(deck, "固定真实桃实体", 4, 2, []) { PhysicalCards = Enumerable.Range(0, 100).Select(_ => new ContentDeckPhysicalCard("standard:peach", Suit.Heart, 7)).ToArray() });
            builder.AddMode(new(Mode, "移动界孙权原生流程", 5, 5, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2, [nameof(Role.Renegade)] = 1 },
                deck, GeneralCandidateCount: 5, GeneralPoolIds: [Owner, .. peers]));
        }
    }
}
