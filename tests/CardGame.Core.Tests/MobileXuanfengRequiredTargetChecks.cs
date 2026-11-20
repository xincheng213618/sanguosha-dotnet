using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using static BoundaryLiDianChecks;

internal static class MobileXuanfengRequiredTargetChecks
{
    private const string Hero = "boundary:ling-tong-mobile", Skill = "boundary:xuanfeng-mobile", Branch = "xuanfeng-branch";
    private const string Source = "fixture:xuanfeng-target-source", Destination = "fixture:xuanfeng-target-destination";
    private const string Deck = "fixture:xuanfeng-target-deck";

    internal static void EquipmentLossOffersOnlyAvailableBranchesCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        Exercise(registry, movableEquipment: true);
        Exercise(registry, movableEquipment: false);
    }

    private static void Exercise(ContentRegistry registry, bool movableEquipment)
    {
        var setup = new ShanheBattleSetup(7, 1, "xuanfeng-target", Hero, 4, 0, 0, 0, 0, [],
            [new(CardKind.QinggangSword, Suit.Spade, 6)], [new(CardKind.Crossbow, Suit.Club, 1)],
            [new(Source) { StartingHand = [], StartingEquipment = movableEquipment ? [new(CardKind.Crossbow, Suit.Diamond, 1)] : [] },
             new(Destination) { StartingHand = [new(CardKind.Peach, Suit.Heart, 3)] }], [], new string('d', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 3, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false
        }, registry);

        PendingDecision? Prompt() => game.CreateSnapshot(0).PendingDecision;
        PendingDecision ReachOwner(Func<PendingDecision, bool> ready)
        {
            for (var step = 0; step < 80 && game.State.Winner == Winner.None; step++)
            {
                if (Prompt() is { } prompt)
                {
                    if (ready(prompt)) return prompt;
                    throw new InvalidOperationException($"Unexpected Xuanfeng fixture prompt {prompt.Kind}/{prompt.SkillPrompt?.SkillId}.");
                }
                Accept(game, new AdvanceOneStepCommand(game.Revision));
            }
            throw new InvalidOperationException($"Xuanfeng fixture missed its expected choice in {game.State.Phase}.");
        }
        void AnswerOwner(PendingDecision prompt, PromptChoice choice) =>
            Accept(game, new AnswerPromptCommand(0, prompt.PromptId, choice.Id, game.Revision));

        Accept(game, new StartGameCommand());
        var play = ReachOwner(prompt => prompt.Kind == DecisionKind.PlayCard);
        var players = game.CreateSnapshot(0).Players;
        var source = players.Single(player => player.GeneralId == Source);
        var destination = players.Single(player => player.GeneralId == Destination);
        Require(source is { IsAlive: true, HandCount: 0 } && source.Equipment.Count == (movableEquipment ? 1 : 0) &&
            destination is { IsAlive: true, HandCount: 1, Equipment.Count: 0 },
            "The potential equipment source has no hand, and the other peer has an empty slot and a discardable card.");
        var oldWeapon = players[0].Equipment.Single(card => card.Kind == CardKind.Crossbow).Id;
        var newWeapon = players[0].Hand.Single(card => card.Kind == CardKind.QinggangSword).Id;
        var equip = game.GetHumanLegalActions().Single(action => action.Kind == LegalActionKind.Equip && action.CardId == newWeapon);
        Accept(game, new PlayCardCommand(0, newWeapon, equip.TargetSeats, game.Revision, play.PromptId, equip.PlayedCardKind));
        var offer = ReachOwner(prompt => prompt.SkillPrompt?.SkillId == Skill && prompt.Choices.Any(choice =>
            choice.Parameters.GetValueOrDefault("program-action") == "activate"));
        var loss = game.CardMovements.Single(move => move.CardId == oldWeapon && move.From == CardLocation.Equipment(0) &&
            move.To == CardLocation.DiscardPile && move.Reason == CardMoveReasons.EquipmentReplace);
        var window = game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Single(frame => frame.Batch.Movements.Contains(loss));
        Require(window.Candidates.Any(candidate => candidate.SkillId == Skill && candidate.BindingId == "equipment-left") &&
            game.CreateSnapshot(0).Players[0].Equipment.Single().Id == newWeapon,
            "The original weapon replacement publishes the real equipment-loss opportunity.");
        AnswerOwner(offer, offer.Choices.Single(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate"));

        var options = ReachOwner(prompt => prompt.SkillPrompt?.SkillId == Skill &&
            prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("result-bind") == Branch));
        var root = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Skill);
        string[] expected = movableEquipment ? ["discard", "move"] : ["discard"];
        Require(options is { PlayerSeat: 0, IsPrivate: true } && root.WindowContext?.ParentFrameId == window.Id &&
            options.Choices.Select(choice => choice.Parameters.GetValueOrDefault("option-id")).SequenceEqual(expected),
            "The movement option requires a legal equipment source, including the equipped peer with no hand.");

        var original = game;
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Require(Enumerable.Range(0, game.PlayerCount).All(seat => SnapshotJson.Serialize(game.CreateSnapshot(seat)) ==
            SnapshotJson.Serialize(original.CreateSnapshot(seat))) && game.CardMovements.SequenceEqual(original.CardMovements),
            "Cold replay preserves the equipment loss and the private branch menu.");
        var option = movableEquipment ? "move" : "discard";
        var restored = Prompt()!;
        AnswerOwner(restored, restored.Choices.Single(choice => choice.Parameters.GetValueOrDefault("option-id") == option));
        var nextAction = movableEquipment ? "boundary-ling-tong-equipment" : "choose-other-owned-card-discard";
        var next = ReachOwner(prompt => prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == nextAction));
        var active = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == root.Id);
        Require(active.ChoiceBindings.Single(binding => binding.Name == Branch).OptionId == option &&
            (movableEquipment
                ? next.Choices is [var pair] && pair.Targets.SequenceEqual([source.Seat, destination.Seat])
                : next.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == nextAction &&
                    choice.Targets.SequenceEqual([destination.Seat]))),
            "The accepted branch opens its actual next target choice on the original equipment-loss frame.");
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-xuanfeng-target", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            builder.AddGeneral(new(Source, "装备来源", "supporter", "standard:none", "wei", 4));
            builder.AddGeneral(new(Destination, "空栏同桌", "supporter", "standard:none", "wei", 4));
            builder.AddDeck(new(Deck, "旋风固定牌堆", 0, 1, [])
            {
                PhysicalCards = Enumerable.Range(0, 24).Select(_ => new ContentDeckPhysicalCard("standard:peach", Suit.Heart, 3)).ToArray()
            });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "旋风分支", 3, 3, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Source, Destination], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
