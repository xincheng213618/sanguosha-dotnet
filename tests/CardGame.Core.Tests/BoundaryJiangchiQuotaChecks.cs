using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using static BoundaryLiDianChecks;

internal static class BoundaryJiangchiQuotaChecks
{
    private const string Hero = "boundary:cao-zhang", Jiangchi = "boundary:jiangchi", Xianzhen = "boundary:xianzhen-current";
    private const string Enemy = "fixture:jiangchi-quota-enemy", Deck = "fixture:jiangchi-quota-deck";

    internal static void RecastAllowsTwoFiniteSlashUsesAndExpiresApartFromDirectedAllowanceCold()
    {
        var (game, content) = Start();
        var turn = game.State.TurnNumber;
        var originalHand = game.CreateSnapshot(0).Players[0].Hand;
        var recastId = originalHand.Single(card => card.Kind == CardKind.Dodge).Id;
        var contestId = originalHand.Single(card => card.Rank == 13).Id;
        Require(Facts<CardsDrawnEvent>(game).Single(entry => entry.PlayerSeat == 0).Count == 2 &&
            !Facts<TurnRuleModifierGrantedEvent>(game).Any(),
            "Genuine normal Draw finishes before the original OL boundary Jiangchi menu publishes.");

        var declined = Cold(game, content);
        Skip(declined);
        Settle(declined);
        Require(Slashes(declined).Any(action => action.TargetSeats.SequenceEqual([1])) &&
            Slashes(declined).All(action => !action.TargetSeats.Contains(2)),
            "Declining Jiangchi leaves one ordinary Slash and normal distance to the far seat.");
        UseSlash(declined, 1, contestId);
        Require(Debits(declined, turn) == 1 && Slashes(declined).Length == 0,
            "The ordinary first Slash really spends the only available use.");

        Answer(game, IsRecastBranch);
        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Jiangchi } &&
            prompt.Choices.Any(choice => choice.Cards.SequenceEqual([recastId])));
        game = Cold(game, content);
        Answer(game, choice => choice.Cards.SequenceEqual([recastId]));
        Reach(game, prompt => IsJiangchiChoice(prompt) || prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        var cost = game.CardMovements.Single(move => move.CardId == recastId && move.Reason == CardMoveReasons.RecastDiscard);
        var draw = game.CardMovements.Single(move => move.Reason == CardMoveReasons.RecastDraw);
        var grants = Facts<TurnRuleModifierGrantedEvent>(game).Select(entry => entry.Modifier).Where(grant => grant.Source.SkillId == Jiangchi).ToArray();
        Require(cost.From == CardLocation.Hand(0) && cost.To == CardLocation.DiscardPile &&
            draw.From == CardLocation.DrawPile && draw.To == CardLocation.Hand(0) && draw.Sequence > cost.Sequence &&
            grants.Length == 2 && grants.All(grant => grant.TurnSeat == 0 && grant.TurnNumber == turn) &&
            grants.Any(grant => grant is { Query: SkillRuleQuery.SlashLimit, Operation: SkillRuleOperation.Add, Amount: 1 }) &&
            grants.Any(grant => grant is { Query: SkillRuleQuery.SlashDistanceLimit, Operation: SkillRuleOperation.Unlimited }),
            "The physical recast pays once and grants a finite count bonus separately from unlimited distance.");
        Require(P(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard },
            "The paid recast completes the original Draw-end choice group and advances directly to Play.");

        UseSlash(game, 1, contestId);
        UseSlash(game, 2, contestId);
        Require(Debits(game, turn) == 2 && Slashes(game).Length == 0 &&
            game.CreateSnapshot(0).Players[0].Hand.Any(card => card.Kind == CardKind.Slash && card.Id != contestId),
            "The two actual Slashes exhaust the quota even though the owner still holds a third and all distances were allowed.");

        Accept(game, new UseProgramSkillCommand(0, Xianzhen, "contest", [contestId], [2], game.Revision, P(game)!.PromptId));
        Settle(game);
        Require(Facts<PindianResultDeterminedEvent>(game).Single().Result is
                { SourceSeat: 0, OpponentSeat: 2, SourceCardId: var chosen, SourceWon: true } && chosen == contestId &&
            Facts<DirectedTurnCardPolicyGrantedEvent>(game).Single().Policy is { ActorSeat: 0, TargetSeat: 2 } policy &&
            policy.Effects.HasFlag(DirectedTurnCardPolicyEffect.BypassSlashLimit) &&
            Slashes(game) is { Length: > 0 } legal && legal.All(action => action.TargetSeats.SequenceEqual([2])),
            "Real OL Xianzhen wins Pindian and restores Slash only against the original marked target.");
        game = Cold(game, content);
        UseSlash(game, 2);
        Require(Debits(game, turn) == 2 &&
            Slashes(game) is { Length: > 0 } remaining && remaining.All(action => action.TargetSeats.SequenceEqual([2])),
            "A real third Slash against the marked seat neither spends another ordinary use nor helps against another target.");

        Accept(game, new EndPlayPhaseCommand(0, game.Revision, P(game)!.PromptId));
        Reach(game, prompt => IsJiangchiChoice(prompt) && game.State.TurnNumber > turn);
        Skip(game);
        Settle(game);
        var nextTurn = game.State.TurnNumber;
        Require(Facts<TurnEndedEvent>(game).Any(entry => entry.TurnNumber == turn && entry.ActorSeat == 0) &&
            Facts<TurnCardUseEffectsExpiredEvent>(game).Single(entry => entry.TurnNumber == turn && entry.TurnSeat == 0)
                .GrantSequences.Intersect(grants.Select(grant => grant.GrantSequence)).Count() == 2 &&
            Slashes(game).Any(action => action.TargetSeats.SequenceEqual([1])) &&
            Slashes(game).All(action => !action.TargetSeats.Contains(2)),
            "Both Jiangchi grants expire at the original actual turn end, and the directed exemption does not return next turn.");
        UseSlash(game, 1);
        Require(Debits(game, nextTurn) == 1 && Slashes(game).Length == 0,
            "The next actual turn, with Jiangchi declined, again permits exactly one ordinary Slash.");
        _ = Cold(game, content);
    }

    private static bool IsJiangchiChoice(PendingDecision prompt) =>
        prompt is { PlayerSeat: 0, Kind: DecisionKind.ProgramTrigger, SkillPrompt.SkillId: Jiangchi } &&
        prompt.Choices.Any(IsRecastBranch);

    private static bool IsRecastBranch(PromptChoice choice) =>
        choice.Parameters.GetValueOrDefault("program-action") == "activate" &&
        choice.Parameters.GetValueOrDefault("choice-group") == "turn-mode" &&
        choice.Parameters.GetValueOrDefault("binding-id") == "recast-assault";

    private static int Debits(GameEngine game, int turn) =>
        Facts<CardUseDebitRecordedEvent>(game).Count(entry => entry.Debit.ActorSeat == 0 && entry.Debit.TurnNumber == turn &&
            entry.Debit.Query == SkillRuleQuery.SlashLimit);

    private static LegalAction[] Slashes(GameEngine game) =>
        game.GetHumanLegalActions().Where(action => action.Kind == LegalActionKind.Slash && action.CardId is > 0).ToArray();

    private static void UseSlash(GameEngine game, int target, int? reservedCard = null)
    {
        var slash = Slashes(game).First(action => action.CardId != reservedCard && action.TargetSeats.SequenceEqual([target]));
        Accept(game, new PlayCardCommand(0, slash.CardId!.Value, slash.TargetSeats, game.Revision, P(game)!.PromptId, slash.PlayedCardKind));
        Settle(game);
    }

    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) &&
            Enumerable.Range(0, 4).All(seat => SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold replay preserves physical payments, original choices and all four public and private views.");
        return restored;
    }

    private static (GameEngine Game, ContentRegistry Content) Start()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(47, 1, "boundary-jiangchi-quota", Hero, 4, 0, 0, 0, 0, [Xianzhen],
            [new(CardKind.Dodge, Suit.Club, 9), new(CardKind.Slash, Suit.Spade, 13), new(CardKind.Slash, Suit.Spade, 7),
                new(CardKind.Slash, Suit.Spade, 8), new(CardKind.Slash, Suit.Spade, 9)], [],
            [new(Enemy) { StartingHand = [] }, new(Enemy) { StartingHand = [new(CardKind.Dodge, Suit.Heart, 2), new(CardKind.Slash, Suit.Club, 2)] },
                new(Enemy) { StartingHand = [] }], [], new string('c', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
            ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false
        }, content);
        Accept(game, new StartGameCommand());
        Reach(game, IsJiangchiChoice);
        return (game, content);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-jiangchi-quota", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero, "boundary:gao-shun");
            builder.AddGeneral(new(Enemy, "固定耐久对手", "supporter", "standard:none", "qun", 8));
            builder.AddDeck(new(Deck, "固定实体杀牌堆", 0, 2, [])
            {
                PhysicalCards = Enumerable.Range(0, 72).Select(_ => new ContentDeckPhysicalCard("standard:slash", Suit.Spade, 3)).ToArray()
            });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "界将驰有限次数与定向免限", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}
