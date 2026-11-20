using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class MobileYouyiGroupRecoveryChecks
{
    private const string HuaTuo = "mobile:shen-hua-tuo", Qiao = "happy:shen-da-qiao-xiao-qiao";
    private const string Youyi = "mobile-god:youyi", Shutu = "happy:shutu";
    private const string Deposit = "skill.mobile-shen-hua-tuo.youyi.deposit", Pay = "skill.mobile-shen-hua-tuo.youyi.pay";
    private const string Mode = "identity:classic-mobile-youyi-group", Deck = "fixture:youyi-group-deck";
    private static readonly string[] Generals = [HuaTuo, Qiao, "fixture:youyi-group-peer-2", "fixture:youyi-group-peer-3"];

    internal static void YouyiKeepsItsPaidGroupRecoveryAcrossUnequalQiaoPoolsCold()
    {
        var (game, content) = Start();
        var firstTurn = game.State.TurnNumber;
        var full = Health(game);
        EndPlay(game, 0);
        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Youyi });
        Activate(game);
        Reach(game, prompt => prompt is { PlayerSeat: 1, Kind: DecisionKind.PlayCard });
        var deposited = game.CardMovements.Where(move => move.Reason.Value == Deposit).ToArray();
        Require(deposited.Length > 0 && deposited.All(move => move.From == CardLocation.DiscardPile &&
            move.To == CardLocation.Benevolence && game.CardMovements.Any(cost => cost.CardId == move.CardId &&
                cost.Sequence < move.Sequence && cost.From == CardLocation.Hand(0) && cost.Reason == CardMoveReasons.HandLimitDiscard)),
            "Youyi fills Ren with real hand-limit discards from its first actual turn.");

        EndPlay(game, 1);
        Reach(game, prompt => prompt is { PlayerSeat: 2, Kind: DecisionKind.PlayCard });
        var otherFull = game.CreateSnapshot(2).Players[2].Hp;
        EndPlay(game, 2);
        Reach(game, prompt => prompt is { PlayerSeat: 3, Kind: DecisionKind.PlayCard });
        Slash(game, 3, 2);
        Reach(game, prompt => prompt is { PlayerSeat: 3, Kind: DecisionKind.PlayCard });
        EndPlay(game, 3);
        Reach(game, prompt => prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        Require(game.State.TurnNumber > firstTurn && Health(game) == full && game.CreateSnapshot(2).Players[2].Hp == otherFull - 1,
            "The next Hua Tuo play phase starts with equal Qiao pools and a wounded later recipient.");

        Slash(game, 0, 1);
        Reach(game, IsQiaoPoolChoice);
        Answer(game, SelectsXiaoQiao);
        Reach(game, prompt => prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } ||
            prompt is { PlayerSeat: 1, SkillPrompt.SkillId: Shutu });
        if (P(game)!.SkillPrompt?.SkillId == Shutu)
        {
            Skip(game);
            Reach(game, prompt => prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        }
        var wounded = Health(game);
        Require(wounded.DaQiaoHp == full.DaQiaoHp && wounded.XiaoQiaoHp == full.XiaoQiaoHp - 1 &&
            Facts<DamageAppliedEvent>(game).Any(entry => entry is { SourceSeat: 0, TargetSeat: 1, Amount: 1 }),
            "The real Slash wounds only Xiao Qiao immediately before the Youyi activation.");

        game = Cold(game, content);
        Accept(game, new UseProgramSkillCommand(0, Youyi, "clear-ren-and-heal", [], [], game.Revision, P(game)!.PromptId));
        Reach(game, IsQiaoPoolChoice);
        var root = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Youyi);
        var request = Facts<HappyQiaoHealthRequestedEvent>(game).Single(entry => entry.OwnerSeat == 1 && entry.Kind == HpChangeKind.Recovery);
        var payment = Facts<MobileHuaTuoPaidEvent>(game).Single(entry => entry.Kind == "ren-payment");
        Require(root.MobileHuaTuo?.Recipients.SequenceEqual([0, 1, 2, 3]) == true && request.Amount == 1 &&
            payment.FrameId == root.Id && game.CardMovements.Where(move => move.Reason.Value == Pay)
                .Select(move => move.CardId).SequenceEqual(deposited.Select(move => move.CardId)) &&
            Health(game) == wounded && game.CreateSnapshot(2).Players[2].Hp == otherFull - 1,
            "After paying real Ren once, the frozen group waits at its original Qiao recovery child before visiting the next seat.");

        game = Cold(game, content);
        Answer(game, SelectsXiaoQiao);
        Settle(game);
        Require(Health(game) == full && game.CreateSnapshot(2).Players[2].Hp == otherFull &&
            Facts<HappyQiaoHealthAppliedEvent>(game).Single(entry => entry.RequestId == request.RequestId) is
                { Pool: HappyQiaoPool.XiaoQiao, Kind: HpChangeKind.Recovery } applied && applied.HpAfter == applied.HpBefore + 1 &&
            Facts<RecoveryAppliedEvent>(game).Count(entry => entry is { SourceSeat: 0, TargetSeat: 2, Amount: 1 }) == 1 &&
            Facts<MobileHuaTuoPaidEvent>(game).Count(entry => entry.Kind == "ren-payment") == 1 &&
            game.CardMovements.Count(move => move.Reason.Value == Pay) == deposited.Length,
            "The original group continues after the restored pool choice, healing both seats once and paying Ren once.");
    }

    private static bool IsQiaoPoolChoice(PendingDecision prompt) => prompt.PlayerSeat == 1 &&
        prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "happy-qiao-health");
    private static bool SelectsXiaoQiao(PromptChoice choice) => choice.Parameters.GetValueOrDefault("pool") == nameof(HappyQiaoPool.XiaoQiao);
    private static HappyQiaoDualHealth Health(GameEngine game) => game.CreateSnapshot(1).Players[1].HappyQiaoHealth!;
    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();

    private static void Slash(GameEngine game, int actor, int target)
    {
        var slash = game.GetLegalActions(actor).First(action => action.Kind == LegalActionKind.Slash && action.TargetSeats.SequenceEqual([target]));
        Accept(game, new PlayCardCommand(actor, slash.CardId!.Value, slash.TargetSeats, game.Revision, P(game)!.PromptId, slash.PlayedCardKind));
    }

    private static void EndPlay(GameEngine game, int actor) => Accept(game, new EndPlayPhaseCommand(actor, game.Revision, P(game)!.PromptId));

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) && Enumerable.Range(0, game.PlayerCount).All(seat =>
            SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold replay preserves actual Ren movements and the original health child on every seat's projection.");
        return restored;
    }

    private static (GameEngine Game, ContentRegistry Content) Start()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 47, PlayerCount = 4, ModeId = Mode,
            HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, UseInteractiveSetup = true,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        Accept(game, new StartGameCommand());
        Reach(game, prompt => prompt.Kind is DecisionKind.SelectGeneral or DecisionKind.SelectFaction or DecisionKind.PlayCard);
        while (P(game) is { Kind: DecisionKind.SelectGeneral or DecisionKind.SelectFaction } selection)
        {
            if (selection.Kind == DecisionKind.SelectGeneral)
                Accept(game, new SelectGeneralCommand(selection.PlayerSeat, Generals[selection.PlayerSeat], game.Revision, selection.PromptId));
            else
                Answer(game, choice => choice.Parameters.GetValueOrDefault("faction-id") == "shu");
            Reach(game, prompt => prompt.Kind is DecisionKind.SelectGeneral or DecisionKind.SelectFaction or DecisionKind.PlayCard);
        }

        return (game, content);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-youyi-group", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, HuaTuo, Qiao);
            foreach (var peer in Generals.Skip(2))
                builder.AddGeneral(new(peer, "同桌", "supporter", "standard:none", "wei", 8));
            builder.AddDeck(new(Deck, "固定实体黑杀", 7, 2, []) { PhysicalCards = Enumerable.Range(0, 64)
                .Select(_ => new ContentDeckPhysicalCard("standard:slash", Suit.Spade, 7)).ToArray() });
            builder.AddMode(new(Mode, "游医全场回复", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}
