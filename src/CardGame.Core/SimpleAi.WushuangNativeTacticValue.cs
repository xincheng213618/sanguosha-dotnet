namespace CardGame.Core;

public sealed partial class SimpleAiBrain
{
    // Native mode rules have no SkillProgram. These are marginal resource
    // estimates from the owning view, never catalogue prices or printed prose.
    private static double? ShopNativeTacticValue(int id, PlayerSnapshot self, PlayerSnapshot? opponent,
        WushuangSnapshot shop, SkillProgram[] owned, double future, int policyVersion)
    {
        bool Has(int tactic) => shop.TacticIds.Contains(tactic) || self.WushuangTacticIds?.Contains(tactic) == true;
        bool Enemy(int tactic) => opponent?.WushuangTacticIds?.Contains(tactic) == true;
        if (Has(id)) return 0;
        var horizon = Math.Clamp(future, 0, 3);
        var fixedDamage = Has(2210);
        var unlimited = self.Equipment.Any(card => card.Kind == CardKind.Crossbow) || owned.SelectMany(program => program.Modifiers)
            .Any(modifier => modifier.Query == SkillRuleQuery.SlashLimit && modifier.Operation == SkillRuleOperation.Unlimited);
        var slashes = NativeShopMaterials(CardKind.Slash);
        var wines = NativeShopMaterials(CardKind.Alcohol);
        var independentWineSlash = wines.Any(wine => slashes.Any(slash => slash != wine));
        var damageSkills = owned.Any(program => program.Activations.SelectMany(activation => activation.Effects)
            .Concat(program.Triggers.SelectMany(trigger => trigger.Effects)).Any(effect =>
                effect.Op is SkillProgramEffectOp.Damage or SkillProgramEffectOp.DamageParticipants && effect.Target != SkillProgramEffectTarget.Owner));
        var damageTricks = self.Hand.Count(card => card.Kind is CardKind.Duel or CardKind.FireAttack or CardKind.BarbarianAssault or CardKind.ArrowBarrage);
        var offense = Math.Min(3, slashes.Count + damageTricks + (damageSkills ? 1 : 0));
        var capped = opponent?.IncomingDamageCap == 1 && !owned.SelectMany(program => program.CardPolicies)
            .Any(policy => policy.Kind is SkillProgramCardPolicyKind.DamageBecomesHpLoss or SkillProgramCardPolicyKind.OtherParticipantDamageBecomesHpLoss);
        var damageValue = fixedDamage || capped ? 0 : horizon * (3 + offense * 1.5);
        var repeat = unlimited || owned.SelectMany(program => program.Modifiers).Any(modifier => modifier.Query == SkillRuleQuery.SlashLimit && modifier.Value > 0) ||
            new[] { 2051, 2052, 2156, 2157, 2158, 2226, 2282 }.Any(Has);
        double SlashQuota(int amount) => unlimited || amount <= 0 ? 0 : Math.Min(amount, Math.Max(0, slashes.Count - 1)) * 5d + horizon * 1.5;
        double ExtraDraw(double active) => active * horizon * 7 / (1 + Math.Max(0,
            owned.SelectMany(program => program.Modifiers).Where(modifier => modifier.Query == SkillRuleQuery.DrawCount &&
                modifier.Operation == SkillRuleOperation.Add).Sum(modifier => modifier.Value)) * .3);
        double Overflow(CardKind kind) => owned.SelectMany(program => program.CardPolicies).Any(policy => policy.Kind == SkillProgramCardPolicyKind.OfferSkipDiscard)
            ? 0 : Math.Min(self.Hand.Count(card => card.Kind == kind), Math.Max(0, self.HandCount + 2 - (self.HandLimit ?? self.Hp))) * 2.5 + horizon * .5;
        var turnsRemain = shop.RemainingMilliseconds >= 60_000 && shop.RemainingTurns is not <= 2;
        return id switch
        {
            2001 => horizon * 2 + (independentWineSlash ? 8 + Math.Min(4, opponent?.HandCount ?? 0) : 0),
            2002 => Overflow(CardKind.Dodge), 2003 => Overflow(CardKind.Peach),
            2004 => self.Hand.Any(card => card.Kind == CardKind.FireAttack) && self.HandCount >= 2 ? 8 + horizon : horizon,
            2005 or 2006 => opponent is not { HandCount: > 1 } || opponent.Hand.Count == opponent.HandCount ? 0 :
                horizon + self.Hand.Count(card => card.Kind == (id == 2005 ? CardKind.Dismantlement : CardKind.Snatch)) * 4,
            2008 => horizon + self.Hand.Count(card => card.Kind == CardKind.Nullification) * 5,
            2030 => horizon * 1.5 + self.Hand.Count(card => card.Kind == CardKind.DrawTwo) * 7,
            2042 or 2043 => damageValue * (id == 2043 ? 2 : 1) * (repeat ? .7 : .35),
            2045 => damageValue * .4 + (fixedDamage || capped ? 0 : self.Hand.Count(card => card.Suit == Suit.Heart && slashes.Contains(card.Id)) * 7),
            2108 or 2109 => repeat && (!fixedDamage || Has(2001)) ?
                Math.Min(id == 2109 ? 2 : 1, Math.Max(0, Math.Min(wines.Count, slashes.Count) - 1)) * 5 + horizon * .5 : 0,
            2115 => damageValue * .45,
            2127 => self.Hp <= 1 && !self.Hand.Any(card => card.Kind is CardKind.Peach or CardKind.Alcohol) ? 0 : damageValue * .6,
            2150 or 2151 or 2152 => damageValue * (id == 2150 ? .8 : id == 2151 ? .5 : .3),
            2153 or 2154 or 2155 => ExtraDraw(turnsRemain ? id == 2153 ? .8 : id == 2154 ? .5 : .3 : .1),
            2156 or 2157 or 2158 => SlashQuota(1) * (id == 2156 ? .8 : id == 2157 ? .6 : .3),
            2162 or 2163 => damageValue * (self.Hp <= (id == 2162 ? 2 : 3) ? 1 : .25),
            2164 or 2165 => ExtraDraw(self.Hp <= (id == 2164 ? 3 : 5) ? 1 : .25),
            2171 or 2172 => damageSkills ? damageValue * (id == 2172 ? 2 : 1) : 0,
            2188 or 2189 => damageValue * (id == 2189 ? .65 : .4),
            // Fixed-two attacks override the finite reductions in the actual
            // finalizer; buying more reduction does not counter that public rule.
            2196 or 2197 => Enemy(2210) ? 0 : horizon * (id == 2197 ? 10 : 6) * ShopDefenseNeed(self, opponent) +
                (self.Hp <= 2 ? id == 2197 ? 8 : 5 : 0),
            2210 => FixedTwoValue(),
            2221 => ReplaceOriginalValue(),
            2226 => SlashQuota(2 * Math.Max(0, shop.ExtraSkillCapacity - shop.ExtraSkillIds.Count)),
            2239 or 2240 => opponent is null ? 0 : Math.Min(id == 2240 ? 2 : 1,
                Math.Max(0, opponent.HandCount + opponent.Equipment.Count + opponent.Judgment.Count - 1)) *
                (horizon * 2 + self.Hand.Count(card => card.Kind == CardKind.Dismantlement) * 4),
            2241 => horizon + self.Hand.Count(card => card.Kind == CardKind.FiveGrains) * 6,
            2243 => horizon * .5 + Math.Min(2, Math.Max(0, self.MaxHp - self.Hp)) * self.Hand.Count(card => card.Kind == CardKind.PeachGarden) * 4,
            2282 => SlashQuota(1) * (shop.Tokens >= 3 ? 1 : turnsRemain ? .5 : 0),
            2283 => ExtraDraw(shop.Tokens >= 7 ? 1 : turnsRemain && shop.Tokens + horizon >= 7 ? .5 : 0),
            2284 => (shop.Tokens >= 3 || turnsRemain && shop.Tokens + horizon >= 3) ?
                self.HandCount + 2 > (self.HandLimit ?? self.Hp) ? 3 : .5 : 0,
            2293 => shop.ExtraSkillIds.Count < shop.ExtraSkillCapacity ? 0 :
                turnsRemain || shop.Offers.Any(offer => offer.Kind == WushuangOfferKind.Skill) ? horizon * 4 + 4 : 0,
            2298 => opponent is not null && new[] { 2059, 2060, 2208, 2153, 2154, 2155, 2164, 2165, 2283 }.Any(Enemy) ? horizon * 5 : 0,
            2299 => opponent is not null && !opponent.Equipment.Any(card => card.Kind == CardKind.Crossbow) &&
                new[] { 2051, 2052, 2209, 2156, 2157, 2158, 2226, 2282 }.Any(Enemy) ? horizon * 3 : 0,
            2333 or 2334 => fixedDamage || capped || self.Hp <= 2 || offense == 0 ? 0 :
                Math.Max(0, horizon * (offense * 2 - (opponent?.HandCount > 0 ? 3 : 1))) * (id == 2334 ? 2 : 1),
            2357 or 2358 or 2359 => damageTricks == 0 && !damageSkills ? damageValue * .1 : damageValue *
                (id == 2357 ? 1.2 : id == 2359 ? .7 : .45),
            // These native rule branches have no effect in the two-seat,
            // no-distance/no-numeric-armor Wushuang configuration.
            2007 or 2010 or 2033 or 2034 or 2129 or 2177 or 2193 or 2238 or 2244 => 0,
            _ => null
        };

        HashSet<int> NativeShopMaterials(CardKind kind) => self.Hand.Where(card =>
            card.Kind == kind || kind == CardKind.Slash && IsSlashCard(card.Kind) ||
            owned.SelectMany(program => program.ViewAs).Any(rule => rule.ForPlay && rule.InputCount <= 1 && rule.OutputKind == kind &&
                rule.SourceZones.Contains(CardZoneKind.Hand) && (rule.InputKinds.Count == 0 || rule.InputKinds.Contains(card.Kind)) &&
                (rule.InputSuits.Count == 0 || rule.InputSuits.Contains(card.Suit)) &&
                (rule.InputCategories.Count == 0 || rule.InputCategories.Contains(ShopCardCategory(card.Kind)))))
            .Select(card => card.Id).ToHashSet();

        double FixedTwoValue()
        {
            var knownBonus = owned.SelectMany(program => program.DamageModifiers).Where(modifier => !modifier.CapsToAmount)
                .Sum(modifier => Math.Max(0, modifier.Amount) * (modifier.Condition == SkillProgramDamageModifierCondition.Always ? 1 : .5));
            knownBonus += (Has(2162) && self.Hp <= 2 ? 1 : 0) + (Has(2163) && self.Hp <= 3 ? 1 : 0) +
                (Has(2333) ? 1 : 0) + (Has(2334) ? 2 : 0) +
                (Has(2045) && self.Hand.Any(card => card.Suit == Suit.Heart && slashes.Contains(card.Id)) ? 2 : 0);
            var reduced = (Enemy(2197) ? 2 : 0) + (Enemy(2196) ? 1 : 0);
            var ordinary = Math.Max(0, 1 + knownBonus - reduced);
            if (opponent?.IncomingDamageCap == 1 || opponent?.Equipment.Any(card => card.Kind == CardKind.SilverLion) == true) ordinary = Math.Min(1, ordinary);
            var gain = Math.Max(0, 2 - ordinary);
            return gain * (horizon * 4 + Math.Min(3, slashes.Count + damageTricks + (damageSkills ? 1 : 0)) * 3);
        }

        double ReplaceOriginalValue()
        {
            var original = owned.Where(program => !shop.ExtraSkillIds.Contains(program.Id) &&
                !program.Id.StartsWith("wushuang:tactic-", StringComparison.Ordinal)).ToArray();
            if (original.Length == 0 || !turnsRemain && !shop.Offers.Any(offer => offer.Kind == WushuangOfferKind.Skill)) return 0;
            var weakest = original.Min(program => ShopSkillReviewValue(program, self, opponent,
                owned.Where(other => other != program).ToArray(), horizon, acquiring: false, policyVersion));
            if (!double.IsFinite(weakest)) return 0;
            return weakest <= 3 ? 3 + horizon * 3 + Math.Min(8, Math.Max(0, -weakest)) : horizon;
        }
    }
}
