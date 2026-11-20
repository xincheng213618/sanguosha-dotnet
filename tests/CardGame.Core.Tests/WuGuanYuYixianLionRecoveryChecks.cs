using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Core.EngineOwners;
using static BoundaryLiDianChecks;

internal static class WuGuanYuYixianLionRecoveryChecks
{
    private const string Hero = "wumiao:guan-yu", Yixian = "wumiao:yixian";
    private const string Peer = "fixture:yixian-lion-peer", Watch = "fixture:yixian-lion-recovery", Deck = "fixture:yixian-lion-deck";
    private const string Obtain = "skill-program.wu-guan-yu.yixian.obtain";

    internal static void FieldLionRecoversOriginalForeignOwnerCold()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(37, 1, "yixian-lion", Hero, 5, 0, 0, 0, 0, [], [], [],
            [new(Peer, Hp: 3) { StartingHand = [], StartingEquipment = [new(CardKind.SilverLion, Suit.Club, 1)] }], [], new string('a', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 3
        }, content);

        T[] Facts<T>() where T : IGameEvent => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();
        PendingDecision? Prompt(int seat) => game.CreateSnapshot(seat).PendingDecision;
        bool AtPlay() => Prompt(0)?.Kind == DecisionKind.PlayCard;
        void Reach(Func<bool> ready)
        {
            for (var step = 0; step < 60 && !ready() && game.State.Winner == Winner.None; step++)
            {
                Require(Prompt(0) is null, "The fixed Yixian scene did not expect another human choice.");
                Accept(game, new AdvanceOneStepCommand(game.Revision));
            }
            Require(ready(), $"Missing Yixian boundary at turn {game.State.TurnNumber}, phase {game.State.Phase}.");
        }
        void AnswerOwner(string branch)
        {
            var prompt = Prompt(0)!;
            var choice = prompt.Choices.Single(item => item.Parameters.GetValueOrDefault("branch") == branch);
            Accept(game, new AnswerPromptCommand(0, prompt.PromptId, choice.Id, game.Revision));
        }

        Accept(game, new StartGameCommand());
        Reach(AtPlay);
        var original = game.CreateSnapshot(0).Players[1];
        var lion = original.Equipment.Single(card => card.Kind == CardKind.SilverLion).Id;
        Require(original is { Hp: 3, MaxHp: 4 } && game.CreateSnapshot(0).Players[0].Equipment.Count == 0,
            "The fixed table has one physical Lion on the original wounded foreign owner.");

        Accept(game, new UseProgramSkillCommand(0, Yixian, "all-equipment", [], [], game.Revision, Prompt(0)!.PromptId));
        Reach(() => Prompt(0)?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("branch") == "field") == true);
        AnswerOwner("field");
        Reach(() => Prompt(1)?.SkillPrompt?.SkillId == Watch);
        var paid = Facts<YixianPaidEvent>().Single();
        var parent = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == paid.FrameId);
        var hp = game.ResolutionStack.OfType<HpChangedTriggerWindowFrame>().Single(frame => frame.ResumeFrameId == parent.Id);
        var watcher = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Watch);
        var transfer = Facts<YixianEntityObtainedEvent>().Single();
        Require(paid is { Branch: YixianBranch.Field, EntityCount: 1, RecipientCount: 1 } &&
            parent.Yixian is { Paid: true, Stage: YixianStage.EquipmentChildren } &&
            transfer.FrameId == parent.Id && transfer.CardId == lion && transfer.PrintedKind == CardKind.SilverLion &&
            transfer.From == CardLocation.Equipment(1) && transfer.To == CardLocation.Hand(0) &&
            Facts<YixianRecipientFrozenEvent>().Single() == new YixianRecipientFrozenEvent(parent.Id, 0, 1, 1) &&
            hp.Change is { Kind: HpChangeKind.Recovery, SourceSeat: 1, TargetSeat: 1, Amount: 1, HpBefore: 3, HpAfter: 4 } &&
            hp.Change.ParentFrameId == parent.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement &&
            watcher.WindowContext is { Window: SkillProgramTriggerWindow.AfterHpRecovered } context && context.ParentFrameId == hp.Id &&
            game.GetRuntimeModule<CharacterGuanYuWumiaoRuntime>().WuYixianFirstChild(parent, hp) &&
            Facts<SilverLionRemovedRecoveryEvent>().Single() is { PlayerSeat: 1, RecoveredAmount: 1, RemainingHp: 4 } recovery &&
            recovery.Reason.Value == Obtain && game.CreateSnapshot(0).Players[0].Hand.Any(card => card.Id == lion),
            "The paid field transfer owns the actual original Lion owner's recovery child before compensation.");

        var savedWindow = JsonSerializer.Serialize(hp);
        var promptId = Prompt(1)!.PromptId;
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        var restoredParent = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == parent.Id);
        var restoredHp = game.ResolutionStack.OfType<HpChangedTriggerWindowFrame>().Single(frame => frame.Id == hp.Id);
        var runtime = game.GetRuntimeModule<CharacterGuanYuWumiaoRuntime>();
        Require(Prompt(1)?.PromptId == promptId && JsonSerializer.Serialize(restoredHp) == savedWindow &&
            runtime.WuYixianFirstChild(restoredParent, restoredHp) &&
            !runtime.WuYixianFirstChild(restoredParent, restoredHp with { Continuation = PostEventContinuation.Program }) &&
            !runtime.WuYixianFirstChild(restoredParent, restoredHp with { Change = restoredHp.Change with { SourceSeat = 0, TargetSeat = 0 } }),
            "Cold replay preserves the paid health child and rejects another continuation or recipient.");
        Accept(game, new AdvanceOneStepCommand(game.Revision));
        Reach(() => Prompt(0)?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("branch") == "skip" && choice.Targets.SequenceEqual([1])) == true);
        AnswerOwner("skip");
        Reach(AtPlay);
        Require(Facts<YixianPaidEvent>().SequenceEqual([paid]) && Facts<YixianEntityObtainedEvent>().SequenceEqual([transfer]) &&
            Facts<YixianCompletedEvent>().Single() == new YixianCompletedEvent(parent.Id, true, YixianBranch.Field, 1, 0) &&
            Facts<RecoveryAppliedEvent>().SequenceEqual([new RecoveryAppliedEvent(1, 1, 1, 4)]) &&
            Facts<SilverLionRemovedRecoveryEvent>().Length == 1 && game.CardMovements.Count(move => move.Reason.Value == Obtain && move.CardId == lion) == 1,
            "The original Yixian returns once without repeating the physical Lion transfer or recovery.");
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-yixian-lion-recovery", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var catalog = SkillProgramCatalog.LoadCurrent($$"""
                {"skills":[{"id":"{{Watch}}","revision":1,"triggers":[{"id":"lion-recovered","window":"afterHpRecovered",
                "subject":"owner","optional":false,"isSkillInvocation":false,
                "effects":[{"op":"chooseOption","target":"owner","resultBind":"health","options":[{"id":"continue"}]}]}]}]}
                """, JsonSerializer.Serialize(new
                {
                    schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object>
                    {
                        [Watch] = new { name = "白银狮子回复", description = "在原生回复后继续。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } }
                    }
                }));
            builder.AddSkill(new(Watch, catalog.Presentations[Watch].Name, catalog.Presentations[Watch].Description)
                { Program = catalog.Programs[Watch], ProgramPresentation = catalog.Presentations[Watch] });
            builder.AddGeneral(new(Peer, "白银狮子原持有者", "supporter", Watch, "wei", 4));
            var lion = CardCatalog.Get(CardKind.SilverLion);
            builder.AddCard(new("fixture:yixian-silver-lion", lion.DisplayName, lion.CategoryName, lion.Description, CardKind.SilverLion));
            builder.AddDeck(new(Deck, "义贤固定牌堆", 0, 2, [new("standard:dodge", 24)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "义贤实际回复", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
