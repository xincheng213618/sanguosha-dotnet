using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class OlSpLiuBei664Checks
{
    private const string Hero = StandardOlSpLiuBei664Package.GeneralId, Zhujiu = StandardOlSpLiuBei664Package.ZhujiuSkillId,
        Xudai = StandardOlSpLiuBei664Package.XudaiSkillId, Jinglei = StandardOlSpLiuBei664Package.JingleiSkillId;
    private const string Claim = "fixture:jinglei-earlier-claim", Mode = "identity:classic-jinglei-original-discard", Deck = "fixture:jinglei-deck";
    private const string Discard = "skill.ol-sp-liu-bei.jinglei.discard", Give = "skill.ol-sp-liu-bei.jinglei.give";
    private const string Take = "skill-program.fixture:jinglei-earlier-claim.ClaimMovedCards";
    private const string Rediscard = "skill-program.fixture:jinglei-earlier-claim.SelectAndMoveOwnedCard";
    private static readonly string[] Generals = [Hero, "fixture:jinglei-holder", "fixture:jinglei-claimant", "fixture:jinglei-peer"];

    internal static void JingleiCannotGiveTheSameCardAtANewerDiscardEntryCold()
    {
        var (game, content) = Start();
        var ownerHp = game.CreateSnapshot(0).Players[0].Hp;
        Accept(game, new UseProgramSkillCommand(0, Zhujiu, "brew", [], [], game.Revision, P(game)!.PromptId));
        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Zhujiu });
        Answer(game, choice => choice.Cards.Count == 1);
        Answer(game, choice => choice.Id.Value.EndsWith(".use", StringComparison.Ordinal));
        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Jinglei } && prompt.Choices.Any(c => Action(c) == "activate"));
        Activate(game);
        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Xudai });
        var brew = Facts<OlSpLiuBeiZhujiuIssuedEvent>(game).Single();
        var paid = Facts<OlSpLiuBeiJingleiPaidEvent>(game).Single();
        Require(brew is { OwnerSeat: 0, AllClub: true, PaidCount: 1 } && paid is { OwnerSeat: 0 } && paid.AlcoholUseId == brew.UseId &&
            Facts<DamageAppliedEvent>(game).Single() is { SourceLess: true, Nature: DamageNature.Thunder, TargetSeat: 0, Amount: 1 },
            "The real Wine completion has paid exactly one source-less thunder damage before the Xudai child returns.");
        if (P(game)!.Choices.Any(choice => Action(choice) == "activate")) Activate(game);
        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Xudai } && prompt.Choices.Any(choice => choice.Targets.SequenceEqual([1])));
        Answer(game, choice => choice.Targets.SequenceEqual([1]));
        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Jinglei } && prompt.Choices.Any(choice => choice.Targets.SequenceEqual([1])));
        Require(Facts<SkillsAcquiredEvent>(game).Any(fact => fact.PlayerSeat == 1 && fact.SourceSkillId == Xudai && fact.SkillIds.Contains(Zhujiu)),
            "The damage child actually grants Zhujiu to the other living holder.");
        var holder = game.CreateSnapshot(1).Players[1];
        Require(holder is { Seat: 1, IsAlive: true } && holder.HandCount == holder.MaxHp + 1,
            $"The living Zhujiu holder must have one excess card before adjustment. Seat 1 has {holder.HandCount} hand cards and {holder.MaxHp} maximum HP.");
        Answer(game, choice => choice.Targets.SequenceEqual([1]));
        Reach(game, prompt => prompt.SkillPrompt?.SkillId == Jinglei || prompt.Kind == DecisionKind.PlayCard);
        var selection = P(game)!;
        Require(selection is { PlayerSeat: 1, IsPrivate: true, SkillPrompt.SkillId: Jinglei } &&
            selection.Choices.All(choice => choice.Parameters.GetValueOrDefault("stage") == "discard"),
            $"The holder must answer the actual adjustment discard. Saw {selection.Kind}/{selection.SkillPrompt?.SkillId} for seat {selection.PlayerSeat}.");
        var card = selection.Choices.First(choice => choice.Cards.Count == 1).Cards.Single();
        Answer(game, choice => choice.Cards.SequenceEqual([card]));
        Reach(game, prompt => prompt is { PlayerSeat: 2, SkillPrompt.SkillId: Claim } && prompt.Choices.Any(choice => Action(choice) == "activate"));
        var original = game.CardMovements.Single(move => move.Reason.Value == Discard);
        var frame = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(item => item.Id == paid.FrameId);
        Require(original.CardId == card && original.From == CardLocation.Hand(1) && original.To == CardLocation.DiscardPile &&
            frame.OlSpLiuBei664 is { Stage: "discarding", TargetSeat: 1, Minimum: 1 } draft && draft.Cards.SequenceEqual([card]) &&
            draft.PaidAdjustmentDiscards?.SequenceEqual([original]) == true && game.CreateSnapshot(1).Players[1].HandCount == holder.MaxHp,
            "The other holder's real adjustment creates the one original discard while Jinglei waits for its child.");

        var control = Cold(game, content);
        Skip(control);
        Settle(control);
        var given = control.CardMovements.Single(move => move.Reason.Value == Give);
        Require(given.CardId == card && given.From == CardLocation.DiscardPile && given.To == CardLocation.Hand(0) && given.Sequence > original.Sequence &&
            Facts<OlSpLiuBeiJingleiPaidEvent>(control).SequenceEqual([paid]) && control.CreateSnapshot(0).Players[0].Hp == ownerHp - 1,
            "The original entry really transfers once when the earlier claimant declines, after the one thunder payment.");

        Activate(game);
        Reach(game, prompt => prompt is { PlayerSeat: 2, SkillPrompt.SkillId: Claim } && prompt.Choices.Any(choice => choice.Cards.SequenceEqual([card])));
        var recovered = game.CardMovements.Single(move => move.CardId == card && move.Reason.Value == Take);
        Require(recovered.From == CardLocation.DiscardPile && recovered.To == CardLocation.Hand(2) && recovered.Sequence > original.Sequence,
            "The earlier claimant really takes the adjustment card before choosing the re-discard.");
        game = Cold(game, content);
        Answer(game, choice => choice.Cards.SequenceEqual([card]));
        Settle(game);
        var newer = game.CardMovements.Single(move => move.CardId == card && move.Reason.Value == Rediscard);
        Require(newer.From == CardLocation.Hand(2) && newer.To == CardLocation.DiscardPile && newer.BatchId != original.BatchId &&
            newer.Sequence > recovered.Sequence && Facts<OlSpLiuBeiJingleiPaidEvent>(game).SequenceEqual([paid]) &&
            Facts<DamageAppliedEvent>(game).Count() == 1 && game.CreateSnapshot(0).Players[0].Hp == ownerHp - 1 &&
            Facts<OlSpLiuBeiZhujiuReturnedEvent>(game).Single().UseId == brew.UseId,
            "The newer genuine discard and the original once-paid thunder/Wine finish independently after cold replay.");
        Require(!game.CardMovements.Any(move => move.Reason.Value == Give) &&
            game.CreateCardZoneDiagnostics().Single(item => item.CardId == card).Location == CardLocation.DiscardPile,
            $"Jinglei may not give discard entry {newer.Sequence} using the original adjustment at entry {original.Sequence}.");
    }

    private static string? Action(PromptChoice choice) => choice.Parameters.GetValueOrDefault("program-action");

    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) && Enumerable.Range(0, game.PlayerCount).All(seat =>
            SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold replay preserves all seat views and the exact paid movement entries.");
        return restored;
    }

    private static (GameEngine Game, ContentRegistry Content) Start()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new StandardOlSpLiuBei664Package(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 47, PlayerCount = 4, ModeId = Mode,
            HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, UseInteractiveSetup = true,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        Accept(game, new StartGameCommand());
        Reach(game, prompt => prompt.Kind is DecisionKind.SelectGeneral or DecisionKind.PlayCard);
        while (P(game) is { Kind: DecisionKind.SelectGeneral } selection)
        {
            Accept(game, new SelectGeneralCommand(selection.PlayerSeat, Generals[selection.PlayerSeat], game.Revision, selection.PromptId));
            Reach(game, prompt => prompt.Kind is DecisionKind.SelectGeneral or DecisionKind.PlayCard);
        }
        return (game, content);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ol-sp-liu-bei-jinglei-entry", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            var rules = $$$"""
                {"skills":[{"id":"{{{Claim}}}","revision":1,"triggers":[{"id":"claim-before-jinglei-give","window":"discardPileReceived",
                    "subject":"owner","discardOwnerScope":"other","movementOccurrence":"perCard","movementReasons":["{{{Discard}}}"],"optional":true,
                    "effects":[{"op":"claimMovedCards","target":"owner"},
                        {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},
                         "zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Claim] = new { name = "取回重弃", description = "先领取调手弃牌，再弃置一张手牌。" } } });
            var catalog = SkillProgramCatalog.LoadCurrent(rules, presentation);
            builder.AddSkill(new(Claim, catalog.Presentations[Claim].Name, catalog.Presentations[Claim].Description)
                { Program = catalog.Programs[Claim], ProgramPresentation = catalog.Presentations[Claim] });
            builder.AddGeneral(new(Generals[1], "持牌者", "supporter", "standard:none", "qun", 2));
            builder.AddGeneral(new(Generals[2], "领取者", "supporter", Claim, "qun", 8));
            builder.AddGeneral(new(Generals[3], "同桌", "supporter", "standard:none", "qun", 8));
            builder.AddDeck(new(Deck, "固定梅花闪", 3, 0, []) { PhysicalCards = Enumerable.Range(0, 32)
                .Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 7)).ToArray() });
            builder.AddMode(new(Mode, "惊雷原入堆", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}
