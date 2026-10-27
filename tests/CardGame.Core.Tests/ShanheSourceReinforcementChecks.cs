using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSourceReinforcementChecks
{
    private const string Deck = "fixture:source-qiangwang-quiet-deck";

    public static void InitialDongdangUsesItsEffectiveOwnerBeforeTheFirstNaturalTurn()
    {
        foreach (var (difficulty, effective) in new[] { (1, true), (16, true), (16, false) })
        {
            var source = StandardShanheCampaignCatalog.CreateSourceCodexDefinition(difficulty);
            var node = source.Nodes.Single(candidate => candidate.Id == "source-city-5108");
            var encounter = node.Encounters!.Single(candidate => candidate.SourceBattleId == 2669);
            var setup = new ShanheBattleSetup(71, 1, node.Id, "classic:guan-yu", 34, 30, 0, 0, 0,
                [], [], [], encounter.Enemies, [], ShanheCampaignContent.GetContentHash(source)) { SourceDifficultyId = difficulty };
            var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup).WithExtension(new QuietDeck(),
                mode => mode.Id == ShanheBattleSetup.BattleModeId ? mode with { DeckId = Deck } : mode);
            if (!effective) setup = setup with { EnemyRoster = [setup.EnemyRoster.Single() with { GeneralId = "classic:guan-yu" }] };
            var game = GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false,
                UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 6
            }, registry);
            Submit(game, new StartGameCommand());
            for (var step = 0; !(game.CreateSnapshot(0).PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } &&
                     game.ResolutionStack.Count == 0); step++)
            {
                Need(step < 300 && game.State.Winner == Winner.None, "The initial-entry fixture must reach the next human Play without a completed match.");
                Need(game.CreateSnapshot(0).PendingDecision is null, "The all-Dodge fixture must not invent an unrelated human choice.");
                Submit(game, new AdvanceOneStepCommand(game.Revision));
            }
            var entries = Facts<ShanheSourceDongdangInitialEntryEvent>(game);
            Need(Facts<TurnStartedEvent>(game).First().ActorSeat == (effective ? 1 : 0) &&
                entries.Length == (effective ? 1 : 0) &&
                (!effective || entries is [{ OwnerSeat: 1, GeneralId: StandardShanheSourceQiangWangPackage.GeneralId,
                    PreviousStartingSeat: 0, Source: { OwnerSeat: 1, SkillId: ShanheSourceDongdangRules.SkillId,
                        BindingId: ShanheSourceDongdangRules.InitialBindingId } }] && !string.IsNullOrWhiteSpace(entries[0].Source.SkillInstanceId)) &&
                Facts<TurnEndedEvent>(game).All(fact => fact.TurnNumber > 0) &&
                Facts<ShanheBossStageStartedEvent>(game).Length == 0 && Facts<ShanheBossTurnInterruptedEvent>(game).Length == 0,
                "Only the effective actual initial owner starts first; initial entry does not invent a retired turn zero or a second-form transformation.");
            Need(game.CardMovements.Count(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(1) &&
                    move.Reason == CardMoveReasons.InitialDeal) == 4 &&
                (setup.EnemyRoster.Single().StartingCards ?? []).All(card => game.CardMovements.Count(move =>
                    move.From == CardLocation.OutsideGame && move.To == CardLocation.Hand(1) &&
                    move.CardKind == card.Kind) == 1),
                "Initial Dongdang retains the four real random opening entities and each configured extra equipment hand entity exactly once.");
        }
    }

    private static T[] Facts<T>(GameEngine game) => game.Events.Select(item => item.Payload).OfType<T>().ToArray();
    private static void Submit(GameEngine game, GameCommand command)
    { var result = game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected source-entry command."); }
    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
    private sealed class QuietDeck : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:source-qiangwang-quiet", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder) => builder.AddDeck(new(Deck, "真实闪牌堆", 4, 2, [new("standard:dodge", 80)]));
    }
}
