using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using static BoundaryLiDianChecks;

internal static class ShiZhongHuiSiziExpiryChecks
{
    private const string Hero = "mobile:shi-zhong-hui", Sizi = "mobile:shi-sizi";
    private const string Driver = "fixture:sizi-expiry-driver", Peer = "fixture:sizi-expiry-peer", Deck = "fixture:sizi-expiry-deck";
    private const string ChargeGain = "skill-program.mobile:shi-xiezhi.gain";
    private const string ChargePayment = "skill-program.mobile:shi-sizi.consume";
    private const string EndingDraw = "skill-program.mobile:shi-sizi.ending-draw";

    internal static void BothPaidSiziActivationsStopAtTheOwnersNextActualTurnCold()
    {
        var (game, content) = Start();
        Settle(game);
        var turn = Facts<TurnStartedEvent>(game).Single().TurnNumber;
        Use(game, Driver, "lose");
        Settle(game);
        Use(game, Driver, "recover");
        Settle(game);
        var gained = Facts<PlayerMarkerChangedEvent>(game).Where(entry => entry.PlayerSeat == 0 && entry.Reason == ChargeGain);
        Require(Facts<ProgramSkillHpLostEvent>(game).Single(entry => entry.SkillId == Driver) is { TargetSeat: 0, Amount: 2, RemainingHp: 2 } &&
            Facts<RecoveryAppliedEvent>(game).Single() == new RecoveryAppliedEvent(0, 0, 2, 4) &&
            gained.Select(entry => entry.Delta).SequenceEqual([2, 2]),
            "Real loss and recovery each trigger Xiezhi and together supply the four actual charge points.");

        Pay(game, "1");
        Settle(game);
        var first = Facts<ShiZhHuiSiziActivatedEvent>(game).Single();
        Require(first is { Source.OwnerSeat: 0, Consumed: 1, OwnerHp: 4 } && first.ActualTurn == turn && !CanUse(game, Sizi, "burn-charge"),
            "The first payment consumes one charge and the regular Play phase's sole use.");

        EndPlay(game);
        Reach(game, prompt => prompt.PlayerSeat == 0 && prompt.Choices.Any(IsInsertedPlayTarget));
        Answer(game, IsInsertedPlayTarget);
        Settle(game);
        var inserted = Facts<ProgramPhaseScheduledEvent>(game).Single(entry => entry.SkillId == Driver && entry.Started);
        Require(inserted is { OwnerSeat: 0, Phase: TurnPhase.Play } &&
            Facts<TurnStartedEvent>(game).Single() == new TurnStartedEvent(turn, 0) &&
            !Facts<TurnEndedEvent>(game).Any() && CanUse(game, Sizi, "burn-charge"),
            "The real inserted Play is a second legal activation phase inside the original actual turn.");

        Pay(game, "3");
        Settle(game);
        var second = Facts<ShiZhHuiSiziActivatedEvent>(game).Single(entry => entry.Ordinal != first.Ordinal);
        Require(second is { Source.OwnerSeat: 0, Consumed: 3, OwnerHp: 4 } && second.ActualTurn == turn && second.Source == first.Source &&
            Facts<PlayerMarkerChangedEvent>(game).Where(entry => entry.PlayerSeat == 0 && entry.Reason == ChargePayment)
                .Select(entry => entry.Delta).SequenceEqual([-1, -3]),
            "Both independently accepted physical phases have committed payments from the same living owner and skill source.");
        game = Cold(game, content);
        Require(Facts<ShiZhHuiSiziActivatedEvent>(game).SequenceEqual([first, second]),
            "Both original paid serials remain available to the replayed state.");

        EndPlay(game);
        Settle(game);
        var returnTurn = game.Events.Single(entry => entry.Payload is TurnStartedEvent started &&
            started.ActorSeat == 0 && started.TurnNumber == turn + 2);
        Require(Facts<TurnEndedEvent>(game).SequenceEqual([new TurnEndedEvent(turn, 0), new TurnEndedEvent(turn + 1, 1)]) &&
            Facts<TurnStartedEvent>(game).Last() == (TurnStartedEvent)returnTurn.Payload &&
            !Facts<ShiZhHuiSlashUsedEvent>(game).Any(),
            "Both actual turns finish and the payer takes a real next turn without any Slash interference.");

        var beforeEnding = game.CardMovements.Count;
        EndPlay(game);
        for (var step = 0; step < 100 && !Facts<TurnEndedEvent>(game).Any(entry => entry.TurnNumber == turn + 2); step++)
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        Require(Facts<TurnEndedEvent>(game).Last() == new TurnEndedEvent(turn + 2, 0) &&
            Facts<ShiZhHuiSiziActivatedEvent>(game).SequenceEqual([first, second]),
            "The payer's next actual ending completes without any new activation.");

        var overdue = game.Events.Where(entry => entry.Sequence > returnTurn.Sequence).Select(entry => entry.Payload)
            .OfType<ShiZhHuiSiziElapsedEvent>().Where(entry => entry.ActivationOrdinal == first.Ordinal || entry.ActivationOrdinal == second.Ordinal).ToArray();
        var draws = game.CardMovements.Skip(beforeEnding).Count(move => move.Reason.Value == EndingDraw && move.To == CardLocation.Hand(0));
        Require(overdue.Length == 0 && draws == 0,
            $"Neither paid Sizi can settle after its payer's next actual turn starts. Late elapsed [{string.Join(", ", overdue.AsEnumerable())}], late draws {draws}, expiries [{string.Join(", ", Facts<ShiZhHuiSiziExpiredEvent>(game))}].");
        _ = Cold(game, content);
    }

    private static bool IsInsertedPlayTarget(PromptChoice choice) =>
        choice.Parameters.GetValueOrDefault("program-action") == "select-target" && choice.Targets.SequenceEqual([0]);

    private static bool CanUse(GameEngine game, string skill, string activation) => game.GetHumanLegalActions()
        .Any(action => action.ProgramSkillId == skill && action.ProgramActivationId == activation);

    private static void Use(GameEngine game, string skill, string activation)
    {
        Require(CanUse(game, skill, activation), $"The published Play actions must contain {skill}/{activation}.");
        Accept(game, new UseProgramSkillCommand(0, skill, activation, [], [], game.Revision, P(game)!.PromptId));
    }

    private static void Pay(GameEngine game, string count)
    {
        Use(game, Sizi, "burn-charge");
        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Sizi });
        Answer(game, choice => choice.Parameters.GetValueOrDefault("count") == count);
    }

    private static void EndPlay(GameEngine game) =>
        Accept(game, new EndPlayPhaseCommand(0, game.Revision, P(game)!.PromptId));

    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) &&
            JsonSerializer.Serialize(restored.ResolutionStack) == JsonSerializer.Serialize(game.ResolutionStack) &&
            Enumerable.Range(0, game.PlayerCount).All(seat => SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold command replay preserves the real phase, both seats' views and all physical movement.");
        return restored;
    }

    private static (GameEngine Game, ContentRegistry Content) Start()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(617, 1, "sizi-expiry", Hero, 4, 0, 0, 0, 0,
            [Driver], [], [], [new(Peer, Hp: 4) { StartingHand = [] }], [], new string('a', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false
        }, content);
        Accept(game, new StartGameCommand());
        return (game, content);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-sizi-expiry", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var rules = $$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Driver}}","revision":1,
                  "activations":[
                    {"id":"lose","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":1,
                     "effects":[{"op":"loseHp","target":"owner","amount":2}]},
                    {"id":"recover","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":1,
                     "effects":[{"op":"recover","target":"owner","amount":2}]}],
                  "triggers":[{"id":"extra-play","window":"playEnding","subject":"owner","turnOwnerScope":"own","optional":false,
                    "usageScope":"game","usageLimit":1,"effects":[
                      {"op":"selectTarget","target":"owner","targetKind":"currentTurnPlayer"},
                      {"op":"insertPhase","target":"selectedTarget","phase":"play","phaseContinuation":"beforeNormalPreparation"}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Driver] = new { name = "真实体力和额外出牌", description = "改变体力并插入真实出牌阶段。" } } });
            var catalog = SkillProgramCatalog.Load(rules, presentation);
            builder.AddSkill(new(Driver, catalog.Presentations[Driver].Name, catalog.Presentations[Driver].Description)
                { Program = catalog.Programs[Driver], ProgramPresentation = catalog.Presentations[Driver] });
            builder.AddGeneral(new(Peer, "无固定技能的对手", "", "standard:none", "qun", 4));
            builder.AddDeck(new(Deck, "固定实体闪牌堆", 0, 2, [])
            {
                PhysicalCards = Enumerable.Range(0, 40).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 2)).ToArray()
            });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "肆恣两次付款期限", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
