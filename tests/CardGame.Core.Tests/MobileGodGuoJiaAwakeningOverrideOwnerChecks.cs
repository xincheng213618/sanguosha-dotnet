using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileGodGuoJiaAwakeningOverrideOwnerChecks
{
    private const string Huishi = "mobile:huishi-gift", Baijia = "ol:baijia", Guju = "ol:guju", Canshi = "ol:canshi";
    private const string OwnerGeneral = "fixture:huishi-baijia-owner", Watch = "fixture:huishi-baijia-maximum";
    private const string Mode = "identity:classic-huishi-baijia", Deck = "fixture:huishi-baijia-deck";

    internal static void PaidHuishiWaivesBaijiaThresholdForNextPreparationCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 7, PlayerCount = 4, HumanSeat = 0, HumanRole = Role.Lord, ModeId = Mode,
            UseInteractiveSetup = true, ConfirmOpeningHand = false, UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false, MaxTurns = 8
        }, registry);

        PendingDecision? Pending() => game.CreateSnapshot(0).PendingDecision;
        PlayerSnapshot Owner() => game.CreateSnapshot(0).Players[0];
        T[] Facts<T>() where T : IGameEvent => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();
        bool AtPlay() => Pending() is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && game.ResolutionStack.Count == 0;
        void Need(bool condition, string message)
        {
            if (!condition)
                throw new InvalidOperationException($"{message} Turn {game.State.TurnNumber}, phase {game.State.Phase}, " +
                    $"prompt {Pending()?.Kind}/{Pending()?.SkillPrompt?.SkillId}.");
        }
        void Submit(GameCommand command)
        {
            var result = game.Submit(command);
            Need(result.Accepted, result.Error?.Message ?? "The real Huishi or Baijia command was rejected.");
        }
        void Answer(Func<PromptChoice, bool> pick)
        {
            var prompt = Pending()!;
            Submit(new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices.Single(pick).Id, game.Revision));
        }
        void ContinueHealthChild() => Answer(choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
        void Reach(Func<bool> ready)
        {
            for (var step = 0; step < 155 && !ready(); step++)
            {
                if (Pending()?.SkillPrompt?.SkillId == Watch)
                    ContinueHealthChild();
                else
                {
                    Need(Pending() is null && game.State.Winner == Winner.None, "The awakening fixture reached an unexpected boundary.");
                    Submit(new AdvanceOneStepCommand(game.Revision));
                }
            }
            Need(ready(), "The actual preparation or its health child was not reached.");
        }

        Submit(new StartGameCommand());
        Reach(() => Pending() is { Kind: DecisionKind.SelectGeneral, PlayerSeat: 0 });
        Submit(new SelectGeneralCommand(0, OwnerGeneral, game.Revision, Pending()!.PromptId));
        Reach(AtPlay);
        var firstTurn = game.State.TurnNumber;
        var initialMaximum = Owner().MaxHp;
        Need(initialMaximum >= game.PlayerCount && Owner().Skills?.Any(skill => skill.ContentId == Baijia) == true &&
            Facts<BeiMiHuAwakeningMaximumPaidEvent>().Length == 0 && Facts<BeiMiHuGujuDrawPaidEvent>().Length == 0,
            "The original first Preparation has zero Guju cards and cannot awaken Baijia on its own.");

        var action = game.GetLegalActions(0).Single(candidate => candidate.ProgramSkillId == Huishi && candidate.ProgramActivationId == "grant-awakening");
        Need(action.SelectableTargetSeats.Contains(0), "The actual limited skill offers its own owner as a target.");
        Submit(new UseProgramSkillCommand(0, Huishi, "grant-awakening", [], [0], game.Revision, Pending()!.PromptId));
        Reach(() => Pending()?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("value") == Baijia) == true);
        Answer(choice => choice.Parameters.GetValueOrDefault("value") == Baijia);
        Reach(AtPlay);
        var granted = Facts<ShenGuoJiaAwakeningOverrideEvent>().Single();
        var expense = Facts<ShenGuoJiaMaximumPaidEvent>().Single();
        var reducedMaximum = initialMaximum - 2;
        Need(granted is { Recipient: 0, SkillId: Baijia, Source: { OwnerSeat: 0, SkillId: Huishi, BindingId: "grant-awakening" } } &&
            granted.FrameId == expense.FrameId && expense is { Seat: 0, Purpose: "huishi-gift" } &&
            expense.Before == initialMaximum && expense.After == reducedMaximum && Owner().MaxHp == reducedMaximum &&
            Facts<MaximumHpChangedEvent>().Count(fact => fact is { PlayerSeat: 0, Delta: -2, SkillId: Huishi } && fact.MaximumHp == reducedMaximum) == 1 &&
            Facts<BeiMiHuAwakeningMaximumPaidEvent>().Length == 0,
            "The original self-targeted Huishi pays precisely two maximum HP and grants the chosen Baijia instance for later Preparation.");

        Submit(new EndPlayPhaseCommand(0, game.Revision, Pending()!.PromptId));
        Reach(() => Pending()?.SkillPrompt?.SkillId == Watch && Facts<BeiMiHuAwakeningMaximumPaidEvent>().Length == 1);
        var paid = Facts<BeiMiHuAwakeningMaximumPaidEvent>().Single();
        var awaken = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == paid.FrameId);
        var health = game.ResolutionStack.OfType<HpChangedTriggerWindowFrame>().Single(frame => frame.ResumeFrameId == paid.FrameId);
        var healthBinding = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Watch);
        Need(Facts<TurnStartedEvent>().Where(fact => fact.ActorSeat == 0).Select(fact => fact.TurnNumber).SequenceEqual([firstTurn, paid.Turn]) &&
            awaken.WindowContext is { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow, OwnerSeat: 0 } preparation &&
            preparation.ParentFrameId == paid.ParentFrameId && awaken.BeiMiHu is { Stage: "maximum-children" } &&
            paid.CumulativeActualCards == 0 && Facts<BeiMiHuGujuDrawPaidEvent>().Length == 0 &&
            paid.Source.SkillInstanceId == granted.InstanceId && paid.GameplayHash == granted.GameplayHash && granted.FrameId < paid.FrameId &&
            paid.MaximumBefore == reducedMaximum && paid.MaximumAfter == reducedMaximum + 1 &&
            health.Change is { Kind: HpChangeKind.MaximumHp, TargetSeat: 0, Amount: 1 } && health.Change.ParentFrameId == paid.FrameId &&
            healthBinding.WindowContext is { Window: SkillProgramTriggerWindow.AfterHealthChanged } change && change.HpChange == health.Change &&
            Facts<BeiMiHuAwakeningCompletedEvent>().Length == 0,
            "The exact grant waives only the natural seven-card threshold. The next actual Preparation pays original Baijia and awaits its native maximum-HP child.");

        var before = game;
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(before.CreateCheckpoint())), registry);
        Need(Enumerable.Range(0, 4).All(seat => SnapshotJson.Serialize(game.CreateSnapshot(seat)) == SnapshotJson.Serialize(before.CreateSnapshot(seat))) &&
            game.CardMovements.SequenceEqual(before.CardMovements) && Facts<ShenGuoJiaAwakeningOverrideEvent>().SequenceEqual([granted]) &&
            Facts<BeiMiHuAwakeningMaximumPaidEvent>().SequenceEqual([paid]) &&
            JsonSerializer.Serialize(game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == paid.FrameId)) == JsonSerializer.Serialize(awaken) &&
            JsonSerializer.Serialize(game.ResolutionStack.OfType<HpChangedTriggerWindowFrame>().Single(frame => frame.Id == health.Id)) == JsonSerializer.Serialize(health),
            "Cold replay retains both original facts and the exact paid awakening waiting for its original health child.");

        ContinueHealthChild();
        Reach(AtPlay);
        var completed = Facts<BeiMiHuAwakeningCompletedEvent>().Single();
        Need(completed is { OwnerSeat: 0, LostSkillId: Guju, GrantedSkillId: Canshi } && completed.FrameId == paid.FrameId &&
            Facts<BeiMiHuAwakeningMaximumPaidEvent>().SequenceEqual([paid]) && Facts<ShenGuoJiaMaximumPaidEvent>().SequenceEqual([expense]) &&
            Owner().MaxHp == reducedMaximum + 1 && Owner().Skills?.Any(skill => skill.ContentId == Canshi) == true &&
            Owner().Skills?.All(skill => skill.ContentId != Guju) == true,
            "Returning that child completes the original awakening once with its normal skill replacement and no second expense.");
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-huishi-baijia-owner", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "mobile:shen-guo-jia", "ol:bei-mi-hu");
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Watch}}","revision":1,"triggers":[
                  {"id":"maximum-paid","window":"afterHealthChanged","subject":"owner",
                    "condition":{"kind":"healthChangeKindIs","hpChangeKind":"maximumHp"},"optional":false,"isSkillInvocation":false,
                    "effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]}
                ]}]}
                """, $$"""
                {"schemaVersion":{{SkillProgramCatalog.PresentationSchemaVersion}},"skills":{"{{Watch}}":{
                  "name":"体力上限已支付","description":"实际体力上限变化后暂停。","optionLabels":{"continue":"继续"} } } }
                """);
            builder.AddSkill(new(Watch, catalog.Presentations[Watch].Name, catalog.Presentations[Watch].Description)
                { Program = catalog.Programs[Watch], ProgramPresentation = catalog.Presentations[Watch] });
            builder.AddGeneral(new(OwnerGeneral, "辉逝拜假原生夹具", "supporter", Huishi, "wei", 6, [Baijia, Guju, Watch]));
            var peers = Enumerable.Range(1, 3).Select(index => $"fixture:huishi-baijia-peer-{index}").ToArray();
            foreach (var peer in peers)
                builder.AddGeneral(new(peer, "固定同桌", "supporter", "standard:none", "wei", 8));
            builder.AddDeck(new(Deck, "辉逝拜假固定牌堆", 4, 0, [new("standard:dodge", 40)]));
            builder.AddMode(new(Mode, "辉逝拜假真实准备阶段", 4, 4,
                new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 3 },
                Deck, GeneralCandidateCount: 4, GeneralPoolIds: [OwnerGeneral, .. peers]));
        }
    }
}
