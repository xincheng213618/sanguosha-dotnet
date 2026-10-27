using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheDamageReplacementChecks
{
    private const string Driver = "fixture:shanhe-replacement-driver", Quiet = "fixture:shanhe-replacement-quiet";
    private const string Layer = "fixture:shanhe-card-layer", Absolute = "fixture:shanhe-absolute";
    private const string Owner = "fixture:shanhe-replacement-owner", Enemy = "fixture:shanhe-replacement-enemy";
    private const string DeathNotice = "fixture:shanhe-replacement-death-notice", Deck = "fixture:shanhe-replacement-deck";
    private const string RecoveryPause = "fixture:shanhe-recovery-counter-pause";

    public static void PaidDyingPeachRecoveryStartsBloodBladeAndReturnsToOriginalDyingCold()
    {
        var s = Scenario.Start(rescue: true);
        var peach = s.V().Hand.Single(c => c.Kind == CardKind.Peach).Id;
        var enemyHp = s.G.CreateSnapshot(0).Players.Skip(1).Sum(p => p.Hp);
        s.Accept(new UseProgramSkillCommand(0, Driver, "hurt-twelve", [], [], s.G.Revision, s.G.PendingDecision!.PromptId));
        s.Reach(() => s.G.PendingDecision is { PlayerSeat: 0, Kind: DecisionKind.RescueDying });
        s.Need(s.V().Hp == 0 && s.Events<PlayerDyingEvent>().Single().VictimSeat == 0,
            "The owner must enter a genuine dying window through actual twelve-point program damage.");
        s.Cold();
        var prompt = s.G.PendingDecision!;
        var choice = prompt.Choices.Single(c => c.Parameters.GetValueOrDefault("response") == "peach" && c.Cards.Contains(peach));
        s.Accept(new AnswerPromptCommand(0, prompt.PromptId, choice.Id, s.G.Revision));
        s.ReachPlay();
        s.Need(s.ColdRecoveredDyingAttack,
            "The fixture must cold-restore while blood blade owns a real attack above its paid native Peach and original dying token.");
        s.Need(s.V().IsAlive && s.V().Hp == 1 && s.G.CreateSnapshot(0).Players.Skip(1).Sum(p => p.Hp) == enemyHp - 1 &&
            s.Events<ProgramBindingStartedEvent>().Count(e => e.SkillId == "shanhe:xueren") == 1 &&
            s.Events<ProgramBindingStartedEvent>().Count(e => e.SkillId == "shanhe:shixue-1") == 1 &&
            s.Events<DyingResolvedEvent>().Single() is { VictimSeat: 0, Survived: true },
            "A real paid rescue must trigger exactly one blood-blade counter and recovery draw, then return to the original dying token.");
        s.Need(s.Events<DamageAppliedEvent>().Count(e => e.SourceSeat == 0 && e.TargetSeat != 0 && e.Amount == 1) == 1 &&
            s.Events<NativeCardRecoveryAppliedEvent>().Single().Receipt is { HpBefore: 0, HpAfter: 1, CardKind: CardKind.Peach } &&
            s.G.CardMovements.Count(m => m.CardId == peach && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == peach && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile) == 1,
            "The nested counter uses a native recovery receipt and never replays the Peach's physical cost.");
        s.Cold();
    }

    private sealed class Scenario(GameEngine game, ContentRegistry registry)
    {
        private bool _traceRecoveredDyingAttack;
        private bool _traceNativePeachAttack;
        private bool _traceProgramBenefitAttack;
        private bool _traceMultipleRecoveryInvoices;
        public bool ColdRecoveredDyingAttack { get; private set; }
        public bool ColdNativePeachAttack { get; private set; }
        public bool ColdProgramBenefitAttack { get; private set; }
        public bool ColdMultipleRecoveryInvoices { get; private set; }
        public GameEngine G { get; private set; } = game;
        public PlayerSnapshot V(int seat = 0) => G.CreateSnapshot(0).Players[seat];
        public T[] Events<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message); }
        public void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.ToString() ?? "Replacement command rejected."); }
        public void Use(string id, int[]? cards = null, int[]? targets = null)
        {
            Accept(new UseProgramSkillCommand(0, Driver, id, cards ?? [], targets ?? [], G.Revision, G.PendingDecision!.PromptId));
            ReachPlay();
        }
        public void Play(CardKind kind, int? target = null) => BeginAndSettle(V().Hand.First(c => c.Kind == kind).Id, target);
        private void BeginAndSettle(int id, int? target) { BeginPlay(id, target); ReachPlay(); }
        public void BeginPlay(int id, int? target)
        {
            var action = G.GetHumanLegalActions().First(a => a.CardId == id && (target is null || a.TargetSeats.SequenceEqual([target.Value])));
            Accept(new PlayCardCommand(0, id, action.TargetSeats, G.Revision, G.PendingDecision!.PromptId,
                action.PlayedCardKind, action.TargetCardId) { ConversionSource = action.ConversionSource });
        }
        public void EndTurn()
        { Accept(new EndPlayPhaseCommand(0, G.Revision, G.PendingDecision!.PromptId)); ReachPlay(); }
        public void ReachPlay() => Reach(() => G.PendingDecision is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Reach(Func<bool> done)
        {
            for (var step = 0; step < 240; step++)
            {
                if (done()) return;
                if (_traceRecoveredDyingAttack && !ColdRecoveredDyingAttack &&
                    G.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.SkillId == "shanhe:xueren" &&
                        f.AttackAttempt is not null && f.AttackReturn?.RecoveryOrigin?.SuspendedDyingFrameId is not null) &&
                    G.ResolutionStack.OfType<CardUseFrame>().Any(f => f.RecoveryApplication is not null))
                {
                    Cold();
                    ColdRecoveredDyingAttack = true;
                }
                if (_traceNativePeachAttack && !ColdNativePeachAttack &&
                    G.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.SkillId == "shanhe:xueren" &&
                        f.AttackAttempt is not null && f.AttackReturn?.RecoveryOrigin is not null) &&
                    G.ResolutionStack.OfType<CardUseFrame>().Any(f => f.HulaoPeachOrigin is not null && f.RecoveryApplication is not null))
                {
                    Cold();
                    ColdNativePeachAttack = true;
                }
                if (_traceProgramBenefitAttack && !ColdProgramBenefitAttack &&
                    G.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.SkillId == "classic:huituo" && f.RecoveryApplications?.Count > 0) &&
                    G.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.SkillId == "shanhe:xueren" && f.AttackAttempt is not null))
                { Cold(); ColdProgramBenefitAttack = true; }
                if (_traceMultipleRecoveryInvoices && !ColdMultipleRecoveryInvoices &&
                    G.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.SkillId == Driver && f.RecoveryApplications?.Count == 2))
                { Cold(); ColdMultipleRecoveryInvoices = true; }
                if (G.ResolutionStack.Any(f => f is DeathFrame)) Cold();
                if (G.PendingDecision is { PlayerSeat: 0, Choices.Count: > 0 } choice)
                    Accept(new AnswerPromptCommand(0, choice.PromptId, choice.Choices[0].Id, G.Revision));
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("No bounded replacement play boundary.");
        }
        private string State() => JsonSerializer.Serialize(new
        {
            Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))),
            Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())),
            Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())),
            G.CardMovements, Checkpoint = GameCheckpointJson.Serialize(G.CreateCheckpoint())
        });
        public void Cold()
        {
            var before = State();
            G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(State() == before, "Cold replay preserves replacement receipts, native death frames, quotas and physical costs.");
        }
        public static Scenario Start(bool absolute = false, bool zeroMaximum = false, bool rescue = false, bool shenqu = false,
            bool longhun = false, bool huituo = false, bool multipleRecovery = false)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardActiveSkillExpansionPackage(true),
                new StandardRescueSkillExpansionPackage(), new StandardClassicGeneralPackage(), new Fixture(absolute, rescue || shenqu || longhun || huituo, huituo));
            var skills = new List<string> { "classic:luoyi" };
            if (rescue || shenqu || longhun || huituo) skills.AddRange(["shanhe:xueren", "shanhe:shixue-1"]);
            if (shenqu) skills.Add("shanhe:shengui-shenqu");
            if (longhun) skills.Add("ol:longhun");
            if (huituo) skills.Add("classic:huituo");
            var starting = new List<ShanheStartingCard> { new(CardKind.Peach), new(CardKind.Alcohol), new(CardKind.FireSlash),
                new(CardKind.Slash), new(CardKind.Slash), new(CardKind.Duel), new(CardKind.Duel), new(CardKind.SilverLion) };
            if (longhun) starting.AddRange([new(CardKind.Dodge, Suit.Heart, 3), new(CardKind.Dodge, Suit.Heart, 4)]);
            var setup = new ShanheBattleSetup(71, 1, "fixture:replacement", Owner, 12, 0, 0, 0, 0,
                skills,
                starting, [],
                Enumerable.Range(1, 3).Select(_ => new ShanheCombatantDefinition(Enemy, MaxHpBonus: zeroMaximum ? 0 : 20,
                    ExtraSkillIds: multipleRecovery ? ["shanhe:xueren", "shanhe:shixue-1", RecoveryPause] : [])).ToArray(),
                [], new string('a', 64));
            var game = GameEngine.CreateStandard(new GameOptions
            {
                Seed = setup.BattleSeed, ModeId = ShanheBattleSetup.BattleModeId, PlayerCount = 4,
                HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 12
            }, registry);
            var scenario = new Scenario(game, registry);
            scenario._traceRecoveredDyingAttack = rescue || longhun;
            scenario._traceNativePeachAttack = shenqu;
            scenario._traceProgramBenefitAttack = huituo;
            scenario._traceMultipleRecoveryInvoices = multipleRecovery;
            scenario.Accept(new StartGameCommand()); scenario.ReachPlay(); return scenario;
        }
    }

    private sealed class Fixture(bool absolute, bool rescue, bool redJudgment) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shanhe-replacements", new Version(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var assembly = typeof(StandardContentPackage).Assembly;
            string Read(string file)
            {
                using var stream = assembly.GetManifestResourceStream("CardGame.Content.Standard.SkillPrograms." + file)!;
                using var reader = new StreamReader(stream); return reader.ReadToEnd();
            }
            var production = SkillProgramCatalog.Load(Read("shanhe-replacements.rules.json"), Read("shanhe-replacements.presentation.json"));
            foreach (var (id, program) in production.Programs)
                builder.AddSkill(new(id, id, id) { Program = program, Tags = SkillTag.Locked });
            var reactions = SkillProgramCatalog.Load(Read("shanhe-reactions.rules.json"), Read("shanhe-reactions.presentation.json"));
            foreach (var id in new[] { "shanhe:xueren", "shanhe:shixue-1" })
                builder.AddSkill(new(id, id, id) { Program = reactions.Programs[id], Tags = SkillTag.Locked });
            var boss = SkillProgramCatalog.Load(Read("shanhe-boss.rules.json"), Read("shanhe-boss.presentation.json"));
            builder.AddSkill(new("shanhe:shengui-shenqu", "神躯", boss.Presentations["shanhe:shengui-shenqu"].Description)
                { Program = boss.Programs["shanhe:shengui-shenqu"], ProgramPresentation = boss.Presentations["shanhe:shengui-shenqu"] });
            var rules = $$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
                  {"id":"{{{Driver}}}","revision":1,"modifiers":[
                    {"id":"slash","query":"slashLimit","operation":"add","value":8,"priority":0},
                    {"id":"keep","query":"handLimit","operation":"add","value":40,"priority":0}],"activations":[
                    {"id":"chain","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"setChainedState","target":"selectedTarget","chained":true}]},
                    {"id":"equip","minCards":1,"maxCards":1,"sourceZones":["hand"],"cardCategories":["equipment"],"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"captureSelectedCards","target":"owner","resultBind":"gear"},{"op":"placeSelectedEquipment","target":"selectedTarget","sourceBind":"gear"}]},
                    {"id":"strip","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"discardParticipantCards","target":"selectedTarget","amount":20,"zones":["hand"]}]},
                    {"id":"hurt-twelve","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"owner","amount":12,"nature":"normal"}]},
                    {"id":"hurt-one","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"owner","amount":1,"nature":"normal"}]},
                    {"id":"hurt-target","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":1,"nature":"normal"}]},
                    {"id":"recover-pair","minCards":0,"maxCards":0,"minTargets":2,"maxTargets":2,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"recover","target":"selectedTargets","amount":1}]}]},
                  {"id":"{{{Layer}}}","revision":1,"damageModifiers":[
                    {"id":"card","cardKinds":["slash","fireSlash","thunderSlash","duel"],"amount":2,"condition":"always","damageLayer":"cardDamage"},
                    {"id":"additional","cardKinds":["slash","fireSlash","thunderSlash","duel"],"amount":3,"condition":"always"}]},
                  {"id":"{{{Absolute}}}","revision":1,"cardPolicies":[{"id":"absolute","kind":"damageBecomesHpLoss","cardKinds":[]}]},
                  {"id":"{{{Quiet}}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]},
                  {"id":"{{{RecoveryPause}}}","revision":1,"triggers":[{"id":"pause-counter","window":"afterDamageApplied","subject":"damageSource","damageOccurrence":"perDamage","optional":true,"condition":{"kind":"damageTargetIsOther"},"effects":[{"op":"draw","target":"owner","amount":1}]}]},
                  {"id":"{{{DeathNotice}}}","revision":1,"triggers":[{"id":"notice","window":"ownerDied","subject":"owner","optional":true,"effects":[{"op":"draw","target":"owner","amount":1,"condition":{"kind":"wounded"}}]}]}
                ]}
                """;
            var fixture = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new[] { Driver, Layer, Absolute, Quiet, DeathNotice, RecoveryPause }.ToDictionary(id => id, _ => new { name = "替换机制夹具", description = "真实牌、原生裸衣与死亡返回" })
            }));
            foreach (var (id, program) in fixture.Programs) builder.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = fixture.Presentations[id] });
            var own = new List<string> { Layer, "shanhe:xiaoming", "shanhe:mouming" };
            if (absolute) own.Add(Absolute);
            if (rescue) own.Add(RecoveryPause);
            builder.AddGeneral(new(Owner, "替换来源", "", Driver, "shu", 12, own));
            builder.AddGeneral(new(Enemy, "固定守军", "", Quiet, "qun", 4, [DeathNotice]));
            builder.AddDeck(new(Deck, "纯闪", 0, 2, [])
                { PhysicalCards = Enumerable.Range(0, 100).Select(_ => new ContentDeckPhysicalCard("standard:dodge", redJudgment ? Suit.Heart : Suit.Spade, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "替换机制", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}
