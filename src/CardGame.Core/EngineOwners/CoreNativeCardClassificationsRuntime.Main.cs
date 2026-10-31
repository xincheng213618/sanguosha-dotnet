#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeCardClassificationsRuntime
{
    internal static bool IsDelayedCard(CardKind kind) => RuntimeSkills.RuntimeRules_Core_CommonRules.IsDelayedCard(kind);
    internal static bool IsOrdinaryTrick(CardKind kind) => RuntimeSkills.RuntimeRules_Core_CommonRules.IsOrdinaryTrick(kind);
    internal static string GetSuitDisplayName(Suit? suit) => RuntimeSkills.RuntimeRules_Core_CommonRules.GetSuitDisplayName(suit);
    internal static bool IsRedSuit(Suit suit) => RuntimeSkills.RuntimeRules_Core_CommonRules.IsRedSuit(suit);
    internal static bool IsLightningHit(Card card) => RuntimeSkills.RuntimeRules_Core_CommonRules.IsLightningHit(card);
    internal static bool IsOwnedPlayableLocation(CharacterState actor, CardLocation location) => location.OwnerSeat == actor.Seat && location.Zone is CardZoneKind.Hand or CardZoneKind.WoodenOxGrain;
    internal static bool IsPublicTargetZone(CardZoneKind zone) => RuntimeSkills.RuntimeRules_Core_CommonRules.IsPublicTargetZone(zone);
    internal static bool ConversionSourcesEqual(IReadOnlyList<CardConversionSource>? left, IReadOnlyList<CardConversionSource>? right) => RuntimeSkills.RuntimeRules_Core_CommonRules.ConversionSourcesEqual(left, right);
    internal static bool IsSlashCard(CardKind kind) => RuntimeSkills.RuntimeRules_Core_CommonRules.IsSlashCard(kind);
    internal static bool IsNativeResponseCard(Card responseCard, CardKind requiredCardKind) => RuntimeSkills.RuntimeRules_Core_CommonRules.IsNativeResponseCard(responseCard, requiredCardKind);
    internal static CardKind? ReadResponseCardKind(PromptChoice choice) => RuntimeSkills.RuntimeRules_Core_CommonRules.ReadResponseCardKind(choice);
    internal static bool MatchesRequiredCard(CardKind actual, CardKind required) => RuntimeSkills.RuntimeRules_Core_CommonRules.MatchesRequiredCard(actual, required);
    internal static DamageNature GetDamageNature(CardKind kind) => RuntimeSkills.RuntimeRules_Core_CommonRules.GetDamageNature(kind);
    internal DamageNature GetDamageNature(GameEngine.IDamageAttempt attack) => _characterMaChaoMobileGodOperations.MobileMaChaoYuliNature(attack, _coreNativeDamagePoliciesOperations.GetDamageNatureBeforeMobileMaChao(attack));
    internal static string GetSuitName(Suit suit) => RuntimeSkills.RuntimeRules_Core_CommonRules.GetSuitName(suit);
    internal static string GetDamageNatureLabel(DamageNature nature) => RuntimeSkills.RuntimeRules_Core_CommonRules.GetDamageNatureLabel(nature);
}
