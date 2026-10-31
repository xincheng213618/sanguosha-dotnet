#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineMovementEquipmentEffectsRuntime
{
    bool IMovementEquipmentFacts.UsesStorageEquipment => _coreNativeDamageHandlesOperations.Value.UsesFormalWoodenOx;

    bool IMovementEquipmentFacts.UsesRecoveryArmor => _coreNativeDamageHandlesOperations.Value.UsesFormalSilverLion;

    CardMovementRecord? IMovementEquipmentFacts.LastMovement(int cardId, CardLocation from, CardLocation to) => _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault(move => move.CardId == cardId && move.From == from && move.To == to);
    bool IMovementEquipmentFacts.WasBoneAt(int cardId, CardLocation location, int sequence) => _characterWeiYanHappyGodOperations.HappyBoneAt(cardId, location, sequence)is not null;
    int IMovementEquipmentFacts.Count(CardLocation location) => _cardZonesOperations._cardZones.Count(location);
    IMovementArmorOwner IMovementEquipmentOperations.ArmorOwner(int seat) => _kernelCoreGameEngineMovementEquipmentEffectsOperations.CreateMovementArmorOwner(_coreMatchParticipantsPlayersAccess._players[seat]);
    bool IMovementEquipmentOperations.QueueArmorRecovery(long parent, int seat, CardMoveReason reason) => _coreRecoveryReplacementOperations.TryQueueRecoveryReplacement(parent, seat, seat, 1, new(RecoveryAttemptProducer.SilverLion, MoveReason: reason));
    long IMovementEquipmentOperations.BeginArmorRecovery(long parent, int seat) => _coreNativeRecoveryFramesOperations.BeginRecovery(parent, seat, seat, 1);
    void IMovementEquipmentOperations.MoveAll(CardLocation from, CardLocation to, CardMoveReason reason) => _corePhysicalMovementFacadesOperations.MoveAllCards(from, to, reason);
    void IMovementEquipmentOperations.RemoveJudgmentIdentity(int cardId) => _coreJudgmentEffectiveCardsJudgmentEffectiveCardKindsAccess._judgmentEffectiveCardKinds.Remove(cardId);
    void IMovementEquipmentOperations.Log(string kind, string message, int actor) => _coreMatchLogOperations.AddLog(kind, message, actor);
}
