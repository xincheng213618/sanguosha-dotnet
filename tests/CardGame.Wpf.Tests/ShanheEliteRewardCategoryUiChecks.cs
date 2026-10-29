using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Wpf.Controls;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static partial class ShanheRewardUiChecks
{
    public static void AutomaticRareEliteOffersAllowCardCompensationAndRequireTacticClaim(string output)
    {
        var tactics = Enumerable.Range(1, 3).Select(index => new ShanheRewardDefinition("rare-tactic-" + index,
            "精英军阵" + index, Bonuses: new(Draw: index)) { Rarity = 2 }).ToArray();
        var cards = new[] { CardKind.FiveGrains, CardKind.Duel, CardKind.Peach }.Select((kind, index) =>
            new ShanheRewardDefinition("rare-card-" + index, CardCatalog.Get(kind).DisplayName,
                StartingCards: [new(kind)]) { Rarity = 2 }).ToArray();
        ShanheRewardDefinition[] common =
        [
            new("common-tactic", "普通军阵", Bonuses: new(Draw: 1)) { Rarity = 1 },
            new("common-card", "普通随身杀", StartingCards: [new(CardKind.Slash)]) { Rarity = 1 }
        ];
        var rewards = tactics.Concat(cards).Concat(common).ToArray();
        var definition = new ShanheCampaignDefinition("fixture:shanhe-rare-elite-rewards", "精英凯旋", [Player], ["elite"],
        [
            new("elite", "精英守军", ShanheNodeKind.EliteBattle, ["home"],
                [new("guard", "守军", [new(Enemy)], VictoryGold: 200)], rewards.Select(reward => reward.Id).ToArray())
            {
                CategorizedRewards = true, AutomaticRewardCategory = true, RewardChoiceCount = 3, RewardSkipGold = 100,
                PrimaryTacticRarity = 2, PrimarySkillRarity = 2, PrimaryCardRarity = 2
            },
            new("home", "营地", ShanheNodeKind.Camp, [], Choices: [new("rest", "休整")])
        ], rewards, StartingGold: 10);
        var save = new MemoryShanheCampaignSaveStore();
        using var vm = new MainViewModel(false, 270028, true, new MemorySaveStore(),
            contentRegistry: FixtureRegistry.Value, shanheSaveStore: save,
            shanheCodexStore: new MemoryShanheCodexStore());
        var panel = new ShanheCampaignPanel { DataContext = vm };
        try
        {
            var session = EliteRewardStage(ShanheRewardCategory.Card); Attach(vm, session); Layout(panel);
            var confirm = (Button)panel.FindName("ShanheConfirmRewardButton");
            Program.Assert(session.Snapshot.Gold == 210 && session.Snapshot.RewardOffers.Count == 3 &&
                session.Snapshot.RewardCategories.Count == 0 && vm.ShanheRewardCards.All(card => card.RewardCategoryChoice is null),
                "The elite reward stage opens its predetermined rare card candidates directly, without a category-selection page.");
            var physical = vm.ShanheRewardCards.Where(card => !card.IsSkip).ToArray();
            var skip = vm.ShanheRewardCards.Single(card => card.IsSkip);
            var renderPath = Path.Combine(output, "shanhe-rare-elite-card-compensation.png");
            Program.Render(panel, 1440, 880, renderPath);
            Program.Assert(physical.Length == 3 && physical.All(card => card.Rarity == 2 && card.HasCardImage &&
                    card.HeadingText == "稀有 · 随身牌") && session.Snapshot.RewardOffers.All(reward => reward.Rarity == 2) &&
                physical.All(card => Program.Find<TextBlock>(panel).Any(text => text.Text == card.HeadingText &&
                    text.Visibility == Visibility.Visible && text.ActualHeight > 0)) &&
                skip.Name.Contains("稀有随身牌", StringComparison.Ordinal) && skip.BonusText.Contains("100", StringComparison.Ordinal) && File.Exists(renderPath),
                "The predetermined card reward shows three actual rare physical cards, rendered rare type labels and the separate hundred-coin decline.");
            var gold = session.Snapshot.Gold;
            Click(Bound(panel, vm.SelectShanheRewardCardCommand, skip), panel);
            Click(confirm, panel);
            Program.Assert(session.Snapshot.Gold == gold + 100 && session.Snapshot.StartingCards.Count == 0 &&
                session.Snapshot.Phase == ShanheCampaignPhase.Map && session.Snapshot.AvailableNodes.Single().Id == "home" &&
                session.AcceptedCommands.OfType<SkipShanheRewardCommand>().Count() == 1,
                "The actual decline and confirmation buttons pay once without granting a card and open the next route.");

            session = EliteRewardStage(ShanheRewardCategory.Tactic); Attach(vm, session); Layout(panel);
            Program.Assert(vm.ShanheRewardCards.Count == 3 && vm.ShanheRewardCards.All(card => !card.IsSkip &&
                    card.RewardCategoryChoice is null && card.Rarity == 2 && card.HeadingText == "稀有 · 战法") &&
                session.Snapshot.RewardCategories.Count == 0 && !session.Snapshot.CanSkipReward &&
                vm.ShanheRewardNotice.Contains("必须领取", StringComparison.Ordinal),
                "A predetermined tactic reward directly shows three rare tactics and keeps tactic compensation unavailable.");
            var chosen = vm.ShanheRewardCards[0];
            gold = session.Snapshot.Gold;
            Click(Bound(panel, vm.SelectShanheRewardCardCommand, chosen), panel);
            Click(confirm, panel);
            Program.Assert(session.Snapshot.Gold == gold && session.Snapshot.Bonuses.Draw > 0 &&
                session.Snapshot.Phase == ShanheCampaignPhase.Map && !session.AcceptedCommands.OfType<SkipShanheRewardCommand>().Any(),
                "The mandatory tactic is granted through its real confirmation without also paying decline compensation.");

        }
        finally { panel.DataContext = null; }

        ShanheCampaignSession EliteRewardStage(ShanheRewardCategory category)
        {
            // Bounded map-only probes choose deterministic coverage without
            // constructing or driving a battle engine for seed exploration.
            var session = Enumerable.Range(1, 10).Select(seed => ShanheCampaignSession.Create(definition,
                new(seed, Player), _ => FixtureRegistry.Value)).First(candidate => candidate.PreviewRewardCategory("elite") == category);
            Accept(session, new MoveShanheNodeCommand("elite", session.Revision));
            var node = session.Definition.Nodes.Single(node => node.Id == "elite");
            PublishPrimaryRewardStageForUi(session, node, definition.StartingGold + 200);
            return session;
        }
    }

    public static void WolfPreviewUsesTheEffectiveEliteRewardProfile(string output)
    {
        const string playerId = "classic:guan-yu", enemyId = "classic:ma-chao";
        var common = new ShanheRewardDefinition("common", "普通军阵", Bonuses: new(Draw: 1)) { Rarity = 1 };
        var rare = Enumerable.Range(1, 3).Select(index => new ShanheRewardDefinition("rare-" + index,
            "稀有军阵" + index, Bonuses: new(Draw: index)) { Rarity = 2 }).ToArray();
        var wolf = new ShanheRewardDefinition("wolf", "狼战") { CampaignEconomy = new(UpgradeOrdinaryBattles: true) };
        var definition = new ShanheCampaignDefinition("fixture:shanhe-wolf-preview", "狼战预览", [playerId], ["normal", "elite"],
        [
            new("normal", "普通驻军", ShanheNodeKind.Battle, ["home"], [new("ordinary", "普通敌军", [new(enemyId)])], [common.Id])
            {
                EliteUpgradeNodeId = "elite", PrimaryTacticRarity = 1, CategorizedRewards = true, AutomaticRewardCategory = true,
                StoryText = "普通剧情"
            },
            new("elite", "精英驻军", ShanheNodeKind.EliteBattle, ["home"],
                [new("elite-enemy", "精英敌军", [new(enemyId, MaxHpBonus: 2)])], rare.Select(reward => reward.Id).ToArray())
            {
                PrimaryTacticRarity = 2, CategorizedRewards = true, AutomaticRewardCategory = true, RewardChoiceCount = 3,
                StoryText = "精英剧情"
            },
            new("home", "营地", ShanheNodeKind.Camp, [], Choices: [new("rest", "休整")])
        ], rare.Concat([common, wolf]).ToArray(), StartingGold: 10);
        var session = ShanheCampaignSession.Create(definition, new(71, playerId) { EquippedTacticIds = [wolf.Id] },
            CardGame.Content.Standard.StandardShanheCampaignCatalog.CreateBattleRegistry);
        var before = session.Snapshot;
        var profile = session.PreviewBattleNode("normal");
        Program.Assert(profile.Id == "normal" && profile.Kind == ShanheNodeKind.EliteBattle && profile.PrimaryTacticRarity == 2 &&
            profile.RewardIds!.SequenceEqual(rare.Select(reward => reward.Id)) && ReferenceEquals(before, session.Snapshot) &&
            session.Revision == 0 && session.AcceptedCommands.Count == 0 && session.PendingBattleSetup is null,
            "The public read-only preview returns the effective elite reward profile on its original route without entering a battle.");
        var save = new MemoryShanheCampaignSaveStore();
        using var vm = new MainViewModel(false, 270028, true, new MemorySaveStore(),
            contentRegistry: FixtureRegistry.Value, shanheSaveStore: save,
            shanheCodexStore: new MemoryShanheCodexStore());
        var panel = new ShanheCampaignPanel { DataContext = vm };
        try
        {
            Attach(vm, session); Layout(panel);
            vm.PreviewShanheNodeCommand.Execute(vm.ShanheMapRows.SelectMany(row => row.Nodes).Single(node => node.Id == "normal"));
            var text = vm.ShanheEncounterRewardText;
            var journal = ShanheCampaignJson.Serialize(session.CreateCheckpoint());
            var game = Program.Engine(vm);
            var gameJournal = GameCheckpointJson.Serialize(game.CreateCheckpoint());
            var renderPath = Path.Combine(output, "shanhe-wolf-effective-reward-preview.png");
            Program.Render(panel, 1440, 880, renderPath);
            var rewardSummary = vm.ShanheEncounterRewardSummary;
            Program.Assert(rewardSummary.Count == 2 && rewardSummary[0] is { Title: "基础铜币", ValueText: "× 0" } &&
                    rewardSummary[1] is { Title: "稀有战法", ValueText: "基础 3 选一" } &&
                    rewardSummary[1].Detail.Contains("战后以实际候选为准", StringComparison.Ordinal),
                "The compact upgraded preview uses the effective rare tactic baseline rather than the ordinary reward profile or already-frozen offer claims.");
            ShanheCampaignUiChecks.AssertEncounterRewardIcon(rewardSummary[0], "Assets/Shanhe/Encounter/14021801.png");
            ShanheCampaignUiChecks.AssertEncounterRewardIcon(rewardSummary[1], "Assets/Wushuang/rogue_award_type_icon_2.png");
            ShanheCampaignUiChecks.AssertEncounterRewardSummaryLayout(vm, panel, panel);
            Program.Assert(ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == journal &&
                    ReferenceEquals(Program.Engine(vm), game) && GameCheckpointJson.Serialize(game.CreateCheckpoint()) == gameJournal && !save.Exists,
                "Reading the effective compact reward baseline and complete rule text does not alter either game or create a save.");
            Program.Assert(vm.IsShanheEncounterPreviewOpen && text.Contains("稀有战法（3选一）", StringComparison.Ordinal) &&
                !text.Contains("普通战法", StringComparison.Ordinal) && !text.Contains("先选择类别", StringComparison.Ordinal) &&
                vm.ShanheEncounterDescription == "精英剧情" &&
                Program.Find<TextBlock>(panel).Any(block => block.Text == text && block.ActualHeight > 0) &&
                File.Exists(renderPath) && ReferenceEquals(before, session.Snapshot) && session.AcceptedCommands.Count == 0,
                "The rendered wolf-upgraded encounter uses its elite story and rare automatic reward preview instead of the original ordinary profile.");
            Accept(session, new MoveShanheNodeCommand("normal", session.Revision));
            var entered = session.Snapshot.CurrentBattleNode!;
            Program.Assert(entered.PrimaryTacticRarity == profile.PrimaryTacticRarity && entered.RewardIds!.SequenceEqual(profile.RewardIds!),
                "The actual move locks the same effective reward profile that was previewed.");
            PublishPrimaryRewardStageForUi(session, entered, definition.StartingGold + 200);
            Attach(vm, session); Layout(panel);
            Program.Assert(session.Snapshot.RewardOffers.Count == 3 && session.Snapshot.RewardOffers.All(reward => reward.Rarity == 2) &&
                session.Snapshot.RewardCategories.Count == 0 && vm.ShanheRewardCards.All(card => !card.IsSkip &&
                    card.RewardCategoryChoice is null && card.HeadingText == "稀有 · 战法"),
                "The production reward planner and actual reward controls agree with the preview's rare tactic tier and direct offer.");
        }
        finally { panel.DataContext = null; }
        VerifyRestrictedAutomaticRewardPreview();
    }

    private static void VerifyRestrictedAutomaticRewardPreview()
    {
        var published = StandardShanheCampaignCatalog.CreateSourceCodexDefinition(1);
        var rewards = published.Rewards.ToDictionary(reward => reward.Id, StringComparer.Ordinal);
        var node = published.Nodes.First(candidate => candidate.Kind == ShanheNodeKind.Battle &&
            candidate.ChapterNumber is 1 or 2 && candidate.ChapterRole == ShanheChapterNodeRole.OpeningBattle && candidate.AutomaticRewardCategory &&
            (candidate.RewardIds ?? []).Any(id => rewards[id].SkillId is not null &&
                (candidate.PrimarySkillRarity == 0 || rewards[id].Rarity == candidate.PrimarySkillRarity)));
        var skill = node.RewardIds!.Select(id => rewards[id]).First(reward => reward.SkillId is not null &&
            (node.PrimarySkillRarity == 0 || reward.Rarity == node.PrimarySkillRarity));
        // A single-node local UI fixture retains the published encounter names,
        // bodies and skill definition. Only navigation and the reward pool are
        // restricted; this does not verify a production source route or distribution.
        var restricted = node with
        {
            NextNodeIds = [], EliteUpgradeNodeId = null, ChapterRouteId = null,
            ChapterRole = ShanheChapterNodeRole.None, ChapterProgressStep = 0,
            RequiredChapterProgress = 0, ExtraChapterBattleIndex = 0, RewardIds = [skill.Id]
        };
        var definition = new ShanheCampaignDefinition("fixture:restricted-automatic-reward-preview", "受限奖励预览",
            ["classic:guan-yu"], [restricted.Id], [restricted], [skill]);
        var session = ShanheCampaignSession.Create(definition, new(71, "classic:guan-yu")
            { UseCodexRewardPool = true, UnlockedRewardIds = [] }, StandardShanheCampaignCatalog.CreateBattleRegistry);
        Program.Assert(session.PreviewRewardCategory(node.Id) is null,
            "The public planner reports an undetermined category when the restricted automatic pool contains only a locked existing skill.");
        var save = new MemoryShanheCampaignSaveStore();
        using var vm = new MainViewModel(false, 71, true, new MemorySaveStore(), useExpandedContent: true,
            shanheSaveStore: save, shanheCodexStore: new MemoryShanheCodexStore());
        var panel = new ShanheCampaignPanel { DataContext = vm };
        try
        {
            vm.SelectedShanheScenario = vm.ShanheScenarios.Single(option => option.Id == StandardShanheCampaignCatalog.LocalEditionId);
            Attach(vm, session); Layout(panel);
            var journal = ShanheCampaignJson.Serialize(session.CreateCheckpoint());
            var game = Program.Engine(vm);
            var gameJournal = GameCheckpointJson.Serialize(game.CreateCheckpoint());
            var marker = vm.ShanheMapRows.SelectMany(row => row.Nodes).Single(candidate => candidate.Id == node.Id);
            Program.Assert(session.Snapshot.AvailableNodes.Single().Id == node.Id && marker.IsAvailable &&
                    vm.PreviewShanheNodeCommand.CanExecute(marker),
                "The restricted local fixture exposes its real available map node to the ordinary preview command without bypassing chapter or command gates.");
            vm.PreviewShanheNodeCommand.Execute(marker);
            Layout(panel);
            var summary = vm.ShanheEncounterRewardSummary;
            Program.Assert(vm.IsShanheEncounterPreviewOpen && summary.Count == 1 && summary[0].Title == "基础铜币" &&
                    vm.ShanheEncounterRewardText.Contains("战利品候选以战后实际结算为准", StringComparison.Ordinal) &&
                    !vm.ShanheEncounterRewardText.Contains("已在本地点确定", StringComparison.Ordinal) &&
                    !vm.ShanheEncounterRewardText.Contains("选一", StringComparison.Ordinal),
                "An undetermined automatic category shows copper and conditional full rules without inventing a frozen reward class or candidate count.");
            ShanheCampaignUiChecks.AssertEncounterRewardIcon(summary[0], "Assets/Shanhe/Encounter/14021801.png");
            ShanheCampaignUiChecks.AssertEncounterRewardSummaryLayout(vm, panel, panel);
            Program.Assert(ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == journal && session.PendingBattleSetup is null &&
                    ReferenceEquals(Program.Engine(vm), game) && GameCheckpointJson.Serialize(game.CreateCheckpoint()) == gameJournal && !save.Exists,
                "Reading the restricted-pool fallback neither enters battle nor changes either game or creates a save.");
        }
        finally { panel.DataContext = null; }
    }

    private static void PublishPrimaryRewardStageForUi(ShanheCampaignSession session, ShanheNodeDefinition node, int gold)
    {
        // The real Move supplies the locked node. Seed this UI fixture's reward
        // boundary on a detached record and use the unchanged production planner.
        var type = typeof(ShanheCampaignSession);
        var fields = BindingFlags.Instance | BindingFlags.NonPublic;
        var stateField = type.GetField("_state", fields)!;
        var original = stateField.GetValue(session)!;
        var state = original.GetType().GetMethod("<Clone>$", BindingFlags.Instance | BindingFlags.Public)!
            .Invoke(original, null)!;
        state.GetType().GetProperty("BattleSetup")!.SetValue(state, null);
        state.GetType().GetProperty("Gold")!.SetValue(state, gold);
        var next = type.GetMethod("BeginBattlePrimaryRewards", fields)!.Invoke(session, [state, node])!;
        var snapshot = type.GetMethod("CreateSnapshot", fields)!.Invoke(session, [next, session.Revision]);
        stateField.SetValue(session, next);
        type.GetField("_snapshot", fields)!.SetValue(session, snapshot);
    }
}
