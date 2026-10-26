using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheWeimuRuleChecks
{
    private const string Weimu = "classic:weimu";
    private const string Owner = "fixture:shanhe-weimu-owner", Other = "fixture:shanhe-weimu-other";
    private const string Blank = "fixture:shanhe-weimu-blank", Deck = "fixture:shanhe-weimu-deck";

    public static void AcquiredWeimuCoversBlackSelfTricksWithoutBlockingRedCardsOrOtherTargets()
    {
        var scene = Scene.Start();
        var view = scene.Game.CreateSnapshot(0).Players[0];
        Need(view.GeneralId == Owner && view.Skills!.Any(skill => skill.ContentId == Weimu) &&
            scene.Game.CreateSnapshot(0).Players[1].Skills!.Any(skill => skill.ContentId == Weimu),
            "Unrelated generals gain the production classic Weimu as real Shanhe extra skills.");
        var hand = view.Hand;
        var blackDraw = hand.Single(card => card.Kind == CardKind.DrawTwo && card.Suit == Suit.Club);
        var blackLightning = hand.Single(card => card.Kind == CardKind.Lightning && card.Suit == Suit.Spade);
        var blackDuel = hand.Single(card => card.Kind == CardKind.Duel && card.Suit == Suit.Spade);
        var redDraw = hand.Single(card => card.Kind == CardKind.DrawTwo && card.Suit == Suit.Heart);
        var redLightning = hand.Single(card => card.Kind == CardKind.Lightning && card.Suit == Suit.Diamond);
        var redDuel = hand.Single(card => card.Kind == CardKind.Duel && card.Suit == Suit.Heart);
        var slash = hand.Single(card => card.Kind == CardKind.Slash);
        var legal = scene.Game.GetHumanLegalActions();
        Need(!legal.Any(action => action.CardId == blackDraw.Id) && !legal.Any(action => action.CardId == blackLightning.Id),
            "Classic Weimu must prohibit actual black DrawTwo and Lightning self-targets, including kinds omitted by the old per-suit lists.");
        Need(!legal.Any(action => action.CardId == blackDuel.Id && action.TargetSeats.Contains(1)) &&
            legal.Any(action => action.CardId == blackDuel.Id && action.TargetSeats.SequenceEqual([2])) &&
            legal.Any(action => action.CardId == redDuel.Id && action.TargetSeats.SequenceEqual([1])) &&
            legal.Any(action => action.CardId == redLightning.Id) &&
            legal.Any(action => action.CardId == slash.Id && action.TargetSeats.SequenceEqual([1])),
            "Black opponent tricks exclude only their protected target; red tricks and black basic Slash retain native legality.");
        foreach (var forbidden in new[] { blackDraw.Id, blackLightning.Id })
        {
            var result = scene.Game.Submit(new PlayCardCommand(0, forbidden,
                forbidden == blackLightning.Id ? [0] : [], scene.Game.Revision, scene.Prompt!.PromptId));
            Need(!result.Accepted && scene.Game.CreateSnapshot(0).Players[0].Hand.Any(card => card.Id == forbidden) &&
                !scene.Game.CardMovements.Any(move => move.CardId == forbidden && move.From == CardLocation.Hand(0)),
                "A forbidden black self-targeting trick is rejected before its real physical cost leaves hand.");
        }

        scene.Play(redDraw.Id);
        Need(scene.Game.CreateSnapshot(0).Players[0].Hand.Count(card => card.Kind == CardKind.Dodge) == 2 &&
            scene.Events<CardUseFinishedEvent>().Count(item => item.CardId == redDraw.Id) == 1 &&
            scene.Game.CardMovements.Count(move => move.CardId == redDraw.Id && move.From == CardLocation.Hand(0) && move.To == CardLocation.Processing) == 1 &&
            scene.Game.CardMovements.Count(move => move.CardId == redDraw.Id && move.To == CardLocation.DiscardPile) == 1,
            "The same acquired policy permits a real red DrawTwo, paying its original entity once and drawing two actual cards.");
        scene.Play(redDuel.Id, [1]);
        Need(scene.Events<DamageAppliedEvent>() is [{ SourceSeat: 0, TargetSeat: 1, Amount: 1 }] &&
            scene.Events<CardUseFinishedEvent>().Count(item => item.CardId == redDuel.Id) == 1,
            "A red Duel truly resolves on the other Weimu owner through the normal response and native damage flow.");
        scene.Play(redLightning.Id);
        Need(scene.Game.CreateSnapshot(0).Players[0].Judgment.Any(card => card.Id == redLightning.Id) &&
            scene.Events<CardUseFinishedEvent>().Count(item => item.CardId == redLightning.Id) == 1 &&
            new[] { blackDraw.Id, blackLightning.Id }.All(id => scene.Game.CreateSnapshot(0).Players[0].Hand.Any(card => card.Id == id)),
            "A real red Lightning enters the acquired owner's judgment area while both rejected black self-tricks remain unpaid in hand.");
    }

    private static void Need(bool value, string message)
    { if (!value) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine Game { get; } = game;
        public PendingDecision? Prompt => Game.PendingDecision;
        public T[] Events<T>() => Game.Events.Select(item => item.Payload).OfType<T>().ToArray();
        private void Accept(GameCommand command)
        { var result = Game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native Weimu command."); }
        private void Finish()
        {
            for (var step = 0; step < 100; step++)
            {
                if (Prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }) return;
                Need(Game.State.Winner == Winner.None, "The small Weimu scene ended before its actual Play boundary.");
                if (Prompt is { PlayerSeat: 0, Choices.Count: > 0 } prompt)
                {
                    var choice = prompt.Choices.FirstOrDefault(item => item.Parameters.GetValueOrDefault("response") is
                        "pass" or "no-nullification" or "take-damage" or "take-duel-damage") ?? prompt.Choices[0];
                    Accept(new AnswerPromptCommand(0, prompt.PromptId, choice.Id, Game.Revision));
                }
                else Accept(new AdvanceOneStepCommand(Game.Revision));
            }
            throw new InvalidOperationException("Missing native Weimu boundary: " + JsonSerializer.Serialize(Prompt));
        }
        public void Play(int cardId, IReadOnlyList<int>? targets = null)
        {
            var action = Game.GetHumanLegalActions().First(item => item.CardId == cardId &&
                (targets is null || item.TargetSeats.SequenceEqual(targets)));
            Accept(new PlayCardCommand(0, cardId, action.TargetSeats, Game.Revision, Prompt!.PromptId,
                action.PlayedCardKind, action.TargetCardId) { ConversionSource = action.ConversionSource });
            Finish();
        }
        public static Scene Start()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var setup = new ShanheBattleSetup(83, 1, "fixture:shanhe-weimu", Owner, 6, 0, 0, 0, 0, [Weimu],
                [new(CardKind.DrawTwo, Suit.Club, 7), new(CardKind.Lightning, Suit.Spade, 1), new(CardKind.Duel, Suit.Spade, 8),
                 new(CardKind.DrawTwo, Suit.Heart, 7), new(CardKind.Lightning, Suit.Diamond, 1), new(CardKind.Duel, Suit.Heart, 8),
                 new(CardKind.Slash, Suit.Club, 9)], [],
                [new(Other, ExtraSkillIds: [Weimu]) { StartingHand = [] }, new(Other) { StartingHand = [] }], [], new string('f', 64));
            var scene = new Scene(GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 3, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 2, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, registry));
            scene.Accept(new StartGameCommand()); scene.Finish(); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shanhe-weimu", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "classic:jia-xu");
            builder.AddSkill(new(Blank, "独立武将", "仅验证真实额外技能"));
            builder.AddGeneral(new(Owner, "非贾诩的帷幕持有者", "supporter", Blank, "shu", 6));
            builder.AddGeneral(new(Other, "独立其他角色", "supporter", Blank, "wei", 6));
            builder.AddDeck(new(Deck, "零自然摸牌的固定实体", 0, 0, [])
            { PhysicalCards = Enumerable.Range(0, 20).Select(index => new ContentDeckPhysicalCard("standard:dodge", Suit.Diamond, index % 13 + 1)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "额外技能帷幕真实目标限制", 3, 3, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Other], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
