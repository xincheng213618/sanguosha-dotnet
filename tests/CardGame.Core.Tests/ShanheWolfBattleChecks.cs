using System.Collections;
using System.Reflection;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheWolfBattleChecks
{
    private static readonly FieldInfo StateField = typeof(ShanheCampaignSession).GetField("_state", BindingFlags.NonPublic | BindingFlags.Instance)!;
    private static readonly MethodInfo PrimaryRarity = typeof(ShanheCampaignSession).GetMethod("IsPrimaryRewardRarity", BindingFlags.NonPublic | BindingFlags.Static)!;
    private static readonly MethodInfo Category = typeof(ShanheCampaignSession).GetMethod("RewardCategory", BindingFlags.NonPublic | BindingFlags.Static)!;
    private static readonly MethodInfo BonusPlanner = typeof(ShanheCampaignSession).GetMethod("BuildBonusRewardGroups", BindingFlags.NonPublic | BindingFlags.Instance)!;
    private static ShanheEconomyTacticSource Wolf => StandardShanheCampaignCatalog.CurrentEconomyTactics.Single(source => source.SourceId == 2015);
    private static ShanheEconomyTacticSource EliteBonus => StandardShanheCampaignCatalog.CurrentEconomyTactics.Single(source => source.SourceId == 2013);

    public static void PaidFutureNodesPreviewAndNativeEntryUseTheRealEliteProfile()
    {
        Need(Wolf is { Rarity: 3, Price: 500, Effect.UpgradeOrdinaryBattles: true },
            "The actual 2015 catalogue row must be epic, five hundred copper, and carry the new native expedition effect.");
        var baseline = SmallSession();
        Need(baseline.Snapshot.CurrentBattleNode is null && baseline.Snapshot.AvailableNodes.Single(node => node.Id == "normal").Kind == ShanheNodeKind.Battle &&
            baseline.VisibleMapNodes.Single(node => node.Id == "later").Kind == ShanheNodeKind.Battle &&
            PlanBonuses(baseline, baseline.Definition.Nodes.Single(node => node.Id == "normal")).Count == 0,
            "Owning the existing elite-bonus tactic does not upgrade an ordinary entry or award an elite-only bonus by itself.");
        var ordinaryPreview = PreviewUnchanged(baseline, "normal");
        Need(ordinaryPreview.Enemies is [{ Hp: 3, InitialArmor: 0 }], "The unowned preview remains the authored ordinary roster.");
        Move(baseline, "normal");
        Need(baseline.Snapshot.CurrentBattleNode is { Kind: ShanheNodeKind.Battle } &&
            StartNative(baseline.PendingBattleSetup!, ShanheCampaignChecks.Registry).CreateSnapshot(0).Players.Count == 2,
            "A real unowned entry installs the ordinary one-enemy battle.");

        var session = SmallSession();
        var normal = session.Definition.Nodes.Single(node => node.Id == "normal");
        var elite = session.Definition.Nodes.Single(node => node.Id == normal.EliteUpgradeNodeId);
        var eliteBefore = session.VisibleMapNodes.Single(node => node.Id == "elite");
        var bossBefore = session.VisibleMapNodes.Single(node => node.Id == "boss");
        BuyOnMap(session);
        var projected = session.Snapshot.AvailableNodes.Single(node => node.Id == "normal");
        Need(session.Snapshot.Gold == 500 && session.Snapshot.TacticIds.Contains(Wolf.RewardId) &&
            projected.Kind == ShanheNodeKind.EliteBattle && projected.Id == normal.Id && projected.NextNodeIds.SequenceEqual(normal.NextNodeIds) &&
            projected.PrimaryTacticRarity == elite.PrimaryTacticRarity && projected.PrimarySkillRarity == elite.PrimarySkillRarity &&
            projected.RewardIds!.SequenceEqual(elite.RewardIds!) &&
            session.VisibleMapNodes.Single(node => node.Id == "later").Kind == ShanheNodeKind.EliteBattle &&
            SameMapNode(session.VisibleMapNodes.Single(node => node.Id == "elite"), eliteBefore) &&
            SameMapNode(session.VisibleMapNodes.Single(node => node.Id == "boss"), bossBefore) && session.Snapshot.CurrentBattleNode is null,
            "A real five-hundred purchase upgrades future ordinary map entries and their actual elite loot, preserves route identity, and leaves existing elites and bosses untouched.");
        var preview = PreviewUnchanged(session, "normal");
        Need(preview.Id == "wolf:normal:elite-fight" && preview.SourceCityId == 5107 && preview.SourceBattleId is null && preview.Enemies.Count == 2,
            "The preview uses the actual elite encounter while retaining the ordinary city and declaring no borrowed official battle identity.");
        Move(session, "normal");
        var profile = session.Snapshot.CurrentBattleNode!;
        var setup = session.PendingBattleSetup!;
        Need(profile.Kind == ShanheNodeKind.EliteBattle && profile.Id == "normal" && setup.NodeId == "normal" &&
            setup.EncounterOrdinal == 1 && SameRoster(preview.Enemies, setup.EnemyRoster),
            "The real Move command locks exactly the previewed elite roster and reward profile on the original route entry.");
        var game = StartNative(setup, ShanheCampaignChecks.Registry);
        Need(game.CreateSnapshot(0).Players.Select(player => (player.GeneralId, player.Hp, player.MaxHp, player.Armor))
                .SequenceEqual(new[] { (ShanheCampaignChecks.Player, 4, 4, 0), (ShanheCampaignChecks.Enemy, 6, 6, 2), (ShanheCampaignChecks.Enemy, 7, 7, 3) }),
            "Native startup installs both real elite enemies with their own health, maximum health and initial armor.");
        var offers = PrimaryOffers(session, profile);
        Need(offers.Select(reward => reward.Id).Order().SequenceEqual(new[] { "elite-skill", "rare-tactic" }) &&
            PlanBonuses(session, profile) is [{ IsBonus: true, OfferIds: var ids }] && ids.SequenceEqual(["rare-tactic"]) &&
            session.Snapshot.Phase == ShanheCampaignPhase.Battle && game.State.Winner == Winner.None,
            "The existing pure reward planners use the locked elite's real rare reward pool and actually enable the owned elite-bonus tactic, without inventing a victory.");
        foreach (var invalid in new[]
        {
            session.Definition with { Nodes = session.Definition.Nodes.Select(node => node.Id == "normal"
                ? node with { EliteUpgradeNodeId = "missing-elite" } : node).ToArray() },
            session.Definition with { Nodes = session.Definition.Nodes.Select(node => node.Id == "normal"
                ? node with { EliteUpgradeNodeId = "boss" } : node).ToArray() },
            session.Definition with { Nodes = session.Definition.Nodes.Select(node => node.Id == "elite"
                ? node with { ChapterNumber = 2 } : node).ToArray() }
        })
        {
            var rejected = false;
            try { _ = ShanheCampaignContent.FreezeAndValidate(invalid); }
            catch (ArgumentException) { rejected = true; }
            Need(rejected, "An upgrade must reject an unknown target, a boss instead of an elite, or an elite from another chapter at content loading.");
        }
    }

    public static void PublishedSourceChaptersUseEliteGrowthAndActualRareRewardPools()
    {
        BothRecommendedEntriesUpgradeTheNewThirdChapterPatrol();
        var definition = StandardShanheCampaignCatalog.CreateSourceDefinition(11);
        var wolf = definition.Rewards.Single(reward => reward.Id == Wolf.RewardId);
        Need(wolf is { Rarity: 3, CampaignEconomy.UpgradeOrdinaryBattles: true } &&
            StandardShanheCampaignCatalog.ShopMetadataByRewardId[wolf.Id].Price == 500,
            "The actual source campaign registers the real epic tactic and its five-hundred shop product.");
        foreach (var chapter in new[] { 1, 2, 3 })
        {
            var ordinary = definition.Nodes.First(node => node.ChapterNumber == chapter && node.Kind == ShanheNodeKind.Battle &&
                !node.IsStoryBattle && node.EliteUpgradeNodeId is not null);
            var elite = definition.Nodes.Single(node => node.Id == ordinary.EliteUpgradeNodeId);
            // Admit the real selected entry alongside the original starts so
            // every original route and elite-profile reference stays intact.
            // No win is simulated to reach a later chapter.
            var targeted = definition with
            { StartingNodeIds = [.. definition.StartingNodeIds, ordinary.Id], RandomStartingNode = false };
            var session = ShanheCampaignSession.Create(targeted,
                new(71, "classic:guan-yu") { SourceDifficultyId = 11, EquippedTacticIds = [wolf.Id, EliteBonus.RewardId] },
                StandardShanheCampaignCatalog.CreateBattleRegistry);
            var preview = PreviewUnchanged(session, ordinary.Id);
            var city = ordinary.Encounters!.Select(encounter => encounter.SourceCityId).Distinct().Single();
            Need(session.Snapshot.AvailableNodes.Single(node => node.Id == ordinary.Id).Kind == ShanheNodeKind.EliteBattle && preview.SourceCityId == city &&
                preview.SourceBattleId is null && preview.Id.StartsWith("wolf:" + ordinary.Id + ":", StringComparison.Ordinal) &&
                preview.Enemies.Count == chapter + 1,
                "Each source chapter projects its own two-, three- or four-enemy local elite at the actual ordinary city.");
            Move(session, ordinary.Id);
            var profile = session.Snapshot.CurrentBattleNode!;
            var setup = session.PendingBattleSetup!;
            Need(profile is { Kind: ShanheNodeKind.EliteBattle, PrimaryTacticRarity: 2, PrimarySkillRarity: 2 } &&
                profile.ChapterNumber == chapter && profile.Id == ordinary.Id && profile.NextNodeIds.SequenceEqual(ordinary.NextNodeIds) &&
                profile.RewardIds!.SequenceEqual(elite.RewardIds!) && SameRoster(preview.Enemies, setup.EnemyRoster),
                "Native entry fixes the same-chapter elite encounter and actual rare reward profile while keeping ordinary route links.");
            var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup);
            var raw = elite.Encounters!.Single(encounter => preview.Id.EndsWith(":" + encounter.Id, StringComparison.Ordinal));
            var deck = registry.Decks[registry.Modes[ShanheBattleSetup.BattleModeId].DeckId!];
            for (var index = 0; index < raw.Enemies.Count; index++)
            {
                var basis = raw.Enemies[index];
                var general = registry.Generals[basis.GeneralId];
                var maximum = general.BaseHp + basis.MaxHpBonus;
                var baseHp = basis.Hp ?? general.InitialHp ?? maximum;
                var attributes = new ShanheSourceEnemyAttributes(baseHp, deck.DrawPerTurn + basis.DrawBonus, basis.SlashBonus,
                    basis.ArmorRecoveryPerRound, (basis.StartingHand?.Count ?? deck.InitialHandSize) + basis.InitialHandBonus,
                    basis.InitialArmor, basis.ExtraDamage);
                var expected = definition.SourceEnemyGrowth!.Calculate(attributes,
                    new(11, 1, chapter, raw.SourceVisibleEnemyCount ?? raw.Enemies.Count, ShanheSourceEncounterCategory.Elite),
                    basis.SourceDifficultyAttributes).Attributes;
                var grown = setup.EnemyRoster[index];
                Need(grown.GeneralId == basis.GeneralId && grown.Hp == expected.Hp && grown.InitialArmor == expected.Armor &&
                    grown.MaxHpBonus == basis.MaxHpBonus + expected.Hp - baseHp &&
                    deck.DrawPerTurn + grown.DrawBonus == expected.Draw && grown.SlashBonus == expected.ExtraSlash &&
                    (grown.StartingHand?.Count ?? deck.InitialHandSize) + grown.InitialHandBonus == expected.StartingCards &&
                    grown.ArmorRecoveryPerRound == expected.ArmorGain && grown.ExtraDamage == expected.ExtraDamage,
                    "Every prepared enemy's seven source attributes use this chapter's actual Elite growth category.");
            }
            var game = StartNative(setup, _ => registry);
            var enemies = game.CreateSnapshot(0).Players.Where(player => player.TeamId == ShanheBattleSetup.EnemyTeamId).ToArray();
            Need(enemies.Length == setup.EnemyRoster.Count && enemies.Select(player => player.GeneralId).SequenceEqual(setup.EnemyRoster.Select(member => member.GeneralId)) &&
                enemies.Where((player, index) => player.Hp != setup.EnemyRoster[index].Hp || player.Armor != setup.EnemyRoster[index].InitialArmor ||
                    player.MaxHp != registry.Generals[player.GeneralId].BaseHp + setup.EnemyRoster[index].MaxHpBonus).Count() == 0,
                "The published encounter registry actually starts the grown elite bodies in native battle rather than merely relabeling the map.");
            var offers = PrimaryOffers(session, profile);
            Need(offers.Any(reward => RewardCategory(reward) == ShanheRewardCategory.Tactic) &&
                offers.Any(reward => RewardCategory(reward) == ShanheRewardCategory.Skill) &&
                offers.Where(reward => RewardCategory(reward) is ShanheRewardCategory.Tactic or ShanheRewardCategory.Skill).All(reward => reward.Rarity == 2) &&
                PlanBonuses(session, profile) is [{ IsBonus: true, OfferIds: var bonusIds }] && bonusIds.Count > 0 &&
                bonusIds.All(id => definition.Rewards.Single(reward => reward.Id == id) is { Rarity: 2 } reward && RewardCategory(reward) == ShanheRewardCategory.Tactic),
                "Each published chapter exposes its actual rare tactic/skill loot and the existing real elite-bonus planner supplies eligible rare tactics.");
        }
    }

    private static void BothRecommendedEntriesUpgradeTheNewThirdChapterPatrol()
    {
        foreach (var codex in new[] { false, true })
        {
            var definition = codex
                ? StandardShanheCampaignCatalog.CreateCodexDefinition(2, StandardShanheCampaignCatalog.Returning2026EditionId)
                : StandardShanheCampaignCatalog.CreateDefinition(2, StandardShanheCampaignCatalog.Returning2026EditionId);
            var patrol = definition.Nodes.Single(node => node.Id == StandardShanheCampaignCatalog.ThirdChapterPatrolNodeId);
            Need(patrol.Kind == ShanheNodeKind.Battle && patrol.EliteUpgradeNodeId == StandardShanheCampaignCatalog.ThirdChapterDongwuEliteId,
                "Both recommended entrypoints register the same real third-chapter elite profile for the new ordinary patrol.");
            var entry = definition with
            {
                StartingNodeIds = definition.StartingNodeIds.Append(patrol.Id).Distinct(StringComparer.Ordinal).ToArray(),
                RandomStartingNode = false
            };
            var session = ShanheCampaignSession.Create(entry, new(71, "classic:guan-yu")
                { EquippedTacticIds = [Wolf.RewardId] }, StandardShanheCampaignCatalog.CreateBattleRegistry);
            var preview = PreviewUnchanged(session, patrol.Id);
            Need(session.Snapshot.AvailableNodes.Single(node => node.Id == patrol.Id).Kind == ShanheNodeKind.EliteBattle &&
                preview.SourceBattleId is null && preview.SourceCityId is null && preview.Enemies.Count == 3 &&
                preview.Enemies.Select(member => member.GeneralId).SequenceEqual(new[]
                {
                    StandardShanheEliteDongwuPackage.HuangGaiId, StandardShanheEliteHandangPackage.HandangId,
                    StandardShanheEliteDongwuPackage.ChengPuId
                }),
                "Owning Wolf Battles really upgrades the fresh patrol preview to the existing three-person elite instead of leaving codex entry ordinary.");
            Move(session, patrol.Id);
            Need(session.Snapshot.CurrentBattleNode is { Kind: ShanheNodeKind.EliteBattle, ChapterNumber: 3, BossRewards: null } actual &&
                actual.Id == patrol.Id && actual.NextNodeIds.SequenceEqual(patrol.NextNodeIds) &&
                SameRoster(preview.Enemies, session.PendingBattleSetup!.EnemyRoster) &&
                session.PendingBattleSetup.SourceDifficultyId is null && session.Snapshot.Phase == ShanheCampaignPhase.Battle,
                "The actual fresh Move locks that same elite roster and onward routes without a final-victory reward or fabricated battle result.");
        }
    }

    public static void BattlePurchaseKeepsTheEnteredOrdinaryProfileAndPaysThroughLiveMarket()
    {
        var session = SmallSession();
        var quote = session.Snapshot.ShopItems.Single(item => item.Reward?.Id == Wolf.RewardId);
        Move(session, "normal");
        var setup = session.PendingBattleSetup!;
        var current = session.Snapshot.CurrentBattleNode;
        Need(current is { Kind: ShanheNodeKind.Battle } && setup.EnemyRoster.Count == 1,
            "This native battle really began without the upgrade tactic and locked the ordinary profile.");
        var before = session.Snapshot;
        Need(!session.Submit(new CardGame.Core.Campaigns.BuyShanheMarketItemCommand(quote.ChoiceId, null, session.Revision)).Accepted &&
            ReferenceEquals(before, session.Snapshot),
            "The active battle owns its market; bypassing it with a direct campaign purchase must be rejected unchanged.");
        var game = StartNative(setup, ShanheCampaignChecks.Registry);
        var playersBefore = NativeBodies(game);
        var marketBefore = game.CreateShanheMarketSnapshot(0)!;
        var item = marketBefore.ShopItems.Single(value => value.Reward?.Id == Wolf.RewardId);
        Need(item is { GoldCost: 500, CanPurchase: true, Reward.CampaignEconomy.UpgradeOrdinaryBattles: true },
            "The real battle market offers the actual five-hundred epic upgrade effect.");
        Accept(game, new CardGame.Core.BuyShanheMarketItemCommand(0, item.ChoiceId, null, game.Revision));
        ReachPlay(game);
        var marketAfter = game.CreateShanheMarketSnapshot(0)!;
        Need(marketAfter.Gold == marketBefore.Gold - 500 && marketAfter.TacticIds.Contains(Wolf.RewardId) &&
            game.Events.Select(value => value.Payload).OfType<ShanheMarketPurchaseCommittedEvent>() is var commits && commits.Single().Price == 500 &&
            game.Events.Select(value => value.Payload).OfType<ShanheMarketPurchaseAppliedEvent>().Single().RewardId == Wolf.RewardId &&
            NativeBodies(game).SequenceEqual(playersBefore) && ReferenceEquals(session.Snapshot.CurrentBattleNode, current) &&
            session.Snapshot.CurrentBattleNode!.Kind == ShanheNodeKind.Battle && SameRoster(setup.EnemyRoster, session.PendingBattleSetup!.EnemyRoster),
            "Native purchase owns the economy effect and charges once immediately, while the already-entered ordinary enemies and loot stay fixed.");
        Need(!session.Snapshot.TacticIds.Contains(Wolf.RewardId) && session.VisibleMapNodes.Single(node => node.Id == "later").Kind == ShanheNodeKind.Battle,
            "The owning journey is not falsely synchronized before its battle result returns; this check does not fabricate that later transfer or victory.");
    }

    private static ShanheCampaignSession SmallSession()
    {
        var income = StandardShanheCampaignCatalog.CurrentEconomyTactics.Single(source => source.SourceId == 2089);
        ShanheRewardDefinition[] rewards =
        [new(Wolf.RewardId, Wolf.Name) { Rarity = Wolf.Rarity, CampaignEconomy = Wolf.Effect },
         new(EliteBonus.RewardId, EliteBonus.Name) { Rarity = EliteBonus.Rarity, CampaignEconomy = EliteBonus.Effect },
         new("common-tactic", "普通战法", Bonuses: new(Slash: 1)) { Rarity = 1 },
         new("ordinary-skill", "普通技能", SkillId: "fixture:shanhe-guard") { Rarity = 1 },
         new("rare-tactic", "实际稀有经济战法") { Rarity = 2, CampaignEconomy = income.Effect },
         new("elite-skill", "稀有技能", SkillId: "fixture:shanhe-raid") { Rarity = 2 }];
        var ordinary = new ShanheNodeDefinition("normal", "普通入口", ShanheNodeKind.Battle, ["exit"],
            [new("normal-fight", "普通守军", [new(ShanheCampaignChecks.Enemy, Hp: 3)], VictoryGold: 200) { SourceCityId = 5107 }],
            ["common-tactic", "ordinary-skill"])
        { ChapterNumber = 1, EliteUpgradeNodeId = "elite", CategorizedRewards = true, PrimaryTacticRarity = 1, PrimarySkillRarity = 1 };
        var elite = new ShanheNodeDefinition("elite", "真实精英模板", ShanheNodeKind.EliteBattle, ["boss"],
            [new("elite-fight", "精英双将",
                [new(ShanheCampaignChecks.Enemy, MaxHpBonus: 2, Hp: 6) { InitialArmor = 2 },
                 new(ShanheCampaignChecks.Enemy, MaxHpBonus: 3, Hp: 7) { InitialArmor = 3 }], VictoryGold: 200)],
            ["rare-tactic", "elite-skill"])
        { ChapterNumber = 1, CategorizedRewards = true, PrimaryTacticRarity = 2, PrimarySkillRarity = 2 };
        var definition = new ShanheCampaignDefinition("fixture:wolf-battles", "群狼之争真实入口", [ShanheCampaignChecks.Player], ["shop"],
            [new("shop", "常驻集市", ShanheNodeKind.Shop, ["normal", "later", "elite", "boss"], Choices:
                [new("wolf", Wolf.Name, Wolf.Price, RewardId: Wolf.RewardId, LeavesNode: false)]) { ChapterNumber = 1 },
             ordinary, ordinary with { Id = "later", Name = "下一普通入口" }, elite,
             new("exit", "普通入口自己的路线", ShanheNodeKind.Event, ["boss"], Choices: [new("continue", "继续")]) { ChapterNumber = 1 },
             new("boss", "原关底", ShanheNodeKind.BossBattle, [], [new("boss-fight", "关底", [new(ShanheCampaignChecks.Enemy)])]) { ChapterNumber = 1 }],
            rewards, StartingGold: 1000);
        return ShanheCampaignSession.Create(definition,
            new(71, ShanheCampaignChecks.Player) { UseCodexRewardPool = true, PersistentMarket = true, EquippedTacticIds = [EliteBonus.RewardId] },
            ShanheCampaignChecks.Registry);
    }

    private static void BuyOnMap(ShanheCampaignSession session)
    {
        var item = session.Snapshot.ShopItems.Single(value => value.Reward?.Id == Wolf.RewardId);
        Need(item is { CanPurchase: true, GoldCost: 500 }, "The published real map product must be purchasable for exactly five hundred.");
        Accept(session, new CardGame.Core.Campaigns.BuyShanheMarketItemCommand(item.ChoiceId, null, session.Revision));
    }
    private static ShanheEncounterDefinition PreviewUnchanged(ShanheCampaignSession session, string node)
    {
        var before = session.Snapshot; var revision = session.Revision;
        var first = session.PreviewEncounter(node); var second = session.PreviewEncounter(node);
        Need(first.Id == second.Id && SameRoster(first.Enemies, second.Enemies) && ReferenceEquals(before, session.Snapshot) &&
            session.Revision == revision && session.PendingBattleSetup is null,
            "Repeated preview selects the same actual roster without entering battle, changing state or consuming the next encounter sequence.");
        return first;
    }
    private static void Move(ShanheCampaignSession session, string node) => Accept(session, new MoveShanheNodeCommand(node, session.Revision));
    private static ShanheRewardCategory RewardCategory(ShanheRewardDefinition reward) => (ShanheRewardCategory)Category.Invoke(null, [reward])!;
    private static ShanheRewardDefinition[] PrimaryOffers(ShanheCampaignSession session, ShanheNodeDefinition profile) =>
        (profile.RewardIds ?? []).Select(id => session.Definition.Rewards.Single(reward => reward.Id == id))
            .Where(reward => (bool)PrimaryRarity.Invoke(null, [profile, reward])!).ToArray();
    private sealed record Bonus(string Id, IReadOnlyList<string> OfferIds, bool IsBonus);
    private static List<Bonus> PlanBonuses(ShanheCampaignSession session, ShanheNodeDefinition profile)
    {
        var state = StateField.GetValue(session)!; var before = session.Snapshot; var revision = session.Revision;
        var groups = ((IEnumerable)BonusPlanner.Invoke(session, [state, profile])!).Cast<object>().Select(group => new Bonus(
            (string)group.GetType().GetProperty("Id")!.GetValue(group)!,
            (IReadOnlyList<string>)group.GetType().GetProperty("OfferIds")!.GetValue(group)!,
            (bool)group.GetType().GetProperty("IsBonus")!.GetValue(group)!)).ToList();
        Need(ReferenceEquals(state, StateField.GetValue(session)) && ReferenceEquals(before, session.Snapshot) && session.Revision == revision,
            "The existing pure bonus planner cannot award loot, advance a battle or mutate its owning journey.");
        return groups;
    }
    private static GameEngine StartNative(ShanheBattleSetup setup, Func<ShanheBattleSetup, ContentRegistry> registry)
    {
        var game = GameEngine.CreateStandard(new()
        {
            Seed = setup.BattleSeed, PlayerCount = 1 + setup.EnemyRoster.Count + setup.AllyRoster.Count, HumanSeat = 0,
            HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8
        }, registry(setup));
        Accept(game, new StartGameCommand()); ReachPlay(game); return game;
    }
    private static void ReachPlay(GameEngine game)
    {
        for (var step = 0; step < 160; step++)
        {
            if (!game.HasPendingShanheMarketPurchase && game.CreateSnapshot(0).PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }) return;
            Need(game.CreateSnapshot(0).PendingDecision is null, "Native opening or a pure economy purchase must not require an invented response.");
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The real wolf-battle opening did not reach native Play.");
    }
    private static (string General, int Hp, int MaxHp, int Armor, int Hand)[] NativeBodies(GameEngine game) =>
        game.CreateSnapshot(0).Players.Select(player => (player.GeneralId, player.Hp, player.MaxHp, player.Armor, player.HandCount)).ToArray();
    private static bool SameRoster(IReadOnlyList<ShanheCombatantDefinition> first, IReadOnlyList<ShanheCombatantDefinition> second) =>
        first.Select(Stats).SequenceEqual(second.Select(Stats));
    private static bool SameMapNode(ShanheNodeDefinition first, ShanheNodeDefinition second) =>
        first.NextNodeIds.SequenceEqual(second.NextNodeIds) &&
        first with { NextNodeIds = second.NextNodeIds } == second;
    private static (string Id, int? Hp, int MaxHp, int Hand, int Draw, int Slash, int Armor, int Recovery, int Damage) Stats(ShanheCombatantDefinition member) =>
        (member.GeneralId, member.Hp, member.MaxHpBonus, member.InitialHandBonus, member.DrawBonus, member.SlashBonus, member.InitialArmor, member.ArmorRecoveryPerRound, member.ExtraDamage);
    private static void Accept(ShanheCampaignSession session, ShanheCampaignCommand command)
    { var result = session.Submit(command); Need(result.Accepted, result.Error ?? "Rejected native wolf-battle campaign command."); }
    private static void Accept(GameEngine game, GameCommand command)
    { var result = game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native wolf-battle game command."); }
    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
}
