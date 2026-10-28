using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    private static IReadOnlyList<ShanheCombatantDefinition> MixedOrdinaryMembers(int group, int tier) => group switch
    {
        2464 =>
        [
            new(StandardShanheSourceDemonAssassinPackage.GetGeneralId(tier)),
            new(StandardShanheSourceDemonArcherPackage.GetTrioGeneralId(tier)),
            new(StandardShanheSourceDemonRaiderPackage.GetTrioGeneralId(tier))
        ],
        2499 => StandardShanheSourceYulinOfficerPackage.GetRosterGeneralIds(tier)
            .Select(id => new ShanheCombatantDefinition(id)).ToArray(),
        2492 => StandardShanheSourceJiangdongTrioPackage.GetRosterGeneralIds(tier)
            .Select(id => new ShanheCombatantDefinition(id)).ToArray(),
        2465 =>
        [
            new(StandardShanheSourceCoalitionTrioPackage.GetSpyGeneralId(tier)),
            new(StandardShanheSourceCoalitionTrioPackage.GetScoutGeneralId(tier)),
            new(StandardShanheSourceCoalitionTrioPackage.GetQuartermasterGeneralId(tier))
        ],
        _ => throw new ArgumentOutOfRangeException(nameof(group))
    };

    private static ShanheEncounterDefinition ReturningMixedOrdinary(int group, int difficulty, string id, int chapter)
    {
        var growth = difficulty - 1;
        var tier = difficulty switch { 1 => 1, 2 => 11, 3 => 21, _ => throw new ArgumentOutOfRangeException(nameof(difficulty)) };
        var third = chapter == 3;
        var data = Source2026EnemyDataByGroup[group];
        var members = MixedOrdinaryMembers(group, tier).Select((member, index) =>
        {
            var attributes = data[index].TierAttributes.Where(t => t.DifficultyGroupId <= (tier + 4) / 5).Select(t => t.Attributes).ToArray();
            var extraHand = attributes.Sum(a => a.StartingCards);
            var hpBonus = (third ? 4 : 3) + 2 * growth + attributes.Sum(a => a.Hp);
            return member with
            {
                Hp = data[index].Hp + hpBonus, MaxHpBonus = hpBonus, DrawBonus = third ? 2 : 1,
                InitialHandBonus = (third ? 2 : 1) + growth + extraHand
            };
        }).ToArray();
        return new(id, OrdinaryTroopName(group), members, [], VictoryGold: 200, HealAfterVictory: third ? 1 : 2)
        { StoryText = OrdinaryTroopStory(group) };
    }
}
