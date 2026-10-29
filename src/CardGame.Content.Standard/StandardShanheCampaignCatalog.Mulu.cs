using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    public const string MuluEncounterNodeId = "luoyang-mulu";

    private static ShanheCampaignDefinition WithMuluEncounter(ShanheCampaignDefinition definition, int difficulty, string editionId)
    {
        if (editionId != Returning2026EditionId) return definition;
        var camp = definition.Nodes.Single(node => node.Id == "finalcamp");
        var rewards = definition.Nodes.Single(node => node.Id == "dagu").RewardIds ?? [];
        var growth = checked((difficulty - 1) * 2);
        var encounter = new ShanheEncounterDefinition("luoyang:mulu-2397", "八纳洞主 · 木鹿大王与四象",
            [new(StandardShanheMuluPackage.MuluId, MaxHpBonus: growth),
             new(StandardShanheMuluPackage.RageElephantId, MaxHpBonus: growth),
             new(StandardShanheMuluPackage.AngryElephantId, MaxHpBonus: growth),
             new(StandardShanheMuluPackage.RageElephantId, MaxHpBonus: growth),
             new(StandardShanheMuluPackage.AngryElephantId, MaxHpBonus: growth)], [], VictoryGold: 200)
        {
            StoryText = "木鹿大王驱使四头巨象拦路。只要召唤者还在，阵亡巨象便会在其回合开始时重新站起。"
        };
        var node = new ShanheNodeDefinition(MuluEncounterNodeId, "第三章 · 八纳洞主", ShanheNodeKind.Battle,
            camp.NextNodeIds, [encounter], rewards)
        {
            ChapterNumber = 3, ChapterTitle = "第三章 · 洛阳", CategorizedRewards = true, AutomaticRewardCategory = true,
            PrimaryTacticRarity = 1, PrimarySkillRarity = 2, PrimaryCardRarity = 1, RewardSkipGold = 100,
            StoryText = encounter.StoryText!,
            Description = "木鹿大王与四头巨象共同登场；扬升按巨象本轮实际受伤次数，在其回合开始时确定本段加伤，至多3。"
        };
        return ShanheCampaignContent.FreezeAndValidate(definition with
        {
            Nodes = definition.Nodes.Select(existing => existing.Id == camp.Id
                ? existing with { NextNodeIds = existing.NextNodeIds.Append(node.Id).ToArray() } : existing).Append(node).ToArray()
        });
    }
}
