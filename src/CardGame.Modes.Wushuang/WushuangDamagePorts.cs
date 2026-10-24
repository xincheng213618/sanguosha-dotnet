using CardGame.Core;

namespace CardGame.Modes.Wushuang;

internal sealed partial class WushuangModeFeature : IMatchModeDamageRuleProvider
{
    public IReadOnlyList<ModeDamageRuleRegistration> DamageRules => Array.AsReadOnly<ModeDamageRuleRegistration>(
        [ModeDamageRuleRegistration.Additive(61001, damage => WushuangSourceDamageBonus(new(damage.SourceSeat,
            damage.CardUserSeat, damage.IsSourceLess, damage.IsChainPropagation, damage.EffectiveCardKind)))]);
}
