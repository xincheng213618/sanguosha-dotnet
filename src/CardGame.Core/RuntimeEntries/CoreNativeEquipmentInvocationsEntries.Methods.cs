#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeEquipmentInvocationsEntries
{
    internal void AssertNativeEquipmentInvocations() => _resolve().AssertNativeEquipmentInvocations();
    internal void CancelNativeEquipmentContinuation(global::CardGame.Core.NativeEquipmentInvocationReceipt r, global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().CancelNativeEquipmentContinuation(r, attack);
    internal global::CardGame.Core.NativeEquipmentInvocationReceipt CaptureNativeEquipmentInvocation(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.NativeEquipmentInvocationKind kind, int actorSeat, int targetSeat) => _resolve().CaptureNativeEquipmentInvocation(attack, kind, actorSeat, targetSeat);
    internal void ContinueInvokedQilinBow(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.Card? mount) => _resolve().ContinueInvokedQilinBow(attack, mount);
    internal void ContinueInvokedQinglong(global::CardGame.Core.NativeEquipmentInvocationReceipt r, global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().ContinueInvokedQinglong(r, attack);
    internal void ContinueNativeEquipmentInvocation(global::CardGame.Core.SkillInvocationFrame frame) => _resolve().ContinueNativeEquipmentInvocation(frame);
    internal global::CardGame.Core.GameEngine.NativeEquipmentSuspensionProjection? FindNativeEquipmentInvocation(long attackId) => _resolve().FindNativeEquipmentInvocation(attackId);
    internal bool HasFactionDefenseEquipmentInvocation(global::CardGame.Core.NativeOwners.FactionDefenseHandle defense) => _resolve().HasFactionDefenseEquipmentInvocation(defense);
    internal bool HasNativeEquipmentInvocation(long attackId) => _resolve().HasNativeEquipmentInvocation(attackId);
    internal bool HasQinglongFactionEquipmentInvocation(global::CardGame.Core.NativeOwners.FactionCardRequestHandle request) => _resolve().HasQinglongFactionEquipmentInvocation(request);
    internal bool NativeEquipmentHostMatches(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.CardUseFrame root, global::CardGame.Core.NativeEquipmentInvocationReceipt r) => _resolve().NativeEquipmentHostMatches(parent, root, r);
    internal global::CardGame.Core.ResolutionFrame? NativeEquipmentInvariantTop(global::CardGame.Core.ResolutionFrame? actualTop) => _resolve().NativeEquipmentInvariantTop(actualTop);
    internal bool NativeEquipmentObserverEdge(int index) => _resolve().NativeEquipmentObserverEdge(index);
    internal void PublishCixiongAcceptedTargetChoice(global::CardGame.Core.NativeOwners.CixiongDoubleSwordsHandle pending) => _resolve().PublishCixiongAcceptedTargetChoice(pending);
    internal bool TryBeginBaguaEquipmentInvocation(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CharacterState defender, bool factionDefense = false) => _resolve().TryBeginBaguaEquipmentInvocation(attack, defender, factionDefense);
    internal bool TryBeginCixiongEquipmentInvocation(global::CardGame.Core.NativeOwners.CixiongDoubleSwordsHandle pending) => _resolve().TryBeginCixiongEquipmentInvocation(pending);
    internal bool TryBeginIceSwordEquipmentInvocation(global::CardGame.Core.NativeOwners.IceSwordHandle pending) => _resolve().TryBeginIceSwordEquipmentInvocation(pending);
    internal bool TryBeginNativeEquipmentInvocation(global::CardGame.Core.NativeEquipmentInvocationReceipt receipt) => _resolve().TryBeginNativeEquipmentInvocation(receipt);
    internal bool TryBeginQilinBowEquipmentInvocation(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.Card discardedMount) => _resolve().TryBeginQilinBowEquipmentInvocation(attack, discardedMount);
    internal bool TryBeginQinglongEquipmentInvocation(global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle pending, global::CardGame.Core.Card slash, global::CardGame.Core.CardKind effectiveSlashKind, int physicalOwnerSeat, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> physicalCards, global::CardGame.Core.NativeOwners.FactionCardRequestHandle? jijiang = default, global::CardGame.Core.CardConversionSource? conversionSource = default) => _resolve().TryBeginQinglongEquipmentInvocation(pending, slash, effectiveSlashKind, physicalOwnerSeat, physicalCards, jijiang, conversionSource);
    internal bool TryBeginStoneAxeEquipmentInvocation(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().TryBeginStoneAxeEquipmentInvocation(attack);
}
