using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSourceShennuChecks
{
    private const string Skill = "shanhe:source-shennu-14214", Owner = "fixture:source-shennu-owner";
    private const string Source = "fixture:source-shennu-source", Target = "fixture:source-shennu-target";
    private const string Driver = "fixture:source-shennu-driver", Quiet = "fixture:source-shennu-quiet";
    private const string Prevent = "fixture:source-shennu-prevent", Watch = "fixture:source-shennu-watch", Deck = "fixture:source-shennu-deck";
    private const string Counter = "fixture:source-shennu-counter";
    private const string Wenyi = "ol:wenyi";
    private const string DrawReason = "skill-program.shanhe-source-shennu-14214.draw";

    public static void ActualDamageEarnsOneNuDrawsRealSourceHpAndOnlyCompletedOutgoingDamageClears()
    {
        var scene = Scene.Start();
        foreach (var (activation, amount, nu) in new[] { ("hurt-three", 3, 1), ("hurt-two", 2, 2) })
        {
            var hand = scene.View(0).HandCount;
            scene.Use(activation, [1]); scene.DrainDrawChildren(5);
            var received = scene.Events<ShanheShennuReceivedEvent>().Last();
            Need(received.Receipt.DamageAmount == amount && received.Receipt.SourceSeat == 1 && received.Receipt.SourceHp == 5 &&
                received.Receipt.NuBefore == nu - 1 && received.Receipt.NuAfter == nu && scene.Nu == nu &&
                scene.View(0).HandCount == hand + 5 && scene.Events<ShanheShennuReceivedEvent>().Length == nu &&
                scene.Events<DamageAppliedEvent>().Count(fact => fact.TargetSeat == 0 && fact.SourceSeat == 1) == nu,
                "Each real multi-point hit adds one independent source Nu and draws the true source's current five HP, rather than the damage amount, owner HP or turn-owner HP.");
        }
        var targetHp = scene.View(2).Hp;
        var slash = scene.Game.GetHumanLegalActions().First(action => action.Kind == LegalActionKind.Slash && action.TargetSeats.SequenceEqual([2]));
        Need(scene.Game.PredictPublicActionDamage(0, slash, 2).DamageAmount == 3 && scene.Nu == 2,
            "Public proposed Slash damage includes both visible Nu without consuming or clearing them.");
        scene.Use("hit", [2]); scene.Play();
        Need(scene.Nu == 2 && scene.View(2).Hp == targetHp && scene.Events<ShanheShennuOutgoingIssuedEvent>().Length == 0 &&
            scene.Events<ShanheShennuClearedEvent>().Length == 0 &&
            scene.Events<ProgramDamagePreventedEvent>().Any(fact => fact.SkillId == Prevent && fact.Amount == 3),
            "A prevented native three-damage attempt neither owns an applied-damage receipt nor consumes Nu.");
        scene.Use("hit", [2]);
        scene.DrainDrawChildren(targetHp - 3, () =>
        {
            var original = scene.Events<ShanheShennuOutgoingIssuedEvent>().Single();
            var nested = scene.Events<ShanheShennuReceivedEvent>().Last().Receipt;
            Need(scene.Nu == 3 && nested.SourceSeat == 2 && nested.SourceHp == targetHp - 3 && nested.DamageAmount == 1 &&
                nested.NuBefore == 2 && nested.NuAfter == 3 && scene.Events<ShanheShennuClearedEvent>().Length == 0 &&
                original.Receipt.Source.OwnerSeat == 0 && original.Receipt.TargetSeat == 2 && original.Receipt.DamageAmount == 3 &&
                scene.Game.ResolutionStack.OfType<DamageFrame>().Any(damage => damage.Id == original.DamageFrameId &&
                    damage.ParentFrameId == original.Receipt.AttackResolutionId && damage.SourceSeat == 0 && damage.TargetSeat == 2 &&
                    damage.ShanheShennuOutgoing is { Cleared: false }) &&
                scene.Events<DamageAppliedEvent>().Count(fact => fact.SourceSeat == 2 && fact.TargetSeat == 0 && fact.Amount == 1) == 1,
                "Real eventSource retaliation earns the third Nu and freezes the retaliator's remaining HP while the original outgoing damage receipt stays live and uncleared.");
        });
        var issued = scene.Events<ShanheShennuOutgoingIssuedEvent>().Single();
        var cleared = scene.Events<ShanheShennuClearedEvent>().Single();
        Need(scene.View(2).Hp == targetHp - 3 && scene.Nu == 0 &&
            scene.Events<DamageAppliedEvent>().Count(fact => fact.SourceSeat == 0 && fact.TargetSeat == 2 && fact.Amount == 3) == 1 &&
            issued.Receipt.Source.OwnerSeat == 0 && issued.Receipt.TargetSeat == 2 && issued.Receipt.DamageAmount == 3 &&
            cleared.DamageFrameId == issued.DamageFrameId && cleared.AttackResolutionId == issued.Receipt.AttackResolutionId &&
            cleared.OwnerSeat == 0 && cleared.RemovedNu == 3 &&
            scene.Events<PlayerMarkerChangedEvent>().Count(fact => fact.Marker == PlayerMarkerKind.ShanheSourceNu && fact.Delta == -3 &&
                fact.ResolutionId == issued.DamageFrameId && fact.PlayerSeat == 0 && fact.Count == 0) == 1,
            "The successful native damage adds its original two Nu; after nested retaliation and its real draw children return, the original DamageFrame clears that source's full three Nu exactly once.");
        Need(scene.Game.PredictPublicActionDamage(0, slash, 2).DamageAmount == 1,
            "Public proposed damage immediately returns to its normal amount after real completion clears Nu.");
        scene.Use("hit", [2]); scene.Play();
        Need(scene.View(2).Hp == targetHp - 4 && scene.Events<ShanheShennuOutgoingIssuedEvent>().Length == 2 &&
            scene.Events<ShanheShennuClearedEvent>().Last().RemovedNu == 0 && scene.Nu == 0 &&
            scene.Events<ProgramBindingStartedEvent>().Count(fact => fact.SkillId == Counter) == 1,
            "A subsequent actual zero-Nu outgoing hit still gets its own source-bound completion receipt and normal one damage.");

        var self = Scene.Start(hp: 4);
        self.Use("self"); self.DrainDrawChildren(1);
        var selfReceived = self.Events<ShanheShennuReceivedEvent>().Single();
        var selfIssued = self.Events<ShanheShennuOutgoingIssuedEvent>().Single();
        Need(self.View(0).Hp == 1 && selfReceived.Receipt.SourceSeat == 0 && selfReceived.Receipt.SourceHp == 1 &&
            selfReceived.Receipt.NuBefore == 0 && selfReceived.Receipt.NuAfter == 1 && self.Nu == 0 &&
            selfIssued.DamageFrameId == selfReceived.Receipt.DamageFrameId &&
            self.Events<ShanheShennuClearedEvent>() is [{ OwnerSeat: 0, RemovedNu: 1 }],
            "An outgoing receipt is issued even at zero Nu: true self-damage earns one Nu and draws its post-hit source HP, then that same actual damage clears the earned Nu.");

        var wenyi = Scene.Start(wenyi: true);
        wenyi.Use("hurt-three", [1]); wenyi.DrainDrawChildren(5);
        wenyi.Use("hit", [2]); wenyi.OfferWenyi();
        var wenyiIssued = wenyi.Events<ShanheShennuOutgoingIssuedEvent>().Single();
        var paidDamage = wenyi.Game.ResolutionStack.OfType<DamageFrame>().Single(frame => frame.Id == wenyiIssued.DamageFrameId);
        var healthChild = wenyi.Game.ResolutionStack.OfType<HpChangedTriggerWindowFrame>().Single();
        var history = wenyi.Game.Events.Select(item => item.Payload).ToArray();
        Need(wenyi.Nu == 1 && wenyi.View(2).Hp == 1 && paidDamage.Amount == 2 && paidDamage.SourceSeat == 0 &&
            paidDamage.ParentFrameId == wenyiIssued.Receipt.AttackResolutionId &&
            paidDamage.ShanheShennuOutgoing is { Cleared: false, Source.OwnerSeat: 0 } &&
            paidDamage.WenyiHealth is { } nativeHealth && nativeHealth.HealthChangeId == healthChild.Id &&
            nativeHealth.DamageFrameId == paidDamage.Id && healthChild.Change.ParentFrameId == paidDamage.Id &&
            healthChild.Change is { Kind: HpChangeKind.Damage, TargetSeat: 2, HpBefore: 3, HpAfter: 1, Amount: 2 } &&
            wenyi.Events<ShanheShennuClearedEvent>().Length == 0 &&
            Array.FindIndex(history, fact => fact is ShanheShennuOutgoingIssuedEvent issuedFact && issuedFact.DamageFrameId == paidDamage.Id) <
                Array.FindIndex(history, fact => fact is WenyiDamageHealthStartedEvent started && started.Receipt.DamageFrameId == paidDamage.Id),
            "The original outgoing receipt must already exist before native Wenyi pauses the actual three-to-one damage HP child; later short-circuited damage hooks cannot lose its Nu-clear responsibility.");
        wenyi.SkipWenyi(); wenyi.Play();
        Need(wenyi.Nu == 0 && wenyi.View(2).Hp == 1 &&
            wenyi.Events<WenyiDamageHealthReturnedEvent>().Count(fact => fact.Receipt.DamageFrameId == paidDamage.Id &&
                fact.Receipt.AttackFrameId == wenyiIssued.Receipt.AttackResolutionId) == 1 &&
            wenyi.Events<ShanheShennuOutgoingIssuedEvent>().Length == 1 &&
            wenyi.Events<ShanheShennuClearedEvent>() is [var wenyiClear] && wenyiClear.RemovedNu == 1 &&
            wenyiClear.DamageFrameId == paidDamage.Id && wenyiClear.AttackResolutionId == wenyiIssued.Receipt.AttackResolutionId &&
            wenyiClear.OwnerSeat == 0 && wenyi.Game.ResolutionStack.All(frame => frame is not DamageFrame and not HpChangedTriggerWindowFrame),
            "Declining the genuine optional Wenyi aid returns its paid HP child normally, then the same original damage receipt clears its source's one Nu exactly once.");
    }

    private static void Need(bool valid, string message) { if (!valid) throw new InvalidOperationException(message); }
    private sealed class Scene(GameEngine game)
    {
        public GameEngine Game { get; } = game;
        public PendingDecision? P => Enumerable.Range(0, 3).Select(seat => Game.CreateSnapshot(seat).PendingDecision)
            .FirstOrDefault(prompt => prompt is not null);
        public PlayerSnapshot View(int seat) => Game.CreateSnapshot(seat).Players[seat];
        public int Nu => View(0).Markers?.SingleOrDefault(marker => marker.Kind == PlayerMarkerKind.ShanheSourceNu)?.Count ?? 0;
        public T[] Events<T>() => Game.Events.Select(item => item.Payload).OfType<T>().ToArray();
        public void Submit(GameCommand command)
        { var result = Game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native Shennu command."); }
        public void Use(string activation, int[]? targets = null) => Submit(new UseProgramSkillCommand(0, Driver,
            activation, [], targets ?? [], Game.Revision, P!.PromptId));
        public void Play() => Until(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void OfferWenyi() => Until(() => P is { Kind: DecisionKind.ProgramTrigger } prompt && prompt.SkillPrompt?.SkillId == Wenyi);
        public void SkipWenyi()
        {
            var prompt = P!;
            Need(prompt.SkillPrompt?.SkillId == Wenyi, "The real Wenyi HP-child must own its published skip choice.");
            Submit(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId,
                prompt.Choices.Single(choice => choice.Parameters.GetValueOrDefault("program-action") == "skip").Id, Game.Revision));
        }
        public void DrainDrawChildren(int count, Action? atFirstChild = null)
        {
            var batches = new HashSet<long>(); var cards = new HashSet<int>();
            for (var index = 0; index < count; index++)
            {
                Until(() => P?.SkillPrompt?.SkillId == Watch);
                if (index == 0) atFirstChild?.Invoke();
                var frame = Game.ResolutionStack.OfType<ProgramSkillFrame>().Single(item => item.SkillId == Skill);
                var receipt = frame.ShanheShennuReceived!;
                var observer = Game.ResolutionStack.OfType<ProgramSkillFrame>().Last(item => item.SkillId == Watch);
                Need(receipt.SourceHp == count && observer.WindowContext?.MovementBatch is { } batch && batches.Add(batch.Id) &&
                    batch.ParentFrameId == frame.Id && batch.OriginSkillId == Skill && batch.OriginOwnerSeat == 0 &&
                    batch.Movements is [var movement] && cards.Add(movement.CardId) && movement.From == CardLocation.DrawPile &&
                    movement.To == CardLocation.Hand(0) && movement.Reason.Value == DrawReason &&
                    Game.ResolutionStack.OfType<DamageFrame>().Any(damage => damage.Id == receipt.DamageFrameId && damage.TargetSeat == 0) &&
                    Game.ResolutionStack.OfType<DamageTriggerWindowFrame>().Any(window => window.Id == receipt.DamageWindowId &&
                        window.ParentFrameId == receipt.DamageFrameId),
                    "Each real draw singleton pauses under the original received-damage program and original applied DamageFrame, without repeating Nu payment.");
                var prompt = P!;
                Submit(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId,
                    prompt.Choices.Single(choice => choice.Parameters.GetValueOrDefault("option-id") == "continue").Id, Game.Revision));
            }
            Play();
            Need(batches.Count == count && cards.Count == count,
                "Every paid source-HP draw entity and observer batch completes exactly once.");
        }
        private void Until(Func<bool> done)
        {
            for (var step = 0; step < 180; step++)
            {
                if (done()) return;
                Need(P is null || P.PlayerSeat != 0, "Unexpected native Shennu prompt: " + JsonSerializer.Serialize(P));
                Submit(new AdvanceOneStepCommand(Game.Revision));
            }
            throw new InvalidOperationException("The bounded native source Shennu flow did not reach its real boundary.");
        }
        public static Scene Start(int hp = 20, bool wenyi = false)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(wenyi));
            var setup = new ShanheBattleSetup(71, 1, "source-shennu-native", Owner, hp, 0, 0, 0, 0, [],
                [new(CardKind.Slash, Suit.Heart, 9)], [], [new(Source) { StartingHand = [] }, new(Target) { StartingHand = [] }], [],
                new string('e', 64)) { TacticSkillIds = [Skill] };
            var game = GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 3, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 6
            }, registry);
            var scene = new Scene(game); scene.Submit(new StartGameCommand()); scene.Play(); return scene;
        }
    }
    private sealed class Fixture(bool wenyi) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-source-shennu", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            string Read(string bundle, string part)
            {
                using var stream = typeof(StandardContentPackage).Assembly.GetManifestResourceStream(
                    "CardGame.Content.Standard.SkillPrograms." + bundle + "." + part + ".json")
                    ?? throw new InvalidOperationException("Missing native bundle " + bundle);
                using var reader = new StreamReader(stream); return reader.ReadToEnd();
            }
            var source = SkillProgramCatalog.Load(Read("shanhe-source-shennu-14214", "rules"), Read("shanhe-source-shennu-14214", "presentation"));
            builder.AddSkill(new(Skill, source.Presentations[Skill].Name, source.Presentations[Skill].Description)
                { Program = source.Programs[Skill], ProgramPresentation = source.Presentations[Skill], Tags = SkillTag.Locked });
            if (wenyi)
            {
                var native = SkillProgramCatalog.Load(Read("ol-huan-shu-jun", "rules"), Read("ol-huan-shu-jun", "presentation"));
                builder.AddSkill(new(Wenyi, native.Presentations[Wenyi].Name, native.Presentations[Wenyi].Description)
                    { Program = native.Programs[Wenyi], ProgramPresentation = native.Presentations[Wenyi] });
            }
            var rules = $$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
                  {"id":"{{{Driver}}}","revision":1,"activations":[
                    {"id":"hurt-three","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                      "effects":[{"op":"damage","target":"owner","amount":3,"sourceRef":{"kind":"selectedTarget"}}]},
                    {"id":"hurt-two","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                      "effects":[{"op":"damage","target":"owner","amount":2,"sourceRef":{"kind":"selectedTarget"}}]},
                    {"id":"hit","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                      "effects":[{"op":"damage","target":"selectedTarget","amount":1}]},
                    {"id":"self","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                      "effects":[{"op":"damage","target":"owner","amount":3}]}]},
                  {"id":"{{{Prevent}}}","revision":1,"triggers":[{"id":"first","window":"beforeDamageApplied","subject":"damageTarget","optional":false,
                    "usageScope":"game","usageLimit":1,"effects":[{"op":"preventCurrentDamage","target":"owner"}]}]},
                  {"id":"{{{Counter}}}","revision":1,"triggers":[{"id":"one-real-retaliation","window":"afterDamageApplied","subject":"owner",
                    "damageOccurrence":"perDamage","optional":false,"usageScope":"game","usageLimit":1,
                    "effects":[{"op":"damage","target":"owner","targetRef":{"kind":"eventSource"},"amount":1}]}]},
                  {"id":"{{{Watch}}}","revision":1,"triggers":[{"id":"real-draw-child","window":"cardsGained","subject":"owner","optional":false,
                    "destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["{{{DrawReason}}}"],
                    "effects":[{"op":"chooseOption","target":"owner","resultBind":"draw","options":[{"id":"continue"}]}]}]},
                  {"id":"{{{Quiet}}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,
                    "effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}
                ]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object>
                {
                    [Driver] = new { name = "实际神怒伤害", description = "真实有来源与自身伤害" },
                    [Prevent] = new { name = "一次防伤", description = "真实防止首个伤害尝试" },
                    [Counter] = new { name = "一次真实反伤", description = "对实际伤害来源造成一点伤害" },
                    [Quiet] = new { name = "静守", description = "跳过出牌" },
                    [Watch] = new { name = "神怒摸牌孩子", description = "真实原牌移动返回",
                        optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } }
                } });
            var catalog = SkillProgramCatalog.Load(rules, presentation);
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                    { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Owner, "真实神怒持有者", "supporter", Driver, "shu", 20)
                { AdditionalSkillIds = wenyi ? [Watch, Wenyi] : [Watch] });
            builder.AddGeneral(new(Source, "五体力真实来源", "supporter", Quiet, "wei", 5));
            builder.AddGeneral(new(Target, "实际伤害目标", "supporter", wenyi ? Quiet : Prevent, "wei", wenyi ? 3 : 20)
                { AdditionalSkillIds = wenyi ? [] : [Quiet, Counter] });
            builder.AddDeck(new(Deck, "真实摸牌实体", 0, 0, [new("standard:dodge", 80)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "神怒三人阵", 3, 3, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Source, Target], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
