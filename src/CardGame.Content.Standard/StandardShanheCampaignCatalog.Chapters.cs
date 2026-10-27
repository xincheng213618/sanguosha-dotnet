using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    private static readonly HashSet<string> ChapterBossIds = new(StringComparer.Ordinal)
        { "huaxiong", "lijue", "guosi", "luoyang", "suanzao", "henei" };
    private static readonly IReadOnlyList<ShanheRewardDefinition> ChapterLocalCards = Array.AsReadOnly(new[]
    {
        new ShanheRewardDefinition("chapter-card-slash", "随身杀", StartingCards: [new(CardKind.Slash, Suit.Spade, 7)])
            { Description = "每场战斗开局额外获得一张杀；本地开场牌编排。" },
        new ShanheRewardDefinition("chapter-card-dodge", "随身闪", StartingCards: [new(CardKind.Dodge, Suit.Diamond, 2)])
            { Description = "每场战斗开局额外获得一张闪；本地开场牌编排。" }
    });

    // Dedicated chapter rewards reuse the verified physical faces while keeping
    // the original four-tier shop catalogue and its existing ids unchanged.
    private static IEnumerable<ShanheRewardDefinition> ChapterEpicCards => CurrentShopCardSources
        .Where(source => source.Rarity == 3 && !EquipmentCatalog.IsEquipment(source.Kind))
        .Select(source => CodexBasicCard(source.SourceId) with
        {
            Id = "chapter-epic-card-" + source.SourceId.ToString(System.Globalization.CultureInfo.InvariantCulture),
            Rarity = 3
        });

    private static ShanheCampaignDefinition WithChapterRewards(ShanheCampaignDefinition definition)
    {
        var currentSeason = definition.Id.StartsWith("shanhe:zhuhou-2026-", StringComparison.Ordinal);
        var additions = currentSeason ? ChapterLegendarySkillRewards.Concat(ChapterEpicCards) : ChapterLocalCards;
        var rewards = definition.Rewards.Concat(additions.Where(addition => definition.Rewards.All(reward => reward.Id != addition.Id))).ToArray();
        var tactics = rewards.Where(IsChapterTactic).Where(reward => !currentSeason || IsEpicChapterReward(reward, "战法"))
            .Select(reward => reward.Id).ToArray();
        var skills = rewards.Where(reward => reward.SkillId is not null && IsEpicChapterReward(reward, "技能"))
            .Select(reward => reward.Id).ToArray();
        // Prefer the verified physical card faces when the codex catalogue is present; ordinary local editions
        // use their existing Peach/Duel recipes plus explicitly labelled local Slash/Dodge opening cards.
        var cards = currentSeason ? rewards.Where(reward => reward.Id.StartsWith("chapter-epic-card-", StringComparison.Ordinal) &&
                reward.Rarity == 3 && reward.StartingCards?.Count > 0).ToArray()
            : rewards.Where(reward => reward.Id.StartsWith("codex-card-", StringComparison.Ordinal) && reward.StartingCards?.Count > 0).ToArray();
        if (cards.Length < 3) cards = rewards.Where(reward => reward.StartingCards?.Count > 0).ToArray();
        var depths = new Dictionary<string, int>(StringComparer.Ordinal);
        var pending = new Queue<(string Id, int Depth)>();
        foreach (var id in definition.StartingNodeIds) pending.Enqueue((id, 0));
        while (pending.TryDequeue(out var item))
        {
            if (!depths.TryAdd(item.Id, item.Depth)) continue;
            foreach (var next in definition.Nodes.Single(node => node.Id == item.Id).NextNodeIds)
                pending.Enqueue((next, item.Depth + 1));
        }
        return ShanheCampaignContent.FreezeAndValidate(definition with
        {
            Rewards = rewards,
            Nodes = definition.Nodes.Select(node =>
            {
                var depth = depths[node.Id];
                var chapter = depth <= 4 ? 1 : depth <= 8 ? 2 : 3;
                var chapterBoss = ChapterBossIds.Contains(node.Id);
                return node with
                {
                    ChapterNumber = chapter,
                    ChapterTitle = ChapterRouteTitle(node.Id, chapter),
                    // Final victories keep their fixed copper credit, but no longer award
                    // unusable build choices or another skill slot after the expedition.
                    Encounters = chapterBoss && chapter == 3
                        ? node.Encounters!.Select(encounter => encounter with { VictoryGold = 500 }).ToArray()
                        : node.Encounters,
                    AddSkillSlotsOnVictory = chapterBoss && chapter == 3 ? 0 : node.AddSkillSlotsOnVictory,
                    BossRewards = chapterBoss && chapter < 3 ? currentSeason
                        ? new(500, 1, [
                            new("tactics", "史诗战法", tactics, Math.Min(3, tactics.Length)),
                            chapter == 2 ? new("skills", "史诗技能", skills, Math.Min(3, skills.Length))
                                : new("cards", "史诗手牌", cards.Select(reward => reward.Id).ToArray(), Math.Min(3, cards.Length))])
                        : new(500, 1, [new("tactics", "战法三选一", tactics),
                            new("cards", "卡牌三选一", cards.Select(reward => reward.Id).ToArray())])
                        : null
                };
            }).ToArray()
        });
    }

    private static bool IsEpicChapterReward(ShanheRewardDefinition reward, string kind) =>
        ShopMetadataByRewardId.TryGetValue(reward.Id, out var metadata) && metadata.Rarity == 3 && metadata.Kind == kind;

    private static bool IsChapterTactic(ShanheRewardDefinition reward) => reward.TacticSkillId is not null || reward.CampaignEconomy is { StartingGold: 0 } ||
        reward.Bonuses is { } bonus && (bonus.MaxHp != 0 || bonus.InitialHand != 0 || bonus.Draw != 0 || bonus.Slash != 0);

    private static string ChapterRouteTitle(string nodeId, int chapter) => nodeId switch
    {
        "hulao-start" or "hulao" => "第一章 · 虎牢争锋",
        "guangcheng-start" or "guangcheng" => "第一章 · 广成进军",
        "huaxiong" => "第一章 · 汜水扬威",
        "dagu" => "第二章 · 大谷关",
        "huanyuan" => "第二章 · 轘辕关",
        "lijue" => "第二章 · 西凉铁骑",
        "guosi" => "第二章 · 乱军追击",
        "luoyang" => "第三章 · 洛阳破晓",
        "suanzao" => "第三章 · 重续盟誓",
        "henei" => "第三章 · 北渡归舟",
        _ => chapter == 1 ? "第一章 · 诸侯会盟" : chapter == 2 ? "第二章 · 西凉烽烟" : "第三章 · 山河抉择"
    };
}
