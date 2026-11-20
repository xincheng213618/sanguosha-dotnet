using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class BoundaryMobileQiaomengDiscardEntryChecks
{
    private const string Hero = "boundary:gongsun-zan-mobile", Qiaomeng = "boundary:qiaomeng-mobile";
    private const string Watch = "fixture:qiaomeng-discard-reentry", Enemy = "fixture:qiaomeng-enemy", Deck = "fixture:qiaomeng-deck";
    private const string Payment = "skill-program.damage-target-mount.discard", Claim = "skill-program.damage-target-mount.claim";
    private const string Take = "skill-program." + Watch + ".ClaimMovedCards";
    private const string Rediscard = "skill-program." + Watch + ".SelectAndMoveOwnedCard";

    internal static void QiaomengClaimsOnlyTheOriginalDiscardOfTheDamagedTargetsMountCold()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var target = new ShanheCombatantDefinition(Enemy, Hp: 4)
        {
            StartingHand = [], StartingEquipment = [new(CardKind.OffensiveHorse, Suit.Club, 4)]
        };
        var setup = new ShanheBattleSetup(83, 1, "fixture:qiaomeng-original-entry", Hero, 4, 0, 0, 0, 0, [Watch],
            [new(CardKind.Slash, Suit.Spade, 7)], [], [target], [], new string('a', 64)) { ExtraSkillSlots = 1 };
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, ConfirmOpeningHand = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false
        }, content);
        Accept(game, new StartGameCommand());
        Reach(game, () => IsPlay(game));
        var mount = game.CreateSnapshot(0).Players[1].Equipment.Single(card => card.Kind == CardKind.OffensiveHorse).Id;
        var slash = game.GetHumanLegalActions().Single(action => action.Kind == LegalActionKind.Slash && action.TargetSeats.SequenceEqual([1]));
        Accept(game, new PlayCardCommand(0, slash.CardId!.Value, slash.TargetSeats, game.Revision, P(game)!.PromptId, slash.PlayedCardKind));
        Reach(game, () => CanInvoke(game, Qiaomeng));
        var use = game.ResolutionStack.OfType<CardUseFrame>().Single();
        Need(use.Action is { Type: CardActionType.Use, ActorSeat: 0, EffectiveKind: CardKind.Slash, EffectiveIsRed: false } &&
            Facts<DamageAppliedEvent>(game).Single() is { SourceSeat: 0, TargetSeat: 1, Amount: 1 },
            "The real mobile Qiaomeng invocation follows the actual damage dealt by the original black Slash.");
        AnswerAction(game, "activate");
        Reach(game, () => P(game) is { SkillPrompt.SkillId: Qiaomeng } prompt && prompt.Choices.Any(IsPayment));
        var choice = P(game)!.Choices.Single(IsPayment);
        Need(choice.Cards.SequenceEqual([mount]) && choice.Parameters.GetValueOrDefault("source-zone") == "Equipment",
            "Only the damaged seat's real public equipment is eligible for this edition's payment.");
        Answer(game, choice);

        Reach(game, () => CanInvoke(game, Watch));
        var paid = game.CardMovements.Single(move => move.CardId == mount && move.Reason.Value == Payment);
        var frame = QiaomengFrame(game);
        Need(frame.DamageTargetMount is { Receipt: { IsMount: true } receipt, ClaimIssued: false } && receipt.CardId == mount &&
            receipt.MovementSequence == paid.Sequence && paid.From == CardLocation.Equipment(1) && paid.To == CardLocation.DiscardPile,
            "The actual mount discard is frozen on the parent before the original discard observer can take that entry.");

        var control = Cold(game, content);
        AnswerAction(control, "skip");
        Reach(control, () => IsPlay(control));
        Finished(control, use.Id);
        Need(control.CardMovements.Single(move => move.Reason.Value == Claim) is { CardId: var claimed } gain &&
            claimed == mount && gain.Sequence > paid.Sequence && gain.From == paid.To && gain.To == CardLocation.Hand(0),
            "The original paid mount stays eligible and enters Gongsun Zan's hand exactly once when the earlier claim is declined.");

        AnswerAction(game, "activate");
        Reach(game, () => P(game) is { SkillPrompt.SkillId: Watch } prompt && prompt.Choices.Any(item => item.Cards.SequenceEqual([mount])));
        var taken = game.CardMovements.Single(move => move.CardId == mount && move.Reason.Value == Take);
        Need(taken.Sequence > paid.Sequence && taken.From == paid.To && taken.To == CardLocation.Hand(0) &&
            QiaomengFrame(game).Id == frame.Id && QiaomengFrame(game).DamageTargetMount?.Receipt == frame.DamageTargetMount?.Receipt,
            "The earlier observer takes that very same entity without changing the original paid Qiaomeng receipt.");
        game = Cold(game, content);
        Answer(game, P(game)!.Choices.Single(item => item.Cards.SequenceEqual([mount])));
        Reach(game, () => IsPlay(game));
        Finished(game, use.Id);
        var later = game.CardMovements.Single(move => move.CardId == mount && move.Reason.Value == Rediscard);
        Need(later.Sequence > taken.Sequence && later.BatchId != paid.BatchId && later.From == CardLocation.Hand(0) && later.To == CardLocation.DiscardPile,
            "The intervening public command really discards the same mount into a later physical batch.");
        Need(game.CardMovements.All(move => move.Reason.Value != Claim) && game.CardMovements.Last(move => move.CardId == mount) == later,
            $"Qiaomeng must leave the later mount entry {later.Sequence} alone after losing its original payment entry {paid.Sequence}.");
        _ = Cold(game, content);
    }

    private static ProgramSkillFrame QiaomengFrame(GameEngine game) =>
        game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Qiaomeng);

    private static void Finished(GameEngine game, long useId) => Need(
        Facts<DamageAppliedEvent>(game).Count(fact => fact.SourceCardUseResolutionId == useId && fact is { SourceSeat: 0, TargetSeat: 1, Amount: 1 }) == 1 &&
        Facts<CardUseFinishedEvent>(game).Count(fact => fact.ResolutionId == useId) == 1 &&
        game.CardMovements.Count(move => move.Reason.Value == Payment) == 1 && game.CreateSnapshot(0).Players[1].Hp == 3,
        "The original black Slash damage, payment and completion all occur once in both branches.");

    private static bool IsPayment(PromptChoice choice) => choice.Parameters.GetValueOrDefault("program-action") == "damage-target-mount";
    private static bool IsPlay(GameEngine game) => P(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard };
    private static bool CanInvoke(GameEngine game, string skill) => P(game) is { } prompt && prompt.SkillPrompt?.SkillId == skill &&
        prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
    private static PendingDecision? P(GameEngine game) => game.CreateSnapshot(0).PendingDecision;
    private static T[] Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();

    private static void AnswerAction(GameEngine game, string action) =>
        Answer(game, P(game)!.Choices.Single(choice => choice.Parameters.GetValueOrDefault("program-action") == action));

    private static void Answer(GameEngine game, PromptChoice choice) =>
        Accept(game, new AnswerPromptCommand(0, P(game)!.PromptId, choice.Id, game.Revision));

    private static void Reach(GameEngine game, Func<bool> reached)
    {
        for (var step = 0; step < 80; step++)
        {
            if (reached())
                return;
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("Qiaomeng did not reach the expected decision or return: " + P(game)?.Prompt);
    }

    private static void Accept(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        Need(result.Accepted, result.Error?.Message ?? "The real Qiaomeng command was rejected.");
    }

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Need(restored.CardMovements.SequenceEqual(game.CardMovements) &&
            JsonSerializer.Serialize(restored.ResolutionStack) == JsonSerializer.Serialize(game.ResolutionStack) &&
            game.Events.Select(entry => JsonSerializer.Serialize(entry.Payload, entry.Payload.GetType())).SequenceEqual(
                restored.Events.Select(entry => JsonSerializer.Serialize(entry.Payload, entry.Payload.GetType()))) &&
            Enumerable.Range(0, game.PlayerCount).All(seat => SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold commands preserve the original Slash and mount receipt, as well as the actual private re-discard choice.");
        return restored;
    }

    private static void Need(bool condition, string message)
    {
        if (!condition)
            throw new InvalidOperationException(message);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-mobile-qiaomeng-entry", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var rules = $$$"""
                {"skills":[{"id":"{{{Watch}}}","revision":1,"triggers":[{"id":"take-and-discard","window":"discardPileReceived",
                    "subject":"owner","movementOccurrence":"perCard","movementReasons":["{{{Payment}}}"],"optional":true,
                    "effects":[{"op":"claimMovedCards","target":"owner"},{"op":"awaitBoundCardMovements","target":"owner"},
                      {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},
                       "zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]}]}]}
                """;
            var catalog = SkillProgramCatalog.LoadCurrent(rules, JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object>
                {
                    [Watch] = new { name = "领取后再弃置", description = "获得原弃置的坐骑，再弃置自己的一张真实手牌。" }
                }
            }));
            builder.AddSkill(new(Watch, catalog.Presentations[Watch].Name, catalog.Presentations[Watch].Description)
                { Program = catalog.Programs[Watch], ProgramPresentation = catalog.Presentations[Watch] });
            builder.AddGeneral(new(Enemy, "固定坐骑的对手", "supporter", "standard:none", "wei", 4));
            builder.AddDeck(new(Deck, "固定闪", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 24)
                .Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, 2)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "趫猛原坐骑弃置", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
