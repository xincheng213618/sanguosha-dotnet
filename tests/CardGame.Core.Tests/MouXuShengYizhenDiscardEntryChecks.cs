using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class MouXuShengYizhenDiscardEntryChecks
{
    private const string Hero = "mou:xu-sheng", Yizhen = "mou:yizhen";
    private const string Source = "fixture:yizhen-source", Driver = "fixture:yizhen-incoming", Reentry = "fixture:yizhen-reentry";
    private const string Mode = "identity:yizhen-discard-entry", Deck = "fixture:yizhen-discard-deck";
    private const string Payment = "skill-program.mou:yizhen.discard", Claim = "skill-program.mou:yizhen.claim";
    private const string Take = "skill-program.fixture:yizhen-reentry.ClaimMovedCards";
    private const string Rediscard = "skill-program.fixture:yizhen-reentry.SelectAndMoveOwnedCard";
    private static readonly string[] Generals = [Source, Hero, "fixture:yizhen-peer-2", "fixture:yizhen-peer-3"];

    internal static void YizhenRecoversOnlyItsOriginalSameColorPaymentsCold()
    {
        var (game, content) = Start();
        Accept(game, new UseProgramSkillCommand(0, Driver, "incoming", [], [1], game.Revision, P(game)!.PromptId));
        Reach(game, prompt => prompt is { PlayerSeat: 1, SkillPrompt.SkillId: Yizhen });
        Require(Facts<DamageAppliedEvent>(game).Any(fact => fact is { SourceSeat: 0, TargetSeat: 1, Amount: 1, SourceLess: false }),
            "Real sourced damage opens Yizhen for Xu Sheng.");
        Activate(game);
        Reach(game, prompt => prompt.PlayerSeat == 1 && YizhenFrame(game).MouXuSheng is { Stage: MouXuShengStage.Selecting, Cursor: 0 });
        var opposing = P(game)!.Choices.First().Cards.Single();
        Answer(game, choice => choice.Cards.SequenceEqual([opposing]));
        Reach(game, prompt => prompt.PlayerSeat == 0 && YizhenFrame(game).MouXuSheng is { Stage: MouXuShengStage.Selecting, Cursor: 1 });
        var own = P(game)!.Choices.First().Cards.Single();
        Require(Facts<MouXuShengDiscardPaidEvent>(game).Length == 0, "Neither opposing card moves until both participants have committed their private choices.");
        Answer(game, choice => choice.Cards.SequenceEqual([own]));

        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Reentry });
        var parent = YizhenFrame(game);
        var paid = Facts<MouXuShengDiscardPaidEvent>(game);
        Require(paid is [{ Chooser: 1, Holder: 0, IsRed: false }, { Chooser: 0, Holder: 1, IsRed: false }] &&
            paid.Select(fact => fact.CardId).SequenceEqual([opposing, own]) &&
            parent.MouXuSheng is { Stage: MouXuShengStage.DiscardChildren, Paid.Count: 2 } receipt &&
            receipt.Paid.Select(material => material.Sequence).SequenceEqual(paid.Select(fact => fact.Sequence)) &&
            game.CardMovements.Where(move => move.Reason.Value == Payment).Select(move => (long)move.Sequence).SequenceEqual(paid.Select(fact => fact.Sequence)),
            "Both selected black entities complete their original native discard before the second payment's child opens.");

        var control = Cold(game, content);
        Skip(control);
        Settle(control);
        Require(Claims(control).SequenceEqual([opposing, own]) &&
            control.CreateSnapshot(1).Players[1].Hand.Count(card => card.Id == own || card.Id == opposing) == 2,
            "Declining the earlier claim allows Yizhen to recover both original black cards into Xu Sheng's hand.");

        Activate(game);
        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Reentry } && prompt.Choices.Any(choice => choice.Cards.SequenceEqual([own])));
        var retrieved = game.CardMovements.Single(move => move.CardId == own && move.Reason.Value == Take);
        Require(retrieved.From == CardLocation.DiscardPile && retrieved.To == CardLocation.Hand(0) && retrieved.Sequence > paid[1].Sequence &&
            YizhenFrame(game).Id == parent.Id && Claims(game).Length == 0,
            "The second payment's real movement child takes that exact card while Yizhen still owns both payments.");
        game = Cold(game, content);
        Answer(game, choice => choice.Cards.SequenceEqual([own]));
        Settle(game);
        var later = game.CardMovements.Single(move => move.CardId == own && move.Reason.Value == Rediscard);
        Require(later.From == CardLocation.Hand(0) && later.To == CardLocation.DiscardPile && later.Sequence > retrieved.Sequence && later.BatchId != retrieved.BatchId,
            "The earlier claimant really discards the same entity a second time in its own batch.");
        Require(Claims(game).SequenceEqual([opposing]) &&
            game.CardMovements.Last(move => move.CardId == own) == later &&
            game.CreateSnapshot(1).Players[1].Hand.Any(card => card.Id == opposing),
            $"Yizhen must still recover the opposing-hand card and must not take later entry {later.Sequence} with original payment {paid[1].Sequence}.");
        _ = Cold(game, content);
    }

    private static ProgramSkillFrame YizhenFrame(GameEngine game) => game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Yizhen);
    private static T[] Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();
    private static int[] Claims(GameEngine game) => game.CardMovements.Where(move => move.Reason.Value == Claim &&
        move.From == CardLocation.DiscardPile && move.To == CardLocation.Hand(1)).Select(move => move.CardId).ToArray();

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) && Enumerable.Range(0, game.PlayerCount).All(seat =>
            SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))) &&
            Facts<MouXuShengDiscardPaidEvent>(restored).SequenceEqual(Facts<MouXuShengDiscardPaidEvent>(game)),
            "Accepted-command cold replay preserves the exact original payments, movement entries and every seat's view.");
        return restored;
    }

    private static (GameEngine Game, ContentRegistry Content) Start()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 47, PlayerCount = 4, ModeId = Mode, HumanSeat = 0,
            HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, UseInteractiveSetup = true,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        Accept(game, new StartGameCommand());
        Reach(game, prompt => prompt.Kind is DecisionKind.SelectGeneral or DecisionKind.PlayCard);
        while (P(game) is { Kind: DecisionKind.SelectGeneral } selection)
        {
            Accept(game, new SelectGeneralCommand(selection.PlayerSeat, Generals[selection.PlayerSeat], game.Revision, selection.PromptId));
            Reach(game, prompt => prompt.Kind is DecisionKind.SelectGeneral or DecisionKind.PlayCard);
        }
        Require(P(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }, "The real damage source starts the first Play phase.");
        return (game, content);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mou-xu-sheng-yizhen-discard-entry", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var rules = $$$"""
                {"skills":[{"id":"{{{Driver}}}","revision":1,"activations":[{"id":"incoming","minCards":0,"maxCards":0,
                    "minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":1,
                    "effects":[{"op":"damage","target":"selectedTarget","amount":1}]}]},
                  {"id":"{{{Reentry}}}","revision":1,"triggers":[{"id":"take-second-payment","window":"discardPileReceived",
                    "subject":"owner","discardOwnerScope":"other","movementOccurrence":"perCard","movementReasons":["{{{Payment}}}"],"optional":true,
                    "effects":[{"op":"claimMovedCards","target":"owner"},{"op":"awaitBoundCardMovements","target":"owner"},
                      {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},
                        "zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object>
                {
                    [Driver] = new { name = "实际伤害", description = "对一名其他角色造成1点伤害。" },
                    [Reentry] = new { name = "取回并再弃置", description = "获得对方弃置的牌，然后弃置一张手牌。" }
                } });
            var catalog = SkillProgramCatalog.LoadCurrent(rules, presentation);
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                    { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Source, "伤害来源", "supporter", Driver, "wei", 6, [Reentry]));
            foreach (var peer in Generals.Skip(2)) builder.AddGeneral(new(peer, "同桌", "supporter", "standard:none", "wei", 6));
            builder.AddDeck(new(Deck, "固定黑色手牌", 2, 0, []) { PhysicalCards = Enumerable.Range(0, 40)
                .Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 7)).ToArray() });
            builder.AddMode(new(Mode, "疑阵弃置原实体", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}
