using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheDrawThresholdChecks
{
    private const string Tactic = "shanhe:current-shenlong-baiwei-2104", Tuxi = "classic:tuxi";
    private const string Owner = "fixture:draw-threshold-owner", Peer = "fixture:draw-threshold-peer";
    private const string Driver = "fixture:draw-threshold-driver", Quiet = "fixture:draw-threshold-quiet";
    private const string Deck = "fixture:draw-threshold-deck";

    public static void ActualDrawMaterialsCarryAcrossTurnsWithoutCountingGainsOrEarlierDraws()
    {
        var s = Scene.Start(Profile.Mixed);
        Need(s.View().GeneralId == Owner && s.View().Skills!.Any(skill => skill.ContentId == Tactic),
            "The unrelated printed general receives the real acquired draw-threshold tactic.");
        Need(s.OwnerInvoices().Select(item => item.ActualCount).SequenceEqual([3, 2]) &&
            s.OwnerInvoices()[0] is { TurnNumber: 0, GameStartingOrigin: not null } &&
            s.Game.CardMovements.Count(move => move.To == CardLocation.Hand(0) && move.Reason == CardMoveReasons.InitialDeal) == 3 &&
            s.Hits().Length == 0,
            "The real game-start bonus draws three and the ordinary Draw phase draws two; opening dealing and the recipe Peach do not become native draw invoices.", s.Details());
        s.Use("draw-two");
        s.Use("recast-one");
        Need(s.OwnerInvoices().Sum(item => item.ActualCount) == 8 && s.Events<CardRecastEvent>() is [{ ActorSeat: 0, DrawCount: 1 }] &&
            s.Events<NativeDrawThresholdProgressedEvent>().Last(item => item.OwnerSeat == 0 && item.Source.SkillId == Tactic) is { RemainderAfter: 8, Crossings: 0 } &&
            s.Hits().Length == 0,
            "A genuine skill draw and a paid native recast accumulate with the earlier normal draws, reaching eight actual materials without damage.", s.Details());
        var count = s.OwnerInvoices().Length;
        var hand = s.View().HandCount;
        s.Use("obtain-one");
        Need(s.View().HandCount == hand + 1 && s.OwnerInvoices().Length == count && s.Hits().Length == 0,
            "Revealing and obtaining one real DrawPile entity gains a Hand card without manufacturing a draw invocation or ninth-card credit.");
        var turn = s.Game.State.TurnNumber;
        s.ReplaceNextNormalDraw = true;
        s.NextOwnPlay();
        var taken = s.Events<ProgramRandomHandCardsTakenEvent>().Single(item => item.SkillId == Tuxi);
        Need(s.Game.State.TurnNumber > turn && taken.CardCount == 2 && taken.TargetSeats.SequenceEqual([2, 3]) &&
            s.OwnerInvoices().Length == count && s.Hits().Length == 0,
            "The next actual turn preserves eight credit; real Tuxi obtains two enemy Hand entities instead of the normal draw and neither gain is counted.", s.Details());
        s.Use("draw-one");
        Need(s.Hits() is [{ SourceSeat: 0, TargetSeat: 2 or 3, Amount: 1, Nature: DamageNature.Normal, SourceCardUseResolutionId: null }] &&
            s.OwnerInvoices().Sum(item => item.ActualCount) == 9 &&
            s.Events<ProgramBindingResolvedEvent>().Count(item => item.SkillId == Tactic && item.Activated && item.Completed) == 1,
            "The ninth actual entity after a real turn boundary causes one ordinary cardless one-point hit to an enemy, once.", s.Details());
        s.AssertActualMaterials();

        var late = Scene.Start(Profile.Late);
        late.Use("draw-four");
        Need(late.OwnerInvoices().Sum(item => item.ActualCount) == 9 && late.Hits().Length == 0,
            "Nine real cards drawn before the actual grant issue no acquired-source opportunity.");
        late.Use("grant-tactic");
        late.Use("draw-eight");
        Need(late.View().Skills!.Any(skill => skill.ContentId == Tactic) && late.Hits().Length == 0,
            "An actual native grant starts its own instance at zero rather than retroactively using the earlier nine draws.", late.Details());
        late.Use("draw-one");
        Need(late.Hits() is [{ SourceSeat: 0, Amount: 1 }] && late.OwnerInvoices().Sum(item => item.ActualCount) == 18 &&
            late.Events<NativeDrawThresholdProgressedEvent>().Where(item => item.OwnerSeat == 0 && item.Source.SkillId == Tactic).Sum(item => item.ActualCount) == 9,
            "The newly granted instance first triggers after its own ninth actual draw.", late.Details());
        late.AssertActualMaterials();
    }

    public static void LargeDamageTriggeredDrawIssuesSeparateHitsAndReturnsToItsNativeParent()
    {
        var s = Scene.Start(Profile.Large);
        Need(s.OwnerInvoices().Select(item => item.ActualCount).SequenceEqual([6, 2]) && s.Hits().Length == 0 &&
            s.View().TeamId == s.View(1).TeamId && s.View().TeamId != s.View(2).TeamId && s.View().TeamId != s.View(3).TeamId,
            "Six real game-start cards and two ordinary draws leave eight credit with a real ally and two real enemies.", s.Details());
        s.Use("hit-me", [2]);
        var invoices = s.OwnerInvoices();
        var draw = invoices.Single(item => item.ActualCount == 20);
        var hits = s.Hits();
        var choices = s.Events<NativeDrawThresholdTargetChosenEvent>();
        var parent = s.Events<ProgramBindingResolvedEvent>().Single(item => item.SkillId == Driver && item.BindingId == "draw-after-damage");
        Need(invoices.Select(item => item.ActualCount).SequenceEqual([6, 2, 20]) && draw.Requested == 20 &&
            s.Events<NativeDrawThresholdProgressedEvent>().Single(item => item.OwnerSeat == 0 && item.ActualCount == 20 && item.Source.SkillId == Tactic) is
                { RemainderBefore: 8, Crossings: 3, RemainderAfter: 1 } &&
            draw.Reason.Value == $"skill-program.{Driver}.{SkillProgramEffectOp.Draw}" && draw.ParentFrameId == parent.FrameId &&
            parent is { Activated: true, Completed: true } && hits.Length == 3 &&
            hits.All(hit => hit.SourceSeat == 0 && hit.TargetSeat is 2 or 3 && hit.Amount == 1 &&
                hit.Nature == DamageNature.Normal && hit.SourceCardUseResolutionId is null) &&
            s.Events<DamageAppliedEvent>().Single(hit => hit.TargetSeat == 0) is { SourceSeat: 2, Amount: 1 } &&
            s.View().Hp == 29 && s.View(1).Hp == 30 && 60 - s.View(2).Hp - s.View(3).Hp == 3,
            "One real twenty-material after-damage draw crosses three thresholds from remainder eight, producing three independent native one-point hits only to enemies before returning to the original damage-triggered draw.", s.Details());
        Need(s.Events<ProgramBindingResolvedEvent>().Where(item => item.SkillId == Tactic).ToArray() is { Length: 3 } bindings &&
            bindings.All(item => item.Activated && item.Completed) && bindings.Select(item => item.FrameId).Distinct().Count() == 3 &&
            choices.Length == 3 && choices.Select(item => item.OccurrenceIndex).SequenceEqual([0, 1, 2]) &&
            choices.All(item => item.InvocationId == draw.InvocationId && item.Source.SkillId == Tactic) &&
            choices.Select(item => item.TargetSeat).SequenceEqual(hits.Select(item => item.TargetSeat)) &&
            choices.Select(item => item.ProgramFrameId).SequenceEqual(bindings.Select(item => item.FrameId)) &&
            s.Events<ProgramSkillResolvedEvent>().Single(item => item.SkillId == Driver && item.ActivationId == "hit-me").Completed &&
            s.Game.ResolutionStack.All(frame => frame is not ProgramSkillFrame) &&
            s.Events<DamageRequestedEvent>().Count(item => item.SourceSeat == 0 && item.SourceCard is null && item.Amount == 1) == 3,
            "Each threshold owns a separate completed candidate; the original active damage command resumes once without duplicate draws, damage, or retained program frames.", s.Details());
        s.AssertActualMaterials();
    }

    private static void Need(bool value, string message, object? details = null)
    { if (!value) throw new InvalidOperationException(details is null ? message : message + " " + JsonSerializer.Serialize(details)); }
    private enum Profile { Mixed, Large, Late }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine Game { get; } = game;
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
                    var choice = prompt.Choices.FirstOrDefault(item => item.Parameters.GetValueOrDefault("program-action") ==
                            (ReplaceNextNormalDraw ? "activate" : "decline"))
                        ?? prompt.Choices.FirstOrDefault(item => item.Parameters.GetValueOrDefault("program-action") == "select-targets" && item.Targets.SequenceEqual([2, 3]))
                        ?? prompt.Choices.FirstOrDefault(item => item.Parameters.GetValueOrDefault("response") is "pass" or "no-nullification" or "take-damage" or "take-duel-damage")
                        ?? prompt.Choices.FirstOrDefault(item => item.Cards.Count == 1)
                        ?? prompt.Choices[0];
                    Accept(new AnswerPromptCommand(0, prompt.PromptId, choice.Id, Game.Revision));
                }
                else Accept(new AdvanceOneStepCommand(Game.Revision));
            }
            throw new InvalidOperationException("Missing bounded draw-threshold boundary: " + JsonSerializer.Serialize(Prompt));
        }
        private void Play() => Reach(() => Prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Use(string activation, int[]? targets = null)
        { Accept(new UseProgramSkillCommand(0, Driver, activation, [], targets ?? [], Game.Revision, Prompt!.PromptId)); Play(); }
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
        public static Scene Start(Profile profile)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheDrawThresholdPackage(), new Fixture(profile));
            var setup = new ShanheBattleSetup(83, 1, "fixture:draw-threshold", Owner, 30, 0, 0, 0, 0,
                profile == Profile.Mixed ? [Tuxi] : [], [new(CardKind.Peach, Suit.Heart, 7)], [],
                [new(Peer) { StartingHand = [] }, new(Peer) { StartingHand = [] }], [new(Peer) { StartingHand = [] }], new string('9', 64))
                { TacticSkillIds = profile == Profile.Late ? [] : [Tactic] };
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
            CharacterFixtureContent.Register(builder, "classic:zhang-liao");
            var afterDamage = profile == Profile.Large ? "," + """
                {"id":"draw-after-damage","window":"afterDamageApplied","subject":"owner","damageOccurrence":"perDamage","optional":false,"usageScope":"game","usageLimit":1,
                 "effects":[{"op":"draw","target":"owner","amount":20}]}
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
                   "triggers":[{"id":"opening-real-draw","window":"gameStarting","subject":"owner","optional":false,"effects":[{"op":"draw","target":"owner","amount":{{{(profile == Profile.Large ? 6 : 3)}}}}]}{{{afterDamage}}}]},
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
            builder.AddGeneral(new(Owner, "独立神龙摆尾持有者", "supporter", Driver, "shu", 30));
            builder.AddGeneral(new(Peer, "真实队伍成员", "supporter", Quiet, "wei", 30));
            builder.AddDeck(new(Deck, "固定真实实体闪", 2, 2, [])
            { PhysicalCards = Enumerable.Range(0, 128).Select(index => new ContentDeckPhysicalCard("standard:dodge", Suit.Diamond, index % 13 + 1)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "真实摸牌张数阈值", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
