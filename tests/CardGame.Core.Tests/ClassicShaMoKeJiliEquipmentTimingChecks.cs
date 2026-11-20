using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ClassicShaMoKeJiliEquipmentTimingChecks
{
    private const string Hero = "classic:sha-mo-ke", Jili = "classic:jili";
    private const string Enemy = "fixture:jili-equipment-enemy", Deck = "fixture:jili-equipment-deck";

    internal static void JiliUsesTheOriginalAttackRangeBeforeInstallingEquipmentCold()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(83, 1, "fixture:jili-equipment-timing", Hero, 4, 0, 0, 0, 0, [],
            [new(CardKind.QinggangSword, Suit.Spade, 6), new(CardKind.DrawTwo, Suit.Heart, 7)], [],
            [new(Enemy) { StartingHand = [] }], [], new string('c', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false
        }, content);
        Accept(game, new StartGameCommand());
        Reach(game, () => IsPlay(game));
        var player = game.CreateSnapshot(0).Players[0];
        var weapon = player.Hand.Single(card => card.Kind == CardKind.QinggangSword);
        var drawTwo = player.Hand.Single(card => card.Kind == CardKind.DrawTwo);
        Need(player.Equipment.Count == 0 && Facts<CardUseDeclaredEvent>(game).Length == 0,
            "Sha Mo Ke starts with the ordinary attack range of one and has yet to use a card.");

        Play(game, weapon.Id);
        Reach(game, () => IsJili(game) || IsPlay(game));
        var declared = Facts<CardUseDeclaredEvent>(game).Single(fact => fact.CardId == weapon.Id);
        var paid = game.CardMovements.Single(move => move.CardId == weapon.Id && move.Reason == CardMoveReasons.EquipmentUse);
        var placedBeforeOffer = Facts<EquipmentChangedEvent>(game).Any(fact => fact.ResolutionId == declared.ResolutionId);
        Need(paid.From == CardLocation.Hand(0) && paid.To == CardLocation.Processing && declared.SourceSeat == 0 &&
                IsJili(game) && !placedBeforeOffer,
            $"The first native weapon use must offer Jili at its original range one before installation; installed {placedBeforeOffer}, decision {P(game)?.Prompt}.");
        var use = game.ResolutionStack.OfType<CardUseFrame>().Single(frame => frame.Id == declared.ResolutionId);
        Need(use.Action is { Type: CardActionType.Use, PhysicalCards: [var material] } && material.CardId == weapon.Id &&
            game.CreateCardZoneDiagnostics().Single(card => card.CardId == weapon.Id).Location == CardLocation.Processing &&
            Facts<ProgramDrawCompletedEvent>(game).Length == 0,
            "The offered skill retains the actual paid weapon in Processing under its native use.");

        var decline = Cold(game, content);
        Answer(decline, "skip");
        Reach(decline, () => IsPlay(decline));
        AssertInstalled(decline, declared.ResolutionId, weapon.Id);
        Need(Facts<ProgramDrawCompletedEvent>(decline).Length == 0,
            "Declining the first Jili leaves the original weapon installation intact and grants no draw.");

        game = Cold(game, content);
        Answer(game, "activate");
        Reach(game, () => IsPlay(game));
        AssertInstalled(game, declared.ResolutionId, weapon.Id);
        var firstReward = Facts<ProgramDrawCompletedEvent>(game).Single();
        Need(firstReward is { RecipientSeat: 0, RequestedCount: 1, ActualDrawCount: 1 } &&
            game.Events.Single(entry => Equals(entry.Payload, firstReward)).Sequence <
            game.Events.Single(entry => entry.Payload is EquipmentChangedEvent equipment && equipment.ResolutionId == declared.ResolutionId).Sequence,
            "One real Jili draw at range one completes before the original weapon installation.");

        Play(game, drawTwo.Id);
        Reach(game, () => IsJili(game) || IsPlay(game));
        Need(IsJili(game) && Facts<CardUseDeclaredEvent>(game).Select(fact => fact.CardId).SequenceEqual([weapon.Id, drawTwo.Id]),
            "The second actual use qualifies against the installed Qinggang Sword's range two.");
        Answer(game, "activate");
        Reach(game, () => IsPlay(game));
        var rewards = Facts<ProgramDrawCompletedEvent>(game);
        Need(rewards.Length == 2 && rewards[0] == firstReward &&
            rewards[1] is { RecipientSeat: 0, RequestedCount: 2, ActualDrawCount: 2 } &&
            Facts<CardUseFinishedEvent>(game).Count(fact => fact.CardId == drawTwo.Id) == 1,
            "The same turn's second qualifying use grants two cards through Jili and finishes its own Draw Two once.");
        _ = Cold(game, content);
    }

    private static void AssertInstalled(GameEngine game, long useId, int cardId) => Need(
        Facts<CardUseFinishedEvent>(game).Count(fact => fact.ResolutionId == useId && fact.CardId == cardId) == 1 &&
        Facts<EquipmentChangedEvent>(game).Count(fact => fact.ResolutionId == useId && fact.CardId == cardId) == 1 &&
        game.CardMovements.Count(move => move.CardId == cardId && move.From == CardLocation.Processing &&
            move.To == CardLocation.Equipment(0) && move.Reason == CardMoveReasons.EquipmentEnter) == 1,
        "The original physical weapon installs and its use finishes exactly once.");

    private static PendingDecision? P(GameEngine game) => game.CreateSnapshot(0).PendingDecision;
    private static bool IsPlay(GameEngine game) => P(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard };
    private static bool IsJili(GameEngine game) => P(game) is { PlayerSeat: 0, SkillPrompt.SkillId: Jili } prompt &&
        prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
    private static T[] Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();

    private static void Play(GameEngine game, int cardId)
    {
        var action = game.GetHumanLegalActions().Single(item => item.CardId == cardId);
        Accept(game, new PlayCardCommand(0, cardId, action.TargetSeats, game.Revision, P(game)!.PromptId, action.PlayedCardKind));
    }

    private static void Answer(GameEngine game, string branch)
    {
        var prompt = P(game)!;
        var choice = prompt.Choices.Single(item => item.Parameters.GetValueOrDefault("program-action") == branch);
        Accept(game, new AnswerPromptCommand(0, prompt.PromptId, choice.Id, game.Revision));
    }

    private static void Reach(GameEngine game, Func<bool> reached)
    {
        for (var step = 0; step < 60; step++)
        {
            if (reached())
                return;
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("Jili did not reach the expected decision or return: " + P(game)?.Prompt);
    }

    private static void Accept(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        Need(result.Accepted, result.Error?.Message ?? "The real Jili command was rejected.");
    }

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Need(restored.CardMovements.SequenceEqual(game.CardMovements) &&
            JsonSerializer.Serialize(restored.ResolutionStack) == JsonSerializer.Serialize(game.ResolutionStack) &&
            game.Events.Select(entry => JsonSerializer.Serialize(entry.Payload, entry.Payload.GetType())).SequenceEqual(
                restored.Events.Select(entry => JsonSerializer.Serialize(entry.Payload, entry.Payload.GetType()))) &&
            Enumerable.Range(0, game.PlayerCount).All(seat => SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold commands preserve the private Jili choice and the actual equipment use order.");
        return restored;
    }

    private static void Need(bool condition, string message)
    {
        if (!condition)
            throw new InvalidOperationException(message);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-classic-jili-equipment-timing", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            builder.AddGeneral(new(Enemy, "无固定技能的对手", "supporter", "standard:none", "wei", 4));
            builder.AddDeck(new(Deck, "固定闪", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 24)
                .Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, 2)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "蒺藜使用装备时机", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
