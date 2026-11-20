using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class MouLuMengHengyeNumericChecks
{
    private const string Hero = "mou:lu-meng";
    private const string Opponent = "fixture:hengye-opponent";
    private const string Deck = "fixture:hengye-deck";

    internal static void OneDamageFundsTwoSlashesHandLimitAndNextDrawCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardActiveSkillExpansionPackage(),
            new StandardRescueSkillExpansionPackage(), new Fixture());
        ShanheStartingCard[] starting =
        [
            new(CardKind.Slash, Suit.Spade, 2), new(CardKind.Slash, Suit.Spade, 3), new(CardKind.Slash, Suit.Spade, 4),
            new(CardKind.Dodge, Suit.Heart, 2), new(CardKind.Dodge, Suit.Heart, 3), new(CardKind.Dodge, Suit.Heart, 4)
        ];
        var opponent = new ShanheCombatantDefinition(Opponent, Hp: 8)
        {
            StartingHand = [new(CardKind.Dodge, Suit.Diamond, 6)]
        };
        var setup = new ShanheBattleSetup(31, 1, "hengye-numeric", Hero, 4, 0, 0, 0, 0, [],
            starting, [], [opponent], [], new string('b', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, ConfirmOpeningHand = false, UseInteractiveDiscard = true,
            AdvanceAfterHumanCommands = false, MaxTurns = 4
        }, registry);

        PendingDecision? Pending() => game.CreateSnapshot(0).PendingDecision;
        PlayerSnapshot Owner() => game.CreateSnapshot(0).Players[0];
        T[] Facts<T>() where T : IGameEvent => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();
        bool AtPlay() => Pending() is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 };
        bool AtDiscard() => Pending() is { Kind: DecisionKind.DiscardCards, PlayerSeat: 0 };
        LegalAction[] Slashes() => game.GetLegalActions(0).Where(action => action.Kind == LegalActionKind.Slash &&
            action.ConversionSource is null && action.TargetSeats.SequenceEqual([1])).ToArray();

        void Submit(GameCommand command)
        {
            var result = game.Submit(command);
            Need(result.Accepted, result.Error?.Message ?? "The Hengye fixture command was rejected.");
        }

        void Reach(Func<bool> ready)
        {
            for (var step = 0; step < 100 && !ready(); step++)
            {
                Need(Pending() is null && game.State.Winner == Winner.None,
                    $"The Hengye fixture reached unexpected prompt {Pending()?.Kind} for seat {Pending()?.PlayerSeat}.");
                Submit(new AdvanceOneStepCommand(game.Revision));
            }
            Need(ready(), "The Hengye fixture missed its required native boundary.");
        }

        void Answer(Func<PromptChoice, bool> pick)
        {
            var prompt = Pending()!;
            Submit(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.Single(pick).Id, game.Revision));
        }

        void EndPlay() => Submit(new EndPlayPhaseCommand(0, game.Revision, Pending()!.PromptId));
        PlayCardCommand Slash(LegalAction action) => new(0, action.CardId!.Value, action.TargetSeats,
            game.Revision, Pending()!.PromptId, action.PlayedCardKind);
        int Draws(int turn) => game.CardMovements.Count(move => move.TurnNumber == turn &&
            move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0) && move.Reason == CardMoveReasons.Draw);
        int Uses(int card, int turn) => game.CardMovements.Count(move => move.CardId == card && move.TurnNumber == turn &&
            move.From == CardLocation.Hand(0) && move.To == CardLocation.Processing && move.Reason == CardMoveReasons.Use);

        Submit(new StartGameCommand());
        Reach(AtPlay);
        var firstTurn = game.State.TurnNumber;
        var openingSlashes = Slashes();
        Need(Draws(firstTurn) == 2 && Owner() is { Hp: 4, HandCount: 8, Equipment.Count: 0 } &&
            openingSlashes.Length == 3 && Facts<HengyeGrowthChangedEvent>().Length == 0,
            "Ordinary Draw starts at two cards with three physical Slashes and no field or growth bonus.");
        var first = openingSlashes[0].CardId!.Value;
        var second = openingSlashes[1].CardId!.Value;
        var last = openingSlashes[2].CardId!.Value;
        var dodge = game.CreateCardZoneDiagnostics().Single(card => card.Location == CardLocation.Hand(1)).CardId;

        Submit(Slash(openingSlashes[0]));
        Reach(() => Pending() is { Kind: DecisionKind.Yingbo, PlayerSeat: 0 });
        Answer(choice => choice.Parameters.GetValueOrDefault("action") == "yingbo-skip");
        Reach(AtPlay);
        var growth = Facts<HengyeGrowthChangedEvent>().Single();
        var damage = Facts<DamageRequestedEvent>().Single();
        Need(growth == new HengyeGrowthChangedEvent(damage.ResolutionId, 0, 0, 1) &&
            damage is { SourceSeat: 0, TargetSeat: 1, Amount: 1, SourceCard: CardKind.Slash } &&
            Facts<DamageAppliedEvent>().Single() is { SourceSeat: 0, TargetSeat: 1, Amount: 1 } && Uses(first, firstTurn) == 1,
            "The original first physical Slash deals real damage and issues exactly one growth on that damage frame.");

        var secondAndLast = Slashes();
        var reserved = secondAndLast.Single(action => action.CardId == last);
        Submit(Slash(secondAndLast.Single(action => action.CardId == second)));
        Reach(AtPlay);
        var debits = Facts<CardUseDebitRecordedEvent>().Where(entry => entry.Debit.ActorSeat == 0 &&
            entry.Debit.Query == SkillRuleQuery.SlashLimit && entry.Debit.TurnNumber == firstTurn).ToArray();
        Need(Uses(second, firstTurn) == 1 && debits.Length == 2 &&
            game.CardMovements.Count(move => move.CardId == dodge && move.From == CardLocation.Hand(1) &&
                move.To == CardLocation.Processing && move.Reason == CardMoveReasons.Respond) == 1 &&
            Facts<HengyeGrowthChangedEvent>().SequenceEqual([growth]) && Facts<DamageAppliedEvent>().Length == 1 &&
            Slashes().Length == 0 && Owner() is { Hp: 4, HandCount: 6, Equipment.Count: 0 } &&
            Owner().Hand.Any(card => card.Id == last && card.Kind == CardKind.Slash) &&
            game.CreateSnapshot(0).Players[1].IsAlive && game.GetAttackRange(0) >= game.GetCombatDistance(0, 1),
            "One growth funds the second physical Slash. A real Dodge keeps growth at one and leaves a third physical Slash in range but out of quota.");
        var revision = game.Revision;
        var rejected = game.Submit(Slash(reserved));
        Need(!rejected.Accepted && rejected.Error?.Code == CommandErrorCode.IllegalAction &&
            game.Revision == revision && Uses(last, firstTurn) == 0,
            "Submitting the formerly legal third Slash fails without a card payment.");

        var hand = Owner().Hand.Select(card => card.Id).ToHashSet();
        EndPlay();
        Reach(AtDiscard);
        var obligation = game.ResolutionStack.OfType<ManualDiscardFrame>().Single();
        Need(obligation is { OwnerSeat: 0, InitialRequiredCount: 1 } && obligation.ActualTurnNumber == firstTurn &&
            Pending() is { RequiredCardCount: 1 } beforeDiscard && hand.SetEquals(beforeDiscard.ValidCardIds),
            "Four health and one growth leave five spaces for six physical cards at the actual Discard boundary.");

        var before = game;
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(before.CreateCheckpoint())), registry);
        Need(Enumerable.Range(0, 2).All(seat => SnapshotJson.Serialize(game.CreateSnapshot(seat)) ==
                SnapshotJson.Serialize(before.CreateSnapshot(seat))) && game.CardMovements.SequenceEqual(before.CardMovements) &&
            Facts<HengyeGrowthChangedEvent>().SequenceEqual([growth]) &&
            JsonSerializer.Serialize(game.ResolutionStack.OfType<ManualDiscardFrame>().Single()) == JsonSerializer.Serialize(obligation),
            "Cold restore preserves both public views, the original damage growth and the unpaid one-card discard.");
        var discard = Pending()!;
        var paid = discard.ValidCardIds.First(card => card != last);
        Submit(new DiscardCardsCommand(0, [paid], discard.PromptId, game.Revision));
        Reach(() => Facts<TurnEndedEvent>().Any(turn => turn.ActorSeat == 0 && turn.TurnNumber == firstTurn));
        Need(game.CardMovements.Where(move => move.TurnNumber == firstTurn && move.From == CardLocation.Hand(0) &&
            move.To == CardLocation.DiscardPile && move.Reason == CardMoveReasons.HandLimitDiscard)
            .Select(move => move.CardId).SequenceEqual([paid]) && Owner().HandCount == 5 &&
            hand.Except([paid]).ToHashSet().SetEquals(Owner().Hand.Select(card => card.Id)),
            "The native hand-limit payment discards the selected entity and keeps exactly the other five cards.");

        Reach(AtPlay);
        var nextTurn = game.State.TurnNumber;
        Need(Facts<TurnStartedEvent>().Where(turn => turn.ActorSeat == 0).Select(turn => turn.TurnNumber)
                .SequenceEqual([firstTurn, nextTurn]) && Draws(firstTurn) == 2 && Draws(nextTurn) == 3 &&
            Owner().HandCount == 8 && Facts<HengyeGrowthChangedEvent>().SequenceEqual([growth]),
            "The next actual owner Draw moves three physical cards onto the five retained cards under the same single growth.");
    }

    private static void Need(bool condition, string message)
    {
        if (!condition)
            throw new InvalidOperationException(message);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new StandardClassicGeneralPackage().Manifest;

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            builder.AddGeneral(new(Opponent, "固定相邻对手", "supporter", "standard:none", "wei", 8));
            builder.AddDeck(new(Deck, "横野固定普通摸牌", 0, 2, [new("standard:dodge", 24)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "横野实际数值", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Opponent], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
