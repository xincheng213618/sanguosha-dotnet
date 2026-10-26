using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class WuYiEnhancementScenario
{
    internal static (GameEngine Game, ContentRegistry Registry) Start()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(31, 1, "benxi-confirm", "classic:wu-yi", 4, 0, 0, 0, 0, [],
            [new(CardKind.Slash, Suit.Heart, 6)], [],
            [new("fixture:benxi-peer", Hp: 6) { StartingHand = [] }, new("fixture:benxi-peer", Hp: 6) { StartingHand = [] }],
            [new("fixture:benxi-peer", Hp: 6) { StartingHand = [] }], new string('b', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, AdvanceAfterHumanCommands = false, UseInteractiveDiscard = false, MaxTurns = 3
        }, registry);
        Accept(game, new StartGameCommand());
        Reach(game, prompt => prompt.Kind == DecisionKind.PlayCard);
        var slash = game.GetHumanLegalActions().Single(action => action.Kind == LegalActionKind.Slash && action.TargetSeats.SequenceEqual([3]));
        Accept(game, new PlayCardCommand(0, slash.CardId!.Value, [3], game.Revision, game.PendingDecision!.PromptId));
        Reach(game, prompt => prompt.SkillPrompt?.SkillId == "classic:benxi" &&
            prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("enhancement-option") == "confirm"));
        return (game, registry);
    }

    internal static PromptChoice Selection(GameEngine game, CurrentCardEnhancement flags) => game.PendingDecision!.Choices.Single(choice =>
        choice.Parameters.GetValueOrDefault("enhancement-selection") == ((int)flags).ToString(System.Globalization.CultureInfo.InvariantCulture));
    internal static void Answer(GameEngine game, PromptChoice choice) => Accept(game,
        new AnswerPromptCommand(0, game.PendingDecision!.PromptId, choice.Id, game.Revision));
    internal static void Reach(GameEngine game, Func<PendingDecision, bool> goal)
    {
        for (var step = 0; step < 80; step++)
        {
            if (game.PendingDecision is { PlayerSeat: 0 } prompt && goal(prompt)) return;
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("Benxi fixture did not reach its next native decision.");
    }
    internal static void Accept(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        if (!result.Accepted) throw new InvalidOperationException(result.Error?.Message);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:benxi-confirm", new(1, 0, 0), []);
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "classic:wu-yi");
            builder.AddGeneral(new("fixture:benxi-peer", "静默同桌", "supporter", "standard:none", "wei", 6));
            builder.AddDeck(new("fixture:benxi-deck", "固定牌堆", 0, 0, [])
            { PhysicalCards = Enumerable.Range(0, 32).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Diamond, 2)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "奔袭确认", 4, 4, new Dictionary<string, int>(), "fixture:benxi-deck",
                GeneralPoolIds: ["classic:wu-yi", "fixture:benxi-peer"], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
