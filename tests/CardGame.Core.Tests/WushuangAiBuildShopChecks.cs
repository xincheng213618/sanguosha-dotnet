using CardGame.Core;

internal static partial class WushuangAiShopChecks
{

    private static WushuangShopOffer BuildSkillOffer(string id, string skill, int cost) =>
        new(id, "ignored label", "ignored prose", cost, skill, WushuangOfferKind.Skill, 1);
}
