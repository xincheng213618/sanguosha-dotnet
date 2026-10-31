#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangShiPingOlRuntime
{
    internal bool CanPlaceZhangShiPingMount(int owner, int target, Card card, CardLocation from) => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.InitializeXingGuMountPile) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(owner) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(target) && owner != target && _coreMatchParticipantsPlayersAccess._players[owner].IsAlive && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && from.Zone == CardZoneKind.PublicPersistentPile && from.OwnerSeat == owner && _cardZonesOperations._cardZones.GetLocation(card.Id) == from && _corePublicPileSourcesOperations.PublicPileSources(owner).Any(source => source.SkillId == "ol:xinggu" && source.Location == from) && IsXingGuMount(card) && !_coreMatchParticipantsPlayersAccess._players[target].EquipmentAreaAbolished && _coreMatchParticipantsPlayersAccess._players[target].EquipmentSlotCapacity(_coreEquipmentContentOperations.EquipmentRules.Get(card.Kind).Slot) > 0;
    internal void PlaceZhangShiPingMount(ProgramSkillFrame frame, int target, Card card, CardLocation from)
    {
        if (_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id) != frame || frame.ZhangShiPingXingGu is not { Stage: XingGuStage.PlacementChildren } receipt || receipt.CardId != card.Id || receipt.RecipientSeat != target || receipt.PileLocation != from || !CanPlaceZhangShiPingMount(frame.OwnerSeat, target, card, from))
            throw new InvalidOperationException("Xinggu lost its exact public mount, native equipment slot or accepted placement.");
        var slot = _coreEquipmentContentOperations.EquipmentRules.Get(card.Kind).Slot;
        var equipped = _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[target]).Where(item => _coreEquipmentContentOperations.EquipmentRules.Get(item.Kind).Slot == slot).ToArray();
        var replaced = equipped.Length >= _coreMatchParticipantsPlayersAccess._players[target].EquipmentSlotCapacity(slot) ? equipped[0] : null;
        if (replaced is not null)
        {
            _coreAdvancedWeaponsOperations.CopyFirstReplacedWeapon(card, replaced);
            _corePhysicalMovementFacadesOperations.MoveCard(replaced, CardLocation.Equipment(target), CardLocation.DiscardPile, CardMoveReasons.EquipmentReplace);
        }

        _corePhysicalMovementFacadesOperations.MoveCard(card, from, CardLocation.Equipment(target), CardMoveReasons.EquipmentEnter);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new EquipmentChangedEvent(frame.Id, target, slot, card.Id, card.Kind, replaced?.Id));
        _coreMatchLogOperations.AddLog("EquipmentChanged", $"{(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].Name)} 将【{(card.DisplayName)}】置入 {(_coreMatchParticipantsPlayersAccess._players[target].Name)} 的装备区。", target);
    }
}
