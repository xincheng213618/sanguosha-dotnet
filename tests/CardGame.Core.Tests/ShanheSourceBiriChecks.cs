using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSourceBiriChecks
{
    private const string Biri = "shanhe:source-biri-14039";
    private const string Owner = "fixture:source-biri-owner", Enemy = "fixture:source-biri-enemy";
    private const string Driver = "fixture:source-biri-driver", HpWatch = "fixture:source-biri-hp-watch";
    private const string DrawWatch = "fixture:source-biri-draw-watch", Quiet = "fixture:source-biri-quiet";
    private const string Deck = "fixture:source-biri-deck", DrawReason = "skill-program.shanhe-source-biri-14039.draw";

    public static void FirePreventionUsesAtomicRecoveryResultBeforeHealthAndDrawChildren()
    {
        foreach (var (amount, wound, recovered) in new[] { (1, 3, 2), (3, 1, 1), (2, 0, 0) })
        {
            var scene = Scene.Start();
            if (wound > 0) { scene.Use("wound-" + wound); scene.Play(); }
            var hp = scene.View.Hp; var hand = scene.View.HandCount;
            scene.Use("fire-" + amount, [1]);
            if (recovered > 0)
            {
                scene.Until(() => scene.Holding("health"));
                var root = scene.Game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Biri);
                var receipt = root.ShanheSourceBiri!;
                var before = scene.Game.ResolutionStack.OfType<BeforeDamageProgramWindowFrame>().Single(frame => frame.Id == receipt.BeforeDamageFrameId);
                Need(before.Prevented && before.Amount == amount && before.Nature == DamageNature.Fire && before.TargetSeat == 0 &&
                    receipt.PreventedAmount == amount && receipt.RequestedRecoveryAmount == amount * 2 &&
                    receipt.ActualRecovered == recovered && receipt.AppliedRecoveryFrameId is not null &&
                    receipt.Stage == ShanheSourceBiriStage.RecoveryChildren && scene.View.Hp == hp + recovered &&
                    scene.View.HandCount == hand && scene.Events<ShanheSourceBiriDrawIssuedEvent>().Length == 0,
                    "The original fire attempt is prevented once; its exact native recovery pays before any draw or health-observer loss.");
                var paid = scene.Events<ShanheSourceBiriRecoveredEvent>().Single();
                Need(paid.ProgramFrameId == root.Id && paid.RecoveryFrameId == receipt.AppliedRecoveryFrameId &&
                    paid.RecoveryParentFrameId == root.Id && paid.OwnerSeat == 0 && paid.Amount == recovered &&
                    scene.Events<ProgramRecoveryAppliedEvent>().Any(fact => fact.ProgramFrameId == root.Id &&
                        fact.Receipt.RecoveryFrameId == paid.RecoveryFrameId && fact.Receipt.TargetSeat == 0),
                    "Actual recovery is tied to its real program owner and recovery child, not a later net HP comparison.");
                scene.Answer("health");
                var batchIds = new HashSet<long>();
                var cardIds = new HashSet<int>();
                // DrawCards pays each DrawOne through its own atomic MoveCard
                // batch. PerBatch observes each real singleton, not the outer
                // draw invocation; each must return to the same paid Biri root.
                for (var childIndex = 0; childIndex < recovered; childIndex++)
                {
                    scene.Until(() => scene.Holding("cards"));
                    root = scene.Game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Biri);
                    var observer = scene.Game.ResolutionStack.OfType<ProgramSkillFrame>().Last(frame => frame.SkillId == DrawWatch);
                    Need(scene.View.Hp == hp + recovered - 1 && root.ShanheSourceBiri is
                        { Stage: ShanheSourceBiriStage.DrawChildren } drawing && drawing.ActualRecovered == recovered &&
                        drawing.DrawCount == recovered && scene.View.HandCount == hand + recovered &&
                        scene.Events<ProgramBindingResolvedEvent>().Any(fact => fact.SkillId == HpWatch && fact.Completed) &&
                        scene.Events<ProgramBindingResolvedEvent>().All(fact => fact.SkillId != Biri) &&
                        observer.WindowContext?.MovementBatch is { } batch && batch.ParentFrameId == root.Id &&
                        batch.OriginSkillId == Biri && batch.OriginOwnerSeat == 0 && batchIds.Add(batch.Id) &&
                        batch.Movements is [var movement] && cardIds.Add(movement.CardId) &&
                        movement.From == CardLocation.DrawPile && movement.To == CardLocation.Hand(0) &&
                        movement.Reason.Value == DrawReason && movement.Sequence > drawing.MovementSequenceBefore &&
                        movement.Sequence <= drawing.MovementSequenceAfter &&
                        scene.Events<ProgramBindingStartedEvent>().Count(fact => fact.SkillId == DrawWatch) == childIndex + 1,
                        "The health child returns before draw; every distinct native singleton draw batch owns one observer and returns to the original paid Biri root.");
                    scene.Answer("cards");
                }
                Need(batchIds.Count == recovered && cardIds.Count == recovered,
                    "Each paid physical draw has exactly one distinct actual gain batch; a duplicate child cannot replace another card's return.");
            }
            scene.Play();
            var issued = scene.Events<ShanheSourceBiriIssuedEvent>().Single();
            var prevented = scene.Events<ProgramDamagePreventedEvent>().Where(fact => fact.SkillId == Biri).ToArray();
            var drawn = scene.Events<ShanheSourceBiriDrawIssuedEvent>().Single();
            var biriCompletions = scene.Events<ProgramBindingResolvedEvent>().Where(fact => fact.SkillId == Biri).ToArray();
            var completionChecks = new Dictionary<string, bool>
            {
                ["frozen amounts"] = issued.PreventedAmount == amount && issued.RequestedRecoveryAmount == amount * 2,
                ["exact prevention"] = prevented is [var prevention] && prevention.FrameId == issued.BeforeDamageFrameId &&
                    prevention.Amount == amount && prevention.OwnerSeat == 0 && prevention.TargetSeat == 0,
                ["no real fire damage"] = scene.Events<DamageAppliedEvent>().All(fact => fact.TargetSeat != 0),
                ["paid recovery draw"] = drawn.ActualRecovered == recovered && drawn.DrawCount == recovered,
                ["one actual recovery"] = scene.Events<ShanheSourceBiriRecoveredEvent>().Length == (recovered > 0 ? 1 : 0),
                ["final hp"] = scene.View.Hp == hp + recovered - (recovered > 0 ? 1 : 0),
                ["final hand"] = scene.View.HandCount == hand + recovered,
                ["exact physical draws"] = scene.Game.CardMovements.Count(move => move.From == CardLocation.DrawPile &&
                    move.To == CardLocation.Hand(0) && move.Reason.Value == DrawReason) == recovered,
                // The normal return protocol records the paid program, then
                // advances its before-damage candidate with a separate fact
                // owned by the parent window. Each exact identity occurs once.
                ["one completed paid Biri program"] = biriCompletions.Count(fact => fact.FrameId == issued.ProgramFrameId &&
                    fact.OwnerSeat == issued.Source.OwnerSeat && fact.BindingId == issued.Source.BindingId &&
                    fact.SkillInstanceId == issued.Source.SkillInstanceId && fact.Window == SkillProgramTriggerWindow.BeforeDamageApplied &&
                    fact.Completed && fact.Activated) == 1,
                ["one completed original parent candidate"] = biriCompletions.Count(fact => fact.FrameId == issued.BeforeDamageFrameId &&
                    fact.OwnerSeat == issued.Source.OwnerSeat && fact.BindingId == issued.Source.BindingId &&
                    fact.SkillInstanceId == issued.Source.SkillInstanceId && fact.Window == SkillProgramTriggerWindow.BeforeDamageApplied &&
                    fact.Completed && fact.Activated) == 1,
                ["no additional Biri completion"] = biriCompletions.Length == 2,
                ["all actual draw children completed"] = scene.Events<ProgramBindingResolvedEvent>().Count(fact =>
                    fact.SkillId == DrawWatch && fact.Completed) == recovered,
                ["quiet original stack"] = scene.Game.ResolutionStack.All(frame =>
                    frame is not ProgramSkillFrame and not BeforeDamageProgramWindowFrame and not RecoveryReplacementFrame)
            };
            Need(completionChecks.Values.All(value => value),
                "Fire 1/3/2 is fully prevented; missing HP clamps actual recovery, full HP draws zero, and every original child returns once. " +
                JsonSerializer.Serialize(new { amount, wound, recovered, hpBefore = hp, handBefore = hand,
                    hpAfter = scene.View.Hp, handAfter = scene.View.HandCount, completionChecks, issued, prevented, drawn,
                    recoveries = scene.Events<ShanheSourceBiriRecoveredEvent>(), damage = scene.Events<DamageAppliedEvent>(),
                    bindings = scene.Events<ProgramBindingResolvedEvent>(),
                    stack = scene.Game.ResolutionStack.Select(frame => new { frame.Id, frame.Kind }).ToArray(),
                    movements = scene.Game.CardMovements.Where(move => move.Reason.Value == DrawReason).ToArray() }));
            var afterFire = scene.View.Hp;
            scene.Use("normal", [1]); scene.Play(); scene.Use("thunder", [1]); scene.Play();
            Need(scene.View.Hp == afterFire - 2 && scene.Events<ShanheSourceBiriIssuedEvent>().Length == 1 &&
                scene.Events<DamageAppliedEvent>().Count(fact => fact.TargetSeat == 0 && fact.Amount == 1 &&
                    fact.Nature is DamageNature.Normal or DamageNature.Thunder) == 2,
                "The same holder takes real normal and thunder damage; source Biri prevents only fire.");
        }
    }

    private static void Need(bool valid, string message) { if (!valid) throw new InvalidOperationException(message); }
    private sealed class Scene(GameEngine game)
    {
        public GameEngine Game { get; } = game;
        public PlayerSnapshot View => Game.CreateSnapshot(0).Players[0];
        private PendingDecision? P => Enumerable.Range(0, 2).Select(seat => Game.CreateSnapshot(seat).PendingDecision)
            .FirstOrDefault(prompt => prompt is not null);
        public T[] Events<T>() => Game.Events.Select(item => item.Payload).OfType<T>().ToArray();
        public bool Holding(string option) => P?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("option-id") == option) == true;
        public void Answer(string option)
        {
            var prompt = P!;
            Accept(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId,
                prompt.Choices.Single(choice => choice.Parameters.GetValueOrDefault("option-id") == option).Id, Game.Revision));
        }
        public void Use(string activation, int[]? targets = null) => Accept(new UseProgramSkillCommand(0, Driver,
            activation, [], targets ?? [], Game.Revision, P!.PromptId));
        public void Play() => Until(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Accept(GameCommand command)
        { var result = Game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Source Biri native command failed."); }
        public void Until(Func<bool> done)
        {
            for (var step = 0; step < 160; step++)
            {
                if (done()) return;
                if (P is { PlayerSeat: 0 } prompt)
                {
                    var choice = prompt.Choices.FirstOrDefault(item => item.Parameters.GetValueOrDefault("response") is
                        "pass" or "take-damage" or "let-die" or "no-nullification");
                    Need(choice is not null, "Unexpected source Biri prompt: " + JsonSerializer.Serialize(prompt));
                    Accept(new AnswerPromptCommand(0, prompt.PromptId, choice!.Id, Game.Revision));
                }
                else Accept(new AdvanceOneStepCommand(Game.Revision));
            }
            throw new InvalidOperationException("Source Biri native flow failed to reach its original child boundary.");
        }
        public static Scene Start()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var setup = new ShanheBattleSetup(71, 1, "source-biri-native", Owner, 8, 0, 0, 0, 0, [], [], [],
                [new(Enemy) { StartingHand = [] }], [], new string('b', 64)) { TacticSkillIds = [Biri] };
            var game = GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 5
            }, registry);
            var scene = new Scene(game); scene.Accept(new StartGameCommand()); scene.Play(); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-source-biri", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            string Read(string part)
            {
                using var stream = typeof(StandardContentPackage).Assembly.GetManifestResourceStream(
                    "CardGame.Content.Standard.SkillPrograms.shanhe-source-biri-14039." + part + ".json")
                    ?? throw new InvalidOperationException("Missing source Biri embedded " + part);
                using var reader = new StreamReader(stream); return reader.ReadToEnd();
            }
            var source = SkillProgramCatalog.Load(Read("rules"), Read("presentation"));
            builder.AddSkill(new(Biri, source.Presentations[Biri].Name, source.Presentations[Biri].Description)
                { Program = source.Programs[Biri], ProgramPresentation = source.Presentations[Biri] });
            var activations = new List<object>();
            foreach (var amount in new[] { 1, 3 }) activations.Add(new { id = "wound-" + amount,
                minCards = 0, maxCards = 0, minTargets = 0, maxTargets = 0, targetKind = "anyLiving", usesPerTurn = (int?)null,
                effects = new object[] { new { op = "loseHp", target = "owner", amount } } });
            foreach (var (id, amount, nature) in new[] { ("fire-1", 1, "fire"), ("fire-2", 2, "fire"), ("fire-3", 3, "fire"),
                         ("normal", 1, "normal"), ("thunder", 1, "thunder") })
                activations.Add(new { id, minCards = 0, maxCards = 0, minTargets = 1, maxTargets = 1,
                    targetKind = "otherLiving", usesPerTurn = (int?)null,
                    effects = new object[] { new { op = "damage", target = "owner", sourceRef = new { kind = "selectedTarget" }, amount, nature } } });
            var fixtureRules = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion,
                skills = new object[] { new { id = Driver, revision = 1, activations } } });
            var observers = $$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
                  {"id":"{{{HpWatch}}}","revision":1,"triggers":[{"id":"real-recovery-child","window":"afterHpRecovered","subject":"owner","optional":false,
                    "effects":[{"op":"chooseOption","target":"owner","resultBind":"health","options":[{"id":"health"}]},
                      {"op":"loseHp","target":"owner","amount":1}]}]},
                  {"id":"{{{DrawWatch}}}","revision":1,"triggers":[{"id":"real-draw-child","window":"cardsGained","subject":"owner","optional":false,
                    "destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["{{{DrawReason}}}"],
                    "effects":[{"op":"chooseOption","target":"owner","resultBind":"cards","options":[{"id":"cards"}]}]}]},
                  {"id":"{{{Quiet}}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,
                    "effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}
                ]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object>
                {
                    [Driver] = new { name = "真实伤害驱动", description = "原生伤害与失去体力" },
                    [Quiet] = new { name = "静守", description = "跳过出牌" },
                    [HpWatch] = new { name = "恢复孩子", description = "恢复后的实际扣血",
                        optionLabels = new Dictionary<string, string> { ["health"] = "恢复孩子继续" } },
                    [DrawWatch] = new { name = "摸牌孩子", description = "原牌摸牌孩子返回",
                        optionLabels = new Dictionary<string, string> { ["cards"] = "摸牌孩子继续" } }
                } });
            using var driverDocument = JsonDocument.Parse(fixtureRules);
            using var observerDocument = JsonDocument.Parse(observers);
            var rules = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion,
                skills = driverDocument.RootElement.GetProperty("skills").EnumerateArray()
                    .Concat(observerDocument.RootElement.GetProperty("skills").EnumerateArray()).ToArray() });
            var catalog = SkillProgramCatalog.Load(rules, presentation);
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                    { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Owner, "蔽日持有者", "supporter", Driver, "shu", 8) { AdditionalSkillIds = [HpWatch, DrawWatch] });
            builder.AddGeneral(new(Enemy, "真实火伤来源", "supporter", Quiet, "wei", 8));
            builder.AddDeck(new(Deck, "零正常起手与摸牌", 0, 0, [new("standard:dodge", 40)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "蔽日两人阵", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
