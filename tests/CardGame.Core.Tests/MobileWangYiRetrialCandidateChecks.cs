using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class MobileWangYiRetrialCandidateChecks
{
    private const string General = "mobile:wang-yi", Skill = "mobile:zhenlie";
    private const string Driver = "fixture:wang-yi-retrial-judge", Watch = "fixture:wang-yi-retrial-movement";
    private const string Peer = "fixture:wang-yi-retrial-peer", Deck = "fixture:wang-yi-retrial-deck";
    private const string Reason = "fixture:wang-yi-retrial";

    internal static void EmptyDrawPileOffersBlindRetrialAndPaysRecycledCardCold()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(31, 1, "wang-yi-retrial", General, 3, 0, 0, 0, 0, [Driver, Watch],
            [new(CardKind.Alcohol, Suit.Spade, 3)], [], [new ShanheCombatantDefinition(Peer, Hp: 4) { StartingHand = [] }],
            [], new string('c', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 2
        }, content);
        Submit(game, new StartGameCommand());
        var play = Reach(game, IsOpenPlay);
        var alcohol = game.CreateSnapshot(0).Players[0].Hand.Single(card => card.Kind == CardKind.Alcohol);
        var drawCard = game.CreateCardZoneDiagnostics().Single(card => card.Location == CardLocation.DrawPile);
        var drink = game.GetHumanLegalActions().Single(action => action.Kind == LegalActionKind.Alcohol &&
            action.CardId == alcohol.Id && action.ConversionSource is null);
        Submit(game, new PlayCardCommand(0, alcohol.Id, drink.TargetSeats, game.Revision, play.PromptId, drink.PlayedCardKind));
        play = Reach(game, IsOpenPlay);
        Need(Location(game, alcohol.Id) == CardLocation.DiscardPile && Location(game, drawCard.CardId) == CardLocation.DrawPile,
            "A real use puts Alcohol in discard and leaves one physical card for the initial judgment.");
        var beforeJudgment = game.CardMovements.Last().Sequence;
        Submit(game, new UseProgramSkillCommand(0, Driver, "judge", [], [], game.Revision, play.PromptId));
        var offer = Reach(game, IsRetrial);
        var judgment = game.ResolutionStack.OfType<JudgmentFrame>().Single();
        var original = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Driver);
        var firstEntry = game.CardMovements.Single(move => move.Sequence > beforeJudgment &&
            move.CardId == drawCard.CardId && move.From == CardLocation.DrawPile && move.To == CardLocation.Judgment(0));
        Need(judgment.CardId == drawCard.CardId && judgment.TargetSeat == 0 && judgment.ParentFrameId == original.Id &&
             game.CreateCardZoneDiagnostics().All(card => card.Location != CardLocation.DrawPile) &&
             game.CreateCardZoneDiagnostics().Single(card => card.Location == CardLocation.DiscardPile).CardId == alcohol.Id,
            "The actual initial judgment consumes the last draw card while the used entity remains the only possible recycled material.");
        Need(offer is { PlayerSeat: 0, IsPrivate: true, ValidCardIds.Count: 0, ValidTargetSeats.Count: 0, Choices.Count: 2 } &&
             offer.Choices.All(choice => choice.Cards.Count == 0 && choice.Targets.Count == 0) &&
             game.CreateSnapshot(1).PendingDecision is null &&
             game.CardMovements.All(move => move.Sequence <= firstEntry.Sequence) &&
             game.CardMovements.All(move => move.Reason != CardMoveReasons.Reshuffle),
            "The original Wang Yi offer is private and blind, with no paid movement or reshuffle.");

        game = Cold(game, content);
        var restoredOffer = Reach(game, IsRetrial);
        Need(restoredOffer.PromptId == offer.PromptId && restoredOffer.ValidCardIds.Count == 0 &&
             game.CreateSnapshot(1).PendingDecision is null &&
             game.CardMovements.Last().Sequence == firstEntry.Sequence && Location(game, alcohol.Id) == CardLocation.DiscardPile,
            "Cold replay keeps the same unpaid blind offer, even though the draw pile is empty.");
        Answer(game, restoredOffer, "action", "program-judgment-top-replace");
        var child = Reach(game, (_, prompt) => prompt.SkillPrompt?.SkillId == Watch);
        var retrial = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Skill);
        var shuffled = game.CardMovements.Single(move => move.Reason == CardMoveReasons.Reshuffle);
        var payment = game.CardMovements.Where(move => move.CardId == alcohol.Id && move.Reason == CardMoveReasons.ProgramJudgmentReplace).ToArray();
        Need(shuffled.CardId == alcohol.Id && shuffled.From == CardLocation.DiscardPile && shuffled.To == CardLocation.DrawPile &&
             shuffled.Sequence > firstEntry.Sequence && payment is [var withdrawn, var entered] &&
             withdrawn.From == CardLocation.DrawPile && withdrawn.To == CardLocation.Processing &&
             entered.From == CardLocation.Processing && entered.To == CardLocation.Judgment(0) &&
             shuffled.Sequence < withdrawn.Sequence && withdrawn.Sequence < entered.Sequence,
            "Only actual acceptance recycles the one discard and pays it as the actual drawer's top entity.");
        Need(retrial.WindowContext?.JudgmentReplacement is
                { JudgmentFrameId: var parent, OldCardId: var old, ReplacementCardId: var paid, ReplacementSuit: Suit.Spade } &&
             parent == judgment.Id && old == drawCard.CardId && paid == alcohol.Id &&
             Location(game, drawCard.CardId) == CardLocation.DiscardPile && Location(game, alcohol.Id) == CardLocation.Judgment(0) &&
             game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Any(window => window.Batch.ParentFrameId == retrial.Id &&
                 window.Batch.OriginSkillId == Skill && window.Batch.Movements.Any(move => move.Sequence == payment[0].Sequence)),
            "The exact paid entity and old judgment remain on the native original parent through its real movement child.");

        game = Cold(game, content);
        var restoredChild = Reach(game, (_, prompt) => prompt.SkillPrompt?.SkillId == Watch);
        Need(restoredChild.PromptId == child.PromptId &&
             game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Skill)
                 .WindowContext?.JudgmentReplacement == retrial.WindowContext?.JudgmentReplacement &&
             Location(game, alcohol.Id) == CardLocation.Judgment(0),
            "Cold replay returns to the original paid movement child with the same replacement entity.");
        Answer(game, restoredChild, "option-id", "continue");
        Reach(game, IsOpenPlay);
        var finished = game.Events.Select(entry => entry.Payload).OfType<JudgmentResolvedEvent>().Single(fact => fact.Reason == Reason);
        var replacement = game.Events.Select(entry => entry.Payload).OfType<ProgramJudgmentReplacementResolvedEvent>()
            .Single(fact => fact.SkillId == Skill);
        Need(finished.ResolutionId == judgment.Id && finished.ParentResolutionId == original.Id &&
             finished.CardId == alcohol.Id && finished.Suit == Suit.Spade &&
             replacement.Activated && replacement.ReplacementCardId == alcohol.Id &&
             game.CardMovements.Count(move => move.Reason == CardMoveReasons.Reshuffle) == 1 &&
             game.CardMovements.Where(move => move.Reason == CardMoveReasons.ProgramJudgmentReplace).SequenceEqual(payment),
            "The original judgment finishes once with the replacement and no second shuffle or payment.");
    }

    private static GameEngine Cold(GameEngine game, ContentRegistry content) =>
        GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);

    private static CardLocation Location(GameEngine game, int cardId) =>
        game.CreateCardZoneDiagnostics().Single(card => card.CardId == cardId).Location;

    private static bool IsOpenPlay(GameEngine game, PendingDecision prompt) =>
        prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && game.ResolutionStack.Count == 0;

    private static bool IsRetrial(GameEngine game, PendingDecision prompt) =>
        prompt is { Kind: DecisionKind.ProgramJudgmentReplacement, SkillPrompt.SkillId: Skill };

    private static PendingDecision Reach(GameEngine game, Func<GameEngine, PendingDecision, bool> ready)
    {
        for (var step = 0; step < 60; step++)
        {
            if (game.CreateSnapshot(0).PendingDecision is { } prompt)
            {
                if (ready(game, prompt))
                    return prompt;
                Need(prompt.Kind == DecisionKind.PlayCard && game.ResolutionStack.Count != 0,
                    $"Unexpected retrial fixture prompt {prompt.Kind} for seat {prompt.PlayerSeat} and skill {prompt.SkillPrompt?.SkillId}.");
            }
            Submit(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The retrial fixture did not reach the expected native prompt.");
    }

    private static void Answer(GameEngine game, PendingDecision prompt, string key, string value)
    {
        var choice = prompt.Choices.Single(candidate => candidate.Parameters.GetValueOrDefault(key) == value);
        Submit(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, choice.Id, game.Revision));
    }

    private static void Submit(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        Need(result.Accepted, result.Error?.Message ?? "The retrial fixture command was rejected.");
    }

    private static void Need(bool condition, string message)
    {
        if (!condition)
            throw new InvalidOperationException(message);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:wang-yi-retrial", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, General);
            var rules = $$$"""
                {"skills":[
                  {"id":"{{{Driver}}}","revision":1,"activations":[{"id":"judge","minCards":0,"maxCards":0,"minTargets":0,
                    "maxTargets":0,"targetKind":"anyLiving","usesPerTurn":1,"effects":[
                      {"op":"startJudgment","target":"owner","judgmentReason":"{{{Reason}}}","resultBind":"judged","visibility":"public"},
                      {"op":"moveBoundCards","target":"owner","sourceBind":"judged","destination":"discardPile"}
                    ]}]},
                  {"id":"{{{Watch}}}","revision":1,"triggers":[{"id":"replacement-payment","window":"cardsMoved","subject":"owner",
                    "sourceZones":["judgment"],"movementOccurrence":"perBatch","movementReasons":["{{{CardMoveReasons.ProgramJudgmentOldCard.Value}}}"],
                    "optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"seen","options":[{"id":"continue"}]}]}]}
                ]}
                """;
            var labels = new Dictionary<string, object>
            {
                [Driver] = new { name = "真实判定", description = "对自己进行一次原生判定。" },
                [Watch] = new { name = "改判支付后继续", description = "原判定牌移走后暂停。",
                    optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } }
            };
            var programs = SkillProgramCatalog.LoadCurrent(rules,
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in programs.Programs)
                builder.AddSkill(new(id, programs.Presentations[id].Name, programs.Presentations[id].Description)
                    { Program = program, ProgramPresentation = programs.Presentations[id] });
            builder.AddGeneral(new(Peer, "同桌", "supporter", "standard:none", "wu", 4));
            builder.AddDeck(new(Deck, "唯一初判实体", 0, 0, [])
                { PhysicalCards = [new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, 7)] });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "贞烈盲顶判定", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [General, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
