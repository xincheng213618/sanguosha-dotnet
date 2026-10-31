#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AtomicCardMovementEffects(Func<RuntimeEntries.CorePhysicalMovementFacadesEntries> getCorePhysicalMovementFacadesRuntime, Func<RuntimeEntries.CoreGameEngineEquipmentSkillsEntries> getCoreGameEngineEquipmentSkillsRuntime, Func<RuntimeEntries.CoreActualDiscardPhaseLedgerEntries> getCoreActualDiscardPhaseLedgerRuntime) : IAtomicCardMovementEffects
{
    public CardMovementRecord Record(Card card, CardLocation from, CardLocation to, CardMoveReason reason, Action<CardMovementRecord>? beforeFact = null, CardKind? adjacentDiscardIdentity = null, CardMovementDisclosure disclosure = CardMovementDisclosure.Unspecified, HappyWeiYanGainDestinationPlan? happyWeiYanGainDestination = null, HappyZuoCiPhantomDestinationPlan? happyZuoCiPhantomDestination = null) => getCorePhysicalMovementFacadesRuntime().RecordMovement(card, from, to, reason, beforeFact, adjacentDiscardIdentity, disclosure, happyWeiYanGainDestination, happyZuoCiPhantomDestination);
    public void ResolveEquipmentSkillGrant(Card card, CardLocation from, CardLocation to) => getCoreGameEngineEquipmentSkillsRuntime().ResolveEquipmentSkillGrant(card, from, to);
    public void ClearJudgmentEffectiveKind(Card card, CardLocation from, CardLocation to) => getCorePhysicalMovementFacadesRuntime().ClearJudgmentEffectiveKindAfterMove(card, from, to);
    public void ResolveArmorRemoval(Card card, CardLocation from, CardMoveReason reason) => getCorePhysicalMovementFacadesRuntime().ResolveSilverLionRemoval(card, from, reason);
    public void ResolveStorageEquipmentMove(Card card, CardLocation from, CardLocation to) => getCorePhysicalMovementFacadesRuntime().ResolveWoodenOxMove(card, from, to);
    public void CollectDiscardPhaseHandDiscard(Card card, CardLocation from, CardLocation to) => getCoreActualDiscardPhaseLedgerRuntime().CollectDiscardPhaseHandDiscard(card, from, to);
}
