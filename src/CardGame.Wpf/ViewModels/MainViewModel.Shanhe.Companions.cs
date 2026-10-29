using System.Windows.Media;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Wpf.ViewModels;

public sealed partial class MainViewModel
{
    public IReadOnlyList<ShanheStoryCompanionCard> ShanheStartingCompanionCards => _shanheSession is not { } session ? [] :
        session.Snapshot.StartingCompanions.Select(companion =>
        {
            var probe = new ShanheBattleSetup(1, 1, "opening-companion-display", session.Snapshot.PlayerGeneralId, 1, 0, 0, 0, 0,
                [], [], [], [new("classic:cao-cao")], [companion], session.ContentHash);
            var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(probe);
            var general = registry.Generals[companion.GeneralId];
            var elite = session.Definition.Rewards.Any(reward => reward.Id == StandardShanheCampaignCatalog.EliteOpeningCompanionRewardId &&
                reward.OpeningCompanionPool?.Any(member => member.GeneralId == companion.GeneralId) == true);
            return new ShanheStoryCompanionCard(general.Name + (elite ? " · 精英" : ""),
                GeneralArt.GetPortrait(companion.GeneralId, decodeWidth: 120), ShanheCompanionRules(registry, companion));
        }).ToArray();

    private static string ShanheCompanionRules(ContentRegistry registry, ShanheCombatantDefinition companion)
    {
        var general = registry.Generals[companion.GeneralId];
        var skills = general.SkillIds.Concat(companion.ExtraSkillIds ?? [])
            .Distinct(StringComparer.Ordinal).Select(id => registry.Skills[id]);
        return general.Name + " · 基础体力" + general.BaseHp + "\n" +
            string.Join("\n", skills.Select(skill => skill.Name + "：" + GetVisibleSkillDescription(skill)));
    }

    public bool HasShanheStartingCompanion => _shanheSession?.Snapshot.StartingCompanions.Count > 0;
    public string ShanheStartingCompanionName => string.Join("、", ShanheStartingCompanionCards.Select(card => card.Name));
    public Brush ShanheStartingCompanionPortrait => ShanheStartingCompanionCards.FirstOrDefault()?.Portrait ?? Brushes.Transparent;
    public string ShanheStartingCompanionDescription => string.Join("\n\n", ShanheStartingCompanionCards.Select(card => card.Description));
}
