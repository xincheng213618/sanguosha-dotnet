using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class WushuangGuzhengChecks
{
    private const string Skill = "wushuang:skill-3139", Driver = "fixture:wushuang-guzheng-driver";
    private const string Mode = "identity:wushuang-guzheng-test", Deck = "fixture:wushuang-guzheng-deck";
    private const string ReturnReason = "skill-program.wushuang-guzheng.return", ClaimReason = "skill-program.wushuang-guzheng.claim";
    private static readonly string[] Generals = ["fixture:wushuang-guzheng-discarder", "fixture:wushuang-guzheng-owner",
        "fixture:wushuang-guzheng-peer-2", "fixture:wushuang-guzheng-peer-3"];

    public static void EquipmentReturnAndOriginalEntryClaimReplay()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 47, PlayerCount = 4, ModeId = Mode,
            HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, UseInteractiveSetup = true,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        Submit(game, new StartGameCommand());
        while (Next(game) is { Kind: DecisionKind.SelectGeneral } selection)
            Submit(game, new SelectGeneralCommand(selection.PlayerSeat, Generals[selection.PlayerSeat], game.Revision, selection.PromptId));

        var play = Next(game);
        var equipment = game.GetHumanLegalActions().First(action => action.Kind == LegalActionKind.Equip);
        var armor = equipment.CardId!.Value;
        Submit(game, new PlayCardCommand(0, armor, equipment.TargetSeats, game.Revision, play.PromptId, equipment.PlayedCardKind));
        play = Next(game);
        Submit(game, new EndPlayPhaseCommand(0, game.Revision, play.PromptId));
        Answer(game, choice => choice.Cards.SequenceEqual([armor]));
        var extraHandDiscard = Next(game).Choices.First(choice => choice.Cards.Count == 1).Cards.Single();
        Answer(game, choice => choice.Cards.SequenceEqual([extraHandDiscard]));
        Answer(game, choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        var returns = Next(game);
        var handDiscards = game.CardMovements.Where(move => move.From == CardLocation.Hand(0) &&
            move.To == CardLocation.DiscardPile && move.Reason == CardMoveReasons.HandLimitDiscard).Select(move => move.CardId)
            .Append(extraHandDiscard).ToHashSet();
        Require(returns.SkillPrompt?.SkillId == Skill && returns.PlayerSeat == 1 && handDiscards.Count >= 3 &&
            returns.Choices.SelectMany(choice => choice.Cards).ToHashSet().SetEquals(handDiscards.Append(armor)),
            $"Wushuang Guzheng should admit armor {armor} and hand discards [{string.Join(", ", handDiscards)}]. " +
            $"The prompt is {returns.Kind}/{returns.SkillPrompt?.SkillId} for seat {returns.PlayerSeat} with [{string.Join(", ", returns.Choices.SelectMany(choice => choice.Cards))}].");

        Answer(game, choice => choice.Cards.SequenceEqual([armor]));
        var child = Next(game);
        var reentered = game.CardMovements.Last(move => handDiscards.Contains(move.CardId) &&
            move.From == CardLocation.DiscardPile && move.To == CardLocation.Hand(0)).CardId;
        Require(child.SkillPrompt?.SkillId == Driver && child.PlayerSeat == 0 &&
            game.CardMovements.Single(move => move.Reason.Value == ReturnReason) is { CardId: var returned, To: var recipient } &&
            returned == armor && recipient == CardLocation.Hand(0) && !game.CardMovements.Any(move => move.Reason.Value == ClaimReason),
            "The returned equipment must reach its recipient before its gain child, ahead of any claim.");
        var cold = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Next(cold);
        Require(Enumerable.Range(0, game.PlayerCount).All(seat => SnapshotJson.Serialize(game.CreateSnapshot(seat)) ==
            SnapshotJson.Serialize(cold.CreateSnapshot(seat))), "Replay must restore the paid return's original child and every seat view.");

        foreach (var (current, take) in new[] { (game, true), (cold, false) })
        {
            Answer(current, choice => choice.Cards.SequenceEqual([reentered]));
            Require(Next(current).SkillPrompt?.SkillId == Skill, "The claim must follow the gain child's discard.");
            Answer(current, choice => choice.Parameters.GetValueOrDefault("value") == (take ? "claim" : "pass"));
            Next(current);
            var claims = current.CardMovements.Where(move => move.Reason.Value == ClaimReason).ToArray();
            var originalEntries = handDiscards.Where(id => id != reentered).ToHashSet();
            Require(claims.Select(move => move.CardId).ToHashSet().SetEquals(take ? originalEntries : []) &&
                claims.Select(move => move.BatchId).Distinct().Count() == claims.Length &&
                current.CreateCardZoneDiagnostics().Single(card => card.CardId == reentered).Location == CardLocation.DiscardPile,
                "Claiming must move each still-valid original entry separately. A recovered and re-discarded card remains in the pile.");
        }
    }

    private static PendingDecision Next(GameEngine game)
    {
        for (var step = 0; step < 150; step++)
        {
            for (var seat = 0; seat < game.PlayerCount; seat++)
                if (game.CreateSnapshot(seat).PendingDecision is { } prompt)
                    return prompt;
            Submit(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("Wushuang Guzheng did not reach a public command boundary.");
    }

    private static void Answer(GameEngine game, Func<PromptChoice, bool> select)
    {
        var prompt = Next(game);
        var choice = prompt.Choices.FirstOrDefault(select) ?? throw new InvalidOperationException(JsonSerializer.Serialize(prompt));
        Submit(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, choice.Id, game.Revision));
    }

    private static void Submit(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        Require(result.Accepted, result.Error?.Message ?? "Wushuang Guzheng command was rejected.");
    }

    private static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-wushuang-guzheng", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            var bundle = CharacterFixtureContent.EmbeddedProgramBundle("wushuang-skills-b-audit");
            builder.AddSkill(new(Skill, bundle.Presentations[Skill].Name, bundle.Presentations[Skill].Description)
                { Program = bundle.Programs[Skill], ProgramPresentation = bundle.Presentations[Skill] });
            var rules = $$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[{"id":"{{{Driver}}}","revision":1,"triggers":[
                    {"id":"phase-discards","window":"discardPhaseEnded","subject":"owner","turnOwnerScope":"own","allowOwnDiscardPhaseEnded":true,
                     "optional":false,"usageScope":"game","usageLimit":1,"effects":[{{{DiscardFrom("equipment")}}},{{{DiscardFrom("hand")}}}]},
                    {"id":"return-child","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch",
                     "movementReasons":["{{{ReturnReason}}}"],"optional":false,"effects":[
                         {"op":"selectRandomPileEquipment","target":"owner","amount":1,"resultBind":"recovered","zones":["discardPile"]},
                         {"op":"moveBoundCards","target":"owner","sourceBind":"recovered","destination":"ownerHand","awaitMovementTriggers":true},
                         {{{DiscardFrom("hand")}}}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Driver] = new { name = "弃置", description = "在指定时机弃置装备或手牌。" } } });
            var catalog = SkillProgramCatalog.Load(rules, presentation);
            builder.AddSkill(new(Driver, catalog.Presentations[Driver].Name, catalog.Presentations[Driver].Description)
                { Program = catalog.Programs[Driver], ProgramPresentation = catalog.Presentations[Driver] });
            builder.AddGeneral(new(Generals[0], "弃牌角色", "supporter", Driver, "wei", 2));
            builder.AddGeneral(new(Generals[1], "固政角色", "supporter", Skill, "wu", 4));
            foreach (var id in Generals.Skip(2))
                builder.AddGeneral(new(id, "其他角色", "supporter", "standard:none", "wei", 4));
            builder.AddDeck(new(Deck, "装备牌", 8, 0, []) { PhysicalCards = Enumerable.Range(0, 64)
                .Select(_ => new ContentDeckPhysicalCard("standard:bagua", Suit.Club, 7)).ToArray() });
            builder.AddMode(new(Mode, "无双固政规则场景", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }

        private static string DiscardFrom(string zone) => $$"""
            {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},
             "zones":["{{zone}}"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}
            """;
    }
}
