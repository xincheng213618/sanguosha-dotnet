using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class OlHuoJunQiongshouDeclaredHandLimitChecks
{
    private const string HuoJun = "ol:huo-jun";
    private const string DataOnlyGeneral = "fixture:declared-hand-limit-owner";
    private const string DataOnlySkill = "fixture:declared-hand-limit-two";
    private const string Peer = "fixture:declared-hand-limit-peer";
    private const string Deck = "fixture:declared-hand-limit-deck";

    internal static void DeclaredAdditionsSetActualDiscardQuotaCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());

        VerifyDiscard(registry, HuoJun, startingCards: 4, expectedLimit: 8);
        VerifyDiscard(registry, DataOnlyGeneral, startingCards: 8, expectedLimit: 6);
    }

    private static void VerifyDiscard(ContentRegistry registry, string general, int startingCards, int expectedLimit)
    {
        var startingHand = Enumerable.Repeat(new ShanheStartingCard(CardKind.Dodge, Suit.Club, 2), startingCards).ToArray();
        var setup = new ShanheBattleSetup(31, 1, "declared-hand-limit", general, 4, 0, 0, 0, 0, [], startingHand, [],
            [new(Peer, Hp: 6) { StartingHand = [] }], [], new string('b', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = true, AdvanceAfterHumanCommands = false, MaxTurns = 4
        }, registry);

        Submit(game, new StartGameCommand());
        var play = ReachPrompt(game, DecisionKind.PlayCard);
        var turn = game.State.TurnNumber;
        var owner = game.CreateSnapshot(0).Players[0];
        var hand = owner.Hand.Select(card => card.Id).ToHashSet();
        Require(owner is { Hp: 4, HandCount: 10 } && hand.Count == 10 &&
            game.CardMovements.Count(move => move.TurnNumber == turn && move.From == CardLocation.DrawPile &&
                move.To == CardLocation.Hand(0) && move.Reason == CardMoveReasons.Draw) == 2,
            "Each owner reaches Play with ten distinct physical cards after the same two-card actual Draw.");

        Submit(game, new EndPlayPhaseCommand(0, game.Revision, play.PromptId));
        var discard = ReachPrompt(game, DecisionKind.DiscardCards);
        var frame = game.ResolutionStack.OfType<ManualDiscardFrame>().Single();
        var required = hand.Count - expectedLimit;
        Require(game.State.Phase == TurnPhase.Discard && frame.OwnerSeat == 0 && frame.ActualTurnNumber == turn &&
            frame.InitialRequiredCount == required && discard.RequiredCardCount == required && hand.SetEquals(discard.ValidCardIds),
            $"The original Discard for {general} must ask for {required} of its ten physical cards.");

        var before = game;
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(before.CreateCheckpoint())), registry);
        Require(SnapshotJson.Serialize(game.CreateSnapshot(0)) == SnapshotJson.Serialize(before.CreateSnapshot(0)) &&
            JsonSerializer.Serialize(game.ResolutionStack.OfType<ManualDiscardFrame>().Single()) == JsonSerializer.Serialize(frame) &&
            game.CardMovements.SequenceEqual(before.CardMovements),
            "Cold restore preserves the exact unpaid native discard with its original prompt and hand.");

        var restored = game.CreateSnapshot(0).PendingDecision!;
        var chosen = restored.ValidCardIds.Take(required).ToArray();
        Submit(game, new DiscardCardsCommand(0, chosen, restored.PromptId, game.Revision));
        var moved = game.CardMovements.Where(move => move.TurnNumber == turn && move.Reason == CardMoveReasons.HandLimitDiscard &&
            move.From == CardLocation.Hand(0) && move.To == CardLocation.DiscardPile).Select(move => move.CardId).ToArray();
        var kept = game.CreateSnapshot(0).Players[0].Hand;
        Require(moved.Length == required && chosen.ToHashSet().SetEquals(moved) && kept.Count == expectedLimit &&
            hand.Except(chosen).ToHashSet().SetEquals(kept.Select(card => card.Id)),
            $"The accepted native discard for {general} must move precisely its chosen cards and keep the original {expectedLimit} others.");
    }

    private static PendingDecision ReachPrompt(GameEngine game, DecisionKind kind)
    {
        for (var step = 0; step < 70; step++)
        {
            var prompt = game.CreateSnapshot(0).PendingDecision;
            if (prompt is { PlayerSeat: 0 } && prompt.Kind == kind)
                return prompt;

            Require(prompt is null && game.State.Winner == Winner.None, $"Unexpected boundary while advancing to {kind}: {prompt?.Kind}.");
            Submit(game, new AdvanceOneStepCommand(game.Revision));
        }

        throw new InvalidOperationException($"The declared hand-limit fixture did not reach {kind}.");
    }

    private static void Submit(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        Require(result.Accepted, result.Error?.Message ?? "The declared hand-limit fixture command failed.");
    }

    private static void Require(bool condition, string message)
    {
        if (!condition)
            throw new InvalidOperationException(message);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:declared-hand-limit", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, HuoJun);
            var rules = $$"""
                {"skills":[{"id":"{{DataOnlySkill}}","revision":1,"modifiers":[
                    {"id":"add-two","query":"handLimit","operation":"add","value":2,"priority":0}
                ]}]}
                """;
            var labels = new Dictionary<string, object>
            {
                [DataOnlySkill] = new { name = "手牌上限加二", description = "你的手牌上限加二。" }
            };
            var programs = SkillProgramCatalog.LoadCurrent(rules,
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            builder.AddSkill(new(DataOnlySkill, programs.Presentations[DataOnlySkill].Name, programs.Presentations[DataOnlySkill].Description)
                { Program = programs.Programs[DataOnlySkill], ProgramPresentation = programs.Presentations[DataOnlySkill] });
            builder.AddGeneral(new(DataOnlyGeneral, "声明手限者", "supporter", DataOnlySkill, "shu", 4));
            builder.AddGeneral(new(Peer, "同桌", "supporter", "standard:none", "wei", 6));
            builder.AddDeck(new(Deck, "固定手限检查", 0, 2, [new("standard:dodge", 28)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "声明数值与真实弃牌", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [HuoJun, DataOnlyGeneral, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
