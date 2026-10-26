namespace CardGame.Core.Campaigns;

public static partial class ShanheCampaignContent
{
    private static void ValidateOpeningCompanionReward(ShanheCampaignDefinition definition, ShanheRewardDefinition reward)
    {
        if (reward.OpeningCompanionPool is not { } pool) return;
        Require(pool.Count is > 0 and <= 16, "An opening companion tactic needs a bounded explicit pool.");
        UniqueIds(pool.Select(member => member.GeneralId), "opening companion");
        foreach (var member in pool) ValidateCombatant(member);
        Require(reward.TacticSkillId is null && reward.SkillId is null && reward.OpeningSkillRewardIds is null &&
            reward.CampaignEconomy is null && !reward.BossBattleOnly && reward.Gold == 0 && reward.Heal == 0 &&
            reward.AddSkillSlots == 0 && reward.Bonuses is null or { MaxHp: 0, InitialHand: 0, Draw: 0, Slash: 0 } &&
            (reward.StartingCards?.Count ?? 0) == 0 && (reward.StartingEquipment?.Count ?? 0) == 0,
            "An opening companion tactic cannot impersonate another reward effect.");
        Require(definition.Nodes.All(node => !(node.RewardIds ?? []).Contains(reward.Id, StringComparer.Ordinal) &&
            !(node.Choices ?? []).Any(choice => choice.RewardId == reward.Id) &&
            !(node.BossRewards?.Groups ?? []).Any(group => group.RewardIds.Contains(reward.Id, StringComparer.Ordinal))),
            "A companion tactic may only be equipped before an expedition.");
    }
}
