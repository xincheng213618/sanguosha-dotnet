using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheBossOpeningStageChecks
{
    private const string Owner = "fixture:boss-opening-owner", Driver = "fixture:boss-opening-driver";
    private const string Deck = "fixture:boss-opening-deck", Quiet = "fixture:boss-opening-quiet";
    private static readonly string[] PreparationSkills =
    [StandardShanhePreparationTacticsPackage.ChoubeiSkillId,
        StandardShanhePreparationTacticsPackage.Choubei2SkillId,
        StandardShanhePreparationTacticsPackage.Choubei3SkillId];
    private static readonly Lazy<ContentRegistry> Registry = new(() => ContentRegistry.Build(
        new StandardContentPackage(), new StandardActiveSkillExpansionPackage(true),
        new StandardRescueSkillExpansionPackage(), new StandardClassicGeneralPackage(),
        new StandardShanhePreparationTacticsPackage(), new StandardShanheDrawThresholdPackage(),
        new StandardShanheDirectSkillDamagePackage(), new StandardShanheS5HuaxiongBasePackage(),
        new StandardShanheS5HuaxiongPackage(), new Fixture()));

    public static void RealOpeningDrawDamageTransformsWithoutRetiringTheUnfinishedStartup()
    {
        var game = CreateGame(firstHp: 5);
        for (var step = 0; step < 160 && Events<ShanheBossStageStartedEvent>(game).Length == 0; step++)
        {
            Need(game.State.Winner == Winner.None && game.PendingDecision is null,
                "The real opening six-card threshold must transform its lethal first form before dying, settlement or a first-turn decision.");
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        var thresholdHit = Events<NativeDrawThresholdTargetChosenEvent>(game).SingleOrDefault();
        var triggeringInvoice = Events<NativeDrawInvocationRecordedEvent>(game)
            .SingleOrDefault(e => e.InvocationId == thresholdHit?.InvocationId);
        var origin = triggeringInvoice?.GameStartingOrigin;
        // Native synchronous scheduling may complete the entire opening in one
        // Advance. The paid invoice and matching completion prove its original return.
        var startupPayloads = game.Events.Select(e => e.Payload).ToArray();
        var startedIndex = Array.FindIndex(startupPayloads, e => origin is not null && e is ProgramBindingStartedEvent started &&
            started.FrameId == origin.ProgramFrameId && started.Window == SkillProgramTriggerWindow.GameStarting &&
            started.SkillId == origin.Source.SkillId && started.SkillInstanceId == origin.Source.SkillInstanceId &&
            started.BindingId == origin.Source.BindingId && started.OwnerSeat == 0);
        var invoiceIndex = Array.FindIndex(startupPayloads, e => e is NativeDrawInvocationRecordedEvent invoice &&
            invoice.InvocationId == triggeringInvoice?.InvocationId);
        var thresholdIndex = Array.FindIndex(startupPayloads, e => e is NativeDrawThresholdTargetChosenEvent chosen &&
            chosen == thresholdHit);
        var stageIndex = Array.FindIndex(startupPayloads, e => e is ShanheBossStageStartedEvent);
        var completedIndex = Array.FindIndex(startupPayloads, e => e is ProgramBindingResolvedEvent resolved &&
            resolved.FrameId == origin?.ProgramFrameId && resolved.Window == SkillProgramTriggerWindow.GameStarting &&
            resolved.Activated && resolved.Completed);
        var laterOpeningIndex = Array.FindIndex(startupPayloads, e => e is ProgramBindingStartedEvent { SkillId: Driver });
        var stageSnapshot = game.CreateSnapshot(0);
        RequireSecondFormResources(game);
        Need(Events<ShanheBossStageStartedEvent>(game) is
                [{ BossSeat: 1, TurnNumber: 0, GeneralId: StandardShanheS5HuaxiongPackage.HuaxiongId, Hp: 8, MaxHp: 8 }] &&
            Events<DamageAppliedEvent>(game) is [{ SourceSeat: 0, TargetSeat: 1, Amount: 6, RemainingHp: <= 0 }] &&
            stageSnapshot is { CurrentSeat: 0, TurnNumber: 0 or 1, Winner: Winner.None } &&
            triggeringInvoice is { OwnerSeat: 0, TurnNumber: 0 } && origin is not null &&
            triggeringInvoice.ParentFrameId == origin.ProgramFrameId && origin.WindowFrameId > 0 &&
            PreparationSkills.Contains(origin.Source.SkillId) && startedIndex >= 0 && startedIndex < invoiceIndex &&
            invoiceIndex < thresholdIndex && thresholdIndex < stageIndex && stageIndex < completedIndex &&
            completedIndex < laterOpeningIndex && Events<ProgramBindingResolvedEvent>(game)
                .Count(e => e.FrameId == origin.ProgramFrameId && e.Activated && e.Completed) == 1,
            "Three genuine preparation draws feed the real six-card tactic and both direct-damage bonuses; the exact paid original program returns and completes once after transformation, before the later binding in the same startup. " +
            JsonSerializer.Serialize(new
            {
                Stages = Events<ShanheBossStageStartedEvent>(game), Damage = Events<DamageAppliedEvent>(game),
                stageSnapshot.CurrentSeat, stageSnapshot.TurnNumber, stageSnapshot.Phase, stageSnapshot.Winner,
                origin, thresholdHit, triggeringInvoice, startedIndex, invoiceIndex, thresholdIndex,
                stageIndex, completedIndex, laterOpeningIndex
            }));
        Need(Events<PlayerDiedEvent>(game).Length == 0 && Events<PlayerDyingEvent>(game).Length == 0 &&
            Events<WinnerDeterminedEvent>(game).Length == 0 && Events<ShanheBossResolutionCancelledEvent>(game).Length == 0 &&
            Events<ShanheBossTurnInterruptedEvent>(game).Length == 0 && Events<TurnEndedEvent>(game).Length == 0,
            "Opening transformation must neither announce first-form death nor cancel pending startup children or manufacture an ended actual turn zero.");
        var logCut = ShanheBossStageChecks.RequireNativeStageLogBoundary(game, requireLethalDamage: true);
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), Registry.Value);
        Need(JsonSerializer.Serialize(game.Events) == JsonSerializer.Serialize(restored.Events) &&
            JsonSerializer.Serialize(game.Log) == JsonSerializer.Serialize(restored.Log) &&
            ShanheBossStageChecks.RequireNativeStageLogBoundary(restored, requireLethalDamage: true) == logCut,
            "Cold opening replay preserves the typed turn-zero stage boundary, original fatal Damage and later second-form native draw/announcement logs.");
        game = restored;

        Until(game, () => game.PendingDecision is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        var openingSources = PreparationSkills.Append(Driver).ToArray();
        var invoices = Events<NativeDrawInvocationRecordedEvent>(game).Where(e => e.OwnerSeat == 0).ToArray();
        var payloads = game.Events.Select(e => e.Payload).ToArray();
        RequireSecondFormResources(game);
        Need(game.CreateSnapshot(0) is { CurrentSeat: 0, TurnNumber: 1 } &&
            game.CreateSnapshot(0).Players[0].HandCount == 7 && game.CreateSnapshot(1).Players[1].HandCount == 2 &&
            openingSources.All(skill => Events<ProgramBindingResolvedEvent>(game).Count(e => e.SkillId == skill &&
                e.Window == SkillProgramTriggerWindow.GameStarting && e.Activated && e.Completed) == 1) &&
            invoices.Length == 4 && invoices.Sum(e => e.ActualCount) == 7 && invoices.All(e =>
                e.TurnNumber == 0 && e.GameStartingOrigin is { } invoiceOrigin &&
                invoiceOrigin.WindowFrameId == origin!.WindowFrameId && openingSources.Contains(invoiceOrigin.Source.SkillId)) &&
            Events<TurnStartedEvent>(game) is [{ ActorSeat: 0, TurnNumber: 1 }] &&
            Array.FindIndex(payloads, e => e is ShanheBossStageStartedEvent) <
                Array.FindIndex(payloads, e => e is ProgramBindingStartedEvent { SkillId: Driver }),
            "The original preparation bindings finish once, the later printed-skill opening draw still executes once, seven actual owner cards are paid, the boss gets only its configured two cards, and the player's true first turn remains first.");

        Accept(game, new UseProgramSkillCommand(0, Driver, "finish", [], [1], game.Revision, game.PendingDecision!.PromptId));
        Until(game, () => game.State.Winner != Winner.None);
        Need(game.CreateSnapshot(0).WinnerTeamId == ShanheBattleSetup.PlayerTeamId &&
            Events<ShanheBossStageStartedEvent>(game).Length == 1 && Events<PlayerDiedEvent>(game) is [{ VictimSeat: 1 }] &&
            Events<TurnEndedEvent>(game).All(e => e.TurnNumber > 0) &&
            openingSources.All(skill => Events<ProgramBindingResolvedEvent>(game).Count(e => e.SkillId == skill &&
                e.Window == SkillProgramTriggerWindow.GameStarting && e.Completed) == 1),
            "After the preserved startup, a genuine first-turn activation kills the second form normally without another transformation, repeated opening reward or phantom turn end.");
    }

    public static void ActualTurnLethalStillRetiresThatTurnAndStartsTheBossNaturalTurn()
    {
        var game = CreateGame(firstHp: 7);
        Until(game, () => game.PendingDecision is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        Need(Events<ShanheBossStageStartedEvent>(game).Length == 0 &&
            Events<DamageAppliedEvent>(game) is [{ SourceSeat: 0, TargetSeat: 1, Amount: 6, RemainingHp: 1 }] &&
            game.CreateSnapshot(1).Players[1] is { GeneralId: StandardShanheS5HuaxiongBasePackage.HuaxiongId, Hp: 1 },
            "The same real opening chain leaves the deliberately seven-health first form alive until an actual player turn.");
        var turn = game.State.TurnNumber;
        Accept(game, new UseProgramSkillCommand(0, Driver, "finish", [], [1], game.Revision, game.PendingDecision!.PromptId));
        Until(game, () => Events<ShanheBossStageStartedEvent>(game).Length != 0);
        RequireSecondFormResources(game);
        Need(Events<ShanheBossStageStartedEvent>(game).Single() is { TurnNumber: 1, BossSeat: 1 } &&
            Events<ShanheBossTurnInterruptedEvent>(game) is [{ TurnNumber: 1, PlayerSeat: 0, BossSeat: 1 }] &&
            Events<ShanheBossResolutionCancelledEvent>(game).Length > 0 &&
            Events<TurnEndedEvent>(game).Count(e => e.TurnNumber == turn && e.ActorSeat == 0) == 1 &&
            game.CreateSnapshot(0) is { CurrentSeat: 1, TurnNumber: 1, Phase: TurnPhase.NotStarted, Winner: Winner.None } &&
            game.ResolutionStack.Count == 0 && Events<PlayerDiedEvent>(game).Length == 0,
            "Lethal damage during a genuine actual turn retains the existing whole-subtree retirement, exact old-turn end and boss-next-natural-turn behavior.");
        Until(game, () => game.PendingDecision is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        Need(Events<TurnStartedEvent>(game).Any(e => e.ActorSeat == 1 && e.TurnNumber == turn + 1) &&
            game.State.TurnNumber > turn && game.CreateSnapshot(1).Players[1] is { IsAlive: true, Hp: 8 },
            "The transformed boss actually takes its scheduled natural turn before returning to the player's next native play phase.");
        Accept(game, new UseProgramSkillCommand(0, Driver, "finish", [], [1], game.Revision, game.PendingDecision!.PromptId));
        Until(game, () => game.State.Winner != Winner.None);
        Need(game.CreateSnapshot(0).WinnerTeamId == ShanheBattleSetup.PlayerTeamId &&
            Events<ShanheBossStageStartedEvent>(game).Length == 1 && Events<PlayerDiedEvent>(game) is [{ VictimSeat: 1 }] &&
            Events<TurnEndedEvent>(game).All(e => e.TurnNumber > 0) &&
            PreparationSkills.Append(Driver).All(skill => Events<ProgramBindingResolvedEvent>(game).Count(e =>
                e.SkillId == skill && e.Window == SkillProgramTriggerWindow.GameStarting && e.Completed) == 1),
            "The second form remains normally killable and neither shared transformation path repeats the already completed startup.");
    }

    private static GameEngine CreateGame(int firstHp)
    {
        var setup = new ShanheBattleSetup(71, 1, "fixture:opening-boss-stage", Owner, 4, 0, 0, 0, 0,
            [], [], [], [new(StandardShanheS5HuaxiongBasePackage.HuaxiongId, MaxHpBonus: firstHp - 5, Hp: firstHp,
                ExtraSkillIds: [Quiet]) { StartingHand = [] }], [], new string('b', 64))
        {
            TacticSkillIds = PreparationSkills.Concat(new[]
            {
                StandardShanheDrawThresholdPackage.ShenlongBaiwei2SkillId,
                StandardShanheCampaignCatalog.CurrentJiyi2SkillId,
                StandardShanheCampaignCatalog.CurrentJiyi3SkillId
            }).ToArray(),
            BossTransition = new(0, StandardShanheS5HuaxiongPackage.HuaxiongId, 8, 8, 2)
            {
                Combatant = new(StandardShanheS5HuaxiongPackage.HuaxiongId) { InitialArmor = 2 },
                StartingEquipment = [new(CardKind.Crossbow, Suit.Club, 11)]
            }
        };
        var game = GameEngine.CreateStandard(new()
        {
            Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false,
            MaxTurns = 6, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
        }, Registry.Value);
        Accept(game, new StartGameCommand());
        return game;
    }

    private static void RequireSecondFormResources(GameEngine game)
    {
        var boss = game.CreateSnapshot(0).Players[1];
        Need(boss.Armor == 2 && boss.Equipment is [{ Kind: CardKind.Crossbow, Suit: Suit.Club, Rank: 11 }],
            "Both opening and actual-turn transitions install the grown second-form armor and one real configured equipment card before continuing.");
    }

    private static T[] Events<T>(GameEngine game) => game.Events.Select(e => e.Payload).OfType<T>().ToArray();
    private static void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message); }
    private static void Accept(GameEngine game, GameCommand command)
    { var result = game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected opening-stage native command."); }
    private static void Until(GameEngine game, Func<bool> predicate)
    {
        for (var step = 0; step < 180; step++)
        {
            if (predicate()) return;
            if (game.PendingDecision is { PlayerSeat: 0 } prompt)
            {
                var pass = prompt.Choices.FirstOrDefault(choice => choice.Cards.Count == 0 &&
                    choice.Parameters.GetValueOrDefault("response") is "pass" or "let-die" or "take-damage");
                Need(pass is not null, "The bounded opening-stage fixture reached an unrelated human choice.");
                Accept(game, new AnswerPromptCommand(0, prompt.PromptId, pass!.Id, game.Revision));
            }
            else Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The opening-stage fixture did not reach its bounded native continuation.");
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boss-opening-stage", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var programs = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,"triggers":[
                    {"id":"remaining-opening-draw","window":"gameStarting","subject":"owner","priority":-1000,
                     "optional":false,"effects":[{"op":"draw","target":"owner","amount":1}]}],
                   "activations":[{"id":"finish","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,
                    "targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":20}]}]},
                  {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner",
                    "optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}
                ]}
                """, JsonSerializer.Serialize(new
                {
                    schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new[] { Driver, Quiet }.ToDictionary(id => id, _ => new { name = "开局与回合观测", description = "后置真实摸牌与原生终结伤害" })
                }));
            foreach (var (id, program) in programs.Programs)
                builder.AddSkill(new(id, id, "开局与回合观测") { Program = program, ProgramPresentation = programs.Presentations[id] });
            builder.AddGeneral(new(Owner, "首回合玩家", "supporter", Driver, "shu", 4));
            builder.AddDeck(new(Deck, "原生实体闪牌堆", 0, 0, [])
                { PhysicalCards = Enumerable.Range(0, 32).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "开局伤害二阶段", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, StandardShanheS5HuaxiongBasePackage.HuaxiongId, StandardShanheS5HuaxiongPackage.HuaxiongId],
                ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
