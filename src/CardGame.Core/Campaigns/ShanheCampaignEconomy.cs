using System.Text.Json.Serialization;

namespace CardGame.Core.Campaigns;

/// <summary>Expedition tactics, including battle programs qualified by the current campaign balance.</summary>
public sealed record ShanheCampaignEconomy(
    int BattleGoldPercent = 0, int PostBattleInterestPercent = 0, int ShopDiscountPercent = 0,
    int ShopRefreshLimit = 0, int ExtraStockPerCategory = 0, int ReplacedEquipmentGold = 0,
    bool SuppressShopPriceEscalation = false, int StartingGold = 0,
    int? GoldAbove = null, string? BattleSkillId = null,
    int TacticRewardChoiceCount = 0, int CardRewardChoiceCount = 0,
    int ExtraEliteRareTactics = 0, int ExtraBossEpicSkills = 0, int SkillRewardChoiceCount = 0,
    int FreeShopRefreshes = 0, bool RestockOnPurchase = false,
    int ShopTacticPromotionPercent = 0, int ShopSkillPromotionPercent = 0, int ShopMaterialPromotionPercent = 0,
    int BattlePrimaryPromotionPercent = 0, bool UnlimitedPaidShopRefreshes = false,
    int BattleGoldRandomMinPercent = 100, int BattleGoldRandomMaxPercent = 100,
    bool UpgradeOrdinaryBattles = false,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)] int ExtraChapterBattles = 0,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)] bool FavorOwnedSchoolRewards = false);

public static partial class ShanheCampaignContent
{
    private static void ValidateEconomy(ShanheRewardDefinition reward)
    {
        if (reward.CampaignEconomy is not { } economy) return;
        Require(reward.SkillId is null && reward.TacticSkillId is null && reward.OpeningSkillRewardIds is null &&
            reward.StartingCards is not { Count: > 0 } && reward.StartingEquipment is not { Count: > 0 } &&
            reward.Gold == 0 && reward.Heal == 0 && reward.AddSkillSlots == 0 &&
            reward.Bonuses is null or { MaxHp: 0, InitialHand: 0, Draw: 0, Slash: 0 },
            "An economy tactic cannot impersonate a battle skill, equipment or immediate reward.");
        Require(economy.BattleGoldPercent is >= -100 and <= 100 && economy.PostBattleInterestPercent is >= 0 and <= 100 &&
            economy.ShopDiscountPercent is >= 0 and <= 100 && economy.ShopRefreshLimit is >= 0 and <= 20 &&
            economy.ExtraStockPerCategory is >= 0 and <= 12 && economy.ReplacedEquipmentGold is >= 0 and <= 100000 &&
            economy.StartingGold is >= 0 and <= 100000 &&
            economy.TacticRewardChoiceCount is 0 or >= 3 and <= 8 && economy.CardRewardChoiceCount is 0 or >= 3 and <= 8 &&
            economy.SkillRewardChoiceCount is 0 or >= 3 and <= 8 &&
            economy.FreeShopRefreshes is >= 0 and <= 20 &&
            economy.ShopTacticPromotionPercent is >= 0 and <= 100 && economy.ShopSkillPromotionPercent is >= 0 and <= 100 &&
            economy.ShopMaterialPromotionPercent is >= 0 and <= 100 &&
            economy.BattlePrimaryPromotionPercent is >= 0 and <= 100 &&
            economy.BattleGoldRandomMinPercent is >= 0 and <= 300 &&
            economy.BattleGoldRandomMaxPercent is >= 0 and <= 300 &&
            (economy.BattleGoldRandomMinPercent == 100 && economy.BattleGoldRandomMaxPercent == 100 ||
                economy.BattleGoldRandomMinPercent < economy.BattleGoldRandomMaxPercent) &&
            economy.ExtraEliteRareTactics is >= 0 and <= 4 && economy.ExtraBossEpicSkills is >= 0 and <= 4 &&
            economy.ExtraChapterBattles is >= 0 and <= 2 &&
            (economy.GoldAbove is null && economy.BattleSkillId is null ||
                economy.GoldAbove is >= 0 and <= 1000000 && !string.IsNullOrWhiteSpace(economy.BattleSkillId)) &&
            economy != new ShanheCampaignEconomy(), "Invalid or empty campaign economy tactic.");
    }
}
