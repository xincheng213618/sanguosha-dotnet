using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class ClassicGuzhengChecks
{
    private const string Guzheng = "classic:guzheng", Hero = "classic:zhang-zhao-zhang-hong";
    private const string Driver = "fixture:classic-guzheng-driver", Discarder = "fixture:classic-guzheng-discarder";
    private const string Mode = "identity:classic-guzheng-test", Deck = "fixture:classic-guzheng-deck";
    private const string GainReason = "skill-program.classic:guzheng.RestorePhaseHandDiscards";
    private static readonly string[] Generals = [Discarder, Hero, "fixture:classic-guzheng-peer-2", "fixture:classic-guzheng-peer-3"];

    public static void ActualDiscardScopeAndOptionalClaimReplay()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 47, PlayerCount = 4, ModeId = Mode,
            HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, UseInteractiveSetup = true,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        game.DriveStart();
        while (Next(game) is { Kind: DecisionKind.SelectGeneral } selection)
            Submit(game, new SelectGeneralCommand(selection.PlayerSeat, Generals[selection.PlayerSeat], game.Revision, selection.PromptId));

        var beforePhase = Answer(game, choice => choice.Cards.Count == 1).Cards.Single();
        var play = Next(game);
        var equip = game.GetHumanLegalActions().First(action => action.Kind == LegalActionKind.Equip);
        var armor = equip.CardId!.Value;
        Submit(game, new PlayCardCommand(0, armor, equip.TargetSeats, game.Revision, play.PromptId, equip.PlayedCardKind));
        play = Next(game);
        Submit(game, new EndPlayPhaseCommand(0, game.Revision, play.PromptId));
        Answer(game, choice => choice.Cards.SequenceEqual([armor]));
        var additionalHandDiscard = Answer(game, choice => choice.Cards.Count == 1).Cards.Single();
        Answer(game, choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");

        var returns = Next(game);
        var handDiscards = game.CardMovements.Where(m => m.From == CardLocation.Hand(0) &&
            m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.HandLimitDiscard).Select(m => m.CardId).ToHashSet();
        handDiscards.Add(additionalHandDiscard);
        Check(returns.SkillPrompt?.SkillId == Guzheng && returns.PlayerSeat == 1 && handDiscards.Count >= 3 &&
            returns.Choices.SelectMany(c => c.Cards).ToHashSet().SetEquals(handDiscards),
            "Only the phase owner's hand discards can be returned, including a genuine discard outside hand-limit payment.");
        Check(game.CreateSnapshot(0).PendingDecision is null, "The return choice must remain private to the skill owner.");
        Answer(game, choice => choice.Cards.SequenceEqual([additionalHandDiscard]));

        var remaining = handDiscards.Where(id => id != additionalHandDiscard).Append(armor).ToHashSet();
        var child = Next(game);
        Check(child.SkillPrompt?.SkillId == Driver && child.PlayerSeat == 0 &&
            game.CreateSnapshot(0).Players[0].Hand.Any(c => c.Id == additionalHandDiscard) &&
            game.CreateCardZoneDiagnostics().Where(c => remaining.Contains(c.CardId)).All(c => c.Location == CardLocation.DiscardPile),
            "The return must be delivered and its gain child must run before asking about the remaining cards.");
        var coldChild = Restore(game, content);
        var afterReturn = Answer(game, choice => choice.Cards is [var id] && id != additionalHandDiscard).Cards.Single();
        Answer(coldChild, choice => choice.Cards.SequenceEqual([afterReturn]));
        var claim = Next(game);
        Check(claim.SkillPrompt?.SkillId == Guzheng && claim.Choices.Select(c => c.Parameters["program-action"]).ToHashSet()
            .SetEquals(["restore-phase-hand-claim", "restore-phase-hand-leave"]), "The claim needs its own take and leave choices.");
        Next(coldChild);
        Compare(game, coldChild);
        var coldClaim = Restore(coldChild, content);

        foreach (var (run, take) in new[] { (game, true), (coldClaim, false) })
        {
            Answer(run, choice => choice.Parameters["program-action"] == (take ? "restore-phase-hand-claim" : "restore-phase-hand-leave"));
            Check(Next(run) is { Kind: DecisionKind.PlayCard, PlayerSeat: 1 }, "The next player should reach Play.");
            var zones = run.CreateCardZoneDiagnostics().ToDictionary(c => c.CardId, c => c.Location);
            Check(zones[additionalHandDiscard] == CardLocation.Hand(0) && zones[beforePhase] == CardLocation.DiscardPile &&
                zones[afterReturn] == CardLocation.DiscardPile && remaining.All(id => zones[id] ==
                    (take ? CardLocation.Hand(1) : CardLocation.DiscardPile)),
                "Only the frozen phase discards may be claimed, and leaving them must preserve the completed return.");
        }
    }

    private static GameEngine Restore(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Next(restored);
        Compare(game, restored);
        return restored;
    }

    private static void Compare(GameEngine expected, GameEngine actual) =>
        Check(Enumerable.Range(0, expected.PlayerCount).All(seat => SnapshotJson.Serialize(expected.CreateSnapshot(seat)) ==
            SnapshotJson.Serialize(actual.CreateSnapshot(seat))), "Command replay must restore each seat's view at the same choice.");

    private static PendingDecision Next(GameEngine game)
    {
        for (var step = 0; step < 150; step++)
        {
            foreach (var seat in Enumerable.Range(0, game.PlayerCount))
                if (game.CreateSnapshot(seat).PendingDecision is { } prompt) return prompt;
            game.DriveAdvanceOneStep();
        }
        throw new InvalidOperationException("Classic Guzheng did not reach the next public command boundary.");
    }

    private static PromptChoice Answer(GameEngine game, Func<PromptChoice, bool> choose)
    {
        var prompt = Next(game);
        var choice = prompt.Choices.FirstOrDefault(choose) ?? throw new InvalidOperationException(
            $"No matching choice for seat {prompt.PlayerSeat} at {prompt.Kind} ({prompt.SkillPrompt?.SkillId}).");
        Submit(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, choice.Id, game.Revision));
        return choice;
    }

    private static void Submit(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        Check(result.Accepted, result.Error?.Message ?? "Classic Guzheng command rejected.");
    }

    private static void Check(bool valid, string message) { if (!valid) throw new InvalidOperationException(message); }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-classic-guzheng", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var handCost = DiscardFrom("hand");
            var equipmentCost = DiscardFrom("equipment");
            var rules = $$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[{"id":"{{{Driver}}}","revision":1,"triggers":[
                    {"id":"earlier-hand-discard","window":"playPhaseStarting","subject":"owner","turnOwnerScope":"own","optional":false,
                     "usageScope":"game","usageLimit":1,"effects":[{{{handCost}}}]},
                    {"id":"phase-discards","window":"discardPhaseEnded","subject":"owner","turnOwnerScope":"own","allowOwnDiscardPhaseEnded":true,
                     "optional":false,"usageScope":"game","usageLimit":1,"effects":[{{{equipmentCost}}},{{{handCost}}}]},
                    {"id":"late-discard","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch",
                     "movementReasons":["{{{GainReason}}}"],"optional":false,"effects":[{{{handCost}}}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Driver] = new { name = "弃置", description = "在指定时机弃置一张牌。" } } });
            var catalog = SkillProgramCatalog.Load(rules, presentation);
            builder.AddSkill(new(Driver, catalog.Presentations[Driver].Name, catalog.Presentations[Driver].Description)
                { Program = catalog.Programs[Driver], ProgramPresentation = catalog.Presentations[Driver] });
            builder.AddGeneral(new(Discarder, "弃牌角色", "supporter", Driver, "wei", 2));
            foreach (var id in Generals.Skip(2)) builder.AddGeneral(new(id, "其他角色", "supporter", "standard:none", "wei", 4));
            builder.AddDeck(new(Deck, "装备牌", 8, 0, []) { PhysicalCards = Enumerable.Range(0, 64)
                .Select(_ => new ContentDeckPhysicalCard("standard:bagua", Suit.Club, 7)).ToArray() });
            builder.AddMode(new(Mode, "经典固政规则场景", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }

        private static string DiscardFrom(string zone) => $$"""
            {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},
             "zones":["{{zone}}"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}
            """;
    }
}
