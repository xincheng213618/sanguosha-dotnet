using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    // Exact rarity-4 skills from https://s.xiaochao.org/data/skills.json and
    // S5 tree nodes 74/93/95/96/100/108 in shanhe-codex-2026.json.
    // Their complete existing programs are reused, never a same-name alternate edition.
    private static IReadOnlyList<ShanheRewardDefinition> ChapterLegendarySkillRewards { get; } = Array.AsReadOnly(new[]
    {
        ChapterSkill(3017, "伤逝", "classic:shangshi", "当你的手牌数小于X后，你可以将手牌摸至X张。（X为你已损失体力值）"),
        ChapterSkill(3038, "龙吟", "classic:longyin", "当一名角色于其出牌阶段内使用【杀】时，你可以弃置一张牌，令此【杀】不计入此阶段使用次数，若此【杀】为红色，你摸一张牌。"),
        ChapterSkill(3091, "当先", "classic:dangxian", "锁定技，回合开始时，你执行一个额外的出牌阶段。"),
        ChapterSkill(3020, "酒池", "boundary:jiuchi-current", "你可以将一张黑桃手牌当【酒】使用。你使用【酒】无次数限制。当你使用【酒】【杀】造成伤害后，本回合“崩坏”失效。"),
        ChapterSkill(3070, "博图", "boundary:botu", "每轮限X次（X为存活角色数且至多为3），回合结束后，若本回合置入弃牌堆的牌包含四种花色，你可以执行一个额外回合。"),
        ChapterSkill(3261, "征南", "ol:zhengnan", "当其他角色死亡后，你可以摸三张牌，然后获得“武圣”“当先”“制蛮”中的一个。")
    });

    private static ShanheRewardDefinition ChapterSkill(int sourceId, string name, string skillId, string description) =>
        new("chapter-skill-" + sourceId.ToString(System.Globalization.CultureInfo.InvariantCulture), "传习·" + name, SkillId: skillId)
        { Description = description + "占用一个额外技能槽。" };
}
