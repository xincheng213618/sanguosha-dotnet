using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class NativePreDiscardEntryOwnerChecks
{
    private const string Hero = "mobile:lu-meng", Keji = "mobile:keji";
    private const string HuaTuo = "anniversary:shen-hua-tuo", Jingyu = "anniversary-god:jingyu";
    private const string Watch = "fixture:keji-invocation-loss", Peer = "fixture:keji-discard-peer";
    private const string Deck = "fixture:keji-discard-deck", JingyuDraw = "skill.shen-hua-tuo.jingyu.draw";

    internal static void AcceptedKejiSkipsFromOriginalEntryAfterRealSkillLossCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardActiveSkillExpansionPackage(),
            new StandardRescueSkillExpansionPackage(), new Fixture());
        var starting = Enumerable.Range(2, 5).Select(rank => new ShanheStartingCard(CardKind.Dodge, Suit.Heart, rank)).ToArray();
        var setup = new ShanheBattleSetup(31, 1, "keji-native-discard-entry", Hero, 4, 0, 0, 0, 0, [Jingyu, Watch],
            starting, [], [new(Peer, Hp: 8) { StartingHand = [] }], [], new string('d', 64)) { ExtraSkillSlots = 2 };
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
            ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false,
            UseInteractiveDiscard = true, AdvanceAfterHumanCommands = false, MaxTurns = 4
        }, registry);

        PendingDecision? Pending() => game.CreateSnapshot(0).PendingDecision;
        PlayerSnapshot Owner() => game.CreateSnapshot(0).Players[0];
        T[] Facts<T>() where T : IGameEvent => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();
        void Need(bool condition, string message)
        {
            if (!condition)
                throw new InvalidOperationException($"{message} Turn {game.State.TurnNumber}, phase {game.State.Phase}, prompt " +
                    $"{Pending()?.Kind}/{Pending()?.SkillPrompt?.SkillId}, recent facts {string.Join(',', game.Events.TakeLast(8).Select(entry => entry.Payload.GetType().Name))}.");
        }
        void Submit(GameCommand command)
        {
            var result = game.Submit(command);
            Need(result.Accepted, result.Error?.Message ?? "The native discard entry fixture command was rejected.");
        }
        void Reach(Func<bool> ready)
        {
            for (var step = 0; step < 100 && !ready(); step++)
            {
                Need(Pending() is null && game.State.Winner == Winner.None, "The original native entry reached an unexpected prompt.");
                Submit(new AdvanceOneStepCommand(game.Revision));
            }
            Need(ready(), "The original native entry missed its required boundary.");
        }
        void Answer(Func<PromptChoice, bool> pick)
        {
            var prompt = Pending()!;
            Submit(new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices.Single(pick).Id, game.Revision));
        }
        bool EnteredDiscard(int turn) => Facts<ActualDiscardRecoveryPhaseStartedEvent>().Any(entry =>
            entry.Phase is { ActorSeat: 0, Kind: ActualDiscardRecoveryPhaseKind.Discard } && entry.Phase.TurnNumber == turn) ||
            Facts<PhaseChangedEvent>().Any(entry => entry is { ActorSeat: 0, Phase: TurnPhase.Discard });

        Submit(new StartGameCommand());
        Reach(() => Pending() is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        var turn = game.State.TurnNumber;
        var originalHand = Owner().Hand.Select(card => card.Id).ToHashSet();
        Need(Owner() is { Hp: 4, HandCount: 5 } && Owner().Skills?.Any(skill => skill.ContentId == Keji) == true &&
            Facts<ActualDiscardRecoveryPhaseStartedEvent>().Any(entry => entry.Phase.TurnNumber == turn &&
                entry.Phase is { ActorSeat: 0, Kind: ActualDiscardRecoveryPhaseKind.Play }),
            "The real mobile Keji holder has an actual Play phase and a physical hand above the ordinary limit.");
        Submit(new EndPlayPhaseCommand(0, game.Revision, Pending()!.PromptId));
        Reach(() => Pending() is { Kind: DecisionKind.SkipDiscardPolicy, PlayerSeat: 0 });
        var entry = game.ResolutionStack.OfType<NativeDiscardEntryFrame>().Single();
        Need(entry is { Stage: NativeDiscardEntryStage.Choosing, BeforePhase: TurnPhase.Play, TurnOwnerSeat: 0 } &&
            entry.Source is { SkillId: Keji, OwnerSeat: 0, BindingId: "before-discard-without-play-slash" } &&
            game.State.Phase == TurnPhase.Play && entry.TurnNumber == turn && entry.PromptId == Pending()!.PromptId &&
            !EnteredDiscard(turn) && Facts<NativeDiscardEntryPolicyChosenEvent>().Length == 0,
            "The actual choice lives on the original Play boundary before any Discard entry or payment fact.");

        var beforeChoice = game;
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(beforeChoice.CreateCheckpoint())), registry);
        Answer(choice => choice.Parameters.GetValueOrDefault("action") == "discard-policy-skip");
        Reach(() => Pending() is { Kind: DecisionKind.DiscardCards, PlayerSeat: 0 });
        Need(Facts<NativeDiscardEntryPolicyChosenEvent>().SequenceEqual([new(entry.Id, entry.Source, entry.GameplayHash,
                turn, 0, TurnPhase.Play, entry.PlayPhaseInstanceId, Applied: false)]) &&
            Facts<ActualDiscardRecoveryPhaseStartedEvent>().Count(fact => fact.Phase.TurnNumber == turn &&
                fact.Phase is { ActorSeat: 0, Kind: ActualDiscardRecoveryPhaseKind.Discard }) == 1 &&
            game.ResolutionStack.OfType<ManualDiscardFrame>().Single() is { OwnerSeat: 0, InitialRequiredCount: 1 } &&
            !game.ResolutionStack.OfType<NativeDiscardEntryFrame>().Any() && game.CardMovements.SequenceEqual(beforeChoice.CardMovements),
            "Declining the restored original choice enters Discard once and requests its normal one-card payment.");
        game = beforeChoice;

        Answer(choice => choice.Parameters.GetValueOrDefault("action") == "discard-policy-use");
        Reach(() => Pending()?.SkillPrompt?.SkillId == Watch);
        var heldEntry = game.ResolutionStack.OfType<NativeDiscardEntryFrame>().Single();
        var observer = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Watch);
        var invocation = Facts<SkillInvocationStartedEvent>().Single(fact => fact.ParentFrameId == entry.Id);
        var paid = Facts<NativeDiscardEntryPolicyChosenEvent>().Single();
        var lost = Facts<SkillStateChangedEvent>().Single(fact => fact.SkillId == Keji);
        var drawn = game.CardMovements.Single(move => move.Reason.Value == JingyuDraw &&
            move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0));
        Need(paid == new NativeDiscardEntryPolicyChosenEvent(entry.Id, entry.Source, entry.GameplayHash, turn, 0,
                TurnPhase.Play, entry.PlayPhaseInstanceId, Applied: true) &&
            heldEntry is { Stage: NativeDiscardEntryStage.Skipping } && heldEntry.Id == entry.Id && heldEntry.Source == entry.Source &&
            invocation is { ActorSeat: 0, SkillKey: Keji, ReturnKind: SkillInvocationReturnKind.RuntimeFrame } &&
            invocation.SkillInstanceId == entry.Source.SkillInstanceId &&
            game.ResolutionStack.OfType<SkillInvocationFrame>().Any(frame => frame.Id == invocation.FrameId) &&
            lost is { Seat: 0, Action: SkillStateAction.Lose } && lost.Origin?.FrameId == observer.Id &&
            Facts<CharacterSkillsLostEvent>().Any(fact => fact.Seat == 0 && fact.SkillIds.SequenceEqual([Keji])) &&
            Owner().Skills?.All(skill => skill.ContentId != Keji) == true && !EnteredDiscard(turn),
            "A real Jingyu draw beneath the accepted Keji invocation loses exactly Keji while that same paid skip is still pending.");

        var before = game;
        var childPrompt = Pending()!.PromptId;
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(before.CreateCheckpoint())), registry);
        Need(Enumerable.Range(0, 2).All(seat => SnapshotJson.Serialize(game.CreateSnapshot(seat)) ==
                SnapshotJson.Serialize(before.CreateSnapshot(seat))) && game.CardMovements.SequenceEqual(before.CardMovements) &&
            Pending()!.PromptId == childPrompt && Facts<NativeDiscardEntryPolicyChosenEvent>().SequenceEqual([paid]) &&
            Facts<SkillStateChangedEvent>().Single(fact => fact.SkillId == Keji) == lost &&
            JsonSerializer.Serialize(game.ResolutionStack.OfType<NativeDiscardEntryFrame>().Single()) == JsonSerializer.Serialize(heldEntry),
            "Cold replay preserves the accepted original entry, the loss proven beneath it and the open invocation child.");

        Answer(choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
        Reach(() => Facts<TurnEndedEvent>().Any(fact => fact.ActorSeat == 0 && fact.TurnNumber == turn));
        Need(Facts<ActualPhaseSkippedEvent>().Count(fact => fact.TurnNumber == turn &&
                fact is { Kind: ActualDiscardRecoveryPhaseKind.Discard, ActorSeat: 0, ActualTurnOwnerSeat: 0 }) == 1 &&
            Facts<SkillInvocationCompletedEvent>().Count(fact => fact.FrameId == invocation.FrameId && fact.ParentFrameId == entry.Id) == 1 &&
            Facts<NativeDiscardEntryPolicyChosenEvent>().SequenceEqual([paid]) &&
            !game.ResolutionStack.OfType<NativeDiscardEntryFrame>().Any() && !EnteredDiscard(turn) &&
            !game.CardMovements.Any(move => move.TurnNumber == turn && move.From == CardLocation.Hand(0) && move.To == CardLocation.DiscardPile) &&
            originalHand.Append(drawn.CardId).ToHashSet().SetEquals(Owner().Hand.Select(card => card.Id)),
            "The original paid skip finishes once, enters no Discard phase and leaves all original cards plus the actual Jingyu draw.");
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new StandardClassicGeneralPackage().Manifest;

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero, HuaTuo);
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Watch}}","revision":1,"triggers":[
                  {"id":"lose-after-jingyu","window":"cardsGained","subject":"owner","destinationZones":["hand"],
                    "movementOccurrence":"perBatch","movementReasons":["{{JingyuDraw}}"],"optional":false,"isSkillInvocation":false,
                    "condition":{"kind":"expression","expr":[">",["phaseCount","now","owner","play","entered"],0]},
                    "effects":[{"op":"skillState","target":"owner","action":"lose","match":{"skillIds":["{{Keji}}"]},"until":[]},
                      {"op":"chooseOption","target":"owner","resultBind":"continue","options":[{"id":"continue"}]}]}
                ]}]}
                """, $$"""
                {"schemaVersion":{{SkillProgramCatalog.PresentationSchemaVersion}},"skills":{"{{Watch}}":{
                  "name":"静域后失去克己","description":"静域实际摸牌后失去克己并暂停。","optionLabels":{"continue":"继续"} } } }
                """);
            builder.AddSkill(new(Watch, catalog.Presentations[Watch].Name, catalog.Presentations[Watch].Description)
                { Program = catalog.Programs[Watch], ProgramPresentation = catalog.Presentations[Watch] });
            builder.AddGeneral(new(Peer, "固定同桌", "supporter", "standard:none", "wei", 8));
            builder.AddDeck(new(Deck, "克己固定实体闪", 0, 0, [new("standard:dodge", 16)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "实际弃牌入口", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
