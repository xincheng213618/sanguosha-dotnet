namespace CardGame.Core;

public sealed partial class SimpleAiBrain
{
    private sealed record WushuangRefreshNeed(int PurchaseReserve, int MaximumRefreshes, string Reason);

    private static int WushuangSavingTurnBudget(PlayerSnapshot self, WushuangSnapshot shop)
    {
        if (shop.RemainingMilliseconds <= 60_000 || shop.RemainingTurns is <= 2) return 0;
        // Base income is at least one token on each natural actor turn. A short
        // bounded wait is conservative about extra turns and future purchases;
        // extra tactic income is not promised in order to justify a lock.
        return Math.Min(self.Hp <= 2 ? 2 : 4, Math.Max(0, (shop.RemainingTurns ?? 5) - 1));
    }

    private static WushuangRefreshNeed? WushuangPaidRefreshNeed(PlayerSnapshot self,
        WushuangSnapshot shop, ContentRegistry definitions, IReadOnlyList<string> ownedIds)
    {
        // The next private page and deck order never enter this decision. Search
        // for a missing role in the current build, preserving a purchase budget.
        if (shop.RemainingTurns is <= 0 || shop.RemainingMilliseconds <= 5_000) return null;
        var programs = ownedIds.Select(id => definitions.Skills.GetValueOrDefault(id)?.Program)
            .OfType<SkillProgram>().ToArray();
        var hand = self.Hand.Concat(self.WoodenOxGrain ?? []).DistinctBy(card => card.Id).ToArray();
        bool CanBecome(CardSnapshot card, CardKind kind) => card.Kind == kind || kind == CardKind.Slash && IsSlashCard(card.Kind) ||
            programs.SelectMany(program => program.ViewAs).Any(rule => rule.ForPlay && rule.InputCount <= 1 &&
                rule.OutputKind == kind && rule.SourceZones.Contains(CardZoneKind.Hand) &&
                (rule.InputKinds.Count == 0 || rule.InputKinds.Contains(card.Kind)) &&
                (rule.InputSuits.Count == 0 || rule.InputSuits.Contains(card.Suit)) &&
                (rule.InputCategories.Count == 0 || rule.InputCategories.Contains(ShopCardCategory(card.Kind))));
        if (!self.HpUndefined && self.Hp <= 2 && self.Hp < self.MaxHp &&
            !hand.Any(card => CanBecome(card, CardKind.Peach) || self.Hp <= 1 && CanBecome(card, CardKind.Alcohol)))
            return new(1, 2, "当前血线缺少救援资源，保留一虎符购买预算，最多付费刷新两次寻找回复或防御。");

        var build = WushuangBuildProfile.Read(programs);
        if ((build.SlashSealsTargetCards || build.SlashBurstBonus > 0) && shop.RemainingTurns is not <= 1)
        {
            var slashIds = hand.Where(card => CanBecome(card, CardKind.Slash)).Select(card => card.Id).ToArray();
            var wineIds = hand.Where(card => CanBecome(card, CardKind.Alcohol)).Select(card => card.Id).ToArray();
            var needsSlash = slashIds.Length == 0;
            bool HasTactic(int id) => shop.TacticIds.Contains(id) || self.WushuangTacticIds?.Contains(id) == true;
            var wineWorks = !HasTactic(2210) || HasTactic(2001);
            var needsWine = wineWorks && !self.HasAlcoholEffect && (needsSlash
                ? wineIds.Length == 0 : !wineIds.Any(wine => slashIds.Any(slash => wine != slash)));
            if (needsSlash || needsWine)
                return new(needsSlash && needsWine ? 2 : 1, 2,
                    "当前杀的增伤或封牌构筑缺少独立杀酒组件，留出购买预算，最多付费刷新两次寻找可执行爆发。");
        }
        return new(4, 1, "当前页没有有效购买，保留四虎符预算，仅付费寻找一次更合适的商品。");
    }
}
