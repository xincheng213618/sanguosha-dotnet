using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class MobileBoundarySidiShensuChecks
{
    private const string CaoZhen = "boundary:cao-zhen-mobile", XiahouYuan = "classic:xiahou-yuan";
    private const string Sidi = "boundary:sidi-mobile", Shensu = "classic:shensu";
    private const string Mode = "identity:classic-mobile-sidi-shensu", Deck = "fixture:mobile-sidi-shensu-deck";
    private static readonly string[] Generals = [CaoZhen, XiahouYuan, "fixture:sidi-shensu-peer-2", "fixture:sidi-shensu-peer-3"];

    internal static void SidiConsumesFirstActualShensuSlashCold()
    {
        var (game, content) = Start();
        var drawTwo = game.GetLegalActions(0).First(action => action.Kind == LegalActionKind.DrawTwo);
        Play(game, 0, drawTwo);
        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Sidi });
        Activate(game);
        Answer(game, choice => HasBranch(choice, "holder", 1));
        Answer(game, choice => HasBranch(choice, "target", 0));
        Settle(game);
        Accept(game, new EndPlayPhaseCommand(0, game.Revision, P(game)!.PromptId));
        Reach(game, prompt => prompt is { PlayerSeat: 1, SkillPrompt.SkillId: Shensu });
        Require(Facts<CardUseFinishedEvent>(game).Any(entry => entry.CardId == drawTwo.CardId && entry.CardKind == CardKind.DrawTwo) &&
            Facts<BoundarySidiForecastRecordedEvent>(game).Single().OwnerSeat == 0 &&
            game.CreateSnapshot(0).BoundarySidiPredictions is [{ HolderSeat: 1, TargetSeat: 0 }] &&
            Enumerable.Range(1, 3).All(seat => game.CreateSnapshot(seat).BoundarySidiPredictions is null),
            "A real completed Draw Two creates one forecast with its holder and target visible only to Cao Zhen.");

        var control = Cold(game, content);
        Skip(control);
        Reach(control, IsXiahouYuanPlayOrShensu);
        if (P(control)!.SkillPrompt?.SkillId == Shensu)
        {
            Skip(control);
            Reach(control, prompt => prompt is { PlayerSeat: 1, Kind: DecisionKind.PlayCard });
        }
        var physical = control.GetLegalActions(1).First(action => action.Kind == LegalActionKind.Slash && action.TargetSeats.SequenceEqual([0]));
        Play(control, 1, physical);
        Reach(control, prompt => prompt is { PlayerSeat: 1, Kind: DecisionKind.PlayCard });
        AssertSlashAndForecast(control, game, physical.CardId!.Value, "physical");

        var restored = Cold(game, content);
        Activate(restored);
        Answer(restored, choice => choice.Targets.SequenceEqual([0]));
        Reach(restored, prompt => prompt is { PlayerSeat: 1, Kind: DecisionKind.PlayCard });
        AssertSlashAndForecast(restored, game, 0, "Shensu");
    }

    private static void AssertSlashAndForecast(GameEngine settled, GameEngine beforeUse, int cardId, string label)
    {
        var use = Facts<CardUseDeclaredEvent>(settled).Single(entry => entry.SourceSeat == 1);
        Require(use is { CardKind: CardKind.Slash } && use.CardId == cardId &&
            Facts<TargetsConfirmedEvent>(settled).Any(entry => entry.ResolutionId == use.ResolutionId && entry.TargetSeats.SequenceEqual([0])) &&
            Facts<DamageAppliedEvent>(settled).Single() is { SourceSeat: 1, TargetSeat: 0, Amount: 1 } hit && hit.SourceCardUseResolutionId == use.ResolutionId &&
            settled.CreateSnapshot(0).Players[0].Hp == beforeUse.CreateSnapshot(0).Players[0].Hp - 1,
            $"The {label} Slash is the first actual choice, targeting Cao Zhen once and inflicting native damage under that same use.");

        var uses = Facts<ProgramBindingStartedEvent>(settled).Count(entry =>
            entry is { OwnerSeat: 0, SkillId: Sidi, BindingId: "consume-first-use" });
        var gains = settled.CardMovements.Skip(beforeUse.CardMovements.Count).Count(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0));
        Require(uses == 1 && gains == 1 && settled.CreateSnapshot(0).BoundarySidiPredictions is null,
            $"The {label} first actual Slash consumes Sidi's forecast and gives Cao Zhen one real draw. Binding starts: {uses}, draws: {gains}.");
    }

    private static bool HasBranch(PromptChoice choice, string branch, int seat) =>
        choice.Parameters.GetValueOrDefault("branch") == branch && choice.Targets.SequenceEqual([seat]);
    private static bool IsXiahouYuanPlayOrShensu(PendingDecision prompt) => prompt.PlayerSeat == 1 &&
        (prompt.Kind == DecisionKind.PlayCard || prompt.SkillPrompt?.SkillId == Shensu);
    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();

    private static void Play(GameEngine game, int actor, LegalAction action) =>
        Accept(game, new PlayCardCommand(actor, action.CardId!.Value, action.TargetSeats, game.Revision, P(game)!.PromptId, action.PlayedCardKind));

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) && Enumerable.Range(0, game.PlayerCount).All(seat =>
            SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold replay preserves the original Shensu opportunity and the correctly private Sidi forecast.");
        return restored;
    }

    private static (GameEngine Game, ContentRegistry Content) Start()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 47, PlayerCount = 4, ModeId = Mode,
            HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, UseInteractiveSetup = true,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        Accept(game, new StartGameCommand());
        Reach(game, prompt => prompt.Kind is DecisionKind.SelectGeneral or DecisionKind.PlayCard);
        while (P(game) is { Kind: DecisionKind.SelectGeneral } selection)
        {
            Accept(game, new SelectGeneralCommand(selection.PlayerSeat, Generals[selection.PlayerSeat], game.Revision, selection.PromptId));
            Reach(game, prompt => prompt.Kind is DecisionKind.SelectGeneral or DecisionKind.PlayCard);
        }
        return (game, content);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-sidi-shensu", new(1, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, CaoZhen, XiahouYuan);
            foreach (var peer in Generals.Skip(2))
                builder.AddGeneral(new(peer, "同桌", "supporter", "standard:none", "wei", 4));
            builder.AddDeck(new(Deck, "固定杀与无中生有", 4, 0, [])
            {
                PhysicalCards = Enumerable.Range(0, 40).Select(index => index % 2 == 0 ?
                    new ContentDeckPhysicalCard("standard:slash", Suit.Spade, 7) : new ContentDeckPhysicalCard("standard:draw_two", Suit.Heart, 5)).ToArray()
            });
            builder.AddMode(new(Mode, "司敌真实首次用牌", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}
