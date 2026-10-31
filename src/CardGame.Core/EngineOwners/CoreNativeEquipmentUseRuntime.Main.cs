#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeEquipmentUseRuntime
{
    internal void ResolveEquip(CharacterState source, Card equipment)
    {
        var stillLegal = _coreNativeLegalActionsOperations.BuildLegalActions(source, selectedPhysicalCardId: equipment.Id).Any(action => action.Kind == LegalActionKind.Equip && action.CardId == equipment.Id && action.TargetSeat is null);
        if (!stillLegal)
        {
            throw new InvalidOperationException("The equipment card became illegal before resolution.");
        }

        var resolutionId = _coreNativeCardUseOperations.BeginCardUse(equipment, source.Seat, []);
        _corePhysicalMovementFacadesOperations.MoveCard(equipment, _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(source, equipment), CardLocation.Processing, CardMoveReasons.EquipmentUse, disclosure: CardMovementDisclosure.FaceUp);
        if (!_coreFinalizedCardTargetsOperations.TryBeginEquipmentTargetPrograms(source, equipment, resolutionId))
            CompleteEquipmentUse(source, equipment, resolutionId);
    }

    internal void CompleteEquipmentUse(CharacterState source, Card equipment, long resolutionId)
    {
        if (_characterWeiYanHappyGodOperations.TryHappyWeiYanEquipmentCommitted(resolutionId, equipment))
            return;
        if (_characterXiangChongOlOperations.IsXiangChongClaimedUseMaterial(resolutionId, equipment.Id) || _characterHuCheErAnniversaryOperations.IsAnniversaryHuCheErClaimedUseMaterial(resolutionId, equipment.Id))
        {
            _coreCardUseLifecycleOperations.BeginSimpleCardUse(resolutionId, new(equipment.Id, SimpleCardUseEffect.Equipment));
            return;
        }

        if (source.EquipmentSlotCapacity(_coreFieldEquipmentOperations.FieldEquipmentSlot(equipment)) == 0)
        {
            _corePhysicalMovementFacadesOperations.MoveCard(equipment, CardLocation.Processing, CardLocation.DiscardPile, new CardMoveReason("area.abolished.equipment"));
            _coreCardUseLifecycleOperations.BeginSimpleCardUse(resolutionId, new(equipment.Id, SimpleCardUseEffect.Equipment));
            return;
        }

        var definition = _coreEquipmentContentOperations.EquipmentRules.Get(equipment.Kind);
        var sameSlot = _corePhysicalOwnedCardQueriesOperations.GetEquipment(source).Where(card => _coreFieldEquipmentOperations.FieldEquipmentSlot(card) == definition.Slot).ToArray();
        var replaced = sameSlot.Length >= source.EquipmentSlotCapacity(definition.Slot) ? sameSlot.FirstOrDefault() : null;
        if (replaced is not null)
        {
            _coreAdvancedWeaponsOperations.CopyFirstReplacedWeapon(equipment, replaced);
            _corePhysicalMovementFacadesOperations.MoveCard(replaced, CardLocation.Equipment(source.Seat), CardLocation.DiscardPile, CardMoveReasons.EquipmentReplace);
        }

        if (_coreRecoveryReplacementOperations.TryPauseRecoveryPaidCardUse(resolutionId, new(RecoveryPaidCardUseKind.EquipmentUse, source.Seat, equipment.Id, ReplacedCardId: replaced?.Id, EquipmentSlot: definition.Slot)))
            return;
        _corePhysicalMovementFacadesOperations.MoveCard(equipment, CardLocation.Processing, CardLocation.Equipment(source.Seat), CardMoveReasons.EquipmentEnter);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new EquipmentChangedEvent(resolutionId, source.Seat, definition.Slot, equipment.Id, equipment.Kind, replaced?.Id));
        _coreMatchLogOperations.AddLog("EquipmentChanged", replaced is null ? $"{(source.Name)} 装备【{(definition.DisplayName)}】。" : $"{(source.Name)} 装备【{(definition.DisplayName)}】，替换并弃置原有{(EquipmentCatalog.GetSlotName(definition.Slot))}。", source.Seat);
        _coreCardUseLifecycleOperations.BeginSimpleCardUse(resolutionId, new(equipment.Id, SimpleCardUseEffect.Equipment));
    }
}
