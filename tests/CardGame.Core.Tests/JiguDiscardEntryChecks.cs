using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class JiguDiscardEntryChecks
{
    private const string Jigu = "mou-ol:jigu", Xunxian = "ol:xunxian";
    private const string Dodge = "fixture:jigu-dodge", Reenter = "fixture:jigu-reentry";
    private const string GiftReason = "skill-program.ol:xunxian.xunxian-gift";
    private const string RediscReason = "skill-program.fixture:jigu-reentry.SelectAndMoveOwnedCard";
    private const string Mode = "identity:jigu-discard-entry-test", Deck = "fixture:jigu-discard-entry-deck";
    private static readonly string[] Generals = ["fixture:jigu-attacker", "fixture:jigu-defender",
        "fixture:jigu-holder", "fixture:jigu-recipient"];

    public static void JiguDoesNotCollectSameDodgeMaterialAfterRediscard()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 47, PlayerCount = 4, ModeId = Mode,
            HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, UseInteractiveSetup = true,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        Submit(game, new StartGameCommand());
        while (Next(game) is { Kind: DecisionKind.SelectGeneral } selection)
            Submit(game, new SelectGeneralCommand(selection.PlayerSeat, Generals[selection.PlayerSeat], game.Revision, selection.PromptId));

        var play = Next(game);
        var slash = game.GetHumanLegalActions().First(action => action.Kind == LegalActionKind.Slash && action.TargetSeats.SequenceEqual([1]));
        Submit(game, new PlayCardCommand(0, slash.CardId!.Value, slash.TargetSeats, game.Revision, play.PromptId, slash.PlayedCardKind));
        Need(Next(game) is { PlayerSeat: 1, Kind: DecisionKind.RespondDodge }, "The real Slash must require the second seat to use Dodge.");
        var material = Answer(game, choice => choice.Cards.Count == 1).Cards.Single();
        var offer = Next(game);
        var origin = game.Events.Select(entry => entry.Payload).OfType<MouDengAiUseDiscardEvent>().Single(fact => fact.CardId == material);
        var window = game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Single(frame => frame.Batch.Id == origin.BatchId);
        Need(offer is { PlayerSeat: 1, SkillPrompt.SkillId: Xunxian } && origin.ActorSeat == 1 &&
            window.Candidates.Any(candidate => candidate.OwnerSeat == 2 && candidate.SkillId == Jigu),
            "The real off-turn Dodge's original settled batch must queue Jigu after Xunxian.");

        var control = Cold(game, content);
        Answer(control, choice => choice.Parameters.GetValueOrDefault("program-action") == "skip");
        Need(Next(control) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } &&
            control.Events.Select(entry => entry.Payload).OfType<MouDengAiGuCollectedEvent>().Single() is { } gain &&
            gain.CardId == material && gain.DiscardSequence == origin.Sequence,
            "Without a prior claimant, Jigu must collect the exact original non-Heart Dodge material.");

        Answer(game, choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        Answer(game, choice => choice.Targets.SequenceEqual([3]));
        Need(Next(game) is { PlayerSeat: 3, SkillPrompt.SkillId: Reenter } &&
            game.CardMovements.Any(move => move.CardId == material && move.Reason.Value == GiftReason &&
                move.From == CardLocation.DiscardPile && move.To == CardLocation.Hand(3)),
            "Xunxian must deliver the original material while the earlier used-card window is paused.");
        game = Cold(game, content);
        Answer(game, choice => choice.Cards.SequenceEqual([material]));
        var later = game.CardMovements.Single(move => move.CardId == material && move.Reason.Value == RediscReason);
        Need(Next(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } &&
            later.From == CardLocation.Hand(3) && later.To == CardLocation.DiscardPile && later.Sequence > origin.Sequence &&
            game.CreateCardZoneDiagnostics().Single(card => card.CardId == material).Location == CardLocation.DiscardPile &&
            !game.Events.Select(entry => entry.Payload).OfType<MouDengAiGuCollectedEvent>().Any(),
            "The new discard of the same physical card must not be taken using the original Dodge's Jigu opportunity.");
        _ = Cold(game, content);
    }

    private static PromptChoice Answer(GameEngine game, Func<PromptChoice, bool> select)
    {
        var prompt = Next(game);
        var choice = prompt.Choices.FirstOrDefault(select) ??
            throw new InvalidOperationException($"No matching choice at {prompt.Kind}, seat {prompt.PlayerSeat}, skill {prompt.SkillPrompt?.SkillId}.");
        Submit(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, choice.Id, game.Revision));
        return choice;
    }

    private static PendingDecision Next(GameEngine game)
    {
        for (var step = 0; step < 120; step++)
        {
            foreach (var seat in Enumerable.Range(0, game.PlayerCount))
                if (game.CreateSnapshot(seat).PendingDecision is { } prompt)
                    return prompt;
            Submit(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("Jigu did not reach the next public command boundary.");
    }

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Need(Enumerable.Range(0, game.PlayerCount).All(seat =>
                SnapshotJson.Serialize(game.CreateSnapshot(seat)) == SnapshotJson.Serialize(restored.CreateSnapshot(seat))) &&
            game.CardMovements.SequenceEqual(restored.CardMovements), "Cold replay must preserve the suspended choice and exact physical entries.");
        return restored;
    }

    private static void Submit(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        Need(result.Accepted, result.Error?.Message ?? "Jigu command rejected.");
    }

    private static void Need(bool valid, string message) { if (!valid) throw new InvalidOperationException(message); }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-jigu-discard-entry", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            foreach (var (id, bundle) in new[] { (Jigu, "mou-deng-ai-ol"), (Xunxian, "ol-yan-jun") })
            {
                var production = CharacterFixtureContent.EmbeddedProgramBundle(bundle);
                var presentation = production.Presentations[id];
                builder.AddSkill(new(id, presentation.Name, presentation.Description)
                    { Program = production.Programs[id], ProgramPresentation = presentation });
            }

            var rules = $$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
                  {"id":"{{{Dodge}}}","revision":1,"viewAs":[{"id":"real-dodge","inputKinds":["slash"],"inputSuits":[],"outputKind":"dodge","forPlay":false,"forResponse":true}]},
                  {"id":"{{{Reenter}}}","revision":1,"triggers":[
                    {"id":"discard-after-gift","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch",
                     "movementReasons":["{{{GiftReason}}}"],"optional":false,"effects":[
                      {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},
                       "zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]}]}]}
                """;
            var presentationJson = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new[] { Dodge, Reenter }.ToDictionary(id => id, id => new { name = id, description = "积谷的真实用牌与弃牌来源场景" }) });
            var catalog = SkillProgramCatalog.Load(rules, presentationJson);
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, id, "积谷来源场景") { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Generals[0], "用杀角色", "supporter", "standard:none", "wei", 8));
            builder.AddGeneral(new(Generals[1], "应闪角色", "supporter", Xunxian, "wei", 4, [Dodge]));
            builder.AddGeneral(new(Generals[2], "积谷角色", "supporter", Jigu, "wei", 5));
            builder.AddGeneral(new(Generals[3], "重弃角色", "supporter", Reenter, "wei", 6));
            builder.AddDeck(new(Deck, "黑色实体杀", 4, 2, []) { PhysicalCards = Enumerable.Range(0, 48)
                .Select(_ => new ContentDeckPhysicalCard("standard:slash", Suit.Spade, 7)).ToArray() });
            builder.AddMode(new(Mode, "积谷原始入堆", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}
