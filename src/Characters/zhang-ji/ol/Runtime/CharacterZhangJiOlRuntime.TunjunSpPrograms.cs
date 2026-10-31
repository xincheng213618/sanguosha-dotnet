#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangJiOlRuntime
{
    internal void IssueZhangJiEquipment(ProgramSkillFrame f)
    {
        var r = f.ZhangJi!;
        var target = _coreMatchParticipantsPlayersAccess._players[r.Target];
        if (r.Slots.Count >= r.HistoricalCount || !target.IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            FinishZhangJi(f);
            return;
        }

        var candidates = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Where(c => _coreEquipmentContentOperations.EquipmentRules.IsEquipment(c.Kind) && !r.Slots.Contains(_coreEquipmentContentOperations.EquipmentRules.Get(c.Kind).Slot) && target.EquipmentSlotCapacity(_coreEquipmentContentOperations.EquipmentRules.Get(c.Kind).Slot) > 0 && !_coreCardUseModulesOperations.IsCardUseForbidden(target.Seat, c.Kind, CardActionType.Use) && !_coreHandComparisonOperations.IsSelfTargetForbiddenAction(target, c.Kind, [target.Seat]) && !_coreResponseEntityExchangeOperations.IsResponseEntityRestricted(target.Seat, c.Id) && !_coreProgramPhaseColorRestrictionsOperations.IsPlayPhasePhysicalCardRestricted(target, c)).OrderBy(c => c.Id).ToArray();
        if (candidates.Length == 0)
        {
            FinishZhangJi(f);
            return;
        }

        var card = candidates[_randomOperations._random.Next(candidates.Length)];
        var slot = _coreEquipmentContentOperations.EquipmentRules.Get(card.Kind).Slot;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhangJi = r with { Stage = "issuing-equipment", NativeUse = null, NativeReturned = false, Slots = r.Slots.Append(slot).ToArray() } });
        var id = _coreNativeCardUseOperations.BeginCardUse(card, target.Seat, [], conversionSource: r.Source);
        _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DrawPile, CardLocation.Processing, CardMoveReasons.EquipmentUse);
        if (!_coreFinalizedCardTargetsOperations.TryBeginEquipmentTargetPrograms(target, card, id))
            _coreNativeEquipmentUseOperations.CompleteEquipmentUse(target, card, id);
    }
}
