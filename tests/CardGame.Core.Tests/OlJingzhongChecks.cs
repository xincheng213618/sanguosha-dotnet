using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class OlJingzhongChecks
{
    private const string Jingzhong = "ol:jingzhong", Hongyan = "boundary:hongyan-mobile";
    private const string Driver = "fixture:jingzhong-driver", Watch = "fixture:jingzhong-watch";
    private const string Mode = "identity:jingzhong-test", Deck = "fixture:jingzhong-deck";
    private const string ClaimReason = "skill-program.ruan-hui.jingzhong.gain";
    private static readonly string[] Generals =
        ["fixture:jingzhong-owner", "fixture:jingzhong-actor", "fixture:jingzhong-target", "fixture:jingzhong-peer"];

    public static void MultiMaterialClaimKeepsMovementWindowsAndUsesOneOfThreeClaims()
    {
        var (game, content, play) = Start(new Fixture(), interactiveDiscard: false);
        Submit(game, new EndPlayPhaseCommand(0, game.Revision, play.PromptId));
        var arm = Next(game);
        Require(arm.SkillPrompt?.SkillId == Jingzhong,
            $"Two genuine black discards should offer Jingzhong. Prompt: {arm.Kind} for seat {arm.PlayerSeat}. " +
            $"Discards: {game.CardMovements.Count(m => m.From == CardLocation.Hand(0) && m.Reason == CardMoveReasons.HandLimitDiscard)}. " +
            $"Recorded black discards: {game.Events.Count(e => e.Payload is JingzhongDiscardEvent)}.");
        Answer(game, arm, c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        Answer(game, Next(game), c => c.Targets.SequenceEqual([1]));

        for (var index = 0; index < 4; index++)
        {
            play = Next(game);
            Require(play is { PlayerSeat: 1, Kind: DecisionKind.PlayCard }, "The named actor should be in the original play phase.");
            var materials = game.CreateSnapshot(1).Players[1].Hand.Take(index == 0 ? 2 : 1).Select(c => c.Id).ToArray();
            Submit(game, index == 0
                ? new UseProgramSkillCommand(1, Driver, "pair", materials, [2], game.Revision, play.PromptId)
                : new PlayCardCommand(1, materials.Single(), [2], game.Revision, play.PromptId, CardKind.Slash));

            var windows = 0;
            for (var child = Next(game); child.Kind != DecisionKind.PlayCard; child = Next(game))
            {
                if (child.Kind == DecisionKind.RespondDodge)
                {
                    Answer(game, child, c => c.Cards.Count == 0);
                    continue;
                }

                Require(child is { PlayerSeat: 0 } && child.SkillPrompt?.SkillId == Watch,
                    "Only the native gain observer should interrupt the original use.");
                if (index == 0 && windows == 0)
                {
                    var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(
                        GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
                    Require(Enumerable.Range(0, game.PlayerCount).All(seat => SnapshotJson.Serialize(game.CreateSnapshot(seat)) ==
                        SnapshotJson.Serialize(restored.CreateSnapshot(seat))), "A suspended gain child must replay identically for all seats.");
                    game = restored;
                }
                Answer(game, child, c => c.Parameters.GetValueOrDefault("option-id") == "continue");
                windows++;
            }

            var obtained = game.CardMovements.Where(m => m.Reason.Value == ClaimReason && materials.Contains(m.CardId)).ToArray();
            var expected = index < 3 ? materials.Length : 0;
            Require(windows == expected && obtained.Length == expected &&
                obtained.All(m => m.From == CardLocation.DiscardPile && m.To == CardLocation.Hand(0)) &&
                obtained.Select(m => m.CardId).Distinct().Count() == expected,
                $"Jingzhong use {index + 1} must deliver {expected} exact entities in their original movement windows.");
        }

        Require(game.Events.Select(e => e.Payload).OfType<JingzhongClaimIssuedEvent>()
            .Select(e => e.EntityCount).SequenceEqual([2, 1, 1]),
            "The multi-material use should consume one claim, and the fourth use should consume none.");
    }

    internal static void JingzhongCountsTheEffectiveColorOfActualHandDiscardsCold()
    {
        Check(Suit.Club, Suit.Club);
        Check(Suit.Spade, Suit.Heart);

        static void Check(Suit printed, Suit effective)
        {
            var (game, content, play) = Start(new Fixture(printed), interactiveDiscard: true);
            var owner = game.CreateSnapshot(0).Players[0];
            Require(owner.Skills?.Any(skill => skill.Id == Hongyan) == true && owner.Hand.All(card => card.Suit == printed),
                "The owner has the real mobile Hongyan and the fixed printed suit.");
            var exemplar = owner.Hand.First();
            Submit(game, new UseProgramSkillCommand(0, Watch, "sample-suit", [], [], game.Revision, play.PromptId));
            Answer(game, Next(game), choice => choice.Cards.SequenceEqual([exemplar.Id]));
            var ready = Next(game);
            var sample = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Watch)
                .CardSetBindings.Single(binding => binding.Name == "sample");
            Require(sample.CardIds.SequenceEqual([exemplar.Id]) && sample.SourceLocations.SequenceEqual([CardLocation.Hand(0)]) &&
                sample.FrozenRevealedSuit == effective,
                $"The selected entity's original paid hand identity is {effective}. Printed {exemplar.Suit}, captured {sample.FrozenRevealedSuit}.");
            Answer(game, ready, choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
            play = Next(game);
            Require(play is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }, "The hand sample returns to the original Play phase.");
            Submit(game, new EndPlayPhaseCommand(0, game.Revision, play.PromptId));
            var discard = Next(game);
            Require(discard is { Kind: DecisionKind.DiscardCards, PlayerSeat: 0, RequiredCardCount: >= 2 } && game.State.Phase == TurnPhase.Discard,
                "The owner is publicly instructed to pay at least two cards in the first actual discard phase.");
            Require(discard.ValidCardIds.Contains(exemplar.Id), "The sampled physical card is one of the legal discard materials.");
            var ids = discard.ValidCardIds.Where(id => id != exemplar.Id).Take(discard.RequiredCardCount - 1).Prepend(exemplar.Id).ToArray();
            var cold = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
            Require(cold.CardMovements.SequenceEqual(game.CardMovements) && Enumerable.Range(0, game.PlayerCount).All(seat =>
                SnapshotJson.Serialize(cold.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
                "Cold recovery preserves the original legal discard prompt and physical hand.");
            game = cold;
            Submit(game, new DiscardCardsCommand(0, ids, Next(game).PromptId, game.Revision));
            var next = Next(game);
            var moves = game.CardMovements.Where(move => move.Reason == CardMoveReasons.HandLimitDiscard &&
                move.From == CardLocation.Hand(0) && move.To == CardLocation.DiscardPile).ToArray();
            Require(moves.Select(move => move.CardId).Order().SequenceEqual(ids.Order()) &&
                game.Events.Select(entry => entry.Payload).OfType<HandLimitDiscardedEvent>().Single() is { ActorSeat: 0 } paid &&
                paid.CardIds.Order().SequenceEqual(ids.Order()),
                "The public command makes every selected physical card a real discard by the phase owner.");
            var recorded = game.Events.Select(entry => entry.Payload).OfType<JingzhongDiscardEvent>().ToArray();
            if (effective == Suit.Heart)
            {
                Require(recorded.Length == 0 && next is { Kind: DecisionKind.PlayCard, PlayerSeat: 1 },
                    $"Hongyan's hearts cannot count toward Jingzhong's black discard requirement. Actual records {recorded.Length}, next {next.SkillPrompt?.SkillId ?? next.Kind.ToString()}.");
                return;
            }

            Require(next is { PlayerSeat: 0, SkillPrompt.SkillId: Jingzhong } && recorded.All(fact => fact.OwnerSeat == 0) &&
                recorded.Select(fact => fact.MovementSequence).Order().SequenceEqual(moves.Select(move => (long)move.Sequence).Order()),
                "The genuinely black club discards satisfy Jingzhong and each original movement is counted once.");
            Answer(game, next, choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
            Answer(game, Next(game), choice => choice.Targets.SequenceEqual([1]));
            Require(game.Events.Select(entry => entry.Payload).OfType<JingzhongLeaseEvent>().Single().RecipientSeat == 1,
                "The real offer can grant Jingzhong for the selected character's next Play phase.");
        }
    }

    private static (GameEngine Game, ContentRegistry Content, PendingDecision Play) Start(Fixture fixture, bool interactiveDiscard)
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), fixture);
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 47, PlayerCount = 4, ModeId = Mode, HumanSeat = 0, HumanSeats = [0, 1, 2, 3],
            HumanRole = Role.Lord, UseInteractiveSetup = true, UseInteractiveDiscard = interactiveDiscard,
            AdvanceAfterHumanCommands = false
        }, content);
        Submit(game, new StartGameCommand());
        var play = Next(game);
        while (play.Kind == DecisionKind.SelectGeneral)
        {
            Submit(game, new SelectGeneralCommand(play.PlayerSeat, Generals[play.PlayerSeat], game.Revision, play.PromptId));
            play = Next(game);
        }

        Require(play is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }, "The Jingzhong owner should start.");
        return (game, content, play);
    }

    private static PendingDecision Next(GameEngine game)
    {
        for (var step = 0; step < 150; step++)
        {
            foreach (var seat in Enumerable.Range(0, game.PlayerCount))
                if (game.CreateSnapshot(seat).PendingDecision is { } prompt)
                    return prompt;
            Submit(game, new AdvanceOneStepCommand(game.Revision));
        }

        throw new InvalidOperationException("Jingzhong did not reach another public command boundary.");
    }

    private static void Answer(GameEngine game, PendingDecision prompt, Func<PromptChoice, bool> select) =>
        Submit(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.Single(select).Id, game.Revision));

    private static void Submit(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        Require(result.Accepted, result.Error?.Message ?? "Jingzhong command rejected.");
    }

    private static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }

    private sealed class Fixture(Suit? hongyanPrintedSuit = null) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ol-jingzhong", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "ol:ruan-hui");
            if (hongyanPrintedSuit is not null)
                CharacterFixtureContent.Register(builder, "boundary:xiao-qiao-mobile");
            var probe = hongyanPrintedSuit is null ? "" : """
                ,"activations":[{"id":"sample-suit","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,
                  "targetKind":"anyLiving","usesPerTurn":1,"effects":[
                    {"op":"selectOwnedCards","target":"owner","amount":1,"zones":["hand"],"resultBind":"sample"},
                    {"op":"chooseOption","target":"owner","resultBind":"sample-ready","options":[{"id":"continue"}]}]}]
                """;
            var rules = $$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,
                   "modifiers":[{"id":"slash-room","query":"slashLimit","operation":"add","value":8,"priority":0}],
                   "viewAs":[{"id":"pair","inputKinds":[],"inputSuits":[],"inputCount":2,"sourceZones":["hand"],
                              "outputKind":"slash","forPlay":true,"forResponse":false,"allowSameKind":true}],
                   "activations":[{"id":"pair","minCards":2,"maxCards":2,"sourceZones":["hand"],"minTargets":1,"maxTargets":1,
                     "targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"useSelectedCardsAs","target":"selectedTarget",
                       "sourceBind":"pair","outputKind":"slash"}]}]},
                  {"id":"{{Watch}}","revision":1,
                   "triggers":[{"id":"each-claim-batch","window":"cardsGained","subject":"owner","destinationZones":["hand"],
                     "movementOccurrence":"perBatch","movementReasons":["{{ClaimReason}}"],"optional":false,
                     "effects":[{"op":"chooseOption","target":"owner","resultBind":"continue","options":[{"id":"continue"}]}]}]{{probe}}}
                ]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object>
                {
                    [Driver] = new { name = "合用两张牌", description = "将两张手牌当杀使用。" },
                    [Watch] = new { name = "观察获得", description = "每批获得后等待。",
                        optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } }
                } });
            var catalog = SkillProgramCatalog.Load(rules, presentation);
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                    { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Generals[0], "敬重角色", "supporter", Jingzhong, "wei", 2,
                hongyanPrintedSuit is null ? [Watch] : [Watch, Hongyan]));
            builder.AddGeneral(new(Generals[1], "被选角色", "supporter", Driver, "shu", 8));
            foreach (var id in Generals.Skip(2))
                builder.AddGeneral(new(id, "其他角色", "supporter", "standard:none", "wu", 8));
            builder.AddDeck(new(Deck, "黑色实体牌", 8, 2, [])
                { PhysicalCards = Enumerable.Range(0, 96).Select(i => new ContentDeckPhysicalCard("standard:slash", hongyanPrintedSuit ?? Suit.Spade, i % 13 + 1)).ToArray() });
            builder.AddMode(new(Mode, "敬重规则场景", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}
