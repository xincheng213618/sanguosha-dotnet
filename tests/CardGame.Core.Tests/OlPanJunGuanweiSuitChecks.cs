using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class OlPanJunGuanweiSuitChecks
{
    private const string Hero = "ol:pan-jun", Guanwei = "ol:guanwei", Wusheng = "classic:wusheng";
    private const string Deck = "fixture:guanwei-accepted-suit-deck";

    internal static void GuanweiCountsTheEffectiveSuitOfAnEquipmentConversionCold()
    {
        foreach (var (fromEquipment, suit) in new[] { (false, Suit.Heart), (true, Suit.Diamond), (true, Suit.Heart) })
        {
            var (game, content) = Start(fromEquipment, suit);
            var play = Reach(game, IsPlay)!;
            var player = game.CreateSnapshot(0).Players[0];
            var draw = player.Hand.Single(card => card.Kind == CardKind.DrawTwo).Id;
            var material = fromEquipment
                ? player.Equipment.Single(card => card.Kind == CardKind.QilinBow).Id
                : player.Hand.Single(card => card.Kind == CardKind.Dodge && card.Suit == Suit.Heart).Id;
            var first = game.GetHumanLegalActions().Single(action => action.CardId == draw && action.ConversionSource is null);
            Play(game, play, first);
            play = Reach(game, IsPlay)!;
            var converted = game.GetHumanLegalActions().Single(action => action.CardId == material && action.Kind == LegalActionKind.Slash &&
                action.ConversionSource?.SkillId == Wusheng && action.TargetSeats.SequenceEqual([1]));
            Play(game, play, converted);
            _ = Reach(game, IsPlay);

            var origin = fromEquipment ? CardLocation.Equipment(0) : CardLocation.Hand(0);
            var accepted = Facts<CardActionAcceptedEvent>(game).Select(fact => fact.Action).Where(action =>
                action.ActorSeat == 0 && action.Type == CardActionType.Use && action.ConversionChain.Any(source => source.SkillId == Wusheng)).ToArray();
            var firstUse = Facts<CardUseDeclaredEvent>(game).Single(fact => fact.SourceSeat == 0 && fact.CardId == draw);
            Need(accepted is [var slash] && slash.EffectiveKind == CardKind.Slash && slash.EffectiveSuit == suit &&
                 slash.PhysicalCards is [{ CardId: var materialId, From: var source }] && materialId == material && source == origin &&
                 slash.TargetSeats.SequenceEqual([1]) && slash.ConversionChain.Single().SkillId == Wusheng &&
                 player.Hand.Single(card => card.Id == draw).Suit == Suit.Heart && firstUse.CardKind == CardKind.DrawTwo &&
                 Facts<CurrentTurnCardUseKindsRecordedEvent>(game).Single(fact => fact.OriginFrameId == firstUse.ResolutionId).HandSuitMask != 0 &&
                 game.CardMovements.Count(move => move.CardId == material && move.From == origin && move.To == CardLocation.Processing && move.Reason == CardMoveReasons.Use) == 1 &&
                 Facts<CardUseFinishedEvent>(game).Count(fact => fact.CardId == draw && fact.CardKind == CardKind.DrawTwo) == 1 &&
                 Facts<CardUseFinishedEvent>(game).Count(fact => fact.CardId == material && fact.CardKind == CardKind.Slash) == 1,
                "Two actual first-turn uses finish once each, including the paid one-entity Wusheng Slash with the accepted material suit.");

            var turn = game.State.TurnNumber;
            game = Cold(game, content);
            Submit(game, new EndPlayPhaseCommand(0, game.Revision, game.CreateSnapshot(0).PendingDecision!.PromptId));
            var prompt = Reach(game, IsGuanwei, () => Facts<TurnEndedEvent>(game).Any(fact => fact.TurnNumber == turn && fact.ActorSeat == 0));
            if (suit != Suit.Heart)
            {
                Need(prompt is null && !Facts<ProgramBindingStartedEvent>(game).Any(fact => fact.SkillId == Guanwei),
                    "A genuine diamond equipment conversion following a heart use fails the same-suit condition.");
                continue;
            }

            Need(IsGuanwei(prompt), $"Guanwei must consider both accepted heart uses when the Wusheng payment came from {origin.Zone}.");
            game = Cold(game, content);
            Answer(game, game.CreateSnapshot(0).PendingDecision!, "activate");
            var payment = Reach(game, next => Choice(next, "select-target") is not null || Choice(next, "select-and-move-owned-card") is not null)!;
            if (Choice(payment, "select-target") is not null)
            {
                Answer(game, payment, "select-target");
                payment = Reach(game, next => Choice(next, "select-and-move-owned-card") is not null)!;
            }
            var discard = Choice(payment, "select-and-move-owned-card")!;
            var cost = discard.Cards.Single();
            Submit(game, new AnswerPromptCommand(0, payment.PromptId, discard.Id, game.Revision));
            _ = Reach(game, IsPlay);
            var binding = Facts<ProgramBindingStartedEvent>(game).Single(fact => fact.SkillId == Guanwei);
            Need(game.State.TurnNumber == turn && binding.OwnerSeat == 0 &&
                 Facts<NativeLogicalDiscardMaterialCompletedEvent>(game).Count(fact => fact.CardId == cost && fact.From == CardLocation.Hand(0)) == 1 &&
                 Facts<ProgramDrawCompletedEvent>(game).Count(fact => fact.FrameId == binding.FrameId && fact is { RecipientSeat: 0, RequestedCount: 2, ActualDrawCount: 2 }) == 1 &&
                 Facts<ProgramPhaseScheduledEvent>(game).Count(fact => fact.SkillId == Guanwei && fact.OwnerSeat == 0 && fact.Phase == TurnPhase.Play && fact.Started) == 1,
                "The accepted original Guanwei pays once, draws two actual cards and opens one extra Play phase in the same turn.");
        }
    }

    private static (GameEngine Game, ContentRegistry Content) Start(bool fromEquipment, Suit suit)
    {
        var setup = new ShanheBattleSetup(71, 1, "fixture:guanwei-accepted-suit", Hero, 3, 0, 0, 0, 0, [Wusheng],
            [new(CardKind.DrawTwo, Suit.Heart, 7), new(CardKind.Dodge, Suit.Heart, 8)],
            fromEquipment ? [new(CardKind.QilinBow, suit, 5)] : [],
            [new("classic:guan-yu") { StartingHand = [] }], [], new string('e', 64)) { ExtraSkillSlots = 1 };
        var content = StandardShanheCampaignCatalog.CreateBattleRegistry(setup).WithExtension(new Fixture(), mode =>
            mode.Id == ShanheBattleSetup.BattleModeId ? mode with { DeckId = Deck } : mode);
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
            ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, ConfirmOpeningHand = false,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false
        }, content);
        Submit(game, new StartGameCommand());
        return (game, content);
    }

    private static bool IsPlay(PendingDecision? prompt) => prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 };
    private static bool IsGuanwei(PendingDecision? prompt) => prompt?.SkillPrompt?.SkillId == Guanwei && Choice(prompt, "activate") is not null;
    private static PromptChoice? Choice(PendingDecision? prompt, string verb) =>
        prompt?.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("program-action") == verb);
    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();

    private static PendingDecision? Reach(GameEngine game, Func<PendingDecision?, bool> reached, Func<bool>? ended = null)
    {
        for (var step = 0; step < 100; step++)
        {
            var prompt = game.CreateSnapshot(0).PendingDecision;
            if (prompt is not null && reached(prompt)) return prompt;
            if (ended?.Invoke() == true) return null;
            Need(prompt is null, "Unexpected Guanwei fixture decision: " + prompt?.Prompt);
            Submit(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("Guanwei did not reach the expected public decision or original turn end.");
    }

    private static void Play(GameEngine game, PendingDecision prompt, LegalAction action) =>
        Submit(game, new PlayCardCommand(0, action.CardId!.Value, action.TargetSeats, game.Revision, prompt.PromptId, action.PlayedCardKind, action.TargetCardId)
        { ConversionSource = action.ConversionSource });

    private static void Answer(GameEngine game, PendingDecision prompt, string verb) =>
        Submit(game, new AnswerPromptCommand(0, prompt.PromptId, Choice(prompt, verb)!.Id, game.Revision));

    private static void Submit(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        Need(result.Accepted, result.Error?.Message ?? "Guanwei command rejected.");
    }

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Need(game.CardMovements.SequenceEqual(restored.CardMovements) &&
             JsonSerializer.Serialize(game.ResolutionStack) == JsonSerializer.Serialize(restored.ResolutionStack) &&
             SnapshotJson.Serialize(game.CreateSnapshot(0)) == SnapshotJson.Serialize(restored.CreateSnapshot(0)),
            "Cold replay retains the accepted native materials, current phase and original decision.");
        return restored;
    }

    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-guanwei-accepted-suit", new(1, 0));

        public void Register(IContentRegistryBuilder builder) => builder.AddDeck(new(Deck, "Fixed Guanwei draw supply", 0, 2, [])
        {
            PhysicalCards = Enumerable.Repeat(new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 2), 12).ToArray()
        });
    }
}
