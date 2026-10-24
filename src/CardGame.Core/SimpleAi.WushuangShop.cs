namespace CardGame.Core;

public enum WushuangShopAction { Buy, Reroll, Lock, Finish }

public sealed record WushuangShopCandidate(WushuangShopAction Action, string? OfferId,
    string? ReplaceSkillId, double Score, string Reason);

public sealed record WushuangShopDecision(WushuangShopAction Action, string? OfferId,
    string? ReplaceSkillId, double Score, string Reason)
{
    public IReadOnlyList<WushuangShopCandidate> Candidates { get; init; } = [];
}

public sealed partial class SimpleAiBrain
{
    private long _wushuangShopWindow = -1;
    private int _wushuangFreeRefreshes, _wushuangPaidRefreshes;
    private string? _wushuangSavedOffer;
    private int _wushuangSavedTurn;

    /// <summary>Only this seat's published shop and player view enter this decision.</summary>
    public WushuangShopDecision ChooseWushuangShop(GameSnapshot ownView, WushuangSnapshot ownShop,
        ContentRegistry publicDefinitions)
    {
        var candidates = new List<WushuangShopCandidate>();
        void Add(WushuangShopAction action, double score, string reason, string? offer = null, string? replace = null) =>
            candidates.Add(new(action, offer, replace, score, reason));
        Add(WushuangShopAction.Finish, 0, "保留虎符，结束本次购买。");
        var self = ownView.Players.FirstOrDefault(p => p.Seat == Seat);
        if (_policyVersion < AiDecisionPolicy.WushuangStrategyVersion || ownShop.Seat != Seat || self is not { IsAlive: true } ||
            ownShop.Phase == WushuangShopPhase.Closed)
            return Result();
        // CanShop is the human command permission in the native projection;
        // AI decisions use the supplied owning intermission and seat instead.
        if (_wushuangShopWindow != ownShop.WindowId)
        { _wushuangShopWindow = ownShop.WindowId; _wushuangFreeRefreshes = 0; _wushuangPaidRefreshes = 0; }
        var opponent = ownView.Players.FirstOrDefault(p => p.Seat != Seat && p.IsAlive);
        var owned = (self.Skills ?? []).Select(s => s.ContentId).Where(s => s is not null).Cast<string>()
            .Concat(ownShop.ExtraSkillIds).Distinct(StringComparer.Ordinal).ToArray();
        var opening = ownShop.Phase == WushuangShopPhase.OpeningTactic;
        var future = ownShop.RemainingMilliseconds < 60_000 ? .25d : self.Hp <= 1 ? 1d : 2.5d;
        var savings = new List<(WushuangShopOffer Offer, double Net)>();
        foreach (var offer in ownShop.Offers)
        {
            if (offer.Cost < 0 || opening && offer.Kind != WushuangOfferKind.Tactic ||
                offer.TacticId is { } duplicate && ownShop.TacticIds.Contains(duplicate) ||
                offer.SkillId is { } known && owned.Contains(known, StringComparer.Ordinal)) continue;
            if (!ShopOfferAvailable(offer, self, publicDefinitions)) continue;
            var replacements = offer.SkillId is null ? new string?[] { null } :
                ownShop.ExtraSkillIds.Concat(ownShop.ReplaceableOriginalSkillIds).Distinct(StringComparer.Ordinal)
                    .Select(id => (string?)id).Prepend(null).Where(id => id is not null ||
                        ownShop.ExtraSkillIds.Count < ownShop.ExtraSkillCapacity).ToArray();
            foreach (var replacement in replacements)
            {
                var programs = owned.Where(id => id != replacement).Select(id => publicDefinitions.Skills.GetValueOrDefault(id)?.Program)
                    .Where(p => p is not null).Cast<SkillProgram>().ToArray();
                var value = ShopOfferValue(offer, self, opponent, ownShop, publicDefinitions, programs, future);
                if (replacement is not null)
                    value -= ShopSkillValue(publicDefinitions.Skills.GetValueOrDefault(replacement)?.Program,
                        self, opponent, programs, future, acquiring: false);
                var net = value - (opening ? 0 : offer.Cost * 3d);
                if (offer.Cost <= ownShop.Tokens)
                    Add(WushuangShopAction.Buy, opening ? net + 100 : net,
                        replacement is null ? "按当前生存、持续收益和手牌协同购买。" : "新增能力的净收益超过被替换技能。",
                        offer.Id, replacement);
                else if (!opening && replacement is null && net > 5)
                    savings.Add((offer, net));
            }
        }
        if (opening) return Result();
        var bestBuy = candidates.Where(c => c.Action == WushuangShopAction.Buy).Select(c => c.Score).DefaultIfEmpty(0).Max();
        if (bestBuy <= 0)
        {
            var saving = savings.Where(s => s.Offer.Cost <= ownShop.Tokens + 2)
                .OrderByDescending(s => s.Net).ThenBy(s => s.Offer.Id, StringComparer.Ordinal).FirstOrDefault();
            var savedStillUseful = ownShop.IsLocked && saving.Offer is not null && saving.Offer.Id == _wushuangSavedOffer &&
                ownView.TurnNumber - _wushuangSavedTurn <= 2;
            if (ownShop.IsLocked && !savedStillUseful)
                Add(WushuangShopAction.Lock, 2, "保存商品已无净收益或等待已到界限，解除锁页。");
            else if (!ownShop.IsLocked && saving.Offer is not null &&
                (_wushuangSavedOffer != saving.Offer.Id || ownView.TurnNumber - _wushuangSavedTurn <= 2))
                Add(WushuangShopAction.Lock, 2, "保存两虎符以内即可购买的有效商品，最多等待两个正常回合。", saving.Offer.Id);
            else if (!ownShop.IsLocked && ownShop.FreeRerolls > 0 && _wushuangFreeRefreshes < 2)
                Add(WushuangShopAction.Reroll, 1, "当前页没有正净收益，使用本窗口有限的免费刷新。");
            else if (!ownShop.IsLocked && ownShop.FreeRerolls == 0 && ownShop.RerollCost > 0 &&
                ownShop.Tokens >= ownShop.RerollCost + 4 && _wushuangPaidRefreshes == 0)
                Add(WushuangShopAction.Reroll, .5, "保留四虎符购买预算，仅付费寻找一次更合适的商品。");
        }
        var result = Result();
        if (result.Action == WushuangShopAction.Reroll)
        { if (ownShop.FreeRerolls > 0) _wushuangFreeRefreshes++; else _wushuangPaidRefreshes++; }
        if (result.Action == WushuangShopAction.Lock && !ownShop.IsLocked)
        { _wushuangSavedOffer = result.OfferId; _wushuangSavedTurn = ownView.TurnNumber; }
        return result;

        WushuangShopDecision Result()
        {
            var ordered = candidates.OrderByDescending(c => c.Score)
                .ThenBy(c => c.Score <= 0 && c.Action != WushuangShopAction.Finish ? 1 : 0).ThenBy(c => c.Action)
                .ThenBy(c => c.OfferId, StringComparer.Ordinal).ThenBy(c => c.ReplaceSkillId, StringComparer.Ordinal).ToArray();
            var selected = ordered[0];
            return new(selected.Action, selected.OfferId, selected.ReplaceSkillId, selected.Score, selected.Reason)
            { Candidates = Array.AsReadOnly(ordered) };
        }
    }

    private void CopyWushuangShopStateTo(SimpleAiBrain copy)
    {
        copy._wushuangShopWindow = _wushuangShopWindow; copy._wushuangFreeRefreshes = _wushuangFreeRefreshes;
        copy._wushuangPaidRefreshes = _wushuangPaidRefreshes; copy._wushuangSavedOffer = _wushuangSavedOffer;
        copy._wushuangSavedTurn = _wushuangSavedTurn;
    }

    private static bool ShopOfferAvailable(WushuangShopOffer offer, PlayerSnapshot self, ContentRegistry definitions)
    {
        if (offer.SkillId is { } skill && (!definitions.Skills.TryGetValue(skill, out var definition) ||
            definition.ImplementationStatus != SkillImplementationStatus.Complete || !WushuangCatalogue.IsSkillAllowed(definition))) return false;
        if (offer.TacticId is { } id && (!WushuangTactics.ById.TryGetValue(id, out var tactic) || !tactic.IsAvailableInShop)) return false;
        if (offer.CardKind is { } kind)
        {
            if (!WushuangCatalogue.IsCardAllowed(kind)) return false;
            if (EquipmentCatalog.IsEquipment(kind) && (self.IsEquipmentAreaAbolished ||
                (self.EquipmentSlotCapacities?.GetValueOrDefault(EquipmentCatalog.Get(kind).Slot, 1) ?? 1) <= 0)) return false;
        }
        return true;
    }

    private static double ShopOfferValue(WushuangShopOffer offer, PlayerSnapshot self, PlayerSnapshot? opponent,
        WushuangSnapshot shop, ContentRegistry definitions, SkillProgram[] owned, double future)
    {
        if (offer.CardKind is { } card) return ShopCardValue(card, self, opponent, owned);
        var skill = offer.SkillId;
        if (offer.TacticId is { } id && WushuangTactics.ById.TryGetValue(id, out var tactic))
        {
            skill = tactic.SkillId;
            // These exact public catalogue identities are native economy rules,
            // rather than skill programs; their effects are defined by the mode.
            var economy = id switch
            {
                2285 => future * 3, 2286 => future * 6, 2366 => future * 6,
                2281 => shop.Tokens >= 5 ? future * 6 : future * 2,
                2287 => 3, 2288 => 6, 2369 => 15, 2289 => future * 2,
                2290 => future * 2, 2291 => 3, 2292 => 5, _ => 0d
            };
            if (economy > 0) return economy;
        }
        if (skill is not null)
            return ShopSkillValue(definitions.Skills.GetValueOrDefault(skill)?.Program, self, opponent, owned, future, acquiring: true);
        return offer.Kind switch
        {
            WushuangOfferKind.Recover => self.Hp < self.MaxHp ? ShopHealthValue(self) : 0,
            WushuangOfferKind.Draw => 7, WushuangOfferKind.MaxHp => 4, _ => 0
        };
    }

    private static double ShopSkillValue(SkillProgram? program, PlayerSnapshot self, PlayerSnapshot? opponent,
        SkillProgram[] owned, double future, bool acquiring)
    {
        if (program is null) return 0;
        var value = 0d;
        var draws = 2 + owned.SelectMany(p => p.Modifiers).Where(m => m.Query == SkillRuleQuery.DrawCount &&
            m.Operation == SkillRuleOperation.Add && m.ValueExpression is null).Sum(m => m.Value);
        var unlimitedSlash = self.Equipment.Any(c => c.Kind == CardKind.Crossbow) || owned.SelectMany(p => p.Modifiers)
            .Any(m => m.Query == SkillRuleQuery.SlashLimit && m.Operation == SkillRuleOperation.Unlimited);
        var slashes = self.Hand.Count(c => IsSlashCard(c.Kind));
        foreach (var modifier in program.Modifiers)
        {
            var amount = modifier.ValueExpression switch
            {
                null => modifier.Value, SkillRuleValueExpression.OwnerLostHp => Math.Max(0, self.MaxHp - self.Hp),
                SkillRuleValueExpression.OwnerMaxHp => self.MaxHp, _ => 0
            };
            var conditional = modifier.Condition.Kind == SkillProgramConditionKind.Always ? 1d : .5d;
            value += conditional * (modifier.Query switch
            {
                SkillRuleQuery.DrawCount => amount * future * 7 / (1 + Math.Max(0, draws - 2) * .3),
                SkillRuleQuery.HandLimit => amount * (self.HandCount + Math.Max(0, draws) > (self.HandLimit ?? self.Hp) ? 2.5 : .7),
                SkillRuleQuery.SlashLimit => unlimitedSlash ? 0 : modifier.Operation == SkillRuleOperation.Unlimited
                    ? 4 + Math.Max(0, slashes - 1) * 5 : Math.Max(0, amount) * (2 + Math.Min(3, Math.Max(0, slashes - 1)) * 2),
                _ => 0
            });
        }
        foreach (var conversion in program.ViewAs)
        {
            var same = owned.SelectMany(p => p.ViewAs).Where(v => v.OutputKind == conversion.OutputKind).ToArray();
            bool Accepts(SkillProgramViewAs v, CardSnapshot c) => v.InputCount <= 1 && v.SourceZones.Contains(CardZoneKind.Hand) &&
                (v.InputKinds.Count == 0 || v.InputKinds.Contains(c.Kind)) && (v.InputSuits.Count == 0 || v.InputSuits.Contains(c.Suit)) &&
                (v.InputCategories.Count == 0 || v.InputCategories.Contains(ShopCardCategory(c.Kind)) ||
                    v.InputCategories.Contains(SkillProgramCardCategory.InstantTrick) && ShopCardCategory(c.Kind) == SkillProgramCardCategory.Trick &&
                    c.Kind is not (CardKind.Indulgence or CardKind.SupplyShortage or CardKind.Lightning));
            var useful = self.Hand.Count(c => Accepts(conversion, c) && !same.Any(v => Accepts(v, c)));
            if (same.Length == 0 || useful > 0)
                value += ((conversion.OutputKind is CardKind.Peach or CardKind.Dodge ? 5 : 3) + Math.Min(3, useful) * 2) /
                    Math.Max(1, conversion.InputCount);
        }
        foreach (var policy in program.CardPolicies)
        {
            if (owned.SelectMany(p => p.CardPolicies).Any(p => p.Kind == policy.Kind && p.InputSuit == policy.InputSuit && p.Value >= policy.Value)) continue;
            value += policy.Kind switch
            {
                SkillProgramCardPolicyKind.BypassSlashLimitBySuit => unlimitedSlash ? 0 : 2 +
                    self.Hand.Count(c => IsSlashCard(c.Kind) && (policy.InputSuit is null || policy.InputSuit == c.Suit)) * 2,
                SkillProgramCardPolicyKind.PreventForeignEquipmentDiscard => self.Equipment.Count == 0 ? 1 : 3,
                SkillProgramCardPolicyKind.ProhibitBlackTrickTarget or SkillProgramCardPolicyKind.ProhibitDelayedTrickTarget => 5,
                SkillProgramCardPolicyKind.MinimumResponseCount => 4 + Math.Min(2, slashes) * 2,
                SkillProgramCardPolicyKind.OfferSkipDiscard => Math.Max(1, self.HandCount - (self.HandLimit ?? self.Hp)) * 2,
                SkillProgramCardPolicyKind.UnrespondableNullification => self.Hand.Any(c => c.Kind == CardKind.Nullification) ? 4 : 1,
                _ => 0
            };
        }
        foreach (var trigger in program.Triggers)
        {
            if (trigger.Window == SkillProgramTriggerWindow.SkillsChanged && !acquiring) continue;
            var frequency = trigger.Window == SkillProgramTriggerWindow.SkillsChanged ? 1 : future;
            if (trigger.Window == SkillProgramTriggerWindow.AfterDamageApplied) frequency *= .6;
            if (trigger.Condition.Kind == SkillProgramTriggerConditionKind.CardActionActualTurnUseOrdinalIs &&
                trigger.Condition.ActualTurnUseOrdinal > self.HandCount + Math.Max(0, draws)) frequency *= .15;
            value += frequency * ShopEffectsValue(trigger.Effects, self, opponent, owned);
        }
        foreach (var activation in program.Activations)
            value += future * .7 * Math.Max(0, ShopEffectsValue(activation.Effects, self, opponent, owned) - DraftActivationCardCost(activation) * 3);
        // Custom executable operations already have a bounded capability reader
        // used by general selection. Reuse it for otherwise unscored programs;
        // reach/support have no value in this two-seat mode.
        if (value == 0 && program.Modifiers.Count == 0 && program.ViewAs.Count == 0 && program.CardPolicies.Count == 0 &&
            (program.Triggers.Any(t => t.Window != SkillProgramTriggerWindow.SkillsChanged) || program.Activations.Count > 0))
        {
            var capability = ReadDraftSkillBenefits(program, null);
            value = future * (capability.Draw * 7 + capability.Defense * 5 + capability.Offense * 5 + capability.Control * 5 + capability.Growth * 3)
                + capability.Recovery * (self.Hp < self.MaxHp ? ShopHealthValue(self) : 2)
                - capability.PaidHp * ShopHealthValue(self) - capability.Attrition * ShopHealthValue(self);
        }
        return value;
    }

    private static double ShopEffectsValue(IReadOnlyList<SkillProgramEffect> effects, PlayerSnapshot self,
        PlayerSnapshot? opponent, SkillProgram[] owned)
    {
        var result = 0d;
        var maximumGain = effects.Where(e => e.Target == SkillProgramEffectTarget.Owner && e.Op == SkillProgramEffectOp.ChangeMaximumHp)
            .Sum(e => Math.Max(0, e.Amount));
        foreach (var effect in effects)
        {
            var amount = effect.Amount > 0 ? effect.Amount : effect.NumberExpression is not null ? 1 : 0;
            var owner = effect.Target == SkillProgramEffectTarget.Owner;
            var benefit = effect.Op switch
            {
                SkillProgramEffectOp.Draw => owner ? amount * 7d : 0,
                SkillProgramEffectOp.ObtainWushuangDeckCards => owner && effect.OutputKind is { } card
                    ? amount * ShopCardValue(card, self, opponent, owned) : 0,
                SkillProgramEffectOp.Recover => owner ? Math.Min(amount, Math.Max(0, self.MaxHp + maximumGain - self.Hp)) * ShopHealthValue(self) : 0,
                SkillProgramEffectOp.RecoverToMaximum => owner ? Math.Max(0, self.MaxHp - self.Hp) * ShopHealthValue(self) : 0,
                SkillProgramEffectOp.ChangeMaximumHp => owner ? effect.Amount * 4d : 0,
                SkillProgramEffectOp.Damage or SkillProgramEffectOp.DamageParticipants => owner ? -amount * ShopHealthValue(self) : amount * 7d,
                SkillProgramEffectOp.LoseHp => owner ? -amount * ShopHealthValue(self) : amount * 6d,
                SkillProgramEffectOp.DiscardSelected => -Math.Max(amount, effect.MinimumCards) * 3d,
                _ => 0
            };
            result += benefit * (effect.Condition.Kind == SkillProgramConditionKind.Always ? 1 : .5);
        }
        return result;
    }

    private static double ShopHealthValue(PlayerSnapshot self) => self.Hp <= 1 ? 32 : self.Hp == 2 ? 14 : 8;

    private static SkillProgramCardCategory ShopCardCategory(CardKind kind) => EquipmentCatalog.IsEquipment(kind)
        ? SkillProgramCardCategory.Equipment : IsSlashCard(kind) || kind is CardKind.Peach or CardKind.Dodge or CardKind.Alcohol
            ? SkillProgramCardCategory.Basic : SkillProgramCardCategory.Trick;

    private static double ShopDefenseNeed(PlayerSnapshot self, PlayerSnapshot? opponent) => opponent is not null &&
        (!opponent.HpUndefined && opponent.Hp >= self.Hp + 2 && opponent.HandCount >= self.HandCount ||
            (opponent.WushuangTacticIds?.Count ?? 0) >= (self.WushuangTacticIds?.Count ?? 0) + 2) ? 1.25 : 1;

    private static double ShopCardValue(CardKind kind, PlayerSnapshot self, PlayerSnapshot? opponent, SkillProgram[] owned)
    {
        var copies = self.Hand.Count(c => c.Kind == kind || IsSlashCard(kind) && IsSlashCard(c.Kind));
        var defense = ShopDefenseNeed(self, opponent);
        if (EquipmentCatalog.IsEquipment(kind))
        {
            var slot = EquipmentCatalog.Get(kind).Slot;
            var current = self.Equipment.Where(c => EquipmentCatalog.Get(c.Kind).Slot == slot).ToArray();
            if (current.Any(c => c.Kind == kind)) return 0;
            double EquipmentValue(CardKind item) => item switch
            {
                CardKind.Crossbow => 3 + Math.Max(0, self.Hand.Count(c => IsSlashCard(c.Kind)) - 1) * 5,
                CardKind.BaguaFormation => (8 + (self.Hp <= 2 ? 4 : 0)) * defense,
                CardKind.RenwangShield => 9 * defense, CardKind.SilverLion => 8 * defense,
                _ => EquipmentCatalog.Get(item).Slot == EquipmentSlot.Armor ? 7 * defense : 3
            };
            return EquipmentValue(kind) - current.Select(c => EquipmentValue(c.Kind)).DefaultIfEmpty(0).Min();
        }
        if (IsSlashCard(kind))
        {
            var unlimited = self.Equipment.Any(c => c.Kind == CardKind.Crossbow) || owned.SelectMany(p => p.Modifiers)
                .Any(m => m.Query == SkillRuleQuery.SlashLimit && m.Operation == SkillRuleOperation.Unlimited);
            return copies == 0 || unlimited ? 7 : copies == 1 ? 2 : .4;
        }
        return kind switch
        {
            CardKind.Peach => copies == 0 ? self.Hp < self.MaxHp ? ShopHealthValue(self) * defense : 6 : copies == 1 ? 2 : .3,
            CardKind.Dodge => copies == 0 ? (self.Hp <= 2 ? 13 : 8) * defense : copies == 1 ? 4 : 1,
            CardKind.Alcohol => copies > 0 || self.HasAlcoholEffect ? 1 : self.Hp <= 1 ? 10 : self.Hand.Any(c => IsSlashCard(c.Kind)) ? 6 : 2,
            CardKind.DrawTwo => 13,
            CardKind.Dismantle or CardKind.Snatch => opponent is not null && opponent.HandCount + opponent.Equipment.Count > 0 ? 8 : 0,
            CardKind.Duel => self.Hand.Count(c => IsSlashCard(c.Kind)) > 0 ? 7 : 3,
            CardKind.Nullification => copies == 0 ? 6 : 2,
            CardKind.Indulgence or CardKind.SupplyShortage => 7,
            CardKind.FireAttack => self.HandCount >= 3 ? 5 : 1,
            CardKind.IronChain => 1,
            _ => 3
        };
    }
}
