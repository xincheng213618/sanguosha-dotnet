using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    private static ShanheCampaignDefinition WithGuideTerrain(ShanheCampaignDefinition definition, int difficulty, string editionId)
    {
        if (editionId != Returning2026EditionId) return definition;
        var dagu = definition.Nodes.Single(node => node.Id == "dagu");
        var huanyuan = definition.Nodes.Single(node => node.Id == "huanyuan");
        var sites = new[]
        {
            (Id: "yingyang-terrain", Name: "荥阳 · 追击乱军", Terrain: StandardShanheTerrainPackage.CityId, Template: dagu),
            (Id: "mengjin-terrain", Name: "孟津 · 争夺渡口", Terrain: StandardShanheTerrainPackage.BattlefieldId, Template: huanyuan),
            (Id: "xiaopingjin-terrain", Name: "小平津 · 抢占津口", Terrain: StandardShanheTerrainPackage.RiverId, Template: dagu),
            (Id: "yingchuan-terrain", Name: "颍川郡 · 探查诸侯", Terrain: StandardShanheTerrainPackage.PlainId, Template: huanyuan),
            (Id: "yique-terrain", Name: "伊阙关 · 追查物资", Terrain: StandardShanheTerrainPackage.MountainId, Template: dagu)
        };
        var additions = sites.Select(site => site.Template with
        {
            Id = site.Id, Name = "第二章 · " + site.Name,
            StoryText = "联军行至" + site.Name.Split(' ')[0] + "，前方出现敌军。选择战利品后继续行军。",
            Description = "沿本章本地敌军池迎战。" + TerrainDescription(site.Terrain, difficulty),
            Encounters = site.Template.Encounters!.Select(encounter => encounter with
            {
                Id = site.Id + ":" + encounter.Id, Name = site.Name, TerrainSkillId = difficulty == 3 ? site.Terrain : null,
                StoryText = "联军行至" + site.Name.Split(' ')[0] + "，前方出现敌军。\n\n" + TerrainDescription(site.Terrain, difficulty)
            }).ToArray()
        }).ToArray();
        return ShanheCampaignContent.FreezeAndValidate(definition with
        {
            IntroText = definition.IntroText + "\n\n困难档启用场地效果，作用于所有角色；普通和中等档不启用。",
            Nodes = definition.Nodes.Select(node => node.Id switch
            {
                "huaxiong" => node with { NextNodeIds = node.NextNodeIds.Concat(additions.Select(site => site.Id)).ToArray() },
                "dagu" => Apply(node, StandardShanheTerrainPackage.RiverId),
                "huanyuan" => Apply(node, StandardShanheTerrainPackage.ForestId),
                _ => node
            }).Concat(additions).ToArray()
        });

        ShanheNodeDefinition Apply(ShanheNodeDefinition node, string terrain) => node with
        {
            Description = node.Description + "\n" + TerrainDescription(terrain, difficulty),
            Encounters = node.Encounters!.Select(encounter => encounter with
            {
                TerrainSkillId = difficulty == 3 ? terrain : null,
                StoryText = (encounter.StoryText ?? node.StoryText) + "\n\n" + TerrainDescription(terrain, difficulty)
            }).ToArray()
        };
    }

    private static string TerrainDescription(string id, int difficulty)
    {
        var rule = id switch
        {
            StandardShanheTerrainPackage.CityId => "城池：所有角色到其他角色的距离-1。",
            StandardShanheTerrainPackage.BattlefieldId => "战场：所有角色摸牌阶段多摸2张牌。",
            StandardShanheTerrainPackage.RiverId => "河流：所有角色的手牌上限至少为8。",
            StandardShanheTerrainPackage.ForestId => "密林：所有角色每个出牌阶段至多使用10张牌。",
            StandardShanheTerrainPackage.PlainId => "平原：所有角色出牌阶段使用杀的次数+1。",
            StandardShanheTerrainPackage.MountainId => "高山：所有角色回合结束时失去1点体力。",
            _ => throw new ArgumentException("Unknown terrain.", nameof(id))
        };
        return rule + (difficulty == 3 ? "" : "本档不启用场地效果。");
    }
}
