using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class OlCaoTengJianxiongChecks
{
    private const string Skill = "ol:caoteng-jianxiong", Watch = "fixture:cao-teng-draw-child";
    private const string Mode = "identity:cao-teng-jianxiong-test", Deck = "fixture:cao-teng-group-deck";
    private const string DrawReason = "skill-program.cao-teng.jianxiong.draw";
    private const string ClaimReason = "skill-program.cao-teng.jianxiong.claim";
    private static readonly string[] Generals =
        ["fixture:cao-teng-attacker", "fixture:cao-teng-first", "fixture:cao-teng-second", "fixture:cao-teng-peer"];

    public static void DrawChildPrecedesOriginalGroupMaterialClaim()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 47, PlayerCount = 4, ModeId = Mode,
            HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, UseInteractiveSetup = true,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        Submit(game, new StartGameCommand());
        while (Next(game) is { Kind: DecisionKind.SelectGeneral } selection)
            Submit(game, new SelectGeneralCommand(selection.PlayerSeat, Generals[selection.PlayerSeat], game.Revision, selection.PromptId));

        var play = Next(game);
        var action = game.GetLegalActions(0).First(action => action.Kind == LegalActionKind.BarbarianAssault);
        var material = action.CardId!.Value;
        Submit(game, new PlayCardCommand(0, material, action.TargetSeats, game.Revision, play.PromptId, action.PlayedCardKind));

        var first = Next(game);
        Require(first is { PlayerSeat: 1, SkillPrompt.SkillId: Skill }, "The first damaged player should be offered Jianxiong.");
        Answer(game, first, choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        var draw = Next(game);
        Require(draw is { PlayerSeat: 1, SkillPrompt.SkillId: Watch }, "The actual draw must open its own gain child.");
        var claimFrame = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Skill);
        var entry = game.CardMovements.Single(move => move.CardId == material && move.To == CardLocation.Processing);
        Require(claimFrame.CaoTeng is { Stage: CaoTengStage.JianxiongDrawChildren } receipt &&
            receipt.Materials is [{ CardId: var frozenId, EntrySequence: var frozenEntry }] &&
            frozenId == material && frozenEntry == entry.Sequence && Zone(game, material) == CardLocation.Processing &&
            !game.CardMovements.Any(move => move.Reason.Value == ClaimReason),
            "The first draw child owns the original Processing entry before any damage material is claimed.");

        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(Enumerable.Range(0, game.PlayerCount).All(seat =>
            SnapshotJson.Serialize(game.CreateSnapshot(seat)) == SnapshotJson.Serialize(restored.CreateSnapshot(seat))),
            "Cold replay must restore the draw child for every seat.");
        game = restored;
        Answer(game, Next(game), choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");

        var second = Next(game);
        Require(second is { PlayerSeat: 2, SkillPrompt.SkillId: Skill }, "The original group use must proceed to its second damaged player.");
        var use = game.ResolutionStack.OfType<CardUseFrame>().Single(frame => frame.CardId == material);
        var delivery = game.CardMovements.Single(move => move.CardId == material && move.Reason.Value == ClaimReason);
        Require(use.MaterialClaims.Any(claim => claim.CardId == material && claim.RecipientSeat == 1 &&
                claim.MovementSequence == delivery.Sequence) &&
            use.Continuations.GroupCard?.DamageClaimedPhysicalCardIds.SequenceEqual([material]) == true &&
            Zone(game, material) == CardLocation.Hand(1),
            "The original use and group ledgers retain the first player's exact physical delivery.");

        Answer(game, second, choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        Require(Next(game) is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } &&
            game.CardMovements.Count(move => move.Reason.Value == ClaimReason) == 1 &&
            game.CardMovements.Count(move => move.Reason.Value == DrawReason &&
                (move.To == CardLocation.Hand(1) || move.To == CardLocation.Hand(2))) == 2,
            "The second damaged player still draws once, and the spent original group card is claimed only once.");
    }

    private static CardLocation Zone(GameEngine game, int cardId) =>
        game.CreateCardZoneDiagnostics().Single(card => card.CardId == cardId).Location;

    private static PendingDecision Next(GameEngine game)
    {
        for (var step = 0; step < 150; step++)
        {
            var prompt = Enumerable.Range(0, game.PlayerCount).Select(seat => game.CreateSnapshot(seat).PendingDecision)
                .FirstOrDefault(decision => decision is not null);
            if (prompt is { Kind: DecisionKind.RespondSlash })
                Answer(game, prompt, choice => choice.Cards.Count == 0);
            else if (prompt is not null)
                return prompt;
            else
                Submit(game, new AdvanceOneStepCommand(game.Revision));
        }

        throw new InvalidOperationException("Cao Teng Jianxiong did not reach another command boundary.");
    }

    private static void Answer(GameEngine game, PendingDecision prompt, Func<PromptChoice, bool> match) =>
        Submit(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.Single(match).Id, game.Revision));

    private static void Submit(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        Require(result.Accepted, result.Error?.Message ?? "Cao Teng Jianxiong command rejected.");
    }

    private static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ol-cao-teng-jianxiong", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "ol:cao-teng", "ol:liu-xie");
            var rules = $$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Watch}}","revision":1,"triggers":[
                 {"id":"draw-child","window":"cardsGained","subject":"owner","destinationZones":["hand"],
                  "movementOccurrence":"perBatch","movementReasons":["{{DrawReason}}"],"optional":false,
                  "effects":[{"op":"chooseOption","target":"owner","resultBind":"paused","options":[{"id":"continue"}]}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Watch] = new { name = "观察奸雄摸牌", description = "摸牌后等待继续。",
                    optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } } });
            var catalog = SkillProgramCatalog.Load(rules, presentation);
            builder.AddSkill(new(Watch, catalog.Presentations[Watch].Name, catalog.Presentations[Watch].Description)
                { Program = catalog.Programs[Watch], ProgramPresentation = catalog.Presentations[Watch] });
            builder.AddGeneral(new(Generals[0], "南蛮使用者", "supporter", "standard:none", "qun", 5));
            builder.AddGeneral(new(Generals[1], "先受伤角色", "supporter", Skill, "wei", 4, [Watch]));
            builder.AddGeneral(new(Generals[2], "后受伤角色", "supporter", Skill, "wei", 4));
            builder.AddGeneral(new(Generals[3], "其他角色", "supporter", "standard:none", "qun", 5));
            builder.AddDeck(new(Deck, "南蛮入侵", 4, 2, []) { PhysicalCards = Enumerable.Range(0, 64)
                .Select(_ => new ContentDeckPhysicalCard("standard:barbarian_assault", Suit.Spade, 7)).ToArray() });
            builder.AddMode(new(Mode, "曹腾奸雄规则场景", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}
