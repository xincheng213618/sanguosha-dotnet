using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    // Project metadata and physical-card rules after the full definition is composed.
    private static ShanheCampaignDefinition WithStartingResourcesContract(ShanheCampaignDefinition definition,
        bool categorizeNormalRewards = false)
    {
        var currentSeason = definition.Id.StartsWith("shanhe:zhuhou-2026-", StringComparison.Ordinal);
        definition = definition with
        {
            Rewards = definition.Rewards.Select(reward => ShopMetadataByRewardId.TryGetValue(reward.Id, out var metadata)
                ? reward with { Rarity = metadata.Rarity } : reward).ToArray()
        };
        const string useNotice = "使用或响应结束时，若原应进入弃牌堆，则移出游戏；若先被技能获得，仍保留该牌。";
        var physical = definition.Rewards.Where(reward => reward.StartingCards?.Count > 0 || reward.StartingEquipment?.Count > 0)
            .ToDictionary(reward => reward.Id, reward => reward with
            {
                Description = reward.Description
                    .Replace("本地版按普通实体牌流转。", string.Empty, StringComparison.Ordinal)
                    .Replace(useNotice, string.Empty, StringComparison.Ordinal)
                    .Replace("每场开始将八卦阵加入手牌", "每场开始直接装备八卦阵", StringComparison.Ordinal)
                    .Replace("每战开始将八卦阵加入手牌", "每战开始直接装备八卦阵", StringComparison.Ordinal) +
                    (reward.StartingEquipment?.Count > 0
                        ? "每场战斗开始时置入装备区，可被其他角色正常获得和使用。"
                        : useNotice)
            }, StringComparer.Ordinal);
        var originals = definition.Rewards.ToDictionary(reward => reward.Id, StringComparer.Ordinal);
        return ShanheCampaignContent.FreezeAndValidate(definition with
        {
            StartingResourcesExpireOnUse = true,
            Rewards = definition.Rewards.Select(reward => physical.GetValueOrDefault(reward.Id) ?? reward).ToArray(),
            Nodes = definition.Nodes.Select(node => node with
            {
                CategorizedRewards = node.CategorizedRewards || categorizeNormalRewards &&
                    node.Kind is ShanheNodeKind.Battle or ShanheNodeKind.EliteBattle,
                AutomaticRewardCategory = node.AutomaticRewardCategory || node.CategorizedRewards || categorizeNormalRewards &&
                    node.Kind is ShanheNodeKind.Battle or ShanheNodeKind.EliteBattle,
                Encounters = currentSeason && node.Kind == ShanheNodeKind.Battle
                    ? node.Encounters!.Select(encounter => encounter with { VictoryGold = 200 }).ToArray()
                    : node.Encounters,
                // Codex shops copy the reward text before adding the price. Keep
                // that authored price and update only the copied resource contract.
                Choices = node.Choices?.Select(choice => choice.RewardId is { } id && physical.TryGetValue(id, out var updated) &&
                    !string.IsNullOrEmpty(originals[id].Description) && !string.IsNullOrEmpty(choice.Description)
                    ? choice with { Description = choice.Description.Replace(originals[id].Description, updated.Description, StringComparison.Ordinal) }
                    : choice).ToArray()
            }).ToArray()
        });
    }
}
