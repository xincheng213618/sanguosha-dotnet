using System.Reflection;
using System.Windows;
using System.Runtime.InteropServices;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interop;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Wpf.Controls;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static class ShanheCompanionRosterUiChecks
{
    public static void BothOpeningCompanionsShowTheirOwnNamesAndRules(string output)
    {
        var ordinary = StandardShanheCampaignCatalog.OpeningCompanionRewardId;
        var elite = StandardShanheCampaignCatalog.EliteOpeningCompanionRewardId;
        var definition = new ShanheCampaignDefinition("fixture:dual-companion-ui", "双同伴", ["classic:guan-yu"], ["prepare"],
            [new("prepare", "行装", ShanheNodeKind.Shop, ["battle"], Choices:
                [new("card", "取得手牌", RewardId: "hand", LeavesNode: false),
                 new("skill", "习得英姿", RewardId: "skill", LeavesNode: false), new("continue", "继续")]),
             new("battle", "下一场", ShanheNodeKind.Battle, [], [new("enemy", "敌军", [new("classic:cao-cao")])])],
            [new(ordinary, "和光盟") { OpeningCompanionPool = [new(StandardShanheCompanionPackage.ManzhuId)] },
             new(elite, "和光盟Ⅱ") { OpeningCompanionPool = [new(StandardShanheExpandedCompanionPackage.ZiyuanId)] },
             new(StandardShanheCampaignCatalog.CurrentDailyTravelRewardId, "日行千里Ⅰ")
                { CampaignEconomy = new(ExtraChapterBattles: 1), Rarity = 2, Description = "每章额外挑战一场战斗。" },
             new("hand", "随身手牌", StartingCards: [new(CardKind.Slash, Suit.Heart, 7)]),
             new("skill", "英姿", SkillId: "classic:yingzi")]);
        var session = ShanheCampaignSession.Create(definition, new(71, "classic:guan-yu")
            { PlayerRank = 4, EquippedTacticIds = [ordinary, elite, StandardShanheCampaignCatalog.CurrentDailyTravelRewardId] },
            StandardShanheCampaignCatalog.CreateBattleRegistry);
        Program.Assert(session.Submit(new MoveShanheNodeCommand("prepare", session.Revision)).Accepted &&
                session.Submit(new ChooseShanheNodeOptionCommand("card", null, session.Revision)).Accepted &&
                session.Submit(new ChooseShanheNodeOptionCommand("skill", null, session.Revision)).Accepted &&
                session.Submit(new ChooseShanheNodeOptionCommand("continue", null, session.Revision)).Accepted,
            "A real preparation shop grants the displayed hand card and extra skill through ordinary campaign commands.");
        using var model = new MainViewModel(false, 71, true, new MemorySaveStore(),
            shanheSaveStore: new MemoryShanheCampaignSaveStore(), shanheCodexStore: new MemoryShanheCodexStore());
        typeof(MainViewModel).GetField("_shanheSession", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(model, session);
        typeof(MainViewModel).GetMethod("RefreshShanhePresentation", BindingFlags.NonPublic | BindingFlags.Instance)!.Invoke(model, null);
        var panel = new ShanheJourneyLoadout { DataContext = model };
        try
        {
            Program.Render(panel, 1000, 192, System.IO.Path.Combine(output, "shanhe-journey-loadout.png"));
            var rows = (ItemsControl)panel.FindName("ShanheStartingCompanionRows");
            Program.Assert(rows.Items.Count == 2 && rows.ActualHeight > 0 &&
                model.ShanheStartingCompanionCards[0].Name == "曼珠" && model.ShanheStartingCompanionCards[1].Name.Contains("紫鸢"),
                $"The HUD creates separate visible rows for ordinary and elite opening companions (rows={rows.Items.Count}, height={rows.ActualHeight}, names={model.ShanheStartingCompanionName}).");
            Program.Assert(model.ShanheStartingCompanionCards[0].Description.Contains("龙吟") &&
                model.ShanheStartingCompanionCards[1].Description.Contains("合击") &&
                Program.Find<TextBlock>(rows).Any(text => text.Text == "曼珠") &&
                Program.Find<TextBlock>(rows).Any(text => text.Text.Contains("紫鸢")),
                "Each row shows its own public name and native rules instead of only the first companion.");
            var hand = (ItemsControl)panel.FindName("ShanheJourneyHandRows");
            var tactics = (ItemsControl)panel.FindName("ShanheJourneyTacticRows");
            var skills = (ItemsControl)panel.FindName("ShanheJourneySkillRows");
            Program.Assert(hand.Items.Count == 1 && hand.Items[0] is CardViewModel { Kind: CardKind.Slash, SuitGlyph: "♥", Rank: "7" } &&
                    tactics.Items.Count == 6 && tactics.Items.Cast<ShanheJourneyTacticSlot>().Count(slot => !slot.IsEmpty) == session.Snapshot.TacticIds.Count &&
                    tactics.Items.Cast<ShanheJourneyTacticSlot>().Where(slot => !slot.IsEmpty).Select(slot => slot.Tactic!.Name)
                        .SequenceEqual(model.ShanheJourneyTactics.Select(tactic => tactic.Name)) &&
                    skills.Items.Count == model.ShanheJourneySkillSlots.Count &&
                    model.ShanheJourneySkillSlots.Any(slot => slot.IsPrinted && slot.Name == "武圣") &&
                    model.ShanheJourneySkillSlots.Any(slot => !slot.IsPrinted && slot.Name == "英姿") &&
                    model.ShanheJourneySkillSlots.Any(slot => slot.IsEmpty),
                "The bottom dock renders actual suited hand cards and acquired tactics in their original order, with the source's extra empty column, native skills and empty extra slots.");
            AssertCompanionPortraits(rows);
            AssertSkillArtwork(skills);
            AssertHeroFrame(model, 3);
            foreach (var width in new[] { 1000, 1400 })
            {
                Program.Render(panel, width, 192, System.IO.Path.Combine(output, $"shanhe-journey-loadout-{width}.png"));
                AssertDockBounds(panel, width);
                AssertFirstColumnRight(tactics, (ScrollViewer)panel.FindName("ShanheJourneyTacticScroll"));
                AssertFirstColumnRight(skills, (ScrollViewer)panel.FindName("ShanheJourneySkillScroll"));
            }
            VerifyTraitsPopup(model, session, output);
            VerifyPurchasedLoadoutOverflowAndRecovery(model, panel, definition, output);
        }
        finally { panel.DataContext = null; }
        VerifyPublishedOpeningCompanionArtwork(definition, output);
        VerifyNativeLongHudRules(output);
    }

    private static void VerifyPurchasedLoadoutOverflowAndRecovery(MainViewModel model, ShanheJourneyLoadout panel,
        ShanheCampaignDefinition original, string output)
    {
        string[] learned = ["classic:rende", "classic:jizhi", "classic:qicai", "classic:qingguo", "classic:qixi", "classic:guanxing", "classic:kongcheng"];
        var supplied = Enumerable.Range(1, 6).Select(index => new ShanheStartingCard(index % 2 == 0 ? CardKind.Dodge : CardKind.Peach,
            index % 2 == 0 ? Suit.Diamond : Suit.Heart, index)).ToArray();
        var additions = new[]
        {
            new ShanheRewardDefinition("supplies", "行军补给", StartingCards: supplied),
            new ShanheRewardDefinition("slots", "技能容量", AddSkillSlots: 6)
        }.Concat(learned.Select((id, index) => new ShanheRewardDefinition("learn-" + index, "传习·" + id, SkillId: id)))
            .Concat(Enumerable.Range(1, 13).Select(index => new ShanheRewardDefinition("wallet-" + index, index == 1 ? "战法·军筹1" : "长名称战法·行军筹备" + index)
                { CampaignEconomy = new(BattleGoldPercent: index), Description = "本次征程的真实经济战法。" })).ToArray();
        var purchases = additions.Select(reward => new ShanheNodeChoiceDefinition(reward.Id, reward.Name, RewardId: reward.Id, LeavesNode: false)).ToArray();
        var definition = original with
        {
            Rewards = original.Rewards.Concat(additions).ToArray(),
            Nodes = original.Nodes.Select(node => node.Id != "prepare" ? node : node with
            { NextNodeIds = ["recruit"], Choices = node.Choices!.Concat(purchases).ToArray() }).Concat(
                [new ShanheNodeDefinition("recruit", "招募同行", ShanheNodeKind.Event, ["recruit-2"],
                    Choices: [new("join", "赵云同行") { Companion = new("classic:zhao-yun") }]),
                 new ShanheNodeDefinition("recruit-2", "再遇故人", ShanheNodeKind.Event, ["battle"],
                    Choices: [new("join-2", "张飞同行") { Companion = new("classic:zhang-fei") }])]).ToArray()
        };
        var options = new ShanheCampaignOptions(71, "classic:guan-yu") { PlayerRank = 5, EquippedTacticIds = original.Rewards.Take(3).Select(reward => reward.Id).ToArray() };
        var session = ShanheCampaignSession.Create(definition, options, StandardShanheCampaignCatalog.CreateBattleRegistry);
        Accept(new MoveShanheNodeCommand("prepare", session.Revision));
        Present(session);
        var notifications = new HashSet<string>(StringComparer.Ordinal);
        System.ComponentModel.PropertyChangedEventHandler changed = (_, args) =>
        { if (args.PropertyName is { } property) notifications.Add(property); };
        model.PropertyChanged += changed;
        try
        {
            foreach (var id in new[] { "card", "skill", "slots", "supplies" }.Concat(learned.Select((_, index) => "learn-" + index))
                .Concat(Enumerable.Range(1, 13).Select(index => "wallet-" + index)))
            {
                model.ChooseShanheOptionCommand.Execute(model.ShanheChoices.Single(choice => choice.Id == id));
                if (id == "wallet-7") Program.Assert(model.ShanheJourneyTactics.Count == 10 && model.ShanheJourneyTacticSlots.Count == 15,
                    "Ten acquired tactics retain the original extra empty column.");
                if (id == "wallet-10") Program.Assert(model.ShanheJourneyTactics.Count == 13 && model.ShanheJourneyTacticSlots.Count == 15,
                    "Above twelve tactics, the source layout only completes its partial column.");
            }
            Program.Assert(session.Snapshot.StartingCards.Count == 7 && session.Snapshot.ExtraSkillIds.Count == 8 && session.Snapshot.TacticIds.Count == 16 &&
                    notifications.Contains(nameof(MainViewModel.ShanheJourneyHandCards)) && notifications.Contains(nameof(MainViewModel.ShanheJourneyTactics)) &&
                    notifications.Contains(nameof(MainViewModel.ShanheJourneyTacticSlots)) &&
                    notifications.Contains(nameof(MainViewModel.ShanheJourneySkillSlots)),
                "Real purchases update the carried cards, acquired tactics and learned-skill capacity through the existing view-model refresh path.");
            model.ChooseShanheOptionCommand.Execute(model.ShanheChoices.Single(choice => choice.Id == "continue"));
            Accept(new MoveShanheNodeCommand("recruit", session.Revision));
            Accept(new ChooseShanheNodeOptionCommand("join", null, session.Revision));
            Accept(new MoveShanheNodeCommand("recruit-2", session.Revision));
            Accept(new ChooseShanheNodeOptionCommand("join-2", null, session.Revision));
            Accept(new MoveShanheNodeCommand("battle", session.Revision));
            Accept(new BeginShanheBattleAttemptCommand(session.Revision));
            Present(session);
            panel.Dispatcher.Invoke(() => { }, DispatcherPriority.ContextIdle);
            Program.Render(panel, 1000, 192, System.IO.Path.Combine(output, "shanhe-journey-loadout-overflow.png"));
            AssertHeroFrame(model, 4);
            var hand = (ScrollViewer)panel.FindName("ShanheJourneyHandScroll");
            var tactics = (ScrollViewer)panel.FindName("ShanheJourneyTacticScroll");
            var skills = (ScrollViewer)panel.FindName("ShanheJourneySkillScroll");
            var companions = (ScrollViewer)panel.FindName("ShanheJourneyCompanionScroll");
            Program.Assert(hand.ScrollableWidth > 0 && hand.ExtentHeight <= hand.ViewportHeight + .5 &&
                    tactics.ScrollableWidth > 0 && tactics.ScrollableHeight == 0 && tactics.ExtentHeight <= tactics.ViewportHeight + .5 &&
                    skills.ScrollableWidth > 0 && skills.ScrollableHeight == 0 && skills.ExtentHeight <= skills.ViewportHeight + .5 &&
                    tactics.HorizontalScrollBarVisibility == ScrollBarVisibility.Auto && tactics.VerticalScrollBarVisibility == ScrollBarVisibility.Disabled &&
                    skills.HorizontalScrollBarVisibility == ScrollBarVisibility.Auto && skills.VerticalScrollBarVisibility == ScrollBarVisibility.Disabled &&
                    companions.ScrollableWidth > 0 && companions.ExtentHeight <= companions.ViewportHeight + .5 &&
                    ((ItemsControl)panel.FindName("ShanheRecruitedCompanionRows")).Items.Count == 2,
                $"Long real loadouts scroll within their own panels, keep full card height above the scrollbar, and include recruited alongside opening companions. " +
                $"hand={hand.ExtentWidth}x{hand.ExtentHeight}/{hand.ViewportWidth}x{hand.ViewportHeight}; " +
                $"tactics={tactics.ExtentWidth}x{tactics.ExtentHeight}/{tactics.ViewportWidth}x{tactics.ViewportHeight}; " +
                $"skills={skills.ExtentWidth}x{skills.ExtentHeight}/{skills.ViewportWidth}x{skills.ViewportHeight}; " +
                $"companions={companions.ExtentWidth}x{companions.ExtentHeight}/{companions.ViewportWidth}x{companions.ViewportHeight}, recruited={model.ShanheRecruitedCompanionCards.Count}.");
            var tacticRows = (ItemsControl)panel.FindName("ShanheJourneyTacticRows");
            var skillRows = (ItemsControl)panel.FindName("ShanheJourneySkillRows");
            Program.Assert(tacticRows.Items.Count == 18 && tacticRows.Items.Cast<ShanheJourneyTacticSlot>().Count(slot => slot.IsEmpty) == 2 &&
                    tacticRows.Items.Cast<ShanheJourneyTacticSlot>().Where(slot => !slot.IsEmpty).Select(slot => slot.Tactic!.Name)
                        .SequenceEqual(session.Snapshot.TacticIds.Select(id => definition.Rewards.Single(reward => reward.Id == id).Name)),
                "Sixteen genuinely acquired tactics remain in order with exactly the source renderer's two visual empty circles and no artificial acquisition capacity.");
            AssertThreeRowColumns(tacticRows);
            AssertThreeRowColumns(skillRows);
            AssertDockBounds(panel, 1000);
            AssertCompanionPortraits((ItemsControl)panel.FindName("ShanheRecruitedCompanionRows"));
            var recruitedRegistry = StandardShanheCampaignCatalog.CreateBattleRegistry(session.PendingBattleSetup!);
            var recruitedCards = ((ItemsControl)panel.FindName("ShanheRecruitedCompanionRows")).Items.Cast<ShanheStoryCompanionCard>().ToArray();
            Program.Assert(session.Snapshot.RecruitedCompanions.Select(member => member.GeneralId)
                    .SequenceEqual(new[] { "classic:zhao-yun", "classic:zhang-fei" }),
                "The actual story commands recruit Zhao Yun and Zhang Fei in their original order.");
            for (var index = 0; index < recruitedCards.Length; index++)
                AssertRegisteredCompanionRules(recruitedCards[index].Description, recruitedRegistry, session.Snapshot.RecruitedCompanions[index]);
            Program.Assert(recruitedCards[0].Description.Contains(recruitedRegistry.GetSkill("classic:longdan").Description, StringComparison.Ordinal) &&
                recruitedCards[1].Description.Contains(recruitedRegistry.GetSkill("classic:paoxiao").Description, StringComparison.Ordinal),
                "Recruited Zhao Yun and Zhang Fei disclose the actual complete registered Longdan and Paoxiao rules, beyond their recruitment introduction.");
            VerifyRecruitedCompanionTooltips(model, session, recruitedRegistry, output);
            AssertSkillArtwork(skillRows);
            VerifyHorizontalWheelInput(model, panel, session);
            hand.ScrollToRightEnd(); tactics.ScrollToRightEnd(); skills.ScrollToRightEnd(); companions.ScrollToRightEnd();
            panel.Dispatcher.Invoke(() => { }, DispatcherPriority.ContextIdle);
            panel.UpdateLayout();
            Program.Assert(hand.HorizontalOffset > 0 && tactics.HorizontalOffset > 0 && skills.HorizontalOffset > 0 && companions.HorizontalOffset > 0 &&
                    Program.Find<TextBlock>(panel).Any(text => text.Name == "TacticFallback" && text.Visibility == Visibility.Visible &&
                        text.DataContext is ShanheJourneyTacticSlot { Tactic.Id: 0, IsEmpty: false } slot && text.Text == slot.Tactic!.DisplayName) &&
                    model.ShanheJourneySkillSlots.Where(slot => !slot.IsEmpty).All(slot => !string.IsNullOrWhiteSpace(slot.Description)),
                "Every overflowing list can reach its ending, missing tactic art keeps a visible text fallback, and printed and learned skills retain readable rules.");
            var shortFallback = Program.Find<TextBlock>(tacticRows).Single(text => text.Name == "TacticFallback" &&
                text.DataContext is ShanheJourneyTacticSlot { Tactic.Name: "战法·军筹1" });
            var fallbackTactic = ((ShanheJourneyTacticSlot)shortFallback.DataContext).Tactic!;
            var tacticViewport = Program.Find<ScrollContentPresenter>(tactics).Single();
            Program.Assert(fallbackTactic.Id == 0 && fallbackTactic.Icon is null && fallbackTactic.Name == definition.Rewards.Single(reward => reward.Id == "wallet-1").Name &&
                    fallbackTactic.DisplayName == "军筹1" && fallbackTactic.Tooltip.StartsWith(fallbackTactic.Name + "\n", StringComparison.Ordinal) &&
                    shortFallback.Visibility == Visibility.Visible && shortFallback.ActualWidth > 0 && shortFallback.ActualHeight > 0 &&
                    shortFallback.Text == fallbackTactic.DisplayName &&
                    new Rect(tacticViewport.RenderSize).Contains(shortFallback.TransformToVisual(tacticViewport).TransformBounds(new Rect(shortFallback.RenderSize))),
                "A genuinely acquired tactic without source artwork visibly falls back to its compact caption inside the map viewport while preserving its full reward identity and rule tooltip.");
            tactics.ScrollToLeftEnd(); skills.ScrollToLeftEnd();
            panel.Dispatcher.Invoke(() => { }, DispatcherPriority.ContextIdle); panel.UpdateLayout();
            AssertItemInViewport(tacticRows, tacticRows.Items.Count - 1, tactics);
            AssertItemInViewport(skillRows, skillRows.Items.Count - 1, skills);
            Program.Render(panel, 1400, 192, System.IO.Path.Combine(output, "shanhe-journey-loadout-overflow-1400.png"));
            AssertDockBounds(panel, 1400);
            var before = Loadout();
            var firstAttempt = session.PendingBattleSetup!;
            var restored = ShanheCampaignSession.Restore(session.CreateCheckpoint(), definition, StandardShanheCampaignCatalog.CreateBattleRegistry);
            Present(restored);
            Program.Assert(Loadout() == before, "Replaying the actual campaign journal restores every carried card, tactic, skill and companion in the dock.");
            var retry = restored.Submit(new BeginShanheBattleAttemptCommand(restored.Revision));
            Program.Assert(retry.Accepted, retry.Error ?? "A retained encounter must accept its next native attempt.");
            Present(restored);
            Program.Assert(Loadout() == before && restored.PendingBattleSetup!.AttemptNumber == firstAttempt.AttemptNumber + 1,
                "A new battle attempt changes its shuffle identity while preserving the actual acquired journey loadout.");
        }
        finally { model.PropertyChanged -= changed; }

        void Accept(ShanheCampaignCommand command)
        {
            var result = session.Submit(command);
            Program.Assert(result.Accepted, result.Error ?? "The real loadout fixture command was rejected.");
        }
        void Present(ShanheCampaignSession current)
        {
            typeof(MainViewModel).GetField("_shanheSession", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(model, current);
            typeof(MainViewModel).GetMethod("RefreshShanhePresentation", BindingFlags.NonPublic | BindingFlags.Instance)!.Invoke(model, null);
        }
        string Loadout() => System.Text.Json.JsonSerializer.Serialize(new
        {
            Cards = model.ShanheJourneyHandCards.Select(card => card.CardHint).ToArray(),
            Tactics = model.ShanheJourneyTactics.Select(tactic => tactic.Tooltip).ToArray(),
            Skills = model.ShanheJourneySkillSlots,
            Companions = model.ShanheStartingCompanionCards.Concat(model.ShanheRecruitedCompanionCards).Select(companion => companion.Name).ToArray()
        });
    }

    private static void VerifyPublishedOpeningCompanionArtwork(ShanheCampaignDefinition fixture, string output)
    {
        var expectedFiles = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            [StandardShanheCompanionPackage.ManzhuId] = "1000046",
            [StandardShanheChilianCompanionPackage.ChilianId] = "1000055",
            [StandardShanheBossPackage.FenghuangId] = "1000043",
            [StandardShanheExpandedCompanionPackage.AlingId] = "1000045",
            [StandardShanheExpandedCompanionPackage.YaohuaId] = "1000049",
            [StandardShanheExpandedCompanionPackage.DuanqingId] = "1000044",
            [StandardShanheExpandedCompanionPackage.QingliId] = "1000040",
            [StandardShanheExpandedCompanionPackage.ZiyuanId] = "1000058"
        };
        var published = StandardShanheCampaignCatalog.CreateCodexDefinition(1, StandardShanheCampaignCatalog.Returning2026EditionId);
        var ordinary = published.Rewards.Single(reward => reward.Id == StandardShanheCampaignCatalog.OpeningCompanionRewardId);
        var elite = published.Rewards.Single(reward => reward.Id == StandardShanheCampaignCatalog.EliteOpeningCompanionRewardId);
        var ordinaryPool = ordinary.OpeningCompanionPool!; var elitePool = elite.OpeningCompanionPool!;
        Program.Assert(ordinaryPool.Count == 4 && elitePool.Count == 4 &&
            ordinaryPool.Concat(elitePool).Select(member => member.GeneralId).ToHashSet(StringComparer.Ordinal).SetEquals(expectedFiles.Keys),
            "The artwork regression exercises all eight actual members of the two published four-member opening pools.");
        for (var pair = 0; pair < 4; pair++)
        {
            ShanheCombatantDefinition[] members = [ordinaryPool[pair], elitePool[pair]];
            // Keep each published combatant intact, including its fixed hand and
            // extra skills; singleton selection avoids searching random seeds.
            var definition = fixture with { Schools = published.Schools, Rewards = fixture.Rewards.Select(reward => reward.Id == ordinary.Id
                ? ordinary with { OpeningCompanionPool = [members[0]] }
                : reward.Id == elite.Id ? elite with { OpeningCompanionPool = [members[1]] } : reward).ToArray() };
            var session = ShanheCampaignSession.Create(definition, new(71, "classic:guan-yu")
                { PlayerRank = 4, EquippedTacticIds = [ordinary.Id, elite.Id] }, StandardShanheCampaignCatalog.CreateBattleRegistry);
            Program.Assert(session.Submit(new MoveShanheNodeCommand("prepare", session.Revision)).Accepted &&
                session.Submit(new ChooseShanheNodeOptionCommand("continue", null, session.Revision)).Accepted &&
                session.Snapshot.StartingCompanions.Select(member => member.GeneralId).SequenceEqual(members.Select(member => member.GeneralId)),
                "The real opening-pool selection retains one exact ordinary and one exact elite companion for this actual expedition.");
            using var model = new MainViewModel(false, 71, true, new MemorySaveStore(), useExpandedContent: true,
                shanheSaveStore: new MemoryShanheCampaignSaveStore(), shanheCodexStore: new MemoryShanheCodexStore())
                { IsMotionEnabled = false, IsSoundEnabled = false, IsMusicEnabled = false };
            typeof(MainViewModel).GetField("_shanheSession", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(model, session);
            typeof(MainViewModel).GetMethod("RefreshShanhePresentation", BindingFlags.NonPublic | BindingFlags.Instance)!.Invoke(model, null);
            using var host = new NativeResponseStatusChecks.NativeHost(model);
            model.OpenShanheCommand.Execute(null);
            host.Capture(output, "shanhe-opening-companions-" + pair, 1120);
            var loadout = Program.Find<ShanheJourneyLoadout>(host.Root).Single(control => control.IsVisible);
            var rows = (ItemsControl)loadout.FindName("ShanheStartingCompanionRows");
            AssertCompanionPortraits(rows);
            var cards = rows.Items.Cast<ShanheStoryCompanionCard>().ToArray();
            var probe = new ShanheBattleSetup(71, 1, "opening-companion-art", "classic:guan-yu", 4,
                0, 0, 0, 0, [], [], [], [new("classic:cao-cao")], members, session.ContentHash);
            var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(probe);
            Program.Assert(cards.Length == 2, "Each real pool pair creates its own two original HUD portraits.");
            for (var index = 0; index < members.Length; index++)
            {
                var member = members[index]; var card = cards[index];
                var path = "Assets/Shanhe/Npc/generalface_" + expectedFiles[member.GeneralId] + ".png";
                var expected = CardGame.Wpf.Resources.AssetImages.Load(path, decodeWidth: 120);
                Program.Assert(GeneralArt.GetSkin(member.GeneralId)?.LocalPath == "src/CardGame.Wpf/" + path &&
                    card.Portrait is ImageBrush { ImageSource: BitmapSource actual } && expected is not null &&
                    actual.PixelWidth == expected.PixelWidth && actual.PixelHeight == expected.PixelHeight && SamePixels(actual, expected),
                    "Every actual pool member decodes the pixels of its own exact original PVE skin, never another named general or a placeholder.");
                AssertRegisteredCompanionRules(card.Description, registry, member);
                var face = Program.Find<Border>(rows).Single(border => ReferenceEquals(border.DataContext, card) && border.ToolTip is ToolTip);
                AssertCompanionNativeTooltip(host, face, card, registry, member, session, output, "shanhe-opening-" + expectedFiles[member.GeneralId] + "-tooltip");
            }
            var battleNode = model.ShanheMapRows.SelectMany(row => row.Nodes).Single(node => node.Id == "battle");
            Program.Assert(model.SelectShanheNodeCommand.CanExecute(battleNode), "The real prepared companion party can enter its next battle.");
            model.SelectShanheNodeCommand.Execute(battleNode);
            host.Capture(output, "shanhe-opening-companions-table-" + pair, 1120);
            Program.Assert(model.IsShanheBattleSession && !model.IsShanheOpen && Program.Engine(model).PlayerCount == 4 &&
                session.PendingBattleSetup!.AllyRoster.Select(member => member.GeneralId).SequenceEqual(members.Select(member => member.GeneralId)),
                "The same two real opening companions enter the actual four-seat battle through the production map command.");
            foreach (var member in members)
            {
                var seat = model.Seats.Single(item => item.GeneralId == member.GeneralId);
                var portrait = Program.Find<System.Windows.Shapes.Rectangle>(host.Root)
                    .Single(rectangle => rectangle.Name == "SinglePortrait" && ReferenceEquals(rectangle.DataContext, seat));
                var expected = CardGame.Wpf.Resources.AssetImages.Load("Assets/Shanhe/Npc/generalface_" + expectedFiles[member.GeneralId] + ".png");
                Program.Assert(seat.IsTeammate && seat.HasPortrait && seat.Portrait.Image is BitmapSource actual && expected is not null &&
                    actual.PixelWidth == expected.PixelWidth && actual.PixelHeight == expected.PixelHeight && SamePixels(actual, expected) &&
                    portrait is { IsVisible: true, ActualWidth: > 0, ActualHeight: > 0, Fill: ImageBrush { ImageSource: not null } },
                    "The actual ally table seat displays the same exact original companion card as its HUD.");
                var crop = new RenderTargetBitmap((int)Math.Ceiling(portrait.ActualWidth), (int)Math.Ceiling(portrait.ActualHeight), 96, 96, PixelFormats.Pbgra32);
                crop.Render(portrait);
                var pixels = new byte[crop.PixelWidth * crop.PixelHeight * 4]; crop.CopyPixels(pixels, crop.PixelWidth * 4, 0);
                Program.Assert(Enumerable.Range(0, pixels.Length / 4).Any(index => pixels[index * 4 + 3] > 0),
                    "The real laid-out companion seat paints nontransparent original portrait pixels.");
            }
        }
    }

    private static void AssertRegisteredCompanionRules(string description, ContentRegistry registry, ShanheCombatantDefinition member)
    {
        var general = registry.Generals[member.GeneralId];
        var skills = general.SkillIds.Concat(member.ExtraSkillIds ?? []).Distinct(StringComparer.Ordinal).Select(registry.GetSkill).ToArray();
        Program.Assert(description.Contains(general.Name, StringComparison.Ordinal) && description.Contains("基础体力" + general.BaseHp, StringComparison.Ordinal) &&
            skills.Length > 0 && skills.All(skill => description.Contains(skill.Name, StringComparison.Ordinal) && description.Contains(skill.Description, StringComparison.Ordinal)),
            "The companion rules contain its registered body and every complete native or extra skill description, independently of the view-model description: " + member.GeneralId);
    }

    private static void VerifyRecruitedCompanionTooltips(MainViewModel model, ShanheCampaignSession session, ContentRegistry registry, string output)
    {
        using var host = new NativeResponseStatusChecks.NativeHost(model);
        model.OpenShanheCommand.Execute(null); host.Capture(output, "shanhe-recruited-companions-native", 1120);
        var loadout = Program.Find<ShanheJourneyLoadout>(host.Root).Single(control => control.IsVisible);
        var rows = (ItemsControl)loadout.FindName("ShanheRecruitedCompanionRows");
        var cards = rows.Items.Cast<ShanheStoryCompanionCard>().ToArray();
        Program.Assert(cards.Length == session.Snapshot.RecruitedCompanions.Count && cards.Length == 2,
            "Both actual recruited allies retain individual native tooltip owners.");
        for (var index = 0; index < cards.Length; index++)
        {
            var card = cards[index]; var member = session.Snapshot.RecruitedCompanions[index];
            var face = Program.Find<Border>(rows).Single(border => ReferenceEquals(border.DataContext, card) && border.ToolTip is ToolTip);
            AssertCompanionNativeTooltip(host, face, card, registry, member, session, output, "shanhe-recruited-" + member.GeneralId.Split(':')[1] + "-tooltip");
        }
    }

    private static void AssertCompanionNativeTooltip(NativeResponseStatusChecks.NativeHost host, Border owner, ShanheStoryCompanionCard card,
        ContentRegistry registry, ShanheCombatantDefinition member, ShanheCampaignSession session, string output, string renderName)
    {
        var campaignBefore = ShanheCampaignJson.Serialize(session.CreateCheckpoint());
        var ordinary = Program.Engine(host.Model);
        var ordinaryBefore = GameCheckpointJson.Serialize(ordinary.CreateCheckpoint());
        var tooltip = (ToolTip)owner.ToolTip;
        tooltip.PlacementTarget = owner; tooltip.Placement = PlacementMode.AbsolutePoint;
        tooltip.HorizontalOffset = -30000; tooltip.VerticalOffset = -30000; tooltip.Opened += Conceal;
        try
        {
            tooltip.IsOpen = true;
            host.Root.Dispatcher.Invoke(() => { }, DispatcherPriority.ContextIdle); host.Root.UpdateLayout();
            var text = Program.Find<TextBlock>(tooltip).Where(block => block.IsVisible && block.ActualWidth > 0 && block.ActualHeight > 0).ToArray();
            Program.Assert(tooltip.ContentTemplate is null && PresentationSource.FromVisual(tooltip) is HwndSource &&
                text.Any(block => block.Text == card.Name) && text.Any(block => block.Text == card.Description) &&
                text.All(block => !block.Text.StartsWith("System.Windows.Controls.", StringComparison.Ordinal)),
                "The actual opened companion tooltip visually renders its full public name and complete rules instead of a UIElement type name.");
            var rules = text.Single(block => block.Text == card.Description);
            AssertRegisteredCompanionRules(rules.Text, registry, member);
            Program.Assert(tooltip.MaxHeight <= 360 && tooltip.Content is ScrollViewer
                    { MaxHeight: <= 320, Focusable: false, VerticalScrollBarVisibility: ScrollBarVisibility.Auto,
                      HorizontalScrollBarVisibility: ScrollBarVisibility.Disabled, Content: StackPanel },
                "The original companion name and complete rules remain together inside the bounded read-only HUD tooltip scroller.");
            var scroll = (ScrollViewer)tooltip.Content;
            scroll.ScrollToBottom();
            host.Root.Dispatcher.Invoke(() => { }, DispatcherPriority.ContextIdle); tooltip.UpdateLayout();
            var lastCharacter = rules.ContentEnd.GetCharacterRect(System.Windows.Documents.LogicalDirection.Backward);
            var lastBounds = rules.TransformToAncestor(scroll).TransformBounds(lastCharacter);
            Program.Assert(!lastCharacter.IsEmpty && lastBounds.Left >= -.5 && lastBounds.Top >= -.5 &&
                lastBounds.Bottom <= scroll.ViewportHeight + .5 && lastBounds.Right <= scroll.ViewportWidth + .5,
                $"The last actual companion rule character remains inside the native tooltip viewport: {member.GeneralId}, last={lastBounds}, viewport={scroll.ViewportWidth}x{scroll.ViewportHeight}.");
            Console.WriteLine($"Companion native tooltip: {member.GeneralId}, characters={rules.Text.Length}, size={tooltip.RenderSize}, last={lastBounds}.");
            Program.Render(tooltip, (int)Math.Ceiling(tooltip.ActualWidth), (int)Math.Ceiling(tooltip.ActualHeight), System.IO.Path.Combine(output, renderName + ".png"));
        }
        finally { tooltip.IsOpen = false; tooltip.Opened -= Conceal; }
        Program.Assert(ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == campaignBefore &&
            ReferenceEquals(Program.Engine(host.Model), ordinary) && GameCheckpointJson.Serialize(ordinary.CreateCheckpoint()) == ordinaryBefore,
            "Opening the real companion rule tooltip preserves both campaign and ordinary-game journals.");

        static void Conceal(object sender, RoutedEventArgs args)
        { if (PresentationSource.FromVisual((ToolTip)sender) is HwndSource source) ShowWindow(source.Handle, 0); }
    }

    private static void AssertDockBounds(ShanheJourneyLoadout panel, double width)
    {
        var blocks = new[] { "ShanheJourneyHandScroll", "ShanheJourneyTacticScroll", "ShanheJourneySkillScroll", "ShanheJourneyCompanionScroll", "ShanheHeroCard" }
            .Select(name => (FrameworkElement)panel.FindName(name)).ToArray();
        var bounds = blocks.Select(block => block.TransformToAncestor(panel).TransformBounds(new Rect(block.RenderSize))).ToArray();
        Program.Assert(panel.RenderSize == new Size(width, 192) &&
                bounds.All(rectangle => rectangle.Width > 0 && rectangle.Height > 0 && rectangle.Left >= -.5 && rectangle.Top >= -.5 &&
                    rectangle.Right <= width + .5 && rectangle.Bottom <= 192.5) &&
                bounds.SelectMany((rectangle, index) => bounds.Skip(index + 1).Select(other => rectangle.IntersectsWith(other))).All(overlap => !overlap) &&
                Math.Abs(bounds[^1].Right - width) <= .5 && bounds[3].Right <= bounds[^1].Left,
            $"The {width}-wide journey dock must keep its real scroll viewports separate and fully visible, with companions to the hero's left and the hero against the right edge; bounds={string.Join("; ", bounds)}.");
    }

    private static void AssertCompanionPortraits(ItemsControl rows)
    {
        var faces = Program.Find<Border>(rows).Where(border => border.ToolTip is ToolTip).ToArray();
        var tag = CardGame.Wpf.Resources.AssetImages.Load("Assets/Shanhe/Hud/rogue_mate_logo.png");
        var checks = faces.Select(face =>
        {
            var card = face.DataContext as ShanheStoryCompanionCard;
            var dpi = VisualTreeHelper.GetDpi(face);
            var names = Program.Find<TextBlock>(face).ToArray();
            var tags = Program.Find<Image>(face).Where(image => image.Name == "CompanionSourceTag").ToArray();
            var source = tags.FirstOrDefault()?.Source as BitmapSource;
            return new
            {
                Name = card?.Name, face.Width, face.Height, face.ActualWidth, face.ActualHeight,
                dpi.DpiScaleX, dpi.DpiScaleY, face.Visibility, face.IsVisible,
                SizeMatches = face.Width == 55 && face.Height == 55 && face.ActualWidth > 0 && face.ActualHeight > 0 &&
                    Math.Abs(face.ActualWidth - 55) <= 1 / dpi.DpiScaleX + .001 &&
                    Math.Abs(face.ActualHeight - 55) <= 1 / dpi.DpiScaleY + .001,
                Visible = face.Visibility == Visibility.Visible && (PresentationSource.FromVisual(face) is null || face.IsVisible),
                AccessibleName = AutomationProperties.GetName(face),
                AccessibleMatches = card is not null && AutomationProperties.GetName(face) == card.Name,
                NameMatches = card is not null && names.Any(text => text.Text == card.Name && text.TextTrimming == TextTrimming.CharacterEllipsis),
                FallbackMatches = card is not null && names.Any(text => text.Name == "CompanionFallback" && text.Text == card.Glyph &&
                    text.Visibility == (card.HasPortrait ? Visibility.Collapsed : Visibility.Visible)),
                TagMatches = tags.Length == 1 && source is not null && tag is not null &&
                    source.PixelWidth == tag.PixelWidth && source.PixelHeight == tag.PixelHeight && SamePixels(source, tag),
                TooltipMatches = face.ToolTip is ToolTip { Content: ScrollViewer { Content: StackPanel } },
                Captions = names.Select(text => new { text.Name, text.Text, text.Visibility }).ToArray()
            };
        }).ToArray();
        foreach (var check in checks.Where(check => check.SizeMatches && (check.ActualWidth != 55 || check.ActualHeight != 55)))
            Console.WriteLine($"Companion portrait DPI rounding: {check.Name}, declared=55x55, actual={check.ActualWidth}x{check.ActualHeight}, dpi={check.DpiScaleX}x{check.DpiScaleY}.");
        Program.Assert(faces.Length == rows.Items.Count && checks.All(check => check.SizeMatches && check.Visible &&
                check.AccessibleMatches && check.NameMatches && check.FallbackMatches && check.TagMatches && check.TooltipMatches),
            "Every actual companion keeps its declared square 55-pixel portrait within one device pixel of DPI rounding, original red companion tag and full accessible name and rule tooltip even when its visible name is ellipsized: " +
            System.Text.Json.JsonSerializer.Serialize(new { rows.Name, Items = rows.Items.Count, Faces = faces.Length, Checks = checks }));
    }

    private static void AssertSkillArtwork(ItemsControl skills)
    {
        var faces = Program.Find<Border>(skills).Where(border => border.ToolTip is ToolTip &&
            border.DataContext is ShanheJourneySkillSlot).ToArray();
        Program.Assert(faces.Length == skills.Items.Count && faces.All(face =>
                face.Focusable == !((ShanheJourneySkillSlot)face.DataContext).IsEmpty &&
                AutomationProperties.GetName(face) == ((ShanheJourneySkillSlot)face.DataContext).Name &&
                AutomationProperties.GetHelpText(face) == ((ShanheJourneySkillSlot)face.DataContext).Description),
            "Only printed and actually learned skills are keyboard reading targets; real unused skill capacity keeps its empty artwork and mouse rules without adding an empty Tab stop.");
        var images = Program.Find<Image>(skills).Where(image => image.Name == "SkillSourceFrame").ToArray();
        foreach (var empty in images.Select(image => ((ShanheJourneySkillSlot)image.DataContext).IsEmpty).Distinct())
        {
            var image = images.First(frame => frame.DataContext is ShanheJourneySkillSlot slot && slot.IsEmpty == empty);
            var expected = CardGame.Wpf.Resources.AssetImages.Load("Assets/Shanhe/Hud/roguesmall_skill_" + (empty ? "null" : "up") + ".png");
            Program.Assert(image.Source is BitmapSource actual && expected is not null && actual.PixelWidth == expected.PixelWidth &&
                    actual.PixelHeight == expected.PixelHeight && SamePixels(actual, expected) && image.ActualWidth > 0 && image.ActualHeight > 0,
                "Printed and learned skills use the original green frame while real remaining skill capacity uses the original empty-slot artwork.");
        }
    }

    private static bool SamePixels(BitmapSource actual, BitmapSource expected)
    {
        var stride = actual.PixelWidth * 4;
        var first = new byte[stride * actual.PixelHeight]; var second = new byte[first.Length];
        new FormatConvertedBitmap(actual, PixelFormats.Bgra32, null, 0).CopyPixels(first, stride, 0);
        new FormatConvertedBitmap(expected, PixelFormats.Bgra32, null, 0).CopyPixels(second, stride, 0);
        return first.AsSpan().SequenceEqual(second);
    }

    private static void AssertHeroFrame(MainViewModel model, int sourceGrade)
    {
        var expected = CardGame.Wpf.Resources.AssetImages.Load($"Assets/Shanhe/Hud/other_general_border{sourceGrade}.png");
        Program.Assert(model.ShanheJourneyHeroFrame is BitmapSource actual && expected is not null &&
                actual.PixelWidth == expected.PixelWidth && actual.PixelHeight == expected.PixelHeight && SamePixels(actual, expected),
            "The hero ornament follows the actual expedition rank mapped to the source display grade.");
    }

    private static void AssertThreeRowColumns(ItemsControl rows)
    {
        var cells = Enumerable.Range(0, rows.Items.Count).Select(index =>
            (FrameworkElement)rows.ItemContainerGenerator.ContainerFromIndex(index)).ToArray();
        var bounds = cells.Select(cell => cell.TransformToAncestor(rows).TransformBounds(new Rect(cell.RenderSize))).ToArray();
        Program.Assert(bounds.All(rectangle => rectangle.Width > 0 && rectangle.Height > 0) &&
                bounds.Select((rectangle, index) => index % 3 == 0 ||
                    Math.Abs(rectangle.Left - bounds[index - 1].Left) < .5 && rectangle.Top >= bounds[index - 1].Bottom - .5).All(valid => valid) &&
                bounds.Select((rectangle, index) => index < 3 ||
                    rectangle.Right <= bounds[index - 3].Left + .5 && Math.Abs(rectangle.Top - bounds[index - 3].Top) < .5).All(valid => valid),
            "Actual tactic and skill cells fill three rows per column and extend new columns to the left of the original first column.");
    }

    private static void AssertItemInViewport(ItemsControl rows, int index, ScrollViewer scroll)
    {
        var container = (FrameworkElement)rows.ItemContainerGenerator.ContainerFromIndex(index);
        var presenter = Program.Find<ScrollContentPresenter>(scroll).Single();
        var bounds = container.TransformToAncestor(presenter).TransformBounds(new Rect(container.RenderSize));
        Program.Assert(bounds.Left >= -.5 && bounds.Top >= -.5 && bounds.Right <= presenter.ActualWidth + .5 && bounds.Bottom <= presenter.ActualHeight + .5,
            "Scrolling left must expose the final real source-layout cell in full, including the third row above the horizontal scrollbar.");
    }

    private static void AssertFirstColumnRight(ItemsControl rows, ScrollViewer scroll)
    {
        var container = (FrameworkElement)rows.ItemContainerGenerator.ContainerFromIndex(0);
        var presenter = Program.Find<ScrollContentPresenter>(scroll).Single();
        var bounds = container.TransformToAncestor(presenter).TransformBounds(new Rect(container.RenderSize));
        Program.Assert(Math.Abs(bounds.Right - presenter.ActualWidth) <= .5,
            "A short loadout keeps its first source column beside the hero rather than against the left of a wider viewport.");
    }

    private static void VerifyTraitsPopup(MainViewModel model, ShanheCampaignSession session, string output)
    {
        using var host = new NativeResponseStatusChecks.NativeHost(model);
        model.OpenShanheCommand.Execute(null);
        foreach (var width in new[] { 1120, 1440 })
        {
            host.Capture(output, "shanhe-journey-native-dock", width);
            AssertNativeHeaderAndHandRemainSeparate(host);
        }
        var campaign = (ShanheCampaignPanel)host.Window.FindName("ShanhePanel");
        var loadout = Program.Find<ShanheJourneyLoadout>(campaign).Single(control => control.IsVisible);
        var button = (ToggleButton)loadout.FindName("ShanheTraitsButton");
        var popup = (Popup)loadout.FindName("ShanheTraitsPopup");
        popup.Placement = PlacementMode.AbsolutePoint; popup.HorizontalOffset = -30000; popup.VerticalOffset = -30000;
        popup.Opened += Conceal;
        var before = ShanheCampaignJson.Serialize(session.CreateCheckpoint());
        var revision = session.Revision;
        var ordinaryRevision = Program.Engine(model).Revision;
        var ordinary = Program.Engine(model);
        var ordinaryBefore = GameCheckpointJson.Serialize(ordinary.CreateCheckpoint());
        var ruleFaces = Program.Find<Border>(loadout).Where(border => border.ToolTip is ToolTip &&
            border.DataContext is ShanheStoryCompanionCard or CardViewModel or ShanheJourneySkillSlot { IsEmpty: false } or
                ShanheJourneyTacticSlot { IsEmpty: false }).ToArray();
        var ruleTips = ruleFaces.Select(face => (ToolTip)face.ToolTip).ToArray();
        foreach (var tip in ruleTips)
        {
            tip.Placement = PlacementMode.AbsolutePoint; tip.HorizontalOffset = -30000; tip.VerticalOffset = -30000;
            tip.Opened += ConcealTooltip;
        }
        try
        {
            var skillRows = (ItemsControl)loadout.FindName("ShanheJourneySkillRows");
            var skillFaces = Program.Find<Border>(skillRows).Where(face => face.ToolTip is ToolTip &&
                face.DataContext is ShanheJourneySkillSlot).ToArray();
            var realSkills = skillFaces.Where(face => face.DataContext is ShanheJourneySkillSlot { IsEmpty: false }).ToArray();
            var emptySkills = skillFaces.Where(face => face.DataContext is ShanheJourneySkillSlot { IsEmpty: true }).ToArray();
            Program.Assert(realSkills.Any(face => face.DataContext is ShanheJourneySkillSlot { IsPrinted: true }) &&
                    realSkills.Any(face => face.DataContext is ShanheJourneySkillSlot { IsPrinted: false }) && emptySkills.Length > 0,
                "The existing native preparation fixture includes a printed skill, a genuinely learned skill and real remaining empty capacity.");
            host.FocusKeyboard();
            foreach (var face in realSkills)
            {
                Program.Assert(face.Focusable && face.Focus(), "An actual printed or learned journey skill remains a keyboard reading target.");
                Flush();
                var slot = (ShanheJourneySkillSlot)face.DataContext;
                Program.Assert(ReferenceEquals(Keyboard.FocusedElement, face) && !string.IsNullOrWhiteSpace(slot.Description) &&
                        AutomationProperties.GetHelpText(face) == slot.Description && ((ToolTip)face.ToolTip).IsOpen,
                    "Native keyboard focus immediately opens each occupied skill's own complete accessible rules.");
            }
            var lastReal = realSkills[^1];
            foreach (var empty in emptySkills)
                Program.Assert(!empty.Focusable && !empty.Focus() && ReferenceEquals(Keyboard.FocusedElement, lastReal),
                    "A real empty journey skill cell rejects keyboard focus instead of taking focus away from the last readable skill.");
            Program.Assert(!TableShortcutChecks.Input(host.Window, Key.Tab) &&
                    lastReal.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next)),
                "The map leaves Tab to normal native focus traversal after the final occupied journey skill.");
            Flush();
            Program.Assert(Keyboard.FocusedElement is DependencyObject focused &&
                    !emptySkills.Any(empty => ReferenceEquals(empty, focused) || empty.IsAncestorOf(focused)) &&
                    session.Revision == revision && ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == before &&
                    Program.Engine(model).Revision == ordinaryRevision,
                "Native Tab skips unused skill cells without activating a skill, advancing either game or changing the expedition journal.");
            var tooltipKinds = new HashSet<string>(StringComparer.Ordinal);
            foreach (var width in new[] { 1120, 1440 })
            {
                host.Capture(output, "shanhe-journey-keyboard-dock", width);
                foreach (var face in ruleFaces)
                {
                var (kind, expectedText) = face.DataContext switch
                {
                    ShanheStoryCompanionCard companion => ("companion", new[] { companion.Name, companion.Description }),
                    CardViewModel card => ("hand", new[] { card.CardHint }),
                    ShanheJourneySkillSlot skill => ("skill", new[] { skill.Name, skill.SourceText, skill.Description }),
                    ShanheJourneyTacticSlot tactic => ("tactic", new[] { tactic.Tooltip }),
                    _ => throw new InvalidOperationException("Unexpected journey tooltip owner.")
                };
                var tooltip = (ToolTip)face.ToolTip;
                try
                {
                    button.Focus();
                    Program.Assert(face.Focusable && ToolTipService.GetShowsToolTipOnKeyboardFocus(face) == true && face.Focus(),
                        "The real hand, tactic, skill or companion face remains a native keyboard reading target.");
                    Flush();
                    Program.Assert(ReferenceEquals(Keyboard.FocusedElement, face) && ReferenceEquals(tooltip.PlacementTarget, face) &&
                            tooltip.IsOpen && tooltip.MaxHeight <= 360 && tooltip.Content is ScrollViewer
                            { MaxHeight: <= 320, Focusable: false, VerticalScrollBarVisibility: ScrollBarVisibility.Auto,
                              HorizontalScrollBarVisibility: ScrollBarVisibility.Disabled } bounded && bounded.ActualHeight is > 0 and <= 320,
                        "Focusing the actual HUD face immediately opens its bounded read-only rule surface without a manual IsOpen assignment.");
                    if (face.DataContext is ShanheStoryCompanionCard companion)
                        Program.Assert(tooltip.IsOpen && tooltip.Content is ScrollViewer { Content: StackPanel contents } &&
                                Program.Find<TextBlock>(contents).Any(text => text.Text == companion.Name) &&
                                Program.Find<TextBlock>(contents).Any(text => text.Text == companion.Description),
                            "Opening each actual square companion tooltip must reveal its complete public name and that companion's own native rules.");
                    var visibleText = Program.Find<TextBlock>(tooltip).Where(text =>
                        text.IsVisible && text.ActualWidth > 0 && text.ActualHeight > 0).ToArray();
                    Program.Assert(tooltip.IsOpen && tooltip.ContentTemplate is null &&
                            PresentationSource.FromVisual(tooltip) is HwndSource &&
                            expectedText.All(expected => visibleText.Any(text => text.Text == expected)) &&
                            visibleText.All(text => !text.Text.StartsWith("System.Windows.Controls.", StringComparison.Ordinal)),
                        "Every opened native hand, tactic, skill and companion tooltip visually renders its complete bound text rather than its UIElement type name.");
                    foreach (var key in new[] { Key.Enter, Key.Space })
                    {
                        Program.Assert(TableShortcutChecks.Input(host.Window, Key.Escape),
                            "The main window routes the first HUD Escape to the active rule tooltip.");
                        Flush();
                        Program.Assert(!tooltip.IsOpen && model.IsShanheOpen && !popup.IsOpen &&
                                ReferenceEquals(Keyboard.FocusedElement, face),
                            "Escape closes only the current HUD rule tip and keeps its focused item and map open.");
                        PressReadingKey(face, key);
                        Program.Assert(tooltip.IsOpen && ReferenceEquals(Keyboard.FocusedElement, face),
                            "Enter and Space on the same HUD face reopen only its original read-only rules.");
                        AssertReadOnly();
                    }
                    if (tooltipKinds.Add(kind))
                        Program.Render(tooltip, (int)Math.Ceiling(tooltip.ActualWidth), (int)Math.Ceiling(tooltip.ActualHeight),
                            System.IO.Path.Combine(output, "shanhe-journey-" + kind + "-tooltip.png"));
                    Program.Assert(button.Focus(), "The separate traits toggle remains the next explicit HUD action after reading.");
                    Flush();
                    Program.Assert(!tooltip.IsOpen && model.IsShanheOpen && !popup.IsOpen,
                        "Losing focus closes the HUD rule tip without activating the focused traits toggle or closing the map.");
                }
                finally { tooltip.IsOpen = false; }
                }
            }
            Program.Assert(tooltipKinds.SetEquals(["hand", "tactic", "skill", "companion"]),
                "The real acquired loadout must exercise opened native tooltips for all four HUD content kinds.");
            AssertTipLifecycle();
            AssertReadOnly();
            Click(button); Flush();
            var content = (FrameworkElement)popup.Child;
            Program.Assert(button.IsChecked == true && popup.IsOpen && content.ActualWidth > 0 && content.ActualHeight > 0 &&
                    PresentationSource.FromVisual(content) is HwndSource &&
                    Program.Find<TextBlock>(content).Any(text => text.Text == model.CurrentShanheGeneralName) &&
                    Program.Find<TextBlock>(content).Any(text => text.Text == model.CurrentShanheGeneralSkillsText) &&
                    Program.Find<TextBlock>(content).Any(text => text.Text == model.ShanheBuildSummary),
                "The actual hero traits button opens a separate native popup containing the current hero name, printed skill names and complete journey build summary.");
            var rank = model.ShanheRankStatusText;
            Program.Assert(rank.Length > 0 && model.ShanheBuildSummary.Split(rank, StringSplitOptions.None).Length == 2 &&
                    Program.Find<TextBlock>(content).Where(text => text.IsVisible)
                        .Sum(text => text.Text.Split(rank, StringSplitOptions.None).Length - 1) == 1,
                "The complete current rank and rebirth state appear once in the build summary, instead of being repeated in a separate traits line.");
            var sourceFrame = Program.Find<ShanheHudSkin>(content).Single(skin => skin.Name == "ShanheTraitsSourceFrame");
            var expectedFrame = CardGame.Wpf.Resources.AssetImages.Load("Assets/Shanhe/Hud/toolTipBg.png");
            Program.Assert(content.Width == 330 && sourceFrame.Insets == new Thickness(11) && !sourceFrame.IsHitTestVisible &&
                    sourceFrame.Source is BitmapSource actualFrame && expectedFrame is not null &&
                    actualFrame.PixelWidth == expectedFrame.PixelWidth && actualFrame.PixelHeight == expectedFrame.PixelHeight &&
                    SamePixels(actualFrame, expectedFrame),
                "The unchanged-width traits popup reuses the exact original HUD tooltip artwork through its native nine-slice renderer.");
            Program.Render(content, (int)Math.Ceiling(content.ActualWidth), (int)Math.Ceiling(content.ActualHeight),
                System.IO.Path.Combine(output, "shanhe-journey-traits.png"));
            Click(Program.Find<Button>(content).Single()); Flush();
            Program.Assert(!popup.IsOpen && button.IsChecked == false, "The traits close button must close the actual popup and reset its toggle.");
            Click(button); Flush();
            var handled = TableShortcutChecks.Input(host.Window, Key.Escape); Flush();
            Program.Assert(handled && !popup.IsOpen && button.IsChecked == false && model.IsShanheOpen && session.Revision == revision &&
                    ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == before && Program.Engine(model).Revision == ordinaryRevision,
                "Escape closes the traits popup without writing campaign commands, changing resources or advancing the underlying ordinary game.");
        }
        finally
        {
            foreach (var tip in ruleTips) { tip.IsOpen = false; tip.Opened -= ConcealTooltip; }
            popup.IsOpen = false; popup.Opened -= Conceal;
        }

        void Flush() { host.Root.Dispatcher.Invoke(() => { }, DispatcherPriority.ContextIdle); host.Root.UpdateLayout(); }
        void Conceal(object? sender, EventArgs args)
        { if (PresentationSource.FromVisual(popup.Child) is HwndSource source) ShowWindow(source.Handle, 0); }
        void ConcealTooltip(object sender, RoutedEventArgs args)
        { if (PresentationSource.FromVisual((ToolTip)sender) is HwndSource source) ShowWindow(source.Handle, 0); }
        static void Click(ButtonBase target) => typeof(ButtonBase).GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(target, null);
        void AssertReadOnly() => Program.Assert(session.Revision == revision && ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == before &&
            ReferenceEquals(Program.Engine(model), ordinary) && GameCheckpointJson.Serialize(ordinary.CreateCheckpoint()) == ordinaryBefore,
            "Reading, focusing, closing or reopening the real HUD rules preserves the full campaign and ordinary-game checkpoints.");
        void PressReadingKey(Border face, Key key)
        {
            TableShortcutChecks.WithModifiers(ModifierKeys.None, () =>
            {
                Program.Assert(!TableShortcutChecks.Input(host.Window, key),
                    "The map window passes the unmodified reading key to its focused HUD item.");
                var input = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromVisual(face)!, Environment.TickCount, key)
                    { RoutedEvent = Keyboard.PreviewKeyDownEvent };
                face.RaiseEvent(input);
                Program.Assert(input.Handled, "The actual HUD item owns its explicit read-only key.");
                return true;
            });
            Flush();
        }
        void AssertTipLifecycle()
        {
            var face = ruleFaces.First();
            var tip = (ToolTip)face.ToolTip;
            button.Focus(); Program.Assert(face.Focus(), "A real HUD face can be focused before checking its visual lifecycle."); Flush();
            Program.Assert(tip.IsOpen, "The actual rule tip is open before hiding its owner.");
            var previous = loadout.ReadLocalValue(UIElement.VisibilityProperty);
            try
            {
                loadout.Visibility = Visibility.Collapsed; Flush();
                Program.Assert(!tip.IsOpen && model.IsShanheOpen, "Hiding the actual HUD closes its floating rules without ending the expedition.");
            }
            finally
            {
                if (previous == DependencyProperty.UnsetValue) loadout.ClearValue(UIElement.VisibilityProperty);
                else loadout.SetValue(UIElement.VisibilityProperty, previous);
                Flush();
            }
            button.Focus(); Program.Assert(face.Focus(), "The same restored HUD face can reopen its actual rule tip."); Flush();
            Program.Assert(tip.IsOpen, "The actual rule tip is open before unloading its visual tree.");
            var root = host.Window.Content;
            try
            {
                host.Window.Content = null; Flush();
                Program.Assert(!tip.IsOpen && model.IsShanheOpen, "Unloading the actual HUD leaves no orphaned rule popup or campaign action.");
            }
            finally { host.Window.Content = root; Flush(); }
            button.Focus(); Flush();
            AssertReadOnly();
        }
    }

    private static void VerifyNativeLongHudRules(string output)
    {
        const string player = "mobile:shen-sima-yi";
        const string skillId = "mobile-god:baiyin";
        var definition = StandardShanheCampaignCatalog.CreateDefinition(1, StandardShanheCampaignCatalog.Source2026EditionId);
        var registry = StandardShanheCampaignCatalog.PlayerRosterRegistry;
        var source = registry.GetSkill(skillId);
        Program.Assert(definition.PlayerGeneralIds.Contains(player) && registry.Generals[player].SkillIds.Contains(skillId) &&
                source.Name == "拜印" && source.Description.Length > 300 && source.Description.Contains('\n'),
            "The untouched default source campaign already permits this actual printed general and its complete published multi-paragraph Baiyin rule.");
        var session = ShanheCampaignSession.Create(definition, new(71, player)
            { SourceDifficultyId = definition.SourceDifficultyId }, StandardShanheCampaignCatalog.CreateBattleRegistry);
        using var model = new MainViewModel(false, 71, true, new MemorySaveStore(), useExpandedContent: true,
            shanheSaveStore: new MemoryShanheCampaignSaveStore(), shanheCodexStore: new MemoryShanheCodexStore())
            { IsMotionEnabled = false, IsSoundEnabled = false, IsMusicEnabled = false };
        typeof(MainViewModel).GetField("_shanheSession", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(model, session);
        typeof(MainViewModel).GetMethod("RefreshShanhePresentation", BindingFlags.NonPublic | BindingFlags.Instance)!.Invoke(model, null);
        using var host = new NativeResponseStatusChecks.NativeHost(model);
        model.OpenShanheCommand.Execute(null);
        var campaignBefore = ShanheCampaignJson.Serialize(session.CreateCheckpoint());
        var game = Program.Engine(model);
        var gameBefore = GameCheckpointJson.Serialize(game.CreateCheckpoint());
        foreach (var width in new[] { 1120, 1440 })
        {
            host.Capture(output, "shanhe-journey-source-baiyin-dock", width);
            var loadout = Program.Find<ShanheJourneyLoadout>(host.Root).Single(control => control.IsVisible);
            var face = Program.Find<Border>((ItemsControl)loadout.FindName("ShanheJourneySkillRows"))
                .Single(border => border.ToolTip is ToolTip && border.DataContext is ShanheJourneySkillSlot slot &&
                    slot.IsPrinted && !slot.IsEmpty && slot.Name == source.Name && slot.Description == source.Description);
            var tooltip = (ToolTip)face.ToolTip;
            tooltip.Placement = PlacementMode.AbsolutePoint; tooltip.HorizontalOffset = -30000; tooltip.VerticalOffset = -30000;
            tooltip.Opened += Conceal;
            try
            {
                host.FocusKeyboard();
                Program.Assert(((ToggleButton)loadout.FindName("ShanheTraitsButton")).Focus() && face.Focus(),
                    "Returning from another real HUD focus target opens the unmodified printed Baiyin rule without entering a battle.");
                Flush();
                var scroll = (ScrollViewer)tooltip.Content;
                Program.Assert(tooltip.IsOpen && ReferenceEquals(Keyboard.FocusedElement, face),
                    "The actual source reader reopens after Escape and a fresh focus transition at either viewport width.");
                var text = Program.Find<TextBlock>(scroll).Single(block => block.Text == source.Description);
                Program.Assert(ReferenceEquals(Keyboard.FocusedElement, face) && tooltip.IsOpen && tooltip.IsVisible &&
                        tooltip.MaxHeight <= 360 && scroll.MaxHeight <= 320 && !scroll.Focusable &&
                        scroll.VerticalScrollBarVisibility == ScrollBarVisibility.Auto &&
                        scroll.HorizontalScrollBarVisibility == ScrollBarVisibility.Disabled &&
                        PresentationSource.FromVisual(tooltip) is HwndSource && text.IsVisible && text.Text == source.Description &&
                        scroll.ScrollableHeight > 0 && Math.Abs(scroll.VerticalOffset) < .5,
                    "The real printed multi-paragraph rule overflows its bounded native HUD reader while retaining its exact original registered text.");
                Press(Key.PageDown);
                var pageDown = scroll.VerticalOffset;
                Program.Assert(pageDown > 0, "PageDown on the actual printed skill scrolls forward through its complete original rule.");
                Press(Key.PageUp);
                Program.Assert(scroll.VerticalOffset < pageDown, "PageUp returns through that same long source rule.");
                Press(Key.End);
                var lastCharacter = text.ContentEnd.GetCharacterRect(System.Windows.Documents.LogicalDirection.Backward);
                var lastBounds = text.TransformToAncestor(scroll).TransformBounds(lastCharacter);
                Program.Assert(scroll.VerticalOffset > 0 && scroll.VerticalOffset + scroll.ViewportHeight >= scroll.ExtentHeight - .7 &&
                        !lastCharacter.IsEmpty && lastBounds.Left >= -.7 && lastBounds.Top >= -.7 &&
                        lastBounds.Right <= scroll.ViewportWidth + .7 && lastBounds.Bottom <= scroll.ViewportHeight + .7,
                    "End reveals the final actual Baiyin rule character completely inside the real tooltip viewport.");
                Capture("bottom");
                Press(Key.Home);
                Program.Assert(Math.Abs(scroll.VerticalOffset) < .5 && text.Text == source.Description,
                    "Home restores the first paragraph without replacing or shortening the original long rule.");
                Capture("top");
                Program.Assert(TableShortcutChecks.Input(host.Window, Key.Escape), "The actual map window forwards Escape to its open printed-skill reader.");
                Flush();
                Program.Assert(!tooltip.IsOpen && model.IsShanheOpen && ReferenceEquals(Keyboard.FocusedElement, face),
                    "The first Escape closes only the actual long-rule tip while retaining the source map and focused skill.");
                AssertReadOnly();

                void Press(Key key)
                {
                    TableShortcutChecks.WithModifiers(ModifierKeys.None, () =>
                    {
                        Program.Assert(!TableShortcutChecks.Input(host.Window, key), "The map passes a read-only rule-navigation key to the focused skill.");
                        var input = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromVisual(face)!, Environment.TickCount, key)
                            { RoutedEvent = Keyboard.PreviewKeyDownEvent };
                        face.RaiseEvent(input);
                        Program.Assert(input.Handled && ReferenceEquals(Keyboard.FocusedElement, face),
                            "The actual skill face handles its own long-rule navigation without moving focus to another action.");
                        return true;
                    });
                    Flush(); AssertReadOnly();
                }
                void Capture(string position) => Program.Render(tooltip, (int)Math.Ceiling(tooltip.ActualWidth),
                    (int)Math.Ceiling(tooltip.ActualHeight), System.IO.Path.Combine(output, $"shanhe-journey-source-baiyin-{position}-{width}.png"));
            }
            finally { tooltip.IsOpen = false; tooltip.Opened -= Conceal; }

            void Flush() { host.Root.Dispatcher.Invoke(() => { }, DispatcherPriority.ContextIdle); host.Root.UpdateLayout(); tooltip.UpdateLayout(); }
        }

        void AssertReadOnly() => Program.Assert(ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == campaignBefore &&
            ReferenceEquals(Program.Engine(model), game) && GameCheckpointJson.Serialize(game.CreateCheckpoint()) == gameBefore,
            "Reading the actual source general's printed long rule changes neither campaign nor ordinary-game checkpoints; it is not a reward drop or a battle simulation.");
        static void Conceal(object sender, RoutedEventArgs args)
        { if (PresentationSource.FromVisual((ToolTip)sender) is HwndSource source) ShowWindow(source.Handle, 0); }
    }

    [DllImport("user32.dll")] private static extern bool ShowWindow(nint handle, int command);

    private static void VerifyHorizontalWheelInput(MainViewModel model, ShanheJourneyLoadout panel, ShanheCampaignSession session)
    {
        var before = ShanheCampaignJson.Serialize(session.CreateCheckpoint());
        var ordinaryRevision = Program.Engine(model).Revision;
        var reachedParent = 0;
        MouseWheelEventHandler parent = (_, _) => reachedParent++;
        panel.AddHandler(Mouse.MouseWheelEvent, parent);
        try
        {
            foreach (var name in new[] { "ShanheJourneyHandScroll", "ShanheJourneyTacticScroll", "ShanheJourneySkillScroll", "ShanheJourneyCompanionScroll" })
            {
                var scroll = (ScrollViewer)panel.FindName(name);
                var source = Program.Find<Border>(scroll).First(border => border.ToolTip is ToolTip);
                scroll.ScrollToLeftEnd(); Flush();
                var previousParents = reachedParent;
                var inwardRight = Wheel(-120);
                Program.Assert(inwardRight.Handled && scroll.HorizontalOffset > 0 && reachedParent == previousParents,
                    "A real bubbled wheel over HUD content scrolls its own overflowing strip right and stays within that strip: " + name);
                scroll.ScrollToRightEnd(); Flush();
                var end = scroll.HorizontalOffset;
                var inwardLeft = Wheel(120);
                Program.Assert(inwardLeft.Handled && scroll.HorizontalOffset < end && reachedParent == previousParents,
                    "The same content accepts the opposite wheel direction without changing the source item order: " + name);
                scroll.ScrollToLeftEnd(); Flush();
                var outwardLeft = Wheel(120);
                Program.Assert(!outwardLeft.Handled && scroll.HorizontalOffset == 0 && reachedParent == previousParents + 1,
                    "At the left boundary the actual original wheel event reaches the outer surface instead of getting swallowed: " + name);
                scroll.ScrollToRightEnd(); Flush();
                var outwardRight = Wheel(-120);
                Program.Assert(!outwardRight.Handled && Math.Abs(scroll.HorizontalOffset - scroll.ScrollableWidth) < .5 && reachedParent == previousParents + 2,
                    "At the right boundary the actual original wheel event also reaches the outer surface: " + name);

                MouseWheelEventArgs Wheel(int delta)
                {
                    var args = new MouseWheelEventArgs(Mouse.PrimaryDevice, Environment.TickCount, delta)
                        { RoutedEvent = Mouse.MouseWheelEvent };
                    source.RaiseEvent(args); Flush();
                    return args;
                }
            }
            Program.Assert(ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == before && Program.Engine(model).Revision == ordinaryRevision,
                "Scrolling every actual acquired hand, tactic, skill and companion strip leaves campaign commands, resources and the ordinary game untouched.");
        }
        finally { panel.RemoveHandler(Mouse.MouseWheelEvent, parent); }
        void Flush() { panel.Dispatcher.Invoke(() => { }, DispatcherPriority.ContextIdle); panel.UpdateLayout(); }
    }

    private static void AssertNativeHeaderAndHandRemainSeparate(NativeResponseStatusChecks.NativeHost host)
    {
        var campaign = (ShanheCampaignPanel)host.Window.FindName("ShanhePanel");
        var header = (FrameworkElement)campaign.FindName("ShanheChapterHeader");
        var actions = new[] { "ShanheRulesButton", "ShanheOpenMarketButton", "ShanheCloseButton", "ShanheEndButton" }
            .Select(name => (Button)campaign.FindName(name)).Where(button => button.IsVisible).ToArray();
        var actionBounds = actions.Select(Bounds).ToArray();
        Program.Assert(actionBounds.All(rect => new Rect(host.Root.RenderSize).Contains(rect) && !rect.IntersectsWith(Bounds(header))) &&
                actionBounds.SelectMany((rect, index) => actionBounds.Skip(index + 1).Select(other => !rect.IntersectsWith(other))).All(separate => separate),
            "The real compact and wide header keeps its chapter title and all four actions fully visible and separate.");
        foreach (var action in actions.Where(action => action.IsEnabled))
        {
            var hit = host.Root.InputHitTest(action.TranslatePoint(new Point(action.ActualWidth / 2, action.ActualHeight / 2), host.Root)) as DependencyObject;
            Program.Assert(hit is not null && (ReferenceEquals(hit, action) || action.IsAncestorOf(hit)),
                "The global end-action layer must leave every other enabled header action reachable: " + action.Name);
        }
        var loadout = Program.Find<ShanheJourneyLoadout>(campaign).Single(control => control.IsVisible);
        var companions = (ScrollViewer)loadout.FindName("ShanheJourneyCompanionScroll");
        Program.Assert(companions.ActualWidth <= 121 && companions.ActualHeight <= 56,
            "Two real companions occupy only their two portrait widths and full portrait height, rather than an invisible fixed HUD rectangle.");
        foreach (var blank in new[] { new Point(150, 20), new Point(30, 65) })
        {
            var hit = host.Root.InputHitTest(companions.TranslatePoint(blank, host.Root)) as DependencyObject;
            Program.Assert(hit is null || !ReferenceEquals(hit, loadout) && !loadout.IsAncestorOf(hit),
                "Empty space to the right and below a short companion row passes through the HUD to the map beneath it.");
        }
        var end = (Button)campaign.FindName("ShanheEndButton");
        var firstCard = Program.Find<Border>((ItemsControl)loadout.FindName("ShanheJourneyHandRows"))
            .First(border => border.DataContext is CardViewModel && border.ToolTip is ToolTip);
        Program.Assert(!Bounds(firstCard).IntersectsWith(Bounds(end)),
            "The global end-expedition action must not cover any part of the first carried card.");
        foreach (var y in new[] { 4d, firstCard.ActualHeight / 2, firstCard.ActualHeight - 4 })
        {
            var hit = host.Root.InputHitTest(firstCard.TranslatePoint(new Point(firstCard.ActualWidth / 2, y), host.Root)) as DependencyObject;
            Program.Assert(hit is not null && (ReferenceEquals(firstCard, hit) || firstCard.IsAncestorOf(hit)),
                "Top, middle and bottom of the real first hand card all remain card input targets rather than the end button.");
        }
        var model = (MainViewModel)campaign.DataContext;
        model.PreviewShanheNodeCommand.Execute(model.ShanheMapNodes.First(node => node.IsAvailable && node.IsBattle).Node);
        host.Root.Dispatcher.Invoke(() => { }, DispatcherPriority.ContextIdle); host.Root.UpdateLayout();
        var close = (Button)campaign.FindName("ShanheEncounterCloseButton");
        var closeHit = host.Root.InputHitTest(close.TranslatePoint(new Point(close.ActualWidth / 2, close.ActualHeight / 2), host.Root)) as DependencyObject;
        Program.Assert(model.IsShanheEncounterPreviewOpen && end.IsEnabled && close.IsVisible && close.IsEnabled &&
                !Bounds(close).IntersectsWith(Bounds(end)) && closeHit is not null && (ReferenceEquals(closeHit, close) || close.IsAncestorOf(closeHit)),
            "The global header end action and actual encounter close action remain separate and reachable while the preview is open.");
        model.CloseShanheEncounterPreviewCommand.Execute(null);
        Rect Bounds(FrameworkElement element) => element.TransformToAncestor(host.Root).TransformBounds(new Rect(element.RenderSize));
    }
}
