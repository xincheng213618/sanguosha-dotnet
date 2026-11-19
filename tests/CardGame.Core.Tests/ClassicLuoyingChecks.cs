using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class ClassicLuoyingChecks
{
    private const string Luoying = "classic:luoying", Driver = "fixture:classic-luoying-driver";
    private const string Mode = "identity:classic-luoying-test", Deck = "fixture:classic-luoying-deck";
    private const string JudgmentReason = "fixture:classic-luoying-judgment";
    private const string ClaimReason = "skill-program.classic:luoying.provenance-claim";
    private const string ReentryDriver = "fixture:classic-luoying-reentry";
    private const string InitialDiscardReason = "skill-program.fixture:classic-luoying-driver.SelectAndMoveOwnedCard";
    private const string LaterDiscardReason = "skill-program.fixture:classic-luoying-reentry.SelectAndMoveOwnedCard";
    private static readonly string[] Generals = ["fixture:classic-luoying-owner", "fixture:classic-luoying-peer-1",
        "fixture:classic-luoying-peer-2", "fixture:classic-luoying-peer-3"];

    public static void OnlyOtherPlayersGenuineClubDiscardAndJudgmentCanBeClaimed()
    {
        var (game, content) = Start(new Fixture());

        var own = Discard(game, 0);
        Need(Next(game) is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } &&
            Zone(game, own) == CardLocation.DiscardPile, "The owner's genuine club discard must remain in the discard pile.");
        var foreign = Discard(game, 1);
        AcceptLuoying(game);

        Use(game, "judge", 1);
        Need(Next(game) is { PlayerSeat: 0, SkillPrompt.SkillId: Luoying }, "The other player's real club judgment must open Luoying.");
        game = Cold(game, content);
        var judgment = game.Events.Select(e => e.Payload).OfType<JudgmentResolvedEvent>().Single(e => e.Reason == JudgmentReason);
        AcceptLuoying(game);
        var claims = game.CardMovements.Where(move => move.Reason.Value == ClaimReason).ToArray();
        Need(judgment is { CardId: { } judged, TargetSeat: 1, Suit: Suit.Club } &&
            claims.Select(move => move.CardId).SequenceEqual([foreign, judged]) &&
            claims.All(move => move.From == CardLocation.DiscardPile && move.To == CardLocation.Hand(0)),
            "Luoying moves exactly the eligible discard and the other player's final judgment result into the owner's hand.");

        var play = Next(game);
        Submit(game, new EndPlayPhaseCommand(0, game.Revision, play.PromptId));
        play = Next(game);
        Need(play is { PlayerSeat: 1, Kind: DecisionKind.PlayCard }, "The other player must reach native Play for recast.");
        var recast = game.CreateSnapshot(1).Players[1].Hand.First(card => card.Kind == CardKind.IronChain).Id;
        Submit(game, new RecastCardCommand(1, recast, game.Revision, play.PromptId));
        Need(Next(game) is { PlayerSeat: 1, Kind: DecisionKind.PlayCard } && Zone(game, recast) == CardLocation.DiscardPile &&
            game.CardMovements.Any(move => move.CardId == recast && move.From == CardLocation.Hand(1) &&
                move.To == CardLocation.DiscardPile && move.Reason == CardMoveReasons.RecastDiscard),
            "A club card of another player entering the discard pile as real recast payment must never open Luoying.");
        _ = Cold(game, content);
    }

    public static void OldDiscardWindowCannotClaimNewPileEntry()
    {
        foreach (var (skill, bundle) in new[]
        {
            (Luoying, "classic-cao-zhi"),
            ("boundary:luoying", "boundary-cao-zhi"),
            ("boundary:luoying-mobile", "boundary-cao-zhi-mobile")
        })
            RequireOriginalEntry(skill, bundle);
    }

    private static void RequireOriginalEntry(string skill, string bundle)
    {
        var (game, content) = Start(new Fixture(skill, bundle));
        var card = Discard(game, 1);
        var first = game.CardMovements.Single(move => move.CardId == card && move.Reason.Value == InitialDiscardReason);
        var discardAgain = Next(game);
        var originalWindow = game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Single(window => window.Batch.Id == first.BatchId);
        Need(discardAgain is { PlayerSeat: 1, SkillPrompt.SkillId: ReentryDriver } &&
            originalWindow.Candidates.Any(candidate => candidate.SkillId == skill) && Zone(game, card) == CardLocation.Hand(1),
            $"The earlier cardsMoved candidate must take the card while the original {skill} candidate is still queued.");
        var reentryChoice = discardAgain.Choices.Single(choice => choice.Cards.SequenceEqual([card]));
        Submit(game, new AnswerPromptCommand(1, discardAgain.PromptId, reentryChoice.Id, game.Revision));

        var fresh = game.CardMovements.Single(move => move.CardId == card && move.Reason.Value == LaterDiscardReason);
        var freshOffer = Next(game);
        Need(fresh.Sequence > first.Sequence && freshOffer.PlayerSeat == 0 && freshOffer.SkillPrompt?.SkillId == skill &&
            game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Last().Batch.Id == fresh.BatchId,
            $"The second actual club discard must issue its own nested {skill} opportunity.");
        var cold = Cold(game, content);
        var claimReason = $"skill-program.{skill}.provenance-claim";
        foreach (var run in new[] { game, cold })
        {
            AnswerTrigger(run, "skip");
            var next = Next(run);
            if (next.SkillPrompt?.SkillId == skill)
            {
                Need(run.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Last().Batch.Id == originalWindow.Batch.Id,
                    "Any remaining offer must belong to the old discard window.");
                AnswerTrigger(run, "activate");
            }

            Need(Next(run) is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && Zone(run, card) == CardLocation.DiscardPile &&
                !run.CardMovements.Any(move => move.Reason.Value == claimReason),
                $"{skill} cannot use the original discard entry {first.Sequence} to claim the same card's newer discard-pile entry {fresh.Sequence}.");
        }
    }

    private static (GameEngine Game, ContentRegistry Content) Start(Fixture fixture)
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), fixture);
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 47, PlayerCount = 4, ModeId = Mode,
            HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, UseInteractiveSetup = true,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        Submit(game, new StartGameCommand());
        while (Next(game) is { Kind: DecisionKind.SelectGeneral } selection)
            Submit(game, new SelectGeneralCommand(selection.PlayerSeat, Generals[selection.PlayerSeat], game.Revision, selection.PromptId));
        return (game, content);
    }

    private static void AnswerTrigger(GameEngine game, string action)
    {
        var prompt = Next(game);
        var choice = prompt.Choices.Single(item => item.Parameters.GetValueOrDefault("program-action") == action);
        Submit(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, choice.Id, game.Revision));
    }

    private static int Discard(GameEngine game, int seat)
    {
        Use(game, "discard", seat);
        var prompt = Next(game);
        Need(prompt is { SkillPrompt.SkillId: Driver } && prompt.PlayerSeat == seat, "The original owner chooses the real discard.");
        var choice = prompt.Choices.First(item => item.Cards.Count == 1);
        Submit(game, new AnswerPromptCommand(seat, prompt.PromptId, choice.Id, game.Revision));
        return choice.Cards.Single();
    }

    private static void AcceptLuoying(GameEngine game)
    {
        var prompt = Next(game);
        Need(prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Luoying } && game.CreateSnapshot(1).PendingDecision is null,
            "The owner privately decides whether to obtain the eligible original card.");
        var choice = prompt.Choices.Single(item => item.Parameters.GetValueOrDefault("program-action") == "activate");
        Submit(game, new AnswerPromptCommand(0, prompt.PromptId, choice.Id, game.Revision));
        Need(Next(game) is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }, "The original Play cursor must continue after Luoying.");
    }

    private static void Use(GameEngine game, string activation, int target)
    {
        var play = Next(game);
        Need(play is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }, "The fixture acts through the original Play prompt.");
        Submit(game, new UseProgramSkillCommand(0, Driver, activation, [], [target], game.Revision, play.PromptId));
    }

    private static CardLocation Zone(GameEngine game, int cardId) =>
        game.CreateCardZoneDiagnostics().Single(card => card.CardId == cardId).Location;

    private static PendingDecision Next(GameEngine game)
    {
        for (var step = 0; step < 100; step++)
        {
            foreach (var seat in Enumerable.Range(0, game.PlayerCount))
                if (game.CreateSnapshot(seat).PendingDecision is { } prompt)
                    return prompt;
            Submit(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("Classic Luoying did not reach the next public command boundary.");
    }

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Need(Enumerable.Range(0, game.PlayerCount).All(seat =>
                SnapshotJson.Serialize(game.CreateSnapshot(seat)) == SnapshotJson.Serialize(restored.CreateSnapshot(seat))) &&
            game.CardMovements.SequenceEqual(restored.CardMovements), "Cold replay preserves the private decision and physical movements.");
        return restored;
    }

    private static void Submit(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        Need(result.Accepted, result.Error?.Message ?? "Classic Luoying command rejected.");
    }

    private static void Need(bool valid, string message) { if (!valid) throw new InvalidOperationException(message); }

    private sealed class Fixture(string skill = Luoying, string? reentryBundle = null) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-classic-luoying", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            if (reentryBundle is null)
                CharacterFixtureContent.Register(builder, "classic:cao-zhi");
            else
            {
                var production = CharacterFixtureContent.EmbeddedProgramBundle(reentryBundle);
                var definition = production.Presentations[skill];
                builder.AddSkill(new(skill, definition.Name, definition.Description)
                    { Program = production.Programs[skill], ProgramPresentation = definition });
                RegisterReentrySkill(builder);
            }
            var rules = $$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[{"id":"{{{Driver}}}","revision":1,"activations":[
                  {"id":"discard","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,
                   "effects":[{"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"selectedTarget"},
                    "cardOwnerRef":{"kind":"selectedTarget"},"zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]},
                  {"id":"judge","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                   "effects":[{"op":"startJudgment","target":"selectedTarget","judgmentReason":"{{{JudgmentReason}}}","resultBind":"judged","visibility":"public"},
                    {"op":"moveBoundCards","target":"owner","sourceBind":"judged","destination":"discardPile","awaitMovementTriggers":true}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Driver] = new { name = "弃置与判定", description = "支付真实手牌弃置，正常判定并弃置结果。" } } });
            var catalog = SkillProgramCatalog.Load(rules, presentation);
            builder.AddSkill(new(Driver, catalog.Presentations[Driver].Name, catalog.Presentations[Driver].Description)
                { Program = catalog.Programs[Driver], ProgramPresentation = catalog.Presentations[Driver] });
            var reenter = reentryBundle is not null;
            builder.AddGeneral(new(Generals[0], reenter ? "落英" : "经典落英", "supporter", skill, "wei", 8, [Driver]));
            foreach (var id in Generals.Skip(1))
                builder.AddGeneral(new(id, "其他角色", "supporter", reenter && id == Generals[1] ? ReentryDriver : "standard:none", "wei", 8));
            builder.AddDeck(new(Deck, reenter ? "梅花八卦阵" : "梅花铁索连环", 4, 2, []) { PhysicalCards = Enumerable.Range(0, 48)
                .Select(_ => new ContentDeckPhysicalCard(reenter ? "standard:bagua" : "standard:iron_chain", Suit.Club, 7)).ToArray() });
            builder.AddMode(new(Mode, "落英来源场景", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }

        private static void RegisterReentrySkill(IContentRegistryBuilder builder)
        {
            var rules = $$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[{"id":"{{{ReentryDriver}}}","revision":1,"triggers":[
                    {"id":"return-and-discard-again","window":"cardsMoved","subject":"owner","sourceZones":["hand"],"movementOccurrence":"perBatch",
                     "movementReasons":["{{{InitialDiscardReason}}}"],"optional":false,"effects":[
                        {"op":"selectRandomPileEquipment","target":"owner","amount":1,"resultBind":"recovered","zones":["discardPile"]},
                        {"op":"moveBoundCards","target":"owner","sourceBind":"recovered","destination":"ownerHand","awaitMovementTriggers":true},
                        {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},
                         "zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [ReentryDriver] = new { name = "取回重弃", description = "取回刚弃置的装备并再次弃置。" } } });
            var catalog = SkillProgramCatalog.Load(rules, presentation);
            builder.AddSkill(new(ReentryDriver, catalog.Presentations[ReentryDriver].Name, catalog.Presentations[ReentryDriver].Description)
                { Program = catalog.Programs[ReentryDriver], ProgramPresentation = catalog.Presentations[ReentryDriver] });
        }
    }
}
