using System.IO;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Media;
using CardGame.Content.Standard;
using CardGame.Wpf.Controls;
using CardGame.Core.Campaigns;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static class ShanheFengxianPreviewUiChecks
{
    public static void InitialNpcEquipmentAppearsInCardsAndRosterWithoutStartingBattle(string output)
    {
        var published = StandardShanheCampaignCatalog.CreateDefinition(2,
            StandardShanheCampaignCatalog.Returning2026EditionId);
        var selected = published.Nodes.Where(node => node.Kind == ShanheNodeKind.EliteBattle)
            .SelectMany(node => (node.Encounters ?? []).Where(encounter => encounter.Enemies.Any(enemy =>
                enemy.GeneralId == StandardShanheBossPackage.ShenguiLvbuId && enemy.StartingEquipment is { Count: 2 }))
                .Select(encounter => (Node: node, Encounter: encounter))).Single();
        var definition = published with
        {
            Nodes = [selected.Node with { NextNodeIds = [], Encounters = [selected.Encounter] }],
            StartingNodeIds = [selected.Node.Id], RandomStartingNode = false
        };
        var session = ShanheCampaignSession.Create(definition, new(71, "classic:guan-yu"),
            StandardShanheCampaignCatalog.CreateBattleRegistry);
        using var vm = new MainViewModel(false, 270027, true, new MemorySaveStore(), useExpandedContent: true,
            shanheSaveStore: new MemoryShanheCampaignSaveStore(), shanheCodexStore: new MemoryShanheCodexStore())
            { IsMotionEnabled = false, IsSoundEnabled = false, IsMusicEnabled = false };
        typeof(MainViewModel).GetField("_shanheSession", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(vm, session);
        typeof(MainViewModel).GetField("_shanheRunDifficulty", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(vm, 2);
        vm.OpenShanheCommand.Execute(null);
        var node = vm.ShanheMapRows.SelectMany(row => row.Nodes).Single(item => item.IsAvailable);
        var revision = session.Revision;
        var ordinal = session.Snapshot.EncounterOrdinal;
        Program.Assert(session.PendingBattleSetup is null && session.Snapshot.Phase == ShanheCampaignPhase.Map,
            "The selected published encounter begins as a fresh map choice with no pending battle.");
        vm.PreviewShanheNodeCommand.Execute(node);
        const string equipment = "初始装备：【无双方天戟】 · 【束发紫金冠】";
        var enemyIndex = selected.Encounter.Enemies.Select((enemy, index) => (Enemy: enemy, Index: index))
            .Single(item => item.Enemy.GeneralId == StandardShanheBossPackage.ShenguiLvbuId).Index;
        var card = vm.ShanheEncounterEnemyCards[enemyIndex];
        var detail = vm.ShanheEncounterEnemyDetail;
        Program.Assert(vm.IsShanheEncounterPreviewOpen && card is { Name: "吕布·神鬼无前", Portrait: ImageBrush } &&
            card.Stats.Contains(equipment, StringComparison.Ordinal) && detail.Contains(equipment, StringComparison.Ordinal),
            "The real published NPC's preview card and complete roster both disclose its two initially installed physical equipment cards.");
        var panel = new ShanheCampaignPanel { DataContext = vm };
        try
        {
            Program.Render(panel, 1440, 900, Path.Combine(output, "shanhe-npc-opening-equipment-preview.png"));
            var cards = (ItemsControl)panel.FindName("ShanheEncounterEnemyCards");
            var roster = (ScrollViewer)panel.FindName("ShanheEncounterEnemyDetailScroll");
            Program.Assert(Program.Find<TextBlock>(cards).Any(text => ReferenceEquals(text.DataContext, card) &&
                    text.Text == card.Stats && text.ActualHeight > 0) &&
                Program.Find<TextBlock>(roster).Any(text => text.Text == detail && text.ActualHeight > 0),
                "The actual preview card and roster controls render the same initial-equipment text supplied by the live view model.");
            Program.Assert(session.Revision == revision && session.Snapshot.EncounterOrdinal == ordinal &&
                session.PendingBattleSetup is null && session.Snapshot.Phase == ShanheCampaignPhase.Map,
                "Opening and rendering the equipment preview must not submit a campaign move, consume an encounter, or start a battle.");
        }
        finally { panel.DataContext = null; }

        var existing = GeneralArt.GetSkin("shen-lu-bu");
        Program.Assert(existing is not null &&
            GeneralArt.GetSkin(StandardShanheFengxianBossPackage.SourceRageHardId)?.LocalPath == existing.LocalPath &&
            GeneralArt.GetSkin(StandardShanheFengxianBossPackage.SourceRageHellId)?.LocalPath == existing.LocalPath &&
            GeneralArt.GetPortrait(StandardShanheFengxianBossPackage.SourceRageHardId, decodeWidth: 96) is ImageBrush &&
            GeneralArt.GetPortrait(StandardShanheFengxianBossPackage.SourceRageHellId, decodeWidth: 96) is ImageBrush,
            "The hard and hell second forms both resolve and decode the existing local god Lu Bu portrait.");
    }

    public static void SecondFormSkillsAndAutomaticEquipmentAppearBeforeChallenge()
    {
        var published = StandardShanheCampaignCatalog.CreateDefinition(2,
            StandardShanheCampaignCatalog.Returning2026EditionId);
        var boss = published.Nodes.Single(node => node.Id == "huaxiong");
        var encounter = boss.Encounters!.Single(item => item.BossTransition?.GeneralId == "shanhe:lvbu-fengxian-rage");
        var definition = published with
        {
            Nodes = [boss with { NextNodeIds = [], Encounters = [encounter] }],
            StartingNodeIds = [boss.Id], RandomStartingNode = false
        };
        var session = ShanheCampaignSession.Create(definition, new(71, "classic:guan-yu"),
            StandardShanheCampaignCatalog.CreateBattleRegistry);
        using var vm = new MainViewModel(false, 270027, true, new MemorySaveStore(), useExpandedContent: true,
            shanheSaveStore: new MemoryShanheCampaignSaveStore(), shanheCodexStore: new MemoryShanheCodexStore())
            { IsMotionEnabled = false, IsSoundEnabled = false, IsMusicEnabled = false };
        typeof(MainViewModel).GetField("_shanheSession", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(vm, session);
        typeof(MainViewModel).GetField("_shanheRunDifficulty", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(vm, 2);
        vm.OpenShanheCommand.Execute(null);
        var node = vm.ShanheMapRows.SelectMany(row => row.Nodes).Single(item => item.IsAvailable);
        var checkpoint = ShanheCampaignJson.Serialize(session.CreateCheckpoint());
        vm.PreviewShanheNodeCommand.Execute(node);
        var detail = vm.ShanheEncounterEnemyDetail;
        Program.Assert(vm.IsShanheEncounterPreviewOpen && vm.ShanheEncounterEnemyCards.Single() is
                { Name: "吕布", Portrait: ImageBrush } &&
            detail.Contains("暴怒的战神", StringComparison.Ordinal) &&
            detail.Contains("二阶段技能：无双 · 修罗 · 马术", StringComparison.Ordinal) &&
            detail.Contains("阶段转换时自动装备：", StringComparison.Ordinal) &&
            detail.Contains("无双方天戟", StringComparison.Ordinal) && detail.Contains("束发紫金冠", StringComparison.Ordinal) &&
            ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == checkpoint && session.PendingBattleSetup is null,
            "The real boss preview discloses its second-form skills and automatic equipment without starting or changing the campaign.");
        Program.Assert(GeneralArt.GetSkin("shanhe:lvbu-fengxian-first")?.LocalPath == GeneralArt.GetSkin("boundary-lu-bu")?.LocalPath &&
            GeneralArt.GetSkin("shanhe:lvbu-fengxian-rage")?.LocalPath == GeneralArt.GetSkin("shen-lu-bu")?.LocalPath &&
            GeneralArt.GetSkin("shanhe:confident-lvbu")?.LocalPath == GeneralArt.GetSkin("shen-lu-bu")?.LocalPath,
            "Each newly published Lu Bu form reuses the intended existing local portrait.");
    }
}
