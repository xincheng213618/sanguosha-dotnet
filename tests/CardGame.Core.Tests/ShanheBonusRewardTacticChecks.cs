using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheBonusRewardTacticChecks
{
    private const string AceTactic = "tactic-economy-2013";
    private const string AceSkill = "tactic-economy-2014";
    private const string MoreSkills = "tactic-economy-2016";
    private const string MoreTactics = "tactic-economy-2017";
    private const string MoreCards = "tactic-economy-2018";
    private const string Guard = "fixture:shanhe-guard";
    private const string Raid = "fixture:shanhe-raid";
    private static readonly Lazy<IReadOnlyList<ShanheRewardDefinition>> ActualTactics = new(() =>
    {
        var catalog = StandardShanheCampaignCatalog.CreateDefinition(1, StandardShanheCampaignCatalog.Returning2026EditionId);
        return new[] { AceTactic, AceSkill, MoreTactics, MoreCards }
            .Select(id => catalog.Rewards.Single(reward => reward.Id == id)).ToArray();
    });

    public static void EliteVictoryFreezesOwnedRareBonusAfterPrimaryRewardOnlyOnce()
    {
        var definition = EliteDefinition();
        var session = Create(definition, AceTactic, AceSkill, MoreTactics, MoreCards);
        Complete(session, "elite");
        Require(session.Snapshot.BossRewardSummary is null && session.Snapshot.RewardCategories.SequenceEqual([ShanheRewardCategory.Supply]) &&
            session.Snapshot.Gold == 107 && session.Snapshot.ExtraSkillSlots == 1,
            "The actual elite victory first publishes its normal category without paying an extra slot or another victory credit.");
        ClaimPrimary(session, "base");
        var summary = session.Snapshot.BossRewardSummary ?? throw new InvalidOperationException("The owned elite tactic must publish its frozen bonus.");
        var group = summary.Groups.Single();
        Require(summary.IsBonusOnly && summary.FixedGold == 0 && summary.SkillSlots == 0 && summary.Title.Contains("追加奖励", StringComparison.Ordinal) &&
            group.Offers.Count == 4 && group.Offers.All(reward => reward.Rarity == 2 && reward.Id.StartsWith("rare-", StringComparison.Ordinal)) &&
            session.Snapshot.RewardCategories.Count == 0 && session.Snapshot.RewardOffers.Count == 0 && !session.Snapshot.CanSkipReward &&
            session.Snapshot.Gold == 118 && session.Snapshot.CompletedNodeIds.Count == 0,
            "The extra group contains four true rare tactics under 2017, excluding held, opening-only and other-rarity content.");
        var frozenIds = group.Offers.Select(reward => reward.Id).ToArray();
        for (var read = 0; read < 3; read++)
            Require(session.Snapshot.BossRewardSummary!.Groups.Single().Offers.Select(reward => reward.Id).SequenceEqual(frozenIds),
                "Reading the already issued bonus never rerolls its pool.");
        Accept(session, new ChooseShanheBossRewardCommand(group.Id, group.Offers[0].Id, null, session.Revision));
        Require(session.Snapshot.Gold == 118 && session.Snapshot.ExtraSkillSlots == 1 && session.Snapshot.BossRewardSummary is { CanConfirm: true },
            "The bonus grants one tactic while preserving the primary reward's exact gold and the real skill capacity.");
        Accept(session, new ConfirmShanheBossRewardCommand(session.Revision));
        Require(session.Snapshot.CompletedNodeIds.SequenceEqual(["elite"]) && session.Snapshot.Gold == 118 &&
            !session.Submit(new ConfirmShanheBossRewardCommand(session.Revision)).Accepted,
            "Confirming completes the elite once and cannot repeat either reward or victory income.");
        Complete(session, "ordinary");
        ClaimPrimary(session, "base");
        Require(session.Snapshot.BossRewardSummary is null && session.Snapshot.Phase == ShanheCampaignPhase.Map,
            "Holding both ace tactics cannot create an extra reward after an ordinary battle.");

        var absent = Create(definition);
        Complete(absent, "elite"); ClaimPrimary(absent, "base");
        Require(absent.Snapshot.BossRewardSummary is null && absent.Snapshot.Phase == ShanheCampaignPhase.Map,
            "An elite victory without the owned source tactic retains its ordinary single reward.");
        var defeated = Create(definition, AceTactic, AceSkill);
        Complete(defeated, "elite", win: false);
        Require(defeated.Snapshot.Phase == ShanheCampaignPhase.Lost && defeated.Snapshot.BossRewardSummary is null &&
            defeated.Snapshot.RewardOffers.Count == 0 && defeated.Snapshot.Gold == 100,
            "A real defeat never pays victory income or either ace bonus.");

        var lateDefinition = definition with
        {
            Nodes = definition.Nodes.Select(node => node.Id == "elite" ? node with { RewardIds = [AceTactic] } :
                node.Id == "ordinary" ? node with { Kind = ShanheNodeKind.EliteBattle } : node).ToArray()
        };
        var late = Create(lateDefinition);
        Complete(late, "elite"); ClaimPrimary(late, AceTactic);
        Require(late.Snapshot.BossRewardSummary is null && late.Snapshot.TacticIds.Contains(AceTactic),
            "Acquiring 2013 from this victory's primary offer cannot retroactively issue its extra reward.");
        Complete(late, "ordinary"); ClaimPrimary(late, "base");
        Require(late.Snapshot.BossRewardSummary is { IsBonusOnly: true } later && later.Groups.Single().Offers.Count == 3,
            "The newly owned tactic starts earning its normal three-choice extra at the next actual elite victory.");

        var skipped = Create(definition, AceTactic);
        Complete(skipped, "elite");
        Accept(skipped, new ChooseShanheRewardCategoryCommand(ShanheRewardCategory.Supply, skipped.Revision));
        Accept(skipped, new SkipShanheRewardCommand(skipped.Revision));
        Require(skipped.Snapshot.Gold == 120 && skipped.Snapshot.BossRewardSummary is { IsBonusOnly: true } &&
            !skipped.Submit(new SkipShanheRewardCommand(skipped.Revision)).Accepted,
            "Declining the primary reward pays its thirteen copper once before the still-required frozen bonus group.");
    }

    public static void BossEpicBonusUsesEarnedSkillReplacementAndExhaustionBeforeFinalSettlement()
    {
        foreach (var baseSkill in new[] { "legendary-guard", "legendary-raid" })
        {
            var definition = BossDefinition(baseSkill);
            var session = Create(definition, AceSkill, MoreCards);
            Complete(session, "boss");
            var summary = session.Snapshot.BossRewardSummary ?? throw new InvalidOperationException("A chapter boss must retain its normal bundle.");
            var epic = summary.Groups.Single(group => group.Id.StartsWith("$bonus:", StringComparison.Ordinal));
            Require(!summary.IsBonusOnly && summary.Groups.Count == 3 && summary.FixedGold == 500 && summary.SkillSlots == 1 &&
                session.Snapshot.Gold == 600 && session.Snapshot.ExtraSkillSlots == 1 && epic.Offers.Count == 1 &&
                epic.Offers[0].Rarity == 3 && epic.Offers[0].SkillId == Raid,
                "The owned boss tactic appends one real epic skill group to the two ordinary groups, deduplicates aliases and grants no extra slot.");
            Accept(session, new ChooseShanheBossRewardCommand("skills", baseSkill, null, session.Revision));
            var afterBase = session.Snapshot.BossRewardSummary!;
            var bonus = afterBase.Groups.Single(group => group.Id == epic.Id);
            if (baseSkill == "legendary-guard")
            {
                var before = session.Snapshot;
                Require(!session.Submit(new ChooseShanheBossRewardCommand(epic.Id, epic.Offers[0].Id, null, session.Revision)).Accepted &&
                    ReferenceEquals(before, session.Snapshot), "An earned locked epic skill still requires an explicit legal replacement when the real slot is full.");
                Accept(session, new ChooseShanheBossRewardCommand(epic.Id, epic.Offers[0].Id, Guard, session.Revision));
                Require(session.Snapshot.ExtraSkillIds.SequenceEqual([Raid]) && session.Snapshot.ExtraSkillSlots == 1,
                    "The earned epic skill bypasses only account research and replaces the chosen actual skill within the one real slot.");
            }
            else
            {
                Require(bonus.IsExhausted && bonus.IsResolved && bonus.Offers.Count == 0 && epic.Offers.Count == 1,
                    "Claiming the same native skill from the base group removes the sole frozen epic candidate, keeps the old snapshot frozen and never redraws a substitute.");
            }
            Accept(session, new ChooseShanheBossRewardCommand("tactics", "legendary-tactic", null, session.Revision));
            Require(session.Snapshot.BossRewardSummary is { CanConfirm: true } && session.Snapshot.Gold == 600 &&
                session.Snapshot.CompletedNodeIds.Count == 0, "All available groups resolve before confirmation, without repeating fixed copper or skill capacity.");
            Accept(session, new ConfirmShanheBossRewardCommand(session.Revision));
            Require(session.Snapshot.Gold == 600 && session.Snapshot.ExtraSkillSlots == 1 && session.Snapshot.CompletedNodeIds.SequenceEqual(["boss"]),
                "Confirming the mixed bundle advances the node and never pays its fixed reward a second time.");
        }

        var final = BossDefinition("legendary-guard");
        final = final with
        {
            Nodes = [final.Nodes[0] with { NextNodeIds = [], ChapterNumber = 3, BossRewards = null,
                RewardIds = ["base"], AddSkillSlotsOnVictory = 0 }]
        };
        var ended = Create(final, AceSkill, MoreCards);
        Complete(ended, "boss");
        Require(ended.Snapshot.Phase == ShanheCampaignPhase.Won && ended.Snapshot.BossRewardSummary is null &&
            ended.Snapshot.RewardOffers.Count == 0 && ended.Snapshot.RewardCategories.Count == 0 &&
            ended.Snapshot.Gold == 107 && ended.Snapshot.ExtraSkillSlots == 0,
            "The real third-chapter final victory settles immediately with encounter copper and no ordinary or additional growth group or skill slot.");
        SkillChoiceTacticExpandsBothBossSkillGroups();
    }

    private static void SkillChoiceTacticExpandsBothBossSkillGroups()
    {
        var source = StandardShanheCampaignCatalog.CurrentEconomyTactics.Single(tactic => tactic.SourceId == 2016);
        var definition = BossDefinition("legendary-guard");
        string[] nativeSkills = ["standard:yingzi", "standard:wusheng", "standard:paoxiao"];
        var extraRewards = nativeSkills.SelectMany((skill, index) => new[]
        {
            new ShanheRewardDefinition("legendary-native-" + index, "传说技能" + index, SkillId: skill) { Rarity = 4 },
            new ShanheRewardDefinition("epic-native-" + index, "史诗技能" + index, SkillId: skill) { Rarity = 3 }
        });
        definition = definition with
        {
            Rewards = definition.Rewards.Concat(extraRewards).Append(new(source.RewardId, source.Name)
                { CampaignEconomy = source.Effect, Rarity = source.Rarity }).ToArray(),
            Nodes = definition.Nodes.Select(node => node.BossRewards is null ? node : node with
            {
                BossRewards = node.BossRewards with
                {
                    Groups = node.BossRewards.Groups.Select(group => group.Id != "skills" ? group : group with
                    { RewardIds = ["legendary-guard", "legendary-native-0", "legendary-native-1", "legendary-native-2"], ChoiceCount = 3 }).ToArray()
                }
            }).ToArray()
        };
        foreach (var hasSkillChoice in new[] { false, true })
        {
            var equipped = hasSkillChoice ? new[] { AceSkill, MoreCards, MoreSkills } : new[] { AceSkill, MoreCards };
            var session = Create(definition, equipped);
            Complete(session, "boss");
            var summary = session.Snapshot.BossRewardSummary!;
            var skills = summary.Groups.Single(group => group.Id == "skills");
            var epic = summary.Groups.Single(group => group.Id.StartsWith("$bonus:", StringComparison.Ordinal));
            var count = hasSkillChoice ? 4 : 3;
            Require(skills.Offers.Count == count && skills.Offers.All(reward => reward.Rarity == 4 && reward.SkillId is not null) &&
                epic.Offers.Count == count && epic.Offers.All(reward => reward.Rarity == 3 && reward.SkillId is not null) &&
                skills.Offers.Select(reward => reward.SkillId).Distinct().Count() == count &&
                epic.Offers.Select(reward => reward.SkillId).Distinct().Count() == count &&
                summary.Groups.Single(group => group.Id == "tactics").Offers.Count == 1 &&
                session.Snapshot.Gold == 600 && session.Snapshot.ExtraSkillSlots == 1,
                "An actual chapter victory offers three base and epic skills by default, or four in both frozen groups only with 2016; card choices cannot expand skills or repeat credits.");
            Accept(session, new ChooseShanheBossRewardCommand("tactics", "legendary-tactic", null, session.Revision));
            var afterTactic = session.Snapshot.BossRewardSummary!;
            Require(afterTactic.Groups.Single(group => group.Id == skills.Id).Offers.Select(reward => reward.Id)
                    .SequenceEqual(skills.Offers.Select(reward => reward.Id)) &&
                afterTactic.Groups.Single(group => group.Id == epic.Id).Offers.Select(reward => reward.Id)
                    .SequenceEqual(epic.Offers.Select(reward => reward.Id)),
                "Claiming the separate tactic group preserves both already frozen skill candidate lists without redrawing.");
        }
    }

    private static ShanheCampaignDefinition EliteDefinition() => new("fixture:ace-elite-rewards", "精英追加奖励",
        [ShanheCampaignChecks.Player], ["elite"],
        [BattleNode("elite", ShanheNodeKind.EliteBattle, ["ordinary"]), BattleNode("ordinary", ShanheNodeKind.Battle, ["finish"]),
            new("finish", "归程", ShanheNodeKind.Camp, [], Choices: [new("leave", "归程")])],
        Rewards(), StartingGold: 100, ExtraSkillSlots: 1);

    private static ShanheCampaignDefinition BossDefinition(string baseSkill) => new("fixture:ace-boss-rewards", "首领追加奖励",
        [ShanheCampaignChecks.Player], ["boss"],
        [BattleNode("boss", ShanheNodeKind.BossBattle, ["finish"]) with
            { ChapterNumber = 1, ChapterTitle = "首章", BossRewards = new(500, 1,
                [new("tactics", "传说战法", ["legendary-tactic"], ChoiceCount: 1), new("skills", "传说技能", [baseSkill], ChoiceCount: 1)]) },
            new("finish", "归程", ShanheNodeKind.Camp, [], Choices: [new("leave", "归程")])],
        Rewards(), StartingGold: 100, ExtraSkillSlots: 0);

    private static ShanheNodeDefinition BattleNode(string id, ShanheNodeKind kind, IReadOnlyList<string> next) =>
        new(id, id, kind, next, [new(id + "-guard", "守军", [new(ShanheCampaignChecks.Enemy)], VictoryGold: 7)], ["base"])
        { CategorizedRewards = true, RewardSkipGold = 13 };

    private static IReadOnlyList<ShanheRewardDefinition> Rewards() => ActualTactics.Value.Concat(new ShanheRewardDefinition[]
    {
        new("base", "基础军资", Gold: 11),
        new("rare-a", "稀有甲") { Rarity = 2, CampaignEconomy = new(ShopDiscountPercent: 1) },
        new("rare-b", "稀有乙") { Rarity = 2, CampaignEconomy = new(ShopDiscountPercent: 2) },
        new("rare-c", "稀有丙") { Rarity = 2, CampaignEconomy = new(ShopDiscountPercent: 3) },
        new("rare-d", "稀有丁") { Rarity = 2, CampaignEconomy = new(ShopDiscountPercent: 4) },
        new("other-rarity", "普通战法") { Rarity = 1, CampaignEconomy = new(ShopDiscountPercent: 5) },
        new("opening-only", "开局家财") { Rarity = 2, CampaignEconomy = new(StartingGold: 300) },
        new("legendary-tactic", "传说战法") { Rarity = 4, CampaignEconomy = new(ShopDiscountPercent: 6) },
        new("legendary-guard", "传说守御", SkillId: Guard) { Rarity = 4 },
        new("legendary-raid", "传说突袭", SkillId: Raid) { Rarity = 4 },
        new("epic-raid", "史诗突袭", SkillId: Raid) { Rarity = 3 },
        new("epic-raid-alias", "史诗突袭同源", SkillId: Raid) { Rarity = 3 },
        new("epic-printed", "本将技能", SkillId: "fixture:shanhe-force") { Rarity = 3 },
        new("rare-skill", "稀有守御", SkillId: Guard) { Rarity = 2 }
    }).ToArray();

    private static ShanheCampaignSession Create(ShanheCampaignDefinition definition, params string[] equipped) =>
        ShanheCampaignSession.Create(definition, new(71, ShanheCampaignChecks.Player)
            { EquippedTacticIds = equipped, UnlockedRewardIds = equipped }, ShanheCampaignChecks.Registry);

    private static void Complete(ShanheCampaignSession session, string nodeId, bool win = true)
    {
        Accept(session, new MoveShanheNodeCommand(nodeId, session.Revision));
        var battle = ShanheCampaignChecks.CreateBattle(session.PendingBattleSetup!);
        ShanheCampaignChecks.FinishBattle(battle, win ? "defeat" : "surrender");
        var result = session.CompleteBattle(battle, session.Revision);
        Require(result.Accepted, result.Error ?? "The completed real reward fixture battle was rejected.");
    }

    private static void ClaimPrimary(ShanheCampaignSession session, string id)
    {
        if (session.Snapshot.RewardCategories.Count > 0)
            Accept(session, new ChooseShanheRewardCategoryCommand(session.Snapshot.RewardCategories.Single(), session.Revision));
        Accept(session, new ChooseShanheRewardCommand(id, null, session.Revision));
    }
    private static void Accept(ShanheCampaignSession session, ShanheCampaignCommand command)
    { var result = session.Submit(command); Require(result.Accepted, result.Error ?? "Ace reward fixture command rejected."); }
    private static void Require(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
}
