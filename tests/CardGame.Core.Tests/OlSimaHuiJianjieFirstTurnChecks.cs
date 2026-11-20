using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class OlSimaHuiJianjieFirstTurnChecks
{
    private const string Hero = "ol:sima-hui", Jianjie = "ol:jianjie";
    private const string Huoji = "ol:huoji-jianjie", Lianhuan = "ol:lianhuan-jianjie";
    private const string Mode = "identity:jianjie-first-turn", Deck = "fixture:jianjie-first-turn-deck";
    private static readonly string[] Generals =
        [Hero, "fixture:jianjie-dragon", "fixture:jianjie-phoenix", "fixture:jianjie-peer"];

    internal static void OrderedSealsGrantBeforeOnlyFirstActualTurnCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 7, PlayerCount = 4, ModeId = Mode, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
            UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8
        }, registry);

        T[] Facts<T>() where T : IGameEvent => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();
        ProgramSkillFrame Root() => game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Jianjie);
        PendingDecision Next()
        {
            for (var step = 0; step < 80 && game.State.Winner == Winner.None; step++)
            {
                if (P(game) is { } prompt) return prompt;
                Accept(game, new AdvanceOneStepCommand(game.Revision));
            }
            throw new InvalidOperationException("The fixed Jianjie match missed its next public decision.");
        }
        void Choose(int seat) => Answer(game, choice => choice.Targets.SequenceEqual([seat]));
        bool Holds(int seat, PlayerMarkerKind kind, string skill)
        {
            var player = game.CreateSnapshot(seat).Players[seat];
            return player.Markers?.Any(marker => marker.Kind == kind && marker.Count == 1) == true &&
                player.Skills?.Any(definition => definition.ContentId == skill) == true;
        }
        void Cold()
        {
            var before = game;
            var seals = Facts<SimaHuiSealMovedEvent>();
            game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
            Require(Enumerable.Range(0, game.PlayerCount).All(seat => SnapshotJson.Serialize(game.CreateSnapshot(seat)) ==
                SnapshotJson.Serialize(before.CreateSnapshot(seat))) && game.CardMovements.SequenceEqual(before.CardMovements) &&
                game.ResolutionStack.Select(frame => JsonSerializer.Serialize(frame, frame.GetType())).SequenceEqual(
                    before.ResolutionStack.Select(frame => JsonSerializer.Serialize(frame, frame.GetType()))) &&
                Facts<SimaHuiSealMovedEvent>().SequenceEqual(seals),
                "Cold replay preserves the original turn prelude, seal grants, and public decisions.");
        }

        Accept(game, new StartGameCommand());
        var dragonMenu = Next();
        while (dragonMenu.Kind == DecisionKind.SelectGeneral)
        {
            Accept(game, new SelectGeneralCommand(dragonMenu.PlayerSeat, Generals[dragonMenu.PlayerSeat], game.Revision, dragonMenu.PromptId));
            dragonMenu = Next();
        }
        Require(dragonMenu is { PlayerSeat: 0, SkillPrompt.SkillId: Jianjie } &&
            dragonMenu.ValidTargetSeats.SequenceEqual([1, 2, 3]) && Facts<TurnStartedEvent>().Length == 0,
            "The mandatory Dragon choice precedes the first actual TurnStarted event.");
        var root = Root();
        var window = game.ResolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().Single(frame =>
            frame.Window == SkillProgramTriggerWindow.TurnBeforeStart);
        var prelude = game.ResolutionStack.Single(frame => frame.UpcomingTurnPrelude is { OwnerSeat: 0, TurnNumber: 1 });
        Require(root is { OwnerSeat: 0, TriggerId: "first-actual-turn", SimaHui: { Stage: "initial-dragon", ActualTurn: 1 } } &&
            root.WindowContext is { BeforeTurnPreludeProof: { } proof } context && context.ParentFrameId == window.Id &&
            proof == window.BeforeTurnPreludeProof && proof.PreludeFrameId == prelude.Id,
            "Jianjie owns the original native first-turn prelude and its pending Dragon instruction.");
        Choose(1);

        var phoenixMenu = Next();
        Require(phoenixMenu is { PlayerSeat: 0, SkillPrompt.SkillId: Jianjie } && phoenixMenu.ValidTargetSeats.SequenceEqual([2, 3]) &&
            Root().Id == root.Id && Root().SimaHui is { Stage: "initial-phoenix", SelectedSeats: [1] } &&
            Facts<TurnStartedEvent>().Length == 0 && Holds(1, PlayerMarkerKind.DragonSeal, Huoji) &&
            Facts<SimaHuiSealMovedEvent>() is [{ Marker: PlayerMarkerKind.DragonSeal, Holder: 1 }],
            "Paying Dragon grants Huoji once and keeps the distinct Phoenix choice on the same pending turn.");
        Cold();
        Choose(2);

        var play = Next();
        var firstStart = game.Events.Single(entry => entry.Payload is TurnStartedEvent { ActorSeat: 0, TurnNumber: 1 });
        var grants = game.Events.Where(entry => entry.Payload is SimaHuiSealMovedEvent).ToArray();
        Require(play is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && grants.Length == 2 &&
            grants.All(entry => entry.Sequence < firstStart.Sequence && entry.Payload is SimaHuiSealMovedEvent {
                PreviousHolder: null, ActualTurn: 1 } grant && grant.FrameId == root.Id &&
                grant.Issuer is { SkillId: Jianjie, OwnerSeat: 0 } && grant.Issuer.SkillInstanceId == root.SkillInstanceId) &&
            grants[1].Payload is SimaHuiSealMovedEvent { Marker: PlayerMarkerKind.PhoenixSeal, Holder: 2 } &&
            Facts<SimaHuiSealMovedEvent>().Select(grant => grant.GrantSourceId).Distinct().Count() == 2 &&
            Holds(1, PlayerMarkerKind.DragonSeal, Huoji) && Holds(2, PlayerMarkerKind.PhoenixSeal, Lianhuan) &&
            !game.GetHumanLegalActions().Any(action => action.ProgramSkillId == Jianjie),
            "Both distinct seals grant their real skills before the owner enters the first actual Play phase.");
        Cold();

        for (var offset = 1; offset <= game.PlayerCount; offset++)
        {
            Accept(game, new EndPlayPhaseCommand(play.PlayerSeat, game.Revision, play.PromptId));
            play = Next();
            Require(play.Kind == DecisionKind.PlayCard && play.PlayerSeat == offset % game.PlayerCount,
                $"Expected ordinary Play for seat {offset % game.PlayerCount}, received {play.Kind}/{play.SkillPrompt?.SkillId} for seat {play.PlayerSeat}.");
        }
        Require(Facts<TurnStartedEvent>().Select(turn => turn.ActorSeat).SequenceEqual([0, 1, 2, 3, 0]) &&
            Facts<TurnEndedEvent>().Any(turn => turn.ActorSeat == 0 && turn.TurnNumber == 1) &&
            Facts<ProgramBindingStartedEvent>().Count(binding => binding is { SkillId: Jianjie, BindingId: "first-actual-turn" }) == 1 &&
            Facts<SimaHuiSealMovedEvent>().SequenceEqual(grants.Select(entry => (SimaHuiSealMovedEvent)entry.Payload)) &&
            Holds(1, PlayerMarkerKind.DragonSeal, Huoji) && Holds(2, PlayerMarkerKind.PhoenixSeal, Lianhuan) &&
            game.GetHumanLegalActions().Any(action => action.ProgramSkillId == Jianjie && action.ProgramActivationId == "transfer"),
            "The next actual owner turn keeps both grants and offers only ordinary Jianjie transfer.");
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-jianjie-first-turn", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            foreach (var general in Generals.Skip(1))
                builder.AddGeneral(new(general, "固定同桌", "supporter", "standard:none", "wei", 4));
            builder.AddDeck(new(Deck, "荐杰固定牌堆", 0, 1, [])
            {
                PhysicalCards = Enumerable.Range(0, 24).Select(_ => new ContentDeckPhysicalCard("standard:peach", Suit.Heart, 3)).ToArray()
            });
            builder.AddMode(new(Mode, "荐杰首次实际回合", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}
