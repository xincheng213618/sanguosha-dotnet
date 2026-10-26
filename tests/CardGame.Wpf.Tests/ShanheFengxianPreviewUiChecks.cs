using System.Reflection;
using System.Windows.Media;
using CardGame.Content.Standard;
using CardGame.Core.Campaigns;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static class ShanheFengxianPreviewUiChecks
{
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
