namespace CardGame.Core.Campaigns;

/// <summary>One frozen catalogue and transaction seed, without a campaign command history.</summary>
public sealed record ShanheBattleMarketSetup(
    ShanheCampaignDefinition Definition, ShanheCampaignOptions Options, ShanheBattleMarketTransfer State)
{
    public bool BossBattle { get; init; }

    public static ShanheBattleMarketSetup Freeze(ShanheBattleMarketSetup value)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(value.Options);
        ArgumentNullException.ThrowIfNull(value.State);
        return value with
        {
            Definition = ShanheCampaignContent.FreezeAndValidate(value.Definition),
            Options = value.Options with { }, State = ShanheBattleMarketTransfer.Freeze(value.State)
        };
    }
}

/// <summary>Wallet, permanent build and chapter shelf owned by the transaction session.</summary>
public sealed record ShanheBattleMarketTransfer
{
    public string MarketContentHash { get; init; } = "";
    public string MarketNodeId { get; init; } = "";
    public int ChapterNumber { get; init; }
    public int Hp { get; init; }
    public int Gold { get; init; }
    public int ExtraSkillSlots { get; init; }
    public ShanheTacticBonuses Bonuses { get; init; } = new();
    public IReadOnlyList<string> ExtraSkillIds { get; init; } = Array.Empty<string>();
    public IReadOnlyList<string> TacticIds { get; init; } = Array.Empty<string>();
    public IReadOnlyList<ShanheStartingCard> StartingCards { get; init; } = Array.Empty<ShanheStartingCard>();
    public IReadOnlyList<ShanheStartingCard> StartingEquipment { get; init; } = Array.Empty<ShanheStartingCard>();
    public IReadOnlyList<ShanheNodeChoiceDefinition> ShopStock { get; init; } = Array.Empty<ShanheNodeChoiceDefinition>();
    public IReadOnlyList<string> PurchasedChoiceIds { get; init; } = Array.Empty<string>();
    public int ShopRefreshCount { get; init; }
    public int ShopFreeRefreshCount { get; init; }
    public int ShopPaidRefreshCount { get; init; }
    public int ShopRestockCount { get; init; }
    public int ShopTacticPurchases { get; init; }
    public int ShopEquipmentPurchases { get; init; }
    public int ShopCardPurchases { get; init; }
    public int ShopSkillPurchases { get; init; }

    public static ShanheBattleMarketTransfer Freeze(ShanheBattleMarketTransfer value)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(value.ExtraSkillIds);
        ArgumentNullException.ThrowIfNull(value.TacticIds);
        ArgumentNullException.ThrowIfNull(value.StartingCards);
        ArgumentNullException.ThrowIfNull(value.StartingEquipment);
        ArgumentNullException.ThrowIfNull(value.ShopStock);
        ArgumentNullException.ThrowIfNull(value.PurchasedChoiceIds);
        if (value.ShopStock.Any(choice => choice is null))
            throw new ArgumentException("A market shelf cannot contain null choices.", nameof(value));
        return value with
        {
            ExtraSkillIds = ShanheCampaignContent.Freeze(value.ExtraSkillIds),
            TacticIds = ShanheCampaignContent.Freeze(value.TacticIds),
            StartingCards = ShanheCampaignContent.Freeze(value.StartingCards),
            StartingEquipment = ShanheCampaignContent.Freeze(value.StartingEquipment),
            ShopStock = ShanheCampaignContent.Freeze(value.ShopStock.Select(choice => choice with { })),
            PurchasedChoiceIds = ShanheCampaignContent.Freeze(value.PurchasedChoiceIds)
        };
    }
}

public static partial class ShanheCampaignContent
{
    internal static void ValidateMarketResources(ShanheBattleMarketTransfer value)
    {
        ValidateBonuses(value.Bonuses);
        ValidateCards(value.StartingCards, equipmentOnly: false);
        ValidateCards(value.StartingEquipment, equipmentOnly: true);
    }
}
