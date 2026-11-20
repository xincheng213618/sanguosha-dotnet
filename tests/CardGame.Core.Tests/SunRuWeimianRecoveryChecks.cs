using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.EngineOwners;

internal static class SunRuWeimianRecoveryChecks
{
    private const string Weimian = "ol:weimian", Jiuyuan = "boundary:jiuyuan";
    private const string OwnerGeneral = "fixture:weimian-recovery-owner", Watch = "fixture:weimian-recovery-child";
    private const string Mode = "identity:classic-weimian-recovery", Deck = "fixture:weimian-recovery-deck";

    internal static void ActualWeimianReturnsFromReplacementAndOriginalApplicationCold()
    {
        RunRecovery(withRedirection: true);
        RunRecovery(withRedirection: false);
    }

    private static void RunRecovery(bool withRedirection)
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(withRedirection));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanRole = Role.Renegade, ModeId = Mode,
            UseInteractiveSetup = true, ConfirmOpeningHand = false, AdvanceAfterHumanCommands = false,
            UseInteractiveDiscard = false, MaxTurns = 12
        }, registry);

        PendingDecision? Prompt() => game.CreateSnapshot(0).PendingDecision;
        PlayerSnapshot Owner() => game.CreateSnapshot(0).Players[0];
        T[] Facts<T>() where T : IGameEvent => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();
        void Need(bool condition, string message)
        {
            if (!condition)
                throw new InvalidOperationException($"{message} Turn {game.State.TurnNumber}, phase {game.State.Phase}, " +
                    $"prompt {Prompt()?.Kind}/{Prompt()?.SkillPrompt?.SkillId}.");
        }
        void Submit(GameCommand command)
        {
            var result = game.Submit(command);
            Need(result.Accepted, result.Error?.Message ?? "The actual Weimian command was rejected.");
        }
        void Reach(Func<bool> ready)
        {
            for (var step = 0; step < 160 && !ready(); step++)
            {
                Need(Prompt() is null && game.State.Winner == Winner.None, "The fixed recovery scene reached an unexpected boundary.");
                Submit(new AdvanceOneStepCommand(game.Revision));
            }
            Need(ready(), "The original recovery or selection did not arrive.");
        }
        void Answer(string key, string value, string? branch = null)
        {
            var prompt = Prompt()!;
            var choice = prompt.Choices.Single(candidate => candidate.Parameters.GetValueOrDefault(key) == value &&
                (branch is null || candidate.Parameters.GetValueOrDefault("branch") == branch));
            Submit(new AnswerPromptCommand(0, prompt.PromptId, choice.Id, game.Revision));
        }
        void Restore()
        {
            var old = game;
            game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(old.CreateCheckpoint())), registry);
            Need(Enumerable.Range(0, 4).All(seat => SnapshotJson.Serialize(game.CreateSnapshot(seat)) == SnapshotJson.Serialize(old.CreateSnapshot(seat))) &&
                game.CardMovements.SequenceEqual(old.CardMovements), "Cold restore retains every seat view and physical movement.");
        }

        Submit(new StartGameCommand());
        Reach(() => Prompt() is { Kind: DecisionKind.SelectGeneral, PlayerSeat: 0 });
        Submit(new SelectGeneralCommand(0, OwnerGeneral, game.Revision, Prompt()!.PromptId));
        Reach(() => Prompt() is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        var hpBefore = Owner().Hp;
        Submit(new UseProgramSkillCommand(0, Weimian, "abolish-for-distinct-benefits", [], [0], game.Revision, Prompt()!.PromptId));
        Reach(() => Prompt()?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("branch") == "slots") == true);
        Answer("value", "1", "slots");
        Reach(() => Prompt()?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("branch") == "benefits") == true);
        Answer("value", "2", "benefits");
        Reach(() => Prompt()?.Kind == DecisionKind.RecoveryReplacement || Prompt()?.SkillPrompt?.SkillId == Watch);
        var replacement = game.ResolutionStack.OfType<RecoveryReplacementFrame>().SingleOrDefault();
        var root = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Weimian);
        var paid = Facts<SunRuOlWeimianPaidEvent>().Single();
        var benefit = Facts<SunRuOlWeimianBenefitIssuedEvent>().Single();
        Need(root.SunRuOlWeimian is { Paid: true, Stage: SunRuOlWeimianStage.RecoveryChildren, RecipientSeat: 0 } &&
            benefit.FrameId == root.Id && benefit is { Benefit: 2, RecipientSeat: 0 } && paid.SlotsMask == 1 &&
            (replacement is not null) == withRedirection, "An actual single-slot cost issues one recovery for the original recipient.");
        if (replacement is not null)
        {
            Need(replacement.ParentFrameId == root.Id &&
                replacement.Attempt is { SourceSeat: 0, TargetSeat: 0, Amount: 1, ProgramOrigin: { } origin } &&
                origin.ProgramFrameId == root.Id && replacement.Attempt.Candidates.Any(candidate => candidate.OwnerSeat != 0) &&
                Owner().Hp == hpBefore, "The real lord can redirect the original request before HP is applied.");
            Restore();
            Need(JsonSerializer.Serialize(game.ResolutionStack.OfType<RecoveryReplacementFrame>().Single()) == JsonSerializer.Serialize(replacement),
                "Cold restore keeps the original unreturned recovery request.");
            Answer("recovery-action", "keep");
        }

        Reach(() => Prompt()?.SkillPrompt?.SkillId == Watch);
        var healthParent = replacement?.Id ?? root.Id;
        var health = game.ResolutionStack.OfType<HpChangedTriggerWindowFrame>().Single(frame => frame.ResumeFrameId == healthParent);
        var ownerFrame = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == root.Id);
        Need(health.Change is { Kind: HpChangeKind.Recovery, SourceSeat: 0, TargetSeat: 0, Amount: 1 } &&
            health.Change.ParentFrameId == healthParent &&
            health.Continuation == (replacement is null ? PostEventContinuation.Program : PostEventContinuation.RecoveryReplacement) &&
            (replacement is not null || ownerFrame.RecoveryApplications?.Single().HpChangeId == health.Change.Id) &&
            Owner().Hp == hpBefore + 1 && Facts<RecoveryAppliedEvent>().Count(fact => fact.TargetSeat == 0) == 1,
            "The selected recovery yields one native HP child under the actual owner.");
        var runtime = game.GetRuntimeModule<CharacterSunRuOlRuntime>();
        if (replacement is null)
        {
            Need(runtime.SunRuOlFirstChild(ownerFrame, health) &&
                !runtime.SunRuOlFirstChild(ownerFrame, health with { Change = health.Change with { ParentFrameId = health.Id } }) &&
                !runtime.SunRuOlFirstChild(ownerFrame, health with { ResumeFrameId = health.Id }) &&
                !runtime.SunRuOlFirstChild(ownerFrame, health with { Continuation = PostEventContinuation.RecoveryReplacement }),
                "The actual direct recovery matches only its original Weimian parent, resume frame and Program continuation.");
        }
        else
            Need(!runtime.SunRuOlFirstChild(ownerFrame, health), "The replacement's actual HP child belongs to the replacement frame.");
        Restore();
        Need(JsonSerializer.Serialize(game.ResolutionStack.OfType<HpChangedTriggerWindowFrame>().Single(frame => frame.Id == health.Id)) ==
            JsonSerializer.Serialize(health), "Cold restore keeps the same paid HP child.");
        Answer("option-id", "continue");
        Reach(() => Prompt() is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && game.ResolutionStack.Count == 0);
        Need(Facts<SunRuOlWeimianPaidEvent>().SequenceEqual([paid]) &&
            Facts<SunRuOlWeimianBenefitIssuedEvent>().SequenceEqual([benefit]) && Owner().Hp == hpBefore + 1 &&
            Facts<RecoveryAppliedEvent>().Count(fact => fact.TargetSeat == 0) == 1,
            "Returning the original child settles Weimian without another equipment cost or recovery.");
    }

    private sealed class Fixture(bool withRedirection) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-weimian-recovery", new(1, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "ol:sun-ru", "boundary:sun-quan");
            var watch = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{
                  "id":"{{Watch}}","revision":1,"triggers":[{"id":"native-health","window":"afterHpRecovered","subject":"owner",
                  "optional":false,"isSkillInvocation":false,
                  "effects":[{"op":"chooseOption","target":"owner","resultBind":"health","options":[{"id":"continue"}]}]}]
                }]}
                """, $$"""
                {"schemaVersion":{{SkillProgramCatalog.PresentationSchemaVersion}},"skills":{"{{Watch}}":{
                  "name":"慰勉回复子窗","description":"实际回复后暂停。","optionLabels":{"continue":"继续"} } } }
                """);
            builder.AddSkill(new(Watch, watch.Presentations[Watch].Name, watch.Presentations[Watch].Description)
            {
                Program = watch.Programs[Watch], ProgramPresentation = watch.Presentations[Watch]
            });
            const string humanSelection = "fixture:weimian-human-selection";
            builder.AddSkill(new(humanSelection, "固定真人候选", "固定真人角色")
            {
                SelectionWeights = Enum.GetValues<Role>().ToDictionary(role => role, _ => -10000d)
            });
            builder.AddGeneral(new(OwnerGeneral, "慰勉原生夹具", "ol-sun-ru", humanSelection, "wu", 6, [Weimian, Watch]) { InitialHp = 2 });
            var peers = Enumerable.Range(1, 3).Select(index => $"fixture:weimian-recovery-peer-{index}").ToArray();
            foreach (var peer in peers)
                builder.AddGeneral(new(peer, "固定同桌", "supporter", withRedirection ? Jiuyuan : "standard:none", "wu", 6) { InitialHp = 1 });
            builder.AddDeck(new(Deck, "慰勉固定牌堆", 2, 0, [new("standard:dodge", 48)]));
            builder.AddMode(new(Mode, "慰勉原生回复", 4, 4,
                new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Renegade)] = 3 },
                Deck, GeneralCandidateCount: 4, GeneralPoolIds: [OwnerGeneral, .. peers]));
        }
    }
}
