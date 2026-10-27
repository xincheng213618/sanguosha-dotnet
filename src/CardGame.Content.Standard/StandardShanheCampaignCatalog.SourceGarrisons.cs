using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    private static IEnumerable<ShanheEncounterDefinition> Source2026LocalGarrisons(ShanheSourceRouteRegionDefinition region,
        ShanheSourceRouteCityDefinition city, int difficulty)
    {
        var footman = StandardShanheLocalTroopPackage.FootmanId;
        var archer = StandardShanheLocalTroopPackage.ArcherId;
        var guard = StandardShanheLocalTroopPackage.GuardId;
        var medic = StandardShanheLocalTroopPackage.MedicId;
        var blade = StandardShanheLocalTroopPackage.RedBladeId;
        var captain = StandardShanheLocalTroopPackage.CaptainId;
        (string Name, string[] Generals)[] recipes = region.ChapterNumber switch
        {
            1 => [("步卒巡哨", [footman]), ("弓手伏击", [archer]),
                  ("赤刃前锋", [footman, blade]), ("军医护卫", [guard, medic])],
            2 => [("步弓合阵", [footman, archer]), ("赤刃坚阵", [guard, blade]),
                  ("校尉督战", [captain, archer, footman]), ("军医援阵", [medic, blade, guard])],
            3 => [("精锐军阵", [captain, guard, archer]), ("赤刃援军", [medic, blade, guard]),
                  ("校尉四部", [captain, archer, blade, footman]), ("军医坚营", [captain, guard, medic, blade])],
            _ => throw new InvalidOperationException("Local source garrisons require one of the three chapters.")
        };
        for (var index = 0; index < recipes.Length; index++)
        {
            var recipe = recipes[index];
            yield return new("source-local:" + city.SourceCityId + ":" + index, region.Name + " · " + recipe.Name + "（本地编排）",
                recipe.Generals.Select(id => new ShanheCombatantDefinition(id,
                    InitialHandBonus: region.ChapterNumber - 1, DrawBonus: region.ChapterNumber == 3 ? 1 : 0)).ToArray(),
                [], VictoryGold: 200, HealAfterVictory: 1)
            {
                SourceCityId = city.SourceCityId,
                TerrainSkillId = Source2026Terrain(city.SourceTerrainSkillId, difficulty),
                StoryText = "联军遭遇" + recipe.Name + "，敌军已封住去路。"
            };
        }
    }

    private static ShanheNodeDefinition Source2026LocalEliteNode(ShanheSourceRouteRegionDefinition region, string id,
        string nextId, string chapterTitle, IReadOnlyList<string> rewards)
    {
        string[] generals = region.ChapterNumber switch
        {
            1 => [StandardShanheLocalTroopPackage.CaptainId, StandardShanheLocalTroopPackage.MedicId],
            2 => [StandardShanheLocalTroopPackage.CaptainId, StandardShanheLocalTroopPackage.GuardId, StandardShanheLocalTroopPackage.MedicId],
            3 => [StandardShanheLocalTroopPackage.CaptainId, StandardShanheLocalTroopPackage.GuardId,
                StandardShanheLocalTroopPackage.MedicId, StandardShanheLocalTroopPackage.RedBladeId],
            _ => throw new InvalidOperationException("Local source elites require one of the three chapters.")
        };
        var encounter = new ShanheEncounterDefinition(id + ":encounter", region.Name + " · 西凉精锐（本地编排）",
            generals.Select(general => new ShanheCombatantDefinition(general, MaxHpBonus: region.ChapterNumber,
                InitialHandBonus: region.ChapterNumber, DrawBonus: 1, SlashBonus: 1)).ToArray(),
            [], VictoryGold: 200, HealAfterVictory: 1)
        { StoryText = "西凉校尉率精锐部众拦路，军医和护卫紧守阵脚。" };
        return new(id, region.Name + " · 精锐军营（本地）", ShanheNodeKind.EliteBattle, [nextId], [encounter], rewards)
        {
            ChapterNumber = region.ChapterNumber, ChapterTitle = chapterTitle, CategorizedRewards = true, AutomaticRewardCategory = true,
            PrimaryTacticRarity = 2, PrimarySkillRarity = region.ChapterNumber >= 2 ? 3 : 2, PrimaryCardRarity = 2, RewardSkipGold = 100,
            StoryText = encounter.StoryText,
            Description = "获得200铜币并选择稀有战法或技能奖励，随后继续本章路线。"
        };
    }
}
