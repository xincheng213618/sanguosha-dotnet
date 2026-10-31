#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeEquipmentAndFactionQueriesEntries
{
    internal bool CanRequestFactionDefense(CharacterState owner, NativeOwners.CardAttackHandle attack) => _resolve().CanRequestFactionDefense(owner, attack);
    internal bool CanRequestFactionSlashResponse(CharacterState owner, NativeOwners.CardAttackHandle attack) => _resolve().CanRequestFactionSlashResponse(owner, attack);
    internal bool CanUseFactionSlashRequest(CharacterState owner, string providerFactionId) => _resolve().CanUseFactionSlashRequest(owner, providerFactionId);
    internal bool CanUseProvidedSlashTarget(CharacterState owner, CharacterState target, CardKind? effectiveKind = default, Suit? physicalSuit = default, bool allowAnyPhysicalSuit = true, bool? effectiveColor = default, int? effectiveRank = default, IReadOnlyList<int>? physicalCardIds = default) => _resolve().CanUseProvidedSlashTarget(owner, target, effectiveKind, physicalSuit, allowAnyPhysicalSuit, effectiveColor, effectiveRank, physicalCardIds);
    internal IReadOnlyList<int> GetFactionProviderSeats(int ownerSeat, string factionId) => _resolve().GetFactionProviderSeats(ownerSeat, factionId);
    internal int GetRequiredResponseCount(CharacterState skillOwner, int sourceSeat, int responderSeat, CardKind incomingCard, CardKind requiredCardKind) => _resolve().GetRequiredResponseCount(skillOwner, sourceSeat, responderSeat, incomingCard, requiredCardKind);
    internal bool HasArmor(CharacterState player) => _resolve().HasArmor(player);
    internal bool HasArmorBypass(CharacterState player) => _resolve().HasArmorBypass(player);
    internal bool HasBagua(CharacterState player) => _resolve().HasBagua(player);
    internal bool HasBlackSlashBarrier(CharacterState player) => _resolve().HasBlackSlashBarrier(player);
    internal bool HasConditionalVirtualBaguaForAi(CharacterState player) => _resolve().HasConditionalVirtualBaguaForAi(player);
    internal bool HasEmptyEquipmentSlotForOwnerHandEquipment(CharacterState owner, CharacterState target) => _resolve().HasEmptyEquipmentSlotForOwnerHandEquipment(owner, target);
    internal bool HasSilverLion(CharacterState player) => _resolve().HasSilverLion(player);
    internal bool HasTengjia(CharacterState player) => _resolve().HasTengjia(player);
}
