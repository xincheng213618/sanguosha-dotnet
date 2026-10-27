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
    private static bool SameRoster(IReadOnlyList<ShanheCombatantDefinition> first, IReadOnlyList<ShanheCombatantDefinition> second) =>
        first.Select(Stats).SequenceEqual(second.Select(Stats));
    private static (string Id, int? Hp, int MaxHp, int Hand, int Draw, int Slash, int Armor, int Recovery, int Damage) Stats(ShanheCombatantDefinition member) =>
        (member.GeneralId, member.Hp, member.MaxHpBonus, member.InitialHandBonus, member.DrawBonus, member.SlashBonus, member.InitialArmor, member.ArmorRecoveryPerRound, member.ExtraDamage);
    private static void Accept(ShanheCampaignSession session, ShanheCampaignCommand command)
    { var result = session.Submit(command); Need(result.Accepted, result.Error ?? "Rejected native wolf-battle campaign command."); }
    private static void Accept(GameEngine game, GameCommand command)
    { var result = game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native wolf-battle game command."); }
    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
}
