using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using static BoundaryFuHuangHouScenario;

internal static class ForeignTurnContestRuntimeChecks
{
    public static void OriginalResultPaidChildAndTypedSlashReturnStayExactCold()
    {
        WinningResultRestrictsTheActualTurnOwner();
        TiedResultReturnsTheOriginalPaidSlashCold();
        EmptyOpponentHandRejectsTheActualContestCandidate();
    }

    private static void WinningResultRestrictsTheActualTurnOwner()
    {
        var (_, game) = CreateScene();
        Reach(game, prompt => Optional(prompt, Zhuikong));
        var parent = game.ResolutionStack.OfType<ForeignActualTurnStartWindowFrame>().Single();
        Contest(game, win: true);
        Reach(game, prompt => prompt is { Kind: DecisionKind.PlayCard } && prompt.PlayerSeat == parent.OwnerSeat);
        var frozen = Facts<ForeignTurnContestOriginFrozenEvent>(game).Single();
        var restricted = Facts<ForeignTurnContestRestrictedEvent>(game).Single();
        var grant = Facts<CardTargetRestrictionGrantedEvent>(game)
            .Single(fact => fact.Restriction.GrantSequence == restricted.GrantSequence).Restriction;
        Require(frozen.Origin.Result is { SourceSeat: 0, SourceRank: 13, OpponentRank: 7, SourceWon: true } &&
                frozen.Origin.ActualTurnOwnerSeat == parent.OwnerSeat && restricted.Origin == frozen.Origin &&
                restricted.ProgramFrameId == frozen.Origin.ProgramFrameId && restricted.Source == frozen.Source &&
                grant.ParentFrameId == frozen.Origin.ProgramFrameId && grant.EffectIndex == 2 &&
                grant.Restriction == SkillProgramCardTargetRestriction.SelfOnly && grant.SubjectSeat == parent.OwnerSeat &&
                grant.TurnNumber == parent.ActualTurnNumber && grant.TurnSeat == parent.OwnerSeat &&
                Facts<ForeignTurnContestCardClaimedEvent>(game).Length == 0 &&
                Facts<ForeignTurnContestSlashIssuedEvent>(game).Length == 0,
            "The frozen winning result issues one restriction to the actual other turn owner and never claims a card or issues a Slash.");
        AssertCompletedParent(game, parent, frozen.Origin.ProgramFrameId);
    }

    private static void TiedResultReturnsTheOriginalPaidSlashCold()
    {
        var (registry, game) = CreateScene();
        Reach(game, prompt => Optional(prompt, Zhuikong));
        var parent = game.ResolutionStack.OfType<ForeignActualTurnStartWindowFrame>().Single();
        Contest(game, win: false);
        Reach(game, prompt => prompt?.SkillPrompt?.SkillId == Watch);
        var frozen = Facts<ForeignTurnContestOriginFrozenEvent>(game).Single();
        var origin = frozen.Origin;
        var program = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == origin.ProgramFrameId);
        var receipt = program.ForeignTurnContest!;
        var claim = Facts<ForeignTurnContestCardClaimedEvent>(game).Single();
        var moved = game.CardMovements.Single(move => move.Sequence == claim.MovementSequence);
        var movementWindow = game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Single();
        Require(origin.Result is { SourceSeat: 0, SourceRank: 7, OpponentRank: 7, SourceWon: false } &&
                program.InstructionIndex == 3 && receipt is { Stage: ForeignTurnContestStage.ClaimChildren, RestrictionIssued: false } &&
                receipt.InstructionIndex == program.InstructionIndex && receipt.Origin == origin && receipt.SlashReturn is null &&
                program.PindianResultBindings.Single().ForeignTurnOrigin == origin &&
                program.PindianResultBindings.Single().NativePindianFrameId == origin.PindianFrameId &&
                program.PendingMovementContinuation is { SubjectSeat: 0, BeforeCount: 0, CoverageResultBind: null } &&
                claim.ProgramFrameId == program.Id && claim.Source == frozen.Source && claim.Origin == origin &&
                claim.CardId == origin.Result.OpponentCardId && receipt.ClaimedCardId == claim.CardId &&
                receipt.MovementSequence == moved.Sequence && moved.CardId == claim.CardId &&
                moved.From == CardLocation.DiscardPile && moved.To == CardLocation.Hand(0) && moved.Reason.Value == ClaimReason &&
                movementWindow.Batch.ParentFrameId == program.Id && movementWindow.Batch.AwaitingProgramFrameId == program.Id &&
                movementWindow.ResumeProgramFrameId is null &&
                movementWindow.Batch.Movements.Any(move => move.Sequence == claim.MovementSequence) &&
                Facts<ForeignTurnContestSlashIssuedEvent>(game).Length == 0,
            "A tie claims the exact revealed opponent entity once and suspends the original paid program in its real gain child before Slash issuance.");

        game = Cold(game, registry);
        Require(game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == program.Id).ForeignTurnContest == receipt,
            "Cold recovery preserves the original paid result, instruction cursor and movement receipt.");
        Continue(game);
        Reach(game, prompt => prompt is { Kind: DecisionKind.RespondDodge });
        var issued = Facts<ForeignTurnContestSlashIssuedEvent>(game).Single().Return;
        var use = game.ResolutionStack.OfType<CardUseFrame>().Single(frame => frame.Id == issued.CardUseFrameId);
        var awaiting = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == program.Id);
        Require(issued.ProgramFrameId == program.Id && issued.InstructionIndex == 3 &&
                issued.Source == frozen.Source && issued.Origin == origin &&
                awaiting.ForeignTurnContest is { Stage: ForeignTurnContestStage.SlashIssued } pending &&
                pending.SlashReturn == issued && pending.ClaimedCardId == claim.CardId && pending.MovementSequence == claim.MovementSequence &&
                awaiting.PendingMovementContinuation is null && use.ForeignTurnContestSlashReturn == issued &&
                use is { CardId: 0, CardKind: CardKind.Slash, PhysicalCardIds.Count: 0 } &&
                use.Action is { Type: CardActionType.Use, EffectiveKind: CardKind.Slash, PhysicalCards.Count: 0 } action &&
                action.ActionId == issued.CardActionId && action.ActorSeat == parent.OwnerSeat && action.ProviderSeat == parent.OwnerSeat &&
                action.TargetSeats.SequenceEqual([0]) && Facts<ForeignTurnContestSlashReturnedEvent>(game).Length == 0 &&
                Facts<ForeignTurnContestRestrictedEvent>(game).Length == 0,
            "Only after the gain child returns does the original program issue one zero-entity native Slash with its exact typed return.");

        game = Cold(game, registry);
        Answer(game, choice => choice.Parameters.GetValueOrDefault("response") == "take-damage");
        Reach(game, prompt => prompt is { Kind: DecisionKind.PlayCard } && prompt.PlayerSeat == parent.OwnerSeat);
        Require(Facts<ForeignTurnContestOriginFrozenEvent>(game).Single() == frozen &&
                Facts<PindianResultDeterminedEvent>(game).Count(fact => fact.FrameId == origin.PindianFrameId && fact.Result == origin.Result) == 1 &&
                Facts<ForeignTurnContestCardClaimedEvent>(game).Single() == claim &&
                game.CardMovements.Count(move => move.CardId == claim.CardId && move.Reason.Value == ClaimReason) == 1 &&
                Facts<ForeignTurnContestSlashIssuedEvent>(game).Single().Return == issued &&
                Facts<ForeignTurnContestSlashReturnedEvent>(game).Single().Return == issued &&
                Facts<CardUseFinishedEvent>(game).Count(fact => fact.ResolutionId == issued.CardUseFrameId) == 1,
            "Cold native Slash completion returns the exact issued receipt once without repeating the original result, card claim or child use.");
        AssertCompletedParent(game, parent, program.Id);
    }

    private static void AssertCompletedParent(GameEngine game, ForeignActualTurnStartWindowFrame parent, long programFrameId)
    {
        Require(!game.ResolutionStack.Any(frame => frame.Id == programFrameId || frame.Id == parent.Id) &&
                Facts<ProgramBindingResolvedEvent>(game).Count(fact => fact.FrameId == programFrameId && fact.Activated && fact.Completed) == 1 &&
                game.State.CurrentSeat == parent.OwnerSeat && game.State.TurnNumber == parent.ActualTurnNumber && game.State.Phase == TurnPhase.Play,
            "The exact original foreign candidate completes once and releases its actual-turn parent cursor into the other owner's normal Play.");
    }

    private static void EmptyOpponentHandRejectsTheActualContestCandidate()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new EmptyInputFixture());
        foreach (var opponentHasHand in new[] { true, false })
        {
            var opponent = new ShanheCombatantDefinition(EmptyInputFixture.Peer, Hp: 4)
            {
                StartingHand = opponentHasHand ? [new(CardKind.Dodge, Suit.Club, 7)] : []
            };
            var setup = new ShanheBattleSetup(31, 1, "foreign-contest-input", EmptyInputFixture.Hero, 3, 0, 0, 0, 0,
                [], [new(CardKind.Dodge, Suit.Heart, 13)], [], [opponent], [], new string('c', 64));
            var game = GameEngine.CreateStandard(new GameOptions
            {
                Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4
            }, registry);
            PendingDecision? Prompt() => game.CreateSnapshot(0).PendingDecision;
            void AdvanceTo(Func<bool> ready)
            {
                for (var step = 0; step < 80 && !ready(); step++)
                {
                    Require(Prompt() is null && game.State.Winner == Winner.None,
                        "The two-seat contest scene reached an unexpected decision.");
                    Accept(game, new AdvanceOneStepCommand(game.Revision));
                }
                Require(ready(), "The two-seat contest scene did not reach its actual boundary.");
            }

            Accept(game, new StartGameCommand());
            AdvanceTo(() => Prompt() is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
            Require(Player(game, 0) is { IsAlive: true, Hp: 3, HandCount: 1 } &&
                Player(game, 1) is { IsAlive: true, Hp: 4, Equipment.Count: 0 } &&
                Player(game, 1).HandCount == (opponentHasHand ? 1 : 0),
                "The source holds a real card and the healthy opponent qualifies by HP in both games.");

            Accept(game, new EndPlayPhaseCommand(0, game.Revision, Prompt()!.PromptId));
            ProgramBindingResolvedEvent[] Resolved() => Facts<ProgramBindingResolvedEvent>(game)
                .Where(fact => fact.SkillId == Zhuikong).ToArray();
            AdvanceTo(() => opponentHasHand ? Optional(Prompt(), Zhuikong) : Resolved().Length > 0);
            Require(Facts<TurnStartedEvent>(game).Last() is { ActorSeat: 1 } turn &&
                turn.TurnNumber == game.State.TurnNumber &&
                Facts<ProgramBindingStartedEvent>(game).All(fact => fact.SkillId != Zhuikong) &&
                Facts<PindianResultDeterminedEvent>(game).Length == 0,
                "The original foreign turn has begun while neither game has paid for or started a contest.");

            if (opponentHasHand)
            {
                Require(Prompt() is { PlayerSeat: 0, IsPrivate: true } && Resolved().Length == 0 &&
                    game.ResolutionStack.OfType<ForeignActualTurnStartWindowFrame>().Single().OwnerSeat == 1,
                    "Adding one real hand card permits the original private contest offer.");
            }
            else
            {
                Require(Resolved() is [{ OwnerSeat: 0, BindingId: "foreign-actual-turn-contest",
                    Window: SkillProgramTriggerWindow.OtherActualTurnStarted, Activated: false, Completed: false }] &&
                    !Optional(Prompt(), Zhuikong) && Player(game, 1).HandCount == 0,
                    "The frozen candidate reaches native admission and is rejected for the opponent's missing input.");
            }
        }
    }

    private sealed class EmptyInputFixture : IGameContentPackage
    {
        internal const string Hero = "boundary:fu-huang-hou-mobile", Peer = "fixture:foreign-contest-empty-peer";
        private const string Deck = "fixture:foreign-contest-empty-deck";
        public PackageManifest Manifest { get; } = new("fixture-foreign-contest-empty-input", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            builder.AddGeneral(new(Peer, "拼点对照", "supporter", "standard:none", "wei", 4));
            builder.AddDeck(new(Deck, "固定牌堆", 0, 0, [new("standard:dodge", 16)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "惴恐输入对照", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team, TeamCounts: new Dictionary<string, int>
                    { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }

    private static GameEngine Cold(GameEngine game, ContentRegistry registry)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Require(State(game) == State(restored), "Cold recovery preserves all original frames, commands, public facts and physical zones.");
        return restored;
    }

    private static T[] Facts<T>(GameEngine game) => game.Events.Select(fact => fact.Payload).OfType<T>().ToArray();
    private static string State(GameEngine game) => JsonSerializer.Serialize(new
    {
        Views = Enumerable.Range(0, 4).Select(seat => SnapshotJson.Serialize(game.CreateSnapshot(seat))).ToArray(),
        Frames = game.ResolutionStack.Select(frame => JsonSerializer.Serialize(frame, frame.GetType())).ToArray(),
        Facts = game.Events.Select(fact => JsonSerializer.Serialize(fact.Payload, fact.Payload.GetType())).ToArray(),
        game.CardMovements, Zones = game.CreateCardZoneDiagnostics(), Commands = CommandJson.Serialize(game.AcceptedCommands)
    });
    private static void Require(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }
}
