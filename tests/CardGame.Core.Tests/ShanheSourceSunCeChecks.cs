using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSourceSunCeChecks
{
    private const string Skill = StandardShanheSourceSunCePackage.HunziSkillId;
    private const string Yinghun = StandardShanheSourceSunCePackage.YinghunSkillId;
    private const string Driver = "fixture:source-sunce-damage", Deck = "fixture:source-sunce-deck";

    public static void WoundedAtTwoHpAwakensAndDrawsThreeOnlyInThatTurnEnding()
    {
        var scene = Scene.Start(1);
        Need(scene.Owner.Hp == 3 && scene.Owner.MaxHp == 3 && scene.Events<SkillAwakenedEvent>().Length == 0,
            "The actual source2655 body starts at 3/3 and its healthy first preparation does not awaken.");
        scene.Hurt();
        Need(scene.Owner.Hp == 2 && scene.Events<DamageAppliedEvent>().Any(fact => fact.TargetSeat == 0 && fact.Amount == 1),
            "The source really takes native damage to 2/3 before a later natural preparation.");
        scene.EndPlay(); scene.Reach(() => scene.IsOwnPlay && scene.Game.State.TurnNumber >= 3);
        AssertAwakening(scene, hp: 2);
        Need(scene.Events<ProgramBindingStartedEvent>().All(fact => fact.SkillId != Skill || fact.BindingId != "awakening-turn-ending"),
            "A preparation awakening does not immediately execute its deferred ending reward.");
        // Keep the awakened body wounded for a later, genuine acquired Yinghun preparation.
        scene.Hurt();
        scene.EndPlay(); scene.Reach(() => scene.IsHunziChoice);
        var rewardTurn = scene.Game.State.TurnNumber;
        var before = scene.Owner.HandCount;
        scene.Answer(option => option.Parameters.GetValueOrDefault("option-id") == "draw-three");
        scene.Reach(() => scene.Completed("awakening-turn-ending") == 1);
        var drawn = scene.Game.CardMovements.Where(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0) &&
            move.Reason.Value == "skill-program." + Skill + ".Draw").ToArray();
        Need(rewardTurn == 3 && drawn.Length == 3 && drawn.Select(move => move.CardId).Distinct().Count() == 3 &&
            scene.Owner.HandCount == before + 3 && scene.Owner.Hp == 1 && scene.Events<RecoveryAppliedEvent>().Length == 0,
            "The awakening turn's real ending choice draws exactly three distinct entities and executes no recovery branch.");
        scene.Reach(() => scene.P is { Kind: DecisionKind.ProgramTrigger, PlayerSeat: 0 } prompt &&
            prompt.Choices.Any(option => option.Parameters.GetValueOrDefault("skill-id") == Yinghun));
        scene.Answer(option => option.Parameters.GetValueOrDefault("skill-id") == Yinghun &&
            option.Parameters.GetValueOrDefault("binding-id") == "draw-many" &&
            option.Parameters.GetValueOrDefault("program-action") == "activate");
        scene.Reach(() => scene.P?.SkillPrompt?.SkillId == Yinghun &&
            scene.P.Choices.Any(option => option.Targets.Contains(1)));
        scene.Answer(option => option.Targets.Contains(1));
        scene.Reach(() => scene.IsOwnPlay && scene.Game.State.TurnNumber >= 5);
        Need(scene.Game.CardMovements.Count(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(1) &&
                move.Reason.Value == "skill-program." + Yinghun + ".Draw") == 1 &&
            scene.Game.CardMovements.Count(move => move.From == CardLocation.Hand(1) && move.To == CardLocation.DiscardPile &&
                move.Reason.Value == "skill-program." + Yinghun + ".MoveBoundCards") == 1,
            "The actually acquired source108 Yinghun works in Shanhe: its selected other character draws X=1 and discards one real card.");
        scene.EndPlay(); scene.Reach(() => scene.IsOwnPlay && scene.Game.State.TurnNumber >= 7);
        Need(scene.Completed("wounded-preparation-awakening") == 1 && scene.Completed("awakening-turn-ending") == 1 &&
            scene.Events<MaximumHpChangedEvent>().Count(fact => fact.SkillId == Skill) == 1 && scene.Owner.MaxHp == 2 &&
            !scene.IsHunziChoice && scene.Game.State.Winner == Winner.None,
            "Acquired skills remain usable while neither the game-limited awakening nor its one-turn ending reward repeats.");
    }

    public static void EndingRecoveryRequestsTwoAndRespectsTheActualAwakenedMaximum()
    {
        foreach (var maxHpBonus in new[] { 0, 2 })
        {
            var scene = Scene.Start(maxHpBonus == 0 ? 2 : 3, maxHpBonus);
            scene.Hurt();
            var hp = maxHpBonus == 0 ? 1 : 2;
            var maximum = 2 + maxHpBonus;
            Need(scene.Owner.Hp == hp && scene.Owner.MaxHp == 3 + maxHpBonus,
                "The fresh source body takes real native damage before preparation; its only additional maximum is the explicit campaign bonus.");
            scene.EndPlay(); scene.Reach(() => scene.IsOwnPlay && scene.Game.State.TurnNumber >= 3);
            AssertAwakening(scene, hp, maximum);
            scene.EndPlay(); scene.Reach(() => scene.IsHunziChoice);
            var reward = scene.Game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Skill);
            var program = scene.Game.ContentRegistry.GetSkill(Skill).Program!;
            Need(reward.GameplayHash == program.GameplayHash && program.Triggers.Single(trigger => trigger.Id == "awakening-turn-ending")
                    .Effects.Single(effect => effect.Op == SkillProgramEffectOp.Recover).CompiledInstruction is
                    RecoverProgramInstruction { Amount: FixedProgramAmount { Value: 2 } },
                "The suspended real ending frame is bound to the compiled recovery-two instruction before its choice is answered.");
            var handBefore = scene.Owner.HandCount;
            scene.Answer(option => option.Parameters.GetValueOrDefault("option-id") == "recover-two");
            scene.Reach(() => scene.Completed("awakening-turn-ending") == 1);
            var receipt = scene.Events<ProgramRecoveryAppliedEvent>().Single(fact => fact.ProgramFrameId == reward.Id).Receipt;
            var actual = scene.Events<RecoveryAppliedEvent>().Single();
            var recovered = maximum - hp;
            Need(receipt.SourceSeat == 0 && receipt.TargetSeat == 0 && receipt.HpBefore == hp && receipt.HpAfter == maximum &&
                scene.Owner.Hp == maximum && scene.Owner.MaxHp == maximum && actual.SourceSeat == 0 && actual.TargetSeat == 0 &&
                actual.Amount == recovered && actual.RemainingHp == maximum && scene.Owner.HandCount == handBefore &&
                !scene.Game.CardMovements.Any(move => move.Reason.Value == "skill-program." + Skill + ".Draw"),
                "The native recovery reaches the actual awakened maximum without drawing: " + JsonSerializer.Serialize(new
                { maxHpBonus, receipt, actual, hp = scene.Owner.Hp, maxHp = scene.Owner.MaxHp, handBefore, handAfter = scene.Owner.HandCount }));
            if (maxHpBonus == 2)
                Need(receipt.RequestedAmount == 2 && actual.Amount == 2,
                    "With two missing HP the same real source skill must request and actually recover both HP: " + JsonSerializer.Serialize(receipt));
            else
                Need(receipt.RequestedAmount == 1 && actual.Amount == 1,
                    "Without recovery-amount modifiers the existing host clamps the recovery-two instruction to one missing HP before issuing its native invoice.");
            scene.Reach(() => scene.IsOwnPlay && scene.Game.State.TurnNumber >= 5);
            Need(scene.Completed("wounded-preparation-awakening") == 1 && scene.Completed("awakening-turn-ending") == 1 &&
                scene.Game.State.Winner == Winner.None && scene.Game.ResolutionStack.Count == 0,
                "The native health child returns to a living, actionable later turn without replaying the paid awakening.");
        }
    }

    private static void AssertAwakening(Scene scene, int hp, int maximum = 2)
    {
        var awakened = scene.Events<SkillAwakenedEvent>().Single(fact => fact.SkillId == Skill);
        var acquired = scene.Events<SkillsAcquiredEvent>().Single(fact => fact.SourceSkillId == Skill);
        Need(scene.Owner.Hp == hp && scene.Owner.MaxHp == maximum && awakened.PlayerSeat == 0 && awakened.MaximumHp == maximum &&
            awakened.AcquiredSkillIds.Order().SequenceEqual(new[] { StandardShanheSourceSunCePackage.YingziSkillId, Yinghun }.Order()) &&
            acquired.PlayerSeat == 0 && acquired.SkillIds.Order().SequenceEqual(awakened.AcquiredSkillIds.Order()) &&
            scene.Owner.Skills!.Any(skill => skill.ContentId == StandardShanheSourceSunCePackage.YingziSkillId) &&
            scene.Owner.Skills.Any(skill => skill.ContentId == Yinghun) && scene.Owner.HandLimit == scene.Owner.MaxHp &&
            scene.Completed("wounded-preparation-awakening") == 1,
            "Wounded preparation pays max-HP minus one once, emits the genuine awakening/acquisition facts, and installs source712 Yingzi plus source108 Yinghun on the actual body.");
    }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine Game { get; } = game;
        public PendingDecision? P => Game.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot Owner => Game.CreateSnapshot(0).Players[0];
        public bool IsOwnPlay => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && Game.ResolutionStack.Count == 0;
        public bool IsHunziChoice => P is { Kind: DecisionKind.ProgramTrigger, PlayerSeat: 0 } prompt &&
            prompt.SkillPrompt?.SkillId == Skill && prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("option-id") == "draw-three");
        public T[] Events<T>() => Game.Events.Select(envelope => envelope.Payload).OfType<T>().ToArray();
        public int Completed(string binding) => Events<ProgramBindingResolvedEvent>().Count(fact =>
            fact.SkillId == Skill && fact.BindingId == binding && fact.Completed && fact.Activated);
        public static Scene Start(int damage, int maxHpBonus = 0)
        {
            var setup = new ShanheBattleSetup(137, 1, "fixture:source-sunce", StandardShanheSourceSunCePackage.SunCeId,
                3 + maxHpBonus, maxHpBonus, 0, 0, 0, [Driver], [], [],
                [new("classic:ma-chao", MaxHpBonus: 16, Hp: 20) { StartingHand = [] }], [], "fixture:source-sunce");
            var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup with { ExtraSkillIds = [] })
                .WithExtension(new Fixture(damage), mode => mode.Id == ShanheBattleSetup.BattleModeId ? mode with { DeckId = Deck } : mode);
            var scene = new Scene(GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
                ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 8
            }, registry));
            scene.Submit(new StartGameCommand()); scene.Reach(() => scene.IsOwnPlay); return scene;
        }
        public void Hurt()
        {
            Submit(new UseProgramSkillCommand(0, Driver, "hurt", [], [], Game.Revision, P!.PromptId));
            Reach(() => IsOwnPlay);
        }
        public void EndPlay() => Submit(new EndPlayPhaseCommand(0, Game.Revision, P!.PromptId));
        public void Answer(Func<PromptChoice, bool> choose)
        {
            var prompt = P!; var choice = prompt.Choices.FirstOrDefault(choose);
            Need(choice is not null, "Missing native Sun Ce option: " + JsonSerializer.Serialize(prompt));
            Submit(new AnswerPromptCommand(0, prompt.PromptId, choice!.Id, Game.Revision));
        }
        public void Reach(Func<bool> stop)
        {
            for (var step = 0; step < 200; step++)
            {
                if (stop()) return;
                if (P is { PlayerSeat: 0 } prompt)
                {
                    var pass = prompt.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("program-action") == "skip" ||
                        choice.Cards.Count == 0 && choice.Parameters.GetValueOrDefault("response") is "pass" or "take-damage");
                    if (pass is not null)
                    { Submit(new AnswerPromptCommand(0, prompt.PromptId, pass.Id, Game.Revision)); continue; }
                    throw new InvalidOperationException("Unexpected native Sun Ce boundary: " + Diagnostic());
                }
                Submit(new AdvanceOneStepCommand(Game.Revision));
            }
            throw new InvalidOperationException("Sun Ce exceeded bounded native progression: " + Diagnostic());
        }
        private string Diagnostic() => JsonSerializer.Serialize(new
        {
            Game.State.TurnNumber, Game.State.Phase, Game.State.CurrentSeat, Game.State.Winner,
            Pending = P, Owner.Hp, Owner.MaxHp,
            Stack = Game.ResolutionStack.Select(frame => new { Type = frame.GetType().Name, frame.Id }),
            LastEvents = Game.Events.TakeLast(10).Select(envelope => new { Type = envelope.Payload.GetType().Name,
                Value = JsonSerializer.Serialize(envelope.Payload, envelope.Payload.GetType()) })
        });
        private void Submit(GameCommand command)
        {
            var result = Game.Submit(command);
            Need(result.Accepted, (result.Error?.Message ?? "Native source Sun Ce command rejected.") + " " + Diagnostic());
        }
    }

    private sealed class Fixture(int damage) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:source-sunce", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var rules = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion,
                skills = new[] { new { id = Driver, revision = 1, activations = new[] { new
                {
                    id = "hurt", minCards = 0, maxCards = 0, minTargets = 0, maxTargets = 0,
                    targetKind = "otherLiving", usesPerTurn = 1,
                    effects = new[] { new { op = "damage", target = "owner", amount = damage } }
                } } } } });
            var catalog = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Driver] = new { name = "真实受伤", description = "测试主体承受原生伤害。" } } }));
            builder.AddSkill(new(Driver, "真实受伤", "测试主体承受原生伤害。")
            { Program = catalog.Programs[Driver], ProgramPresentation = catalog.Presentations[Driver] });
            builder.AddDeck(new(Deck, "安全实体牌堆", 0, 0, [])
            { PhysicalCards = Enumerable.Range(0, 64).Select(index => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, index % 13 + 1)).ToArray() });
        }
    }

    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
}
