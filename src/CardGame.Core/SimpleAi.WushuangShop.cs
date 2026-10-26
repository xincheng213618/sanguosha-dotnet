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
        if (ownShop.RemainingTurns is { } remaining) future = Math.Min(future, Math.Max(0, remaining) / 2d);
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
                var value = ShopOfferValue(offer, self, opponent, ownShop, publicDefinitions, programs, future, _policyVersion);
                if (replacement is not null)
                    value -= ShopSkillReviewValue(publicDefinitions.Skills.GetValueOrDefault(replacement)?.Program,
                        self, opponent, programs, future, acquiring: false, _policyVersion);
                // An unconfirmed/conflicting hypothetical draw build is not
                // a legal purchase estimate, including during opening choice.
                if (!double.IsFinite(value)) continue;
                var net = value - (opening ? 0 : offer.Cost * 3d);
                if (offer.Cost <= ownShop.Tokens)
                    Add(WushuangShopAction.Buy, opening ? net + 100 : net,
                        replacement is null ? "按当前生存、持续收益和手牌协同购买。" : "新增能力的净收益超过被替换技能。",
                        offer.Id, replacement);
                else if (!opening && net > 5)
                    savings.Add((offer, net));
            }
        }
        if (opening) return Result();
        var bestBuy = candidates.Where(c => c.Action == WushuangShopAction.Buy).Select(c => c.Score).DefaultIfEmpty(0).Max();
        if (bestBuy <= 0)
        {
            var savingTurns = WushuangSavingTurnBudget(self, ownShop);
            var saving = savings.Where(s => s.Offer.Cost <= ownShop.Tokens + savingTurns)
                .OrderByDescending(s => s.Net / (1 + (s.Offer.Cost - ownShop.Tokens) * .15))
                .ThenBy(s => s.Offer.Id, StringComparer.Ordinal).FirstOrDefault();
            var savedStillUseful = ownShop.IsLocked && saving.Offer is not null && saving.Offer.Id == _wushuangSavedOffer &&
                ownView.TurnNumber - _wushuangSavedTurn <= savingTurns;
            if (ownShop.IsLocked && !savedStillUseful)
                Add(WushuangShopAction.Lock, 2, "保存商品已无净收益或等待已到界限，解除锁页。");
            else if (!ownShop.IsLocked && saving.Offer is not null &&
                (_wushuangSavedOffer != saving.Offer.Id || ownView.TurnNumber - _wushuangSavedTurn <= savingTurns))
                Add(WushuangShopAction.Lock, 2, $"保存当前构筑有净收益的商品，保留虎符等待购买；本次最多等待{savingTurns}个行动回合。", saving.Offer.Id);
            else if (!ownShop.IsLocked && ownShop.FreeRerolls > 0 && _wushuangFreeRefreshes < 2)
                Add(WushuangShopAction.Reroll, 1, "当前页没有正净收益，使用本窗口有限的免费刷新。");
            else if (!ownShop.IsLocked && ownShop.FreeRerolls == 0 && ownShop.RerollCost > 0 &&
                WushuangPaidRefreshNeed(self, ownShop, publicDefinitions, owned) is { } need &&
                ownShop.Tokens >= ownShop.RerollCost + need.PurchaseReserve &&
                _wushuangPaidRefreshes < need.MaximumRefreshes)
                Add(WushuangShopAction.Reroll, .5, need.Reason);
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
        WushuangSnapshot shop, ContentRegistry definitions, SkillProgram[] owned, double future, int policyVersion)
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
            if (ShopNativeTacticValue(id, self, opponent, shop, owned, future, policyVersion) is { } native)
                return native + ShopNativeBuildFit(id, native, self, opponent, owned);
        }
        if (skill is not null)
            return ShopSkillReviewValue(definitions.Skills.GetValueOrDefault(skill)?.Program, self, opponent, owned, future, acquiring: true, policyVersion);
        return offer.Kind switch
        {
            WushuangOfferKind.Recover => self.Hp < self.MaxHp ? ShopHealthValue(self) + ShopBuildRecoveryBonus(owned) : 0,
            WushuangOfferKind.Draw => 7, WushuangOfferKind.MaxHp => 4 + ShopBuildRecoveryBonus(owned) * .5, _ => 0
        };
    }

    private static double ShopSkillValue(SkillProgram? program, PlayerSnapshot self, PlayerSnapshot? opponent,
        SkillProgram[] owned, double future, bool acquiring)
        => ShopSkillValueCore(program, self, opponent, owned, future, acquiring, review: false);

    private static double ShopSkillReviewValue(SkillProgram? program, PlayerSnapshot self, PlayerSnapshot? opponent,
        SkillProgram[] owned, double future, bool acquiring, int policyVersion)
        => ShopSkillValueCore(program, self, opponent, owned, future, acquiring,
            review: policyVersion >= AiDecisionPolicy.WushuangLossReviewVersion,
            refinement: policyVersion >= AiDecisionPolicy.WushuangRefinementVersion);

    private static double ShopSkillValueCore(SkillProgram? program, PlayerSnapshot self, PlayerSnapshot? opponent,
        SkillProgram[] owned, double future, bool acquiring, bool review, bool refinement = false)
    {
        if (program is null) return 0;
        var value = 0d;
        var ownedDraw = owned.SelectMany(p => p.Modifiers).Where(m =>
                m.Query == SkillRuleQuery.DrawCount && m.RecipientScope == SkillRuleRecipientScope.Owner)
            .Select(m => (Modifier: m, Amount: ShopKnownModifierAmount(m, self), Active: ShopDrawConditionApplies(m.Condition, self))).ToArray();
        if (program.Modifiers.Any(m => m.Query == SkillRuleQuery.DrawCount) &&
            ownedDraw.Any(item => item.Active is null || item.Active == true && item.Amount is null)) return double.NegativeInfinity;
        var drawContributions = ownedDraw.Where(item => item.Active == true && item.Amount is not null)
            .Select((item, index) => (RuleQueryContribution)new FiniteRuleQueryContribution(
                "shop-owned-draw:" + index, item.Modifier.Operation, item.Amount!.Value, item.Modifier.Priority)).ToList();
        if (ShopReducedDrawCount(drawContributions) is not { } draws) return double.NegativeInfinity;
        var offeredDraw = program.Modifiers.Where(m => m.Query == SkillRuleQuery.DrawCount && m.RecipientScope == SkillRuleRecipientScope.Owner)
            .Select(m => (Modifier: m, Amount: ShopKnownModifierAmount(m, self), Active: ShopDrawConditionApplies(m.Condition, self))).ToArray();
        if (offeredDraw.Any(item => item.Active is null || item.Active == true && item.Amount is null)) return double.NegativeInfinity;
        var additions = offeredDraw.Where(item => item.Active == true).Select((item, index) =>
            (item.Modifier, Contribution: (RuleQueryContribution)new FiniteRuleQueryContribution("shop-added-draw:" + index,
                item.Modifier.Operation, item.Amount!.Value, item.Modifier.Priority))).ToArray();
        // Add each complete modifier group atomically: an intermediate lower
        // Set conflict may be resolved by the same skill's higher Set.
        if (ShopReducedDrawCount(drawContributions.Concat(additions.Where(item =>
                item.Modifier.Condition.Kind == SkillProgramConditionKind.Always).Select(item => item.Contribution)).ToArray()) is not { } guaranteedDraws ||
            ShopReducedDrawCount(drawContributions.Concat(additions.Select(item => item.Contribution)).ToArray()) is not { } possibleDraws)
            return double.NegativeInfinity;
        value += (guaranteedDraws - draws + (possibleDraws - guaranteedDraws) * .5) * future * 7 / (1 + Math.Max(0, draws - 2) * .3);
        if (ShopHandLimitValue(program, self, owned, draws) is not { } handLimitValue) return double.NegativeInfinity;
        value += handLimitValue;
        var unlimitedSlash = self.Equipment.Any(c => c.Kind == CardKind.Crossbow) || owned.SelectMany(p => p.Modifiers)
            .Any(m => m.Query == SkillRuleQuery.SlashLimit && m.Operation == SkillRuleOperation.Unlimited);
        var slashes = self.Hand.Count(c => IsSlashCard(c.Kind));
        foreach (var modifier in program.Modifiers)
        {
            var conditional = modifier.Condition.Kind == SkillProgramConditionKind.Always ? 1d : .5d;
            if (modifier.Query is SkillRuleQuery.DrawCount or SkillRuleQuery.HandLimit) continue;
            var amount = modifier.ValueExpression switch
            {
                null => modifier.Value, SkillRuleValueExpression.OwnerLostHp => Math.Max(0, self.MaxHp - self.Hp),
                SkillRuleValueExpression.OwnerMaxHp => self.MaxHp, _ => 0
            };
            value += conditional * (modifier.Query switch
            {
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
            var frequency = ShopTriggerFrequency(trigger, self, owned.Append(program).ToArray(), future);
            if (trigger.Condition.Kind == SkillProgramTriggerConditionKind.CardActionActualTurnUseOrdinalIs &&
                trigger.Condition.ActualTurnUseOrdinal > self.HandCount + Math.Max(0, draws)) frequency *= .15;
            value += frequency * (review ? ShopReviewedTriggerValue(program, trigger, self, opponent, owned) :
                ShopEffectsValue(trigger.Effects, self, opponent, owned));
        }
        foreach (var activation in program.Activations)
            value += future * .7 * Math.Max(0, review
                ? ShopReviewedActivationValue(program, activation, self, opponent, owned, future, refinement)
                : ShopEffectsValue(activation.Effects, self, opponent, owned) - DraftActivationCardCost(activation) * 3);
        // Custom executable operations already have a bounded capability reader
        // used by general selection. Reuse it for otherwise unscored programs;
        // reach/support have no value in this two-seat mode.
        if (value == 0 && (!review || !ShopReviewHasPricedEffects(program)) &&
            program.Modifiers.Count == 0 && program.ViewAs.Count == 0 && program.CardPolicies.Count == 0 &&
            (program.Triggers.Any(t => t.Window != SkillProgramTriggerWindow.SkillsChanged) || program.Activations.Count > 0))
        {
            var capability = ReadDraftSkillBenefits(program, null);
            value = future * (capability.Draw * 7 + capability.Defense * 5 + capability.Offense * 5 + capability.Control * 5 + capability.Growth * 3)
                + capability.Recovery * (self.Hp < self.MaxHp ? ShopHealthValue(self) : 2)
                - capability.PaidHp * ShopHealthValue(self) - capability.Attrition * ShopHealthValue(self);
        }
        return value + ShopSkillBuildFit(program, self, opponent, owned, future);
    }

    private static int? ShopKnownModifierAmount(SkillProgramModifier modifier, PlayerSnapshot self) => modifier.ValueExpression switch
    {
        null => modifier.Value,
        SkillRuleValueExpression.OwnerLostHp => Math.Max(0, self.MaxHp - self.Hp),
        SkillRuleValueExpression.OwnerMaxHp => self.MaxHp,
        _ => null
    };

    private static int? ShopReducedDrawCount(IReadOnlyList<RuleQueryContribution> contributions)
        => ShopReducedFiniteCount(2, contributions);

    private static int? ShopReducedFiniteCount(int baseValue, IReadOnlyList<RuleQueryContribution> contributions)
    {
        // The native query applies the highest-priority Set to its base, adds
        // all Add contributions, then clamps once. Price only its change;
        // replacing a skill uses the same remaining build on both sides.
        var sets = contributions.OfType<FiniteRuleQueryContribution>().Where(item => item.Operation == SkillRuleOperation.Set).ToArray();
        if (sets.Length > 0)
        {
            var priority = sets.Max(set => set.Priority);
            if (sets.Where(item => item.Priority == priority).Select(item => item.Value).Distinct().Skip(1).Any()) return null;
        }
        return ((FiniteRuleQueryValue)RuleQueryReducer.Reduce(baseValue, new(0, int.MaxValue), contributions)).Value;
    }

    // Estimate the owner's next normal Draw from current public state. Unknown
    // frame/state conditions are not assumed active, or silently treated as off.
    private static bool? ShopDrawConditionApplies(SkillProgramCondition condition, PlayerSnapshot self) => condition.Kind switch
    {
        SkillProgramConditionKind.Always or SkillProgramConditionKind.OwnTurn => true,
        SkillProgramConditionKind.NotOwnTurn or SkillProgramConditionKind.ClassicIdentityMode => false,
        SkillProgramConditionKind.Wounded => self.HpUndefined ? null : self.Hp < self.MaxHp,
        SkillProgramConditionKind.HpAtLeast => self.HpUndefined ? null : self.Hp >= condition.Value,
        SkillProgramConditionKind.HandCountAtLeast => self.HandCount >= condition.Value,
        SkillProgramConditionKind.HandCountGreaterThanHp => self.HpUndefined ? null : self.HandCount > self.Hp,
        SkillProgramConditionKind.Not => ShopDrawConditionApplies(condition.Children[0], self) is { } child ? !child : null,
        SkillProgramConditionKind.All => condition.Children.Any(child => ShopDrawConditionApplies(child, self) == false) ? false :
            condition.Children.All(child => ShopDrawConditionApplies(child, self) == true) ? true : null,
        SkillProgramConditionKind.Any => condition.Children.Any(child => ShopDrawConditionApplies(child, self) == true) ? true :
            condition.Children.All(child => ShopDrawConditionApplies(child, self) == false) ? false : null,
        _ => null
    };

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
                SkillProgramEffectOp.Draw or SkillProgramEffectOp.AdjustNormalDraw => owner ? amount * 7d : 0,
                SkillProgramEffectOp.ObtainWushuangDeckCards => !owner ? 0 : effect.OutputKind is { } card
                    ? amount * ShopCardValue(card, self, opponent, owned) : amount * 6d,
                SkillProgramEffectOp.Recover => owner ? Math.Min(amount, Math.Max(0, self.MaxHp + maximumGain - self.Hp)) * (ShopHealthValue(self) + ShopBuildRecoveryBonus(owned)) : 0,
                SkillProgramEffectOp.RecoverToMaximum => owner ? Math.Max(0, self.MaxHp - self.Hp) * (ShopHealthValue(self) + ShopBuildRecoveryBonus(owned)) : 0,
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
                CardKind.RenwangShield => 9 * defense,
                CardKind.SilverLion => (8 + (opponent?.WushuangTacticIds?.Contains(2210) == true ? 0 :
                    Math.Min(4, ShopBuildRecoveryBonus(owned) * .5))) * defense,
                _ => EquipmentCatalog.Get(item).Slot == EquipmentSlot.Armor ? 7 * defense : 3
            };
            return EquipmentValue(kind) - current.Select(c => EquipmentValue(c.Kind)).DefaultIfEmpty(0).Min();
        }
        if (IsSlashCard(kind))
        {
            var unlimited = self.Equipment.Any(c => c.Kind == CardKind.Crossbow) || owned.SelectMany(p => p.Modifiers)
                .Any(m => m.Query == SkillRuleQuery.SlashLimit && m.Operation == SkillRuleOperation.Unlimited);
            return (copies == 0 || unlimited ? 7 : copies == 1 ? 2 : .4) +
                (copies == 0 || unlimited ? ShopSlashBuildBonus(self, opponent, owned) : 0);
        }
        return kind switch
        {
            CardKind.Peach => copies == 0 ? (self.Hp < self.MaxHp ? ShopHealthValue(self) * defense : 6) + ShopBuildRecoveryBonus(owned) : copies == 1 ? 2 : .3,
            CardKind.Dodge => copies == 0 ? (self.Hp <= 2 ? 13 : 8) * defense : copies == 1 ? 4 : 1,
            CardKind.Alcohol => copies > 0 || self.HasAlcoholEffect ? 1 : self.Hp <= 1 ? 10 : self.Hand.Any(c => IsSlashCard(c.Kind))
                ? ((self.WushuangTacticIds?.Contains(2210) == true || opponent?.IncomingDamageCap == 1) && self.WushuangTacticIds?.Contains(2001) != true
                    ? 2 : 6 + ShopSlashBuildBonus(self, opponent, owned)) : 2,
            CardKind.DrawTwo => 13,
            CardKind.Dismantlement or CardKind.Snatch => opponent is not null && opponent.HandCount + opponent.Equipment.Count > 0 ? 8 : 0,
            CardKind.Duel => self.Hand.Count(c => IsSlashCard(c.Kind)) > 0 ? 7 : 3,
            CardKind.Nullification => copies == 0 ? 6 : 2,
            CardKind.Indulgence or CardKind.SupplyShortage => 7,
            CardKind.FireAttack => self.HandCount >= 3 ? 5 : 1,
            CardKind.IronChain => 1,
            _ => 3
        };
    }
}
