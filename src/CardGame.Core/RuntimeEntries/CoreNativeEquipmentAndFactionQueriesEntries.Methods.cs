#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeEquipmentAndFactionQueriesEntries
{
    internal bool CanRequestFactionDefense(global::CardGame.Core.CharacterState owner, global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().CanRequestFactionDefense(owner, attack);
    internal bool CanRequestFactionSlashResponse(global::CardGame.Core.CharacterState owner, global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().CanRequestFactionSlashResponse(owner, attack);
    internal bool CanUseFactionSlashRequest(global::CardGame.Core.CharacterState owner, string providerFactionId) => _resolve().CanUseFactionSlashRequest(owner, providerFactionId);
    internal bool CanUseProvidedSlashTarget(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.Suit? physicalSuit = default, bool allowAnyPhysicalSuit = true, bool? effectiveColor = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default) => _resolve().CanUseProvidedSlashTarget(owner, target, effectiveKind, physicalSuit, allowAnyPhysicalSuit, effectiveColor, effectiveRank, physicalCardIds);
    internal global::System.Collections.Generic.IReadOnlyList<int> GetFactionProviderSeats(int ownerSeat, string factionId) => _resolve().GetFactionProviderSeats(ownerSeat, factionId);
    internal int GetRequiredResponseCount(global::CardGame.Core.CharacterState skillOwner, int sourceSeat, int responderSeat, global::CardGame.Core.CardKind incomingCard, global::CardGame.Core.CardKind requiredCardKind) => _resolve().GetRequiredResponseCount(skillOwner, sourceSeat, responderSeat, incomingCard, requiredCardKind);
    internal bool HasArmor(global::CardGame.Core.CharacterState player) => _resolve().HasArmor(player);
    internal bool HasArmorBypass(global::CardGame.Core.CharacterState player) => _resolve().HasArmorBypass(player);
    internal bool HasBagua(global::CardGame.Core.CharacterState player) => _resolve().HasBagua(player);
    internal bool HasBlackSlashBarrier(global::CardGame.Core.CharacterState player) => _resolve().HasBlackSlashBarrier(player);
    internal bool HasConditionalVirtualBaguaForAi(global::CardGame.Core.CharacterState player) => _resolve().HasConditionalVirtualBaguaForAi(player);
    internal bool HasEmptyEquipmentSlotForOwnerHandEquipment(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CharacterState target) => _resolve().HasEmptyEquipmentSlotForOwnerHandEquipment(owner, target);
    internal bool HasSilverLion(global::CardGame.Core.CharacterState player) => _resolve().HasSilverLion(player);
    internal bool HasTengjia(global::CardGame.Core.CharacterState player) => _resolve().HasTengjia(player);
}
