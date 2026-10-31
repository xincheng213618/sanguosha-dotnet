#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeWoodenOxRuntime
{
    internal void ResolveWoodenOx(CharacterState actor, int storedCardId, int targetSeat)
    {
        var storedCard = _corePhysicalOwnedCardQueriesOperations.GetHand(actor).Single(card => card.Id == storedCardId);
        _corePhysicalMovementFacadesOperations.MoveCard(storedCard, CardLocation.Hand(actor.Seat), CardLocation.WoodenOxGrain(actor.Seat), CardMoveReasons.WoodenOxStore, disclosure: CardMovementDisclosure.FaceDown);
        _coreCurrentTurnStatisticsWoodenOxUsedThisTurnAccess._woodenOxUsedThisTurn = true;
        _coreMatchLogOperations.AddLog("EquipmentEffect", $"{(actor.Name)} 将一张手牌扣置于【木牛流马】下。", actor.Seat);
        if (targetSeat < 0)
        {
            return;
        }

        if (_coreMatchParticipantsPlayersAccess._players[targetSeat].EquipmentAreaAbolished)
            throw new InvalidOperationException("Wooden Ox cannot enter an abolished equipment area.");
        var woodenOx = _characterHuCheErMouOperations.KongwuEffectiveEquipment(actor).Single(card => card.Kind == CardKind.WoodenOx);
        _corePhysicalMovementFacadesOperations.MoveCard(woodenOx, CardLocation.Equipment(actor.Seat), CardLocation.Equipment(targetSeat), CardMoveReasons.WoodenOxTransfer);
        _coreMatchLogOperations.AddLog("EquipmentEffect", $"{(actor.Name)} 将【木牛流马】移动给 {(_coreMatchParticipantsPlayersAccess._players[targetSeat].Name)}。", actor.Seat);
    }
}
