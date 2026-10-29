using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using CardGame.Content.Standard;
using CardGame.Core.Campaigns;
using CardGame.Wpf.Controls;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static partial class ShanheCodexUiChecks
{
    public static void UnlockEquipPersistAndMarket(string output)
    {
        var store = new MemoryShanheCodexStore();
        store.Write(new() { Books = 5000 }); // Earned currency fixture; never alters the player's profile.
        var campaign = new MemoryShanheCampaignSaveStore();
        using var vm = Create(store, campaign);
        vm.SelectedShanheScenario = vm.ShanheScenarios.Single(item => item.Id == StandardShanheCampaignCatalog.Returning2026EditionId);
        var book = new ShanheCodexPanel { DataContext = vm };
        Layout(book);
        Program.Assert(vm.ShanheCodexSlots.Count == 6 && vm.ShanheCodexSlots.All(slot => slot.RewardId is null) &&
            vm.ShanheCodexNodes.Count == 150 && Node(16).PathCost == 24 && Node(57).PathCost == 272,
            "Six initial slots and the actual prerequisite costs must be shown, rather than generated lanes or flat unlock prices.");
        Program.Render(book, 1400, 714, Path.Combine(output, "shanhe-book-empty.png"));
        vm.SelectCodexNodeCommand.Execute(Node(2));
        vm.UnlockCodexNodeCommand.Execute(null);
        Layout(book);
        var slotButton = Program.Find<Button>(book).Single(button => button.CommandParameter is ShanheCodexSlot { Index: 0 });
        typeof(ButtonBase).GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(slotButton, null);
        Layout(book);
        Program.Assert(vm.IsShanheCodexPickerOpen && vm.HasShanheEquippableTactics &&
            vm.ShanheEquippableTactics.All(node => node.Rarity == "普通") && vm.SelectedCodexNode is { TreeId: 2, TacticImage: not null },
            "Clicking an initial slot must open the picker with only unlocked tactics within that slot's rarity cap.");
        Program.Render(book, 1400, 714, Path.Combine(output, "shanhe-tactic-picker.png"));
        typeof(ButtonBase).GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke((Button)book.FindName("PickerEquipButton"), null);
        Layout(book);
        Program.Assert(!vm.IsShanheCodexPickerOpen && vm.ShanheCodexSlots[0].RewardId == StandardShanheCampaignCatalog.OpeningInheritanceRewardId,
            "The actual picker confirmation must equip inheritance, persist it and close the overlay.");
        var selectedIds = new[] { 16, 20, 33, 37, 44, 71 };
        for (var index = 0; index < selectedIds.Length; index++)
        {
            vm.SelectCodexSlotCommand.Execute(vm.ShanheCodexSlots[index]);
            vm.SelectCodexNodeCommand.Execute(Node(selectedIds[index]));
            var cost = vm.SelectedCodexNode!.PathCost;
            var balance = vm.ShanheBookBalance;
            Program.Assert(vm.UnlockCodexNodeCommand.CanExecute(null), "A supported tactic with sufficient books must be unlockable.");
            vm.UnlockCodexNodeCommand.Execute(null);
            Program.Assert(vm.ShanheBookBalance == balance - cost && vm.SelectedCodexNode!.IsUnlocked &&
                !vm.UnlockCodexNodeCommand.CanExecute(null), "Unlock must deduct the remaining route exactly once.");
            vm.UnlockCodexNodeCommand.Execute(null);
            vm.EquipCodexTacticCommand.Execute(null);
            Program.Assert(vm.ShanheBookBalance == balance - cost && vm.ShanheCodexSlots[index].RewardId == vm.SelectedCodexNode!.RuntimeRewardId,
                "A disabled repeated unlock cannot spend currency, and each selected slot receives the actual tactic.");
        }
        foreach (var id in new[] { 3, 55, 57 })
        {
            vm.SelectCodexNodeCommand.Execute(Node(id));
            if (vm.UnlockCodexNodeCommand.CanExecute(null)) vm.UnlockCodexNodeCommand.Execute(null);
            Program.Assert(vm.SelectedCodexNode!.IsUnlocked && !vm.EquipCodexTacticCommand.CanExecute(null),
                "Skill and equipment research must never be equippable in initial tactic slots.");
        }
        var saved = store.Read();
        Program.Assert(saved.EquippedTacticIds.OfType<string>().Count() == 6 && saved.UnlockedRewardIds.Contains("codex-55") &&
            saved.UnlockedRewardIds.Contains("codex-57"),
            "Account research and all six slots must persist independently of a run.");
        using (var cold = Create(store, new MemoryShanheCampaignSaveStore()))
            Program.Assert(cold.ShanheBookBalance == saved.Books && cold.ShanheCodexSlots.Select(slot => slot.RewardId).SequenceEqual(saved.EquippedTacticIds),
                "A fresh VM must restore the independent book and loadout profile.");
        vm.SelectCodexNodeCommand.Execute(Node(16));
        Layout(book);
        ((ScrollViewer)book.FindName("CodexTreeScroll")).ScrollToHorizontalOffset(Node(16).X - 160);
        Program.Render(book, 1400, 714, Path.Combine(output, "shanhe-book-equipped.png"));
        vm.StartShanheCommand.Execute(null);
        var checkpoint = campaign.Read().Campaign;
        Program.Assert(checkpoint.Options.UseCodexRewardPool && checkpoint.Options.EquippedTacticIds!.Count == 6 &&
            checkpoint.Options.UnlockedRewardIds!.Contains("codex-55") && checkpoint.Options.UnlockedRewardIds!.Contains("codex-57") &&
            !vm.EquipCodexTacticCommand.CanExecute(null),
            "Starting must freeze the equipped loadout and research pool; active expeditions cannot edit that loadout.");

        // Actual random-stock merchant, with fixture funds and all supported research available.
        var original = StandardShanheCampaignCatalog.CreateCodexDefinition(1, vm.SelectedShanheScenario.Id);
        var merchant = original.Nodes.First(node => node.Kind == ShanheNodeKind.Shop);
        var definition = original with { StartingNodeIds = original.StartingNodeIds.Append(merchant.Id).Distinct().ToArray(), StartingGold = 3000, RandomStartingNode = false };
        var session = ShanheCampaignSession.Create(definition, new(270017, definition.PlayerGeneralIds[0])
        { UseCodexRewardPool = true, UnlockedRewardIds = definition.Rewards.Select(reward => reward.Id).ToArray() }, StandardShanheCampaignCatalog.CreateBattleRegistry);
        Program.Assert(session.Submit(new MoveShanheNodeCommand(merchant.Id, session.Revision)).Accepted, "Merchant fixture must enter through the real command path.");
        Attach(vm, session);
        var shop = new ShanheShopPanel { DataContext = vm };
        Layout(shop);
        var categories = (ListBox)shop.FindName("MarketCategories");
        Program.Assert(categories.Items.Cast<string>().SequenceEqual(new[] { "战法", "装备", "卡牌", "技能" }),
            "The market exposes its four real goods categories without an empty supplies category.");
        Program.Assert(vm.ShanheShopItems.All(item => item.MarketCategory != "补给") &&
                session.Snapshot.ShopItems.All(item => item.Reward?.Id != "medicine"),
            "The actual opt-in catalogue merchant has no local medicine stock; showing categories cannot invent an unpriced source supply.");
        foreach (var category in new[] { "战法", "装备", "卡牌", "技能" })
        {
            categories.SelectedItem = category;
            Layout(shop);
            Program.Assert(vm.ShanheMarketGoods.Any() && vm.ShanheMarketGoods.All(item => item.MarketCategory == category),
                "The visible category selector must filter actual stock.");
            if (category is "装备" or "卡牌") Program.Assert(vm.ShanheMarketGoods.All(item => item.HasCardImage), "Card goods must show their real card art.");
            Program.Render(shop, 1400, 714, Path.Combine(output, "shanhe-market-" + category + ".png"));
        }
        categories.SelectedItem = "卡牌";
        Layout(shop);
        var goods = vm.ShanheMarketGoods.First(item => item.CanBuy);
        var sameCategory = vm.ShanheMarketGoods.First(item => item.Id != goods.Id);
        var gold = session.Snapshot.Gold;
        var button = Program.Find<Button>(shop).Single(item => ReferenceEquals(item.CommandParameter, goods));
        typeof(ButtonBase).GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(button, null);
        Layout(shop);
        Program.Assert(session.Snapshot.Gold == gold - goods.Price && vm.ShanheMarketGoods.Single(item => item.Id == sameCategory.Id).Price == sameCategory.Price + 100,
            "The real card purchase button must charge the shown price and update the other visible card prices.");
        gold = session.Snapshot.Gold;
        vm.RefreshShanheMarketCommand.Execute(null);
        Program.Assert(session.Snapshot.Gold == gold - 100 && !vm.RefreshShanheMarketCommand.CanExecute(null),
            "Refresh must use the real command and update its availability.");

        VerifyPreviouslyResearchedPathGainsCurrentRuntimeEntitlements();
        VerifyLegacyMedicineCategory(vm, session, shop, output);

        ShanheCodexNode Node(int id) => vm.ShanheCodexNodes.Single(node => node.TreeId == id);
    }

    private static void VerifyLegacyMedicineCategory(MainViewModel vm, ShanheCampaignSession sourceSession,
        ShanheShopPanel shop, string output)
    {
        // Reuse the published local village, its paid training and original next
        // merchant. As above, expose an existing authored node as a direct start;
        // do not add stock, change prices, or turn local medicine into source data.
        var published = StandardShanheCampaignCatalog.CreateDefinition(1);
        var definition = ShanheCampaignContent.FreezeAndValidate(published with
        {
            StartingNodeIds = published.StartingNodeIds.Append("village").Distinct().ToArray(),
            RandomStartingNode = false
        });
        const string player = "classic:zhang-fei";
        Program.Assert(definition.PlayerGeneralIds.Contains(player), "The original local player roster contains the unmodified training customer.");
        var legacy = ShanheCampaignSession.Create(definition, new(270017, player), StandardShanheCampaignCatalog.CreateBattleRegistry);
        var originalMaximum = legacy.Snapshot.MaxHp;
        Program.Assert(legacy.Submit(new MoveShanheNodeCommand("village", legacy.Revision)).Accepted &&
                legacy.Submit(new ChooseShanheNodeOptionCommand("training", null, legacy.Revision)).Accepted &&
                legacy.Snapshot.Hp == originalMaximum - 1 &&
                legacy.Submit(new MoveShanheNodeCommand("market1", legacy.Revision)).Accepted,
            "The original village training pays one real health before following its unchanged edge into the legacy merchant.");
        var authored = definition.Nodes.Single(node => node.Id == "market1").Choices!.Single(choice => choice.Id == "buy-medicine");
        var reward = definition.Rewards.Single(item => item.Id == authored.RewardId);
        Program.Assert(authored.GoldCost == 35 && reward.Id == "medicine" && reward.Heal == 3,
            "The legacy medicine check retains the published thirty-five-copper, three-health medicine package.");
        var sourceBefore = ShanheCampaignJson.Serialize(sourceSession.CreateCheckpoint());
        Attach(vm, legacy); Layout(shop);
        var health = (TextBlock)shop.FindName("MarketHealth");
        AssertHealthHeader("The actual legacy merchant header shows the village training wound before any supply purchase.");
        var woundedHealthText = health.Text;
        var categories = (ListBox)shop.FindName("MarketCategories");
        Program.Assert(categories.Items.Cast<string>().SequenceEqual(new[] { "战法", "装备", "卡牌", "技能", "补给" }) &&
                vm.ShanheShopItems.Any(item => item.MarketCategory == "补给" && item.Id == authored.Id),
            "An existing legacy supply product adds only its missing category to the actual bound merchant selector.");
        var beforeSelection = ShanheCampaignJson.Serialize(legacy.CreateCheckpoint());
        categories.SelectedItem = "补给"; Layout(shop);
        var medicine = vm.ShanheMarketGoods.Single(item => item.Id == authored.Id);
        var purchase = Program.Find<Button>(shop).Single(button => ReferenceEquals(button.CommandParameter, medicine) &&
            ReferenceEquals(button.Command, vm.RequestShanheShopPurchaseCommand));
        Program.Assert(vm.SelectedShanheMarketCategory == "补给" && vm.ShanheMarketGoods.All(item => item.MarketCategory == "补给") &&
                medicine.Name == reward.Name && medicine.Price == authored.GoldCost && medicine.CanBuy && purchase.IsEnabled &&
                vm.ShanheMarketCategoryNotice == "补给按当前货品说明结算" && vm.ShanheMarketSurchargeNotice.Length == 0 &&
                ShanheCampaignJson.Serialize(legacy.CreateCheckpoint()) == beforeSelection,
            "Selecting the real supply tab exposes its original medicine purchase without charging, healing, or advertising category escalation.");
        var before = legacy.Snapshot;
        typeof(ButtonBase).GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(purchase, null);
        Layout(shop);
        Program.Assert(legacy.Snapshot.Gold == before.Gold - authored.GoldCost &&
                legacy.Snapshot.Hp == Math.Min(before.MaxHp, before.Hp + reward.Heal) && legacy.Snapshot.Hp > before.Hp &&
                legacy.Snapshot.MaxHp == before.MaxHp && legacy.Snapshot.Phase == ShanheCampaignPhase.NodeChoice &&
                legacy.Snapshot.CurrentNodeId == "market1" &&
                legacy.AcceptedCommands.OfType<ChooseShanheNodeOptionCommand>().Last().ChoiceId == authored.Id &&
                !vm.ShanheMarketGoods.Single(item => item.Id == authored.Id).CanBuy &&
                !vm.RequestShanheShopPurchaseCommand.CanExecute(medicine),
            "The original bound medicine button pays thirty-five copper, actually restores the training wound up to the real ceiling, stays in the shop and retires its prior control.");
        AssertHealthHeader("The same live market header immediately reflects the actual medicine recovery and unchanged health ceiling.");
        Program.Assert(health.Text != woundedHealthText,
            "Medicine confirmation replaces the displayed wounded-health value rather than retaining the previous market header.");
        Program.Render(shop, 1400, 714, Path.Combine(output, "shanhe-legacy-market-supply-purchased.png"));
        var afterPurchase = ShanheCampaignJson.Serialize(legacy.CreateCheckpoint());
        typeof(ButtonBase).GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(purchase, null);
        vm.RequestShanheShopPurchaseCommand.Execute(medicine);
        Program.Assert(ShanheCampaignJson.Serialize(legacy.CreateCheckpoint()) == afterPurchase,
            "The old medicine button cannot charge or heal a second time after its actual purchase.");
        Attach(vm, sourceSession); Layout(shop);
        Program.Assert(categories.Items.Cast<string>().SequenceEqual(new[] { "战法", "装备", "卡牌", "技能" }) &&
                !vm.ShanheShopItems.Any(item => item.MarketCategory == "补给") &&
                vm.SelectedShanheMarketCategory == "战法" && Equals(categories.SelectedItem, "战法") &&
                vm.ShanheMarketGoods.All(item => item.MarketCategory == "战法") &&
                ShanheCampaignJson.Serialize(sourceSession.CreateCheckpoint()) == sourceBefore,
            "Returning to a real catalogue shelf without supplies removes the extra tab, resets its invalid selection and leaves that original source stock and journal unchanged.");

        void AssertHealthHeader(string message)
        {
            var values = System.Text.RegularExpressions.Regex.Matches(health.Text, @"-?\d+")
                .Select(match => int.Parse(match.Value)).ToArray();
            Program.Assert(health.Visibility == Visibility.Visible && health.ActualWidth > 0 && health.ActualHeight > 0 &&
                    health.Text == vm.ShanheMarketHealthText && values.SequenceEqual(new[] { legacy.Snapshot.Hp, legacy.Snapshot.MaxHp }),
                message + $" Actual='{health.Text}', expectedHp={legacy.Snapshot.Hp}, expectedMaxHp={legacy.Snapshot.MaxHp}.");
        }
    }

    private static void VerifyPreviouslyResearchedPathGainsCurrentRuntimeEntitlements()
    {
        const string reward = "codex-48", skill = "boundary:kanpo-current";
        const string tacticReward = StandardShanheCampaignCatalog.CurrentJinnangJiRewardId;
        const string tacticSkill = StandardShanheCampaignCatalog.CurrentJinnangJiSkillId;
        var profile = new ShanheCodexProfile
        {
            Books = 5000,
            UnlockedRewardIds = [.. ShanheCodexTree.RequiredPath(55, []).Select(entry => ShanheCodexTree.Key(entry.Id)), "codex-55", "codex-3"]
        };
        var account = new TrackingCodexStore(profile);
        var before = JsonSerializer.Serialize(account.Read());
        var currentDefinition = StandardShanheCampaignCatalog.CreateCodexDefinition(1, StandardShanheCampaignCatalog.Returning2026EditionId);
        Program.Assert(profile.UnlockedRewardIds.Contains(ShanheCodexTree.Key(48)) && !profile.UnlockedRewardIds.Contains(reward) &&
            profile.UnlockedRewardIds.Contains(ShanheCodexTree.Key(42)) && !profile.UnlockedRewardIds.Contains(tacticReward) &&
            currentDefinition.Rewards.Single(item => item.Id == reward).SkillId == skill &&
            currentDefinition.Rewards.Single(item => item.Id == tacticReward).TacticSkillId == tacticSkill,
            "A historical paid route to node 55 includes nodes 42 and 48's tree keys but predates their actual runtime reward mappings.");

        var frozen = ShanheCampaignSession.Create(currentDefinition, new(83, currentDefinition.PlayerGeneralIds[0])
        { UseCodexRewardPool = true, UnlockedRewardIds = ["codex-55", "codex-3"] }, StandardShanheCampaignCatalog.CreateBattleRegistry);
        var frozenStore = new MemoryShanheCampaignSaveStore();
        frozenStore.Write(new(ShanheCampaignSaveFile.CurrentFormatVersion, DateTimeOffset.UnixEpoch, 1, frozen.CreateCheckpoint()));
        var frozenBefore = ShanheCampaignJson.Serialize(frozenStore.Read().Campaign);
        using (var resumed = Create(account, frozenStore))
        {
            resumed.ContinueShanheCommand.Execute(null);
            var resumedRun = (ShanheCampaignSession)typeof(MainViewModel).GetField("_shanheSession", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(resumed)!;
            var upgraded = resumedRun.CreateCheckpoint();
            Program.Assert(resumed.IsShanheRunInProgress && resumed.ShanheExtraSkillCards.All(item => item.Id != skill) &&
                upgraded.Commands.Skip((int)frozen.Revision).SequenceEqual([new EnableShanheBalancedRewardsCommand(frozen.Revision)]) &&
                ShanheCampaignJson.Serialize(upgraded with { Revision = frozen.Revision, Commands = upgraded.Commands.Take((int)frozen.Revision).ToArray() }) == frozenBefore &&
                ShanheCampaignJson.Serialize(frozenStore.Read().Campaign) == ShanheCampaignJson.Serialize(upgraded) && account.Writes == 0,
                "Resuming preserves the exact original journal and unlocked reward options, appending only the safe reward-rule upgrade without granting new account entitlements.");
        }

        var frozenAfterUpgrade = ShanheCampaignJson.Serialize(frozenStore.Read().Campaign);
        var currentStore = new MemoryShanheCampaignSaveStore();
        using (var current = Create(account, currentStore))
        {
            current.SelectedShanheScenario = current.ShanheScenarios.Single(item => item.Id == StandardShanheCampaignCatalog.Returning2026EditionId);
            current.SelectCodexNodeCommand.Execute(current.ShanheCodexNodes.Single(node => node.TreeId == 48));
            Program.Assert(current.SelectedCodexNode is { IsUnlocked: true, IsAvailable: true, IsSkill: true, RuntimeRewardId: reward } &&
                !current.UnlockCodexNodeCommand.CanExecute(null) && !current.EquipCodexTacticCommand.CanExecute(null),
                "The newly implemented skill is already researched by its paid route; it neither charges books again nor occupies an initial tactic slot.");
            current.SelectedShanheScenario = current.ShanheScenarios.Single(item => item.Id == StandardShanheCampaignCatalog.LocalEditionId);
            Program.Assert(current.SelectedCodexNode is { TreeId: 48, IsAvailable: false }, "The original edition keeps the same tree node as a preview.");
            current.SelectCodexNodeCommand.Execute(current.ShanheCodexNodes.Single(node => node.TreeId == 42));
            Program.Assert(current.SelectedCodexNode is { IsAvailable: false }, "The original edition also keeps current Jinnang Ji as a preview.");
            current.SelectedShanheScenario = current.ShanheScenarios.Single(item => item.Id == StandardShanheCampaignCatalog.Returning2026EditionId);
            Program.Assert(account.Writes == 0 && JsonSerializer.Serialize(account.Read()) == before && current.ShanheBookBalance == profile.Books,
                "Reading and switching editions only derives availability; it never migrates or saves the account implicitly.");
            current.StartShanheCommand.Execute(null);
            var issued = currentStore.Read().Campaign;
            var unlocked = issued.Options.UnlockedRewardIds ?? throw new InvalidOperationException("The current codex expedition must freeze its explicit unlock pool.");
            Program.Assert(unlocked.Contains(reward) && unlocked.Contains("codex-55") && unlocked.Contains(tacticReward) &&
                unlocked.Contains("codex-3") && issued.Options.EquippedTacticIds.Count == 0 &&
                current.ShanheExtraSkillCards.Count == 0 && current.ShanheTacticCards.Count == 0 &&
                current.ShanheCodexSlots.All(slot => slot.RewardId is null) && account.Writes == 0,
                "A genuinely started current expedition freezes both newly derived and old direct unlock IDs without granting unearned skills or initial tactics.");
        }

        var historicalStore = new MemoryShanheCampaignSaveStore();
        using (var historical = Create(account, historicalStore))
        {
            historical.SelectedShanheScenario = historical.ShanheScenarios.Single(item => item.Id == StandardShanheCampaignCatalog.LocalEditionId);
            historical.StartShanheCommand.Execute(null);
            var issued = historicalStore.Read().Campaign.Options;
            var unlocked = issued.UnlockedRewardIds ?? throw new InvalidOperationException("The original codex expedition must freeze its explicit unlock pool.");
            Program.Assert(!unlocked.Contains(reward) && !unlocked.Contains("codex-55") && !unlocked.Contains(tacticReward) &&
                unlocked.Contains("codex-3") && account.Writes == 0,
                "A new original-edition run filters out current-only derived rewards while preserving supported direct rewards.");
        }

        using (var research = Create(account, new MemoryShanheCampaignSaveStore()))
        {
            research.SelectedShanheScenario = research.ShanheScenarios.Single(item => item.Id == StandardShanheCampaignCatalog.Returning2026EditionId);
            research.SelectCodexNodeCommand.Execute(research.ShanheCodexNodes.Single(node => node.TreeId == 57));
            var cost = research.SelectedCodexNode!.PathCost;
            Program.Assert(cost > 0 && research.UnlockCodexNodeCommand.CanExecute(null), "The historical profile still has an actual unresearched, supported route.");
            research.UnlockCodexNodeCommand.Execute(null);
            Program.Assert(account.Writes == 1 && account.Read().Books == profile.Books - cost &&
                account.Read().UnlockedRewardIds.Contains(reward) && account.Read().UnlockedRewardIds.Contains(tacticReward) &&
                account.Read().UnlockedRewardIds.Contains("codex-3") &&
                ShanheCampaignJson.Serialize(frozenStore.Read().Campaign) == frozenAfterUpgrade,
                "Only explicit research persists the derived old entitlement, charges just the new route, and leaves an existing run's frozen options untouched.");
        }

        var equippedAccount = new TrackingCodexStore(profile) { RejectValidatedWrites = true };
        var equippedCampaign = new MemoryShanheCampaignSaveStore();
        using (var equipped = Create(equippedAccount, equippedCampaign))
        {
            equipped.SelectedShanheScenario = equipped.ShanheScenarios.Single(item => item.Id == StandardShanheCampaignCatalog.Returning2026EditionId);
            equipped.SelectCodexSlotCommand.Execute(equipped.ShanheCodexSlots[2]);
            equipped.SelectCodexNodeCommand.Execute(equipped.ShanheCodexNodes.Single(node => node.TreeId == 42));
            Program.Assert(equipped.SelectedCodexNode is { IsUnlocked: true, IsAvailable: true, IsTactic: true, Rarity: "稀有", RuntimeRewardId: tacticReward } &&
                equipped.EquipCodexTacticCommand.CanExecute(null) && !equipped.UnlockCodexNodeCommand.CanExecute(null) && equippedAccount.Writes == 0,
                "The previously paid node 42 can enter the third, rare tactic slot without purchasing its route again.");
            equipped.EquipCodexTacticCommand.Execute(null);
            Program.Assert(equippedAccount.RejectedWrites == 1 && equippedAccount.Writes == 0 &&
                JsonSerializer.Serialize(equippedAccount.Read()) == before && equipped.ShanheCodexSlots.All(slot => slot.RewardId is null) &&
                equipped.SelectedCodexNode is { IsEquipped: false } && equipped.ShanheBookBalance == profile.Books &&
                equipped.ShanheCodexMessage.Contains("未保存"),
                "A failed write after real profile validation publishes neither the derived unlock nor the equipped slot and spends no books.");
            equippedAccount.RejectValidatedWrites = false;
            equipped.EquipCodexTacticCommand.Execute(null);
            var saved = equippedAccount.Read();
            Program.Assert(equippedAccount.Writes == 1 && saved.Books == profile.Books && saved.UnlockedRewardIds.Contains(tacticReward) &&
                saved.EquippedTacticIds[2] == tacticReward && saved.EquippedTacticIds.Where((_, index) => index != 2).All(id => id is null) &&
                equipped.ShanheCodexSlots[2].RewardId == tacticReward,
                "The actual equip commits the newly derived entitlement and exactly one occupied slot in one profile write, with no book charge.");
            equipped.StartShanheCommand.Execute(null);
            Program.Assert(equippedCampaign.Read().Campaign.Options.EquippedTacticIds.SequenceEqual([tacticReward]) &&
                equipped.ShanheTacticCards.Select(item => item.Id).SequenceEqual([tacticReward]) && equipped.ShanheExtraSkillCards.Count == 0 &&
                equippedAccount.Writes == 1 && equipped.ShanheBookBalance == profile.Books,
                "A real new expedition carries the equipped Jinnang Ji as a tactic, never an extra general skill or a second account payment.");
        }
    }

    private sealed class TrackingCodexStore(ShanheCodexProfile profile) : IShanheCodexStore
    {
        private readonly MemoryShanheCodexStore _inner = CreateInitial(profile);
        public int Writes { get; private set; }
        public int RejectedWrites { get; private set; }
        public bool RejectValidatedWrites { get; set; }
        public bool Exists => _inner.Exists;
        public ShanheCodexProfile Read() => _inner.Read();
        public void Write(ShanheCodexProfile next)
        {
            if (RejectValidatedWrites)
            {
                new MemoryShanheCodexStore().Write(next); // Uses the real file-profile validation before the I/O failure.
                RejectedWrites++;
                throw new IOException("fixture: account write rejected after validation");
            }
            _inner.Write(next); Writes++;
        }
        private static MemoryShanheCodexStore CreateInitial(ShanheCodexProfile initial)
        { var store = new MemoryShanheCodexStore(); store.Write(initial); return store; }
    }

    public static void FailedProfileWritePreservesProgress()
    {
        using var vm = Create(new FailingStore(), new MemoryShanheCampaignSaveStore());
        vm.SelectCodexNodeCommand.Execute(vm.ShanheCodexNodes.Single(node => node.TreeId == 16));
        vm.UnlockCodexNodeCommand.Execute(null);
        Program.Assert(vm.ShanheBookBalance == 5000 && !vm.SelectedCodexNode!.IsUnlocked && vm.ShanheCodexMessage.Contains("未保存"),
            "A failed profile write must neither consume books nor publish an unlock.");
    }
    private sealed class FailingStore : IShanheCodexStore
    {
        public bool Exists => true;
        public ShanheCodexProfile Read() => new() { Books = 5000 };
        public void Write(ShanheCodexProfile profile) => throw new IOException("fixture");
    }
    private static MainViewModel Create(IShanheCodexStore store, IShanheCampaignSaveStore campaign) =>
        new(false, 270017, true, new MemorySaveStore(), useExpandedContent: true, shanheSaveStore: campaign, shanheCodexStore: store);
    private static void Attach(MainViewModel vm, ShanheCampaignSession session)
    {
        typeof(MainViewModel).GetField("_shanheSession", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(vm, session);
        typeof(MainViewModel).GetMethod("RefreshShanhePresentation", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(vm, null);
    }
    private static void Layout(FrameworkElement root)
    {
        root.Measure(new Size(1050, 535)); root.Arrange(new Rect(0, 0, 1050, 535)); root.UpdateLayout();
        root.Dispatcher.Invoke(() => { }, DispatcherPriority.DataBind); root.UpdateLayout();
    }
}
