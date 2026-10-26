using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheChapterRewardChecks
{
    public static void FirstTwoBossesGrantNativeTacticsAndSkillsThenFinalVictorySettlesDirectly()
    {
        var definition = FixtureDefinition();
        var session = Create(definition);
        for (var chapter = 1; chapter <= 2; chapter++)
        {
            var node = "boss-" + chapter;
            Accept(session, new MoveShanheNodeCommand(node, session.Revision));
            Require(session.PendingBattleSetup!.ExtraSkillIds.SequenceEqual(session.Snapshot.ExtraSkillIds),
                "The selected native skills enter the next actual battle's setup.");
            var battle = ShanheCampaignChecks.CreateBattle(session.PendingBattleSetup!);
            ShanheCampaignChecks.FinishBattle(battle, "defeat");
            var gold = session.Snapshot.Gold;
            var slots = session.Snapshot.ExtraSkillSlots;
            Require(session.CompleteBattle(battle, session.Revision).Accepted, "A real native fixture victory must enter the chapter bundle.");
            var summary = session.Snapshot.BossRewardSummary ?? throw new InvalidOperationException("A chapter victory must publish its summary.");
            Require(session.Snapshot.Phase == ShanheCampaignPhase.Reward &&
                summary.ChapterNumber == chapter && summary.FixedGold == 500 && summary.SkillSlots == 1 &&
                session.Snapshot.Gold == gold + 500 && session.Snapshot.ExtraSkillSlots == slots + 1 &&
                summary.Groups.Count == 2 && summary.Groups.Single(group => group.Id == "tactics").Offers.Count == 3 &&
                summary.Groups.Single(group => group.Id == "skills").Offers.Count == 3 - chapter && summary.Groups.All(group => !group.IsResolved) &&
                !summary.CanConfirm && session.Snapshot.AvailableNodes.Count == 0 &&
                session.Snapshot.RewardOffers.Count == 0 && !session.Snapshot.CanSkipReward,
                "The first two bosses freeze tactics and earned native skills, even on a zero-research account.");
            RejectMutation(summary.Groups);
            foreach (var group in summary.Groups) RejectMutation(group.Offers);
            Cold(session, definition);
            var before = Snapshot(session);
            Require(!session.CompleteBattle(battle, session.Revision).Accepted &&
                !session.Submit(new SkipShanheRewardCommand(session.Revision)).Accepted &&
                !session.Submit(new ChooseShanheRewardCommand(summary.Groups[0].Offers[0].Id, null, session.Revision)).Accepted &&
                !session.Submit(new ConfirmShanheBossRewardCommand(session.Revision)).Accepted &&
                !session.Submit(new MoveShanheNodeCommand("boss-" + (chapter + 1), session.Revision)).Accepted && Snapshot(session) == before,
                "Duplicate victory, ordinary reward commands, skip, early confirmation and direct movement cannot bypass the boss bundle.");

            var skills = summary.Groups.Single(group => group.Id == "skills");
            var tactics = summary.Groups.Single(group => group.Id == "tactics");
            var oldRevision = session.Revision;
            var skill = skills.Offers[0];
            Accept(session, new ChooseShanheBossRewardCommand(skills.Id, skill.Id, null, session.Revision));
            var afterSkill = session.Snapshot.BossRewardSummary!;
            Require(session.Snapshot.ExtraSkillIds.Count == chapter && session.Snapshot.ExtraSkillIds.Contains(skill.SkillId!) &&
                session.Snapshot.StartingCards.Count == 0 && session.Snapshot.Gold == gold + 500 &&
                afterSkill.Groups.Single(group => group.Id == skills.Id).SelectedReward?.Id == skill.Id &&
                afterSkill.Groups.Single(group => group.Id == tactics.Id).Offers.Select(reward => reward.Id).SequenceEqual(tactics.Offers.Select(reward => reward.Id)) &&
                session.Snapshot.CompletedNodeIds.Count == chapter - 1 && !afterSkill.CanConfirm,
                "The skill group grants its actual native skill instead of a physical card, without rerolling the other group or finishing early.");
            if (chapter == 1) Cold(session, definition);
            before = Snapshot(session);
            Require(!session.Submit(new ChooseShanheBossRewardCommand(skills.Id, skill.Id, null, session.Revision)).Accepted &&
                !session.Submit(new ChooseShanheBossRewardCommand(tactics.Id, skill.Id, null, session.Revision)).Accepted &&
                !session.Submit(new ChooseShanheBossRewardCommand(tactics.Id, tactics.Offers[0].Id, null, oldRevision)).Accepted &&
                !session.Submit(new ConfirmShanheBossRewardCommand(session.Revision)).Accepted && Snapshot(session) == before,
                "Claimed groups, a reward from another group, stale revisions and unfinished confirmation all reject without mutation.");
            Accept(session, new ChooseShanheBossRewardCommand(tactics.Id, tactics.Offers[0].Id, null, session.Revision));
            Require(session.Snapshot.BossRewardSummary is { CanConfirm: true } &&
                session.Snapshot.Phase == ShanheCampaignPhase.Reward && session.Snapshot.AvailableNodes.Count == 0 &&
                session.Snapshot.Bonuses.Draw + session.Snapshot.Bonuses.Slash + session.Snapshot.Bonuses.InitialHand == chapter &&
                session.Snapshot.ExtraSkillIds.Count == chapter && session.Snapshot.Gold == gold + 500,
                "Claiming both groups still waits on the summary confirmation; a tactic does not consume the new skill slot or repeat fixed credits.");
            Accept(session, new ConfirmShanheBossRewardCommand(session.Revision));
            Require(session.Snapshot.BossRewardSummary is null && session.Snapshot.CompletedNodeIds.Count == chapter &&
                session.Snapshot.Phase == ShanheCampaignPhase.Map && session.Snapshot.AvailableNodes.Single().Id == "boss-" + (chapter + 1),
                "Confirming each of the first two summaries opens the next chapter.");
            before = Snapshot(session);
            Require(!session.Submit(new ConfirmShanheBossRewardCommand(session.Revision)).Accepted && Snapshot(session) == before,
                "Chapter confirmation cannot be repeated after it has advanced the route.");
        }
        Accept(session, new MoveShanheNodeCommand("boss-3", session.Revision));
        var finalBattle = ShanheCampaignChecks.CreateBattle(session.PendingBattleSetup!);
        ShanheCampaignChecks.FinishBattle(finalBattle, "defeat");
        Require(session.CompleteBattle(finalBattle, session.Revision).Accepted && session.Snapshot.Phase == ShanheCampaignPhase.Won &&
            session.Snapshot.EndingText == "第三章已定" && session.Snapshot.BossRewardSummary is null &&
            session.Snapshot.RewardOffers.Count == 0 && session.Snapshot.AvailableNodes.Count == 0 && !session.Snapshot.CanSkipReward &&
            session.Snapshot.Gold == 1510 && session.Snapshot.ExtraSkillSlots == 3 && session.Snapshot.ExtraSkillIds.Count == 2 &&
            session.AcceptedCommands.OfType<ChooseShanheBossRewardCommand>().Count() == 4 &&
            session.AcceptedCommands.OfType<ConfirmShanheBossRewardCommand>().Count() == 2,
            "The actual third boss reaches final settlement immediately, retaining its copper but issuing no unusable build reward or slot.");
        var finished = Snapshot(session);
        Require(!session.Submit(new ConfirmShanheBossRewardCommand(session.Revision)).Accepted &&
            !session.Submit(new ChooseShanheRewardCommand("opening", null, session.Revision)).Accepted &&
            !session.CompleteBattle(finalBattle, session.Revision).Accepted && Snapshot(session) == finished,
            "A finished expedition cannot reopen build rewards or settle its final battle twice.");
        Cold(session, definition);
    }

    public static void CurrentChapterPoolsAreEpicAndNestedDefinitionsRemainBounded()
    {
        var source = FixtureDefinition();
        var candidateIds = source.Nodes[0].BossRewards!.Groups[0].RewardIds.ToList();
        var groups = source.Nodes[0].BossRewards!.Groups.ToList();
        groups[0] = groups[0] with { RewardIds = candidateIds };
        source = source with { Nodes = source.Nodes.Select((node, index) => index == 0 ? node with { BossRewards = node.BossRewards! with { Groups = groups } } : node).ToArray() };
        var session = Create(source);
        var hash = session.ContentHash;
        candidateIds.Clear(); groups.Clear();
        Require(session.Definition.Nodes[0].BossRewards!.Groups[0].RewardIds.Count == 3 && session.ContentHash == hash,
            "Published chapter definitions detach mutable group and candidate lists.");
        RejectMutation(session.Definition.Nodes[0].BossRewards!.Groups);
        RejectMutation(session.Definition.Nodes[0].BossRewards!.Groups[0].RewardIds);
        var frozen = session.Definition;
        MustThrow(() => ShanheCampaignContent.FreezeAndValidate(frozen with
        { Nodes = frozen.Nodes.Select((node, index) => index == 0 ? node with { ChapterNumber = 0 } : node).ToArray() }));
        MustThrow(() => ShanheCampaignContent.FreezeAndValidate(frozen with
        { Nodes = frozen.Nodes.Select((node, index) => index == 0 ? node with { BossRewards = node.BossRewards! with { FixedGold = -1 } } : node).ToArray() }));
        MustThrow(() => ShanheCampaignContent.FreezeAndValidate(frozen with
        { Nodes = frozen.Nodes.Select((node, index) => index == 0 ? node with { BossRewards = node.BossRewards! with { Groups = [new("bad", "缺失奖励", ["missing"])] } } : node).ToArray() }));

        foreach (var tier in new[] { 1, 2, 3 })
        foreach (var codex in new[] { false, true })
        {
            var current = codex ? StandardShanheCampaignCatalog.CreateCodexDefinition(tier, StandardShanheCampaignCatalog.Returning2026EditionId)
                : StandardShanheCampaignCatalog.CreateDefinition(tier, StandardShanheCampaignCatalog.Returning2026EditionId);
            var bosses = current.Nodes.Where(node => node.BossRewards is not null).ToArray();
            Require(current.Nodes.All(node => node.ChapterNumber is >= 1 and <= 3 && !string.IsNullOrWhiteSpace(node.ChapterTitle)) &&
                bosses.Select(node => node.Id).Order(StringComparer.Ordinal).SequenceEqual(new[] { "huaxiong", "lijue", "guosi" }.Order(StringComparer.Ordinal)) &&
                bosses.All(node => node.BossRewards is { FixedGold: 500, SkillSlots: 1 } && node.BossRewards.Groups.Count == 2 &&
                    node.BossRewards.Groups.Select(group => group.Id).SequenceEqual(["tactics", "cards"]) &&
                    node.BossRewards.Groups.All(group => group.ChoiceCount == 3 && group.RewardIds.Count >= 3)),
                "Every current difficulty and codex pool publishes epic tactic and physical hand-card choices for its first two chapter bosses.");
            foreach (var boss in bosses)
            foreach (var group in boss.BossRewards!.Groups)
            foreach (var id in group.RewardIds)
            {
                var reward = current.Rewards.Single(reward => reward.Id == id);
                Require(reward.Rarity == 3 && (group.Id == "cards"
                        ? reward.StartingCards is { Count: 1 } && !EquipmentCatalog.IsEquipment(reward.StartingCards[0].Kind) &&
                            reward.SkillId is null && reward.TacticSkillId is null
                        : StandardShanheCampaignCatalog.ShopMetadataByRewardId[id] is { Rarity: 3, Kind: "战法" } &&
                            (reward.TacticSkillId is not null || reward.CampaignEconomy is not null)) &&
                    (reward.StartingEquipment?.Count ?? 0) == 0,
                    "Chapter offers contain actual epic tactics and epic physical hand cards, preserving their source quality.");
            }
            Require(current.Nodes.Where(node => node.BossRewards is null && node.ChapterNumber < 3 && node.RewardIds is { Count: > 0 } &&
                    (!node.IsStoryBattle || node.Kind == ShanheNodeKind.Battle))
                    .SelectMany(node => node.RewardIds!).All(id => current.Rewards.Single(reward => reward.Id == id).Rarity is 1 or 2) &&
                current.Rewards.Any(reward => reward.Rarity == 4) &&
                current.Nodes.Single(node => node.Id == "hulao").Kind == ShanheNodeKind.EliteBattle &&
                current.Nodes.Where(node => !node.IsStoryBattle && node.Kind is ShanheNodeKind.Battle or ShanheNodeKind.EliteBattle)
                    .All(node => node.PrimaryTacticRarity == 1 && node.PrimarySkillRarity is 1 or 2) &&
                current.Nodes.Where(node => node.Kind == ShanheNodeKind.BossBattle && node.ChapterNumber < 3)
                    .All(node => node.BossRewards is not null) &&
                current.Nodes.Where(node => node.Encounters is { Count: > 0 }).All(node =>
                    node.Encounters!.All(encounter => encounter.VictoryGold == (node.Kind == ShanheNodeKind.BossBattle ? 500 :
                        node.IsStoryBattle && node.Kind == ShanheNodeKind.EliteBattle ? 300 : 200))),
                "Ordinary battle pools stay common or rare and historical legendary sources remain available; route battle credits are 200/500 and an optional elite story battle retains its authored 300.");
            var finals = current.Nodes.Where(node => node.NextNodeIds.Count == 0).ToArray();
            Require(finals.Length == 3 && finals.All(node => node.ChapterNumber == 3 && node.BossRewards is null &&
                node.AddSkillSlotsOnVictory == 0 && node.Encounters!.All(encounter => encounter.VictoryGold == 500)),
                "All three final routes retain their fixed copper credit without another build reward or slot.");
            _ = ShanheCampaignSession.Create(current, new(71, "classic:guan-yu") { UseCodexRewardPool = codex, UnlockedRewardIds = [] },
                StandardShanheCampaignCatalog.CreateBattleRegistry);
        }
        var choices = Enumerable.Range(0, 65).Select(index => new ShanheNodeChoiceDefinition("stock-" + index, "军资", RewardId: "draw", LeavesNode: false)).ToArray();
        var shop = new ShanheNodeDefinition("large-shop", "行商目录", ShanheNodeKind.Shop, ["boss-1"], Choices: choices);
        var shopDefinition = frozen with { StartingNodeIds = [shop.Id], Nodes = new[] { shop }.Concat(frozen.Nodes).ToArray() };
        Require(ShanheCampaignContent.FreezeAndValidate(shopDefinition).Nodes[0].Choices!.Count == 65,
            "A real catalogue shop may exceed the former sixty-four product limit while remaining bounded.");
        MustThrow(() => ShanheCampaignContent.FreezeAndValidate(shopDefinition with
        { Nodes = shopDefinition.Nodes.Select(node => node.Id == shop.Id ? node with { Kind = ShanheNodeKind.Event } : node).ToArray() }));
        MustThrow(() => ShanheCampaignContent.FreezeAndValidate(shopDefinition with
        { Nodes = shopDefinition.Nodes.Select(node => node.Id == shop.Id ? node with { Choices = Enumerable.Range(0, 513).Select(index => choices[0] with { Id = "stock-" + index }).ToArray() } : node).ToArray() }));
        var lockedShop = shop with { Choices = [new("locked-skill", "待研习技能", RewardId: "skill-guard", LeavesNode: false)] };
        var locked = Create(shopDefinition with { Nodes = new[] { lockedShop }.Concat(frozen.Nodes).ToArray() });
        Accept(locked, new MoveShanheNodeCommand(lockedShop.Id, locked.Revision));
        var beforeShop = Snapshot(locked);
        Require(!locked.Snapshot.NodeChoices.Any(choice => choice.Id == "locked-skill") &&
            !locked.Submit(new ChooseShanheNodeOptionCommand("locked-skill", null, locked.Revision)).Accepted && Snapshot(locked) == beforeShop,
            "The earned boss skill exception cannot bypass ordinary shop research requirements.");
    }
    private static ShanheCampaignDefinition FixtureDefinition()
    {
        string[] tactics = ["opening", "draw", "slash"];
        string[] skills = ["skill-guard", "skill-raid", "skill-guard-alias"];
        return new("fixture:chapter-rewards", "三章征程", [ShanheCampaignChecks.Player], ["boss-1"],
            Enumerable.Range(1, 3).Select(chapter => new ShanheNodeDefinition("boss-" + chapter, "第" + chapter + "章首领",
                ShanheNodeKind.BossBattle, chapter < 3 ? ["boss-" + (chapter + 1)] : [],
                [new("guard-" + chapter, "守军", [new(ShanheCampaignChecks.Enemy)], VictoryGold: chapter == 3 ? 500 : 7)], tactics)
            {
                ChapterNumber = chapter, ChapterTitle = "第" + chapter + "章", RewardSkipGold = 100,
                AddSkillSlotsOnVictory = chapter < 3 ? 1 : 0, VictoryText = chapter == 3 ? "第三章已定" : "",
                BossRewards = chapter < 3 ? new(500, 1, [new("tactics", "战法三选一", tactics), new("skills", "技能三选一", skills)]) : null
            }).ToArray(),
            [new("opening", "筹备", Bonuses: new(InitialHand: 1)), new("draw", "粮道", Bonuses: new(Draw: 1)),
                new("slash", "连击", Bonuses: new(Slash: 1)), new("skill-guard", "守御", SkillId: "fixture:shanhe-guard"),
                new("skill-raid", "突袭", SkillId: "fixture:shanhe-raid"), new("skill-guard-alias", "守御同源", SkillId: "fixture:shanhe-guard")],
            StartingGold: 10, ExtraSkillSlots: 1);
    }
    private static ShanheCampaignSession Create(ShanheCampaignDefinition definition) => ShanheCampaignSession.Create(definition,
        new(71, ShanheCampaignChecks.Player) { UnlockedRewardIds = [] }, ShanheCampaignChecks.Registry);
    private static string Snapshot(ShanheCampaignSession session) => JsonSerializer.Serialize(session.Snapshot);
    private static void Cold(ShanheCampaignSession session, ShanheCampaignDefinition definition)
    {
        var saved = ShanheCampaignJson.Deserialize(ShanheCampaignJson.Serialize(session.CreateCheckpoint()));
        var restored = ShanheCampaignSession.Restore(saved, definition, ShanheCampaignChecks.Registry);
        Require(Snapshot(restored) == Snapshot(session), "Cold replay must retain fixed credits, exact group offers, partial claims and chapter phase.");
    }
    private static void Accept(ShanheCampaignSession session, ShanheCampaignCommand command)
    {
        var result = session.Submit(command); Require(result.Accepted, result.Error ?? "Chapter fixture command rejected.");
    }
    private static void RejectMutation<T>(IReadOnlyList<T> values)
    {
        Require(values is IList<T> { IsReadOnly: true }, "Published chapter collections must be read-only.");
        try { ((IList<T>)values)[0] = values[0]; } catch (NotSupportedException) { return; }
        throw new InvalidOperationException("A published chapter collection accepted mutation.");
    }
    private static void MustThrow(Action action)
    {
        try { action(); } catch (Exception error) when (error is ArgumentException or InvalidOperationException) { return; }
        throw new InvalidOperationException("Invalid chapter content or checkpoint was accepted.");
    }
    private static void Require(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
}
