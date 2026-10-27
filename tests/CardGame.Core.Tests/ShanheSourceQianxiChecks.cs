using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSourceQianxiChecks
{
    private const string Skill = StandardShanheSourceDemonAssassinPackage.QianxiSkillId;
    private const string Deck = "fixture:source-qianxi-deck";

    public static void RealHorseDistanceJudgmentRestrictsTheChosenTargetsBlackHand()
    {
        var opponents = Enumerable.Range(1, 3).Select(_ => new ShanheCombatantDefinition("classic:guan-yu")
        {
            StartingHand = [new(CardKind.Dodge, Suit.Spade, 2), new(CardKind.Dodge, Suit.Spade, 3)]
        }).ToArray();
        var setup = new ShanheBattleSetup(83, 1, "source-qianxi-native", StandardShanheSourceDemonAssassinPackage.BaseId,
            3, 0, 0, 0, 0, [], [new(CardKind.Slash, Suit.Heart, 7)], [new(CardKind.OffensiveHorse, Suit.Club, 5)],
            opponents, [], new string('e', 64));
        var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup).WithExtension(new Fixture(), mode =>
            mode.Id == ShanheBattleSetup.BattleModeId ? mode with { DeckId = Deck } : mode);
        var game = GameEngine.CreateStandard(new()
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 2
        }, registry);
        Submit(game, new StartGameCommand());
        Until(game, () => P(game)?.SkillPrompt?.SkillId == Skill &&
            P(game)!.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
        Need(game.GetSeatDistance(0, 2) == 2 && game.GetCombatDistance(0, 2) == 1 &&
            game.CreateSnapshot(0).Players[0].Equipment.Single().Kind == CardKind.OffensiveHorse,
            "One real installed offensive horse makes seat2 combat-distance one while its raw seat distance remains two.");
        Answer(game, c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        Until(game, () => P(game)?.SkillPrompt?.SkillId == Skill && P(game)!.Choices.Any(c => c.Targets.SequenceEqual([2])));
        var original = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Skill);
        var judgment = Events<JudgmentResolvedEvent>(game).Single(e => e.Reason == "wushuang-qianxi");
        var outcome = original.JudgmentOutcomeBindings!.Single();
        // Succeeded is the native reason-specific verdict, not proof that a
        // ProgramSkill judgment produced a real finalized card and color.
        Need(judgment is { Suit: Suit.Spade, CardId: > 0, CardKind: CardKind.Dodge, Rank: 2 } &&
            judgment.ParentResolutionId == original.Id && judgment.TargetSeat == 0 &&
            outcome is { Name: "qianxi-outcome", ResultBind: "qianxi-judge", InstructionIndex: 1, TargetSeat: 0 } &&
            outcome.JudgmentFrameId == judgment.ResolutionId && outcome.Reason == judgment.Reason &&
            outcome.CardId == judgment.CardId && outcome.Suit == judgment.Suit && outcome.Rank == judgment.Rank &&
            Events<ProgramJudgmentOutcomeRecordedEvent>(game).Count(e => e.ProgramFrameId == original.Id && e.Outcome == outcome) == 1,
            "The original source-compatible Qianxi uses its real finalized public judgment before choosing the actual combat-distance-one target.");
        Answer(game, c => c.Targets.SequenceEqual([2]));
        Until(game, () => P(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } && game.ResolutionStack.Count == 0);
        var restriction = Events<HandCardColorRestrictionGrantedEvent>(game).Single().Restriction;
        Need(restriction.Source.SkillId == Skill && restriction.Source.OwnerSeat == 0 && restriction.ParentFrameId == original.Id &&
            restriction.AffectedSeat == 2 && !restriction.IsRed && restriction.TurnNumber == game.State.TurnNumber && restriction.TurnSeat == 0 &&
            Events<ProgramBindingResolvedEvent>(game).Count(e => e.FrameId == original.Id && e.Completed) == 1 &&
            game.CreateCardZoneDiagnostics().Single(c => c.CardId == judgment.CardId).Location == CardLocation.DiscardPile &&
            game.CardMovements.Count(m => m.CardId == judgment.CardId && m.From == CardLocation.DrawPile && m.To == CardLocation.Judgment(0)) == 1 &&
            game.CardMovements.Count(m => m.CardId == judgment.CardId && m.To == CardLocation.DiscardPile) == 1,
            "The original program completes once, cleans its one judgment entity and grants the black-hand restriction to the legal distance-two seat without a raw-distance exception.");

        var targetHand = game.CreateSnapshot(2).Players[2].Hand.Select(c => c.Id).ToArray();
        var slash = game.GetHumanLegalActions().Single(a => a.Kind == LegalActionKind.Slash &&
            a.ConversionSource is null && a.TargetSeats.SequenceEqual([2]));
        Submit(game, new PlayCardCommand(0, slash.CardId!.Value, slash.TargetSeats, game.Revision, P(game)!.PromptId, slash.PlayedCardKind));
        Until(game, () => P(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } && game.ResolutionStack.Count == 0);
        var use = Events<CardUseDeclaredEvent>(game).Single(e => e.CardId == slash.CardId);
        Need(targetHand.Length == 2 && game.CreateSnapshot(2).Players[2].Hand.Select(c => c.Id).SequenceEqual(targetHand) &&
            Events<DamageAppliedEvent>(game) is [{ SourceSeat: 0, TargetSeat: 2, Amount: 1 }] &&
            !Events<CardResponseCompletedEvent>(game).Any(e => e.ParentFrameId == use.ResolutionId && e.ActorSeat == 2) &&
            Events<CardUseFinishedEvent>(game).Count(e => e.ResolutionId == use.ResolutionId) == 1 &&
            targetHand.All(id => game.CardMovements.All(m => m.CardId != id || m.From != CardLocation.Hand(2))),
            "The same-turn native Slash deals real damage because the target's two black hand Dodges are sealed; neither response entity is paid or moved.");
    }

    private static T[] Events<T>(GameEngine game) => game.Events.Select(e => e.Payload).OfType<T>().ToArray();
    private static PendingDecision? P(GameEngine game) => Enumerable.Range(0, 4)
        .Select(seat => game.CreateSnapshot(seat).PendingDecision).FirstOrDefault(p => p is not null);
    private static void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message); }
    private static void Submit(GameEngine game, GameCommand command)
    { var result = game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native judgment-Qianxi command."); }
    private static void Answer(GameEngine game, Func<PromptChoice, bool> choose)
    {
        var prompt = P(game)!; Need(prompt.PlayerSeat == 0, "Only the true human submits an external Qianxi choice.");
        Submit(game, new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices.Single(choose).Id, game.Revision));
    }
    private static void Until(GameEngine game, Func<bool> done)
    {
        for (var step = 0; step < 160; step++)
        {
            if (done()) return;
            var prompt = P(game);
            Need(prompt is null || !game.CreateSnapshot(prompt.PlayerSeat).Players[prompt.PlayerSeat].IsHuman,
                "Unexpected native judgment-Qianxi boundary: " + JsonSerializer.Serialize(prompt));
            Submit(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The bounded native judgment-Qianxi boundary was not reached.");
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-source-qianxi", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder) => builder.AddDeck(new(Deck,
            "零随机起手和摸牌的黑色判定牌堆", 0, 0, [])
        { PhysicalCards = Enumerable.Range(0, 32).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, 2)).ToArray() });
    }
}
