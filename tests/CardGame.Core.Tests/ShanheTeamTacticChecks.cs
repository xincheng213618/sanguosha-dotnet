using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheTeamTacticChecks
{
    private const string Jiban = "shanhe:jiban-2026", Duichu = "shanhe:duichu-2026", Duipai = "shanhe:duipai-2026";
    private const string Owner = "fixture:team-tactic-owner", Driver = "fixture:team-tactic-driver";
    private const string Giver = "fixture:team-tactic-giver", Quiet = "fixture:team-tactic-quiet";
    private const string Weishu = "ol:weishu", WeishuWatch = "fixture:team-tactic-weishu-child-watch";
    private const string Zhiyi = "ol:zhiyi", GainInstruction = "fixture:team-tactic-gained-draw";
    private const string Ally = "fixture:team-tactic-ally", Other = "fixture:team-tactic-other", Enemy = "fixture:team-tactic-enemy";
    private const string Deck = "fixture:team-tactic-deck", IdentityDeck = "fixture:team-tactic-identity-deck";
    private const string IdentityMode = "identity:classic-team-tactic-fixture";
    private const string AuraBoundary = "fixture:team-aura-boundary", AuraGain = "fixture:team-aura-gain", AuraLoss = "fixture:team-aura-loss";
    private const string AuraNativeCapability = "fixture:team-aura-native-capability";
    private static readonly Lazy<ContentSkillDefinition> ActualDuipai = new(() => StandardShanheCampaignCatalog.CreateBattleRegistry(
        new(43, 1, "fixture-team-aura-program", "classic:guan-yu", 4, 0, 0, 0, 0, [], [], [],
            [new("standard:cao-cao")], [], new string('e', 64))).GetSkill(Duipai));
    private static readonly ShanheStartingCard[] FenghuangFaces =
    [new(CardKind.Slash, Suit.Heart, 5), new(CardKind.Dodge, Suit.Heart, 13),
        new(CardKind.ArrowBarrage, Suit.Heart, 1), new(CardKind.Alcohol, Suit.Spade, 3)];

    public static void TeamDrawAuraRechecksBeforeIssueAndKeepsIssuedRequestsFrozenCold()
    {
        foreach (var issued in new[] { false, true })
        foreach (var lose in new[] { false, true })
        {
            var scene = "aura-" + (issued ? "issued-" : "before-") + (lose ? "loss" : "death");
            var s = Scenario.Create(scene); s.StartGame();
            s.Reach(() => s.P is { PlayerSeat: 0 } && s.P.SkillPrompt?.SkillId == AuraBoundary);
            var phase = s.G.ResolutionStack.OfType<DrawPhaseObligationFrame>().Single();
            var attempt = s.G.ResolutionStack.OfType<NativeDrawAttemptFrame>().SingleOrDefault();
            s.Need(s.V(1).IsAlive && s.V(1).Skills!.Any(skill => skill.ContentId == Duipai) && s.V(0).HandCount == 0 &&
                (issued ? phase is { Stage: DrawPhaseObligationStage.RequestingDraw, NativeDraw.RequestedCount: 4 } &&
                    attempt is { Context.RequestedCount: 4, Return.Kind: NativeDrawAttemptReturnKind.DrawPhase } &&
                    attempt.Context.ParentFrameId == phase.Id
                    : phase is { Stage: DrawPhaseObligationStage.Drawing, NativeDraw: null } && attempt is null),
                "The real pre-phase window precedes request capture, whereas nativeDrawStarting owns an already-issued four-card request before any physical card moves.");
            var phaseId = phase.Id;
            s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "proceed");
            s.Reach(() => s.P is { PlayerSeat: 0 } && s.P.SkillPrompt?.SkillId == AuraBoundary && s.P.Choices.Any(c => c.Targets.SequenceEqual([1])));
            s.Cold(); s.Answer(c => c.Targets.SequenceEqual([1]));
            s.Reach(() => s.P is { PlayerSeat: 0 } && s.P.SkillPrompt?.SkillId == AuraGain);
            var requested = issued ? 4 : 2;
            var gainFrame = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == AuraGain);
            var gainContext = gainFrame.WindowContext;
            var gainBatch = gainContext?.MovementBatch;
            var proof = gainBatch?.NativeDrawInvocation;
            if (proof is null) s.Need(false, "The genuine gained-card stop must belong to the final batch of the actual normal Draw invocation. " +
                JsonSerializer.Serialize(new
                {
                    scene, issued, lose, requested, gainFrame.Id, gainFrame.SkillId, gainFrame.TriggerId, gainFrame.InstructionIndex,
                    Window = gainContext?.Window,
                    Batch = gainBatch is null ? null : new { gainBatch.Id, gainBatch.ParentFrameId, gainBatch.Movements, gainBatch.SourceCounts, gainBatch.DestinationCounts },
                    Source = new { s.V(1).IsAlive, s.V(1).Hp, s.V(1).Armor, Skills = s.V(1).Skills?.Select(skill => skill.ContentId) },
                    phaseId, Hand = s.V(0).HandCount, Draws = s.Events<NativeDrawInvocationRecordedEvent>()
                        .Select(draw => new { draw.InvocationId, draw.Requested, draw.ActualCount, draw.LastBatchId, draw.ParentFrameId }),
                    Gains = s.G.CardMovements.Where(movement => movement.To.Zone == CardZoneKind.Hand)
                }));
            var actualProof = proof ?? throw new InvalidOperationException("The missing actual draw invoice must fail at the detailed native-batch diagnostic above.");
            var sourceInactive = lose ? s.V(1).IsAlive && s.V(1).Hp == 1 && s.V(1).Skills?.Any(skill => skill.ContentId == Duipai) != true : !s.V(1).IsAlive;
            var exactRequested = actualProof.OwnerSeat == 0 && actualProof.ActualCount == requested && actualProof.RequestedCount == requested;
            var exactParent = actualProof.ParentFrameId == phaseId && actualProof.Reason == CardMoveReasons.Draw;
            var exactMaterials = s.V(0).HandCount == requested && PhaseDraws(s, 0).Length == requested;
            var exactDamage = s.Events<DamageAppliedEvent>().Count(e => e.TargetSeat == 1 && e.Amount == 1) == 1;
            if (!(sourceInactive && exactRequested && exactParent && exactMaterials && exactDamage)) s.Need(false,
                "An actual one-damage death or legal dying-entry recovery/skill replacement disables the aura: unissued draw becomes two, while the already-issued four remains four through genuine CardsGained children. " +
                JsonSerializer.Serialize(new
                {
                    scene, issued, lose, requested, sourceInactive, exactRequested, exactParent, exactMaterials, exactDamage,
                    Source = new { s.V(1).IsAlive, s.V(1).Hp, s.V(1).Armor, Skills = s.V(1).Skills?.Select(skill => skill.ContentId) },
                    Proof = new { actualProof.OwnerSeat, actualProof.RequestedCount, actualProof.ActualCount, actualProof.ParentFrameId, actualProof.Reason, actualProof.InvocationId },
                    phaseId, Hand = s.V(0).HandCount, PhaseMaterials = PhaseDraws(s, 0).Length,
                    Phases = s.G.ResolutionStack.OfType<DrawPhaseObligationFrame>().Select(frame => new { frame.Id, frame.Stage, frame.FrozenBaseDrawCount, frame.DrawCount, Requested = frame.NativeDraw?.RequestedCount }),
                    Damage = s.Events<DamageAppliedEvent>(), Recovery = s.Events<ProgramRecoveryAppliedEvent>(),
                    Replaced = s.Events<ProgramOwnerSkillsReplacedEvent>()
                }));
            var invocationId = actualProof.InvocationId;
            s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); s.Play();
            s.Need(s.Events<NativeDrawInvocationRecordedEvent>().Count(e => e.InvocationId == invocationId && e.ActualCount == requested && e.Requested == requested) == 1 &&
                s.Events<ActualDrawPhaseCompletedEvent>().Single(e => e.FrameId == phaseId).DrawCount == requested,
                "The owning real draw phase, exact native invoice and actual material count settle once after cold child recovery.");
            s.Cold(); s.EndPlay(); s.Play();
            s.Need(PhaseDraws(s, 0).Length == requested + 2 && s.V(0).HandCount == requested + 2 &&
                s.Events<DamageAppliedEvent>().Count(e => e.TargetSeat == 1) == 1,
                "The next real normal draw rechecks the dead or lost holder and returns to two without repeating the previous mutation or issued payment.");
            s.Conserved(); s.Cold();
        }
    }

    private static CardMovementRecord[] PhaseDraws(Scenario s, int seat) => s.G.CardMovements.Where(m =>
        m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(seat) && m.Reason == CardMoveReasons.Draw).ToArray();

    private sealed class Scenario(GameEngine game, ContentRegistry registry, bool interactive, string player)
    {
        public GameEngine G { get; private set; } = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(seat => G.CreateSnapshot(seat).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] Events<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public void Need(bool condition, string why) { if (!condition) throw new InvalidOperationException(why); }
        public void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Team-tactic command rejected."); }
        public void Answer(Func<PromptChoice, bool> choose)
        { var p = P!; Need(p.PlayerSeat == 0, "NPC choices must use native AI commands."); Accept(new AnswerPromptCommand(0, p.PromptId, p.Choices.First(choose).Id, G.Revision)); }
        public void StartGame()
        {
            Accept(new StartGameCommand());
            if (!interactive) return;
            Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.SelectGeneral });
            Accept(new SelectGeneralCommand(0, player, G.Revision, P!.PromptId));
        }
        public bool CanSlash() => G.GetHumanLegalActions().Any(a => a.Kind == LegalActionKind.Slash && a.TargetSeats.SequenceEqual([3]));
        public int Debits() => Events<CardUseDebitRecordedEvent>().Count(e => e.Debit.ActorSeat == 0 && e.Debit.Query == SkillRuleQuery.SlashLimit);
        public void Slash(bool converted = false)
        {
            var a = G.GetHumanLegalActions().First(a => a.Kind == LegalActionKind.Slash && a.TargetSeats.SequenceEqual([3]) &&
                (converted ? a.ConversionSource?.SkillId == "classic:wusheng" && V(0).Hand.Any(c => c.Id == a.CardId && c.Kind == CardKind.Dodge) : a.ConversionSource is null));
            Accept(new PlayCardCommand(0, a.CardId!.Value, a.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind, a.TargetCardId)
            { ConversionSource = a.ConversionSource }); Play();
        }
        public void Use(string activation, int[]? targets = null)
        { Accept(new UseProgramSkillCommand(0, Driver, activation, [], targets ?? [], G.Revision, P!.PromptId)); Play(); }
        public void EndPlay() => Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Reach(Func<bool> ready)
        {
            for (var step = 0; step < 220; step++)
            {
                if (ready()) return;
                Need(G.State.Winner == Winner.None, "Team-tactic fixture ended before its actual boundary.");
                if (P is { PlayerSeat: 0 } human)
                {
                    var choice = human.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("response") is "take-damage" or "pass" or "let-die" ||
                        c.Parameters.GetValueOrDefault("program-action") == "skip" || c.Parameters.GetValueOrDefault("branch") == "decline" ||
                        human.SkillPrompt?.SkillId == "ol:fudao" &&
                        c.Parameters.GetValueOrDefault("program-action") == "ding-shang-wan" &&
                        c.Parameters.GetValueOrDefault("branch") == "finish");
                    Need(choice is not null, "Unexpected native human team-tactic boundary: " + JsonSerializer.Serialize(human));
                    Answer(c => c.Id == choice!.Id);
                }
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Missing bounded team-tactic boundary: " + JsonSerializer.Serialize(P));
        }
        public void Conserved()
        { var zones = G.CreateCardZoneDiagnostics(); Need(zones.Select(c => c.CardId).Distinct().Count() == zones.Count, "Every actual physical card has exactly one zone."); }
        private string State() => JsonSerializer.Serialize(new
        {
            Views = Enumerable.Range(0, 4).Select(seat => SnapshotJson.Serialize(G.CreateSnapshot(seat))),
            Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())),
            Facts = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())), G.CardMovements,
            Zones = G.CreateCardZoneDiagnostics(), Checkpoint = GameCheckpointJson.Serialize(G.CreateCheckpoint())
        });
        public void Cold()
        { var before = State(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry); Need(State() == before, "Cold replay retains native debits, queued gain choices, physical faces and paid child movements exactly once."); }
        public static Scenario Create(string scene, int copies = 1, bool publicTeams = true)
        {
            var aura = scene.StartsWith("aura-", StringComparison.Ordinal);
            var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardActiveSkillExpansionPackage(true),
                new StandardRescueSkillExpansionPackage(), new StandardClassicGeneralPackage(), new StandardShanheBossPackage(), new Fixture(scene));
            var hongde = scene is "hongde" or "hongde-weishu";
            var player = hongde ? "ol:bu-zhi" : scene == "zhiyi-weishu" ? "ol:tang-tang" :
                scene is "human-fenghuang" or "fudao-gift-weishu" or "fudao-draw-weishu" ? StandardShanheBossPackage.FenghuangId : Owner;
            GameOptions options;
            if (publicTeams)
            {
                var allies = scene switch
                {
                    "fenghuang" => new ShanheCombatantDefinition[] { new(StandardShanheBossPackage.FenghuangId) { StartingHand = FenghuangFaces }, new(Ally) },
                    "hongde" or "hongde-weishu" or "weishu" or "compiled-gain-weishu" or "zhiyi-weishu" => [new(Giver), new(Ally)],
                    "human-fenghuang" => [new(Giver), new(Ally)],
                    "fudao-gift-weishu" => [new(Ally)],
                    "fudao-draw-weishu" => [new(Ally), new(Other)],
                    _ => [new(Ally, Hp: aura && scene != "aura-stacked" ? 1 : null), new(Other, Hp: aura ? null : 1)]
                };
                ShanheStartingCard[] opening = scene == "quota" ? Enumerable.Range(0, 6).Select(i => new ShanheStartingCard(CardKind.Slash, Suit.Spade, i + 1))
                    .Append(new(CardKind.Dodge, Suit.Heart, 5)).ToArray() : scene == "human-fenghuang" ? FenghuangFaces :
                    scene == "zhiyi-weishu" ? [new(CardKind.Slash, Suit.Club, 9)] : [];
                ShanheCombatantDefinition[] enemies = scene == "fudao-gift-weishu" ? [new("ol:bu-zhi"), new(Enemy)] : [new(Enemy, MaxHpBonus: 20)];
                var setup = new ShanheBattleSetup(23, 1, "fixture-team-" + scene, player, hongde || scene == "zhiyi-weishu" ? 3 :
                    scene is "human-fenghuang" or "fudao-gift-weishu" or "fudao-draw-weishu" ? 4 : 12,
                    0, 0, 0, 0, hongde ? [Driver] : scene == "fudao-draw-weishu" ? [Weishu, WeishuWatch] : [], opening, [], enemies, allies, new string('d', 64))
                { TacticSkillIds = scene == "fenghuang" && copies == 2 ? [Duichu] : scene == "aura-stacked" ? [Duipai, Duipai] : [] };
                options = new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                    HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                    UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8 };
            }
            else options = new GameOptions { Seed = 23, PlayerCount = 4, HumanSeat = 0, HumanRole = Role.Lord,
                ModeId = IdentityMode, UseInteractiveSetup = true, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 8 };
            return new(GameEngine.CreateStandard(options, registry), registry, !publicTeams, player);
        }
    }

    private sealed class Fixture(string scene) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shanhe-team-tactics", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            var aura = scene.StartsWith("aura-", StringComparison.Ordinal);
            static string Resource(string suffix)
            {
                var assembly = typeof(StandardContentPackage).Assembly;
                using var stream = assembly.GetManifestResourceStream(assembly.GetManifestResourceNames().Single(n => n.EndsWith("." + suffix, StringComparison.Ordinal)))!;
                using var reader = new StreamReader(stream); return reader.ReadToEnd();
            }
            var production = SkillProgramCatalog.Load(Resource("shanhe-team-2026.rules.json"), Resource("shanhe-team-2026.presentation.json"));
            foreach (var (id, program) in production.Programs)
                b.AddSkill(new(id, production.Presentations[id].Name, production.Presentations[id].Description)
                { Program = program, ProgramPresentation = production.Presentations[id] });
            if (aura) b.AddSkill(ActualDuipai.Value);
            var rules = $$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
              {"id":"{{Driver}}","revision":1,"activations":[
                {"id":"draw-one","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"draw","target":"selectedTarget","amount":1}]},
                {"id":"kill","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":20}]},
                {"id":"lose-jiban","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"loseOwnerSkillsAndGrant","target":"owner","skillIds":["{{Jiban}}"],"sourceBind":"standard:none"}]}]},
              {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]},
              {"id":"{{WeishuWatch}}","revision":1,"triggers":[{"id":"real-weishu-startup-child","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["skill-program.ol:weishu.outside-phase-draw"],"optional":false,"usageScope":"game","usageLimit":1,"effects":[{"op":"chooseOption","target":"owner","resultBind":"resumed","options":[{"id":"continue"}]}]}]},
              {"id":"{{GainInstruction}}","revision":1,"triggers":[{"id":"startup-gain-draw","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["skill-program.{{Duichu}}.Draw"],"optional":false,"usageScope":"game","usageLimit":1,"condition":{"kind":"compare","left":{"kind":"destinationZoneCountAfter"},"operator":"equal","right":{"kind":"integerConstant","value":2} },"effects":[{"op":"selectTarget","target":"owner","targetKind":"otherLiving"},{"op":"draw","target":"selectedTarget","amount":1}]}]}
            ]}
            """;
            var catalog = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new[] { Driver, Quiet, WeishuWatch, GainInstruction }.ToDictionary(id => id, id => new
                {
                    name = "真实队友战法检查", description = "原生技能失去、伤害及独立队伍",
                    optionLabels = id == WeishuWatch ? new Dictionary<string, string> { ["continue"] = "继续卫戍真实摸牌子流程" } : []
                })
            }));
            foreach (var (id, program) in catalog.Programs)
                b.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            if (aura) RegisterAuraBoundaries(b, scene);
            b.AddGeneral(new(Owner, "队友战法持有者", "", Driver, "wu", 12,
                aura ? scene == "aura-unknown" ? [Duipai] : scene == "aura-stacked" ? [] : [AuraBoundary, AuraGain] :
                scene == "quota" ? [Jiban, "classic:wusheng"] : scene == "weishu" ? [Weishu, WeishuWatch] : scene == "compiled-gain-weishu" ? [GainInstruction] : [Duichu]));
            b.AddGeneral(new(Giver, "队初持有者", "", Duichu, "wu", 12,
                scene is "hongde-weishu" or "compiled-gain-weishu" or "zhiyi-weishu" ? [Weishu, WeishuWatch] : []));
            foreach (var id in new[] { Ally, Other, Enemy })
                b.AddGeneral(new(id, "同桌角色", "", Quiet, "wu", 12,
                    aura && scene is not "aura-stacked" and not "aura-unknown" && id == Ally
                        ? scene.EndsWith("loss", StringComparison.Ordinal) ? [Duipai, AuraLoss] : [Duipai] :
                    scene == "fudao-gift-weishu" && id == Enemy ? [Weishu, WeishuWatch] : []));
            foreach (var (id, initial) in new[] { (Deck, 0), (IdentityDeck, scene == "quota" ? 4 : 0) })
                b.AddDeck(new(id, "真实固定牌堆", initial, aura ? 2 : 0, [])
                { PhysicalCards = Enumerable.Range(0, 120).Select(i => new ContentDeckPhysicalCard("standard:slash", Suit.Club, i % 13 + 1)).ToArray() });
            string[] pool = scene switch
            {
                "fenghuang" => [Owner, StandardShanheBossPackage.FenghuangId, Ally, Enemy],
                "hongde" or "hongde-weishu" => ["ol:bu-zhi", Giver, Ally, Enemy],
                "weishu" or "compiled-gain-weishu" => [Owner, Giver, Ally, Enemy],
                "zhiyi-weishu" => ["ol:tang-tang", Giver, Ally, Enemy],
                "human-fenghuang" => [StandardShanheBossPackage.FenghuangId, Giver, Ally, Enemy],
                "fudao-gift-weishu" => [StandardShanheBossPackage.FenghuangId, Ally, "ol:bu-zhi", Enemy],
                "fudao-draw-weishu" => [StandardShanheBossPackage.FenghuangId, Ally, Other, Enemy],
                _ => [Owner, Ally, Other, Enemy]
            };
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "真实队友战法", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: pool, ModeKind: ContentModeKind.Team, TeamCounts: new Dictionary<string, int>
                { [ShanheBattleSetup.PlayerTeamId] = scene == "fudao-gift-weishu" ? 2 : 3,
                    [ShanheBattleSetup.EnemyTeamId] = scene == "fudao-gift-weishu" ? 2 : 1 }));
            b.AddMode(new(IdentityMode, "未知公开队伍", 4, 4,
                new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 },
                IdentityDeck, GeneralCandidateCount: 4, GeneralPoolIds: pool));
        }

        private static void RegisterAuraBoundaries(IContentRegistryBuilder builder, string scene)
        {
            var window = scene.StartsWith("aura-issued-", StringComparison.Ordinal) ? "nativeDrawStarting" : "drawPhaseStarting";
            var actualRequested = scene.StartsWith("aura-issued-", StringComparison.Ordinal) ? 4 : 2;
            var rules = $$$"""
            {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
              {"id":"{{{AuraBoundary}}}","revision":1,"triggers":[{"id":"mutate-source-once","window":"{{{window}}}","subject":"owner","optional":false,"usageScope":"game","usageLimit":1,"effects":[{"op":"chooseOption","target":"owner","resultBind":"proceed","options":[{"id":"proceed"}]},{"op":"selectTarget","target":"owner","targetKind":"otherLiving"},{"op":"damage","target":"selectedTarget","amount":1}]}]},
              {"id":"{{{AuraGain}}}","revision":1,"triggers":[{"id":"real-phase-materials","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["rule.draw"],"optional":false,"usageScope":"game","usageLimit":1,"condition":{"kind":"compare","left":{"kind":"destinationZoneCountAfter"},"operator":"equal","right":{"kind":"integerConstant","value":{{{actualRequested}}}}},"effects":[{"op":"chooseOption","target":"owner","resultBind":"resumed","options":[{"id":"continue"}]}]}]},
              {"id":"{{{AuraLoss}}}","revision":1,"triggers":[{"id":"real-dying-source-loss","window":"dyingEntering","subject":"owner","optional":false,"usageScope":"game","usageLimit":1,"effects":[{"op":"recover","target":"owner","amount":1},{"op":"loseOwnerSkillsAndGrant","target":"owner","skillIds":["{{{Duipai}}}"],"sourceBind":"standard:none"}]}]},
              {"id":"{{{AuraNativeCapability}}}","revision":1,"triggers":[{"id":"unowned-native-capability","window":"nativeDrawStarting","subject":"owner","optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"unused","options":[{"id":"continue"}]}]}]}
            ]}
            """;
            var catalog = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new[] { AuraBoundary, AuraGain, AuraLoss, AuraNativeCapability }.ToDictionary(id => id, id => new
                {
                    name = "真实队牌边界", description = "真实摸牌请求与伤害失技检查",
                    optionLabels = id == AuraBoundary ? new Dictionary<string, string> { ["proceed"] = "继续真实伤害" } :
                        id is AuraGain or AuraNativeCapability ? new Dictionary<string, string> { ["continue"] = "完成真实摸牌子流程" } : []
                })
            }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
        }
    }
}
