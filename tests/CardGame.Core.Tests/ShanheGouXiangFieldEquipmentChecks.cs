using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheGouXiangFieldEquipmentChecks
{
    private const string Owner = "fixture:gouxiang-owner", Enemy = "fixture:gouxiang-enemy", Deck = "fixture:gouxiang-deck";
    private const string Skill = StandardShanheGouXiangArmorPackage.SkillId;
    private const string TakeReason = "skill-program.shanhe:gouxiang-armor-14088.random-hand";

    internal static void MatchingDodgeUsesEffectiveArmorAndHonoursQinggangCold()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheGouXiangArmorPackage(), new Fixture());
        foreach (var bypass in new[] { false, true })
        {
            var setup = new ShanheBattleSetup(71, 1, "gouxiang-native", Owner, 10, 0, 0, 0, 0, [],
                [new(CardKind.Dodge, Suit.Club, 2)], [new(CardKind.GouXiang, Suit.Club, 2)],
                [new(Enemy)
                {
                    StartingHand = [new(CardKind.Slash, Suit.Club, 7), new(CardKind.Peach, Suit.Heart, 3), new(CardKind.Peach, Suit.Diamond, 4)],
                    StartingEquipment = bypass ? [new(CardKind.QinggangSword, Suit.Spade, 6)] : []
                }], [], new string('b', 64));
            var game = GameEngine.CreateStandard(new GameOptions
            {
                Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 3
            }, content);

            PlayerSnapshot Player(int seat) => game.CreateSnapshot(seat).Players[seat];
            PendingDecision? Prompt() => game.CreateSnapshot(0).PendingDecision;
            IEnumerable<T> Facts<T>() => game.Events.Select(entry => entry.Payload).OfType<T>();
            void Need(bool condition, string message)
            {
                if (!condition)
                    throw new InvalidOperationException(message + $" Bypass={bypass}, seat={game.State.CurrentSeat}, " +
                        $"phase={game.State.Phase}, prompt={JsonSerializer.Serialize(Prompt())}.");
            }
            void Accept(GameCommand command)
            {
                var result = game.Submit(command);
                Need(result.Accepted, result.Error?.Message ?? "Gouxiang command was rejected.");
            }
            void Reach(Func<bool> ready)
            {
                for (var step = 0; step < 100; step++)
                {
                    if (ready()) return;
                    var prompt = Prompt();
                    Need(game.State.Winner == Winner.None && (prompt is null ||
                        prompt.Kind == DecisionKind.PlayCard && game.ResolutionStack.Count != 0),
                        "An unexpected boundary interrupted the Gouxiang fixture.");
                    Accept(new AdvanceOneStepCommand(game.Revision));
                }
                Need(false, "The requested Gouxiang command boundary was not reached.");
            }

            Accept(new StartGameCommand());
            Reach(() => Prompt() is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && game.ResolutionStack.Count == 0);
            var armor = Player(0).Equipment.Single();
            var dodge = Player(0).Hand.Single();
            var slashCard = Player(1).Hand.Single(card => card.Kind == CardKind.Slash);
            var otherHand = Player(1).Hand.Where(card => card.Id != slashCard.Id).Select(card => card.Id).ToArray();
            Need(armor is { Kind: CardKind.GouXiang, Suit: Suit.Club, Rank: 2 } &&
                Player(0).Skills?.Any(skill => skill.ContentId == Skill) == true &&
                game.CardMovements.Count(move => move.CardId == armor.Id && move.From == CardLocation.OutsideGame &&
                    move.To == CardLocation.Equipment(0)) == 1 &&
                dodge is { Kind: CardKind.Dodge, Suit: Suit.Club, Rank: 2 } && slashCard is { Suit: Suit.Club, Rank: 7 } &&
                (bypass ? Player(1).Equipment.Single().Kind == CardKind.QinggangSword : Player(1).Equipment.Count == 0),
                "The formal physical Gouxiang grants its skill with distinct same-suit cards and the comparison weapon is genuinely equipped.");

            Accept(new EndPlayPhaseCommand(0, game.Revision, Prompt()!.PromptId));
            Reach(() => Prompt() is { Kind: DecisionKind.RespondDodge, PlayerSeat: 0 });
            var slash = Facts<CardUseDeclaredEvent>().Single(fact => fact.SourceSeat == 1 && fact.CardKind == CardKind.Slash);
            var use = game.ResolutionStack.OfType<CardUseFrame>().Single(frame => frame.Id == slash.ResolutionId);
            Need(slash.CardId == slashCard.Id && use.CardAttack?.IgnoresArmor == bypass && use.Action?.EffectiveSuit == Suit.Club &&
                game.CardMovements.Count(move => move.CardId == slashCard.Id && move.From == CardLocation.Hand(1) &&
                    move.To == CardLocation.Processing) == 1,
                "The genuine original Slash owns the live Dodge prompt and records only the comparison case's armor bypass.");

            var before = Enumerable.Range(0, 2).Select(seat => SnapshotJson.Serialize(game.CreateSnapshot(seat))).ToArray();
            var responsePromptId = Prompt()!.PromptId;
            game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
            Need(before.SequenceEqual(Enumerable.Range(0, 2).Select(seat => SnapshotJson.Serialize(game.CreateSnapshot(seat)))) &&
                Prompt()?.PromptId == responsePromptId && game.ResolutionStack.OfType<CardUseFrame>().Any(frame => frame.Id == use.Id),
                "Cold recovery retains both private views, the original response prompt and the same Slash owner.");
            var responseChoice = Prompt()!.Choices.Single(choice => choice.Cards.SequenceEqual([dodge.Id]));
            Accept(new AnswerPromptCommand(0, responsePromptId, responseChoice.Id, game.Revision));
            Reach(() => Facts<CardUseFinishedEvent>().Any(fact => fact.ResolutionId == use.Id) && game.ResolutionStack.Count == 0);

            var response = Facts<CardActionAcceptedEvent>().Select(fact => fact.Action).Single(action =>
                action.Type == CardActionType.Response && action.EffectiveKind == CardKind.Dodge);
            Need(response.ActorSeat == 0 && response.ProviderSeat == 0 && response.ParentActionId == use.Action!.ActionId &&
                response.PhysicalCards is [var cost] && cost.CardId == dodge.Id && cost.From == CardLocation.Hand(0) &&
                game.CardMovements.Count(move => move.CardId == dodge.Id && move.From == CardLocation.Hand(0) &&
                    move.To == CardLocation.Processing && move.Reason == CardMoveReasons.Respond) == 1 &&
                Facts<CardResponseCompletedEvent>().Count(fact => fact.ActionId == response.ActionId && fact.ParentFrameId == use.Id) == 1 &&
                Facts<CardUseFinishedEvent>().Count(fact => fact.ResolutionId == use.Id) == 1 && Player(0).Hp == 10,
                "The actual Dodge pays its original entity once and returns once to the same Slash in both cases.");

            var expected = bypass ? 0 : 1;
            var transfers = game.CardMovements.Where(move => move.Reason.Value == TakeReason).ToArray();
            var takes = Facts<ProgramRandomHandCardsTakenEvent>().Where(fact => fact.SkillId == Skill).ToArray();
            Need(Player(0).Equipment.Single().Id == armor.Id && Player(0).Skills?.Any(skill => skill.ContentId == Skill) == true &&
                transfers.Length == expected && takes.Length == expected && takes.All(fact => fact.OwnerSeat == 0 &&
                    fact.CardCount == 1 && fact.TargetSeats.SequenceEqual([1])) &&
                transfers.All(move => otherHand.Contains(move.CardId) && move.From == CardLocation.Hand(1) &&
                    move.To == CardLocation.Hand(0) && Player(0).Hand.Any(card => card.Id == move.CardId)) &&
                Player(1).HandCount == otherHand.Length - expected,
                "The same-suit response takes one original enemy hand entity only when Gouxiang's physical armor applies.");
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:gouxiang-native", new(1, 0, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            builder.AddGeneral(new(Owner, "钩镶持有者", "supporter", "standard:none", "shu", 10));
            builder.AddGeneral(new(Enemy, "真实杀来源", "supporter", "standard:none", "qun", 6));
            builder.AddDeck(new(Deck, "无额外自然牌", 0, 0, [new("standard:dodge", 32)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "钩镶真实响应", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
