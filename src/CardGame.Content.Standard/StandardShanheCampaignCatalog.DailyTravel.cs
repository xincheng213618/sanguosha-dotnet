using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    private static ShanheCampaignDefinition WithDailyTravelBattles(ShanheCampaignDefinition definition, string editionId)
    {
        if (editionId != Returning2026EditionId) return definition;
        var nodes = definition.Nodes.ToList();
        foreach (var chapter in new[] { 1, 2, 3 })
        {
            var bosses = definition.Nodes.Where(node => node.ChapterNumber == chapter && ChapterBossIds.Contains(node.Id))
                .Select(node => node.Id).ToArray();
            var basis = definition.Nodes.First(node => node.ChapterNumber == chapter && node.Kind == ShanheNodeKind.Battle &&
                !node.IsStoryBattle && node.Id is not ("hulao-start" or "guangcheng-start"));
            var ids = new[] { "chapter-" + chapter + ":extra-battle-1", "chapter-" + chapter + ":extra-battle-2" };
            for (var index = 0; index < nodes.Count; index++)
                if (nodes[index].ChapterNumber == chapter && !ChapterBossIds.Contains(nodes[index].Id) &&
                    nodes[index].NextNodeIds.Any(bosses.Contains))
                    nodes[index] = nodes[index] with { NextNodeIds = nodes[index].NextNodeIds.Append(ids[0]).ToArray() };
            for (var extra = 1; extra <= 2; extra++)
                nodes.Add(basis with
                {
                    Id = ids[extra - 1], Name = "第" + chapter + "章 · 追击" + extra,
                    ExtraChapterBattleIndex = extra,
                    NextNodeIds = extra == 1 ? bosses.Prepend(ids[1]).ToArray() : bosses,
                    Encounters = basis.Encounters!.Select(encounter => encounter with
                    { Id = ids[extra - 1] + ":" + encounter.Id, VictoryGold = 200, SourceBattleId = null }).ToArray(),
                    StoryText = "日行千里让你赶上了本章额外的敌军。可追击积累铜币与战利品，也可直接迎战关底。",
                    Description = "日行千里额外战斗；胜利获得200铜币及战利品。"
                });
        }
        return ShanheCampaignContent.FreezeAndValidate(definition with { Nodes = nodes });
    }
}
