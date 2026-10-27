using CardGame.Content.Standard;
using CardGame.Core;

namespace CardGame.Wpf.ViewModels;

public sealed partial class MainViewModel
{
    private IReadOnlyList<WushuangTacticViewModel> BuildTableTacticBadges(PlayerSnapshot player)
    {
        var badges = (player.WushuangTacticIds ?? []).Select(id => new WushuangTacticViewModel(id)).ToList();
        if (!player.IsHuman || !IsShanheBattleSession || _shanheSession is not { } session ||
            CurrentShanheBuildState is not { } state) return badges;

        // Reward ownership supplies the category. A rule's name, locked tag and
        // skill id namespace do not establish that it occupies a tactic slot.
        foreach (var id in state.TacticIds)
        {
            var reward = session.Definition.Rewards.SingleOrDefault(reward => reward.Id == id);
            if (reward is null || !IsShanheTactic(reward)) continue;
            StandardShanheCampaignCatalog.ShopMetadataByRewardId.TryGetValue(reward.Id, out var metadata);
            var codex = metadata is null ? ShanheCodexTree.Entries.FirstOrDefault(entry =>
                ShanheCodexTree.RuntimeRewardId(entry.Id) == reward.Id) : null;
            var quality = reward.Rarity > 0 ? reward.Rarity : metadata?.Rarity ?? codex?.Rarity ?? 1;
            var rule = RewardRule(reward);
            badges.Add(WushuangTacticViewModel.FromContent(metadata?.SourceRewardId ?? codex?.RewardId ?? 0,
                reward.Name, string.IsNullOrWhiteSpace(rule) ? reward.Description : rule, quality,
                reward.TacticSkillId ?? reward.CampaignEconomy?.BattleSkillId));
        }
        return badges;
    }
}
