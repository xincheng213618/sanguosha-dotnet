using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class OlLuShiZhuyanDrawChecks
{
    private const string Hero = "ol:lu-shi", Zhuyan = "ol:zhuyan";
    private const string Driver = "fixture:zhuyan-damage", Watch = "fixture:zhuyan-gain";
    private const string Peer = "fixture:zhuyan-peer", Deck = "fixture:zhuyan-deck";
    private const string DrawReason = "skill.ol.lushi.draw";

    internal static void OpeningHandCapAndActualEndingRecoveryCold()
    {
        const int owner = 0, peer = 1;
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var startingHand = Enumerable.Repeat(new ShanheStartingCard(CardKind.Dodge, Suit.Club, 2), 7).ToArray();
        var setup = new ShanheBattleSetup(53, 1, "zhuyan-original-draw", Hero, 3, 0, 0, 0, 0, [Driver, Watch],
            startingHand, [], [new(Peer, Hp: 6) { StartingHand = [] }], [], new string('c', 64)) { ExtraSkillSlots = 2 };
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = owner, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = true,
            AdvanceAfterHumanCommands = false, MaxTurns = 4
        }, registry);

        Submit(new StartGameCommand());
        Reach(IsPlay);
        var firstTurn = game.State.TurnNumber;
        var opening = Facts<OlLuShiSnapshotEvent>().Single(fact => fact.Seat == owner);
        Require(opening is { Initial: true, BoundaryId: 0, Hp: 3, HandCount: 7 } && Player() is { Hp: 3, HandCount: 7 },
            "The opening snapshot records seven real hand cards and three HP before Lu Shi's first actual turn.");

        DamageAndOpenZhuyan(discardCount: 5, handAfterDiscard: 2);
        Require(Prompt()!.Choices.Any(choice => IsAdjustment(choice, "hp")), "The HP adjustment is independently available.");
        Answer(choice => IsAdjustment(choice, "hand"));
        Reach(() => Prompt()?.SkillPrompt?.SkillId == Watch);
        var parent = Parent();
        var paid = parent.NativeOperationDrawReturns.Single();
        var handIssue = Facts<OlLuShiAdjustmentIssuedEvent>().Single();
        var actual = paid.Outcome;
        var moves = game.CardMovements.Where(move => move.Reason.Value == DrawReason).ToArray();
        Require(parent.OlLuShi is { Stage: OlLuShiStage.Children, Item: "hand", TargetSeat: owner, Amount: 3, Draw: null, Before: 0, After: 0 } &&
            handIssue.FrameId == parent.Id && handIssue.Snapshot == opening && handIssue is { Item: "hand", TargetSeat: owner } &&
            paid.Operation == SkillProgramEffectOp.ResolveOlLuShiZhuyan && paid.InstructionIndex == parent.InstructionIndex && paid.Reason.Value == DrawReason &&
            actual is { RecipientSeat: owner, RequestedCount: 3, Replacement: null } && actual.ActualDrawIds.Count == 3 &&
            moves.Select(move => move.CardId).SequenceEqual(actual.ActualDrawIds) && moves.All(move => move.From == CardLocation.DrawPile &&
                move.To == CardLocation.Hand(owner) && move.Sequence > actual.SequenceBefore && move.Sequence <= actual.SequenceAfter) &&
            Facts<OlLuShiMovementEvent>() is [var movement] && movement == new OlLuShiMovementEvent(parent.Id, owner, true, 3, actual.SequenceBefore, actual.SequenceAfter) &&
            game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Any(window => window.Batch.ParentFrameId == parent.Id &&
                window.Batch.Movements.Count > 0 && window.Batch.Movements.All(move => moves.Contains(move))) && Player().HandCount == 5,
            "The first actual Discard end uses the opening seven, requests three real cards to reach the cap and owns the paid gain child.");

        var gainPrompt = Prompt()!.PromptId;
        var receiptJson = JsonSerializer.Serialize(paid);
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Require(Prompt()?.PromptId == gainPrompt && Parent().Id == parent.Id &&
            JsonSerializer.Serialize(Parent().NativeOperationDrawReturns.Single()) == receiptJson && Player().HandCount == 5,
            "Cold restore preserves the original paid Draw, its three entities and their gain decision.");
        Answer(choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");

        Reach(IsPlay);
        var nextTurn = game.State.TurnNumber;
        var ending = Facts<OlLuShiSnapshotEvent>().Last(fact => fact.Seat == owner);
        Require(ending is { Initial: false, Hp: 2, HandCount: 5 } && ending.Turn == firstTurn && nextTurn > firstTurn &&
            Facts<ActualPhaseEndedEvent>().Any(fact => fact.BoundaryFrameId == ending.BoundaryId &&
                fact.Phase is { ActorSeat: owner, Kind: ActualDiscardRecoveryPhaseKind.Ending } && fact.Phase.TurnNumber == firstTurn) &&
            Facts<TurnStartedEvent>().Any(fact => fact.ActorSeat == peer && fact.TurnNumber > firstTurn && fact.TurnNumber < nextTurn),
            "Lu Shi's real Ending saves the post-gain hand and wounded HP before the intervening enemy turn.");

        DamageAndOpenZhuyan(discardCount: 4, handAfterDiscard: 1);
        Require(!Prompt()!.Choices.Any(choice => IsAdjustment(choice, "hand")) && Prompt()!.Choices.Any(choice => IsAdjustment(choice, "hp")),
            "The same target has spent its hand adjustment while its independent HP adjustment remains available.");
        var hpFrame = Parent();
        Answer(choice => IsAdjustment(choice, "hp"));
        Reach(() => Facts<ActualTurnCursorReturnedEvent>().Any(fact => fact.ActualTurn == nextTurn && fact.OwnerSeat == owner));

        var hpIssue = Facts<OlLuShiAdjustmentIssuedEvent>().Single(fact => fact.Item == "hp");
        Require(hpIssue.FrameId == hpFrame.Id && hpIssue.TargetSeat == owner && hpIssue.Snapshot == ending && Player() is { Hp: 2, HandCount: 1 } &&
            Facts<OlLuShiAdjustmentIssuedEvent>().Length == 2 && Facts<OlLuShiMovementEvent>().Length == 1 &&
            Facts<DamageAppliedEvent>().Count(fact => fact.TargetSeat == owner && fact.Amount == 1) == 2 &&
            Facts<RecoveryAppliedEvent>() is [{ SourceSeat: owner, TargetSeat: owner, Amount: 1 }] &&
            Facts<ProgramBindingResolvedEvent>().Count(fact => fact.SkillId == Zhuyan && fact.Window == SkillProgramTriggerWindow.DiscardPhaseEnded &&
                fact.Activated && fact.Completed) == 2 && game.CardMovements.Where(move => move.Reason.Value == DrawReason).SequenceEqual(moves),
            "The next original Discard end restores one real HP to the saved Ending value and completes each lifetime item once.");

        void DamageAndOpenZhuyan(int discardCount, int handAfterDiscard)
        {
            Submit(new UseProgramSkillCommand(owner, Driver, "damage-owner", [], [], game.Revision, Prompt()!.PromptId));
            Reach(IsPlay);
            Require(Player().Hp == handAfterDiscard, "The owner took real damage before discarding to the current HP limit.");
            Submit(new EndPlayPhaseCommand(owner, game.Revision, Prompt()!.PromptId));
            Reach(() => Prompt() is { Kind: DecisionKind.DiscardCards, PlayerSeat: owner });
            var discard = Prompt()!;
            var chosen = discard.ValidCardIds.Take(discardCount).ToArray();
            Require(discard.RequiredCardCount == discardCount && chosen.Length == discardCount, "The native discard quotes the expected number of physical cards.");
            Submit(new DiscardCardsCommand(owner, chosen, discard.PromptId, game.Revision));
            Reach(() => Prompt()?.SkillPrompt?.SkillId == Zhuyan && Prompt()!.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate"));
            var discarded = game.CardMovements.Where(move => move.TurnNumber == game.State.TurnNumber && move.Reason == CardMoveReasons.HandLimitDiscard &&
                move.From == CardLocation.Hand(owner) && move.To == CardLocation.DiscardPile).Select(move => move.CardId).ToHashSet();
            Require(Player().HandCount == handAfterDiscard && discarded.SetEquals(chosen), "The original Discard really moved the accepted hand entities before offering Zhuyan.");
            Answer(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
            Reach(() => Prompt()?.SkillPrompt?.SkillId == Zhuyan && Prompt()!.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "ol-lushi"));
            Require(Parent().WindowContext is { Window: SkillProgramTriggerWindow.DiscardPhaseEnded, SourceSeat: owner },
                "The adjustment belongs to Lu Shi's original actual Discard end.");
        }

        bool IsAdjustment(PromptChoice choice, string item) => choice.Targets.SequenceEqual([owner]) && choice.Parameters.GetValueOrDefault("item") == item;
        bool IsPlay() => Prompt() is { Kind: DecisionKind.PlayCard, PlayerSeat: owner };
        PendingDecision? Prompt() => game.CreateSnapshot(owner).PendingDecision;
        PlayerSnapshot Player() => game.CreateSnapshot(owner).Players[owner];
        ProgramSkillFrame Parent() => game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Zhuyan);
        T[] Facts<T>() => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();
        void Answer(Func<PromptChoice, bool> predicate)
        {
            var prompt = Prompt()!;
            Submit(new AnswerPromptCommand(owner, prompt.PromptId, prompt.Choices.Single(predicate).Id, game.Revision));
        }

        void Reach(Func<bool> ready)
        {
            for (var step = 0; step < 100 && !ready(); step++)
            {
                Require(Prompt() is null or { PlayerSeat: peer } && game.State.Winner == Winner.None, "Reached a different public decision.");
                Submit(new AdvanceOneStepCommand(game.Revision));
            }
            Require(ready(), "Did not reach the expected Zhuyan boundary.");
        }

        void Submit(GameCommand command)
        {
            var result = game.Submit(command);
            Require(result.Accepted, result.Error?.Message ?? "The native Zhuyan fixture command was rejected.");
        }

        void Require(bool condition, string message)
        {
            if (!condition)
                throw new InvalidOperationException($"{message} Turn {game.State.TurnNumber}/{game.State.Phase}, prompt {Prompt()?.Kind}/{Prompt()?.SkillPrompt?.SkillId}.");
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ol-zhuyan-draw", new(1, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,"activations":[{"id":"damage-owner","minCards":0,"maxCards":0,
                    "minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                    "effects":[{"op":"damage","target":"owner","amount":1}]}]},
                  {"id":"{{Watch}}","revision":1,"triggers":[
                    {"id":"watch-actual-gain","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch",
                      "movementReasons":["{{DrawReason}}"],"usageScope":"game","usageLimit":1,"optional":false,"isSkillInvocation":false,
                      "effects":[{"op":"chooseOption","target":"owner","resultBind":"seen","options":[{"id":"continue"}]}]}
                  ]}]}
                """, $$"""
                {"schemaVersion":{{SkillProgramCatalog.PresentationSchemaVersion}},"skills":{
                  "{{Driver}}":{"name":"自伤","description":"对自己造成1点实际伤害。"},
                  "{{Watch}}":{"name":"驻颜得牌","description":"停在实际得牌子窗。","optionLabels":{"continue":"继续"} } } }
                """);
            foreach (var (id, program) in catalog.Programs)
            {
                var shown = catalog.Presentations[id];
                builder.AddSkill(new(id, shown.Name, shown.Description) { Program = program, ProgramPresentation = shown });
            }
            builder.AddGeneral(new(Peer, "固定对手", "supporter", "standard:none", "wei", 6));
            builder.AddDeck(new(Deck, "固定闪牌堆", 0, 0, [new("standard:dodge", 24)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "驻颜实际弃牌与历史快照", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
