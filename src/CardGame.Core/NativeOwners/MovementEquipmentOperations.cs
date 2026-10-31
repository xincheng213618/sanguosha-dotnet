#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MovementEquipmentOperations(global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::CardGame.Core.NativeOwners.MovementArmorOwnerFactory createMovementArmorOwner, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativeRecoveryFramesEntries> getCoreNativeRecoveryFramesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries> getCorePhysicalMovementFacadesRuntime, global::CardGame.Core.EngineStates.CoreJudgmentEffectiveCardsState judgmentEffectiveCardsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries> getCoreMatchLogRuntime) : IMovementEquipmentOperations
{
    public IMovementArmorOwner ArmorOwner(int seat) => createMovementArmorOwner(matchParticipantsState.ReferenceToPlayers[seat]);
    public bool QueueArmorRecovery(long parent, int seat, CardMoveReason reason) => getCoreRecoveryReplacementRuntime().TryQueueRecoveryReplacement(parent, seat, seat, 1, new(RecoveryAttemptProducer.SilverLion, MoveReason: reason));
    public long BeginArmorRecovery(long parent, int seat) => getCoreNativeRecoveryFramesRuntime().BeginRecovery(parent, seat, seat, 1);
    public void MoveAll(CardLocation from, CardLocation to, CardMoveReason reason) => getCorePhysicalMovementFacadesRuntime().MoveAllCards(from, to, reason);
    public void RemoveJudgmentIdentity(int cardId) => judgmentEffectiveCardsState.ReferenceToJudgmentEffectiveCardKinds.Remove(cardId);
    public void Log(string kind, string message, int actor) => getCoreMatchLogRuntime().AddLog(kind, message, actor);
}
