using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using CardGame.Content.Standard;
using CardGame.Core.Campaigns;
using CardGame.Wpf.Controls;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static class ShanheCompanionRosterUiChecks
{
    public static void BothOpeningCompanionsShowTheirOwnNamesAndRules()
    {
        var ordinary = StandardShanheCampaignCatalog.OpeningCompanionRewardId;
        var elite = StandardShanheCampaignCatalog.EliteOpeningCompanionRewardId;
        var definition = new ShanheCampaignDefinition("fixture:dual-companion-ui", "双同伴", ["classic:guan-yu"], ["battle"],
            [new("battle", "下一场", ShanheNodeKind.Battle, [], [new("enemy", "敌军", [new("classic:cao-cao")])])],
            [new(ordinary, "和光盟") { OpeningCompanionPool = [new(StandardShanheCompanionPackage.ManzhuId)] },
             new(elite, "和光盟Ⅱ") { OpeningCompanionPool = [new(StandardShanheExpandedCompanionPackage.ZiyuanId)] }]);
        var session = ShanheCampaignSession.Create(definition, new(71, "classic:guan-yu") { EquippedTacticIds = [ordinary, elite] },
            StandardShanheCampaignCatalog.CreateBattleRegistry);
        using var model = new MainViewModel(false, 71, true, new MemorySaveStore(),
            shanheSaveStore: new MemoryShanheCampaignSaveStore(), shanheCodexStore: new MemoryShanheCodexStore());
        typeof(MainViewModel).GetField("_shanheSession", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(model, session);
        typeof(MainViewModel).GetMethod("RefreshShanhePresentation", BindingFlags.NonPublic | BindingFlags.Instance)!.Invoke(model, null);
        var panel = new ShanhePlayerHud { DataContext = model };
        try
        {
            panel.Measure(new Size(430, 235)); panel.Arrange(new Rect(0, 0, 430, 235)); panel.UpdateLayout();
            var rows = (ItemsControl)panel.FindName("ShanheStartingCompanionRows");
            Program.Assert(rows.Items.Count == 2 && rows.ActualHeight > 0 &&
                model.ShanheStartingCompanionCards[0].Name == "曼珠" && model.ShanheStartingCompanionCards[1].Name.Contains("紫鸢"),
                $"The HUD creates separate visible rows for ordinary and elite opening companions (rows={rows.Items.Count}, height={rows.ActualHeight}, names={model.ShanheStartingCompanionName}).");
            Program.Assert(model.ShanheStartingCompanionCards[0].Description.Contains("龙吟") &&
                model.ShanheStartingCompanionCards[1].Description.Contains("合击") &&
                Program.Find<TextBlock>(rows).Any(text => text.Text == "曼珠") &&
                Program.Find<TextBlock>(rows).Any(text => text.Text.Contains("紫鸢")),
                "Each row shows its own public name and native rules instead of only the first companion.");
        }
        finally { panel.DataContext = null; }
    }
}
