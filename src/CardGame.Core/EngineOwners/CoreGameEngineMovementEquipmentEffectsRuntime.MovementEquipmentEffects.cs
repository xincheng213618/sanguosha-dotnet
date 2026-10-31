#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineMovementEquipmentEffectsRuntime
{
    bool global::CardGame.Core.IMovementEquipmentFacts.UsesStorageEquipment => _coreNativeDamageHandlesOperations.UsesFormalWoodenOx;

    bool global::CardGame.Core.IMovementEquipmentFacts.UsesRecoveryArmor => _coreNativeDamageHandlesOperations.UsesFormalSilverLion;

    CardMovementRecord? global::CardGame.Core.IMovementEquipmentFacts.LastMovement(int cardId, CardLocation from, CardLocation to) => _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault(move => move.CardId == cardId && move.From == from && move.To == to);
    bool global::CardGame.Core.IMovementEquipmentFacts.WasBoneAt(int cardId, CardLocation location, int sequence) => _characterWeiYanHappyGodOperations.HappyBoneAt(cardId, location, sequence)is not null;
    int global::CardGame.Core.IMovementEquipmentFacts.Count(CardLocation location) => _cardZonesOperations._cardZones.Count(location);
    IMovementArmorOwner global::CardGame.Core.IMovementEquipmentOperations.ArmorOwner(int seat) => _kernelCoreGameEngineMovementEquipmentEffectsOperations.CreateMovementArmorOwner(_coreMatchParticipantsPlayersAccess._players[seat]);
    bool global::CardGame.Core.IMovementEquipmentOperations.QueueArmorRecovery(long parent, int seat, CardMoveReason reason) => _coreRecoveryReplacementOperations.TryQueueRecoveryReplacement(parent, seat, seat, 1, new(RecoveryAttemptProducer.SilverLion, MoveReason: reason));
    long global::CardGame.Core.IMovementEquipmentOperations.BeginArmorRecovery(long parent, int seat) => _coreNativeRecoveryFramesOperations.BeginRecovery(parent, seat, seat, 1);
    void global::CardGame.Core.IMovementEquipmentOperations.MoveAll(CardLocation from, CardLocation to, CardMoveReason reason) => _corePhysicalMovementFacadesOperations.MoveAllCards(from, to, reason);
    void global::CardGame.Core.IMovementEquipmentOperations.RemoveJudgmentIdentity(int cardId) => _coreJudgmentEffectiveCardsJudgmentEffectiveCardKindsAccess._judgmentEffectiveCardKinds.Remove(cardId);
    void global::CardGame.Core.IMovementEquipmentOperations.Log(string kind, string message, int actor) => _coreMatchLogOperations.AddLog(kind, message, actor);
}
