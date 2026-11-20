using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileGuzhengChecks
{
    private const string Guzheng = "boundary:guzheng-mobile", Hero = "boundary:zhang-zhao-zhang-hong-mobile";
    private const string Discarder = "fixture:guzheng-discarder", Driver = "fixture:guzheng-phase";
    private const string Mode = "identity:guzheng-test", Deck = "fixture:guzheng-deck";
    private static readonly string[] Generals = [Discarder, Hero, "fixture:guzheng-peer-2", "fixture:guzheng-peer-3"];

    public static void ReturnOneHandDiscardThenChooseRemainingCards()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 47, PlayerCount = 4, ModeId = Mode,
            HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, UseInteractiveSetup = true,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        game.DriveStart();
        while (Next(game) is { Kind: DecisionKind.SelectGeneral } selection)
            Submit(game, new SelectGeneralCommand(selection.PlayerSeat, Generals[selection.PlayerSeat], game.Revision, selection.PromptId));
        var play = Next(game);
        var equipment = game.GetHumanLegalActions().First(action => action.Kind == LegalActionKind.Equip);
        var armor = equipment.CardId!.Value;
        Submit(game, new PlayCardCommand(0, armor, equipment.TargetSeats, game.Revision, play.PromptId, equipment.PlayedCardKind));
        play = Next(game);
        Submit(game, new EndPlayPhaseCommand(0, game.Revision, play.PromptId));
        Answer(game, choice => choice.Cards.SequenceEqual([armor]));
        Answer(game, choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");

        var selectionPrompt = Next(game);
        var handDiscards = game.CardMovements.Where(m => m.From == CardLocation.Hand(0) &&
            m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.HandLimitDiscard).Select(m => m.CardId).ToArray();
        Check(selectionPrompt.SkillPrompt?.SkillId == Guzheng && handDiscards.Length >= 2 &&
            selectionPrompt.Choices.Select(c => c.Cards.Single()).ToHashSet().SetEquals(handDiscards),
            "The published return choices must be exactly the phase owner's hand discards.");
        var returned = handDiscards[0];
        Answer(game, choice => choice.Cards.SequenceEqual([returned]));
        var remaining = handDiscards.Skip(1).Append(armor).ToHashSet();
        var gainChild = Next(game);
        Check(gainChild.SkillPrompt?.SkillId == Driver && game.CreateSnapshot(0).Players[0].Hand.Any(c => c.Id == returned) &&
            game.CreateCardZoneDiagnostics().Where(c => remaining.Contains(c.CardId)).All(c => c.Location == CardLocation.DiscardPile),
            "The return is delivered before its gain child, while the rest of the phase discards stay in the pile.");
        Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
        var claim = Next(game);
        Check(claim.Choices.Select(c => c.Parameters.GetValueOrDefault("branch")).ToHashSet().SetEquals(["claim", "decline"]),
            "Taking the rest must remain a separate decision after returning a card.");
        var resumed = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Check(Enumerable.Range(0, game.PlayerCount).All(seat => SnapshotJson.Serialize(game.CreateSnapshot(seat)) ==
            SnapshotJson.Serialize(resumed.CreateSnapshot(seat))), "Replay must restore every seat's view at the optional claim.");

        foreach (var (run, take) in new[] { (game, true), (resumed, false) })
        {
            Answer(run, choice => choice.Parameters.GetValueOrDefault("branch") == (take ? "claim" : "decline"));
            Check(Next(run) is { Kind: DecisionKind.PlayCard, PlayerSeat: 1 }, "The next player should reach Play.");
            var zones = run.CreateCardZoneDiagnostics().ToDictionary(c => c.CardId, c => c.Location);
            Check(zones[returned] == CardLocation.Hand(0) &&
                remaining.All(id => zones[id] == (take ? CardLocation.Hand(1) : CardLocation.DiscardPile)),
                $"The returned card and remaining hand/equipment discards have the wrong destinations after take={take}.");
        }
    }

    private static PendingDecision Next(GameEngine game)
    {
        for (var step = 0; step < 150; step++)
        {
            foreach (var seat in Enumerable.Range(0, game.PlayerCount))
                if (game.CreateSnapshot(seat).PendingDecision is { } prompt) return prompt;
            game.DriveAdvanceOneStep();
        }
        throw new InvalidOperationException("Guzheng did not reach the next public command boundary.");
    }

    private static void Answer(GameEngine game, Func<PromptChoice, bool> choose)
    {
        var prompt = Next(game);
        var choice = prompt.Choices.SingleOrDefault(choose) ?? throw new InvalidOperationException(
            $"No matching choice for seat {prompt.PlayerSeat} at {prompt.Kind} ({prompt.SkillPrompt?.SkillId}): {string.Join(", ", prompt.Choices.Select(c => c.Id))}.");
        Submit(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, choice.Id, game.Revision));
    }

    private static void Submit(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        Check(result.Accepted, result.Error?.Message ?? "Guzheng command rejected.");
    }

    private static void Check(bool valid, string message) { if (!valid) throw new InvalidOperationException(message); }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-guzheng", new(1, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var rules = $$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[{"id":"{{{Driver}}}","revision":1,"triggers":[
                    {"id":"discard-armor","window":"discardPhaseEnded","subject":"owner","turnOwnerScope":"own",
                     "allowOwnDiscardPhaseEnded":true,"optional":false,"usageScope":"game","usageLimit":1,
                     "effects":[{"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},
                                 "zones":["equipment"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]},
                    {"id":"returned-card","window":"cardsGained","subject":"owner","destinationZones":["hand"],
                     "movementOccurrence":"perBatch","movementReasons":["skill-program.boundary:guzheng-mobile.phase-return"],"optional":false,
                     "effects":[{"op":"chooseOption","target":"owner","resultBind":"continue","options":[{"id":"continue"}]}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Driver] = new { name = "弃置装备与归还响应", description = "弃牌阶段弃置装备，归还手牌后等待。",
                    optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } } });
            var catalog = SkillProgramCatalog.Load(rules, presentation);
            builder.AddSkill(new(Driver, catalog.Presentations[Driver].Name, catalog.Presentations[Driver].Description)
                { Program = catalog.Programs[Driver], ProgramPresentation = catalog.Presentations[Driver] });
            builder.AddGeneral(new(Discarder, "弃牌角色", "supporter", Driver, "wei", 2));
            foreach (var id in Generals.Skip(2)) builder.AddGeneral(new(id, "其他角色", "supporter", "standard:none", "wei", 4));
            builder.AddDeck(new(Deck, "装备牌", 8, 0, []) { PhysicalCards = Enumerable.Range(0, 64)
                .Select(_ => new ContentDeckPhysicalCard("standard:bagua", Suit.Club, 7)).ToArray() });
            builder.AddMode(new(Mode, "固政规则场景", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}
