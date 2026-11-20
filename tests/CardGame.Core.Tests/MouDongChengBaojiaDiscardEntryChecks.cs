using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using static BoundaryLiDianChecks;

internal static class MouDongChengBaojiaDiscardEntryChecks
{
    private const string Hero = "mou:dong-cheng", Baojia = "mou:baojia";
    private const string Observer = "fixture:baojia-retire-lightning", Peer = "fixture:baojia-lightning-peer", Deck = "fixture:baojia-lightning-deck";
    private const string Relay = "fixture:baojia-transfer-lightning";
    private const string Reentry = "skill-program.fixture:baojia-retire-lightning.SelectAndMoveOwnedCard";
    private const string Claim = "skill-program.mou:baojia.delayed-claim";

    internal static void BaojiaClaimsOnlyTheOriginalLightningFinishEntry()
    {
        var (game, content) = Start();
        Reach(game, prompt => prompt.PlayerSeat == 0 && prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("branch") == "link"));
        Answer(game, choice => choice.Targets.SequenceEqual([1]));
        Settle(game);
        var lightning = game.CreateSnapshot(0).Players[0].Hand.Single(card => card.Kind == CardKind.Lightning).Id;
        var play = game.GetHumanLegalActions().Single(action => action.Kind == LegalActionKind.Lightning && action.CardId == lightning);
        Accept(game, new PlayCardCommand(0, lightning, play.TargetSeats, game.Revision, P(game)!.PromptId, play.PlayedCardKind));
        Settle(game);
        Require(game.CreateCardZoneDiagnostics().Single(card => card.CardId == lightning).Location == CardLocation.Judgment(0),
            "The opening physical Lightning is used normally and remains in its actual judgment area.");

        Accept(game, new EndPlayPhaseCommand(0, game.Revision, P(game)!.PromptId));
        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Baojia } &&
            prompt.Choices.Any(choice => IsAction(choice, "activate")));
        Activate(game);
        Reach(game, prompt => prompt.PlayerSeat == 0 && prompt.Choices.Any(IsArmorChoice));
        var judgment = game.ResolutionStack.OfType<JudgmentFrame>().Single(f => f.DelayedCardId == lightning);
        var protection = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Baojia);
        Require(judgment is { Continuation: JudgmentContinuationKind.Lightning, Succeeded: true, TargetSeat: 0 } &&
                protection.MouDongCheng is { Stage: MouDongChengStage.Slot, Occurrence.First: true } pay &&
                pay.Occurrence!.AttackId == judgment.Id && pay.Occurrence.CardId == lightning,
            "Baojia is offered against the successful original physical Lightning and its own pending damage.");
        Answer(game, IsArmorChoice);
        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Observer } &&
            prompt.Choices.Any(choice => IsAction(choice, "activate")));
        Require(game.ResolutionStack.OfType<JudgmentFrame>().Any(f => f.Id == judgment.Id && f.DelayedCardId == lightning &&
                f.MouDongChengClaims?.Any(claim => claim.ProtectionFrameId == protection.Id && !claim.Settled) == true) &&
                game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Any(frame => frame.Batch.ParentFrameId == protection.Id &&
                    frame.Batch.Movements.Any(move => move.From == CardLocation.Equipment(0) && move.To == CardLocation.DiscardPile &&
                        move.Reason.Value == "equipment.slot-abolished")),
            "The original paid Baojia slot holds the live judgment while its real equipment departure child is pending.");

        var positive = Cold(game, content);
        Answer(positive, choice => IsAction(choice, "skip"));
        Settle(positive);
        var nativeFinish = positive.CardMovements.Single(move => move.CardId == lightning && move.Reason == CardMoveReasons.DelayedCardFinish);
        var positiveClaim = positive.CardMovements.Single(move => move.CardId == lightning && move.Reason.Value == Claim);
        Require(nativeFinish.From == CardLocation.Judgment(0) && nativeFinish.To == CardLocation.DiscardPile &&
                positiveClaim.Sequence > nativeFinish.Sequence && positiveClaim.From == nativeFinish.To && positiveClaim.To == CardLocation.Hand(0) &&
                Settled(positive, judgment.Id, protection.Id) is { Actual: 1 },
            "Baojia obtains the entity after the original physical Lightning finishes into the discard pile.");

        Activate(game);
        Reach(game, prompt => Selects(prompt, lightning, CardZoneKind.Judgment));
        Answer(game, choice => IsSelection(choice, lightning, CardZoneKind.Judgment));
        Reach(game, prompt => Selects(prompt, lightning, CardZoneKind.Hand));
        var lifted = game.CardMovements.Single(move => move.CardId == lightning && move.From == CardLocation.Judgment(0) && move.Reason.Value == Reentry);
        Require(lifted.To == CardLocation.Hand(0) && !game.Events.Any(e => e.Payload is MouDcDelayedClaimSettledEvent),
            "The paid equipment child physically lifts the exact Lightning before the original judgment settles.");
        game = Cold(game, content);
        Answer(game, choice => IsSelection(choice, lightning, CardZoneKind.Hand));
        Settle(game);

        var later = game.CardMovements.Single(move => move.CardId == lightning && move.From == CardLocation.Hand(0) && move.Reason.Value == Reentry);
        Require(later.To == CardLocation.DiscardPile && later.Sequence > lifted.Sequence && later.BatchId != lifted.BatchId &&
                game.CardMovements.All(move => move.CardId != lightning || move.Reason != CardMoveReasons.DelayedCardFinish) &&
                game.Events.Select(e => e.Payload).OfType<MouDcSlotPaidEvent>().Count(e => e.FrameId == protection.Id) == 1 &&
                game.CreateSnapshot(0).Players[0].Hp == 3 && game.CreateSnapshot(0).Players[0].EquipmentSlotCapacities!.GetValueOrDefault(EquipmentSlot.Armor, 1) == 0,
            "The same entity reenters from hand in a different batch before native completion, and the original armor payment still prevents the damage.");
        Require(Settled(game, judgment.Id, protection.Id) is { Actual: 0 } &&
                game.CardMovements.All(move => move.Reason.Value != Claim) &&
                game.CreateCardZoneDiagnostics().Single(card => card.CardId == lightning).Location == CardLocation.DiscardPile,
            "Baojia cannot take the later hand-discard entry through the original Lightning settlement.");
    }

    internal static void BaojiaClaimsTheOriginalDeathDiscardAfterChainedLightning()
    {
        var (game, content) = Start(chainDeath: true);
        Reach(game, prompt => prompt.PlayerSeat == 0 && prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("branch") == "link"));
        Answer(game, choice => choice.Targets.SequenceEqual([2]));
        Settle(game);
        var lightning = game.CreateSnapshot(0).Players[0].Hand.Single(card => card.Kind == CardKind.Lightning).Id;
        var lightningUse = game.GetHumanLegalActions().Single(action => action.Kind == LegalActionKind.Lightning && action.CardId == lightning);
        Accept(game, new PlayCardCommand(0, lightning, lightningUse.TargetSeats, game.Revision, P(game)!.PromptId, lightningUse.PlayedCardKind));
        Settle(game);
        Accept(game, new UseProgramSkillCommand(0, Relay, "transfer-judgment", [], [1], game.Revision, P(game)!.PromptId));
        Reach(game, prompt => Selects(prompt, lightning, CardZoneKind.Judgment));
        Answer(game, choice => IsSelection(choice, lightning, CardZoneKind.Judgment));
        Settle(game);
        var moved = game.CardMovements.Single(move => move.CardId == lightning && move.To == CardLocation.Judgment(1));
        Require(moved.From == CardLocation.Judgment(0), "The physical Lightning changes owner through a real judgment-area transfer.");

        var chain = game.GetHumanLegalActions().First(action => action.Kind == LegalActionKind.IronChain &&
            action.TargetSeats.Count == 2 && action.TargetSeats.Contains(0) && action.TargetSeats.Contains(1));
        Accept(game, new PlayCardCommand(0, chain.CardId!.Value, chain.TargetSeats, game.Revision, P(game)!.PromptId, chain.PlayedCardKind));
        Settle(game);
        Require(game.CreateSnapshot(0).Players.Take(2).All(player => player.IsChained), "The original Lightning host and Baojia owner are both actually chained.");
        Accept(game, new EndPlayPhaseCommand(0, game.Revision, P(game)!.PromptId));
        ReachChainedProtection(game);

        var judgment = game.ResolutionStack.OfType<JudgmentFrame>().Single(f => f.DelayedCardId == lightning);
        var deathEntry = game.CardMovements.Single(move => move.CardId == lightning && move.Reason == CardMoveReasons.DeathDiscard);
        var snapshot = game.CreateSnapshot(0);
        Require(!snapshot.Players[1].IsAlive && snapshot.Players[0].IsAlive && snapshot.Players[0].Hp == 3 &&
                judgment is { Continuation: JudgmentContinuationKind.Lightning, Succeeded: true, TargetSeat: 1 } &&
                deathEntry.From == CardLocation.Judgment(1) && deathEntry.To == CardLocation.DiscardPile && deathEntry.Sequence > moved.Sequence &&
                game.Events.Select(e => e.Payload).OfType<ChainedDamagePropagatedEvent>().Any(e =>
                    e.ResolutionId == judgment.Id && e.FromSeat == 1 && e.TargetSeat == 0),
            "The physical Lightning really kills its judgment host, enters the pile in death cleanup and propagates to the living owner.");
        Activate(game);
        Reach(game, prompt => prompt.PlayerSeat == 0 && prompt.Choices.Any(IsArmorChoice));
        var protection = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Baojia);
        Require(protection.MouDongCheng is { Stage: MouDongChengStage.Slot, Occurrence: { First: true, Target: 0 } occurrence } &&
                occurrence.AttackId == judgment.Id && occurrence.CardId == lightning,
            "Baojia protects its still-living owner from the propagated damage of the original physical judgment.");
        game = Cold(game, content);
        Answer(game, IsArmorChoice);
        Settle(game);

        var gain = game.CardMovements.Single(move => move.CardId == lightning && move.Reason.Value == Claim);
        Require(gain.From == deathEntry.To && gain.To == CardLocation.Hand(0) && gain.Sequence > deathEntry.Sequence &&
                game.CardMovements.Where(move => move.CardId == lightning && move.Sequence > deathEntry.Sequence).SequenceEqual([gain]) &&
                Settled(game, judgment.Id, protection.Id) is { Actual: 1 } && game.CreateSnapshot(0).Players[0].Hp == 3 &&
                game.CreateSnapshot(0).Players[0].EquipmentSlotCapacities!.GetValueOrDefault(EquipmentSlot.Armor, 1) == 0,
            "After cold recovery, the paid chained protection obtains the still-current original death discard exactly once.");
    }

    private static void ReachChainedProtection(GameEngine game)
    {
        for (var step = 0; step < 120; step++)
        {
            if (P(game) is { PlayerSeat: 0 } prompt)
            {
                if (prompt.SkillPrompt?.SkillId == Baojia && prompt.Choices.Any(choice => IsAction(choice, "activate")))
                    return;
                Require(prompt.Kind == DecisionKind.RescueDying, "Only the original Lightning host's dying decision may precede the chained protection.");
                Answer(game, choice => choice.Parameters.GetValueOrDefault("response") is "pass" or "let-die");
            }
            else
                Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The original Lightning did not reach its chained protection window.");
    }

    private static MouDcDelayedClaimSettledEvent Settled(GameEngine game, long judgment, long protection) =>
        game.Events.Select(e => e.Payload).OfType<MouDcDelayedClaimSettledEvent>().Single(e => e.JudgmentId == judgment && e.ProtectionId == protection);

    private static bool IsArmorChoice(PromptChoice choice) => choice.Parameters.GetValueOrDefault("slot") == nameof(EquipmentSlot.Armor);
    private static bool IsAction(PromptChoice choice, string action) => choice.Parameters.GetValueOrDefault("program-action") == action;

    private static bool Selects(PendingDecision prompt, int cardId, CardZoneKind zone) =>
        prompt.PlayerSeat == 0 && prompt.Choices.Any(choice => IsSelection(choice, cardId, zone));

    private static bool IsSelection(PromptChoice choice, int cardId, CardZoneKind zone) =>
        IsAction(choice, "select-and-move-owned-card") && choice.Cards.SequenceEqual([cardId]) &&
        choice.Parameters.GetValueOrDefault("source-zone") == zone.ToString();

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) && Enumerable.Range(0, 4).All(seat =>
                SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold replay preserves the actual pending judgment, paid slot, movement entries and observer choices.");
        return restored;
    }

    private static (GameEngine Game, ContentRegistry Content) Start(bool chainDeath = false)
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var others = Enumerable.Range(0, 3).Select(index => new ShanheCombatantDefinition(Peer, Hp: chainDeath && index == 0 ? 1 : 8) { StartingHand = [] }).ToArray();
        var setup = new ShanheBattleSetup(7, 1, "baojia-original-lightning-entry", Hero, 3, 0, 0, 0, 0, chainDeath ? [Relay] : [Observer],
            chainDeath ? [new(CardKind.Lightning, Suit.Spade, 1), new(CardKind.IronChain, Suit.Club, 2)] : [new(CardKind.Lightning, Suit.Spade, 1)],
            chainDeath ? [] : [new(CardKind.BaguaFormation, Suit.Spade, 2)], others, [], new string('e', 64));
        var game = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0,
            HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        Accept(game, new StartGameCommand());
        return (game, content);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-baojia-lightning-entry", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var rules = $$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
                  {"id":"{{{Observer}}}","revision":1,"triggers":[{"id":"after-slot-payment","window":"cardsMoved","subject":"owner",
                    "sourceZones":["equipment"],"movementReasons":["equipment.slot-abolished"],"movementOccurrence":"perBatch","optional":true,"effects":[
                    {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},"zones":["judgment"],
                      "cardKinds":["lightning"],"count":1,"destination":"ownerHand","awaitMovementTriggers":true},
                    {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},"zones":["hand"],
                      "cardKinds":["lightning"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]}]},
                  {"id":"{{{Relay}}}","revision":1,"activations":[{"id":"transfer-judgment","minCards":0,"maxCards":0,
                    "minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":1,"effects":[
                    {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},"zones":["judgment"],
                      "cardKinds":["lightning"],"count":1,"destination":"selectedTargetCorrespondingZone","targetRef":{"kind":"selectedTarget"},"awaitMovementTriggers":true}]}]}
                ]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object>
                {
                    [Observer] = new { name = "移走闪电", description = "废栏后取得判定区的实体闪电，再将其弃置。" },
                    [Relay] = new { name = "转移闪电", description = "将实体闪电移入另一名角色的判定区。" }
                } });
            var catalog = SkillProgramCatalog.Load(rules, presentation);
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                    { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Peer, "实际同桌", "supporter", "standard:none", "qun", 8));
            builder.AddDeck(new(Deck, "固定黑桃五闪", 0, 2, []) { PhysicalCards = Enumerable.Range(0, 64)
                .Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, 5)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "保驾原闪电入堆", 4, 4, new Dictionary<string, int>(), Deck, GeneralPoolIds: [Hero, Peer],
                ModeKind: ContentModeKind.Team, TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}
