using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class ShiHuangChongCujunClaimChecks
{
    private const string Hero = "mobile:shi-huang-chong", Cujun = "mobile:shi-cujun";
    private const string Watch = "fixture:cujun-first-claim-gain";
    private const string Claim = "skill-program.mobile:shi-cujun.claim";
    private const string Mode = "identity:classic-cujun-claim-child", Deck = "fixture:cujun-claim-child-deck";
    private static readonly string[] Generals = [Hero, "fixture:cujun-recipient", "fixture:cujun-peer-2", "fixture:cujun-peer-3"];

    internal static void CujunCompletesBothDiscardClaimsAfterFirstGainChildCold()
    {
        var (game, content) = Start();
        var initial = game.CreateSnapshot(0);
        Require(initial.Players[0].HandCount == 7 && initial.Players[1].HandCount == 5,
            "The chosen recipient holds fewer cards than the actual discard phase owner.");
        Accept(game, new EndPlayPhaseCommand(0, game.Revision, P(game)!.PromptId));
        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Cujun } &&
            prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate"));
        Activate(game);
        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Cujun } &&
            prompt.Choices.Any(choice => Branch(choice) == "target-1"));
        Answer(game, choice => Branch(choice) == "target-1");
        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Cujun } &&
            prompt.Choices.Any(choice => Branch(choice) == "slash-plus"));
        Answer(game, choice => Branch(choice) == "slash-plus");
        Reach(game, prompt => prompt is { PlayerSeat: 2, SkillPrompt.SkillId: Cujun } &&
            prompt.Choices.Any(choice => Branch(choice) == "slash-plus"));
        Answer(game, choice => Branch(choice) == "slash-plus");
        Reach(game, prompt => prompt is { PlayerSeat: 0, Kind: DecisionKind.DiscardCards });

        var discard = P(game)!;
        var issued = Facts<ShiHcCujunClaimIssuedEvent>(game).Single();
        var started = Facts<ShiHcCujunStartedEvent>(game).Single();
        Require(discard.RequiredCardCount == 2 && issued is { OwnerSeat: 0, TargetSeat: 1 } &&
            issued.FrameId == started.FrameId && Facts<ShiHcCujunSlashGrantedEvent>(game)
                .Select(grant => grant.ChooserSeat).SequenceEqual([0, 2]),
            $"Cujun issues one claim for the recipient before the real discard obligation; found {discard.RequiredCardCount} required discards.");
        var cards = discard.ValidCardIds.Take(discard.RequiredCardCount).ToArray();
        Accept(game, new DiscardCardsCommand(0, cards, discard.PromptId, game.Revision));
        Reach(game, prompt => prompt is { PlayerSeat: 1, SkillPrompt.SkillId: Watch } &&
            prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("option-id") == "continue"));

        var paid = game.CardMovements.Where(move => move.Reason == CardMoveReasons.HandLimitDiscard).ToArray();
        var first = Received(game).Single();
        var parent = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame =>
            frame.SkillId == Cujun && frame.TriggerId == "cujun-claim");
        var gain = game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Single(window => window.Batch.Id == first.BatchId);
        var remaining = cards.Single(card => card != first.CardId);
        Require(paid.Select(move => move.CardId).Order().SequenceEqual(cards.Order()) && paid.All(move =>
                move.CardKind == CardKind.Slash && move.From == CardLocation.Hand(0) && move.To == CardLocation.DiscardPile &&
                move.TurnNumber == issued.TurnNumber && move.Sequence < first.Sequence) &&
            first.From == CardLocation.DiscardPile && first.To == CardLocation.Hand(1) && gain.Batch.ParentFrameId == parent.Id &&
            game.CreateCardZoneDiagnostics().Single(item => item.CardId == remaining).Location == CardLocation.DiscardPile,
            "Two genuine phase discards precede the first physical claim, whose actual gain child pauses with the second entity still in discard.");

        game = Cold(game, content);
        Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
        Reach(game, prompt => prompt is { PlayerSeat: 1, Kind: DecisionKind.PlayCard });
        var completed = Facts<ShiHcCujunClaimSettledEvent>(game).Single();
        var received = Received(game);
        Require(received is [var firstAfter, var second] && firstAfter == first && second.CardId == remaining &&
            second.Sequence > first.Sequence && second.From == CardLocation.DiscardPile && second.To == CardLocation.Hand(1) &&
            completed.ClaimFrameId == issued.FrameId && completed.SettleFrameId == parent.Id &&
            completed.CardIds.Order().SequenceEqual(cards.Order()) && cards.All(card =>
                game.CreateCardZoneDiagnostics().Single(item => item.CardId == card).Location == CardLocation.Hand(1)) &&
            Facts<ShiHcCujunClaimIssuedEvent>(game).Count() == 1,
            "Resuming the first physical gain delivers the second original discard once, preserving one claim and one completed settlement.");

        var grantedTurn = game.State.TurnNumber;
        var materials = game.CreateSnapshot(1).Players[1].Hand.Where(card => card.Kind == CardKind.Slash).Take(4)
            .Select(card => card.Id).ToArray();
        var targetHp = game.CreateSnapshot(1).Players[2].Hp;
        Require(grantedTurn > issued.TurnNumber && materials.Length == 4 && targetHp > 3 && game.GetCombatDistance(1, 2) == 1,
            "The recipient's next actual Play has four physical Slashes and a living adjacent target that survives the whole quota.");
        foreach (var material in materials.Take(3))
            PlayOrdinarySlash(game, material);

        var debits = SlashDebits(game, grantedTurn);
        Require(debits.Length == 3 && debits.Select(debit => debit.PhaseInstanceId).Distinct().Count() == 1 &&
            game.CreateSnapshot(1).Players[2].Hp == targetHp - 3,
            "Both Cujun grants add to the ordinary allowance and each of the three actual Slashes spends one use in the same Play phase.");
        RejectExhaustedSlash(game, materials[3]);
        game = Cold(game, content);

        ReachRecipientsNextPlay(game, grantedTurn);
        var laterTurn = game.State.TurnNumber;
        var next = game.CreateSnapshot(1).Players[1].Hand.First(card => card.Kind == CardKind.Slash && card.Id != materials[3]).Id;
        PlayOrdinarySlash(game, next);
        Require(Facts<ShiHcCujunSlashGrantedEvent>(game).Count() == 2 &&
            SlashDebits(game, laterTurn) is [var ordinary] && ordinary.PhaseInstanceId > debits[0].PhaseInstanceId,
            "The same recipient spends only the ordinary allowance in their following actual Play without a new Cujun grant.");
        RejectExhaustedSlash(game, materials[3]);
    }

    private static CardUseDebitIdentity[] SlashDebits(GameEngine game, int turn) => Facts<CardUseDebitRecordedEvent>(game)
        .Select(entry => entry.Debit).Where(debit => debit.ActorSeat == 1 && debit.Query == SkillRuleQuery.SlashLimit && debit.TurnNumber == turn).ToArray();

    private static void PlayOrdinarySlash(GameEngine game, int card)
    {
        var action = game.GetLegalActions(1).SingleOrDefault(item => item.Kind == LegalActionKind.Slash &&
            item.CardId == card && item.ConversionSource is null && item.TargetSeats.SequenceEqual([2]))
            ?? throw new InvalidOperationException($"The finite allowance did not publish ordinary Slash {card} against the adjacent target.");
        Accept(game, new PlayCardCommand(1, card, action.TargetSeats, game.Revision, P(game)!.PromptId, action.PlayedCardKind));
        Reach(game, prompt => prompt is { PlayerSeat: 1, Kind: DecisionKind.PlayCard });
        Require(game.CardMovements.Count(move => move.CardId == card && move.From == CardLocation.Hand(1) &&
                move.To == CardLocation.Processing && move.Reason == CardMoveReasons.Use) == 1 &&
            Facts<CardUseFinishedEvent>(game).Count(entry => entry.CardId == card && entry.CardKind == CardKind.Slash) == 1,
            "The public command pays its exact held Slash and finishes it once.");
    }

    private static void RejectExhaustedSlash(GameEngine game, int card)
    {
        Require(game.CreateSnapshot(1).Players[1].Hand.Any(item => item.Id == card && item.Kind == CardKind.Slash) &&
            !game.GetLegalActions(1).Any(action => action.Kind == LegalActionKind.Slash && action.CardId == card),
            "An actual unspent Slash remains in hand but is no longer offered after the counted quota is exhausted.");
        var revision = game.Revision;
        var movements = game.CardMovements.Count;
        var result = game.Submit(new PlayCardCommand(1, card, [2], revision, P(game)!.PromptId, CardKind.Slash));
        Require(!result.Accepted && game.Revision == revision && game.CardMovements.Count == movements,
            "Direct submission of the still-held Slash above the quota rejects without payment.");
    }

    private static void ReachRecipientsNextPlay(GameEngine game, int grantedTurn)
    {
        for (var step = 0; step < 100; step++)
        {
            var prompt = P(game);
            if (prompt is { PlayerSeat: 1, Kind: DecisionKind.PlayCard } && game.State.TurnNumber > grantedTurn)
                return;
            if (prompt is { Kind: DecisionKind.PlayCard })
                Accept(game, new EndPlayPhaseCommand(prompt.PlayerSeat, game.Revision, prompt.PromptId));
            else if (prompt is { Kind: DecisionKind.DiscardCards })
                Accept(game, new DiscardCardsCommand(prompt.PlayerSeat, prompt.ValidCardIds.Take(prompt.RequiredCardCount).ToArray(),
                    prompt.PromptId, game.Revision));
            else if (prompt is { SkillPrompt.SkillId: Cujun })
                Skip(game);
            else if (prompt is null)
                Accept(game, new AdvanceOneStepCommand(game.Revision));
            else
                throw new InvalidOperationException($"Unexpected prompt before the recipient's next Play: {prompt.Prompt}");
        }
        throw new InvalidOperationException("The recipient did not reach the following actual Play phase.");
    }

    private static string? Branch(PromptChoice choice) => choice.Parameters.GetValueOrDefault("branch");
    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();
    private static CardMovementRecord[] Received(GameEngine game) => game.CardMovements.Where(move => move.Reason.Value == Claim).ToArray();

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) && Enumerable.Range(0, game.PlayerCount).All(seat =>
            SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold replay preserves the paid discard, first physical delivery and pending gain child in all seat views.");
        return restored;
    }

    private static (GameEngine Game, ContentRegistry Content) Start()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 47, PlayerCount = 4, ModeId = Mode,
            HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, UseInteractiveSetup = true,
            UseInteractiveDiscard = true, AdvanceAfterHumanCommands = false }, content);
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
        public PackageManifest Manifest { get; } = new("fixture-cujun-claim-child", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var rules = $$$"""
                {"skills":[{"id":"{{{Watch}}}","revision":1,
                    "modifiers":[{"id":"recipient-opening","query":"initialHandSize","operation":"add","value":-2,"priority":0}],
                    "triggers":[{"id":"pause-before-next-claim","window":"cardsGained","subject":"owner","destinationZones":["hand"],
                    "movementReasons":["{{{Claim}}}"],"movementOccurrence":"perBatch","usageScope":"game","usageLimit":1,"optional":false,
                    "effects":[{"op":"chooseOption","target":"owner","resultBind":"gain-child","options":[{"id":"continue"}]}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Watch] = new { name = "首次获得后确认", description = "首次因促军获得牌后确认继续。",
                    optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } } });
            var catalog = SkillProgramCatalog.LoadCurrent(rules, presentation);
            builder.AddSkill(new(Watch, catalog.Presentations[Watch].Name, catalog.Presentations[Watch].Description)
                { Program = catalog.Programs[Watch], ProgramPresentation = catalog.Presentations[Watch] });
            builder.AddGeneral(new(Generals[1], "促军受领者", "supporter", Watch, "shu", 8));
            foreach (var peer in Generals.Skip(2))
                builder.AddGeneral(new(peer, "同桌", "supporter", "standard:none", "shu", 8));
            builder.AddDeck(new(Deck, "固定伤害牌", 7, 0, []) { PhysicalCards = Enumerable.Range(0, 36)
                .Select(_ => new ContentDeckPhysicalCard("standard:slash", Suit.Spade, 7)).ToArray() });
            builder.AddMode(new(Mode, "促军领取续程", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}
