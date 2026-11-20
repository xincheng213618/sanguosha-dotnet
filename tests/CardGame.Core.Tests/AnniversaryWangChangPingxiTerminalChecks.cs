using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using static BoundaryLiDianChecks;

internal static class AnniversaryWangChangPingxiTerminalChecks
{
    private const string Hero = "anniversary:wang-chang", Pingxi = "anniversary:pingxi";
    private const string Doomed = "fixture:pingxi-first", Peer = "fixture:pingxi-second";
    private const string Watch = "fixture:pingxi-discard-child", Deck = "fixture:pingxi-terminal-deck";
    private const string Discard = "skill-program.anniversary:wang-chang.discard";

    internal static void FirstPingxiDiscardDeathReturnsToTheOriginalGroupOrFinishesCold()
    {
        Check(terminal: false);
        Check(terminal: true);
    }

    private static void Check(bool terminal)
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture(terminal));
        var game = Start(content, terminal);
        var firstSeat = terminal ? 2 : 1;
        var secondSeat = terminal ? 1 : 2;
        var play = Next(game);
        Require(play is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }, "Wang Chang reaches his first actual Play phase.");
        var before = game.CreateSnapshot(0).Players[0];
        Accept(game, new EndPlayPhaseCommand(0, game.Revision, play!.PromptId));
        var offer = Next(game);
        var physicalDiscards = game.CardMovements.Count(move => move.From == CardLocation.Hand(0) &&
            move.To == CardLocation.DiscardPile && move.Reason == CardMoveReasons.HandLimitDiscard);
        var logicalDiscards = Facts<NativeLogicalDiscardInvocationRecordedEvent>(game)
            .Where(fact => fact.ExecutorSeat == 0).Select(fact => $"{fact.Reason.Value}={fact.ActualCount}").ToArray();
        Require(offer is { PlayerSeat: 0, SkillPrompt.SkillId: Pingxi } &&
            physicalDiscards == 2 &&
            Facts<NativeLogicalDiscardInvocationRecordedEvent>(game).Any(fact =>
                fact is { ExecutorSeat: 0, ActualCount: 2 } && fact.Reason == CardMoveReasons.HandLimitDiscard),
            $"Two ordinary physical hand-limit discards offer the actual anniversary Pingxi. " +
            $"Before discard: hand {before.HandCount}, HP {before.Hp}. Actual: {physicalDiscards} physical, " +
            $"[{string.Join(", ", logicalDiscards)}] logical. Now {game.State.Phase}, actor {game.State.CurrentSeat}, " +
            $"prompt {offer?.Kind}/{offer?.SkillPrompt?.SkillId}, seat {offer?.PlayerSeat}.");
        Answer(game, offer!, choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        var targets = Next(game);
        var root = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Pingxi);
        Require(root.AnniversaryWangChang is { Stage: AnniversaryWangChangStage.Targets, Maximum: 2 },
            "Pingxi freezes the original two-card budget before choosing targets.");
        Answer(game, targets!, choice => choice.Targets.SequenceEqual([firstSeat]));
        Answer(game, Next(game)!, choice => choice.Targets.SequenceEqual([secondSeat]));
        Answer(game, Next(game)!, choice => Verb(choice) == "seal");
        var first = Next(game);
        var crossbow = game.CreateSnapshot(firstSeat).Players[firstSeat].Equipment.Single(card => card.Kind == CardKind.Crossbow);
        Require(first is { PlayerSeat: 0, SkillPrompt.SkillId: Pingxi } &&
            first.Choices.Any(choice => Verb(choice) == "foreign" && choice.Cards.SequenceEqual([crossbow.Id])),
            "The sealed group offers its first target's real equipment for discard.");
        Answer(game, first!, choice => choice.Cards.SequenceEqual([crossbow.Id]));

        var child = Next(game);
        var paid = Facts<AnniversaryWangChangDiscardPaidEvent>(game).Single(fact => fact.FrameId == root.Id);
        var move = game.CardMovements.Single(item => item.CardId == crossbow.Id && item.Reason.Value == Discard);
        var active = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == root.Id);
        Require(child is { SkillPrompt.SkillId: Watch } && child.PlayerSeat == firstSeat &&
            active.AnniversaryWangChang is { Stage: AnniversaryWangChangStage.MovementChildren, Cursor: 0 } receipt &&
            receipt.Targets.SequenceEqual([firstSeat, secondSeat]) && receipt.Moves.SequenceEqual([move]) &&
            paid is { Cursor: 0, Count: 1 } && paid.SubjectSeat == firstSeat &&
            move.From == CardLocation.Equipment(firstSeat) && move.To == CardLocation.DiscardPile &&
            !Facts<AnniversaryWangChangSlashIssuedEvent>(game).Any(),
            "The first physical discard stays paid on the original sealed group while its movement child waits.");

        game = Cold(game, content);
        Answer(game, Next(game)!, choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
        var next = Next(game);
        Require(Facts<PlayerDiedEvent>(game).Single() is { VictimSeat: var dead } && dead == firstSeat &&
            game.CreateSnapshot(secondSeat).Players[secondSeat].IsAlive,
            "The original discard child kills only the first target and leaves the second target alive.");
        if (terminal)
        {
            Require(Facts<WinnerDeterminedEvent>(game).Any() && game.State.Status == EngineStatus.Completed &&
                next is null && game.ResolutionStack.Count == 0 &&
                Facts<AnniversaryWangChangDiscardPaidEvent>(game).Count(fact => fact.FrameId == root.Id) == 1 &&
                !Facts<AnniversaryWangChangSlashIssuedEvent>(game).Any(),
                "The final opponent's death finishes Pingxi's original turn without a new discard decision or Slash.");
            return;
        }

        Require(next is { PlayerSeat: 0, SkillPrompt.SkillId: Pingxi } &&
            game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == root.Id).AnniversaryWangChang is
                { Stage: AnniversaryWangChangStage.ForeignCard, Cursor: 1 } &&
            next.Choices.All(choice => Verb(choice) == "foreign" && choice.Cards.Count == 0) &&
            !Facts<AnniversaryWangChangSlashIssuedEvent>(game).Any(),
            "With another enemy alive, the same group reaches the next target's discard before any Slash.");
        Answer(game, next!, choice => choice.Parameters.GetValueOrDefault("source-zone") == "Hand");
        var continued = Next(game, () => Facts<TurnEndedEvent>(game).Any(fact => fact.ActorSeat == 0));
        var payments = Facts<AnniversaryWangChangDiscardPaidEvent>(game).Where(fact => fact.FrameId == root.Id).ToArray();
        var issued = Facts<AnniversaryWangChangSlashIssuedEvent>(game).ToArray();
        Require(continued is null && Facts<TurnEndedEvent>(game).Any(fact => fact.ActorSeat == 0) &&
            payments.Select(fact => fact.SubjectSeat).SequenceEqual([firstSeat, secondSeat]) &&
            payments.All(fact => fact.Count == 1) &&
            issued is [{ Cursor: 1 } slash] && slash.FrameId == root.Id && slash.TargetSeat == secondSeat &&
            Facts<CardUseFinishedEvent>(game).Count(fact => fact.ResolutionId == slash.UseId) == 1 &&
            Facts<AnniversaryWangChangCompletedEvent>(game).Count(fact => fact.FrameId == root.Id) == 1,
            "Both original discards finish first, then only the surviving target receives one complete native Slash.");
    }

    private static string? Verb(PromptChoice choice) => choice.Parameters.GetValueOrDefault("verb");

    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();

    private static PendingDecision? Next(GameEngine game, Func<bool>? finished = null)
    {
        for (var step = 0; step < 140; step++)
        {
            if (game.State.Status == EngineStatus.Completed || finished?.Invoke() == true)
                return null;
            var prompt = Enumerable.Range(0, game.PlayerCount).Select(seat => game.CreateSnapshot(seat).PendingDecision)
                .FirstOrDefault(item => item is not null);
            if (prompt is not null)
            {
                var pass = prompt.Kind is DecisionKind.RescueDying or DecisionKind.RespondDodge
                    ? prompt.Choices.FirstOrDefault(choice => choice.Cards.Count == 0)
                    : null;
                if (pass is null)
                    return prompt;
                Answer(game, prompt, choice => choice.Id == pass.Id);
            }
            else
                Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("Pingxi did not reach the next public boundary.");
    }

    private static void Answer(GameEngine game, PendingDecision prompt, Func<PromptChoice, bool> select) =>
        Accept(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.Single(select).Id, game.Revision));

    private static GameEngine Cold(GameEngine original, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(original.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(original.CardMovements) &&
            JsonSerializer.Serialize(restored.ResolutionStack) == JsonSerializer.Serialize(original.ResolutionStack) &&
            Enumerable.Range(0, original.PlayerCount).All(seat =>
                SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(original.CreateSnapshot(seat))),
            "Cold public replay keeps the original paid material and sealed group during its discard child.");
        return restored;
    }

    private static GameEngine Start(ContentRegistry content, bool terminal)
    {
        var first = new ShanheCombatantDefinition(Doomed, Hp: 1)
        {
            StartingHand = [], StartingEquipment = [new(CardKind.Crossbow, Suit.Club, 1)]
        };
        var peer = new ShanheCombatantDefinition(Peer) { StartingHand = [new(CardKind.Dodge, Suit.Heart, 2)] };
        var setup = new ShanheBattleSetup(83, 1, "anniversary-pingxi-terminal", Hero, 3, 0, 0, 0, 0, [],
            [new(CardKind.Dodge, Suit.Heart, 3), new(CardKind.Dodge, Suit.Heart, 4), new(CardKind.Dodge, Suit.Heart, 5)], [],
            terminal ? [first] : [first, peer], terminal ? [peer] : [], new string('e', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 3, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false
        }, content);
        Accept(game, new StartGameCommand());
        return game;
    }

    private sealed class Fixture(bool terminal) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-anniversary-pingxi-terminal", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var rules = $$"""
                {"skills":[{"id":"{{Watch}}","revision":1,"triggers":[{"id":"after-actual-equipment-discard","window":"cardsMoved",
                    "subject":"owner","sourceZones":["equipment"],"movementOccurrence":"perBatch","movementReasons":["{{Discard}}"],
                    "optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]},
                    {"op":"loseHp","target":"owner","amount":1}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object>
                {
                    [Watch] = new
                    {
                        name = "失去装备", description = "装备因平袭弃置后失去1点体力。",
                        optionLabels = new Dictionary<string, string> { ["continue"] = "继续" }
                    }
                }
            });
            var catalog = SkillProgramCatalog.LoadCurrent(rules, presentation);
            var shown = catalog.Presentations[Watch];
            builder.AddSkill(new(Watch, shown.Name, shown.Description)
                { Program = catalog.Programs[Watch], ProgramPresentation = shown });
            builder.AddGeneral(new(Doomed, "平袭首个目标", "supporter", Watch, "qun", 4));
            builder.AddGeneral(new(Peer, "平袭第二个目标", "supporter", "standard:none", "wei", 4));
            builder.AddDeck(new(Deck, "固定无救援牌堆", InitialHandSize: 0, DrawPerTurn: 2, [])
            {
                PhysicalCards = Enumerable.Range(1, 12).Select(rank => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, rank)).ToArray()
            });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "周年平袭首次弃牌", 3, 3, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Doomed, Peer], ModeKind: ContentModeKind.Team, TeamCounts: new Dictionary<string, int>
                {
                    [ShanheBattleSetup.PlayerTeamId] = terminal ? 2 : 1,
                    [ShanheBattleSetup.EnemyTeamId] = terminal ? 1 : 2
                }));
        }
    }
}
