using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class OlJingzhongChecks
{
    private const string Jingzhong = "ol:jingzhong";
    private const string Driver = "fixture:jingzhong-driver", Watch = "fixture:jingzhong-watch";
    private const string Mode = "identity:jingzhong-test", Deck = "fixture:jingzhong-deck";
    private const string ClaimReason = "skill-program.ruan-hui.jingzhong.gain";
    private static readonly string[] Generals =
        ["fixture:jingzhong-owner", "fixture:jingzhong-actor", "fixture:jingzhong-target", "fixture:jingzhong-peer"];

    public static void MultiMaterialClaimKeepsMovementWindowsAndUsesOneOfThreeClaims()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 47, PlayerCount = 4, ModeId = Mode, HumanSeat = 0, HumanSeats = [0, 1, 2, 3],
            HumanRole = Role.Lord, UseInteractiveSetup = true, UseInteractiveDiscard = false,
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

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ol-jingzhong", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "ol:ruan-hui");
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
                     "effects":[{"op":"chooseOption","target":"owner","resultBind":"continue","options":[{"id":"continue"}]}]}]}
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
            builder.AddGeneral(new(Generals[0], "敬重角色", "supporter", Jingzhong, "wei", 2, [Watch]));
            builder.AddGeneral(new(Generals[1], "被选角色", "supporter", Driver, "shu", 8));
            foreach (var id in Generals.Skip(2))
                builder.AddGeneral(new(id, "其他角色", "supporter", "standard:none", "wu", 8));
            builder.AddDeck(new(Deck, "黑色实体牌", 8, 2, [])
                { PhysicalCards = Enumerable.Range(0, 96).Select(i => new ContentDeckPhysicalCard("standard:slash", Suit.Spade, i % 13 + 1)).ToArray() });
            builder.AddMode(new(Mode, "敬重规则场景", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}
