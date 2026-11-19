using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class ClassicLuoyingChecks
{
    private const string Luoying = "classic:luoying", Driver = "fixture:classic-luoying-driver";
    private const string Mode = "identity:classic-luoying-test", Deck = "fixture:classic-luoying-deck";
    private const string JudgmentReason = "fixture:classic-luoying-judgment";
    private const string ClaimReason = "skill-program.classic:luoying.provenance-claim";
    private static readonly string[] Generals = ["fixture:classic-luoying-owner", "fixture:classic-luoying-peer-1",
        "fixture:classic-luoying-peer-2", "fixture:classic-luoying-peer-3"];

    public static void OnlyOtherPlayersGenuineClubDiscardAndJudgmentCanBeClaimed()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 47, PlayerCount = 4, ModeId = Mode,
            HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, UseInteractiveSetup = true,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        Submit(game, new StartGameCommand());
        while (Next(game) is { Kind: DecisionKind.SelectGeneral } selection)
            Submit(game, new SelectGeneralCommand(selection.PlayerSeat, Generals[selection.PlayerSeat], game.Revision, selection.PromptId));

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

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-classic-luoying", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "classic:cao-zhi");
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
            builder.AddGeneral(new(Generals[0], "经典落英", "supporter", Luoying, "wei", 8, [Driver]));
            foreach (var id in Generals.Skip(1))
                builder.AddGeneral(new(id, "其他角色", "supporter", "standard:none", "wei", 8));
            builder.AddDeck(new(Deck, "梅花铁索连环", 4, 2, []) { PhysicalCards = Enumerable.Range(0, 48)
                .Select(_ => new ContentDeckPhysicalCard("standard:iron_chain", Suit.Club, 7)).ToArray() });
            builder.AddMode(new(Mode, "落英来源场景", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}
