#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuBanOlRuntime
{
    internal bool IsHuiyunOutsideGeneralWeaponMaterial(long useId, int cardId) => _coreCardUseLifecycleOperations.LifecycleCardUse(useId)is { HuiyunOrigin: { } origin } use && origin.Material.CardId == cardId && origin.Material.From.Zone == CardZoneKind.Equipment && _coreAdvancedLifecycleOperations.GetAdvancedCard(cardId).IsGeneralWeapon && _cardZonesOperations._cardZones.GetLocation(cardId) == CardLocation.OutsideGame && HuiyunUseProof(use);
}
