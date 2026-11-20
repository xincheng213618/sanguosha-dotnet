using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class BoundaryMobileHanDangAttackRangeAidOwnerChecks
{
    private const string Hero = "boundary:han-dang-mobile", Jiefan = "boundary:jiefan-mobile";
    private const string Activation = "aid-by-attack-range", Watch = "fixture:attack-range-aid-gain";
    private const string Mode = "identity:attack-range-aid-owner", Deck = "fixture:attack-range-aid-deck";
    private const string Reason = "skill-program.boundary:jiefan-mobile.RequestAttackRangeAid";
    private const int Owner = 0, Recipient = 1, DrawResponder = 2;
    private static readonly string[] Generals = [Hero, "fixture:aid-target", "fixture:aid-adjacent", "fixture:aid-distant"];

    internal static void NativeWeaponAndTypedDrawReturnToOriginalResponderRosterCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 47, PlayerCount = 4, HumanSeat = Owner, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, ModeId = Mode,
            UseInteractiveSetup = true, ConfirmOpeningHand = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false
        }, registry);
        Submit(game, new StartGameCommand());
        var play = Reach(game, OwnPlay, selectGenerals: true);
        var weapon = game.CreateSnapshot(Owner).Players[Owner].Hand.First(card => card.Kind == CardKind.Crossbow).Id;
        var originalRecipientHand = game.CreateSnapshot(Recipient).Players[Recipient].Hand.Select(card => card.Id).ToHashSet();
        Require(game.GetCombatDistance(Owner, Recipient) == 1 && game.GetCombatDistance(DrawResponder, Recipient) == 1 &&
            game.GetCombatDistance(3, Recipient) == 2, "The actual unmounted table has exactly two attackers in range of the recipient.");

        Submit(game, new UseProgramSkillCommand(Owner, Jiefan, Activation, [], [Recipient], game.Revision, play.PromptId));
        var first = Reach(game, prompt => AidPrompt(prompt, Owner));
        var started = Facts<ProgramAttackRangeAidStartedEvent>(game).Single();
        Require(started is { SkillId: Jiefan, BindingId: Activation, OwnerSeat: Owner, TargetSeat: Recipient } &&
            started.ResponderSeats.SequenceEqual([Owner, DrawResponder]) && first.IsPrivate,
            "The real activation freezes the original recipient and only the two actual responders in table order.");
        Answer(game, first, choice => Action(choice) == "attack-range-aid-discard-weapon" && choice.Cards.SequenceEqual([weapon]));

        var second = Reach(game, prompt => AidPrompt(prompt, DrawResponder));
        var discard = Facts<ProgramAttackRangeAidChoiceResolvedEvent>(game).Single();
        Require(discard is { ResponderSeat: Owner, TargetSeat: Recipient } && discard.FrameId == started.FrameId &&
            discard.DiscardedWeaponCardId == weapon && discard.DrawnCardIds.Count == 0 &&
            game.CardMovements.Count(move => move.CardId == weapon && move.From == CardLocation.Hand(Owner) &&
                move.To == CardLocation.Processing && move.Reason.Value == Reason) == 1 &&
            game.CardMovements.Count(move => move.CardId == weapon && move.From == CardLocation.Processing &&
                move.To == CardLocation.DiscardPile && move.Reason.Value == Reason) == 1,
            "The first responder actually pays the original hand weapon before the second receives a choice.");
        Answer(game, second, choice => Action(choice) == "attack-range-aid-draw" && choice.Targets.SequenceEqual([Recipient]));

        var child = Reach(game, prompt => prompt.PlayerSeat == Recipient && prompt.SkillPrompt?.SkillId == Watch &&
            prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("option-id") == "continue"));
        var parent = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == started.FrameId);
        var aid = parent.AttackRangeAid!;
        var draw = aid.Payment?.Draw;
        var gained = game.CardMovements.Single(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(Recipient) &&
            move.Reason.Value == Reason);
        var resolved = Facts<ProgramAttackRangeAidChoiceResolvedEvent>(game).ToArray();
        Require(aid.ResponderIndex == 1 && aid.ResponderSeats.SequenceEqual(started.ResponderSeats) &&
            aid.Payment is { ResponderSeat: DrawResponder, Stage: ProgramAttackRangeAidPaymentStage.MovementChildren, CardId: null } &&
            draw is { RecipientSeat: Recipient, RequestedCount: 1 } && draw.ActualDrawIds.SequenceEqual([gained.CardId]) &&
            draw.SequenceBefore < gained.Sequence && gained.Sequence <= draw.SequenceAfter &&
            resolved.Length == 2 && resolved[0] == discard && resolved[1] is { ResponderSeat: DrawResponder, TargetSeat: Recipient, DiscardedWeaponCardId: null } &&
            resolved[1].FrameId == parent.Id && resolved[1].DrawnCardIds.SequenceEqual([gained.CardId]) &&
            originalRecipientHand.Append(gained.CardId).ToHashSet().SetEquals(game.CreateSnapshot(Recipient).Players[Recipient].Hand.Select(card => card.Id)),
            "The second responder's one physical draw has returned to its typed original frame while the recipient's real gain child runs.");

        var before = game;
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(before.CreateCheckpoint())), registry);
        var restoredChild = Prompt(game)!;
        var restoredParent = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == parent.Id);
        Require(restoredChild.PromptId == child.PromptId && child.PlayerSeat == Recipient &&
            Enumerable.Range(0, 4).All(seat => SnapshotJson.Serialize(game.CreateSnapshot(seat)) == SnapshotJson.Serialize(before.CreateSnapshot(seat))) &&
            game.CardMovements.SequenceEqual(before.CardMovements) && JsonSerializer.Serialize(restoredParent.AttackRangeAid) == JsonSerializer.Serialize(aid),
            "Cold restore preserves the original responder cursor, paid physical draw, movement trail and private continuation.");
        Answer(game, restoredChild, choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
        Reach(game, OwnPlay);
        Require(Facts<ProgramAttackRangeAidChoiceResolvedEvent>(game).Count() == 2 && game.CardMovements.Count(move => move == gained) == 1 &&
            game.ResolutionStack.All(frame => frame.Id != parent.Id) && !game.GetLegalActions(Owner).Any(action => action.ProgramSkillId == Jiefan),
            "Both paid responses retire once and the game-limited actual activation finishes back at the original Play phase.");
    }

    private static string? Action(PromptChoice choice) => choice.Parameters.GetValueOrDefault("program-action");
    private static bool OwnPlay(PendingDecision prompt) => prompt.PlayerSeat == Owner && prompt.Kind == DecisionKind.PlayCard;
    private static bool AidPrompt(PendingDecision prompt, int seat) => prompt.PlayerSeat == seat && prompt.SkillPrompt?.SkillId == Jiefan &&
        prompt.Choices.Any(choice => Action(choice) == "attack-range-aid-draw");
    private static PendingDecision? Prompt(GameEngine game) => Enumerable.Range(0, 4).Select(seat => game.CreateSnapshot(seat).PendingDecision)
        .FirstOrDefault(prompt => prompt is not null);

    private static PendingDecision Reach(GameEngine game, Func<PendingDecision, bool> ready, bool selectGenerals = false)
    {
        for (var step = 0; step < 100; step++)
        {
            var prompt = Prompt(game);
            if (prompt is not null && ready(prompt))
                return prompt;
            if (selectGenerals && prompt is { Kind: DecisionKind.SelectGeneral })
            {
                Submit(game, new SelectGeneralCommand(prompt.PlayerSeat, Generals[prompt.PlayerSeat], game.Revision, prompt.PromptId));
                continue;
            }
            Require(prompt is null && game.State.Winner == Winner.None, $"Unexpected attack-range-aid boundary: {prompt?.Kind}/{prompt?.SkillPrompt?.SkillId}.");
            Submit(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The attack-range-aid fixture did not reach its requested decision.");
    }

    private static void Answer(GameEngine game, PendingDecision prompt, Func<PromptChoice, bool> pick) =>
        Submit(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.Single(pick).Id, game.Revision));
    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();

    private static void Submit(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        Require(result.Accepted, result.Error?.Message ?? "The attack-range-aid fixture command failed.");
    }

    private static void Require(bool condition, string message)
    {
        if (!condition)
            throw new InvalidOperationException(message);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:han-dang-attack-range-aid", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var rules = $$"""
                {"skills":[{"id":"{{Watch}}","revision":1,"triggers":[
                  {"id":"received-exact-aid-draw","window":"cardsGained","subject":"owner","destinationZones":["hand"],
                   "movementOccurrence":"perBatch","movementReasons":["{{Reason}}"],"optional":false,
                   "effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]}
                ]}]}
                """;
            var labels = new Dictionary<string, object>
            {
                [Watch] = new { name = "解烦摸牌后继续", description = "真实摸牌后暂停。",
                    optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } }
            };
            var catalog = SkillProgramCatalog.LoadCurrent(rules,
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            builder.AddSkill(new(Watch, catalog.Presentations[Watch].Name, catalog.Presentations[Watch].Description)
                { Program = catalog.Programs[Watch], ProgramPresentation = catalog.Presentations[Watch] });
            builder.AddGeneral(new(Generals[Recipient], "受益者", "supporter", Watch, "shu", 4));
            builder.AddGeneral(new(Generals[DrawResponder], "相邻响应者", "supporter", "standard:none", "wei", 4));
            builder.AddGeneral(new(Generals[3], "远邻", "supporter", "standard:none", "wei", 4));
            builder.AddDeck(new(Deck, "解烦固定原实体", 2, 0, [])
            {
                PhysicalCards = Enumerable.Repeat(new ContentDeckPhysicalCard("standard:crossbow", Suit.Club, 1), 24).ToArray()
            });
            builder.AddMode(new(Mode, "解烦原响应顺序", 4, 4,
                new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}
