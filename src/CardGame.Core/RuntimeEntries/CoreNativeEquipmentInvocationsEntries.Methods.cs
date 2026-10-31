#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeEquipmentInvocationsEntries
{
    internal void CancelNativeEquipmentContinuation(NativeEquipmentInvocationReceipt r, NativeOwners.CardAttackHandle attack) => _resolve().CancelNativeEquipmentContinuation(r, attack);
    internal NativeEquipmentInvocationReceipt CaptureNativeEquipmentInvocation(NativeOwners.CardAttackHandle attack, NativeEquipmentInvocationKind kind, int actorSeat, int targetSeat) => _resolve().CaptureNativeEquipmentInvocation(attack, kind, actorSeat, targetSeat);
    internal void ContinueInvokedQilinBow(NativeOwners.CardAttackHandle attack, Card? mount) => _resolve().ContinueInvokedQilinBow(attack, mount);
    internal void ContinueInvokedQinglong(NativeEquipmentInvocationReceipt r, NativeOwners.CardAttackHandle attack) => _resolve().ContinueInvokedQinglong(r, attack);
    internal void ContinueNativeEquipmentInvocation(SkillInvocationFrame frame) => _resolve().ContinueNativeEquipmentInvocation(frame);
    internal GameEngine.NativeEquipmentSuspensionProjection? FindNativeEquipmentInvocation(long attackId) => _resolve().FindNativeEquipmentInvocation(attackId);
    internal bool NativeEquipmentHostMatches(ResolutionFrame parent, CardUseFrame root, NativeEquipmentInvocationReceipt r) => _resolve().NativeEquipmentHostMatches(parent, root, r);
    internal ResolutionFrame? NativeEquipmentInvariantTop(ResolutionFrame? actualTop) => _resolve().NativeEquipmentInvariantTop(actualTop);
    internal bool NativeEquipmentObserverEdge(int index) => _resolve().NativeEquipmentObserverEdge(index);
    internal void PublishCixiongAcceptedTargetChoice(NativeOwners.CixiongDoubleSwordsHandle pending) => _resolve().PublishCixiongAcceptedTargetChoice(pending);
    internal bool TryBeginBaguaEquipmentInvocation(NativeOwners.CardAttackHandle attack, CharacterState defender, bool factionDefense = false) => _resolve().TryBeginBaguaEquipmentInvocation(attack, defender, factionDefense);
    internal bool TryBeginCixiongEquipmentInvocation(NativeOwners.CixiongDoubleSwordsHandle pending) => _resolve().TryBeginCixiongEquipmentInvocation(pending);
    internal bool TryBeginIceSwordEquipmentInvocation(NativeOwners.IceSwordHandle pending) => _resolve().TryBeginIceSwordEquipmentInvocation(pending);
    internal bool TryBeginNativeEquipmentInvocation(NativeEquipmentInvocationReceipt receipt) => _resolve().TryBeginNativeEquipmentInvocation(receipt);
    internal bool TryBeginQilinBowEquipmentInvocation(NativeOwners.CardAttackHandle attack, Card discardedMount) => _resolve().TryBeginQilinBowEquipmentInvocation(attack, discardedMount);
    internal bool TryBeginQinglongEquipmentInvocation(NativeOwners.QinglongCrescentBladeHandle pending, Card slash, CardKind effectiveSlashKind, int physicalOwnerSeat, IReadOnlyList<Card> physicalCards, NativeOwners.FactionCardRequestHandle? jijiang = default, CardConversionSource? conversionSource = default) => _resolve().TryBeginQinglongEquipmentInvocation(pending, slash, effectiveSlashKind, physicalOwnerSeat, physicalCards, jijiang, conversionSource);
    internal bool TryBeginStoneAxeEquipmentInvocation(NativeOwners.CardAttackHandle attack) => _resolve().TryBeginStoneAxeEquipmentInvocation(attack);
}
