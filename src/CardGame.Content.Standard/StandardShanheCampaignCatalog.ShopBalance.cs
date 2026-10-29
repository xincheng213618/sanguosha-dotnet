using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    private static ShanheCampaignDefinition WithChapterShopBalance(ShanheCampaignDefinition definition)
    {
        if (!definition.Id.StartsWith("shanhe:zhuhou-2026-", StringComparison.Ordinal)) return definition;
        var horses = definition.Rewards.Where(reward => (reward.StartingEquipment ?? []).Concat(reward.StartingCards ?? [])
                .Any(card => EquipmentCatalog.IsEquipment(card.Kind) &&
                    EquipmentCatalog.Get(card.Kind).Slot is EquipmentSlot.OffensiveHorse or EquipmentSlot.DefensiveHorse))
            .Select(reward => reward.Id).ToHashSet(StringComparer.Ordinal);
        var openingOnly = definition.Rewards.Where(reward => reward.CampaignEconomy?.StartingGold > 0)
            .Select(reward => reward.Id).ToHashSet(StringComparer.Ordinal);
        return ShanheCampaignContent.FreezeAndValidate(definition with
        {
            ShopRarityPolicy = new(new(0, 70, 20, 10), new(0, 0, 10, 90)),
            Nodes = definition.Nodes.Select(node => node with
            {
                RewardIds = node.RewardIds?.Where(id => !openingOnly.Contains(id)).ToArray(),
                Choices = node.Kind == ShanheNodeKind.Shop
                    ? (node.Choices ?? []).Where(choice => choice.RewardId is not { } id || !horses.Contains(id)).ToArray()
                    : node.Choices
            }).ToArray()
        });
    }
}
