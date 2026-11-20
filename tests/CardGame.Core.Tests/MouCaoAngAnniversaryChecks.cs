using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

using static BoundaryLiDianChecks;

internal static class MouCaoAngAnniversaryChecks
{
    private const string Hero = "mou:cao-ang", Fengmin = "mou:fengmin";
    private const string Peer = "fixture:fengmin-peer", Watch = "fixture:fengmin-gain", Deck = "fixture:fengmin-deck";
    private const string DrawReason = "skill-program.mou:fengmin.draw";

    internal static void ForeignEquipmentLossDrawsForEmptySlotsOnceCold()
    {
        const int owner = 0, emptySlotsAfterFirstLoss = 4;
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var peer = new ShanheCombatantDefinition(Peer) { StartingHand = [] };
        var equipped = peer with
        {
            StartingEquipment = [new(CardKind.Crossbow, Suit.Spade, 1), new(CardKind.OffensiveHorse, Suit.Heart, 5)]
        };
        var setup = new ShanheBattleSetup(7, 1, "mou-cao-ang-fengmin-native", Hero, 4, 0, 0, 0, 0, [Watch],
            [new(CardKind.Dismantlement, Suit.Spade, 3), new(CardKind.Dismantlement, Suit.Spade, 4)], [],
            [equipped, peer], [peer], new string('e', 64)) { ExtraSkillSlots = 1 };
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = owner, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
            ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4
        }, registry);

        Accept(game, new StartGameCommand());
        Settle(game);
        var turn = game.State.TurnNumber;
        var target = game.CreateSnapshot(owner).Players.Single(player => player.Seat != owner && player.Equipment.Count == 2).Seat;
        var weapon = Player(target).Equipment.Single(card => card.Kind == CardKind.Crossbow).Id;
        var horse = Player(target).Equipment.Single(card => card.Kind == CardKind.OffensiveHorse).Id;
        var dismantlements = Player(owner).Hand.Where(card => card.Kind == CardKind.Dismantlement).OrderBy(card => card.Rank).ToArray();
        Dismantle(dismantlements[0].Id, weapon);
        Reach(game, IsGain);

        var paid = Facts<MouCaFengminPaidEvent>().Single();
        var returned = Facts<MouCaDrawReturnedEvent>().Single();
        var parent = Root();
        var pending = P(game)!.PromptId;
        var lossWindow = game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Single(frame => frame.Id == paid.BatchId);
        var gainWindow = game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Single(frame => frame.Batch.ParentFrameId == parent.Id);
        var lost = lossWindow.Batch.Movements.Single(move => move.From == CardLocation.Equipment(target) && move.CardId == weapon);
        var draws = Draws();
        var drawnIds = draws.Select(move => move.CardId).ToArray();
        var receipt = parent.NativeOperationDrawReturns.Single();
        Require(paid is { Owner: owner, TurnOwner: owner, Count: emptySlotsAfterFirstLoss } && paid.Loser == target && paid.Turn == turn &&
            paid.FrameId == parent.Id && parent.WindowContext?.ParentFrameId == lossWindow.Id &&
            parent.MouCaoAng is { Stage: MouCaoAngStage.DrawChildren, Requested: emptySlotsAfterFirstLoss } &&
            lost.To == CardLocation.Processing && Player(target).Equipment.Select(card => card.Id).SequenceEqual([horse]) &&
            receipt is { InstructionIndex: 1, Operation: SkillProgramEffectOp.ResolveMouCaoAngFengmin } && receipt.Reason.Value == DrawReason &&
            receipt.Outcome is { RecipientSeat: owner, RequestedCount: emptySlotsAfterFirstLoss, Replacement: null } &&
            receipt.Outcome.SequenceBefore >= lost.Sequence && draws.Length == emptySlotsAfterFirstLoss && drawnIds.Distinct().Count() == emptySlotsAfterFirstLoss &&
            receipt.Outcome.ActualDrawIds.SequenceEqual(drawnIds) && gainWindow.Batch.Movements.Count > 0 &&
            gainWindow.Batch.Movements.All(move => drawnIds.Contains(move.CardId)) &&
            draws.All(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(owner) &&
                move.Sequence > receipt.Outcome.SequenceBefore && move.Sequence <= receipt.Outcome.SequenceAfter) &&
            returned is { Requested: emptySlotsAfterFirstLoss, Actual: emptySlotsAfterFirstLoss, Replacement: 0 } && returned.FrameId == parent.Id &&
            !Facts<MouCaCompletedEvent>().Any(fact => fact.FrameId == parent.Id),
            "The first actual Dismantlement leaves the foreign horse equipped and pays four physical cards to Cao Ang before the original observer returns.");

        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Require(P(game)?.PromptId == pending && Root().Id == parent.Id && Root().NativeOperationDrawReturns.Single().Outcome.ActualDrawIds.SequenceEqual(drawnIds) &&
            game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Any(frame => frame.Id == gainWindow.Id) &&
            Facts<MouCaFengminPaidEvent>().SequenceEqual([paid]) && Draws().SequenceEqual(draws),
            "Cold replay preserves the original foreign loss, completed draw and exact gain observer without paying twice.");
        for (var paidWindow = 0; paidWindow < draws.Length; paidWindow++)
        {
            Reach(game, prompt => IsGain(prompt) || prompt is { PlayerSeat: owner, Kind: DecisionKind.PlayCard });
            if (!IsGain(P(game)!)) break;
            Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
        }
        Settle(game);

        Dismantle(dismantlements[1].Id, horse);
        Reach(game, prompt => IsGain(prompt) || prompt is { PlayerSeat: owner, Kind: DecisionKind.PlayCard });
        Require(P(game) is { PlayerSeat: owner, Kind: DecisionKind.PlayCard } && game.State.TurnNumber == turn && Player(target).Equipment.Count == 0 &&
            game.CardMovements.Count(move => move.CardId == horse && move.From == CardLocation.Equipment(target) && move.To == CardLocation.Processing) == 1 &&
            game.CreateCardZoneDiagnostics().Where(card => card.CardId == weapon || card.CardId == horse).All(card => card.Location == CardLocation.DiscardPile) &&
            Facts<MouCaFengminPaidEvent>().SequenceEqual([paid]) && Facts<MouCaDrawReturnedEvent>().SequenceEqual([returned]) &&
            Facts<MouCaCompletedEvent>().Count(fact => fact.FrameId == parent.Id) == 1 && Draws().SequenceEqual(draws),
            "The second native equipment loss opens the fifth slot in the same Play phase without another Fengmin payment or draw.");

        PlayerSnapshot Player(int seat) => game.CreateSnapshot(seat).Players[seat];
        T[] Facts<T>() => game.Events.Select(item => item.Payload).OfType<T>().ToArray();
        ProgramSkillFrame Root() => game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Fengmin);
        CardMovementRecord[] Draws() => game.CardMovements.Where(move => move.Reason.Value == DrawReason).ToArray();
        bool IsGain(PendingDecision prompt) => prompt.PlayerSeat == owner && prompt.SkillPrompt?.SkillId == Watch;
        void Dismantle(int cardId, int equipmentId)
        {
            var action = game.GetHumanLegalActions().Single(entry => entry is { Kind: LegalActionKind.Dismantlement, ConversionSource: null } &&
                entry.CardId == cardId && entry.TargetCardId == equipmentId && entry.TargetSeats.SequenceEqual([target]));
            Accept(game, new PlayCardCommand(owner, cardId, action.TargetSeats, game.Revision, P(game)!.PromptId, action.PlayedCardKind, equipmentId));
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mou-cao-ang-fengmin", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Watch}}","revision":1,"triggers":[
                    {"id":"paid-fengmin","window":"cardsGained","subject":"owner","destinationZones":["hand"],
                     "movementReasons":["{{DrawReason}}"],"movementOccurrence":"perBatch","optional":false,"isSkillInvocation":false,
                     "effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]}
                ]}]}
                """, $$"""
                {"schemaVersion":{{SkillProgramCatalog.PresentationSchemaVersion}},"skills":{"{{Watch}}":{
                    "name":"丰愍得牌","description":"在丰愍的实体牌到账后暂停。","optionLabels":{"continue":"继续"} } } }
                """);
            builder.AddSkill(new(Watch, catalog.Presentations[Watch].Name, catalog.Presentations[Watch].Description)
                { Program = catalog.Programs[Watch], ProgramPresentation = catalog.Presentations[Watch] });
            builder.AddGeneral(new(Peer, "固定同桌", "", "standard:none", "wei", 8));
            builder.AddDeck(new(Deck, "固定闪牌堆", 0, 2, [new("standard:dodge", 40)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "丰愍原生装备失去", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
