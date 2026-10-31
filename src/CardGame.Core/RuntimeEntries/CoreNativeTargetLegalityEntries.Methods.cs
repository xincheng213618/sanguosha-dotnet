#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeTargetLegalityEntries
{
    internal bool CanUseSlashTarget(CharacterState source, CharacterState target, Card slashCard, CardConversionSource? conversionSource = default, CardKind effectiveKind = (CardKind)0, bool ignoreDistance = false, long? existingUseFrameId = default, bool noEffectiveRank = false, int? specificEffectiveRank = default, IReadOnlyList<int>? physicalCardIds = default, int? excludedEquipmentId = default, EquipmentSlot? excludedEquipmentSlot = default) => _resolve().CanUseSlashTarget(source, target, slashCard, conversionSource, effectiveKind, ignoreDistance, existingUseFrameId, noEffectiveRank, specificEffectiveRank, physicalCardIds, excludedEquipmentId, excludedEquipmentSlot);
    internal bool CanUseVirtualSlashTarget(CharacterState source, CharacterState target, CardKind effectiveKind = (CardKind)0, Suit? physicalSuit = default, bool? effectiveColor = default, int? effectiveRank = default, IReadOnlyList<int>? physicalCardIds = default, bool ignoreDistance = false, CardConversionSource? conversionSource = default, bool actualUnconvertedPhysical = false, bool countsTowardLimit = true) => _resolve().CanUseVirtualSlashTarget(source, target, effectiveKind, physicalSuit, effectiveColor, effectiveRank, physicalCardIds, ignoreDistance, conversionSource, actualUnconvertedPhysical, countsTowardLimit);
    internal bool CanUseZhangbaSerpentSpear(CharacterState actor) => _resolve().CanUseZhangbaSerpentSpear(actor);
    internal IReadOnlyList<CardKind> GetSlashUseKinds(CharacterState actor, CardKind baseEffectiveKind) => _resolve().GetSlashUseKinds(actor, baseEffectiveKind);
    internal bool HasZhuqueFan(CharacterState actor) => _resolve().HasZhuqueFan(actor);
    internal bool IsZhuqueFanConversion(CharacterState actor, CardKind baseEffectiveKind, CardKind requestedEffectiveKind) => _resolve().IsZhuqueFanConversion(actor, baseEffectiveKind, requestedEffectiveKind);
}
