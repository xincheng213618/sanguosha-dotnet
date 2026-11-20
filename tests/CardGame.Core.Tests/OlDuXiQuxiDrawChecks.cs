using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class OlDuXiQuxiDrawChecks
{
    private const string DuXi = "ol:du-xi", Quxi = "ol:quxi";
    private const string More = "fixture:quxi-draw-more", Less = "fixture:quxi-draw-less";
    private const string Watch = "fixture:quxi-draw-gain", Deck = "fixture:quxi-draw-deck";
    private const string ObtainReason = "skill-program.du-xi.quxi.obtain";

    internal static void IssuedFengAndQianModifyTheirActualHoldersNormalDrawCold()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(31, 1, "quxi-draw", DuXi, 3, 0, 0, 0, 0, [], [], [],
            [
                new ShanheCombatantDefinition(More, Hp: 8)
                    { StartingHand = [new(CardKind.Dodge, Suit.Heart, 2), new(CardKind.Dodge, Suit.Diamond, 3)] },
                new ShanheCombatantDefinition(Less, Hp: 8)
                    { StartingHand = [new(CardKind.Dodge, Suit.Heart, 4)] }
            ], [], new string('b', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 3, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 5
        }, content);
        Submit(game, new StartGameCommand());
        Reach(game, () => game.CreateSnapshot(0).PendingDecision is { Kind: DecisionKind.PlayCard } && game.ResolutionStack.Count == 0);
        var play = game.CreateSnapshot(0).PendingDecision!;
        var ownerTurn = FirstTurn(game, 0);
        Need(ActualDraws(game, ownerTurn, 0) == 2 && game.CreateSnapshot(0).Players[1].HandCount == 2 &&
             game.CreateSnapshot(0).Players[2].HandCount == 1,
            "The table's first normal Draw moves two cards, and the other two seats start with genuinely unequal hands.");

        Submit(game, new EndPlayPhaseCommand(0, game.Revision, play.PromptId));
        var offer = AwaitPrompt(game, 0, Quxi, choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        Answer(game, offer, choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        var pair = AwaitPrompt(game, 0, Quxi, choice => IsBranch(choice, "pair"));
        Need(pair.Choices.Count == 1 && pair.Choices[0].Targets.SequenceEqual([2, 1]) &&
             game.CreateSnapshot(0).Players[0].IsFaceDown,
            "The genuine Play-ending skill turns Du Xi face down and offers the exact lesser-hand and greater-hand pair.");
        Answer(game, pair, choice => choice.Targets.SequenceEqual([2, 1]));

        var obtain = AwaitPrompt(game, 2, Quxi, choice => IsBranch(choice, "obtain"));
        var offeredCard = game.CreateCardZoneDiagnostics().Single(card => card.Location == CardLocation.Hand(1) && card.ZoneIndex == 0);
        Need(obtain is { IsPrivate: true, TargetSeat: 1, ValidCardIds.Count: 0 } && obtain.Choices.Count == 2 &&
             obtain.Choices.All(choice => choice.Cards.Count == 0) && game.CreateSnapshot(0).PendingDecision is null,
            "Only the lesser-hand recipient receives the private opaque choices for the original owner's two cards.");
        Answer(game, obtain, choice => choice.Parameters.GetValueOrDefault("slot-index") == "0");
        var child = AwaitPrompt(game, 2, Watch, choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
        var issuedFrame = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Quxi);
        var gain = Events<DuXiQuxiCardObtainedEvent>(game).Single();
        var payment = game.CardMovements.Single(move => move.Sequence == gain.SequenceAfter);
        Need(issuedFrame.DuXiQuxi is { Stage: DuXiQuxiStage.ObtainChildren, LessHandSeat: 2, MoreHandSeat: 1 } &&
             issuedFrame.WindowContext?.Window == SkillProgramTriggerWindow.PlayEnding &&
             gain.FrameId == issuedFrame.Id && gain.RecipientSeat == 2 && gain.CardId == offeredCard.CardId &&
             payment.CardId == offeredCard.CardId && payment.From == CardLocation.Hand(1) &&
             payment.To == CardLocation.Hand(2) && payment.Reason.Value == ObtainReason &&
             !Events<DuXiQuxiMarkerMovedEvent>(game).Any(),
            "The exact chosen entity really moves from the greater hand to the lesser hand before either mark is issued.");

        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        var restoredChild = AwaitPrompt(game, 2, Watch, choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
        Need(restoredChild.PromptId == child.PromptId && Events<DuXiQuxiCardObtainedEvent>(game).Single() == gain &&
             game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == issuedFrame.Id).DuXiQuxi == issuedFrame.DuXiQuxi,
            "The actual gain child restores with the original paid receipt.");
        Answer(game, restoredChild, choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
        Reach(game, () => Events<PhaseChangedEvent>(game).Any(fact => fact is { ActorSeat: 2, Phase: TurnPhase.Play }));

        var accepted = Events<DuXiQuxiAcceptedEvent>(game).Single();
        var marks = Events<DuXiQuxiMarkerMovedEvent>(game).ToArray();
        var firstMarkedTurn = FirstTurn(game, 1);
        var secondMarkedTurn = FirstTurn(game, 2);
        Need(accepted.FrameId == issuedFrame.Id && accepted.ActualTurnNumber == ownerTurn &&
             accepted.Source is { SkillId: Quxi, OwnerSeat: 0 } &&
             marks is [var feng, var qian] && feng is { Marker: PlayerMarkerKind.QuxiFeng, PreviousHolder: null, Holder: 2 } &&
             qian is { Marker: PlayerMarkerKind.QuxiQian, PreviousHolder: null, Holder: 1 } &&
             marks.All(mark => mark.Source == accepted.Source && mark.FrameId == accepted.FrameId && mark.ActualTurnNumber == ownerTurn),
            "The same original limited activation issues one Feng and one Qian to their real holders.");
        Need(ownerTurn < firstMarkedTurn && firstMarkedTurn < secondMarkedTurn &&
             Events<PhaseChangedEvent>(game).Any(fact => fact is { ActorSeat: 1, Phase: TurnPhase.Play }) &&
             ActualDraws(game, firstMarkedTurn, 1) == 1 && ActualDraws(game, secondMarkedTurn, 2) == 3,
            "The holders reach their own actual Play phases after moving one and three normal Draw cards respectively.");
        Need(game.CreateSnapshot(1).Players[1].Markers?.Any(mark => mark is { Kind: PlayerMarkerKind.QuxiQian, Count: 1 }) == true &&
             game.CreateSnapshot(2).Players[2].Markers?.Any(mark => mark is { Kind: PlayerMarkerKind.QuxiFeng, Count: 1 }) == true &&
             Events<DuXiQuxiCardObtainedEvent>(game).Single() == gain &&
             game.CardMovements.Count(move => move.Reason.Value == ObtainReason) == 1,
            "Both issued marks remain after their Draw phases, and the cold return never repeats the card gain.");
    }

    private static int FirstTurn(GameEngine game, int seat) => Events<TurnStartedEvent>(game).First(fact => fact.ActorSeat == seat).TurnNumber;

    private static int ActualDraws(GameEngine game, int turn, int seat) =>
        game.CardMovements.Count(move => move.TurnNumber == turn && move.From == CardLocation.DrawPile &&
            move.To == CardLocation.Hand(seat) && move.Reason == CardMoveReasons.Draw);

    private static IEnumerable<T> Events<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();

    private static bool IsBranch(PromptChoice choice, string branch) => choice.Parameters.GetValueOrDefault("branch") == branch;

    private static PendingDecision AwaitPrompt(GameEngine game, int seat, string skill, Func<PromptChoice, bool> choice)
    {
        bool Ready() => game.CreateSnapshot(seat).PendingDecision is { } prompt &&
            prompt.SkillPrompt?.SkillId == skill && prompt.Choices.Any(choice);
        Reach(game, Ready);
        return game.CreateSnapshot(seat).PendingDecision!;
    }

    private static void Reach(GameEngine game, Func<bool> ready)
    {
        for (var step = 0; step < 140; step++)
        {
            if (ready())
                return;
            var humanView = game.CreateSnapshot(0).PendingDecision;
            Need(game.State.Winner == Winner.None && (humanView is not { PlayerSeat: 0 } ||
                 humanView.Kind == DecisionKind.PlayCard && game.ResolutionStack.Count > 0),
                $"Unexpected Quxi fixture boundary at turn {game.State.TurnNumber}, phase {game.State.Phase}, seat {game.PendingDecision?.PlayerSeat}, skill {game.PendingDecision?.SkillPrompt?.SkillId}.");
            Submit(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The Quxi fixture did not reach the required actual phase or prompt.");
    }

    private static void Answer(GameEngine game, PendingDecision prompt, Func<PromptChoice, bool> pick) =>
        Submit(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.Single(pick).Id, game.Revision));

    private static void Submit(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        Need(result.Accepted, result.Error?.Message ?? "The Quxi fixture command was rejected.");
    }

    private static void Need(bool condition, string message)
    {
        if (!condition)
            throw new InvalidOperationException(message);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:quxi-draw", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, DuXi);
            var rules = $$$"""
                {"skills":[{"id":"{{{Watch}}}","revision":1,"triggers":[{"id":"actual-quxi-gain","window":"cardsGained",
                  "subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["{{{ObtainReason}}}"],
                  "optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"gain","options":[{"id":"continue"}]}]}]}]}
                """;
            var labels = new Dictionary<string, object>
            {
                [Watch] = new { name = "驱徙领取后继续", description = "真实取得驱徙的原牌后暂停。",
                    optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } }
            };
            var programs = SkillProgramCatalog.LoadCurrent(rules,
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            builder.AddSkill(new(Watch, programs.Presentations[Watch].Name, programs.Presentations[Watch].Description)
                { Program = programs.Programs[Watch], ProgramPresentation = programs.Presentations[Watch] });
            builder.AddGeneral(new(More, "手牌较多者", "supporter", "standard:none", "wei", 8));
            builder.AddGeneral(new(Less, "手牌较少者", "supporter", Watch, "wei", 8));
            builder.AddDeck(new(Deck, "固定普通摸牌", 0, 2, [new("standard:dodge", 32)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "驱徙实际摸牌", 3, 3, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [DuXi, More, Less], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
