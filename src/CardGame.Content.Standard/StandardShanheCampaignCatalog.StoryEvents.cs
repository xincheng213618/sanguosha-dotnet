using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    public const string RecruitmentStoryId = "story:toumingzhuang";
    public const string RecruitmentCompanionId = "boundary:cai-wen-ji";

    private static ShanheCampaignDefinition WithStoryEventPools(ShanheCampaignDefinition definition, string editionId)
    {
        if (editionId != Returning2026EditionId) return definition;
        var eventIds = new HashSet<string>(StringComparer.Ordinal) { "village", "caravan", "rumor" };
        var stories = definition.Nodes.Where(node => eventIds.Contains(node.Id)).Select(node =>
            new ShanheStoryEventDefinition("story:" + node.Id, node.Name, node.StoryText, node.Choices!)).ToList();
        const string shared = "蔡文姬 · 体力3 · 悲歌、断肠。悲歌先判定再选择弃牌，花色相同获得判定牌，点数相同收回弃牌。";
        const string accompany = "加入后在本次征程的后续战斗助战，每战重新登场；满员八人时暂不入战。";
        stories.Add(CreateCaosongRecruitmentStory());
        stories.Add(CreateCaoruiRecruitmentStory());
        stories.Add(CreateWuguotaiRecruitmentStory());
        stories.Add(CreateGuanpingRecruitmentStory());
        stories.Add(new(RecruitmentStoryId, "投名状",
            "山上一声暴喝，窜出一名大汉，提当地污吏首级献上，想加入你军，闯荡一番事业。\n请选择帮手；普通和精英为同一名队友，精英额外拥有技能。",
            [
                new("recruit-normal", "划入军中 · 普通帮手 · 免费")
                {
                    Description = shared + "\n" + accompany,
                    Companion = new(RecruitmentCompanionId)
                },
                new("recruit-elite", "封为前锋 · 精英帮手", GoldCost: 400)
                {
                    Description = shared + "\n额外技能【回火】：出牌阶段使用杀的次数上限+1；死亡后对所有敌方角色各造成3点火焰伤害。\n" + accompany,
                    Companion = new(RecruitmentCompanionId, ExtraSkillIds: [StandardShanheRecruitmentPackage.HuihuoSkillId])
                },
                new("decline-recruit", "婉言谢绝 · 获得200铜币", RewardId: "story-decline-recruit")
                { Description = "谢绝帮手，获得200铜币，继续行军。" }
            ]));
        stories.Add(new("story:jianluojinpao", "箭落锦袍",
            "部将献上的锦袍被风吹起挂于树梢。一支利箭射出，直中细枝，摇落锦袍。射箭者对加入你军很感兴趣：‘400铜币，为将军鞍前马后。’",
            [
                new("recruit-xunyu-normal", "招纳此人 · 普通帮手 · 免费")
                {
                    Description = "荀彧 · 体力3\n驱虎：出牌阶段限一次，与体力大于你的角色拼点，赢则令其对范围内你指定的角色造成1点伤害，没赢则其对你造成1点伤害。\n" +
                        "节命：每受到1点伤害后，可令一名角色将手牌摸至其体力上限（至多5）。\n" + accompany,
                    Companion = new(StandardShanheRecruitmentPackage.XunyuNormalId)
                },
                new("recruit-xunyu-elite", "强化帮手 · 精英帮手", GoldCost: 400)
                {
                    Description = "荀彧 · 体力3 · 驱虎与普通版相同。\n精英节命：每受到1点伤害后或死亡时，可令一名角色摸X张牌，然后将手牌弃至X张（X为其体力上限且至多5）。\n" + accompany,
                    Companion = new(StandardShanheRecruitmentPackage.XunyuEliteId)
                },
                new("decline-xunyu", "忍痛拒绝 · 获得200铜币", RewardId: "story-decline-recruit")
                { Description = "谢绝帮手，获得200铜币。" }
            ]));
        var rewards = definition.Rewards.Concat(new ShanheRewardDefinition[]
        {
            new("story-decline-recruit", "谢绝帮手", Gold: 200),
            new("story-avoid-battle", "高抬贵手", Gold: 120),
            new("story-elite-equipment", "精英缴获·八卦阵", StartingEquipment: [new(CardKind.BaguaFormation, Suit.Spade, 2)])
            { Rarity = 2, Description = "本地精英奇遇奖励：每战开始将八卦阵加入手牌；遵循本征程随身装备的使用规则。" }
        }).ToArray();
        var skillRewards = rewards.Where(reward => reward.SkillId is not null && reward.Rarity == 2).Select(reward => reward.Id).ToArray();
        if (skillRewards.Length >= 2)
            stories.Add(new("story:shiyoudaru", "世有大儒", "路过此地，你听闻城中有人施粥讲学。你决定？",
                [new("decline-story-skills", "维持秩序 · 获得210铜币", RewardId: "story-decline-skills")
                { Description = "放弃此次两个技能选项，获得210铜币后继续行军。" }]) { SkillRewardIds = skillRewards });
        var eliteRewards = rewards.Where(reward => reward.Rarity >= 2 && (reward.SkillId is not null || reward.TacticSkillId is not null ||
            reward.CampaignEconomy is { StartingGold: 0 } || reward.StartingCards?.Count > 0 || reward.StartingEquipment?.Count > 0))
            .Select(reward => reward.Id).ToArray();
        var ordinaryRewards = rewards.Where(reward => reward.Rarity is 1 or 2 && (reward.SkillId is not null || reward.TacticSkillId is not null ||
            reward.StartingCards?.Count > 0 || reward.StartingEquipment?.Count > 0)).Select(reward => reward.Id).ToArray();
        var nodes = definition.Nodes.Select(node => !eventIds.Contains(node.Id) ? node : node with
        {
            Name = "奇遇", Choices = [], StoryEvents = stories.Where(story => node.ChapterNumber == 1 || story.Id != "story:zhijinanpeng")
                .Concat(CreateAdditionalRecruitmentAStories(node.ChapterNumber)).Concat(CreateAdditionalRecruitmentBStories(node.ChapterNumber))
                .Concat(BattleStories(node)).ToArray(),
            StoryText = "行军途中可能遇到求援、商旅、秘闻、投名来归的帮手或可选战斗。进入后揭晓本次奇遇。",
            Description = "随机奇遇：不同事件有各自的选项与收益，优先出现本次征程尚未遇过的事件。"
        }).Concat(definition.Nodes.Where(node => eventIds.Contains(node.Id)).SelectMany(node => new[]
        { BattleNode(node, false), BattleNode(node, true) })).ToArray();
        return ShanheCampaignContent.FreezeAndValidate(definition with
        {
            Rewards = rewards.Append(new ShanheRewardDefinition("story-decline-skills", "维持秩序", Gold: 210)).ToArray(), Nodes = nodes
        });

        IEnumerable<ShanheStoryEventDefinition> BattleStories(ShanheNodeDefinition node)
        {
            yield return new("story:huangjin", "黄巾余孽", "路遇被多次围剿未能消除的黄巾余众，对方显得十分疲惫，不想应战。",
                [Fight(false), Avoid()]);
            yield return new("story:huangjin-elite", "黄巾精锐", "黄巾余众背后还有一队精锐。你可以迎战普通部众、挑战精英，或接受铜币继续赶路。",
                [Fight(false), Fight(true), Avoid()]);
            ShanheNodeChoiceDefinition Fight(bool elite) => new(elite ? "fight-elite" : "fight-normal", elite ? "挑战精锐 · 精英战斗" : "趁机攻打 · 普通战斗")
            {
                BattleNodeId = node.Id + (elite ? ":story-elite" : ":story-normal"),
                Description = elite ? "胜利至少获得300铜币，并从稀有及以上战法、技能、装备或手牌中选择一项。" : "胜利获得200铜币及战利品。"
            };
            ShanheNodeChoiceDefinition Avoid() => new("avoid-battle", "高抬贵手 · 获得120铜币", RewardId: "story-avoid-battle")
            { Description = "不进入战斗，获得120铜币后继续行军。" };
        }
        ShanheNodeDefinition BattleNode(ShanheNodeDefinition origin, bool elite) => new(origin.Id + (elite ? ":story-elite" : ":story-normal"),
            elite ? "奇遇 · 黄巾精锐" : "奇遇 · 黄巾余孽", elite ? ShanheNodeKind.EliteBattle : ShanheNodeKind.Battle, origin.NextNodeIds,
            [new(origin.Id + (elite ? ":elite-encounter" : ":normal-encounter"), elite ? "黄巾精锐" : "黄巾余孽",
                [new("classic:zhang-jiao", MaxHpBonus: (elite ? 3 : 0) + Math.Max(0, origin.ChapterNumber - 1),
                    InitialHandBonus: elite ? 2 : 0, DrawBonus: elite ? 1 : 0, SlashBonus: elite ? 1 : 0)], VictoryGold: elite ? 300 : 200)],
            elite ? eliteRewards : ordinaryRewards)
        {
            IsStoryBattle = true, ChapterNumber = origin.ChapterNumber, ChapterTitle = origin.ChapterTitle,
            PrimaryTacticRarity = elite ? 0 : 1,
            PrimarySkillRarity = elite ? 0 : origin.ChapterNumber >= 2 ? 2 : 1,
            CategorizedRewards = true, RewardSkipGold = 100,
            StoryText = elite ? "击败黄巾精锐，领取300铜币与稀有及以上战利品。" : "击败疲惫的黄巾余众，领取200铜币。",
            Description = "由奇遇选项进入的本地战斗编排，胜利后回到原来的行军路线。"
        };
    }
}
