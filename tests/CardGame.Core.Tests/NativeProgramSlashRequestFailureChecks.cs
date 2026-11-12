using System.Text.Json;
using System.Text.Json.Nodes;
using CardGame.Content.Standard;
using CardGame.Core;
using static DrawFundedBasicFixture;

internal static class NativeProgramSlashRequestFailureChecks
{
    public static void PaidDrawChildCancelsOriginalRequestWithoutInventingUseCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, ModeId = Mode,
            UseInteractiveSetup = true, UseInteractiveDiscard = true,
            AdvanceAfterHumanCommands = false, MaxTurns = 20
        }, registry);
        Accept(game, new StartGameCommand());
        Reach(game, p => p is { Kind: DecisionKind.SelectGeneral, PlayerSeat: 0 });
        Accept(game, new SelectGeneralCommand(0, "fixture:dfb-owner", game.Revision, P(game)!.PromptId));
        ReachPlay(game, selectGenerals: true);
        Use(game, "request", [1]);
        Reach(game, p => p is { PlayerSeat: 0, Kind: DecisionKind.ProgramTrigger } &&
            p.Choices.Any(c => c.Targets.SequenceEqual([2])));
        Answer(game, c => c.Targets.SequenceEqual([2]));
        Reach(game, p => p is { PlayerSeat: 1, Kind: DecisionKind.ProgramTrigger } &&
            p.Choices.Any(c => c.Parameters.GetValueOrDefault("draw-funded-basic") == DrawFundedDistinctBasicIntent.AssistedSlash.ToString()));
        var original = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Driver);
        Require(original.AssistedSlashRequest is { ActorSeat: 1, TargetSeat: 2 } &&
            V(game, 1) is { Hp: 4, HandCount: 4 },
            "The actual selected actor owns the original assisted request before paying its private draw cost.");
        Answer(game, c => c.Parameters.GetValueOrDefault("draw-funded-basic") == DrawFundedDistinctBasicIntent.AssistedSlash.ToString() &&
            c.Targets.SequenceEqual([2]));
        Reach(game, p => p.SkillPrompt?.SkillId == Gain && p.PlayerSeat == 1);
        var paid = game.ResolutionStack.OfType<DrawFundedDistinctBasicFrame>().Single();
        var acceptance = paid.Payment.RequestAcceptance;
        Require(paid.Payment is { Intent: DrawFundedDistinctBasicIntent.AssistedSlash, ActualDrawCount: 1, TargetSeat: 2 } &&
            acceptance is { Identity.ActorSeat: 1, TargetSeat: 2 } && acceptance.Identity.FrameId == original.Id &&
            acceptance.Identity.Spec.FailureReturnPolicy == ProgramSlashRequestFailureReturnPolicy.CancelBinding &&
            V(game, 1).HandCount == 5 && !F<DrawFundedDistinctBasicIssuedEvent>(game).Any(),
            "The genuine paid producer freezes the original request acceptance while its actual gain child is still unresolved.");
        game = Cold(game, registry);
        Continue(game);
        Reach(game, p => p.SkillPrompt?.SkillId == Gain && p.PlayerSeat == 1 &&
            p.Choices.Any(c => c.Targets.SequenceEqual([2])));
        Answer(game, c => c.Targets.SequenceEqual([2]));
        ReachPlay(game);
        Require(!V(game, 2).IsAlive && V(game, 0).IsAlive && V(game, 1).IsAlive && V(game, 3).IsAlive &&
            V(game, 1).HandCount == 5 &&
            F<DrawFundedDistinctBasicPaidEvent>(game).Count(e => e.PaymentFrameId == paid.Id && e.ActualDrawCount == 1) == 1 &&
            F<DrawFundedDistinctBasicCancelledEvent>(game).Count(e => e.PaymentFrameId == paid.Id) == 1 &&
            !F<DrawFundedDistinctBasicIssuedEvent>(game).Any(e => e.PaymentFrameId == paid.Id) &&
            !F<DrawFundedDistinctBasicReturnedEvent>(game).Any(e => e.PaymentFrameId == paid.Id) &&
            !F<ProgramOptionChosenEvent>(game).Any(e => e.FrameId == original.Id &&
                (e.ResultBind == "after-request" || e.OptionId == "used-slash")) &&
            F<ProgramSkillResolvedEvent>(game).Count(e => e.FrameId == original.Id && !e.Completed) == 1 &&
            !game.ResolutionStack.Any(f => f.Id == original.Id || f.Id == paid.Id) &&
            !game.ResolutionStack.OfType<CardUseFrame>().Any(),
            "The paid target death cancels the exact original binding, keeps its real cost, restores Play and invents neither a Slash nor a successful request suffix.");
        Require(game.CardMovements.Count(m => m.Sequence > paid.Payment.SequenceBefore && m.Sequence <= paid.Payment.SequenceAfter &&
            m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(1) &&
            m.Reason.Value == "skill-program.draw-funded-distinct-basic.draw") == 1,
            "The failed original request neither refunds nor repeats the producer's one physical draw.");
        _ = Cold(game, registry);
    }

    private static void ReachPlay(GameEngine game, bool selectGenerals = false)
    {
        for (var step = 0; step < 240; step++)
        {
            var prompt = P(game);
            if (prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }) return;
            if (selectGenerals && prompt is { Kind: DecisionKind.SelectGeneral })
            {
                var preferred = $"fixture:dfb-other-{prompt.PlayerSeat}";
                var general = prompt.ValidContentIds.Contains(preferred) ? preferred : prompt.ValidContentIds.First();
                Accept(game, new SelectGeneralCommand(prompt.PlayerSeat, general, game.Revision, prompt.PromptId));
            }
            else if (prompt is { Kind: DecisionKind.RescueDying })
                Answer(game, c => c.Cards.Count == 0 && !c.Parameters.ContainsKey("draw-funded-basic") &&
                    (c.Parameters.GetValueOrDefault("response") is "let-die" or "pass" || c.Parameters.GetValueOrDefault("action") == "pass"));
            else
                Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The real paid failure did not return to the original Play decision: " + JsonSerializer.Serialize(P(game)));
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:slash-request-failure", "1.0.0", "真实已付请求失败返回");

        public void Register(IContentRegistryBuilder builder)
        {
            var production = CharacterFixtureContent.EmbeddedProgramBundle("ordinary-wang-xiang");
            builder.AddSkill(new(Bingxin, "冰心", "真实摸牌支付能力") { Program = production.Programs[Bingxin] });
            var rules = JsonNode.Parse("""
                {"skills":[
                  {"id":"fixture:dfb-driver","revision":1,"activations":[{"id":"request","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[
                    {"op":"requestSlashAgainstChosenTarget","target":"selectedTarget","resultBind":"answer"},
                    {"op":"chooseOption","target":"owner","resultBind":"after-request","options":[{"id":"continue"}]}]}]},
                  {"id":"fixture:dfb-gain","revision":1,"triggers":[{"id":"gain","window":"cardsGained","subject":"owner","optional":false,"usageScope":"game","usageLimit":1,"destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["skill-program.draw-funded-distinct-basic.draw"],"effects":[
                    {"op":"chooseOption","target":"owner","resultBind":"seen","options":[{"id":"continue"}]},
                    {"op":"selectTarget","target":"owner","targetKind":"otherLiving"},
                    {"op":"loseHp","target":"selectedTarget","amount":4}]}]},
                  {"id":"fixture:dfb-quiet","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}
                ]}
                """)!;
            rules["schemaVersion"] = SkillProgramCatalog.RulesSchemaVersion;
            var labels = rules["skills"]!.AsArray().ToDictionary(n => n!["id"]!.GetValue<string>(), n =>
            {
                var label = new Dictionary<string, object> { ["name"] = "已付请求失败", ["description"] = "真实摸牌子结算" };
                if (n!["id"]!.GetValue<string>() is Driver or Gain)
                    label["optionLabels"] = new Dictionary<string, string> { ["continue"] = "继续" };
                return label;
            });
            var catalog = SkillProgramCatalog.Load(rules.ToJsonString(), JsonSerializer.Serialize(new { schemaVersion = 3, skills = labels }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, id, "原请求与真实支付子结算") { Program = program });
            builder.AddSkill(new("fixture:dfb-selection", "固定候选", "固定小夹具")
            {
                SelectionWeights = Enum.GetValues<Role>().ToDictionary(role => role, _ => 100d)
            });
            builder.AddGeneral(new("fixture:dfb-owner", "原请求拥有者", "supporter", "fixture:dfb-selection", "jin", 5, [Driver], GeneralGender.Male));
            for (var seat = 1; seat < 4; seat++)
                builder.AddGeneral(new($"fixture:dfb-other-{seat}", "实际付款角色", "supporter", "fixture:dfb-selection", "qun", 4,
                    [Bingxin, Gain, "fixture:dfb-quiet"], GeneralGender.Male));
            builder.AddDeck(new("fixture:dfb-deck", "固定真实实体杀", 4, 2, [])
            {
                PhysicalCards = Enumerable.Range(0, 80).Select(i => new ContentDeckPhysicalCard("standard:slash", Suit.Spade, i % 13 + 1)).ToArray()
            });
            builder.AddMode(new(Mode, "原请求已付失败", 4, 4,
                new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Renegade)] = 3 },
                "fixture:dfb-deck", GeneralCandidateCount: 4,
                GeneralPoolIds: ["fixture:dfb-owner", "fixture:dfb-other-1", "fixture:dfb-other-2", "fixture:dfb-other-3"]));
        }
    }
}
