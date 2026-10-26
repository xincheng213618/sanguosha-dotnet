using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheRoundRecoveryChecks
{
    private const string Tactic = StandardShanheRoundRecoveryPackage.Xuezhan2SkillId;
    private const string Driver = "fixture:round-recovery-driver", Quiet = "fixture:round-recovery-quiet";
    private const string Peer = "fixture:round-recovery-peer", Deck = "fixture:round-recovery-deck";
    private static readonly Lazy<ContentRegistry> Registry = new(() => ContentRegistry.Build(
        new StandardContentPackage(), new StandardShanheRoundRecoveryPackage(), new Fixture()));

    public static void XuezhanOpeningFloorsAtOneClampsHpAndNewBattleUsesItsOwnBaseline()
    {
        foreach (var baseline in new[] { 1, 2, 4 })
        {
            var scene = Scene.Start(baseline);
            var expected = Math.Max(1, baseline - 2);
            Need(scene.View() is { IsAlive: true } owner && owner.MaxHp == expected && owner.Hp == expected &&
                scene.Events<MaximumHpChangedEvent>() is [{ PlayerSeat: 0, SkillId: Tactic } maximum] &&
                maximum.Delta == expected - baseline && maximum.MaximumHp == expected &&
                scene.Events<PlayerDyingEvent>().Length == 0 && scene.Events<DamageAppliedEvent>().Length == 0,
                "The real acquired tactic lowers maximum HP once, floors it at one and clamps current HP without damage or a dying window.");
            scene.ReachPlay();
            Need(scene.Events<MaximumHpChangedEvent>().Length == 1,
                "Repeated progress at the same real Play boundary cannot pay the opening maximum-HP cost again.");
        }
        var first = Scene.Start(4);
        first.Hurt(1);
        Need(first.View() is { MaxHp: 2, Hp: 1 }, "The first battle retains its actual remaining health after native damage.");
        var next = Scene.Start(4, first.View().Hp, ordinal: 2);
        Need(next.View() is { MaxHp: 2, Hp: 2 } &&
            next.Events<MaximumHpChangedEvent>() is [{ Delta: -2, MaximumHp: 2 }] &&
            next.Events<RecoveryAppliedEvent>() is [{ SourceSeat: 0, TargetSeat: 0, Amount: 1, RemainingHp: 2 }],
            "A distinct new battle uses the original four-HP general baseline, reduces it to two once, and really recovers the carried one HP instead of cumulatively reducing the previous battle maximum.");
    }

    public static void XuezhanRecoversAtMostTwoOncePerRealRound()
    {
        var scene = Scene.Start(6);
        Need(scene.Round == 1 && scene.View() is { MaxHp: 4, Hp: 4 } && scene.Events<RecoveryAppliedEvent>().Length == 0,
            "The opening round's real recovery cannot manufacture excess HP or a positive recovery event at full health.");
        scene.Hurt(2);
        Need(scene.View().Hp == 2 && scene.Events<DamageAppliedEvent>() is [{ TargetSeat: 0, Amount: 2 }],
            "A genuine two-damage segment leaves the tactic owner wounded during its first round.");
        scene.NextOwnPlay();
        Need(scene.Round == 2 && scene.View() is { MaxHp: 4, Hp: 4 } &&
            scene.Events<RecoveryAppliedEvent>() is [{ SourceSeat: 0, TargetSeat: 0, Amount: 2, RemainingHp: 4 }],
            "The next real normal-round start performs the actual two-point recovery before the owner's actions.");
        scene.Hurt(1);
        scene.NextOwnPlay();
        Need(scene.Round == 3 && scene.View().Hp == 4 &&
            scene.Events<RecoveryAppliedEvent>().Select(fact => fact.Amount).SequenceEqual([2, 1]),
            "A later round recovers only the actual missing health, preserving the native two-point cap.");
        var recoveries = scene.Game.Events.Where(envelope => envelope.Payload is RecoveryAppliedEvent).ToArray();
        Need(recoveries.Select(envelope => scene.Game.Events.Where(before => before.Sequence < envelope.Sequence)
                    .Select(before => before.Payload).OfType<RoundStartedEvent>().Last().RoundNumber).SequenceEqual([2, 3]) &&
            recoveries.All(envelope => scene.Game.Events.Where(before => before.Sequence < envelope.Sequence)
                .Select(before => before.Payload).OfType<TurnStartedEvent>().Last().ActorSeat == 0) &&
            scene.Events<TurnStartedEvent>().Count(fact => fact.ActorSeat == 1) == 2 &&
            scene.Events<SkillUsageConsumedEvent>().Count(fact => fact.SkillId == Tactic && fact.Scope == SkillUsageScope.Round) == 3 &&
            scene.Events<MaximumHpChangedEvent>().Length == 1,
            "The enemy's intervening normal turns never repeat same-round recovery; the production tactic consumes one round allowance at each true round and never repeats the opening maximum cost.");
    }

    private static string Owner(int maximum) => "fixture:round-recovery-owner-" + maximum;
    private static void Need(bool valid, string message) { if (!valid) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine Game { get; } = game;
        private PendingDecision? Pending => Game.PendingDecision;
        public int Round => Events<RoundStartedEvent>().Last().RoundNumber;
        public PlayerSnapshot View() => Game.CreateSnapshot(0).Players[0];
        public T[] Events<T>() => Game.Events.Select(envelope => envelope.Payload).OfType<T>().ToArray();
        private void Accept(GameCommand command)
        { var result = Game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native round-recovery command."); }
        public void ReachPlay()
        {
            for (var step = 0; step < 100; step++)
            {
                if (Pending is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }) return;
                Need(Game.State.Winner == Winner.None, "The bounded round-recovery fixture must remain a live small battle.");
                if (Pending is { PlayerSeat: 0, Choices.Count: > 0 } prompt)
                    Accept(new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices[0].Id, Game.Revision));
                else Accept(new AdvanceOneStepCommand(Game.Revision));
            }
            throw new InvalidOperationException("Missing bounded round-recovery Play boundary.");
        }
        public void Hurt(int amount)
        {
            Accept(new UseProgramSkillCommand(0, Driver, "hurt-" + amount, [], [], Game.Revision, Pending!.PromptId));
            ReachPlay();
        }
        public void NextOwnPlay()
        {
            var turn = Game.State.TurnNumber;
            Accept(new EndPlayPhaseCommand(0, Game.Revision, Pending!.PromptId));
            for (var step = 0; step < 100; step++)
            {
                if (Game.State.TurnNumber > turn && Pending is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }) return;
                Need(Game.State.Winner == Winner.None, "The next real round must not require a complete-match simulation.");
                if (Pending is { PlayerSeat: 0, Choices.Count: > 0 } prompt)
                    Accept(new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices[0].Id, Game.Revision));
                else Accept(new AdvanceOneStepCommand(Game.Revision));
            }
            throw new InvalidOperationException("Missing next own round-recovery Play boundary.");
        }
        public static Scene Start(int maximum, int? carriedHp = null, int ordinal = 1)
        {
            var setup = new ShanheBattleSetup(83, ordinal, "fixture:round-recovery", Owner(maximum), carriedHp ?? maximum,
                0, 0, 0, 0, [], [], [], [new(Peer) { StartingHand = [] }], [], new string('a', 64))
                { TacticSkillIds = [Tactic] };
            var engine = GameEngine.CreateStandard(new GameOptions
            {
                Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false,
                MaxTurns = 8, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, Registry.Value);
            var scene = new Scene(engine); scene.Accept(new StartGameCommand()); scene.ReachPlay(); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-round-recovery", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,"activations":[
                    {"id":"hurt-1","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                     "effects":[{"op":"damage","target":"owner","amount":1}]},
                    {"id":"hurt-2","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                     "effects":[{"op":"damage","target":"owner","amount":2}]}]},
                  {"id":"{{Quiet}}","revision":1,"triggers":[
                    {"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,
                     "effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}
                ]}
                """, JsonSerializer.Serialize(new
                {
                    schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new[] { Driver, Quiet }.ToDictionary(id => id,
                        _ => new { name = "真实轮回复驱动", description = "小型原生受伤和正常回合推进" })
                }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            foreach (var maximum in new[] { 1, 2, 4, 6 })
                builder.AddGeneral(new(Owner(maximum), "血战使用者" + maximum, "supporter", Driver, "shu", maximum));
            builder.AddGeneral(new(Peer, "固定守军", "supporter", Quiet, "wei", 10));
            builder.AddDeck(new(Deck, "固定实体闪", 0, 0, [])
                { PhysicalCards = Enumerable.Range(0, 16).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Diamond, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "血战真实小型战斗", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: new[] { 1, 2, 4, 6 }.Select(Owner).Append(Peer).ToArray(), ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
