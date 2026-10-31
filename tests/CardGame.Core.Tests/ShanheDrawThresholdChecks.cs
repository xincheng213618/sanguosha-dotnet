using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheDrawThresholdChecks
{
    private const string Tactic = "shanhe:current-shenlong-baiwei-2104", Tuxi = "classic:tuxi";
    private const string Niepan = StandardShanheS5YuduPackage.NiepanId;
    private const string Owner = "fixture:draw-threshold-owner", Peer = "fixture:draw-threshold-peer";
    private const string Driver = "fixture:draw-threshold-driver", Quiet = "fixture:draw-threshold-quiet";
    private const string Deck = "fixture:draw-threshold-deck";
    private const string CurrentTactic = "shanhe:current-shenlong-baiwei-2143", Jieming = "boundary:jieming-current";

    public static void NativePaidDrawsInsertThresholdDamageAndReturnToOriginalDamageCold()
    {
        foreach (var profile in new[] { Profile.Capped, Profile.NativeBenefit })
        foreach (var nestedVictimDies in new[] { false, true })
        {
            var s = Scene.Start(profile, nestedVictimDies);
            Need(s.Hits().Length == 0, "The fixed opening stays below the real draw threshold.");
            s.HitThroughNativePaidDraw(profile);
            if (nestedVictimDies)
                Need(s.Game.ResolutionStack.OfType<ProgramSkillFrame>().Any(frame =>
                        frame.AttackAttempt is not null && frame.AttackReturn?.Insertion is not null),
                    "The real inserted lethal hit uses the shared scheduler scope rather than a skill-specific permission.");
            var reason = profile == Profile.Capped ? "program.capped-hand-refresh.draw" : "program.applied-damage-benefit.draw";
            var invoice = s.OwnerInvoices().Single(item => item.Reason.Value == reason);
            var target = s.Events<NativeDrawThresholdTargetChosenEvent>().Single();
            Need(invoice.ProgramOrigin is { } origin && origin.Source.OwnerSeat == 0 &&
                origin.Operation == (profile == Profile.Capped ? SkillProgramEffectOp.DrawThenDiscardHandToMaximumHp : SkillProgramEffectOp.DrawOwnerAtAppliedDamage),
                "Every native draw retains its actual issuing skill instance, content hash and paused instruction.");
            Need(invoice.ActualCount == (profile == Profile.Capped ? 5 : 4) && target.InvocationId == invoice.InvocationId &&
                s.Hits() is [{ SourceSeat: 0, TargetSeat: 2 or 3, Amount: 1 }] &&
                s.Events<DamageAppliedEvent>().Single(item => item.TargetSeat == 0) is { SourceSeat: 2, Amount: 1 },
                "A paid native instruction's real draw inserts one enemy-only threshold hit inside the original sourced damage.");
            s.Cold();
            s.FinishNativePaidDraw();
            Need(s.OwnerInvoices().Count(item => item.Reason.Value == reason) == 1 &&
                s.Events<NativeDrawThresholdTargetChosenEvent>().Length == 1 && s.Hits().Length == 1 &&
                s.Events<PlayerDiedEvent>().Length == (nestedVictimDies ? 1 : 0) &&
                s.Game.ResolutionStack.All(frame => frame is not DamageFrame and not DamageTriggerWindowFrame and not ProgramSkillFrame) &&
                !s.Game.IsFaulted,
                "Cold recovery never repeats the paid draw or inserted hit, and every child returns to the original Play prompt.");
            if (profile == Profile.Capped)
                Need(s.View().HandCount == 5 && s.Events<ProgramCappedHandRefreshCompletedEvent>() is [{ Completed: true }],
                    "Jieming's target-owned discard completes only after the inserted damage returns and retains the original five-card cap.");
            s.AssertActualMaterials();
            if (profile == Profile.NativeBenefit) s.AssertProgramOriginCannotBeReassigned(invoice);
            s.Cold();
        }
    }

    public static void NiepanDyingDrawCrossesOneThresholdAndResumesOriginalDamage()
    {
        foreach (var nestedVictimDies in new[] { false, true })
        {
            var s = Scene.Start(Profile.Niepan, nestedVictimDies);
            Need(s.View() is { Hp: 1, MaxHp: 30 } && s.View().Skills!.Any(skill => skill.ContentId == Niepan) &&
                s.View().Skills!.Any(skill => skill.ContentId == Tactic) &&
                s.OwnerInvoices().Select(item => item.ActualCount).SequenceEqual([4, 2]) && s.Hits().Length == 0,
                "The unrelated acquired owner starts with production Niepan and Shenlong Baiwei, six actual drawn materials and one real remaining HP.", s.Details());
            Need(s.View(2).Hp == (nestedVictimDies ? 1 : 30) && s.View(3).Hp == (nestedVictimDies ? 1 : 30),
                "Both genuine enemies have the scenario's legal initial HP, so every seeded target of the inner hit either survives or genuinely enters dying without selecting a convenient seed.");
            var originalParent = s.HitThroughNiepan();
            var draw = s.OwnerInvoices().Single(item => item.Reason.Value == $"skill-program.{Niepan}.{SkillProgramEffectOp.Draw}");
            var rescue = s.Events<ProgramBindingResolvedEvent>().Single(item => item.SkillId == Niepan);
            var hit = s.Hits();
            Need(s.OwnerInvoices().Select(item => item.ActualCount).SequenceEqual([4, 2, 3]) &&
                draw.ActualCount == 3 && draw.ParentFrameId == rescue.FrameId &&
                s.Events<NativeDrawThresholdProgressedEvent>().Single(item => item.OwnerSeat == 0 && item.InvocationId == draw.InvocationId) is
                    { RemainderBefore: 6, ActualCount: 3, Crossings: 1, RemainderAfter: 0 } &&
                hit is [{ SourceSeat: 0, TargetSeat: 2 or 3, Amount: 1, Nature: DamageNature.Normal, SourceCardUseResolutionId: null }] &&
                s.Events<DamageAppliedEvent>().Single(item => item.TargetSeat == 0) is { SourceSeat: 2, Amount: 1 } &&
                s.View() is { Hp: 3, IsAlive: true, HandCount: 3 } && s.View(1).Hp == 30 &&
                s.Events<RecoveryAppliedEvent>() is [{ SourceSeat: 0, TargetSeat: 0, Amount: 3, RemainingHp: 3 }],
                "The real enemy-sourced lethal damage enters dying; production Niepan discards, truly draws three, recovers to three, and its ninth drawn entity produces one ordinary enemy-only threshold damage rather than dropping the opportunity during rescue.", s.Details());
            Need(rescue is { OwnerSeat: 0, Window: SkillProgramTriggerWindow.SelfDyingResponse, Activated: true, Completed: true } &&
                s.Events<ProgramBindingResolvedEvent>().Where(item => item.SkillId == Tactic).ToArray() is [{ Activated: true, Completed: true }] &&
                s.Events<DyingResolvedEvent>().Count(item => item.VictimSeat == 0 && item.Survived) == 1 &&
                (nestedVictimDies
                    ? s.Events<DyingResolvedEvent>() is [{ VictimSeat: 2 or 3, Survived: false }, { VictimSeat: 0, Survived: true }] &&
                        s.Events<PlayerDiedEvent>() is [{ KillerSeat: 0 }] &&
                        s.Events<PlayerDiedEvent>()[0].VictimSeat == hit[0].TargetSeat && !s.View(hit[0].TargetSeat).IsAlive &&
                        s.View(hit[0].TargetSeat == 2 ? 3 : 2) is { Hp: 1, IsAlive: true } && s.Game.State.Winner == Winner.None
                    : s.Events<DyingResolvedEvent>() is [{ VictimSeat: 0, Survived: true }] && s.Events<PlayerDiedEvent>().Length == 0) &&
                s.Events<SkillUsageConsumedEvent>().Where(item => item.SkillId == Niepan).ToArray() is [{ SkillOwnerSeat: 0, Scope: SkillUsageScope.Game, Count: 1 }] &&
                s.Events<ProgramSkillResolvedEvent>().Single(item => item.SkillId == Driver && item.ActivationId == "hit-me") is { Completed: true } parent &&
                parent.FrameId == originalParent && s.Game.ResolutionStack.All(frame => frame is not ProgramSkillFrame and not DyingFrame and not DamageFrame),
                "Exactly one limited Niepan and one threshold candidate complete. When the enemy starts at one HP, its own genuine inner dying and death finish before the suspended original dying survives; another enemy remains alive. The original lethal command returns once with no rescue, damage or program frame left behind.", s.Details());
            s.AssertActualMaterials();
        }
    }

    private static void Need(bool value, string message, object? details = null)
    { if (!value) throw new InvalidOperationException(details is null ? message : message + " " + JsonSerializer.Serialize(details)); }
    private enum Profile { Mixed, Large, Late, Niepan, Capped, NativeBenefit }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine Game { get; private set; } = game;
        public bool ReplaceNextNormalDraw { get; set; }
        private PendingDecision? Prompt => Game.PendingDecision;
        public PlayerSnapshot View(int seat = 0) => Game.CreateSnapshot(seat).Players[seat];
        public T[] Events<T>() => Game.Events.Select(item => item.Payload).OfType<T>().ToArray();
        public NativeDrawInvocationRecordedEvent[] OwnerInvoices() => Events<NativeDrawInvocationRecordedEvent>().Where(item => item.OwnerSeat == 0).ToArray();
        public DamageAppliedEvent[] Hits() => Events<DamageAppliedEvent>().Where(item => item.SourceSeat == 0).ToArray();
        public object Details() => new { Invoices = OwnerInvoices(), Hits = Events<DamageAppliedEvent>(), Bindings = Events<ProgramBindingResolvedEvent>(), Owner = View() };
        private void Accept(GameCommand command)
        { var result = Game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected real draw-threshold command."); }
        private void Reach(Func<bool> ready)
        {
            for (var step = 0; step < 240; step++)
            {
                if (ready()) return;
                Need(Game.State.Winner == Winner.None, "The bounded draw-threshold scene ended before its expected boundary.");
                if (Prompt is { PlayerSeat: 0, Choices.Count: > 0 } prompt)
                {
                    Need(prompt.Kind != DecisionKind.PlayCard, "Unexpected ordinary Play while a native draw-threshold child is pending.");
                    var choice = prompt.Kind == DecisionKind.RescueDying
                        ? prompt.Choices.SingleOrDefault(item => item.Parameters.GetValueOrDefault("response") == "let-die")
                        : prompt.Choices.FirstOrDefault(item => item.Parameters.GetValueOrDefault("program-action") ==
                            (ReplaceNextNormalDraw ? "activate" : "decline"))
                        ?? prompt.Choices.FirstOrDefault(item => item.Parameters.GetValueOrDefault("program-action") == "select-targets" && item.Targets.SequenceEqual([2, 3]))
                        ?? prompt.Choices.FirstOrDefault(item => item.Parameters.GetValueOrDefault("response") is "pass" or "no-nullification" or "take-damage" or "take-duel-damage")
                        ?? prompt.Choices.FirstOrDefault(item => item.Cards.Count == 1)
                        ?? prompt.Choices[0];
                    if (prompt.Kind == DecisionKind.RescueDying)
                        Need(Game.ResolutionStack.OfType<DyingFrame>().LastOrDefault()?.VictimSeat is 2 or 3 && choice is not null,
                            "The human explicitly declines rescue only for the real inner enemy dying window; the original owner Niepan prompt is chosen separately.", Details());
                    Accept(new AnswerPromptCommand(0, prompt.PromptId, choice!.Id, Game.Revision));
                }
                else Accept(new AdvanceOneStepCommand(Game.Revision));
            }
            throw new InvalidOperationException("Missing bounded draw-threshold boundary: " + JsonSerializer.Serialize(Prompt));
        }
        private void Play() => Reach(() => Prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Use(string activation, int[]? targets = null)
        { Accept(new UseProgramSkillCommand(0, Driver, activation, [], targets ?? [], Game.Revision, Prompt!.PromptId)); Play(); }
        public void HitThroughNativePaidDraw(Profile profile)
        {
            Accept(new UseProgramSkillCommand(0, Driver, "hit-me", [], [2], Game.Revision, Prompt!.PromptId));
            if (profile == Profile.Capped)
            {
                Reach(() => Prompt?.SkillPrompt?.SkillId == Jieming &&
                    Prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate"));
                Answer(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
                Reach(() => Prompt?.SkillPrompt?.SkillId == Jieming && Prompt.Choices.Any(choice => choice.Targets.SequenceEqual([0])));
                Cold();
                Answer(choice => choice.Targets.SequenceEqual([0]));
            }
            Reach(() => Events<NativeDrawThresholdTargetChosenEvent>().Length == 1);
        }
        public void FinishNativePaidDraw() => Play();
        public void Cold()
        {
            var checkpoint = GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(Game.CreateCheckpoint()));
            var registry = Game.ContentRegistry;
            var restored = GameReplay.Restore(checkpoint, registry);
            Need(SnapshotJson.Serialize(restored.CreateSnapshot(0)) == SnapshotJson.Serialize(Game.CreateSnapshot(0)) &&
                GameCheckpointJson.Serialize(restored.CreateCheckpoint()) == GameCheckpointJson.Serialize(Game.CreateCheckpoint()),
                "The native draw, pending target discard and nested damage return restore from the unchanged command prefix.");
            Game = restored;
        }
        public void AssertProgramOriginCannotBeReassigned(NativeDrawInvocationRecordedEvent invoice)
        {
            var materials = Game.CardMovements.Where(move => move.Sequence > invoice.SequenceBefore && move.Sequence <= invoice.SequenceAfter &&
                    move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(invoice.OwnerSeat) && move.Reason == invoice.Reason)
                .Select(move => new NativeDrawMaterial(move.CardId, move.CardKind, move.Sequence)).ToArray();
            var proof = new NativeDrawInvocationProof
            {
                InvocationId = invoice.InvocationId, ParentFrameId = invoice.ParentFrameId, OwnerSeat = invoice.OwnerSeat,
                RequestedCount = invoice.Requested, ActualTurnNumber = invoice.TurnNumber, ActualTurnOwnerSeat = invoice.ActualTurnOwnerSeat,
                PhaseActorSeat = invoice.PhaseActorSeat, Phase = invoice.Phase, DirectProducer = invoice.DirectProducer,
                ProgramOrigin = invoice.ProgramOrigin, Reason = invoice.Reason, SequenceBefore = invoice.SequenceBefore,
                SequenceAfter = invoice.SequenceAfter, LastBatchId = invoice.LastBatchId, Materials = materials,
                ThresholdProgress = Events<NativeDrawThresholdProgressedEvent>().Where(item => item.InvocationId == invoice.InvocationId)
                    .Select(item => new NativeDrawThresholdProgress(item.Source, item.GameplayHash, item.Threshold,
                        item.RemainderBefore, item.ActualCount, item.Crossings, item.RemainderAfter)).ToArray()
            };
            var validate = global::FixtureRuntimeAccess.Engine.GetMethod("ValidNativeDrawInvocation",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!;
            bool Valid(NativeDrawInvocationProof value) => (bool)validate.Invoke(Game, [value])!;
            var origin = proof.ProgramOrigin!;
            Need(Valid(proof) && !Valid(proof with { ProgramOrigin = null }) &&
                !Valid(proof with { ProgramOrigin = origin with { InstructionIndex = origin.InstructionIndex + 1 } }) &&
                !Valid(proof with { ProgramOrigin = origin with { GameplayHash = "forged" } }) &&
                !Valid(proof with { ProgramOrigin = origin with { Source = origin.Source with { SkillInstanceId = "other-instance" } } }),
                "The shared invoice accepts its real native producer and rejects removing or reassigning its instruction, content or instance.");
        }
        private void Answer(Func<PromptChoice, bool> choose) =>
            Accept(new AnswerPromptCommand(0, Prompt!.PromptId, Prompt.Choices.First(choose).Id, Game.Revision));
        public long HitThroughNiepan()
        {
            Accept(new UseProgramSkillCommand(0, Driver, "hit-me", [], [2], Game.Revision, Prompt!.PromptId));
            Reach(() => Prompt?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("response") == "program-trigger" &&
                choice.Parameters.GetValueOrDefault("skill-id") == Niepan) == true);
            Need(View().Hp == 0 && Game.ResolutionStack.OfType<DyingFrame>().Any(frame => frame.VictimSeat == 0),
                "A real sourced one-point damage reaches the native dying prompt before Niepan is chosen.", Details());
            var parent = Game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Driver && frame.ActivationId == "hit-me").Id;
            var choice = Prompt!.Choices.Single(item => item.Parameters.GetValueOrDefault("response") == "program-trigger" &&
                item.Parameters.GetValueOrDefault("skill-id") == Niepan);
            Accept(new AnswerPromptCommand(0, Prompt.PromptId, choice.Id, Game.Revision));
            Play();
            return parent;
        }
        public void NextOwnPlay()
        {
            var turn = Game.State.TurnNumber;
            Accept(new EndPlayPhaseCommand(0, Game.Revision, Prompt!.PromptId));
            Reach(() => Game.State.TurnNumber > turn && Prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        }
        public void AssertActualMaterials()
        {
            var invoices = OwnerInvoices();
            Need(invoices.Select(item => item.InvocationId).Distinct().Count() == invoices.Length && invoices.All(item =>
                Game.CardMovements.Where(move => move.Sequence > item.SequenceBefore && move.Sequence <= item.SequenceAfter &&
                    move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0) && move.Reason == item.Reason)
                    .Select(move => move.CardId).Distinct().Count() == item.ActualCount),
                "Every counted invoice occurs once and its ActualCount equals distinct real DrawPile-to-owner material movements.", Details());
        }
        public static Scene Start(Profile profile, bool nestedVictimDies = false)
        {
            var registry = profile is Profile.Niepan or Profile.Capped
                ? ContentRegistry.Build(new StandardContentPackage(), new StandardActiveSkillExpansionPackage(includeJijiu: true),
                    new StandardRescueSkillExpansionPackage(), new StandardClassicGeneralPackage(),
                    new StandardShanheS5YuduPackage(), new StandardShanheDrawThresholdPackage(), new Fixture(profile))
                : ContentRegistry.Build(new StandardContentPackage(), new StandardShanheDrawThresholdPackage(), new Fixture(profile));
            var setup = new ShanheBattleSetup(83, 1, "fixture:draw-threshold", Owner, profile == Profile.Niepan ? 1 : profile == Profile.Capped ? 5 : 30, 0, 0, 0, 0,
                profile == Profile.Mixed ? [Tuxi] : profile == Profile.Niepan ? [Niepan] : profile == Profile.Capped ? [Jieming] : [], [new(CardKind.Peach, Suit.Heart, 7)], [],
                [new(Peer, Hp: nestedVictimDies ? 1 : null) { StartingHand = [] }, new(Peer, Hp: nestedVictimDies ? 1 : null) { StartingHand = [] }],
                [new(Peer) { StartingHand = [] }], new string('9', 64))
                { TacticSkillIds = profile == Profile.Late ? [] : profile == Profile.Capped ? [CurrentTactic] : [Tactic] };
            var scene = new Scene(GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 8, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, registry));
            scene.Accept(new StartGameCommand()); scene.Play(); return scene;
        }
    }

    private sealed class Fixture(Profile profile) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shanhe-draw-threshold", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            if (profile is not (Profile.Niepan or Profile.Capped)) CharacterFixtureContent.Register(builder, "classic:zhang-liao");
            var afterDamage = profile == Profile.Large ? "," + """
                {"id":"draw-after-damage","window":"afterDamageApplied","subject":"owner","damageOccurrence":"perDamage","optional":false,"usageScope":"game","usageLimit":1,
                 "effects":[{"op":"draw","target":"owner","amount":20}]}
                """ : profile == Profile.NativeBenefit ? "," + """
                {"id":"native-draw-after-damage","window":"afterDamageApplied","subject":"owner","damageOccurrence":"perDamage","optional":false,"usageScope":"game","usageLimit":1,
                 "effects":[{"op":"drawOwnerAtAppliedDamage","target":"owner","amount":4},
                    {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},
                     "cardOwnerRef":{"kind":"eventTarget"},"zones":["equipment","judgment"],"count":1,
                     "destination":"discardPile","skipIfNoCards":true,"awaitMovementTriggers":true}]}
                """ : "";
            var rules = $$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
                  {"id":"{{{Driver}}}","revision":1,"activations":[
                    {"id":"draw-one","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"draw","target":"owner","amount":1}]},
                    {"id":"draw-two","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"draw","target":"owner","amount":2}]},
                    {"id":"draw-four","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"draw","target":"owner","amount":4}]},
                    {"id":"draw-eight","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"draw","target":"owner","amount":8}]},
                    {"id":"grant-tactic","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"grantSkills","target":"owner","skillIds":["{{{Tactic}}}"]}]},
                    {"id":"obtain-one","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"revealTopCards","target":"owner","amount":1,"resultBind":"ordinary-gain","visibility":"public"},{"op":"moveBoundCards","target":"owner","sourceBind":"ordinary-gain","destination":"ownerHand"}]},
                    {"id":"recast-one","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"selectOwnedCards","target":"owner","minimumCards":1,"maximumCards":1,"zones":["hand"],"resultBind":"recast"},{"op":"recastBoundCards","target":"owner","sourceBind":"recast"}]},
                    {"id":"hit-me","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"owner","sourceRef":{"kind":"selectedTarget"},"amount":1}]}],
                   "triggers":[{"id":"opening-real-draw","window":"gameStarting","subject":"owner","optional":false,"effects":[{"op":"draw","target":"owner","amount":{{{(profile == Profile.Large ? 6 : profile == Profile.Niepan ? 4 : 3)}}}}]}{{{afterDamage}}}]},
                  {"id":"{{{Quiet}}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}
                ]}
                """;
            var programs = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new[] { Driver, Quiet }.ToDictionary(id => id, _ => new { name = "真实摸牌与伤害驱动", description = "开局、阶段、重铸、替换获牌与伤后摸牌" })
            }));
            foreach (var (id, program) in programs.Programs)
                builder.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = programs.Presentations[id] });
            builder.AddGeneral(new(Owner, "独立神龙摆尾持有者", "supporter", Driver, "shu", profile == Profile.Capped ? 5 : 30));
            builder.AddGeneral(new(Peer, "真实队伍成员", "supporter", Quiet, "wei", 30));
            builder.AddDeck(new(Deck, "固定真实实体闪", 2, 2, [])
            { PhysicalCards = Enumerable.Range(0, 128).Select(index => new ContentDeckPhysicalCard("standard:dodge", Suit.Diamond, index % 13 + 1)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "真实摸牌张数阈值", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
