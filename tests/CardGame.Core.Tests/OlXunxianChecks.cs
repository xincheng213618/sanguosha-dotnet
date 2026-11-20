using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class OlXunxianChecks
{
    private const string Skill = "ol:xunxian";
    private const string Mode = "identity:classic-xunxian-test";
    private const string Owner = "fixture:xunxian-owner";
    private const string Recipient = "fixture:xunxian-recipient";
    private const string Peer = "fixture:xunxian-peer";
    private const string Watch = "fixture:xunxian-reentry";
    private const string Rediscard = "skill-program.fixture:xunxian-reentry.SelectAndMoveOwnedCard";

    public static void SettledEntityIsGivenOnceAfterChoiceRestore()
    {
        var (game, content) = Start();

        var id = PlayDrawTwo(game);
        var activation = Next(game);
        Require(activation.SkillPrompt?.SkillId == Skill, "The settled physical card must offer 逊贤.");
        Answer(game, activation, activation.Choices.Single(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate"));
        var choicePrompt = Next(game);
        var choices = choicePrompt.Choices.Where(item => item.Parameters.GetValueOrDefault("program-action") == "xunxian-gift").ToArray();
        var players = game.CreateSnapshot(0).Players;
        var owner = players[0];
        var eligible = players.Where(player => player.Seat != 0 && player.IsAlive &&
            (player.HandCount > owner.HandCount || player.Hp > owner.Hp)).Select(player => player.Seat).ToArray();
        var choiceDetails = string.Join(", ", choicePrompt.Choices.Select(item => $"{item.Id} [{string.Join(",", item.Targets)}]"));
        var playerDetails = string.Join(", ", players.Select(player => $"{player.Seat}: HP {player.Hp}, hand {player.HandCount}"));
        Require(choicePrompt is { Kind: DecisionKind.ProgramTrigger, PlayerSeat: 0 } && choicePrompt.SkillPrompt?.SkillId == Skill &&
                eligible.SequenceEqual([1]) && players[1].Hp > owner.Hp && players[1].HandCount <= owner.HandCount &&
                choices.Select(item => item.Targets.SingleOrDefault(-1)).SequenceEqual(eligible) &&
                game.CreateCardZoneDiagnostics().Single(card => card.CardId == id).Location == CardLocation.DiscardPile,
            $"逊贤 must offer exactly the eligible recipients for the still-discarded card. Prompt {choicePrompt.Kind}, seat {choicePrompt.PlayerSeat}, choices {choiceDetails}. Players {playerDetails}.");
        var choice = choices.Single(item => item.Targets.SequenceEqual([1]));

        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(SnapshotJson.Serialize(game.CreateSnapshot(0)) == SnapshotJson.Serialize(restored.CreateSnapshot(0)),
            "Replay must restore the pending recipient choice and original card in the public discard pile.");
        foreach (var run in new[] { game, restored })
        {
            Answer(run, Next(run), choice);
            var second = PlayDrawTwo(run);
            Require(Next(run) is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } &&
                    run.CreateCardZoneDiagnostics().Single(card => card.CardId == second).Location == CardLocation.DiscardPile,
                "A second settled card must remain discarded during the same turn.");

            var discard = run.CardMovements.Single(move => move.CardId == id && move.Reason.Value == "card.use-finished");
            var gift = run.CardMovements.Single(move => move.Reason.Value == "skill-program.ol:xunxian.xunxian-gift");
            var giftFact = run.Events.Select(entry => entry.Payload).OfType<ProgramXunxianGiftEvent>().Single();
            var logicalGive = run.Events.Select(entry => entry.Payload).OfType<NativeCardsGivenEvent>().Single();
            Require(gift.CardId == id && gift.From == CardLocation.DiscardPile && gift.To == CardLocation.Hand(1) &&
                    giftFact.CardId == id && giftFact.RecipientSeat == 1 && giftFact.DiscardMovementSequence == discard.Sequence &&
                    logicalGive.Giver == 0 && logicalGive.Recipient == 1 && logicalGive.Count == 1 && logicalGive.CompletionSequence == gift.Sequence &&
                    run.CreateSnapshot(1).Players[1].Hand.Any(card => card.Id == id),
                "The original entity must move exactly once and produce one logical Give with the correct receipt.");
        }
        Require(game.CardMovements.SequenceEqual(restored.CardMovements) &&
                SnapshotJson.Serialize(game.CreateSnapshot(1)) == SnapshotJson.Serialize(restored.CreateSnapshot(1)),
            "The restored recipient choice must settle identically.");
    }

    public static void XunxianCannotGiveANewerEntryOfItsSettledUseCold()
    {
        var (game, content) = Start(reentry: true);
        var play = Next(game);
        Require(play is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && game.CreateSnapshot(0) is
            { DrawPileCount: 0, DiscardPileCount: 0 }, "The real first draw exhausts the fixed deck before Duel.");
        var use = game.GetHumanLegalActions().First(action => action.Kind == LegalActionKind.Duel && action.TargetSeats.SequenceEqual([2]));
        var card = use.CardId!.Value;
        Submit(game, new PlayCardCommand(0, card, use.TargetSeats, game.Revision, play.PromptId));

        var watcher = Next(game);
        var original = game.CardMovements.Single(move => move.CardId == card && move.Reason == CardMoveReasons.UseFinished);
        var settled = Facts<SettledActionCardDiscardEvent>(game).Single();
        Require(watcher is { PlayerSeat: 0, SkillPrompt.SkillId: Watch } && original.From == CardLocation.Processing &&
            original.To == CardLocation.DiscardPile && settled.ActorSeat == 0 && settled.CardId == card &&
            settled.BatchId == original.BatchId && settled.MovementSequence == original.Sequence &&
            Facts<CardUseFinishedEvent>(game).Single() is { CardKind: CardKind.Duel } finished && finished.CardId == card,
            "The exact true Duel has finished and its settled-use discard remains pending behind the completed-use child.");

        var control = Cold(game, content);
        AnswerAction(control, "skip");
        Give(control);
        var gift = Facts<ProgramXunxianGiftEvent>(control).Single();
        Require(gift.CardId == card && gift.DiscardMovementSequence == original.Sequence &&
            control.CreateCardZoneDiagnostics().Single(c => c.CardId == card).Location == CardLocation.Hand(1),
            "The untouched original Duel can still be given to the eligible recipient.");

        AnswerAction(game, "activate");
        var choose = Next(game);
        var returning = game.CardMovements.Where(move => move.CardId == card && move.Sequence > original.Sequence).ToArray();
        Require(choose is { PlayerSeat: 0, SkillPrompt.SkillId: Watch } && returning is [var shuffled, var drawn] &&
            shuffled.From == CardLocation.DiscardPile && shuffled.To == CardLocation.DrawPile && shuffled.Reason == CardMoveReasons.Reshuffle &&
            drawn.From == CardLocation.DrawPile && drawn.To == CardLocation.Hand(0),
            "The real completed-use child reshuffles and draws the settled Duel before the next public discard choice.");
        game = Cold(game, content);
        choose = Next(game);
        Answer(game, choose, choose.Choices.Single(choice => choice.Cards.SequenceEqual([card])));
        var fresh = game.CardMovements.Single(move => move.CardId == card && move.Reason.Value == Rediscard);
        Require(fresh.Sequence > original.Sequence && fresh.BatchId != original.BatchId && fresh.From == CardLocation.Hand(0) &&
            fresh.To == CardLocation.DiscardPile && Facts<SettledActionCardDiscardEvent>(game).SequenceEqual([settled]),
            "The same physical Duel reenters in a hand discard with no second settled-use fact.");
        if (Next(game).SkillPrompt?.SkillId == Skill)
            Give(game);
        Require(Next(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } && !Facts<ProgramXunxianGiftEvent>(game).Any() &&
            game.CreateCardZoneDiagnostics().Single(c => c.CardId == card).Location == CardLocation.DiscardPile,
            $"Xunxian may not give discard entry {fresh.Sequence} using the old Duel settlement at entry {original.Sequence}.");
        BoundaryLiDianChecks.ReplayFourViews(game, content);
    }

    public static void XunxianKeepsTurnUseWhenNoRecipientCanReceiveCold()
    {
        var (game, content) = Start(emptyRecipients: true);
        var firstCard = PlayDuel(game, 1);
        var first = Next(game);
        var players = game.CreateSnapshot(0).Players;
        var owner = players[0];
        var eligible = players.Where(player => player.Seat != 0 && player.IsAlive &&
            (player.Hp > owner.Hp || player.HandCount > owner.HandCount)).Select(player => player.Seat).ToArray();
        var finishes = Facts<CardUseFinishedEvent>(game).ToArray();
        var settlements = Facts<SettledActionCardDiscardEvent>(game).ToArray();
        var firstDiscard = game.CardMovements.SingleOrDefault(move => move.CardId == firstCard && move.Reason == CardMoveReasons.UseFinished);
        var visibleStats = string.Join(", ", players.Select(player => $"{player.Seat}={player.Hp}/{player.HandCount}"));
        var details = $"Card {firstCard}, prompt {first.Kind}/{first.SkillPrompt?.SkillId}, HP/hand [{visibleStats}], eligible [{string.Join(", ", eligible)}], completion [{string.Join(", ", finishes.AsEnumerable())}], original movement {firstDiscard}, settlements [{string.Join(", ", settlements.AsEnumerable())}].";
        Require(players.Select(player => (player.Hp, player.HandCount)).SequenceEqual([(4, 2), (4, 1), (2, 1), (2, 1)]) &&
            eligible.Length == 0, $"The four actual seats have the reproduced zero-recipient values. {details}");
        Require(finishes is [{ CardKind: CardKind.Duel } finish] && finish.CardId == firstCard &&
            firstDiscard is { From: { Zone: CardZoneKind.Processing }, To: { Zone: CardZoneKind.DiscardPile } } &&
            settlements is [{ ActorSeat: 0 } settled] && settled.BatchId == firstDiscard.BatchId &&
            settled.CardId == firstCard && settled.MovementSequence == firstDiscard.Sequence,
            $"The first Duel completes with one original settled-use entry from Processing to the discard pile. {details}");

        var control = Cold(game, content);
        if (Next(control).SkillPrompt?.SkillId == Skill)
            AnswerAction(control, "skip");
        var controlCard = PlayToHandRecipient(control);
        GiveToHandRecipient(control, controlCard);

        var offered = first.SkillPrompt?.SkillId == Skill;
        if (offered)
            AnswerAction(game, "activate");
        var firstUsage = TurnUsage(game).ToArray();
        Require(Next(game) is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } &&
            game.CreateCardZoneDiagnostics().Single(card => card.CardId == firstCard).Location == CardLocation.DiscardPile &&
            !Facts<ProgramXunxianGiftEvent>(game).Any(),
            "The first Duel stays in the discard pile, and the same turn resumes with no gift.");
        game = Cold(game, content);

        var laterCard = PlayToHandRecipient(game);
        var laterOffered = Next(game).SkillPrompt?.SkillId == Skill;
        if (laterOffered)
            GiveToHandRecipient(game, laterCard);
        Require(!offered && firstUsage.Length == 0 && laterOffered,
            $"Xunxian must suppress the zero-recipient offer, preserve the turn use, and offer the later real gift. First offer {offered}, first usage [{string.Join(", ", firstUsage.Select(use => use.Count))}], eligible later offer {laterOffered}.");
        BoundaryLiDianChecks.ReplayFourViews(game, content);
    }

    private static int PlayToHandRecipient(GameEngine game)
    {
        PlayDuel(game, 2);
        if (Next(game).SkillPrompt?.SkillId == Skill)
            AnswerAction(game, "skip");
        var card = PlayDuel(game, 3);
        _ = Next(game);
        var players = game.CreateSnapshot(0).Players;
        Require(players[1].IsAlive && players[1].Hp == players[0].Hp && players[1].HandCount > players[0].HandCount &&
            Facts<SettledActionCardDiscardEvent>(game).Count() == 3 && !Facts<TurnEndedEvent>(game).Any(),
            "A third real Duel in the same turn makes seat 1 eligible through hand count while HP stays equal.");
        return card;
    }

    private static void GiveToHandRecipient(GameEngine game, int card)
    {
        Require(Next(game).SkillPrompt?.SkillId == Skill && !TurnUsage(game).Any(),
            "The later real Duel offers the still-unspent use of Xunxian.");
        AnswerAction(game, "activate");
        var prompt = Next(game);
        Answer(game, prompt, prompt.Choices.Single(choice => choice.Parameters.GetValueOrDefault("program-action") == "xunxian-gift" &&
            choice.Targets.SequenceEqual([1])));
        Require(Next(game) is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } &&
            TurnUsage(game).Single().Count == 1 &&
            Facts<ProgramXunxianGiftEvent>(game).Single() is { RecipientSeat: 1 } gift && gift.CardId == card &&
            game.CardMovements.Count(move => move.CardId == card && move.From == CardLocation.DiscardPile &&
                move.To == CardLocation.Hand(1) && move.Reason.Value == "skill-program.ol:xunxian.xunxian-gift") == 1,
            "The original third Duel is physically given to the newly eligible recipient and spends exactly one turn use.");
    }

    private static IEnumerable<SkillUsageConsumedEvent> TurnUsage(GameEngine game) =>
        Facts<SkillUsageConsumedEvent>(game).Where(use => use.SkillOwnerSeat == 0 && use.SkillId == Skill && use.Scope == SkillUsageScope.Turn);

    private static int PlayDuel(GameEngine game, int target)
    {
        var play = Next(game);
        Require(play is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }, "The same Xunxian owner is still in their actual Play phase.");
        var action = game.GetHumanLegalActions().First(item => item.Kind == LegalActionKind.Duel && item.TargetSeats.SequenceEqual([target]));
        var id = action.CardId!.Value;
        Submit(game, new PlayCardCommand(0, id, action.TargetSeats, game.Revision, play.PromptId));
        return id;
    }

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(game.CardMovements.SequenceEqual(restored.CardMovements) && Enumerable.Range(0, 4).All(seat =>
            SnapshotJson.Serialize(game.CreateSnapshot(seat)) == SnapshotJson.Serialize(restored.CreateSnapshot(seat))),
            "Cold replay must preserve the complete Use, exact movements and every seat's visible prompt.");
        return restored;
    }

    private static void AnswerAction(GameEngine game, string action)
    {
        var prompt = Next(game);
        Answer(game, prompt, prompt.Choices.Single(choice => choice.Parameters.GetValueOrDefault("program-action") == action));
    }

    private static void Give(GameEngine game)
    {
        Require(Next(game).SkillPrompt?.SkillId == Skill, "The original settlement must offer Xunxian before its recipient choice.");
        var players = game.CreateSnapshot(0).Players;
        Require(players[1].IsAlive && players[1].Hp > players[0].Hp,
            "The untouched high-HP peer qualifies as Xunxian's recipient after another peer takes Duel damage.");
        AnswerAction(game, "activate");
        var prompt = Next(game);
        var give = prompt.Choices.SingleOrDefault(choice => choice.Parameters.GetValueOrDefault("program-action") == "xunxian-gift" &&
            choice.Targets.SequenceEqual([1]));
        Require(give is not null, $"Xunxian must offer its eligible recipient after activation, but opened {prompt.Kind}/{prompt.SkillPrompt?.SkillId}.");
        Answer(game, prompt, give!);
    }

    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();

    private static (GameEngine Game, ContentRegistry Content) Start(bool reentry = false, bool emptyRecipients = false)
    {
        var content = ContentRegistry.Build(new StandardContentPackage(),
            new StandardActiveSkillExpansionPackage(includeJijiu: true), new StandardRescueSkillExpansionPackage(),
            new StandardClassicGeneralPackage(), new Fixture(reentry, emptyRecipients));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 47, PlayerCount = 4, ModeId = Mode, HumanSeat = 0, HumanSeats = [0, 1, 2, 3],
            HumanRole = Role.Lord, UseInteractiveSetup = true, AdvanceAfterHumanCommands = false
        }, content);
        Submit(game, new StartGameCommand());
        while (Next(game) is { Kind: DecisionKind.SelectGeneral } setup)
            Submit(game, new SelectGeneralCommand(setup.PlayerSeat,
                setup.PlayerSeat switch { 0 => Owner, 1 => Recipient, _ => $"{Peer}-{setup.PlayerSeat}" },
                game.Revision, setup.PromptId));
        return (game, content);
    }

    private static int PlayDrawTwo(GameEngine game)
    {
        var play = Next(game);
        Require(play is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }, "The 逊贤 owner must be in Play.");
        var action = game.GetHumanLegalActions().First(item => item.Kind == LegalActionKind.DrawTwo);
        var id = action.CardId!.Value;
        Submit(game, new PlayCardCommand(0, id, action.TargetSeats, game.Revision, play.PromptId, action.PlayedCardKind));
        return id;
    }

    private static PendingDecision Next(GameEngine game)
    {
        for (var step = 0; step < 100; step++)
        {
            for (var seat = 0; seat < game.PlayerCount; seat++)
                if (game.CreateSnapshot(seat).PendingDecision is { } prompt)
                    return prompt;
            game.DriveAdvanceOneStep();
        }
        throw new InvalidOperationException("逊贤 did not reach the next public command boundary.");
    }

    private static void Answer(GameEngine game, PendingDecision prompt, PromptChoice choice) =>
        Submit(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, choice.Id, game.Revision));

    private static void Submit(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        Require(result.Accepted, result.Error?.Message ?? "逊贤 command rejected.");
    }

    private static void Require(bool condition, string message)
    {
        if (!condition)
            throw new InvalidOperationException(message);
    }

    private sealed class Fixture(bool reentry, bool emptyRecipients) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ol-xunxian", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            var generals = new[] { Owner, Recipient, $"{Peer}-2", $"{Peer}-3" };
            if (reentry)
                RegisterWatch(builder);
            builder.AddGeneral(new(Owner, "逊贤使用者", "supporter", Skill, "wu", 3, reentry ? [Watch] : []));
            builder.AddGeneral(new(Recipient, "高体力角色", "supporter", "standard:none", "wu", 5));
            foreach (var id in generals.Skip(2))
                builder.AddGeneral(new(id, "其他角色", "supporter", "standard:none", "wu", 2));
            const string deck = "fixture:xunxian-deck";
            var duelDeck = reentry || emptyRecipients;
            builder.AddDeck(new(deck, duelDeck ? "六张实体决斗" : "无中生有牌堆", duelDeck ? 1 : 4, 2, [])
            {
                PhysicalCards = Enumerable.Range(0, duelDeck ? 6 : 48)
                    .Select(_ => new ContentDeckPhysicalCard(duelDeck ? "standard:duel" : "standard:draw_two", Suit.Heart, 7)).ToArray()
            });
            builder.AddMode(new(Mode, "逊贤规则场景", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, deck,
                GeneralCandidateCount: 4, GeneralPoolIds: generals));
        }

        private static void RegisterWatch(IContentRegistryBuilder builder)
        {
            var rules = $$$"""
                {"skills":[{"id":"{{{Watch}}}","revision":1,"triggers":[{"id":"draw-then-rediscard","window":"cardUseCompleted",
                    "ownerRelation":"actor","cardKinds":["duel"],"singleActionInstance":true,"optional":true,"effects":[
                      {"op":"draw","target":"owner","amount":1},
                      {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},
                        "zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Watch] = new { name = "结算后摸回重弃", description = "摸一张牌，再弃置一张手牌。" } } });
            var catalog = SkillProgramCatalog.LoadCurrent(rules, presentation);
            builder.AddSkill(new(Watch, catalog.Presentations[Watch].Name, catalog.Presentations[Watch].Description)
                { Program = catalog.Programs[Watch], ProgramPresentation = catalog.Presentations[Watch] });
        }
    }
}
