using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class MobileZhugeJinMingzheDiscardSuitChecks
{
    private const string Mingzhe = "mobile:mingzhe", Hongyan = "boundary:hongyan-mobile";
    private const string Mode = "identity:mingzhe-discard-suit", Deck = "fixture:mingzhe-discard-deck";
    private static readonly string[] Generals = ["fixture:mingzhe-attacker", "fixture:mingzhe-owner", "fixture:mingzhe-peer-2", "fixture:mingzhe-peer-3"];

    internal static void DismantlementUsesTheOriginalHandSuitForMingzheCold()
    {
        Check(Suit.Heart, Suit.Heart);
        Check(Suit.Club, Suit.Club);
        Check(Suit.Spade, Suit.Heart);
    }

    private static void Check(Suit printed, Suit effective)
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture(printed));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 17, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
            ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false
        }, content);
        Accept(game, new StartGameCommand());
        var play = Next(game);
        while (play.Kind == DecisionKind.SelectGeneral)
        {
            Accept(game, new SelectGeneralCommand(play.PlayerSeat, Generals[play.PlayerSeat], game.Revision, play.PromptId));
            play = Next(game);
        }

        var owner = game.CreateSnapshot(1).Players[1];
        var lost = owner.Hand.Single();
        var trick = game.CreateSnapshot(0).Players[0].Hand.Single();
        Require(play is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } && owner.Skills?.Any(skill => skill.Id == Mingzhe) == true &&
            owner.Skills.Any(skill => skill.Id == Hongyan) && lost.Suit == printed && trick.Kind == CardKind.Dismantlement,
            "The unaffected actor has a physical Dismantlement and the other player has one real hand card, Mingzhe and mobile Hongyan.");
        Accept(game, new PlayCardCommand(0, trick.Id, [1], game.Revision, play.PromptId));
        var selection = Next(game);
        while (selection.Kind == DecisionKind.Nullification)
        {
            Answer(game, choice => choice.Parameters.GetValueOrDefault("response") == "pass");
            selection = Next(game);
        }

        var original = game.ResolutionStack.OfType<TargetCardSelectionFrame>().Single();
        Require(selection is { Kind: DecisionKind.SelectTargetCard, PlayerSeat: 0, TargetSeat: 1 } &&
            selection.Choices is [{ Cards.Count: 0 } choice] && choice.Parameters.GetValueOrDefault("slot-index") == "0",
            "The original native trick asks for the target's single anonymous hand slot.");
        var cold = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(cold.CardMovements.SequenceEqual(game.CardMovements) &&
            JsonSerializer.Serialize(cold.ResolutionStack) == JsonSerializer.Serialize(game.ResolutionStack) &&
            Enumerable.Range(0, game.PlayerCount).All(seat => SnapshotJson.Serialize(cold.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold restoration preserves the native trick and its private original hand-slot prompt.");
        game = cold;
        Answer(game, selected => selected.Parameters.GetValueOrDefault("slot-index") == "0");
        var next = Next(game);
        var payment = game.CardMovements.Where(move => move.CardId == lost.Id &&
            (move.Reason == CardMoveReasons.Dismantlement || move.Reason == CardMoveReasons.DismantlementFinished)).ToArray();
        Require(payment is [var enter, var finish] && enter.From == CardLocation.Hand(1) && enter.To == CardLocation.Processing &&
            finish.From == CardLocation.Processing && finish.To == CardLocation.DiscardPile && enter.Sequence < finish.Sequence &&
            Facts<TargetCardDiscardedEvent>(game).Single() is { SourceSeat: 0, TargetSeat: 1, FromZone: CardZoneKind.Hand, PublicCardId: null } discard &&
            discard.ResolutionId == original.ParentFrameId && game.State.CurrentSeat == 0 &&
            game.CreateSnapshot(1).Players[1].HandCount == 0,
            "The other player's original hand entity is physically discarded by the actual native trick during the attacker's turn.");

        var mingzhe = Facts<ProgramBindingResolvedEvent>(game).Where(fact => fact.SkillId == Mingzhe).ToArray();
        if (effective == Suit.Club)
        {
            Require(next is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } && payment[0].RedOwnedLoss is null &&
                mingzhe.All(fact => !fact.Activated),
                $"The unchanged black club does not provide a Mingzhe opportunity. Actual frozen suit {payment[0].RedOwnedLoss?.EffectiveSuit}, " +
                $"next {next.Kind}/{next.SkillPrompt?.SkillId} for seat {next.PlayerSeat}, results {string.Join(", ", mingzhe.Select(fact => $"{fact.BindingId} started={fact.Activated}"))}.");
            return;
        }

        Require(next is { PlayerSeat: 1, SkillPrompt.SkillId: Mingzhe } &&
            payment[0].RedOwnedLoss is { OwnerSeat: 1, EffectiveSuit: Suit.Heart } loss && loss.MovementSequence == payment[0].Sequence,
            $"Printed {printed} has its original heart identity in the paid hand, so native Dismantlement must offer Mingzhe. " +
            $"Actual frozen suit {payment[0].RedOwnedLoss?.EffectiveSuit}, next {next.SkillPrompt?.SkillId ?? next.Kind.ToString()}.");
        Activate(game);
        Require(Next(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } &&
            game.CardMovements.Count(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(1)) == 2 &&
            Facts<ProgramBindingResolvedEvent>(game).Count(fact => fact.SkillId == Mingzhe && fact is { OwnerSeat: 1, Activated: true, Completed: true }) == 1 &&
            Facts<CardUseFinishedEvent>(game).Count(fact => fact.ResolutionId == original.ParentFrameId && fact.CardId == trick.Id) == 1,
            "One real red discard gives one optional native draw and the original trick returns to Play exactly once.");
    }

    private static PendingDecision Next(GameEngine game) { Reach(game, _ => true); return P(game)!; }

    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();

    private sealed class Fixture(Suit printed) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-mingzhe-discard-suit", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "mobile:zhuge-jin", "ol:zhuge-jin", "boundary:xiao-qiao-mobile");
            builder.AddGeneral(new(Generals[1], "明哲角色", "supporter", Mingzhe, "wu", 3, [Hongyan]));
            foreach (var peer in Generals.Where(id => id != Generals[1]))
                builder.AddGeneral(new(peer, "同桌", "supporter", "standard:none", "wei", 4));
            builder.AddDeck(new(Deck, "确定花色的拆牌", InitialHandSize: 1, DrawPerTurn: 0, [])
            {
                PhysicalCards = Enumerable.Range(2, 6).Select(rank => new ContentDeckPhysicalCard("standard:dismantlement", printed, rank)).ToArray()
            });
            builder.AddMode(new(Mode, "明哲真实弃牌花色", 4, 4, new Dictionary<string, int>
            {
                [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2
            }, Deck, GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}
