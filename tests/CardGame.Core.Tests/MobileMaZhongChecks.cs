using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileMaZhongChecks
{
    private const string Skill = "mobile:fuman", Driver = "fixture:mobile-ma-zhong-driver", Watch = "fixture:mobile-ma-zhong-watch", Clock = "fixture:mobile-ma-zhong-clock";
    private const string GiftReason = "skill-program.mobile:fuman.MoveBoundCards", DrawReason = "skill-program.mobile:fuman.Draw";

    public static void PhysicalSlashGiftPaidAndCommittedOneDrawSurvivesSkillLossCold()
    {
        var (game, registry) = Create();
        EmptyRecipient(game);
        var gift = game.CreateSnapshot(0, true).Players[0].Hand.First(card => card.Kind == CardKind.Slash).Id;
        Give(game, gift);
        Reach(game, _ => ActiveWatch(game) == "gift-paid");
        Require(game.ResolutionStack.OfType<ProgramSkillFrame>().Any(frame => frame.SkillId == Skill && frame.InstructionIndex == 4 &&
            frame.CardSetBindings.Any(binding => binding.Name == "fuman-gift" && binding.CardIds.SequenceEqual([gift]))) &&
            game.CardMovements.Any(move => move.CardId == gift && move.From == CardLocation.Hand(0) && move.To == CardLocation.Hand(1) && move.Reason.Value == GiftReason),
            "The public gift observer must suspend above its exact paid original program frame.");
        var restored = Restore(game, registry);
        foreach (var branch in new[] { game, restored })
        {
            Continue(branch); Settle(branch);
            Accept(branch, new UseProgramSkillCommand(0, Driver, "lose-fuman", [], [], branch.Revision, Prompt(branch)!.PromptId));
            Settle(branch);
            Require(branch.CardMovements.Count(move => move.CardId == gift && move.Reason.Value == GiftReason &&
                move.From == CardLocation.Hand(0) && move.To == CardLocation.Hand(1)) == 1 &&
                branch.GetHumanLegalActions().All(action => action.ProgramSkillId != Skill),
                "A paid gift must finish once after cold recovery, and a real terminal activation must remove Fuman before the recipient uses its original Slash.");
            Request(branch);
            Reach(branch, _ => ActiveWatch(branch) == "reward-paid");
            Require(branch.ResolutionStack.OfType<CardUseFrame>().Any(frame => frame.SourceSeat == 1 &&
                frame.Action?.PhysicalCards.SingleOrDefault()?.CardId == gift) &&
                branch.Events.Select(item => item.Payload).OfType<DamageAppliedEvent>().All(damage => damage.SourceSeat != 1),
                "Mobile Fuman draws at actual Slash use commitment, before the gifted Slash deals damage.");
            Require(branch.CardMovements.Count(move => move.To == CardLocation.Hand(0) && move.Reason.Value == DrawReason) == 1,
                "A real gifted Slash use grants exactly one immediate draw even when its skill instance has already been lost.");
        }
        Equal(game, restored);
        var rewardCold = Restore(game, registry);
        foreach (var branch in new[] { game, restored, rewardCold })
        {
            Continue(branch); Settle(branch);
            Require(branch.CardMovements.Count(move => move.Reason.Value == DrawReason && move.To == CardLocation.Hand(0)) == 1 &&
                branch.CardMovements.Count(move => move.CardId == gift && move.From == CardLocation.Hand(1) && move.To == CardLocation.Processing) == 1,
                "The paid draw child returns to the original use without redrawing, regifting or paying the original Slash twice.");
        }
        Equal(game, restored); Equal(game, rewardCold);
    }

    public static void GiftRejectsNonSlashPlayedResponseAndExpiresAfterRecipientNextTurn()
    {
        var (invalid, _) = Create(onlyDodge: true);
        var before = GameCheckpointJson.Serialize(invalid.CreateCheckpoint());
        Require(invalid.GetHumanLegalActions().All(action => action.ProgramSkillId != Skill), "A hand containing only physical Dodge must offer no mobile Fuman activation.");
        var reject = invalid.Submit(new UseProgramSkillCommand(0, Skill, "give-physical-slash", [], [1], invalid.Revision, Prompt(invalid)!.PromptId));
        Require(!reject.Accepted && before == GameCheckpointJson.Serialize(invalid.CreateCheckpoint()),
            "Mobile Fuman cannot give arbitrary hand cards as Slash, and a rejected non-Slash pays no target quota.");

        var (played, _) = Create(humanRecipient: true); EmptyRecipient(played);
        var playedGift = played.CreateSnapshot(0, true).Players[0].Hand.First().Id;
        Give(played, playedGift); Reach(played, _ => ActiveWatch(played) == "gift-paid"); Continue(played); Settle(played);
        var hand = played.CreateSnapshot(0, true).Players[0].Hand.Select(card => card.Id).ToArray();
        Accept(played, new UseProgramSkillCommand(0, Driver, "duel", hand, [], played.Revision, Prompt(played)!.PromptId));
        Reach(played, prompt => prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("card-kind") == nameof(CardKind.Duel)));
        Answer(played, choice => choice.Targets.SequenceEqual(new[] { 1 }));
        Reach(played, prompt => prompt is { PlayerSeat: 1, Kind: DecisionKind.RespondSlash, IncomingCard: CardKind.Duel });
        Require(Prompt(played)!.Choices.Any(choice => choice.Cards.SequenceEqual(new[] { playedGift })),
            "The real recipient's Duel defense must offer the exact physical gifted Slash. " + DuelDiagnostic(played, playedGift));
        Answer(played, choice => choice.Cards.SequenceEqual(new[] { playedGift })); Settle(played);
        Require(played.CardMovements.Any(move => move.CardId == playedGift && move.From == CardLocation.Hand(1) && move.Reason == CardMoveReasons.Respond) &&
            played.Events.Select(item => item.Payload).OfType<DuelResponseEvent>().Any(response => response.ResponderSeat == 1 && response.UsedSlash && response.SlashCardId == playedGift),
            "The recipient must really play the gifted Slash from its own hand in the native Duel response. " + DuelDiagnostic(played, playedGift));
        Require(played.CardMovements.All(move => move.Reason.Value != DrawReason),
            "Merely playing the gifted Slash in a real Duel response must not count as using it for mobile Fuman. " + DuelDiagnostic(played, playedGift));

        var (expired, registry) = Create(); EmptyRecipient(expired);
        var expiredGift = expired.CreateSnapshot(0, true).Players[0].Hand.First().Id;
        Give(expired, expiredGift); Reach(expired, _ => ActiveWatch(expired) == "gift-paid"); Continue(expired); Settle(expired);
        var giftTurn = expired.CreateSnapshot(0, true).TurnNumber;
        Accept(expired, new EndPlayPhaseCommand(0, expired.Revision, Prompt(expired)!.PromptId));
        Reach(expired, prompt => prompt.Kind == DecisionKind.PlayCard && prompt.PlayerSeat == 0 && expired.CreateSnapshot(0, true).TurnNumber > giftTurn);
        Require(expired.Events.Select(item => item.Payload).OfType<TurnEndedEvent>().Any(item => item.ActorSeat == 1 && item.TurnNumber > giftTurn) &&
            expired.CreateSnapshot(0, true).Players[1].Hand.Any(card => card.Id == expiredGift),
            "The expiry fixture must retain the original gifted entity through the recipient's actual next turn end.");
        var cold = Restore(expired, registry);
        foreach (var branch in new[] { expired, cold })
        {
            Request(branch); Settle(branch);
            Require(branch.CardMovements.Any(move => move.CardId == expiredGift && move.From == CardLocation.Hand(1) && move.To == CardLocation.Processing && move.Reason == CardMoveReasons.Use) &&
                branch.CardMovements.All(move => move.Reason.Value != DrawReason),
                "Using the retained original Slash after the recipient's next actual turn ends must no longer reward its giver.");
        }
        Equal(expired, cold);
    }

    private static void EmptyRecipient(GameEngine game)
    {
        Require(game.CreateSnapshot(0, true).Players[1].Hand.Count == 1, "The recipient starts with one deterministic physical card.");
        Use(game, "discard-peer", 1);
        Reach(game, prompt => prompt.PlayerSeat == 0 && prompt.SkillPrompt?.SkillId == Driver && prompt.Choices.Count > 0);
        Answer(game, _ => true); Settle(game);
        Require(game.CreateSnapshot(0, true).Players[1].Hand.Count == 0, "The foreign discard must really empty the recipient's hand.");
    }
    private static void Give(GameEngine game, int card)
    {
        Accept(game, new UseProgramSkillCommand(0, Skill, "give-physical-slash", [], [1], game.Revision, Prompt(game)!.PromptId));
        Reach(game, prompt => prompt.PlayerSeat == 0 && prompt.SkillPrompt?.SkillId == Skill && prompt.Choices.Any(choice => choice.Cards.SequenceEqual(new[] { card })));
        Answer(game, choice => choice.Cards.SequenceEqual(new[] { card }));
    }
    private static void Use(GameEngine game, string activation, int target) => Accept(game,
        new UseProgramSkillCommand(0, Driver, activation, [], [target], game.Revision, Prompt(game)!.PromptId));
    private static void Request(GameEngine game)
    {
        Use(game, "request", 1);
        Reach(game, prompt => prompt.PlayerSeat == 1 && prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "request-slash"));
        Accept(game, new AdvanceOneStepCommand(game.Revision));
    }
    private static PendingDecision? Prompt(GameEngine game) => Enumerable.Range(0, 4).Select(seat => game.CreateSnapshot(seat, true).PendingDecision).FirstOrDefault(prompt => prompt is not null);
    private static string? ActiveWatch(GameEngine game) => game.ResolutionStack.OfType<ProgramSkillFrame>().LastOrDefault() is { SkillId: Watch } frame ? frame.TriggerId : null;
    private static void Continue(GameEngine game) => Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
    private static void Reach(GameEngine game, Func<PendingDecision, bool> predicate)
    {
        for (var step = 0; step < 220; step++)
        {
            if (Prompt(game) is { } prompt && predicate(prompt)) return;
            Step(game);
        }
        throw new InvalidOperationException($"Mobile Ma Zhong did not reach its required native boundary: {Prompt(game)?.Kind}/{Prompt(game)?.SkillPrompt?.SkillId}/{ActiveWatch(game)}.");
    }
    private static void Step(GameEngine game)
    {
        if (Prompt(game) is { PlayerSeat: 0, Kind: DecisionKind.RespondDodge or DecisionKind.RespondSlash }) Answer(game, choice => choice.Cards.Count == 0);
        else if (Prompt(game) is { PlayerSeat: 0, Kind: DecisionKind.Nullification }) Answer(game, choice => choice.Parameters.GetValueOrDefault("response") == "pass");
        else if (ActiveWatch(game) is not null) Continue(game);
        else Accept(game, new AdvanceOneStepCommand(game.Revision));
    }
    private static void Settle(GameEngine game)
    {
        for (var step = 0; step < 220 && game.ResolutionStack.Count > 0; step++) Step(game);
        Require(game.ResolutionStack.Count == 0, "The original gift/use program must finish with no orphaned child.");
        if (Prompt(game) is null) Reach(game, prompt => prompt.Kind == DecisionKind.PlayCard && prompt.PlayerSeat == 0);
    }
    private static void Answer(GameEngine game, Func<PromptChoice, bool> predicate)
    { var prompt = Prompt(game)!; Accept(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.First(predicate).Id, game.Revision)); }
    private static void Accept(GameEngine game, GameCommand command)
    { var result = game.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Require(result.Accepted, result.Error?.Message ?? "Mobile Ma Zhong command rejected."); }
    private static GameEngine Restore(GameEngine game, ContentRegistry registry) => GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
    private static void Equal(GameEngine a, GameEngine b) => Require(SnapshotJson.Serialize(a.CreateSnapshot(0, true)) == SnapshotJson.Serialize(b.CreateSnapshot(0, true)) &&
        a.CardMovements.SequenceEqual(b.CardMovements) && a.Events.Select(item => JsonSerializer.Serialize(item.Payload, item.Payload.GetType())).SequenceEqual(b.Events.Select(item => JsonSerializer.Serialize(item.Payload, item.Payload.GetType()))),
        "Cold restoration must preserve the gifted entity, original source instance, expiry, use commitment, movement and exactly-once draw.");
    private static void Require(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
    private static string DuelDiagnostic(GameEngine game, int gift) => JsonSerializer.Serialize(new
    {
        Gift = gift, Pending = Prompt(game),
        Movements = game.CardMovements.Where(move => move.CardId == gift || move.Reason.Value == DrawReason),
        Responses = game.Events.Select(item => item.Payload).OfType<DuelResponseEvent>(),
        AcceptedActions = game.Events.Select(item => item.Payload).OfType<CardActionAcceptedEvent>().Select(item => item.Action),
        Hand = game.CreateSnapshot(0, true).Players[1].Hand
    });

    private static (GameEngine Game, ContentRegistry Registry) Create(bool onlyDodge = false, bool humanRecipient = false)
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardActiveSkillExpansionPackage(true), new StandardRescueSkillExpansionPackage(), new StandardClassicGeneralPackage(), new Fixture(onlyDodge));
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 31, HumanSeat = 0, HumanSeats = humanRecipient ? [0, 1] : null, HumanRole = Role.Lord, PlayerCount = 4,
            ModeId = "identity:classic-mobile-ma-zhong", UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 20 }, registry);
        Accept(game, new StartGameCommand());
        for (var step = 0; step < 160; step++)
        {
            if (Prompt(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }) return (game, registry);
            if (Prompt(game) is { Kind: DecisionKind.SelectGeneral } prompt && (prompt.PlayerSeat == 0 || humanRecipient && prompt.PlayerSeat == 1))
                Accept(game, new SelectGeneralCommand(prompt.PlayerSeat, prompt.PlayerSeat == 0 ? "fixture:mobile-ma-zhong-owner" : "fixture:mobile-ma-zhong-1", game.Revision, prompt.PromptId));
            else Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The fixed mobile Ma Zhong setup did not reach its owner's Play phase.");
    }
    private sealed class Fixture(bool onlyDodge) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-ma-zhong", new Version(1, 0, 0), []);
        public void Register(IContentRegistryBuilder builder)
        {
            var rules = $$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
             {"id":"{{Driver}}","revision":1,"activations":[
              {"id":"lose-fuman","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"loseOwnerSkillsAndGrant","target":"owner","skillIds":["mobile:fuman"],"sourceBind":"standard:none"}]},
              {"id":"discard-peer","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLivingWithHand","usesPerTurn":null,"effects":[{"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"selectedTarget"},"zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]},
              {"id":"request","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"requestSlashByTarget","target":"selectedTarget","resultBind":"asked"}]},
              {"id":"duel","minCards":1,"maxCards":64,"sourceZones":["hand"],"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"useAllHandCardsAsOrdinaryTrick","target":"owner","viewAsId":"fixture-duel","outputKind":"duel"}]}]},
             {"id":"{{Watch}}","revision":1,"triggers":[
              {"id":"gift-paid","window":"cardsMoved","subject":"owner","sourceZones":["hand"],"movementReasons":["{{GiftReason}}"],"movementOccurrence":"perOwnerBatch","optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"paid-gift","options":[{"id":"continue"}]}]},
              {"id":"reward-paid","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementReasons":["{{DrawReason}}"],"movementOccurrence":"perBatch","optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"paid-reward","options":[{"id":"continue"}]}]}]},
             {"id":"{{Clock}}","revision":1,"triggers":[{"id":"retain-gift-through-next-turn","window":"turnStartBeforeNormalFlow","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}
            ]}
            """;
            var presentation = new Dictionary<string, object>
            {
                [Driver] = new { name = "实体规则驱动", description = "原生弃置、请求与决斗" },
                [Watch] = new { name = "付款后子窗口", description = "实际赠牌与摸牌后暂停", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } },
                [Clock] = new { name = "实际下回合", description = "保留实体牌通过下一实际回合" }
            };
            var catalog = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = 3, skills = presentation }));
            foreach (var id in new[] { Driver, Watch, Clock }) builder.AddSkill(new(id, id, id) { Program = catalog.Programs[id], ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new("fixture:mobile-ma-zhong-owner", "移动马忠", "supporter", Skill, "shu", BaseHp: 20, AdditionalSkillIds: [Driver, Watch]));
            foreach (var seat in Enumerable.Range(1, 3)) builder.AddGeneral(new($"fixture:mobile-ma-zhong-{seat}", "真实受赠者", "supporter", Clock, "shu", BaseHp: 20));
            builder.AddDeck(new("fixture:mobile-ma-zhong-deck", "固定同质实体牌", 1, 2, [])
            { PhysicalCards = Enumerable.Range(0, 160).Select(index => new ContentDeckPhysicalCard(onlyDodge ? "standard:dodge" : "standard:slash", Suit.Spade, 7)).ToArray() });
            builder.AddMode(new("identity:classic-mobile-ma-zhong", "移动马忠实际赠牌", 4, 4,
                new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, "fixture:mobile-ma-zhong-deck",
                GeneralCandidateCount: 4, GeneralPoolIds: ["fixture:mobile-ma-zhong-owner", "fixture:mobile-ma-zhong-1", "fixture:mobile-ma-zhong-2", "fixture:mobile-ma-zhong-3"]));
        }
    }
}
