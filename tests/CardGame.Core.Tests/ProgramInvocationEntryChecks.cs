using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core.Campaigns;

namespace CardGame.Core.Tests;

internal static class ProgramInvocationEntryChecks
{
    private const string Hero = "anniversary:shen-hua-tuo", Luxin = "anniversary-god:luxin";
    private const string Driver = "fixture:shen-ht-driver", Pulse = "fixture:shen-ht-pulse";
    private const string Watch = "fixture:shen-ht-hp-watch", NoDraw = "fixture:shen-ht-no-draw";
    private const string Quiet = "fixture:shen-ht-quiet", Ally = "fixture:shen-ht-a";
    private const string Enemy = "fixture:shen-ht-enemy", Deck = "fixture:shen-ht-deck";

    internal static void LuxinInterruptsNextTrueInvocationAndJingyuCountsEachNameOnce()
    {
        var scene = Scene.Start();
        scene.Play();
        var gift = scene.View(0).Hand.First(card => card.Kind == CardKind.Slash).Id;
        scene.Use(Luxin, "give-and-filter", [gift], [1]);
        scene.Reach(() => scene.Prompt?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("option") == "discard") == true);
        scene.Answer(choice => choice.Parameters.GetValueOrDefault("option") == "discard");
        scene.Play();
        scene.Require(scene.Facts<ShenHuaTuoLuxinArmedEvent>() is [{ TargetSeat: 1, HpDelta: -1 }] &&
            scene.Game.CardMovements.Any(move => move.CardId == gift && move.From == CardLocation.Hand(0) && move.To == CardLocation.Hand(1)) &&
            scene.Game.CardMovements.Count(move => move.From == CardLocation.Hand(1) && move.To == CardLocation.DiscardPile &&
                move.Reason.Value == "skill.shen-hua-tuo.luxin.random-discard") == 1,
            "Luxin genuinely gives a Slash, discards a same-name hand card and arms the recipient's next invocation.");

        for (var use = 0; use < 2; use++)
        {
            scene.Use(Driver, "touch", [], []);
            scene.Play();
        }
        var starts = scene.Facts<SkillInvocationStartedEvent>().Where(e => e.SkillKey == Driver).ToArray();
        scene.Require(starts.Length == 2 && starts.Select(e => e.ParentFrameId).Distinct().Count() == 2 &&
            starts.All(e => e is { ActorSeat: 0, OccurrenceKey: "program:touch" } &&
                scene.Facts<ProgramDrawCompletedEvent>().Count(draw => draw.FrameId == e.ParentFrameId && draw.ActualDrawCount == 1) == 1) &&
            scene.Facts<ShenHuaTuoJingyuReservedEvent>().Count(e => e.SkillName == "发动基准") == 1 &&
            scene.Facts<SkillInvocationStartedEvent>().All(e => e.SkillKey != NoDraw),
            "Two genuine starts each release their own active program, Jingyu reserves that name once, and maintenance never invokes.");

        scene.EndPlay();
        scene.Reach(() => scene.Prompt?.SkillPrompt?.SkillId == Pulse);
        scene.Answer(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        scene.Reach(() => scene.Prompt is { Kind: DecisionKind.RescueDying, PlayerSeat: 0 });
        var parent = scene.Game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Pulse);
        var invocation = scene.Game.ResolutionStack.OfType<SkillInvocationFrame>().Single(frame => frame.SkillKey == Pulse);
        var started = scene.Facts<SkillInvocationStartedEvent>().Single(e => e.SkillKey == Pulse);
        scene.Require(scene.View(1).Hp == 0 && parent.InstructionIndex == 0 &&
            started is { ActorSeat: 1, OccurrenceKey: "program:pulse", ReturnKind: SkillInvocationReturnKind.RuntimeFrame } &&
            started.ParentFrameId == parent.Id && started.FrameId == invocation.Id && invocation.ParentFrameId == parent.Id &&
            started.SkillInstanceId == parent.SkillInstanceId && started.GameplayHash == parent.GameplayHash &&
            !string.IsNullOrEmpty(invocation.SourceFrameFingerprint) &&
            scene.Game.ResolutionStack.OfType<DyingFrame>().Any(frame => frame.Id == invocation.DyingFrameId &&
                frame.ParentFrameId == invocation.Id && frame.VictimSeat == 1 && frame.Continuation == DyingContinuationKind.SkillInvocation) &&
            !scene.Facts<ProgramDrawCompletedEvent>().Any(draw => draw.FrameId == parent.Id),
            "The accepted Pulse is still at instruction zero, with its exact invocation and Luxin dying child above it.");

        scene.Cold();
        scene.Require(scene.Facts<SkillInvocationStartedEvent>().Single(e => e.SkillKey == Pulse) == started &&
            scene.Game.ResolutionStack.OfType<SkillInvocationFrame>().Single(frame => frame.Id == invocation.Id)
                .SourceFrameFingerprint == invocation.SourceFrameFingerprint,
            "Cold restoration retains the same start, parent frame and source fingerprint.");
        scene.Answer(choice => choice.Parameters.GetValueOrDefault("response") == "peach");
        scene.Reach(() => scene.Prompt?.SkillPrompt?.SkillId == Watch);
        scene.Require(scene.View(1).Hp == 1 &&
            scene.Game.ResolutionStack.OfType<ProgramSkillFrame>().Any(frame => frame.Id == parent.Id && frame.InstructionIndex == 0) &&
            scene.Game.ResolutionStack.OfType<SkillInvocationFrame>().Any(frame => frame.Id == invocation.Id && frame.Stage == SkillInvocationStage.Children),
            "The real Peach rescue returns to the original invocation's HP observer before any Pulse instruction runs.");
        scene.Answer(choice => choice.Parameters.GetValueOrDefault("program-action") == "skip");
        scene.Reach(() => scene.Facts<ProgramBindingResolvedEvent>().Any(e => e.FrameId == parent.Id && e.Activated && e.Completed));
        scene.Require(scene.Facts<SkillInvocationStartedEvent>().Count(e => e.ParentFrameId == parent.Id) == 1 &&
            scene.Facts<SkillInvocationCompletedEvent>().Count(e => e.FrameId == invocation.Id && e.ParentFrameId == parent.Id) == 1 &&
            scene.Facts<ShenHuaTuoLuxinConsumedEvent>().Count(e => e.InvocationFrameId == invocation.Id && e.TargetSeat == 1 && e.HpDelta == -1) == 1 &&
            scene.Facts<SkillInvocationHpPaidEvent>().Count(e => e.FrameId == invocation.Id && e.TargetSeat == 1 && e.Before == 1 && e.After == 0) == 1 &&
            scene.Facts<ProgramDrawCompletedEvent>().Count(e => e.FrameId == parent.Id && e.ActualDrawCount == 1) == 1 &&
            scene.Facts<ProgramBindingResolvedEvent>().Count(e => e.FrameId == parent.Id && e.Activated && e.Completed) == 1,
            "The original invocation, Luxin loss and Pulse draw each finish once after the cold return.");
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shen-hua-tuo", new(1, 0, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var rules = $$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,"modifiers":[{"id":"keep","query":"handLimit","operation":"add","value":20,"priority":0}],"activations":[
                    {"id":"touch","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"draw","target":"owner","amount":1}]}]},
                  {"id":"{{NoDraw}}","revision":1,"triggers":[{"id":"no-draw","isSkillInvocation":false,"window":"drawPhaseStarting","subject":"owner","optional":false,"effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]}]},
                  {"id":"{{Pulse}}","revision":1,"triggers":[{"id":"pulse","window":"drawPhaseStarting","subject":"owner","optional":true,"effects":[{"op":"draw","target":"owner","amount":1}]}]},
                  {"id":"{{Watch}}","revision":1,"triggers":[{"id":"watch","window":"afterHpLost","subject":"owner","optional":true,"effects":[{"op":"draw","target":"owner","amount":1}]}]},
                  {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","isSkillInvocation":false,"window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]} ]}
                """;
            var labels = new Dictionary<string, string>
            {
                [Driver] = "发动基准", [Pulse] = "真实脉动", [Watch] = "失体力观察", [Quiet] = "安静同行", [NoDraw] = "无正常摸牌"
            };
            var catalog = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = labels.ToDictionary(pair => pair.Key, pair => new { name = pair.Value, description = pair.Value })
            }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                    { Program = program, ProgramPresentation = catalog.Presentations[id] });

            builder.AddGeneral(new(Ally, "滤心受领者", "", Quiet, "wei", 12, [Pulse, Watch]));
            builder.AddGeneral(new(Enemy, "安静角色", "", Quiet, "wei", 12));
            builder.AddDeck(new(Deck, "固定牌堆", 0, 2, [])
                { PhysicalCards = Enumerable.Range(0, 80).Select(_ => new ContentDeckPhysicalCard("standard:slash", Suit.Club, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "神华佗原生小局", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Ally, Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        internal GameEngine Game { get; private set; } = game;
        internal PendingDecision? Prompt => Enumerable.Range(0, 4).Select(seat => Game.CreateSnapshot(seat).PendingDecision).FirstOrDefault(prompt => prompt is not null);
        internal PlayerSnapshot View(int seat) => Game.CreateSnapshot(seat).Players[seat];
        internal T[] Facts<T>() => Game.Events.Select(e => e.Payload).OfType<T>().ToArray();
        internal void Use(string skill, string activation, IReadOnlyList<int> cards, IReadOnlyList<int> targets) =>
            Accept(new UseProgramSkillCommand(0, skill, activation, cards, targets, Game.Revision, Prompt!.PromptId));
        internal void EndPlay() => Accept(new EndPlayPhaseCommand(0, Game.Revision, Prompt!.PromptId));
        internal void Play() => Reach(() => Prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });

        internal void Answer(Func<PromptChoice, bool> predicate)
        {
            var prompt = Prompt ?? throw new InvalidOperationException("Expected a skill invocation prompt.");
            var choice = prompt.Choices.FirstOrDefault(predicate) ??
                throw new InvalidOperationException("Invocation choice was absent. " + JsonSerializer.Serialize(prompt));
            Accept(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, choice.Id, Game.Revision));
        }

        internal void Reach(Func<bool> done)
        {
            for (var step = 0; step < 120; step++)
            {
                if (done()) return;
                Require(Prompt?.PlayerSeat != 0, "Unexpected human boundary before the requested invocation step.");
                Accept(new AdvanceOneStepCommand(Game.Revision));
            }
            Require(false, "The requested invocation step was not reached.");
        }

        internal void Cold()
        {
            var before = Enumerable.Range(0, 4).Select(seat => SnapshotJson.Serialize(Game.CreateSnapshot(seat))).ToArray();
            Game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(Game.CreateCheckpoint())), registry);
            Require(before.SequenceEqual(Enumerable.Range(0, 4).Select(seat => SnapshotJson.Serialize(Game.CreateSnapshot(seat)))),
                "Cold restore preserves every seat's original pending prompt and state.");
        }

        private void Accept(GameCommand command)
        {
            var result = Game.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single());
            Require(result.Accepted, result.Error?.Message ?? "Invocation fixture command was rejected.");
        }

        internal void Require(bool condition, string message)
        {
            if (!condition)
                throw new InvalidOperationException(message + " " + JsonSerializer.Serialize(new
                {
                    Game.State.CurrentSeat, Game.State.TurnNumber, Game.State.Phase, Prompt,
                    Facts = Game.Events.TakeLast(8).Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray()
                }));
        }

        internal static Scene Start()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var setup = new ShanheBattleSetup(37, 1, "shen-ht-native", Hero, 3, 0, 0, 0, 0, [Driver, NoDraw],
                [new(CardKind.Slash, Suit.Spade, 7), new(CardKind.Slash, Suit.Heart, 8), new(CardKind.Peach, Suit.Heart, 3)], [],
                [new(Enemy) { StartingHand = [] }, new(Enemy) { StartingHand = [] }],
                [new(Ally, Hp: 1, ExtraSkillIds: [NoDraw]) { StartingHand = [new(CardKind.Slash, Suit.Club, 4)] }],
                new string('e', 64)) { DefaultGodFactionId = "wu" };
            var game = GameEngine.CreateStandard(new GameOptions
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false,
                UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 10
            }, registry);
            var scene = new Scene(game, registry);
            scene.Accept(new StartGameCommand());
            return scene;
        }
    }
}
