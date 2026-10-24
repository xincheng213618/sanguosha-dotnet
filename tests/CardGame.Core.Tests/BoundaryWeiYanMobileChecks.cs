using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class BoundaryWeiYanMobileChecks
{
    private const string Hero = "boundary:wei-yan-mobile", Kuanggu = "boundary:kuanggu-mobile", Qimou = "boundary:qimou-mobile",
        Quiet = "fixture:mobile-weiyan-quiet", Driver = "fixture:mobile-weiyan-driver", Deck = "fixture:mobile-weiyan-deck";

    public static void NoDrawPaidLossSurvivesRescueColdAndKeepsActualXForPointBenefits()
    {
        var scene = Scene.Start(); var hand = scene.V.HandCount; var draws = scene.Draws; var turn = scene.G.State.TurnNumber;
        var peach = scene.V.Hand.Single(card => card.Kind == CardKind.Peach).Id;
        Need(scene.V.Hp == 4 && scene.G.GetCombatDistance(0, 2) == 2, "The real four-HP owner starts without a distance privilege and has a physical Peach available for its forthcoming dying response.");
        scene.Accept(new UseProgramSkillCommand(0, Qimou, "lose-hp-for-turn-benefits", [], [], scene.G.Revision, scene.P!.PromptId));
        scene.Reach(() => scene.P?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "hp-loss-quantity") == true);
        Need(scene.P!.Choices.Select(choice => choice.Parameters["quantity"]).SequenceEqual(["1", "2", "3", "4"]), "Any positive quantity up to current HP is offered by the original finite native quantity producer, including losing all four.");
        scene.Answer(choice => choice.Parameters.GetValueOrDefault("quantity") == "4"); scene.Reach(() => scene.P is { Kind: DecisionKind.RescueDying, PlayerSeat: 0 });
        var mother = scene.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Qimou);
        Need(scene.V.Hp == 0 && mother.PaidHpLossReceipt is { Requested: 4, HpBefore: 4, HpAfter: 0, ActualLost: 4, DrawIssued: false, DistanceGrantSequence: null, SlashGrantSequence: null } &&
            scene.Draws == draws && !scene.Grants.Any(), "The original limited activation has actually paid four HP before rescue and before either entitlement; mobile Qimou has issued no parallel Draw4.");
        scene.Cold(); scene.Answer(choice => choice.Parameters.GetValueOrDefault("response") == "peach" && choice.Cards.SequenceEqual([peach])); scene.Play();
        var grants = scene.Grants.ToArray();
        Need(scene.V.Hp == 1 && scene.V.HandCount == hand - 1 && scene.Draws == draws && grants.Length == 2 &&
            grants.All(grant => grant.ParentFrameId == mother.Id && grant.PaidHpLossOrigin is { Requested: 4, HpBefore: 4, HpAfter: 0, ActualLost: 4 }) &&
            grants.Single(grant => grant.Query == SkillRuleQuery.OutgoingDistance).Amount == -4 && grants.Single(grant => grant.Query == SkillRuleQuery.SlashLimit).Amount == 4 &&
            scene.G.GetCombatDistance(0, 2) == 1 && !scene.G.GetHumanLegalActions().Any(action => action.ProgramSkillId == Qimou) &&
            scene.G.CardMovements.Count(move => move.CardId == peach && move.From == CardLocation.Hand(0) && move.To == CardLocation.Processing) == 1,
            "Cold rescue returns to the same paid mother, grants actual X=4 despite the survivor now having one HP, draws no cards and consumes the limited use once.");

        scene.Use("two-damage", [2]); scene.Reach(() => scene.OptionalKuanggu);
        scene.Activate(); scene.Reach(() => scene.Benefit); scene.Answer(choice => choice.Parameters.GetValueOrDefault("option-id") == "recover");
        scene.Reach(() => scene.OptionalKuanggu); scene.Activate(); scene.Reach(() => scene.Benefit);
        scene.Answer(choice => choice.Parameters.GetValueOrDefault("option-id") == "draw"); scene.Play();
        Need(scene.V.Hp == 2 && scene.V.HandCount == hand && scene.Draws == draws + 1 &&
            scene.G.CardMovements.Count(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0) && move.Reason.Value == $"skill-program.{Kuanggu}.{SkillProgramEffectOp.Draw}") == 1 &&
            scene.Grants.Select(grant => grant.Amount).Order().SequenceEqual([-4, 4]),
            "One true two-point nearby damage permits two separate Kuanggu choices: one recovery and one native Draw1. Later HP recovery cannot rewrite the already-paid Qimou amount or manufacture its omitted draw.");
        for (var index = 0; index < 5; index++) { scene.Slash(2); scene.Play(); }
        Need(scene.V.Hand.Any(card => card.Kind == CardKind.Slash) && !scene.G.GetHumanLegalActions().Any(action => action.Kind == LegalActionKind.Slash) &&
            scene.G.Events.Select(entry => entry.Payload).OfType<DamageAppliedEvent>().Count(damage => damage.SourceSeat == 0 && damage.TargetSeat == 2 && damage.Amount == 1) == 5,
            "Exactly the native base one plus the four actually paid extra Slashes resolve; an unused real Slash remains when the genuine use quota is exhausted.");
        scene.Accept(new EndPlayPhaseCommand(0, scene.G.Revision, scene.P!.PromptId)); scene.Reach(() => scene.G.State.TurnNumber > turn);
        Need(scene.G.GetCombatDistance(0, 2) == 2 && scene.Grants.Count() == 2, "The once-issued paid rights expire at the actual turn boundary, restoring the original distance without duplicating a grant.");
    }

    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        internal GameEngine G = game;
        internal PendingDecision? P => G.PendingDecision ?? G.CreateSnapshot(0).PendingDecision;
        internal PlayerSnapshot V => G.CreateSnapshot(0).Players[0];
        internal int Draws => G.CardMovements.Count(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0));
        internal IEnumerable<TurnRuleModifier> Grants => G.Events.Select(entry => entry.Payload).OfType<TurnRuleModifierGrantedEvent>().Where(entry => entry.Modifier.Source.SkillId == Qimou).Select(entry => entry.Modifier);
        internal bool OptionalKuanggu => P?.SkillPrompt?.SkillId == Kuanggu && P.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        internal bool Benefit => P?.SkillPrompt?.SkillId == Kuanggu && P.Choices.Any(choice => choice.Parameters.GetValueOrDefault("option-id") == "draw");
        internal void Accept(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected mobile Wei Yan scene command."); }
        internal void Answer(Func<PromptChoice, bool> predicate) { var prompt = P!; Accept(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.First(predicate).Id, G.Revision)); }
        internal void Activate() => Answer(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        internal void Use(string activation, int[] targets) => Accept(new UseProgramSkillCommand(0, Driver, activation, [], targets, G.Revision, P!.PromptId));
        internal void Slash(int target)
        {
            var action = G.GetHumanLegalActions().First(action => action.Kind == LegalActionKind.Slash && action.TargetSeats.SequenceEqual([target]));
            Accept(new PlayCardCommand(0, action.CardId!.Value, action.TargetSeats, G.Revision, P!.PromptId, action.PlayedCardKind) { ConversionSource = action.ConversionSource, AdditionalConversionSources = action.AdditionalConversionSources });
        }
        internal void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        internal void Reach(Func<bool> done)
        {
            for (var step = 0; step < 140; step++)
            {
                if (done()) return;
                if (OptionalKuanggu) Answer(choice => choice.Parameters.GetValueOrDefault("program-action") == "skip");
                else if (P is { Kind: DecisionKind.RespondDodge }) Answer(choice => choice.Parameters.GetValueOrDefault("response") == "take-damage");
                else if (P is not null) throw new InvalidOperationException("Unexpected mobile Wei Yan decision " + P.Kind + " seat " + P.PlayerSeat + " skill " + P.SkillPrompt?.SkillId);
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("The bounded mobile Wei Yan scene did not reach its native boundary.");
        }
        internal void Cold()
        {
            var original = Enumerable.Range(0, 4).Select(viewer => SnapshotJson.Serialize(G.CreateSnapshot(viewer))).ToArray();
            var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(original.SequenceEqual(Enumerable.Range(0, 4).Select(viewer => SnapshotJson.Serialize(restored.CreateSnapshot(viewer)))), "The original paid HP invoice and dying response remain identical through JSON cold restore."); G = restored;
        }
        internal static Scene Start()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var cards = Enumerable.Range(1, 6).Select(rank => new ShanheStartingCard(CardKind.Slash, Suit.Spade, rank)).Append(new(CardKind.Peach, Suit.Heart, 3)).ToArray();
            var setup = new ShanheBattleSetup(60, 1, "mobile-weiyan", Hero, 4, 0, 0, 0, 0, [Quiet, Driver], cards, [],
                [new("fixture:mobile-weiyan-peer", Hp: 20) { StartingHand = [] }, new("fixture:mobile-weiyan-peer", Hp: 20) { StartingHand = [] }],
                [new("fixture:mobile-weiyan-peer", Hp: 20) { StartingHand = [] }], new string('e', 64));
            var game = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 6 }, registry);
            var scene = new Scene(game, registry); scene.Accept(new StartGameCommand()); scene.Play(); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-weiyan", new(1, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var rules = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = new object[] {
                new { id = Quiet, revision = 1, modifiers = new[] { new { id = "keep-original-hand", query = "handLimit", operation = "add", value = 20, priority = 0 } },
                    triggers = new[] { new { id = "no-normal-draw", isSkillInvocation = false, window = "drawPhaseStarting", subject = "owner", optional = false, effects = new[] { new { op = "adjustNormalDraw", target = "owner", amount = -2 } } } } },
                new { id = Driver, revision = 1, activations = new[] { new { id = "two-damage", minCards = 0, maxCards = 0, minTargets = 1, maxTargets = 1, targetKind = "otherLiving", usesPerTurn = (int?)null,
                    effects = new[] { new { op = "damage", target = "selectedTarget", amount = 2 } } } } }
            } });
            var presentations = new Dictionary<string, object> { [Quiet] = new { name = "保留真实手牌", description = "原生奇谋场景" }, [Driver] = new { name = "真实两点伤害", description = "逐伤害点狂骨场景" } };
            var programs = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = presentations }));
            foreach (var (id, program) in programs.Programs) builder.AddSkill(new(id, programs.Presentations[id].Name, programs.Presentations[id].Description) { Program = program, ProgramPresentation = programs.Presentations[id] });
            builder.AddGeneral(new("fixture:mobile-weiyan-peer", "真实远位角色", "supporter", Quiet, "wei", 20));
            builder.AddDeck(new(Deck, "原生真实摸牌", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 48).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 2)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "移动界魏延原生场景", 4, 4, new Dictionary<string, int>(), Deck, GeneralPoolIds: [Hero, "fixture:mobile-weiyan-peer"], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
